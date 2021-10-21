using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class ProductStep : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        private int item = 0;

        public ProductStep(BudgetForm budgetForm)
        {
            InitializeComponent();
            Globals.budgetForm = budgetForm;

            //Popula o combobox de produtos
            Globals.productStepDataTable = Database.query("SELECT produto.nomeProduto FROM produto ORDER BY produto.nomeProduto;");
            for (int i = 0; i < Globals.productStepDataTable.Rows.Count; i++) this.cbb_productName.Items.Insert(i, " " + Globals.productStepDataTable.Rows[i].ItemArray[0].ToString());

            //Popula o combobox de categoria do produto
            DataTable productCategoryDataTable = Database.query("SELECT categoria.nomeCategoria FROM categoria WHERE categoria.categoriaProduto = true ORDER BY categoria.nomeCategoria;");
            for (int i = 0; i < productCategoryDataTable.Rows.Count; i++) this.cbb_productCategory.Items.Insert(i, " " + productCategoryDataTable.Rows[i].ItemArray[0].ToString());

            this.lbl_btnUpdateTag.Visible = false;
            this.pcb_btnUpdate.Visible = false;
            this.lbl_btnDeleteTag.Visible = false;
            this.pcb_btnDelete.Visible = false;

            Globals.incomeDataTable = Database.query("SELECT * FROM receita WHERE numeroOrcamento = " + Globals.numeroOrcamento);
            Globals.productStepDataTable = Database.query("SELECT * FROM orcamento WHERE numeroOrcamento = " + Globals.numeroOrcamento);
            Globals.idReceita = Convert.ToInt32(Globals.incomeDataTable.Rows[0].ItemArray[0]);
            Globals.idConta = Convert.ToInt32(Globals.incomeDataTable.Rows[0].ItemArray[1]);

            if (Globals.idConta != 0)
            {
                DataTable sumTotalValueDataTable = Database.query("SELECT somarTotal FROM conta WHERE idConta = " + Globals.idConta);
                if (sumTotalValueDataTable.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(sumTotalValueDataTable.Rows[0].ItemArray[0])) Globals.saldoConta = Convert.ToDecimal(Database.query("SELECT saldoConta FROM conta WHERE idConta = " + Globals.idConta).Rows[0].ItemArray[0]);
                    else Globals.saldoConta = 0;
                }
            }

            //Produtos do orçamento
            Globals.budgetedProductDataTable = Database.query("SELECT produtoOrcado.idProdutoOrcado, produtoOrcado.item AS 'Item:', produtoOrcado.quantidadeProduto AS 'Quantidade:', produto.nomeProduto AS 'Nome do produto:', produto.valorUnitario AS 'Valor unitário:', produtoOrcado.valorTotal AS 'Valor total:' FROM produtoOrcado JOIN produto ON produtoOrcado.idProduto = produto.idProduto WHERE produtoOrcado.numeroOrcamento = " + Globals.numeroOrcamento + " ORDER BY produtoOrcado.item;");

            if (Globals.budgetedProductDataTable.Rows.Count > 0)
            {
                //Há produtos no orçamento
                List<BudgetedProduct> budgetedProducts = new List<BudgetedProduct>();
                int i = 0;
                item = 0;

                //Atualiza o número do item do produto orçado
                foreach (DataRow dataRow in Globals.budgetedProductDataTable.Rows)
                {
                    budgetedProducts.Add(new BudgetedProduct());
                    budgetedProducts[i].IdProdutoOrcado = Convert.ToInt32(dataRow.ItemArray[0]);
                    budgetedProducts[i].Item = item + 1;
                    this.item = budgetedProducts[i].Item;
                    if (Database.updateBudgetedProductItemValue(budgetedProducts[i])) continue;
                    i++;
                }

                //Atualiza o dataGridView se houveram mudanças
                Globals.budgetedProductDataTable = Database.query("SELECT produtoOrcado.idProdutoOrcado, produtoOrcado.item AS 'Item:', produtoOrcado.quantidadeProduto AS 'Quantidade:', produto.nomeProduto AS 'Nome do produto:', produto.valorUnitario AS 'Valor unitário:', produtoOrcado.valorTotal AS 'Valor total:' FROM produtoOrcado JOIN produto ON produtoOrcado.idProduto = produto.idProduto WHERE produtoOrcado.numeroOrcamento = " + Globals.numeroOrcamento + " ORDER BY produtoOrcado.item;");
                this.dgv_budgetedProduct.DataSource = Globals.budgetedProductDataTable;

                this.lbl_btnUpdateTag.Visible = true;
                this.pcb_btnUpdate.Visible = true;
                this.lbl_btnDeleteTag.Visible = true;
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

        //FUNÇÕES

        //Função que limpa os campos do formulário
        private void clearFields()
        {
            this.cbb_productCategory.Text = " Categoria do produto";
            this.cbb_productName.SelectedIndex = -1;
            this.cbb_productName.Text = " Nome do produto";
            this.txt_productQuantity.Clear();
            this.txt_productQuantity.Focus();
            this.txt_productQuantity.PlaceholderText = "";
        }

        //Função que atualiza o número de item dos produtos do orçamento
        private void updateItemNumber()
        {
            List<BudgetedProduct> budgetedProducts = new List<BudgetedProduct>();
            int i = 0;
            this.item = 0;
            foreach (DataRow dataRow in Globals.budgetedProductDataTable.Rows)
            {
                budgetedProducts.Add(new BudgetedProduct());
                budgetedProducts[i].IdProdutoOrcado = Convert.ToInt32(dataRow.ItemArray[0]);
                budgetedProducts[i].Item = item + 1;
                this.item = budgetedProducts[i].Item;
                if (Database.updateBudgetedProductItemValue(budgetedProducts[i])) continue;
                else MessageBox.Show("[ERROR] Item: " + i.ToString());
                i++;
            }

            //Atualiza o dataGridView com o novo produto adicionado
            Globals.budgetedProductDataTable = Database.query("SELECT produtoOrcado.idProdutoOrcado, produtoOrcado.item AS 'Item:', produtoOrcado.quantidadeProduto AS 'Quantidade:', produto.nomeProduto AS 'Nome do produto:', produto.valorUnitario AS 'Valor unitário:', produtoOrcado.valorTotal AS 'Valor total:' FROM produtoOrcado JOIN produto ON produtoOrcado.idProduto = produto.idProduto WHERE produtoOrcado.numeroOrcamento = " + Globals.numeroOrcamento + " ORDER BY produtoOrcado.item;");
            if (Globals.budgetedProductDataTable.Rows.Count > 0) this.dgv_budgetedProduct.DataSource = Globals.budgetedProductDataTable;
        }

        //Função que atualiza o valor total do orçamento
        private void updateBudgetTotalValue(BudgetedProduct budgetedProduct)
        {
            //Atualiza o dataGridView com o novo produto adicionado
            Globals.budgetedProductDataTable = Database.query("SELECT produtoOrcado.idProdutoOrcado, produtoOrcado.item AS 'Item:', produtoOrcado.quantidadeProduto AS 'Quantidade:', produto.nomeProduto AS 'Nome do produto:', produto.valorUnitario AS 'Valor unitário:', produtoOrcado.valorTotal AS 'Valor total:' FROM produtoOrcado JOIN produto ON produtoOrcado.idProduto = produto.idProduto WHERE produtoOrcado.numeroOrcamento = " + Globals.numeroOrcamento + " ORDER BY produtoOrcado.item;");
            if (Globals.budgetedProductDataTable.Rows.Count > 0) this.dgv_budgetedProduct.DataSource = Globals.budgetedProductDataTable;

            //Soma o valor total de cada produto do orçamento
            decimal valorTotalProdutos = 0;
            foreach (DataRow dataRow in Globals.budgetedProductDataTable.Rows) valorTotalProdutos += Convert.ToDecimal(dataRow.ItemArray[5]);
            decimal valorTotalOrcamento = valorTotalProdutos + Convert.ToDecimal(Globals.productStepDataTable.Rows[0].ItemArray[3]);

            //Atualiza o valor total do orçamento
            if (Database.updateBudgetTotalValue(budgetedProduct, valorTotalOrcamento))
            {
                //Seleciona a receita vinculada ao orçamento
                DataTable incomesDataTable = Database.query("SELECT * FROM receita WHERE numeroOrcamento = " + budgetedProduct.NumeroOrcamento);

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
                            Globals.parcels.Clear();
                            foreach (DataRow dataRow in parcelsDataTable.Rows)
                            {
                                Globals.parcels.Add(new Parcel());
                                Globals.parcels[i].IdParcela = Convert.ToInt32(dataRow.ItemArray[0]);
                                Globals.parcels[i].ValorParcela = valorTotalOrcamento;
                                if (Database.updateParcelValue(Globals.parcels[i]))
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
                        this.lbl_btnUpdateTag.Visible = true;
                        this.pcb_btnUpdate.Visible = true;
                        this.lbl_btnDeleteTag.Visible = true;
                        this.pcb_btnDelete.Visible = true;
                    }
                    else MessageBox.Show("[ERRO] Não foi possível atualizar o valor total do orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                this.updateItemNumber();
            }
            else MessageBox.Show("[ERRO] Não foi possível atualizar o valor total do orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Função que cadastra produto orçado
        private void budgetedProductRegister()
        {
            if (this.cbb_productName.SelectedIndex == -1) MessageBox.Show("Selecione um produto para adicioná-lo ao orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                //Definição do número do orçamento e busca do id do produto e do seu valor unitário
                Account account = new Account();
                BudgetedProduct budgetedProduct = new BudgetedProduct();
                account.IdConta = Globals.idConta;
                budgetedProduct.NumeroOrcamento = Globals.numeroOrcamento;
                DataTable productDataTable = Database.query("SELECT produto.idProduto, produto.valorUnitario FROM produto WHERE produto.nomeProduto = '" + this.cbb_productName.SelectedItem.ToString().Trim() + "';");

                //Verifica se o produto já está no orçamento
                foreach (DataGridViewRow dataGridViewRow in dgv_budgetedProduct.Rows)
                {
                    if (dataGridViewRow.Cells[3].Value.ToString().Equals(this.cbb_productName.SelectedItem.ToString().Trim()))
                    {
                        MessageBox.Show("O produto já está no orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.clearFields();
                        return;
                    }
                }

                //Define o número do item no orçamento
                budgetedProduct.IdProduto = Convert.ToInt32(productDataTable.Rows[0].ItemArray[0]);
                budgetedProduct.Item = item + 1;
                item = item + 1;

                //Calcula o valor total de cada produto de acordo com seu preço e quantidade
                if (String.IsNullOrEmpty(this.txt_productQuantity.Text.Trim())) budgetedProduct.QuantidadeProduto = 1; 
                else budgetedProduct.QuantidadeProduto = Convert.ToInt32(this.txt_productQuantity.Text.Trim());
                decimal valorUnitario = Convert.ToDecimal(productDataTable.Rows[0].ItemArray[1]);
                budgetedProduct.ValorTotal = budgetedProduct.QuantidadeProduto * valorUnitario;

                //Adiciona produto ao orçamento
                if (Database.newBudgetedProduct(budgetedProduct)) this.updateBudgetTotalValue(budgetedProduct);
                else MessageBox.Show("[ERRO] Não foi possível cadastrar produto orçado!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Função que atualiza produto orçado
        private void budgetedProductUpdate()
        {
            if (Convert.ToInt32(this.dgv_budgetedProduct.Rows.Count) == 0) MessageBox.Show("Não há produto selecionado para atualizar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Convert.ToInt32(this.dgv_budgetedProduct.SelectedRows.Count) > 1) MessageBox.Show("Selecione apenas uma linha da tabela para atualizar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (this.cbb_productName.SelectedIndex == -1) MessageBox.Show("Selecione um produto para atualizá-lo ao orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    Account account = new Account();
                    account.IdConta = Globals.idConta;
                    BudgetedProduct budgetedProduct = new BudgetedProduct();
                    budgetedProduct.IdProdutoOrcado = Convert.ToInt32(this.dgv_budgetedProduct.SelectedRows[0].Cells[0].Value);
                    budgetedProduct.NumeroOrcamento = Globals.numeroOrcamento;

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

                    budgetedProduct.IdProduto = Convert.ToInt32(productDataTable.Rows[0].ItemArray[0]);
                    budgetedProduct.Item = Convert.ToInt32(this.dgv_budgetedProduct.SelectedRows[0].Cells[1].Value);
                    if (String.IsNullOrEmpty(this.txt_productQuantity.Text)) budgetedProduct.QuantidadeProduto = 1; 
                    else budgetedProduct.QuantidadeProduto = Convert.ToInt32(this.txt_productQuantity.Text);

                    //Atribui o valor total pela multiplicação do valor unitário pela quantidade do produto
                    decimal valorUnitario = Convert.ToDecimal(productDataTable.Rows[0].ItemArray[1]);
                    budgetedProduct.ValorTotal = budgetedProduct.QuantidadeProduto * valorUnitario;
                    if (Database.updateBudgetedProduct(budgetedProduct)) this.updateBudgetTotalValue(budgetedProduct);
                    else MessageBox.Show("[ERRO] Não foi possível atualizar produto orçado!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Função que exclui produto orçado
        private void budgetedProductDelete()
        {
            if (Convert.ToInt32(this.dgv_budgetedProduct.Rows.Count) == 0) MessageBox.Show("Não há produto selecionado para excluir!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Convert.ToInt32(this.dgv_budgetedProduct.SelectedRows.Count) > 1) MessageBox.Show("Selecione apenas uma linha da tabela para excluir!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Account account = new Account();
                account.IdConta = Globals.idConta;
                BudgetedProduct budgetedProduct = new BudgetedProduct();
                budgetedProduct.IdProdutoOrcado = Convert.ToInt32(this.dgv_budgetedProduct.SelectedRows[0].Cells[0].Value);
                budgetedProduct.NumeroOrcamento = Globals.numeroOrcamento;
                if (Database.deleteBudgetedProduct(budgetedProduct)) this.updateBudgetTotalValue(budgetedProduct);
                else MessageBox.Show("[ERRO] Não foi possível excluir produto orçado!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            DataTable productCategoryDataTable = Database.query("SELECT produto.nomeProduto FROM produto WHERE idCategoria = (SELECT idCategoria FROM categoria WHERE nomeCategoria = '" + this.cbb_productCategory.SelectedItem.ToString().Trim() + "');");
            this.cbb_productName.Items.Clear();
            for (int i = 0; i < productCategoryDataTable.Rows.Count; i++) this.cbb_productName.Items.Insert(i, " " + productCategoryDataTable.Rows[i].ItemArray[0].ToString());
        }

        //ADICIONAR PRODUTO ORÇADO
        private void pcb_btnBudgetedProductRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnBudgetedProductRegister.Image = Properties.Resources.btn_add_budgeted_product_active;
        }

        private void pcb_btnBudgetedProductRegister_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnBudgetedProductRegisterTag.ClientRectangle.Contains(lbl_btnBudgetedProductRegisterTag.PointToClient(Cursor.Position))) this.pcb_btnBudgetedProductRegister.Image = Properties.Resources.btn_add_budgeted_product;
        }

        private void pcb_btnBudgetedProductRegister_Click(object sender, EventArgs e)
        {
            this.budgetedProductRegister();
        }

        private void lbl_btnBudgetedProductRegisterTag_Click(object sender, EventArgs e)
        {
            this.budgetedProductRegister();
        }

        //ATUALIZAÇÃO DE CLIENTE
        private void pcb_btnUpdate_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnUpdate.Image = Properties.Resources.btn_edit_active;
        }

        private void pcb_btnUpdate_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnUpdateTag.ClientRectangle.Contains(lbl_btnUpdateTag.PointToClient(Cursor.Position))) this.pcb_btnUpdate.Image = Properties.Resources.btn_edit;
        }

        private void pcb_btnUpdate_Click(object sender, EventArgs e)
        {
            this.budgetedProductUpdate();
        }

        private void lbl_btnUpdateTag_Click(object sender, EventArgs e)
        {
            this.budgetedProductUpdate();
        }

        //EXCLUSÃO DE PRODUTO ORÇADO
        private void pcb_btnDelete_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnDelete.Image = Properties.Resources.btn_delete_active;
        }

        private void pcb_btnDelete_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnDeleteTag.ClientRectangle.Contains(lbl_btnDeleteTag.PointToClient(Cursor.Position))) this.pcb_btnDelete.Image = Properties.Resources.btn_delete;
        }

        private void pcb_btnDelete_Click(object sender, EventArgs e)
        {
            this.budgetedProductDelete();
        }

        private void lbl_btnDeleteTag_Click(object sender, EventArgs e)
        {
            this.budgetedProductDelete();
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO

        //Formatação da tabela após disposição dos dados
        private void dgv_budgetedProduct_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.dgv_budgetedProduct.Columns[4].DefaultCellStyle.Format = "C";
            this.dgv_budgetedProduct.Columns[5].DefaultCellStyle.Format = "C";
            foreach (DataGridViewColumn dataGridViewColumn in dgv_budgetedProduct.Columns) dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            int width = 0;
            int columnCount = 0;
            if (dgv_budgetedProduct.ColumnCount > 0)
            {
                this.dgv_budgetedProduct.Columns[0].Visible = false;
                for (int i = 1; i < dgv_budgetedProduct.ColumnCount; i++)
                {
                    dgv_budgetedProduct.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    if (this.dgv_budgetedProduct.Columns[i].Visible)
                    {
                        width += Convert.ToInt32(dgv_budgetedProduct.Columns[i].Width);
                        columnCount++;
                    }
                }
                if (width < dgv_budgetedProduct.Width)
                {
                    width = dgv_budgetedProduct.Width / columnCount;
                    for (int i = 1; i < dgv_budgetedProduct.ColumnCount; i++)
                    {
                        dgv_budgetedProduct.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        dgv_budgetedProduct.Columns[i].Width = width;
                    }
                }
            }
        }

        //KeyPress
        private void txt_productQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        //Leave
        private void txt_productQuantity_Leave(object sender, EventArgs e)
        {
            this.txt_productQuantity.PlaceholderText = "Quantidade do produto";
        }
    }
}
