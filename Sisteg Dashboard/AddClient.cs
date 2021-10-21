using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class AddClient : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        public bool editClick = false;

        //CARREGA INSTÂNCIA DO PAINEL CONJUNTO À UM DATATABLE DE CLIENTE PARA ATUALIZAÇÃO OU EXCLUSÃO, QUE PODE SER NULO EM CASO DE CADASTRO
        public AddClient(DataTable dataTable)
        {
            InitializeComponent();
            Globals.clientDataTable = dataTable;
            this.txt_clientName.Focus();
            
            if (Globals.clientDataTable != null)
            {
                foreach (DataRow dataRowClient in Globals.clientDataTable.Rows)
                {
                    if (Globals.clientDataTable.Rows.IndexOf(dataRowClient) == 0)
                    {
                        Globals.idCliente = Convert.ToInt32(dataRowClient.ItemArray[0]);
                        this.txt_clientName.Text = dataRowClient.ItemArray[1].ToString().Trim();
                        this.txt_clientAddress.Text = dataRowClient.ItemArray[2].ToString().Trim();
                        this.txt_clientHouseNumber.Text = dataRowClient.ItemArray[3].ToString().Trim();
                        this.txt_clientNeighbourhood.Text = dataRowClient.ItemArray[4].ToString().Trim();
                        this.txt_clientCity.Text = dataRowClient.ItemArray[5].ToString().Trim();
                        this.cbb_clientState.SelectedIndex = cbb_clientState.FindString(" " + dataRowClient.ItemArray[6].ToString().Trim());
                        this.txt_clientEmail.Text = dataRowClient.ItemArray[7].ToString().Trim();
                        //Telefone
                        this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idCliente = " + Globals.idCliente);
                        if (this.dgv_telephones.Rows.Count == 0) this.displayTelephoneSettings(false, 204);
                    }
                }
            } 
            else
            {
                Globals.idCliente = 0;
                this.displayTelephoneSettings(false, 204);
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

        //FUNÇÕES

        //Função que retorna os campos do formulário ao estado inicial
        private void clearFields()
        {
            MessageBox.Show("Cliente cadastrado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.txt_clientName.Clear();
            this.txt_clientAddress.Clear();
            this.txt_clientHouseNumber.Clear();
            this.txt_clientNeighbourhood.Clear();
            this.txt_clientCity.Clear();
            this.cbb_clientState.SelectedIndex = -1;
            this.cbb_clientState.Text = " Estado";
            this.txt_clientName.PlaceholderText = "";
            this.txt_clientName.Focus();
        }

        //Função que cadastra novo telefone
        private void newTelephone()
        {
            string mtb = this.mtb_telephoneNumber.Text.Trim();
            if ((this.cbb_telephoneType.SelectedIndex == -1) || ((this.mtb_telephoneNumber.Text.IndexOf(' ')) != (this.mtb_telephoneNumber.Text.LastIndexOf(' '))) || (mtb.Trim().Length < 10)) MessageBox.Show("Selecione um telefone para atualizar ou preencha os dados corretamente!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Telephone telephone = new Telephone();
                telephone.IdCliente = Globals.idCliente;
                telephone.TipoTelefone = this.cbb_telephoneType.SelectedItem.ToString().Trim();
                telephone.NumeroTelefone = this.mtb_telephoneNumber.Text.Trim();
                if (Database.newTelephone(telephone))
                {
                    MessageBox.Show("Telefone cadastrado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idCliente = " + Globals.idCliente);
                    this.displayTelephoneSettings(true, 518);
                }
                else MessageBox.Show("[ERRO] Não foi possível cadastrar telefone!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Função que disponibiliza os métodos associados à classe telefone
        private void displayTelephoneSettings(bool value, int height)
        {
            this.dgv_telephones.Visible = value;
            this.bsp_dgvTelephones.Visible = value;
            this.pcb_btnUpdateTelephone.Visible = value;
            this.pcb_btnDeleteTelephone.Visible = value;
            this.bsp_telephones.Height = height;
        }

        //Retorna ao formulário de clientes
        private void goToClientForm()
        {
            if (Application.OpenForms.OfType<ClientForm>().Count() == 0)
            {
                this.editClick = false;
                ClientForm clientForm = new ClientForm();
                clientForm.Show();
                this.Close();
            }
        }

        //Função que remove as colunas vazias da tabela
        private void dataTableRemoveEmptyColumns(string query)
        {
            Globals.telephoneDataTable = Database.query(query);
            for (int col = Globals.telephoneDataTable.Columns.Count - 1; col >= 0; col--)
            {
                bool removeColumn = true;
                foreach (DataRow dataRow in Globals.telephoneDataTable.Rows)
                {
                    if (!dataRow.IsNull(col))
                    {
                        removeColumn = false;
                        break;
                    }
                }
                if (removeColumn) Globals.telephoneDataTable.Columns.RemoveAt(col);
            }
            this.dgv_telephones.DataSource = Globals.telephoneDataTable;
        }

        //Função que cadastra cliente
        private void clientRegister()
        {
            if (String.IsNullOrEmpty(txt_clientName.Text.Trim()) || String.IsNullOrEmpty(txt_clientAddress.Text.Trim()) || String.IsNullOrEmpty(txt_clientHouseNumber.Text.Trim()) || String.IsNullOrEmpty(txt_clientNeighbourhood.Text.Trim()) || String.IsNullOrEmpty(txt_clientCity.Text.Trim()) || (this.cbb_clientState.SelectedIndex == -1)) MessageBox.Show("Preencha todos os campos obrigatórios para cadastrar cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (Globals.idCliente == 0)
                {
                    if ((Database.query("SELECT nomeCliente FROM cliente WHERE nomeCliente = '" + this.txt_clientName.Text.Trim() + "';")).Rows.Count > 0) MessageBox.Show("Já existe um cliente cadastrado com esse nome!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                    {
                        Client client = new Client();
                        client.NomeCliente = txt_clientName.Text.Trim();
                        client.EnderecoCliente = txt_clientAddress.Text.Trim();
                        client.NumeroResidencia = txt_clientHouseNumber.Text.Trim();
                        client.BairroCliente = txt_clientNeighbourhood.Text.Trim();
                        client.CidadeCliente = txt_clientCity.Text.Trim();
                        client.EstadoCliente = cbb_clientState.SelectedItem.ToString().Trim();
                        client.EmailCliente = txt_clientEmail.Text.Trim();
                        if (Database.newClient(client)) this.clearFields();
                        else MessageBox.Show("[ERRO] Não foi possível cadastrar cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    this.clearFields();
                    Globals.idCliente = 0;
                }
            }
        }

        //Função que atualiza cliente
        private void clientUpdate()
        {
            if (String.IsNullOrEmpty(txt_clientName.Text.Trim()) || String.IsNullOrEmpty(txt_clientAddress.Text.Trim()) || String.IsNullOrEmpty(txt_clientHouseNumber.Text.Trim()) || String.IsNullOrEmpty(txt_clientNeighbourhood.Text.Trim()) || String.IsNullOrEmpty(txt_clientCity.Text.Trim()) || (this.cbb_clientState.SelectedIndex == -1)) MessageBox.Show("Preencha todos os campos obrigatórios para atualizar cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Client client = new Client();
                client.IdCliente = Globals.idCliente;
                client.NomeCliente = txt_clientName.Text.Trim();
                client.EnderecoCliente = txt_clientAddress.Text.Trim();
                client.NumeroResidencia = txt_clientHouseNumber.Text.Trim();
                client.BairroCliente = txt_clientNeighbourhood.Text.Trim();
                client.CidadeCliente = txt_clientCity.Text.Trim();
                client.EstadoCliente = cbb_clientState.SelectedItem.ToString().Trim();
                client.EmailCliente = txt_clientEmail.Text.Trim();
                if (Database.updateClient(client))
                {
                    MessageBox.Show("Cliente atualizado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txt_clientName.PlaceholderText = "";
                    this.txt_clientName.Focus();
                }
                else MessageBox.Show("[ERRO] Não foi possível atualizar cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Função que exclui cliente
        private void clientDelete()
        {
            Client client = new Client();
            client.IdCliente = Globals.idCliente;
            DataTable budgetsDataTable = Database.query("SELECT * FROM orcamento WHERE idCliente = " + Globals.idCliente);
            if (budgetsDataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in budgetsDataTable.Rows)
                {
                    if (Database.updateBudgetNumber(client))
                    {
                        if (Database.deleteAllBudgets(client))
                        {
                            if (Database.deleteAllTelephones(client))
                            {
                                if (Database.deleteClient(client))
                                {
                                    MessageBox.Show("Cliente excluido com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.goToClientForm();
                                }
                                else MessageBox.Show("[ERRO] Não foi possível excluir cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else MessageBox.Show("[ERRO] Não foi possível excluir cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else MessageBox.Show("[ERRO] Não foi possível excluir cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show("[ERRO] Não foi possível excluir cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (Database.deleteAllTelephones(client))
                {
                    if (Database.deleteClient(client))
                    {
                        MessageBox.Show("Cliente excluido com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.goToClientForm();
                    }
                    else MessageBox.Show("[ERRO] Não foi possível excluir cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else MessageBox.Show("[ERRO] Não foi possível excluir cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Função que cadastra telefone
        private void phoneRegister()
        {
            if (String.IsNullOrEmpty(mtb_telephoneNumber.Text.Trim()) || (this.cbb_telephoneType.SelectedIndex == -1)) MessageBox.Show("Preencha todos os campos para cadastrar telefone!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (Globals.idCliente == 0)
            {
                if ((String.IsNullOrEmpty(this.txt_clientName.Text.Trim())) || (String.IsNullOrEmpty(this.txt_clientAddress.Text.Trim())) || (String.IsNullOrEmpty(this.txt_clientHouseNumber.Text.Trim())) || (String.IsNullOrEmpty(this.txt_clientCity.Text.Trim())) || (this.cbb_clientState.SelectedIndex == -1)) MessageBox.Show("Cadastre o cliente antes de cadastrar seu telefone!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    if ((Database.query("SELECT nomeCliente FROM cliente WHERE nomeCliente = '" + this.txt_clientName.Text.Trim() + "';")).Rows.Count > 0) MessageBox.Show("Já existe um cliente cadastrado com esse nome!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                    {
                        Client client = new Client();
                        client.NomeCliente = txt_clientName.Text.Trim();
                        client.EnderecoCliente = txt_clientAddress.Text.Trim();
                        client.NumeroResidencia = txt_clientHouseNumber.Text.Trim();
                        client.BairroCliente = txt_clientNeighbourhood.Text.Trim();
                        client.CidadeCliente = txt_clientCity.Text.Trim();
                        client.EstadoCliente = cbb_clientState.SelectedItem.ToString().Trim();
                        client.EmailCliente = txt_clientEmail.Text.Trim();
                        if (Database.newClient(client))
                        {
                            Globals.idCliente = Convert.ToInt32(Database.query("SELECT idCliente FROM cliente ORDER BY idCliente DESC LIMIT 1;").Rows[0].ItemArray[0]);
                            this.newTelephone();
                        }
                    }
                }
            }
            else this.newTelephone();
        }

        //Função que atualiza telefone
        private void phoneUpdate()
        {
            if (Convert.ToInt32(this.dgv_telephones.Rows.Count) == 0) MessageBox.Show("Não há telefone selecionado para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Convert.ToInt32(this.dgv_telephones.SelectedRows.Count) > 1) MessageBox.Show("Selecione apenas uma linha da tabela para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                string mtb = this.mtb_telephoneNumber.Text.Trim();
                if ((this.cbb_telephoneType.SelectedIndex == -1) || ((this.mtb_telephoneNumber.Text.IndexOf(' ')) != (this.mtb_telephoneNumber.Text.LastIndexOf(' '))) || (mtb.Trim().Length < 10)) MessageBox.Show("Selecione um telefone para atualizar ou preencha os dados corretamente!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    Telephone telephone = new Telephone();
                    telephone.IdTelefone = Convert.ToInt32(this.dgv_telephones.SelectedRows[0].Cells[0].Value);
                    telephone.TipoTelefone = this.cbb_telephoneType.SelectedItem.ToString().Trim();
                    telephone.NumeroTelefone = this.mtb_telephoneNumber.Text.Trim();
                    if (Database.updateTelephone(telephone))
                    {
                        MessageBox.Show("Telefone atualizado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idCliente = " + Globals.idCliente);
                    }
                    else MessageBox.Show("[ERRO] Não foi possível atualizar telefone!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Função que exclui telefone
        private void phoneDelete()
        {
            if (Convert.ToInt32(this.dgv_telephones.Rows.Count) == 0) MessageBox.Show("Não há telefone selecionado para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Convert.ToInt32(this.dgv_telephones.SelectedRows.Count) > 1) MessageBox.Show("Selecione apenas uma linha da tabela para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Telephone telephone = new Telephone();
                telephone.IdTelefone = Convert.ToInt32(this.dgv_telephones.SelectedRows[0].Cells[0].Value);
                if (Database.deleteTelephone(telephone))
                {
                    MessageBox.Show("Telefone excluído com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.mtb_telephoneNumber.Focus();
                    this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idCliente = " + Globals.idCliente);
                    if (this.dgv_telephones.Rows.Count == 0) this.displayTelephoneSettings(false, 204);
                }
                else MessageBox.Show("[ERRO] Não foi possível excluir telefone!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Função que muda a cor do campo telefone
        private void setRGB(int r, int g, int b)
        {
            this.mtb_telephoneNumber.BackColor = Color.FromArgb(r, g, b);
            this.border_mtbTelephoneNumber.FillColor = Color.FromArgb(r, g, b);
            this.border_mtbTelephoneNumber.OnIdleState.FillColor = Color.FromArgb(r, g, b);
        }

        //CADASTRO DE CLIENTE
        private void pcb_clientRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_clientRegister.BackgroundImage = Properties.Resources.btn_client_form_active;
        }

        private void pcb_clientRegister_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_clientRegisterTag.ClientRectangle.Contains(lbl_clientRegisterTag.PointToClient(Cursor.Position))) this.pcb_clientRegister.BackgroundImage = Properties.Resources.btn_client_form;
        }

        private void pcb_clientRegister_Click(object sender, EventArgs e)
        {
            this.clientRegister();
        }

        private void lbl_clientRegisterTag_Click(object sender, EventArgs e)
        {
            this.clientRegister();
        }

        //ATUALIZAÇÃO DE CLIENTE
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
            this.clientUpdate();
        }

        private void lbl_btnUpdateTag_Click(object sender, EventArgs e)
        {
            this.clientUpdate();
        }

        //EXCLUSÃO DE CLIENTE
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
            this.clientDelete();
        }

        private void lbl_btnDeleteTag_Click(object sender, EventArgs e)
        {
            this.clientDelete();
        }

        //CADASTRO DE TELEFONE
        private void pcb_btnPhoneRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnPhoneRegister.BackgroundImage = Properties.Resources.btn_add_phone_active;
        }

        private void pcb_btnPhoneRegister_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_phoneRegisterTag.ClientRectangle.Contains(lbl_phoneRegisterTag.PointToClient(Cursor.Position))) this.pcb_btnPhoneRegister.BackgroundImage = Properties.Resources.btn_add_phone;
        }

        private void pcb_btnPhoneRegister_Click(object sender, EventArgs e)
        {
            this.phoneRegister();
        }

        private void lbl_phoneRegisterTag_Click(object sender, EventArgs e)
        {
            this.phoneRegister();
        }

        //ATUALIZAR TELEFONE
        private void pcb_btnUpdateTelephone_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnUpdateTelephone.BackgroundImage = Properties.Resources.btn_update_active;
        }

        private void pcb_btnUpdateTelephone_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnUpdatePhoneTag.ClientRectangle.Contains(lbl_btnUpdatePhoneTag.PointToClient(Cursor.Position))) this.pcb_btnUpdateTelephone.BackgroundImage = Properties.Resources.btn_update;
        }

        private void pcb_btnUpdateTelephone_Click(object sender, EventArgs e)
        {
            this.phoneUpdate();
        }

        private void lbl_btnUpdatePhoneTag_Click(object sender, EventArgs e)
        {
            this.phoneUpdate();
        }

        //EXCLUIR TELEFONE
        private void pcb_btnDeleteTelephone_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnDeleteTelephone.BackgroundImage = Properties.Resources.btn_delete_active;
        }

        private void pcb_btnDeleteTelephone_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnDeletePhoneTag.ClientRectangle.Contains(lbl_btnDeletePhoneTag.PointToClient(Cursor.Position))) this.pcb_btnDeleteTelephone.BackgroundImage = Properties.Resources.btn_delete;
        }

        private void pcb_btnDeleteTelephone_Click(object sender, EventArgs e)
        {
            this.phoneDelete();
        }

        private void lbl_btnDeletePhoneTag_Click(object sender, EventArgs e)
        {
            this.phoneDelete();
        }

        //CANCELAR EDIÇÃO OU CADASTRO 
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
            if (Globals.idCliente != 0 && editClick != true)
            {
                Client client = new Client();
                client.IdCliente = Globals.idCliente;
                if (Database.deleteAllTelephones(client)) if (Database.deleteClient(client)) this.goToClientForm();
            }
            else this.goToClientForm();
        }

        private void lbl_btnCancelTag_Click(object sender, EventArgs e)
        {
            if (Globals.idCliente != 0 && editClick != true)
            {
                Client client = new Client();
                client.IdCliente = Globals.idCliente;
                if (Database.deleteAllTelephones(client)) if (Database.deleteClient(client)) this.goToClientForm();
            }
            else this.goToClientForm();
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO

        //Formatação da tabela após a disposição dos dados
        private void dgv_telephones_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn dataGridViewColumn in dgv_telephones.Columns) dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            if (dgv_telephones.ColumnCount > 0)
            {
                this.dgv_telephones.Columns[0].Visible = false;
                for (int i = 1; i < dgv_telephones.ColumnCount; i++) dgv_telephones.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        //KeyPress

        private void txt_clientName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == (char)Keys.Delete) || (e.KeyChar == (char)Keys.Space))) e.Handled = true;
        }

        private void txt_clientAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == (char)Keys.Delete) || (e.KeyChar == (char)Keys.Space))) e.Handled = true;
        }

        private void txt_clientHouseNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void txt_clientNeighbourhood_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == (char)Keys.Delete) || (e.KeyChar == (char)Keys.Space))) e.Handled = true;
        }

        private void txt_clientCity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == (char)Keys.Delete) || (e.KeyChar == (char)Keys.Space))) e.Handled = true;
        }

        private void txt_clientEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetterOrDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == (char)Keys.Delete) || (e.KeyChar == '@') || (e.KeyChar == '.'))) e.Handled = true;
        }

        private void mtb_telephoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        //MouseEnter

        private void mtb_telephoneNumber_MouseEnter(object sender, EventArgs e)
        {
            setRGB(0, 104, 232);
        }

        //MouseLeave

        private void mtb_telephoneNumber_MouseLeave(object sender, EventArgs e)
        {
            if (!this.mtb_telephoneNumber.Focused) setRGB(0, 76, 157);
        }

        //Enter

        private void border_mtbTelephoneNumber_Enter(object sender, EventArgs e)
        {
            this.mtb_telephoneNumber.Focus();
            this.setRGB(0, 104, 232);
        }
        private void mtb_telephoneNumber_Enter(object sender, EventArgs e)
        {
            this.setRGB(0, 104, 232);
        }

        //Leave

        private void txt_clientName_Leave(object sender, EventArgs e)
        {
            this.txt_clientName.PlaceholderText = "Nome";
        }

        private void mtb_telephoneNumber_Leave(object sender, EventArgs e)
        {
            this.setRGB(0, 76, 157);
        }

        //SelectionChanged

        private void dgv_telephones_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_telephones.SelectedRows.Count > 0)
            {
                if (dgv_telephones.SelectedRows[0].Cells.Count > 2)
                {
                    this.cbb_telephoneType.SelectedIndex = this.cbb_telephoneType.FindString(" " + this.dgv_telephones.SelectedRows[0].Cells[1].Value.ToString().Trim());
                    this.mtb_telephoneNumber.Text = this.dgv_telephones.SelectedRows[0].Cells[2].Value.ToString().Trim();
                }
            }
        }

        //SelectedIndexChanged        

        private void cbb_telephoneType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.cbb_telephoneType.SelectedIndex != -1)
            {
                if (this.cbb_telephoneType.SelectedItem.ToString().Trim() == "Residencial") this.mtb_telephoneNumber.Mask = "(99) 0000-0000"; 
                else this.mtb_telephoneNumber.Mask = "(99) 00000-0000";
            }
        }
    }
}
