using System;
using System.Data;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class CategorySettings : Form
    {
        //INICIA INSTÂNCIA DO PAINEL
        public CategorySettings(ConfigForm configForm)
        {
            InitializeComponent();
            Globals.configForm = configForm;
            Globals.configForm.lbl_btnGoBackTag.Visible = true;
            Globals.configForm.pcb_btnGoBack.Visible = true;
            Globals.configForm.lbl_btnGoForwardTag.Visible = false;
            Globals.configForm.pcb_btnGoForward.Visible = false;

            this.updateCategoriesDataGridView();
        }

        //FUNÇÕES

        //Função que atualiza o DataGridView de categorias
        private void updateCategoriesDataGridView()
        {
            DataTable categoriesDataTable = null;
            if (this.rbtn_isIncome.Checked) categoriesDataTable = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaReceita = true");
            else if (this.rbtn_isExpense.Checked) categoriesDataTable = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaDespesa = true");
            else if (this.rbtn_isProduct.Checked) categoriesDataTable = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaProduto = true");
            if (categoriesDataTable.Rows.Count > 0)
            {
                this.dgv_categories.Show();
                this.dgv_categories.DataSource = categoriesDataTable;
            }
            else this.dgv_categories.Hide();
        }

        //Função que cadastra categoria
        private void categoryRegister()
        {
            if (String.IsNullOrEmpty(this.txt_categoryName.Text.Trim())) MessageBox.Show("Informe o nome da categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if ((Database.query("SELECT nomeCategoria FROM categoria WHERE nomeCategoria = '" + this.txt_categoryName.Text.Trim() + "';")).Rows.Count > 0) MessageBox.Show("Já existe uma categoria cadastrada com esse nome!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    Category category = new Category();
                    category.NomeCategoria = txt_categoryName.Text.Trim();
                    if (this.rbtn_isIncome.Checked) category.CategoriaReceita = true;
                    else if (this.rbtn_isExpense.Checked) category.CategoriaDespesa = true;
                    else if (this.rbtn_isProduct.Checked) category.CategoriaProduto = true;
                    if (Database.newCategory(category))
                    {
                        MessageBox.Show("Categoria cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.txt_categoryName.Clear();
                        this.txt_categoryName.PlaceholderText = "";
                        this.txt_categoryName.Focus();
                        this.updateCategoriesDataGridView();
                    }
                    else MessageBox.Show("[ERRO] Não foi possível cadastrar categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Função que atualiza categoria
        private void categoryUpdate()
        {
            if (String.IsNullOrEmpty(this.txt_categoryName.Text.Trim())) MessageBox.Show("Informe o nome da categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                switch (this.txt_categoryName.Text.Trim())
                {
                    case "Orçamentos":
                        MessageBox.Show("Não é possível atualizar esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Materiais elétricos":
                        MessageBox.Show("Não é possível atualizar esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Produtos eletrônicos":
                        MessageBox.Show("Não é possível atualizar esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Investimentos":
                        MessageBox.Show("Não é possível atualizar esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Outros":
                        MessageBox.Show("Não é possível atualizar esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Presentes":
                        MessageBox.Show("Não é possível atualizar esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Alimentação":
                        MessageBox.Show("Não é possível atualizar esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Educação":
                        MessageBox.Show("Não é possível atualizar esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Lazer":
                        MessageBox.Show("Não é possível atualizar esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Moradia":
                        MessageBox.Show("Não é possível atualizar esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Pagamentos":
                        MessageBox.Show("Não é possível atualizar esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Roupas":
                        MessageBox.Show("Não é possível atualizar esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Saúde":
                        MessageBox.Show("Não é possível atualizar esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Transporte":
                        MessageBox.Show("Não é possível atualizar esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                }
                if ((Database.query("SELECT nomeCategoria FROM categoria WHERE nomeCategoria = '" + this.txt_categoryName.Text.Trim() + "';")).Rows.Count > 0) MessageBox.Show("Já existe uma categoria cadastrada com esse nome!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    Category category = new Category();
                    category.NomeCategoria = txt_categoryName.Text.Trim();
                    category.IdCategoria = Convert.ToInt32(this.dgv_categories.SelectedRows[0].Cells[0].Value);
                    if (Database.updateCategory(category))
                    {
                        MessageBox.Show("Categoria atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.txt_categoryName.Clear();
                        this.txt_categoryName.PlaceholderText = "";
                        this.txt_categoryName.Focus();
                        this.updateCategoriesDataGridView();
                    }
                    else MessageBox.Show("[ERRO] Não foi possível atualizar categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Função que exclui categoria
        private void categoryDelete()
        {
            if (String.IsNullOrEmpty(this.txt_categoryName.Text.Trim())) MessageBox.Show("Informe o nome da categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                switch (this.txt_categoryName.Text.Trim())
                {
                    case "Orçamentos":
                        MessageBox.Show("Não é possível excluir esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Materiais elétricos":
                        MessageBox.Show("Não é possível excluir esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Produtos eletrônicos":
                        MessageBox.Show("Não é possível excluir esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Investimentos":
                        MessageBox.Show("Não é possível excluir esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Outros":
                        MessageBox.Show("Não é possível excluir esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Presentes":
                        MessageBox.Show("Não é possível excluir esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Alimentação":
                        MessageBox.Show("Não é possível excluir esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Educação":
                        MessageBox.Show("Não é possível excluir esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Lazer":
                        MessageBox.Show("Não é possível excluir esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Moradia":
                        MessageBox.Show("Não é possível excluir esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Pagamentos":
                        MessageBox.Show("Não é possível excluir esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Roupas":
                        MessageBox.Show("Não é possível excluir esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Saúde":
                        MessageBox.Show("Não é possível excluir esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case "Transporte":
                        MessageBox.Show("Não é possível excluir esta categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                }
                if (((DialogResult)MessageBox.Show("Tem certeza que deseja excluir a categoria?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)).ToString().ToUpper() == "NO") return;
                Category category = new Category();
                category.IdCategoria = Convert.ToInt32(this.dgv_categories.SelectedRows[0].Cells[0].Value);
                if (Database.deleteCategory(category))
                {
                    MessageBox.Show("Categoria excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.updateCategoriesDataGridView();
                }
                else MessageBox.Show("[ERRO] Não foi possível excluir categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //ADICIONAR CATEGORIA
        private void pcb_btnCategoryRegister_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnCategoryRegister.BackgroundImage = Properties.Resources.btn_add_category_active;
        }

        private void pcb_btnCategoryRegister_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnCategoryRegisterTag.ClientRectangle.Contains(lbl_btnCategoryRegisterTag.PointToClient(Cursor.Position))) this.pcb_btnCategoryRegister.BackgroundImage = Properties.Resources.btn_add_category;
        }

        private void pcb_btnCategoryRegister_Click(object sender, EventArgs e)
        {
            this.categoryRegister();
        }

        private void lbl_btnCategoryRegisterTag_Click(object sender, EventArgs e)
        {
            this.categoryRegister();
        }

        //ATUALIZAR CATEGORIA
        private void pcb_btnUpdate_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnUpdate.BackgroundImage = Properties.Resources.btn_update_active;
        }

        private void pcb_btnUpdate_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnUpdateTag.ClientRectangle.Contains(lbl_btnUpdateTag.PointToClient(Cursor.Position))) this.pcb_btnUpdate.BackgroundImage = Properties.Resources.btn_update;
        }

        private void pcb_btnUpdate_Click(object sender, EventArgs e)
        {
            this.categoryUpdate();
        }

        private void lbl_btnUpdateTag_Click(object sender, EventArgs e)
        {
            this.categoryUpdate();
        }

        //EXCLUIR CATEGORIA
        private void pcb_btnDelete_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnDelete.BackgroundImage = Properties.Resources.btn_delete_active;
        }

        private void pcb_btnDelete_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnDeleteTag.ClientRectangle.Contains(lbl_btnDeleteTag.PointToClient(Cursor.Position))) this.pcb_btnDelete.BackgroundImage = Properties.Resources.btn_delete;
        }

        private void pcb_btnDelete_Click(object sender, EventArgs e)
        {
            this.categoryDelete();
        }

        private void lbl_btnDeleteTag_Click(object sender, EventArgs e)
        {
            this.categoryDelete();
        }

        //FORMATAÇÃO DOS COMPONENTES DO FORMULÁRIO

        //Formatação da tabela após disposição dos dados
        private void dgv_categories_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn dataGridViewColumn in dgv_categories.Columns) dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgv_categories.Columns[0].Visible = false;
            dgv_categories.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        //CheckedChanged
        private void rbtn_isIncome_CheckedChanged(object sender, EventArgs e)
        {
            this.updateCategoriesDataGridView();
        }

        private void rbtn_isExpense_CheckedChanged(object sender, EventArgs e)
        {
            this.updateCategoriesDataGridView();
        }

        private void rbtn_isProduct_CheckedChanged(object sender, EventArgs e)
        {
            this.updateCategoriesDataGridView();
        }

        //Leave
        private void txt_categoryName_Leave(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(this.txt_categoryName.Text.Trim())) && (!this.txt_categoryName.Focused)) this.txt_categoryName.PlaceholderText = "Nome da categoria";
        }

        //SelectionChanged
        private void dgv_categories_SelectionChanged(object sender, EventArgs e)
        {
            if ((dgv_categories.SelectedRows.Count > 0) && (dgv_categories.SelectedRows[0].Cells.Count > 1)) this.txt_categoryName.Text = this.dgv_categories.SelectedRows[0].Cells[1].Value.ToString().Trim();
        }
    }
}
