using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class AddExpense : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        private static DataTable dataTableExpense;
        private static int idDespesa;
        private List<Repeat> repeats = new List<Repeat>();
        private List<Parcel> parcels = new List<Parcel>();

        //INICIA INSTÂNCIA DO PAINEL CONJUNTO À UM DATABASE DE DESPESAS PARA ATUALIZAÇÃO E EXCLUSÃO, QUE PODE SER NULO EM CASO DE CADASTRO
        public AddExpense(DataTable dataTable)
        {
            InitializeComponent();
            dataTableExpense = dataTable;
            this.panel_expense.VerticalScroll.Enabled = false;
            this.panel_expense.VerticalScroll.Visible = false;
            this.txt_expenseValue.Focus();
            if (dataTableExpense != null)
            {
                foreach (DataRow dataRow in dataTableExpense.Rows)
                {
                    idDespesa = Convert.ToInt32(dataRow.ItemArray[0]);
                    this.txt_expenseValue.Text = String.Format("{0:C}", dataRow.ItemArray[3]);
                    this.txt_expenseDescription.Text = dataRow.ItemArray[4].ToString();
                    this.mtb_expenseDate.Text = (Convert.ToDateTime(dataRow.ItemArray[5]).ToShortDateString()).ToString();
                    this.cbb_expenseCategory.SelectedIndex = this.cbb_expenseCategory.FindString(dataRow.ItemArray[6].ToString());
                    this.txt_expenseObservations.Text = dataRow.ItemArray[7].ToString();

                    if (Convert.ToBoolean(dataRow.ItemArray[8]) == true) this.ckb_expensePaid.Checked = true; else this.ckb_expensePaid.Checked = false;

                    if (Convert.ToBoolean(dataRow.ItemArray[9]) == true)
                    {
                        this.ckb_repeatOrParcelValue.Checked = true;
                        this.cbb_period.SelectedIndex = this.cbb_period.FindString(dataRow.ItemArray[14].ToString());
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

                    if (dataRow.ItemArray[10] != null)
                    {
                        if (Convert.ToBoolean(dataRow.ItemArray[10]))
                        {
                            this.rbtn_fixedValue.Show();
                            this.lbl_fixedValue.Show();
                            this.rbtn_fixedValue.Checked = true;
                            this.rbtn_parcelValue.Checked = false;
                            this.txt_repeatsOrParcels.Show();
                            this.txt_repeatsOrParcels.Text = dataRow.ItemArray[11].ToString();
                            int i = 0;
                            foreach (DataRow dataRowRepeats in Database.query("SELECT idRepeticao FROM repeticao WHERE idDespesa = " + idDespesa.ToString()).Rows)
                            {
                                repeats.Add(new Repeat());
                                repeats[i].idRepeticao = Convert.ToInt32(dataRowRepeats.ItemArray[0]);
                                i++;
                            }
                        }
                        else this.rbtn_fixedValue.Checked = false;
                    }
                    else this.rbtn_fixedValue.Checked = false;

                    if (dataRow.ItemArray[12].ToString() != null)
                    {
                        if (Convert.ToBoolean(dataRow.ItemArray[12]) == true)
                        {
                            this.rbtn_parcelValue.Show();
                            this.lbl_parcelValue.Show();
                            this.rbtn_fixedValue.Checked = false;
                            this.rbtn_parcelValue.Checked = true;
                            this.txt_repeatsOrParcels.Show();
                            this.txt_repeatsOrParcels.Text = dataRow.ItemArray[13].ToString();
                            this.txt_expenseValue.Text = String.Format("{0:C}", (Convert.ToDecimal(dataRow.ItemArray[3]) * Convert.ToInt32(dataRow.ItemArray[13])));
                            int i = 0;
                            foreach (DataRow dataRowParcels in Database.query("SELECT * FROM parcela WHERE idDespesa = " + idDespesa.ToString()).Rows)
                            {
                                parcels.Add(new Parcel());
                                parcels[i].idParcela = Convert.ToInt32(dataRowParcels.ItemArray[0]);
                                i++;
                            }
                        }
                        else this.rbtn_parcelValue.Checked = false;
                    }
                    else this.rbtn_parcelValue.Checked = false;
                }
            }
            else
            {
                this.clearFields();
            }
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

        //FUNÇÃO QUE RETORNA OS COMPONENTES DO FORMULÁRIO AO ESTADO INICIAL CASO NÃO HAJA REPETIÇÕES OU PARCELAS
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
            this.cbb_period.Text = "Período";
            this.cbb_period.Hide();
            this.txt_expenseValue.PlaceholderText = "";
            this.txt_expenseValue.Focus();
        }

        //FUNÇÃO QUE RETORNA OS CAMPOS DO FORMULÁRIO AO ESTADO INICIAL
        private void clearFields()
        {
            this.txt_expenseValue.Clear();
            this.txt_expenseDescription.Clear();
            this.mtb_expenseDate.Text = (DateTime.Today.ToShortDateString()).ToString();
            this.cbb_expenseCategory.SelectedIndex = -1;
            this.cbb_expenseCategory.Text = "Categoria";
            this.txt_expenseObservations.Clear();
            this.doNotRepeatOrParcel();
        }

        //FUNÇÃO QUE SELECIONA A DATA DA TRANSAÇÃO DA REPETIÇÃO DE ACORDO COM O PERÍDO ESCOLHIDO PELO USUÁRIO
        private void periodSelection(int i, Expense expense, List<Repeat> repeats)
        {
            if (i == 0)
            {
                switch (expense.periodoRepetirParcelarDespesa)
                {
                    case "Diário":
                        repeats[i].dataTransacao = expense.dataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        repeats[i].dataTransacao = expense.dataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        repeats[i].dataTransacao = expense.dataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        repeats[i].dataTransacao = expense.dataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        repeats[i].dataTransacao = expense.dataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        repeats[i].dataTransacao = expense.dataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        repeats[i].dataTransacao = expense.dataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        repeats[i].dataTransacao = expense.dataTransacao.AddYears(1);
                        break;
                }
            }
            else
            {
                switch (expense.periodoRepetirParcelarDespesa)
                {
                    case "Diário":
                        repeats[i].dataTransacao = repeats[i - 1].dataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        repeats[i].dataTransacao = repeats[i - 1].dataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        repeats[i].dataTransacao = repeats[i - 1].dataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        repeats[i].dataTransacao = repeats[i - 1].dataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        repeats[i].dataTransacao = repeats[i - 1].dataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        repeats[i].dataTransacao = repeats[i - 1].dataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        repeats[i].dataTransacao = repeats[i - 1].dataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        repeats[i].dataTransacao = repeats[i - 1].dataTransacao.AddYears(1);
                        break;
                }
            }
        }

        //FUNÇÃO QUE SELECIONA A DATA DA TRANSAÇÃO DA PARCELA DE ACORDO COM O PERÍDO ESCOLHIDO PELO USUÁRIO
        private void periodSelection(int i, Expense expense, List<Parcel> parcels)
        {
            if (i == 0)
            {
                switch (expense.periodoRepetirParcelarDespesa)
                {
                    case "Diário":
                        parcels[i].dataTransacao = expense.dataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        parcels[i].dataTransacao = expense.dataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        parcels[i].dataTransacao = expense.dataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        parcels[i].dataTransacao = expense.dataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        parcels[i].dataTransacao = expense.dataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        parcels[i].dataTransacao = expense.dataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        parcels[i].dataTransacao = expense.dataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        parcels[i].dataTransacao = expense.dataTransacao.AddYears(1);
                        break;
                }
            }
            else
            {
                switch (expense.periodoRepetirParcelarDespesa)
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
        private void updateOneParcelOrRepeat(Expense expense)
        {
            if (Database.updateExpenseNotParceledOrRepeated(expense))
            {
                MessageBox.Show("Despesa atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.doNotRepeatOrParcel();
            }
            else MessageBox.Show("Não foi possível atualizar despesa!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //FUNÇÃO QUE ATUALIZA PARCELA OU REPETIÇÃO
        private void updateParcelOrRepeat(Expense expense)
        {
            DataTable parcelOrRepeatDataTable = Database.query("SELECT repetirParcelarDespesa, valorFixoDespesa, parcelasDespesa, repeticoesValorFixoDespesa FROM despesa WHERE idDespesa = " + idDespesa);
            if (Database.updateExpense(expense))
            {
                int qtde = Convert.ToInt32(Database.query("SELECT repeticoesValorFixoDespesa FROM despesa WHERE idDespesa = " + idDespesa).Rows[0].ItemArray[0]);
                if (qtde == 0)
                {
                    //Valor parcelado
                    qtde = Convert.ToInt32(Database.query("SELECT parcelasDespesa FROM despesa WHERE idDespesa = " + idDespesa).Rows[0].ItemArray[0]);
                    this.parcels.Clear();
                    if (qtde == 1) this.updateOneParcelOrRepeat(expense);
                    DataTable dataTable = Database.query("SELECT idParcela FROM parcela WHERE idDespesa = " + idDespesa);
                    if (dataTable.Rows.Count == 0)
                    {
                        for (int j = 0; j < (expense.parcelasDespesa - 1); j++)
                        {
                            parcels.Add(new Parcel());
                            parcels[j].idDespesa = idDespesa;
                            parcels[j].valorParcela = expense.valorDespesa;
                            parcels[j].descricaoParcela = expense.descricaoDespesa;
                            this.periodSelection(j, expense, this.parcels);
                            parcels[j].categoriaParcela = expense.categoriaDespesa;
                            parcels[j].observacoesParcela = expense.observacoesDespesa;
                            parcels[j].pagamentoConfirmado = false;
                            if (Database.newParcel(parcels[j])) continue;
                            else
                            {
                                MessageBox.Show("Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                    }
                    dataTable = Database.query("SELECT idParcela FROM parcela WHERE idDespesa = " + idDespesa);
                    //Parcelas
                    int i = 0;
                    foreach (DataRow dataRowParcels in dataTable.Rows)
                    {
                        if (this.parcels.Count == 0 && i == 0) parcels.Add(new Parcel());
                        parcels[i].idParcela = Convert.ToInt32(dataRowParcels.ItemArray[0]);
                        i++;
                    }
                    this.changeParcel(parcelOrRepeatDataTable, expense);
                }
                else
                {
                    //Valor repetido
                    this.repeats.Clear();
                    if (qtde == 1) this.updateOneParcelOrRepeat(expense);
                    DataTable dataTable = Database.query("SELECT idRepeticao FROM repeticao WHERE idDespesa = " + idDespesa);
                    if (dataTable.Rows.Count == 0)
                    {
                        for (int j = 0; j < (expense.repeticoesValorFixoDespesa - 1); j++)
                        {
                            this.repeats.Add(new Repeat());
                            this.repeats[j].idDespesa = idDespesa;
                            this.repeats[j].valorRepeticao = expense.valorDespesa;
                            this.repeats[j].descricaoRepeticao = expense.descricaoDespesa;
                            this.periodSelection(j, expense, this.repeats);
                            this.repeats[j].categoriaRepeticao = expense.categoriaDespesa;
                            this.repeats[j].observacoesRepeticao = expense.observacoesDespesa;
                            this.repeats[j].pagamentoConfirmado = false;
                            if (Database.newRepeat(this.repeats[j])) continue;
                            else
                            {
                                MessageBox.Show("Não foi possível atualizar todas as repetições!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                    }
                    dataTable = Database.query("SELECT idRepeticao FROM repeticao WHERE idDespesa = " + idDespesa);
                    //Repetições
                    if (this.repeats.Count == 0)
                    {
                        int i = 0;
                        foreach (DataRow dataRowRepeats in dataTable.Rows)
                        {
                            repeats.Add(new Repeat());
                            repeats[i].idRepeticao = Convert.ToInt32(dataRowRepeats.ItemArray[0]);
                            i++;
                        }
                        this.changeRepeat(parcelOrRepeatDataTable, expense);
                    }
                    else
                    {
                        int i = 0;
                        foreach (DataRow dataRowRepeats in dataTable.Rows)
                        {
                            repeats[i].idRepeticao = Convert.ToInt32(dataRowRepeats.ItemArray[0]);
                            i++;
                        }
                        this.changeRepeat(parcelOrRepeatDataTable, expense);
                    }
                }
            }
            else MessageBox.Show("Não foi possível atualizar despesa!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //FUNÇÃO QUE ATUALIZA AS INFORMAÇÕES DAS REPETIÇÕES
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
                    if (qtde != expense.repeticoesValorFixoDespesa)
                    {
                        if (qtde > expense.repeticoesValorFixoDespesa)
                        {
                            //Diminiu repetições
                            for (int i = 0; i < (expense.repeticoesValorFixoDespesa - 1); i++)
                            {
                                repeats.Add(new Repeat());
                                repeats[i].idRepeticao = this.repeats[i].idRepeticao;
                                repeats[i].valorRepeticao = expense.valorDespesa;
                                repeats[i].descricaoRepeticao = expense.descricaoDespesa;
                                this.periodSelection(i, expense, repeats);
                                repeats[i].categoriaRepeticao = expense.categoriaDespesa;
                                repeats[i].observacoesRepeticao = expense.observacoesDespesa;
                                repeats[i].pagamentoConfirmado = false;
                                if (Database.updateRepeat(repeats[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                            for (int i = (expense.repeticoesValorFixoDespesa - 1); i < (qtde - 1); i++)
                            {
                                repeats.Add(new Repeat());
                                repeats[i].idRepeticao = this.repeats[i].idRepeticao;
                                if (Database.deleteRepeat(repeats[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                        }
                        else if (qtde < expense.repeticoesValorFixoDespesa)
                        {
                            //Aumentou repetições
                            for (int i = 0; i < (qtde - 1); i++)
                            {
                                repeats.Add(new Repeat());
                                repeats[i].idRepeticao = this.repeats[i].idRepeticao;
                                repeats[i].valorRepeticao = expense.valorDespesa;
                                repeats[i].descricaoRepeticao = expense.descricaoDespesa;
                                this.periodSelection(i, expense, repeats);
                                repeats[i].categoriaRepeticao = expense.categoriaDespesa;
                                repeats[i].observacoesRepeticao = expense.observacoesDespesa;
                                repeats[i].pagamentoConfirmado = false;
                                if (Database.updateRepeat(repeats[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                            for (int i = (qtde - 1); i < (expense.repeticoesValorFixoDespesa - 1); i++)
                            {
                                repeats.Add(new Repeat());
                                repeats[i].idDespesa = idDespesa;
                                repeats[i].valorRepeticao = expense.valorDespesa;
                                repeats[i].descricaoRepeticao = expense.descricaoDespesa;
                                this.periodSelection(i, expense, repeats);
                                repeats[i].categoriaRepeticao = expense.categoriaDespesa;
                                repeats[i].observacoesRepeticao = expense.observacoesDespesa;
                                repeats[i].recebimentoConfirmado = false;
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
                        for (int i = 0; i < (expense.repeticoesValorFixoDespesa - 1); i++)
                        {
                            repeats.Add(new Repeat());
                            repeats[i].idRepeticao = this.repeats[i].idRepeticao;
                            repeats[i].valorRepeticao = expense.valorDespesa;
                            repeats[i].descricaoRepeticao = expense.descricaoDespesa;
                            this.periodSelection(i, expense, repeats);
                            repeats[i].categoriaRepeticao = expense.categoriaDespesa;
                            repeats[i].observacoesRepeticao = expense.observacoesDespesa;
                            repeats[i].recebimentoConfirmado = false;
                            if (Database.updateRepeat(repeats[i])) continue;
                            else
                            {
                                success = 0;
                                break;
                            }
                        }
                    }
                    if (success == 0) MessageBox.Show("Não foi possível atualizar todas as repetições!", "", MessageBoxButtons.OK, MessageBoxIcon.Error); else MessageBox.Show("Despesa atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        //FUNÇÃO QUE ATUALIZA AS INFORMAÇÕES DAS PARCELAS
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
                    if (qtde != expense.parcelasDespesa)
                    {
                        if (qtde > expense.parcelasDespesa)
                        {
                            //Diminuiu parcelas
                            for (int i = 0; i < (expense.parcelasDespesa - 1); i++)
                            {
                                parcels.Add(new Parcel());
                                parcels[i].idParcela = this.parcels[i].idParcela;
                                parcels[i].valorParcela = expense.valorDespesa;
                                parcels[i].descricaoParcela = expense.descricaoDespesa;
                                this.periodSelection(i, expense, parcels);
                                parcels[i].categoriaParcela = expense.categoriaDespesa;
                                parcels[i].observacoesParcela = expense.observacoesDespesa;
                                parcels[i].pagamentoConfirmado = false;
                                if (Database.updateParcel(parcels[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                            for (int i = (expense.parcelasDespesa - 1); i < (qtde - 1); i++)
                            {
                                parcels.Add(new Parcel());
                                parcels[i].idParcela = this.parcels[i].idParcela;
                                if (Database.deleteParcel(parcels[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                        }
                        else if (qtde < expense.parcelasDespesa)
                        {
                            //Aumentou parcelas
                            for (int i = 0; i < (qtde - 1); i++)
                            {
                                parcels.Add(new Parcel());
                                parcels[i].idParcela = this.parcels[i].idParcela;
                                parcels[i].valorParcela = expense.valorDespesa;
                                parcels[i].descricaoParcela = expense.descricaoDespesa;
                                this.periodSelection(i, expense, parcels);
                                parcels[i].categoriaParcela = expense.categoriaDespesa;
                                parcels[i].observacoesParcela = expense.observacoesDespesa;
                                parcels[i].pagamentoConfirmado = false;
                                if (Database.updateParcel(parcels[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                            for (int i = (qtde - 1); i < (expense.parcelasDespesa - 1); i++)
                            {
                                parcels.Add(new Parcel());
                                parcels[i].idReceita = idDespesa;
                                parcels[i].valorParcela = expense.valorDespesa;
                                parcels[i].descricaoParcela = expense.descricaoDespesa;
                                this.periodSelection(i, expense, parcels);
                                parcels[i].categoriaParcela = expense.categoriaDespesa;
                                parcels[i].observacoesParcela = expense.observacoesDespesa;
                                parcels[i].pagamentoConfirmado = false;
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
                        for (int i = 0; i < (expense.parcelasDespesa - 1); i++)
                        {
                            parcels.Add(new Parcel());
                            parcels[i].idParcela = this.parcels[i].idParcela;
                            parcels[i].valorParcela = expense.valorDespesa;
                            parcels[i].descricaoParcela = expense.descricaoDespesa;
                            this.periodSelection(i, expense, parcels);
                            parcels[i].categoriaParcela = expense.categoriaDespesa;
                            parcels[i].observacoesParcela = expense.observacoesDespesa;
                            parcels[i].recebimentoConfirmado = false;
                            if (Database.updateParcel(parcels[i])) continue;
                            else
                            {
                                success = 0;
                                break;
                            }
                        }
                    }
                    if (success == 0) MessageBox.Show("Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error); else MessageBox.Show("Despesa atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        //CADASTRAR DESPESA
        private void pcb_expenseRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_expenseRegister.Image = Properties.Resources.btn_expenseRegister_active;
        }

        private void pcb_expenseRegister_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_expenseRegister.Image = Properties.Resources.btn_expenseRegister;
        }

        private void pcb_expenseRegister_Click(object sender, EventArgs e)
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
                    Expense expense = new Expense();
                    expense.idConta = 1;
                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                    string valorDespesa = txt_expenseValue.Text;
                    if (regexValor.IsMatch(valorDespesa))
                    {
                        if (valorDespesa.Contains("R$ ")) expense.valorDespesa = Convert.ToDecimal(valorDespesa.Substring(3));
                        else if (valorDespesa.Contains("R$")) expense.valorDespesa = Convert.ToDecimal(valorDespesa.Substring(2));
                        else expense.valorDespesa = Convert.ToDecimal(txt_expenseValue.Text);
                    }
                    else
                    {
                        MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.txt_expenseValue.Clear();
                        this.txt_expenseValue.PlaceholderText = "";
                        this.txt_expenseValue.Focus();
                        return;
                    }
                    expense.descricaoDespesa = txt_expenseDescription.Text;
                    expense.dataTransacao = Convert.ToDateTime(mtb_expenseDate.Text);
                    expense.categoriaDespesa = cbb_expenseCategory.SelectedItem.ToString();
                    expense.observacoesDespesa = txt_expenseObservations.Text;
                    if (ckb_expensePaid.Checked) expense.pagamentoConfirmado = true; else expense.pagamentoConfirmado = false;

                    if (ckb_repeatOrParcelValue.Checked)
                    {
                        expense.repetirParcelarDespesa = true;

                        if (rbtn_fixedValue.Checked)
                        {
                            expense.valorFixoDespesa = true;
                            expense.repeticoesValorFixoDespesa = Convert.ToInt32(txt_repeatsOrParcels.Text);
                            expense.periodoRepetirParcelarDespesa = cbb_period.SelectedItem.ToString();
                            if (Database.newExpense(expense))
                            {
                                List<Repeat> repeats = new List<Repeat>();
                                int success = 1;
                                for (int i = 0; i < (expense.repeticoesValorFixoDespesa - 1); i++)
                                {
                                    repeats.Add(new Repeat());
                                    repeats[i].idReceita = Convert.ToInt32(Database.query("SELECT idDespesa FROM despesa ORDER BY idDespesa DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                    repeats[i].valorRepeticao = expense.valorDespesa;
                                    repeats[i].descricaoRepeticao = expense.descricaoDespesa;
                                    this.periodSelection(i, expense, repeats);
                                    repeats[i].categoriaRepeticao = expense.categoriaDespesa;
                                    repeats[i].observacoesRepeticao = expense.observacoesDespesa;
                                    repeats[i].pagamentoConfirmado = false;
                                    if (Database.newRepeat(repeats[i])) continue;
                                    else
                                    {
                                        success = 0;
                                        break;
                                    }
                                }
                                if (success == 0) MessageBox.Show("Não foi possível cadastrar todas as repetições!", "", MessageBoxButtons.OK, MessageBoxIcon.Error); else MessageBox.Show("Despesa cadastrada com suceso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.clearFields();
                            }
                            else MessageBox.Show("Não foi possível cadastrar despesa!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else expense.valorFixoDespesa = false;

                        if (rbtn_parcelValue.Checked)
                        {
                            expense.parcelarValorDespesa = true;
                            expense.parcelasDespesa = Convert.ToInt32(txt_repeatsOrParcels.Text);
                            expense.periodoRepetirParcelarDespesa = cbb_period.SelectedItem.ToString();
                            expense.valorDespesa = expense.valorDespesa / expense.parcelasDespesa;

                            if (Database.newExpense(expense))
                            {
                                List<Parcel> parcels = new List<Parcel>();
                                int success = 1;
                                for (int i = 0; i < (expense.parcelasDespesa - 1); i++)
                                {
                                    parcels.Add(new Parcel());
                                    parcels[i].idReceita = Convert.ToInt32(Database.query("SELECT idDespesa FROM despesa ORDER BY idDespesa DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                    parcels[i].valorParcela = expense.valorDespesa;
                                    parcels[i].descricaoParcela = expense.descricaoDespesa;
                                    this.periodSelection(i, expense, parcels);
                                    parcels[i].categoriaParcela = expense.categoriaDespesa;
                                    parcels[i].observacoesParcela = expense.observacoesDespesa;
                                    parcels[i].pagamentoConfirmado = false;
                                    if (Database.newParcel(parcels[i])) continue;
                                    else
                                    {
                                        success = 0;
                                        break;
                                    }
                                }
                                if (success == 0) MessageBox.Show("Não foi possível cadastrar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error); else MessageBox.Show("Despesa cadastrada com suceso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.clearFields();
                            }
                            else MessageBox.Show("Não foi possível cadastrar despesa!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        else expense.parcelarValorDespesa = false;
                    }
                    else
                    {
                        expense.repetirParcelarDespesa = false;
                        if (Database.newExpense(expense))
                        {
                            MessageBox.Show("Despesa cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.clearFields();
                        }
                        else MessageBox.Show("Não foi possível cadastrar despesa!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //ATUALIZAR DESPESA
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
                    Expense expense = new Expense();
                    expense.idDespesa = idDespesa;
                    expense.idConta = 1;
                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                    string valorDespesa = txt_expenseValue.Text;
                    if (regexValor.IsMatch(valorDespesa))
                    {
                        if (valorDespesa.Contains("R$ ")) expense.valorDespesa = Convert.ToDecimal(valorDespesa.Substring(3));
                        else if (valorDespesa.Contains("R$")) expense.valorDespesa = Convert.ToDecimal(valorDespesa.Substring(2));
                        else expense.valorDespesa = Convert.ToDecimal(txt_expenseValue.Text);
                    }
                    else
                    {
                        MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    expense.descricaoDespesa = txt_expenseDescription.Text;
                    expense.dataTransacao = Convert.ToDateTime(mtb_expenseDate.Text);
                    expense.categoriaDespesa = cbb_expenseCategory.SelectedItem.ToString();
                    expense.observacoesDespesa = txt_expenseObservations.Text;
                    if (ckb_expensePaid.Checked) expense.pagamentoConfirmado = true; else expense.pagamentoConfirmado = false;

                    if (ckb_repeatOrParcelValue.Checked)
                    {
                        expense.repetirParcelarDespesa = true;

                        if (rbtn_fixedValue.Checked)
                        {
                            expense.valorFixoDespesa = true;
                            expense.repeticoesValorFixoDespesa = Convert.ToInt32(txt_repeatsOrParcels.Text);
                            expense.periodoRepetirParcelarDespesa = cbb_period.SelectedItem.ToString();
                            if (Database.deleteAllParcels(expense))
                            {
                                this.updateParcelOrRepeat(expense);
                            }
                        }
                        else expense.valorFixoDespesa = false;

                        if (rbtn_parcelValue.Checked)
                        {
                            expense.parcelarValorDespesa = true;
                            expense.parcelasDespesa = Convert.ToInt32(txt_repeatsOrParcels.Text);
                            expense.periodoRepetirParcelarDespesa = cbb_period.SelectedItem.ToString();
                            expense.valorDespesa = expense.valorDespesa / expense.parcelasDespesa;
                            if (Database.deleteAllRepeats(expense))
                            {
                                this.updateParcelOrRepeat(expense);
                            }
                        }
                        else expense.parcelarValorDespesa = false;
                    }
                    else
                    {
                        if (Database.deleteAllParcels(expense)) if (Database.deleteAllRepeats(expense)) this.updateOneParcelOrRepeat(expense);
                    }
                }
            }
        }

        //EXCLUIR DESPESA
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
            Expense expense = new Expense();
            expense.idDespesa = idDespesa;
            foreach (DataRow dataRow in dataTableExpense.Rows)
            {
                if (Convert.ToBoolean(dataRow.ItemArray[9]))
                {
                    if (Convert.ToBoolean(dataRow.ItemArray[10]))
                    {
                        int success = 1;
                        for (int i = 0; i < (this.repeats.Count); i++)
                        {
                            if (Database.deleteRepeat(this.repeats[i])) continue;
                            else
                            {
                                success = 0;
                                break;
                            }
                        }
                        if (success == 0) MessageBox.Show("Não foi possível atualizar todas as repetições!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (Database.deleteExpense(expense)) MessageBox.Show("Despesa excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information); else MessageBox.Show("Não foi possível excluída receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        for (int i = 0; i < (this.parcels.Count); i++)
                        {
                            if (Database.deleteParcel(this.parcels[i])) continue;
                            else
                            {
                                success = 0;
                                break;
                            }
                        }
                        if (success == 0) MessageBox.Show("Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (Database.deleteExpense(expense)) MessageBox.Show("Despesa excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information); else MessageBox.Show("Não foi possível excluída receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    if (Database.deleteExpense(expense)) MessageBox.Show("Despesa excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information); else MessageBox.Show("Não foi possível excluída receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (Application.OpenForms.OfType<Main>().Count() == 0)
                    {
                        Main main = new Main();
                        main.Show();
                        this.Close();
                    }
                }
            }
            if (Database.deleteExpense(expense)) MessageBox.Show("Despesa excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information); else MessageBox.Show("Não foi possível excluída receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (Application.OpenForms.OfType<Main>().Count() == 0)
            {
                Main main = new Main();
                main.Show();
                this.Close();
            }
        }

        //CANCELAR CADASTRO OU EDIÇÃO
        private void pcb_btnCancel_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnCancel.Image = Properties.Resources.btn_cancel_active;
        }

        private void pcb_btnCancel_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnCancel.Image = Properties.Resources.btn_cancel;
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

        //ENCERRAR APLICAÇÃO
        private void pcb_appClose_Click(object sender, EventArgs e)
        {
            if (((DialogResult)MessageBox.Show("Tem certeza que deseja encerrar a aplicação?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)).ToString().ToUpper() == "YES") Application.Exit();
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO
        private void ckb_repeatOrParcelValue_OnChange(object sender, EventArgs e)
        {
            if (ckb_repeatOrParcelValue.Checked)
            {
                this.rbtn_fixedValue.Show();
                this.rbtn_fixedValue.Checked = true;
                this.rbtn_fixedValue.Location = new Point(24, 315);
                this.lbl_fixedValue.Show();
                this.rbtn_parcelValue.Show();
                this.rbtn_parcelValue.Location = new Point(24, 350);
                this.lbl_parcelValue.Show();
                this.txt_repeatsOrParcels.Show();
                this.txt_repeatsOrParcels.Location = new Point(16, 385);
                this.cbb_period.Show();
                this.cbb_period.Location = new Point(16, 430);
            }
            else
            {
                this.doNotRepeatOrParcel();
            }
        }

        private void rbtn_fixedValue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_fixedValue.Checked) this.txt_repeatsOrParcels.PlaceholderText = "Repetições";
        }

        private void rbtn_parcelValue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_parcelValue.Checked) this.txt_repeatsOrParcels.PlaceholderText = "Parcelas";
        }

        private void txt_expenseValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ',') && (e.KeyChar != 'R') && (e.KeyChar != '$') && (e.KeyChar != ' ')) { e.Handled = true; }
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1)) e.Handled = true;
        }

        private void txt_expenseValue_Leave(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(this.txt_expenseValue.Text)) && (!this.txt_expenseValue.Focused)) this.txt_expenseValue.PlaceholderText = "R$ 0,00";
        }

        private void mtb_expenseDate_MouseEnter(object sender, EventArgs e)
        {
            this.mtb_expenseDate.BackColor = Color.FromArgb(0, 104, 232);
            this.border_mtbExpenseDate.FillColor = Color.FromArgb(0, 104, 232);
            this.border_mtbExpenseDate.OnIdleState.FillColor = Color.FromArgb(0, 104, 232);
        }

        private void mtb_expenseDate_MouseLeave(object sender, EventArgs e)
        {
            if (!this.mtb_expenseDate.Focused)
            {
                this.mtb_expenseDate.BackColor = Color.FromArgb(0, 76, 157);
                this.border_mtbExpenseDate.FillColor = Color.FromArgb(0, 76, 157);
                this.border_mtbExpenseDate.OnIdleState.FillColor = Color.FromArgb(0, 76, 157);
            }
        }

        private void border_mtbExpenseDate_Enter(object sender, EventArgs e)
        {
            this.mtb_expenseDate.Focus();
            this.mtb_expenseDate.BackColor = Color.FromArgb(0, 104, 232);
            this.border_mtbExpenseDate.FillColor = Color.FromArgb(0, 104, 232);
            this.border_mtbExpenseDate.OnIdleState.FillColor = Color.FromArgb(0, 104, 232);
        }

        private void txt_repeatsOrParcels_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void mtb_expenseDate_Leave(object sender, EventArgs e)
        {
            this.mtb_expenseDate.BackColor = Color.FromArgb(0, 76, 157);
            this.border_mtbExpenseDate.FillColor = Color.FromArgb(0, 76, 157);
            this.border_mtbExpenseDate.OnIdleState.FillColor = Color.FromArgb(0, 76, 157);
        }

        private void mtb_expenseDate_Enter(object sender, EventArgs e)
        {
            this.mtb_expenseDate.BackColor = Color.FromArgb(0, 104, 232);
            this.border_mtbExpenseDate.FillColor = Color.FromArgb(0, 104, 232);
            this.border_mtbExpenseDate.OnIdleState.FillColor = Color.FromArgb(0, 104, 232);
        }
    }
}