using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class Main : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        protected internal bool isCountSelected = false;
        //Bitmap backGround, backGroundTemp;

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

        /*protected internal void initialize(Image image)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            backGroundTemp = new Bitmap(image);
            backGround = new Bitmap(backGroundTemp, backGroundTemp.Width, backGroundTemp.Height);
        }*/

        /*protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.DrawImageUnscaled(backGround, 0, 0);
            base.OnPaint(e);
        }*/

        //INICIA INSTÂNCIA DO PAINEL, POPULANDO OS GRÁFICOS, TABELAS E INSCRIÇÕES DO QUADRO COM AS INFORMAÇÕES DO BANCO DE DADOS DE ACORDO COM O PERÍODO MENSAL
        public Main()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Globals.step = 0;
            Globals.month = 0;
            Globals.expenseGraphs = new ExpenseGraphs(this, Globals.month);
            Globals.transactions = new Transactions(this, Globals.month);

            //Popula o combobox de conta ativa
            DataTable accountsDataTable = Database.query("SELECT conta.nomeConta FROM conta ORDER BY conta.nomeConta;");
            for (int i = 0; i < accountsDataTable.Rows.Count; i++) this.cbb_activeAccount.Items.Insert(i, " " + accountsDataTable.Rows[i].ItemArray[0].ToString().Trim());

            //Seleciona a conta ativa
            DataTable activeAccountDataTable = Database.query("SELECT conta.idConta, conta.nomeConta FROM conta WHERE conta.contaAtiva = 1 ORDER BY conta.nomeConta;");
            if(activeAccountDataTable.Rows.Count > 0)
            {
                this.cbb_activeAccount.SelectedIndex = this.cbb_activeAccount.FindString(" " + activeAccountDataTable.Rows[0].ItemArray[1].ToString().Trim());
                Globals.idConta = Convert.ToInt32(activeAccountDataTable.Rows[0].ItemArray[0]);

                decimal saldoConta = 0;
                decimal totalDespesas = 0;
                decimal totalReceitas = 0;
                saldoConta = Convert.ToDecimal(Database.query("SELECT saldoConta FROM conta WHERE idConta = " + Globals.idConta).Rows[0].ItemArray[0]);
                DataTable incomesTotalDataTable = Database.query("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + Globals.idConta +
                                                                " UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + Globals.idConta +
                                                                " UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + Globals.idConta + ");");
                if (incomesTotalDataTable.Rows[0].ItemArray[0] != System.DBNull.Value) totalReceitas = Convert.ToDecimal(incomesTotalDataTable.Rows[0].ItemArray[0]);

                DataTable expensesTotalTable = Database.query("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta +
                                                              " UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta +
                                                              " UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + ");");
                if(expensesTotalTable.Rows[0].ItemArray[0] != System.DBNull.Value) totalDespesas = Convert.ToDecimal(expensesTotalTable.Rows[0].ItemArray[0]);

                Account account = new Account();
                account.IdConta = Globals.idConta;
                account.SaldoConta = saldoConta + totalReceitas - totalDespesas;
                if (Convert.ToBoolean(Database.query("SELECT somarTotal FROM conta WHERE idConta = " + Globals.idConta).Rows[0].ItemArray[0]))
                {
                    this.lbl_balance.Text = String.Format("{0:C}", account.SaldoConta);
                    if (Convert.ToDecimal(account.SaldoConta) < 0)
                    {
                        this.lbl_balanceTag.ForeColor = Color.FromArgb(243, 104, 82);
                        this.lbl_balance.ForeColor = Color.FromArgb(243, 104, 82);
                    }
                    else
                    {
                        this.lbl_balanceTag.ForeColor = Color.FromArgb(77, 255, 255);
                        this.lbl_balance.ForeColor = Color.FromArgb(77, 255, 255);
                    }
                }
                else
                {
                    this.lbl_balance.Text = String.Format("{0:C}", (totalReceitas - totalDespesas));
                    if (Convert.ToDecimal(totalReceitas - totalDespesas) < 0)
                    {
                        this.lbl_balanceTag.ForeColor = Color.FromArgb(243, 104, 82);
                        this.lbl_balance.ForeColor = Color.FromArgb(243, 104, 82);
                    }
                    else
                    {
                        this.lbl_balanceTag.ForeColor = Color.FromArgb(77, 255, 255);
                        this.lbl_balance.ForeColor = Color.FromArgb(77, 255, 255);
                    }
                }
            }

            //Inicia painel dos gráficos de receitas
            Globals.incomeGraphs = new IncomeGraphs(this, Globals.month) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            this.panel_steps.Controls.Add(Globals.incomeGraphs);
            Globals.incomeGraphs.Show();
        }

        //MENU DE NAVEGAÇÃO DA APLICAÇÃO

        //Formulário Cliente
        private void pcb_btnClient_MouseEnter(object sender, EventArgs e) 
        { 
            this.pcb_btnClient.Image = Properties.Resources.btn_client_form_active; 
        }

        private void pcb_btnClient_MouseLeave(object sender, EventArgs e) 
        {
            if (!lbl_clientTag.ClientRectangle.Contains(lbl_clientTag.PointToClient(Cursor.Position))) this.pcb_btnClient.Image = Properties.Resources.btn_client_form;
        }

        private void pcb_btnClient_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ProductForm>().Count() == 0)
            {
                ClientForm clientForm = new ClientForm();
                clientForm.Show();
                this.Close();
            }
        }

        private void lbl_clientTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ProductForm>().Count() == 0)
            {
                ClientForm clientForm = new ClientForm();
                clientForm.Show();
                this.Close();
            }
        }

        //Formulário Produto
        private void pcb_btnProduct_MouseEnter(object sender, EventArgs e) 
        { 
            this.pcb_btnProduct.Image = Properties.Resources.btn_product_form_active; 
        }

        private void pcb_btnProduct_MouseLeave(object sender, EventArgs e) 
        {
            if (!lbl_productTag.ClientRectangle.Contains(lbl_productTag.PointToClient(Cursor.Position))) this.pcb_btnProduct.Image = Properties.Resources.btn_product_form; 
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

        //Formulário Orçamentos
        private void pcb_btnBudget_MouseEnter(object sender, EventArgs e) 
        { 
            this.pcb_btnBudget.Image = Properties.Resources.btn_budget_form_active; 
        }

        private void pcb_btnBudget_MouseLeave(object sender, EventArgs e) 
        {
            if (!lbl_budgetTag.ClientRectangle.Contains(lbl_budgetTag.PointToClient(Cursor.Position))) this.pcb_btnBudget.Image = Properties.Resources.btn_budget_form; 
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
            this.pcb_btnConfig.Image = Properties.Resources.btn_config_main_active; 
        }

        private void pcb_btnConfig_MouseLeave(object sender, EventArgs e) 
        {
            if (!lbl_configTag.ClientRectangle.Contains(lbl_configTag.PointToClient(Cursor.Position))) this.pcb_btnConfig.Image = Properties.Resources.btn_config_main; 
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

        //Função que renderiza as inscrições no quadro de acordo com as informaçõe vindas do banco de dados
        protected internal void renderLabels(Label label, string query)
        {
            DataTable dataTable = Database.query(query);
            if (!String.IsNullOrEmpty(dataTable.Rows[0].ItemArray[0].ToString().Trim())) foreach (DataRow dataRow in dataTable.Rows) label.Text = String.Format("{0:C}", dataRow.ItemArray[0]); else label.Text = "R$ 0,00";
        }

        //Função que limpa os dados do formulário
        private void clearChartData()
        {
            Globals.transactions.dgv_transactions.DataSource = null;
            foreach (var series in Globals.expenseGraphs.chart_expenseAccount.Series) series.Points.Clear();
            foreach (var series in Globals.expenseGraphs.chart_expenseCategory.Series) series.Points.Clear();
            foreach (var series in Globals.incomeGraphs.chart_incomeAccount.Series) series.Points.Clear();
            foreach (var series in Globals.incomeGraphs.chart_incomeCategory.Series) series.Points.Clear();
            foreach (var series in Globals.transactions.chart_dailyExpenses.Series) series.Points.Clear();
        }

        //Mudar período mensal para atualização dos gráficos, tabelas e inscrições no formulário
        private void changeMonth(int month)
        {
            DateTime changeMonth = DateTime.Now.AddMonths(month);
            DataTable dailyExpensesDataTable;
            DataTable accountDataTable = Database.query("SELECT idConta, nomeConta FROM conta;");
            DataTable incomeCategoryDataTable = Database.query("SELECT idCategoria, nomeCategoria FROM categoria WHERE categoriaReceita != 0;");
            DataTable expenseCategoryDataTable = Database.query("SELECT idCategoria, nomeCategoria FROM categoria WHERE categoriaDespesa != 0;");
            if (month < -1)
            {
                if (month == -2)
                {
                    clearChartData();
                    if (incomeCategoryDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow categoryDataRow in incomeCategoryDataTable.Rows)
                        {
                            Globals.incomeGraphs.renderIncomeChart(Globals.incomeGraphs.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + Globals.idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND receita.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime'));", categoryDataRow);
                        }
                    }
                    else Globals.incomeGraphs.chart_incomeCategory.Hide();

                    if (accountDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow accountDataRow in accountDataTable.Rows)
                        {
                            Globals.incomeGraphs.renderIncomeChart(Globals.incomeGraphs.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND receita.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                                                             "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                                                             "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime'));", accountDataRow);
                        }
                    }
                    else Globals.incomeGraphs.chart_incomeAccount.Hide();

                    this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + Globals.idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                             "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                             "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime'));");

                    DataTable transactionsDataTable = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE receita.idConta = " + Globals.idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                     "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                     "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                     "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') ORDER BY dataTransacao DESC;");
                    if (transactionsDataTable.Rows.Count > 0)
                    {
                        Globals.transactions.dgv_transactions.Show();
                        Globals.transactions.dgv_transactions.DataSource = transactionsDataTable;
                    }
                    else Globals.transactions.dgv_transactions.Hide();

                    dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') GROUP BY dataTransacao UNION ALL " +
                                                            "SELECT idRepeticao, dataTransacao FROM repeticao WHERE repeticao.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.idDespesa != 0 GROUP BY dataTransacao UNION ALL " +
                                                            "SELECT idParcela, dataTransacao FROM parcela WHERE parcela.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.idDespesa != 0 GROUP BY dataTransacao; ");

                    if (dailyExpensesDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                        {
                            Globals.transactions.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') AND despesa.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "');", dailyExpensesDataRow);
                        }
                    }
                    else
                    {
                        Globals.transactions.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM").Trim();
                        Globals.transactions.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                        Globals.transactions.lbl_noExpenses.Show();
                        Globals.transactions.chart_dailyExpenses.Hide();
                    }

                    if (expenseCategoryDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow categoryDataRow in expenseCategoryDataTable.Rows)
                        {
                            Globals.expenseGraphs.renderExpenseChart(Globals.expenseGraphs.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                                                                  "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                                                                  "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime'));", categoryDataRow);
                        }
                    }
                    else Globals.expenseGraphs.chart_expenseCategory.Hide();

                    if (accountDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow accountDataRow in accountDataTable.Rows)
                        {
                            Globals.expenseGraphs.renderExpenseChart(Globals.expenseGraphs.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                                                                 "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                                                                 "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime'));", accountDataRow);
                        }
                    }
                    else Globals.expenseGraphs.chart_expenseAccount.Hide();

                    this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '-2 month', 'localtime') " +
                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '-2 month', 'localtime') " +
                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '-2 month', 'localtime'));");

                    this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime'));");
                }
                else
                {
                    clearChartData();
                    int monthLess = month + 1;
                    if (incomeCategoryDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow categoryDataRow in incomeCategoryDataTable.Rows)
                        {
                            Globals.incomeGraphs.renderIncomeChart(Globals.incomeGraphs.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + Globals.idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND receita.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime'));", categoryDataRow);
                        }
                    }
                    else Globals.incomeGraphs.chart_incomeCategory.Hide();

                    if (accountDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow accountDataRow in accountDataTable.Rows)
                        {
                            Globals.incomeGraphs.renderIncomeChart(Globals.incomeGraphs.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND receita.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                                                             "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                                                             "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime'));", accountDataRow);
                        }
                    }
                    else Globals.incomeGraphs.chart_incomeAccount.Hide();

                    this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + Globals.idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                             "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                             "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime'));");

                    DataTable transactionsDataTable = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE receita.idConta = " + Globals.idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                     "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                     "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                     "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') ORDER BY dataTransacao DESC;");
                    
                    if (transactionsDataTable.Rows.Count > 0)
                    {
                        Globals.transactions.dgv_transactions.Show();
                        Globals.transactions.dgv_transactions.DataSource = transactionsDataTable;
                    }
                    else Globals.transactions.dgv_transactions.Hide();

                    dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') GROUP BY dataTransacao UNION ALL " +
                                                            "SELECT idRepeticao, dataTransacao FROM repeticao WHERE repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') AND repeticao.idDespesa != 0 GROUP BY dataTransacao UNION ALL " +
                                                            "SELECT idParcela, dataTransacao FROM parcela WHERE parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') AND parcela.idDespesa != 0 GROUP BY dataTransacao; ");

                    if (dailyExpensesDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                        {
                            Globals.transactions.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') AND despesa.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') AND repeticao.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') AND parcela.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "');", dailyExpensesDataRow);
                        }
                    }
                    else
                    {
                        Globals.transactions.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM").Trim();
                        Globals.transactions.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                        Globals.transactions.lbl_noExpenses.Show();
                        Globals.transactions.chart_dailyExpenses.Hide();
                    }

                    if (expenseCategoryDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow categoryDataRow in expenseCategoryDataTable.Rows)
                        {
                            Globals.expenseGraphs.renderExpenseChart(Globals.expenseGraphs.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                                                                  "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                                                                  "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime'));", categoryDataRow);
                        }
                    }
                    else Globals.expenseGraphs.chart_expenseCategory.Hide();

                    if (accountDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow accountDataRow in accountDataTable.Rows)
                        {
                            Globals.expenseGraphs.renderExpenseChart(Globals.expenseGraphs.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                                                                 "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                                                                 "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime'));", accountDataRow);
                        }
                    }
                    else Globals.expenseGraphs.chart_expenseAccount.Hide();

                    this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '" + month + " month', 'localtime') " +
                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '" + month + " month', 'localtime') " +
                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '" + month + " month', 'localtime'));");

                    this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime'));");
                }
            }
            else if (month == -1)
            {
                clearChartData();
                if (incomeCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in incomeCategoryDataTable.Rows)
                    {
                        Globals.incomeGraphs.renderIncomeChart(Globals.incomeGraphs.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + Globals.idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND receita.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND receita.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', 'localtime'));", categoryDataRow);
                    }
                }
                else Globals.incomeGraphs.chart_incomeCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        Globals.incomeGraphs.renderIncomeChart(Globals.incomeGraphs.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND receita.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND receita.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', 'localtime'));", accountDataRow);
                    }
                }
                else Globals.incomeGraphs.chart_incomeAccount.Hide();

                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + Globals.idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND receita.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', 'localtime'));");

                DataTable transactionsDataTable = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE receita.idConta = " + Globals.idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                 "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                 "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                 "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', 'localtime') ORDER BY dataTransacao DESC;");
                if (transactionsDataTable.Rows.Count > 0)
                {
                    Globals.transactions.dgv_transactions.Show();
                    Globals.transactions.dgv_transactions.DataSource = transactionsDataTable;
                }
                else Globals.transactions.dgv_transactions.Hide();

                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', 'localtime') GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idRepeticao, dataTransacao FROM repeticao WHERE repeticao.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', 'localtime') AND repeticao.idDespesa != 0 GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idParcela, dataTransacao FROM parcela WHERE parcela.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', 'localtime') AND parcela.idDespesa != 0 GROUP BY dataTransacao; ");

                if (dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        Globals.transactions.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', 'localtime') AND despesa.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                                      "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', 'localtime') AND repeticao.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                                      "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', 'localtime') AND parcela.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "');", dailyExpensesDataRow);
                    }
                }
                else
                {
                    Globals.transactions.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM").Trim();
                    Globals.transactions.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    Globals.transactions.lbl_noExpenses.Show();
                    Globals.transactions.chart_dailyExpenses.Hide();
                }

                if (expenseCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in expenseCategoryDataTable.Rows)
                    {
                        Globals.expenseGraphs.renderExpenseChart(Globals.expenseGraphs.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', 'localtime'));", categoryDataRow);
                    }
                }
                else Globals.expenseGraphs.chart_expenseCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        Globals.expenseGraphs.renderExpenseChart(Globals.expenseGraphs.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                                                             "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                                                             "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', 'localtime'));", accountDataRow);
                    }
                }
                else Globals.expenseGraphs.chart_expenseAccount.Hide();

                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '-1 month', 'localtime', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '-1 month', 'localtime', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '-1 month', 'localtime', 'localtime'));");

                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', 'localtime'));");
            }
            else if (month == 0)
            {
                clearChartData();
                if (incomeCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in incomeCategoryDataTable.Rows)
                    {
                        Globals.incomeGraphs.renderIncomeChart(Globals.incomeGraphs.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + Globals.idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND receita.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime'));", categoryDataRow);
                    }
                }
                else Globals.incomeGraphs.chart_incomeCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        Globals.incomeGraphs.renderIncomeChart(Globals.incomeGraphs.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND receita.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime'));", accountDataRow);
                    }
                }
                else Globals.incomeGraphs.chart_incomeAccount.Hide();

                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + Globals.idConta + " AND receita.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime'));");

                DataTable transactionsDataTable = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE receita.idConta = " + Globals.idConta + " AND receita.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                 "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                 "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                 "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', 'localtime') ORDER BY dataTransacao DESC;");
                if (transactionsDataTable.Rows.Count > 0)
                {
                    Globals.transactions.dgv_transactions.Show();
                    Globals.transactions.dgv_transactions.DataSource = transactionsDataTable;
                }
                else Globals.transactions.dgv_transactions.Hide();

                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', 'localtime') GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idRepeticao, dataTransacao FROM repeticao WHERE repeticao.dataTransacao > datetime('start of month', '+1 month', 'localtime') AND repeticao.idDespesa != 0 GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idParcela, dataTransacao FROM parcela WHERE parcela.dataTransacao > datetime('start of month', '+1 month', 'localtime') AND parcela.idDespesa != 0 GROUP BY dataTransacao; ");

                if (dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        Globals.transactions.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', 'localtime') AND despesa.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                                      "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') AND repeticao.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                                      "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime') AND parcela.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "');", dailyExpensesDataRow);
                    }
                }
                else
                {
                    Globals.transactions.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM").Trim();
                    Globals.transactions.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    Globals.transactions.lbl_noExpenses.Show();
                    Globals.transactions.chart_dailyExpenses.Hide();
                }

                if (expenseCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in expenseCategoryDataTable.Rows)
                    {
                        Globals.expenseGraphs.renderExpenseChart(Globals.expenseGraphs.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime'));", categoryDataRow);
                    }
                }
                else Globals.expenseGraphs.chart_expenseCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        Globals.expenseGraphs.renderExpenseChart(Globals.expenseGraphs.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                                                             "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                                                             "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime'));", accountDataRow);
                    }
                }
                else Globals.expenseGraphs.chart_expenseAccount.Hide();

                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao = DATE('now', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao = DATE('start of month', '+1 month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao = DATE('start of month', '+1 month', 'localtime'));");

                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime'));");
            }
            else if (month == 1)
            {
                clearChartData();
                if (incomeCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in incomeCategoryDataTable.Rows)
                    {
                        Globals.incomeGraphs.renderIncomeChart(Globals.incomeGraphs.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + Globals.idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND receita.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime'));", categoryDataRow);
                    }
                }
                else Globals.incomeGraphs.chart_incomeCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        Globals.incomeGraphs.renderIncomeChart(Globals.incomeGraphs.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND receita.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime'));", accountDataRow);
                    }
                }
                else Globals.incomeGraphs.chart_incomeAccount.Hide();

                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + Globals.idConta + " AND receita.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime'));");

                DataTable transactionsDataTable = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE receita.idConta = " + Globals.idConta + " AND receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                 "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                 "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                 "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') ORDER BY dataTransacao DESC;");
                
                if (transactionsDataTable.Rows.Count > 0)
                {
                    Globals.transactions.dgv_transactions.Show();
                    Globals.transactions.dgv_transactions.DataSource = transactionsDataTable;
                }
                else Globals.transactions.dgv_transactions.Hide();

                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idRepeticao, dataTransacao FROM repeticao WHERE repeticao.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') AND repeticao.idDespesa != 0 GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idParcela, dataTransacao FROM parcela WHERE parcela.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') AND parcela.idDespesa != 0 GROUP BY dataTransacao; ");

                if (dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        Globals.transactions.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') AND despesa.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                                      "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') AND repeticao.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                                      "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') AND parcela.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "');", dailyExpensesDataRow);
                    }
                }
                else
                {
                    Globals.transactions.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM").Trim();
                    Globals.transactions.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    Globals.transactions.lbl_noExpenses.Show();
                    Globals.transactions.chart_dailyExpenses.Hide();
                }

                if (expenseCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in expenseCategoryDataTable.Rows)
                    {
                        Globals.expenseGraphs.renderExpenseChart(Globals.expenseGraphs.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime'));", categoryDataRow);
                    }
                }
                else Globals.expenseGraphs.chart_expenseCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        Globals.expenseGraphs.renderExpenseChart(Globals.expenseGraphs.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                                                             "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                                                             "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime'));", accountDataRow);
                    }
                }
                else Globals.expenseGraphs.chart_expenseAccount.Hide();

                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '+1 month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '+1 month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '+1 month', 'localtime'));");

                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime'));");
            }
            else if (month > 1)
            {
                clearChartData();
                int monthMore = month + 1;
                if (incomeCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in incomeCategoryDataTable.Rows)
                    {
                        Globals.incomeGraphs.renderIncomeChart(Globals.incomeGraphs.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + Globals.idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') " +
                                                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') " +
                                                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime'));", categoryDataRow);
                    }
                }
                else Globals.incomeGraphs.chart_incomeCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        Globals.incomeGraphs.renderIncomeChart(Globals.incomeGraphs.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') " +
                                                                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') " +
                                                                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime'));", accountDataRow);
                    }
                }
                else Globals.incomeGraphs.chart_incomeAccount.Hide();

                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + Globals.idConta + " AND receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') " +
                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') " +
                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime'));");


                DataTable transactionsDataTable = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE receita.idConta = " + Globals.idConta + " AND receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') " +
                                                                 "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') " +
                                                                 "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') " +
                                                                 "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') ORDER BY dataTransacao DESC;");
                
                if (transactionsDataTable.Rows.Count > 0)
                {
                    Globals.transactions.dgv_transactions.Show();
                    Globals.transactions.dgv_transactions.DataSource = transactionsDataTable;
                }
                else Globals.transactions.dgv_transactions.Hide();

                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idRepeticao, dataTransacao FROM repeticao WHERE repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') AND repeticao.idDespesa != 0 GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idParcela, dataTransacao FROM parcela WHERE parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') AND parcela.idDespesa != 0 GROUP BY dataTransacao; ");

                if (dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        Globals.transactions.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') AND despesa.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                                      "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') AND repeticao.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                                      "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') AND parcela.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "');", dailyExpensesDataRow);
                    }
                }
                else
                {
                    Globals.transactions.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM").Trim();
                    Globals.transactions.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    Globals.transactions.lbl_noExpenses.Show();
                    Globals.transactions.chart_dailyExpenses.Hide();
                }

                if (expenseCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in expenseCategoryDataTable.Rows)
                    {
                        Globals.expenseGraphs.renderExpenseChart(Globals.expenseGraphs.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') " +
                                                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') " +
                                                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime'));", categoryDataRow);
                    }
                }
                else Globals.expenseGraphs.chart_expenseCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        Globals.expenseGraphs.renderExpenseChart(Globals.expenseGraphs.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') " +
                                                                                                             "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') " +
                                                                                                             "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime'));", accountDataRow);
                    }
                }
                else Globals.expenseGraphs.chart_expenseAccount.Hide();

                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '+" + month + " month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '+" + month + " month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '+" + month + " month', 'localtime'));");

                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month', 'localtime'));");
            }
        }

        //Função que retorna à organização anterior dos componentes do formulário
        private void goBack()
        {
            Globals.step -= 1;
            if (Globals.step == 0)
            {
                this.panel_steps.Controls.Remove(Globals.transactions);
                Globals.incomeGraphs = new IncomeGraphs(this, Globals.month) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Add(Globals.incomeGraphs);
                Globals.incomeGraphs.Show();
            }
            else if (Globals.step == 1)
            {
                this.panel_steps.Controls.Remove(Globals.expenseGraphs);
                Globals.transactions = new Transactions(this, Globals.month) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Add(Globals.transactions);
                Globals.transactions.Show();
            }
        }

        //Função que avança à organização posterior dos componentes do formulário
        private void goForward()
        {
            Globals.step += 1;
            if (Globals.step == 1)
            {
                this.panel_steps.Controls.Remove(Globals.incomeGraphs);
                Globals.transactions = new Transactions(this, Globals.month) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Add(Globals.transactions);
                Globals.transactions.Show();
            }
            else if (Globals.step == 2)
            {
                this.panel_steps.Controls.Remove(Globals.transactions);
                Globals.expenseGraphs = new ExpenseGraphs(this, Globals.month) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Add(Globals.expenseGraphs);
                Globals.expenseGraphs.Show();
            }
        }

        //ADICIONAR RECEITA
        private void pcb_addIncome_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<AddIncome>().Count() == 0)
            {
                AddIncome addIncome = new AddIncome(null, this);
                addIncome.pcb_btnUpdate.Hide();
                addIncome.lbl_btnUpdateTag.Hide();
                addIncome.pcb_btnDelete.Hide();
                addIncome.lbl_btnDeleteTag.Hide();
                addIncome.pcb_incomeRegister.Location = new Point(42, 198);
                addIncome.lbl_incomeRegisterTag.Location = new Point(127, 226);
                addIncome.lbl_btnCancelTag.Text = "CANCELAR CADASTRO///";
                addIncome.Show();
                this.Close();
            }
        }

        //ADICIONAR DESPESA
        private void pcb_addExpense_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<AddExpense>().Count() == 0)
            {
                AddExpense addExpense = new AddExpense(null, this);
                addExpense.pcb_btnUpdate.Hide();
                addExpense.lbl_btnUpdateTag.Hide();
                addExpense.pcb_btnDelete.Hide();
                addExpense.lbl_btnDeleteTag.Hide();
                addExpense.pcb_expenseRegister.Location = new Point(42, 198);
                addExpense.lbl_expenseRegisterTag.Location = new Point(127, 226);
                addExpense.lbl_btnCancelTag.Text = "CANCELAR CADASTRO///";
                addExpense.Show();
                this.Close();
            }
        }

        //ENCERRAR APLICAÇÃO
        private void pcb_appClose_Click(object sender, EventArgs e)
        {
            if (((DialogResult)MessageBox.Show("Tem certeza que deseja encerrar a aplicação?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)).ToString().Trim().ToUpper() == "YES") Application.Exit();
        }

        //MINIMIZAR APLICAÇÃO
        private void pcb_minimizeProgram_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //RETORNAR PERÍODO MENSAL
        private void pcb_previousMonth_Click(object sender, EventArgs e)
        {
            Globals.month = Globals.month - 1;
            this.changeMonth(Globals.month);
        }

        //AVANÇAR PERÍODO MENSAL
        private void pcb_nextMonth_Click(object sender, EventArgs e)
        {
            Globals.month = Globals.month + 1;
            this.changeMonth(Globals.month);
        }

        //AVANÇAR AO PRÓXIMO QUADRO
        private void pcb_btnGoForward_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnGoForward.Image = Properties.Resources.btn_go_forward_active;
        }

        private void pcb_btnGoForward_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnGoForwardTag.ClientRectangle.Contains(lbl_btnGoForwardTag.PointToClient(Cursor.Position))) this.pcb_btnGoForward.Image = Properties.Resources.btn_go_forward;
        }

        private void pcb_btnGoForward_Click(object sender, EventArgs e)
        {
            this.goForward();
        }

        private void lbl_btnGoForwardTag_Click(object sender, EventArgs e)
        {
            this.goForward();
        }

        //RETORNAR AO QUADRO ANTERIOR
        private void pcb_btnGoBack_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnGoBack.Image = Properties.Resources.btn_go_back_active;
        }

        private void pcb_btnGoBack_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnGoBackTag.ClientRectangle.Contains(lbl_btnGoBackTag.PointToClient(Cursor.Position))) this.pcb_btnGoBack.Image = Properties.Resources.btn_go_back;
        }

        private void pcb_btnGoBack_Click(object sender, EventArgs e)
        {
            this.goBack();
        }

        private void lbl_btnGoBackTag_Click(object sender, EventArgs e)
        {
            this.goBack();
        }

        //SELECIONAR CONTA ATIVA
        private void cbb_activeAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isCountSelected)
            {
                Account account = new Account();
                DataTable activeAccountDataTable = Database.query("SELECT idConta FROM conta WHERE contaAtiva = 1;");
                if(activeAccountDataTable.Rows.Count > 0)
                {
                    account.IdConta = Convert.ToInt32(activeAccountDataTable.Rows[0].ItemArray[0]);
                    account.ContaAtiva = false;
                    Database.updateActiveAccount(account);
                }

                activeAccountDataTable = Database.query("SELECT idConta FROM conta WHERE nomeConta = '" + this.cbb_activeAccount.SelectedItem.ToString().Trim() + "';");
                Globals.idConta = Convert.ToInt32(activeAccountDataTable.Rows[0].ItemArray[0]);
                account.IdConta = Convert.ToInt32(activeAccountDataTable.Rows[0].ItemArray[0]);
                account.ContaAtiva = true;
                Database.updateActiveAccount(account);
                isCountSelected = true;
            }
            else
            {
                DataTable activeAccountDataTable = Database.query("SELECT idConta FROM conta WHERE contaAtiva = 1;");
                Account account = new Account();
                account.IdConta = Convert.ToInt32(activeAccountDataTable.Rows[0].ItemArray[0]);
                account.ContaAtiva = false;
                Database.updateActiveAccount(account);

                activeAccountDataTable = Database.query("SELECT idConta FROM conta WHERE nomeConta = '" + this.cbb_activeAccount.SelectedItem.ToString().Trim() + "';");
                Globals.idConta = Convert.ToInt32(activeAccountDataTable.Rows[0].ItemArray[0]);
                account.IdConta = Convert.ToInt32(activeAccountDataTable.Rows[0].ItemArray[0]);
                account.ContaAtiva = true;
                Database.updateActiveAccount(account);

                //Atualiza os dados após a seleção da conta ativa
                clearChartData();
                DataTable accountDataTable = Database.query("SELECT idConta, nomeConta FROM conta;");
                DataTable categoryDataTable = Database.query("SELECT idCategoria, nomeCategoria FROM categoria WHERE categoriaReceita != 0;");

                if (categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        Globals.incomeGraphs.renderIncomeChart(Globals.incomeGraphs.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + Globals.idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND receita.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', 'localtime'));", categoryDataRow);
                    }
                }
                else Globals.incomeGraphs.chart_incomeCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        Globals.incomeGraphs.renderIncomeChart(Globals.incomeGraphs.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND receita.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', 'localtime'));", accountDataRow);
                    }
                }
                else Globals.incomeGraphs.chart_incomeAccount.Hide();

                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + Globals.idConta + " AND receita.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', 'localtime'));");

                this.renderLabels(this.lbl_balance, "SELECT saldoConta FROM conta WHERE idConta = " + Globals.idConta);
            }
        }
    }
}
