using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class BudgetStep : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        public BudgetForm budgetStepBudgetForm;
        protected internal DataTable budgetStepDataTable, incomeDataTable;
        private int idReceita;
        private List<Parcel> parcels = new List<Parcel>();

        public BudgetStep(BudgetForm budgetForm, DataTable dataTable)
        {
            InitializeComponent();
            budgetStepBudgetForm = budgetForm;
            if (dataTable != null)
            {
                budgetStepDataTable = dataTable;
                foreach (DataRow dataRow in budgetStepDataTable.Rows)
                {
                    incomeDataTable = Database.query("SELECT * FROM receita WHERE numeroOrcamento = " + dataRow.ItemArray[0]);
                    this.mtb_budgetDate.Text = Convert.ToDateTime(dataRow.ItemArray[2]).ToShortDateString();
                    this.txt_laborValue.Text = String.Format("{0:C}", dataRow.ItemArray[3]);
                    this.cbb_paymentCondition.SelectedIndex = cbb_paymentCondition.FindString(" " + dataRow.ItemArray[5].ToString());
                    if (Convert.ToBoolean(dataRow.ItemArray[6]) == true) this.ckb_confirmedBudget.Checked = true; else this.ckb_confirmedBudget.Checked = false;
                }
                foreach (DataRow dataRow in incomeDataTable.Rows)
                {
                    idReceita = Convert.ToInt32(dataRow.ItemArray[0]);
                    //this.txt_incomeValue.Text = String.Format("{0:C}", dataRow.ItemArray[3]);
                    this.txt_incomeDescription.Text = dataRow.ItemArray[4].ToString();
                    //this.mtb_incomeDate.Text = (Convert.ToDateTime(dataRow.ItemArray[5]).ToShortDateString()).ToString();
                    this.txt_incomeObservations.Text = dataRow.ItemArray[7].ToString();

                    if (Convert.ToBoolean(dataRow.ItemArray[8]) == true) this.ckb_incomeReceived.Checked = true; else this.ckb_incomeReceived.Checked = false;

                    if (Convert.ToBoolean(dataRow.ItemArray[9]) == true)
                    {
                        this.ckb_parcelValue.Checked = true;
                        this.cbb_period.SelectedIndex = this.cbb_period.FindString(" " + dataRow.ItemArray[14].ToString().Trim());

                        this.txt_parcels.Show();
                        this.txt_parcels.Text = dataRow.ItemArray[13].ToString();
                        //this.txt_incomeValue.Text = String.Format("{0:C}", (Convert.ToDecimal(dataRow.ItemArray[3]) * Convert.ToInt32(dataRow.ItemArray[13])));
                        int i = 0;
                        foreach (DataRow dataRowParcels in Database.query("SELECT * FROM parcela WHERE idReceita = " + idReceita.ToString()).Rows)
                        {
                            parcels.Add(new Parcel());
                            parcels[i].idParcela = Convert.ToInt32(dataRowParcels.ItemArray[0]);
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
                this.pcb_btnUpdate.Visible = false;
                this.pcb_btnDelete.Visible = false;
            }
        }

        //FUNÇÃO QUE RETORNA OS COMPONENTES DO FORMULÁRIO AO ESTADO INICIAL CASO NÃO HAJA PARCELAS
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

        //FUNÇÃO QUE RETORNA OS CAMPOS DO FORMULÁRIO AO ESTADO INICIAL
        private void clearFields()
        {
            this.txt_laborValue.Clear();
            this.cbb_paymentCondition.SelectedIndex = this.cbb_paymentCondition.FindString(" Dinheiro");
            this.txt_incomeDescription.Clear();
            this.mtb_budgetDate.Text = (DateTime.Today.ToShortDateString()).ToString();
            this.txt_incomeObservations.Clear();
            this.doNotParcel();
        }

        //FUNÇÃO QUE SELECIONA A DATA DA TRANSAÇÃO DA PARCELA DE ACORDO COM O PERÍDO ESCOLHIDO PELO USUÁRIO
        private void periodSelection(int i, Income income, List<Parcel> parcels)
        {
            if (i == 0)
            {
                switch (income.periodoRepetirParcelarReceita)
                {
                    case "Diário":
                        parcels[i].dataTransacao = income.dataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        parcels[i].dataTransacao = income.dataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        parcels[i].dataTransacao = income.dataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        parcels[i].dataTransacao = income.dataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        parcels[i].dataTransacao = income.dataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        parcels[i].dataTransacao = income.dataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        parcels[i].dataTransacao = income.dataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        parcels[i].dataTransacao = income.dataTransacao.AddYears(1);
                        break;
                }
            }
            else
            {
                switch (income.periodoRepetirParcelarReceita)
                {
                    case "Diário":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddYears(1);
                        break;
                }
            }
        }

        //FUNÇÃO QUE ATUALIZA UMA ÚNICA PARCELA OU REPETIÇÃO POR VEZ
        private void updateOneParcel(Income income)
        {
            if (Database.updateIncomeNotParceledOrRepeated(income))
            {
                MessageBox.Show("Receita atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.doNotParcel();
            }
            else MessageBox.Show("[ERRO] Não foi possível atualizar receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //FUNÇÃO QUE ATUALIZA PARCELA OU REPETIÇÃO
        private bool updateParcel(Income income)
        {
            DataTable parcelOrRepeatDataTable = Database.query("SELECT repetirParcelarReceita, valorFixoReceita, parcelasReceita, repeticoesValorFixoReceita FROM receita WHERE idReceita = " + idReceita);
            if (Database.updateIncome(income))
            {
                int qtde = Convert.ToInt32(Database.query("SELECT repeticoesValorFixoReceita FROM receita WHERE idReceita = " + idReceita).Rows[0].ItemArray[0]);
                if (qtde == 0)
                {
                    //Valor parcelado
                    this.parcels.Clear();
                    qtde = Convert.ToInt32(Database.query("SELECT parcelasReceita FROM receita WHERE idReceita = " + idReceita).Rows[0].ItemArray[0]);
                    if (qtde == 1)
                    {
                        this.updateOneParcel(income);
                        return true;
                    }
                    DataTable dataTable = Database.query("SELECT idParcela FROM parcela WHERE idReceita = " + idReceita);
                    if (dataTable.Rows.Count == 0)
                    {
                        for (int j = 0; j < (income.parcelasReceita - 1); j++)
                        {
                            parcels.Add(new Parcel());
                            parcels[j].idReceita = idReceita;
                            parcels[j].valorParcela = income.valorReceita;
                            parcels[j].descricaoParcela = income.descricaoReceita;
                            this.periodSelection(j, income, this.parcels);
                            parcels[j].categoriaParcela = income.categoriaReceita;
                            parcels[j].observacoesParcela = income.observacoesReceita;
                            parcels[j].recebimentoConfirmado = false;
                            if (Database.newParcel(parcels[j])) continue;
                            else
                            {
                                MessageBox.Show("[ERRO] Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                    }
                    dataTable = Database.query("SELECT idParcela FROM parcela WHERE idReceita = " + idReceita);
                    //Parcelas
                    int i = 0;
                    foreach (DataRow dataRowParcels in dataTable.Rows)
                    {
                        this.parcels.Add(new Parcel());
                        this.parcels[i].idParcela = Convert.ToInt32(dataRowParcels.ItemArray[0]);
                        i++;
                    }
                    if (this.changeParcel(parcelOrRepeatDataTable, income)) return true; else return false;
                }
                else return false;
            }
            else
            {
                MessageBox.Show("[ERRO] Não foi possível atualizar receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //FUNÇÃO QUE ATUALIZA AS INFORMAÇÕES DAS PARCELAS
        private bool changeParcel(DataTable dataTable, Income income)
        {
            List<Parcel> parcels = new List<Parcel>();
            int success = 1;
            int qtde = 0;
            if (dataTable.Rows[0].ItemArray[2] != System.DBNull.Value) qtde = Convert.ToInt32(dataTable.Rows[0].ItemArray[2]);
            if (Convert.ToBoolean(dataTable.Rows[0].ItemArray[0]))
            {
                //Já existiam parcelas ou repetições antes
                if (qtde != income.parcelasReceita)
                {
                    if (qtde > income.parcelasReceita)
                    {
                        //Diminuiu parcelas
                        for (int i = 0; i < (income.parcelasReceita - 1); i++)
                        {
                            parcels.Add(new Parcel());
                            parcels[i].idParcela = this.parcels[i].idParcela;
                            parcels[i].valorParcela = income.valorReceita;
                            parcels[i].descricaoParcela = income.descricaoReceita;
                            this.periodSelection(i, income, parcels);
                            parcels[i].categoriaParcela = income.categoriaReceita;
                            parcels[i].observacoesParcela = income.observacoesReceita;
                            parcels[i].recebimentoConfirmado = false;
                            if (Database.updateParcel(parcels[i])) continue;
                            else
                            {
                                success = 0;
                                return false;
                            }
                        }
                        for (int i = (income.parcelasReceita - 1); i < (qtde - 1); i++)
                        {
                            parcels.Add(new Parcel());
                            parcels[i].idParcela = this.parcels[i].idParcela;
                            if (Database.deleteParcel(parcels[i])) continue;
                            else
                            {
                                success = 0;
                                return false;
                            }
                        }
                        return true;
                    }
                    else if (qtde < income.parcelasReceita)
                    {
                        //Aumentou parcelas
                        for (int i = 0; i < (qtde - 1); i++)
                        {
                            parcels.Add(new Parcel());
                            parcels[i].idParcela = this.parcels[i].idParcela;
                            parcels[i].valorParcela = income.valorReceita;
                            parcels[i].descricaoParcela = income.descricaoReceita;
                            this.periodSelection(i, income, parcels);
                            parcels[i].categoriaParcela = income.categoriaReceita;
                            parcels[i].observacoesParcela = income.observacoesReceita;
                            parcels[i].recebimentoConfirmado = false;
                            if (Database.updateParcel(parcels[i])) continue;
                            else
                            {
                                success = 0;
                                return false;
                            }
                        }
                        for (int i = (qtde - 1); i < (income.parcelasReceita - 1); i++)
                        {
                            parcels.Add(new Parcel());
                            parcels[i].idReceita = idReceita;
                            parcels[i].valorParcela = income.valorReceita;
                            parcels[i].descricaoParcela = income.descricaoReceita;
                            this.periodSelection(i, income, parcels);
                            parcels[i].categoriaParcela = income.categoriaReceita;
                            parcels[i].observacoesParcela = income.observacoesReceita;
                            parcels[i].recebimentoConfirmado = false;
                            if (Database.newParcel(parcels[i])) continue;
                            else
                            {
                                success = 0;
                                return false;
                            }
                        }
                        return true;
                    }
                }
                else
                {
                    for (int i = 0; i < (income.parcelasReceita - 1); i++)
                    {
                        parcels.Add(new Parcel());
                        parcels[i].idParcela = this.parcels[i].idParcela;
                        parcels[i].valorParcela = income.valorReceita;
                        parcels[i].descricaoParcela = income.descricaoReceita;
                        this.periodSelection(i, income, parcels);
                        parcels[i].categoriaParcela = income.categoriaReceita;
                        parcels[i].observacoesParcela = income.observacoesReceita;
                        parcels[i].recebimentoConfirmado = false;
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
                    MessageBox.Show("[ERRO} Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show("Receita atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.mtb_budgetDate.Focus();
                    return true;
                }
            }
            else
            {
                MessageBox.Show("Receita atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.mtb_budgetDate.Focus();
                return true;
            }
        }

        //ATUALIZAR PRODUTO
        private void pcb_btnUpdate_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnUpdate.Image = Properties.Resources.btn_edit_active;
        }

        private void pcb_btnUpdate_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnUpdate.Image = Properties.Resources.btn_edit;
        }

        private void pcb_btnUpdate_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(mtb_budgetDate.Text.Trim()) || String.IsNullOrEmpty(txt_laborValue.Text.Trim())) MessageBox.Show("Informe o valor e a data da receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if ((this.ckb_parcelValue.Checked) && ((String.IsNullOrEmpty(this.txt_parcels.Text.Trim())) || (this.cbb_period.SelectedIndex == -1))) MessageBox.Show("Informe o número de parcelas da receita e o período em que elas se repetem!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    Budget budget = new Budget();
                    Income income = new Income();
                    budget.numeroOrcamento = Convert.ToInt32(this.budgetStepDataTable.Rows[0].ItemArray[0]);
                    budget.idCliente = Convert.ToInt32(this.budgetStepDataTable.Rows[0].ItemArray[1]);
                    income.idReceita = idReceita;
                    income.numeroOrcamento = Convert.ToInt32(this.budgetStepDataTable.Rows[0].ItemArray[0]);
                    budget.dataOrcamento = Convert.ToDateTime(this.mtb_budgetDate.Text.Trim());
                    income.dataTransacao = Convert.ToDateTime(this.mtb_budgetDate.Text.Trim());
                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                    string valorTrabalho = this.txt_laborValue.Text;
                    if (regexValor.IsMatch(valorTrabalho))
                    {
                        if (valorTrabalho.Contains("R$ "))
                        {
                            budget.valorTrabalho = Convert.ToDecimal(valorTrabalho.Substring(3));
                            budget.valorTotal = Convert.ToDecimal(valorTrabalho.Substring(3));
                            income.valorReceita = Convert.ToDecimal(valorTrabalho.Substring(3));
                        }
                        else if (valorTrabalho.Contains("R$"))
                        {
                            budget.valorTrabalho = Convert.ToDecimal(valorTrabalho.Substring(2));
                            budget.valorTotal = Convert.ToDecimal(valorTrabalho.Substring(2));
                            income.valorReceita = Convert.ToDecimal(valorTrabalho.Substring(2));
                        }
                        else
                        {
                            budget.valorTrabalho = Convert.ToDecimal(this.txt_laborValue.Text);
                            budget.valorTotal = Convert.ToDecimal(this.txt_laborValue.Text);
                            income.valorReceita = Convert.ToDecimal(this.txt_laborValue.Text);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.txt_laborValue.Clear();
                        this.txt_laborValue.PlaceholderText = "";
                        this.txt_laborValue.Focus();
                        return;
                    }
                    budget.condicaoPagamento = this.cbb_paymentCondition.SelectedItem.ToString().Trim();
                    if (this.ckb_confirmedBudget.Checked) budget.orcamentoConfirmado = true; else budget.orcamentoConfirmado = false;
                    income.descricaoReceita = this.txt_incomeDescription.Text;
                    income.categoriaReceita = " Orçamentos";
                    income.idConta = 1;
                    income.observacoesReceita = this.txt_incomeObservations.Text;
                    if (this.ckb_incomeReceived.Checked) income.recebimentoConfirmado = true; else income.recebimentoConfirmado = false;

                    if (ckb_parcelValue.Checked)
                    {
                        //Orçamento parcelado
                        income.repetirParcelarReceita = true;

                        //Parcela e atualiza valor do orçamento
                        income.parcelarValorReceita = true;
                        income.parcelasReceita = Convert.ToInt32(txt_parcels.Text);
                        income.periodoRepetirParcelarReceita = cbb_period.SelectedItem.ToString().Trim();
                        income.valorReceita = income.valorReceita / income.parcelasReceita;
                        if (this.updateParcel(income))
                        {
                            //Soma o valor total de cada produto do orçamento e atribui a diferença do valor dos produtos pelo valor do trabalho
                            DataTable productStepBudgetedProduct = Database.query("SELECT produtoOrcado.valorTotal FROM produtoOrcado WHERE produtoOrcado.numeroOrcamento = " + budget.numeroOrcamento + " ORDER BY produtoOrcado.item;");
                            decimal valorTotalProdutos = 0;
                            foreach (DataRow dataRow in productStepBudgetedProduct.Rows) valorTotalProdutos += Convert.ToDecimal(dataRow.ItemArray[0]);
                            budget.valorTotal = budget.valorTrabalho + valorTotalProdutos;
                            MessageBox.Show("Valor total dos produtos " + valorTotalProdutos.ToString());
                            MessageBox.Show("Valor do trabalho " + budget.valorTrabalho.ToString());
                            MessageBox.Show("Valor total " + budget.valorTotal.ToString());

                            //Atualiza orçamento
                            if (Database.updateBudget(budget))
                            {
                                DataTable dataTable = Database.query("SELECT * FROM produtoOrcado WHERE numeroORcamento  = " + budget.numeroOrcamento + " ORDER BY numeroOrcamento DESC LIMIT 1;");
                                if(dataTable.Rows.Count > 0)
                                {
                                    //Já existem produtos no orçamento
                                    BudgetedProduct budgetedProduct = new BudgetedProduct();
                                    budgetedProduct.numeroOrcamento = budget.numeroOrcamento;
                                    decimal incomeValue = budget.valorTotal;
                                    incomeValue = incomeValue / income.parcelasReceita;

                                    //Atualiza valor total da receita, após parcelamento do valor
                                    if (Database.updateIncomeTotalValue(budgetedProduct, incomeValue))
                                    {
                                        DataTable parcelsDataTable = Database.query("SELECT idParcela FROM parcela WHERE idReceita = " + income.idReceita);
                                        int success = 1;
                                        int i = 0;
                                        this.parcels.Clear();
                                        foreach (DataRow dataRow in parcelsDataTable.Rows)
                                        {
                                            parcels.Add(new Parcel());
                                            parcels[i].idParcela = Convert.ToInt32(dataRow.ItemArray[0]);
                                            parcels[i].valorParcela = incomeValue;
                                            if (Database.updateParcelValue(parcels[i]))
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
                        income.parcelarValorReceita = false;
                        if (Database.deleteAllParcels(income)) this.updateOneParcel(income);
                    }
                }
            }
        }

        //EXCLUIR PRODUTO
        private void pcb_btnDelete_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnDelete.Image = Properties.Resources.btn_delete_active;
        }

        private void pcb_btnDelete_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnDelete.Image = Properties.Resources.btn_delete;
        }

        private void pcb_btnDelete_Click(object sender, EventArgs e)
        {
            Budget budget = new Budget();
            Income income = new Income();
            budget.numeroOrcamento = Convert.ToInt32(budgetStepDataTable.Rows[0].ItemArray[0]);
            income.idReceita = idReceita;
            if (Database.deleteAllParcels(income)) if (Database.deleteAllRepeats(income)) if(Database.deleteIncome(income)) if (Database.deleteAllBudgetedProducts(budget)) if (Database.deleteBudget(budget)) MessageBox.Show("Orçamento excluído com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information); else MessageBox.Show("[ERRO] Não foi possível excluir orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            budgetStepBudgetForm.step = 0;
            budgetStepBudgetForm.clientStep = new ClientStep(budgetStepBudgetForm, budgetStepBudgetForm.clientStepDataTable) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            budgetStepBudgetForm.panel_steps.Controls.Remove(budgetStepBudgetForm.budgetStep);
            budgetStepBudgetForm.panel_steps.Controls.Add(budgetStepBudgetForm.clientStep);
            budgetStepBudgetForm.clientStep.Show();
            budgetStepBudgetForm.pcb_btnGoBack.Visible = false;
        }

        private void ckb_parcelValue_OnChange(object sender, EventArgs e)
        {
            if (ckb_parcelValue.Checked)
            {
                this.txt_parcels.Show();
                this.cbb_period.Show();
            }
            else this.doNotParcel();
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO

        private void txt_laborValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ',') && (e.KeyChar != 'R') && (e.KeyChar != '$') && (e.KeyChar != ' ')) { e.Handled = true; }
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1)) { e.Handled = true; }
        }
    }
}
