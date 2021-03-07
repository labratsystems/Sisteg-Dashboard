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
            for (int i = 0; i < dataTableSuppliers.Rows.Count; i++)
            {
                this.cbb_supplierName.Items.Insert(i, dataTableSuppliers.Rows[i].ItemArray[0].ToString());
            }
            if (dataTableProduct != null)
            {
                foreach (DataRow dataRowProduct in dataTableProduct.Rows)
                {
                    idProduto = Convert.ToInt32(dataRowProduct.ItemArray[0]);
                    this.txt_productName.Text = dataRowProduct.ItemArray[2].ToString();
                    DataTable dataTableSuppliersName = Database.query("SELECT fornecedor.nomeFornecedor FROM fornecedor WHERE idFornecedor = " + Convert.ToInt32(dataRowProduct.ItemArray[1])  + ";");        
                    int indexProduct = cbb_supplierName.FindString(dataTableSuppliersName.Rows[0].ItemArray[0].ToString());
                    cbb_supplierName.SelectedIndex = indexProduct;
                    string money = String.Format("{0:C}", dataRowProduct.ItemArray[3]);
                    this.txt_productValue.Text = String.Format("{0:C}", dataRowProduct.ItemArray[3]);
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
            Product product = new Product();
            string supplierName = cbb_supplierName.SelectedItem.ToString();
            DataTable dataTable = Database.query("SELECT idFornecedor FROM fornecedor WHERE nomeFornecedor = '" + supplierName + "';");
            product.idFornecedor = Convert.ToInt32(dataTable.Rows[0].ItemArray[0]);
            product.nomeProduto = txt_productName.Text;
            Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
            string valorUnitario = txt_productValue.Text;
            if (regexValor.IsMatch(valorUnitario))
            {
                if (valorUnitario.Contains("R$ ")) { product.valorUnitario = Convert.ToDecimal(valorUnitario.Substring(3)); }
                else if (valorUnitario.Contains("R$")) { product.valorUnitario = Convert.ToDecimal(valorUnitario.Substring(2)); } 
                else { product.valorUnitario = Convert.ToDecimal(txt_productValue.Text); }
            }
            else
            {
                MessageBox.Show("Formato monetário incorreto!");
            }
            
            if (Database.newProduct(product))
            {
                MessageBox.Show("Produto cadastrado com sucesso!");
                txt_productName.Clear();
                txt_productName.Focus();
                cbb_supplierName.SelectedIndex = -1;
                cbb_supplierName.Text = "Fornecedor";
                txt_productValue.Clear();
            }
            else
            {
                MessageBox.Show("Não foi possível cadastrar produto!");
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
            Product product = new Product();
            product.idProduto = idProduto;
            string supplierName = cbb_supplierName.SelectedItem.ToString();
            DataTable dataTable = Database.query("SELECT idFornecedor FROM fornecedor WHERE nomeFornecedor = '" + supplierName + "';");
            product.idFornecedor = Convert.ToInt32(dataTable.Rows[0].ItemArray[0]);
            product.nomeProduto = txt_productName.Text;
            Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
            string valorUnitario = txt_productValue.Text;
            if (regexValor.IsMatch(valorUnitario))
            {
                if (valorUnitario.Contains("R$ ")) { product.valorUnitario = Convert.ToDecimal(valorUnitario.Substring(3)); }
                else if (valorUnitario.Contains("R$")) { product.valorUnitario = Convert.ToDecimal(valorUnitario.Substring(2)); }
                else { product.valorUnitario = Convert.ToDecimal(txt_productValue.Text); }
            }
            else
            {
                MessageBox.Show("Formato monetário incorreto!");
            }

            if (Database.updateProduct(product))
            {
                MessageBox.Show("Produto atualizado com sucesso!");
                txt_productName.Focus();
            }
            else
            {
                MessageBox.Show("Não foi possível atualizar produto!");
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
            if (Database.deleteProduct(product))
            {
                MessageBox.Show("Produto excluído com sucesso!");
            }
            else
            {
                MessageBox.Show("Não foi possível excluir produto!");
            }
            if (Application.OpenForms.OfType<ProductForm>().Count() == 0)
            {
                ProductForm productForm = new ProductForm();
                productForm.Show();
                this.Close();
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
    }
}
