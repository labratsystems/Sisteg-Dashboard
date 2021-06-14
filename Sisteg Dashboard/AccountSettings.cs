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
    public partial class AccountSettings : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        protected internal ConfigForm accountSettingsConfigForm;
        private static int idConta = 0;

        public AccountSettings(ConfigForm configForm)
        {
            InitializeComponent();
            accountSettingsConfigForm = configForm;
            accountSettingsConfigForm.pcb_btnGoBack.Visible = false;
            accountSettingsConfigForm.pcb_btnGoForward.Visible = true;

            //Popula o combobox de conta ativa
            DataTable accountDataTable = Database.query("SELECT conta.nomeConta FROM conta ORDER BY conta.nomeConta;");
            for (int i = 0; i < accountDataTable.Rows.Count; i++) this.cbb_selectedAccount.Items.Insert(i, " " + accountDataTable.Rows[i].ItemArray[0].ToString());
        }

        //FUNÇÃO QUE RETORNA OS CAMPOS DO FORMULÁRIO AO ESTADO INICIAL
        private void clearFields()
        {
            this.txt_accountBalance.Clear();
            this.txt_accountName.Clear();
            this.cbb_accountType.SelectedIndex = -1;
            this.cbb_accountType.Text = " Tipo da conta";
            this.ckb_accountSumBalance.Checked = false;
        }

        //ADICIONAR CONTA
        private void pcb_btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txt_accountBalance.Text.Trim()) || String.IsNullOrEmpty(txt_accountName.Text.Trim()) || (this.cbb_accountType.SelectedIndex == -1)) MessageBox.Show("Preencha todos os campos!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Account account = new Account();
                Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                string saldoConta = txt_accountBalance.Text;
                if (regexValor.IsMatch(saldoConta))
                {
                    if (saldoConta.Contains("R$ ")) account.saldoConta = Convert.ToDecimal(saldoConta.Substring(3));
                    else if (saldoConta.Contains("R$")) account.saldoConta = Convert.ToDecimal(saldoConta.Substring(2));
                    else account.saldoConta = Convert.ToDecimal(txt_accountBalance.Text);
                }
                else
                {
                    MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.txt_accountBalance.Clear();
                    this.txt_accountBalance.PlaceholderText = "";
                    this.txt_accountBalance.Focus();
                    return;
                }
                account.nomeConta = txt_accountName.Text.Trim();
                account.tipoConta = cbb_accountType.SelectedItem.ToString().Trim();
                if (ckb_accountSumBalance.Checked) account.somarTotal = true; else account.somarTotal = false;
                if (Database.newAccount(account))
                {
                    MessageBox.Show("Conta cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.clearFields();
                }
                else MessageBox.Show("Não foi possível cadastrar conta!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbb_activeAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable accountDataTable = Database.query("SELECT * FROM conta WHERE conta.nomeConta = '" + this.cbb_selectedAccount.SelectedItem.ToString().Trim() + "';");
            foreach(DataRow dataRow in accountDataTable.Rows)
            {
                idConta = Convert.ToInt32(dataRow.ItemArray[0]);
                this.txt_accountBalance.Text = Convert.ToDecimal(dataRow.ItemArray[1]).ToString("C");
                this.txt_accountName.Text = dataRow.ItemArray[2].ToString();
                this.cbb_accountType.SelectedIndex = this.cbb_accountType.FindString(" " + dataRow.ItemArray[3].ToString());
                if (Convert.ToBoolean(dataRow.ItemArray[4])) this.ckb_accountSumBalance.Checked = true; 
                else this.ckb_accountSumBalance.Checked = false;
            }
        }

        //ATUALIZAR CONTA
        private void pcb_btnUpdateAccount_Click(object sender, EventArgs e)
        {
            if(this.cbb_selectedAccount.SelectedIndex == -1) MessageBox.Show("Selecione uma conta para atualizar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (String.IsNullOrEmpty(txt_accountBalance.Text.Trim()) || String.IsNullOrEmpty(txt_accountName.Text.Trim()) || (this.cbb_accountType.SelectedIndex == -1)) MessageBox.Show("Preencha todos os campos!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    Account account = new Account();
                    account.idConta = idConta;
                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                    string saldoConta = txt_accountBalance.Text;
                    if (regexValor.IsMatch(saldoConta))
                    {
                        if (saldoConta.Contains("R$ ")) account.saldoConta = Convert.ToDecimal(saldoConta.Substring(3));
                        else if (saldoConta.Contains("R$")) account.saldoConta = Convert.ToDecimal(saldoConta.Substring(2));
                        else account.saldoConta = Convert.ToDecimal(txt_accountBalance.Text);
                    }
                    else
                    {
                        MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.txt_accountBalance.Clear();
                        this.txt_accountBalance.PlaceholderText = "";
                        this.txt_accountBalance.Focus();
                        return;
                    }
                    account.nomeConta = txt_accountName.Text.Trim();
                    account.tipoConta = cbb_accountType.SelectedItem.ToString().Trim();
                    if (ckb_accountSumBalance.Checked) account.somarTotal = true; else account.somarTotal = false;
                    if (Database.updateAccount(account))
                    {
                        MessageBox.Show("Conta atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.clearFields();
                    }
                    else MessageBox.Show("Não foi possível atualizar conta!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pcb_btnDeleteAccount_Click(object sender, EventArgs e)
        {
            if (this.cbb_selectedAccount.SelectedIndex == -1) MessageBox.Show("Selecione uma conta para atualizar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Account account = new Account();
                account.idConta = idConta;
                if (Database.deleteAccount(account))
                {
                    MessageBox.Show("Conta excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.clearFields();
                }
                else MessageBox.Show("Não foi possível excluir conta!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pcb_btnAdd_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnAdd.Image = Properties.Resources.btn_add_active;
        }

        private void pcb_btnAdd_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnAdd.Image = Properties.Resources.btn_add;
        }

        private void pcb_btnUpdateAccount_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnUpdateAccount.Image = Properties.Resources.btn_update_active;
        }

        private void pcb_btnUpdateAccount_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnUpdateAccount.Image = Properties.Resources.btn_update;
        }

        private void pcb_btnDeleteAccount_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnDeleteAccount.Image = Properties.Resources.btn_delete_active;
        }

        private void pcb_btnDeleteAccount_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnDeleteAccount.Image = Properties.Resources.btn_delete;
        }
    }
}
