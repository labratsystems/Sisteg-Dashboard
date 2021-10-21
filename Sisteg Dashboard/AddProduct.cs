using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class AddProduct : Form
    {
        //CARREGA INSTÂNCIA DO PAINEL CONJUNTO À UM DATATABLE DE PRODUTO PARA ATUALIZAÇÃO OU EXCLUSÃO, QUE PODE SER NULO EM CASO DE CADASTRO
        public AddProduct(DataTable dataTable)
        {
            InitializeComponent();
            Globals.productDataTable = dataTable;
            this.txt_productName.Focus();

            //Popula o combobox de fornecedor do produto
            DataTable dataTableSuppliers = Database.query("SELECT fornecedor.nomeFornecedor FROM fornecedor;");
            for (int i = 0; i < dataTableSuppliers.Rows.Count; i++) this.cbb_supplierName.Items.Insert(i, " " + dataTableSuppliers.Rows[i].ItemArray[0].ToString().Trim());

            //Popula o combobox de categoria do produto
            DataTable productCategoryDataTable = Database.query("SELECT categoria.nomeCategoria FROM categoria WHERE categoria.categoriaProduto = true ORDER BY categoria.nomeCategoria;");
            for (int i = 0; i < productCategoryDataTable.Rows.Count; i++) this.cbb_productCategory.Items.Insert(i, " " + productCategoryDataTable.Rows[i].ItemArray[0].ToString().Trim());

            if (Globals.productDataTable != null)
            {
                foreach (DataRow dataRowProduct in Globals.productDataTable.Rows)
                {
                    Globals.idProduto = Convert.ToInt32(dataRowProduct.ItemArray[0]);
                    cbb_supplierName.SelectedIndex = cbb_supplierName.FindString(" " + Database.query("SELECT fornecedor.nomeFornecedor FROM fornecedor WHERE idFornecedor = " + Convert.ToInt32(dataRowProduct.ItemArray[1])).Rows[0].ItemArray[0].ToString().Trim());
                    cbb_productCategory.SelectedIndex = cbb_productCategory.FindString(" " + Database.query("SELECT categoria.nomeCategoria FROM categoria WHERE idCategoria = " + Convert.ToInt32(dataRowProduct.ItemArray[2])).Rows[0].ItemArray[0].ToString().Trim());
                    this.txt_productName.Text = dataRowProduct.ItemArray[3].ToString().Trim();
                    this.txt_productValue.Text = String.Format("{0:C}", dataRowProduct.ItemArray[4]).Trim();
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

        //FUNÇÕES

        //Função que cadastra produto
        private void productRegister()
        {
            if ((String.IsNullOrEmpty(txt_productName.Text.Trim())) || (this.cbb_supplierName.SelectedIndex == -1) || (this.cbb_productCategory.SelectedIndex == -1) || (String.IsNullOrEmpty(txt_productValue.Text.Trim()))) MessageBox.Show("Preencha todos os campos para cadastrar produto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if ((Database.query("SELECT nomeProduto FROM produto WHERE nomeProduto = '" + this.txt_productName.Text.Trim() + "';")).Rows.Count > 0) MessageBox.Show("Já existe um produto cadastrado com esse nome!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    Product product = new Product();
                    DataTable dataTable = Database.query("SELECT idFornecedor FROM fornecedor WHERE nomeFornecedor = '" + cbb_supplierName.SelectedItem.ToString().Trim() + "';");
                    product.IdFornecedor = Convert.ToInt32(dataTable.Rows[0].ItemArray[0]);
                    product.IdCategoria = Convert.ToInt32(Database.query("SELECT idCategoria FROM categoria WHERE nomeCategoria = '" + this.cbb_productCategory.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    product.NomeProduto = txt_productName.Text.Trim();
                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                    string valorUnitario = txt_productValue.Text.Trim();
                    if (regexValor.IsMatch(valorUnitario))
                    {
                        if (valorUnitario.Contains("R$ ")) product.ValorUnitario = Convert.ToDecimal(valorUnitario.Substring(3).Trim());
                        else if (valorUnitario.Contains("R$")) product.ValorUnitario = Convert.ToDecimal(valorUnitario.Substring(2).Trim());
                        else product.ValorUnitario = Convert.ToDecimal(txt_productValue.Text.Trim());
                    }
                    else MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    if (Database.newProduct(product))
                    {
                        MessageBox.Show("Produto cadastrado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txt_productName.Clear();
                        cbb_supplierName.SelectedIndex = -1;
                        cbb_productCategory.SelectedIndex = -1;
                        cbb_supplierName.Text = " Fornecedor";
                        cbb_productCategory.Text = " Categoria";
                        txt_productValue.Clear();
                        txt_productName.PlaceholderText = "";
                        txt_productName.Focus();
                    }
                    else MessageBox.Show("[ERRO] Não foi possível cadastrar produto!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Função que atualiza produto
        private void productUpdate()
        {
            if ((String.IsNullOrEmpty(txt_productName.Text.Trim())) || (this.cbb_supplierName.SelectedIndex == -1) || (this.cbb_productCategory.SelectedIndex == -1) || (String.IsNullOrEmpty(txt_productValue.Text.Trim()))) MessageBox.Show("Preencha todos os campos para cadastrar produto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if ((Database.query("SELECT nomeProduto FROM produto WHERE nomeProduto = '" + this.txt_productName.Text.Trim() + "';")).Rows.Count > 0) MessageBox.Show("Já existe um produto cadastrado com esse nome!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    Product product = new Product();
                    product.IdProduto = Globals.idProduto;
                    
                    product.IdFornecedor = Convert.ToInt32(Database.query("SELECT idFornecedor FROM fornecedor WHERE nomeFornecedor = '" + cbb_supplierName.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    product.IdCategoria = Convert.ToInt32(Database.query("SELECT idCategoria FROM categoria WHERE nomeCategoria = '" + this.cbb_productCategory.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                    
                    product.NomeProduto = txt_productName.Text.Trim();
                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                    string valorUnitario = txt_productValue.Text.Trim();
                    if (regexValor.IsMatch(valorUnitario))
                    {
                        if (valorUnitario.Contains("R$ ")) product.ValorUnitario = Convert.ToDecimal(valorUnitario.Substring(3).Trim());
                        else if (valorUnitario.Contains("R$")) product.ValorUnitario = Convert.ToDecimal(valorUnitario.Substring(2).Trim());
                        else product.ValorUnitario = Convert.ToDecimal(txt_productValue.Text.Trim());
                    }
                    else MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (Database.updateProduct(product)) MessageBox.Show("Produto atualizado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else MessageBox.Show("[ERRO] Não foi possível atualizar o produto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        //Função que exclui produto
        private void productDelete()
        {
            Product product = new Product();
            product.IdProduto = Globals.idProduto;
            DataTable productsDataTable = Database.query("SELECT * FROM produtoOrcado WHERE idProduto = " + Globals.idProduto);
            if (Database.deleteAllBudgetedProducts(product))
            {
                if (Database.deleteProduct(product)) MessageBox.Show("Produto atualizado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else MessageBox.Show("[ERRO] Não foi possível excluir produto!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (Application.OpenForms.OfType<ProductForm>().Count() == 0)
                {
                    ProductForm productForm = new ProductForm();
                    productForm.Show();
                    this.Close();
                }
            }
        }

        //CADASTRO DE PRODUTO
        private void pcb_productRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnProductRegister.BackgroundImage = Properties.Resources.btn_product_form_active;
        }

        private void pcb_productRegister_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_productRegisterTag.ClientRectangle.Contains(lbl_productRegisterTag.PointToClient(Cursor.Position))) this.pcb_btnProductRegister.BackgroundImage = Properties.Resources.btn_product_form;
        }

        private void pcb_productRegister_Click(object sender, EventArgs e)
        {
            this.productRegister();
        }

        private void lbl_productRegisterTag_Click(object sender, EventArgs e)
        {
            this.productRegister();
        }

        //ATUALIZAR PRODUTO
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
            this.productUpdate();
        }

        private void lbl_btnUpdateTag_Click(object sender, EventArgs e)
        {
            this.productUpdate();
        }

        //EXCLUIR PRODUTO
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
            this.productDelete();
        }

        private void lbl_btnDeleteTag_Click(object sender, EventArgs e)
        {
            this.productDelete();
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
            if (Application.OpenForms.OfType<ProductForm>().Count() == 0)
            {
                ProductForm product = new ProductForm();
                product.Show();
                this.Close();
            }
        }

        private void lbl_btnCancelTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ProductForm>().Count() == 0)
            {
                ProductForm product = new ProductForm();
                product.Show();
                this.Close();
            }
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO

        //KeyPress
        private void txt_productValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ',') && (e.KeyChar != 'R') && (e.KeyChar != '$') && (e.KeyChar != ' ')) { e.Handled = true; }
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1)) e.Handled = true;
        }

        //Leave
        private void txt_productName_Leave(object sender, EventArgs e)
        {
            this.txt_productName.PlaceholderText = "Nome";
        }
    }
}
