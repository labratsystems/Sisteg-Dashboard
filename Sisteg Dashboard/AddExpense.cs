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

        //INICIA INSTÂNCIA DO PAINEL CONJUNTO À UM DATABASE DE DESPESAS PARA ATUALIZAÇÃO E EXCLUSÃO, QUE PODE SER NULO EM CASO DE CADASTRO
        public AddExpense(DataTable dataTable)
        {
            InitializeComponent();
            dataTableExpense = dataTable;
            this.txt_expenseValue.Focus();
            this.txt_expenseValue.Select();
            if (dataTableExpense != null)
            {
                foreach (DataRow dataRow in dataTableExpense.Rows)
                {
                    idDespesa = Convert.ToInt32(dataRow.ItemArray[0]);

                    string money = String.Format("{0:C}", dataRow.ItemArray[2]);
                    this.txt_expenseValue.Text = String.Format("{0:C}", dataRow.ItemArray[2]);
                    this.txt_expenseDescription.Text = dataRow.ItemArray[3].ToString();
                    this.dtp_expenseDate.Value = Convert.ToDateTime(dataRow.ItemArray[4].ToString());

                    this.txt_expenseObservations.Text = dataRow.ItemArray[6].ToString();

                    if (Convert.ToBoolean(dataRow.ItemArray[7]) == true) { this.ckb_expenseReceived.Checked = true; } else { this.ckb_expenseReceived.Checked = false; }
                    if (Convert.ToBoolean(dataRow.ItemArray[8]) == true)
                    {
                        this.ckb_repeatOrParcelValue.Checked = true;
                        int indexPeriod = cbb_period.FindString(dataRow.ItemArray[13].ToString());
                        this.cbb_period.SelectedIndex = indexPeriod;
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

                    if (dataRow.ItemArray[9].ToString() != "")
                    {
                        if (Convert.ToBoolean(dataRow.ItemArray[9]) == true)
                        {
                            this.rbtn_fixedValue.Show();
                            this.lbl_fixedValue.Show();
                            this.rbtn_fixedValue.Checked = true;
                            this.txt_repeatsOrParcels.Show();
                            this.txt_repeatsOrParcels.Text = dataRow.ItemArray[10].ToString();
                        }
                        else
                        {
                            this.rbtn_parcelValue.Checked = false;
                        }
                    }
                    else
                    {
                        this.rbtn_fixedValue.Checked = false;
                    }

                    if (dataRow.ItemArray[11].ToString() != "")
                    {
                        if (Convert.ToBoolean(dataRow.ItemArray[11]) == true)
                        {
                            this.rbtn_parcelValue.Show();
                            this.lbl_parcelValue.Show();
                            this.rbtn_parcelValue.Checked = true;
                            this.txt_repeatsOrParcels.Show();
                            this.txt_repeatsOrParcels.Text = dataRow.ItemArray[12].ToString();
                        }
                        else
                        {
                            this.rbtn_parcelValue.Checked = false;
                        }
                    }
                    else
                    {
                        this.rbtn_parcelValue.Checked = false;
                    }
                }
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
            Expense expense = new Expense();
            expense.idConta = 1;
            Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
            string valorDespesa = txt_expenseValue.Text;
            if (regexValor.IsMatch(valorDespesa))
            {
                if (valorDespesa.Contains("R$ ")) { expense.valorDespesa = Convert.ToDecimal(valorDespesa.Substring(3)); }
                else if (valorDespesa.Contains("R$")) { expense.valorDespesa = Convert.ToDecimal(valorDespesa.Substring(2)); } 
                else { expense.valorDespesa = Convert.ToDecimal(txt_expenseValue.Text); }
            }
            else
            {
                MessageBox.Show("Formato monetário incorreto!");
            }
            expense.descricaoDespesa = txt_expenseDescription.Text;
            expense.dataTransacao = Convert.ToDateTime(dtp_expenseDate.Text);
            expense.categoriaDespesa = cbb_expenseCategory.SelectedValue.ToString();
            expense.observacoesDespesa = txt_expenseObservations.Text;
            if (ckb_expenseReceived.Checked) { expense.pagamentoConfirmado = true; } else { expense.pagamentoConfirmado = false; }
            if (ckb_repeatOrParcelValue.Checked) { expense.repetirParcelarDespesa = true; } else { expense.repetirParcelarDespesa = false; }

            if (rbtn_fixedValue.Checked)
            {
                expense.valorFixoDespesa = true;
                expense.repeticoesValorFixoDespesa = Convert.ToInt32(txt_repeatsOrParcels.Text);
            }
            else
            {
                expense.valorFixoDespesa = false;
            }

            if (rbtn_parcelValue.Checked)
            {
                expense.parcelarValorDespesa = true;
                expense.parcelasDespesa = Convert.ToInt32(txt_repeatsOrParcels.Text);
            }
            else
            {
                expense.parcelarValorDespesa = false;
            }

            expense.periodoRepetirParcelarDespesa = cbb_period.SelectedValue.ToString();

            if (Database.newExpense(expense))
            {
                MessageBox.Show("Despesa cadastrada com suceso!");
                this.txt_expenseValue.Clear();
                this.txt_expenseDescription.Clear();
                this.dtp_expenseDate.Text = DateTime.Today.ToString();
                this.txt_expenseObservations.Clear();
                this.ckb_repeatOrParcelValue.Checked = false;
                this.rbtn_fixedValue.Hide();
                this.lbl_fixedValue.Hide();
                this.rbtn_fixedValue.Checked = false;
                this.rbtn_parcelValue.Hide();
                this.lbl_parcelValue.Hide();
                this.rbtn_parcelValue.Checked = false;
                this.txt_repeatsOrParcels.Hide();
                int indexPeriod = this.cbb_period.FindString("Mensal");
                this.cbb_period.SelectedIndex = indexPeriod;
                this.cbb_period.Hide();
                this.txt_expenseValue.Focus();
                this.txt_expenseValue.Select();
            }
            else
            {
                MessageBox.Show("Não foi possível cadastrar despesa!");
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
            Expense expense = new Expense();
            expense.idDespesa = idDespesa;
            expense.idConta = 1;
            Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
            string valorDespesa = txt_expenseValue.Text;
            if (regexValor.IsMatch(valorDespesa))
            {
                if (valorDespesa.Contains("R$ "))
                {
                    expense.valorDespesa = Convert.ToDecimal(valorDespesa.Substring(3));
                }
                else
                {
                    if (valorDespesa.Contains("R$")) { expense.valorDespesa = Convert.ToDecimal(valorDespesa.Substring(2)); } else { expense.valorDespesa = Convert.ToDecimal(txt_expenseValue.Text); }
                }
            }
            else
            {
                MessageBox.Show("Formato monetário incorreto!");
            }
            expense.descricaoDespesa = txt_expenseDescription.Text;
            expense.dataTransacao = Convert.ToDateTime(dtp_expenseDate.Text);
            expense.categoriaDespesa = cbb_expenseCategory.SelectedValue.ToString();
            expense.observacoesDespesa = txt_expenseObservations.Text;
            if (ckb_expenseReceived.Checked) { expense.pagamentoConfirmado = true; } else { expense.pagamentoConfirmado = false; }
            if (ckb_repeatOrParcelValue.Checked) { expense.repetirParcelarDespesa = true; } else { expense.repetirParcelarDespesa = false; }
            if (rbtn_fixedValue.Checked)
            {
                expense.valorFixoDespesa = true;
                expense.repeticoesValorFixoDespesa = Convert.ToInt32(txt_repeatsOrParcels.Text);
            }
            else
            {
                expense.valorFixoDespesa = false;
            }
            if (rbtn_parcelValue.Checked)
            {
                expense.parcelarValorDespesa = true;
                expense.parcelasDespesa = Convert.ToInt32(txt_repeatsOrParcels.Text);
            }
            else
            {
                expense.parcelarValorDespesa = false;
            }
            expense.periodoRepetirParcelarDespesa = cbb_period.SelectedValue.ToString();
            if (Database.updateExpense(expense))
            {
                MessageBox.Show("Despesa atualizada com suceso!");
                txt_expenseValue.Focus();
                this.txt_expenseValue.Select();
            }
            else
            {
                MessageBox.Show("Não foi possível atualizar despesa!");
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
            if (Database.deleteExpense(expense)) { MessageBox.Show("Despesa excluída com suceso!"); } else { MessageBox.Show("Não foi possível excluída despesa!"); }
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
            Application.Exit();
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO
        private void dtp_expenseDate_MouseEnter(object sender, EventArgs e)
        {
            this.dtp_expenseDate.BackColor = Color.FromArgb(0, 104, 232);
        }

        private void dtp_expenseDate_MouseLeave(object sender, EventArgs e)
        {
            this.dtp_expenseDate.BackColor = Color.FromArgb(0, 76, 157);
        }

        private void ckb_repeatOrParcelValue_OnChange(object sender, EventArgs e)
        {
            if (ckb_repeatOrParcelValue.Checked)
            {
                this.panel_expense.AutoScrollMinSize = new Size(425, 520);
                this.rbtn_fixedValue.Show();
                this.rbtn_fixedValue.Checked = true;
                this.lbl_fixedValue.Show();
                this.rbtn_parcelValue.Show();
                this.lbl_parcelValue.Show();
                this.txt_repeatsOrParcels.Show();
                this.cbb_period.Show();
            }
            else
            {
                this.panel_expense.AutoScrollMinSize = new Size(425, 347);
                this.rbtn_fixedValue.Hide();
                this.lbl_fixedValue.Hide();
                this.rbtn_fixedValue.Checked = false;
                this.rbtn_parcelValue.Hide();
                this.lbl_parcelValue.Hide();
                this.rbtn_parcelValue.Checked = false;
                this.txt_repeatsOrParcels.Hide();
                this.cbb_period.Hide();
            }
        }

        private void rbtn_fixedValue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_fixedValue.Checked) { this.txt_repeatsOrParcels.PlaceholderText = "Repetições"; }
        }

        private void rbtn_parcelValue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_parcelValue.Checked) { this.txt_repeatsOrParcels.PlaceholderText = "Parcelas"; }
        }

        private void txt_expenseValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ',') && (e.KeyChar != 'R') && (e.KeyChar != '$') && (e.KeyChar != ' ')) { e.Handled = true; }
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1)) { e.Handled = true; }
        }
    }
}