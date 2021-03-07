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
        private static DataTable dataTableSupplier;
        public static int idFornecedor;

        //CARREGA INSTÂNCIA DO PAINEL CONJUNTO À UM DATATABLE DE FORNECEDOR PARA ATUALIZAÇÃO OU EXCLUSÃO, QUE PODE SER NULO EM CASO DE CADASTRO
        public AddSupplier(DataTable dataTable)
        {
            InitializeComponent();
            dataTableSupplier = dataTable;
            if (dataTableSupplier != null)
            {
                foreach (DataRow dataRowSupplier in dataTableSupplier.Rows)
                {
                    if (dataTableSupplier.Rows.IndexOf(dataRowSupplier) == 0)
                    {
                        idFornecedor = Convert.ToInt32(dataRowSupplier.ItemArray[0]);
                        this.txt_supplierName.Text = dataRowSupplier.ItemArray[1].ToString();
                        this.txt_supplierAddress.Text = dataRowSupplier.ItemArray[2].ToString();
                        this.txt_supplierHouseNumber.Text = dataRowSupplier.ItemArray[3].ToString();
                        this.txt_supplierCity.Text = dataRowSupplier.ItemArray[4].ToString();
                        int index = cbb_supplierState.FindString(dataRowSupplier.ItemArray[5].ToString());
                        this.txt_supplierEmail.Text = dataRowSupplier.ItemArray[6].ToString();
                        this.cbb_supplierState.SelectedIndex = index;
                        DataTable dataTableTel = Database.query("SELECT primeiroTelefoneFornecedor, segundoTelefoneFornecedor, terceiroTelefoneFornecedor FROM fornecedor WHERE idFornecedor = " + idFornecedor + ";");
                        int rowsNumber = 0;
                        for (int col = dataTableTel.Columns.Count - 1; col >= 0; col--)
                        {
                            foreach (DataRow dataRow in dataTableTel.Rows)
                            {
                                if (!dataRow.IsNull(col))
                                {
                                    rowsNumber++;
                                }
                            }
                        }
                        int indexTel = cbb_supplierTelephoneQuantity.FindString(rowsNumber.ToString());
                        cbb_supplierTelephoneQuantity.SelectedIndex = indexTel;
                        for (int i = 1; i <= rowsNumber; i++)
                        {
                            if (rowsNumber == 1)
                            {
                                this.txt_firstTel.Show();
                                this.txt_firstTel.Text = dataTableSupplier.Rows[0].ItemArray[7].ToString();
                                this.cbb_firstTelephoneType.Show();
                                int indexFirstTel = cbb_firstTelephoneType.FindString(dataTableSupplier.Rows[0].ItemArray[8].ToString());
                                this.cbb_firstTelephoneType.SelectedIndex = indexFirstTel;
                                this.txt_secondTel.Hide();
                                this.cbb_secondTelephoneType.Hide();
                                this.txt_thirdTel.Hide();
                                this.cbb_thirdTelephoneType.Hide();
                            }
                            else if (rowsNumber == 2)
                            {
                                this.txt_firstTel.Show();
                                this.txt_firstTel.Text = dataTableSupplier.Rows[0].ItemArray[7].ToString();
                                this.cbb_firstTelephoneType.Show();
                                int indexFirstTel = cbb_firstTelephoneType.FindString(dataTableSupplier.Rows[0].ItemArray[8].ToString());
                                this.cbb_firstTelephoneType.SelectedIndex = indexFirstTel;
                                this.txt_secondTel.Show();
                                this.txt_secondTel.Text = dataTableSupplier.Rows[0].ItemArray[9].ToString();
                                this.cbb_secondTelephoneType.Show();
                                int indexSecondTel = cbb_secondTelephoneType.FindString(dataTableSupplier.Rows[0].ItemArray[10].ToString());
                                this.cbb_secondTelephoneType.SelectedIndex = indexSecondTel;
                                this.txt_thirdTel.Hide();
                                this.cbb_thirdTelephoneType.Hide();
                            }
                            else if (rowsNumber == 3)
                            {
                                this.txt_firstTel.Show();
                                this.txt_firstTel.Text = dataTableSupplier.Rows[0].ItemArray[7].ToString();
                                this.cbb_firstTelephoneType.Show();
                                int indexFirstTel = cbb_firstTelephoneType.FindString(dataTableSupplier.Rows[0].ItemArray[8].ToString());
                                this.cbb_firstTelephoneType.SelectedIndex = indexFirstTel;
                                this.txt_secondTel.Show();
                                this.txt_secondTel.Text = dataTableSupplier.Rows[0].ItemArray[9].ToString();
                                this.cbb_secondTelephoneType.Show();
                                int indexSecondTel = cbb_secondTelephoneType.FindString(dataTableSupplier.Rows[0].ItemArray[10].ToString());
                                this.cbb_secondTelephoneType.SelectedIndex = indexSecondTel;
                                this.txt_thirdTel.Show();
                                this.txt_thirdTel.Text = dataTableSupplier.Rows[0].ItemArray[11].ToString();
                                this.cbb_thirdTelephoneType.Show();
                                int indexThirdTel = cbb_thirdTelephoneType.FindString(dataTableSupplier.Rows[0].ItemArray[12].ToString());
                                this.cbb_thirdTelephoneType.SelectedIndex = indexThirdTel;
                            }
                        }
                    }
                }
            }
            else
            {
                int index = cbb_supplierTelephoneQuantity.FindString("1");
                cbb_supplierTelephoneQuantity.SelectedIndex = index;
                this.txt_firstTel.Show();
                this.cbb_firstTelephoneType.Show();
                this.txt_secondTel.Hide();
                this.cbb_secondTelephoneType.Hide();
                this.txt_thirdTel.Hide();
                this.cbb_thirdTelephoneType.Hide();
            }
            this.txt_supplierName.Select();
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

        //CADASTRAR FORNECEDOR
        private void pcb_supplierRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnSupplierRegister.Image = Properties.Resources.btn_supplierRegister_active;
        }

        private void pcb_supplierRegister_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnSupplierRegister.Image = Properties.Resources.btn_supplierRegister;
        }

        private void pcb_supplierRegister_Click(object sender, EventArgs e)
        {
            Supplier supplier = new Supplier();
            supplier.nomeFornecedor = txt_supplierName.Text;
            supplier.enderecoFornecedor = txt_supplierAddress.Text;
            supplier.numeroResidencia = txt_supplierHouseNumber.Text;
            supplier.cidadeFornecedor = txt_supplierCity.Text;
            supplier.estadoFornecedor = cbb_supplierState.SelectedItem.ToString();
            supplier.primeiroTelefoneFornecedor = txt_firstTel.Text;
            supplier.tipoPrimeiroTelefoneFornecedor = cbb_firstTelephoneType.SelectedItem.ToString();
            if ((this.cbb_secondTelephoneType.SelectedIndex != -1) && (!string.IsNullOrEmpty(this.txt_secondTel.Text)))
            {
                supplier.segundoTelefoneFornecedor = txt_secondTel.Text;
                supplier.tipoSegundoTelefoneFornecedor = cbb_secondTelephoneType.SelectedItem.ToString();
            }
            if ((this.cbb_thirdTelephoneType.SelectedIndex != -1) && (!string.IsNullOrEmpty(this.txt_thirdTel.Text)))
            {
                supplier.terceiroTelefoneFornecedor = txt_thirdTel.Text;
                supplier.tipoTerceiroTelefoneFornecedor = cbb_thirdTelephoneType.SelectedItem.ToString();
            }
            if (Database.newSupplier(supplier))
            {
                MessageBox.Show("Fornecedor cadastrado com sucesso!");
                txt_supplierName.Clear();
                txt_supplierName.Select();
                txt_supplierAddress.Clear();
                txt_supplierHouseNumber.Clear();
                txt_supplierCity.Clear();
                cbb_supplierState.SelectedIndex = -1;
                this.cbb_supplierState.Text = "Estado";
                txt_supplierEmail.Clear();
                cbb_supplierTelephoneQuantity.SelectedIndex = 0;
                txt_firstTel.Clear();
                cbb_firstTelephoneType.SelectedIndex = -1;
                this.cbb_firstTelephoneType.Text = "Tipo do primeiro telefone";
                txt_secondTel.Clear();
                cbb_secondTelephoneType.SelectedIndex = -1;
                this.cbb_secondTelephoneType.Text = "Tipo do segundo telefone";
                txt_thirdTel.Clear();
                cbb_thirdTelephoneType.SelectedIndex = -1;
                this.cbb_thirdTelephoneType.Text = "Tipo do terceiro telefone";
            }
            else
            {
                MessageBox.Show("Não foi possível cadastrar fornecedor!");
            }
        }

        //ATUALIZAR FORNECEDOR
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
            Supplier supplier = new Supplier();
            supplier.idFornecedor = idFornecedor;
            supplier.nomeFornecedor = txt_supplierName.Text;
            supplier.enderecoFornecedor = txt_supplierAddress.Text;
            supplier.numeroResidencia = txt_supplierHouseNumber.Text;
            supplier.cidadeFornecedor = txt_supplierCity.Text;
            supplier.estadoFornecedor = cbb_supplierState.SelectedItem.ToString();
            supplier.emailFornecedor = txt_supplierEmail.Text;
            supplier.primeiroTelefoneFornecedor = txt_firstTel.Text;
            supplier.tipoPrimeiroTelefoneFornecedor = cbb_firstTelephoneType.SelectedItem.ToString();
            if ((this.cbb_secondTelephoneType.SelectedIndex != -1) && (!string.IsNullOrEmpty(this.txt_secondTel.Text)))
            {
                supplier.segundoTelefoneFornecedor = txt_secondTel.Text;
                supplier.tipoSegundoTelefoneFornecedor = cbb_secondTelephoneType.SelectedItem.ToString();
            }
            else if ((this.cbb_secondTelephoneType.SelectedIndex == -1) && (string.IsNullOrEmpty(this.txt_secondTel.Text)))
            {
                supplier.segundoTelefoneFornecedor = null;
                supplier.tipoSegundoTelefoneFornecedor = null;
            }
            if ((this.cbb_thirdTelephoneType.SelectedIndex != -1) && (!string.IsNullOrEmpty(this.txt_thirdTel.Text)))
            {
                supplier.terceiroTelefoneFornecedor = txt_thirdTel.Text;
                supplier.tipoTerceiroTelefoneFornecedor = cbb_thirdTelephoneType.SelectedItem.ToString();
            }
            else if ((this.cbb_thirdTelephoneType.SelectedIndex == -1) && (string.IsNullOrEmpty(this.txt_thirdTel.Text)))
            {
                supplier.terceiroTelefoneFornecedor = null;
                supplier.tipoTerceiroTelefoneFornecedor = null;
            }
            if (Database.updateSupplier(supplier))
            {
                MessageBox.Show("Fornecedor atualizado com sucesso!");
                txt_supplierName.Focus();
            }
            else
            {
                MessageBox.Show("Não foi possível atualizar fornecedor!");
            }
        }

        //EXCLUIR FORNECEDOR
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
            Supplier supplier = new Supplier();
            supplier.idFornecedor = idFornecedor;
            if (Database.deleteAllProducts(supplier) && Database.deleteSupplier(supplier))
            {
                MessageBox.Show("Fornecedor excluído com sucesso!");
            }
            else
            {
                MessageBox.Show("Não foi possível excluir Fornecedor!");
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
                ProductForm productForm = new ProductForm();
                productForm.Show();
                this.Close();
            }
        }

        //ENCERRAR A APLICAÇÃO
        private void pcb_appClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cbb_supplierTelephoneQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbb_supplierTelephoneQuantity.SelectedItem.ToString() == "1")
            {
                var sampleSize = new System.Drawing.Size(425, 423);
                this.panel_client.AutoScrollMinSize = sampleSize;
                this.txt_secondTel.Hide();
                this.cbb_secondTelephoneType.Hide();
                this.txt_secondTel.Clear();
                this.txt_thirdTel.Hide();
                this.cbb_thirdTelephoneType.Hide();
                this.txt_thirdTel.Clear();
                this.cbb_secondTelephoneType.SelectedIndex = -1;
                this.cbb_secondTelephoneType.Text = "Tipo do segundo telefone";
                this.cbb_thirdTelephoneType.SelectedIndex = -1;
                this.cbb_thirdTelephoneType.Text = "Tipo do terceiro telefone";
            }
            else if (this.cbb_supplierTelephoneQuantity.SelectedItem.ToString() == "2")
            {
                var sampleSize = new System.Drawing.Size(425, 518);
                this.panel_client.AutoScrollMinSize = sampleSize;
                this.txt_secondTel.Show();
                this.cbb_secondTelephoneType.Show();
                this.txt_thirdTel.Hide();
                this.cbb_thirdTelephoneType.Hide();
                this.txt_thirdTel.Clear();
                this.cbb_thirdTelephoneType.SelectedIndex = -1;
                this.cbb_thirdTelephoneType.Text = "Tipo do terceiro telefone";
            }
            else if (this.cbb_supplierTelephoneQuantity.SelectedItem.ToString() == "3")
            {
                var sampleSize = new System.Drawing.Size(425, 613);
                this.panel_client.AutoScrollMinSize = sampleSize;
                this.txt_secondTel.Show();
                this.cbb_secondTelephoneType.Show();
                this.txt_thirdTel.Show();
                this.cbb_thirdTelephoneType.Show();
            }
        }
    }
}
