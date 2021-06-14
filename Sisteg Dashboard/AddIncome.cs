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
    public partial class AddIncome : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        private static DataTable dataTableIncome;
        private static int idReceita;
        private List<Repeat> repeats = new List<Repeat>();
        private List<Parcel> parcels = new List<Parcel>();

        //INICIA INSTÂNCIA DO PAINEL CONJUNTO À UM DATABASE DE DESPESAS PARA ATUALIZAÇÃO E EXCLUSÃO, QUE PODE SER NULO EM CASO DE CADASTRO
        public AddIncome(DataTable dataTable)
        {
            InitializeComponent();
            dataTableIncome = dataTable;
            this.panel_income.VerticalScroll.Enabled = false;
            this.panel_income.VerticalScroll.Visible = false;
            this.txt_incomeValue.Focus();

            //Popula o combobox de conta da receita
            DataTable incomeAccountDataTable = Database.query("SELECT conta.nomeConta FROM conta ORDER BY conta.nomeConta;");
            for (int i = 0; i < incomeAccountDataTable.Rows.Count; i++) this.cbb_incomeAccount.Items.Insert(i, " " + incomeAccountDataTable.Rows[i].ItemArray[0].ToString());

            //Popula o combobox de categoria da despesa
            DataTable incomeCategoryDataTable = Database.query("SELECT categoria.nomeCategoria FROM categoria WHERE categoria.categoriaReceita = true ORDER BY conta.nomeCategoria;");
            for (int i = 0; i < incomeCategoryDataTable.Rows.Count; i++) this.cbb_incomeCategory.Items.Insert(i, " " + incomeCategoryDataTable.Rows[i].ItemArray[0].ToString());

            if (dataTableIncome != null)
            {
                foreach (DataRow dataRow in dataTableIncome.Rows)
                {
                    idReceita = Convert.ToInt32(dataRow.ItemArray[0]);
                    this.txt_incomeValue.Text = String.Format("{0:C}", dataRow.ItemArray[3]);
                    this.txt_incomeDescription.Text = dataRow.ItemArray[4].ToString();
                    this.mtb_incomeDate.Text = (Convert.ToDateTime(dataRow.ItemArray[5]).ToShortDateString()).ToString();
                    this.cbb_incomeCategory.SelectedIndex = this.cbb_incomeCategory.FindString(dataRow.ItemArray[6].ToString());
                    this.txt_incomeObservations.Text = dataRow.ItemArray[7].ToString();

                    if (Convert.ToBoolean(dataRow.ItemArray[8]) == true) this.ckb_incomeReceived.Checked = true; else this.ckb_incomeReceived.Checked = false;

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
                            foreach (DataRow dataRowRepeats in Database.query("SELECT idRepeticao FROM repeticao WHERE idReceita = " + idReceita.ToString()).Rows)
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
                            this.txt_incomeValue.Text = String.Format("{0:C}", (Convert.ToDecimal(dataRow.ItemArray[3]) * Convert.ToInt32(dataRow.ItemArray[13])));
                            int i = 0;
                            foreach (DataRow dataRowParcels in Database.query("SELECT * FROM parcela WHERE idReceita = " + idReceita.ToString()).Rows)
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
            this.cbb_period.Text = " Período";
            this.cbb_period.Hide();
            this.txt_incomeValue.PlaceholderText = "";
            this.txt_incomeValue.Focus();
        }

        //FUNÇÃO QUE RETORNA OS CAMPOS DO FORMULÁRIO AO ESTADO INICIAL
        private void clearFields()
        {
            this.txt_incomeValue.Clear();
            this.txt_incomeDescription.Clear();
            this.mtb_incomeDate.Text = (DateTime.Today.ToShortDateString()).ToString();
            this.cbb_incomeCategory.SelectedIndex = -1;
            this.cbb_incomeCategory.Text = " Categoria";
            this.txt_incomeObservations.Clear();
            this.doNotRepeatOrParcel();
        }

        //FUNÇÃO QUE SELECIONA A DATA DA TRANSAÇÃO DA REPETIÇÃO DE ACORDO COM O PERÍDO ESCOLHIDO PELO USUÁRIO
        private void periodSelection(int i, Income income, List<Repeat> repeats)
        {
            if (i == 0)
            {
                switch (income.periodoRepetirParcelarReceita)
                {
                    case "Diário":
                        repeats[i].dataTransacao = income.dataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        repeats[i].dataTransacao = income.dataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        repeats[i].dataTransacao = income.dataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        repeats[i].dataTransacao = income.dataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        repeats[i].dataTransacao = income.dataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        repeats[i].dataTransacao = income.dataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        repeats[i].dataTransacao = income.dataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        repeats[i].dataTransacao = income.dataTransacao.AddYears(1);
                        break;
                }
            }
            else
            {
                switch (income.periodoRepetirParcelarReceita)
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
        private void updateOneParcelOrRepeat(Income income)
        {
            if (Database.updateIncomeNotParceledOrRepeated(income))
            {
                MessageBox.Show("Receita atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.doNotRepeatOrParcel();
            }
            else MessageBox.Show("[ERRO] Não foi possível atualizar receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //FUNÇÃO QUE ATUALIZA PARCELA OU REPETIÇÃO
        private void updateParcelOrRepeat(Income income)
        {
            DataTable parcelOrRepeatDataTable = Database.query("SELECT repetirParcelarReceita, valorFixoReceita, parcelasReceita, repeticoesValorFixoReceita FROM receita WHERE idReceita = " + idReceita);
            if (Database.updateIncome(income))
            {
                int qtde = Convert.ToInt32(Database.query("SELECT repeticoesValorFixoReceita FROM receita WHERE idReceita = " + idReceita).Rows[0].ItemArray[0]);
                if (qtde == 0)
                {
                    //Valor parcelado
                    qtde = Convert.ToInt32(Database.query("SELECT parcelasReceita FROM receita WHERE idReceita = " + idReceita).Rows[0].ItemArray[0]);
                    this.parcels.Clear();
                    if (qtde == 1) this.updateOneParcelOrRepeat(income);
                    DataTable dataTable = Database.query("SELECT idParcela FROM parcela WHERE idReceita = " + idReceita);
                    if (dataTable.Rows.Count == 0)
                    {
                        for (int j = 0; j < (income.parcelasReceita - 1); j++)
                        {
                            parcels.Add(new Parcel());
                            parcels[j].idReceita = idReceita;
                            parcels[j].idConta = income.idConta;
                            parcels[j].idCategoria = income.idCategoria;
                            parcels[j].valorParcela = income.valorReceita;
                            parcels[j].descricaoParcela = income.descricaoReceita;
                            this.periodSelection(j, income, this.parcels);
                            parcels[j].observacoesParcela = income.observacoesReceita;
                            parcels[j].recebimentoConfirmado = false;
                            if (Database.newParcel(parcels[j])) continue;
                            else
                            {
                                MessageBox.Show("Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                    }
                    dataTable = Database.query("SELECT idParcela FROM parcela WHERE idReceita = " + idReceita);
                    //Parcelas
                    int i = 0;
                    foreach (DataRow dataRowParcels in dataTable.Rows)
                    {
                        if(this.parcels.Count == 0 && i == 0) parcels.Add(new Parcel());
                        parcels[i].idParcela = Convert.ToInt32(dataRowParcels.ItemArray[0]);
                        i++;
                    }
                    this.changeParcel(parcelOrRepeatDataTable, income);
                }
                else
                {
                    //Valor repetido
                    this.repeats.Clear();
                    if (qtde == 1) this.updateOneParcelOrRepeat(income);
                    DataTable dataTable = Database.query("SELECT idRepeticao FROM repeticao WHERE idReceita = " + idReceita);
                    if (dataTable.Rows.Count == 0)
                    {
                        for (int j = 0; j < (income.repeticoesValorFixoReceita - 1); j++)
                        {
                            this.repeats.Add(new Repeat());
                            this.repeats[j].idReceita = idReceita;
                            this.repeats[j].idConta = income.idConta;
                            this.repeats[j].idCategoria = income.idCategoria;
                            this.repeats[j].valorRepeticao = income.valorReceita;
                            this.repeats[j].descricaoRepeticao = income.descricaoReceita;
                            this.periodSelection(j, income, this.repeats);
                            this.repeats[j].observacoesRepeticao = income.observacoesReceita;
                            this.repeats[j].recebimentoConfirmado = false;
                            if (Database.newRepeat(this.repeats[j])) continue;
                            else
                            {
                                MessageBox.Show("Não foi possível atualizar todas as repetições!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                    }
                    dataTable = Database.query("SELECT idRepeticao FROM repeticao WHERE idReceita = " + idReceita);
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
                        this.changeRepeat(parcelOrRepeatDataTable, income);
                    } 
                    else
                    {
                        int i = 0;
                        foreach (DataRow dataRowRepeats in dataTable.Rows)
                        {
                            repeats[i].idRepeticao = Convert.ToInt32(dataRowRepeats.ItemArray[0]);
                            i++;
                        }
                        this.changeRepeat(parcelOrRepeatDataTable, income);
                    }
                }
            }
            else MessageBox.Show("Não foi possível atualizar receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //FUNÇÃO QUE ATUALIZA AS INFORMAÇÕES DAS REPETIÇÕES
        private void changeRepeat(DataTable dataTable, Income income)
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
                    if (qtde != income.repeticoesValorFixoReceita)
                    {
                        if (qtde > income.repeticoesValorFixoReceita)
                        {
                            //Diminiu repetições
                            for (int i = 0; i < (income.repeticoesValorFixoReceita - 1); i++)
                            {
                                repeats.Add(new Repeat());
                                repeats[i].idRepeticao = this.repeats[i].idRepeticao;
                                repeats[i].idConta = income.idConta;
                                repeats[i].idCategoria = income.idCategoria;
                                repeats[i].valorRepeticao = income.valorReceita;
                                repeats[i].descricaoRepeticao = income.descricaoReceita;
                                this.periodSelection(i, income, repeats);
                                repeats[i].observacoesRepeticao = income.observacoesReceita;
                                repeats[i].recebimentoConfirmado = false;
                                if (Database.updateRepeat(repeats[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                            for (int i = (income.repeticoesValorFixoReceita - 1); i < (qtde - 1); i++)
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
                        else if (qtde < income.repeticoesValorFixoReceita)
                        {
                            //Aumentou repetições
                            for (int i = 0; i < (qtde - 1); i++)
                            {
                                repeats.Add(new Repeat());
                                repeats[i].idRepeticao = this.repeats[i].idRepeticao;
                                repeats[i].idConta = income.idConta;
                                repeats[i].idCategoria = income.idCategoria;
                                repeats[i].valorRepeticao = income.valorReceita;
                                repeats[i].descricaoRepeticao = income.descricaoReceita;
                                this.periodSelection(i, income, repeats);
                                repeats[i].observacoesRepeticao = income.observacoesReceita;
                                repeats[i].recebimentoConfirmado = false;
                                if (Database.updateRepeat(repeats[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                            for (int i = (qtde - 1); i < (income.repeticoesValorFixoReceita - 1); i++)
                            {
                                repeats.Add(new Repeat());
                                repeats[i].idReceita = idReceita;
                                repeats[i].idConta = income.idConta;
                                repeats[i].idCategoria = income.idCategoria;
                                repeats[i].valorRepeticao = income.valorReceita;
                                repeats[i].descricaoRepeticao = income.descricaoReceita;
                                this.periodSelection(i, income, repeats);
                                repeats[i].observacoesRepeticao = income.observacoesReceita;
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
                        for (int i = 0; i < (income.repeticoesValorFixoReceita - 1); i++)
                        {
                            repeats.Add(new Repeat());
                            repeats[i].idRepeticao = this.repeats[i].idRepeticao;
                            repeats[i].idConta = income.idConta;
                            repeats[i].idCategoria = income.idCategoria;
                            repeats[i].valorRepeticao = income.valorReceita;
                            repeats[i].descricaoRepeticao = income.descricaoReceita;
                            this.periodSelection(i, income, repeats);
                            repeats[i].observacoesRepeticao = income.observacoesReceita;
                            repeats[i].recebimentoConfirmado = false;
                            if (Database.updateRepeat(repeats[i])) continue;
                            else
                            {
                                success = 0;
                                break;
                            }
                        }
                    }
                    if (success == 0) MessageBox.Show("Não foi possível atualizar todas as repetições!", "", MessageBoxButtons.OK, MessageBoxIcon.Error); else MessageBox.Show("Receita atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txt_incomeValue.PlaceholderText = "";
                    this.txt_incomeValue.Focus();
                    return;
                }
                else
                {
                    MessageBox.Show("Receita atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txt_incomeValue.PlaceholderText = "";
                    this.txt_incomeValue.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Receita atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txt_incomeValue.PlaceholderText = "";
                this.txt_incomeValue.Focus();
                return;
            }
        }

        //FUNÇÃO QUE ATUALIZA AS INFORMAÇÕES DAS PARCELAS
        private void changeParcel(DataTable dataTable, Income income)
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
                    if (qtde != income.parcelasReceita)
                    {
                        if (qtde > income.parcelasReceita)
                        {
                            //Diminuiu parcelas
                            for (int i = 0; i < (income.parcelasReceita - 1); i++)
                            {
                                parcels.Add(new Parcel());
                                parcels[i].idParcela = this.parcels[i].idParcela;
                                parcels[i].idConta = income.idConta;
                                parcels[i].idCategoria = income.idCategoria;
                                parcels[i].valorParcela = income.valorReceita;
                                parcels[i].descricaoParcela = income.descricaoReceita;
                                this.periodSelection(i, income, parcels);
                                parcels[i].observacoesParcela = income.observacoesReceita;
                                parcels[i].recebimentoConfirmado = false;
                                if (Database.updateParcel(parcels[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
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
                                    break;
                                }
                            }
                        }
                        else if (qtde < income.parcelasReceita)
                        {
                            //Aumentou parcelas
                            for (int i = 0; i < (qtde - 1); i++)
                            {
                                parcels.Add(new Parcel());
                                parcels[i].idParcela = this.parcels[i].idParcela;
                                parcels[i].idConta = income.idConta;
                                parcels[i].idCategoria = income.idCategoria;
                                parcels[i].valorParcela = income.valorReceita;
                                parcels[i].descricaoParcela = income.descricaoReceita;
                                this.periodSelection(i, income, parcels);
                                parcels[i].observacoesParcela = income.observacoesReceita;
                                parcels[i].recebimentoConfirmado = false;
                                if (Database.updateParcel(parcels[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                            for (int i = (qtde - 1); i < (income.parcelasReceita - 1); i++)
                            {
                                parcels.Add(new Parcel());
                                parcels[i].idReceita = idReceita;
                                parcels[i].idConta = income.idConta;
                                parcels[i].idCategoria = income.idCategoria;
                                parcels[i].valorParcela = income.valorReceita;
                                parcels[i].descricaoParcela = income.descricaoReceita;
                                this.periodSelection(i, income, parcels);
                                parcels[i].observacoesParcela = income.observacoesReceita;
                                parcels[i].recebimentoConfirmado = false;
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
                        for (int i = 0; i < (income.parcelasReceita - 1); i++)
                        {
                            parcels.Add(new Parcel());
                            parcels[i].idParcela = this.parcels[i].idParcela;
                            parcels[i].idConta = income.idConta;
                            parcels[i].idCategoria = income.idCategoria;
                            parcels[i].valorParcela = income.valorReceita;
                            parcels[i].descricaoParcela = income.descricaoReceita;
                            this.periodSelection(i, income, parcels);
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
                    if (success == 0) MessageBox.Show("Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error); else MessageBox.Show("Receita atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txt_incomeValue.PlaceholderText = "";
                    this.txt_incomeValue.Focus();
                    return;
                }
                else
                {
                    MessageBox.Show("Receita atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txt_incomeValue.PlaceholderText = "";
                    this.txt_incomeValue.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Receita atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txt_incomeValue.PlaceholderText = "";
                this.txt_incomeValue.Focus();
                return;
            }
        }

        //CADASTRAR RECEITA
        private void pcb_incomeRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_incomeRegister.Image = Properties.Resources.btn_add_income_active;
        }

        private void pcb_incomeRegister_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_incomeRegister.Image = Properties.Resources.btn_add_income;
        }

        private void pcb_incomeRegister_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txt_incomeValue.Text.Trim()) || (this.cbb_incomeCategory.SelectedIndex == -1)) MessageBox.Show("Informe o valor e a categoria da receita afafsf!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if ((this.ckb_repeatOrParcelValue.Checked) && ((String.IsNullOrEmpty(this.txt_repeatsOrParcels.Text.Trim())) || (this.cbb_period.SelectedIndex == -1)))
                {
                    if (this.rbtn_fixedValue.Checked) MessageBox.Show("Informe o número de repetições da receita e o período em que elas se repetem!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else if (this.rbtn_parcelValue.Checked) MessageBox.Show("Informe o número de parcelas da receita e o período em que elas se repetem!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Income income = new Income();
                    income.idConta = Convert.ToInt32(Database.query("SELECT idConta FROM conta WHERE nomeConta = '" + this.cbb_incomeAccount.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    income.idCategoria = Convert.ToInt32(Database.query("SELECT idCategoria FROM categoria WHERE nomeCategoria = '" + this.cbb_incomeCategory.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                    string valorReceita = txt_incomeValue.Text.Trim();
                    if (regexValor.IsMatch(valorReceita))
                    {
                        if (valorReceita.Contains("R$ ")) income.valorReceita = Convert.ToDecimal(valorReceita.Substring(3));
                        else if (valorReceita.Contains("R$")) income.valorReceita = Convert.ToDecimal(valorReceita.Substring(2));
                        else income.valorReceita = Convert.ToDecimal(txt_incomeValue.Text);
                    }
                    else
                    {
                        MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.txt_incomeValue.Clear();
                        this.txt_incomeValue.PlaceholderText = "";
                        this.txt_incomeValue.Focus();
                        return;
                    }
                    income.descricaoReceita = txt_incomeDescription.Text.Trim();
                    income.dataTransacao = Convert.ToDateTime(mtb_incomeDate.Text.Trim());
                    income.observacoesReceita = txt_incomeObservations.Text.Trim();
                    if (ckb_incomeReceived.Checked) income.recebimentoConfirmado = true; else income.recebimentoConfirmado = false;

                    if (ckb_repeatOrParcelValue.Checked)
                    {
                        income.repetirParcelarReceita = true;

                        if (rbtn_fixedValue.Checked)
                        {
                            income.valorFixoReceita = true;
                            income.repeticoesValorFixoReceita = Convert.ToInt32(txt_repeatsOrParcels.Text);
                            income.periodoRepetirParcelarReceita = cbb_period.SelectedItem.ToString();
                            if (Database.newIncome(income))
                            {
                                List<Repeat> repeats = new List<Repeat>();
                                int success = 1;
                                for (int i = 0; i < (income.repeticoesValorFixoReceita - 1); i++)
                                {
                                    repeats.Add(new Repeat());
                                    repeats[i].idReceita = Convert.ToInt32(Database.query("SELECT idReceita FROM receita ORDER BY idReceita DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                    repeats[i].idConta = income.idConta;
                                    repeats[i].idCategoria = income.idCategoria;
                                    repeats[i].valorRepeticao = income.valorReceita;
                                    repeats[i].descricaoRepeticao = income.descricaoReceita;
                                    this.periodSelection(i, income, repeats);
                                    repeats[i].observacoesRepeticao = income.observacoesReceita;
                                    repeats[i].recebimentoConfirmado = false;
                                    if (Database.newRepeat(repeats[i])) continue;
                                    else
                                    {
                                        success = 0;
                                        break;
                                    }
                                }
                                if (success == 0) MessageBox.Show("Não foi possível cadastrar todas as repetições!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MessageBox.Show("Receita cadastrada com suceso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.clearFields();
                            }
                            else MessageBox.Show("Não foi possível cadastrar receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else income.valorFixoReceita = false;

                        if (rbtn_parcelValue.Checked)
                        {
                            income.parcelarValorReceita = true;
                            income.parcelasReceita = Convert.ToInt32(txt_repeatsOrParcels.Text);
                            income.periodoRepetirParcelarReceita = cbb_period.SelectedItem.ToString();
                            income.valorReceita = income.valorReceita / income.parcelasReceita;

                            if (Database.newIncome(income))
                            {
                                List<Parcel> parcels = new List<Parcel>();
                                int success = 1;
                                for (int i = 0; i < (income.parcelasReceita - 1); i++)
                                {
                                    parcels.Add(new Parcel());
                                    parcels[i].idReceita = Convert.ToInt32(Database.query("SELECT idReceita FROM receita ORDER BY idReceita DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                    parcels[i].idConta = income.idConta;
                                    parcels[i].idCategoria = income.idCategoria;
                                    parcels[i].valorParcela = income.valorReceita;
                                    parcels[i].descricaoParcela = income.descricaoReceita;
                                    this.periodSelection(i, income, parcels);
                                    parcels[i].observacoesParcela = income.observacoesReceita;
                                    parcels[i].recebimentoConfirmado = false;
                                    if (Database.newParcel(parcels[i])) continue;
                                    else
                                    {
                                        success = 0;
                                        break;
                                    }
                                }
                                if (success == 0) MessageBox.Show("Não foi possível cadastrar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MessageBox.Show("Receita cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.clearFields();
                            }
                            else MessageBox.Show("Não foi possível cadastrar receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        else income.parcelarValorReceita = false;
                    }
                    else
                    {
                        income.repetirParcelarReceita = false;
                        if (Database.newIncome(income))
                        {
                            MessageBox.Show("Receita cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.clearFields();
                        }
                        else MessageBox.Show("Não foi possível cadastrar receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //ATUALIZAR RECEITA
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
            if (String.IsNullOrEmpty(txt_incomeValue.Text.Trim()) || (this.cbb_incomeCategory.SelectedIndex == -1)) MessageBox.Show("Informe o valor e a categoria da receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if ((this.ckb_repeatOrParcelValue.Checked) && ((String.IsNullOrEmpty(this.txt_repeatsOrParcels.Text.Trim())) || (this.cbb_period.SelectedIndex == -1)))
                {
                    if (this.rbtn_fixedValue.Checked) MessageBox.Show("Informe o número de repetições da receita e o período em que elas se repetem!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else if (this.rbtn_parcelValue.Checked) MessageBox.Show("Informe o número de parcelas da receita e o período em que elas se repetem!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Income income = new Income();
                    income.idReceita = idReceita;
                    income.idConta = Convert.ToInt32(Database.query("SELECT idConta FROM conta WHERE nomeConta = '" + this.cbb_incomeAccount.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    income.idCategoria = Convert.ToInt32(Database.query("SELECT idCategoria FROM categoria WHERE nomeCategoria = '" + this.cbb_incomeCategory.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                    string valorReceita = txt_incomeValue.Text;
                    if (regexValor.IsMatch(valorReceita))
                    {
                        if (valorReceita.Contains("R$ ")) income.valorReceita = Convert.ToDecimal(valorReceita.Substring(3));
                        else if (valorReceita.Contains("R$")) income.valorReceita = Convert.ToDecimal(valorReceita.Substring(2));
                        else income.valorReceita = Convert.ToDecimal(txt_incomeValue.Text);
                    }
                    else
                    {
                        MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    income.descricaoReceita = txt_incomeDescription.Text;
                    income.dataTransacao = Convert.ToDateTime(mtb_incomeDate.Text);
                    income.observacoesReceita = txt_incomeObservations.Text;
                    if (ckb_incomeReceived.Checked) income.recebimentoConfirmado = true; else income.recebimentoConfirmado = false;

                    if (ckb_repeatOrParcelValue.Checked)
                    {
                        income.repetirParcelarReceita = true;

                        if (rbtn_fixedValue.Checked)
                        {
                            income.valorFixoReceita = true;
                            income.repeticoesValorFixoReceita = Convert.ToInt32(txt_repeatsOrParcels.Text);
                            income.periodoRepetirParcelarReceita = cbb_period.SelectedItem.ToString();
                            if (Database.deleteAllParcels(income))
                            {
                                this.updateParcelOrRepeat(income);
                            }
                        }
                        else income.valorFixoReceita = false;

                        if (rbtn_parcelValue.Checked)
                        {
                            income.parcelarValorReceita = true;
                            income.parcelasReceita = Convert.ToInt32(txt_repeatsOrParcels.Text);
                            income.periodoRepetirParcelarReceita = cbb_period.SelectedItem.ToString();
                            income.valorReceita = income.valorReceita / income.parcelasReceita;
                            if (Database.deleteAllRepeats(income))
                            {
                                this.updateParcelOrRepeat(income);
                            }
                        }
                        else income.parcelarValorReceita = false;
                    }
                    else
                    {
                        if (Database.deleteAllParcels(income)) if (Database.deleteAllRepeats(income)) this.updateOneParcelOrRepeat(income);
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
            Income income = new Income();
            income.idReceita = idReceita;
            foreach(DataRow dataRow in dataTableIncome.Rows)
            {
                if (Convert.ToBoolean(dataRow.ItemArray[9]))
                {
                    if (Convert.ToBoolean(dataRow.ItemArray[10]))
                    {
                        int success = 1;
                        for(int i = 0; i < (this.repeats.Count); i++) 
                        {
                            if (Database.deleteRepeat(this.repeats[i])) continue;
                            else
                            {
                                success = 0;
                                break;
                            }
                        }
                        if (success == 0) MessageBox.Show("Não foi possível atualizar todas as repetições!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (Database.deleteIncome(income)) MessageBox.Show("Receita excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information); else MessageBox.Show("Não foi possível excluída receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        if (Database.deleteIncome(income)) MessageBox.Show("Receita excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information); else MessageBox.Show("Não foi possível excluída receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    if (Database.deleteIncome(income)) MessageBox.Show("Receita excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information); else MessageBox.Show("Não foi possível excluída receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (Application.OpenForms.OfType<Main>().Count() == 0)
                    {
                        Main main = new Main();
                        main.Show();
                        this.Close();
                    }
                }
            }
            if (Database.deleteIncome(income)) MessageBox.Show("Receita excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information); else MessageBox.Show("Não foi possível excluída receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            else this.doNotRepeatOrParcel();
        }

        private void rbtn_fixedValue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_fixedValue.Checked) this.txt_repeatsOrParcels.PlaceholderText = "Repetições";
        }

        private void rbtn_parcelValue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_parcelValue.Checked) this.txt_repeatsOrParcels.PlaceholderText = "Parcelas";
        }

        private void txt_incomeValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ',') && (e.KeyChar != 'R') && (e.KeyChar != '$') && (e.KeyChar != ' ')) e.Handled = true;
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1)) e.Handled = true;
        }

        private void txt_incomeValue_Leave(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(this.txt_incomeValue.Text)) && (!this.txt_incomeValue.Focused)) this.txt_incomeValue.PlaceholderText = "R$ 0,00";
        }

        private void mtb_incomeDate_MouseEnter(object sender, EventArgs e)
        {
            this.mtb_incomeDate.BackColor = Color.FromArgb(0, 104, 232);
            this.border_mtbIncomeDate.FillColor  = Color.FromArgb(0, 104, 232);
            this.border_mtbIncomeDate.OnIdleState.FillColor = Color.FromArgb(0, 104, 232);
        }

        private void mtb_incomeDate_MouseLeave(object sender, EventArgs e)
        {
            if (!this.mtb_incomeDate.Focused)
            {
                this.mtb_incomeDate.BackColor = Color.FromArgb(0, 76, 157);
                this.border_mtbIncomeDate.FillColor = Color.FromArgb(0, 76, 157);
                this.border_mtbIncomeDate.OnIdleState.FillColor = Color.FromArgb(0, 76, 157);
            }
        }

        private void border_mtbIncomeDate_Enter(object sender, EventArgs e)
        {
            this.mtb_incomeDate.Focus();
            this.mtb_incomeDate.BackColor = Color.FromArgb(0, 104, 232);
            this.border_mtbIncomeDate.FillColor = Color.FromArgb(0, 104, 232);
            this.border_mtbIncomeDate.OnIdleState.FillColor = Color.FromArgb(0, 104, 232);
        }

        private void txt_repeatsOrParcels_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void mtb_incomeDate_Leave(object sender, EventArgs e)
        {
            this.mtb_incomeDate.BackColor = Color.FromArgb(0, 76, 157);
            this.border_mtbIncomeDate.FillColor = Color.FromArgb(0, 76, 157);
            this.border_mtbIncomeDate.OnIdleState.FillColor = Color.FromArgb(0, 76, 157);
        }

        private void mtb_incomeDate_Enter(object sender, EventArgs e)
        {
            this.mtb_incomeDate.BackColor = Color.FromArgb(0, 104, 232);
            this.border_mtbIncomeDate.FillColor = Color.FromArgb(0, 104, 232);
            this.border_mtbIncomeDate.OnIdleState.FillColor = Color.FromArgb(0, 104, 232);
        }
    }
}
