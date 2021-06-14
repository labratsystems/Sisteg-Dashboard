namespace Sisteg_Dashboard
{
    partial class IncomeGraphs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IncomeGraphs));
            this.chart_incomeCategory = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_incomeAccount = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart_incomeCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_incomeAccount)).BeginInit();
            this.SuspendLayout();
            // 
            // chart_incomeCategory
            // 
            this.chart_incomeCategory.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chart_incomeCategory.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend2";
            this.chart_incomeCategory.Legends.Add(legend1);
            this.chart_incomeCategory.Location = new System.Drawing.Point(61, 70);
            this.chart_incomeCategory.Name = "chart_incomeCategory";
            series1.BorderColor = System.Drawing.Color.Transparent;
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Color = System.Drawing.Color.Transparent;
            series1.Font = new System.Drawing.Font("Courier Prime Code", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.LabelForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            series1.Legend = "Legend2";
            series1.Name = "Series1";
            this.chart_incomeCategory.Series.Add(series1);
            this.chart_incomeCategory.Size = new System.Drawing.Size(277, 241);
            this.chart_incomeCategory.TabIndex = 1;
            // 
            // chart_incomeAccount
            // 
            this.chart_incomeAccount.BackColor = System.Drawing.Color.Transparent;
            chartArea2.BackColor = System.Drawing.Color.Transparent;
            chartArea2.Name = "ChartArea1";
            this.chart_incomeAccount.ChartAreas.Add(chartArea2);
            legend2.Enabled = false;
            legend2.Name = "Legend2";
            this.chart_incomeAccount.Legends.Add(legend2);
            this.chart_incomeAccount.Location = new System.Drawing.Point(434, 70);
            this.chart_incomeAccount.Name = "chart_incomeAccount";
            series2.BorderColor = System.Drawing.Color.Transparent;
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series2.Color = System.Drawing.Color.Transparent;
            series2.Font = new System.Drawing.Font("Courier Prime Code", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series2.LabelForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            series2.Legend = "Legend2";
            series2.Name = "Series1";
            this.chart_incomeAccount.Series.Add(series2);
            this.chart_incomeAccount.Size = new System.Drawing.Size(277, 241);
            this.chart_incomeAccount.TabIndex = 2;
            // 
            // IncomeGraphs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(773, 381);
            this.Controls.Add(this.chart_incomeCategory);
            this.Controls.Add(this.chart_incomeAccount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "IncomeGraphs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "incomeGraphs";
            ((System.ComponentModel.ISupportInitialize)(this.chart_incomeCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_incomeAccount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected internal System.Windows.Forms.DataVisualization.Charting.Chart chart_incomeCategory;
        protected internal System.Windows.Forms.DataVisualization.Charting.Chart chart_incomeAccount;
    }
}