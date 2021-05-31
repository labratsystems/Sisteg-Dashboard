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
    public partial class AddSupplier : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        private static DataTable supplierDataTable, telephoneDataTable;
        public static int idFornecedor = 0;
        public bool editClick = false;

        //CARREGA INSTÂNCIA DO PAINEL CONJUNTO À UM DATATABLE DE FORNECEDOR PARA ATUALIZAÇÃO OU EXCLUSÃO, QUE PODE SER NULO EM CASO DE CADASTRO
        public AddSupplier(DataTable dataTable)
        {
            InitializeComponent();
            supplierDataTable = dataTable;
            if (supplierDataTable != null)
            {
                foreach (DataRow dataRow in supplierDataTable.Rows)
                {
                    if (supplierDataTable.Rows.IndexOf(dataRow) == 0)
                    {
                        idFornecedor = Convert.ToInt32(dataRow.ItemArray[0]);
                        MessageBox.Show(idFornecedor.ToString());
                        this.txt_supplierName.Text = dataRow.ItemArray[1].ToString();
                        this.txt_supplierAddress.Text = dataRow.ItemArray[2].ToString();
                        this.txt_supplierHouseNumber.Text = dataRow.ItemArray[3].ToString();
                        this.txt_supplierNeighbourhood.Text = dataRow.ItemArray[4].ToString();
                        this.txt_supplierCity.Text = dataRow.ItemArray[5].ToString();
                        this.cbb_supplierState.SelectedIndex = cbb_supplierState.FindString(" " + dataRow.ItemArray[6].ToString());
                        this.txt_supplierEmail.Text = dataRow.ItemArray[7].ToString();
                        //Telefone
                        this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idFornecedor = " + idFornecedor);
                        if (this.dgv_telephones.Rows.Count == 0) this.displayTelephoneSettings(false, 204);
                    }
                }
            }
            else
            {
                idFornecedor = 0;
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
            MessageBox.Show("Fornecedor cadastrado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.txt_supplierName.Clear();
            this.txt_supplierAddress.Clear();
            this.txt_supplierHouseNumber.Clear();
            this.txt_supplierNeighbourhood.Clear();
            this.txt_supplierCity.Clear();
            this.cbb_supplierState.SelectedIndex = -1;
            this.cbb_supplierState.Text = " Estado";
            this.txt_supplierEmail.Clear();
            this.txt_supplierName.PlaceholderText = "";
            this.txt_supplierName.Focus();
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
                telephone.idFornecedor = idFornecedor;
                telephone.tipoTelefone = this.cbb_telephoneType.SelectedItem.ToString().Trim();
                telephone.numeroTelefone = this.mtb_telephoneNumber.Text;
                if (Database.newTelephone(telephone))
                {
                    MessageBox.Show("Telefone cadastrado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.cbb_telephoneType.SelectedIndex = -1;
                    this.cbb_telephoneType.Text = "Tipo do telefone";
                    this.mtb_telephoneNumber.Clear();
                    this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idFornecedor = " + idFornecedor);
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

        //RETORNA AO FORMULÁRIO DE PRODUTOS
        private void goToProductForm()
        {
            if (Application.OpenForms.OfType<ProductForm>().Count() == 0)
            {
                ProductForm productForm = new ProductForm();
                this.editClick = false;
                productForm.Show();
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

        //CADASTRAR FORNECEDOR
        private void pcb_supplierRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnSupplierRegister.Image = Properties.Resources.btn_supplierRegister_active;
        }

        private void pcb_supplierRegister_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnSupplierRegister.Image = Properties.Resources.btn_supplierRegister;
        }

        private void pcb_supplierRegister_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txt_supplierName.Text.Trim()) || String.IsNullOrEmpty(txt_supplierAddress.Text.Trim()) || String.IsNullOrEmpty(txt_supplierHouseNumber.Text.Trim()) || String.IsNullOrEmpty(txt_supplierNeighbourhood.Text.Trim()) || String.IsNullOrEmpty(txt_supplierCity.Text.Trim()) || (this.cbb_supplierState.SelectedIndex == -1)) MessageBox.Show("Preencha todos os campos obrigatórios para cadastrar fornecedor!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if(idFornecedor == 0)
                {
                    Supplier supplier = new Supplier();
                    supplier.nomeFornecedor = txt_supplierName.Text;
                    supplier.enderecoFornecedor = txt_supplierAddress.Text;
                    supplier.numeroResidencia = txt_supplierHouseNumber.Text;
                    supplier.bairroFornecedor = txt_supplierNeighbourhood.Text;
                    supplier.cidadeFornecedor = txt_supplierCity.Text;
                    supplier.estadoFornecedor = cbb_supplierState.SelectedItem.ToString().Trim();
                    supplier.emailFornecedor = txt_supplierEmail.Text;
                    if (Database.newSupplier(supplier)) this.clearFields(); else MessageBox.Show("Não foi possível cadastrar fornecedor!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    this.clearFields();
                    idFornecedor = 0;
                }
            }
        }

        //ATUALIZAR FORNECEDOR
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
            if (String.IsNullOrEmpty(txt_supplierName.Text.Trim()) || String.IsNullOrEmpty(txt_supplierAddress.Text.Trim()) || String.IsNullOrEmpty(txt_supplierHouseNumber.Text.Trim()) || String.IsNullOrEmpty(txt_supplierNeighbourhood.Text.Trim()) || String.IsNullOrEmpty(txt_supplierCity.Text.Trim()) || (this.cbb_supplierState.SelectedIndex == -1)) MessageBox.Show("Preencha todos os campos obrigatórios para cadastrar fornecedor!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Supplier supplier = new Supplier();
                supplier.idFornecedor = idFornecedor;
                supplier.nomeFornecedor = txt_supplierName.Text;
                supplier.enderecoFornecedor = txt_supplierAddress.Text;
                supplier.numeroResidencia = txt_supplierHouseNumber.Text;
                supplier.bairroFornecedor = txt_supplierNeighbourhood.Text;
                supplier.cidadeFornecedor = txt_supplierCity.Text;
                supplier.estadoFornecedor = cbb_supplierState.SelectedItem.ToString().Trim();
                supplier.emailFornecedor = txt_supplierEmail.Text;
                if (Database.updateSupplier(supplier))
                {
                    MessageBox.Show("Fornecedor atualizado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txt_supplierName.PlaceholderText = "";
                    this.txt_supplierName.Focus();
                }
                else MessageBox.Show("Não foi possível atualizar fornecedor!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //EXCLUIR FORNECEDOR
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
            Supplier supplier = new Supplier();
            supplier.idFornecedor = idFornecedor;
            if (Database.deleteAllProducts(supplier) && Database.deleteSupplier(supplier))
            {
                MessageBox.Show("Fornecedor excluído com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.goToProductForm();
            }
            else MessageBox.Show("[ERRO] Não foi possível excluir fornecedor!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (idFornecedor == 0)
            {
                Supplier supplier = new Supplier();
                supplier.nomeFornecedor = txt_supplierName.Text;
                supplier.enderecoFornecedor = txt_supplierAddress.Text;
                supplier.numeroResidencia = txt_supplierHouseNumber.Text;
                supplier.bairroFornecedor = txt_supplierNeighbourhood.Text;
                supplier.cidadeFornecedor = txt_supplierCity.Text;
                supplier.estadoFornecedor = cbb_supplierState.SelectedItem.ToString().Trim();
                supplier.emailFornecedor = txt_supplierEmail.Text;
                if (Database.newSupplier(supplier))
                {
                    idFornecedor = Convert.ToInt32(Database.query("SELECT idFornecedor FROM fornecedor ORDER BY idFornecedor DESC LIMIT 1;").Rows[0].ItemArray[0]);
                    this.newTelephone();
                }
            }
            else this.newTelephone();
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
                else if (mtb.Trim().Length < 10) MessageBox.Show("Selecione um telefone para atualizar ou preencha os dados corretamente!");
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
                        this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idFornecedor = " + idFornecedor);
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
                    this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idFornecedor = " + idFornecedor);
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
            MessageBox.Show(editClick.ToString());
            if (idFornecedor != 0 && editClick != true)
            {
                Supplier supplier = new Supplier();
                supplier.idFornecedor = idFornecedor;
                if (Database.deleteAllTelephones(supplier)) if (Database.deleteSupplier(supplier)) this.goToProductForm();
            }
            else this.goToProductForm();
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

        private void txt_supplierName_Leave(object sender, EventArgs e)
        {
            this.txt_supplierName.PlaceholderText = "Nome";
        }

        private void txt_supplierName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == (char)Keys.Delete) || (e.KeyChar == (char)Keys.Space))) e.Handled = true;
        }

        private void txt_supplierAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == (char)Keys.Delete) || (e.KeyChar == (char)Keys.Space))) e.Handled = true;
        }

        private void txt_supplierHouseNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void txt_supplierNeighbourhood_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == (char)Keys.Delete) || (e.KeyChar == (char)Keys.Space))) e.Handled = true;
        }

        private void txt_supplierCity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == (char)Keys.Delete) || (e.KeyChar == (char)Keys.Space))) e.Handled = true;
        }

        private void txt_supplierEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetterOrDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == (char)Keys.Delete) || (e.KeyChar == '@') || (e.KeyChar == '.'))) e.Handled = true;
        }
    }
}
