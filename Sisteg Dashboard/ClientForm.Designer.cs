namespace Sisteg_Dashboard
{
    partial class ClientForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientForm));
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties5 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties6 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties7 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties8 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            this.pcb_btnConfig = new System.Windows.Forms.PictureBox();
            this.pcb_btnBudget = new System.Windows.Forms.PictureBox();
            this.pcb_btnProduct = new System.Windows.Forms.PictureBox();
            this.pcb_btnMain = new System.Windows.Forms.PictureBox();
            this.pcb_btnClient = new System.Windows.Forms.PictureBox();
            this.dgv_clients = new System.Windows.Forms.DataGridView();
            this.txt_searchClient = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this.lbl_parcelValue = new System.Windows.Forms.Label();
            this.lbl_fixedValue = new System.Windows.Forms.Label();
            this.rbtn_parcelValue = new Bunifu.UI.WinForms.BunifuRadioButton();
            this.rbtn_fixedValue = new Bunifu.UI.WinForms.BunifuRadioButton();
            this.pcb_btnEdit = new System.Windows.Forms.PictureBox();
            this.pcb_btnAdd = new System.Windows.Forms.PictureBox();
            this.pcb_btnPay = new System.Windows.Forms.PictureBox();
            this.pcb_appClose = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnBudget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnProduct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_clients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnPay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_appClose)).BeginInit();
            this.SuspendLayout();
            // 
            // pcb_btnConfig
            // 
            this.pcb_btnConfig.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnConfig.Image = global::Sisteg_Dashboard.Properties.Resources.btn_config;
            this.pcb_btnConfig.Location = new System.Drawing.Point(22, 339);
            this.pcb_btnConfig.Name = "pcb_btnConfig";
            this.pcb_btnConfig.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnConfig.TabIndex = 19;
            this.pcb_btnConfig.TabStop = false;
            this.pcb_btnConfig.Click += new System.EventHandler(this.pcb_btnConfig_Click);
            this.pcb_btnConfig.MouseEnter += new System.EventHandler(this.pcb_btnConfig_MouseEnter);
            this.pcb_btnConfig.MouseLeave += new System.EventHandler(this.pcb_btnConfig_MouseLeave);
            // 
            // pcb_btnBudget
            // 
            this.pcb_btnBudget.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnBudget.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnBudget.Image = global::Sisteg_Dashboard.Properties.Resources.btn_budget;
            this.pcb_btnBudget.Location = new System.Drawing.Point(22, 263);
            this.pcb_btnBudget.Name = "pcb_btnBudget";
            this.pcb_btnBudget.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnBudget.TabIndex = 17;
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
            this.pcb_btnProduct.Location = new System.Drawing.Point(22, 187);
            this.pcb_btnProduct.Name = "pcb_btnProduct";
            this.pcb_btnProduct.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnProduct.TabIndex = 16;
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
            this.pcb_btnMain.Location = new System.Drawing.Point(22, 35);
            this.pcb_btnMain.Name = "pcb_btnMain";
            this.pcb_btnMain.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnMain.TabIndex = 15;
            this.pcb_btnMain.TabStop = false;
            this.pcb_btnMain.Click += new System.EventHandler(this.pcb_btnMain_Click);
            this.pcb_btnMain.MouseEnter += new System.EventHandler(this.pcb_btnMain_MouseEnter);
            this.pcb_btnMain.MouseLeave += new System.EventHandler(this.pcb_btnMain_MouseLeave);
            // 
            // pcb_btnClient
            // 
            this.pcb_btnClient.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnClient.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnClient.Image = global::Sisteg_Dashboard.Properties.Resources.btn_client_active;
            this.pcb_btnClient.Location = new System.Drawing.Point(22, 111);
            this.pcb_btnClient.Name = "pcb_btnClient";
            this.pcb_btnClient.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnClient.TabIndex = 14;
            this.pcb_btnClient.TabStop = false;
            // 
            // dgv_clients
            // 
            this.dgv_clients.AllowUserToAddRows = false;
            this.dgv_clients.AllowUserToDeleteRows = false;
            this.dgv_clients.AllowUserToResizeColumns = false;
            this.dgv_clients.AllowUserToResizeRows = false;
            this.dgv_clients.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_clients.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.dgv_clients.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_clients.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgv_clients.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Courier Prime Code", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_clients.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_clients.ColumnHeadersHeight = 30;
            this.dgv_clients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_clients.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dgv_clients.EnableHeadersVisualStyles = false;
            this.dgv_clients.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.dgv_clients.Location = new System.Drawing.Point(426, 206);
            this.dgv_clients.Name = "dgv_clients";
            this.dgv_clients.ReadOnly = true;
            this.dgv_clients.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_clients.RowHeadersVisible = false;
            this.dgv_clients.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Courier Prime Code", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(104)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_clients.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_clients.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgv_clients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_clients.Size = new System.Drawing.Size(714, 167);
            this.dgv_clients.TabIndex = 20;
            this.dgv_clients.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_clients_DataBindingComplete);
            // 
            // txt_searchClient
            // 
            this.txt_searchClient.AcceptsReturn = false;
            this.txt_searchClient.AcceptsTab = false;
            this.txt_searchClient.AnimationSpeed = 200;
            this.txt_searchClient.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txt_searchClient.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txt_searchClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.txt_searchClient.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txt_searchClient.BackgroundImage")));
            this.txt_searchClient.BorderColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_searchClient.BorderColorDisabled = System.Drawing.Color.Gray;
            this.txt_searchClient.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_searchClient.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.txt_searchClient.BorderRadius = 30;
            this.txt_searchClient.BorderThickness = 1;
            this.txt_searchClient.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txt_searchClient.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_searchClient.DefaultFont = new System.Drawing.Font("Courier Prime Code", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_searchClient.DefaultText = "";
            this.txt_searchClient.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.txt_searchClient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_searchClient.HideSelection = true;
            this.txt_searchClient.IconLeft = null;
            this.txt_searchClient.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_searchClient.IconPadding = 10;
            this.txt_searchClient.IconRight = null;
            this.txt_searchClient.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_searchClient.Lines = new string[0];
            this.txt_searchClient.Location = new System.Drawing.Point(914, 122);
            this.txt_searchClient.MaxLength = 32767;
            this.txt_searchClient.MinimumSize = new System.Drawing.Size(100, 35);
            this.txt_searchClient.Modified = false;
            this.txt_searchClient.Multiline = false;
            this.txt_searchClient.Name = "txt_searchClient";
            stateProperties5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties5.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            stateProperties5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties5.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_searchClient.OnActiveState = stateProperties5;
            stateProperties6.BorderColor = System.Drawing.Color.Gray;
            stateProperties6.FillColor = System.Drawing.Color.White;
            stateProperties6.ForeColor = System.Drawing.Color.Empty;
            stateProperties6.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txt_searchClient.OnDisabledState = stateProperties6;
            stateProperties7.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties7.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            stateProperties7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties7.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_searchClient.OnHoverState = stateProperties7;
            stateProperties8.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            stateProperties8.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            stateProperties8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties8.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_searchClient.OnIdleState = stateProperties8;
            this.txt_searchClient.PasswordChar = '\0';
            this.txt_searchClient.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_searchClient.PlaceholderText = "Nome do cliente";
            this.txt_searchClient.ReadOnly = false;
            this.txt_searchClient.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txt_searchClient.SelectedText = "";
            this.txt_searchClient.SelectionLength = 0;
            this.txt_searchClient.SelectionStart = 0;
            this.txt_searchClient.ShortcutsEnabled = true;
            this.txt_searchClient.Size = new System.Drawing.Size(200, 35);
            this.txt_searchClient.Style = Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox._Style.Material;
            this.txt_searchClient.TabIndex = 45;
            this.txt_searchClient.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txt_searchClient.TextMarginBottom = 0;
            this.txt_searchClient.TextMarginLeft = -5;
            this.txt_searchClient.TextMarginTop = 1;
            this.txt_searchClient.TextPlaceholder = "Nome do cliente";
            this.txt_searchClient.UseSystemPasswordChar = false;
            this.txt_searchClient.WordWrap = true;
            this.txt_searchClient.TextChange += new System.EventHandler(this.txt_searchClient_TextChange);
            // 
            // lbl_parcelValue
            // 
            this.lbl_parcelValue.AutoSize = true;
            this.lbl_parcelValue.BackColor = System.Drawing.Color.Transparent;
            this.lbl_parcelValue.Font = new System.Drawing.Font("Courier Prime Code", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_parcelValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_parcelValue.Location = new System.Drawing.Point(484, 136);
            this.lbl_parcelValue.Name = "lbl_parcelValue";
            this.lbl_parcelValue.Size = new System.Drawing.Size(190, 21);
            this.lbl_parcelValue.TabIndex = 44;
            this.lbl_parcelValue.Text = "Clientes em dívida";
            // 
            // lbl_fixedValue
            // 
            this.lbl_fixedValue.AutoSize = true;
            this.lbl_fixedValue.BackColor = System.Drawing.Color.Transparent;
            this.lbl_fixedValue.Font = new System.Drawing.Font("Courier Prime Code", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_fixedValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_fixedValue.Location = new System.Drawing.Point(484, 96);
            this.lbl_fixedValue.Name = "lbl_fixedValue";
            this.lbl_fixedValue.Size = new System.Drawing.Size(180, 21);
            this.lbl_fixedValue.TabIndex = 43;
            this.lbl_fixedValue.Text = "Todos os clientes";
            // 
            // rbtn_parcelValue
            // 
            this.rbtn_parcelValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.rbtn_parcelValue.Checked = false;
            this.rbtn_parcelValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbtn_parcelValue.Location = new System.Drawing.Point(444, 132);
            this.rbtn_parcelValue.Name = "rbtn_parcelValue";
            this.rbtn_parcelValue.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_parcelValue.RadioColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_parcelValue.Size = new System.Drawing.Size(25, 25);
            this.rbtn_parcelValue.TabIndex = 42;
            this.rbtn_parcelValue.Text = null;
            // 
            // rbtn_fixedValue
            // 
            this.rbtn_fixedValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.rbtn_fixedValue.Checked = true;
            this.rbtn_fixedValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbtn_fixedValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_fixedValue.Location = new System.Drawing.Point(444, 92);
            this.rbtn_fixedValue.Name = "rbtn_fixedValue";
            this.rbtn_fixedValue.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_fixedValue.RadioColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_fixedValue.Size = new System.Drawing.Size(25, 25);
            this.rbtn_fixedValue.TabIndex = 41;
            this.rbtn_fixedValue.Text = null;
            // 
            // pcb_btnEdit
            // 
            this.pcb_btnEdit.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnEdit.Image = global::Sisteg_Dashboard.Properties.Resources.btn_modify;
            this.pcb_btnEdit.Location = new System.Drawing.Point(765, 577);
            this.pcb_btnEdit.Name = "pcb_btnEdit";
            this.pcb_btnEdit.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnEdit.TabIndex = 48;
            this.pcb_btnEdit.TabStop = false;
            this.pcb_btnEdit.Click += new System.EventHandler(this.pcb_btnEdit_Click);
            this.pcb_btnEdit.MouseEnter += new System.EventHandler(this.pcb_btnEdit_MouseEnter);
            this.pcb_btnEdit.MouseLeave += new System.EventHandler(this.pcb_btnEdit_MouseLeave);
            // 
            // pcb_btnAdd
            // 
            this.pcb_btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnAdd.Image = global::Sisteg_Dashboard.Properties.Resources.btn_add;
            this.pcb_btnAdd.Location = new System.Drawing.Point(765, 501);
            this.pcb_btnAdd.Name = "pcb_btnAdd";
            this.pcb_btnAdd.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnAdd.TabIndex = 47;
            this.pcb_btnAdd.TabStop = false;
            this.pcb_btnAdd.Click += new System.EventHandler(this.pcb_btnAdd_Click);
            this.pcb_btnAdd.MouseEnter += new System.EventHandler(this.pcb_btnAdd_MouseEnter);
            this.pcb_btnAdd.MouseLeave += new System.EventHandler(this.pcb_btnAdd_MouseLeave);
            // 
            // pcb_btnPay
            // 
            this.pcb_btnPay.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnPay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnPay.Image = global::Sisteg_Dashboard.Properties.Resources.btn_payParcel;
            this.pcb_btnPay.Location = new System.Drawing.Point(765, 425);
            this.pcb_btnPay.Name = "pcb_btnPay";
            this.pcb_btnPay.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnPay.TabIndex = 46;
            this.pcb_btnPay.TabStop = false;
            this.pcb_btnPay.MouseEnter += new System.EventHandler(this.pcb_btnPay_MouseEnter);
            this.pcb_btnPay.MouseLeave += new System.EventHandler(this.pcb_btnPay_MouseLeave);
            // 
            // pcb_appClose
            // 
            this.pcb_appClose.BackColor = System.Drawing.Color.Transparent;
            this.pcb_appClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pcb_appClose.BackgroundImage")));
            this.pcb_appClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcb_appClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_appClose.Location = new System.Drawing.Point(965, 7);
            this.pcb_appClose.Name = "pcb_appClose";
            this.pcb_appClose.Size = new System.Drawing.Size(32, 32);
            this.pcb_appClose.TabIndex = 49;
            this.pcb_appClose.TabStop = false;
            this.pcb_appClose.Click += new System.EventHandler(this.pcb_appClose_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.client_sisteg_bg_value;
            this.ClientSize = new System.Drawing.Size(1220, 686);
            this.Controls.Add(this.pcb_appClose);
            this.Controls.Add(this.pcb_btnEdit);
            this.Controls.Add(this.pcb_btnAdd);
            this.Controls.Add(this.pcb_btnPay);
            this.Controls.Add(this.txt_searchClient);
            this.Controls.Add(this.lbl_parcelValue);
            this.Controls.Add(this.lbl_fixedValue);
            this.Controls.Add(this.rbtn_parcelValue);
            this.Controls.Add(this.rbtn_fixedValue);
            this.Controls.Add(this.dgv_clients);
            this.Controls.Add(this.pcb_btnConfig);
            this.Controls.Add(this.pcb_btnBudget);
            this.Controls.Add(this.pcb_btnProduct);
            this.Controls.Add(this.pcb_btnMain);
            this.Controls.Add(this.pcb_btnClient);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ClientForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client";
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnBudget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnProduct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_clients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnPay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_appClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pcb_btnConfig;
        private System.Windows.Forms.PictureBox pcb_btnBudget;
        private System.Windows.Forms.PictureBox pcb_btnProduct;
        private System.Windows.Forms.PictureBox pcb_btnMain;
        private System.Windows.Forms.PictureBox pcb_btnClient;
        private System.Windows.Forms.DataGridView dgv_clients;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox txt_searchClient;
        private System.Windows.Forms.Label lbl_parcelValue;
        private System.Windows.Forms.Label lbl_fixedValue;
        private Bunifu.UI.WinForms.BunifuRadioButton rbtn_parcelValue;
        private Bunifu.UI.WinForms.BunifuRadioButton rbtn_fixedValue;
        private System.Windows.Forms.PictureBox pcb_btnEdit;
        private System.Windows.Forms.PictureBox pcb_btnAdd;
        private System.Windows.Forms.PictureBox pcb_btnPay;
        private System.Windows.Forms.PictureBox pcb_appClose;
    }
}