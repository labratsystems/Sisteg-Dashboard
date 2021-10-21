namespace Sisteg_Dashboard
{
    partial class ProductForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductForm));
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties1 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties2 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pcb_btnConfig = new System.Windows.Forms.PictureBox();
            this.pcb_btnBudget = new System.Windows.Forms.PictureBox();
            this.pcb_btnProduct = new System.Windows.Forms.PictureBox();
            this.pcb_btnMain = new System.Windows.Forms.PictureBox();
            this.pcb_btnClient = new System.Windows.Forms.PictureBox();
            this.pcb_appClose = new System.Windows.Forms.PictureBox();
            this.pcb_btnEdit = new System.Windows.Forms.PictureBox();
            this.pcb_btnAdd = new System.Windows.Forms.PictureBox();
            this.txt_searchProductOrSupplier = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this.lbl_supplier = new System.Windows.Forms.Label();
            this.lbl_products = new System.Windows.Forms.Label();
            this.rbtn_supplier = new Bunifu.UI.WinForms.BunifuRadioButton();
            this.rbtn_products = new Bunifu.UI.WinForms.BunifuRadioButton();
            this.dgv_productsOrSuppliers = new System.Windows.Forms.DataGridView();
            this.bunifuShadowPanel1 = new Bunifu.UI.WinForm.BunifuShadowPanel.BunifuShadowPanel();
            this.lbl_configTag = new System.Windows.Forms.Label();
            this.lbl_budgetTag = new System.Windows.Forms.Label();
            this.lbl_productTag = new System.Windows.Forms.Label();
            this.lbl_clientTag = new System.Windows.Forms.Label();
            this.lbl_mainTag = new System.Windows.Forms.Label();
            this.lbl_btnEditTag = new System.Windows.Forms.Label();
            this.lbl_btnAddTag = new System.Windows.Forms.Label();
            this.lbl_searchTag = new System.Windows.Forms.Label();
            this.pcb_minimizeProgram = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnBudget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnProduct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_appClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_productsOrSuppliers)).BeginInit();
            this.bunifuShadowPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_minimizeProgram)).BeginInit();
            this.SuspendLayout();
            // 
            // pcb_btnConfig
            // 
            this.pcb_btnConfig.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnConfig.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.btn_config_main;
            this.pcb_btnConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnConfig.Location = new System.Drawing.Point(22, 340);
            this.pcb_btnConfig.Name = "pcb_btnConfig";
            this.pcb_btnConfig.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnConfig.TabIndex = 25;
            this.pcb_btnConfig.TabStop = false;
            this.pcb_btnConfig.Click += new System.EventHandler(this.pcb_btnConfig_Click);
            this.pcb_btnConfig.MouseEnter += new System.EventHandler(this.pcb_btnConfig_MouseEnter);
            this.pcb_btnConfig.MouseLeave += new System.EventHandler(this.pcb_btnConfig_MouseLeave);
            // 
            // pcb_btnBudget
            // 
            this.pcb_btnBudget.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnBudget.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.btn_budget_form;
            this.pcb_btnBudget.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnBudget.Location = new System.Drawing.Point(22, 264);
            this.pcb_btnBudget.Name = "pcb_btnBudget";
            this.pcb_btnBudget.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnBudget.TabIndex = 23;
            this.pcb_btnBudget.TabStop = false;
            this.pcb_btnBudget.Click += new System.EventHandler(this.pcb_btnBudget_Click);
            this.pcb_btnBudget.MouseEnter += new System.EventHandler(this.pcb_btnBudget_MouseEnter);
            this.pcb_btnBudget.MouseLeave += new System.EventHandler(this.pcb_btnBudget_MouseLeave);
            // 
            // pcb_btnProduct
            // 
            this.pcb_btnProduct.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnProduct.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.btn_product_form_active;
            this.pcb_btnProduct.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnProduct.Location = new System.Drawing.Point(22, 188);
            this.pcb_btnProduct.Name = "pcb_btnProduct";
            this.pcb_btnProduct.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnProduct.TabIndex = 22;
            this.pcb_btnProduct.TabStop = false;
            // 
            // pcb_btnMain
            // 
            this.pcb_btnMain.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnMain.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.btn_main_form;
            this.pcb_btnMain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnMain.Location = new System.Drawing.Point(22, 35);
            this.pcb_btnMain.Name = "pcb_btnMain";
            this.pcb_btnMain.Size = new System.Drawing.Size(328, 71);
            this.pcb_btnMain.TabIndex = 21;
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
            this.pcb_btnClient.Location = new System.Drawing.Point(22, 112);
            this.pcb_btnClient.Name = "pcb_btnClient";
            this.pcb_btnClient.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnClient.TabIndex = 20;
            this.pcb_btnClient.TabStop = false;
            this.pcb_btnClient.Click += new System.EventHandler(this.pcb_btnClient_Click);
            this.pcb_btnClient.MouseEnter += new System.EventHandler(this.pcb_btnClient_MouseEnter);
            this.pcb_btnClient.MouseLeave += new System.EventHandler(this.pcb_btnClient_MouseLeave);
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
            this.pcb_appClose.TabIndex = 59;
            this.pcb_appClose.TabStop = false;
            this.pcb_appClose.Click += new System.EventHandler(this.pcb_appClose_Click);
            // 
            // pcb_btnEdit
            // 
            this.pcb_btnEdit.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnEdit.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.btn_edit;
            this.pcb_btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnEdit.Location = new System.Drawing.Point(765, 538);
            this.pcb_btnEdit.Name = "pcb_btnEdit";
            this.pcb_btnEdit.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnEdit.TabIndex = 58;
            this.pcb_btnEdit.TabStop = false;
            this.pcb_btnEdit.Click += new System.EventHandler(this.pcb_btnEdit_Click);
            this.pcb_btnEdit.MouseEnter += new System.EventHandler(this.pcb_btnEdit_MouseEnter);
            this.pcb_btnEdit.MouseLeave += new System.EventHandler(this.pcb_btnEdit_MouseLeave);
            // 
            // pcb_btnAdd
            // 
            this.pcb_btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnAdd.BackgroundImage = global::Sisteg_Dashboard.Properties.Resources.btn_add;
            this.pcb_btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnAdd.Location = new System.Drawing.Point(765, 462);
            this.pcb_btnAdd.Name = "pcb_btnAdd";
            this.pcb_btnAdd.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnAdd.TabIndex = 57;
            this.pcb_btnAdd.TabStop = false;
            this.pcb_btnAdd.Click += new System.EventHandler(this.pcb_btnAdd_Click);
            this.pcb_btnAdd.MouseEnter += new System.EventHandler(this.pcb_btnAdd_MouseEnter);
            this.pcb_btnAdd.MouseLeave += new System.EventHandler(this.pcb_btnAdd_MouseLeave);
            // 
            // txt_searchProductOrSupplier
            // 
            this.txt_searchProductOrSupplier.AcceptsReturn = false;
            this.txt_searchProductOrSupplier.AcceptsTab = false;
            this.txt_searchProductOrSupplier.AnimationSpeed = 200;
            this.txt_searchProductOrSupplier.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txt_searchProductOrSupplier.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txt_searchProductOrSupplier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.txt_searchProductOrSupplier.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txt_searchProductOrSupplier.BackgroundImage")));
            this.txt_searchProductOrSupplier.BorderColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_searchProductOrSupplier.BorderColorDisabled = System.Drawing.Color.Gray;
            this.txt_searchProductOrSupplier.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_searchProductOrSupplier.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.txt_searchProductOrSupplier.BorderRadius = 30;
            this.txt_searchProductOrSupplier.BorderThickness = 1;
            this.txt_searchProductOrSupplier.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txt_searchProductOrSupplier.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_searchProductOrSupplier.DefaultFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_searchProductOrSupplier.DefaultText = "";
            this.txt_searchProductOrSupplier.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.txt_searchProductOrSupplier.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_searchProductOrSupplier.HideSelection = true;
            this.txt_searchProductOrSupplier.IconLeft = null;
            this.txt_searchProductOrSupplier.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_searchProductOrSupplier.IconPadding = 10;
            this.txt_searchProductOrSupplier.IconRight = null;
            this.txt_searchProductOrSupplier.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_searchProductOrSupplier.Lines = new string[0];
            this.txt_searchProductOrSupplier.Location = new System.Drawing.Point(914, 123);
            this.txt_searchProductOrSupplier.MaxLength = 32767;
            this.txt_searchProductOrSupplier.MinimumSize = new System.Drawing.Size(100, 35);
            this.txt_searchProductOrSupplier.Modified = false;
            this.txt_searchProductOrSupplier.Multiline = false;
            this.txt_searchProductOrSupplier.Name = "txt_searchProductOrSupplier";
            stateProperties1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            stateProperties1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties1.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_searchProductOrSupplier.OnActiveState = stateProperties1;
            stateProperties2.BorderColor = System.Drawing.Color.Gray;
            stateProperties2.FillColor = System.Drawing.Color.White;
            stateProperties2.ForeColor = System.Drawing.Color.Empty;
            stateProperties2.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txt_searchProductOrSupplier.OnDisabledState = stateProperties2;
            stateProperties3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            stateProperties3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties3.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_searchProductOrSupplier.OnHoverState = stateProperties3;
            stateProperties4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            stateProperties4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            stateProperties4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties4.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_searchProductOrSupplier.OnIdleState = stateProperties4;
            this.txt_searchProductOrSupplier.PasswordChar = '\0';
            this.txt_searchProductOrSupplier.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_searchProductOrSupplier.PlaceholderText = "Nome do produto";
            this.txt_searchProductOrSupplier.ReadOnly = false;
            this.txt_searchProductOrSupplier.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txt_searchProductOrSupplier.SelectedText = "";
            this.txt_searchProductOrSupplier.SelectionLength = 0;
            this.txt_searchProductOrSupplier.SelectionStart = 0;
            this.txt_searchProductOrSupplier.ShortcutsEnabled = true;
            this.txt_searchProductOrSupplier.Size = new System.Drawing.Size(200, 35);
            this.txt_searchProductOrSupplier.Style = Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox._Style.Material;
            this.txt_searchProductOrSupplier.TabIndex = 11;
            this.txt_searchProductOrSupplier.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txt_searchProductOrSupplier.TextMarginBottom = 0;
            this.txt_searchProductOrSupplier.TextMarginLeft = -5;
            this.txt_searchProductOrSupplier.TextMarginTop = 1;
            this.txt_searchProductOrSupplier.TextPlaceholder = "Nome do produto";
            this.txt_searchProductOrSupplier.UseSystemPasswordChar = false;
            this.txt_searchProductOrSupplier.WordWrap = true;
            this.txt_searchProductOrSupplier.TextChange += new System.EventHandler(this.txt_searchProductOrSupplier_TextChange);
            // 
            // lbl_supplier
            // 
            this.lbl_supplier.AutoSize = true;
            this.lbl_supplier.BackColor = System.Drawing.Color.Transparent;
            this.lbl_supplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_supplier.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_supplier.Location = new System.Drawing.Point(484, 137);
            this.lbl_supplier.Name = "lbl_supplier";
            this.lbl_supplier.Size = new System.Drawing.Size(146, 20);
            this.lbl_supplier.TabIndex = 9;
            this.lbl_supplier.Text = "Listar fornecedores";
            // 
            // lbl_products
            // 
            this.lbl_products.AutoSize = true;
            this.lbl_products.BackColor = System.Drawing.Color.Transparent;
            this.lbl_products.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_products.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_products.Location = new System.Drawing.Point(484, 97);
            this.lbl_products.Name = "lbl_products";
            this.lbl_products.Size = new System.Drawing.Size(115, 20);
            this.lbl_products.TabIndex = 7;
            this.lbl_products.Text = "Listar produtos";
            // 
            // rbtn_supplier
            // 
            this.rbtn_supplier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.rbtn_supplier.Checked = false;
            this.rbtn_supplier.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbtn_supplier.Location = new System.Drawing.Point(444, 133);
            this.rbtn_supplier.Name = "rbtn_supplier";
            this.rbtn_supplier.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_supplier.RadioColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_supplier.Size = new System.Drawing.Size(25, 25);
            this.rbtn_supplier.TabIndex = 8;
            this.rbtn_supplier.Text = null;
            this.rbtn_supplier.CheckedChanged += new System.EventHandler(this.rbtn_supplier_CheckedChanged);
            // 
            // rbtn_products
            // 
            this.rbtn_products.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.rbtn_products.Checked = true;
            this.rbtn_products.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbtn_products.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_products.Location = new System.Drawing.Point(444, 93);
            this.rbtn_products.Name = "rbtn_products";
            this.rbtn_products.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_products.RadioColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_products.Size = new System.Drawing.Size(25, 25);
            this.rbtn_products.TabIndex = 6;
            this.rbtn_products.Text = null;
            this.rbtn_products.CheckedChanged += new System.EventHandler(this.rbtn_products_CheckedChanged);
            // 
            // dgv_productsOrSuppliers
            // 
            this.dgv_productsOrSuppliers.AllowUserToAddRows = false;
            this.dgv_productsOrSuppliers.AllowUserToDeleteRows = false;
            this.dgv_productsOrSuppliers.AllowUserToResizeColumns = false;
            this.dgv_productsOrSuppliers.AllowUserToResizeRows = false;
            this.dgv_productsOrSuppliers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_productsOrSuppliers.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.dgv_productsOrSuppliers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_productsOrSuppliers.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgv_productsOrSuppliers.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_productsOrSuppliers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_productsOrSuppliers.ColumnHeadersHeight = 30;
            this.dgv_productsOrSuppliers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_productsOrSuppliers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dgv_productsOrSuppliers.EnableHeadersVisualStyles = false;
            this.dgv_productsOrSuppliers.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.dgv_productsOrSuppliers.Location = new System.Drawing.Point(2, 2);
            this.dgv_productsOrSuppliers.Name = "dgv_productsOrSuppliers";
            this.dgv_productsOrSuppliers.ReadOnly = true;
            this.dgv_productsOrSuppliers.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_productsOrSuppliers.RowHeadersVisible = false;
            this.dgv_productsOrSuppliers.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(104)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_productsOrSuppliers.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_productsOrSuppliers.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgv_productsOrSuppliers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_productsOrSuppliers.Size = new System.Drawing.Size(707, 160);
            this.dgv_productsOrSuppliers.TabIndex = 12;
            this.dgv_productsOrSuppliers.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_productsOrSuppliers_DataBindingComplete);
            // 
            // bunifuShadowPanel1
            // 
            this.bunifuShadowPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.bunifuShadowPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bunifuShadowPanel1.Controls.Add(this.dgv_productsOrSuppliers);
            this.bunifuShadowPanel1.Location = new System.Drawing.Point(426, 206);
            this.bunifuShadowPanel1.Name = "bunifuShadowPanel1";
            this.bunifuShadowPanel1.PanelColor = System.Drawing.Color.Empty;
            this.bunifuShadowPanel1.ShadowDept = 2;
            this.bunifuShadowPanel1.ShadowTopLeftVisible = false;
            this.bunifuShadowPanel1.Size = new System.Drawing.Size(715, 168);
            this.bunifuShadowPanel1.TabIndex = 13;
            // 
            // lbl_configTag
            // 
            this.lbl_configTag.AutoSize = true;
            this.lbl_configTag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_configTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_configTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_configTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_configTag.Location = new System.Drawing.Point(105, 367);
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
            this.lbl_budgetTag.Location = new System.Drawing.Point(105, 291);
            this.lbl_budgetTag.Name = "lbl_budgetTag";
            this.lbl_budgetTag.Size = new System.Drawing.Size(134, 20);
            this.lbl_budgetTag.TabIndex = 4;
            this.lbl_budgetTag.Text = "ORÇAMENTOS///";
            this.lbl_budgetTag.Click += new System.EventHandler(this.lbl_budgetTag_Click);
            // 
            // lbl_productTag
            // 
            this.lbl_productTag.AutoSize = true;
            this.lbl_productTag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_productTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_productTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_productTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_productTag.Location = new System.Drawing.Point(105, 215);
            this.lbl_productTag.Name = "lbl_productTag";
            this.lbl_productTag.Size = new System.Drawing.Size(111, 20);
            this.lbl_productTag.TabIndex = 3;
            this.lbl_productTag.Text = "PRODUTOS///";
            // 
            // lbl_clientTag
            // 
            this.lbl_clientTag.AutoSize = true;
            this.lbl_clientTag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_clientTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_clientTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_clientTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_clientTag.Location = new System.Drawing.Point(105, 139);
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
            this.lbl_mainTag.Location = new System.Drawing.Point(105, 63);
            this.lbl_mainTag.Name = "lbl_mainTag";
            this.lbl_mainTag.Size = new System.Drawing.Size(166, 20);
            this.lbl_mainTag.TabIndex = 1;
            this.lbl_mainTag.Text = "PAINEL PRINCIPAL///";
            this.lbl_mainTag.Click += new System.EventHandler(this.lbl_mainTag_Click);
            // 
            // lbl_btnEditTag
            // 
            this.lbl_btnEditTag.AutoSize = true;
            this.lbl_btnEditTag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_btnEditTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_btnEditTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_btnEditTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_btnEditTag.Location = new System.Drawing.Point(848, 566);
            this.lbl_btnEditTag.Name = "lbl_btnEditTag";
            this.lbl_btnEditTag.Size = new System.Drawing.Size(164, 20);
            this.lbl_btnEditTag.TabIndex = 15;
            this.lbl_btnEditTag.Text = "EDITAR PRODUTO///";
            this.lbl_btnEditTag.Click += new System.EventHandler(this.lbl_btnEditTag_Click);
            // 
            // lbl_btnAddTag
            // 
            this.lbl_btnAddTag.AutoSize = true;
            this.lbl_btnAddTag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_btnAddTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_btnAddTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_btnAddTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_btnAddTag.Location = new System.Drawing.Point(848, 490);
            this.lbl_btnAddTag.Name = "lbl_btnAddTag";
            this.lbl_btnAddTag.Size = new System.Drawing.Size(194, 20);
            this.lbl_btnAddTag.TabIndex = 14;
            this.lbl_btnAddTag.Text = "ADICIONAR PRODUTO///";
            this.lbl_btnAddTag.Click += new System.EventHandler(this.lbl_btnAddTag_Click);
            // 
            // lbl_searchTag
            // 
            this.lbl_searchTag.AutoSize = true;
            this.lbl_searchTag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_searchTag.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbl_searchTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_searchTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_searchTag.Location = new System.Drawing.Point(910, 92);
            this.lbl_searchTag.Name = "lbl_searchTag";
            this.lbl_searchTag.Size = new System.Drawing.Size(113, 20);
            this.lbl_searchTag.TabIndex = 10;
            this.lbl_searchTag.Text = "PROCURAR///";
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
            this.pcb_minimizeProgram.TabIndex = 75;
            this.pcb_minimizeProgram.TabStop = false;
            this.pcb_minimizeProgram.Click += new System.EventHandler(this.pcb_minimizeProgram_Click);
            // 
            // ProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 675);
            this.Controls.Add(this.pcb_minimizeProgram);
            this.Controls.Add(this.lbl_searchTag);
            this.Controls.Add(this.lbl_btnEditTag);
            this.Controls.Add(this.lbl_btnAddTag);
            this.Controls.Add(this.lbl_configTag);
            this.Controls.Add(this.lbl_budgetTag);
            this.Controls.Add(this.lbl_productTag);
            this.Controls.Add(this.lbl_clientTag);
            this.Controls.Add(this.lbl_mainTag);
            this.Controls.Add(this.bunifuShadowPanel1);
            this.Controls.Add(this.pcb_appClose);
            this.Controls.Add(this.pcb_btnEdit);
            this.Controls.Add(this.pcb_btnAdd);
            this.Controls.Add(this.txt_searchProductOrSupplier);
            this.Controls.Add(this.lbl_supplier);
            this.Controls.Add(this.lbl_products);
            this.Controls.Add(this.rbtn_supplier);
            this.Controls.Add(this.rbtn_products);
            this.Controls.Add(this.pcb_btnConfig);
            this.Controls.Add(this.pcb_btnBudget);
            this.Controls.Add(this.pcb_btnProduct);
            this.Controls.Add(this.pcb_btnMain);
            this.Controls.Add(this.pcb_btnClient);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProductForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Produtos";
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnBudget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnProduct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_appClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_productsOrSuppliers)).EndInit();
            this.bunifuShadowPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pcb_minimizeProgram)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pcb_btnConfig;
        private System.Windows.Forms.PictureBox pcb_btnBudget;
        private System.Windows.Forms.PictureBox pcb_btnProduct;
        private System.Windows.Forms.PictureBox pcb_btnMain;
        private System.Windows.Forms.PictureBox pcb_btnClient;
        private System.Windows.Forms.PictureBox pcb_appClose;
        private System.Windows.Forms.PictureBox pcb_btnEdit;
        private System.Windows.Forms.PictureBox pcb_btnAdd;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox txt_searchProductOrSupplier;
        private System.Windows.Forms.Label lbl_supplier;
        private System.Windows.Forms.Label lbl_products;
        private Bunifu.UI.WinForms.BunifuRadioButton rbtn_supplier;
        private Bunifu.UI.WinForms.BunifuRadioButton rbtn_products;
        private System.Windows.Forms.DataGridView dgv_productsOrSuppliers;
        private Bunifu.UI.WinForm.BunifuShadowPanel.BunifuShadowPanel bunifuShadowPanel1;
        protected internal System.Windows.Forms.Label lbl_configTag;
        protected internal System.Windows.Forms.Label lbl_budgetTag;
        protected internal System.Windows.Forms.Label lbl_productTag;
        protected internal System.Windows.Forms.Label lbl_clientTag;
        protected internal System.Windows.Forms.Label lbl_mainTag;
        protected internal System.Windows.Forms.Label lbl_btnEditTag;
        protected internal System.Windows.Forms.Label lbl_btnAddTag;
        protected internal System.Windows.Forms.Label lbl_searchTag;
        private System.Windows.Forms.PictureBox pcb_minimizeProgram;
    }
}