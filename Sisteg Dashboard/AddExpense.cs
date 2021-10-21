using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class AddExpense : Form
    {
        //INICIA INSTÂNCIA DO PAINEL CONJUNTO À UM DATATABLE DE DESPESAS PARA ATUALIZAÇÃO E EXCLUSÃO, QUE PODE SER NULO EM CASO DE CADASTRO
        public AddExpense(DataTable dataTable, Main main)
        {
            InitializeComponent();
            Globals.expenseDataTable = dataTable;
            Globals.main = main;
            this.txt_expenseValue.Focus();

            //Popula o combobox de conta da despesa
            DataTable expenseAccountDataTable = Database.query("SELECT conta.nomeConta FROM conta ORDER BY conta.nomeConta;");
            for (int i = 0; i < expenseAccountDataTable.Rows.Count; i++) this.cbb_expenseAccount.Items.Insert(i, " " + expenseAccountDataTable.Rows[i].ItemArray[0].ToString().Trim());

            //Popula o combobox de categoria da despesa
            DataTable expenseCategoryDataTable = Database.query("SELECT categoria.nomeCategoria FROM categoria WHERE categoria.categoriaDespesa = true ORDER BY categoria.nomeCategoria;");
            for (int i = 0; i < expenseCategoryDataTable.Rows.Count; i++) this.cbb_expenseCategory.Items.Insert(i, " " + expenseCategoryDataTable.Rows[i].ItemArray[0].ToString().Trim());

            if (Globals.idConta != 0)
            {
                DataTable sumTotalValueDataTable = Database.query("SELECT somarTotal FROM conta WHERE idConta = " + Globals.idConta);
                if (sumTotalValueDataTable.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(sumTotalValueDataTable.Rows[0].ItemArray[0])) Globals.saldoConta = Convert.ToDecimal(Database.query("SELECT saldoConta FROM conta WHERE idConta = " + Globals.idConta).Rows[0].ItemArray[0]);
                    else Globals.saldoConta = 0;
                }
            }

            if (Globals.saldoConta < 0)
            {
                Globals.main.lbl_balanceTag.ForeColor = Color.FromArgb(243, 104, 82);
                Globals.main.lbl_balance.ForeColor = Color.FromArgb(243, 104, 82);
            }
            else
            {
                Globals.main.lbl_balanceTag.ForeColor = Color.FromArgb(77, 255, 255);
                Globals.main.lbl_balance.ForeColor = Color.FromArgb(77, 255, 255);
            }

            if (Globals.expenseDataTable != null)
            {
                foreach (DataRow dataRow in Globals.expenseDataTable.Rows)
                {
                    Globals.idDespesa = Convert.ToInt32(dataRow.ItemArray[0]);
                    Globals.idConta = Convert.ToInt32(dataRow.ItemArray[1]);
                    Globals.idCategoria = Convert.ToInt32(dataRow.ItemArray[2]);

                    this.txt_expenseValue.Text = String.Format("{0:C}", dataRow.ItemArray[3]).Trim();
                    this.txt_expenseDescription.Text = dataRow.ItemArray[4].ToString().Trim();
                    this.mtb_expenseDate.Text = (Convert.ToDateTime(dataRow.ItemArray[5]).ToShortDateString()).ToString().Trim();
                    this.txt_expenseObservations.Text = dataRow.ItemArray[6].ToString().Trim();
                    this.cbb_expenseCategory.SelectedIndex = this.cbb_expenseCategory.FindString(" " + Database.query("SELECT nomeCategoria FROM categoria WHERE idCategoria = " + Globals.idCategoria).Rows[0].ItemArray[0].ToString().Trim());
                    this.cbb_expenseAccount.SelectedIndex = this.cbb_expenseAccount.FindString(" " + Database.query("SELECT nomeConta FROM conta WHERE idConta = " + Globals.idConta).Rows[0].ItemArray[0].ToString().Trim());

                    if (Convert.ToBoolean(dataRow.ItemArray[7])) this.ckb_expensePaid.Checked = true; 
                    else this.ckb_expensePaid.Checked = false;

                    if (Convert.ToBoolean(dataRow.ItemArray[8]))
                    {
                        this.ckb_repeatOrParcelValue.Checked = true;
                        this.cbb_period.SelectedIndex = this.cbb_period.FindString(dataRow.ItemArray[13].ToString().Trim());
                    }
                    else
                    {
                        this.ckb_repeatOrParcelValue.Checked = false;
                        this.rbtn_fixedValue.Hide();
                        this.lbl_fixedValue.Hide();
                        this.rbtn_fixedValue.Checked = false;
                        this.rbtn_parcelValue.Hide();
                        this.lbl_parcelValue.Hide();
                        this.rbtn_parcelValue.Checked = false;
                        this.txt_repeatsOrParcels.Hide();
                        this.cbb_period.Hide();
                        break;
                    }

                    if (dataRow.ItemArray[9] != null)
                    {
                        if (Convert.ToBoolean(dataRow.ItemArray[9]))
                        {
                            this.rbtn_fixedValue.Show();
                            this.lbl_fixedValue.Show();
                            this.rbtn_fixedValue.Checked = true;
                            this.rbtn_parcelValue.Checked = false;
                            this.txt_repeatsOrParcels.Show();
                            this.txt_repeatsOrParcels.Text = dataRow.ItemArray[10].ToString().Trim();
                            int i = 0;
                            foreach (DataRow dataRowRepeats in Database.query("SELECT * FROM repeticao WHERE idDespesa = " + Globals.idDespesa).Rows)
                            {
                                Globals.repeats.Add(new Repeat());
                                Globals.repeats[i].IdRepeticao = Convert.ToInt32(dataRowRepeats.ItemArray[0]);
                                Globals.repeats[i].ValorRepeticao = Convert.ToDecimal(dataRowRepeats.ItemArray[5]);
                                i++;
                            }
                        }
                        else this.rbtn_fixedValue.Checked = false;
                    }
                    else this.rbtn_fixedValue.Checked = false;

                    if (dataRow.ItemArray[12].ToString().Trim() != null)
                    {
                        if (Convert.ToBoolean(dataRow.ItemArray[11]) == true)
                        {
                            this.rbtn_parcelValue.Show();
                            this.lbl_parcelValue.Show();
                            this.rbtn_fixedValue.Checked = false;
                            this.rbtn_parcelValue.Checked = true;
                            this.txt_repeatsOrParcels.Show();
                            this.txt_repeatsOrParcels.Text = dataRow.ItemArray[12].ToString().Trim();
                            this.txt_expenseValue.Text = String.Format("{0:C}", (Convert.ToDecimal(dataRow.ItemArray[3]) * Convert.ToInt32(dataRow.ItemArray[12]))).Trim();
                            int i = 0;
                            foreach (DataRow dataRowParcels in Database.query("SELECT * FROM parcela WHERE idDespesa = " + Globals.idDespesa).Rows)
                            {
                                Globals.parcels.Add(new Parcel());
                                Globals.parcels[i].IdParcela = Convert.ToInt32(dataRowParcels.ItemArray[0]);
                                Globals.parcels[i].ValorParcela = Convert.ToDecimal(dataRowParcels.ItemArray[5]);
                                i++;
                            }
                        }
                        else this.rbtn_parcelValue.Checked = false;
                    }
                    else this.rbtn_parcelValue.Checked = false;
                }
            }
            else this.clearFields();
        }

        //EVITA TREMULAÇÃO DE COMPONENTES
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handlerparam = base.CreateParams;
                handlerparam.ExStyle |= 0x02000000;
                return handlerparam;
            }
        }

        //FUNÇÕES

        //Função que retorna os componentes do formulário ao estado inicial, caso não haja repetições ou parcelas
        private void doNotRepeatOrParcel()
        {
            this.ckb_repeatOrParcelValue.Checked = false;
            this.rbtn_fixedValue.Hide();
            this.lbl_fixedValue.Hide();
            this.rbtn_fixedValue.Checked = false;
            this.rbtn_parcelValue.Hide();
            this.lbl_parcelValue.Hide();
            this.rbtn_parcelValue.Checked = false;
            this.txt_repeatsOrParcels.Clear();
            this.txt_repeatsOrParcels.Hide();
            this.cbb_period.SelectedIndex = -1;
            this.cbb_period.Text = " Período";
            this.cbb_period.Hide();
            this.txt_expenseValue.PlaceholderText = "";
            this.txt_expenseValue.Focus();
        }

        //Função que retorna os campos do formulário ao estado inicial
        private void clearFields()
        {
            this.txt_expenseValue.Clear();
            this.txt_expenseDescription.Clear();
            this.mtb_expenseDate.Text = (DateTime.Today.ToShortDateString()).ToString().Trim();
            this.cbb_expenseCategory.SelectedIndex = -1;
            this.cbb_expenseCategory.Text = " Categoria";
            this.txt_expenseObservations.Clear();
            this.doNotRepeatOrParcel();
        }

        //Função que seleciona a data de transação da repetição de acordo com o período escolhido pelo usuário
        private void periodSelection(int i, Expense expense, List<Repeat> repeats)
        {
            if (i == 0)
            {
                switch (expense.PeriodoRepetirParcelarDespesa)
                {
                    case "Diário":
                        repeats[i].DataTransacao = expense.DataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        repeats[i].DataTransacao = expense.DataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        repeats[i].DataTransacao = expense.DataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        repeats[i].DataTransacao = expense.DataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        repeats[i].DataTransacao = expense.DataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        repeats[i].DataTransacao = expense.DataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        repeats[i].DataTransacao = expense.DataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        repeats[i].DataTransacao = expense.DataTransacao.AddYears(1);
                        break;
                }
            }
            else
            {
                switch (expense.PeriodoRepetirParcelarDespesa)
                {
                    case "Diário":
                        repeats[i].DataTransacao = repeats[i - 1].DataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        repeats[i].DataTransacao = repeats[i - 1].DataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        repeats[i].DataTransacao = repeats[i - 1].DataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        repeats[i].DataTransacao = repeats[i - 1].DataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        repeats[i].DataTransacao = repeats[i - 1].DataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        repeats[i].DataTransacao = repeats[i - 1].DataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        repeats[i].DataTransacao = repeats[i - 1].DataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        repeats[i].DataTransacao = repeats[i - 1].DataTransacao.AddYears(1);
                        break;
                }
            }
        }

        //Função que seleciona a data de transação da parcela de acordo com o período escolhido pelo usuário
        private void periodSelection(int i, Expense expense, List<Parcel> parcels)
        {
            if (i == 0)
            {
                switch (expense.PeriodoRepetirParcelarDespesa)
                {
                    case "Diário":
                        parcels[i].DataTransacao = expense.DataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        parcels[i].DataTransacao = expense.DataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        parcels[i].DataTransacao = expense.DataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        parcels[i].DataTransacao = expense.DataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        parcels[i].DataTransacao = expense.DataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        parcels[i].DataTransacao = expense.DataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        parcels[i].DataTransacao = expense.DataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        parcels[i].DataTransacao = expense.DataTransacao.AddYears(1);
                        break;
                }
            }
            else
            {
                switch (expense.PeriodoRepetirParcelarDespesa)
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

        //Função que atualiza uma única parcela ou repetição por vez
        private void updateOneParcelOrRepeat(Expense expense)
        {
            if (Database.updateExpenseNotParceledOrRepeated(expense))
            {
                MessageBox.Show("Receita atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.doNotRepeatOrParcel();
            }
            else MessageBox.Show("[ERRO] Não foi possível atualizar despesa!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Função que atualiza parcela ou repetição
        private void updateParcelOrRepeat(Expense expense)
        {
            DataTable parcelOrRepeatDataTable = Database.query("SELECT repetirParcelarDespesa, valorFixoDespesa, parcelasDespesa, repeticoesValorFixoDespesa FROM despesa WHERE idDespesa = " + Globals.idDespesa);
            
            if (Database.updateExpense(expense))
            {
                int qtde = Convert.ToInt32(Database.query("SELECT repeticoesValorFixoDespesa FROM despesa WHERE idDespesa = " + Globals.idDespesa).Rows[0].ItemArray[0]);
                if (qtde == 0)
                {
                    //Valor parcelado
                    qtde = Convert.ToInt32(Database.query("SELECT parcelasDespesa FROM despesa WHERE idDespesa = " + Globals.idDespesa).Rows[0].ItemArray[0]);
                    Globals.parcels.Clear();
                    if (qtde == 1) this.updateOneParcelOrRepeat(expense);
                    DataTable dataTable = Database.query("SELECT idParcela FROM parcela WHERE idDespesa = " + Globals.idDespesa);
                    if (dataTable.Rows.Count == 0)
                    {
                        for (int j = 0; j < (expense.ParcelasDespesa - 1); j++)
                        {
                            Globals.parcels.Add(new Parcel());
                            Globals.parcels[j].IdDespesa = Globals.idDespesa;
                            Globals.parcels[j].IdConta = expense.IdConta;
                            Globals.parcels[j].IdCategoria = expense.IdCategoria;
                            Globals.parcels[j].ValorParcela = expense.ValorDespesa;
                            Globals.parcels[j].DescricaoParcela = expense.DescricaoDespesa;
                            this.periodSelection(j, expense, Globals.parcels);
                            Globals.parcels[j].ObservacoesParcela = expense.ObservacoesDespesa;
                            if (ckb_expensePaid.Checked) Globals.parcels[j].PagamentoConfirmado = true; 
                            else Globals.parcels[j].PagamentoConfirmado = false;

                            if (Database.newParcel(Globals.parcels[j])) continue;
                            else
                            {
                                MessageBox.Show("[ERRO] Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                    }
                    dataTable = Database.query("SELECT idParcela FROM parcela WHERE idDespesa = " + Globals.idDespesa);
                    //Parcelas
                    int i = 0;
                    foreach (DataRow dataRowParcels in dataTable.Rows)
                    {
                        if (Globals.parcels.Count == 0 && i == 0) Globals.parcels.Add(new Parcel());
                        Globals.parcels[i].IdParcela = Convert.ToInt32(dataRowParcels.ItemArray[0]);
                        i++;
                    }
                    this.changeParcel(parcelOrRepeatDataTable, expense);
                }
                else
                {
                    //Valor repetido
                    Globals.repeats.Clear();
                    if (qtde == 1) this.updateOneParcelOrRepeat(expense);
                    DataTable dataTable = Database.query("SELECT idRepeticao FROM repeticao WHERE idDespesa = " + Globals.idDespesa);
                    if (dataTable.Rows.Count == 0)
                    {
                        for (int j = 0; j < (expense.RepeticoesValorFixoDespesa - 1); j++)
                        {
                            Globals.repeats.Add(new Repeat());
                            Globals.repeats[j].IdDespesa = Globals.idDespesa;
                            Globals.repeats[j].IdConta = expense.IdConta;
                            Globals.repeats[j].IdCategoria = expense.IdCategoria;
                            Globals.repeats[j].ValorRepeticao = expense.ValorDespesa;
                            Globals.repeats[j].DescricaoRepeticao = expense.DescricaoDespesa;
                            this.periodSelection(j, expense, Globals.repeats);
                            Globals.repeats[j].ObservacoesRepeticao = expense.ObservacoesDespesa;
                            if (ckb_expensePaid.Checked) Globals.repeats[j].PagamentoConfirmado = true; 
                            else Globals.repeats[j].PagamentoConfirmado = false;

                            if (Database.newRepeat(Globals.repeats[j])) continue;
                            else
                            {
                                MessageBox.Show("[ERRO] Não foi possível atualizar todas as repetições!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                    }
                    dataTable = Database.query("SELECT idRepeticao FROM repeticao WHERE idDespesa = " + Globals.idDespesa);
                    //Repetições
                    if (Globals.repeats.Count == 0)
                    {
                        int i = 0;
                        foreach (DataRow dataRowRepeats in dataTable.Rows)
                        {
                            Globals.repeats.Add(new Repeat());
                            Globals.repeats[i].IdRepeticao = Convert.ToInt32(dataRowRepeats.ItemArray[0]);
                            i++;
                        }
                        this.changeRepeat(parcelOrRepeatDataTable, expense);
                    }
                    else
                    {
                        int i = 0;
                        foreach (DataRow dataRowRepeats in dataTable.Rows)
                        {
                            Globals.repeats[i].IdRepeticao = Convert.ToInt32(dataRowRepeats.ItemArray[0]);
                            i++;
                        }
                        this.changeRepeat(parcelOrRepeatDataTable, expense);
                    }
                }
            }
            else MessageBox.Show("[ERRO] Não foi possível atualizar despesa!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Função que atualiza as informações das repetições
        private void changeRepeat(DataTable dataTable, Expense expense)
        {
            List<Repeat> repeats = new List<Repeat>();
            int success = 1;
            int qtde = 0;
            if (dataTable.Rows[0].ItemArray[3] != System.DBNull.Value) qtde = Convert.ToInt32(dataTable.Rows[0].ItemArray[3]);
            if (Convert.ToBoolean(dataTable.Rows[0].ItemArray[0]))
            {
                //Já existiam parcelas ou repetições antes
                if (Convert.ToBoolean(dataTable.Rows[0].ItemArray[1]))
                {
                    //Não alterou a forma de divisão do pagamento
                    if (qtde != expense.RepeticoesValorFixoDespesa)
                    {
                        if (qtde > expense.RepeticoesValorFixoDespesa)
                        {
                            //Diminuiu repetições
                            for (int i = 0; i < (expense.RepeticoesValorFixoDespesa - 1); i++)
                            {
                                repeats.Add(new Repeat());
                                repeats[i].IdRepeticao = Globals.repeats[i].IdRepeticao;
                                repeats[i].IdConta = expense.IdConta;
                                repeats[i].IdCategoria = expense.IdCategoria;
                                repeats[i].ValorRepeticao = expense.ValorDespesa;
                                repeats[i].DescricaoRepeticao = expense.DescricaoDespesa;
                                this.periodSelection(i, expense, repeats);
                                repeats[i].ObservacoesRepeticao = expense.ObservacoesDespesa;
                                if (ckb_expensePaid.Checked) repeats[i].PagamentoConfirmado = true; 
                                else repeats[i].PagamentoConfirmado = false;

                                if (Database.updateRepeat(repeats[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                            for (int i = (expense.RepeticoesValorFixoDespesa - 1); i < (qtde - 1); i++)
                            {
                                repeats.Add(new Repeat());
                                repeats[i].IdRepeticao = Globals.repeats[i].IdRepeticao;
                                if (Database.deleteRepeat(repeats[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                        }
                        else if (qtde < expense.RepeticoesValorFixoDespesa)
                        {
                            //Aumentou repetições
                            for (int i = 0; i < (qtde - 1); i++)
                            {
                                repeats.Add(new Repeat());
                                repeats[i].IdRepeticao = Globals.repeats[i].IdRepeticao;
                                repeats[i].IdConta = expense.IdConta;
                                repeats[i].IdCategoria = expense.IdCategoria;
                                repeats[i].ValorRepeticao = expense.ValorDespesa;
                                repeats[i].DescricaoRepeticao = expense.DescricaoDespesa;
                                this.periodSelection(i, expense, repeats);
                                repeats[i].ObservacoesRepeticao = expense.ObservacoesDespesa;
                                if (ckb_expensePaid.Checked) repeats[i].PagamentoConfirmado = true; 
                                else repeats[i].PagamentoConfirmado = false;

                                if (Database.updateRepeat(repeats[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                            for (int i = (qtde - 1); i < (expense.RepeticoesValorFixoDespesa - 1); i++)
                            {
                                repeats.Add(new Repeat());
                                repeats[i].IdDespesa = Globals.idDespesa;
                                repeats[i].IdConta = expense.IdConta;
                                repeats[i].IdCategoria = expense.IdCategoria;
                                repeats[i].ValorRepeticao = expense.ValorDespesa;
                                repeats[i].DescricaoRepeticao = expense.DescricaoDespesa;
                                this.periodSelection(i, expense, repeats);
                                repeats[i].ObservacoesRepeticao = expense.ObservacoesDespesa;
                                if (ckb_expensePaid.Checked) repeats[i].PagamentoConfirmado = true; 
                                else repeats[i].PagamentoConfirmado = false;

                                if (Database.newRepeat(repeats[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < (expense.RepeticoesValorFixoDespesa - 1); i++)
                        {
                            repeats.Add(new Repeat());
                            repeats[i].IdRepeticao = Globals.repeats[i].IdRepeticao;
                            repeats[i].IdConta = expense.IdConta;
                            repeats[i].IdCategoria = expense.IdCategoria;
                            repeats[i].ValorRepeticao = expense.ValorDespesa;
                            repeats[i].DescricaoRepeticao = expense.DescricaoDespesa;
                            this.periodSelection(i, expense, repeats);
                            repeats[i].ObservacoesRepeticao = expense.ObservacoesDespesa;
                            if (ckb_expensePaid.Checked) repeats[i].PagamentoConfirmado = true; 
                            else repeats[i].PagamentoConfirmado = false;

                            if (Database.updateRepeat(repeats[i])) continue;
                            else
                            {
                                success = 0;
                                break;
                            }
                        }
                    }
                    if (success == 0) MessageBox.Show("[ERRO] Não foi possível atualizar todas as repetições!", "", MessageBoxButtons.OK, MessageBoxIcon.Error); else MessageBox.Show("Despesa atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txt_expenseValue.PlaceholderText = "";
                    this.txt_expenseValue.Focus();
                    return;
                }
                else
                {
                    MessageBox.Show("Despesa atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txt_expenseValue.PlaceholderText = "";
                    this.txt_expenseValue.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Despesa atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txt_expenseValue.PlaceholderText = "";
                this.txt_expenseValue.Focus();
                return;
            }
        }

        //Função que atualiza as informações das parcelas
        private void changeParcel(DataTable dataTable, Expense expense)
        {
            List<Parcel> parcels = new List<Parcel>();
            int success = 1;
            int qtde = 0;
            if (dataTable.Rows[0].ItemArray[2] != System.DBNull.Value) qtde = Convert.ToInt32(dataTable.Rows[0].ItemArray[2]);
            if (Convert.ToBoolean(dataTable.Rows[0].ItemArray[0]))
            {
                //Já existiam parcelas ou repetições antes
                if (!Convert.ToBoolean(dataTable.Rows[0].ItemArray[1]))
                {
                    //Não alterou a forma de divisão do pagamento
                    if (qtde != expense.ParcelasDespesa)
                    {
                        if (qtde > expense.ParcelasDespesa)
                        {
                            //Diminuiu parcelas
                            for (int i = 0; i < (expense.ParcelasDespesa - 1); i++)
                            {
                                parcels.Add(new Parcel());
                                parcels[i].IdParcela = Globals.parcels[i].IdParcela;
                                parcels[i].IdConta = expense.IdConta;
                                parcels[i].IdCategoria = expense.IdCategoria;
                                parcels[i].ValorParcela = expense.ValorDespesa;
                                parcels[i].DescricaoParcela = expense.DescricaoDespesa;
                                this.periodSelection(i, expense, parcels);
                                parcels[i].ObservacoesParcela = expense.ObservacoesDespesa;
                                if (ckb_expensePaid.Checked) parcels[i].PagamentoConfirmado = true; 
                                else parcels[i].PagamentoConfirmado = false;

                                if (Database.updateParcel(parcels[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                            for (int i = (expense.ParcelasDespesa - 1); i < (qtde - 1); i++)
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
                        }
                        else if (qtde < expense.ParcelasDespesa)
                        {
                            //Aumentou parcelas
                            for (int i = 0; i < (qtde - 1); i++)
                            {
                                parcels.Add(new Parcel());
                                parcels[i].IdParcela = Globals.parcels[i].IdParcela;
                                parcels[i].IdConta = expense.IdConta;
                                parcels[i].IdCategoria = expense.IdCategoria;
                                parcels[i].ValorParcela = expense.ValorDespesa;
                                parcels[i].DescricaoParcela = expense.DescricaoDespesa;
                                this.periodSelection(i, expense, parcels);
                                parcels[i].ObservacoesParcela = expense.ObservacoesDespesa;
                                if (ckb_expensePaid.Checked) parcels[i].PagamentoConfirmado = true; 
                                else parcels[i].PagamentoConfirmado = false;
                                
                                if (Database.updateParcel(parcels[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                            for (int i = (qtde - 1); i < (expense.ParcelasDespesa - 1); i++)
                            {
                                parcels.Add(new Parcel());
                                parcels[i].IdDespesa = Globals.idDespesa;
                                parcels[i].IdConta = expense.IdConta;
                                parcels[i].IdCategoria = expense.IdCategoria;
                                parcels[i].ValorParcela = expense.ValorDespesa;
                                parcels[i].DescricaoParcela = expense.DescricaoDespesa;
                                this.periodSelection(i, expense, parcels);
                                parcels[i].ObservacoesParcela = expense.ObservacoesDespesa;
                                if (ckb_expensePaid.Checked) parcels[i].PagamentoConfirmado = true; 
                                else parcels[i].PagamentoConfirmado = false;

                                if (Database.newParcel(parcels[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < (expense.ParcelasDespesa - 1); i++)
                        {
                            parcels.Add(new Parcel());
                            parcels[i].IdParcela = Globals.parcels[i].IdParcela;
                            parcels[i].IdConta = expense.IdConta;
                            parcels[i].IdCategoria = expense.IdCategoria;
                            parcels[i].ValorParcela = expense.ValorDespesa;
                            parcels[i].DescricaoParcela = expense.DescricaoDespesa;
                            this.periodSelection(i, expense, parcels);
                            parcels[i].ObservacoesParcela = expense.ObservacoesDespesa;
                            if (ckb_expensePaid.Checked) parcels[i].PagamentoConfirmado = true; 
                            else parcels[i].PagamentoConfirmado = false;

                            if (Database.updateParcel(parcels[i])) continue;
                            else
                            {
                                success = 0;
                                break;
                            }
                        }
                    }
                    if (success == 0) MessageBox.Show("[ERRO] Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error); else MessageBox.Show("Despesa atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txt_expenseValue.PlaceholderText = "";
                    this.txt_expenseValue.Focus();
                    return;
                }
                else
                {
                    MessageBox.Show("Despesa atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txt_expenseValue.PlaceholderText = "";
                    this.txt_expenseValue.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Despesa atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txt_expenseValue.PlaceholderText = "";
                this.txt_expenseValue.Focus();
                return;
            }
        }

        //Função que cadastra despesa
        private void expenseRegister()
        {
            if (String.IsNullOrEmpty(txt_expenseValue.Text.Trim()) || (this.cbb_expenseCategory.SelectedIndex == -1)) MessageBox.Show("Informe o valor e a categoria da despesa!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if ((this.ckb_repeatOrParcelValue.Checked) && ((String.IsNullOrEmpty(this.txt_repeatsOrParcels.Text.Trim())) || (this.cbb_period.SelectedIndex == -1)))
                {
                    if (this.rbtn_fixedValue.Checked) MessageBox.Show("Informe o número de repetições da despesa e o período em que elas se repetem!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else if (this.rbtn_parcelValue.Checked) MessageBox.Show("Informe o número de parcelas da despesa e o período em que elas se repetem!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Account account = new Account();
                    Expense expense = new Expense();
                    account.IdConta = Globals.idConta;
                    expense.IdConta = Convert.ToInt32(Database.query("SELECT idConta FROM conta WHERE nomeConta = '" + this.cbb_expenseAccount.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    expense.IdCategoria = Convert.ToInt32(Database.query("SELECT idCategoria FROM categoria WHERE nomeCategoria = '" + this.cbb_expenseCategory.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                    string valorDespesa = txt_expenseValue.Text.Trim();
                    if (regexValor.IsMatch(valorDespesa))
                    {
                        if (valorDespesa.Contains("R$ ")) expense.ValorDespesa = Convert.ToDecimal(valorDespesa.Substring(3).Trim());
                        else if (valorDespesa.Contains("R$")) expense.ValorDespesa = Convert.ToDecimal(valorDespesa.Substring(2).Trim());
                        else expense.ValorDespesa = Convert.ToDecimal(txt_expenseValue.Text.Trim());
                    }
                    else
                    {
                        MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.txt_expenseValue.Clear();
                        this.txt_expenseValue.PlaceholderText = "";
                        this.txt_expenseValue.Focus();
                        return;
                    }
                    expense.DescricaoDespesa = txt_expenseDescription.Text.Trim();
                    expense.DataTransacao = Convert.ToDateTime(mtb_expenseDate.Text.Trim());
                    expense.ObservacoesDespesa = txt_expenseObservations.Text.Trim();
                    if (ckb_expensePaid.Checked) expense.PagamentoConfirmado = true; else expense.PagamentoConfirmado = false;

                    if (ckb_repeatOrParcelValue.Checked)
                    {
                        expense.RepetirParcelarDespesa = true;

                        if (rbtn_fixedValue.Checked)
                        {
                            expense.ValorFixoDespesa = true;
                            expense.RepeticoesValorFixoDespesa = Convert.ToInt32(txt_repeatsOrParcels.Text.Trim());
                            expense.PeriodoRepetirParcelarDespesa = cbb_period.SelectedItem.ToString().Trim();
                            if (Database.newExpense(expense))
                            {
                                List<Repeat> repeats = new List<Repeat>();
                                int success = 1;
                                for (int i = 0; i < (expense.RepeticoesValorFixoDespesa - 1); i++)
                                {
                                    repeats.Add(new Repeat());
                                    repeats[i].IdDespesa = Convert.ToInt32(Database.query("SELECT idDespesa FROM despesa ORDER BY idDespesa DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                    repeats[i].IdConta = expense.IdConta;
                                    repeats[i].IdCategoria = expense.IdCategoria;
                                    repeats[i].ValorRepeticao = expense.ValorDespesa;
                                    repeats[i].DescricaoRepeticao = expense.DescricaoDespesa;
                                    this.periodSelection(i, expense, repeats);
                                    repeats[i].ObservacoesRepeticao = expense.ObservacoesDespesa;
                                    if (ckb_expensePaid.Checked) repeats[i].PagamentoConfirmado = true; 
                                    else repeats[i].PagamentoConfirmado = false;

                                    if (Database.newRepeat(repeats[i])) continue;
                                    else
                                    {
                                        success = 0;
                                        break;
                                    }
                                }
                                if (success == 0) MessageBox.Show("[ERRO] Não foi possível cadastrar todas as repetições!", "", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                                else MessageBox.Show("Despesa cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.clearFields();
                            }
                            else MessageBox.Show("[ERRO] Não foi possível cadastrar despesa!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else expense.ValorFixoDespesa = false;

                        if (rbtn_parcelValue.Checked)
                        {
                            expense.ParcelarValorDespesa = true;
                            expense.ParcelasDespesa = Convert.ToInt32(txt_repeatsOrParcels.Text.Trim());
                            expense.PeriodoRepetirParcelarDespesa = cbb_period.SelectedItem.ToString().Trim();
                            expense.ValorDespesa = expense.ValorDespesa / expense.ParcelasDespesa;

                            if (Database.newExpense(expense))
                            {
                                List<Parcel> parcels = new List<Parcel>();
                                int success = 1;
                                for (int i = 0; i < (expense.ParcelasDespesa - 1); i++)
                                {
                                    parcels.Add(new Parcel());
                                    parcels[i].IdDespesa = Convert.ToInt32(Database.query("SELECT idDespesa FROM despesa ORDER BY idDespesa DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                    parcels[i].IdConta = expense.IdConta;
                                    parcels[i].IdCategoria = expense.IdCategoria;
                                    parcels[i].ValorParcela = expense.ValorDespesa;
                                    parcels[i].DescricaoParcela = expense.DescricaoDespesa;
                                    this.periodSelection(i, expense, parcels);
                                    parcels[i].ObservacoesParcela = expense.ObservacoesDespesa;
                                    if (ckb_expensePaid.Checked) parcels[i].PagamentoConfirmado = true; 
                                    else parcels[i].PagamentoConfirmado = false;
                                    if (Database.newParcel(parcels[i])) continue;
                                    else
                                    {
                                        success = 0;
                                        break;
                                    }
                                }
                                if (success == 0) MessageBox.Show("[ERRO] Não foi possível cadastrar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                                else MessageBox.Show("Despesa cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.clearFields();
                            }
                            else MessageBox.Show("[ERRO] Não foi possível cadastrar despesa!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        else expense.ParcelarValorDespesa = false;
                    }
                    else
                    {
                        expense.RepetirParcelarDespesa = false;
                        if (Database.newExpense(expense))
                        {
                            MessageBox.Show("Despesa cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.clearFields();
                        }
                        else MessageBox.Show("[ERRO] Não foi possível cadastrar despesa!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //Função que atualiza despesa
        private void expenseUpdate()
        {
            if (String.IsNullOrEmpty(txt_expenseValue.Text.Trim()) || (this.cbb_expenseCategory.SelectedIndex == -1)) MessageBox.Show("Informe o valor e a categoria da despesa!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if ((this.ckb_repeatOrParcelValue.Checked) && ((String.IsNullOrEmpty(this.txt_repeatsOrParcels.Text.Trim())) || (this.cbb_period.SelectedIndex == -1)))
                {
                    if (this.rbtn_fixedValue.Checked) MessageBox.Show("Informe o número de repetições da despesa e o período em que elas se repetem!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else if (this.rbtn_parcelValue.Checked) MessageBox.Show("Informe o número de parcelas da despesa e o período em que elas se repetem!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Account account = new Account();
                    Expense expense = new Expense();
                    account.IdConta = Globals.idConta;
                    expense.IdDespesa = Globals.idDespesa;
                    expense.IdConta = Convert.ToInt32(Database.query("SELECT idConta FROM conta WHERE nomeConta = '" + this.cbb_expenseAccount.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    expense.IdCategoria = Convert.ToInt32(Database.query("SELECT idCategoria FROM categoria WHERE nomeCategoria = '" + this.cbb_expenseCategory.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                    string valorDespesa = txt_expenseValue.Text.Trim();
                    if (regexValor.IsMatch(valorDespesa))
                    {
                        if (valorDespesa.Contains("R$ ")) expense.ValorDespesa = Convert.ToDecimal(valorDespesa.Substring(3).Trim());
                        else if (valorDespesa.Contains("R$")) expense.ValorDespesa = Convert.ToDecimal(valorDespesa.Substring(2).Trim());
                        else expense.ValorDespesa = Convert.ToDecimal(txt_expenseValue.Text.Trim());
                    }
                    else
                    {
                        MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    expense.DescricaoDespesa = txt_expenseDescription.Text.Trim();
                    expense.DataTransacao = Convert.ToDateTime(mtb_expenseDate.Text.Trim());
                    expense.ObservacoesDespesa = txt_expenseObservations.Text.Trim();
                    if (ckb_expensePaid.Checked) expense.PagamentoConfirmado = true; else expense.PagamentoConfirmado = false;

                    if (ckb_repeatOrParcelValue.Checked)
                    {
                        expense.RepetirParcelarDespesa = true;

                        if (rbtn_fixedValue.Checked)
                        {
                            expense.ValorFixoDespesa = true;
                            expense.RepeticoesValorFixoDespesa = Convert.ToInt32(txt_repeatsOrParcels.Text.Trim());
                            expense.PeriodoRepetirParcelarDespesa = cbb_period.SelectedItem.ToString().Trim();
                            if (Database.deleteAllParcels(expense)) this.updateParcelOrRepeat(expense);
                        }
                        else expense.ValorFixoDespesa = false;

                        if (rbtn_parcelValue.Checked)
                        {
                            expense.ParcelarValorDespesa = true;
                            expense.ParcelasDespesa = Convert.ToInt32(txt_repeatsOrParcels.Text.Trim());
                            expense.PeriodoRepetirParcelarDespesa = cbb_period.SelectedItem.ToString().Trim();
                            expense.ValorDespesa = expense.ValorDespesa / expense.ParcelasDespesa;
                            if (Database.deleteAllRepeats(expense)) this.updateParcelOrRepeat(expense);
                        }
                        else expense.ParcelarValorDespesa = false;
                    }
                    else if (Database.deleteAllParcels(expense)) if (Database.deleteAllRepeats(expense)) this.updateOneParcelOrRepeat(expense);
                }
            }
        }

        //Função que exclui despesa
        private void expenseDelete()
        {
            if (((DialogResult)MessageBox.Show("Tem certeza que deseja excluir a despesa?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)).ToString().ToUpper() == "NO") return;
            Account account = new Account();
            Expense expense = new Expense();
            account.IdConta = Globals.idConta;
            expense.IdDespesa = Globals.idDespesa;
            foreach (DataRow dataRow in Globals.expenseDataTable.Rows)
            {
                if (Convert.ToBoolean(dataRow.ItemArray[9]))
                {
                    if (Convert.ToBoolean(dataRow.ItemArray[10]))
                    {
                        int success = 1;
                        for (int i = 0; i < (Globals.repeats.Count); i++)
                        {
                            if (Database.deleteRepeat(Globals.repeats[i])) continue;
                            else
                            {
                                success = 0;
                                break;
                            }
                        }
                        if (success == 0) MessageBox.Show("[ERRO] Não foi possível excluir todas as repetições!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (Database.deleteExpense(expense)) MessageBox.Show("Despesa excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else MessageBox.Show("[ERRO] Não foi possível excluir despesa!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (Application.OpenForms.OfType<Main>().Count() == 0)
                        {
                            Main main = new Main();
                            main.Show();
                            this.Close();
                        }
                    }
                    else if (Convert.ToBoolean(dataRow.ItemArray[12]))
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
                        if (success == 0) MessageBox.Show("[ERRO] Não foi possível excluir todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (Database.deleteExpense(expense)) MessageBox.Show("Despesa excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else MessageBox.Show("[ERRO] Não foi possível excluir despesa!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (Application.OpenForms.OfType<Main>().Count() == 0)
                        {
                            Main main = new Main();
                            main.Show();
                            this.Close();
                        }
                    }
                }
                else
                {
                    if (Database.deleteExpense(expense)) MessageBox.Show("Despesa excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else MessageBox.Show("[ERRO] Não foi possível excluir despesa!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (Application.OpenForms.OfType<Main>().Count() == 0)
                    {
                        Main main = new Main();
                        main.Show();
                        this.Close();
                    }
                }
            }
        }

        //Função que muda a cor do campo data da despesa
        private void setRGB(int r, int g, int b)
        {
            this.mtb_expenseDate.BackColor = Color.FromArgb(r, g, b);
            this.border_mtbExpenseDate.FillColor = Color.FromArgb(r, g, b);
            this.border_mtbExpenseDate.OnIdleState.FillColor = Color.FromArgb(r, g, b);
        }

        //CADASTRAR DESPESA
        private void pcb_expenseRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_expenseRegister.BackgroundImage = Properties.Resources.btn_add_expense_active;
        }

        private void pcb_expenseRegister_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_expenseRegisterTag.ClientRectangle.Contains(lbl_expenseRegisterTag.PointToClient(Cursor.Position))) this.pcb_expenseRegister.BackgroundImage = Properties.Resources.btn_add_expense;
        }

        private void pcb_expenseRegister_Click(object sender, EventArgs e)
        {
            this.expenseRegister();
        }

        private void lbl_expenseRegisterTag_Click(object sender, EventArgs e)
        {
            this.expenseRegister();
        }

        //ATUALIZAR DESPESA
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
            this.expenseUpdate();
        }

        private void lbl_btnUpdateTag_Click(object sender, EventArgs e)
        {
            this.expenseUpdate();
        }

        //EXCLUIR DESPESA
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
            this.expenseDelete();
        }
        private void lbl_btnDeleteTag_Click(object sender, EventArgs e)
        {
            this.expenseDelete();
        }

        //CANCELAR CADASTRO OU EDIÇÃO
        private void pcb_btnCancel_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnCancel.BackgroundImage = Properties.Resources.btn_cancel_active;
        }

        private void pcb_btnCancel_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnCancelTag.ClientRectangle.Contains(lbl_btnCancelTag.PointToClient(Cursor.Position))) this.pcb_btnCancel.BackgroundImage = Properties.Resources.btn_cancel;
        }

        private void pcb_btnCancel_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Main>().Count() == 0)
            {
                Main main = new Main();
                main.Show();
                this.Close();
            }
        }

        private void lbl_btnCancelTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Main>().Count() == 0)
            {
                Main main = new Main();
                main.Show();
                this.Close();
            }
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO

        //KeyPress

        private void txt_expenseValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ',') && (e.KeyChar != 'R') && (e.KeyChar != '$') && (e.KeyChar != ' ')) { e.Handled = true; }
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1)) e.Handled = true;
        }

        private void mtb_expenseDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void txt_repeatsOrParcels_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        //MouseEnter

        private void mtb_expenseDate_MouseEnter(object sender, EventArgs e)
        {
            this.setRGB(0, 104, 232);
        }

        //MouseLeave

        private void mtb_expenseDate_MouseLeave(object sender, EventArgs e)
        {
            if (!this.mtb_expenseDate.Focused) this.setRGB(0, 76, 157);
        }

        //Enter

        private void border_mtbExpenseDate_Enter(object sender, EventArgs e)
        {
            this.mtb_expenseDate.Focus();
            this.setRGB(0, 104, 232);
        }

        private void mtb_expenseDate_Enter(object sender, EventArgs e)
        {
            this.setRGB(0, 104, 232);
        }

        //Leave

        private void txt_expenseValue_Leave(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(this.txt_expenseValue.Text)) && (!this.txt_expenseValue.Focused)) this.txt_expenseValue.PlaceholderText = "R$ 0,00";
        }

        private void mtb_expenseDate_Leave(object sender, EventArgs e)
        {
            this.setRGB(0, 76, 157);
        }

        //OnChange

        private void ckb_repeatOrParcelValue_OnChange(object sender, EventArgs e)
        {
            if (ckb_repeatOrParcelValue.Checked)
            {
                this.rbtn_fixedValue.Show();
                this.rbtn_fixedValue.Checked = true;
                this.rbtn_fixedValue.Location = new Point(24, 335);
                this.lbl_fixedValue.Show();
                this.rbtn_parcelValue.Show();
                this.rbtn_parcelValue.Location = new Point(24, 370);
                this.lbl_parcelValue.Show();
                this.txt_repeatsOrParcels.Show();
                this.txt_repeatsOrParcels.Location = new Point(16, 405);
                this.cbb_period.Show();
                this.cbb_period.Location = new Point(16, 450);
            }
            else this.doNotRepeatOrParcel();
        }

        //CheckedChanged

        private void rbtn_fixedValue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_fixedValue.Checked) this.txt_repeatsOrParcels.PlaceholderText = "Repetições";
        }

        private void rbtn_parcelValue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_parcelValue.Checked) this.txt_repeatsOrParcels.PlaceholderText = "Parcelas";
        }
    }
}