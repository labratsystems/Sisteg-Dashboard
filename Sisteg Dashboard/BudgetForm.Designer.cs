namespace Sisteg_Dashboard
{
    partial class BudgetForm
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
            backGroundTemp.Dispose();
            backGround.Dispose();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BudgetForm));
            this.panel_steps = new System.Windows.Forms.Panel();
            this.lbl_configTag = new System.Windows.Forms.Label();
            this.lbl_budgetTag = new System.Windows.Forms.Label();
            this.lbl_productTag = new System.Windows.Forms.Label();
            this.lbl_clientTag = new System.Windows.Forms.Label();
            this.lbl_mainTag = new System.Windows.Forms.Label();
            this.lbl_btnGoBackTag = new System.Windows.Forms.Label();
            this.lbl_btnEndBudgetTag = new System.Windows.Forms.Label();
            this.lbl_btnEditTag = new System.Windows.Forms.Label();
            this.lbl_btnGoForwardTag = new System.Windows.Forms.Label();
            this.pcb_minimizeProgram = new System.Windows.Forms.PictureBox();
            this.pcb_appClose = new System.Windows.Forms.PictureBox();
            this.pcb_btnEndBudget = new System.Windows.Forms.PictureBox();
            this.pcb_btnEdit = new System.Windows.Forms.PictureBox();
            this.pcb_btnGoForward = new System.Windows.Forms.PictureBox();
            this.pcb_btnGoBack = new System.Windows.Forms.PictureBox();
            this.pcb_btnConfig = new System.Windows.Forms.PictureBox();
            this.pcb_btnBudget = new System.Windows.Forms.PictureBox();
            this.pcb_btnProduct = new System.Windows.Forms.PictureBox();
            this.pcb_btnMain = new System.Windows.Forms.PictureBox();
            this.pcb_btnClient = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_minimizeProgram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_appClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnEndBudget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnGoForward)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnGoBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnBudget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnProduct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnClient)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_steps
            // 
            this.panel_steps.BackColor = System.Drawing.Color.Transparent;
            this.panel_steps.Location = new System.Drawing.Point(400, 30);
            this.panel_steps.Name = "panel_steps";
            this.panel_steps.Size = new System.Drawing.Size(773, 381);
            this.panel_steps.TabIndex = 6;
            // 
            // lbl_configTag
            // 
            this.lbl_configTag.AutoSize = true;
            this.lbl_configTag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_configTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_configTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_configTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_configTag.Location = new System.Drawing.Point(104, 367);
            this.lbl_configTag.Name = "lbl_configTag";
            this.lbl_configTag.Size = new System.Drawing.Size(163, 20);
            this.lbl_configTag.TabIndex = 5;
            this.lbl_configTag.Text = "CONFIGURAÇÕES///";
            this.lbl_configTag.Click += new System.EventHandler(this.lbl_configTag_Click);
            // 
            // lbl_budgetTag
            // 
            this.lbl_budgetTag.AutoSize = true;
            this.lbl_budgetTag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_budgetTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_budgetTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_budgetTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_budgetTag.Location = new System.Drawing.Point(104, 291);
            this.lbl_budgetTag.Name = "lbl_budgetTag";
            this.lbl_budgetTag.Size = new System.Drawing.Size(134, 20);
            this.lbl_budgetTag.TabIndex = 4;
            this.lbl_budgetTag.Text = "ORÇAMENTOS///";
            // 
            // lbl_productTag
            // 
            this.lbl_productTag.AutoSize = true;
            this.lbl_productTag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_productTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_productTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_productTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_productTag.Location = new System.Drawing.Point(104, 215);
            this.lbl_productTag.Name = "lbl_productTag";
            this.lbl_productTag.Size = new System.Drawing.Size(111, 20);
            this.lbl_productTag.TabIndex = 3;
            this.lbl_productTag.Text = "PRODUTOS///";
            this.lbl_productTag.Click += new System.EventHandler(this.lbl_productTag_Click);
            // 
            // lbl_clientTag
            // 
            this.lbl_clientTag.AutoSize = true;
            this.lbl_clientTag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_clientTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_clientTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_clientTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_clientTag.Location = new System.Drawing.Point(104, 139);
            this.lbl_clientTag.Name = "lbl_clientTag";
            this.lbl_clientTag.Size = new System.Drawing.Size(99, 20);
            this.lbl_clientTag.TabIndex = 2;
            this.lbl_clientTag.Text = "CLIENTES///";
            this.lbl_clientTag.Click += new System.EventHandler(this.lbl_clientTag_Click);
            // 
            // lbl_mainTag
            // 
            this.lbl_mainTag.AutoSize = true;
            this.lbl_mainTag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_mainTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_mainTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_mainTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_mainTag.Location = new System.Drawing.Point(104, 63);
            this.lbl_mainTag.Name = "lbl_mainTag";
            this.lbl_mainTag.Size = new System.Drawing.Size(166, 20);
            this.lbl_mainTag.TabIndex = 1;
            this.lbl_mainTag.Text = "PAINEL PRINCIPAL///";
            this.lbl_mainTag.Click += new System.EventHandler(this.lbl_mainTag_Click);
            // 
            // lbl_btnGoBackTag
            // 
            this.lbl_btnGoBackTag.AutoSize = true;
            this.lbl_btnGoBackTag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_btnGoBackTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_btnGoBackTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_btnGoBackTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_btnGoBackTag.Location = new System.Drawing.Point(480, 445);
            this.lbl_btnGoBackTag.Name = "lbl_btnGoBackTag";
            this.lbl_btnGoBackTag.Size = new System.Drawing.Size(85, 20);
            this.lbl_btnGoBackTag.TabIndex = 7;
            this.lbl_btnGoBackTag.Text = "VOLTAR///";
            this.lbl_btnGoBackTag.Click += new System.EventHandler(this.lbl_btnGoBackTag_Click);
            // 
            // lbl_btnEndBudgetTag
            // 
            this.lbl_btnEndBudgetTag.AutoSize = true;
            this.lbl_btnEndBudgetTag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_btnEndBudgetTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_btnEndBudgetTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_btnEndBudgetTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_btnEndBudgetTag.Location = new System.Drawing.Point(925, 597);
            this.lbl_btnEndBudgetTag.Name = "lbl_btnEndBudgetTag";
            this.lbl_btnEndBudgetTag.Size = new System.Drawing.Size(211, 20);
            this.lbl_btnEndBudgetTag.TabIndex = 10;
            this.lbl_btnEndBudgetTag.Text = "FINALIZAR ORÇAMENTO///";
            this.lbl_btnEndBudgetTag.Click += new System.EventHandler(this.lbl_btnEndBudgetTag_Click);
            // 
            // lbl_btnEditTag
            // 
            this.lbl_btnEditTag.AutoSize = true;
            this.lbl_btnEditTag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_btnEditTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_btnEditTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_btnEditTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_btnEditTag.Location = new System.Drawing.Point(925, 521);
            this.lbl_btnEditTag.Name = "lbl_btnEditTag";
            this.lbl_btnEditTag.Size = new System.Drawing.Size(187, 20);
            this.lbl_btnEditTag.TabIndex = 9;
            this.lbl_btnEditTag.Text = "EDITAR ORÇAMENTO///";
            this.lbl_btnEditTag.Click += new System.EventHandler(this.lbl_btnEditTag_Click);
            // 
            // lbl_btnGoForwardTag
            // 
            this.lbl_btnGoForwardTag.AutoSize = true;
            this.lbl_btnGoForwardTag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_btnGoForwardTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_btnGoForwardTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_btnGoForwardTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_btnGoForwardTag.Location = new System.Drawing.Point(925, 445);
            this.lbl_btnGoForwardTag.Name = "lbl_btnGoForwardTag";
            this.lbl_btnGoForwardTag.Size = new System.Drawing.Size(99, 20);
            this.lbl_btnGoForwardTag.TabIndex = 8;
            this.lbl_btnGoForwardTag.Text = "AVANÇAR///";
            this.lbl_btnGoForwardTag.Click += new System.EventHandler(this.lbl_btnGoForwardTag_Click);
            // 
            // pcb_minimizeProgram
            // 
            this.pcb_minimizeProgram.BackColor = System.Drawing.Color.Transparent;
            this.pcb_minimizeProgram.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.minus;
            this.pcb_minimizeProgram.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcb_minimizeProgram.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_minimizeProgram.Location = new System.Drawing.Point(1002, 8);
            this.pcb_minimizeProgram.Name = "pcb_minimizeProgram";
            this.pcb_minimizeProgram.Size = new System.Drawing.Size(32, 32);
            this.pcb_minimizeProgram.TabIndex = 82;
            this.pcb_minimizeProgram.TabStop = false;
            this.pcb_minimizeProgram.Click += new System.EventHandler(this.pcb_minimizeProgram_Click);
            // 
            // pcb_appClose
            // 
            this.pcb_appClose.BackColor = System.Drawing.Color.Transparent;
            this.pcb_appClose.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.cancel__2_;
            this.pcb_appClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcb_appClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_appClose.Location = new System.Drawing.Point(964, 8);
            this.pcb_appClose.Name = "pcb_appClose";
            this.pcb_appClose.Size = new System.Drawing.Size(32, 32);
            this.pcb_appClose.TabIndex = 81;
            this.pcb_appClose.TabStop = false;
            this.pcb_appClose.Click += new System.EventHandler(this.pcb_appClose_Click);
            // 
            // pcb_btnEndBudget
            // 
            this.pcb_btnEndBudget.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnEndBudget.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.btn_end_budget;
            this.pcb_btnEndBudget.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnEndBudget.Location = new System.Drawing.Point(845, 569);
            this.pcb_btnEndBudget.Name = "pcb_btnEndBudget";
            this.pcb_btnEndBudget.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnEndBudget.TabIndex = 36;
            this.pcb_btnEndBudget.TabStop = false;
            this.pcb_btnEndBudget.Click += new System.EventHandler(this.pcb_btnEndBudget_Click);
            this.pcb_btnEndBudget.MouseEnter += new System.EventHandler(this.pcb_btnEndBudget_MouseEnter);
            this.pcb_btnEndBudget.MouseLeave += new System.EventHandler(this.pcb_btnEndBudget_MouseLeave);
            // 
            // pcb_btnEdit
            // 
            this.pcb_btnEdit.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnEdit.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.btn_edit;
            this.pcb_btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnEdit.Location = new System.Drawing.Point(845, 493);
            this.pcb_btnEdit.Name = "pcb_btnEdit";
            this.pcb_btnEdit.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnEdit.TabIndex = 35;
            this.pcb_btnEdit.TabStop = false;
            this.pcb_btnEdit.Click += new System.EventHandler(this.pcb_btnEdit_Click);
            this.pcb_btnEdit.MouseEnter += new System.EventHandler(this.pcb_btnEdit_MouseEnter);
            this.pcb_btnEdit.MouseLeave += new System.EventHandler(this.pcb_btnEdit_MouseLeave);
            // 
            // pcb_btnGoForward
            // 
            this.pcb_btnGoForward.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnGoForward.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.btn_go_forward;
            this.pcb_btnGoForward.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnGoForward.Location = new System.Drawing.Point(845, 417);
            this.pcb_btnGoForward.Name = "pcb_btnGoForward";
            this.pcb_btnGoForward.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnGoForward.TabIndex = 34;
            this.pcb_btnGoForward.TabStop = false;
            this.pcb_btnGoForward.Click += new System.EventHandler(this.pcb_btnGoForward_Click);
            this.pcb_btnGoForward.MouseEnter += new System.EventHandler(this.pcb_btnGoForward_MouseEnter);
            this.pcb_btnGoForward.MouseLeave += new System.EventHandler(this.pcb_btnGoForward_MouseLeave);
            // 
            // pcb_btnGoBack
            // 
            this.pcb_btnGoBack.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnGoBack.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.btn_go_back;
            this.pcb_btnGoBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnGoBack.Location = new System.Drawing.Point(400, 417);
            this.pcb_btnGoBack.Name = "pcb_btnGoBack";
            this.pcb_btnGoBack.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnGoBack.TabIndex = 33;
            this.pcb_btnGoBack.TabStop = false;
            this.pcb_btnGoBack.Click += new System.EventHandler(this.pcb_btnGoBack_Click);
            this.pcb_btnGoBack.MouseEnter += new System.EventHandler(this.pcb_btnGoBack_MouseEnter);
            this.pcb_btnGoBack.MouseLeave += new System.EventHandler(this.pcb_btnGoBack_MouseLeave);
            // 
            // pcb_btnConfig
            // 
            this.pcb_btnConfig.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnConfig.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.btn_config_main;
            this.pcb_btnConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnConfig.Location = new System.Drawing.Point(22, 339);
            this.pcb_btnConfig.Name = "pcb_btnConfig";
            this.pcb_btnConfig.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnConfig.TabIndex = 31;
            this.pcb_btnConfig.TabStop = false;
            this.pcb_btnConfig.Click += new System.EventHandler(this.pcb_btnConfig_Click);
            this.pcb_btnConfig.MouseEnter += new System.EventHandler(this.pcb_btnConfig_MouseEnter);
            this.pcb_btnConfig.MouseLeave += new System.EventHandler(this.pcb_btnConfig_MouseLeave);
            // 
            // pcb_btnBudget
            // 
            this.pcb_btnBudget.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnBudget.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.btn_budget_form_active;
            this.pcb_btnBudget.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnBudget.Location = new System.Drawing.Point(22, 263);
            this.pcb_btnBudget.Name = "pcb_btnBudget";
            this.pcb_btnBudget.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnBudget.TabIndex = 29;
            this.pcb_btnBudget.TabStop = false;
            // 
            // pcb_btnProduct
            // 
            this.pcb_btnProduct.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnProduct.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.btn_product_form;
            this.pcb_btnProduct.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnProduct.Location = new System.Drawing.Point(22, 187);
            this.pcb_btnProduct.Name = "pcb_btnProduct";
            this.pcb_btnProduct.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnProduct.TabIndex = 28;
            this.pcb_btnProduct.TabStop = false;
            this.pcb_btnProduct.Click += new System.EventHandler(this.pcb_btnProduct_Click);
            this.pcb_btnProduct.MouseEnter += new System.EventHandler(this.pcb_btnProduct_MouseEnter);
            this.pcb_btnProduct.MouseLeave += new System.EventHandler(this.pcb_btnProduct_MouseLeave);
            // 
            // pcb_btnMain
            // 
            this.pcb_btnMain.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnMain.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.btn_main_form;
            this.pcb_btnMain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnMain.Location = new System.Drawing.Point(22, 35);
            this.pcb_btnMain.Name = "pcb_btnMain";
            this.pcb_btnMain.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnMain.TabIndex = 27;
            this.pcb_btnMain.TabStop = false;
            this.pcb_btnMain.Click += new System.EventHandler(this.pcb_btnMain_Click);
            this.pcb_btnMain.MouseEnter += new System.EventHandler(this.pcb_btnMain_MouseEnter);
            this.pcb_btnMain.MouseLeave += new System.EventHandler(this.pcb_btnMain_MouseLeave);
            // 
            // pcb_btnClient
            // 
            this.pcb_btnClient.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnClient.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.btn_client_form;
            this.pcb_btnClient.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnClient.Location = new System.Drawing.Point(22, 111);
            this.pcb_btnClient.Name = "pcb_btnClient";
            this.pcb_btnClient.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnClient.TabIndex = 26;
            this.pcb_btnClient.TabStop = false;
            this.pcb_btnClient.Click += new System.EventHandler(this.pcb_btnClient_Click);
            this.pcb_btnClient.MouseEnter += new System.EventHandler(this.pcb_btnClient_MouseEnter);
            this.pcb_btnClient.MouseLeave += new System.EventHandler(this.pcb_btnClient_MouseLeave);
            // 
            // BudgetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1200, 675);
            this.Controls.Add(this.pcb_minimizeProgram);
            this.Controls.Add(this.pcb_appClose);
            this.Controls.Add(this.lbl_btnEndBudgetTag);
            this.Controls.Add(this.lbl_btnEditTag);
            this.Controls.Add(this.lbl_btnGoForwardTag);
            this.Controls.Add(this.lbl_btnGoBackTag);
            this.Controls.Add(this.lbl_configTag);
            this.Controls.Add(this.lbl_budgetTag);
            this.Controls.Add(this.lbl_productTag);
            this.Controls.Add(this.lbl_clientTag);
            this.Controls.Add(this.lbl_mainTag);
            this.Controls.Add(this.pcb_btnEndBudget);
            this.Controls.Add(this.pcb_btnEdit);
            this.Controls.Add(this.pcb_btnGoForward);
            this.Controls.Add(this.pcb_btnGoBack);
            this.Controls.Add(this.panel_steps);
            this.Controls.Add(this.pcb_btnConfig);
            this.Controls.Add(this.pcb_btnBudget);
            this.Controls.Add(this.pcb_btnProduct);
            this.Controls.Add(this.pcb_btnMain);
            this.Controls.Add(this.pcb_btnClient);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BudgetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Orçamentos";
            ((System.ComponentModel.ISupportInitialize)(this.pcb_minimizeProgram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_appClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnEndBudget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnGoForward)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnGoBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnBudget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnProduct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnClient)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pcb_btnMain;
        private System.Windows.Forms.PictureBox pcb_btnProduct;
        private System.Windows.Forms.PictureBox pcb_btnBudget;
        private System.Windows.Forms.PictureBox pcb_btnConfig;
        private System.Windows.Forms.PictureBox pcb_btnClient;
        public System.Windows.Forms.Panel panel_steps;
        protected internal System.Windows.Forms.PictureBox pcb_btnEdit;
        protected internal System.Windows.Forms.PictureBox pcb_btnGoForward;
        protected internal System.Windows.Forms.PictureBox pcb_btnGoBack;
        private System.Windows.Forms.PictureBox pcb_btnEndBudget;
        protected internal System.Windows.Forms.Label lbl_configTag;
        protected internal System.Windows.Forms.Label lbl_budgetTag;
        protected internal System.Windows.Forms.Label lbl_productTag;
        protected internal System.Windows.Forms.Label lbl_clientTag;
        protected internal System.Windows.Forms.Label lbl_mainTag;
        protected internal System.Windows.Forms.Label lbl_btnGoBackTag;
        protected internal System.Windows.Forms.Label lbl_btnEndBudgetTag;
        protected internal System.Windows.Forms.Label lbl_btnEditTag;
        protected internal System.Windows.Forms.Label lbl_btnGoForwardTag;
        private System.Windows.Forms.PictureBox pcb_minimizeProgram;
        private System.Windows.Forms.PictureBox pcb_appClose;
    }
}