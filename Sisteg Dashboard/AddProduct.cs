using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class AddProduct : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        private static DataTable dataTableProduct;
        public static int idProduto;

        //CARREGA INSTÂNCIA DO PAINEL CONJUNTO À UM DATATABLE DE PRODUTO PARA ATUALIZAÇÃO OU EXCLUSÃO, QUE PODE SER NULO EM CASO DE CADASTRO
        public AddProduct(DataTable dataTable)
        {
            InitializeComponent();
            dataTableProduct = dataTable;
            this.txt_productName.Focus();
            DataTable dataTableSuppliers = Database.query("SELECT fornecedor.nomeFornecedor FROM fornecedor;");
            for (int i = 0; i < dataTableSuppliers.Rows.Count; i++) this.cbb_supplierName.Items.Insert(i, " " + dataTableSuppliers.Rows[i].ItemArray[0].ToString());
            if (dataTableProduct != null)
            {
                foreach (DataRow dataRowProduct in dataTableProduct.Rows)
                {
                    idProduto = Convert.ToInt32(dataRowProduct.ItemArray[0]);
                    this.txt_productName.Text = dataRowProduct.ItemArray[2].ToString();
                    DataTable dataTableSuppliersName = Database.query("SELECT fornecedor.nomeFornecedor FROM fornecedor WHERE idFornecedor = " + Convert.ToInt32(dataRowProduct.ItemArray[1])  + ";");        
                    cbb_supplierName.SelectedIndex = cbb_supplierName.FindString(" " + dataTableSuppliersName.Rows[0].ItemArray[0].ToString());
                    cbb_productCategory.SelectedIndex = cbb_productCategory.FindString(" " + dataRowProduct.ItemArray[3].ToString());
                    this.txt_productValue.Text = String.Format("{0:C}", dataRowProduct.ItemArray[4]);
                }
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

        //CADASTRO DE PRODUTO
        private void pcb_productRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnProductRegister.Image = Properties.Resources.btn_productRegister_active;
        }

        private void pcb_productRegister_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnProductRegister.Image = Properties.Resources.btn_productRegister;
        }

        private void pcb_productRegister_Click(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(txt_productName.Text.Trim())) || (this.cbb_supplierName.SelectedIndex == -1) || (this.cbb_productCategory.SelectedIndex == -1) || (String.IsNullOrEmpty(txt_productValue.Text.Trim()))) MessageBox.Show("Preencha todos os campos para cadastrar produto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Product product = new Product();
                DataTable dataTable = Database.query("SELECT idFornecedor FROM fornecedor WHERE nomeFornecedor = '" + cbb_supplierName.SelectedItem.ToString().Trim() + "';");
                product.idFornecedor = Convert.ToInt32(dataTable.Rows[0].ItemArray[0]);
                product.nomeProduto = txt_productName.Text;
                product.categoriaProduto = cbb_productCategory.SelectedItem.ToString().Trim();
                Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                string valorUnitario = txt_productValue.Text;
                if (regexValor.IsMatch(valorUnitario))
                {
                    if (valorUnitario.Contains("R$ ")) { product.valorUnitario = Convert.ToDecimal(valorUnitario.Substring(3)); }
                    else if (valorUnitario.Contains("R$")) { product.valorUnitario = Convert.ToDecimal(valorUnitario.Substring(2)); }
                    else { product.valorUnitario = Convert.ToDecimal(txt_productValue.Text); }
                }
                else MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (Database.newProduct(product))
                {
                    MessageBox.Show("Produto cadastrado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_productName.Clear();
                    cbb_supplierName.SelectedIndex = -1;
                    cbb_productCategory.SelectedIndex = -1;
                    cbb_supplierName.Text = " Fornecedor";
                    txt_productValue.Clear();
                    txt_productName.PlaceholderText = "";
                    txt_productName.Focus();
                }
                else MessageBox.Show("Não foi possível cadastrar produto!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //ATUALIZAR PRODUTO
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
            if ((String.IsNullOrEmpty(txt_productName.Text.Trim())) || (this.cbb_supplierName.SelectedIndex == -1) || (this.cbb_productCategory.SelectedIndex == -1) || (String.IsNullOrEmpty(txt_productValue.Text.Trim()))) MessageBox.Show("Preencha todos os campos para cadastrar produto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Product product = new Product();
                product.idProduto = idProduto;
                DataTable SupplierIdDataTable = Database.query("SELECT idFornecedor FROM fornecedor WHERE nomeFornecedor = '" + cbb_supplierName.SelectedItem.ToString().Trim() + "';");
                product.idFornecedor = Convert.ToInt32(SupplierIdDataTable.Rows[0].ItemArray[0]);
                product.nomeProduto = txt_productName.Text;
                product.categoriaProduto = cbb_productCategory.SelectedItem.ToString().Trim();
                Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                string valorUnitario = txt_productValue.Text;
                if (regexValor.IsMatch(valorUnitario))
                {
                    if (valorUnitario.Contains("R$ ")) product.valorUnitario = Convert.ToDecimal(valorUnitario.Substring(3));
                    else if (valorUnitario.Contains("R$")) product.valorUnitario = Convert.ToDecimal(valorUnitario.Substring(2));
                    else product.valorUnitario = Convert.ToDecimal(txt_productValue.Text);
                }
                else MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (Database.updateProduct(product))
                {
                    DataTable budgetsDataTable = Database.query("SELECT * FROM orcamento;");
                    foreach (DataRow budgetsDataRow in budgetsDataTable.Rows)
                    {
                        DataTable productStepBudgetedProduct = Database.query("SELECT produtoOrcado.idProdutoOrcado, produtoOrcado.item AS 'Item:', produtoOrcado.quantidadeProduto AS 'Quantidade:', produto.nomeProduto AS 'Nome do produto:', produto.valorUnitario AS 'Valor unitário:', produtoOrcado.valorTotal AS 'Valor total:' FROM produtoOrcado JOIN produto ON produtoOrcado.idProduto = produto.idProduto WHERE produtoOrcado.numeroOrcamento = " + budgetsDataRow.ItemArray[0] + " ORDER BY produtoOrcado.item;");
                        if (productStepBudgetedProduct.Rows.Count > 0)
                        {
                            List<BudgetedProduct> budgetedProducts = new List<BudgetedProduct>();
                            int i = 0;
                            foreach (DataRow dataRow in productStepBudgetedProduct.Rows)
                            {
                                budgetedProducts.Add(new BudgetedProduct());
                                budgetedProducts[i].idProdutoOrcado = Convert.ToInt32(dataRow.ItemArray[0]);
                                budgetedProducts[i].item = i + 1;
                                budgetedProducts[i].numeroOrcamento = Convert.ToInt32(budgetsDataTable.Rows[0].ItemArray[0]);
                                DataTable productDataTable = Database.query("SELECT idProduto FROM produto WHERE nomeProduto = '" + dataRow.ItemArray[3] + "';");
                                budgetedProducts[i].idProduto = Convert.ToInt32(productDataTable.Rows[0].ItemArray[0]);
                                budgetedProducts[i].quantidadeProduto = Convert.ToInt32(dataRow.ItemArray[2]);
                                decimal valorUnitarioProduct = Convert.ToDecimal(dataRow.ItemArray[4]);
                                budgetedProducts[i].valorTotal = budgetedProducts[i].quantidadeProduto * valorUnitarioProduct;
                                if (Database.updateBudgetedProduct(budgetedProducts[i]))
                                {
                                    decimal valorTotal = 0;
                                    valorTotal = valorTotal + Convert.ToDecimal(dataRow.ItemArray[5]);
                                    MessageBox.Show("Valor total: " + valorTotal.ToString());
                                    valorTotal = valorTotal + Convert.ToDecimal(budgetsDataTable.Rows[0].ItemArray[4]);
                                    if (Database.updateBudgetTotalValue(budgetedProducts[i], valorTotal))
                                        if (Database.updateIncomeTotalValue(budgetedProducts[i], valorTotal)) continue;
                                        else MessageBox.Show("[ERRO] Não foi possível atualizar o valor total do orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    else MessageBox.Show("[ERRO] Não foi possível atualizar o valor total do orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else MessageBox.Show("[ERRO] Não foi possível atualizar produto orçado!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                if (Database.updateBudgetedProductItemValue(budgetedProducts[i])) continue;
                                i++;
                            }
                            MessageBox.Show("Produto atualizado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txt_productName.PlaceholderText = "";
                            txt_productName.Focus();
                        }
                        else MessageBox.Show("[ERRO] Não foi possível atualizar produto!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //EXCLUIR PRODUTO
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
            Product product = new Product();
            product.idProduto = idProduto;
            DataTable productsDataTable = Database.query("SELECT * FROM produtoOrcado WHERE idProduto =" + idProduto.ToString());
            if (Database.deleteAllBudgetedProducts(product))
            {
                if (Database.deleteProduct(product)) MessageBox.Show("Produto excluído com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information); else MessageBox.Show("Não foi possível excluir produto!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (Application.OpenForms.OfType<ProductForm>().Count() == 0)
                {
                    ProductForm productForm = new ProductForm();
                    productForm.Show();
                    this.Close();
                }
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
            if (Application.OpenForms.OfType<ProductForm>().Count() == 0)
            {
                ProductForm product = new ProductForm();
                product.Show();
                this.Close();
            }
        }

        private void txt_productValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ',') && (e.KeyChar != 'R') && (e.KeyChar != '$') && (e.KeyChar != ' ')) { e.Handled = true; }
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1)) e.Handled = true;
        }

        private void txt_productName_Leave(object sender, EventArgs e)
        {
            this.txt_productName.PlaceholderText = "Nome";
        }
    }
}
