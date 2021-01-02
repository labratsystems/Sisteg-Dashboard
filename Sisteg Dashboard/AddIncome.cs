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
        private static DataTable dataTableIncome;
        private static int idReceita;
        public AddIncome(DataTable dataTable)
        {
            InitializeComponent();
            dataTableIncome = dataTable;
            this.txt_incomeValue.Focus();
            this.txt_incomeValue.Select();
            if (dataTableIncome != null)
            {
                foreach (DataRow dataRow in dataTableIncome.Rows)
                {
                    idReceita = Convert.ToInt32(dataRow.ItemArray[0]);

                    string money = String.Format("{0:C}", dataRow.ItemArray[2]);
                    this.txt_incomeValue.Text = String.Format("{0:C}", dataRow.ItemArray[2]);
                    this.txt_incomeDescription.Text = dataRow.ItemArray[3].ToString();
                    this.dtp_incomeDate.Value = Convert.ToDateTime(dataRow.ItemArray[4].ToString());

                    this.txt_incomeObservations.Text = dataRow.ItemArray[6].ToString();

                    if (Convert.ToBoolean(dataRow.ItemArray[7]) == true)
                    {
                        this.ckb_incomeReceived.Checked = true;
                    }
                    else
                    {
                        this.ckb_incomeReceived.Checked = false;
                    }

                    if (Convert.ToBoolean(dataRow.ItemArray[8]) == true)
                    {
                        this.ckb_repeatOrParcelValue.Checked = true;
                        int selected = -1;
                        int count = this.cbb_period.Items.Count;
                        for(int i = 0; (i <= (count -1)); i++)
                        {
                            this.cbb_period.selectedIndex = i;
                            if((string)(this.cbb_period.selectedValue) == dataRow.ItemArray[13].ToString())
                            {
                                selected = i;
                                break;
                            }
                        }
                        this.cbb_period.selectedIndex = selected;
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

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handlerparam = base.CreateParams;
                handlerparam.ExStyle |= 0x02000000;
                return handlerparam;
            }
        }

        private void pcb_incomeRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_incomeRegister.Image = Properties.Resources.btn_incomeRegister_active;
        }

        private void pcb_incomeRegister_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_incomeRegister.Image = Properties.Resources.btn_incomeRegister;
        }

        private void pcb_incomeRegister_Click(object sender, EventArgs e)
        {
            Income income = new Income();
            income.idConta = 1;
            Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
            string valorReceita = txt_incomeValue.Text;
            if (regexValor.IsMatch(valorReceita))
            {
                if (valorReceita.Contains("R$ "))
                {
                    income.valorReceita = Convert.ToDecimal(valorReceita.Substring(3));
                }
                else
                {
                    if (valorReceita.Contains("R$"))
                    {
                        income.valorReceita = Convert.ToDecimal(valorReceita.Substring(2));
                    }
                    else
                    {
                        income.valorReceita = Convert.ToDecimal(txt_incomeValue.Text);
                    }
                }
            }
            else
            {
                MessageBox.Show("Formato monetário incorreto!");
            }
            income.descricaoReceita = txt_incomeDescription.Text;
            income.dataTransacao = Convert.ToDateTime(dtp_incomeDate.Text);
            income.categoriaReceita = cbb_incomeCategory.selectedValue.ToString();
            income.observacoesReceita = txt_incomeObservations.Text;
            if (ckb_incomeReceived.Checked)
            {
                income.recebimentoConfirmado = true;
            }
            else
            {
                income.recebimentoConfirmado = false;
            }

            if (ckb_repeatOrParcelValue.Checked)
            {
                income.repetirParcelarReceita = true;
            }
            else
            {
                income.repetirParcelarReceita = false;
            }

            if (rbtn_fixedValue.Checked)
            {
                income.valorFixoReceita = true;
                income.repeticoesValorFixoReceita = Convert.ToInt32(txt_repeatsOrParcels.Text);
            }
            else
            {
                income.valorFixoReceita = false;
            }

            if (rbtn_parcelValue.Checked)
            {
                income.parcelarValorReceita = true;
                income.parcelasReceita = Convert.ToInt32(txt_repeatsOrParcels.Text);
            }
            else
            {
                income.parcelarValorReceita = false;
            }

            income.periodoRepetirParcelarReceita = cbb_period.selectedValue.ToString();

            if (Database.newIncome(income))
            {
                MessageBox.Show("Receita cadastrada com suceso!");
                this.txt_incomeValue.Clear();
                this.txt_incomeDescription.Clear();
                this.dtp_incomeDate.Text = DateTime.Today.ToString();
                this.txt_incomeObservations.Clear();
                this.ckb_repeatOrParcelValue.Checked = false;
                this.rbtn_fixedValue.Hide();
                this.lbl_fixedValue.Hide();
                this.rbtn_fixedValue.Checked = false;
                this.rbtn_parcelValue.Hide();
                this.lbl_parcelValue.Hide();
                this.rbtn_parcelValue.Checked = false;
                this.txt_repeatsOrParcels.Hide();
                int selected = -1;
                int count = this.cbb_period.Items.Count;
                for (int i = 0; (i <= (count - 1)); i++)
                {
                    this.cbb_period.selectedIndex = i;
                    if ((string)(this.cbb_period.selectedValue) == "Mensal")
                    {
                        selected = i;
                        break;
                    }
                }
                this.cbb_period.selectedIndex = selected;
                this.cbb_period.Hide();
                this.txt_incomeValue.Focus();
                this.txt_incomeValue.Select();
            }
            else
            {
                MessageBox.Show("Não foi possível cadastrar receita!");
            }
        }

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
            Income income = new Income();
            income.idReceita = idReceita;
            income.idConta = 1;
            Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
            string valorReceita = txt_incomeValue.Text;
            if (regexValor.IsMatch(valorReceita))
            {
                if (valorReceita.Contains("R$ "))
                {
                    income.valorReceita = Convert.ToDecimal(valorReceita.Substring(3));
                }
                else
                {
                    if (valorReceita.Contains("R$"))
                    {
                        income.valorReceita = Convert.ToDecimal(valorReceita.Substring(2));
                    }
                    else
                    {
                        income.valorReceita = Convert.ToDecimal(txt_incomeValue.Text);
                    }
                }
            }
            else
            {
                MessageBox.Show("Formato monetário incorreto!");
            }
            income.descricaoReceita = txt_incomeDescription.Text;
            income.dataTransacao = Convert.ToDateTime(dtp_incomeDate.Text);
            income.categoriaReceita = cbb_incomeCategory.selectedValue.ToString();
            income.observacoesReceita = txt_incomeObservations.Text;
            if (ckb_incomeReceived.Checked)
            {
                income.recebimentoConfirmado = true;
            }
            else
            {
                income.recebimentoConfirmado = false;
            }

            if (ckb_repeatOrParcelValue.Checked)
            {
                income.repetirParcelarReceita = true;
            }
            else
            {
                income.repetirParcelarReceita = false;
            }

            if (rbtn_fixedValue.Checked)
            {
                income.valorFixoReceita = true;
                income.repeticoesValorFixoReceita = Convert.ToInt32(txt_repeatsOrParcels.Text);
            }
            else
            {
                income.valorFixoReceita = false;
            }

            if (rbtn_parcelValue.Checked)
            {
                income.parcelarValorReceita = true;
                income.parcelasReceita = Convert.ToInt32(txt_repeatsOrParcels.Text);
            }
            else
            {
                income.parcelarValorReceita = false;
            }

            income.periodoRepetirParcelarReceita = cbb_period.selectedValue.ToString();

            if (Database.updateIncome(income))
            {
                MessageBox.Show("Receita atualizada com suceso!");
                txt_incomeValue.Focus();
                this.txt_incomeValue.Select();
            }
            else
            {
                MessageBox.Show("Não foi possível atualizar receita!");
            }
        }

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
            if (Database.deleteIncome(income))
            {
                MessageBox.Show("Receita excluída com suceso!");
            }
            else
            {
                MessageBox.Show("Não foi possível excluída receita!");
            }
            if (Application.OpenForms.OfType<Main>().Count() == 0)
            {
                Main main = new Main();
                main.Show();
                this.Close();
            }
        }

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

        private void pcb_appClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dtp_incomeDate_MouseEnter(object sender, EventArgs e)
        {
            this.dtp_incomeDate.BackColor = Color.FromArgb(0, 104, 232);
        }

        private void dtp_incomeDate_MouseLeave(object sender, EventArgs e)
        {
            this.dtp_incomeDate.BackColor = Color.FromArgb(0, 76, 157);
        }

        private void ckb_repeatOrParcelValue_OnChange(object sender, EventArgs e)
        {
            if (ckb_repeatOrParcelValue.Checked)
            {
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
            if (rbtn_fixedValue.Checked)
            {
                this.txt_repeatsOrParcels.PlaceholderText = "Repetições";
            }
        }

        private void rbtn_parcelValue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_parcelValue.Checked)
            {
                this.txt_repeatsOrParcels.PlaceholderText = "Parcelas";
            }
        }

        private void txt_incomeValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ',') && (e.KeyChar != 'R') && (e.KeyChar != '$') && (e.KeyChar != ' '))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }
    }
}
