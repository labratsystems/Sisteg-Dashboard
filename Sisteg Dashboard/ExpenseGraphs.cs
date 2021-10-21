using System;
using System.Data;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class ExpenseGraphs : Form
    {
        public ExpenseGraphs(Main main, int month)
        {
            InitializeComponent();
            Globals.main = main;
            Globals.month = month;
            Globals.main.BackgroundImage = Properties.Resources.expense_homepage_bg;
            Globals.main.lbl_balance.Visible = false;
            Globals.main.lbl_monthIncomes.Visible = false;
            Globals.main.pcb_addIncome.Visible = false;
            Globals.main.pcb_btnGoBack.Visible = true;
            Globals.main.lbl_btnGoBackTag.Visible = true;
            Globals.main.pcb_btnGoForward.Visible = false;
            Globals.main.lbl_btnGoForwardTag.Visible = false;
            Globals.main.lbl_dailyExpenses.Visible = true;
            Globals.main.lbl_dailyExpensesTag.Visible = true;
            Globals.main.lbl_monthExpenses.Visible = true;
            Globals.main.lbl_monthExpensesTag.Visible = true;
            Globals.main.pcb_addExpense.Visible = true;
            Globals.main.cbb_activeAccount.Visible = false;

            if (Globals.month == 0)
            {
                DataTable accountDataTable = Database.query("SELECT idConta, nomeConta FROM conta;");
                DataTable categoryDataTable = Database.query("SELECT idCategoria, nomeCategoria FROM categoria WHERE categoriaDespesa != 0;");

                if(categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                            "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                            "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime'));", categoryDataRow);
                    }
                }
                else this.chart_expenseCategory.Hide();

                if(accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                           "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                           "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime'));", accountDataRow);
                    }
                }
                else this.chart_expenseAccount.Hide();

                Globals.main.renderLabels(Globals.main.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao = DATE('now', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao = DATE('start of month', '+1 month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao = DATE('start of month', '+1 month', 'localtime'));");

                Globals.main.renderLabels(Globals.main.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime'));");
            }
            else changeMonth();
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

        //Função que limpa os dados do formulário
        private void clearChartData()
        {
            foreach (var series in this.chart_expenseAccount.Series) series.Points.Clear();
            foreach (var series in this.chart_expenseCategory.Series) series.Points.Clear();
        }

        //Função que renderiza os gráficos de pizza de acordo com as informações vindas do banco de dados
        protected internal void renderExpenseChart(System.Windows.Forms.DataVisualization.Charting.Chart chart, string query, DataRow accountDataRow)
        {
            chart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            chart.PaletteCustomColors = Globals.myPalette;
            DataTable dataTable = Database.query(query);
            if (dataTable.Rows.Count > 0)
            {
                chart.Show();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string category = accountDataRow.ItemArray[1].ToString().Trim();
                    if ((!string.IsNullOrEmpty(dataRow.ItemArray[0].ToString().Trim())) && ((Convert.ToInt32(dataRow.ItemArray[0])) != 0))
                    {
                        string graph = dataRow.ItemArray[0].ToString().Trim();
                        string sum = String.Format("{0:C}", dataRow.ItemArray[0]);
                        string label = category + "\n" + sum;
                        chart.Series[0].Points.AddXY(label, graph);
                    }
                }
            }
            else chart.Hide();
        }

        //Função que muda o período mensal para atualização dos gráficos, tabelas e inscrições no formulário
        private void changeMonth()
        {
            DataTable accountDataTable = Database.query("SELECT idConta, nomeConta FROM conta;");
            DataTable categoryDataTable = Database.query("SELECT idCategoria, nomeCategoria FROM categoria WHERE categoriaDespesa != 0;");
            if (Globals.month < -1)
            {
                if (Globals.month == -2)
                {
                    clearChartData();
                    if(categoryDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                        {
                            this.renderExpenseChart(this.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                                "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                                "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime'));", categoryDataRow);
                        }
                    }
                    else this.chart_expenseCategory.Hide();

                    if(accountDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow accountDataRow in accountDataTable.Rows)
                        {
                            this.renderExpenseChart(this.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                               "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                               "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime'));", accountDataRow);
                        }
                    }
                    else this.chart_expenseAccount.Hide();

                    Globals.main.renderLabels(Globals.main.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '-2 month', 'localtime') " +
                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '-2 month', 'localtime') " +
                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '-2 month', 'localtime'));");

                    Globals.main.renderLabels(Globals.main.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime'));");
                }
                else
                {
                    clearChartData();
                    int monthLess = Globals.month + 1;
                    if(categoryDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                        {
                            this.renderExpenseChart(this.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                                "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                                "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime'));", categoryDataRow);
                        }
                    }
                    else this.chart_expenseCategory.Hide();

                    if(accountDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow accountDataRow in accountDataTable.Rows)
                        {
                            this.renderExpenseChart(this.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                               "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                               "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime'));", accountDataRow);
                        }
                    }
                    else this.chart_expenseAccount.Hide();

                    Globals.main.renderLabels(Globals.main.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '" + Globals.month + " month', 'localtime') " +
                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '" + Globals.month + " month', 'localtime') " +
                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '" + Globals.month + " month', 'localtime'));");

                    Globals.main.renderLabels(Globals.main.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime'));");
                }
            }
            else if (Globals.month == -1)
            {
                clearChartData();
                if(categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                            "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                            "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', 'localtime'));", categoryDataRow);
                    }
                }
                else this.chart_expenseCategory.Hide();

                if(accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                           "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                           "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', 'localtime'));", accountDataRow);
                    }
                }
                else this.chart_expenseAccount.Hide();

                Globals.main.renderLabels(Globals.main.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '-1 month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '-1 month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '-1 month', 'localtime'));");

                Globals.main.renderLabels(Globals.main.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', 'localtime'));");
            }
            else if (Globals.month == 0)
            {
                clearChartData();
                if(categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                            "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                            "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime'));", categoryDataRow);
                    }
                }
                else this.chart_expenseCategory.Hide();

                if(accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                           "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                           "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime'));", accountDataRow);
                    }
                }
                else this.chart_expenseAccount.Hide();

                Globals.main.renderLabels(Globals.main.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao = DATE('now', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao = DATE('start of month', '+1 month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao = DATE('start of month', '+1 month', 'localtime'));");

                Globals.main.renderLabels(Globals.main.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime'));");
            }
            else if (Globals.month == 1)
            {
                clearChartData();
                if(categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                            "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                            "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime'));", categoryDataRow);
                    }
                }
                else this.chart_expenseCategory.Hide();

                if(accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                           "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                           "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime'));", accountDataRow);
                    }
                }
                else this.chart_expenseAccount.Hide();

                Globals.main.renderLabels(Globals.main.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '+1 month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '+1 month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '+1 month', 'localtime'));");

                Globals.main.renderLabels(Globals.main.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime'));");
            }
            else if (Globals.month > 1)
            {
                clearChartData();
                int monthMore = Globals.month + 1;
                if(categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') " +
                                                                            "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') " +
                                                                            "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime'));", categoryDataRow);
                    }
                }
                else this.chart_expenseCategory.Hide();

                if(accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') " +
                                                                           "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') " +
                                                                           "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString().Trim() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime'));", accountDataRow);
                    }
                }
                else this.chart_expenseAccount.Hide();

                Globals.main.renderLabels(Globals.main.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '+" + Globals.month + " month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '+" + Globals.month + " month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '+" + Globals.month + " month', 'localtime'));");

                Globals.main.renderLabels(Globals.main.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime'));");
            }
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO

        //Tooltip do gráfico de despesas da conta ativa por categoria
        private void chart_expenseCategory_MouseHover(object sender, EventArgs e)
        {
            this.ttp_expenseCategory.Show("Despesas da conta ativa por categoria.", this.chart_expenseCategory);
        }

        //Tooltip do gráfico de despesas por conta
        private void chart_expenseAccount_MouseHover(object sender, EventArgs e)
        {
            this.ttp_expenseAccount.Show("Despesas por conta.", this.chart_expenseAccount);
        }
    }
}
