namespace Sisteg_Dashboard
{
    partial class CategorySettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CategorySettings));
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties5 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties6 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties7 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties8 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel_client = new System.Windows.Forms.Panel();
            this.lbl_isProduct = new System.Windows.Forms.Label();
            this.rbtn_isProduct = new Bunifu.UI.WinForms.BunifuRadioButton();
            this.lbl_isExpense = new System.Windows.Forms.Label();
            this.rbtn_isExpense = new Bunifu.UI.WinForms.BunifuRadioButton();
            this.lbl_isIncome = new System.Windows.Forms.Label();
            this.rbtn_isIncome = new Bunifu.UI.WinForms.BunifuRadioButton();
            this.pcb_btnUpdateCategory = new System.Windows.Forms.PictureBox();
            this.txt_categoryName = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this.pcb_btnDeleteCategory = new System.Windows.Forms.PictureBox();
            this.pcb_btnAdd = new System.Windows.Forms.PictureBox();
            this.border_dgvCategories = new Bunifu.UI.WinForm.BunifuShadowPanel.BunifuShadowPanel();
            this.dgv_categories = new System.Windows.Forms.DataGridView();
            this.panel_client.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnUpdateCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnDeleteCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnAdd)).BeginInit();
            this.border_dgvCategories.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_categories)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_client
            // 
            this.panel_client.AutoScroll = true;
            this.panel_client.AutoScrollMargin = new System.Drawing.Size(0, 12);
            this.panel_client.BackColor = System.Drawing.Color.Transparent;
            this.panel_client.Controls.Add(this.lbl_isProduct);
            this.panel_client.Controls.Add(this.rbtn_isProduct);
            this.panel_client.Controls.Add(this.lbl_isExpense);
            this.panel_client.Controls.Add(this.rbtn_isExpense);
            this.panel_client.Controls.Add(this.lbl_isIncome);
            this.panel_client.Controls.Add(this.rbtn_isIncome);
            this.panel_client.Controls.Add(this.pcb_btnUpdateCategory);
            this.panel_client.Controls.Add(this.txt_categoryName);
            this.panel_client.Controls.Add(this.pcb_btnDeleteCategory);
            this.panel_client.Controls.Add(this.pcb_btnAdd);
            this.panel_client.Controls.Add(this.border_dgvCategories);
            this.panel_client.Location = new System.Drawing.Point(35, 29);
            this.panel_client.Name = "panel_client";
            this.panel_client.Size = new System.Drawing.Size(700, 320);
            this.panel_client.TabIndex = 122;
            // 
            // lbl_isProduct
            // 
            this.lbl_isProduct.AutoSize = true;
            this.lbl_isProduct.BackColor = System.Drawing.Color.Transparent;
            this.lbl_isProduct.Font = new System.Drawing.Font("Courier Prime Code", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_isProduct.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_isProduct.Location = new System.Drawing.Point(611, 29);
            this.lbl_isProduct.Name = "lbl_isProduct";
            this.lbl_isProduct.Size = new System.Drawing.Size(80, 21);
            this.lbl_isProduct.TabIndex = 123;
            this.lbl_isProduct.Text = "Produto";
            // 
            // rbtn_isProduct
            // 
            this.rbtn_isProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.rbtn_isProduct.Checked = false;
            this.rbtn_isProduct.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbtn_isProduct.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_isProduct.Location = new System.Drawing.Point(580, 25);
            this.rbtn_isProduct.Name = "rbtn_isProduct";
            this.rbtn_isProduct.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_isProduct.RadioColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_isProduct.Size = new System.Drawing.Size(25, 25);
            this.rbtn_isProduct.TabIndex = 122;
            this.rbtn_isProduct.Text = null;
            this.rbtn_isProduct.CheckedChanged += new System.EventHandler(this.rbtn_isProduct_CheckedChanged);
            // 
            // lbl_isExpense
            // 
            this.lbl_isExpense.AutoSize = true;
            this.lbl_isExpense.BackColor = System.Drawing.Color.Transparent;
            this.lbl_isExpense.Font = new System.Drawing.Font("Courier Prime Code", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_isExpense.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_isExpense.Location = new System.Drawing.Point(494, 29);
            this.lbl_isExpense.Name = "lbl_isExpense";
            this.lbl_isExpense.Size = new System.Drawing.Size(80, 21);
            this.lbl_isExpense.TabIndex = 121;
            this.lbl_isExpense.Text = "Despesa";
            // 
            // rbtn_isExpense
            // 
            this.rbtn_isExpense.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.rbtn_isExpense.Checked = false;
            this.rbtn_isExpense.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbtn_isExpense.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_isExpense.Location = new System.Drawing.Point(463, 25);
            this.rbtn_isExpense.Name = "rbtn_isExpense";
            this.rbtn_isExpense.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_isExpense.RadioColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_isExpense.Size = new System.Drawing.Size(25, 25);
            this.rbtn_isExpense.TabIndex = 120;
            this.rbtn_isExpense.Text = null;
            this.rbtn_isExpense.CheckedChanged += new System.EventHandler(this.rbtn_isExpense_CheckedChanged);
            // 
            // lbl_isIncome
            // 
            this.lbl_isIncome.AutoSize = true;
            this.lbl_isIncome.BackColor = System.Drawing.Color.Transparent;
            this.lbl_isIncome.Font = new System.Drawing.Font("Courier Prime Code", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_isIncome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_isIncome.Location = new System.Drawing.Point(377, 29);
            this.lbl_isIncome.Name = "lbl_isIncome";
            this.lbl_isIncome.Size = new System.Drawing.Size(80, 21);
            this.lbl_isIncome.TabIndex = 119;
            this.lbl_isIncome.Text = "Receita";
            // 
            // rbtn_isIncome
            // 
            this.rbtn_isIncome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.rbtn_isIncome.Checked = true;
            this.rbtn_isIncome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbtn_isIncome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_isIncome.Location = new System.Drawing.Point(347, 25);
            this.rbtn_isIncome.Name = "rbtn_isIncome";
            this.rbtn_isIncome.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_isIncome.RadioColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtn_isIncome.Size = new System.Drawing.Size(25, 25);
            this.rbtn_isIncome.TabIndex = 118;
            this.rbtn_isIncome.Text = null;
            this.rbtn_isIncome.CheckedChanged += new System.EventHandler(this.rbtn_isIncome_CheckedChanged);
            // 
            // pcb_btnUpdateCategory
            // 
            this.pcb_btnUpdateCategory.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnUpdateCategory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnUpdateCategory.Image = ((System.Drawing.Image)(resources.GetObject("pcb_btnUpdateCategory.Image")));
            this.pcb_btnUpdateCategory.Location = new System.Drawing.Point(353, 152);
            this.pcb_btnUpdateCategory.Name = "pcb_btnUpdateCategory";
            this.pcb_btnUpdateCategory.Size = new System.Drawing.Size(327, 70);
            this.pcb_btnUpdateCategory.TabIndex = 117;
            this.pcb_btnUpdateCategory.TabStop = false;
            this.pcb_btnUpdateCategory.Click += new System.EventHandler(this.pcb_btnUpdateCategory_Click);
            this.pcb_btnUpdateCategory.MouseEnter += new System.EventHandler(this.pcb_btnUpdateCategory_MouseEnter);
            this.pcb_btnUpdateCategory.MouseLeave += new System.EventHandler(this.pcb_btnUpdateCategory_MouseLeave);
            // 
            // txt_categoryName
            // 
            this.txt_categoryName.AcceptsReturn = false;
            this.txt_categoryName.AcceptsTab = false;
            this.txt_categoryName.AnimationSpeed = 200;
            this.txt_categoryName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txt_categoryName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txt_categoryName.BackColor = System.Drawing.Color.Transparent;
            this.txt_categoryName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txt_categoryName.BackgroundImage")));
            this.txt_categoryName.BorderColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_categoryName.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(161)))), ((int)(((byte)(161)))));
            this.txt_categoryName.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_categoryName.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_categoryName.BorderRadius = 1;
            this.txt_categoryName.BorderThickness = 1;
            this.txt_categoryName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txt_categoryName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_categoryName.DefaultFont = new System.Drawing.Font("Courier Prime Code", 12F);
            this.txt_categoryName.DefaultText = "";
            this.txt_categoryName.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.txt_categoryName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_categoryName.HideSelection = true;
            this.txt_categoryName.IconLeft = null;
            this.txt_categoryName.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_categoryName.IconPadding = 10;
            this.txt_categoryName.IconRight = null;
            this.txt_categoryName.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_categoryName.Lines = new string[0];
            this.txt_categoryName.Location = new System.Drawing.Point(13, 20);
            this.txt_categoryName.MaxLength = 32767;
            this.txt_categoryName.MinimumSize = new System.Drawing.Size(100, 35);
            this.txt_categoryName.Modified = false;
            this.txt_categoryName.Multiline = false;
            this.txt_categoryName.Name = "txt_categoryName";
            stateProperties5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties5.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(104)))), ((int)(((byte)(232)))));
            stateProperties5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties5.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(104)))), ((int)(((byte)(232)))));
            this.txt_categoryName.OnActiveState = stateProperties5;
            stateProperties6.BorderColor = System.Drawing.Color.Empty;
            stateProperties6.FillColor = System.Drawing.Color.White;
            stateProperties6.ForeColor = System.Drawing.Color.Empty;
            stateProperties6.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txt_categoryName.OnDisabledState = stateProperties6;
            stateProperties7.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties7.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(104)))), ((int)(((byte)(232)))));
            stateProperties7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties7.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_categoryName.OnHoverState = stateProperties7;
            stateProperties8.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties8.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            stateProperties8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            stateProperties8.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_categoryName.OnIdleState = stateProperties8;
            this.txt_categoryName.PasswordChar = '\0';
            this.txt_categoryName.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_categoryName.PlaceholderText = "Nome da categoria";
            this.txt_categoryName.ReadOnly = false;
            this.txt_categoryName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txt_categoryName.SelectedText = "";
            this.txt_categoryName.SelectionLength = 0;
            this.txt_categoryName.SelectionStart = 0;
            this.txt_categoryName.ShortcutsEnabled = true;
            this.txt_categoryName.Size = new System.Drawing.Size(300, 35);
            this.txt_categoryName.Style = Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox._Style.Bunifu;
            this.txt_categoryName.TabIndex = 114;
            this.txt_categoryName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txt_categoryName.TextMarginBottom = 0;
            this.txt_categoryName.TextMarginLeft = 5;
            this.txt_categoryName.TextMarginTop = 1;
            this.txt_categoryName.TextPlaceholder = "Nome da categoria";
            this.txt_categoryName.UseSystemPasswordChar = false;
            this.txt_categoryName.WordWrap = true;
            // 
            // pcb_btnDeleteCategory
            // 
            this.pcb_btnDeleteCategory.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnDeleteCategory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnDeleteCategory.Image = ((System.Drawing.Image)(resources.GetObject("pcb_btnDeleteCategory.Image")));
            this.pcb_btnDeleteCategory.Location = new System.Drawing.Point(353, 228);
            this.pcb_btnDeleteCategory.Name = "pcb_btnDeleteCategory";
            this.pcb_btnDeleteCategory.Size = new System.Drawing.Size(327, 70);
            this.pcb_btnDeleteCategory.TabIndex = 116;
            this.pcb_btnDeleteCategory.TabStop = false;
            this.pcb_btnDeleteCategory.Click += new System.EventHandler(this.pcb_btnDeleteCategory_Click);
            this.pcb_btnDeleteCategory.MouseEnter += new System.EventHandler(this.pcb_btnDeleteCategory_MouseEnter);
            this.pcb_btnDeleteCategory.MouseLeave += new System.EventHandler(this.pcb_btnDeleteCategory_MouseLeave);
            // 
            // pcb_btnAdd
            // 
            this.pcb_btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.pcb_btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcb_btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("pcb_btnAdd.Image")));
            this.pcb_btnAdd.Location = new System.Drawing.Point(353, 76);
            this.pcb_btnAdd.Name = "pcb_btnAdd";
            this.pcb_btnAdd.Size = new System.Drawing.Size(328, 70);
            this.pcb_btnAdd.TabIndex = 117;
            this.pcb_btnAdd.TabStop = false;
            this.pcb_btnAdd.Click += new System.EventHandler(this.pcb_btnAdd_Click);
            this.pcb_btnAdd.MouseEnter += new System.EventHandler(this.pcb_btnAdd_MouseEnter);
            this.pcb_btnAdd.MouseLeave += new System.EventHandler(this.pcb_btnAdd_MouseLeave);
            // 
            // border_dgvCategories
            // 
            this.border_dgvCategories.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.border_dgvCategories.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.border_dgvCategories.Controls.Add(this.dgv_categories);
            this.border_dgvCategories.Location = new System.Drawing.Point(13, 75);
            this.border_dgvCategories.Name = "border_dgvCategories";
            this.border_dgvCategories.PanelColor = System.Drawing.Color.Empty;
            this.border_dgvCategories.ShadowDept = 2;
            this.border_dgvCategories.ShadowTopLeftVisible = false;
            this.border_dgvCategories.Size = new System.Drawing.Size(328, 227);
            this.border_dgvCategories.TabIndex = 110;
            // 
            // dgv_categories
            // 
            this.dgv_categories.AllowUserToAddRows = false;
            this.dgv_categories.AllowUserToDeleteRows = false;
            this.dgv_categories.AllowUserToResizeColumns = false;
            this.dgv_categories.AllowUserToResizeRows = false;
            this.dgv_categories.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_categories.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.dgv_categories.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_categories.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgv_categories.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Courier Prime Code", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_categories.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_categories.ColumnHeadersHeight = 30;
            this.dgv_categories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_categories.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dgv_categories.EnableHeadersVisualStyles = false;
            this.dgv_categories.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            this.dgv_categories.Location = new System.Drawing.Point(3, 3);
            this.dgv_categories.Name = "dgv_categories";
            this.dgv_categories.ReadOnly = true;
            this.dgv_categories.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_categories.RowHeadersVisible = false;
            this.dgv_categories.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(157)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Courier Prime Code", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(104)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_categories.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_categories.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgv_categories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_categories.Size = new System.Drawing.Size(318, 218);
            this.dgv_categories.TabIndex = 23;
            this.dgv_categories.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_categories_DataBindingComplete);
            // 
            // CategorySettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(773, 381);
            this.Controls.Add(this.panel_client);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CategorySettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CategorySettings";
            this.panel_client.ResumeLayout(false);
            this.panel_client.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnUpdateCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnDeleteCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_btnAdd)).EndInit();
            this.border_dgvCategories.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_categories)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_client;
        public System.Windows.Forms.PictureBox pcb_btnUpdateCategory;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox txt_categoryName;
        public System.Windows.Forms.PictureBox pcb_btnDeleteCategory;
        private System.Windows.Forms.PictureBox pcb_btnAdd;
        private Bunifu.UI.WinForm.BunifuShadowPanel.BunifuShadowPanel border_dgvCategories;
        private System.Windows.Forms.Label lbl_isProduct;
        private Bunifu.UI.WinForms.BunifuRadioButton rbtn_isProduct;
        private System.Windows.Forms.Label lbl_isExpense;
        private Bunifu.UI.WinForms.BunifuRadioButton rbtn_isExpense;
        private System.Windows.Forms.Label lbl_isIncome;
        private Bunifu.UI.WinForms.BunifuRadioButton rbtn_isIncome;
        private System.Windows.Forms.DataGridView dgv_categories;
    }
}