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
    public partial class AddClient : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        private static DataTable dataTableClient;
        public static int idCliente;

        //CARREGA INSTÂNCIA DO PAINEL CONJUNTO À UM DATATABLE DE CLIENTE PARA ATUALIZAÇÃO OU EXCLUSÃO, QUE PODE SER NULO EM CASO DE CADASTRO
        public AddClient(DataTable dataTable)
        {
            InitializeComponent();
            dataTableClient = dataTable;
            this.txt_clientName.Focus();
            if (dataTableClient != null)
            {
                foreach (DataRow dataRowClient in dataTableClient.Rows)
                {
                    if (dataTableClient.Rows.IndexOf(dataRowClient) == 0)
                    {
                        idCliente = Convert.ToInt32(dataRowClient.ItemArray[0]);
                        this.txt_clientName.Text = dataRowClient.ItemArray[1].ToString();
                        this.txt_clientAddress.Text = dataRowClient.ItemArray[2].ToString();
                        this.txt_clientHouseNumber.Text = dataRowClient.ItemArray[3].ToString();
                        this.txt_clientCity.Text = dataRowClient.ItemArray[4].ToString();
                        int index = cbb_clientState.FindString(dataRowClient.ItemArray[5].ToString());
                        this.cbb_clientState.SelectedIndex = index;
                        DataTable dataTableTel = Database.query("SELECT primeiroTelefoneCliente, segundoTelefoneCliente, terceiroTelefoneCliente FROM cliente WHERE idCliente = " + idCliente + ";");
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
                        int indexTel = cbb_clientTelephoneQuantity.FindString(rowsNumber.ToString());
                        cbb_clientTelephoneQuantity.SelectedIndex = indexTel;
                        for (int i = 1; i <= rowsNumber; i++)
                        {
                            dataTableClient.Rows[0].ItemArray[2].ToString();
                            if (rowsNumber == 1)
                            {
                                this.txt_firstTel.Show();
                                this.txt_firstTel.Text = dataTableClient.Rows[0].ItemArray[6].ToString();
                                this.cbb_firstTelephoneType.Show();
                                int indexFirstTel = cbb_firstTelephoneType.FindString(dataTableClient.Rows[0].ItemArray[7].ToString());
                                this.cbb_firstTelephoneType.SelectedIndex = indexFirstTel;
                                this.txt_secondTel.Hide();
                                this.cbb_secondTelephoneType.Hide();
                                this.txt_thirdTel.Hide();
                                this.cbb_thirdTelephoneType.Hide();
                            }
                            else if (rowsNumber == 2)
                            {
                                this.txt_firstTel.Show();
                                this.txt_firstTel.Text = dataTableClient.Rows[0].ItemArray[6].ToString();
                                this.cbb_firstTelephoneType.Show();
                                int indexFirstTel = cbb_firstTelephoneType.FindString(dataTableClient.Rows[0].ItemArray[7].ToString());
                                this.cbb_firstTelephoneType.SelectedIndex = indexFirstTel;
                                this.txt_secondTel.Show();
                                this.txt_secondTel.Text = dataTableClient.Rows[0].ItemArray[8].ToString();
                                this.cbb_secondTelephoneType.Show();
                                int indexSecondTel = cbb_secondTelephoneType.FindString(dataTableClient.Rows[0].ItemArray[9].ToString());
                                this.cbb_secondTelephoneType.SelectedIndex = indexSecondTel;
                                this.txt_thirdTel.Hide();
                                this.cbb_thirdTelephoneType.Hide();
                            }
                            else if (rowsNumber == 3)
                            {
                                this.txt_firstTel.Show();
                                this.txt_firstTel.Text = dataTableClient.Rows[0].ItemArray[6].ToString();
                                this.cbb_firstTelephoneType.Show();
                                int indexFirstTel = cbb_firstTelephoneType.FindString(dataTableClient.Rows[0].ItemArray[7].ToString());
                                this.cbb_firstTelephoneType.SelectedIndex = indexFirstTel;
                                this.txt_secondTel.Show();
                                this.txt_secondTel.Text = dataTableClient.Rows[0].ItemArray[8].ToString();
                                this.cbb_secondTelephoneType.Show();
                                int indexSecondTel = cbb_secondTelephoneType.FindString(dataTableClient.Rows[0].ItemArray[9].ToString());
                                this.cbb_secondTelephoneType.SelectedIndex = indexSecondTel;
                                this.txt_thirdTel.Show();
                                this.txt_thirdTel.Text = dataTableClient.Rows[0].ItemArray[10].ToString();
                                this.cbb_thirdTelephoneType.Show();
                                int indexThirdTel = cbb_thirdTelephoneType.FindString(dataTableClient.Rows[0].ItemArray[11].ToString());
                                this.cbb_thirdTelephoneType.SelectedIndex = indexThirdTel;
                            }
                        }
                    }
                }
            }
            else
            {
                int index = cbb_clientTelephoneQuantity.FindString("1");
                cbb_clientTelephoneQuantity.SelectedIndex = index;
                this.txt_firstTel.Show();
                this.cbb_firstTelephoneType.Show();
                this.txt_secondTel.Hide();
                this.cbb_secondTelephoneType.Hide();
                this.txt_thirdTel.Hide();
                this.cbb_thirdTelephoneType.Hide();
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

        //CADASTRO DE CLIENTE
        private void pcb_clientRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_clientRegister.Image = Properties.Resources.btn_clientRegister_active;
        }

        private void pcb_clientRegister_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_clientRegister.Image = Properties.Resources.btn_clientRegister;
        }

        private void pcb_clientRegister_Click(object sender, EventArgs e)
        {
            Client client = new Client();
            client.nomeCliente = txt_clientName.Text;
            client.enderecoCliente = txt_clientAddress.Text;
            client.numeroResidencia = txt_clientHouseNumber.Text;
            client.cidadeCliente = txt_clientCity.Text;
            client.estadoCliente = cbb_clientState.SelectedItem.ToString();
            client.primeiroTelefoneCliente = txt_firstTel.Text;
            client.tipoPrimeiroTelefoneCliente = cbb_firstTelephoneType.SelectedItem.ToString();
            if ((this.cbb_secondTelephoneType.SelectedIndex != -1) && (!string.IsNullOrEmpty(this.txt_secondTel.Text)))
            {
                client.segundoTelefoneCliente = txt_secondTel.Text;
                client.tipoSegundoTelefoneCliente = cbb_secondTelephoneType.SelectedItem.ToString();
            }
            if ((this.cbb_thirdTelephoneType.SelectedIndex != -1) && (!string.IsNullOrEmpty(this.txt_thirdTel.Text)))
            {
                client.terceiroTelefoneCliente = txt_thirdTel.Text;
                client.tipoTerceiroTelefoneCliente = cbb_thirdTelephoneType.SelectedItem.ToString();
            }
            if (Database.newClient(client))
            {
                MessageBox.Show("Cliente cadastrado com sucesso!");
                txt_clientName.Clear();
                txt_clientName.Focus();
                txt_clientAddress.Clear();
                txt_clientHouseNumber.Clear();
                txt_clientCity.Clear();
                cbb_clientState.SelectedIndex = -1;
                cbb_clientTelephoneQuantity.SelectedIndex = 0;
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
                MessageBox.Show("Não foi possível cadastrar cliente!");
            }
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
            Client client = new Client();
            client.idCliente = idCliente;
            client.nomeCliente = txt_clientName.Text;
            client.enderecoCliente = txt_clientAddress.Text;
            client.numeroResidencia = txt_clientHouseNumber.Text;
            client.cidadeCliente = txt_clientCity.Text;
            client.estadoCliente = cbb_clientState.SelectedItem.ToString();
            client.primeiroTelefoneCliente = txt_firstTel.Text;
            client.tipoPrimeiroTelefoneCliente = cbb_firstTelephoneType.SelectedItem.ToString();
            if((this.cbb_secondTelephoneType.SelectedIndex != -1) && (!string.IsNullOrEmpty(this.txt_secondTel.Text)))
            {
                client.segundoTelefoneCliente = txt_secondTel.Text;
                client.tipoSegundoTelefoneCliente = cbb_secondTelephoneType.SelectedItem.ToString();
            }else if ((this.cbb_secondTelephoneType.SelectedIndex == -1) && (string.IsNullOrEmpty(this.txt_secondTel.Text)))
            {
                client.segundoTelefoneCliente = null;
                client.tipoSegundoTelefoneCliente = null;
            }
            if ((this.cbb_thirdTelephoneType.SelectedIndex != -1) && (!string.IsNullOrEmpty(this.txt_thirdTel.Text)))
            {
                client.terceiroTelefoneCliente = txt_thirdTel.Text;
                client.tipoTerceiroTelefoneCliente = cbb_thirdTelephoneType.SelectedItem.ToString();
            }else if ((this.cbb_thirdTelephoneType.SelectedIndex == -1) && (string.IsNullOrEmpty(this.txt_thirdTel.Text)))
            {
                client.terceiroTelefoneCliente = null;
                client.tipoTerceiroTelefoneCliente = null;
            }
            if (Database.updateClient(client))
            {
                MessageBox.Show("Cliente atualizado com sucesso!");
                txt_clientName.Focus();
            }
            else
            {
                MessageBox.Show("Não foi possível atualizar cliente!");
            }
        }

        //EXCLUSÃO DE CLIENTE
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
            Client client = new Client();
            client.idCliente = idCliente;
            if (Database.deleteClient(client))
            {
                MessageBox.Show("Cliente excluído com suceso!");
            }
            else
            {
                MessageBox.Show("Não foi possível excluir cliente!");
            }
            if (Application.OpenForms.OfType<ClientForm>().Count() == 0)
            {
                ClientForm clientForm = new ClientForm();
                clientForm.Show();
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
            if (Application.OpenForms.OfType<ClientForm>().Count() == 0)
            {
                ClientForm client = new ClientForm();
                client.Show();
                this.Close();
            }
        }

        //ENCERRAR A APLICAÇÃO
        private void pcb_appClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO
        private void cbb_clientTelephoneQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbb_clientTelephoneQuantity.SelectedItem.ToString() == "1")
            {
                var sampleSize = new System.Drawing.Size(425, 375);
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
            else if (this.cbb_clientTelephoneQuantity.SelectedItem.ToString() == "2")
            {
                var sampleSize = new System.Drawing.Size(425, 470);
                this.panel_client.AutoScrollMinSize = sampleSize;
                this.txt_secondTel.Show();
                this.cbb_secondTelephoneType.Show();
                this.txt_thirdTel.Hide();
                this.cbb_thirdTelephoneType.Hide();
                this.txt_thirdTel.Clear();
                this.cbb_thirdTelephoneType.SelectedIndex = -1;
                this.cbb_thirdTelephoneType.Text = "Tipo do terceiro telefone";
            }
            else if (this.cbb_clientTelephoneQuantity.SelectedItem.ToString() == "3")
            {
                var sampleSize = new System.Drawing.Size(425, 565);
                this.panel_client.AutoScrollMinSize = sampleSize;
                this.txt_secondTel.Show();
                this.cbb_secondTelephoneType.Show();
                this.txt_thirdTel.Show();
                this.cbb_thirdTelephoneType.Show();
            }
        }
    }
}
