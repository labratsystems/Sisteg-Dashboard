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
    public partial class BudgetForm : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        protected internal ClientStep clientStep;
        protected internal BudgetStep budgetStep;
        protected internal ProductStep productStep;
        protected internal DataTable clientStepDataTable, budgetStepDataTable, budgetNumberDataTable;
        protected internal int step = 0, budgetNumber = 0, selectedIndex = -1;
        private static int idReceita, idCliente = 0;
        private List<Repeat> repeats = new List<Repeat>();
        private List<Parcel> parcels = new List<Parcel>();

        public BudgetForm()
        {
            InitializeComponent();
            clientStep = new ClientStep(this, null) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true};
            this.panel_steps.Controls.Add(clientStep);
            clientStep.Show();
            this.pcb_btnGoBack.Visible = false;
            this.pcb_btnEndBudget.Visible = false;
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

        private void selectClientLastBudget()
        {
            //Seleciona todos os dados do último orçamento cadastrado do cliente
            budgetStepDataTable = Database.query("SELECT * FROM orcamento WHERE orcamento.idCliente = " + clientStepDataTable.Rows[0].ItemArray[0] + " ORDER BY numeroOrcamento DESC LIMIT 1");
            this.budgetNumber = Convert.ToInt32(budgetStepDataTable.Rows[0].ItemArray[0]);
            productStep = new ProductStep(this, budgetStepDataTable) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            this.panel_steps.Controls.Remove(budgetStep);
            this.panel_steps.Controls.Add(productStep);
            productStep.Show();
            this.pcb_btnGoBack.Visible = true;
            this.pcb_btnGoForward.Visible = false;
            this.pcb_btnEndBudget.Visible = true;
            this.pcb_btnEndBudget.Location = new Point(845, 417);
        }

        //FUNÇÃO QUE RETORNA OS COMPONENTES DO FORMULÁRIO AO ESTADO INICIAL CASO NÃO HAJA REPETIÇÕES OU PARCELAS
        private void doNotRepeatOrParcel()
        {
            budgetStep.txt_parcels.Clear();
            budgetStep.txt_parcels.Hide();
            budgetStep.cbb_period.SelectedIndex = -1;
            budgetStep.cbb_period.Text = " Período";
            budgetStep.cbb_period.Hide();
        }

        //FUNÇÃO QUE RETORNA OS CAMPOS DO FORMULÁRIO AO ESTADO INICIAL
        private void clearFields()
        {
            budgetStep.txt_laborValue.Clear();
            budgetStep.txt_incomeDescription.Clear();
            budgetStep.mtb_budgetDate.Text = DateTime.Today.ToShortDateString();
            budgetStep.cbb_paymentCondition.SelectedIndex = budgetStep.cbb_paymentCondition.FindString(" Dinheiro");
            budgetStep.ckb_confirmedBudget.Checked = false;
            budgetStep.txt_incomeObservations.Clear();
            this.doNotRepeatOrParcel();
        }

        //FUNÇÃO QUE SELECIONA A DATA DA TRANSAÇÃO DA PARCELA DE ACORDO COM O PERÍDO ESCOLHIDO PELO USUÁRIO
        private void periodSelection(int i, Income income, List<Parcel> parcels)
        {
            if (i == 0)
            {
                switch (income.periodoRepetirParcelarReceita)
                {
                    case "Diário":
                        parcels[i].dataTransacao = income.dataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        parcels[i].dataTransacao = income.dataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        parcels[i].dataTransacao = income.dataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        parcels[i].dataTransacao = income.dataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        parcels[i].dataTransacao = income.dataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        parcels[i].dataTransacao = income.dataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        parcels[i].dataTransacao = income.dataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        parcels[i].dataTransacao = income.dataTransacao.AddYears(1);
                        break;
                }
            }
            else
            {
                switch (income.periodoRepetirParcelarReceita)
                {
                    case "Diário":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        parcels[i].dataTransacao = parcels[i - 1].dataTransacao.AddYears(1);
                        break;
                }
            }
        }

        private void displayOnPanel()
        {
            //Retorna ao formulário de orçamentos se usuário estiver no último formulário
            if (this.panel_steps.Controls.Contains(productStep)) this.panel_steps.Controls.Remove(productStep);

            //Avança ao formulário de orçamentos se usuário estiver no primeiro formulário
            if (this.panel_steps.Controls.Contains(clientStep)) this.panel_steps.Controls.Remove(clientStep);
            this.panel_steps.Controls.Add(budgetStep);
            budgetStep.Show();

            this.pcb_btnGoBack.Visible = true;
            this.pcb_btnEdit.Visible = false;
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

        private void pcb_btnClient_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnClient.Image = Properties.Resources.btn_client_active;
        }

        private void pcb_btnClient_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnClient.Image = Properties.Resources.btn_client;
        }

        private void pcb_btnClient_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ClientForm>().Count() == 0)
            {
                ClientForm client = new ClientForm();
                client.Show();
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
                ProductForm productForm = new ProductForm();
                productForm.Show();
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

        private void pcb_btnGoBack_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnGoBack.Image = Properties.Resources.btn_goBack_active;
        }

        private void pcb_btnGoBack_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnGoBack.Image = Properties.Resources.btn_goBack;
        }

        //VOLTAR
        private void pcb_btnGoBack_Click(object sender, EventArgs e)
        {
            step -= 1;
            if(step == 0)
            {
                //Usuário retornou ao painel de clientes
                clientStep = new ClientStep(this, clientStepDataTable) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Remove(budgetStep);
                this.panel_steps.Controls.Add(clientStep);
                clientStep.Show();
                this.pcb_btnGoBack.Visible = false;
            }
            else if(step == 1)
            {
                if (clientStep.cbb_clientName.SelectedIndex == -1)
                {
                    MessageBox.Show("Selecione um cliente, antes de avançar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    step += 1;
                }

                //Usuário avançou e retornou ao formulário de orçamentos
                if (idCliente != 0)
                {
                    MessageBox.Show(idCliente.ToString());
                    MessageBox.Show(budgetNumber.ToString());
                    if (this.budgetNumber != 0) budgetStepDataTable = Database.query("SELECT * FROM orcamento WHERE orcamento.numeroOrcamento = " + this.budgetNumber);
                    else budgetStepDataTable = clientStep.budgetStepDataTable;

                    if (budgetStepDataTable.Rows.Count > 0) budgetStep = new BudgetStep(this, budgetStepDataTable) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                    else budgetStep = new BudgetStep(this, null) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                }
                //Usuário não passou pelo formulário de orçamentos
                else budgetStep = new BudgetStep(this, null) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.displayOnPanel();
                this.pcb_btnEndBudget.Visible = false;
                this.pcb_btnEndBudget.Location = new Point(845, 569);
                this.pcb_btnGoForward.Visible = true;
            }
        }

        private void pcb_btnGoForward_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnGoForward.Image = Properties.Resources.btn_goForward_active;
        }

        private void pcb_btnGoForward_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnGoForward.Image = Properties.Resources.btn_goForward;
        }

        //AVANÇAR
        private void pcb_btnGoForward_Click(object sender, EventArgs e)
        {
            step += 1;
            MessageBox.Show(step.ToString());
            if(step == 1)
            {
                if (clientStep.cbb_clientName.SelectedIndex == -1)
                {
                    MessageBox.Show("Selecione um cliente antes de avançar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    step -= 1;
                }
                else
                {
                    //Cadastro de orçamento
                    MessageBox.Show("Cadastro de orçamento");
                    this.budgetNumber = 0;

                    //Usuário avançou e retornou ao formulário de orçamentos
                    /*if (idCliente != 0)
                    {
                        MessageBox.Show("Usuário avançou e retornou ao formulário de orçamentos");
                        //selectedIndex = clientStep.cbb_budgetNumber.SelectedIndex;
                        clientStep.cbb_budgetNumber.SelectedIndex = -1;
                        clientStep.cbb_budgetNumber.Text = " Número do orçamento";
                        budgetStepDataTable = clientStep.budgetStepDataTable;
                        if (budgetStepDataTable.Rows.Count > 0)
                        {
                            //Cliente já tem orçamento
                            MessageBox.Show("Cliente já tem orçamento");
                            budgetStep = new BudgetStep(this, budgetStepDataTable) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                            budgetStep.pcb_btnUpdate.Visible = false;
                            budgetStep.pcb_btnDelete.Visible = false;
                        }
                        else
                        {
                            //Cliente não tem orçamento
                            budgetStep = new BudgetStep(this, null) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                            MessageBox.Show("Cliente não tem orçamento");
                        }
                    }
                    else
                    {*/
                        //Usuário não passou pelo formulário de orçamentos
                        budgetStep = new BudgetStep(this, null) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                        MessageBox.Show("Usuário não passou pelo formulário de orçamentos");
                    //}

                    //Todos os dados do cliente
                    clientStepDataTable = clientStep.clientStepDataTable;
                    this.displayOnPanel();
                }
            }
            else if(step == 2)
            {
                //Todos os dados do orçamento do cliente
                MessageBox.Show(this.budgetNumber.ToString());
                budgetStepDataTable = Database.query("SELECT * FROM orcamento WHERE orcamento.numeroOrcamento = " + this.budgetNumber);
                if (budgetStepDataTable.Rows.Count > 0)
                {
                    //Cliente já tem orçamento cadastrado
                    idCliente = Convert.ToInt32(budgetStepDataTable.Rows[0].ItemArray[1]);
                    productStep = new ProductStep(this, budgetStepDataTable) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                    this.panel_steps.Controls.Remove(budgetStep);
                    this.panel_steps.Controls.Add(productStep);
                    productStep.Show();
                    this.pcb_btnGoBack.Visible = true;
                    this.pcb_btnGoForward.Visible = false;
                    this.pcb_btnEndBudget.Visible = true;
                    this.pcb_btnEndBudget.Location = new Point(845, 417);
                }
                else
                {
                    if (String.IsNullOrEmpty(budgetStep.mtb_budgetDate.Text) || String.IsNullOrEmpty(budgetStep.txt_laborValue.Text))
                    {
                        MessageBox.Show("Preencha todos os campos antes de avançar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        step -= 1;
                    }
                    else
                    {
                        //Cliente ainda não cadastrou orçamento
                        if ((budgetStep.ckb_parcelValue.Checked) && ((String.IsNullOrEmpty(budgetStep.txt_parcels.Text.Trim())) || (budgetStep.cbb_period.SelectedIndex == -1))) MessageBox.Show("Informe o número de parcelas da receita e o período em que elas se repetem!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else
                        {
                            Budget budget = new Budget();
                            Income income = new Income();
                            budget.idCliente = Convert.ToInt32(clientStepDataTable.Rows[0].ItemArray[0]);
                            idCliente = Convert.ToInt32(clientStepDataTable.Rows[0].ItemArray[0]);
                            budget.dataOrcamento = Convert.ToDateTime(budgetStep.mtb_budgetDate.Text.Trim());
                            income.dataTransacao = Convert.ToDateTime(budgetStep.mtb_budgetDate.Text.Trim());
                            Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                            string valorTrabalho = budgetStep.txt_laborValue.Text;
                            if (regexValor.IsMatch(valorTrabalho))
                            {
                                if (valorTrabalho.Contains("R$ "))
                                {
                                    budget.valorTrabalho = Convert.ToDecimal(valorTrabalho.Substring(3));
                                    budget.valorTotal = Convert.ToDecimal(valorTrabalho.Substring(3));
                                    income.valorReceita = Convert.ToDecimal(valorTrabalho.Substring(3));
                                }
                                else if (valorTrabalho.Contains("R$"))
                                {
                                    budget.valorTrabalho = Convert.ToDecimal(valorTrabalho.Substring(2));
                                    budget.valorTotal = Convert.ToDecimal(valorTrabalho.Substring(2));
                                    income.valorReceita = Convert.ToDecimal(valorTrabalho.Substring(2));
                                }
                                else
                                {
                                    budget.valorTrabalho = Convert.ToDecimal(budgetStep.txt_laborValue.Text);
                                    budget.valorTotal = Convert.ToDecimal(budgetStep.txt_laborValue.Text);
                                    income.valorReceita = Convert.ToDecimal(budgetStep.txt_laborValue.Text);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                budgetStep.txt_laborValue.Clear();
                                budgetStep.txt_laborValue.PlaceholderText = "";
                                budgetStep.txt_laborValue.Focus();
                                return;
                            }
                            budget.condicaoPagamento = budgetStep.cbb_paymentCondition.SelectedItem.ToString().Trim();
                            if (budgetStep.ckb_confirmedBudget.Checked) budget.orcamentoConfirmado = true; else budget.orcamentoConfirmado = false;
                            income.descricaoReceita = budgetStep.txt_incomeDescription.Text;
                            income.categoriaReceita = " Orçamentos";
                            income.idConta = 1;
                            income.observacoesReceita = budgetStep.txt_incomeObservations.Text;
                            if (budgetStep.ckb_incomeReceived.Checked) income.recebimentoConfirmado = true; else income.recebimentoConfirmado = false;

                            if (budgetStep.ckb_parcelValue.Checked)
                            {
                                if (budgetStep.ckb_parcelValue.Checked)
                                {
                                    income.repetirParcelarReceita = true;

                                    income.parcelarValorReceita = true;
                                    income.parcelasReceita = Convert.ToInt32(budgetStep.txt_parcels.Text);
                                    income.periodoRepetirParcelarReceita = budgetStep.cbb_period.SelectedItem.ToString();
                                    income.valorReceita = income.valorReceita / income.parcelasReceita;

                                    if (Database.newBudget(budget))
                                    {
                                        income.numeroOrcamento = Convert.ToInt32(Database.query("SELECT numeroOrcamento FROM orcamento ORDER BY numeroOrcamento DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                        if (Database.newIncome(income))
                                        {
                                            List<Parcel> parcels = new List<Parcel>();
                                            int success = 1;
                                            for (int i = 0; i < (income.parcelasReceita - 1); i++)
                                            {
                                                parcels.Add(new Parcel());
                                                parcels[i].idReceita = Convert.ToInt32(Database.query("SELECT idReceita FROM receita ORDER BY idReceita DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                                parcels[i].valorParcela = income.valorReceita;
                                                parcels[i].descricaoParcela = income.descricaoReceita;
                                                this.periodSelection(i, income, parcels);
                                                parcels[i].categoriaParcela = income.categoriaReceita;
                                                parcels[i].observacoesParcela = income.observacoesReceita;
                                                parcels[i].recebimentoConfirmado = false;
                                                if (Database.newParcel(parcels[i])) continue;
                                                else
                                                {
                                                    success = 0;
                                                    break;
                                                }
                                            }
                                            if (success == 0) MessageBox.Show("Não foi possível cadastrar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            this.selectClientLastBudget();
                                            MessageBox.Show("Receita cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            MessageBox.Show("Orçamento cadastrado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            this.clearFields();
                                        }
                                        else MessageBox.Show("[ERRO] Não foi possível cadastrar receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else MessageBox.Show("[ERRO] Não foi possível cadastrar orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else income.parcelarValorReceita = false;
                            }
                            else
                            {
                                income.repetirParcelarReceita = false;
                                if (Database.newBudget(budget))
                                {
                                    income.numeroOrcamento = Convert.ToInt32(Database.query("SELECT numeroOrcamento FROM orcamento ORDER BY numeroOrcamento DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                    if (Database.newIncome(income))
                                    {
                                        this.selectClientLastBudget();
                                        MessageBox.Show("Receita cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        MessageBox.Show("Orçamento cadastrado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        this.clearFields();
                                    }
                                    else MessageBox.Show("[ERRO] Não foi possível cadastrar receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else MessageBox.Show("[ERRO] Não foi possível cadastrar orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        //EDITAR
        private void pcb_btnEdit_MouseEnter(object sender, EventArgs e) { this.pcb_btnEdit.Image = Properties.Resources.btn_modify_active; }

        private void pcb_btnEndBudget_Click(object sender, EventArgs e)
        {
            step = 0;
            clientStep = new ClientStep(this, null) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            this.panel_steps.Controls.Remove(productStep);
            this.panel_steps.Controls.Add(clientStep);
            clientStep.Show();
            this.pcb_btnGoBack.Visible = false;
            this.pcb_btnEndBudget.Visible = false;
            this.pcb_btnEndBudget.Location = new Point(845, 569);
            this.pcb_btnGoForward.Visible = true;
            MessageBox.Show("Orçamento concluído com sucesso!");
        }

        private void pcb_btnEndBudget_MouseEnter(object sender, EventArgs e) { this.pcb_btnEndBudget.Image = Properties.Resources.btn_budget_active; }

        private void pcb_btnEndBudget_MouseLeave(object sender, EventArgs e) { this.pcb_btnEndBudget.Image = Properties.Resources.btn_budget; }

        private void pcb_btnEdit_MouseLeave(object sender, EventArgs e) { this.pcb_btnEdit.Image = Properties.Resources.btn_modify; }

        private void pcb_btnEdit_Click(object sender, EventArgs e)
        {
            if (clientStep.cbb_clientName.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um cliente antes de avançar!");
                if (clientStep.cbb_budgetNumber.Visible == true) if (clientStep.cbb_budgetNumber.SelectedIndex == -1) MessageBox.Show("Selecione um orçamento antes de avançar!");
            }
            clientStepDataTable = clientStep.clientStepDataTable;
            this.budgetNumber = clientStep.budgetNumber;
            budgetStepDataTable = Database.query("SELECT * FROM orcamento WHERE orcamento.numeroOrcamento = " + clientStep.budgetNumber);
            budgetStep = new BudgetStep(this, budgetStepDataTable) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            if (this.panel_steps.Controls.Contains(productStep)) this.panel_steps.Controls.Remove(productStep); if (this.panel_steps.Controls.Contains(clientStep)) this.panel_steps.Controls.Remove(clientStep);
            step = 1;
            this.panel_steps.Controls.Add(budgetStep);
            budgetStep.Show();
            this.pcb_btnGoBack.Visible = true;
            this.pcb_btnEdit.Visible = false;
        }
    }
}
