using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class ProductForm : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        private bool firstTime = true;
        Bitmap backGround, backGroundTemp;

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

        private void initialize()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            backGroundTemp = new Bitmap(Properties.Resources.product_sisteg_bg);
            backGround = new Bitmap(backGroundTemp, backGroundTemp.Width, backGroundTemp.Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.DrawImageUnscaled(backGround, 0, 0);
            base.OnPaint(e);
        }

        //INICIA INSTÂNCIA DO PAINEL, POPULANDO A TABELA DE LISTAGEM DE PRODUTOS OU FORNECEDORES
        public ProductForm()
        {
            InitializeComponent();
            initialize();
            this.dgv_productsOrSuppliers.Visible = true;
            this.pcb_btnEdit.Visible = true;
            this.lbl_btnEditTag.Visible = true;
            formatDataTable("SELECT produto.idProduto, produto.nomeProduto AS 'Nome do produto:', fornecedor.nomeFornecedor AS 'Nome do fornecedor:', categoria.nomeCategoria AS 'Categoria do produto:', produto.valorUnitario AS 'Valor unitário:' FROM produto JOIN categoria ON produto.idCategoria = categoria.idCategoria JOIN fornecedor ON fornecedor.idFornecedor = produto.idFornecedor WHERE categoria.categoriaProduto = true ORDER BY produto.nomeProduto;", Globals.productDataTable, false);
        }

        //MENU DE NAVEGAÇÃO DA APLICAÇÃO
        //Formulário Painel principal
        private void pcb_btnMain_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnMain.Image = Properties.Resources.btn_main_form_active;
        }

        private void pcb_btnMain_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_mainTag.ClientRectangle.Contains(lbl_mainTag.PointToClient(Cursor.Position))) this.pcb_btnMain.Image = Properties.Resources.btn_main_form;
        }

        private void pcb_btnMain_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Main>().Count() == 0)
            {
                Main main = new Main();
                main.Show();
                this.Close();
            }
        }

        private void lbl_mainTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Main>().Count() == 0)
            {
                Main main = new Main();
                main.Show();
                this.Close();
            }
        }

        //Formulário Cliente
        private void pcb_btnClient_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnClient.Image = Properties.Resources.btn_client_form_active;
        }

        private void pcb_btnClient_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_clientTag.ClientRectangle.Contains(lbl_clientTag.PointToClient(Cursor.Position))) this.pcb_btnClient.Image = Properties.Resources.btn_client_form;
        }

        private void pcb_btnClient_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ClientForm>().Count() == 0)
            {
                ClientForm client = new ClientForm();
                client.Show();
                this.Close();
            }
        }

        private void lbl_clientTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ClientForm>().Count() == 0)
            {
                ClientForm client = new ClientForm();
                client.Show();
                this.Close();
            }
        }

        //Formulário Orçamento
        private void pcb_btnBudget_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnBudget.Image = Properties.Resources.btn_budget_form_active;
        }

        private void pcb_btnBudget_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_budgetTag.ClientRectangle.Contains(lbl_budgetTag.PointToClient(Cursor.Position))) this.pcb_btnBudget.Image = Properties.Resources.btn_budget_form;
        }

        private void pcb_btnBudget_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<BudgetForm>().Count() == 0)
            {
                BudgetForm budget = new BudgetForm();
                budget.Show();
                this.Close();
            }
        }

        private void lbl_budgetTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<BudgetForm>().Count() == 0)
            {
                BudgetForm budget = new BudgetForm();
                budget.Show();
                this.Close();
            }
        }

        //Formulário Configurações
        private void pcb_btnConfig_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnConfig.Image = Properties.Resources.btn_config_main_active;
        }

        private void pcb_btnConfig_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_configTag.ClientRectangle.Contains(lbl_configTag.PointToClient(Cursor.Position))) this.pcb_btnConfig.Image = Properties.Resources.btn_config_main;
        }

        private void pcb_btnConfig_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ConfigForm>().Count() == 0)
            {
                ConfigForm config = new ConfigForm();
                config.Show();
                this.Close();
            }
        }

        private void lbl_configTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ConfigForm>().Count() == 0)
            {
                ConfigForm config = new ConfigForm();
                config.Show();
                this.Close();
            }
        }

        //FUNÇÕES
        //Função que formata o DataTable antes de atribuir os dados ao DataGridView
        private void formatDataTable(string query, DataTable dataTable, bool isSupplier)
        {
            dataTable = Database.query(query);
            
            if(dataTable.Rows.Count > 0)
            {
                this.dgv_productsOrSuppliers.Visible = true;
                this.pcb_btnEdit.Visible = true;
                this.lbl_btnEditTag.Visible = true;
                if (isSupplier)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        DataColumn dataColumn = dataTable.Columns.Add("Endereço:", typeof(string));
                        dataColumn.SetOrdinal(7);
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            dataTable.Rows[i]["Endereço:"] = dataTable.Rows[i].ItemArray[2] + ", " + dataTable.Rows[i].ItemArray[3] + " - " + dataTable.Rows[i].ItemArray[4] + " - " + dataTable.Rows[i].ItemArray[5] + ", " + dataTable.Rows[i].ItemArray[6];
                            DataTable telephoneDataTable = Database.query("SELECT * FROM telefone WHERE idFornecedor = " + dataTable.Rows[i].ItemArray[0] + " ORDER BY tipoTelefone ASC;");
                            if (firstTime)
                            {
                                this.listTelephones(firstTime, telephoneDataTable, dataTable, i);
                            }
                            else this.listTelephones(firstTime, telephoneDataTable, dataTable, i);
                        }
                        //Remove as colunas vazias da tabela
                        for (int col = dataTable.Columns.Count - 1; col >= 0; col--)
                        {
                            bool removeColumn = true;
                            foreach (DataRow dataRow in dataTable.Rows)
                            {
                                if (dataRow.RowState != DataRowState.Deleted)
                                {
                                    if (!String.IsNullOrEmpty(dataRow.ItemArray[col].ToString()))
                                    {
                                        removeColumn = false;
                                        break;
                                    }
                                }
                            }
                            if (removeColumn) dataTable.Columns.RemoveAt(col);
                        }

                        this.dgv_productsOrSuppliers.DataSource = dataTable;
                    }
                }
                else
                {
                    //Remove as colunas vazias da tabela
                    for (int col = dataTable.Columns.Count - 1; col >= 0; col--)
                    {
                        bool removeColumn = true;
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            if (dataRow.RowState != DataRowState.Deleted)
                            {
                                if (!String.IsNullOrEmpty(dataRow.ItemArray[col].ToString()))
                                {
                                    removeColumn = false;
                                    break;
                                }
                            }
                        }
                        if (removeColumn) dataTable.Columns.RemoveAt(col);
                    }

                    this.dgv_productsOrSuppliers.DataSource = dataTable;
                }
            }
            else
            {
                this.dgv_productsOrSuppliers.Visible = false;
                this.pcb_btnEdit.Visible = false;
                this.lbl_btnEditTag.Visible = false;
            }
        }

        //Função que lista os telefones do cliente na tabela de clientes
        private void listTelephones(bool firstTime, DataTable telephoneDataTable, DataTable dataTable, int i)
        {
            foreach (DataRow dataRow in telephoneDataTable.Rows)
            {
                if (firstTime)
                {
                    dataTable.Columns.Add("Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":", typeof(string));
                    firstTime = false;
                }
                DataTable telephoneTypeDataTable = Database.query("SELECT numeroTelefone FROM telefone WHERE idFornecedor = " + dataTable.Rows[i].ItemArray[0] + " AND tipoTelefone = '" + dataRow.ItemArray[3] + "';");
                if (telephoneTypeDataTable.Rows.Count == 1)
                {
                    if (dataTable.Columns.Contains("Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":")) dataTable.Rows[i]["Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":"] = dataRow.ItemArray[4];
                    else
                    {
                        dataTable.Columns.Add("Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":", typeof(string));
                        dataTable.Rows[i]["Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":"] = dataRow.ItemArray[4];
                    }
                }
                else if (telephoneTypeDataTable.Rows.Count > 1)
                {
                    string numbers = null;
                    int j = 0;
                    foreach (DataRow dataRowType in telephoneTypeDataTable.Rows)
                    {
                        numbers += dataRowType.ItemArray[0].ToString();
                        if (j != Convert.ToInt32(telephoneTypeDataTable.Rows.Count - 1)) numbers += "; ";
                        j++;
                    }
                    dataTable.Rows[i]["Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":"] = numbers;
                }
            }
        }

        //Função que adiciona produto ou fornecedor
        private void addProductOrSupplier()
        {
            if (rbtn_products.Checked)
            {
                if (Application.OpenForms.OfType<AddProduct>().Count() == 0)
                {
                    AddProduct addProduct = new AddProduct(null);
                    addProduct.lbl_btnUpdateTag.Hide();
                    addProduct.pcb_btnUpdate.Hide();
                    addProduct.lbl_btnDeleteTag.Hide();
                    addProduct.pcb_btnDelete.Hide();
                    addProduct.lbl_productRegisterTag.Location = new Point(124, 226);
                    addProduct.pcb_btnProductRegister.Location = new Point(42, 192);
                    addProduct.lbl_btnCancelTag.Text = "CANCELAR CADASTRO///";
                    addProduct.Show();
                    this.Close();
                }
            }
            else if (rbtn_supplier.Checked)
            {
                if (Application.OpenForms.OfType<AddSupplier>().Count() == 0)
                {
                    AddSupplier addSupplier = new AddSupplier(null);
                    addSupplier.pcb_btnUpdate.Hide();
                    addSupplier.lbl_btnUpdateTag.Hide();
                    addSupplier.pcb_btnDelete.Hide();
                    addSupplier.lbl_btnDeleteTag.Hide();
                    addSupplier.lbl_supplierRegisterTag.Location = new Point(100, 226);
                    addSupplier.pcb_btnSupplierRegister.Location = new Point(27, 198);
                    addSupplier.lbl_btnCancelTag.Text = "CANCELAR CADASTRO///";
                    addSupplier.Show();
                    this.Close();
                }
            }
        }

        //Função que edita produto ou fornecedor
        private void editProductOrSupplier()
        {
            if (rbtn_products.Checked)
            {
                if (Convert.ToInt32(this.dgv_productsOrSuppliers.Rows.Count) == 0) MessageBox.Show("Não há produto selecionado para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (Convert.ToInt32(this.dgv_productsOrSuppliers.SelectedRows.Count) > 1) MessageBox.Show("Selecione apenas uma linha da tabela para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    int id = Convert.ToInt32(this.dgv_productsOrSuppliers.SelectedRows[0].Cells[0].Value);
                    if (Database.query("SELECT * FROM produtoOrcado WHERE idProduto = " + id).Rows.Count > 0) MessageBox.Show("Não é possível editar este produto porque ele está vinculado à um orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else if (Application.OpenForms.OfType<AddProduct>().Count() == 0)
                    {
                        DataTable dataTableProduct = Database.query("SELECT * FROM produto WHERE idProduto = " + id + ";");
                        AddProduct addProduct = new AddProduct(dataTableProduct);
                        addProduct.Show();
                        this.Close();
                    }
                }
            }
            else if (rbtn_supplier.Checked)
            {
                if (Convert.ToInt32(this.dgv_productsOrSuppliers.Rows.Count) == 0) MessageBox.Show("Não há fornecedor selecionado para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (Convert.ToInt32(this.dgv_productsOrSuppliers.SelectedRows.Count) > 1) MessageBox.Show("Selecione apenas uma linha da tabela para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    foreach (DataGridViewRow dataGridViewRow in this.dgv_productsOrSuppliers.SelectedRows)
                    {
                        string id = dataGridViewRow.Cells[0].Value.ToString();
                        if (Application.OpenForms.OfType<AddSupplier>().Count() == 0)
                        {
                            DataTable dataTableSupplier = Database.query("SELECT * FROM fornecedor WHERE idFornecedor = " + id + ";");
                            AddSupplier addSupplier = new AddSupplier(dataTableSupplier);
                            addSupplier.editClick = true;
                            addSupplier.Show();
                            this.Close();
                        }
                    }
                }
            }
        }

        //BUSCAR PRODUTO OU FORNECEDOR NA TABELA
        private void txt_searchProductOrSupplier_TextChange(object sender, EventArgs e)
        {
            if (rbtn_products.Checked)
            {
                string searchProduct = this.txt_searchProductOrSupplier.Text;
                if (searchProduct.Trim() != null) formatDataTable("SELECT produto.idProduto, produto.nomeProduto AS 'Nome do produto:', fornecedor.nomeFornecedor AS 'Nome do fornecedor:', categoria.nomeCategoria AS 'Categoria do produto:', produto.valorUnitario AS 'Valor unitário:' FROM produto JOIN categoria ON produto.idCategoria = categoria.idCategoria JOIN fornecedor ON fornecedor.idFornecedor = produto.idFornecedor WHERE (produto.nomeProduto LIKE '%" + searchProduct + "%' OR fornecedor.nomeFornecedor LIKE '%" + searchProduct + "%') AND categoria.categoriaProduto = true ORDER BY produto.nomeProduto;", Globals.productDataTable, false);
            } else if (rbtn_supplier.Checked)
            {
                string searchSupplier = this.txt_searchProductOrSupplier.Text;
                if (searchSupplier.Trim() != null) formatDataTable("SELECT fornecedor.idFornecedor, fornecedor.nomeFornecedor AS 'Nome do fornecedor:', fornecedor.enderecoFornecedor, fornecedor.numeroResidencia, fornecedor.bairroFornecedor, fornecedor.cidadeFornecedor, fornecedor.estadoFornecedor, fornecedor.emailFornecedor FROM fornecedor WHERE fornecedor.nomeFornecedor LIKE '%" + searchSupplier + "%' ORDER BY fornecedor.nomeFornecedor;", Globals.supplierDataTable, true);
            }
        }

        //ADICIONAR PRODUTO OU FORNECEDOR
        private void pcb_btnAdd_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnAdd.Image = Properties.Resources.btn_add_active;
        }

        private void pcb_btnAdd_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnAddTag.ClientRectangle.Contains(lbl_btnAddTag.PointToClient(Cursor.Position))) this.pcb_btnAdd.Image = Properties.Resources.btn_add;
        }

        private void pcb_btnAdd_Click(object sender, EventArgs e)
        {
            this.addProductOrSupplier();
        }

        private void lbl_btnAddTag_Click(object sender, EventArgs e)
        {
            this.addProductOrSupplier();
        }

        //EDITAR PRODUTO OU FORNECEDOR
        private void pcb_btnEdit_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnEdit.Image = Properties.Resources.btn_edit_active;
        }

        private void pcb_btnEdit_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnEditTag.ClientRectangle.Contains(lbl_btnEditTag.PointToClient(Cursor.Position))) this.pcb_btnEdit.Image = Properties.Resources.btn_edit;
        }

        private void pcb_btnEdit_Click(object sender, EventArgs e)
        {
            this.editProductOrSupplier();
        }

        private void lbl_btnEditTag_Click(object sender, EventArgs e)
        {
            this.editProductOrSupplier();
        }

        //ENCERRAR APLICAÇÃO
        private void pcb_appClose_Click(object sender, EventArgs e)
        {
            if (((DialogResult)MessageBox.Show("Tem certeza que deseja encerrar a aplicação?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)).ToString().ToUpper() == "YES") Application.Exit();
        }

        //MINIMIZAR APLICAÇÃO
        private void pcb_minimizeProgram_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO

        //Formatação da tabela após disposição dos dados
        private void dgv_productsOrSuppliers_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            int width = 0;
            int columnCount = 0;
            foreach (DataGridViewColumn dataGridViewColumn in dgv_productsOrSuppliers.Columns) dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            if (dgv_productsOrSuppliers.ColumnCount > 0)
            {
                this.dgv_productsOrSuppliers.Columns[0].Visible = false;
                if(rbtn_supplier.Checked) for (int i = 2; i < 7; i++) this.dgv_productsOrSuppliers.Columns[i].Visible = false;
                if (rbtn_products.Checked) dgv_productsOrSuppliers.Columns[4].DefaultCellStyle.Format = "C";
                for (int i = 1; i < dgv_productsOrSuppliers.ColumnCount; i++)
                {
                    dgv_productsOrSuppliers.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    if (this.dgv_productsOrSuppliers.Columns[i].Visible)
                    {
                        width += Convert.ToInt32(dgv_productsOrSuppliers.Columns[i].Width);
                        columnCount++;
                    }
                }
                if (width < dgv_productsOrSuppliers.Width)
                {
                    width = dgv_productsOrSuppliers.Width / columnCount;
                    for (int i = 1; i < dgv_productsOrSuppliers.ColumnCount; i++)
                    {
                        dgv_productsOrSuppliers.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        dgv_productsOrSuppliers.Columns[i].Width = width;
                    }
                }
            }
        }

        private void rbtn_supplier_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_supplier.Checked)
            {
                this.txt_searchProductOrSupplier.PlaceholderText = "Nome do fornecedor";
                this.lbl_btnAddTag.Text = "ADICIONAR FORNECEDOR//";
                this.lbl_btnEditTag.Text = "EDITAR FORNECEDOR///";
                this.dgv_productsOrSuppliers.DataSource = null;
                formatDataTable("SELECT fornecedor.idFornecedor, fornecedor.nomeFornecedor AS 'Nome do fornecedor:', fornecedor.enderecoFornecedor, fornecedor.numeroResidencia, fornecedor.bairroFornecedor, fornecedor.cidadeFornecedor, fornecedor.estadoFornecedor, fornecedor.emailFornecedor FROM fornecedor ORDER BY fornecedor.nomeFornecedor;", Globals.supplierDataTable, true);
            }
        }

        private void rbtn_products_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_products.Checked)
            {
                this.txt_searchProductOrSupplier.PlaceholderText = "Nome do produto";
                this.lbl_btnAddTag.Text = "ADICIONAR PRODUTO///";
                this.lbl_btnEditTag.Text = "EDITAR PRODUTO///";
                this.dgv_productsOrSuppliers.DataSource = null;
                formatDataTable("SELECT produto.idProduto, produto.nomeProduto AS 'Nome do produto:', fornecedor.nomeFornecedor AS 'Nome do fornecedor:', categoria.nomeCategoria AS 'Categoria do produto:', produto.valorUnitario AS 'Valor unitário:' FROM produto JOIN categoria ON produto.idCategoria = categoria.idCategoria JOIN fornecedor ON fornecedor.idFornecedor = produto.idFornecedor WHERE categoria.categoriaProduto = true ORDER BY produto.nomeProduto;", Globals.productDataTable, false);
            }
        }
    }
}
