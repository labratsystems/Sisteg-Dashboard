using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class AddIncome : Form
    {
        //INICIA INSTÂNCIA DO PAINEL CONJUNTO À UM DATATABLE DE DESPESAS PARA ATUALIZAÇÃO E EXCLUSÃO, QUE PODE SER NULO EM CASO DE CADASTRO
        public AddIncome(DataTable dataTable, Main main)
        {
            InitializeComponent();
            Globals.incomeDataTable = dataTable;
            Globals.main = main;
            this.txt_incomeValue.Focus();

            //Popula o combobox de contas da receita
            DataTable incomeAccountDataTable = Database.query("SELECT conta.nomeConta FROM conta ORDER BY conta.nomeConta;");
            for (int i = 0; i < incomeAccountDataTable.Rows.Count; i++) this.cbb_incomeAccount.Items.Insert(i, " " + incomeAccountDataTable.Rows[i].ItemArray[0].ToString().Trim());

            //Popula o combobox de categorias da despesa
            DataTable incomeCategoryDataTable = Database.query("SELECT categoria.nomeCategoria FROM categoria WHERE categoria.categoriaReceita = true ORDER BY categoria.nomeCategoria;");
            for (int i = 0; i < incomeCategoryDataTable.Rows.Count; i++) this.cbb_incomeCategory.Items.Insert(i, " " + incomeCategoryDataTable.Rows[i].ItemArray[0].ToString().Trim());
            this.cbb_incomeCategory.Items.RemoveAt(this.cbb_incomeCategory.FindString(" Orçamentos"));

            if(Globals.idConta != 0)
            {
                DataTable sumTotalValueDataTable = Database.query("SELECT somarTotal FROM conta WHERE idConta = " + Globals.idConta);
                if(sumTotalValueDataTable.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(sumTotalValueDataTable.Rows[0].ItemArray[0])) Globals.saldoConta = Convert.ToDecimal(Database.query("SELECT saldoConta FROM conta WHERE idConta = " + Globals.idConta).Rows[0].ItemArray[0]);
                    else Globals.saldoConta = 0;
                }
            }

            if(Globals.saldoConta < 0)
            {
                Globals.main.lbl_balanceTag.ForeColor = Color.FromArgb(243, 104, 82);
                Globals.main.lbl_balance.ForeColor = Color.FromArgb(243, 104, 82);
            }
            else
            {
                Globals.main.lbl_balanceTag.ForeColor = Color.FromArgb(77, 255, 255);
                Globals.main.lbl_balance.ForeColor = Color.FromArgb(77, 255, 255);
            }

            if (Globals.incomeDataTable != null)
            {
                foreach (DataRow dataRow in Globals.incomeDataTable.Rows)
                {
                    Globals.idReceita = Convert.ToInt32(dataRow.ItemArray[0]);
                    Globals.idConta = Convert.ToInt32(dataRow.ItemArray[1]);
                    Globals.idCategoria = Convert.ToInt32(dataRow.ItemArray[3]);
                    
                    this.txt_incomeValue.Text = String.Format("{0:C}", dataRow.ItemArray[4]).Trim();
                    this.txt_incomeDescription.Text = dataRow.ItemArray[5].ToString().Trim();
                    this.mtb_incomeDate.Text = (Convert.ToDateTime(dataRow.ItemArray[6]).ToShortDateString()).ToString().Trim();
                    this.txt_incomeObservations.Text = dataRow.ItemArray[7].ToString().Trim();
                    this.cbb_incomeCategory.SelectedIndex = this.cbb_incomeCategory.FindString(" " + Database.query("SELECT nomeCategoria FROM categoria WHERE idCategoria = " + Globals.idCategoria).Rows[0].ItemArray[0].ToString().Trim());
                    this.cbb_incomeAccount.SelectedIndex = this.cbb_incomeAccount.FindString(" " + Database.query("SELECT nomeConta FROM conta WHERE idConta = " + Globals.idConta).Rows[0].ItemArray[0].ToString().Trim());

                    if (Convert.ToBoolean(dataRow.ItemArray[8])) this.ckb_incomeReceived.Checked = true; 
                    else this.ckb_incomeReceived.Checked = false;

                    if (Convert.ToBoolean(dataRow.ItemArray[9]))
                    {
                        this.ckb_repeatOrParcelValue.Checked = true;
                        this.cbb_period.SelectedIndex = this.cbb_period.FindString(dataRow.ItemArray[14].ToString().Trim());
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
                            this.txt_repeatsOrParcels.Text = dataRow.ItemArray[11].ToString().Trim();
                            int i = 0;
                            foreach (DataRow dataRowRepeats in Database.query("SELECT * FROM repeticao WHERE idReceita = " + Globals.idReceita).Rows)
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

                    if (dataRow.ItemArray[12].ToString() != null)
                    {
                        if (Convert.ToBoolean(dataRow.ItemArray[12]))
                        {
                            this.rbtn_parcelValue.Show();
                            this.lbl_parcelValue.Show();
                            this.rbtn_fixedValue.Checked = false;
                            this.rbtn_parcelValue.Checked = true;
                            this.txt_repeatsOrParcels.Show();
                            this.txt_repeatsOrParcels.Text = dataRow.ItemArray[13].ToString().Trim();
                            this.txt_incomeValue.Text = String.Format("{0:C}", (Convert.ToDecimal(dataRow.ItemArray[4]) * Convert.ToInt32(dataRow.ItemArray[13]))).Trim();
                            int i = 0;
                            foreach (DataRow dataRowParcels in Database.query("SELECT * FROM parcela WHERE idReceita = " + Globals.idReceita).Rows)
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
            this.txt_incomeValue.PlaceholderText = "";
            this.txt_incomeValue.Focus();
        }

        //Função que retorna os campos do formulário ao estado inicial
        private void clearFields()
        {
            this.txt_incomeValue.Clear();
            this.txt_incomeDescription.Clear();
            this.mtb_incomeDate.Text = (DateTime.Today.ToShortDateString()).ToString().Trim();
            this.cbb_incomeCategory.SelectedIndex = -1;
            this.cbb_incomeCategory.Text = " Categoria";
            this.txt_incomeObservations.Clear();
            this.doNotRepeatOrParcel();
        }

        //Função que seleciona a data de transação da repetição de acordo com o período escolhido pelo usuário
        private void periodSelection(int i, Income income, List<Repeat> repeats)
        {
            if (i == 0)
            {
                switch (income.PeriodoRepetirParcelarReceita)
                {
                    case "Diário":
                        repeats[i].DataTransacao = income.DataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        repeats[i].DataTransacao = income.DataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        repeats[i].DataTransacao = income.DataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        repeats[i].DataTransacao = income.DataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        repeats[i].DataTransacao = income.DataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        repeats[i].DataTransacao = income.DataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        repeats[i].DataTransacao = income.DataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        repeats[i].DataTransacao = income.DataTransacao.AddYears(1);
                        break;
                }
            }
            else
            {
                switch (income.PeriodoRepetirParcelarReceita)
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

        //Função que atualiza uma única parcela ou repetição por vez
        private void updateOneParcelOrRepeat(Income income)
        {
            if (Database.updateIncomeNotParceledOrRepeated(income))
            {
                MessageBox.Show("Receita cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.clearFields();
            }
            else MessageBox.Show("[ERRO] Não foi possível atualizar receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Função que atualiza parcela ou repetição
        private void updateParcelOrRepeat(Income income)
        {
            DataTable parcelOrRepeatDataTable = Database.query("SELECT repetirParcelarReceita, valorFixoReceita, parcelasReceita, repeticoesValorFixoReceita FROM receita WHERE idReceita = " + Globals.idReceita);
            if (Database.updateIncome(income))
            {
                int qtde = Convert.ToInt32(Database.query("SELECT repeticoesValorFixoReceita FROM receita WHERE idReceita = " + Globals.idReceita).Rows[0].ItemArray[0]);
                if (qtde == 0)
                {
                    //Valor parcelado
                    qtde = Convert.ToInt32(Database.query("SELECT parcelasReceita FROM receita WHERE idReceita = " + Globals.idReceita).Rows[0].ItemArray[0]);
                    Globals.parcels.Clear();
                    if (qtde == 1) this.updateOneParcelOrRepeat(income);
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
                            if (ckb_incomeReceived.Checked) Globals.parcels[j].RecebimentoConfirmado = true; 
                            else Globals.parcels[j].RecebimentoConfirmado = false;
                            if (Database.newParcel(Globals.parcels[j])) continue;
                            else
                            {
                                MessageBox.Show("[ERRO] Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                    }
                    dataTable = Database.query("SELECT idParcela FROM parcela WHERE idReceita = " + Globals.idReceita);
                    //Parcelas
                    int i = 0;
                    foreach (DataRow dataRowParcels in dataTable.Rows)
                    {
                        if (Globals.parcels.Count == 0 && i == 0) Globals.parcels.Add(new Parcel());
                        Globals.parcels[i].IdParcela = Convert.ToInt32(dataRowParcels.ItemArray[0]);
                        i++;
                    }
                    this.changeParcel(parcelOrRepeatDataTable, income);
                }
                else
                {
                    //Valor repetido
                    Globals.repeats.Clear();
                    if (qtde == 1) this.updateOneParcelOrRepeat(income);
                    DataTable dataTable = Database.query("SELECT idRepeticao FROM repeticao WHERE idReceita = " + Globals.idReceita);
                    if (dataTable.Rows.Count == 0)
                    {
                        for (int j = 0; j < (income.RepeticoesValorFixoReceita - 1); j++)
                        {
                            Globals.repeats.Add(new Repeat());
                            Globals.repeats[j].IdReceita = Globals.idReceita;
                            Globals.repeats[j].IdConta = income.IdConta;
                            Globals.repeats[j].IdCategoria = income.IdCategoria;
                            Globals.repeats[j].ValorRepeticao = income.ValorReceita;
                            Globals.repeats[j].DescricaoRepeticao = income.DescricaoReceita;
                            this.periodSelection(j, income, Globals.repeats);
                            Globals.repeats[j].ObservacoesRepeticao = income.ObservacoesReceita;
                            if (ckb_incomeReceived.Checked) Globals.repeats[j].RecebimentoConfirmado = true; 
                            else Globals.repeats[j].RecebimentoConfirmado = false;
                            if (Database.newRepeat(Globals.repeats[j])) continue;
                            else
                            {
                                MessageBox.Show("[ERRO] Não foi possível atualizar todas as repetições!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                    }
                    dataTable = Database.query("SELECT idRepeticao FROM repeticao WHERE idReceita = " + Globals.idReceita);
                    //Repetições
                    int i = 0;
                    foreach (DataRow dataRowRepeats in dataTable.Rows)
                    {
                        if (Globals.repeats.Count == 0 && i == 0) Globals.repeats.Add(new Repeat());
                        Globals.repeats[i].IdRepeticao = Convert.ToInt32(dataRowRepeats.ItemArray[0]);
                        i++;
                    }
                    this.changeRepeat(parcelOrRepeatDataTable, income);
                }
            }
            else MessageBox.Show("[ERRO] Não foi possível atualizar receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Função que atualiza as informações das repetições
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
                    if (qtde != income.RepeticoesValorFixoReceita)
                    {
                        if (qtde > income.RepeticoesValorFixoReceita)
                        {
                            //Diminuiu repetições
                            for (int i = 0; i < (income.RepeticoesValorFixoReceita - 1); i++)
                            {
                                repeats.Add(new Repeat());
                                repeats[i].IdRepeticao = Globals.repeats[i].IdRepeticao;
                                repeats[i].IdConta = income.IdConta;
                                repeats[i].IdCategoria = income.IdCategoria;
                                repeats[i].ValorRepeticao = income.ValorReceita;
                                repeats[i].DescricaoRepeticao = income.DescricaoReceita;
                                this.periodSelection(i, income, repeats);
                                repeats[i].ObservacoesRepeticao = income.ObservacoesReceita;
                                if (ckb_incomeReceived.Checked) repeats[i].RecebimentoConfirmado = true; 
                                else repeats[i].RecebimentoConfirmado = false;

                                if (Database.updateRepeat(repeats[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                            for (int i = (income.RepeticoesValorFixoReceita - 1); i < (qtde - 1); i++)
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
                        else if (qtde < income.RepeticoesValorFixoReceita)
                        {
                            //Aumentou repetições
                            for (int i = 0; i < (qtde - 1); i++)
                            {
                                repeats.Add(new Repeat());
                                repeats[i].IdRepeticao = Globals.repeats[i].IdRepeticao;
                                repeats[i].IdConta = income.IdConta;
                                repeats[i].IdCategoria = income.IdCategoria;
                                repeats[i].ValorRepeticao = income.ValorReceita;
                                repeats[i].DescricaoRepeticao = income.DescricaoReceita;
                                this.periodSelection(i, income, repeats);
                                repeats[i].ObservacoesRepeticao = income.ObservacoesReceita;
                                if (ckb_incomeReceived.Checked) repeats[i].RecebimentoConfirmado = true; 
                                else repeats[i].RecebimentoConfirmado = false;

                                if (Database.updateRepeat(repeats[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                            for (int i = (qtde - 1); i < (income.RepeticoesValorFixoReceita - 1); i++)
                            {
                                repeats.Add(new Repeat());
                                repeats[i].IdReceita = Globals.idReceita;
                                repeats[i].IdConta = income.IdConta;
                                repeats[i].IdCategoria = income.IdCategoria;
                                repeats[i].ValorRepeticao = income.ValorReceita;
                                repeats[i].DescricaoRepeticao = income.DescricaoReceita;
                                this.periodSelection(i, income, repeats);
                                repeats[i].ObservacoesRepeticao = income.ObservacoesReceita;
                                if (ckb_incomeReceived.Checked) repeats[i].RecebimentoConfirmado = true; 
                                else repeats[i].RecebimentoConfirmado = false;
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
                        for (int i = 0; i < (income.RepeticoesValorFixoReceita - 1); i++)
                        {
                            repeats.Add(new Repeat());
                            repeats[i].IdRepeticao = Globals.repeats[i].IdRepeticao;
                            repeats[i].IdConta = income.IdConta;
                            repeats[i].IdCategoria = income.IdCategoria;
                            repeats[i].ValorRepeticao = income.ValorReceita;
                            repeats[i].DescricaoRepeticao = income.DescricaoReceita;
                            this.periodSelection(i, income, repeats);
                            repeats[i].ObservacoesRepeticao = income.ObservacoesReceita;
                            if (ckb_incomeReceived.Checked) repeats[i].RecebimentoConfirmado = true; 
                            else repeats[i].RecebimentoConfirmado = false;

                            if (Database.updateRepeat(repeats[i])) continue;
                            else
                            {
                                success = 0;
                                break;
                            }
                        }
                    }
                    if (success == 0) MessageBox.Show("[ERRO] Não foi possível atualizar todas as repetições!", "", MessageBoxButtons.OK, MessageBoxIcon.Error); else MessageBox.Show("Receita atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        //Função que atualiza as informações das parcelas
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
                                if (ckb_incomeReceived.Checked) parcels[i].RecebimentoConfirmado = true; 
                                else parcels[i].RecebimentoConfirmado = false;
                                
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
                                if (ckb_incomeReceived.Checked) parcels[i].RecebimentoConfirmado = true; 
                                else parcels[i].RecebimentoConfirmado = false;

                                if (Database.updateParcel(parcels[i])) continue;
                                else
                                {
                                    success = 0;
                                    break;
                                }
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
                                if (ckb_incomeReceived.Checked) parcels[i].RecebimentoConfirmado = true; 
                                else parcels[i].RecebimentoConfirmado = false;
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
                            if (ckb_incomeReceived.Checked) parcels[i].RecebimentoConfirmado = true; 
                            else parcels[i].RecebimentoConfirmado = false;

                            if (Database.updateParcel(parcels[i])) continue;
                            else
                            {
                                success = 0;
                                break;
                            }
                        }
                    }
                    if (success == 0) MessageBox.Show("[ERRO] Não foi possível atualizar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error); else MessageBox.Show("Receita atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        //Função que cadastra receita
        private void incomeRegister()
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
                    Account account = new Account();
                    Income income = new Income();
                    account.IdConta = Globals.idConta;
                    income.IdConta = Convert.ToInt32(Database.query("SELECT idConta FROM conta WHERE nomeConta = '" + this.cbb_incomeAccount.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    income.IdCategoria = Convert.ToInt32(Database.query("SELECT idCategoria FROM categoria WHERE nomeCategoria = '" + this.cbb_incomeCategory.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                    string valorReceita = txt_incomeValue.Text.Trim();
                    if (regexValor.IsMatch(valorReceita))
                    {
                        if (valorReceita.Contains("R$ ")) income.ValorReceita = Convert.ToDecimal(valorReceita.Substring(3).Trim());
                        else if (valorReceita.Contains("R$")) income.ValorReceita = Convert.ToDecimal(valorReceita.Substring(2).Trim());
                        else income.ValorReceita = Convert.ToDecimal(txt_incomeValue.Text.Trim());
                    }
                    else
                    {
                        MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.txt_incomeValue.Clear();
                        this.txt_incomeValue.PlaceholderText = "";
                        this.txt_incomeValue.Focus();
                        return;
                    }
                    income.DescricaoReceita = txt_incomeDescription.Text.Trim();
                    income.DataTransacao = Convert.ToDateTime(mtb_incomeDate.Text.Trim());
                    income.ObservacoesReceita = txt_incomeObservations.Text.Trim();
                    if (ckb_incomeReceived.Checked) income.RecebimentoConfirmado = true; else income.RecebimentoConfirmado = false;

                    if (ckb_repeatOrParcelValue.Checked)
                    {
                        income.RepetirParcelarReceita = true;

                        if (rbtn_fixedValue.Checked)
                        {
                            income.ValorFixoReceita = true;
                            income.RepeticoesValorFixoReceita = Convert.ToInt32(txt_repeatsOrParcels.Text.Trim());
                            income.PeriodoRepetirParcelarReceita = cbb_period.SelectedItem.ToString().Trim();
                            if (Database.newIncome(income))
                            {
                                List<Repeat> repeats = new List<Repeat>();
                                int success = 1;
                                for (int i = 0; i < (income.RepeticoesValorFixoReceita - 1); i++)
                                {
                                    repeats.Add(new Repeat());
                                    repeats[i].IdReceita = Convert.ToInt32(Database.query("SELECT idReceita FROM receita ORDER BY idReceita DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                    repeats[i].IdConta = income.IdConta;
                                    repeats[i].IdCategoria = income.IdCategoria;
                                    repeats[i].ValorRepeticao = income.ValorReceita;
                                    repeats[i].DescricaoRepeticao = income.DescricaoReceita;
                                    this.periodSelection(i, income, repeats);
                                    repeats[i].ObservacoesRepeticao = income.ObservacoesReceita;
                                    if (ckb_incomeReceived.Checked) repeats[i].RecebimentoConfirmado = true; 
                                    else repeats[i].RecebimentoConfirmado = false;
                                    if (Database.newRepeat(repeats[i])) continue;
                                    else
                                    {
                                        success = 0;
                                        break;
                                    }
                                }
                                if (success == 0) MessageBox.Show("[ERRO] Não foi possível cadastrar todas as repetições!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MessageBox.Show("Receita cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.clearFields();
                            }
                            else MessageBox.Show("[ERRO] Não foi possível cadastrar receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else income.ValorFixoReceita = false;

                        if (rbtn_parcelValue.Checked)
                        {
                            income.ParcelarValorReceita = true;
                            income.ParcelasReceita = Convert.ToInt32(txt_repeatsOrParcels.Text.Trim());
                            income.PeriodoRepetirParcelarReceita = cbb_period.SelectedItem.ToString().Trim();
                            income.ValorReceita = income.ValorReceita / income.ParcelasReceita;

                            if (Database.newIncome(income))
                            {
                                List<Parcel> parcels = new List<Parcel>();
                                int success = 1;
                                for (int i = 0; i < (income.ParcelasReceita - 1); i++)
                                {
                                    parcels.Add(new Parcel());
                                    parcels[i].IdReceita = Convert.ToInt32(Database.query("SELECT idReceita FROM receita ORDER BY idReceita DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                    parcels[i].IdConta = income.IdConta;
                                    parcels[i].IdCategoria = income.IdCategoria;
                                    parcels[i].ValorParcela = income.ValorReceita;
                                    parcels[i].DescricaoParcela = income.DescricaoReceita;
                                    this.periodSelection(i, income, parcels);
                                    parcels[i].ObservacoesParcela = income.ObservacoesReceita;
                                    if (ckb_incomeReceived.Checked) parcels[i].RecebimentoConfirmado = true; 
                                    else parcels[i].RecebimentoConfirmado = false;
                                    if (Database.newParcel(parcels[i])) continue;
                                    else
                                    {
                                        success = 0;
                                        break;
                                    }
                                }
                                if (success == 0) MessageBox.Show("[ERRO] Não foi possível cadastrar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MessageBox.Show("Receita cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.clearFields();
                            }
                            else MessageBox.Show("[ERRO] Não foi possível cadastrar receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        else income.ParcelarValorReceita = false;
                    }
                    else
                    {
                        income.RepetirParcelarReceita = false;
                        if (Database.newIncome(income))
                        {
                            MessageBox.Show("Receita cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.clearFields();
                        }
                        else MessageBox.Show("[ERRO] Não foi possível cadastrar receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //Função que atualiza receita
        private void incomeUpdate()
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
                    Account account = new Account();
                    Income income = new Income();
                    account.IdConta = Globals.idConta;
                    income.IdReceita = Globals.idReceita;
                    income.IdConta = Convert.ToInt32(Database.query("SELECT idConta FROM conta WHERE nomeConta = '" + this.cbb_incomeAccount.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    income.IdCategoria = Convert.ToInt32(Database.query("SELECT idCategoria FROM categoria WHERE nomeCategoria = '" + this.cbb_incomeCategory.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                    string valorReceita = txt_incomeValue.Text.Trim();
                    if (regexValor.IsMatch(valorReceita))
                    {
                        if (valorReceita.Contains("R$ ")) income.ValorReceita = Convert.ToDecimal(valorReceita.Substring(3).Trim());
                        else if (valorReceita.Contains("R$")) income.ValorReceita = Convert.ToDecimal(valorReceita.Substring(2).Trim());
                        else income.ValorReceita = Convert.ToDecimal(txt_incomeValue.Text.Trim());
                    }
                    else
                    {
                        MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    income.DescricaoReceita = txt_incomeDescription.Text.Trim();
                    income.DataTransacao = Convert.ToDateTime(mtb_incomeDate.Text.Trim());
                    income.ObservacoesReceita = txt_incomeObservations.Text.Trim();
                    if (ckb_incomeReceived.Checked) income.RecebimentoConfirmado = true; else income.RecebimentoConfirmado = false;

                    if (ckb_repeatOrParcelValue.Checked)
                    {
                        income.RepetirParcelarReceita = true;

                        if (rbtn_fixedValue.Checked)
                        {
                            income.ValorFixoReceita = true;
                            income.RepeticoesValorFixoReceita = Convert.ToInt32(txt_repeatsOrParcels.Text.Trim());
                            income.PeriodoRepetirParcelarReceita = cbb_period.SelectedItem.ToString().Trim();
                            if (Database.deleteAllParcels(income)) this.updateParcelOrRepeat(income);
                        }
                        else income.ValorFixoReceita = false;

                        if (rbtn_parcelValue.Checked)
                        {
                            income.ParcelarValorReceita = true;
                            income.ParcelasReceita = Convert.ToInt32(txt_repeatsOrParcels.Text.Trim());
                            income.PeriodoRepetirParcelarReceita = cbb_period.SelectedItem.ToString().Trim();
                            income.ValorReceita = income.ValorReceita / income.ParcelasReceita;
                            if (Database.deleteAllRepeats(income)) this.updateParcelOrRepeat(income);
                        }
                        else income.ParcelarValorReceita = false;
                    }
                    else if (Database.deleteAllParcels(income)) if (Database.deleteAllRepeats(income)) this.updateOneParcelOrRepeat(income);
                }
            }
        }

        //Função que exclui receita
        private void incomeDelete()
        {
            if (((DialogResult)MessageBox.Show("Tem certeza que deseja excluir a receita?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)).ToString().ToUpper() == "NO") return;
            Account account = new Account();
            Income income = new Income();
            account.IdConta = Globals.idConta;
            income.IdReceita = Globals.idReceita;
            foreach (DataRow dataRow in Globals.incomeDataTable.Rows)
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
                        if (Database.deleteIncome(income)) MessageBox.Show("Receita excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else MessageBox.Show("[ERRO] Não foi possível excluir receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        if (Database.deleteIncome(income)) MessageBox.Show("Receita excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else MessageBox.Show("[ERRO] Não foi possível excluir receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    if (Database.deleteIncome(income)) MessageBox.Show("Receita excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else MessageBox.Show("[ERRO] Não foi possível excluir receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (Application.OpenForms.OfType<Main>().Count() == 0)
                    {
                        Main main = new Main();
                        main.Show();
                        this.Close();
                    }
                }
            }
        }

        //Função que muda a cor do campo data da receita
        private void setRGB(int r, int g, int b)
        {
            this.mtb_incomeDate.BackColor = Color.FromArgb(r, g, b);
            this.border_mtbIncomeDate.FillColor = Color.FromArgb(r, g, b);
            this.border_mtbIncomeDate.OnIdleState.FillColor = Color.FromArgb(r, g, b);
        }

        //CADASTRAR RECEITA
        private void pcb_incomeRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_incomeRegister.BackgroundImage = Properties.Resources.btn_add_income_active;
        }

        private void pcb_incomeRegister_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_incomeRegisterTag.ClientRectangle.Contains(lbl_incomeRegisterTag.PointToClient(Cursor.Position))) this.pcb_incomeRegister.BackgroundImage = Properties.Resources.btn_add_income;
        }

        private void pcb_incomeRegister_Click(object sender, EventArgs e)
        {
            this.incomeRegister();
        }

        private void lbl_incomeRegisterTag_Click(object sender, EventArgs e)
        {
            this.incomeRegister();
        }

        //ATUALIZAR RECEITA
        private void pcb_btnUpdate_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnUpdate.BackgroundImage = Properties.Resources.btn_update_active;
        }

        private void pcb_btnUpdate_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnUpdateTag.ClientRectangle.Contains(lbl_btnUpdateTag.PointToClient(Cursor.Position)))  this.pcb_btnUpdate.BackgroundImage = Properties.Resources.btn_update;
        }

        private void pcb_btnUpdate_Click(object sender, EventArgs e)
        {
            this.incomeUpdate();
        }

        private void lbl_btnUpdateTag_Click(object sender, EventArgs e)
        {
            this.incomeUpdate();
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
            this.incomeDelete();
        }

        private void lbl_btnDeleteTag_Click(object sender, EventArgs e)
        {
            this.incomeDelete();
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

        private void txt_incomeValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ',') && (e.KeyChar != 'R') && (e.KeyChar != '$') && (e.KeyChar != ' ')) e.Handled = true;
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1)) e.Handled = true;
        }

        private void mtb_incomeDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void txt_repeatsOrParcels_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        //MouseEnter
        private void mtb_incomeDate_MouseEnter(object sender, EventArgs e)
        {
            this.setRGB(0, 104, 232);
        }

        //MouseLeave

        private void mtb_incomeDate_MouseLeave(object sender, EventArgs e)
        {
            if (!this.mtb_incomeDate.Focused) this.setRGB(0, 76, 157);
        }

        //Enter

        private void border_mtbIncomeDate_Enter(object sender, EventArgs e)
        {
            this.mtb_incomeDate.Focus();
            this.setRGB(0, 104, 232);
        }

        private void mtb_incomeDate_Enter(object sender, EventArgs e)
        {
            this.setRGB(0, 104, 232);
        }

        //Leave

        private void txt_incomeValue_Leave(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(this.txt_incomeValue.Text)) && (!this.txt_incomeValue.Focused)) this.txt_incomeValue.PlaceholderText = "R$ 0,00";
        }

        private void mtb_incomeDate_Leave(object sender, EventArgs e)
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
