using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class ClientForm : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        private List<DateTime> dates = new List<DateTime>();
        private List<decimal> values = new List<decimal>();
        private List<int> ids = new List<int>();
        Income income = new Income();
        Parcel parcel = new Parcel();
        //Bitmap backGround, backGroundTemp;

        //EVITA TREMULAÇÃO DOS COMPONENTES
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handlerparam = base.CreateParams;
                handlerparam.ExStyle |= 0x02000000;
                return handlerparam;
            }
        }

        /*private void initialize(System.Drawing.Image image)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            backGroundTemp = new Bitmap(image);
            backGround = new Bitmap(backGroundTemp, backGroundTemp.Width, backGroundTemp.Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.DrawImageUnscaled(backGround, 0, 0);
            base.OnPaint(e);
        }*/

        //INICIA INSTÂNCIA DO PAINEL, POPULANDO A TABELA DE LISTAGEM DE CLIENTES
        public ClientForm()
        {
            InitializeComponent();
            this.dgv_clients.Visible = true;
            this.pcb_btnEdit.Visible = false;
            this.lbl_btnEditTag.Visible = false;
            this.clearForm(false);

            string query = "SELECT cliente.idCliente, cliente.nomeCliente AS 'Nome:', cliente.enderecoCliente, cliente.numeroResidencia, cliente.bairroCliente, cliente.cidadeCliente, cliente.estadoCliente, cliente.emailCliente AS 'E-mail:' FROM cliente ORDER BY cliente.nomeCliente;";
            Globals.clientDataTable = Database.query(query);
            if (Globals.clientDataTable.Rows.Count > 0)
            {
                this.pcb_btnEdit.Visible = true;
                this.lbl_btnEditTag.Visible = true;
                this.formatDataTable(query);
                Globals.idCliente = Convert.ToInt32(this.dgv_clients.SelectedRows[0].Cells[0].Value);
            }

            Main main = new Main();
            DataTable accountBalance = Database.query("SELECT saldoConta, somarTotal FROM conta WHERE idConta = " + Globals.idConta);
            if (accountBalance.Rows.Count > 0)
            {
                if (Convert.ToBoolean(accountBalance.Rows[0].ItemArray[1])) Globals.saldoConta = Convert.ToDecimal(accountBalance.Rows[0].ItemArray[0]);
                else Globals.saldoConta = 0;
            }
        }

        //MENU DE NAVEGAÇÃO DA APLICAÇÃO
        //Formulário Painel principal
        private void pcb_btnMain_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnMain.BackgroundImage = Properties.Resources.btn_main_form_active;
        }

        private void pcb_btnMain_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_mainTag.ClientRectangle.Contains(lbl_mainTag.PointToClient(Cursor.Position))) this.pcb_btnMain.BackgroundImage = Properties.Resources.btn_main_form;
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

        private void lbl_mainTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Main>().Count() == 0)
            {
                Main main = new Main();
                main.Show();
                this.Close();
            }
        }

        //Formulário Produto
        private void pcb_btnProduct_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnProduct.BackgroundImage = Properties.Resources.btn_product_form_active;
        }

        private void pcb_btnProduct_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_productTag.ClientRectangle.Contains(lbl_productTag.PointToClient(Cursor.Position))) this.pcb_btnProduct.BackgroundImage = Properties.Resources.btn_product_form;
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

        private void lbl_productTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ProductForm>().Count() == 0)
            {
                ProductForm product = new ProductForm();
                product.Show();
                this.Close();
            }
        }

        //Formulário Orçamento
        private void pcb_btnBudget_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnBudget.BackgroundImage = Properties.Resources.btn_budget_form_active;
        }

        private void pcb_btnBudget_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_budgetTag.ClientRectangle.Contains(lbl_budgetTag.PointToClient(Cursor.Position))) this.pcb_btnBudget.BackgroundImage = Properties.Resources.btn_budget_form;
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

        private void lbl_budgetTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<BudgetForm>().Count() == 0)
            {
                BudgetForm budget = new BudgetForm();
                budget.Show();
                this.Close();
            }
        }

        //Formulário Configurações
        private void pcb_btnConfig_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnConfig.BackgroundImage = Properties.Resources.btn_config_main_active;
        }

        private void pcb_btnConfig_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_configTag.ClientRectangle.Contains(lbl_configTag.PointToClient(Cursor.Position))) this.pcb_btnConfig.BackgroundImage = Properties.Resources.btn_config_main;
        }

        private void pcb_btnConfig_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ConfigForm>().Count() == 0)
            {
                ConfigForm config = new ConfigForm();
                config.Show();
                this.Close();
            }
        }

        private void lbl_configTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ConfigForm>().Count() == 0)
            {
                ConfigForm config = new ConfigForm();
                config.Show();
                this.Close();
            }
        }

        //FUNÇÕES
        //Função que limpa o formulário
        private void clearForm(bool b)
        {
            this.lbl_parcelsNumber.Visible = b;
            this.lbl_lastParcelDate.Visible = b;
            this.lbl_lastParcelValueTag.Visible = b;
            this.txt_lastParcelValue.Visible = b;
            this.lbl_btnPayTag.Visible = b;
            this.pcb_btnPay.Visible = b;
        }

        //Função que formata o DataTable antes de atribuir os dados ao DataGridView
        private void formatDataTable(string query)
        {
            Globals.clientDataTable = Database.query(query);

            if (Globals.clientDataTable.Rows.Count > 0)
            {
                bool firstTime = true;
                DataColumn dataColumn = Globals.clientDataTable.Columns.Add("Endereço:", typeof(string));
                dataColumn.SetOrdinal(7);
                for (int i = 0; i < Globals.clientDataTable.Rows.Count; i++)
                {
                    //Formata endereço e e-mail do cliente e lista seus respectivos telefones
                    Globals.clientDataTable.Rows[i]["Endereço:"] = Globals.clientDataTable.Rows[i].ItemArray[2] + ", " + Globals.clientDataTable.Rows[i].ItemArray[3] + " - " + Globals.clientDataTable.Rows[i].ItemArray[4] + " - " + Globals.clientDataTable.Rows[i].ItemArray[5] + ", " + Globals.clientDataTable.Rows[i].ItemArray[6];

                    DataTable telephoneDataTable = Database.query("SELECT * FROM telefone WHERE idCliente = " + Globals.clientDataTable.Rows[i].ItemArray[0] + " ORDER BY tipoTelefone ASC;");
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
                    DataTable budgetsDataTable = Database.query("SELECT numeroOrcamento FROM orcamento WHERE idCliente = " + Globals.clientDataTable.Rows[i].ItemArray[0]);
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

                            if (Convert.ToBoolean(incomesDataRow.ItemArray[1])) continue; //Parcela paga
                            else
                            {
                                //Existe receita pendente
                                allIncomesPaid = false;
                                allBudgetsPaid = false;
                                break;
                            }
                        }
                    }
                    if (rbtn_clientsInDebt.Checked)
                    {
                        if (allParcelsPaid && allIncomesPaid && allBudgetsPaid) Globals.clientDataTable.Rows[i].Delete(); //Deleta da tabela os clientes que já pagaram todas as parcelas e receitas
                        else
                        {
                            this.clearForm(true);
                            this.dgv_clients.Visible = true;
                            this.pcb_btnEdit.Visible = true;
                            this.lbl_btnEditTag.Visible = true;
                            this.BackgroundImage = Properties.Resources.client_sisteg_bg_value;
                        }
                    }
                }

                //Remove as colunas vazias da tabela
                for (int col = Globals.clientDataTable.Columns.Count - 1; col >= 0; col--)
                {
                    bool removeColumn = true;
                    foreach (DataRow dataRow in Globals.clientDataTable.Rows)
                    {
                        if (dataRow.RowState != DataRowState.Deleted)
                        {
                            if (!String.IsNullOrEmpty(dataRow[col].ToString().Trim()))
                            {
                                removeColumn = false;
                                break;
                            }
                        }
                    }
                    if (removeColumn) Globals.clientDataTable.Columns.RemoveAt(col);
                }
            }
            else
            {
                this.BackgroundImage = Properties.Resources.product_sisteg_bg;
                this.dgv_clients.Visible = false;
                this.pcb_btnEdit.Visible = false;
                this.lbl_btnEditTag.Visible = false;
                this.clearForm(false);
            }
            this.dgv_clients.DataSource = Globals.clientDataTable;

            if (dgv_clients.Rows.Count == 0)
            {
                this.BackgroundImage = Properties.Resources.product_sisteg_bg;
                this.clearForm(false);
                this.dgv_clients.Visible = false;
                this.pcb_btnEdit.Visible = false;
                this.lbl_btnEditTag.Visible = false;
                this.dgv_clients.DataSource = null;
            }
        }


        //Função que lista os telefones do cliente na tabela de clientes
        private void listTelephones(bool firstTime, DataTable telephoneDataTable, int i)
        {
            foreach (DataRow dataRow in telephoneDataTable.Rows)
            {
                if (firstTime) if (!Globals.clientDataTable.Columns.Contains("Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":")) Globals.clientDataTable.Columns.Add("Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":", typeof(string));
                DataTable telephoneTypeDataTable = Database.query("SELECT numeroTelefone FROM telefone WHERE idCliente = " + Globals.clientDataTable.Rows[i].ItemArray[0] + " AND tipoTelefone = '" + dataRow.ItemArray[3] + "';");
                if (telephoneTypeDataTable.Rows.Count == 1)
                {
                    if (Globals.clientDataTable.Columns.Contains("Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":")) Globals.clientDataTable.Rows[i]["Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":"] = dataRow.ItemArray[4];
                    else
                    {
                        Globals.clientDataTable.Columns.Add("Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":", typeof(string));
                        Globals.clientDataTable.Rows[i]["Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":"] = dataRow.ItemArray[4];
                    }
                }
                else if (telephoneTypeDataTable.Rows.Count > 1)
                {
                    string numbers = null;
                    int j = 0;
                    foreach (DataRow dataRowType in telephoneTypeDataTable.Rows)
                    {
                        numbers += dataRowType.ItemArray[0].ToString().Trim();
                        if (j != Convert.ToInt32(telephoneTypeDataTable.Rows.Count - 1)) numbers += "; ";
                        j++;
                    }
                    if (Globals.clientDataTable.Columns.Contains("Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":")) Globals.clientDataTable.Rows[i]["Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":"] = numbers;
                }
            }
        }

        //Função que verifica as parcelas não pagas
        private int incomeParceled(DataTable parcelsDataTable, bool t, int count)
        {
            if (parcelsDataTable.Rows.Count > 0)
            {
                int k = 0;
                int total;
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

        //Função que atualiza os dados do cliente no DataGridView
        private void updateData()
        {
            if (this.dgv_clients.SelectedRows.Count == 1)
            {
                Globals.idCliente = Convert.ToInt32(this.dgv_clients.SelectedRows[0].Cells[0].Value);

                DataTable incomesDataTable = Database.query("SELECT receita.idReceita, receita.valorReceita, receita.dataTransacao, receita.recebimentoConfirmado, receita.repetirParcelarReceita FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento WHERE orcamento.idCliente = " + Globals.idCliente);
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
                                DataTable parcelsDataTable = Database.query("SELECT parcela.idParcela, parcela.valorParcela, parcela.dataTransacao FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento JOIN parcela ON parcela.idReceita = receita.idReceita WHERE orcamento.idCliente = " + Globals.idCliente + " AND parcela.recebimentoConfirmado = false AND receita.idReceita = " + incomesDataTable.Rows[i].ItemArray[0] + " ORDER BY parcela.dataTransacao;");
                                if (parcelsDataTable.Rows.Count > 0) count += this.incomeParceled(parcelsDataTable, true, count);
                                else this.clearForm(false);
                            }
                        }
                        else
                        {
                            //Recebimento não confirmado
                            incomesDataTable = Database.query("SELECT receita.idReceita, receita.valorReceita, receita.dataTransacao, receita.recebimentoConfirmado, receita.repetirParcelarReceita FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento WHERE orcamento.idCliente = " + Globals.idCliente + " AND receita.recebimentoConfirmado = false;");
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
                                    DataTable parcelsDataTable = Database.query("SELECT parcela.idParcela, parcela.valorParcela, parcela.dataTransacao FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento JOIN parcela ON parcela.idReceita = receita.idReceita WHERE orcamento.idCliente = " + Globals.idCliente + " AND parcela.recebimentoConfirmado = false AND receita.idReceita = " + incomesDataTable.Rows[0].ItemArray[0] + " ORDER BY parcela.dataTransacao;");
                                    if (parcelsDataTable.Rows.Count > 0) count = this.incomeParceled(parcelsDataTable, false, count);
                                    else this.clearForm(false);
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
                                DataTable parcelsDataTable = Database.query("SELECT parcela.idParcela, parcela.valorParcela, parcela.dataTransacao FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento JOIN parcela ON parcela.idReceita = receita.idReceita WHERE orcamento.idCliente = " + Globals.idCliente + " AND parcela.recebimentoConfirmado = false AND receita.idReceita = " + incomesDataTable.Rows[i].ItemArray[0] + " ORDER BY parcela.dataTransacao;");
                                if (parcelsDataTable.Rows.Count > 0) count = this.incomeParceled(parcelsDataTable, false, count);
                                else this.clearForm(false);
                            }
                        }
                    }
                    this.lbl_parcelsNumber.Text = count.ToString().Trim();

                    if (this.lbl_parcelsNumber.Text.Length > 1) this.lbl_parcelsNumber.Location = new System.Drawing.Point(568, 485);
                    else this.lbl_parcelsNumber.Location = new System.Drawing.Point(595, 485);
                }
                else
                {
                    //Receita parcelada
                    DataTable parcelsDataTable = Database.query("SELECT parcela.idParcela, parcela.valorParcela, parcela.dataTransacao FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento JOIN parcela ON parcela.idReceita = receita.idReceita WHERE orcamento.idCliente = " + Globals.idCliente + " AND parcela.recebimentoConfirmado = false ORDER BY parcela.dataTransacao;");
                    if (parcelsDataTable.Rows.Count > 0)
                    {
                        count = this.incomeParceled(parcelsDataTable, true, count);
                        this.lbl_parcelsNumber.Text = count.ToString().Trim();
                    }
                    else this.clearForm(false);
                }

                if (this.ids.Count > 0)
                {
                    this.lbl_lastParcelDate.Text = this.dates[0].ToShortDateString().Trim();
                    this.txt_lastParcelValue.Text = this.values[0].ToString("C").Trim();
                    if (DateTime.Compare(DateTime.Today, this.dates[0]) > 0) changeColor(243, 104, 82);
                    else changeColor(77, 255, 255);
                }
            }
        }

        //Função que define o valor do pagamento
        private bool setPaymentValue(DataTable differentIncomeValueDataTable)
        {
            if (income.ValorReceita == Math.Round(Convert.ToDecimal(differentIncomeValueDataTable.Rows[0].ItemArray[4]), 2))
            {
                //O valor do pagamento não foi alterado
                if (Database.payIncome(income))
                {
                    MessageBox.Show("Receita paga!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Imprimir cupom não fiscal
                    this.printInvoice();
                    return true;
                }
            }
            else
            {
                if (income.ValorReceita > Math.Round(Convert.ToDecimal(differentIncomeValueDataTable.Rows[0].ItemArray[4]), 2))
                {
                    //O valor do pagamento é maior do que o valor da parcela
                    MessageBox.Show("O valor a ser pago deve ser menor do que o valor da parcela!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                {
                    //O valor do pagamento é menor do que o valor da parcela
                    Account account = new Account();
                    account.IdConta = Convert.ToInt32(differentIncomeValueDataTable.Rows[0].ItemArray[1]);
                    income.IdReceita = Convert.ToInt32(differentIncomeValueDataTable.Rows[0].ItemArray[0]);
                    income.IdConta = Convert.ToInt32(differentIncomeValueDataTable.Rows[0].ItemArray[1]);
                    income.NumeroOrcamento = Convert.ToInt32(differentIncomeValueDataTable.Rows[0].ItemArray[2]);
                    income.IdCategoria = Convert.ToInt32(differentIncomeValueDataTable.Rows[0].ItemArray[3]);
                    Globals.numeroOrcamento = income.NumeroOrcamento;
                    decimal valorReceita = income.ValorReceita;
                    this.values[0] = valorReceita;
                    income.ValorReceita = Convert.ToDecimal(differentIncomeValueDataTable.Rows[0].ItemArray[4]) - income.ValorReceita;
                    income.DescricaoReceita = differentIncomeValueDataTable.Rows[0].ItemArray[5].ToString().Trim();
                    income.DataTransacao = Convert.ToDateTime(differentIncomeValueDataTable.Rows[0].ItemArray[6]);
                    income.ObservacoesReceita = differentIncomeValueDataTable.Rows[0].ItemArray[7].ToString().Trim();
                    income.RecebimentoConfirmado = false;
                    income.RepetirParcelarReceita = Convert.ToBoolean(differentIncomeValueDataTable.Rows[0].ItemArray[9]);
                    MessageBox.Show(income.RepetirParcelarReceita.ToString().Trim());
                    if(income.RepetirParcelarReceita == true)
                    {
                        income.ValorFixoReceita = Convert.ToBoolean(differentIncomeValueDataTable.Rows[0].ItemArray[10]);
                        income.RepeticoesValorFixoReceita = Convert.ToInt32(differentIncomeValueDataTable.Rows[0].ItemArray[11]);
                        income.ParcelarValorReceita = Convert.ToBoolean(differentIncomeValueDataTable.Rows[0].ItemArray[12]);
                        income.ParcelasReceita = Convert.ToInt32(differentIncomeValueDataTable.Rows[0].ItemArray[13]);
                        income.PeriodoRepetirParcelarReceita = differentIncomeValueDataTable.Rows[0].ItemArray[14].ToString().Trim();
                    }
                    decimal oldIncomeValue = Convert.ToDecimal(Database.query("SELECT valorReceita FROM receita WHERE idReceita = " + income.IdReceita).Rows[0].ItemArray[0]);
                    if (Database.updateIncome(income))
                    {
                        if (oldIncomeValue > income.ValorReceita) account.SaldoConta = Globals.saldoConta - (oldIncomeValue - income.ValorReceita);
                        else if (oldIncomeValue < income.ValorReceita) account.SaldoConta = Globals.saldoConta + (income.ValorReceita - oldIncomeValue);
                        if (Database.updateAccountBalance(account))
                        {
                            income.ValorReceita = valorReceita;
                            income.RecebimentoConfirmado = true;
                            if (Database.newIncome(income))
                            {
                                account.SaldoConta = Globals.saldoConta + income.ValorReceita;
                                if (Database.updateAccountBalance(account)) MessageBox.Show("Receita paga!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                else MessageBox.Show("[ERRO] Não foi possível atualizar o saldo da conta!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                //Imprimir cupom não fiscal
                                this.printInvoice();
                                return true;
                            }
                        }
                        else MessageBox.Show("[ERRO] Não foi possível atualizar o saldo da conta!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return false;
        }

        private void payParcel()
        {
            if (this.dgv_clients.SelectedRows.Count == 1)
            {
                Globals.idCliente = Convert.ToInt32(this.dgv_clients.SelectedRows[0].Cells[0].Value);
                DataTable incomesDataTable = Database.query("SELECT receita.idReceita, receita.numeroOrcamento, receita.valorReceita, receita.dataTransacao, receita.recebimentoConfirmado, receita.repetirParcelarReceita FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento WHERE orcamento.idCliente = " + Globals.idCliente);
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
                                DataTable oneIncomeDataTable = Database.query("SELECT receita.idReceita, receita.numeroOrcamento, receita.valorReceita, receita.dataTransacao, receita.recebimentoConfirmado, receita.repetirParcelarReceita FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento WHERE orcamento.idCliente = " + Globals.idCliente + " AND receita.recebimentoConfirmado = false;");
                                if (oneIncomeDataTable.Rows.Count == 1)
                                {
                                    //Receita parcelada em única parcela, ou seja, não há parcelas apenas uma receita com recebimento não confirmado
                                    DataTable differentIncomeValueDataTable = Database.query("SELECT * FROM receita WHERE idReceita = " + this.ids[0]);
                                    income.IdReceita = this.ids[0];
                                    Globals.numeroOrcamento = Convert.ToInt32(differentIncomeValueDataTable.Rows[0].ItemArray[2]);
                                    Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                                    string valorParcela = txt_lastParcelValue.Text.Trim();
                                    if (regexValor.IsMatch(valorParcela))
                                    {
                                        if (valorParcela.Contains("R$ ")) income.ValorReceita = Convert.ToDecimal(valorParcela.Substring(3).Trim());
                                        else if (valorParcela.Contains("R$")) income.ValorReceita = Convert.ToDecimal(valorParcela.Substring(2).Trim());
                                        else income.ValorReceita = Convert.ToDecimal(txt_lastParcelValue.Text.Trim());
                                    }
                                    else
                                    {
                                        MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }

                                    if (this.setPaymentValue(differentIncomeValueDataTable)) break;
                                    else break;
                                }

                                DataTable parcelsDataTable = Database.query("SELECT parcela.idParcela, orcamento.numeroOrcamento, parcela.valorParcela, parcela.dataTransacao FROM orcamento JOIN receita ON orcamento.numeroOrcamento = receita.numeroOrcamento JOIN parcela ON parcela.idReceita = receita.idReceita WHERE orcamento.idCliente = " + Globals.idCliente + " AND parcela.recebimentoConfirmado = false AND receita.idReceita = " + incomesDataTable.Rows[i].ItemArray[0] + " ORDER BY parcela.dataTransacao;");
                                if (parcelsDataTable.Rows.Count > 0)
                                {
                                    //Existe mais de uma parcela a ser paga
                                    for (int j = 0; j < parcelsDataTable.Rows.Count; j++)
                                    {
                                        DataTable differentParcelValueDataTable = Database.query("SELECT * FROM parcela WHERE idParcela = " + this.ids[0]);
                                        parcel.IdParcela = this.ids[0];
                                        Globals.numeroOrcamento = Convert.ToInt32(parcelsDataTable.Rows[0].ItemArray[1]);
                                        Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                                        string valorParcela = txt_lastParcelValue.Text.Trim();
                                        if (regexValor.IsMatch(valorParcela))
                                        {
                                            if (valorParcela.Contains("R$ ")) parcel.ValorParcela = Convert.ToDecimal(valorParcela.Substring(3).Trim());
                                            else if (valorParcela.Contains("R$")) parcel.ValorParcela = Convert.ToDecimal(valorParcela.Substring(2).Trim());
                                            else parcel.ValorParcela = Convert.ToDecimal(txt_lastParcelValue.Text.Trim());
                                        }
                                        else
                                        {
                                            MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            return;
                                        }

                                        if (parcel.ValorParcela == Math.Round(Convert.ToDecimal(differentParcelValueDataTable.Rows[0].ItemArray[5]), 2))
                                        {
                                            //O valor do pagamento não foi alterado
                                            if (Database.payParcel(parcel))
                                            {
                                                MessageBox.Show("Parcela paga!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                //Imprimir cupom não fiscal
                                                this.printInvoice();
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (parcel.ValorParcela > Math.Round(Convert.ToDecimal(differentParcelValueDataTable.Rows[0].ItemArray[5]), 2))
                                            {
                                                //O valor do pagamento é maior do que o valor da parcela
                                                MessageBox.Show("O valor a ser pago deve ser menor do que o valor da parcela!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                return;
                                            }
                                            else
                                            {
                                                //O valor do pagamento é menor do que o valor da parcela
                                                Account account = new Account();
                                                this.values[0] = parcel.ValorParcela;
                                                account.IdConta = Convert.ToInt32(differentParcelValueDataTable.Rows[0].ItemArray[3]);
                                                parcel.IdParcela = Convert.ToInt32(differentParcelValueDataTable.Rows[0].ItemArray[0]);
                                                parcel.IdReceita = Convert.ToInt32(differentParcelValueDataTable.Rows[0].ItemArray[1]);
                                                parcel.IdDespesa = Convert.ToInt32(differentParcelValueDataTable.Rows[0].ItemArray[2]);
                                                parcel.IdConta = Convert.ToInt32(differentParcelValueDataTable.Rows[0].ItemArray[3]);
                                                parcel.IdCategoria = Convert.ToInt32(differentParcelValueDataTable.Rows[0].ItemArray[4]);
                                                parcel.DescricaoParcela = differentParcelValueDataTable.Rows[0].ItemArray[6].ToString().Trim();
                                                parcel.DataTransacao = Convert.ToDateTime(differentParcelValueDataTable.Rows[0].ItemArray[7]);
                                                parcel.ObservacoesParcela = differentParcelValueDataTable.Rows[0].ItemArray[8].ToString().Trim();
                                                parcel.RecebimentoConfirmado = true;
                                                parcel.PagamentoConfirmado = Convert.ToBoolean(differentParcelValueDataTable.Rows[0].ItemArray[10]);
                                                decimal oldIncomeValue = Convert.ToDecimal(Database.query("SELECT valorParcela FROM parcela WHERE idParcela = " + parcel.IdParcela).Rows[0].ItemArray[0]);
                                                if (Database.updateParcel(parcel))
                                                {
                                                    if (oldIncomeValue > parcel.ValorParcela) account.SaldoConta = Globals.saldoConta - (oldIncomeValue - parcel.ValorParcela);
                                                    else if (oldIncomeValue < parcel.ValorParcela) account.SaldoConta = Globals.saldoConta + (parcel.ValorParcela - oldIncomeValue);
                                                    if (Database.updateAccountBalance(account))
                                                    {
                                                        parcel.ValorParcela = Convert.ToDecimal(differentParcelValueDataTable.Rows[0].ItemArray[5]) - parcel.ValorParcela;
                                                        parcel.RecebimentoConfirmado = false;
                                                        if (Database.newParcel(parcel))
                                                        {
                                                            account.SaldoConta = Globals.saldoConta + parcel.ValorParcela;
                                                            if (Database.updateAccountBalance(account)) MessageBox.Show("Parcela paga!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                            else MessageBox.Show("[ERRO] Não foi possível atualizar o saldo da conta!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                                            //Imprimir cupom não fiscal
                                                            this.printInvoice();
                                                            break;
                                                        }
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
                            income.IdReceita = this.ids[0];
                            Globals.numeroOrcamento = Convert.ToInt32(differentIncomeValueDataTable.Rows[0].ItemArray[2]);
                            Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                            string valorParcela = txt_lastParcelValue.Text.Trim();
                            if (regexValor.IsMatch(valorParcela))
                            {
                                if (valorParcela.Contains("R$ ")) income.ValorReceita = Convert.ToDecimal(valorParcela.Substring(3).Trim());
                                else if (valorParcela.Contains("R$")) income.ValorReceita = Convert.ToDecimal(valorParcela.Substring(2).Trim());
                                else income.ValorReceita = Convert.ToDecimal(txt_lastParcelValue.Text.Trim());
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

        //Função que imprime o cupom não fiscal
        private void printInvoice()
        {
            this.sfd_saveInvoice.Filter = "Portable Document File (.pdf)|*.pdf";

            //Abrir a janela "Salvar arquivo"
            if (this.sfd_saveInvoice.ShowDialog() == DialogResult.Cancel) return;
            else
            {
                try
                {
                    using (PdfWriter pdfWriter = new PdfWriter(sfd_saveInvoice.FileName, new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)))
                    {
                        //Document
                        var pdfDocument = new PdfDocument(pdfWriter);
                        var document = new Document(pdfDocument, PageSize.A6);

                        //Font
                        string avengeance = Globals.path + @"\assets\fonts\avengeance.ttf";

                        PdfFont pdfFontBody = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.COURIER);
                        PdfFontFactory.EmbeddingStrategy embeddingStrategy = new PdfFontFactory.EmbeddingStrategy();
                        PdfFont pdfFontCompanyName = PdfFontFactory.CreateFont(avengeance, embeddingStrategy);
                        PdfFont pdfFontTitle = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.TIMES_ROMAN);

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

                        clientTable.AddCell(new Cell(1, 2).Add(new Paragraph(this.dgv_clients.SelectedRows[0].Cells[1].Value.ToString().Trim())).AddStyle(bodyStyle));
                        clientTable.AddCell(new Cell(1, 2).Add(new Paragraph(this.dgv_clients.SelectedRows[0].Cells[2].Value.ToString().Trim() + ", " + this.dgv_clients.SelectedRows[0].Cells[3].Value.ToString().Trim())).AddStyle(bodyStyle));
                        clientTable.AddCell(new Cell(1, 2).Add(new Paragraph(this.dgv_clients.SelectedRows[0].Cells[4].Value.ToString().Trim() + " - " + this.dgv_clients.SelectedRows[0].Cells[5].Value.ToString().Trim() + ", " + this.dgv_clients.SelectedRows[0].Cells[6].Value.ToString().Trim())).AddStyle(bodyStyle));
                        if (this.dgv_clients.Columns.Contains("Telefone celular:"))
                        {
                            clientTable.AddCell(new Cell().Add(new Paragraph("Telefone celular:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle));
                            clientTable.AddCell(new Cell().Add(new Paragraph(this.dgv_clients.SelectedRows[0].Cells["Telefone celular:"].Value.ToString().Trim())).AddStyle(bodyStyle));
                        }
                        if (this.dgv_clients.Columns.Contains("Telefone comercial:"))
                        {
                            clientTable.AddCell(new Cell().Add(new Paragraph("Telefone comercial:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle));
                            clientTable.AddCell(new Cell().Add(new Paragraph(this.dgv_clients.SelectedRows[0].Cells["Telefone comercial:"].Value.ToString().Trim())).AddStyle(bodyStyle));
                        }
                        if (this.dgv_clients.Columns.Contains("Telefone residencial:"))
                        {
                            clientTable.AddCell(new Cell().Add(new Paragraph("Telefone residencial:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle));
                            clientTable.AddCell(new Cell().Add(new Paragraph(this.dgv_clients.SelectedRows[0].Cells["Telefone residencial:"].Value.ToString().Trim())).AddStyle(bodyStyle));
                        }
                        clientTable.SetBorderBottom(new SolidBorder(1));

                        document.Add(clientTable);

                        //Table 4
                        Table invoiceTable = new Table(columnWidth);
                        invoiceTable.AddCell(new Cell(1, 2).Add(new Paragraph("PARCELA")).AddStyle(titleStyle).SetBorderBottom(new SolidBorder(1)));

                        DataTable incomesDataTable = Database.query("SELECT DISTINCT receita.recebimentoConfirmado, receita.parcelarValorReceita, receita.parcelasReceita FROM receita JOIN orcamento ON receita.numeroOrcamento = orcamento.numeroOrcamento WHERE orcamento.numeroOrcamento = " + Globals.numeroOrcamento);
                        int count = 0;
                        int total = 1;
                        if (incomesDataTable.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(incomesDataTable.Rows[0].ItemArray[1]))
                            {
                                //Recebimento confirmado
                                if (Convert.ToBoolean(incomesDataTable.Rows[0].ItemArray[2]))
                                {
                                    //Receita parcelada
                                    total = Convert.ToInt32(incomesDataTable.Rows[0].ItemArray[2]);
                                    DataTable parcelsDataTable = Database.query("SELECT parcela.recebimentoConfirmado FROM parcela JOIN receita ON parcela.idReceita = receita.idReceita JOIN orcamento ON receita.numeroOrcamento = orcamento.numeroOrcamento WHERE orcamento.numeroOrcamento = " + Globals.numeroOrcamento);
                                    if (parcelsDataTable.Rows.Count > 0)
                                    {
                                        int countChanged = 0;
                                        for (int i = 0; i < parcelsDataTable.Rows.Count; i++)
                                        {
                                            if (parcelsDataTable.Rows.Count >= total) if (!Convert.ToBoolean(parcelsDataTable.Rows[i].ItemArray[0])) countChanged++;
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
                                count++;
                                if (Convert.ToBoolean(incomesDataTable.Rows[0].ItemArray[2]))
                                {
                                    //Receita parcelada
                                    total = Convert.ToInt32(incomesDataTable.Rows[0].ItemArray[2]);
                                    DataTable parcelsDataTable = Database.query("SELECT parcela.recebimentoConfirmado FROM parcela JOIN receita ON parcela.idReceita = receita.idReceita JOIN orcamento ON receita.numeroOrcamento = orcamento.numeroOrcamento WHERE orcamento.numeroOrcamento = " + Globals.numeroOrcamento);
                                    if (parcelsDataTable.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < parcelsDataTable.Rows.Count; i++) if (Convert.ToBoolean(parcelsDataTable.Rows[i].ItemArray[0])) count++;
                                    }
                                }
                            }
                        }
                        invoiceTable.AddCell(new Cell().Add(new Paragraph("Data da parcela:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle));
                        invoiceTable.AddCell(new Cell().Add(new Paragraph(this.dates[0].ToShortDateString().Trim())).SetBorder(Border.NO_BORDER).AddStyle(bodyStyle));

                        invoiceTable.AddCell(new Cell().Add(new Paragraph("Data do pagamento:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle));
                        invoiceTable.AddCell(new Cell().Add(new Paragraph(DateTime.Today.ToShortDateString().Trim())).SetBorder(Border.NO_BORDER).AddStyle(bodyStyle));

                        invoiceTable.AddCell(new Cell().Add(new Paragraph("Valor:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle));
                        invoiceTable.AddCell(new Cell().Add(new Paragraph(this.values[0].ToString("C").Trim())).SetBorder(Border.NO_BORDER).AddStyle(bodyStyle));

                        invoiceTable.AddCell(new Cell().Add(new Paragraph("Parcela:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle));
                        invoiceTable.AddCell(new Cell().Add(new Paragraph(count.ToString() + "/" + total.ToString().Trim())).SetBorder(Border.NO_BORDER).AddStyle(bodyStyle));

                        document.Add(invoiceTable);

                        document.Close();

                        System.Diagnostics.Process.Start(this.sfd_saveInvoice.FileName);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("[ERRO] " + exception.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //BUSCAR CLIENTE NA TABELA
        private void txt_searchClient_TextChange(object sender, EventArgs e)
        {
            string searchClient = this.txt_searchClient.Text;
            if (searchClient.Trim() != null) this.formatDataTable("SELECT cliente.idCliente, cliente.nomeCliente AS 'Nome:', cliente.enderecoCliente, cliente.numeroResidencia, cliente.bairroCliente, cliente.cidadeCliente, cliente.estadoCliente, cliente.emailCliente FROM cliente WHERE cliente.nomeCliente LIKE '%" + searchClient + "%' ORDER BY cliente.nomeCliente;");
        }

        //PAGAR PARCELA
        private void pcb_btnPay_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnPay.BackgroundImage = Properties.Resources.btn_pay_parcel_active;
        }

        private void pcb_btnPay_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnPayTag.ClientRectangle.Contains(lbl_btnPayTag.PointToClient(Cursor.Position))) this.pcb_btnPay.BackgroundImage = Properties.Resources.btn_pay_parcel;
        }

        private void pcb_btnPay_Click(object sender, EventArgs e)
        {
            this.payParcel();
        }

        private void lbl_btnPayTag_Click(object sender, EventArgs e)
        {
            this.payParcel();
        }

        //ADICIONAR CLIENTE
        private void pcb_btnAdd_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnAdd.BackgroundImage = Properties.Resources.btn_add_active;
        }

        private void pcb_btnAdd_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnAddTag.ClientRectangle.Contains(lbl_btnAddTag.PointToClient(Cursor.Position))) this.pcb_btnAdd.BackgroundImage = Properties.Resources.btn_add;
        }

        private void pcb_btnAdd_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<AddClient>().Count() == 0)
            {
                AddClient addClient = new AddClient(null);
                addClient.pcb_btnUpdate.Hide();
                addClient.lbl_btnUpdateTag.Hide();
                addClient.pcb_btnDelete.Hide();
                addClient.lbl_btnDeleteTag.Hide();
                addClient.lbl_clientRegisterTag.Location = new System.Drawing.Point(115, 224);
                addClient.pcb_clientRegister.Location = new System.Drawing.Point(27, 198);
                addClient.lbl_btnCancelTag.Text = "CANCELAR CADASTRO///";
                addClient.Show();
                this.Close();
            }
        }

        private void lbl_btnAddTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<AddClient>().Count() == 0)
            {
                AddClient addClient = new AddClient(null);
                addClient.pcb_btnUpdate.Hide();
                addClient.lbl_btnUpdateTag.Hide();
                addClient.pcb_btnDelete.Hide();
                addClient.lbl_btnDeleteTag.Hide();
                addClient.lbl_clientRegisterTag.Location = new System.Drawing.Point(115, 224);
                addClient.pcb_clientRegister.Location = new System.Drawing.Point(27, 198);
                addClient.lbl_btnCancelTag.Text = "CANCELAR CADASTRO///";
                addClient.Show();
                this.Close();
            }
        }

        //EDITAR CLIENTE
        private void pcb_btnEdit_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnEdit.BackgroundImage = Properties.Resources.btn_edit_active;
        }

        private void pcb_btnEdit_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnEditTag.ClientRectangle.Contains(lbl_btnEditTag.PointToClient(Cursor.Position))) this.pcb_btnEdit.BackgroundImage = Properties.Resources.btn_edit;
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
                        DataTable dataTableClient = Database.query("SELECT * FROM cliente WHERE idCliente = " + dataGridViewRow.Cells[0].Value.ToString().Trim() + ";");
                        AddClient addClient = new AddClient(dataTableClient);
                        addClient.editClick = true;
                        addClient.Show();
                        this.Close();
                    }
                }
            }
        }

        private void lbl_pcbEditTag_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(this.dgv_clients.Rows.Count) == 0) MessageBox.Show("Não há cliente selecionado para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Convert.ToInt32(this.dgv_clients.SelectedRows.Count) > 1) MessageBox.Show("Selecione apenas uma linha da tabela para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                foreach (DataGridViewRow dataGridViewRow in this.dgv_clients.SelectedRows)
                {
                    if (Application.OpenForms.OfType<AddIncome>().Count() == 0)
                    {
                        DataTable dataTableClient = Database.query("SELECT * FROM cliente WHERE idCliente = " + dataGridViewRow.Cells[0].Value.ToString().Trim() + ";");
                        AddClient addClient = new AddClient(dataTableClient);
                        addClient.editClick = true;
                        addClient.Show();
                        this.Close();
                    }
                }
            }
        }

        //ENCERRAR APLICAÇÃO
        private void pcb_appClose_Click(object sender, EventArgs e)
        {
            if (((DialogResult)MessageBox.Show("Tem certeza que deseja encerrar a aplicação?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)).ToString().ToUpper() == "YES") Application.Exit();
        }

        //MINIMIZAR APLICAÇÃO
        private void pcb_minimizeProgram_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO

        //Formatação da tabela após disposição dos dados
        private void dgv_clients_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.dgv_clients.Rows.Count > 0)
            {
                int width = 0;
                int columnCount = 0;
                foreach (DataGridViewColumn dataGridViewColumn in dgv_clients.Columns) dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                if (dgv_clients.ColumnCount > 0)
                {
                    this.dgv_clients.Columns[0].Visible = false;
                    for (int i = 2; i < 7; i++) this.dgv_clients.Columns[i].Visible = false;
                    for (int i = 1; i < dgv_clients.ColumnCount; i++)
                    {
                        dgv_clients.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        if (this.dgv_clients.Columns[i].Visible) 
                        {
                            width += Convert.ToInt32(dgv_clients.Columns[i].Width);
                            columnCount++;
                        }
                    }
                    if (width < dgv_clients.Width)
                    {
                        width = dgv_clients.Width / columnCount;
                        for (int i = 1; i < dgv_clients.ColumnCount; i++)
                        {
                            dgv_clients.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dgv_clients.Columns[i].Width = width;
                        }
                    }
                }
            }
        }
        private void changeColor(int r, int g, int b)
        {
            this.lbl_parcelsNumber.ForeColor = Color.FromArgb(r, g, b);
            this.lbl_lastParcelDate.ForeColor = Color.FromArgb(r, g, b);
        }

        private void rbtn_clientsInDebt_CheckedChanged(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.product_sisteg_bg;
            this.clearForm(false);
            this.dgv_clients.Visible = false;
            this.pcb_btnEdit.Visible = false;
            this.lbl_btnEditTag.Visible = false;
            this.dgv_clients.DataSource = null;
            this.formatDataTable("SELECT cliente.idCliente, cliente.nomeCliente AS 'Nome:', cliente.enderecoCliente, cliente.numeroResidencia, cliente.bairroCliente, cliente.cidadeCliente, cliente.estadoCliente, cliente.emailCliente AS 'E-mail:' FROM cliente ORDER BY cliente.nomeCliente;");
        }

        private void rbtn_allClients_CheckedChanged(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.product_sisteg_bg;
            this.clearForm(false);
            this.dgv_clients.Visible = true;
            this.pcb_btnEdit.Visible = true;
            this.lbl_btnEditTag.Visible = true;
            this.dgv_clients.DataSource = null;
            this.formatDataTable("SELECT cliente.idCliente, cliente.nomeCliente AS 'Nome:', cliente.enderecoCliente, cliente.numeroResidencia, cliente.bairroCliente, cliente.cidadeCliente, cliente.estadoCliente, cliente.emailCliente AS 'E-mail:' FROM cliente ORDER BY cliente.nomeCliente;");
        }

        private void dgv_clients_SelectionChanged(object sender, EventArgs e)
        {
            this.updateData();
        }

        //Tooltip da quantidade de  parcelas a pagar restantes
        private void lbl_parcelsNumber_MouseHover(object sender, EventArgs e)
        {
            this.ttp_lastParcelNumber.Show("Quantidade de parcelas a pagar restantes.", this.lbl_parcelsNumber);
        }

        //Tooltip da data da próxima parcela a pagar
        private void lbl_lastParcelDate_MouseHover(object sender, EventArgs e)
        {
            this.ttp_lastParcelDate.Show("Data da próxima parcela a pagar.", this.lbl_lastParcelDate);
        }
    }
}
