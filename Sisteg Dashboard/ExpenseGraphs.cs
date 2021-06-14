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
    public partial class ExpenseGraphs : Form
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
        private Main expenseGraphsMain;
        private static DateTime dateTimeMonth = DateTime.Now.Date;
        protected internal int expenseGraphsMonth = 0, idConta = 0;
        protected internal string nomeConta;

        public ExpenseGraphs(Main main, int month)
        {
            InitializeComponent();
            expenseGraphsMain = main;
            expenseGraphsMonth = month;
            expenseGraphsMain.BackgroundImage = Properties.Resources.expense_homepage_bg;
            expenseGraphsMain.lbl_balance.Visible = false;
            expenseGraphsMain.lbl_monthIncomes.Visible = false;
            expenseGraphsMain.pcb_addIncome.Visible = false;
            expenseGraphsMain.pcb_btnGoBack.Visible = true;
            expenseGraphsMain.pcb_btnGoForward.Visible = false;
            expenseGraphsMain.lbl_dailyExpenses.Visible = true;
            expenseGraphsMain.lbl_monthExpenses.Visible = true;
            expenseGraphsMain.pcb_addExpense.Visible = true;
            expenseGraphsMain.cbb_activeAccount.Visible = false;

            idConta = expenseGraphsMain.idConta;

            if (expenseGraphsMonth == 0)
            {
                DataTable accountDataTable = Database.query("SELECT idConta, nomeConta FROM conta;");
                DataTable categoryDataTable = Database.query("SELECT idCategoria, nomeCategoria FROM categoria WHERE categoriaDespesa != 0;");

                if(categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month') " +
                                                                               "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                               "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month'));", categoryDataRow);
                    }
                }
                else this.chart_expenseCategory.Hide();

                if(accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month') " +
                                                                                 "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                                 "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month'));", accountDataRow);
                    }
                }
                else this.chart_expenseAccount.Hide();

                expenseGraphsMain.renderLabels(expenseGraphsMain.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao = DATE('now', 'localtime') " +
                                                                                    "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao = DATE('now', 'localtime') " +
                                                                                    "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao = DATE('now', 'localtime'));");

                expenseGraphsMain.renderLabels(expenseGraphsMain.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month') " +
                                                                                    "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                                    "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month'));");
            }
            else
            {
                MessageBox.Show(expenseGraphsMonth.ToString());
                changeMonth();
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
            DataTable accountDataTable = Database.query("SELECT idConta, nomeConta FROM conta;");
            DataTable categoryDataTable = Database.query("SELECT idCategoria, nomeCategoria FROM categoria WHERE categoriaDespesa != 0;");
            if (expenseGraphsMonth < -1)
            {
                if (expenseGraphsMonth == -2)
                {
                    clearChartData();
                    if(categoryDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                        {
                            this.renderExpenseChart(this.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                   "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                   "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month'));", categoryDataRow);
                        }
                    }
                    else this.chart_expenseCategory.Hide();

                    if(accountDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow accountDataRow in accountDataTable.Rows)
                        {
                            this.renderExpenseChart(this.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                     "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                     "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month'));", accountDataRow);
                        }
                    }
                    else this.chart_expenseAccount.Hide();

                    expenseGraphsMain.renderLabels(expenseGraphsMain.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '-2 month', 'localtime') " +
                                                                                        "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '-2 month', 'localtime') " +
                                                                                        "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '-2 month', 'localtime'));");

                    expenseGraphsMain.renderLabels(expenseGraphsMain.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                        "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                        "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month'));");
                }
                else
                {
                    clearChartData();
                    int monthLess = expenseGraphsMonth + 1;
                    if(categoryDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                        {
                            this.renderExpenseChart(this.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + expenseGraphsMonth + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                   "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + expenseGraphsMonth + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                   "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + expenseGraphsMonth + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month'));", categoryDataRow);
                        }
                    }
                    else this.chart_expenseCategory.Hide();

                    if(accountDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow accountDataRow in accountDataTable.Rows)
                        {
                            this.renderExpenseChart(this.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + expenseGraphsMonth + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                     "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + expenseGraphsMonth + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                     "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + expenseGraphsMonth + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month'));", accountDataRow);
                        }
                    }
                    else this.chart_expenseAccount.Hide();

                    expenseGraphsMain.renderLabels(expenseGraphsMain.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '" + expenseGraphsMonth + " month', 'localtime') " +
                                                                                        "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '" + expenseGraphsMonth + " month', 'localtime') " +
                                                                                        "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '" + expenseGraphsMonth + " month', 'localtime'));");

                    expenseGraphsMain.renderLabels(expenseGraphsMain.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + expenseGraphsMonth + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                        "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + expenseGraphsMonth + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                        "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + expenseGraphsMonth + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month'));");
                }
            }
            else if (expenseGraphsMonth == -1)
            {
                clearChartData();
                if(categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') " +
                                                                               "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                                               "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month'));", categoryDataRow);
                    }
                }
                else this.chart_expenseCategory.Hide();

                if(accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') " +
                                                                                 "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                                                 "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month'));", accountDataRow);
                    }
                }
                else this.chart_expenseAccount.Hide();

                expenseGraphsMain.renderLabels(expenseGraphsMain.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '-1 month', 'localtime') " +
                                                                                    "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '-1 month', 'localtime') " +
                                                                                    "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '-1 month', 'localtime'));");

                expenseGraphsMain.renderLabels(expenseGraphsMain.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') " +
                                                                                    "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                                                    "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month'));");
            }
            else if (expenseGraphsMonth == 0)
            {
                clearChartData();
                if(categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month') " +
                                                                               "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                               "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month'));", categoryDataRow);
                    }
                }
                else this.chart_expenseCategory.Hide();

                if(accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month') " +
                                                                                 "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                                 "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month'));", accountDataRow);
                    }
                }
                else this.chart_expenseAccount.Hide();

                expenseGraphsMain.renderLabels(expenseGraphsMain.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao = DATE('now', 'localtime') " +
                                                                                    "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao = DATE('now', 'localtime') " +
                                                                                    "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao = DATE('now', 'localtime'));");
                
                expenseGraphsMain.renderLabels(expenseGraphsMain.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month') " +
                                                                                    "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                                    "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month'));");
            }
            else if (expenseGraphsMonth == 1)
            {
                clearChartData();
                if(categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                               "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                               "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month'));", categoryDataRow);
                    }
                }
                else this.chart_expenseCategory.Hide();

                if(accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                                 "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                                 "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month'));", accountDataRow);
                    }
                }
                else this.chart_expenseAccount.Hide();

                expenseGraphsMain.renderLabels(expenseGraphsMain.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '+1 month', 'localtime') " +
                                                                                    "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '+1 month', 'localtime') " +
                                                                                    "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '+1 month', 'localtime'));");

                expenseGraphsMain.renderLabels(expenseGraphsMain.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                                    "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                                    "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month'));");
            }
            else if (expenseGraphsMonth > 1)
            {
                clearChartData();
                int monthMore = expenseGraphsMonth + 1;
                if(categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + expenseGraphsMonth + " month') " +
                                                                               "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + expenseGraphsMonth + " month') " +
                                                                               "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + expenseGraphsMonth + " month'));", categoryDataRow);
                    }
                }
                else this.chart_expenseCategory.Hide();

                if(accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        this.renderExpenseChart(this.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + expenseGraphsMonth + " month') " +
                                                                                 "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + expenseGraphsMonth + " month') " +
                                                                                 "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + expenseGraphsMonth + " month'));", accountDataRow);
                    }
                }
                else this.chart_expenseAccount.Hide();

                expenseGraphsMain.renderLabels(expenseGraphsMain.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '+" + expenseGraphsMonth + " month', 'localtime') " +
                                                                    "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '+" + expenseGraphsMonth + " month', 'localtime') " +
                                                                    "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '+" + expenseGraphsMonth + " month', 'localtime');");

                expenseGraphsMain.renderLabels(expenseGraphsMain.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + expenseGraphsMonth + " month') " +
                                                                                    "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + expenseGraphsMonth + " month') " +
                                                                                    "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + expenseGraphsMonth + " month'));");
            }
        }

        //LIMPAR DADOS DO FORMULÁRIO
        private void clearChartData()
        {
            foreach (var series in this.chart_expenseAccount.Series) series.Points.Clear();
            foreach (var series in this.chart_expenseCategory.Series) series.Points.Clear();
        }

        //FUNÇÃO QUE RENDERIZA OS GRÁFICOS DE PIZZA DE ACORDO COM AS INFORMAÇÕES VINDAS DO BANCO DE DADOS
        protected internal void renderExpenseChart(System.Windows.Forms.DataVisualization.Charting.Chart chart, string query, DataRow accountDataRow)
        {
            chart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            chart.PaletteCustomColors = myPalette;
            DataTable dataTable = Database.query(query);
            if (dataTable.Rows.Count > 0)
            {
                chart.Show();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string category = accountDataRow.ItemArray[1].ToString();
                    if ((!string.IsNullOrEmpty(dataRow.ItemArray[0].ToString())) && ((Convert.ToInt32(dataRow.ItemArray[0])) != 0))
                    {
                        string graph = dataRow.ItemArray[0].ToString();
                        string sum = String.Format("{0:C}", dataRow.ItemArray[0]);
                        string label = category + "\n" + sum;
                        chart.Series[0].Points.AddXY(label, graph);
                    }
                }
            }
            else chart.Hide();
        }
    }
}
