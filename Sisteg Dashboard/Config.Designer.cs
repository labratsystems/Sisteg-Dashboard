namespace Sisteg_Dashboard
{
    partial class Config
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
            this.pcb_btnConfig = new System.Windows.Forms.PictureBox();
            this.pcb_btnBudget = new System.Windows.Forms.PictureBox();
            this.pcb_btnProduct = new System.Windows.Forms.PictureBox();
            this.pcb_btnMain = new System.Windows.Forms.PictureBox();
            this.pcb_btnClient = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnBudget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnProduct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnClient)).BeginInit();
            this.SuspendLayout();
            // 
            // pcb_btnConfig
            // 
            this.pcb_btnConfig.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnConfig.Image = global::Sisteg_Dashboard.Properties.Resources.btn_config_active;
            this.pcb_btnConfig.Location = new System.Drawing.Point(37, 316);
            this.pcb_btnConfig.Name = "pcb_btnConfig";
            this.pcb_btnConfig.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnConfig.TabIndex = 43;
            this.pcb_btnConfig.TabStop = false;
            // 
            // pcb_btnBudget
            // 
            this.pcb_btnBudget.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnBudget.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnBudget.Image = global::Sisteg_Dashboard.Properties.Resources.btn_budget;
            this.pcb_btnBudget.Location = new System.Drawing.Point(37, 240);
            this.pcb_btnBudget.Name = "pcb_btnBudget";
            this.pcb_btnBudget.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnBudget.TabIndex = 41;
            this.pcb_btnBudget.TabStop = false;
            this.pcb_btnBudget.Click += new System.EventHandler(this.pcb_btnBudget_Click);
            this.pcb_btnBudget.MouseEnter += new System.EventHandler(this.pcb_btnBudget_MouseEnter);
            this.pcb_btnBudget.MouseLeave += new System.EventHandler(this.pcb_btnBudget_MouseLeave);
            // 
            // pcb_btnProduct
            // 
            this.pcb_btnProduct.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnProduct.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnProduct.Image = global::Sisteg_Dashboard.Properties.Resources.btn_product;
            this.pcb_btnProduct.Location = new System.Drawing.Point(37, 164);
            this.pcb_btnProduct.Name = "pcb_btnProduct";
            this.pcb_btnProduct.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnProduct.TabIndex = 40;
            this.pcb_btnProduct.TabStop = false;
            this.pcb_btnProduct.Click += new System.EventHandler(this.pcb_btnProduct_Click);
            this.pcb_btnProduct.MouseEnter += new System.EventHandler(this.pcb_btnProduct_MouseEnter);
            this.pcb_btnProduct.MouseLeave += new System.EventHandler(this.pcb_btnProduct_MouseLeave);
            // 
            // pcb_btnMain
            // 
            this.pcb_btnMain.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnMain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnMain.Image = global::Sisteg_Dashboard.Properties.Resources.btn_main;
            this.pcb_btnMain.Location = new System.Drawing.Point(37, 12);
            this.pcb_btnMain.Name = "pcb_btnMain";
            this.pcb_btnMain.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnMain.TabIndex = 39;
            this.pcb_btnMain.TabStop = false;
            this.pcb_btnMain.Click += new System.EventHandler(this.pcb_btnMain_Click);
            this.pcb_btnMain.MouseEnter += new System.EventHandler(this.pcb_btnMain_MouseEnter);
            this.pcb_btnMain.MouseLeave += new System.EventHandler(this.pcb_btnMain_MouseLeave);
            // 
            // pcb_btnClient
            // 
            this.pcb_btnClient.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnClient.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnClient.Image = global::Sisteg_Dashboard.Properties.Resources.btn_client;
            this.pcb_btnClient.Location = new System.Drawing.Point(37, 88);
            this.pcb_btnClient.Name = "pcb_btnClient";
            this.pcb_btnClient.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnClient.TabIndex = 38;
            this.pcb_btnClient.TabStop = false;
            this.pcb_btnClient.Click += new System.EventHandler(this.pcb_btnClient_Click);
            this.pcb_btnClient.MouseEnter += new System.EventHandler(this.pcb_btnClient_MouseEnter);
            this.pcb_btnClient.MouseLeave += new System.EventHandler(this.pcb_btnClient_MouseLeave);
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 675);
            this.Controls.Add(this.pcb_btnConfig);
            this.Controls.Add(this.pcb_btnBudget);
            this.Controls.Add(this.pcb_btnProduct);
            this.Controls.Add(this.pcb_btnMain);
            this.Controls.Add(this.pcb_btnClient);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Config";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Config";
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnBudget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnProduct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnClient)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pcb_btnConfig;
        private System.Windows.Forms.PictureBox pcb_btnBudget;
        private System.Windows.Forms.PictureBox pcb_btnProduct;
        private System.Windows.Forms.PictureBox pcb_btnMain;
        private System.Windows.Forms.PictureBox pcb_btnClient;
    }
}