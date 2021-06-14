using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class AddClient : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        private static DataTable clientDataTable, telephoneDataTable;
        public static int idCliente = 0;
        public bool editClick = false;

        //CARREGA INSTÂNCIA DO PAINEL CONJUNTO À UM DATATABLE DE CLIENTE PARA ATUALIZAÇÃO OU EXCLUSÃO, QUE PODE SER NULO EM CASO DE CADASTRO
        public AddClient(DataTable dataTable)
        {
            InitializeComponent();
            clientDataTable = dataTable;
            this.txt_clientName.Focus();
            if (clientDataTable != null)
            {
                foreach (DataRow dataRowClient in clientDataTable.Rows)
                {
                    if (clientDataTable.Rows.IndexOf(dataRowClient) == 0)
                    {
                        idCliente = Convert.ToInt32(dataRowClient.ItemArray[0]);
                        this.txt_clientName.Text = dataRowClient.ItemArray[1].ToString();
                        this.txt_clientAddress.Text = dataRowClient.ItemArray[2].ToString();
                        this.txt_clientHouseNumber.Text = dataRowClient.ItemArray[3].ToString();
                        this.txt_clientNeighbourhood.Text = dataRowClient.ItemArray[4].ToString();
                        this.txt_clientCity.Text = dataRowClient.ItemArray[5].ToString();
                        this.cbb_clientState.SelectedIndex = cbb_clientState.FindString(" " + dataRowClient.ItemArray[6].ToString());
                        this.txt_clientEmail.Text = dataRowClient.ItemArray[7].ToString();
                        //Telefone
                        this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idCliente = " + idCliente);
                        if (this.dgv_telephones.Rows.Count == 0) this.displayTelephoneSettings(false, 204);
                    }
                }
            } 
            else
            {
                idCliente = 0;
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

        //FUNÇÃO QUE RETORNA OS CAMPOS DO FORMULÁRIO AO ESTADO INICIAL
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

        //FUNÇÃO QUE CADASTRA NOVO TELEFONE
        private void newTelephone()
        {
            this.mtb_telephoneNumber.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            string mtb = this.mtb_telephoneNumber.Text;
            this.mtb_telephoneNumber.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
            if ((this.cbb_telephoneType.SelectedIndex == -1) || ((this.mtb_telephoneNumber.Text.IndexOf(' ')) != (this.mtb_telephoneNumber.Text.LastIndexOf(' ')))) MessageBox.Show("Selecione um telefone para atualizar ou preencha os dados corretamente!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (mtb.Trim().Length < 10) MessageBox.Show("Selecione um telefone para atualizar ou preencha os dados corretamente!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Telephone telephone = new Telephone();
                telephone.idCliente = idCliente;
                telephone.tipoTelefone = this.cbb_telephoneType.SelectedItem.ToString().Trim();
                telephone.numeroTelefone = this.mtb_telephoneNumber.Text;
                if (Database.newTelephone(telephone))
                {
                    MessageBox.Show("Telefone cadastro com sucesso!");
                    this.cbb_telephoneType.SelectedIndex = -1;
                    this.cbb_telephoneType.Text = "Tipo do telefone";
                    this.mtb_telephoneNumber.Clear();
                    this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idCliente = " + idCliente);
                    this.displayTelephoneSettings(true, 518);
                }
                else MessageBox.Show("Não foi possível cadastrar telefone!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //FUNÇÃO QUE DISPONIBILIZA OS MÉTODOS ASSOCIADOS À CLASSE TELEFONE
        private void displayTelephoneSettings(bool value, int height)
        {
            this.dgv_telephones.Visible = value;
            this.bsp_dgvTelephones.Visible = value;
            this.pcb_btnUpdateTelephone.Visible = value;
            this.pcb_btnDeleteTelephone.Visible = value;
            this.bsp_telephones.Height = height;
        }

        //RETORNA AO FORMULÁRIO DE CLIENTES
        private void goToClientForm()
        {
            if (Application.OpenForms.OfType<ClientForm>().Count() == 0)
            {
                ClientForm clientForm = new ClientForm();
                this.editClick = false;
                clientForm.Show();
                this.Close();
            }
        }

        //FUNÇÃO QUE REMOVE AS COLUNAS VAZIAS DA TABELA
        private void dataTableRemoveEmptyColumns(string query)
        {
            telephoneDataTable = Database.query(query);
            for (int col = telephoneDataTable.Columns.Count - 1; col >= 0; col--)
            {
                bool removeColumn = true;
                foreach (DataRow dataRow in telephoneDataTable.Rows)
                {
                    if (!dataRow.IsNull(col))
                    {
                        removeColumn = false;
                        break;
                    }
                }
                if (removeColumn) telephoneDataTable.Columns.RemoveAt(col);
            }
            this.dgv_telephones.DataSource = telephoneDataTable;
        }

        //FORMATAÇÃO A TABELA APÓS A DISPOSIÇÃO DOS DADOS
        private void dgv_telephones_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn dataGridViewColumn in dgv_telephones.Columns)
            {
                dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            int col = dgv_telephones.ColumnCount;
            if (col > 0)
            {
                this.dgv_telephones.Columns[0].Visible = false;
                for (int i = 1; i < col; i++) dgv_telephones.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        //CADASTRO DE CLIENTE
        private void pcb_clientRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_clientRegister.Image = Properties.Resources.btn_client_form_active;
        }

        private void pcb_clientRegister_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_clientRegister.Image = Properties.Resources.btn_client_form;
        }

        private void pcb_clientRegister_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txt_clientName.Text.Trim()) || String.IsNullOrEmpty(txt_clientAddress.Text.Trim()) || String.IsNullOrEmpty(txt_clientHouseNumber.Text.Trim()) || String.IsNullOrEmpty(txt_clientNeighbourhood.Text.Trim()) || String.IsNullOrEmpty(txt_clientCity.Text.Trim()) || (this.cbb_clientState.SelectedIndex == -1)) MessageBox.Show("Preencha todos os campos obrigatórios para cadastrar cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (idCliente == 0)
                {
                    Client client = new Client();
                    client.nomeCliente = txt_clientName.Text;
                    client.enderecoCliente = txt_clientAddress.Text;
                    client.numeroResidencia = txt_clientHouseNumber.Text;
                    client.bairroCliente = txt_clientNeighbourhood.Text;
                    client.cidadeCliente = txt_clientCity.Text;
                    client.estadoCliente = cbb_clientState.SelectedItem.ToString().Trim();
                    client.emailCliente = txt_clientEmail.Text;
                    if (Database.newClient(client)) this.clearFields(); else MessageBox.Show("Não foi possível cadastrar cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    this.clearFields();
                    idCliente = 0;
                }
            }
        }

        //ATUALIZAÇÃO DE CLIENTE
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
            if (String.IsNullOrEmpty(txt_clientName.Text.Trim()) || String.IsNullOrEmpty(txt_clientAddress.Text.Trim()) || String.IsNullOrEmpty(txt_clientHouseNumber.Text.Trim()) || String.IsNullOrEmpty(txt_clientNeighbourhood.Text.Trim()) || String.IsNullOrEmpty(txt_clientCity.Text.Trim()) || (this.cbb_clientState.SelectedIndex == -1)) MessageBox.Show("Preencha todos os campos obrigatórios para cadastrar cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Client client = new Client();
                client.idCliente = idCliente;
                client.nomeCliente = txt_clientName.Text;
                client.enderecoCliente = txt_clientAddress.Text;
                client.numeroResidencia = txt_clientHouseNumber.Text;
                client.bairroCliente = txt_clientNeighbourhood.Text;
                client.cidadeCliente = txt_clientCity.Text;
                client.estadoCliente = cbb_clientState.SelectedItem.ToString().Trim();
                client.emailCliente = txt_clientEmail.Text;
                if (Database.updateClient(client))
                {
                    MessageBox.Show("Cliente atualizado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txt_clientName.PlaceholderText = "";
                    this.txt_clientName.Focus();
                }
                else MessageBox.Show("Não foi possível atualizar cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //EXCLUSÃO DE CLIENTE
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
            Client client = new Client();
            client.idCliente = idCliente;
            DataTable budgetsDataTable = Database.query("SELECT * FROM orcamento WHERE idCliente = " + idCliente.ToString());
            if(budgetsDataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in budgetsDataTable.Rows)
                {
                    Budget budget = new Budget();
                    budget.numeroOrcamento = Convert.ToInt32(dataRow.ItemArray[0]);
                    if (Database.deleteAllBudgetedProducts(budget))
                    {
                        DataTable budgetIncomesDataTable = Database.query("SELECT * FROM receita WHERE numeroOrcamento = " + budget.numeroOrcamento.ToString());
                        Income income = new Income();
                        income.idReceita = Convert.ToInt32(budgetIncomesDataTable.Rows[0].ItemArray[0]);
                        if(Database.deleteAllParcels(income)) if(Database.deleteAllRepeats(income)) if(Database.deleteAllIncomes(income))
                        if (Database.deleteAllBudgets(client))
                        {
                            if (Database.deleteAllTelephones(client))
                            {
                                if (Database.deleteClient(client)) MessageBox.Show("Cliente excluído com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information); else MessageBox.Show("Não foi possível excluir cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.goToClientForm();
                            }
                        }
                        else MessageBox.Show("[ERRO] Não foi possível excluir cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show("[ERRO] Não foi possível excluir cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (Database.deleteAllBudgets(client))
                {
                    if (Database.deleteAllTelephones(client))
                    {
                        if (Database.deleteClient(client)) MessageBox.Show("Cliente excluído com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information); else MessageBox.Show("Não foi possível excluir cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.goToClientForm();
                    }
                }
                else MessageBox.Show("[ERRO] Não foi possível excluir cliente!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //CADASTRO DE TELEFONE
        private void pcb_btnAdd_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnAdd.Image = Properties.Resources.btn_add_active;
        }

        private void pcb_btnAdd_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnAdd.Image = Properties.Resources.btn_add;
        }

        private void pcb_btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(mtb_telephoneNumber.Text.Trim()) || (this.cbb_telephoneType.SelectedIndex == -1)) MessageBox.Show("Preencha todos os campos obrigatórios para cadastrar telefone!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (idCliente == 0)
            {
                Client client = new Client();
                client.nomeCliente = txt_clientName.Text;
                client.enderecoCliente = txt_clientAddress.Text;
                client.numeroResidencia = txt_clientHouseNumber.Text;
                client.bairroCliente = txt_clientNeighbourhood.Text;
                client.cidadeCliente = txt_clientCity.Text;
                client.estadoCliente = cbb_clientState.SelectedItem.ToString().Trim();
                client.emailCliente = txt_clientEmail.Text;
                if (Database.newClient(client))
                {
                    idCliente = Convert.ToInt32(Database.query("SELECT idCliente FROM cliente ORDER BY idCliente DESC LIMIT 1;").Rows[0].ItemArray[0]);
                    this.newTelephone();
                }
            } else this.newTelephone();
        }

        //ATUALIZAR TELEFONE
        private void pcb_btnUpdateTelephone_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnUpdateTelephone.Image = Properties.Resources.btn_edit_active;
        }

        private void pcb_btnUpdateTelephone_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnUpdateTelephone.Image = Properties.Resources.btn_edit;
        }

        private void pcb_btnUpdateTelephone_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(this.dgv_telephones.Rows.Count) == 0) MessageBox.Show("Não há telefone selecionado para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Convert.ToInt32(this.dgv_telephones.SelectedRows.Count) > 1) MessageBox.Show("Selecione apenas uma linha da tabela para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                this.mtb_telephoneNumber.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                string mtb = this.mtb_telephoneNumber.Text;
                this.mtb_telephoneNumber.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
                if ((this.cbb_telephoneType.SelectedIndex == -1) || ((this.mtb_telephoneNumber.Text.IndexOf(' ')) != (this.mtb_telephoneNumber.Text.LastIndexOf(' ')))) MessageBox.Show("Selecione um telefone para atualizar ou preencha os dados corretamente!");
                else if(mtb.Trim().Length < 10) MessageBox.Show("Selecione um telefone para atualizar ou preencha os dados corretamente!");
                else
                {
                    MessageBox.Show(this.mtb_telephoneNumber.Text.Trim().Length.ToString());
                    Telephone telephone = new Telephone();
                    telephone.idTelefone = Convert.ToInt32(this.dgv_telephones.SelectedRows[0].Cells[0].Value);
                    telephone.tipoTelefone = this.cbb_telephoneType.SelectedItem.ToString().Trim();
                    telephone.numeroTelefone = this.mtb_telephoneNumber.Text;
                    if (Database.updateTelephone(telephone))
                    {
                        MessageBox.Show("Telefone atualizado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idCliente = " + idCliente);
                    }
                    else MessageBox.Show("Não foi possível atualizar telefone!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //EXCLUIR TELEFONE
        private void pcb_btnDeleteTelephone_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnDeleteTelephone.Image = Properties.Resources.btn_delete_active;
        }

        private void pcb_btnDeleteTelephone_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnDeleteTelephone.Image = Properties.Resources.btn_delete;
        }

        private void pcb_btnDeleteTelephone_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(this.dgv_telephones.Rows.Count) == 0) MessageBox.Show("Não há telefone selecionado para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Convert.ToInt32(this.dgv_telephones.SelectedRows.Count) > 1) MessageBox.Show("Selecione apenas uma linha da tabela para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Telephone telephone = new Telephone();
                telephone.idTelefone = Convert.ToInt32(this.dgv_telephones.SelectedRows[0].Cells[0].Value);
                if (Database.deleteTelephone(telephone))
                {
                    MessageBox.Show("Telefone excluído com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.mtb_telephoneNumber.Focus();
                    this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idCliente = " + idCliente);
                    if (this.dgv_telephones.Rows.Count == 0) this.displayTelephoneSettings(false, 204);
                }
                else MessageBox.Show("Não foi possível excluir telefone!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //CANCELAR EDIÇÃO OU CADASTRO 
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
            if (idCliente != 0 && editClick != true)
            {
                Client client = new Client();
                client.idCliente = idCliente;
                if (Database.deleteAllTelephones(client)) if (Database.deleteClient(client)) this.goToClientForm();
            }
            else this.goToClientForm();
        }

        //ENCERRAR A APLICAÇÃO
        private void pcb_appClose_Click(object sender, EventArgs e)
        {
            if (((DialogResult)MessageBox.Show("Tem certeza que deseja encerrar a aplicação?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)).ToString().ToUpper() == "YES") Application.Exit();
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO
        private void setRGB(int r, int g, int b)
        {
            this.mtb_telephoneNumber.BackColor = Color.FromArgb(r, g, b);
            this.border_mtbTelephoneNumber.FillColor = Color.FromArgb(r, g, b);
            this.border_mtbTelephoneNumber.OnIdleState.FillColor = Color.FromArgb(r, g, b);
        }

        private void mtb_telephoneNumber_MouseEnter(object sender, EventArgs e)
        {
            setRGB(0, 104, 232);
        }

        private void mtb_telephoneNumber_MouseLeave(object sender, EventArgs e)
        {
            if (!this.mtb_telephoneNumber.Focused) setRGB(0, 76, 157);
        }

        private void border_mtbTelephoneNumber_Enter(object sender, EventArgs e)
        {
            this.mtb_telephoneNumber.Focus();
            setRGB(0, 104, 232);
        }

        private void mtb_telephoneNumber_Enter(object sender, EventArgs e)
        {
            setRGB(0, 104, 232);
        }

        private void mtb_telephoneNumber_Leave(object sender, EventArgs e)
        {
            setRGB(0, 76, 157);
        }

        private void dgv_telephones_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_telephones.Focused)
            {
                if (dgv_telephones.SelectedRows.Count > 0)
                {
                    if (dgv_telephones.SelectedRows[0].Cells.Count > 2)
                    {
                        this.cbb_telephoneType.SelectedIndex = this.cbb_telephoneType.FindString(" " + this.dgv_telephones.SelectedRows[0].Cells[1].Value.ToString());
                        this.mtb_telephoneNumber.Text = this.dgv_telephones.SelectedRows[0].Cells[2].Value.ToString();
                    }
                }
            }
        }

        private void txt_clientName_Leave(object sender, EventArgs e)
        {
            this.txt_clientName.PlaceholderText = "Nome";
        }

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

        private void cbb_telephoneType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.cbb_telephoneType.SelectedIndex != -1)
            {
                if (this.cbb_telephoneType.SelectedItem.ToString().Trim() == "Residencial") this.mtb_telephoneNumber.Mask = "(99) 0000-0000"; else this.mtb_telephoneNumber.Mask = "(99) 00000-0000";
            }
        }
    }
}
