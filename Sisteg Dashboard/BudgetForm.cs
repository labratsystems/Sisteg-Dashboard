using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class BudgetForm : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        Bitmap backGround, backGroundTemp;

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

        private void initialize()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            backGroundTemp = new Bitmap(Properties.Resources.empty_bg);
            backGround = new Bitmap(backGroundTemp, backGroundTemp.Width, backGroundTemp.Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.DrawImageUnscaled(backGround, 0, 0);
            base.OnPaint(e);
        }

        //CARREGA INSTÂNCIA DO FORMULÁRIO DE ORÇAMENTOS CONJUNTO À UMA INSTÂNCIA DO PAINEL DE CLIENTES
        public BudgetForm()
        {
            InitializeComponent();
            initialize();
            Globals.clientStep = new ClientStep(this, null) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true};
            this.panel_steps.Controls.Add(Globals.clientStep);
            Globals.clientStep.Show();
            
            if(Globals.idConta != 0)
            {
                DataTable sumTotalValueDataTable = Database.query("SELECT somarTotal FROM conta WHERE idConta = " + Globals.idConta);
                if(sumTotalValueDataTable.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(sumTotalValueDataTable.Rows[0].ItemArray[0])) Globals.saldoConta = Convert.ToDecimal(Database.query("SELECT saldoConta FROM conta WHERE idConta = " + Globals.idConta).Rows[0].ItemArray[0]);
                    else Globals.saldoConta = 0;
                }
            }

            this.pcb_btnGoBack.Visible = false;
            this.lbl_btnGoBackTag.Visible = false;
            this.pcb_btnEndBudget.Visible = false;
            this.lbl_btnEndBudgetTag.Visible = false;
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

        //Formulário Cliente
        private void pcb_btnClient_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnClient.BackgroundImage = Properties.Resources.btn_client_form_active;
        }

        private void pcb_btnClient_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_clientTag.ClientRectangle.Contains(lbl_clientTag.PointToClient(Cursor.Position))) this.pcb_btnClient.BackgroundImage = Properties.Resources.btn_client_form;
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

        private void lbl_clientTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ClientForm>().Count() == 0)
            {
                ClientForm client = new ClientForm();
                client.Show();
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
                ProductForm productForm = new ProductForm();
                productForm.Show();
                this.Close();
            }
        }

        private void lbl_productTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ProductForm>().Count() == 0)
            {
                ProductForm productForm = new ProductForm();
                productForm.Show();
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

        //Seleciona todos os dados do último orçamento cadastrado do cliente
        private void selectClientLastBudget()
        {
            Globals.budgetStepDataTable = Database.query("SELECT * FROM orcamento WHERE orcamento.idCliente = " + Globals.clientStepDataTable.Rows[0].ItemArray[0] + " ORDER BY numeroOrcamento DESC LIMIT 1;");
            Globals.numeroOrcamento = Convert.ToInt32(Globals.budgetStepDataTable.Rows[0].ItemArray[0]);
            
            Globals.productStep = new ProductStep(this) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            this.panel_steps.Controls.Remove(Globals.budgetStep);
            this.panel_steps.Controls.Add(Globals.productStep);
            Globals.productStep.Show();

            this.pcb_btnGoBack.Visible = true;
            this.lbl_btnGoBackTag.Visible = true;
            this.pcb_btnGoForward.Visible = false;
            this.lbl_btnGoForwardTag.Visible = false;
            this.pcb_btnEndBudget.Visible = true;
            this.lbl_btnEndBudgetTag.Visible = true;
            this.lbl_btnEndBudgetTag.Location = new Point(925, 445);
            this.pcb_btnEndBudget.Location = new Point(845, 417);
        }

        //Função que seleciona a data de transação da parcela de acordo com o período escolhido pelo usuário
        private void periodSelection(int i, Income income, List<Parcel> parcels)
        {
            if (i == 0)
            {
                switch (income.PeriodoRepetirParcelarReceita)
                {
                    case "Diário":
                        parcels[i].DataTransacao = income.DataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        parcels[i].DataTransacao = income.DataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        parcels[i].DataTransacao = income.DataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        parcels[i].DataTransacao = income.DataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        parcels[i].DataTransacao = income.DataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        parcels[i].DataTransacao = income.DataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        parcels[i].DataTransacao = income.DataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        parcels[i].DataTransacao = income.DataTransacao.AddYears(1);
                        break;
                }
            }
            else
            {
                switch (income.PeriodoRepetirParcelarReceita)
                {
                    case "Diário":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddDays(1);
                        break;
                    case "Semanal":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddDays(7);
                        break;
                    case "A cada 2 semanas":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddDays(14);
                        break;
                    case "Mensal":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddMonths(1);
                        break;
                    case "Bimestral":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddMonths(2);
                        break;
                    case "Trimestral":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddMonths(3);
                        break;
                    case "Semestral":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddMonths(6);
                        break;
                    case "Anual":
                        parcels[i].DataTransacao = parcels[i - 1].DataTransacao.AddYears(1);
                        break;
                }
            }
        }

        //Função que dispõe outros formulários dentro do painel
        private void displayOnPanel()
        {
            //Retorna ao formulário inicial se usuário estiver no último formulário
            if (this.panel_steps.Controls.Contains(Globals.productStep)) this.panel_steps.Controls.Remove(Globals.productStep);

            //Avança ao formulário de orçamentos se usuário estiver no primeiro formulário
            if (this.panel_steps.Controls.Contains(Globals.clientStep)) this.panel_steps.Controls.Remove(Globals.clientStep);
            this.panel_steps.Controls.Add(Globals.budgetStep);
            Globals.budgetStep.Show();

            this.pcb_btnGoBack.Visible = true;
            this.lbl_btnGoBackTag.Visible = true;
            this.pcb_btnEdit.Visible = false;
            this.lbl_btnEditTag.Visible = false;
        }

        //Função que retorna ao formulário anterior
        private void goBack()
        {
            Globals.step -= 1;
            if (Globals.step == 0)
            {
                //Usuário retornou ao painel de clientes
                Globals.clientStep = new ClientStep(this, Globals.clientStepDataTable) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Remove(Globals.budgetStep);
                this.panel_steps.Controls.Add(Globals.clientStep);
                Globals.clientStep.Show();
                
                this.lbl_btnGoBackTag.Visible = false;
                this.pcb_btnGoBack.Visible = false;
            }
            else if (Globals.step == 1)
            {
                if (Globals.clientStep.cbb_clientName.SelectedIndex == -1)
                {
                    MessageBox.Show("Selecione um cliente, antes de avançar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Globals.step += 1;
                }

                //Usuário avançou e retornou ao formulário de orçamentos
                if (Globals.idCliente != 0)
                {
                    if (Globals.numeroOrcamento != 0) Globals.budgetStepDataTable = Database.query("SELECT * FROM orcamento WHERE orcamento.numeroOrcamento = " + Globals.numeroOrcamento);

                    if (Globals.budgetStepDataTable.Rows.Count > 0) Globals.budgetStep = new BudgetStep(this, Globals.budgetStepDataTable) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                    else Globals.budgetStep = new BudgetStep(this, null) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                }
                //Usuário não passou pelo formulário de orçamentos
                else Globals.budgetStep = new BudgetStep(this, null) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                
                this.displayOnPanel();
                this.lbl_btnGoForwardTag.Visible = true;
                this.pcb_btnGoForward.Visible = true;
                this.lbl_btnEndBudgetTag.Visible = false;
                this.pcb_btnEndBudget.Visible = false;
                this.lbl_btnEndBudgetTag.Location = new Point(925, 597);
                this.pcb_btnEndBudget.Location = new Point(845, 569);
            }
        }

        //Função que avança ao próximo formulário
        private void goForward()
        {
            Globals.step += 1;
            if (Globals.step == 1)
            {
                if (Globals.clientStep.cbb_clientName.SelectedIndex == -1)
                {
                    MessageBox.Show("Selecione um cliente antes de avançar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Globals.step -= 1;
                }
                else
                {
                    //Cadastro de orçamento
                    Globals.numeroOrcamento = 0;

                    //Usuário não passou pelo formulário de orçamentos
                    Globals.budgetStep = new BudgetStep(this, null) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };

                    //Todos os dados do cliente
                    this.displayOnPanel();
                }
            }
            else if (Globals.step == 2)
            {
                //Todos os dados do orçamento do cliente
                Globals.budgetStepDataTable = Database.query("SELECT * FROM orcamento WHERE orcamento.numeroOrcamento = " + Globals.numeroOrcamento);
                if (Globals.budgetStepDataTable.Rows.Count > 0)
                {
                    //Cliente já tem orçamento cadastrado
                    Globals.idCliente = Convert.ToInt32(Globals.budgetStepDataTable.Rows[0].ItemArray[1]);
                    
                    Globals.productStep = new ProductStep(this) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                    this.panel_steps.Controls.Remove(Globals.budgetStep);
                    this.panel_steps.Controls.Add(Globals.productStep);
                    Globals.productStep.Show();

                    this.lbl_btnGoBackTag.Visible = true;
                    this.pcb_btnGoBack.Visible = true;
                    this.lbl_btnGoForwardTag.Visible = false;
                    this.pcb_btnGoForward.Visible = false;
                    this.lbl_btnEndBudgetTag.Visible = true;
                    this.pcb_btnEndBudget.Visible = true;
                    this.lbl_btnEndBudgetTag.Location = new Point(925, 445);
                    this.pcb_btnEndBudget.Location = new Point(845, 417);
                }
                else
                {
                    if (String.IsNullOrEmpty(Globals.budgetStep.mtb_budgetDate.Text.Trim()) || String.IsNullOrEmpty(Globals.budgetStep.txt_laborValue.Text.Trim()) || Globals.budgetStep.cbb_incomeAccount.SelectedIndex == -1 || Globals.budgetStep.cbb_paymentCondition.SelectedIndex == -1)
                    {
                        MessageBox.Show("Preencha todos os campos antes de avançar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Globals.step -= 1;
                    }
                    else
                    {
                        //Cliente ainda não cadastrou orçamento
                        if ((Globals.budgetStep.ckb_parcelValue.Checked) && ((String.IsNullOrEmpty(Globals.budgetStep.txt_parcels.Text.Trim())) || (Globals.budgetStep.cbb_period.SelectedIndex == -1))) MessageBox.Show("Informe o número de parcelas da receita e o período em que elas se repetem!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else
                        {
                            Account account = new Account();
                            Budget budget = new Budget();
                            Income income = new Income();
                            account.IdConta = income.IdConta;
                            budget.IdCliente = Convert.ToInt32(Globals.clientStepDataTable.Rows[0].ItemArray[0]);
                            Globals.idCliente = Convert.ToInt32(Globals.clientStepDataTable.Rows[0].ItemArray[0]);
                            budget.DataOrcamento = Convert.ToDateTime(Globals.budgetStep.mtb_budgetDate.Text.Trim());
                            income.DataTransacao = Convert.ToDateTime(Globals.budgetStep.mtb_budgetDate.Text.Trim());
                            Regex regexValor = new Regex(@"[R$ ]?[R$]?\d{1,3}(\.\d{3})*,\d{2}");
                            string valorTrabalho = Globals.budgetStep.txt_laborValue.Text.Trim();
                            if (regexValor.IsMatch(valorTrabalho))
                            {
                                if (valorTrabalho.Contains("R$ "))
                                {
                                    budget.ValorTrabalho = Convert.ToDecimal(valorTrabalho.Substring(3).Trim());
                                    budget.ValorTotal = Convert.ToDecimal(valorTrabalho.Substring(3).Trim());
                                    income.ValorReceita = Convert.ToDecimal(valorTrabalho.Substring(3).Trim());
                                }
                                else if (valorTrabalho.Contains("R$"))
                                {
                                    budget.ValorTrabalho = Convert.ToDecimal(valorTrabalho.Substring(2).Trim());
                                    budget.ValorTotal = Convert.ToDecimal(valorTrabalho.Substring(2).Trim());
                                    income.ValorReceita = Convert.ToDecimal(valorTrabalho.Substring(2).Trim());
                                }
                                else
                                {
                                    budget.ValorTrabalho = Convert.ToDecimal(Globals.budgetStep.txt_laborValue.Text.Trim());
                                    budget.ValorTotal = Convert.ToDecimal(Globals.budgetStep.txt_laborValue.Text.Trim());
                                    income.ValorReceita = Convert.ToDecimal(Globals.budgetStep.txt_laborValue.Text.Trim());
                                }
                            }
                            else
                            {
                                MessageBox.Show("Formato monetário incorreto!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                Globals.budgetStep.txt_laborValue.Clear();
                                Globals.budgetStep.txt_laborValue.PlaceholderText = "";
                                Globals.budgetStep.txt_laborValue.Focus();
                                return;
                            }
                            budget.CondicaoPagamento = Globals.budgetStep.cbb_paymentCondition.SelectedItem.ToString().Trim();
                            if (Globals.budgetStep.ckb_confirmedBudget.Checked) budget.OrcamentoConfirmado = true; else budget.OrcamentoConfirmado = false;
                            income.DescricaoReceita = Globals.budgetStep.txt_incomeDescription.Text.Trim();
                            income.IdConta = Convert.ToInt32(Database.query("SELECT idConta FROM conta WHERE nomeConta = '" + Globals.budgetStep.cbb_incomeAccount.SelectedItem.ToString().Trim() + "';").Rows[0].ItemArray[0]);
                            income.IdCategoria = Convert.ToInt32(Database.query("SELECT idCategoria FROM categoria WHERE nomeCategoria = 'Orçamentos';").Rows[0].ItemArray[0]);
                            income.ObservacoesReceita = Globals.budgetStep.txt_incomeObservations.Text.Trim();
                            if (Globals.budgetStep.ckb_incomeReceived.Checked) income.RecebimentoConfirmado = true; else income.RecebimentoConfirmado = false;

                            if (Globals.budgetStep.ckb_parcelValue.Checked)
                            {
                                if (Globals.budgetStep.ckb_parcelValue.Checked)
                                {
                                    income.RepetirParcelarReceita = true;

                                    income.ParcelarValorReceita = true;
                                    income.ParcelasReceita = Convert.ToInt32(Globals.budgetStep.txt_parcels.Text.Trim());
                                    income.PeriodoRepetirParcelarReceita = Globals.budgetStep.cbb_period.SelectedItem.ToString().Trim();
                                    income.ValorReceita = income.ValorReceita / income.ParcelasReceita;

                                    if (Database.newBudget(budget))
                                    {
                                        income.NumeroOrcamento = Convert.ToInt32(Database.query("SELECT numeroOrcamento FROM orcamento ORDER BY numeroOrcamento DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                        Globals.numeroOrcamento = income.NumeroOrcamento;
                                        if (Database.newIncome(income))
                                        {
                                            Globals.idReceita = Convert.ToInt32(Database.query("SELECT idReceita FROM receita ORDER BY idReceita DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                            List<Parcel> parcels = new List<Parcel>();
                                            int success = 1;
                                            for (int i = 0; i < (income.ParcelasReceita - 1); i++)
                                            {
                                                parcels.Add(new Parcel());
                                                parcels[i].IdReceita = Convert.ToInt32(Database.query("SELECT idReceita FROM receita ORDER BY idReceita DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                                parcels[i].IdConta = income.IdConta;
                                                parcels[i].IdCategoria = income.IdCategoria;
                                                parcels[i].ValorParcela = income.ValorReceita;
                                                parcels[i].DescricaoParcela = income.DescricaoReceita;
                                                this.periodSelection(i, income, parcels);
                                                parcels[i].ObservacoesParcela = income.ObservacoesReceita;
                                                parcels[i].RecebimentoConfirmado = false;
                                                if (Database.newParcel(parcels[i])) continue;
                                                else
                                                {
                                                    success = 0;
                                                    break;
                                                }
                                            }
                                            if (success == 0) MessageBox.Show("[ERRO] Não foi possível cadastrar todas as parcelas!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            
                                            this.selectClientLastBudget();
                                            Globals.idCliente = Convert.ToInt32(Globals.budgetStepDataTable.Rows[0].ItemArray[1]);
                                            Globals.productStep = new ProductStep(this) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                                            this.panel_steps.Controls.Remove(Globals.budgetStep);
                                            this.panel_steps.Controls.Add(Globals.productStep);
                                            Globals.productStep.Show();

                                            this.lbl_btnGoBackTag.Visible = true;
                                            this.pcb_btnGoBack.Visible = true;
                                            this.lbl_btnGoForwardTag.Visible = false;
                                            this.pcb_btnGoForward.Visible = false;
                                            this.lbl_btnEndBudgetTag.Visible = true;
                                            this.pcb_btnEndBudget.Visible = true;
                                            this.lbl_btnEndBudgetTag.Location = new Point(925, 445);
                                            this.pcb_btnEndBudget.Location = new Point(845, 417);
                                        }
                                        else MessageBox.Show("[ERRO] Não foi possível cadastrar receita!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else MessageBox.Show("[ERRO] Não foi possível cadastrar orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else income.ParcelarValorReceita = false;
                            }
                            else
                            {
                                income.RepetirParcelarReceita = false;
                                if (Database.newBudget(budget))
                                {
                                    income.NumeroOrcamento = Convert.ToInt32(Database.query("SELECT numeroOrcamento FROM orcamento ORDER BY numeroOrcamento DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                    Globals.numeroOrcamento = income.NumeroOrcamento;
                                    if (Database.newIncome(income))
                                    {
                                        Globals.idReceita = Convert.ToInt32(Database.query("SELECT idReceita FROM receita ORDER BY idReceita DESC LIMIT 1;").Rows[0].ItemArray[0]);
                                        
                                        Globals.productStep = new ProductStep(this) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                                        this.panel_steps.Controls.Remove(Globals.budgetStep);
                                        this.panel_steps.Controls.Add(Globals.productStep);
                                        Globals.productStep.Show();
                                        
                                        this.lbl_btnGoBackTag.Visible = true;
                                        this.pcb_btnGoBack.Visible = true;
                                        this.lbl_btnGoForwardTag.Visible = false;
                                        this.pcb_btnGoForward.Visible = false;
                                        this.lbl_btnEndBudgetTag.Visible = true;
                                        this.pcb_btnEndBudget.Visible = true;
                                        this.lbl_btnEndBudgetTag.Location = new Point(925, 445);
                                        this.pcb_btnEndBudget.Location = new Point(845, 417);
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

        //Função que edita orçamento
        private void editBudget()
        {
            if (Globals.clientStep.cbb_clientName.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um cliente antes de avançar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (Globals.clientStep.cbb_budgetNumber.Visible == true) if (Globals.clientStep.cbb_budgetNumber.SelectedIndex == -1) MessageBox.Show("Selecione um orçamento antes de avançar!");
            }
            Globals.budgetStepDataTable = Database.query("SELECT * FROM orcamento WHERE orcamento.numeroOrcamento = " + Globals.numeroOrcamento);
            
            Globals.budgetStep = new BudgetStep(this, Globals.budgetStepDataTable) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            if (this.panel_steps.Controls.Contains(Globals.productStep)) this.panel_steps.Controls.Remove(Globals.productStep); 
            if (this.panel_steps.Controls.Contains(Globals.clientStep)) this.panel_steps.Controls.Remove(Globals.clientStep);
            Globals.step = 1;
            
            this.panel_steps.Controls.Add(Globals.budgetStep);
            Globals.budgetStep.Show();
            
            this.lbl_btnGoBackTag.Visible = true;
            this.pcb_btnGoBack.Visible = true;
            this.lbl_btnEditTag.Visible = false;
            this.pcb_btnEdit.Visible = false;
        }

        //Função que finaliza orçamento
        private void endBudget()
        {
            Globals.step = 0;
            
            Globals.clientStep = new ClientStep(this, null) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            this.panel_steps.Controls.Remove(Globals.productStep);
            this.panel_steps.Controls.Add(Globals.clientStep);
            Globals.clientStep.Show();
            
            this.lbl_btnGoBackTag.Visible = false;
            this.pcb_btnGoBack.Visible = false;
            this.lbl_btnEndBudgetTag.Visible = false;
            this.pcb_btnEndBudget.Visible = false;
            this.lbl_btnEndBudgetTag.Location = new Point(925, 445);
            this.pcb_btnEndBudget.Location = new Point(845, 569);
            this.lbl_btnGoForwardTag.Visible = true;
            this.pcb_btnGoForward.Visible = true;
            
            MessageBox.Show("Orçamento concluído com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        //VOLTAR
        private void pcb_btnGoBack_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnGoBack.BackgroundImage = Properties.Resources.btn_go_back_active;
        }

        private void pcb_btnGoBack_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnGoBackTag.ClientRectangle.Contains(lbl_btnGoBackTag.PointToClient(Cursor.Position))) this.pcb_btnGoBack.BackgroundImage = Properties.Resources.btn_go_back;
        }

        private void pcb_btnGoBack_Click(object sender, EventArgs e)
        {
            this.goBack();
        }

        private void lbl_btnGoBackTag_Click(object sender, EventArgs e)
        {
            this.goBack();
        }

        private void pcb_btnGoForward_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnGoForward.BackgroundImage = Properties.Resources.btn_go_forward_active;
        }

        private void pcb_btnGoForward_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnGoForwardTag.ClientRectangle.Contains(lbl_btnGoForwardTag.PointToClient(Cursor.Position))) this.pcb_btnGoForward.BackgroundImage = Properties.Resources.btn_go_forward;
        }

        //AVANÇAR
        private void pcb_btnGoForward_Click(object sender, EventArgs e)
        {
            this.goForward();
        }

        private void lbl_btnGoForwardTag_Click(object sender, EventArgs e)
        {
            this.goForward();
        }

        //EDITAR ORÇAMENTO
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
            this.editBudget();
        }

        private void lbl_btnEditTag_Click(object sender, EventArgs e)
        {
            this.editBudget();
        }

        //FINALIZAR ORÇAMENTO
        private void pcb_btnEndBudget_MouseEnter(object sender, EventArgs e) 
        { 
            this.pcb_btnEndBudget.BackgroundImage = Properties.Resources.btn_confirm_budget_active; 
        }

        private void pcb_btnEndBudget_MouseLeave(object sender, EventArgs e) 
        {
            if (!lbl_btnEndBudgetTag.ClientRectangle.Contains(lbl_btnEndBudgetTag.PointToClient(Cursor.Position))) this.pcb_btnEndBudget.BackgroundImage = Properties.Resources.btn_confirm_budget; 
        }

        private void pcb_btnEndBudget_Click(object sender, EventArgs e)
        {
            this.endBudget();
        }

        private void lbl_btnEndBudgetTag_Click(object sender, EventArgs e)
        {
            this.endBudget();
        }
    }
}
