namespace Sisteg_Dashboard
{
    partial class Transactions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Transactions));
            this.dgv_transactions = new System.Windows.Forms.DataGridView();
            this.lbl_noExpenses = new System.Windows.Forms.Label();
            this.chart_dailyExpenses = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pcb_updateIncomeOrExpense = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_transactions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_dailyExpenses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_updateIncomeOrExpense)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_transactions
            // 
            this.dgv_transactions.AllowUserToAddRows = false;
            this.dgv_transactions.AllowUserToDeleteRows = false;
            this.dgv_transactions.AllowUserToResizeColumns = false;
            this.dgv_transactions.AllowUserToResizeRows = false;
            this.dgv_transactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_transactions.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.dgv_transactions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_transactions.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgv_transactions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Courier Prime Code", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_transactions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_transactions.ColumnHeadersHeight = 30;
            this.dgv_transactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_transactions.EnableHeadersVisualStyles = false;
            this.dgv_transactions.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.dgv_transactions.Location = new System.Drawing.Point(65, 83);
            this.dgv_transactions.Name = "dgv_transactions";
            this.dgv_transactions.ReadOnly = true;
            this.dgv_transactions.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_transactions.RowHeadersVisible = false;
            this.dgv_transactions.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Courier Prime Code", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(104)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_transactions.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv_transactions.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgv_transactions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_transactions.Size = new System.Drawing.Size(296, 217);
            this.dgv_transactions.TabIndex = 17;
            this.dgv_transactions.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_transactions_DataBindingComplete);
            // 
            // lbl_noExpenses
            // 
            this.lbl_noExpenses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_noExpenses.BackColor = System.Drawing.Color.Transparent;
            this.lbl_noExpenses.Font = new System.Drawing.Font("Courier Prime Code", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_noExpenses.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_noExpenses.Location = new System.Drawing.Point(483, 47);
            this.lbl_noExpenses.MaximumSize = new System.Drawing.Size(184, 300);
            this.lbl_noExpenses.MinimumSize = new System.Drawing.Size(184, 300);
            this.lbl_noExpenses.Name = "lbl_noExpenses";
            this.lbl_noExpenses.Size = new System.Drawing.Size(184, 300);
            this.lbl_noExpenses.TabIndex = 34;
            this.lbl_noExpenses.Text = "Não há gastos no mês de";
            this.lbl_noExpenses.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chart_dailyExpenses
            // 
            this.chart_dailyExpenses.BackColor = System.Drawing.Color.Transparent;
            this.chart_dailyExpenses.BorderlineColor = System.Drawing.Color.Transparent;
            chartArea3.AxisX.InterlacedColor = System.Drawing.Color.Transparent;
            chartArea3.AxisX.IsLabelAutoFit = false;
            chartArea3.AxisX.LabelStyle.Font = new System.Drawing.Font("Courier Prime Code", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea3.AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            chartArea3.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            chartArea3.AxisX.MajorGrid.Enabled = false;
            chartArea3.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            chartArea3.AxisX.MajorTickMark.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            chartArea3.AxisX.ScaleBreakStyle.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            chartArea3.AxisX.ScaleBreakStyle.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea3.AxisX.ScrollBar.Enabled = false;
            chartArea3.AxisX.Title = "Dia";
            chartArea3.AxisX.TitleFont = new System.Drawing.Font("Courier Prime Code", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea3.AxisX.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            chartArea3.AxisY.InterlacedColor = System.Drawing.Color.Transparent;
            chartArea3.AxisY.IsLabelAutoFit = false;
            chartArea3.AxisY.LabelStyle.Font = new System.Drawing.Font("Courier Prime Code", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea3.AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            chartArea3.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            chartArea3.AxisY.MajorGrid.Enabled = false;
            chartArea3.AxisY.MajorTickMark.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            chartArea3.AxisY.Title = "(R$)";
            chartArea3.AxisY.TitleFont = new System.Drawing.Font("Courier Prime Code", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea3.AxisY.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            chartArea3.BackColor = System.Drawing.Color.Transparent;
            chartArea3.BorderColor = System.Drawing.Color.Transparent;
            chartArea3.Name = "ChartArea1";
            this.chart_dailyExpenses.ChartAreas.Add(chartArea3);
            legend3.Enabled = false;
            legend3.Name = "Legend1";
            this.chart_dailyExpenses.Legends.Add(legend3);
            this.chart_dailyExpenses.Location = new System.Drawing.Point(410, 40);
            this.chart_dailyExpenses.Name = "chart_dailyExpenses";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            series3.Legend = "Legend1";
            series3.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(104)))), ((int)(((byte)(232)))));
            series3.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(104)))), ((int)(((byte)(232)))));
            series3.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series3.Name = "Series1";
            this.chart_dailyExpenses.Series.Add(series3);
            this.chart_dailyExpenses.Size = new System.Drawing.Size(313, 316);
            this.chart_dailyExpenses.TabIndex = 33;
            this.chart_dailyExpenses.Text = "chart1";
            title3.Font = new System.Drawing.Font("Courier Prime Code", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            title3.Name = "Title1";
            title3.Text = "Gastos";
            this.chart_dailyExpenses.Titles.Add(title3);
            // 
            // pcb_updateIncomeOrExpense
            // 
            this.pcb_updateIncomeOrExpense.BackColor = System.Drawing.Color.Transparent;
            this.pcb_updateIncomeOrExpense.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pcb_updateIncomeOrExpense.BackgroundImage")));
            this.pcb_updateIncomeOrExpense.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcb_updateIncomeOrExpense.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_updateIncomeOrExpense.Location = new System.Drawing.Point(387, 286);
            this.pcb_updateIncomeOrExpense.Name = "pcb_updateIncomeOrExpense";
            this.pcb_updateIncomeOrExpense.Size = new System.Drawing.Size(32, 32);
            this.pcb_updateIncomeOrExpense.TabIndex = 35;
            this.pcb_updateIncomeOrExpense.TabStop = false;
            this.pcb_updateIncomeOrExpense.Click += new System.EventHandler(this.pcb_updateIncomeOrExpense_Click);
            // 
            // Transactions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(773, 381);
            this.Controls.Add(this.pcb_updateIncomeOrExpense);
            this.Controls.Add(this.lbl_noExpenses);
            this.Controls.Add(this.chart_dailyExpenses);
            this.Controls.Add(this.dgv_transactions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Transactions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transactions";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_transactions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_dailyExpenses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_updateIncomeOrExpense)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pcb_updateIncomeOrExpense;
        protected internal System.Windows.Forms.DataGridView dgv_transactions;
        protected internal System.Windows.Forms.DataVisualization.Charting.Chart chart_dailyExpenses;
        protected internal System.Windows.Forms.Label lbl_noExpenses;
    }
}