using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Geom;
using iText.Kernel.Font;
using iText.IO.Font;
using iText.Layout.Element;
using iText.Layout.Borders;
using iText.Layout.Properties;

namespace Sisteg_Dashboard
{
    public partial class ClientForm : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        private DataTable clientDataTable;
        private List<DateTime> dates = new List<DateTime>();
        private List<decimal> values = new List<decimal>();
        private List<int> ids = new List<int>();
        Income income = new Income();
        Parcel parcel = new Parcel();
        private int idCliente = 0, numeroOrcamento = 0;
        public static string path = System.Environment.CurrentDirectory;

        //INICIA INSTÂNCIA DO PAINEL, POPULANDO A TABELA DE LISTAGEM DE CLIENTES
        public ClientForm()
        {
            InitializeComponent();
            this.formatDataTable("SELECT cliente.idCliente, cliente.nomeCliente AS 'Nome:', cliente.enderecoCliente, cliente.numeroResidencia, cliente.bairroCliente, cliente.cidadeCliente, cliente.estadoCliente, cliente.emailCliente FROM cliente ORDER BY cliente.nomeCliente;");
            this.idCliente = Convert.ToInt32(this.dgv_clients.SelectedRows[0].Cells[0].Value);
            this.BackgroundImage = Properties.Resources.product_sisteg_bg;
            this.lbl_parcelsNumber.Visible = false;
            this.lbl_lastParcelDate.Visible = false;
            this.txt_lastParcelValue.Visible = false;
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

        //FUNÇÃO QUE FORMATA O DATATABLE ANTES DE ATRIBUIR OS DADOS AO DATAGRIDVIEW
        private void formatDataTable(string query)
        {
            this.clientDataTable = Database.query(query);
            if (this.clientDataTable.Rows.Count > 0)
            {
                //Remove as colunas vazias da tabela
                for (int col = this.clientDataTable.Columns.Count - 1; col >= 0; col--)
                {
                    bool removeColumn = true;
                    foreach (DataRow dataRow in this.clientDataTable.Rows)
                    {
                        if (!dataRow.IsNull(col))
                        {
                            removeColumn = false;
                            break;
                        }
                    }
                    if (removeColumn) this.clientDataTable.Columns.RemoveAt(col);
                }

                bool firstTime = true;
                this.clientDataTable.Columns.Add("Endereço:", typeof(string));
                this.clientDataTable.Columns.Add("E-mail:", typeof(string));
                for (int i = 0; i < this.clientDataTable.Rows.Count; i++)
                {
                    //Formata endereço do cliente e lista seus respectivos telefones
                    this.clientDataTable.Rows[i]["Endereço:"] = this.clientDataTable.Rows[i].ItemArray[2] + ", " + this.clientDataTable.Rows[i].ItemArray[3] + " - " + this.clientDataTable.Rows[i].ItemArray[4] + " - " + this.clientDataTable.Rows[i].ItemArray[5] + ", " + this.clientDataTable.Rows[i].ItemArray[6];
                    this.clientDataTable.Rows[i]["E-mail:"] = this.clientDataTable.Rows[i].ItemArray[7];
                    DataTable telephoneDataTable = Database.query("SELECT * FROM telefone WHERE idCliente = " + this.clientDataTable.Rows[i].ItemArray[0] + " ORDER BY tipoTelefone ASC;");
                    if (firstTime)
                    {
                        this.listTelephones(firstTime, telephoneDataTable, i);
                        firstTime = false;
                    }
                    else this.listTelephones(firstTime, telephoneDataTable, i);

                    //Lista os clientes que estão em divida
                    bool allParcelsPaid = true;
                    bool allIncomesPaid = true;
                    bool allBudgetsPaid = true;
                    DataTable budgetsDataTable = Database.query("SELECT numeroOrcamento FROM orcamento WHERE idCliente = " + this.clientDataTable.Rows[i].ItemArray[0]);
                    foreach (DataRow budgetsDataRow in budgetsDataTable.Rows)
                    {
                        //Cliente tem orçamento
                        DataTable incomesDataTable = Database.query("SELECT idReceita, recebimentoConfirmado, repetirParcelarReceita FROM receita WHERE numeroOrcamento = " + budgetsDataRow.ItemArray[0]);
                        foreach (DataRow incomesDataRow in incomesDataTable.Rows)
                        {
                            if (Convert.ToBoolean(incomesDataRow.ItemArray[2]))
                            {
                                //Receita parcelada
                                DataTable parcelsDataTable = Database.query("SELECT idParcela, recebimentoConfirmado FROM parcela WHERE idReceita = " + incomesDataRow.ItemArray[0]);
                                foreach (DataRow parcelsDataRow in parcelsDataTable.Rows)
                                {
                                    if (Convert.ToBoolean(parcelsDataRow.ItemArray[1])) continue; //Parcela paga
                                    else
                                    {
                                        //Existe parcela pendente
                                        allParcelsPaid = false;
                                        allIncomesPaid = false;
                                        allBudgetsPaid = false;
                                        break;
                                    }
                                }
                            }

                            if (Convert.ToBoolean(incomesDataRow.ItemArray[1])) continue; //Receita paga
                            else
                            {
                                //Existe receita pendente
                                allIncomesPaid = false;
                                allBudgetsPaid = false;
                                break;
                            }
                        }
                    }
                    if (rbtn_clientsInDebt.Checked) if (allParcelsPaid && allIncomesPaid && allBudgetsPaid) this.clientDataTable.Rows[i].Delete(); //Deleta da tabela os clientes que já pagaram todas as parcelas e receitas
                }
            }
            else
            {
                this.BackgroundImage = Properties.Resources.product_sisteg_bg;
                this.dgv_clients.Visible = false;
                this.lbl_parcelsNumber.Visible = false;
                this.lbl_lastParcelDate.Visible = false;
                this.txt_lastParcelValue.Visible = false;
            }

            this.dgv_clients.DataSource = this.clientDataTable;
        }


        //FUNÇÃO QUE LISTA OS TELEFONES DO CLIENTE NA TABELA DE CLIENTES
        private void listTelephones(bool firstTime, DataTable telephoneDataTable, int i)
        {
            foreach (DataRow dataRow in telephoneDataTable.Rows)
            {
                if (firstTime) if (!this.clientDataTable.Columns.Contains("Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":")) this.clientDataTable.Columns.Add("Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":", typeof(string));
                DataTable telephoneTypeDataTable = Database.query("SELECT numeroTelefone FROM telefone WHERE idCliente = " + this.clientDataTable.Rows[i].ItemArray[0] + " AND tipoTelefone = '" + dataRow.ItemArray[3] + "';");
                if (telephoneTypeDataTable.Rows.Count == 1)
                {
                    if(this.clientDataTable.Columns.Contains("Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":")) this.clientDataTable.Rows[i]["Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":"] = dataRow.ItemArray[4];
                    else
                    {
                        this.clientDataTable.Columns.Add("Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":", typeof(string));
                        this.clientDataTable.Rows[i]["Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":"] = dataRow.ItemArray[4];
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
                    this.clientDataTable.Rows[i]["Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":"] = numbers;
                }
            }
        }

        //FUNÇÃO QUE VERIFICA AS PARCELAS NÃO PAGAS
        private int incomeParceled(DataTable parcelsDataTable, bool t, int count)
        {
            if (parcelsDataTable.Rows.Count > 0)
            {
                int k = 0;
                int total = 0;
                if (t) total = parcelsDataTable.Rows.Count; else total = this.ids.Count + parcelsDataTable.Rows.Count;
                for (int j = this.ids.Count; j < total; j++)
                {
                    this.dates.Add(new DateTime());
                    this.values.Add(new decimal());
                    this.ids.Add(new int());
                    this.ids[j] = Convert.ToInt32(parcelsDataTable.Rows[k].ItemArray[0]);
                    this.values[j] = Convert.ToDecimal(parcelsDataTable.Rows[k].ItemArray[1]);
                    this.dates[j] = Convert.ToDateTime(parcelsDataTable.Rows[k].ItemArray[2]);
                    count++;
                    k++;
                }
            }
            return count;
        }

        //FUNÇÃO QUE ATUALIZA OS DADOS DO CLIENTE NO DATAGRIDVIEW
        private void updateData()
        {
            if (this.dgv_clients.SelectedRows.Count == 1)
            {
                this.idCliente = Convert.ToInt32(this.dgv_clients.SelectedRows[0].Cells[0].Value);

                DataTable incomesDataTable = Database.query("SELECT receita.idReceita, receita.valorReceita, receita.dataTransacao, receita.recebimentoConfirmado, receita.repetirParcelarReceita FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento WHERE orcamento.idCliente = " + this.idCliente);
                this.dates.Clear();
                this.values.Clear();
                this.ids.Clear();
                int count = 0;
                if (incomesDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < incomesDataTable.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(incomesDataTable.Rows[i].ItemArray[3]))
                        {
                            //Recebimento confirmado
                            if (Convert.ToBoolean(incomesDataTable.Rows[i].ItemArray[4]))
                            {
                                //Receita parcelada
                                DataTable parcelsDataTable = Database.query("SELECT parcela.idParcela, parcela.valorParcela, parcela.dataTransacao FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento JOIN parcela ON parcela.idReceita = receita.idReceita WHERE orcamento.idCliente = " + this.idCliente + " AND parcela.recebimentoConfirmado = false AND receita.idReceita = " + incomesDataTable.Rows[i].ItemArray[0] + " ORDER BY parcela.dataTransacao;");
                                if(parcelsDataTable.Rows.Count > 0) count += this.incomeParceled(parcelsDataTable, true, count);
                            }
                        }
                        else
                        {
                            //Recebimento não confirmado
                            incomesDataTable = Database.query("SELECT receita.idReceita, receita.valorReceita, receita.dataTransacao, receita.recebimentoConfirmado, receita.repetirParcelarReceita FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento WHERE orcamento.idCliente = " + this.idCliente + " AND receita.recebimentoConfirmado = false;");
                            if (incomesDataTable.Rows.Count == 1)
                            {
                                this.dates.Add(new DateTime());
                                this.values.Add(new decimal());
                                this.ids.Add(new int());
                                this.ids[0] = Convert.ToInt32(incomesDataTable.Rows[0].ItemArray[0]);
                                this.values[0] = Convert.ToDecimal(incomesDataTable.Rows[0].ItemArray[1]);
                                this.dates[0] = Convert.ToDateTime(incomesDataTable.Rows[0].ItemArray[2]);
                                count++;
                                if (Convert.ToBoolean(incomesDataTable.Rows[0].ItemArray[4]))
                                {
                                    //Receita parcelada
                                    DataTable parcelsDataTable = Database.query("SELECT parcela.idParcela, parcela.valorParcela, parcela.dataTransacao FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento JOIN parcela ON parcela.idReceita = receita.idReceita WHERE orcamento.idCliente = " + this.idCliente + " AND parcela.recebimentoConfirmado = false AND receita.idReceita = " + incomesDataTable.Rows[0].ItemArray[0] + " ORDER BY parcela.dataTransacao;");
                                    count = this.incomeParceled(parcelsDataTable, false, count);
                                }
                                break;
                            }
                            this.dates.Add(new DateTime());
                            this.values.Add(new decimal());
                            this.ids.Add(new int());
                            this.ids[i] = Convert.ToInt32(incomesDataTable.Rows[i].ItemArray[0]);
                            this.values[i] = Convert.ToDecimal(incomesDataTable.Rows[i].ItemArray[1]);
                            this.dates[i] = Convert.ToDateTime(incomesDataTable.Rows[i].ItemArray[2]);
                            count++;
                            if (Convert.ToBoolean(incomesDataTable.Rows[i].ItemArray[4]))
                            {
                                //Receita parcelada
                                DataTable parcelsDataTable = Database.query("SELECT parcela.idParcela, parcela.valorParcela, parcela.dataTransacao FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento JOIN parcela ON parcela.idReceita = receita.idReceita WHERE orcamento.idCliente = " + this.idCliente + " AND parcela.recebimentoConfirmado = false AND receita.idReceita = " + incomesDataTable.Rows[i].ItemArray[0] + " ORDER BY parcela.dataTransacao;");
                                count = this.incomeParceled(parcelsDataTable, false, count);
                            }
                        }
                    }
                    this.lbl_parcelsNumber.Text = count.ToString();
                }
                else
                {
                    //Receita parcelada
                    DataTable parcelsDataTable = Database.query("SELECT parcela.idParcela, parcela.valorParcela, parcela.dataTransacao FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento JOIN parcela ON parcela.idReceita = receita.idReceita WHERE orcamento.idCliente = " + this.idCliente + " AND parcela.recebimentoConfirmado = false ORDER BY parcela.dataTransacao;");
                    count = this.incomeParceled(parcelsDataTable, true, count);
                }

                if (this.ids.Count > 0)
                {
                    this.lbl_lastParcelDate.Text = this.dates[0].ToShortDateString();
                    this.txt_lastParcelValue.Text = this.values[0].ToString("C");
                    if (DateTime.Compare(DateTime.Today, this.dates[0]) > 0) changeColor(243, 104, 82); else changeColor(77, 255, 255);
                }
            }
        }

        private bool setPaymentValue(DataTable differentIncomeValueDataTable)
        {
            if (income.valorReceita == Math.Round(Convert.ToDecimal(differentIncomeValueDataTable.Rows[0].ItemArray[3]), 2))
            {
                //O valor do pagamento não foi alterado
                if (Database.payIncome(income))
                {
                    MessageBox.Show("Receita paga!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Abrir o quadro de configuração da impressão
                    /*if (this.pdl_printSettings.ShowDialog() == DialogResult.Cancel) return true;
                    else this.ptd_printInvoice.PrinterSettings = this.pdl_printSettings.PrinterSettings;*/

                    //Imprimir cupom não fiscal
                    this.printInvoice();
                    return true;
                }
            }
            else
            {
                if (income.valorReceita > Math.Round(Convert.ToDecimal(differentIncomeValueDataTable.Rows[0].ItemArray[3]), 2))
                {
                    //O valor do pagamento é maior do que o valor da parcela
                    MessageBox.Show("O valor a ser pago deve ser menor do que o valor da parcela!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                {
                    //O valor do pagamento é menor do que o valor da parcela
                    income.idConta = Convert.ToInt32(differentIncomeValueDataTable.Rows[0].ItemArray[1]);
                    income.numeroOrcamento = Convert.ToInt32(differentIncomeValueDataTable.Rows[0].ItemArray[2]);
                    this.numeroOrcamento = income.numeroOrcamento;
                    decimal valorReceita = income.valorReceita;
                    this.values[0] = valorReceita;
                    income.valorReceita = Convert.ToDecimal(differentIncomeValueDataTable.Rows[0].ItemArray[3]) - income.valorReceita;
                    income.descricaoReceita = differentIncomeValueDataTable.Rows[0].ItemArray[4].ToString();
                    income.dataTransacao = Convert.ToDateTime(differentIncomeValueDataTable.Rows[0].ItemArray[5]);
                    income.categoriaReceita = differentIncomeValueDataTable.Rows[0].ItemArray[6].ToString();
                    income.observacoesReceita = differentIncomeValueDataTable.Rows[0].ItemArray[7].ToString();
                    income.recebimentoConfirmado = false;
                    income.repetirParcelarReceita = Convert.ToBoolean(differentIncomeValueDataTable.Rows[0].ItemArray[9]);
                    income.valorFixoReceita = Convert.ToBoolean(differentIncomeValueDataTable.Rows[0].ItemArray[10]);
                    income.repeticoesValorFixoReceita = Convert.ToInt32(differentIncomeValueDataTable.Rows[0].ItemArray[11]);
                    income.parcelarValorReceita = Convert.ToBoolean(differentIncomeValueDataTable.Rows[0].ItemArray[12]);
                    income.parcelasReceita = Convert.ToInt32(differentIncomeValueDataTable.Rows[0].ItemArray[13]);
                    income.periodoRepetirParcelarReceita = differentIncomeValueDataTable.Rows[0].ItemArray[14].ToString();
                    if (Database.updateIncome(income))
                    {
                        income.valorReceita = valorReceita;
                        income.recebimentoConfirmado = true;
                        if (Database.newIncome(income))
                        {
                            MessageBox.Show("Receita paga!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //Abrir o quadro de configuração da impressão
                            /*if (this.pdl_printSettings.ShowDialog() == DialogResult.Cancel) return true;
                            else this.ptd_printInvoice.PrinterSettings = this.pdl_printSettings.PrinterSettings;*/

                            //Imprimir cupom não fiscal
                            this.printInvoice();
                            return true;
                        }
                    }
                }
            }
            return false;
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
            if (Application.OpenForms.OfType<ProductForm>().Count() == 0)
            {
                ProductForm product = new ProductForm();
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
            if (Application.OpenForms.OfType<BudgetForm>().Count() == 0)
            {
                BudgetForm budget = new BudgetForm();
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

        //FORMATAÇÃO DA TABELA APÓS A DISPOSIÇÃO DOS DADOS
        private void dgv_clients_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if(this.dgv_clients.Rows.Count > 0)
            {
                foreach (DataGridViewColumn dataGridViewColumn in dgv_clients.Columns) dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                if (dgv_clients.ColumnCount > 0)
                {
                    this.dgv_clients.Columns[0].Visible = false;
                    for (int i = 2; i < 8; i++) this.dgv_clients.Columns[i].Visible = false;
                    for (int i = 1; i < dgv_clients.ColumnCount; i++) dgv_clients.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }
        }

        //BUSCAR CLIENTE NA TABELA
        private void txt_searchClient_TextChange(object sender, EventArgs e)
        {
            string searchClient = this.txt_searchClient.Text;
            if(searchClient.Trim() != null) this.formatDataTable("SELECT cliente.idCliente, cliente.nomeCliente AS 'Nome:', cliente.enderecoCliente, cliente.numeroResidencia, cliente.bairroCliente, cliente.cidadeCliente, cliente.estadoCliente, cliente.emailCliente FROM cliente WHERE cliente.nomeCliente LIKE '%" + searchClient + "%' ORDER BY cliente.nomeCliente;");
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

        private void pcb_btnPay_Click(object sender, EventArgs e)
        {
            if (this.dgv_clients.SelectedRows.Count == 1)
            {
                this.idCliente = Convert.ToInt32(this.dgv_clients.SelectedRows[0].Cells[0].Value);
                DataTable incomesDataTable = Database.query("SELECT receita.idReceita, receita.numeroOrcamento, receita.valorReceita, receita.dataTransacao, receita.recebimentoConfirmado, receita.repetirParcelarReceita FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento WHERE orcamento.idCliente = " + this.idCliente);
                if (incomesDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < incomesDataTable.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(incomesDataTable.Rows[i].ItemArray[4]))
                        {
                            //Recebimento confirmado
                            if (Convert.ToBoolean(incomesDataTable.Rows[i].ItemArray[5]))
                            {
                                //Receita parcelada
                                DataTable oneIncomeDataTable = Database.query("SELECT receita.idReceita, receita.numeroOrcamento, receita.valorReceita, receita.dataTransacao, receita.recebimentoConfirmado, receita.repetirParcelarReceita FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento WHERE orcamento.idCliente = " + this.idCliente + " AND receita.recebimentoConfirmado = false;");
                                if (oneIncomeDataTable.Rows.Count == 1)
                                {
                                    //Receita parcelada em única parcela, ou seja, não há parcelas apenas uma receita com recebimento não confirmado
                                    DataTable differentIncomeValueDataTable = Database.query("SELECT * FROM receita WHERE idReceita = " + this.ids[0]);
                                    income.idReceita = this.ids[0];
                                    this.numeroOrcamento = Convert.ToInt32(differentIncomeValueDataTable.Rows[0].ItemArray[2]);
                                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                                    string valorParcela = txt_lastParcelValue.Text;
                                    if (regexValor.IsMatch(valorParcela))
                                    {
                                        if (valorParcela.Contains("R$ ")) income.valorReceita = Convert.ToDecimal(valorParcela.Substring(3));
                                        else if (valorParcela.Contains("R$")) income.valorReceita = Convert.ToDecimal(valorParcela.Substring(2));
                                        else income.valorReceita = Convert.ToDecimal(txt_lastParcelValue.Text);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }

                                    if (this.setPaymentValue(differentIncomeValueDataTable)) break; else break;
                                }

                                DataTable parcelsDataTable = Database.query("SELECT parcela.idParcela, orcamento.numeroOrcamento, parcela.valorParcela, parcela.dataTransacao FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento JOIN parcela ON parcela.idReceita = receita.idReceita WHERE orcamento.idCliente = " + this.idCliente + " AND parcela.recebimentoConfirmado = false AND receita.idReceita = " + incomesDataTable.Rows[i].ItemArray[0] + " ORDER BY parcela.dataTransacao;");
                                if (parcelsDataTable.Rows.Count > 0)
                                {
                                    //Existe mais de uma parcela a ser paga
                                    for (int j = 0; j < parcelsDataTable.Rows.Count; j++)
                                    {
                                        DataTable differentParcelValueDataTable = Database.query("SELECT * FROM parcela WHERE idParcela = " + this.ids[0]);
                                        parcel.idParcela = this.ids[0];
                                        this.numeroOrcamento = Convert.ToInt32(parcelsDataTable.Rows[0].ItemArray[1]);
                                        Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                                        string valorParcela = txt_lastParcelValue.Text;
                                        if (regexValor.IsMatch(valorParcela))
                                        {
                                            if (valorParcela.Contains("R$ ")) parcel.valorParcela = Convert.ToDecimal(valorParcela.Substring(3));
                                            else if (valorParcela.Contains("R$")) parcel.valorParcela = Convert.ToDecimal(valorParcela.Substring(2));
                                            else parcel.valorParcela = Convert.ToDecimal(txt_lastParcelValue.Text);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            return;
                                        }

                                        if (parcel.valorParcela == Math.Round(Convert.ToDecimal(differentParcelValueDataTable.Rows[0].ItemArray[3]), 2))
                                        {
                                            //O valor do pagamento não foi alterado
                                            if (Database.payParcel(parcel))
                                            {
                                                MessageBox.Show("Parcela paga!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                //Abrir o quadro de configuração da impressão
                                                /*if (this.pdl_printSettings.ShowDialog() == DialogResult.Cancel) break;
                                                else this.ptd_printInvoice.PrinterSettings = this.pdl_printSettings.PrinterSettings;*/

                                                //Imprimir cupom não fiscal
                                                this.printInvoice();
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (parcel.valorParcela > Math.Round(Convert.ToDecimal(differentParcelValueDataTable.Rows[0].ItemArray[3]), 2))
                                            {
                                                //O valor do pagamento é maior do que o valor da parcela
                                                MessageBox.Show("O valor a ser pago deve ser menor do que o valor da parcela!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                return;
                                            }
                                            else
                                            {
                                                //O valor do pagamento é menor do que o valor da parcela
                                                this.values[0] = parcel.valorParcela;
                                                parcel.idReceita = Convert.ToInt32(differentParcelValueDataTable.Rows[0].ItemArray[1]);
                                                parcel.idDespesa = Convert.ToInt32(differentParcelValueDataTable.Rows[0].ItemArray[2]);
                                                parcel.descricaoParcela = differentParcelValueDataTable.Rows[0].ItemArray[4].ToString();
                                                parcel.dataTransacao = Convert.ToDateTime(differentParcelValueDataTable.Rows[0].ItemArray[5]);
                                                parcel.categoriaParcela = differentParcelValueDataTable.Rows[0].ItemArray[6].ToString();
                                                parcel.observacoesParcela = differentParcelValueDataTable.Rows[0].ItemArray[7].ToString();
                                                parcel.recebimentoConfirmado = true;
                                                parcel.pagamentoConfirmado = Convert.ToBoolean(differentParcelValueDataTable.Rows[0].ItemArray[9]);
                                                if (Database.updateParcel(parcel))
                                                {
                                                    parcel.valorParcela = Convert.ToDecimal(differentParcelValueDataTable.Rows[0].ItemArray[3]) - parcel.valorParcela;
                                                    parcel.recebimentoConfirmado = false;
                                                    if (Database.newParcel(parcel))
                                                    {
                                                        MessageBox.Show("Parcela paga!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                        //Abrir o quadro de configuração da impressão
                                                        /*if (this.pdl_printSettings.ShowDialog() == DialogResult.Cancel) break;
                                                        else this.ptd_printInvoice.PrinterSettings = this.pdl_printSettings.PrinterSettings;*/

                                                        //Imprimir cupom não fiscal
                                                        this.printInvoice();
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //Recebimento não confirmado
                            DataTable differentIncomeValueDataTable = Database.query("SELECT * FROM receita WHERE idReceita = " + this.ids[0]);
                            income.idReceita = this.ids[0];
                            this.numeroOrcamento = Convert.ToInt32(differentIncomeValueDataTable.Rows[0].ItemArray[2]);
                            Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                            string valorParcela = txt_lastParcelValue.Text;
                            if (regexValor.IsMatch(valorParcela))
                            {
                                if (valorParcela.Contains("R$ ")) income.valorReceita = Convert.ToDecimal(valorParcela.Substring(3));
                                else if (valorParcela.Contains("R$")) income.valorReceita = Convert.ToDecimal(valorParcela.Substring(2));
                                else income.valorReceita = Convert.ToDecimal(txt_lastParcelValue.Text);
                            }
                            else
                            {
                                MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            if (this.setPaymentValue(differentIncomeValueDataTable)) break; else break;
                        }
                    }
                }
            }

            //Atualiza dados
            this.formatDataTable("SELECT cliente.idCliente, cliente.nomeCliente AS 'Nome:', cliente.enderecoCliente, cliente.numeroResidencia, cliente.bairroCliente, cliente.cidadeCliente, cliente.estadoCliente, cliente.emailCliente FROM cliente ORDER BY cliente.nomeCliente;");

            this.updateData();
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
                addClient.pcb_clientRegister.Location = new System.Drawing.Point(628, 312);
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
            if (Convert.ToInt32(this.dgv_clients.Rows.Count) == 0) MessageBox.Show("Não há cliente selecionado para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Convert.ToInt32(this.dgv_clients.SelectedRows.Count) > 1) MessageBox.Show("Selecione apenas uma linha da tabela para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                foreach (DataGridViewRow dataGridViewRow in this.dgv_clients.SelectedRows)
                {
                    if (Application.OpenForms.OfType<AddIncome>().Count() == 0)
                    {
                        DataTable dataTableClient = Database.query("SELECT * FROM cliente WHERE idCliente = " + dataGridViewRow.Cells[0].Value.ToString() + ";");
                        AddClient addClient = new AddClient(dataTableClient);
                        addClient.editClick = true;
                        addClient.Show();
                        this.Close();
                    }
                }
            }
        }

        //ENCERRA A APLICAÇÃO
        private void pcb_appClose_Click(object sender, EventArgs e)
        {
            if (((DialogResult)MessageBox.Show("Tem certeza que deseja encerrar a aplicação?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)).ToString().ToUpper() == "YES") Application.Exit();
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO
        private void changeColor(int r, int g, int b)
        {
            this.lbl_parcelsNumber.ForeColor = Color.FromArgb(r, g, b);
            this.lbl_lastParcelDate.ForeColor = Color.FromArgb(r, g, b);
            /*this.txt_lastParcelValue.OnActiveState.ForeColor = Color.FromArgb(r, g, b);
            this.txt_lastParcelValue.OnActiveState.PlaceholderForeColor = Color.FromArgb(r, g, b);
            this.txt_lastParcelValue.OnHoverState.ForeColor = Color.FromArgb(r, g, b);
            this.txt_lastParcelValue.OnHoverState.PlaceholderForeColor = Color.FromArgb(r, g, b);
            this.txt_lastParcelValue.OnIdleState.ForeColor = Color.FromArgb(r, g, b);
            this.txt_lastParcelValue.OnIdleState.PlaceholderForeColor = Color.FromArgb(r, g, b);
            this.txt_lastParcelValue.ForeColor = Color.FromArgb(r, g, b);
            this.txt_lastParcelValue.PlaceholderForeColor = Color.FromArgb(r, g, b);*/
        }

        private void rbtn_clientsInDebt_CheckedChanged(object sender, EventArgs e)
        {
            string query = "SELECT cliente.idCliente, cliente.nomeCliente AS 'Nome:', cliente.enderecoCliente, cliente.numeroResidencia, cliente.bairroCliente, cliente.cidadeCliente, cliente.estadoCliente, cliente.emailCliente FROM cliente ORDER BY cliente.nomeCliente;";
            if (Database.query(query).Rows.Count > 0) this.formatDataTable(query);
            else
            {
                this.BackgroundImage = Properties.Resources.client_sisteg_bg_value;
                this.dgv_clients.Visible = false;
                this.lbl_parcelsNumber.Visible = true;
                this.lbl_lastParcelDate.Visible = true;
                this.txt_lastParcelValue.Visible = true;
            }
        }

        private void rbtn_allClients_CheckedChanged(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.product_sisteg_bg;
            this.lbl_parcelsNumber.Visible = false;
            this.lbl_lastParcelDate.Visible = false;
            this.txt_lastParcelValue.Visible = false;
            this.formatDataTable("SELECT cliente.idCliente, cliente.nomeCliente AS 'Nome:', cliente.enderecoCliente, cliente.numeroResidencia, cliente.bairroCliente, cliente.cidadeCliente, cliente.estadoCliente, cliente.emailCliente FROM cliente ORDER BY cliente.nomeCliente;");
        }

        private void dgv_clients_SelectionChanged(object sender, EventArgs e)
        {
            this.updateData();
        }

        private void printInvoice()
        {
            this.sfd_saveInvoice.Filter = "Portable Document File (.pdf)|*.pdf";

            //Abrir a janela "Salvar arquivo"
            if (this.sfd_saveInvoice.ShowDialog() == DialogResult.Cancel) return;
            else
            {
                using (PdfWriter pdfWriter = new PdfWriter(sfd_saveInvoice.FileName, new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)))
                {
                    //Document
                    var pdfDocument = new PdfDocument(pdfWriter);
                    var document = new Document(pdfDocument, PageSize.A6);

                    //Font
                    string avengeance = path + @"\assets\fonts\avengeance.ttf";

                    PdfFont pdfFontBody = PdfFontFactory.CreateFont(FontConstants.COURIER);
                    PdfFont pdfFontCompanyName = PdfFontFactory.CreateFont(avengeance, true);
                    PdfFont pdfFontTitle = PdfFontFactory.CreateFont(FontConstants.TIMES_ROMAN);

                    //Styles
                    Style companyStyle = new Style();
                    companyStyle.SetBorder(Border.NO_BORDER).SetFont(pdfFontCompanyName).SetFontSize(28).SetPaddingBottom(-10).SetTextAlignment(TextAlignment.CENTER);

                    Style titleStyle = new Style();
                    titleStyle.SetBorder(Border.NO_BORDER).SetFont(pdfFontTitle).SetFontSize(6).SetTextAlignment(TextAlignment.CENTER);

                    Style bodyStyle = new Style();
                    bodyStyle.SetBorder(Border.NO_BORDER).SetFont(pdfFontBody).SetFontSize(6).SetTextAlignment(TextAlignment.CENTER);

                    //Table 1
                    float[] columnWidth = new float[] { 140, 140 };

                    Table headerTable = new Table(columnWidth);

                    headerTable.AddCell(new Cell(1, 2).Add(new Paragraph("Sisteg")).AddStyle(companyStyle));
                    headerTable.AddCell(new Cell(1, 2).Add(new Paragraph("SEGURANÇA ELETRÔNICA")).AddStyle(titleStyle));
                    headerTable.AddCell(new Cell(1, 2).Add(new Paragraph("CUPOM NÃO FISCAL")).AddStyle(titleStyle).SetBorderTop(new SolidBorder(1)));
                    headerTable.SetBorderBottom(new SolidBorder(1));

                    document.Add(headerTable);

                    //Table 2
                    Table companyTable = new Table(columnWidth);

                    //Row 3
                    companyTable.AddCell(new Cell(1, 2).Add(new Paragraph("Rua professora Angelina de Felice Mesanelli, 152")).AddStyle(bodyStyle));

                    //Row 4
                    companyTable.AddCell(new Cell(1, 2).Add(new Paragraph("Jd. Victório Lucato - Limeira - SP")).AddStyle(bodyStyle));

                    //Row 5
                    companyTable.AddCell(new Cell().Add(new Paragraph("Telefone:")).AddStyle(titleStyle));
                    companyTable.AddCell(new Cell().Add(new Paragraph("(19) 98881-1660")).AddStyle(bodyStyle));

                    //Row 6
                    companyTable.AddCell(new Cell().Add(new Paragraph("CNPJ:")).AddStyle(titleStyle));
                    companyTable.AddCell(new Cell().Add(new Paragraph("23.524.384/0001-47")).AddStyle(bodyStyle));
                    companyTable.SetBorderBottom(new SolidBorder(1));

                    document.Add(companyTable);

                    //Table 3
                    Table clientTable = new Table(columnWidth);
                    clientTable.AddCell(new Cell(1, 2).Add(new Paragraph("CLIENTE")).AddStyle(titleStyle).SetBorderBottom(new SolidBorder(1)));

                    clientTable.AddCell(new Cell(1, 2).Add(new Paragraph(this.dgv_clients.SelectedRows[0].Cells[1].Value.ToString())).AddStyle(bodyStyle));
                    clientTable.AddCell(new Cell(1, 2).Add(new Paragraph(this.dgv_clients.SelectedRows[0].Cells[2].Value.ToString() + ", " + this.dgv_clients.SelectedRows[0].Cells[3].Value.ToString())).AddStyle(bodyStyle));
                    clientTable.AddCell(new Cell(1, 2).Add(new Paragraph(this.dgv_clients.SelectedRows[0].Cells[4].Value.ToString() + " - " + this.dgv_clients.SelectedRows[0].Cells[5].Value.ToString() + ", " + this.dgv_clients.SelectedRows[0].Cells[6].Value.ToString())).AddStyle(bodyStyle));
                    if (this.dgv_clients.Columns.Contains("Telefone celular:"))
                    {
                        clientTable.AddCell(new Cell().Add(new Paragraph("Telefone celular:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle));
                        clientTable.AddCell(new Cell().Add(new Paragraph(this.dgv_clients.SelectedRows[0].Cells["Telefone celular:"].Value.ToString())).AddStyle(bodyStyle));
                    }
                    if (this.dgv_clients.Columns.Contains("Telefone comercial:"))
                    {
                        clientTable.AddCell(new Cell().Add(new Paragraph("Telefone comercial:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle));
                        clientTable.AddCell(new Cell().Add(new Paragraph(this.dgv_clients.SelectedRows[0].Cells["Telefone comercial:"].Value.ToString())).AddStyle(bodyStyle));
                    }
                    if (this.dgv_clients.Columns.Contains("Telefone residencial:"))
                    {
                        clientTable.AddCell(new Cell().Add(new Paragraph("Telefone residencial:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle));
                        clientTable.AddCell(new Cell().Add(new Paragraph(this.dgv_clients.SelectedRows[0].Cells["Telefone residencial:"].Value.ToString())).AddStyle(bodyStyle));
                    }
                    clientTable.SetBorderBottom(new SolidBorder(1));

                    document.Add(clientTable);

                    //Table 4
                    Table invoiceTable = new Table(columnWidth);
                    invoiceTable.AddCell(new Cell(1, 2).Add(new Paragraph("PARCELA")).AddStyle(titleStyle).SetBorderBottom(new SolidBorder(1)));

                    DataTable incomesDataTable = Database.query("SELECT DISTINCT receita.recebimentoConfirmado, receita.parcelarValorReceita, receita.parcelasReceita FROM receita JOIN orcamento ON receita.numeroOrcamento = orcamento.numeroOrcamento WHERE orcamento.numeroOrcamento = " + this.numeroOrcamento);
                    int count = 0;
                    int total = 1;
                    if (incomesDataTable.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(incomesDataTable.Rows[0].ItemArray[1]))
                        {
                            //Recebimento confirmado
                            count++;
                            MessageBox.Show(count.ToString());
                            if (Convert.ToBoolean(incomesDataTable.Rows[0].ItemArray[2]))
                            {
                                //Receita parcelada
                                total = Convert.ToInt32(incomesDataTable.Rows[0].ItemArray[2]);
                                DataTable parcelsDataTable = Database.query("SELECT parcela.recebimentoConfirmado FROM parcela JOIN receita ON parcela.idReceita = receita.idReceita JOIN orcamento ON receita.numeroOrcamento = orcamento.numeroOrcamento WHERE orcamento.numeroOrcamento = " + this.numeroOrcamento);
                                if (parcelsDataTable.Rows.Count > 0)
                                {
                                    int countChanged = 0;
                                    for (int i = 0; i < parcelsDataTable.Rows.Count; i++)
                                    {
                                        if (parcelsDataTable.Rows.Count >= total)
                                        {
                                            if (!Convert.ToBoolean(parcelsDataTable.Rows[i].ItemArray[0])) countChanged++;
                                        }
                                        else if (Convert.ToBoolean(parcelsDataTable.Rows[i].ItemArray[0])) count++;
                                    }
                                    if (countChanged != 0) count = total - countChanged;
                                    if (count == 1) count = total;
                                }
                            }
                        }
                        else
                        {
                            //Recebimento não confirmado
                            if (Convert.ToBoolean(incomesDataTable.Rows[0].ItemArray[2]))
                            {
                                //Receita parcelada
                                total = Convert.ToInt32(incomesDataTable.Rows[0].ItemArray[2]);
                                DataTable parcelsDataTable = Database.query("SELECT parcela.recebimentoConfirmado FROM parcela JOIN receita ON parcela.idReceita = receita.idReceita JOIN orcamento ON receita.numeroOrcamento = orcamento.numeroOrcamento WHERE orcamento.numeroOrcamento = " + this.numeroOrcamento);
                                if (parcelsDataTable.Rows.Count > 0)
                                {
                                    for (int i = 0; i < parcelsDataTable.Rows.Count; i++) if (Convert.ToBoolean(parcelsDataTable.Rows[i].ItemArray[0])) count++;
                                }
                            }
                        }
                    }
                    invoiceTable.AddCell(new Cell().Add(new Paragraph("Data da parcela:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle));
                    invoiceTable.AddCell(new Cell().Add(new Paragraph(this.dates[0].ToShortDateString())).SetBorder(Border.NO_BORDER).AddStyle(bodyStyle));

                    invoiceTable.AddCell(new Cell().Add(new Paragraph("Data do pagamento:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle));
                    invoiceTable.AddCell(new Cell().Add(new Paragraph(DateTime.Today.ToShortDateString())).SetBorder(Border.NO_BORDER).AddStyle(bodyStyle));

                    invoiceTable.AddCell(new Cell().Add(new Paragraph("Valor:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle));
                    invoiceTable.AddCell(new Cell().Add(new Paragraph(this.values[0].ToString("C"))).SetBorder(Border.NO_BORDER).AddStyle(bodyStyle));

                    invoiceTable.AddCell(new Cell().Add(new Paragraph("Parcela:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle));
                    invoiceTable.AddCell(new Cell().Add(new Paragraph(count.ToString() + "/" + total.ToString())).SetBorder(Border.NO_BORDER).AddStyle(bodyStyle));

                    document.Add(invoiceTable);

                    document.Close();

                    System.Diagnostics.Process.Start(this.sfd_saveInvoice.FileName);
                }
            }
        }
    }
}
