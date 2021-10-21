using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class BudgetStep : Form
    {
        public BudgetStep(BudgetForm budgetForm, DataTable dataTable)
        {
            InitializeComponent();
            Globals.budgetForm = budgetForm;

            //Popula o combobox de conta da receita
            DataTable incomeAccountDataTable = Database.query("SELECT conta.nomeConta FROM conta ORDER BY conta.nomeConta;");
            for (int i = 0; i < incomeAccountDataTable.Rows.Count; i++) this.cbb_incomeAccount.Items.Insert(i, " " + incomeAccountDataTable.Rows[i].ItemArray[0].ToString().Trim());

            if (Globals.idConta != 0)
            {
                DataTable sumTotalValueDataTable = Database.query("SELECT somarTotal FROM conta WHERE idConta = " + Globals.idConta);
                if (sumTotalValueDataTable.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(sumTotalValueDataTable.Rows[0].ItemArray[0])) Globals.saldoConta = Convert.ToDecimal(Database.query("SELECT saldoConta FROM conta WHERE idConta = " + Globals.idConta).Rows[0].ItemArray[0]);
                    else Globals.saldoConta = 0;
                }
            }

            if (dataTable != null)
            {
                Globals.budgetStepDataTable = dataTable;
                foreach (DataRow dataRow in Globals.budgetStepDataTable.Rows)
                {
                    Globals.incomeDataTable = Database.query("SELECT * FROM receita WHERE numeroOrcamento = " + dataRow.ItemArray[0]);
                    this.mtb_budgetDate.Text = Convert.ToDateTime(dataRow.ItemArray[2]).ToShortDateString().Trim();
                    this.txt_laborValue.Text = String.Format("{0:C}", dataRow.ItemArray[3]).Trim();
                    this.cbb_paymentCondition.SelectedIndex = cbb_paymentCondition.FindString(" " + dataRow.ItemArray[5].ToString().Trim());
                    if (Convert.ToBoolean(dataRow.ItemArray[6]) == true) this.ckb_confirmedBudget.Checked = true; else this.ckb_confirmedBudget.Checked = false;
                }
                foreach (DataRow dataRow in Globals.incomeDataTable.Rows)
                {
                    Globals.idReceita = Convert.ToInt32(dataRow.ItemArray[0]);
                    Globals.idConta = Convert.ToInt32(dataRow.ItemArray[1]);
                    Globals.numeroOrcamento = Convert.ToInt32(dataRow.ItemArray[2]);
                    Globals.idCategoria = Convert.ToInt32(dataRow.ItemArray[3]);
                    this.cbb_incomeAccount.SelectedIndex = this.cbb_incomeAccount.FindString(" " + Database.query("SELECT nomeConta FROM conta WHERE idConta = " + Globals.idConta).Rows[0].ItemArray[0].ToString().Trim());

                    this.txt_incomeDescription.Text = dataRow.ItemArray[5].ToString().Trim();
                    
                    this.txt_incomeObservations.Text = dataRow.ItemArray[7].ToString().Trim();

                    if (Convert.ToBoolean(dataRow.ItemArray[8]) == true) this.ckb_incomeReceived.Checked = true; else this.ckb_incomeReceived.Checked = false;

                    if (Convert.ToBoolean(dataRow.ItemArray[9]) == true)
                    {
                        this.ckb_parcelValue.Checked = true;
                        this.cbb_period.SelectedIndex = this.cbb_period.FindString(" " + dataRow.ItemArray[14].ToString().Trim());

                        this.txt_parcels.Show();
                        this.txt_parcels.Text = dataRow.ItemArray[13].ToString().Trim();
                        
                        int i = 0;
                        foreach (DataRow dataRowParcels in Database.query("SELECT * FROM parcela WHERE idReceita = " + Globals.idReceita).Rows)
                        {
                            Globals.parcels.Add(new Parcel());
                            Globals.parcels[i].IdParcela = Convert.ToInt32(dataRowParcels.ItemArray[0]);
                            i++;
                        }
                    }
                    else
                    {
                        this.ckb_parcelValue.Checked = false;
                        this.txt_parcels.Hide();
                        this.cbb_period.Hide();
                        break;
                    }
                }
            }
            else
            {
                this.clearFields();
                this.lbl_btnUpdateTag.Visible = false;
                this.pcb_btnUpdate.Visible = false;
                this.lbl_btnDeleteTag.Visible = false;
                this.pcb_btnDelete.Visible = false;
            }
        }

        //FUNÇÕES

        //Função que atualiza o valor total do orçamento
        private void updateBudgetTotalValue(Budget budget)
        {
            BudgetedProduct budgetedProduct = new BudgetedProduct();
            budgetedProduct.NumeroOrcamento = Convert.ToInt32(Globals.numeroOrcamento);
            Globals.budgetedProductDataTable = Database.query("SELECT produtoOrcado.idProdutoOrcado, produtoOrcado.item AS 'Item:', produtoOrcado.quantidadeProduto AS 'Quantidade:', produto.nomeProduto AS 'Nome do produto:', produto.valorUnitario AS 'Valor unitário:', produtoOrcado.valorTotal AS 'Valor total:' FROM produtoOrcado JOIN produto ON produtoOrcado.idProduto = produto.idProduto WHERE produtoOrcado.numeroOrcamento = " + Globals.numeroOrcamento + " ORDER BY produtoOrcado.item;");

            //Soma o valor total de cada produto do orçamento
            decimal valorTotalProdutos = 0;
            foreach (DataRow dataRow in Globals.budgetedProductDataTable.Rows) valorTotalProdutos += Convert.ToDecimal(dataRow.ItemArray[5]);
            decimal valorTotalOrcamento = valorTotalProdutos + budget.ValorTrabalho;

            //Atualiza o valor total do orçamento
            if (Database.updateBudgetTotalValue(budgetedProduct, valorTotalOrcamento))
            {
                //Seleciona a receita vinculada ao orçamento
                DataTable incomesDataTable = Database.query("SELECT * FROM receita WHERE numeroOrcamento = " + Globals.numeroOrcamento);

                if(incomesDataTable.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(incomesDataTable.Rows[0].ItemArray[9])) return;
                    else
                    {
                        //Receita não parcelada
                        if (Database.updateIncomeTotalValue(budgetedProduct, valorTotalOrcamento)) return;
                        else MessageBox.Show("[ERRO] Não foi possível atualizar o valor total do orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else MessageBox.Show("[ERRO] Não foi possível atualizar o valor total do orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Função que retorna os componentes do formulário ao estado inicial, caso não haja parcelas
        private void doNotParcel()
        {
            this.ckb_parcelValue.Checked = false;
            this.txt_parcels.Clear();
            this.txt_parcels.Hide();
            this.cbb_period.SelectedIndex = -1;
            this.cbb_period.Text = " Período";
            this.cbb_period.Hide();
            this.mtb_budgetDate.Focus();
        }

        //Função que retorna os campos do formulário ao estado inicial
        private void clearFields()
        {
            this.txt_laborValue.Clear();
            this.cbb_paymentCondition.SelectedIndex = this.cbb_paymentCondition.FindString(" Dinheiro");
            this.txt_incomeDescription.Clear();
            this.mtb_budgetDate.Text = (DateTime.Today.ToShortDateString()).ToString().Trim();
            this.txt_incomeObservations.Clear();
            this.doNotParcel();
        }

        //Função que seleciona a data de transação da parcela de acordo com o período escolhido pelo usuário
        private void periodSelection(int i, Income income, List<Parcel> parcels)
        {
            if (i == 0)
            {
                switch (income.PeriodoRepetirParcelarReceita)
                {
                    case "Diário":
                        parcels[i].DataTransacao = income.DataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        parcels[i].DataTransacao = income.DataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        parcels[i].DataTransacao = income.DataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        parcels[i].DataTransacao = income.DataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        parcels[i].DataTransacao = income.DataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        parcels[i].DataTransacao = income.DataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        parcels[i].DataTransacao = income.DataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        parcels[i].DataTransacao = income.DataTransacao.AddYears(1);
                        break;
                }
            }
            else
            {
                switch (income.PeriodoRepetirParcelarReceita)
                {
                    case "Diário":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddYears(1);
                        break;
                }
            }
        }

        //Função que atualiza uma única parcela por vez
        private void updateOneParcel(Income income)
        {
            if (Database.updateIncomeNotParceledOrRepeated(income))
            {
                MessageBox.Show("Orçamento atualizado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.doNotParcel();
            }
            else MessageBox.Show("[ERRO] Não foi possível atualizar orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Função que atualiza parcela
        private bool updateParcel(Income income)
        {
            DataTable parcelOrRepeatDataTable = Database.query("SELECT repetirParcelarReceita, valorFixoReceita, parcelasReceita, repeticoesValorFixoReceita FROM receita WHERE idReceita = " + Globals.idReceita);
            if (Database.updateIncome(income))
            {
                int qtde = Convert.ToInt32(Database.query("SELECT repeticoesValorFixoReceita FROM receita WHERE idReceita = " + Globals.idReceita).Rows[0].ItemArray[0]);
                if (qtde == 0)
                {
                    //Valor parcelado
                    Globals.parcels.Clear();
                    qtde = Convert.ToInt32(Database.query("SELECT parcelasReceita FROM receita WHERE idReceita = " + Globals.idReceita).Rows[0].ItemArray[0]);
                    if (qtde == 1)
                    {
                        this.updateOneParcel(income);
                        return true;
                    }
                    DataTable dataTable = Database.query("SELECT idParcela FROM parcela WHERE idReceita = " + Globals.idReceita);
                    if (dataTable.Rows.Count == 0)
                    {
                        for (int j = 0; j < (income.ParcelasReceita - 1); j++)
                        {
                            Globals.parcels.Add(new Parcel());
                            Globals.parcels[j].IdReceita = Globals.idReceita;
                            Globals.parcels[j].IdConta = income.IdConta;
                            Globals.parcels[j].IdCategoria = income.IdCategoria;
                            Globals.parcels[j].ValorParcela = income.ValorReceita;
                            Globals.parcels[j].DescricaoParcela = income.DescricaoReceita;
                            this.periodSelection(j, income, Globals.parcels);
                            Globals.parcels[j].ObservacoesParcela = income.ObservacoesReceita;
                            Globals.parcels[j].RecebimentoConfirmado = false;
                            if (Database.newParcel(Globals.parcels[j])) continue;
                            else
                            {
                                MessageBox.Show("[ERRO] Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                    }
                    dataTable = Database.query("SELECT idParcela FROM parcela WHERE idReceita = " + Globals.idReceita);
                    //Parcelas
                    int i = 0;
                    foreach (DataRow dataRowParcels in dataTable.Rows)
                    {
                        Globals.parcels.Add(new Parcel());
                        Globals.parcels[i].IdParcela = Convert.ToInt32(dataRowParcels.ItemArray[0]);
                        i++;
                    }
                    if (this.changeParcel(parcelOrRepeatDataTable, income)) return true; else return false;
                }
                else return false;
            }
            else
            {
                MessageBox.Show("[ERRO] Não foi possível atualizar orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //Função que atualiza as informações da parcela
        private bool changeParcel(DataTable dataTable, Income income)
        {
            List<Parcel> parcels = new List<Parcel>();
            int success = 1;
            int qtde = 0;
            if (dataTable.Rows[0].ItemArray[2] != System.DBNull.Value) qtde = Convert.ToInt32(dataTable.Rows[0].ItemArray[2]);
            if (Convert.ToBoolean(dataTable.Rows[0].ItemArray[0]))
            {
                //Já existiam parcelas ou repetições antes
                if (qtde != income.ParcelasReceita)
                {
                    if (qtde > income.ParcelasReceita)
                    {
                        //Diminuiu parcelas
                        for (int i = 0; i < (income.ParcelasReceita - 1); i++)
                        {
                            parcels.Add(new Parcel());
                            parcels[i].IdParcela = Globals.parcels[i].IdParcela;
                            parcels[i].IdConta = income.IdConta;
                            parcels[i].IdCategoria = income.IdCategoria;
                            parcels[i].ValorParcela = income.ValorReceita;
                            parcels[i].DescricaoParcela = income.DescricaoReceita;
                            this.periodSelection(i, income, parcels);
                            parcels[i].ObservacoesParcela = income.ObservacoesReceita;
                            parcels[i].RecebimentoConfirmado = false;
                            if (Database.updateParcel(parcels[i])) continue;
                            else
                            {
                                success = 0;
                                break;
                            }
                        }
                        for (int i = (income.ParcelasReceita - 1); i < (qtde - 1); i++)
                        {
                            parcels.Add(new Parcel());
                            parcels[i].IdParcela = Globals.parcels[i].IdParcela;
                            if (Database.deleteParcel(parcels[i])) continue;
                            else
                            {
                                success = 0;
                                break;
                            }
                        }
                        return true;
                    }
                    else if (qtde < income.ParcelasReceita)
                    {
                        //Aumentou parcelas
                        for (int i = 0; i < (qtde - 1); i++)
                        {
                            parcels.Add(new Parcel());
                            parcels[i].IdParcela = Globals.parcels[i].IdParcela;
                            parcels[i].IdConta = income.IdConta;
                            parcels[i].IdCategoria = income.IdCategoria;
                            parcels[i].ValorParcela = income.ValorReceita;
                            parcels[i].DescricaoParcela = income.DescricaoReceita;
                            this.periodSelection(i, income, parcels);
                            parcels[i].ObservacoesParcela = income.ObservacoesReceita;
                            parcels[i].RecebimentoConfirmado = false;
                            if (Database.updateParcel(parcels[i])) continue;
                            else
                            {
                                success = 0;
                                break;
                            }
                        }
                        if (success == 0)
                        {
                            MessageBox.Show("[ERRO] Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        for (int i = (qtde - 1); i < (income.ParcelasReceita - 1); i++)
                        {
                            parcels.Add(new Parcel());
                            parcels[i].IdReceita = Globals.idReceita;
                            parcels[i].IdConta = income.IdConta;
                            parcels[i].IdCategoria = income.IdCategoria;
                            parcels[i].ValorParcela = income.ValorReceita;
                            parcels[i].DescricaoParcela = income.DescricaoReceita;
                            this.periodSelection(i, income, parcels);
                            parcels[i].ObservacoesParcela = income.ObservacoesReceita;
                            parcels[i].RecebimentoConfirmado = false;
                            if (Database.newParcel(parcels[i])) continue;
                            else
                            {
                                MessageBox.Show("[ERRO] Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                        return true;
                    }
                }
                else
                {
                    for (int i = 0; i < (income.ParcelasReceita - 1); i++)
                    {
                        parcels.Add(new Parcel());
                        parcels[i].IdParcela = Globals.parcels[i].IdParcela;
                        parcels[i].IdConta = income.IdConta;
                        parcels[i].IdCategoria = income.IdCategoria;
                        parcels[i].ValorParcela = income.ValorReceita;
                        parcels[i].DescricaoParcela = income.DescricaoReceita;
                        this.periodSelection(i, income, parcels);
                        parcels[i].ObservacoesParcela = income.ObservacoesReceita;
                        parcels[i].RecebimentoConfirmado = false;
                        if (Database.updateParcel(parcels[i])) continue;
                        else
                        {
                            success = 0;
                            break;
                        }
                    }
                }
                if (success == 0)
                {
                    MessageBox.Show("[ERRO] Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show("Orçamento atualizado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.mtb_budgetDate.Focus();
                    return true;
                }
            }
            else
            {
                MessageBox.Show("Orçamento atualizado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.mtb_budgetDate.Focus();
                return true;
            }
        }

        //Função que atualiza orçamento
        private void budgetUpdate()
        {
            if (String.IsNullOrEmpty(mtb_budgetDate.Text.Trim()) || String.IsNullOrEmpty(txt_laborValue.Text.Trim())) MessageBox.Show("Informe o valor e a data do orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if ((this.ckb_parcelValue.Checked) && ((String.IsNullOrEmpty(this.txt_parcels.Text.Trim())) || (this.cbb_period.SelectedIndex == -1))) MessageBox.Show("Informe o número de parcelas do orçamento e o período em que elas se repetem!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    Account account = new Account();
                    Budget budget = new Budget();
                    Income income = new Income();
                    account.IdConta = Globals.idConta;
                    budget.NumeroOrcamento = Convert.ToInt32(Globals.numeroOrcamento);
                    income.NumeroOrcamento = Convert.ToInt32(Globals.numeroOrcamento);
                    budget.IdCliente = Convert.ToInt32(Globals.budgetStepDataTable.Rows[0].ItemArray[1]);
                    income.IdReceita = Globals.idReceita;
                    budget.DataOrcamento = Convert.ToDateTime(this.mtb_budgetDate.Text.Trim());
                    income.DataTransacao = Convert.ToDateTime(this.mtb_budgetDate.Text.Trim());
                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                    string valorTrabalho = this.txt_laborValue.Text.Trim();
                    if (regexValor.IsMatch(valorTrabalho))
                    {
                        if (valorTrabalho.Contains("R$ ")) budget.ValorTrabalho = Convert.ToDecimal(valorTrabalho.Substring(3).Trim());
                        else if (valorTrabalho.Contains("R$")) budget.ValorTrabalho = Convert.ToDecimal(valorTrabalho.Substring(2).Trim());
                        else budget.ValorTrabalho = Convert.ToDecimal(this.txt_laborValue.Text.Trim());
                        budget.ValorTotal = Convert.ToDecimal(Globals.budgetStepDataTable.Rows[0].ItemArray[4]);
                        income.ValorReceita = Convert.ToDecimal(Globals.budgetStepDataTable.Rows[0].ItemArray[4]);
                    }
                    else
                    {
                        MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.txt_laborValue.Clear();
                        this.txt_laborValue.PlaceholderText = "";
                        this.txt_laborValue.Focus();
                        return;
                    }
                    budget.CondicaoPagamento = this.cbb_paymentCondition.SelectedItem.ToString().Trim();
                    if (this.ckb_confirmedBudget.Checked) budget.OrcamentoConfirmado = true; 
                    else budget.OrcamentoConfirmado = false;
                    income.DescricaoReceita = this.txt_incomeDescription.Text.Trim();
                    income.IdConta = Convert.ToInt32(Database.query("SELECT idConta FROM conta WHERE nomeConta = '" + this.cbb_incomeAccount.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    income.IdCategoria = Convert.ToInt32(Database.query("SELECT idCategoria FROM categoria WHERE nomeCategoria = 'Orçamentos';").Rows[0].ItemArray[0]);
                    income.ObservacoesReceita = this.txt_incomeObservations.Text.Trim();
                    if (this.ckb_incomeReceived.Checked) income.RecebimentoConfirmado = true; else income.RecebimentoConfirmado = false;

                    if (ckb_parcelValue.Checked)
                    {
                        //Orçamento parcelado
                        income.RepetirParcelarReceita = true;

                        //Parcela e atualiza valor do orçamento
                        income.ParcelarValorReceita = true;
                        income.ParcelasReceita = Convert.ToInt32(txt_parcels.Text.Trim());
                        income.PeriodoRepetirParcelarReceita = cbb_period.SelectedItem.ToString().Trim();
                        income.ValorReceita = income.ValorReceita / income.ParcelasReceita;
                        if (this.updateParcel(income))
                        {
                            //Soma o valor total de cada produto do orçamento e atribui a diferença do valor dos produtos pelo valor do trabalho
                            DataTable productStepBudgetedProduct = Database.query("SELECT produtoOrcado.valorTotal FROM produtoOrcado WHERE produtoOrcado.numeroOrcamento = " + budget.NumeroOrcamento + " ORDER BY produtoOrcado.item;");
                            decimal valorTotalProdutos = 0;
                            foreach (DataRow dataRow in productStepBudgetedProduct.Rows) valorTotalProdutos += Convert.ToDecimal(dataRow.ItemArray[0]);
                            budget.ValorTotal = budget.ValorTrabalho + valorTotalProdutos;

                            //Atualiza orçamento
                            if (Database.updateBudget(budget))
                            {
                                DataTable dataTable = Database.query("SELECT * FROM produtoOrcado WHERE numeroORcamento  = " + Globals.numeroOrcamento + " ORDER BY numeroOrcamento DESC LIMIT 1;");
                                if (dataTable.Rows.Count > 0)
                                {
                                    //Já existem produtos no orçamento
                                    BudgetedProduct budgetedProduct = new BudgetedProduct();
                                    budgetedProduct.NumeroOrcamento = Globals.numeroOrcamento;
                                    decimal incomeValue = budget.ValorTotal / income.ParcelasReceita;

                                    //Atualiza valor total da receita, após parcelamento do valor
                                    if (Database.updateIncomeTotalValue(budgetedProduct, incomeValue))
                                    {
                                        DataTable parcelsDataTable = Database.query("SELECT idParcela FROM parcela WHERE idReceita = " + income.IdReceita);
                                        int success = 1;
                                        int i = 0;
                                        Globals.parcels.Clear();
                                        foreach (DataRow dataRow in parcelsDataTable.Rows)
                                        {
                                            Globals.parcels.Add(new Parcel());
                                            Globals.parcels[i].IdParcela = Convert.ToInt32(dataRow.ItemArray[0]);
                                            Globals.parcels[i].ValorParcela = incomeValue;
                                            if (Database.updateParcelValue(Globals.parcels[i]))
                                            {
                                                i++;
                                                continue;
                                            }
                                            else
                                            {
                                                success = 0;
                                                break;
                                            }
                                        }
                                        MessageBox.Show("Orçamento atualizado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        this.mtb_budgetDate.Focus();
                                        if (success == 0)
                                        {
                                            MessageBox.Show("[ERRO] Não foi possível atualizar o valor das parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                    }
                                    else MessageBox.Show("[ERRO] Não foi possível atualizar o valor total do orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    //Não existem produtos no orçamento
                                    MessageBox.Show("Orçamento atualizado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.mtb_budgetDate.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("[ERRO] Não foi possível atualizar orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.mtb_budgetDate.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("[ERRO] Não foi possível atualizar orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.mtb_budgetDate.Focus();
                        }
                    }
                    else
                    {
                        //Orçamento não parcelado
                        income.ParcelarValorReceita = false;
                        DataTable parcelsDataTable = Database.query("SELECT * FROM parcela WHERE idReceita = " + income.IdReceita);
                        bool success = true;
                        foreach (DataRow dataRow in parcelsDataTable.Rows)
                        {
                            Parcel parcel = new Parcel();
                            parcel.IdParcela = Convert.ToInt32(dataRow.ItemArray[0]);
                            if (Database.deleteParcel(parcel)) continue;
                            else
                            {
                                success = false;
                                MessageBox.Show("[ERRO] Não foi excluir a parcela!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        if (!success) MessageBox.Show("[ERRO] Não foi atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if(Database.updateBudget(budget)) this.updateOneParcel(income);
                    }
                    this.updateBudgetTotalValue(budget);
                }
            }
        }

        //Função que exclui orçamento
        private void budgetDelete()
        {
            Account account = new Account();
            Budget budget = new Budget();
            Income income = new Income();
            account.IdConta = Globals.idConta;
            budget.NumeroOrcamento = Convert.ToInt32(Globals.budgetStepDataTable.Rows[0].ItemArray[0]);
            income.IdReceita = Globals.idReceita;

            foreach (DataRow dataRow in Globals.incomeDataTable.Rows)
            {
                if (Convert.ToBoolean(dataRow.ItemArray[9]))
                {
                    if (Convert.ToBoolean(dataRow.ItemArray[12]))
                    {
                        int success = 1;
                        for (int i = 0; i < (Globals.parcels.Count); i++)
                        {
                            if (Database.deleteParcel(Globals.parcels[i])) continue;
                            else
                            {
                                success = 0;
                                break;
                            }
                        }
                        if (success == 0) MessageBox.Show("[ERRO] Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (!Database.deleteIncome(income)) MessageBox.Show("[ERRO] Não foi possível excluir receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (!Database.deleteIncome(income)) MessageBox.Show("[ERRO] Não foi possível excluir receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (!Database.deleteIncome(income)) MessageBox.Show("[ERRO] Não foi possível excluir receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (Database.deleteAllBudgetedProducts(budget)) if (Database.deleteBudget(budget)) MessageBox.Show("Orçamento excluído com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information); else MessageBox.Show("[ERRO] Não foi possível excluir orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Globals.step = 0;
            Globals.clientStep = new ClientStep(Globals.budgetForm, Globals.clientStepDataTable) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            Globals.budgetForm.panel_steps.Controls.Remove(Globals.budgetStep);
            Globals.budgetForm.panel_steps.Controls.Add(Globals.clientStep);
            Globals.clientStep.Show();
            Globals.budgetForm.pcb_btnGoBack.Visible = false;
            Globals.budgetForm.lbl_btnGoBackTag.Visible = false;
        }

        //Função que muda a cor do campo data
        private void setRGB(int r, int g, int b)
        {
            this.mtb_budgetDate.BackColor = Color.FromArgb(r, g, b);
            this.border_mtbBudgetDate.FillColor = Color.FromArgb(r, g, b);
            this.border_mtbBudgetDate.OnIdleState.FillColor = Color.FromArgb(r, g, b);
        }

        //ATUALIZAR ORÇAMENTO
        private void pcb_btnUpdate_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnUpdate.BackgroundImage = Properties.Resources.btn_update_active;
        }

        private void pcb_btnUpdate_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnUpdateTag.ClientRectangle.Contains(lbl_btnUpdateTag.PointToClient(Cursor.Position))) this.pcb_btnUpdate.BackgroundImage = Properties.Resources.btn_update;
        }

        private void pcb_btnUpdate_Click(object sender, EventArgs e)
        {
            this.budgetUpdate();
        }

        private void lbl_btnUpdateTag_Click(object sender, EventArgs e)
        {
            this.budgetUpdate();
        }

        //EXCLUIR ORÇAMENTO
        private void pcb_btnDelete_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnDelete.BackgroundImage = Properties.Resources.btn_delete_active;
        }
        private void pcb_btnDelete_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnDeleteTag.ClientRectangle.Contains(lbl_btnDeleteTag.PointToClient(Cursor.Position))) this.pcb_btnDelete.BackgroundImage = Properties.Resources.btn_delete;
        }

        private void pcb_btnDelete_Click(object sender, EventArgs e)
        {
            this.budgetDelete();
        }

        private void lbl_btnDeleteTag_Click(object sender, EventArgs e)
        {
            this.budgetDelete();
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO

        //KeyPress
        private void mtb_budgetDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }
        private void txt_laborValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ',') && (e.KeyChar != 'R') && (e.KeyChar != '$') && (e.KeyChar != ' ')) { e.Handled = true; }
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1)) { e.Handled = true; }
        }

        private void txt_parcels_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        //MouseEnter

        private void mtb_budgetDate_MouseEnter(object sender, EventArgs e)
        {
            this.setRGB(0, 104, 232);
        }

        //MouseLeave

        private void mtb_budgetDate_MouseLeave(object sender, EventArgs e)
        {
            if (!this.mtb_budgetDate.Focused) this.setRGB(0, 76, 157);
        }

        //Enter

        private void border_mtbBudgetDate_Enter(object sender, EventArgs e)
        {
            this.mtb_budgetDate.Focus();
            this.setRGB(0, 104, 232);
        }

        private void mtb_budgetDate_Enter(object sender, EventArgs e)
        {
            this.setRGB(0, 104, 232);
        }

        //Leave

        private void mtb_budgetDate_Leave(object sender, EventArgs e)
        {
            this.setRGB(0, 76, 157);
        }

        //OnChange

        private void ckb_parcelValue_OnChange(object sender, EventArgs e)
        {
            if (ckb_parcelValue.Checked)
            {
                this.txt_parcels.Show();
                this.cbb_period.Show();
            }
            else this.doNotParcel();
        }
    }
}
