using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class ClientForm : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        DataTable dataTableClient;

        //INICIA INSTÂNCIA DO PAINEL, POPULANDO A TABELA DE LISTAGEM DE CLIENTES
        public ClientForm()
        {
            InitializeComponent();
            dataTableRemoveEmptyColumns("SELECT cliente.idCliente, cliente.nomeCliente AS 'Nome:', cliente.enderecoCliente AS 'Endereço:', cliente.numeroResidencia AS 'Número residencial:', cliente.cidadeCliente AS 'Cidade:', cliente.estadoCliente AS 'Estado:', cliente.primeiroTelefoneCliente AS 'Primeiro telefone:', cliente.tipoPrimeiroTelefoneCliente AS 'Tipo do primeiro telefone:', cliente.segundoTelefoneCliente AS 'Segundo telefone:', cliente.tipoSegundoTelefoneCliente AS 'Tipo do segundo telefone:', cliente.terceiroTelefoneCliente AS 'Terceiro telefone:', cliente.tipoTerceiroTelefoneCliente AS 'Tipo do terceiro telefone:' FROM cliente ORDER BY cliente.nomeCliente;");
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
        private void dataTableRemoveEmptyColumns(string query)
        {
            dataTableClient = Database.query(query);
            for(int col = dataTableClient.Columns.Count - 1; col >= 0; col--)
            {
                bool removeColumn = true;
                foreach(DataRow dataRow in dataTableClient.Rows)
                {
                    if (!dataRow.IsNull(col))
                    {
                        removeColumn = false;
                        break;
                    }
                }
                if (removeColumn)
                {
                    dataTableClient.Columns.RemoveAt(col);
                }
            }
            this.dgv_clients.DataSource = dataTableClient;
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

        private void pcb_btnProduct_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnProduct.Image = Properties.Resources.btn_product_active;
        }

        private void pcb_btnProduct_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnProduct.Image = Properties.Resources.btn_product;
        }

        private void pcb_btnProduct_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Product>().Count() == 0)
            {
                Product product = new Product();
                product.Show();
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
        private void dgv_clients_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn dataGridViewColumn in dgv_clients.Columns)
            {
                dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            int col = dgv_clients.ColumnCount;
            if (col > 0)
            {
                this.dgv_clients.Columns[0].Visible = false;
                for(int i = 1; i < col; i++)
                {
                    dgv_clients.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }
        }

        //BUSCAR CLIENTE NA TABELA
        private void txt_searchClient_TextChange(object sender, EventArgs e)
        {
            string searchClient = this.txt_searchClient.Text;
            dataTableRemoveEmptyColumns("SELECT cliente.idCliente, cliente.nomeCliente AS 'Nome:', cliente.enderecoCliente AS 'Endereço:', cliente.numeroResidencia AS 'Número residencial:', cliente.cidadeCliente AS 'Cidade:', cliente.estadoCliente AS 'Estado:', cliente.primeiroTelefoneCliente AS 'Primeiro telefone:', cliente.tipoPrimeiroTelefoneCliente AS 'Tipo do primeiro telefone:', cliente.primeiroTelefoneCliente AS 'Primeiro telefone:', cliente.tipoSegundoTelefoneCliente AS 'Tipo do segundo telefone:', cliente.terceiroTelefoneCliente AS 'Terceiro telefone:', cliente.tipoTerceiroTelefoneCliente AS 'Tipo do terceiro telefone:' FROM cliente WHERE cliente.nomeCliente LIKE '%" + searchClient + "%' ORDER BY cliente.nomeCliente;");
        }

        //PAGAR PARCELA
        private void pcb_btnPay_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnPay.Image = Properties.Resources.btn_payParcel_active;
        }

        private void pcb_btnPay_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnPay.Image = Properties.Resources.btn_payParcel;
        }

        //ADICIONAR CLIENTE
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
            if (Application.OpenForms.OfType<AddClient>().Count() == 0)
            {
                AddClient addClient = new AddClient(null);
                addClient.pcb_btnUpdate.Hide();
                addClient.pcb_btnDelete.Hide();
                addClient.pcb_clientRegister.Location = new Point(628, 312);
                addClient.Show();
                this.Close();
            }
        }

        //EDITAR CLIENTE
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
            foreach (DataGridViewRow dataGridViewRow in this.dgv_clients.SelectedRows)
            {
                string id = dataGridViewRow.Cells[0].Value.ToString();
                if (Application.OpenForms.OfType<AddIncome>().Count() == 0)
                {
                    DataTable dataTableClient = Database.query("SELECT * FROM cliente WHERE idCliente = " + id + ";");
                    AddClient addClient = new AddClient(dataTableClient);
                    addClient.Show();
                    this.Close();
                }
            }
        }

        //ENCERRA A APLICAÇÃO
        private void pcb_appClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
