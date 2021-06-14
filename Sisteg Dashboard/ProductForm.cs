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
    public partial class ProductForm : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        private DataTable productDataTable, supplierDataTable;
        private bool firstTime = true;

        //INICIA INSTÂNCIA DO PAINEL, POPULANDO A TABELA DE LISTAGEM DE PRODUTOS OU FORNECEDORES
        public ProductForm()
        {
            InitializeComponent();
            rbtn_products.Checked = true;
            dataTableRemoveEmptyColumns("SELECT produto.idProduto, produto.nomeProduto AS 'Nome do produto:', fornecedor.nomeFornecedor AS 'Nome do fornecedor:', produto.categoriaProduto AS 'Categoria do produto:', produto.valorUnitario AS 'Valor unitário:' FROM produto join fornecedor ON fornecedor.idFornecedor = produto.idFornecedor ORDER BY produto.nomeProduto;", productDataTable, false);
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

        //FUNÇÃO QUE REMOVE AS COLUNAS VAZIAS DA TABELA
        private void dataTableRemoveEmptyColumns(string query, DataTable dataTable, bool isSupplier)
        {
            dataTable = Database.query(query);
            for (int col = dataTable.Columns.Count - 1; col >= 0; col--)
            {
                bool removeColumn = true;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dataRow.ItemArray[col])))
                    {
                        removeColumn = false;
                        break;
                    }
                }
                if (removeColumn) dataTable.Columns.RemoveAt(col);
            }
            if (isSupplier)
            {
                if (dataTable.Rows.Count > 0)
                {
                    dataTable.Columns.Add("Endereço:", typeof(string));
                    dataTable.Columns.Add("E-mail:", typeof(string));
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i]["Endereço:"] = dataTable.Rows[i].ItemArray[2] + ", " + dataTable.Rows[i].ItemArray[3] + " - " + dataTable.Rows[i].ItemArray[4] + " - " + dataTable.Rows[i].ItemArray[5] + ", " + dataTable.Rows[i].ItemArray[6];
                        dataTable.Rows[i]["E-mail:"] = dataTable.Rows[i].ItemArray[7];
                        DataTable telephoneDataTable = Database.query("SELECT * FROM telefone WHERE idFornecedor = " + dataTable.Rows[i].ItemArray[0] + " ORDER BY tipoTelefone ASC;");
                        if (firstTime)
                        {
                            this.listTelephones(firstTime, telephoneDataTable, dataTable, i);
                        }
                        else this.listTelephones(firstTime, telephoneDataTable, dataTable, i);
                    }
                    this.dgv_productsOrSuppliers.DataSource = dataTable;
                }
            }
            else this.dgv_productsOrSuppliers.DataSource = dataTable;
        }

        //FUNÇÃO QUE LISTA OS TELEFONES DO CLIENTE NA TABELA DE CLIENTES
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

        //MENU DE NAVEGAÇÃO DA APLICAÇÃO
        private void pcb_btnMain_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnMain.Image = Properties.Resources.btn_main_form_active;
        }

        private void pcb_btnMain_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnMain.Image = Properties.Resources.btn_main_form;
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

        private void pcb_btnClient_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnClient.Image = Properties.Resources.btn_client_form_active;
        }

        private void pcb_btnClient_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnClient.Image = Properties.Resources.btn_client_form;
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

        private void pcb_btnBudget_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnBudget.Image = Properties.Resources.btn_budget_form_active;
        }

        private void pcb_btnBudget_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnBudget.Image = Properties.Resources.btn_budget_form;
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

        private void pcb_btnConfig_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnConfig.Image = Properties.Resources.btn_config_main_active;
        }

        private void pcb_btnConfig_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnConfig.Image = Properties.Resources.btn_config_main;
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

        //FORMATAÇÃO A TABELA APÓS A DISPOSIÇÃO DOS DADOS
        private void dgv_productsOrSuppliers_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.dgv_productsOrSuppliers.Rows.Count > 0)
            {
                foreach (DataGridViewColumn dataGridViewColumn in dgv_productsOrSuppliers.Columns) dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                int col = dgv_productsOrSuppliers.ColumnCount;
                if (col > 0)
                {
                    this.dgv_productsOrSuppliers.Columns[0].Visible = false;
                    for (int i = 1; i < (col - 2); i++) dgv_productsOrSuppliers.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    if (rbtn_products.Checked) dgv_productsOrSuppliers.Columns[4].DefaultCellStyle.Format = "C";
                    else for (int i = 2; i < 7; i++) this.dgv_productsOrSuppliers.Columns[i].Visible = false;
                }
            }
        }

        //BUSCAR PRODUTO OU FORNECEDOR NA TABELA
        private void txt_searchProductOrSupplier_TextChange(object sender, EventArgs e)
        {
            if (rbtn_products.Checked)
            {
                string searchProduct = this.txt_searchProductOrSupplier.Text;
                if (searchProduct.Trim() != null) { dataTableRemoveEmptyColumns("SELECT produto.idProduto, produto.nomeProduto AS 'Nome do produto:', fornecedor.nomeFornecedor AS 'Nome do fornecedor:', produto.categoriaProduto AS 'Categoria do produto:', produto.valorUnitario AS 'Valor unitário:' FROM produto join fornecedor ON fornecedor.idFornecedor = produto.idFornecedor WHERE produto.nomeProduto LIKE '%" + searchProduct + "%' OR fornecedor.nomeFornecedor LIKE '%" + searchProduct + "%' ORDER BY produto.nomeProduto;", productDataTable, false); }
            } else if (rbtn_supplier.Checked)
            {
                string searchSupplier = this.txt_searchProductOrSupplier.Text;
                if (searchSupplier.Trim() != null) { dataTableRemoveEmptyColumns("SELECT fornecedor.idFornecedor, fornecedor.nomeFornecedor AS 'Nome do fornecedor:', fornecedor.enderecoFornecedor, fornecedor.numeroResidencia, fornecedor.bairroFornecedor, fornecedor.cidadeFornecedor, fornecedor.estadoFornecedor, fornecedor.emailFornecedor FROM fornecedor WHERE fornecedor.nomeFornecedor LIKE '%" + searchSupplier + "%' ORDER BY fornecedor.nomeFornecedor;", supplierDataTable, true); }
            }
        }

        //ADICIONAR PRODUTO OU FORNECEDOR
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
            if (rbtn_products.Checked)
            {
                if (Application.OpenForms.OfType<AddProduct>().Count() == 0)
                {
                    AddProduct addProduct = new AddProduct(null);
                    addProduct.pcb_btnUpdate.Hide();
                    addProduct.pcb_btnDelete.Hide();
                    addProduct.pcb_btnProductRegister.Location = new Point(628, 312);
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
                    addSupplier.pcb_btnDelete.Hide();
                    addSupplier.pcb_btnSupplierRegister.Location = new Point(628, 312);
                    addSupplier.Show();
                    this.Close();
                }
            }
        }

        //EDITAR PRODUTO OU FORNECEDOR
        private void pcb_btnEdit_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnEdit.Image = Properties.Resources.btn_edit_active;
        }

        private void pcb_btnEdit_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnEdit.Image = Properties.Resources.btn_edit;
        }

        private void pcb_btnEdit_Click(object sender, EventArgs e)
        {
            if (rbtn_products.Checked)
            {
                if (Convert.ToInt32(this.dgv_productsOrSuppliers.Rows.Count) == 0) MessageBox.Show("Não há produto selecionado para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (Convert.ToInt32(this.dgv_productsOrSuppliers.SelectedRows.Count) > 1) MessageBox.Show("Selecione apenas uma linha da tabela para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    foreach (DataGridViewRow dataGridViewRow in this.dgv_productsOrSuppliers.SelectedRows)
                    {
                        string id = dataGridViewRow.Cells[0].Value.ToString();
                        if (Application.OpenForms.OfType<AddProduct>().Count() == 0)
                        {
                            DataTable dataTableProduct = Database.query("SELECT * FROM produto WHERE idProduto = " + id + ";");
                            AddProduct addProduct = new AddProduct(dataTableProduct);
                            addProduct.Show();
                            this.Close();
                        }
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

        //ENCERRAR A APLICAÇÃO
        private void pcb_appClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO
        private void rbtn_supplier_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_supplier.Checked)
            {
                this.txt_searchProductOrSupplier.PlaceholderText = "Nome do fornecedor";
                dataTableRemoveEmptyColumns("SELECT fornecedor.idFornecedor, fornecedor.nomeFornecedor AS 'Nome do fornecedor:', fornecedor.enderecoFornecedor, fornecedor.numeroResidencia, fornecedor.bairroFornecedor, fornecedor.cidadeFornecedor, fornecedor.estadoFornecedor, fornecedor.emailFornecedor FROM fornecedor ORDER BY fornecedor.nomeFornecedor;", supplierDataTable, true);
            }
        }

        private void rbtn_products_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_products.Checked)
            {
                this.txt_searchProductOrSupplier.PlaceholderText = "Nome do produto";
                dataTableRemoveEmptyColumns("SELECT produto.idProduto, produto.nomeProduto AS 'Nome do produto:', fornecedor.nomeFornecedor AS 'Nome do fornecedor:', produto.categoriaProduto AS 'Categoria do produto:', produto.valorUnitario AS 'Valor unitário:' FROM produto join fornecedor ON fornecedor.idFornecedor = produto.idFornecedor ORDER BY produto.nomeProduto;", productDataTable, false);
            }
        }
    }
}
