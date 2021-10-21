using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class AccountSettings : Form
    {
        //DECLARAÇÃO DE VARÍAVEIS
        DataTable accountDataTable = null;

        //INICIA INSTÂNCIA DO PAINEL
        public AccountSettings(ConfigForm configForm)
        {
            InitializeComponent();
            Globals.configForm = configForm;
            Globals.configForm.lbl_btnGoBackTag.Visible = false;
            Globals.configForm.pcb_btnGoBack.Visible = false;
            Globals.configForm.lbl_btnGoForwardTag.Visible = true;
            Globals.configForm.pcb_btnGoForward.Visible = true;

            this.updateActiveAccount();
        }

        //FUNÇÕES

        //Função que retorna os campos do formulário ao estado inicial
        private void clearFields()
        {
            this.txt_accountBalance.Clear();
            this.txt_accountName.Clear();
            this.cbb_accountType.SelectedIndex = -1;
            this.cbb_accountType.Text = " Tipo da conta";
            this.ckb_accountSumBalance.Checked = false;
        }

        //Função que atualiza o comboBox de conta ativa
        private void updateActiveAccount()
        {
            //Atualiza o combobox de conta ativa
            this.cbb_selectedAccount.Items.Clear();
            accountDataTable = Database.query("SELECT conta.nomeConta FROM conta ORDER BY conta.nomeConta;");
            for (int i = 0; i < accountDataTable.Rows.Count; i++) this.cbb_selectedAccount.Items.Insert(i, " " + accountDataTable.Rows[i].ItemArray[0].ToString());
        }

        //Função que cadastra conta
        private void accountRegister()
        {
            if (String.IsNullOrEmpty(txt_accountBalance.Text.Trim()) || String.IsNullOrEmpty(txt_accountName.Text.Trim()) || (this.cbb_accountType.SelectedIndex == -1)) MessageBox.Show("Preencha todos os campos!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if ((Database.query("SELECT nomeConta FROM conta WHERE nomeConta = '" + this.txt_accountName.Text.Trim() + "';")).Rows.Count > 0) MessageBox.Show("Já existe uma conta cadastrada com esse nome!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    Account account = new Account();
                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                    string saldoConta = txt_accountBalance.Text.Trim();
                    if (regexValor.IsMatch(saldoConta))
                    {
                        if (saldoConta.Contains("R$ ")) account.SaldoConta = Convert.ToDecimal(saldoConta.Substring(3).Trim());
                        else if (saldoConta.Contains("R$")) account.SaldoConta = Convert.ToDecimal(saldoConta.Substring(2).Trim());
                        else account.SaldoConta = Convert.ToDecimal(txt_accountBalance.Text.Trim());
                    }
                    else
                    {
                        MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.txt_accountBalance.Clear();
                        this.txt_accountBalance.Focus();
                        return;
                    }
                    account.NomeConta = txt_accountName.Text.Trim();
                    account.TipoConta = cbb_accountType.SelectedItem.ToString().Trim();
                    if (ckb_accountSumBalance.Checked) account.SomarTotal = true; else account.SomarTotal = false;
                    if (Database.newAccount(account))
                    {
                        MessageBox.Show("Conta cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.updateActiveAccount();
                        this.clearFields();
                    }
                    else MessageBox.Show("[ERRO] Não foi possível cadastrar conta!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Função que atualiza conta
        private void accountUpdate()
        {
            if (this.cbb_selectedAccount.SelectedIndex == -1) MessageBox.Show("Selecione uma conta para atualizar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (String.IsNullOrEmpty(txt_accountBalance.Text.Trim()) || String.IsNullOrEmpty(txt_accountName.Text.Trim()) || (this.cbb_accountType.SelectedIndex == -1)) MessageBox.Show("Preencha todos os campos!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    if(((this.cbb_selectedAccount.Text.Trim()) != (this.txt_accountName.Text.Trim())) && ((Database.query("SELECT nomeConta FROM conta WHERE nomeConta = '" + this.txt_accountName.Text.Trim() + "';")).Rows.Count > 0))
                    {
                        MessageBox.Show("Já existe uma conta cadastrada com esse nome!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    Account account = new Account();
                    account.IdConta = Globals.idConta;
                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                    string saldoConta = txt_accountBalance.Text.Trim();
                    if (regexValor.IsMatch(saldoConta))
                    {
                        if (saldoConta.Contains("R$ ")) account.SaldoConta = Convert.ToDecimal(saldoConta.Substring(3).Trim());
                        else if (saldoConta.Contains("R$")) account.SaldoConta = Convert.ToDecimal(saldoConta.Substring(2).Trim());
                        else account.SaldoConta = Convert.ToDecimal(txt_accountBalance.Text.Trim());
                    }
                    else
                    {
                        MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.txt_accountBalance.Clear();
                        return;
                    }
                    account.NomeConta = txt_accountName.Text.Trim();
                    account.TipoConta = cbb_accountType.SelectedItem.ToString().Trim();
                    if (ckb_accountSumBalance.Checked) account.SomarTotal = true; else account.SomarTotal = false;
                    if (Database.updateAccount(account))
                    {
                        MessageBox.Show("Conta atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.updateActiveAccount();
                        this.txt_accountBalance.Focus();
                    }
                    else MessageBox.Show("[ERRO] Não foi possível atualizar conta!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Função que exclui conta
        private void accountDelete()
        {
            if(this.txt_accountName.Text.Trim() == "Carteira")
            {
                MessageBox.Show("Não é possível excluir esta conta!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (((DialogResult)MessageBox.Show("Tem certeza que deseja excluir a conta?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)).ToString().ToUpper() == "NO") return;
            if (this.cbb_selectedAccount.SelectedIndex == -1) MessageBox.Show("Selecione uma conta para excluir!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Account account = new Account();
                account.IdConta = Globals.idConta;
                if (Database.deleteAccount(account))
                {
                    MessageBox.Show("Conta excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.updateActiveAccount();
                    this.clearFields();
                }
                else MessageBox.Show("[ERRO] Não foi possível excluir conta!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //CADASTRAR CONTA
        private void pcb_btnAccountRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnAccountRegister.BackgroundImage = Properties.Resources.btn_add_account_active;
        }

        private void pcb_btnAccountRegister_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnAccountRegisterTag.ClientRectangle.Contains(lbl_btnAccountRegisterTag.PointToClient(Cursor.Position))) this.pcb_btnAccountRegister.BackgroundImage = Properties.Resources.btn_add_account;
        }

        private void pcb_btnAccountRegister_Click(object sender, EventArgs e)
        {
            this.accountRegister();
        }

        private void lbl_btnAccountRegisterTag_Click(object sender, EventArgs e)
        {
            this.accountRegister();
        }

        //ATUALIZAR CONTA
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
            this.accountUpdate();
        }

        private void lbl_btnUpdateTag_Click(object sender, EventArgs e)
        {
            this.accountUpdate();
        }

        //EXCLUIR CONTA
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
            this.accountDelete();
        }

        private void lbl_btnDeleteTag_Click(object sender, EventArgs e)
        {
            this.accountDelete();
        }

        //SELECIONAR CONTA QUE SERÁ EDITADA
        private void cbb_activeAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            accountDataTable = Database.query("SELECT * FROM conta WHERE conta.nomeConta = '" + this.cbb_selectedAccount.SelectedItem.ToString().Trim() + "';");
            foreach (DataRow dataRow in accountDataTable.Rows)
            {
                Globals.idConta = Convert.ToInt32(dataRow.ItemArray[0]);
                this.txt_accountBalance.Text = Convert.ToDecimal(dataRow.ItemArray[1]).ToString("C").Trim();
                this.txt_accountName.Text = dataRow.ItemArray[2].ToString().Trim();
                this.cbb_accountType.SelectedIndex = this.cbb_accountType.FindString(" " + dataRow.ItemArray[3].ToString().Trim());
                if (Convert.ToBoolean(dataRow.ItemArray[4])) this.ckb_accountSumBalance.Checked = true;
                else this.ckb_accountSumBalance.Checked = false;
            }
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO

        //KeyPress
        private void txt_accountBalance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ',') && (e.KeyChar != 'R') && (e.KeyChar != '$') && (e.KeyChar != ' ')) { e.Handled = true; }
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1)) e.Handled = true;
        }

        private void txt_accountName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == (char)Keys.Delete) || (e.KeyChar == (char)Keys.Space))) e.Handled = true;
        }
    }
}
