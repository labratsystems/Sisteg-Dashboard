namespace Sisteg_Dashboard
{
    partial class ExpenseGraphs
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExpenseGraphs));
            this.chart_expenseCategory = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_expenseAccount = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart_expenseCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_expenseAccount)).BeginInit();
            this.SuspendLayout();
            // 
            // chart_expenseCategory
            // 
            this.chart_expenseCategory.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chart_expenseCategory.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend2";
            this.chart_expenseCategory.Legends.Add(legend1);
            this.chart_expenseCategory.Location = new System.Drawing.Point(62, 70);
            this.chart_expenseCategory.Name = "chart_expenseCategory";
            series1.BorderColor = System.Drawing.Color.Transparent;
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Color = System.Drawing.Color.Transparent;
            series1.Font = new System.Drawing.Font("Courier Prime Code", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.LabelForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(104)))), ((int)(((byte)(82)))));
            series1.Legend = "Legend2";
            series1.Name = "Series1";
            this.chart_expenseCategory.Series.Add(series1);
            this.chart_expenseCategory.Size = new System.Drawing.Size(277, 241);
            this.chart_expenseCategory.TabIndex = 18;
            this.chart_expenseCategory.Text = "chart1";
            // 
            // chart_expenseAccount
            // 
            this.chart_expenseAccount.BackColor = System.Drawing.Color.Transparent;
            chartArea2.BackColor = System.Drawing.Color.Transparent;
            chartArea2.Name = "ChartArea1";
            this.chart_expenseAccount.ChartAreas.Add(chartArea2);
            legend2.Enabled = false;
            legend2.Name = "Legend2";
            this.chart_expenseAccount.Legends.Add(legend2);
            this.chart_expenseAccount.Location = new System.Drawing.Point(435, 70);
            this.chart_expenseAccount.Name = "chart_expenseAccount";
            series2.BorderColor = System.Drawing.Color.Transparent;
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series2.Color = System.Drawing.Color.Transparent;
            series2.Font = new System.Drawing.Font("Courier Prime Code", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series2.LabelForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(104)))), ((int)(((byte)(82)))));
            series2.Legend = "Legend2";
            series2.Name = "Series1";
            this.chart_expenseAccount.Series.Add(series2);
            this.chart_expenseAccount.Size = new System.Drawing.Size(277, 241);
            this.chart_expenseAccount.TabIndex = 21;
            this.chart_expenseAccount.Text = "chart1";
            // 
            // ExpenseGraphs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(773, 381);
            this.Controls.Add(this.chart_expenseAccount);
            this.Controls.Add(this.chart_expenseCategory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ExpenseGraphs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "expenseGraphs";
            ((System.ComponentModel.ISupportInitialize)(this.chart_expenseCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_expenseAccount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected internal System.Windows.Forms.DataVisualization.Charting.Chart chart_expenseCategory;
        protected internal System.Windows.Forms.DataVisualization.Charting.Chart chart_expenseAccount;
    }
}