using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class Transactions : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        private static DateTime dateTimeMonth = DateTime.Now.Date;
        private static bool thereIsExpense = false;

        //INICIA INSTÂNCIA DO PAINEL, POPULANDO A TABELA DE RECEITAS E DESPESAS E O GRÁFICO DE GASTOS DIÁRIOS COM AS INFORMAÇÕES DO BANCO DE DADOS DE ACORDO COM O PERÍODO MENSAL
        public Transactions(Main main, int month)
        {
            InitializeComponent();
            Globals.main = main;
            Globals.month = month;
            Globals.main.BackgroundImage = Properties.Resources.empty_bg;
            Globals.main.lbl_balance.Visible = false;
            Globals.main.lbl_balanceTag.Visible = false;
            Globals.main.lbl_monthIncomes.Visible = false;
            Globals.main.lbl_monthIncomesTag.Visible = false;
            Globals.main.pcb_addIncome.Visible = false;
            Globals.main.pcb_btnGoBack.Visible = true;
            Globals.main.lbl_btnGoBackTag.Visible = true;
            Globals.main.pcb_btnGoForward.Visible = true;
            Globals.main.lbl_btnGoForwardTag.Visible = true;
            Globals.main.lbl_dailyExpenses.Visible = false;
            Globals.main.lbl_dailyExpensesTag.Visible = false;
            Globals.main.lbl_monthExpenses.Visible = false;
            Globals.main.lbl_monthExpensesTag.Visible = false;
            Globals.main.pcb_addExpense.Visible = false;
            Globals.main.cbb_activeAccount.Visible = false;

            if (Globals.month == 0)
            {
                DataTable transactionsDataTable = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE receita.idConta = " + Globals.idConta + " AND receita.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                 "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                 "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                 "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', 'localtime') ORDER BY dataTransacao DESC;");
                if (transactionsDataTable.Rows.Count > 0)
                {
                    this.dgv_transactions.Show();
                    this.dgv_transactions.DataSource = transactionsDataTable;
                }
                else this.dgv_transactions.Hide();

                DataTable dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', 'localtime') GROUP BY dataTransacao UNION ALL " +
                                                                  "SELECT idRepeticao, dataTransacao FROM repeticao WHERE repeticao.dataTransacao > datetime('start of month', '+1 month', 'localtime') AND repeticao.idDespesa != 0 GROUP BY dataTransacao UNION ALL " +
                                                                  "SELECT idParcela, dataTransacao FROM parcela WHERE parcela.dataTransacao > datetime('start of month', '+1 month', 'localtime') AND parcela.idDespesa != 0 GROUP BY dataTransacao; ");

                if (dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        this.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', 'localtime') AND despesa.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                      "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') AND repeticao.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                      "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime') AND parcela.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "');", dailyExpensesDataRow);
                    }
                }
                else
                {
                    this.lbl_noExpenses.Text = "Não há gastos no mês de " + dateTimeMonth.ToString("MMMM").Trim();
                    this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    this.lbl_noExpenses.Show();
                    this.chart_dailyExpenses.Hide();
                }
            }
            else this.changeMonth();
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
            this.dgv_transactions.DataSource = null;
            foreach (var series in this.chart_dailyExpenses.Series) series.Points.Clear();
        }

        //Função que renderiza especificamente o gráfico de linha de despesas diárias de acordo com as informações vindas do banco de dados
        protected internal void renderDailyExpensesChart(string query, DataRow dailyExpensesDataRow)
        {
            this.chart_dailyExpenses.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.chart_dailyExpenses.PaletteCustomColors = Globals.myPalette;

            DataTable dataTable = Database.query(query);
            if (dataTable.Rows.Count > 0)
            {
                this.BackgroundImage = Properties.Resources.income_and_expense_table_bg;
                this.lbl_noExpenses.Hide();
                this.chart_dailyExpenses.Show();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string date = dailyExpensesDataRow.ItemArray[1].ToString().Trim();
                    if ((!string.IsNullOrEmpty(dataRow.ItemArray[0].ToString().Trim())) && ((Convert.ToInt32(dataRow.ItemArray[0])) != 0))
                    {
                        DateTime dateTime = Convert.ToDateTime(date);
                        string day = dateTime.Day.ToString().Trim();
                        string month = dateTime.ToString("MMMM").Trim();
                        string sum = String.Format("{0:C}", dataRow.ItemArray[0]);
                        this.chart_dailyExpenses.Titles[0].Text = "Gastos do mês de " + month;
                        this.chart_dailyExpenses.Series[0].Points.AddXY(day, sum);
                        thereIsExpense = true;
                    }
                    else if (!thereIsExpense)
                    {
                        this.lbl_noExpenses.Text = "Não há gastos no mês de " + dateTimeMonth.ToString("MMMM").Trim();
                        this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                        this.lbl_noExpenses.Show();
                        this.chart_dailyExpenses.Hide();
                    }
                }
            }
            else if (!thereIsExpense)
            {
                this.lbl_noExpenses.Text = "Não há gastos no mês de " + dateTimeMonth.ToString("MMMM").Trim();
                this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                this.lbl_noExpenses.Show();
                this.chart_dailyExpenses.Hide();
            }
        }

        //Função que muda período mensal para atualização dos gráficos, tabelas e inscrições no formulário
        private void changeMonth()
        {
            DateTime changeMonth = dateTimeMonth.AddMonths(Globals.month);
            DataTable dailyExpensesDataTable;
            if (Globals.month < -1)
            {
                if (Globals.month == -2)
                {
                    clearChartData();
                    dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') GROUP BY dataTransacao UNION ALL " +
                                                            "SELECT idRepeticao, dataTransacao FROM repeticao WHERE repeticao.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.idDespesa != 0 GROUP BY dataTransacao UNION ALL " +
                                                            "SELECT idParcela, dataTransacao FROM parcela WHERE parcela.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.idDespesa != 0 GROUP BY dataTransacao; ");

                    DataTable transactionsDataTable = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE receita.idConta = " + Globals.idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                     "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                     "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') " +
                                                                     "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') ORDER BY dataTransacao DESC;");
                    if (transactionsDataTable.Rows.Count > 0)
                    {
                        this.dgv_transactions.Show();
                        this.dgv_transactions.DataSource = transactionsDataTable;
                    }
                    else this.dgv_transactions.Hide();

                    if (dailyExpensesDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                        {
                            this.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') AND despesa.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "');", dailyExpensesDataRow);
                        }
                    }
                    else
                    {
                        this.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM").Trim();
                        this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                        this.lbl_noExpenses.Show();
                        this.chart_dailyExpenses.Hide();
                    }
                }
                else
                {
                    clearChartData();
                    int monthLess = Globals.month + 1;
                    dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') UNION ALL " +
                                                            "SELECT idRepeticao, dataTransacao FROM repeticao WHERE repeticao.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') AND repeticao.idDespesa != 0 UNION ALL " +
                                                            "SELECT idParcela, dataTransacao FROM parcela WHERE parcela.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') AND parcela.idDespesa != 0; ");

                    DataTable transactionsDataTable = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE receita.idConta = " + Globals.idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                     "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                     "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') " +
                                                                     "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') ORDER BY dataTransacao DESC;");
                    if (transactionsDataTable.Rows.Count > 0)
                    {
                        this.dgv_transactions.Show();
                        this.dgv_transactions.DataSource = transactionsDataTable;
                    }
                    else this.dgv_transactions.Hide();

                    if (dailyExpensesDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                        {
                            this.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') AND despesa.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') AND repeticao.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + Globals.month + " month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month', 'localtime') AND parcela.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "');", dailyExpensesDataRow);
                        }
                    }
                    else
                    {
                        this.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM").Trim();
                        this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                        this.lbl_noExpenses.Show();
                        this.chart_dailyExpenses.Hide();
                    }
                }
            }
            else if (Globals.month == -1)
            {
                clearChartData();
                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', 'localtime') GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idRepeticao, dataTransacao FROM repeticao WHERE repeticao.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', 'localtime') AND repeticao.idDespesa != 0 GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idParcela, dataTransacao FROM parcela WHERE parcela.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', 'localtime') AND parcela.idDespesa != 0 GROUP BY dataTransacao; ");

                DataTable transactionsDataTable = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE receita.idConta = " + Globals.idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND receita.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                 "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                 "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', 'localtime') " +
                                                                 "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', 'localtime') ORDER BY dataTransacao DESC;");
                if (transactionsDataTable.Rows.Count > 0)
                {
                    this.dgv_transactions.Show();
                    this.dgv_transactions.DataSource = transactionsDataTable;
                }
                else this.dgv_transactions.Hide();

                if (dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        this.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND despesa.dataTransacao < datetime('now', 'start of month', 'localtime') AND despesa.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                      "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND repeticao.dataTransacao < datetime('now', 'start of month', 'localtime') AND repeticao.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                      "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month', 'localtime') AND parcela.dataTransacao < datetime('now', 'start of month', 'localtime') AND parcela.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "');", dailyExpensesDataRow);
                    }
                }
                else
                {
                    this.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM").Trim();
                    this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    this.lbl_noExpenses.Show();
                    this.chart_dailyExpenses.Hide();
                }
            }
            else if (Globals.month == 0)
            {
                clearChartData();
                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', 'localtime') GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idRepeticao, dataTransacao FROM repeticao WHERE repeticao.dataTransacao > datetime('start month', '1+ month', 'localtime') AND repeticao.idDespesa != 0 GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idParcela, dataTransacao FROM parcela WHERE parcela.dataTransacao > datetime('start of month', '1+ month', 'localtime') AND parcela.idDespesa != 0 GROUP BY dataTransacao; ");

                DataTable transactionsDataTable = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE receita.idConta = " + Globals.idConta + " AND receita.dataTransacao > datetime('now', 'start of month', 'localtime') " +
                                                                 "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                 "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime') " +
                                                                 "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', 'localtime') ORDER BY dataTransacao DESC;");
                if (transactionsDataTable.Rows.Count > 0)
                {
                    this.dgv_transactions.Show();
                    this.dgv_transactions.DataSource = transactionsDataTable;
                }
                else this.dgv_transactions.Hide();

                if (dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        this.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', 'localtime') AND despesa.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                      "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('start of month', '+1 month', 'localtime') AND repeticao.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                      "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('start of month', '+1 month', 'localtime') AND parcela.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "');", dailyExpensesDataRow);
                    }
                }
                else
                {
                    this.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM").Trim();
                    this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    this.lbl_noExpenses.Show();
                    this.chart_dailyExpenses.Hide();
                }
            }
            else if (Globals.month == 1)
            {
                clearChartData();
                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idRepeticao, dataTransacao FROM repeticao WHERE repeticao.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') AND repeticao.idDespesa != 0 GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idParcela, dataTransacao FROM parcela WHERE parcela.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') AND parcela.idDespesa != 0 GROUP BY dataTransacao; ");

                DataTable transactionsDataTable = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE receita.idConta = " + Globals.idConta + " AND receita.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                 "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                 "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') " +
                                                                 "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') ORDER BY dataTransacao DESC;");
                if (transactionsDataTable.Rows.Count > 0)
                {
                    this.dgv_transactions.Show();
                    this.dgv_transactions.DataSource = transactionsDataTable;
                }
                else this.dgv_transactions.Hide();

                if (dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        this.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') AND despesa.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                      "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') AND repeticao.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                      "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month', 'localtime') AND parcela.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "');", dailyExpensesDataRow);
                    }
                }
                else
                {
                    this.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM").Trim();
                    this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    this.lbl_noExpenses.Show();
                    this.chart_dailyExpenses.Hide();
                }
            }
            else if (Globals.month > 1)
            {
                clearChartData();
                int monthMore = Globals.month + 1;
                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idRepeticao, dataTransacao FROM repeticao WHERE repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') AND repeticao.idDespesa != 0 GROUP BY dataTransacao UNION ALL " +
                                                        "SELECT idParcela, dataTransacao FROM parcela WHERE parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') AND parcela.idDespesa != 0 GROUP BY dataTransacao; ");

                DataTable transactionsDataTable = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE receita.idConta = " + Globals.idConta + " AND receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND receita.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') " +
                                                                 "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') " +
                                                                 "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') " +
                                                                 "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') ORDER BY dataTransacao DESC;");
                if (transactionsDataTable.Rows.Count > 0)
                {
                    this.dgv_transactions.Show();
                    this.dgv_transactions.DataSource = transactionsDataTable;
                }
                else this.dgv_transactions.Hide();

                if (dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        this.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') AND despesa.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                      "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') AND repeticao.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "' " +
                                                      "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = " + Globals.idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month', 'localtime') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + Globals.month + " month', 'localtime') AND parcela.dataTransacao LIKE '%" + Convert.ToDateTime(dailyExpensesDataRow.ItemArray[1]).Day + "');", dailyExpensesDataRow);
                    }
                }
                else
                {
                    this.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM").Trim();
                    this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    this.lbl_noExpenses.Show();
                    this.chart_dailyExpenses.Hide();
                }
            }
        }

        //Função que abre o formulário de edição de receita
        private void openIncomeForm(string query)
        {
            if (Application.OpenForms.OfType<AddIncome>().Count() == 0)
            {
                DataTable dataTable = Database.query(query);
                AddIncome addIncome = new AddIncome(dataTable, Globals.main);
                addIncome.lbl_btnCancelTag.Text = "CANCELAR EDIÇÃO///";
                addIncome.Show();
                Globals.main.Close();
            }
        }

        //Função que abre o formulário de edição de despesa
        private void openExpenseForm(string query)
        {
            if (Application.OpenForms.OfType<AddExpense>().Count() == 0)
            {
                DataTable dataTable = Database.query(query);
                AddExpense addExpense = new AddExpense(dataTable, Globals.main);
                addExpense.lbl_btnCancelTag.Text = "CANCELAR EDIÇÃO///";
                addExpense.Show();
                Globals.main.Close();
            }
        }

        //ATUALIZAR RECEITA OU DESPESA
        private void pcb_updateIncomeOrExpense_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(this.dgv_transactions.Rows.Count) == 0) MessageBox.Show("Não há receita ou despesa selecionada para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Convert.ToInt32(this.dgv_transactions.SelectedRows.Count) > 1) MessageBox.Show("Selecione apenas uma linha da tabela para editar!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                foreach (DataGridViewRow dataGridViewRow in this.dgv_transactions.SelectedRows)
                {
                    int id = Convert.ToInt32(dataGridViewRow.Cells[0].Value);
                    if (Database.query("SELECT idDespesa FROM despesa WHERE idDespesa = " + id).Rows.Count == 1) this.openExpenseForm("SELECT * FROM despesa WHERE idDespesa = " + id);
                    else if (Database.query("SELECT idReceita FROM receita WHERE idReceita = " + id).Rows.Count == 1)
                    {
                        if(Database.query("SELECT * FROM receita WHERE numeroOrcamento != 0 AND idReceita = " + id).Rows.Count > 0) MessageBox.Show("Não é possível editar as receitas vinculadas à um orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else this.openIncomeForm("SELECT * FROM receita WHERE idReceita = " + id);
                    }
                    else if (Database.query("SELECT idRepeticao FROM repeticao WHERE idrepeticao = " + id).Rows.Count == 1)
                    {
                        //Repetição
                        if ((Database.query("SELECT idDespesa FROM repeticao WHERE idRepeticao = " + id).Rows[0].ItemArray[0] != System.DBNull.Value) && (Convert.ToInt32(Database.query("SELECT idDespesa FROM repeticao WHERE idRepeticao = " + id).Rows[0].ItemArray[0]) != 0))
                        {
                            id = Convert.ToInt32(Database.query("SELECT idDespesa FROM repeticao WHERE idRepeticao = " + id).Rows[0].ItemArray[0]);
                            this.openExpenseForm("SELECT * FROM despesa WHERE idDespesa = " + id);
                        }
                        else if ((Database.query("SELECT idReceita FROM repeticao WHERE idRepeticao = " + id).Rows[0].ItemArray[0] != System.DBNull.Value) && (Convert.ToInt32(Database.query("SELECT idReceita FROM repeticao WHERE idRepeticao = " + id).Rows[0].ItemArray[0]) != 0))
                        {
                            id = Convert.ToInt32(Database.query("SELECT idReceita FROM repeticao WHERE idRepeticao = " + id).Rows[0].ItemArray[0]);
                            this.openIncomeForm("SELECT * FROM receita WHERE idReceita = " + id);
                        }
                    }
                    else if (Database.query("SELECT idParcela FROM parcela WHERE idParcela = " + id).Rows.Count == 1)
                    {
                        //Parcela
                        if ((Database.query("SELECT idDespesa FROM parcela WHERE idParcela = " + id).Rows[0].ItemArray[0] != System.DBNull.Value) && (Convert.ToInt32(Database.query("SELECT idDespesa FROM parcela WHERE idParcela = " + id).Rows[0].ItemArray[0]) != 0))
                        {
                            id = Convert.ToInt32(Database.query("SELECT idDespesa FROM parcela WHERE idParcela = " + id).Rows[0].ItemArray[0]);
                            this.openExpenseForm("SELECT * FROM despesa WHERE idDespesa = " + id);
                        }
                        else if ((Database.query("SELECT idReceita FROM parcela WHERE idParcela = " + id).Rows[0].ItemArray[0] != System.DBNull.Value) && (Convert.ToInt32(Database.query("SELECT idReceita FROM parcela WHERE idParcela = " + id).Rows[0].ItemArray[0]) != 0))
                        {
                            if (Database.query("SELECT * FROM parcela WHERE numeroOrcamento != 0 AND idParcela = " + id).Rows.Count > 0) MessageBox.Show("Não é possível editar as parcelas vinculadas à um orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            else
                            {
                                id = Convert.ToInt32(Database.query("SELECT idReceita FROM parcela WHERE idParcela = " + id).Rows[0].ItemArray[0]);
                                this.openIncomeForm("SELECT * FROM receita WHERE idReceita = " + id);
                            }
                        }
                    }
                }
            }
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO

        //Formatação da tabela após disposição dos dados
        private void dgv_transactions_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.dgv_transactions.Columns[1].DefaultCellStyle.Format = "C";
            foreach (DataGridViewColumn dataGridViewColumn in dgv_transactions.Columns) dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            foreach (DataGridViewRow dataGridViewRow in dgv_transactions.Rows)
            {
                string id = dataGridViewRow.Cells[0].Value.ToString().Trim();
                if (Database.query("SELECT idDespesa FROM despesa WHERE idDespesa = " + id).Rows.Count == 1)
                {
                    dataGridViewRow.DefaultCellStyle.ForeColor = Color.FromArgb(243, 104, 82);
                    dataGridViewRow.DefaultCellStyle.SelectionForeColor = Color.FromArgb(243, 104, 82);
                }
                else if (Database.query("SELECT idRepeticao FROM repeticao WHERE idRepeticao = " + id).Rows.Count == 1)
                {
                    if (Convert.ToInt32(Database.query("SELECT idDespesa FROM repeticao WHERE idRepeticao = " + id).Rows[0].ItemArray[0]) != 0)
                    {
                        dataGridViewRow.DefaultCellStyle.ForeColor = Color.FromArgb(243, 104, 82);
                        dataGridViewRow.DefaultCellStyle.SelectionForeColor = Color.FromArgb(243, 104, 82);
                    }
                }
                else if (Database.query("SELECT idParcela FROM parcela WHERE idParcela = " + id).Rows.Count == 1)
                {
                    if (Convert.ToInt32(Database.query("SELECT idDespesa FROM parcela WHERE idParcela = " + id).Rows[0].ItemArray[0]) != 0)
                    {
                        dataGridViewRow.DefaultCellStyle.ForeColor = Color.FromArgb(243, 104, 82);
                        dataGridViewRow.DefaultCellStyle.SelectionForeColor = Color.FromArgb(243, 104, 82);
                    }
                }
            }
            this.dgv_transactions.Columns[0].Visible = false;
            dgv_transactions.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv_transactions.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv_transactions.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }
}
