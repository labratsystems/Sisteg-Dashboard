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
    public partial class ProductStep : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        private BudgetForm productStepBudgetForm;
        private DataTable productStepDataTable, productStepBudgetedProduct;
        private int item = 0;
        private int idReceita;
        private List<Parcel> parcels = new List<Parcel>();

        public ProductStep(BudgetForm budgetForm, DataTable dataTable)
        {
            InitializeComponent();
            //Popula o combobox de produtos
            productStepDataTable = Database.query("SELECT produto.nomeProduto FROM produto ORDER BY produto.nomeProduto;");
            for (int i = 0; i < productStepDataTable.Rows.Count; i++) this.cbb_productName.Items.Insert(i, " " + productStepDataTable.Rows[i].ItemArray[0].ToString());
            
            productStepBudgetForm = budgetForm;
            productStepDataTable = dataTable;
            this.pcb_btnUpdate.Visible = false;
            this.pcb_btnDelete.Visible = false;

            //Produtos do orçamento
            productStepBudgetedProduct = Database.query("SELECT produtoOrcado.idProdutoOrcado, produtoOrcado.item AS 'Item:', produtoOrcado.quantidadeProduto AS 'Quantidade:', produto.nomeProduto AS 'Nome do produto:', produto.valorUnitario AS 'Valor unitário:', produtoOrcado.valorTotal AS 'Valor total:' FROM produtoOrcado JOIN produto ON produtoOrcado.idProduto = produto.idProduto WHERE produtoOrcado.numeroOrcamento = " + this.productStepDataTable.Rows[0].ItemArray[0] + " ORDER BY produtoOrcado.item;");

            if (productStepBudgetedProduct.Rows.Count > 0)
            {
                //Há produtos no orçamento
                List<BudgetedProduct> budgetedProducts = new List<BudgetedProduct>();
                int i = 0;
                item = 0;

                //Atualiza o número do item do produto orçado
                foreach (DataRow dataRow in productStepBudgetedProduct.Rows)
                {
                    budgetedProducts.Add(new BudgetedProduct());
                    budgetedProducts[i].idProdutoOrcado = Convert.ToInt32(dataRow.ItemArray[0]);
                    budgetedProducts[i].item = item + 1;
                    this.item = budgetedProducts[i].item;
                    if (Database.updateBudgetedProductItemValue(budgetedProducts[i])) continue;
                    i++;
                }

                //Atualiza o dataGridView se houveram mudanças
                productStepBudgetedProduct = Database.query("SELECT produtoOrcado.idProdutoOrcado, produtoOrcado.item AS 'Item:', produtoOrcado.quantidadeProduto AS 'Quantidade:', produto.nomeProduto AS 'Nome do produto:', produto.valorUnitario AS 'Valor unitário:', produtoOrcado.valorTotal AS 'Valor total:' FROM produtoOrcado JOIN produto ON produtoOrcado.idProduto = produto.idProduto WHERE produtoOrcado.numeroOrcamento = " + this.productStepDataTable.Rows[0].ItemArray[0] + " ORDER BY produtoOrcado.item;");
                this.dgv_budgetedProduct.DataSource = productStepBudgetedProduct;

                //Verifica o número do último item disponível no banco de dados e o atribui a variável
                this.pcb_btnUpdate.Visible = true;
                this.pcb_btnDelete.Visible = true;
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

        private void clearFields()
        {
            this.cbb_productCategory.Text = " Categoria do produto";
            this.cbb_productName.SelectedIndex = -1;
            this.cbb_productName.Text = " Nome do produto";
            this.txt_productQuantity.Clear();
        }

        //BUSCAR PRODUTO
        private void txt_searchProduct_TextChange(object sender, EventArgs e)
        {
            string searchProduct = this.txt_searchProduct.Text;
            if (searchProduct.Trim() != null)
            {
                DataTable searchProductDataTable = Database.query("SELECT produto.nomeProduto FROM produto WHERE produto.nomeProduto LIKE '%" + searchProduct.Trim() + "%' ORDER BY produto.nomeProduto;");
                this.cbb_productName.Items.Clear();
                for (int i = 0; i < searchProductDataTable.Rows.Count; i++) this.cbb_productName.Items.Insert(i, " " + searchProductDataTable.Rows[i].ItemArray[0].ToString());
            }
        }

        //SELECIONAR CATEGORIA DO PRODUTO
        private void cbb_productCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable productCategoryDataTable = Database.query("SELECT produto.nomeProduto FROM produto WHERE produto.categoriaProduto = '" + this.cbb_productCategory.SelectedItem.ToString().Trim() + "';");
            this.cbb_productName.Items.Clear();
            for (int i = 0; i < productCategoryDataTable.Rows.Count; i++) this.cbb_productName.Items.Insert(i, " " + productCategoryDataTable.Rows[i].ItemArray[0].ToString());
        }

        private void updateItemNumber()
        {
            List<BudgetedProduct> budgetedProducts = new List<BudgetedProduct>();
            int i = 0;
            this.item = 0;
            foreach (DataRow dataRow in productStepBudgetedProduct.Rows)
            {
                budgetedProducts.Add(new BudgetedProduct());
                budgetedProducts[i].idProdutoOrcado = Convert.ToInt32(dataRow.ItemArray[0]);
                budgetedProducts[i].item = item + 1;
                this.item = budgetedProducts[i].item;
                MessageBox.Show("Item " + budgetedProducts[i].item.ToString());
                if (Database.updateBudgetedProductItemValue(budgetedProducts[i])) continue;
                else MessageBox.Show("[ERROR] Item: " + i.ToString());
                i++;
            }

            //Atualiza o dataGridView com o novo produto adicionado
            productStepBudgetedProduct = Database.query("SELECT produtoOrcado.idProdutoOrcado, produtoOrcado.item AS 'Item:', produtoOrcado.quantidadeProduto AS 'Quantidade:', produto.nomeProduto AS 'Nome do produto:', produto.valorUnitario AS 'Valor unitário:', produtoOrcado.valorTotal AS 'Valor total:' FROM produtoOrcado JOIN produto ON produtoOrcado.idProduto = produto.idProduto WHERE produtoOrcado.numeroOrcamento = " + this.productStepDataTable.Rows[0].ItemArray[0] + " ORDER BY produtoOrcado.item;");
            if (productStepBudgetedProduct.Rows.Count > 0) this.dgv_budgetedProduct.DataSource = productStepBudgetedProduct;
        }

        private void updateBudgetTotalValue(BudgetedProduct budgetedProduct)
        {
            //Atualiza o dataGridView com o novo produto adicionado
            productStepBudgetedProduct = Database.query("SELECT produtoOrcado.idProdutoOrcado, produtoOrcado.item AS 'Item:', produtoOrcado.quantidadeProduto AS 'Quantidade:', produto.nomeProduto AS 'Nome do produto:', produto.valorUnitario AS 'Valor unitário:', produtoOrcado.valorTotal AS 'Valor total:' FROM produtoOrcado JOIN produto ON produtoOrcado.idProduto = produto.idProduto WHERE produtoOrcado.numeroOrcamento = " + this.productStepDataTable.Rows[0].ItemArray[0] + " ORDER BY produtoOrcado.item;");
            if (productStepBudgetedProduct.Rows.Count > 0) this.dgv_budgetedProduct.DataSource = productStepBudgetedProduct;

            //Soma o valor total de cada produto do orçamento
            decimal valorTotalProdutos = 0;
            foreach (DataRow dataRow in productStepBudgetedProduct.Rows) valorTotalProdutos += Convert.ToDecimal(dataRow.ItemArray[5]);
            decimal valorTotalOrcamento = valorTotalProdutos + Convert.ToDecimal(productStepDataTable.Rows[0].ItemArray[3]);

            //Atualiza o valor total do orçamento
            if (Database.updateBudgetTotalValue(budgetedProduct, valorTotalOrcamento))
            {
                //Seleciona a receita vinculada ao orçamento
                DataTable incomesDataTable = Database.query("SELECT * FROM receita WHERE numeroOrcamento = " + budgetedProduct.numeroOrcamento);

                if (Convert.ToBoolean(incomesDataTable.Rows[0].ItemArray[9]) == true)
                {
                    //Receita parcelada
                    if (Convert.ToBoolean(incomesDataTable.Rows[0].ItemArray[12]) == true)
                    {
                        valorTotalOrcamento = valorTotalOrcamento / Convert.ToInt32(incomesDataTable.Rows[0].ItemArray[13]);
                        if (Database.updateIncomeTotalValue(budgetedProduct, valorTotalOrcamento))
                        {
                            DataTable parcelsDataTable = Database.query("SELECT idParcela FROM parcela WHERE idReceita = " + incomesDataTable.Rows[0].ItemArray[0]);
                            int success = 1;
                            int i = 0;
                            this.parcels.Clear();
                            foreach (DataRow dataRow in parcelsDataTable.Rows)
                            {
                                parcels.Add(new Parcel());
                                parcels[i].idParcela = Convert.ToInt32(dataRow.ItemArray[0]);
                                parcels[i].valorParcela = valorTotalOrcamento;
                                if (Database.updateParcelValue(parcels[i]))
                                {
                                    i++;
                                    continue;
                                }
                                else
                                {
                                    success = 0;
                                    break;
                                }
                            }
                            this.clearFields();
                            this.pcb_btnUpdate.Visible = true;
                            this.pcb_btnDelete.Visible = true;
                            if (success == 0)
                            {
                                MessageBox.Show("[ERRO] Não foi possível atualizar o valor das parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else MessageBox.Show("[ERRO] Não foi possível atualizar o valor total do orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    //Receita não parcelada
                    if (Database.updateIncomeTotalValue(budgetedProduct, valorTotalOrcamento))
                    {
                        this.clearFields();
                        this.pcb_btnUpdate.Visible = true;
                        this.pcb_btnDelete.Visible = true;
                    }
                    else MessageBox.Show("[ERRO] Não foi possível atualizar o valor total do orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                this.updateItemNumber();
            }
            else MessageBox.Show("[ERRO] Não foi possível atualizar o valor total do orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //ADICIONAR PRODUTO ORÇADO
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
            //Definição do número do orçamento e busca do id do produto e do seu valor unitário
            BudgetedProduct budgetedProduct = new BudgetedProduct();
            budgetedProduct.numeroOrcamento = Convert.ToInt32(this.productStepDataTable.Rows[0].ItemArray[0]);
            DataTable productDataTable = Database.query("SELECT produto.idProduto, produto.valorUnitario FROM produto WHERE produto.nomeProduto = '" + this.cbb_productName.SelectedItem.ToString().Trim() + "';");
            
            //Verifica se o produto já está no orçamento
            foreach(DataGridViewRow dataGridViewRow in dgv_budgetedProduct.Rows)
            {
                if (dataGridViewRow.Cells[3].Value.ToString().Equals(this.cbb_productName.SelectedItem.ToString().Trim()))
                {
                    MessageBox.Show("O produto já está no orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.clearFields();
                    return;
                }
            }

            //Define o número do item no orçamento
            budgetedProduct.idProduto = Convert.ToInt32(productDataTable.Rows[0].ItemArray[0]);
            budgetedProduct.item = item + 1;
            item = item + 1;

            //Calcula o valor total de cada produto de acordo com seu preço e quantidade
            if (String.IsNullOrEmpty(this.txt_productQuantity.Text)) budgetedProduct.quantidadeProduto = 1; else budgetedProduct.quantidadeProduto = Convert.ToInt32(this.txt_productQuantity.Text);
            decimal valorUnitario = Convert.ToDecimal(productDataTable.Rows[0].ItemArray[1]);
            budgetedProduct.valorTotal = budgetedProduct.quantidadeProduto * valorUnitario;

            //Adiciona produto ao orçamento
            if (Database.newBudgetedProduct(budgetedProduct)) this.updateBudgetTotalValue(budgetedProduct);
            else MessageBox.Show("[ERRO] Não foi possível cadastrar produto orçado!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if ((Convert.ToInt32(this.dgv_budgetedProduct.Rows.Count) == 0) || ((this.cbb_productName.SelectedIndex == -1) || (String.IsNullOrEmpty(this.txt_productQuantity.Text)))) MessageBox.Show("Não há produto selecionado para atualizar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Convert.ToInt32(this.dgv_budgetedProduct.SelectedRows.Count) > 1) MessageBox.Show("Selecione apenas uma linha da tabela para atualizar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                BudgetedProduct budgetedProduct = new BudgetedProduct();
                budgetedProduct.idProdutoOrcado = Convert.ToInt32(this.dgv_budgetedProduct.SelectedRows[0].Cells[0].Value);
                budgetedProduct.numeroOrcamento = Convert.ToInt32(this.productStepDataTable.Rows[0].ItemArray[0]);

                //Verifica se o produto já está no orçamento
                DataTable productDataTable = Database.query("SELECT produto.idProduto, produto.valorUnitario FROM produto WHERE produto.nomeProduto = '" + this.cbb_productName.SelectedItem.ToString().Trim() + "';");
                foreach (DataGridViewRow dataGridViewRow in dgv_budgetedProduct.Rows)
                {
                    if (dataGridViewRow.Cells[3].Value.ToString().Equals(this.cbb_productName.SelectedItem.ToString()))
                    {
                        if (dgv_budgetedProduct.SelectedRows[0].Cells[3].Value.ToString().Equals(this.cbb_productName.SelectedItem.ToString())) continue;
                        else
                        {
                            MessageBox.Show("O produto já está no orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                budgetedProduct.idProduto = Convert.ToInt32(productDataTable.Rows[0].ItemArray[0]);
                budgetedProduct.item = Convert.ToInt32(this.dgv_budgetedProduct.SelectedRows[0].Cells[1].Value);
                if (String.IsNullOrEmpty(this.txt_productQuantity.Text)) budgetedProduct.quantidadeProduto = 1; else budgetedProduct.quantidadeProduto = Convert.ToInt32(this.txt_productQuantity.Text);

                //Atribui o valor total pela multiplicação do valor unitário pela quantidade do produto
                decimal valorUnitario = Convert.ToDecimal(productDataTable.Rows[0].ItemArray[1]);
                budgetedProduct.valorTotal = budgetedProduct.quantidadeProduto * valorUnitario;
                if (Database.updateBudgetedProduct(budgetedProduct))
                {
                    this.updateBudgetTotalValue(budgetedProduct);
                }
                else MessageBox.Show("[ERRO] Não foi possível atualizar produto orçado!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //EXCLUSÃO DE PRODUTO ORÇADO
        private void pcb_btnDelete_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnDelete.Image = Properties.Resources.btn_delete_active;
        }

        private void pcb_btnDelete_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnDelete.Image = Properties.Resources.btn_delete;
        }

        private void dgv_budgetedProduct_Click(object sender, EventArgs e)
        {
            if (this.dgv_budgetedProduct.Rows.Count == 1)
            {
                this.cbb_productName.SelectedIndex = this.cbb_productName.FindString(this.dgv_budgetedProduct.SelectedRows[0].Cells[3].Value.ToString());
                this.txt_productQuantity.Text = this.dgv_budgetedProduct.SelectedRows[0].Cells[2].Value.ToString();
            }
        }

        private void pcb_btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dgv_budgetedProduct.Rows.Count == 1)
            {
                this.cbb_productName.SelectedIndex = this.cbb_productName.FindString(this.dgv_budgetedProduct.SelectedRows[0].Cells[3].Value.ToString());
                this.txt_productQuantity.Text = this.dgv_budgetedProduct.SelectedRows[0].Cells[2].Value.ToString();
            }
            BudgetedProduct budgetedProduct = new BudgetedProduct();
            budgetedProduct.idProdutoOrcado = Convert.ToInt32(this.dgv_budgetedProduct.SelectedRows[0].Cells[0].Value);
            budgetedProduct.numeroOrcamento = Convert.ToInt32(productStepDataTable.Rows[0].ItemArray[0]);
            if (Database.deleteBudgetedProduct(budgetedProduct)) this.updateBudgetTotalValue(budgetedProduct);
            else MessageBox.Show("[ERRO] Não foi possível excluir produto orçado!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO
        private void txt_productQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void dgv_budgetedProduct_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.dgv_budgetedProduct.Columns[4].DefaultCellStyle.Format = "C";
            this.dgv_budgetedProduct.Columns[5].DefaultCellStyle.Format = "C";
            foreach (DataGridViewColumn dataGridViewColumn in dgv_budgetedProduct.Columns)
            {
                dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            int col = dgv_budgetedProduct.ColumnCount;
            if (col > 0)
            {
                this.dgv_budgetedProduct.Columns[0].Visible = false;
                for (int i = 1; i < (col - 2); i++)
                {
                    dgv_budgetedProduct.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                dgv_budgetedProduct.Columns[col - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void dgv_budgetedProduct_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_budgetedProduct.Focused)
            {
                if (dgv_budgetedProduct.SelectedRows.Count > 0)
                {
                    if (dgv_budgetedProduct.SelectedRows[0].Cells.Count > 2)
                    {
                        this.cbb_productName.SelectedIndex = this.cbb_productName.FindString(" " + this.dgv_budgetedProduct.SelectedRows[0].Cells[3].Value.ToString());
                        this.txt_productQuantity.Text = this.dgv_budgetedProduct.SelectedRows[0].Cells[2].Value.ToString().Trim();
                    }
                }
            }
        }
    }
}
