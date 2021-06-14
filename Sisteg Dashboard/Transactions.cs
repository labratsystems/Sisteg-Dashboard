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
    public partial class Transactions : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        Color[] myPalette = new Color[6]{
            Color.FromArgb(0, 104, 232),
            Color.FromKnownColor(KnownColor.Transparent),
            Color.FromArgb(0, 104, 232),
            Color.FromKnownColor(KnownColor.Transparent),
            Color.FromArgb(0, 104, 232),
            Color.FromKnownColor(KnownColor.Transparent),
        };
        private static DateTime dateTimeMonth = DateTime.Now.Date;
        private Main transactionsMain;
        protected internal int monthTransactions = 0, idConta = 0;

        public Transactions(Main main, int month)
        {
            InitializeComponent();
            transactionsMain = main;
            monthTransactions = month;
            transactionsMain.BackgroundImage = Properties.Resources.empty_bg;
            transactionsMain.lbl_balance.Visible = false;
            transactionsMain.lbl_monthIncomes.Visible = false;
            transactionsMain.pcb_addIncome.Visible = false;
            transactionsMain.pcb_btnGoBack.Visible = true;
            transactionsMain.pcb_btnGoForward.Visible = true;
            transactionsMain.lbl_dailyExpenses.Visible = false;
            transactionsMain.lbl_monthExpenses.Visible = false;
            transactionsMain.pcb_addExpense.Visible = false;
            transactionsMain.cbb_activeAccount.Visible = false;

            idConta = transactionsMain.idConta;

            if (monthTransactions == 0)
            {
                this.lbl_noExpenses.Text = "Não há gastos no mês de " + dateTimeMonth.ToString("MMMM");
                this.dgv_transactions.DataSource = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month') " +
                                                                  "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                  "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month') " +
                                                                  "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month') ORDER BY dataTransacao DESC;");

                DataTable dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month');");
                
                if(dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        this.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month') " +
                                                      "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                      "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month'));", dailyExpensesDataRow);
                    }
                }
                else
                {
                    this.lbl_noExpenses.Text = "Não há gastos no mês de " + monthTransactions.ToString("MMMM");
                    this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    this.lbl_noExpenses.Show();
                    this.chart_dailyExpenses.Hide();
                }
            }
            else
            {
                MessageBox.Show(monthTransactions.ToString());
                this.changeMonth();
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

        private void changeMonth()
        {
            DateTime changeMonth = dateTimeMonth.AddMonths(monthTransactions);
            DataTable dailyExpensesDataTable;
            if (monthTransactions < -1)
            {
                if (monthTransactions == -2)
                {
                    clearChartData();
                    dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month');");

                    this.dgv_transactions.DataSource = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                      "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                      "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                      "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') ORDER BY dataTransacao DESC;");

                    if(dailyExpensesDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                        {
                            this.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = 1 AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month'));", dailyExpensesDataRow);
                        }
                    }
                    else
                    {
                        this.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM");
                        this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                        this.lbl_noExpenses.Show();
                        this.chart_dailyExpenses.Hide();
                    }
                }
                else
                {
                    clearChartData();
                    int monthLess = monthTransactions + 1;
                    dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '" + monthTransactions + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month');");

                    this.dgv_transactions.DataSource = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '" + monthTransactions + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                      "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + monthTransactions + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                      "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + monthTransactions + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                      "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + monthTransactions + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') ORDER BY dataTransacao DESC;");

                    if(dailyExpensesDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                        {
                            this.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + monthTransactions + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + monthTransactions + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = 1 AND parcela.dataTransacao > datetime('now', 'start of month', '" + monthTransactions + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month'));", dailyExpensesDataRow);
                        }
                    }
                    else
                    {
                        this.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM");
                        this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                        this.lbl_noExpenses.Show();
                        this.chart_dailyExpenses.Hide();
                    }
                }
            }
            else if (monthTransactions == -1)
            {
                clearChartData();
                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month');");

                this.dgv_transactions.DataSource = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month') " +
                                                                  "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                                  "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month') " +
                                                                  "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') ORDER BY dataTransacao DESC;");

                if(dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        this.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') " +
                                                      "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                      "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = 1 AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month'));", dailyExpensesDataRow);
                    }
                }
                else
                {
                    this.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM");
                    this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    this.lbl_noExpenses.Show();
                    this.chart_dailyExpenses.Hide();
                }
            }
            else if (monthTransactions == 0)
            {
                clearChartData();
                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month');");

                this.dgv_transactions.DataSource = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month') " +
                                                                  "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                  "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month') " +
                                                                  "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month') ORDER BY dataTransacao DESC;");
                
                if(dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        this.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month') " +
                                                      "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                      "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = 1 AND parcela.dataTransacao > datetime('now', 'start of month'));", dailyExpensesDataRow);
                    }
                }
                else
                {
                    this.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM");
                    this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    this.lbl_noExpenses.Show();
                    this.chart_dailyExpenses.Hide();
                }
            }
            else if (monthTransactions == 1)
            {
                clearChartData();
                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month');");

                this.dgv_transactions.DataSource = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                  "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                  "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                  "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') ORDER BY dataTransacao DESC;");

                if (dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        this.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                      "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                      "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = 1 AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month'));", dailyExpensesDataRow);
                    }
                }
                else
                {
                    this.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM");
                    this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    this.lbl_noExpenses.Show();
                    this.chart_dailyExpenses.Hide();
                }
            }
            else if (monthTransactions > 1)
            {
                clearChartData();
                int monthMore = monthTransactions + 1;
                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + monthTransactions + " month');");

                this.dgv_transactions.DataSource = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + monthTransactions + " month') " +
                                                                  "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + monthTransactions + " month') " +
                                                                  "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + monthTransactions + " month') " +
                                                                  "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + monthTransactions + " month') ORDER BY dataTransacao DESC;");

                if(dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        this.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + monthTransactions + " month') " +
                                                      "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + monthTransactions + " month') " +
                                                      "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = 1 AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + monthTransactions + " month'));", dailyExpensesDataRow);
                    }
                }
                else
                {
                    this.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM");
                    this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    this.lbl_noExpenses.Show();
                    this.chart_dailyExpenses.Hide();
                }
            }
        }

        //LIMPAR DADOS DO FORMULÁRIO
        private void clearChartData()
        {
            this.dgv_transactions.DataSource = null;
            foreach (var series in this.chart_dailyExpenses.Series) series.Points.Clear();
        }

        //FORMATAÇÃO A TABELA APÓS A DISPOSIÇÃO DOS DADOS
        private void dgv_transactions_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.dgv_transactions.Columns[1].DefaultCellStyle.Format = "C";
            foreach (DataGridViewColumn dataGridViewColumn in dgv_transactions.Columns) dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            foreach (DataGridViewRow dataGridViewRow in dgv_transactions.Rows)
            {
                string id = dataGridViewRow.Cells[0].Value.ToString();
                if (Database.query("SELECT idDespesa FROM despesa WHERE idDespesa = " + id).Rows.Count == 1)
                {
                    dataGridViewRow.DefaultCellStyle.ForeColor = Color.FromArgb(243, 104, 82);
                    dataGridViewRow.DefaultCellStyle.SelectionForeColor = Color.FromArgb(243, 104, 82);
                }
            }
            this.dgv_transactions.Columns[0].Visible = false;
            dgv_transactions.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv_transactions.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv_transactions.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        //FUNÇÃO QUE RENDERIZA ESPECIFICAMENTE O GRÁFICO DE LINHA DE DESPESAS DIÁRIAS DE ACORDO COM AS INFORMAÇÕES VINDAS DO BANCO DE DADOS
        protected internal void renderDailyExpensesChart(string query, DataRow dailyExpensesDataRow)
        {
            this.chart_dailyExpenses.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.chart_dailyExpenses.PaletteCustomColors = myPalette;

            DataTable dataTable = Database.query(query);
            if (dataTable.Rows.Count > 0)
            {
                this.BackgroundImage = Properties.Resources.income_and_expense_table_bg;
                this.lbl_noExpenses.Hide();
                this.chart_dailyExpenses.Show();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string date = dailyExpensesDataRow.ItemArray[1].ToString();
                    if ((!string.IsNullOrEmpty(dataRow.ItemArray[0].ToString())) && ((Convert.ToInt32(dataRow.ItemArray[0])) != 0))
                    {
                        DateTime dateTime = Convert.ToDateTime(date);
                        string day = dateTime.Day.ToString();
                        string month = dateTime.ToString("MMMM");
                        string sum = String.Format("{0:C}", dataRow.ItemArray[0]);
                        this.chart_dailyExpenses.Titles[0].Text = "Gastos do mês de " + month;
                        this.chart_dailyExpenses.Series[0].Points.AddXY(day, sum);
                    }
                    else
                    {
                        this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                        this.lbl_noExpenses.Show();
                        this.chart_dailyExpenses.Hide();
                    }
                }
            }
            else
            {
                this.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                this.lbl_noExpenses.Show();
                this.chart_dailyExpenses.Hide();
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
                    string id = dataGridViewRow.Cells[0].Value.ToString();
                    if (Database.query("SELECT idDespesa FROM despesa WHERE idDespesa = " + id).Rows.Count == 1) this.openExpenseForm("SELECT * FROM despesa WHERE idDespesa = " + id + ";");
                    else if (Database.query("SELECT idReceita FROM receita WHERE idReceita = " + id).Rows.Count == 1) this.openIncomeForm("SELECT * FROM receita WHERE idReceita = " + id + ";");
                    else if (Database.query("SELECT idRepeticao FROM repeticao WHERE idrepeticao = " + id + ";").Rows.Count == 1)
                    {
                        //Repetição
                        if (Database.query("SELECT idDespesa FROM repeticao WHERE idRepeticao = " + id + ";").Rows[0].ItemArray[0] != System.DBNull.Value)
                        {
                            id = Database.query("SELECT idDespesa FROM repeticao WHERE idRepeticao = " + id + ";").Rows[0].ItemArray[0].ToString();
                            this.openExpenseForm("SELECT * FROM despesa WHERE idDespesa = " + id + ";");
                        }
                        else if (Database.query("SELECT idReceita FROM repeticao WHERE idRepeticao = " + id + ";").Rows[0].ItemArray[0] != System.DBNull.Value)
                        {
                            id = Database.query("SELECT idReceita FROM repeticao WHERE idRepeticao = " + id + ";").Rows[0].ItemArray[0].ToString();
                            this.openIncomeForm("SELECT * FROM receita WHERE idReceita = " + id + ";");
                        }
                    }
                    else if (Database.query("SELECT idParcela FROM parcela WHERE idParcela = " + id + ";").Rows.Count == 1)
                    {
                        //Parcela
                        if (Database.query("SELECT idDespesa FROM parcela WHERE idParcela = " + id + ";").Rows[0].ItemArray[0] != System.DBNull.Value)
                        {
                            id = Database.query("SELECT idDespesa FROM parcela WHERE idParcela = " + id + ";").Rows[0].ItemArray[0].ToString();
                            this.openExpenseForm("SELECT * FROM despesa WHERE idDespesa = " + id + ";");
                        }
                        else if (Database.query("SELECT idReceita FROM parcela WHERE idParcela = " + id + ";").Rows[0].ItemArray[0] != System.DBNull.Value)
                        {
                            id = Database.query("SELECT idReceita FROM parcela WHERE idParcela = " + id + ";").Rows[0].ItemArray[0].ToString();
                            this.openIncomeForm("SELECT * FROM receita WHERE idReceita = " + id + ";");
                        }
                    }
                }
            }
        }

        private void openExpenseForm(string query)
        {
            if (Application.OpenForms.OfType<AddExpense>().Count() == 0)
            {
                DataTable dataTable = Database.query(query);
                AddExpense addExpense = new AddExpense(dataTable);
                addExpense.Show();
                transactionsMain.Close();
            }
        }

        private void openIncomeForm(string query)
        {
            if (Application.OpenForms.OfType<AddIncome>().Count() == 0)
            {
                DataTable dataTable = Database.query(query);
                AddIncome addIncome = new AddIncome(dataTable);
                addIncome.Show();
                transactionsMain.Close();
            }
        }
    }
}
