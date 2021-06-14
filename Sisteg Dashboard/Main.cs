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
        //DECLARAÇÃO DE VARIÁVEIS
        Color[] myPalette = new Color[6]{
            Color.FromArgb(0, 104, 232),
            Color.FromKnownColor(KnownColor.Transparent),
            Color.FromArgb(0, 104, 232),
            Color.FromKnownColor(KnownColor.Transparent),
            Color.FromArgb(0, 104, 232),
            Color.FromKnownColor(KnownColor.Transparent),
        };
        protected internal int step = 0, month = 0, idConta = 0;
        protected internal IncomeGraphs incomeGraphs;
        protected internal Transactions transactions;
        protected internal ExpenseGraphs expenseGraphs;

        //INICIA INSTÂNCIA DO PAINEL, POPULANDO OS GRÁFICOS, TABELAS E INSCRIÇÕES DO QUADRO COM AS INFORMAÇÕES DO BANCO DE DADOS DE ACORDO COM O PERÍODO MENSAL
        public Main()
        {
            InitializeComponent();
            expenseGraphs = new ExpenseGraphs(this, month);
            transactions = new Transactions(this, month);
            this.pcb_minimizeProgram.Visible = true;

            //Popula o combobox de conta ativa
            DataTable activeAccountDataTable = Database.query("SELECT conta.nomeConta FROM conta ORDER BY conta.nomeConta;");
            for (int i = 0; i < activeAccountDataTable.Rows.Count; i++) this.cbb_activeAccount.Items.Insert(i, " " + activeAccountDataTable.Rows[i].ItemArray[0].ToString());

            //Seleciona a conta ativa
            activeAccountDataTable = Database.query("SELECT conta.idConta, conta.nomeConta FROM conta WHERE conta.contaAtiva = 1 ORDER BY conta.nomeConta;");
            this.cbb_activeAccount.SelectedIndex = this.cbb_activeAccount.FindString(" " + activeAccountDataTable.Rows[0].ItemArray[1].ToString());
            idConta = Convert.ToInt32(activeAccountDataTable.Rows[0].ItemArray[0]);
            MessageBox.Show(idConta.ToString());
            
            this.renderLabels(this.lbl_balance, "SELECT saldoConta FROM conta WHERE idConta = " + idConta);

            //Inicia painel dos gráficos de receitas
            incomeGraphs = new IncomeGraphs(this, month) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            this.panel_steps.Controls.Add(incomeGraphs);
            incomeGraphs.Show();
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

        //MENU DE NAVEGAÇÃO DA APLICAÇÃO
        private void pcb_btnClient_MouseEnter(object sender, EventArgs e) 
        { 
            this.pcb_btnClient.Image = Properties.Resources.btn_client_form_active; 
        }

        private void pcb_btnClient_MouseLeave(object sender, EventArgs e) 
        { 
            this.pcb_btnClient.Image = Properties.Resources.btn_client_form; 
        }

        private void pcb_btnClient_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ClientForm>().Count() == 0)
            {
                ClientForm clientForm = new ClientForm();
                clientForm.Show();
                this.Close();
            }
        }

        private void pcb_btnProduct_MouseEnter(object sender, EventArgs e) 
        { 
            this.pcb_btnProduct.Image = Properties.Resources.btn_product_form_active; 
        }

        private void pcb_btnProduct_MouseLeave(object sender, EventArgs e) 
        { 
            this.pcb_btnProduct.Image = Properties.Resources.btn_product_form; 
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

        private void pcb_btnBudget_MouseEnter(object sender, EventArgs e) 
        { 
            this.pcb_btnBudget.Image = Properties.Resources.btn_budget_form_active; 
        }

        private void pcb_btnBudget_MouseLeave(object sender, EventArgs e) 
        { 
            this.pcb_btnBudget.Image = Properties.Resources.btn_budget_form; 
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

        private void pcb_btnConfig_MouseEnter(object sender, EventArgs e) 
        { 
            this.pcb_btnConfig.Image = Properties.Resources.btn_config_main_active; 
        }

        private void pcb_btnConfig_MouseLeave(object sender, EventArgs e) 
        { 
            this.pcb_btnConfig.Image = Properties.Resources.btn_config_main; 
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

        //FUNÇÃO QUE RENDERIZA AS INSCRIÇÕES NO QUADRO DE ACORDO COM AS INFORMAÇÕES VINDAS DO BANCO DE DADOS
        protected internal void renderLabels(Label label, string query)
        {
            DataTable dataTable = Database.query(query);
            if (!String.IsNullOrEmpty(dataTable.Rows[0].ItemArray[0].ToString().Trim())) foreach (DataRow dataRow in dataTable.Rows) label.Text = String.Format("{0:C}", dataRow.ItemArray[0]); else label.Text = "R$ 0,00";
        }

        //ADICIONAR RECEITA
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

        //ADICIONAR DESPESA
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

        //MUDAR PERÍODO MENSAL PARA ATUALIZAÇÃO DOS GRÁFICOS, TABELAS E INSCRIÇÕES NO FORMULÁRIO
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
                            incomeGraphs.renderIncomeChart(incomeGraphs.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month'));", categoryDataRow);
                        }
                    }
                    else incomeGraphs.chart_incomeCategory.Hide();

                    if (accountDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow accountDataRow in accountDataTable.Rows)
                        {
                            incomeGraphs.renderIncomeChart(incomeGraphs.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                            "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                            "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month'));", accountDataRow);
                        }
                    }
                    else incomeGraphs.chart_incomeAccount.Hide();

                    this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                             "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                             "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month'));");

                    transactions.dgv_transactions.DataSource = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '-2 month') AND receita.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                              "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                              "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                              "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') ORDER BY dataTransacao DESC;");
                    if (transactions.dgv_transactions.DataSource == null) transactions.dgv_transactions.Hide();

                    dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month');");

                    if (dailyExpensesDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                        {
                            transactions.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                  "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                  "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = 1 AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month'));", dailyExpensesDataRow);
                        }
                    }
                    else
                    {
                        transactions.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM");
                        transactions.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                        transactions.lbl_noExpenses.Show();
                        transactions.chart_dailyExpenses.Hide();
                    }

                    if (expenseCategoryDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow categoryDataRow in expenseCategoryDataTable.Rows)
                        {
                            expenseGraphs.renderExpenseChart(expenseGraphs.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                   "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                   "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month'));", categoryDataRow);
                        }
                    }
                    else expenseGraphs.chart_expenseCategory.Hide();

                    if (accountDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow accountDataRow in accountDataTable.Rows)
                        {
                            expenseGraphs.renderExpenseChart(expenseGraphs.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                                "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                                                                "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month'));", accountDataRow);
                        }
                    }
                    else expenseGraphs.chart_expenseAccount.Hide();

                    this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '-2 month', 'localtime') " +
                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '-2 month', 'localtime') " +
                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '-2 month', 'localtime'));");

                    this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-2 month') AND despesa.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-2 month') AND repeticao.dataTransacao < datetime('now', 'start of month', '-1 month') " +
                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-2 month') AND parcela.dataTransacao < datetime('now', 'start of month', '-1 month'));");
                }
                else
                {
                    clearChartData();
                    int monthLess = month + 1;
                    if (incomeCategoryDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow categoryDataRow in incomeCategoryDataTable.Rows)
                        {
                            incomeGraphs.renderIncomeChart(incomeGraphs.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month'));", categoryDataRow);
                        }
                    }
                    else incomeGraphs.chart_incomeCategory.Hide();

                    if (accountDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow accountDataRow in accountDataTable.Rows)
                        {
                            incomeGraphs.renderIncomeChart(incomeGraphs.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                            "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                            "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month'));", accountDataRow);
                        }
                    }
                    else incomeGraphs.chart_incomeAccount.Hide();

                    this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                             "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                             "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month'));");

                    transactions.dgv_transactions.DataSource = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND receita.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                              "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                              "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                              "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') ORDER BY dataTransacao DESC;");
                    if (transactions.dgv_transactions.DataSource == null) transactions.dgv_transactions.Hide();

                    dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month');");

                    if (dailyExpensesDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                        {
                            transactions.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                  "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                  "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = 1 AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month'));", dailyExpensesDataRow);
                        }
                    }
                    else
                    {
                        transactions.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM");
                        transactions.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                        transactions.lbl_noExpenses.Show();
                        transactions.chart_dailyExpenses.Hide();
                    }

                    if (expenseCategoryDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow categoryDataRow in expenseCategoryDataTable.Rows)
                        {
                            expenseGraphs.renderExpenseChart(expenseGraphs.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                                  "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                                  "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month'));", categoryDataRow);
                        }
                    }
                    else expenseGraphs.chart_expenseCategory.Hide();

                    if (accountDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow accountDataRow in accountDataTable.Rows)
                        {
                            expenseGraphs.renderExpenseChart(expenseGraphs.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                                "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                                                                "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month'));", accountDataRow);
                        }
                    }
                    else expenseGraphs.chart_expenseAccount.Hide();

                    this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '" + month + " month', 'localtime') " +
                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '" + month + " month', 'localtime') " +
                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '" + month + " month', 'localtime'));");

                    this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND despesa.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND repeticao.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month') " +
                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '" + month + " month') AND parcela.dataTransacao < datetime('now', 'start of month', '" + monthLess + " month'));");
                }
            }
            else if (month == -1)
            {
                clearChartData();
                if (incomeCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in incomeCategoryDataTable.Rows)
                    {
                        incomeGraphs.renderIncomeChart(incomeGraphs.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month') " +
                                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month'));", categoryDataRow);
                    }
                }
                else incomeGraphs.chart_incomeCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        incomeGraphs.renderIncomeChart(incomeGraphs.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month') " +
                                                                                        "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                                                        "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month'));", accountDataRow);
                    }
                }
                else incomeGraphs.chart_incomeAccount.Hide();

                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month') " +
                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month'));");

                transactions.dgv_transactions.DataSource = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month', '-1 month') AND receita.dataTransacao < datetime('now', 'start of month') " +
                                                                          "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                                          "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month') " +
                                                                          "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') ORDER BY dataTransacao DESC;");
                if (transactions.dgv_transactions.DataSource == null) transactions.dgv_transactions.Hide();

                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month');");

                if (dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        transactions.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') " +
                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = 1 AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month'));", dailyExpensesDataRow);
                    }
                }
                else
                {
                    transactions.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM");
                    transactions.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    transactions.lbl_noExpenses.Show();
                    transactions.chart_dailyExpenses.Hide();
                }

                if (expenseCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in expenseCategoryDataTable.Rows)
                    {
                        expenseGraphs.renderExpenseChart(expenseGraphs.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') " +
                                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month'));", categoryDataRow);
                    }
                }
                else expenseGraphs.chart_expenseCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        expenseGraphs.renderExpenseChart(expenseGraphs.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') " +
                                                                                            "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                                                            "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month'));", accountDataRow);
                    }
                }
                else expenseGraphs.chart_expenseAccount.Hide();

                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '-1 month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '-1 month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '-1 month', 'localtime'));");

                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month', '-1 month') AND despesa.dataTransacao < datetime('now', 'start of month') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month', '-1 month') AND repeticao.dataTransacao < datetime('now', 'start of month') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month', '-1 month') AND parcela.dataTransacao < datetime('now', 'start of month'));");
            }
            else if (month == 0)
            {
                clearChartData();
                if (incomeCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in incomeCategoryDataTable.Rows)
                    {
                        incomeGraphs.renderIncomeChart(incomeGraphs.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month') " +
                                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month'));", categoryDataRow);
                    }
                }
                else incomeGraphs.chart_incomeCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        incomeGraphs.renderIncomeChart(incomeGraphs.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month') " +
                                                                                        "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                                        "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month'));", accountDataRow);
                    }
                }
                else incomeGraphs.chart_incomeAccount.Hide();

                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month') " +
                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month'));");

                transactions.dgv_transactions.DataSource = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month') " +
                                                                          "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                          "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month') " +
                                                                          "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month') ORDER BY dataTransacao DESC;");
                if (transactions.dgv_transactions.DataSource == null) transactions.dgv_transactions.Hide();

                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao > datetime('now', 'start of month');");

                if (dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        transactions.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month') " +
                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = 1 AND parcela.dataTransacao > datetime('now', 'start of month'));", dailyExpensesDataRow);
                    }
                }
                else
                {
                    transactions.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM");
                    transactions.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    transactions.lbl_noExpenses.Show();
                    transactions.chart_dailyExpenses.Hide();
                }

                if (expenseCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in expenseCategoryDataTable.Rows)
                    {
                        expenseGraphs.renderExpenseChart(expenseGraphs.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month') " +
                                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month'));", categoryDataRow);
                    }
                }
                else expenseGraphs.chart_expenseCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        expenseGraphs.renderExpenseChart(expenseGraphs.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao > datetime('now', 'start of month') " +
                                                                                            "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                                            "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month'));", accountDataRow);
                    }
                }
                else expenseGraphs.chart_expenseAccount.Hide();

                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao = DATE('now', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao = DATE('now', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao = DATE('now', 'localtime'));");

                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao > datetime('now', 'start of month') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month'));");
            }
            else if (month == 1)
            {
                clearChartData();
                if (incomeCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in incomeCategoryDataTable.Rows)
                    {
                        incomeGraphs.renderIncomeChart(incomeGraphs.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month'));", categoryDataRow);
                    }
                }
                else incomeGraphs.chart_incomeCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        incomeGraphs.renderIncomeChart(incomeGraphs.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                                        "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                                        "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month'));", accountDataRow);
                    }
                }
                else incomeGraphs.chart_incomeAccount.Hide();

                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month'));");

                transactions.dgv_transactions.DataSource = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao < datetime('now', 'start of month', '+2 month') AND receita.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                          "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                          "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                          "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') ORDER BY dataTransacao DESC;");
                if (transactions.dgv_transactions.DataSource == null) transactions.dgv_transactions.Hide();

                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month');");

                if (dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        transactions.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = 1 AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month'));", dailyExpensesDataRow);
                    }
                }
                else
                {
                    transactions.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM");
                    transactions.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    transactions.lbl_noExpenses.Show();
                    transactions.chart_dailyExpenses.Hide();
                }

                if (expenseCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in expenseCategoryDataTable.Rows)
                    {
                        expenseGraphs.renderExpenseChart(expenseGraphs.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month'));", categoryDataRow);
                    }
                }
                else expenseGraphs.chart_expenseCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        expenseGraphs.renderExpenseChart(expenseGraphs.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                                            "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                                                            "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month'));", accountDataRow);
                    }
                }
                else expenseGraphs.chart_expenseAccount.Hide();

                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '+1 month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '+1 month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '+1 month', 'localtime'));");

                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+2 month') AND despesa.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+2 month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+1 month') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+2 month') AND parcela.dataTransacao > datetime('now', 'start of month', '+1 month'));");
            }
            else if (month > 1)
            {
                clearChartData();
                int monthMore = month + 1;
                if (incomeCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in incomeCategoryDataTable.Rows)
                    {
                        incomeGraphs.renderIncomeChart(incomeGraphs.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month'));", categoryDataRow);
                    }
                }
                else incomeGraphs.chart_incomeCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        incomeGraphs.renderIncomeChart(incomeGraphs.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                                                        "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                                                        "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month'));", accountDataRow);
                    }
                }
                else incomeGraphs.chart_incomeAccount.Hide();

                this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                         "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                         "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month'));");
                

                transactions.dgv_transactions.DataSource = Database.query("SELECT idReceita AS 'ID:', valorReceita AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM receita JOIN categoria ON receita.idCategoria = categoria.idCategoria WHERE recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND receita.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                                          "UNION ALL SELECT idRepeticao AS 'ID:', valorRepeticao AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM repeticao JOIN categoria ON repeticao.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                                          "UNION ALL SELECT idParcela AS 'ID:', valorParcela AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM parcela JOIN categoria ON parcela.idCategoria = categoria.idCategoria WHERE (recebimentoConfirmado = true OR pagamentoConfirmado = true) AND parcela.idConta = " + idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                                          "UNION ALL SELECT idDespesa AS 'ID:', valorDespesa AS 'Valor:', dataTransacao AS 'Data:', nomeCategoria AS 'Categoria:' FROM despesa JOIN categoria ON despesa.idCategoria = categoria.idCategoria WHERE pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month') ORDER BY dataTransacao DESC;");
                if (transactions.dgv_transactions.DataSource == null) transactions.dgv_transactions.Hide();

                dailyExpensesDataTable = Database.query("SELECT idDespesa, dataTransacao FROM despesa WHERE despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month');");

                if (dailyExpensesDataTable.Rows.Count > 0)
                {
                    foreach (DataRow dailyExpensesDataRow in dailyExpensesDataTable.Rows)
                    {
                        transactions.renderDailyExpensesChart("SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa JOIN conta ON despesa.idConta = conta.idConta WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao JOIN despesa ON despesa.idDespesa = repeticao.idDespesa WHERE repeticao.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela JOIN despesa ON despesa.idDespesa = parcela.idDespesa WHERE parcela.pagamentoConfirmado = true AND despesa.idConta = 1 AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month'));", dailyExpensesDataRow);
                    }
                }
                else
                {
                    transactions.lbl_noExpenses.Text = "Não há gastos no mês de " + changeMonth.ToString("MMMM");
                    transactions.BackgroundImage = Properties.Resources.income_and_expense_table_there_is_no_expense_bg;
                    transactions.lbl_noExpenses.Show();
                    transactions.chart_dailyExpenses.Hide();
                }

                if (expenseCategoryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow categoryDataRow in expenseCategoryDataTable.Rows)
                    {
                        expenseGraphs.renderExpenseChart(expenseGraphs.chart_expenseCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                                                              "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                                                              "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month'));", categoryDataRow);
                    }
                }
                else expenseGraphs.chart_expenseCategory.Hide();

                if (accountDataTable.Rows.Count > 0)
                {
                    foreach (DataRow accountDataRow in accountDataTable.Rows)
                    {
                        expenseGraphs.renderExpenseChart(expenseGraphs.chart_expenseAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                                                            "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                                                            "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month'));", accountDataRow);
                    }
                }
                else expenseGraphs.chart_expenseAccount.Hide();

                this.renderLabels(this.lbl_dailyExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao = datetime('now', 'start of day', '+" + month + " month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao = datetime('now', 'start of day', '+" + month + " month', 'localtime') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao = datetime('now', 'start of day', '+" + month + " month', 'localtime');");

                this.renderLabels(this.lbl_monthExpenses, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorDespesa) valorTotal FROM despesa WHERE despesa.pagamentoConfirmado = true AND despesa.idConta = " + idConta + " AND despesa.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND despesa.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                          "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.pagamentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND repeticao.dataTransacao > datetime('now', 'start of month', '+" + month + " month') " +
                                                          "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.pagamentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao < datetime('now', 'start of month', '+" + monthMore + " month') AND parcela.dataTransacao > datetime('now', 'start of month', '+" + month + " month'));");
            }
        }

        //RETORNAR PERÍODO MENSAL
        private void pcb_previousMonth_Click(object sender, EventArgs e)
        {
            month = month - 1;
            this.changeMonth(month);
        }

        //AVANÇAR PERÍODO MENSAL
        private void pcb_nextMonth_Click(object sender, EventArgs e)
        {
            month = month + 1;
            this.changeMonth(month);
        }

        //LIMPAR DADOS DO FORMULÁRIO
        private void clearChartData()
        {
            transactions.dgv_transactions.DataSource = null;
            foreach (var series in expenseGraphs.chart_expenseAccount.Series) series.Points.Clear();
            foreach (var series in expenseGraphs.chart_expenseCategory.Series) series.Points.Clear();
            foreach (var series in incomeGraphs.chart_incomeAccount.Series) series.Points.Clear();
            foreach (var series in incomeGraphs.chart_incomeCategory.Series) series.Points.Clear();
            foreach (var series in transactions.chart_dailyExpenses.Series) series.Points.Clear();
        }

        //AVANÇAR AO PRÓXIMO QUADRO
        private void pcb_btnGoForward_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnGoForward.Image = Properties.Resources.btn_go_forward_active;
        }

        private void pcb_btnGoForward_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnGoForward.Image = Properties.Resources.btn_go_forward;
        }

        private void pcb_btnGoForward_Click(object sender, EventArgs e)
        {
            step += 1;
            if(step == 1)
            {
                this.panel_steps.Controls.Remove(incomeGraphs);
                transactions = new Transactions(this, month) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Add(transactions);
                transactions.Show();
            }
            else if(step == 2)
            {
                this.panel_steps.Controls.Remove(transactions);
                expenseGraphs = new ExpenseGraphs(this, month) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Add(expenseGraphs);
                expenseGraphs.Show();
            }
        }

        //RETORNAR AO QUADRO ANTERIOR
        private void pcb_btnGoBack_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnGoBack.Image = Properties.Resources.btn_go_back_active;
        }

        private void pcb_btnGoBack_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnGoBack.Image = Properties.Resources.btn_go_back;
        }

        private void pcb_btnGoBack_Click(object sender, EventArgs e)
        {
            step -= 1;
            if (step == 0)
            {
                this.panel_steps.Controls.Remove(transactions);
                incomeGraphs = new IncomeGraphs(this, month) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Add(incomeGraphs);
                incomeGraphs.Show();
            }
            else if (step == 1)
            {
                this.panel_steps.Controls.Remove(expenseGraphs);
                transactions = new Transactions(this, month) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Add(transactions);
                transactions.Show();
            }
        }

        //SELECIONAR CONTA ATIVA
        private void cbb_activeAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable activeAccountDataTable = Database.query("SELECT idConta FROM conta WHERE contaAtiva = 1;");
            Account account = new Account();
            account.idConta = Convert.ToInt32(activeAccountDataTable.Rows[0].ItemArray[0]);
            account.contaAtiva = false;
            Database.updateActiveAccount(account);

            activeAccountDataTable = Database.query("SELECT idConta FROM conta WHERE nomeConta = '" + this.cbb_activeAccount.SelectedItem.ToString().Trim() + "';");
            idConta = Convert.ToInt32(activeAccountDataTable.Rows[0].ItemArray[0]);
            account.idConta = Convert.ToInt32(activeAccountDataTable.Rows[0].ItemArray[0]);
            account.contaAtiva = true;
            Database.updateActiveAccount(account);

            //Atualiza os dados após a seleção da conta ativa
            /*clearChartData();
            DataTable accountDataTable = Database.query("SELECT idConta, nomeConta FROM conta;");
            DataTable categoryDataTable = Database.query("SELECT idCategoria, nomeCategoria FROM categoria WHERE categoriaReceita != 0;");

            if (categoryDataTable.Rows.Count > 0)
            {
                foreach (DataRow categoryDataRow in categoryDataTable.Rows)
                {
                    incomeGraphs.renderIncomeChart(incomeGraphs.chart_incomeCategory, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month') " +
                                                                                      "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                                      "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.idCategoria = " + categoryDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month'));", categoryDataRow);
                }
            }
            else incomeGraphs.chart_incomeCategory.Hide();

            if (accountDataTable.Rows.Count > 0)
            {
                foreach (DataRow accountDataRow in accountDataTable.Rows)
                {
                    incomeGraphs.renderIncomeChart(incomeGraphs.chart_incomeAccount, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND receita.dataTransacao > datetime('now', 'start of month') " +
                                                                                    "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                                                    "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + accountDataRow.ItemArray[0].ToString() + " AND parcela.dataTransacao > datetime('now', 'start of month'));", accountDataRow);
                }
            }
            else incomeGraphs.chart_incomeAccount.Hide();

            this.renderLabels(this.lbl_monthIncomes, "SELECT SUM(valorTotal) valorTotal FROM (SELECT SUM(valorReceita) valorTotal FROM receita WHERE receita.recebimentoConfirmado = true AND receita.idConta = " + idConta + " AND receita.dataTransacao > datetime('now', 'start of month') " +
                                                     "UNION ALL SELECT SUM(valorRepeticao) valorTotal FROM repeticao WHERE repeticao.recebimentoConfirmado = true AND repeticao.idConta = " + idConta + " AND repeticao.dataTransacao > datetime('now', 'start of month') " +
                                                     "UNION ALL SELECT SUM(valorParcela) valorTotal FROM parcela WHERE parcela.recebimentoConfirmado = true AND parcela.idConta = " + idConta + " AND parcela.dataTransacao > datetime('now', 'start of month'));");

            this.renderLabels(this.lbl_balance, "SELECT saldoConta FROM conta WHERE idConta = " + idConta);*/
        }
    }
}
