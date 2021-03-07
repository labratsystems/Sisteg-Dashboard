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
        DataTable dataTableProduct;
        DataTable dataTableSupplier;

        //INICIA INSTÂNCIA DO PAINEL, POPULANDO A TABELA DE LISTAGEM DE PRODUTOS OU FORNECEDORES
        public ProductForm()
        {
            InitializeComponent();
            rbtn_products.Checked = true;
            dataTableRemoveEmptyColumns("SELECT produto.idProduto, produto.nomeProduto AS 'Nome do produto:', fornecedor.nomeFornecedor AS 'Nome do fornecedor:', produto.valorUnitario AS 'Valor unitário:' FROM produto join fornecedor ON fornecedor.idFornecedor = produto.idFornecedor ORDER BY produto.nomeProduto;", dataTableProduct);
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
        private void dataTableRemoveEmptyColumns(string query, DataTable dataTable)
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
                if (removeColumn)
                {
                    dataTable.Columns.RemoveAt(col);
                }
            }
            this.dgv_productsOrSuppliers.DataSource = dataTable;
        }

        //MENU DE NAVEGAÇÃO DA APLICAÇÃO
        private void pcb_btnMain_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnMain.Image = Properties.Resources.btn_main_active;
        }

        private void pcb_btnMain_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnMain.Image = Properties.Resources.btn_main;
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
            this.pcb_btnClient.Image = Properties.Resources.btn_client_active;
        }

        private void pcb_btnClient_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnClient.Image = Properties.Resources.btn_client;
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
            this.pcb_btnBudget.Image = Properties.Resources.btn_budget_active;
        }

        private void pcb_btnBudget_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnBudget.Image = Properties.Resources.btn_budget;
        }

        private void pcb_btnBudget_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Budget>().Count() == 0)
            {
                Budget budget = new Budget();
                budget.Show();
                this.Close();
            }
        }

        private void pcb_btnConfig_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnConfig.Image = Properties.Resources.btn_config_active;
        }

        private void pcb_btnConfig_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnConfig.Image = Properties.Resources.btn_config;
        }

        private void pcb_btnConfig_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Config>().Count() == 0)
            {
                Config config = new Config();
                config.Show();
                this.Close();
            }
        }

        //FORMATAÇÃO A TABELA APÓS A DISPOSIÇÃO DOS DADOS
        private void dgv_productsOrSuppliers_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (rbtn_products.Checked)
            {
                dgv_productsOrSuppliers.Columns[3].DefaultCellStyle.Format = "C";
            }
            foreach (DataGridViewColumn dataGridViewColumn in dgv_productsOrSuppliers.Columns)
            {
                dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            int col = dgv_productsOrSuppliers.ColumnCount;
            if (col > 0)
            {
                this.dgv_productsOrSuppliers.Columns[0].Visible = false;
                for (int i = 1; i < (col - 2); i++)
                {
                    dgv_productsOrSuppliers.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (rbtn_products.Checked)
                {
                    dgv_productsOrSuppliers.Columns[3].DefaultCellStyle.Format = "C";
                    dgv_productsOrSuppliers.Columns[col - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        //BUSCAR PRODUTO OU FORNECEDOR NA TABELA
        private void txt_searchProductOrSupplier_TextChange(object sender, EventArgs e)
        {
            if (rbtn_products.Checked)
            {
                string searchProduct = this.txt_searchProductOrSupplier.Text;
                dataTableRemoveEmptyColumns("SELECT produto.idProduto, produto.nomeProduto AS 'Nome do produto:', fornecedor.nomeFornecedor AS 'Nome do fornecedor:', produto.valorUnitario AS 'Valor unitário:' FROM produto join fornecedor ON fornecedor.idFornecedor = produto.idFornecedor WHERE produto.nomeProduto LIKE '%" + searchProduct + "%' OR fornecedor.nomeFornecedor LIKE '%" + searchProduct + "%' ORDER BY produto.nomeProduto;", dataTableProduct);
            } else if (rbtn_supplier.Checked)
            {
                string searchSupplier = this.txt_searchProductOrSupplier.Text;
                dataTableRemoveEmptyColumns("SELECT fornecedor.idFornecedor, fornecedor.nomeFornecedor AS 'Nome do fornecedor:', fornecedor.enderecoFornecedor AS 'Endereço:', fornecedor.numeroResidencia AS 'Número residencial:', fornecedor.cidadeFornecedor AS 'Cidade:', fornecedor.estadoFornecedor AS 'Estado:', fornecedor.emailFornecedor AS 'E-mail:', fornecedor.primeiroTelefoneFornecedor AS 'Primeiro telefone:', fornecedor.tipoPrimeiroTelefoneFornecedor AS 'Tipo do primeiro telefone:', fornecedor.segundoTelefoneFornecedor AS 'Segundo telefone:', fornecedor.tipoSegundoTelefoneFornecedor AS 'Tipo do segundo telefone:', fornecedor.terceiroTelefoneFornecedor AS 'Terceiro telefone:', fornecedor.tipoTerceiroTelefoneFornecedor AS 'Tipo do terceiro telefone:' FROM fornecedor WHERE fornecedor.nomeFornecedor LIKE '%" + searchSupplier + "%' ORDER BY fornecedor.nomeFornecedor;", dataTableSupplier);
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
            this.pcb_btnEdit.Image = Properties.Resources.btn_modify_active;
        }

        private void pcb_btnEdit_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnEdit.Image = Properties.Resources.btn_modify;
        }

        private void pcb_btnEdit_Click(object sender, EventArgs e)
        {
            if (rbtn_products.Checked)
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
            else if (rbtn_supplier.Checked)
            {
                foreach (DataGridViewRow dataGridViewRow in this.dgv_productsOrSuppliers.SelectedRows)
                {
                    string id = dataGridViewRow.Cells[0].Value.ToString();
                    if (Application.OpenForms.OfType<AddSupplier>().Count() == 0)
                    {
                        DataTable dataTableSupplier = Database.query("SELECT * FROM fornecedor WHERE idFornecedor = " + id + ";");
                        AddSupplier addSupplier = new AddSupplier(dataTableSupplier);
                        addSupplier.Show();
                        this.Close();
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
                dataTableRemoveEmptyColumns("SELECT fornecedor.idFornecedor, fornecedor.nomeFornecedor AS 'Nome do fornecedor:', fornecedor.enderecoFornecedor AS 'Endereço:', fornecedor.numeroResidencia AS 'Número residencial:', fornecedor.cidadeFornecedor AS 'Cidade:', fornecedor.estadoFornecedor AS 'Estado:', fornecedor.emailFornecedor AS 'E-mail:', fornecedor.primeiroTelefoneFornecedor AS 'Primeiro telefone:', fornecedor.tipoPrimeiroTelefoneFornecedor AS 'Tipo do primeiro telefone:', fornecedor.segundoTelefoneFornecedor AS 'Segundo telefone:', fornecedor.tipoSegundoTelefoneFornecedor AS 'Tipo do segundo telefone:', fornecedor.terceiroTelefoneFornecedor AS 'Terceiro telefone:', fornecedor.tipoTerceiroTelefoneFornecedor AS 'Tipo do terceiro telefone:' FROM fornecedor ORDER BY fornecedor.nomeFornecedor;", dataTableSupplier);
            }
        }

        private void rbtn_products_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_products.Checked)
            {
                this.txt_searchProductOrSupplier.PlaceholderText = "Nome do produto";
                dataTableRemoveEmptyColumns("SELECT produto.idProduto, produto.nomeProduto AS 'Nome do produto:', fornecedor.nomeFornecedor AS 'Nome do fornecedor:', produto.valorUnitario AS 'Valor unitário:' FROM produto join fornecedor ON fornecedor.idFornecedor = produto.idFornecedor ORDER BY produto.nomeProduto;", dataTableProduct);
            }
        }
    }
}
