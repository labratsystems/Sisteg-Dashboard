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
        public bool editClick = false;

        //CARREGA INSTÂNCIA DO PAINEL CONJUNTO À UM DATATABLE DE FORNECEDOR PARA ATUALIZAÇÃO OU EXCLUSÃO, QUE PODE SER NULO EM CASO DE CADASTRO
        public AddSupplier(DataTable dataTable)
        {
            InitializeComponent();
            Globals.supplierDataTable = dataTable;
            if (Globals.supplierDataTable != null)
            {
                foreach (DataRow dataRow in Globals.supplierDataTable.Rows)
                {
                    if (Globals.supplierDataTable.Rows.IndexOf(dataRow) == 0)
                    {
                        Globals.idFornecedor = Convert.ToInt32(dataRow.ItemArray[0]);
                        this.txt_supplierName.Text = dataRow.ItemArray[1].ToString().Trim();
                        this.txt_supplierAddress.Text = dataRow.ItemArray[2].ToString().Trim();
                        this.txt_supplierHouseNumber.Text = dataRow.ItemArray[3].ToString().Trim();
                        this.txt_supplierNeighbourhood.Text = dataRow.ItemArray[4].ToString().Trim();
                        this.txt_supplierCity.Text = dataRow.ItemArray[5].ToString().Trim();
                        this.cbb_supplierState.SelectedIndex = cbb_supplierState.FindString(" " + dataRow.ItemArray[6].ToString().Trim());
                        this.txt_supplierEmail.Text = dataRow.ItemArray[7].ToString().Trim();
                        //Telefone
                        this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idFornecedor = " + Globals.idFornecedor);
                        if (this.dgv_telephones.Rows.Count == 0) this.displayTelephoneSettings(false, 204);
                    }
                }
            }
            else
            {
                Globals.idFornecedor = 0;
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

        //Função que cadastra novo telefone
        private void newTelephone()
        {
            string mtb = this.mtb_telephoneNumber.Text.Trim();
            if ((this.cbb_telephoneType.SelectedIndex == -1) || ((this.mtb_telephoneNumber.Text.IndexOf(' ')) != (this.mtb_telephoneNumber.Text.LastIndexOf(' ')))) MessageBox.Show("Selecione um telefone para atualizar ou preencha os dados corretamente!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (mtb.Trim().Length < 10) MessageBox.Show("Selecione um telefone para atualizar ou preencha os dados corretamente!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Telephone telephone = new Telephone();
                telephone.IdFornecedor = Globals.idFornecedor;
                telephone.TipoTelefone = this.cbb_telephoneType.SelectedItem.ToString().Trim();
                telephone.NumeroTelefone = this.mtb_telephoneNumber.Text.Trim();
                if (Database.newTelephone(telephone))
                {
                    MessageBox.Show("Telefone cadastrado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.cbb_telephoneType.SelectedIndex = -1;
                    this.cbb_telephoneType.Text = "Tipo do telefone";
                    this.mtb_telephoneNumber.Clear();
                    this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idFornecedor = " + Globals.idFornecedor);
                    this.displayTelephoneSettings(true, 518);
                }
                else MessageBox.Show("Não foi possível cadastrar telefone!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        //Função que retorna ao formulário de produtos
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

        //Função que cadastra fornecedor
        private void supplierRegister()
        {
            if (String.IsNullOrEmpty(txt_supplierName.Text.Trim()) || String.IsNullOrEmpty(txt_supplierAddress.Text.Trim()) || String.IsNullOrEmpty(txt_supplierHouseNumber.Text.Trim()) || String.IsNullOrEmpty(txt_supplierNeighbourhood.Text.Trim()) || String.IsNullOrEmpty(txt_supplierCity.Text.Trim()) || (this.cbb_supplierState.SelectedIndex == -1)) MessageBox.Show("Preencha todos os campos obrigatórios para cadastrar fornecedor!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (Globals.idFornecedor == 0)
                {
                    if ((String.IsNullOrEmpty(this.txt_supplierName.Text.Trim())) || (String.IsNullOrEmpty(this.txt_supplierName.Text.Trim())) || (String.IsNullOrEmpty(this.txt_supplierHouseNumber.Text.Trim())) || (String.IsNullOrEmpty(this.txt_supplierCity.Text.Trim())) || (this.cbb_supplierState.SelectedIndex == -1)) MessageBox.Show("Cadastre o fornecdor antes de cadastrar seu telefone!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                    {
                        if ((Database.query("SELECT nomeFornecedor FROM fornecedor WHERE nomeFornecedor = '" + this.txt_supplierName.Text.Trim() + "';")).Rows.Count > 0) MessageBox.Show("Já existe um fornecedor cadastrado com esse nome!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else
                        {
                            Supplier supplier = new Supplier();
                            supplier.NomeFornecedor = txt_supplierName.Text.Trim();
                            supplier.EnderecoFornecedor = txt_supplierAddress.Text.Trim();
                            supplier.NumeroResidencia = txt_supplierHouseNumber.Text.Trim();
                            supplier.BairroFornecedor = txt_supplierNeighbourhood.Text.Trim();
                            supplier.CidadeFornecedor = txt_supplierCity.Text.Trim();
                            supplier.EstadoFornecedor = cbb_supplierState.SelectedItem.ToString().Trim();
                            supplier.EmailFornecedor = txt_supplierEmail.Text.Trim();
                            if (Database.newSupplier(supplier)) this.clearFields();
                            else MessageBox.Show("[ERRO] Não foi possível cadastrar fornecedor!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    this.clearFields();
                    Globals.idFornecedor = 0;
                }
            }
        }

        //Função que atualiza fornecedor
        private void supplierUpdate()
        {
            if (String.IsNullOrEmpty(txt_supplierName.Text.Trim()) || String.IsNullOrEmpty(txt_supplierAddress.Text.Trim()) || String.IsNullOrEmpty(txt_supplierHouseNumber.Text.Trim()) || String.IsNullOrEmpty(txt_supplierNeighbourhood.Text.Trim()) || String.IsNullOrEmpty(txt_supplierCity.Text.Trim()) || (this.cbb_supplierState.SelectedIndex == -1)) MessageBox.Show("Preencha todos os campos obrigatórios para cadastrar fornecedor!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if ((String.IsNullOrEmpty(this.txt_supplierName.Text.Trim())) || (String.IsNullOrEmpty(this.txt_supplierName.Text.Trim())) || (String.IsNullOrEmpty(this.txt_supplierHouseNumber.Text.Trim())) || (String.IsNullOrEmpty(this.txt_supplierCity.Text.Trim())) || (this.cbb_supplierState.SelectedIndex == -1)) MessageBox.Show("Cadastre o fornecdor antes de cadastrar seu telefone!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    Supplier supplier = new Supplier();
                    supplier.IdFornecedor = Globals.idFornecedor;
                    supplier.NomeFornecedor = txt_supplierName.Text.Trim();
                    supplier.EnderecoFornecedor = txt_supplierAddress.Text.Trim();
                    supplier.NumeroResidencia = txt_supplierHouseNumber.Text.Trim();
                    supplier.BairroFornecedor = txt_supplierNeighbourhood.Text.Trim();
                    supplier.CidadeFornecedor = txt_supplierCity.Text.Trim();
                    supplier.EstadoFornecedor = cbb_supplierState.SelectedItem.ToString().Trim();
                    supplier.EmailFornecedor = txt_supplierEmail.Text.Trim();
                    if (Database.updateSupplier(supplier))
                    {
                        MessageBox.Show("Fornecedor atualizado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.txt_supplierName.PlaceholderText = "";
                        this.txt_supplierName.Focus();
                    }
                    else MessageBox.Show("[ERRO] Não foi possível atualizar fornecedor!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Função que exclui fornecedor
        private void supplierDelete()
        {
            Supplier supplier = new Supplier();
            supplier.IdFornecedor = Globals.idFornecedor;
            DataTable productsDataTable = Database.query("SELECT idProduto FROM produto WHERE idFornecedor = " + Globals.idFornecedor);
            foreach (DataRow dataRow in productsDataTable.Rows)
            {
                Product product = new Product();
                product.IdProduto = Convert.ToInt32(dataRow.ItemArray[0]);
                if (Database.deleteAllBudgetedProducts(product)) continue;
            }
            if (Database.deleteAllProducts(supplier) && Database.deleteSupplier(supplier))
            {
                MessageBox.Show("Fornecedor excluído com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.goToProductForm();
            }
            else MessageBox.Show("[ERRO] Não foi possível excluir fornecedor!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Função que cadastra telefone
        private void phoneRegister()
        {
            if (String.IsNullOrEmpty(mtb_telephoneNumber.Text.Trim()) || (this.cbb_telephoneType.SelectedIndex == -1)) MessageBox.Show("Preencha todos os campos obrigatórios para cadastrar telefone!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (Globals.idFornecedor == 0)
            {
                if ((String.IsNullOrEmpty(this.txt_supplierName.Text.Trim())) || (String.IsNullOrEmpty(this.txt_supplierName.Text.Trim())) || (String.IsNullOrEmpty(this.txt_supplierHouseNumber.Text.Trim())) || (String.IsNullOrEmpty(this.txt_supplierCity.Text.Trim())) || (this.cbb_supplierState.SelectedIndex == -1)) MessageBox.Show("Cadastre o fornecdor antes de cadastrar seu telefone!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    if ((Database.query("SELECT nomeFornecedor FROM fornecedor WHERE nomeFornecedor = '" + this.txt_supplierName.Text.Trim() + "';")).Rows.Count > 0) MessageBox.Show("Já existe um fornecedor cadastrado com esse nome!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                    {
                        Supplier supplier = new Supplier();
                        supplier.NomeFornecedor = txt_supplierName.Text.Trim();
                        supplier.EnderecoFornecedor = txt_supplierAddress.Text.Trim();
                        supplier.NumeroResidencia = txt_supplierHouseNumber.Text.Trim();
                        supplier.BairroFornecedor = txt_supplierNeighbourhood.Text.Trim();
                        supplier.CidadeFornecedor = txt_supplierCity.Text.Trim();
                        supplier.EstadoFornecedor = cbb_supplierState.SelectedItem.ToString().Trim();
                        supplier.EmailFornecedor = txt_supplierEmail.Text.Trim();
                        if (Database.newSupplier(supplier))
                        {
                            Globals.idFornecedor = Convert.ToInt32(Database.query("SELECT idFornecedor FROM fornecedor ORDER BY idFornecedor DESC LIMIT 1;").Rows[0].ItemArray[0]);
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
                if ((this.cbb_telephoneType.SelectedIndex == -1) || ((this.mtb_telephoneNumber.Text.IndexOf(' ')) != (this.mtb_telephoneNumber.Text.LastIndexOf(' '))) || (mtb.Trim().Length < 10)) MessageBox.Show("Selecione um telefone para atualizar ou preencha os dados corretamente!");
                else
                {
                    Telephone telephone = new Telephone();
                    telephone.IdTelefone = Convert.ToInt32(this.dgv_telephones.SelectedRows[0].Cells[0].Value);
                    telephone.TipoTelefone = this.cbb_telephoneType.SelectedItem.ToString().Trim();
                    telephone.NumeroTelefone = this.mtb_telephoneNumber.Text.Trim();
                    if (Database.updateTelephone(telephone))
                    {
                        MessageBox.Show("Telefone atualizado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idFornecedor = " + Globals.idFornecedor);
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
                    this.dataTableRemoveEmptyColumns("SELECT idTelefone, tipoTelefone AS 'Tipo:', numeroTelefone AS 'Número:' FROM telefone WHERE idFornecedor = " + Globals.idFornecedor);
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

        //CADASTRAR FORNECEDOR
        private void pcb_supplierRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnSupplierRegister.BackgroundImage = Properties.Resources.btn_add_supplier_active;
        }

        private void pcb_supplierRegister_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_supplierRegisterTag.ClientRectangle.Contains(lbl_supplierRegisterTag.PointToClient(Cursor.Position))) this.pcb_btnSupplierRegister.BackgroundImage = Properties.Resources.btn_add_supplier;
        }

        private void pcb_supplierRegister_Click(object sender, EventArgs e)
        {
            this.supplierRegister();
        }

        private void lbl_supplierRegisterTag_Click(object sender, EventArgs e)
        {
            this.supplierRegister();
        }

        //ATUALIZAR FORNECEDOR
        private void pcb_btnUpdate_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnUpdate.BackgroundImage = Properties.Resources.btn_edit_active;
        }

        private void pcb_btnUpdate_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnUpdateTag.ClientRectangle.Contains(lbl_btnUpdateTag.PointToClient(Cursor.Position))) this.pcb_btnUpdate.BackgroundImage = Properties.Resources.btn_edit;
        }

        private void pcb_btnUpdate_Click(object sender, EventArgs e)
        {
            this.supplierUpdate();
        }

        private void lbl_btnUpdateTag_Click(object sender, EventArgs e)
        {
            this.supplierUpdate();
        }

        //EXCLUIR FORNECEDOR
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
            this.supplierDelete();
        }

        private void lbl_btnDeleteTag_Click(object sender, EventArgs e)
        {
            this.supplierDelete();
        }

        //CADASTRO DE TELEFONE
        private void pcb_btnPhoneRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnPhoneRegister.BackgroundImage = Properties.Resources.btn_add_phone_active;
        }

        private void pcb_btnPhoneRegister_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnPhoneRegisterTag.ClientRectangle.Contains(lbl_btnPhoneRegisterTag.PointToClient(Cursor.Position))) this.pcb_btnPhoneRegister.BackgroundImage = Properties.Resources.btn_add_phone;
        }

        private void pcb_btnPhoneRegister_Click(object sender, EventArgs e)
        {
            this.phoneRegister();
        }

        private void lbl_btnPhoneRegisterTag_Click(object sender, EventArgs e)
        {
            this.phoneRegister();
        }

        //ATUALIZAR TELEFONE
        private void pcb_btnUpdateTelephone_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnUpdateTelephone.BackgroundImage = Properties.Resources.btn_edit_active;
        }

        private void pcb_btnUpdateTelephone_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnUpdateTag.ClientRectangle.Contains(lbl_btnUpdateTag.PointToClient(Cursor.Position))) this.pcb_btnUpdateTelephone.BackgroundImage = Properties.Resources.btn_edit;
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
            if (!lbl_btnDeleteTag.ClientRectangle.Contains(lbl_btnDeleteTag.PointToClient(Cursor.Position))) this.pcb_btnDeleteTelephone.BackgroundImage = Properties.Resources.btn_delete;
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
            if (Globals.idFornecedor != 0 && editClick != true)
            {
                Supplier supplier = new Supplier();
                supplier.IdFornecedor = Globals.idFornecedor;
                if (Database.deleteAllTelephones(supplier)) if (Database.deleteSupplier(supplier)) this.goToProductForm();
            }
            else this.goToProductForm();
        }

        private void lbl_btnCancelTag_Click(object sender, EventArgs e)
        {
            if (Globals.idFornecedor != 0 && editClick != true)
            {
                Supplier supplier = new Supplier();
                supplier.IdFornecedor = Globals.idFornecedor;
                if (Database.deleteAllTelephones(supplier)) if (Database.deleteSupplier(supplier)) this.goToProductForm();
            }
            else this.goToProductForm();
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO

        //Formatação da tabela após disposição dos dados
        private void dgv_telephones_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn dataGridViewColumn in dgv_telephones.Columns) dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            int col = dgv_telephones.ColumnCount;
            if (col > 0)
            {
                this.dgv_telephones.Columns[0].Visible = false;
                for (int i = 1; i < col; i++) dgv_telephones.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        //KeyPress

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

        private void mtb_telephoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        //MouseEnter

        private void mtb_telephoneNumber_MouseEnter(object sender, EventArgs e)
        {
            this.setRGB(0, 104, 232);
        }

        //MouseLeave

        private void mtb_telephoneNumber_MouseLeave(object sender, EventArgs e)
        {
            if (!this.mtb_telephoneNumber.Focused) this.setRGB(0, 76, 157);
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
        private void txt_supplierName_Leave(object sender, EventArgs e)
        {
            this.txt_supplierName.PlaceholderText = "Nome";
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
            if (this.cbb_telephoneType.SelectedIndex != -1)
            {
                if (this.cbb_telephoneType.SelectedItem.ToString().Trim() == "Residencial") this.mtb_telephoneNumber.Mask = "(99) 0000-0000";
                else this.mtb_telephoneNumber.Mask = "(99) 00000-0000";
            }
        }
    }
}
