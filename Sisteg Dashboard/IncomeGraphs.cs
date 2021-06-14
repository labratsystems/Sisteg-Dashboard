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
    public partial class IncomeGraphs : Form
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
        private Main incomeGraphsMain;
        protected internal int incomeGraphsMonth = 0, idConta = 0;
        protected internal string nomeConta;

        //INICIA INSTÂNCIA DO PAINEL, POPULANDO OS GRÁFICOS DE RECEITAS COM AS INFORMAÇÕES DO BANCO DE DADOS DE ACORDO COM O PERÍODO MENSAL
        public IncomeGraphs(Main main, int month)
        {
            InitializeComponent();
            //Atribuições e formatação dos componentes do formulário principal
            incomeGraphsMain = main;
            incomeGraphsMonth = month;
            incomeGraphsMain.BackgroundImage = Properties.Resources.income_homepage_bg;
            incomeGraphsMain.lbl_balance.Visible = true;
            incomeGraphsMain.lbl_balanceTag.Visible = true;
            incomeGraphsMain.lbl_monthIncomes.Visible = true;
            incomeGraphsMain.lbl_monthIncomesTag.Visible = true;
            incomeGraphsMain.pcb_addIncome.Visible = true;
            incomeGraphsMain.pcb_btnGoBack.Visible = false;
            incomeGraphsMain.pcb_btnGoForward.Visible = true;
            incomeGraphsMain.lbl_dailyExpenses.Visible = false;
            incomeGraphsMain.lbl_dailyExpensesTag.Visible = false;
            incomeGraphsMain.lbl_monthExpenses.Visible = false;
            incomeGraphsMain.lbl_monthExpensesTag.Visible = false;
            incomeGraphsMain.pcb_addExpense.Visible = false;
            incomeGraphsMain.cbb_activeAccount.Visible = true;

            idConta = incomeGraphsMain.idConta;

            if (incomeGraphsMonth == 0)
            {
                DataTable accountDataTable = Database.query("SELECT idConta, nomeConta FROM conta;");
                DataTable categoryDataTable = Database.query("SELECT idCategoria, nomeCategoria FROM categoria WHERE categoriaReceita != 0;");

                if (categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        this.renderIncomeChart(this.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month') " +
                                                                               "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                               "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month'));", categoryDataRow);
                    }
                }
                else this.chart_incomeCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        this.renderIncomeChart(this.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month') " +
                                                                               "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                               "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month'));", accountDataRow);
                    }
                }
                else this.chart_incomeAccount.Hide();

                incomeGraphsMain.renderLabels(incomeGraphsMain.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month') " +
                                                                                 "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                                 "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month'));");
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

        //MUDAR PERÍODO MENSAL PARA ATUALIZAÇÃO DOS GRÁFICOS, TABELAS E INSCRIÇÕES NO FORMULÁRIO
        private void changeMonth()
        {
            DataTable accountDataTable = Database.query("SELECT idConta, nomeConta FROM conta;");
            DataTable categoryDataTable = Database.query("SELECT idCategoria, nomeCategoria FROM categoria WHERE categoriaReceita != 0;");
            if (incomeGraphsMonth < -1)
            {
                if (incomeGraphsMonth == -2)
                {
                    clearChartData();
                    if (categoryDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                        {
                            this.renderIncomeChart(this.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month'));", categoryDataRow);
                        }
                    }
                    else this.chart_incomeCategory.Hide();

                    if (accountDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow accountDataRow in accountDataTable.Rows)
                        {
                            this.renderIncomeChart(this.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                             "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                             "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month'));", accountDataRow);
                        }
                    }
                    else this.chart_incomeAccount.Hide();
                    
                    incomeGraphsMain.renderLabels(incomeGraphsMain.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                     "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                     "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month'));");
                }
                else
                {
                    clearChartData();
                    int monthLess = incomeGraphsMonth + 1;
                    if (categoryDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                        {
                            this.renderIncomeChart(this.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month', '" + incomeGraphsMonth + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + incomeGraphsMonth + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + incomeGraphsMonth + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month'));", categoryDataRow);
                        }
                    }
                    else this.chart_incomeCategory.Hide();

                    if (accountDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow accountDataRow in accountDataTable.Rows)
                        {
                            this.renderIncomeChart(this.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month', '" + incomeGraphsMonth + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                             "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + incomeGraphsMonth + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                             "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + incomeGraphsMonth + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month'));", accountDataRow);
                        }
                    }
                    else this.chart_incomeAccount.Hide();
                    
                    incomeGraphsMain.renderLabels(incomeGraphsMain.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '" + incomeGraphsMonth + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                     "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + incomeGraphsMonth + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                     "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + incomeGraphsMonth + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month'));");
                }
            }
            else if (incomeGraphsMonth == -1)
            {
                clearChartData();
                if (categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        this.renderIncomeChart(this.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month') " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month'));", categoryDataRow);
                    }
                }
                else chart_incomeCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        this.renderIncomeChart(this.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month') " +
                                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month'));", accountDataRow);
                    }
                }
                else chart_incomeAccount.Hide();
                
                incomeGraphsMain.renderLabels(incomeGraphsMain.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month') " +
                                                                                 "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                                                 "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month'));");
            }
            else if (incomeGraphsMonth == 0)
            {
                clearChartData();
                if (categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        this.renderIncomeChart(this.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month') " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month'));", categoryDataRow);
                    }
                }
                else this.chart_incomeCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        this.renderIncomeChart(this.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month') " +
                                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month'));", accountDataRow);
                    }
                }
                else this.chart_incomeAccount.Hide();
                
                incomeGraphsMain.renderLabels(incomeGraphsMain.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month') " +
                                                                                 "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                                 "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month'));");
            }
            else if (incomeGraphsMonth == 1)
            {
                clearChartData();
                if (categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        this.renderIncomeChart(this.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month'));", categoryDataRow);
                    }
                }
                else this.chart_incomeCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        this.renderIncomeChart(this.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month'));", accountDataRow);
                    }
                }
                else this.chart_incomeAccount.Hide();
                
                incomeGraphsMain.renderLabels(incomeGraphsMain.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                                 "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                                 "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month'));");
            }
            else if (incomeGraphsMonth > 1)
            {
                clearChartData();
                int monthMore = incomeGraphsMonth + 1;
                if (categoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                    {
                        this.renderIncomeChart(this.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + incomeGraphsMonth + " month') " +
                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + incomeGraphsMonth + " month') " +
                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + incomeGraphsMonth + " month'));", categoryDataRow);
                    }
                }
                else this.chart_incomeCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        this.renderIncomeChart(this.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + incomeGraphsMonth + " month') " +
                                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + incomeGraphsMonth + " month') " +
                                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + incomeGraphsMonth + " month'));", accountDataRow);
                    }
                }
                else this.chart_incomeCategory.Hide();
                
                incomeGraphsMain.renderLabels(incomeGraphsMain.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + incomeGraphsMonth + " month') " +
                                                                                 "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + incomeGraphsMonth + " month') " +
                                                                                 "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + incomeGraphsMonth + " month'));");
            }
        }

        //LIMPAR DADOS DO FORMULÁRIO
        private void clearChartData()
        {
            foreach (var series in this.chart_incomeAccount.Series) series.Points.Clear();
            foreach (var series in this.chart_incomeCategory.Series) series.Points.Clear();
        }

        //FUNÇÃO QUE RENDERIZA OS GRÁFICOS DE PIZZA DE ACORDO COM AS INFORMAÇÕES VINDAS DO BANCO DE DADOS
        protected internal void renderIncomeChart(System.Windows.Forms.DataVisualization.Charting.Chart chart, string query, DataRow accountDataRow)
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
