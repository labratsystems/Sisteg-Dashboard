using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class Main : Form
    {
        Color[] myPalette = new Color[6]{
            Color.FromArgb(0, 104, 232),
            Color.FromKnownColor(KnownColor.Transparent),
            Color.FromArgb(0, 104, 232),
            Color.FromKnownColor(KnownColor.Transparent),
            Color.FromArgb(0, 104, 232),
            Color.FromKnownColor(KnownColor.Transparent),
        };
        private static int month = 0;
        private static DateTime dateTimeMonth = DateTime.Now;

        public Main()
        {
            InitializeComponent();
            DateTime month = dateTimeMonth.Date;
            this.lbl_noExpenses.Text = "Não há gastos no mês de " + month.ToString("MMMM");
            this.dgv_transactions.DataSource = Database.readTransactions("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', categoriaReceita AS 'Categoria:' FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month') UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', categoriaDespesa AS 'Categoria:' FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month') ORDER BY dataTransacao DESC; ");
            this.renderCharts(this.chart_expenseCategory, "SELECT categoriaDespesa, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month') GROUP BY categoriaDespesa;");
            this.renderCharts(this.chart_incomeCategory, "SELECT categoriaReceita, SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month') GROUP BY categoriaReceita;");
            this.renderCharts(this.chart_expenseAcount, "SELECT nomeConta, SUM(valorReceita) FROM conta, receita WHERE receita.dataTransacao > datetime('now', 'start of month') GROUP BY nomeConta;");
            this.renderCharts(this.chart_incomeAcount, "SELECT nomeConta, SUM(valorDespesa) FROM conta, despesa WHERE despesa.dataTransacao > datetime('now', 'start of month') GROUP BY nomeConta;");
            this.renderDailyExpensesChart("SELECT dataTransacao, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month') GROUP BY dataTransacao;");
            this.renderLabels(this.lbl_balance, "SELECT saldoConta FROM conta WHERE idConta = 1;");
            this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao = DATE();");
            this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month');");
            this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month');");
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handlerparam = base.CreateParams;
                handlerparam.ExStyle |= 0x02000000;
                return handlerparam;
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
            if (Application.OpenForms.OfType<Client>().Count() == 0)
            {
                Client client = new Client();
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
            if (Application.OpenForms.OfType<Product>().Count() == 0)
            {
                Product product = new Product();
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
            if (Application.OpenForms.OfType<Budget>().Count() == 0)
            {
                Budget budget = new Budget();
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

        private void dgv_transactions_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv_transactions.Columns[1].DefaultCellStyle.Format = "C";
            foreach (DataGridViewColumn dataGridViewColumn in dgv_transactions.Columns)
            {
                dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewRow dataGridViewRow in dgv_transactions.Rows)
            {
                string id = dataGridViewRow.Cells[0].Value.ToString();
                string value = (dataGridViewRow.Cells[1].Value.ToString()).Replace(',', '.');
                string date = dataGridViewRow.Cells[2].Value.ToString();
                string category = dataGridViewRow.Cells[3].Value.ToString();
                if (Database.checkIfItIsExpense(id, value, date, category))
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

        private void renderCharts(System.Windows.Forms.DataVisualization.Charting.Chart chart, string query)
        {
            chart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            chart.PaletteCustomColors = myPalette;
            DataTable dataTable = Database.readExpensesOrIncomes(query);
            if (dataTable.Rows.Count > 0)
            {
                chart.Show();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string category = dataRow.ItemArray[0].ToString();
                    string graph = dataRow.ItemArray[1].ToString();
                    string sum = String.Format("{0:C}", dataRow.ItemArray[1]);
                    string label = category + "\n" + sum;
                    chart.Series[0].Points.AddXY(label, graph);
                }
            }
            else
            {
                chart.Hide();
            }
        }

        private void renderDailyExpensesChart(string query)
        {
            this.chart_dailyExpenses.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.chart_dailyExpenses.PaletteCustomColors = myPalette;

            DataTable dataTable = Database.readExpensesOrIncomes(query);
            if(dataTable.Rows.Count > 0)
            {
                this.BackgroundImage = Properties.Resources.homepage_sisteg;
                this.lbl_noExpenses.Hide();
                this.chart_dailyExpenses.Show();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string date = dataRow.ItemArray[0].ToString();
                    DateTime dateTime = Convert.ToDateTime(date);
                    string day = dateTime.Day.ToString();
                    string month = dateTime.ToString("MMMM");
                    string sum = String.Format("{0:C}", dataRow.ItemArray[1]);
                    this.chart_dailyExpenses.Titles[0].Text = "Gastos do mês de " + month;
                    this.chart_dailyExpenses.Series[0].Points.AddXY(day, sum);
                }
            }
            else
            {
                this.BackgroundImage = Properties.Resources.homepage_sisteg_noExpenses;
                this.lbl_noExpenses.Show();
                this.chart_dailyExpenses.Hide();
            }
        }

        private void renderLabels(Label label, string query)
        {
            DataTable dataTable = Database.readExpensesOrIncomes(query);
            if (dataTable.Rows[0].ItemArray[0].ToString() != "")
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    label.Text = String.Format("{0:C}", dataRow.ItemArray[0]);
                }
            }
            else
            {
                label.Text = "R$ 0,00";
            }
        }

        private void pcb_addIncome_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<AddIncome>().Count() == 0)
            {
                AddIncome addIncome = new AddIncome(null);
                addIncome.pcb_btnUpdate.Hide();
                addIncome.pcb_btnDelete.Hide();
                addIncome.pcb_incomeRegister.Location = new Point(628, 312);
                addIncome.Show();
                this.Close();
            }
        }

        private void pcb_appClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pcb_updateIncome_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow dataGridViewRow in this.dgv_transactions.SelectedRows)
            {
                string id = dataGridViewRow.Cells[0].Value.ToString();
                string value = dataGridViewRow.Cells[1].Value.ToString().Replace(',', '.');
                string date = dataGridViewRow.Cells[2].Value.ToString();
                string category = dataGridViewRow.Cells[3].Value.ToString();
                if (Database.checkIfItIsExpense(id, value, date, category))
                {
                    if (Application.OpenForms.OfType<AddExpense>().Count() == 0)
                    {
                        DataTable dataTable = Database.query("SELECT * FROM despesa WHERE idDespesa = " + id + ";");
                        AddExpense addExpense = new AddExpense(dataTable);
                        addExpense.Show();
                        this.Close();
                    }
                }
                else
                {
                    if (Application.OpenForms.OfType<AddIncome>().Count() == 0)
                    {
                        DataTable dataTable = Database.query("SELECT * FROM receita WHERE idReceita = " + id + ";");
                        AddIncome addIncome = new AddIncome(dataTable);
                        addIncome.Show();
                        this.Close();
                    }
                }
            }
        }

        private void pcb_addExpense_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<AddExpense>().Count() == 0)
            {
                AddExpense addExpense = new AddExpense(null);
                addExpense.pcb_btnUpdate.Hide();
                addExpense.pcb_btnDelete.Hide();
                addExpense.pcb_expenseRegister.Location = new Point(628, 312);
                addExpense.Show();
                this.Close();
            }
        }

        private void pcb_previousMonth_Click(object sender, EventArgs e)
        {
            month = month - 1;
            DateTime previousMonth = dateTimeMonth.AddMonths(month);
            this.lbl_noExpenses.Text = "Não há gastos no mês de " + previousMonth.ToString("MMMM");
            if (month < -1)
            {
                if (month == -2)
                {
                    clearChartData();
                    this.dgv_transactions.DataSource = Database.readTransactions("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', categoriaReceita AS 'Categoria:' FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month') UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', categoriaDespesa AS 'Categoria:' FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') ORDER BY dataTransacao DESC; ");
                    this.renderCharts(this.chart_expenseCategory, "SELECT categoriaDespesa, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') GROUP BY categoriaDespesa;");
                    this.renderCharts(this.chart_incomeCategory, "SELECT categoriaReceita, SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month') GROUP BY categoriaReceita;");
                    this.renderCharts(this.chart_expenseAcount, "SELECT nomeConta, SUM(valorReceita) FROM conta, receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month') GROUP BY nomeConta;");
                    this.renderCharts(this.chart_incomeAcount, "SELECT nomeConta, SUM(valorDespesa) FROM conta, despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') GROUP BY nomeConta;");
                    this.renderDailyExpensesChart("SELECT dataTransacao, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') GROUP BY dataTransacao;");
                    this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao = datetime('now', 'start of day', '-2 month');");
                    this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month');");
                    this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month');");
                }
                else
                {
                    clearChartData();
                    int monthLess = month + 1;
                    this.dgv_transactions.DataSource = Database.readTransactions("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', categoriaReceita AS 'Categoria:' FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', categoriaDespesa AS 'Categoria:' FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') ORDER BY dataTransacao DESC; ");
                    this.renderCharts(this.chart_expenseCategory, "SELECT categoriaDespesa, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') GROUP BY categoriaDespesa;");
                    this.renderCharts(this.chart_incomeCategory, "SELECT categoriaReceita, SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') GROUP BY categoriaReceita;");
                    this.renderCharts(this.chart_expenseAcount, "SELECT nomeConta, SUM(valorReceita) FROM conta, receita WHERE receita.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') GROUP BY nomeConta;");
                    this.renderCharts(this.chart_incomeAcount, "SELECT nomeConta, SUM(valorDespesa) FROM conta, despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') GROUP BY nomeConta;");
                    this.renderDailyExpensesChart("SELECT dataTransacao, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') GROUP BY dataTransacao;");
                    this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao = datetime('now', 'start of day', '" + month + " month');");
                    this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month');");
                    this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month');");
                }
            }
            else if (month == -1)
            {
                clearChartData();
                this.dgv_transactions.DataSource = Database.readTransactions("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', categoriaReceita AS 'Categoria:' FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month') UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', categoriaDespesa AS 'Categoria:' FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') ORDER BY dataTransacao DESC; ");
                this.renderCharts(this.chart_expenseCategory, "SELECT categoriaDespesa, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') GROUP BY categoriaDespesa;");
                this.renderCharts(this.chart_incomeCategory, "SELECT categoriaReceita, SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month') GROUP BY categoriaReceita;");
                this.renderCharts(this.chart_expenseAcount, "SELECT nomeConta, SUM(valorReceita) FROM conta, receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month') GROUP BY nomeConta;");
                this.renderCharts(this.chart_incomeAcount, "SELECT nomeConta, SUM(valorDespesa) FROM conta, despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') GROUP BY nomeConta;");
                this.renderDailyExpensesChart("SELECT dataTransacao, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') GROUP BY dataTransacao;");
                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao = datetime('now', 'start of day', '-1 month');");
                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month');");
                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month');");
            }
            else if (month == 0)
            {
                clearChartData();
                this.dgv_transactions.DataSource = Database.readTransactions("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', categoriaReceita AS 'Categoria:' FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month') UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', categoriaDespesa AS 'Categoria:' FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month') ORDER BY dataTransacao DESC; ");
                this.renderCharts(this.chart_expenseCategory, "SELECT categoriaDespesa, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month') GROUP BY categoriaDespesa;");
                this.renderCharts(this.chart_incomeCategory, "SELECT categoriaReceita, SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month') GROUP BY categoriaReceita;");
                this.renderCharts(this.chart_expenseAcount, "SELECT nomeConta, SUM(valorReceita) FROM conta, receita WHERE receita.dataTransacao > datetime('now', 'start of month') GROUP BY nomeConta;");
                this.renderCharts(this.chart_incomeAcount, "SELECT nomeConta, SUM(valorDespesa) FROM conta, despesa WHERE despesa.dataTransacao > datetime('now', 'start of month') GROUP BY nomeConta;");
                this.renderDailyExpensesChart("SELECT dataTransacao, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month') GROUP BY dataTransacao;");
                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao = DATE();");
                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month');");
                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month');");
            }
            else if (month == 1)
            {
                clearChartData();
                this.dgv_transactions.DataSource = Database.readTransactions("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', categoriaReceita AS 'Categoria:' FROM receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month') UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', categoriaDespesa AS 'Categoria:' FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') ORDER BY dataTransacao DESC; ");
                this.renderCharts(this.chart_expenseCategory, "SELECT categoriaDespesa, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') GROUP BY categoriaDespesa;");
                this.renderCharts(this.chart_incomeCategory, "SELECT categoriaReceita, SUM(valorReceita) FROM receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month') GROUP BY categoriaReceita;");
                this.renderCharts(this.chart_expenseAcount, "SELECT nomeConta, SUM(valorReceita) FROM conta, receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month') GROUP BY nomeConta;");
                this.renderCharts(this.chart_incomeAcount, "SELECT nomeConta, SUM(valorDespesa) FROM conta, despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') GROUP BY nomeConta;");
                this.renderDailyExpensesChart("SELECT dataTransacao, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') GROUP BY dataTransacao;");
                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao = datetime('now', 'start of day', '+1 month');");
                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorReceita) FROM receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month');");
                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month');");
            }
            else if (month > 1)
            {
                clearChartData();
                int monthMore = month + 1;
                this.dgv_transactions.DataSource = Database.readTransactions("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', categoriaReceita AS 'Categoria:' FROM receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month') UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', categoriaDespesa AS 'Categoria:' FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month') ORDER BY dataTransacao DESC; ");
                this.renderCharts(this.chart_expenseCategory, "SELECT categoriaDespesa, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month') GROUP BY categoriaDespesa;");
                this.renderCharts(this.chart_incomeCategory, "SELECT categoriaReceita, SUM(valorReceita) FROM receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month') GROUP BY categoriaReceita;");
                this.renderCharts(this.chart_expenseAcount, "SELECT nomeConta, SUM(valorReceita) FROM conta, receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month') GROUP BY nomeConta;");
                this.renderCharts(this.chart_incomeAcount, "SELECT nomeConta, SUM(valorDespesa) FROM conta, despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month') GROUP BY nomeConta;");
                this.renderDailyExpensesChart("SELECT dataTransacao, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month') GROUP BY dataTransacao;");
                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao = datetime('now', 'start of day', '+" + month + " month');");
                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorReceita) FROM receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month');");
                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month');");
            }
        }

        private void pcb_nextMonth_Click(object sender, EventArgs e)
        {
            month = month + 1;
            DateTime nextMonth = dateTimeMonth.AddMonths(month);
            this.lbl_noExpenses.Text = "Não há gastos no mês de " + nextMonth.ToString("MMMM");
            if (month < -1)
            {
                if (month == -2)
                {
                    clearChartData();
                    this.dgv_transactions.DataSource = Database.readTransactions("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', categoriaReceita AS 'Categoria:' FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month') UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', categoriaDespesa AS 'Categoria:' FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') ORDER BY dataTransacao DESC; ");
                    this.renderCharts(this.chart_expenseCategory, "SELECT categoriaDespesa, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') GROUP BY categoriaDespesa;");
                    this.renderCharts(this.chart_incomeCategory, "SELECT categoriaReceita, SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month') GROUP BY categoriaReceita;");
                    this.renderCharts(this.chart_expenseAcount, "SELECT nomeConta, SUM(valorReceita) FROM conta, receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month') GROUP BY nomeConta;");
                    this.renderCharts(this.chart_incomeAcount, "SELECT nomeConta, SUM(valorDespesa) FROM conta, despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') GROUP BY nomeConta;");
                    this.renderDailyExpensesChart("SELECT dataTransacao, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') GROUP BY dataTransacao;");
                    this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao = datetime('now', 'start of day', '-2 month');");
                    this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month');");
                    this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month');");
                }
                else
                {
                    clearChartData();
                    int monthLess = month + 1;
                    this.dgv_transactions.DataSource = Database.readTransactions("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', categoriaReceita AS 'Categoria:' FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', categoriaDespesa AS 'Categoria:' FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') ORDER BY dataTransacao DESC; ");
                    this.renderCharts(this.chart_expenseCategory, "SELECT categoriaDespesa, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') GROUP BY categoriaDespesa;");
                    this.renderCharts(this.chart_incomeCategory, "SELECT categoriaReceita, SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') GROUP BY categoriaReceita;");
                    this.renderCharts(this.chart_expenseAcount, "SELECT nomeConta, SUM(valorReceita) FROM conta, receita WHERE receita.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') GROUP BY nomeConta;");
                    this.renderCharts(this.chart_incomeAcount, "SELECT nomeConta, SUM(valorDespesa) FROM conta, despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') GROUP BY nomeConta;");
                    this.renderDailyExpensesChart("SELECT dataTransacao, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') GROUP BY dataTransacao;");
                    this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao = datetime('now', 'start of day', '" + month + " month');");
                    this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month');");
                    this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month');");
                }
            }
            else if (month == -1)
            {
                clearChartData();
                this.dgv_transactions.DataSource = Database.readTransactions("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', categoriaReceita AS 'Categoria:' FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month') UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', categoriaDespesa AS 'Categoria:' FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') ORDER BY dataTransacao DESC; ");
                this.renderCharts(this.chart_expenseCategory, "SELECT categoriaDespesa, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') GROUP BY categoriaDespesa;");
                this.renderCharts(this.chart_incomeCategory, "SELECT categoriaReceita, SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month') GROUP BY categoriaReceita;");
                this.renderCharts(this.chart_expenseAcount, "SELECT nomeConta, SUM(valorReceita) FROM conta, receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month') GROUP BY nomeConta;");
                this.renderCharts(this.chart_incomeAcount, "SELECT nomeConta, SUM(valorDespesa) FROM conta, despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') GROUP BY nomeConta;");
                this.renderDailyExpensesChart("SELECT dataTransacao, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') GROUP BY dataTransacao;");
                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao = datetime('now', 'start of day', '-1 month');");
                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month');");
                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month');");
            }
            else if (month == 0)
            {
                clearChartData();
                this.dgv_transactions.DataSource = Database.readTransactions("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', categoriaReceita AS 'Categoria:' FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month') UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', categoriaDespesa AS 'Categoria:' FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month') ORDER BY dataTransacao DESC; ");
                this.renderCharts(this.chart_expenseCategory, "SELECT categoriaDespesa, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month') GROUP BY categoriaDespesa;");
                this.renderCharts(this.chart_incomeCategory, "SELECT categoriaReceita, SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month') GROUP BY categoriaReceita;");
                this.renderCharts(this.chart_expenseAcount, "SELECT nomeConta, SUM(valorReceita) FROM conta, receita WHERE receita.dataTransacao > datetime('now', 'start of month') GROUP BY nomeConta;");
                this.renderCharts(this.chart_incomeAcount, "SELECT nomeConta, SUM(valorDespesa) FROM conta, despesa WHERE despesa.dataTransacao > datetime('now', 'start of month') GROUP BY nomeConta;");
                this.renderDailyExpensesChart("SELECT dataTransacao, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month') GROUP BY dataTransacao;");
                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao = DATE();");
                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorReceita) FROM receita WHERE receita.dataTransacao > datetime('now', 'start of month');");
                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month');");
            }
            else if (month == 1)
            {
                clearChartData();
                this.dgv_transactions.DataSource = Database.readTransactions("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', categoriaReceita AS 'Categoria:' FROM receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month') UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', categoriaDespesa AS 'Categoria:' FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') ORDER BY dataTransacao DESC; ");
                this.renderCharts(this.chart_expenseCategory, "SELECT categoriaDespesa, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') GROUP BY categoriaDespesa;");
                this.renderCharts(this.chart_incomeCategory, "SELECT categoriaReceita, SUM(valorReceita) FROM receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month') GROUP BY categoriaReceita;");
                this.renderCharts(this.chart_expenseAcount, "SELECT nomeConta, SUM(valorReceita) FROM conta, receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month') GROUP BY nomeConta;");
                this.renderCharts(this.chart_incomeAcount, "SELECT nomeConta, SUM(valorDespesa) FROM conta, despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') GROUP BY nomeConta;");
                this.renderDailyExpensesChart("SELECT dataTransacao, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') GROUP BY dataTransacao;");
                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao = datetime('now', 'start of day', '+1 month');");
                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorReceita) FROM receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month');");
                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month');");
            }
            else if (month > 1)
            {
                clearChartData();
                int monthMore = month + 1;
                this.dgv_transactions.DataSource = Database.readTransactions("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', categoriaReceita AS 'Categoria:' FROM receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month') UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', categoriaDespesa AS 'Categoria:' FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month') ORDER BY dataTransacao DESC; ");
                this.renderCharts(this.chart_expenseCategory, "SELECT categoriaDespesa, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month') GROUP BY categoriaDespesa;");
                this.renderCharts(this.chart_incomeCategory, "SELECT categoriaReceita, SUM(valorReceita) FROM receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month') GROUP BY categoriaReceita;");
                this.renderCharts(this.chart_expenseAcount, "SELECT nomeConta, SUM(valorReceita) FROM conta, receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month') GROUP BY nomeConta;");
                this.renderCharts(this.chart_incomeAcount, "SELECT nomeConta, SUM(valorDespesa) FROM conta, despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month') GROUP BY nomeConta;");
                this.renderDailyExpensesChart("SELECT dataTransacao, SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month') GROUP BY dataTransacao;");
                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao = datetime('now', 'start of day', '+" + month + " month');");
                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorReceita) FROM receita WHERE receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month');");
                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorDespesa) FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month');");
            }
        }

        private void clearChartData()
        {
            this.dgv_transactions.DataSource = null;
            foreach (var series in this.chart_expenseCategory.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in this.chart_incomeCategory.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in this.chart_expenseAcount.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in this.chart_incomeAcount.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in this.chart_dailyExpenses.Series)
            {
                series.Points.Clear();
            }
        }
    }
}
