using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class CategorySettings : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        protected internal ConfigForm categorySettingsConfigForm;

        public CategorySettings(ConfigForm configForm)
        {
            InitializeComponent();
            categorySettingsConfigForm = configForm;
            categorySettingsConfigForm.pcb_btnGoBack.Visible = true;
            categorySettingsConfigForm.pcb_btnGoForward.Visible = false;

            this.dgv_categories.DataSource = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaReceita = true");
        }

        private void pcb_btnAdd_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnAdd.Image = Properties.Resources.btn_add_active;
        }

        private void pcb_btnAdd_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnAdd.Image = Properties.Resources.btn_add;
        }

        private void pcb_btnUpdateCategory_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnUpdateCategory.Image = Properties.Resources.btn_update_active;
        }

        private void pcb_btnUpdateCategory_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnUpdateCategory.Image = Properties.Resources.btn_update;
        }

        private void pcb_btnDeleteCategory_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnDeleteCategory.Image = Properties.Resources.btn_delete_active;
        }

        private void pcb_btnDeleteCategory_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnDeleteCategory.Image = Properties.Resources.btn_delete;
        }

        //FORMATAÇÃO DA TABELA APÓS DISPOSIÇÃO DOS DADOS
        private void dgv_categories_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn dataGridViewColumn in dgv_categories.Columns) dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgv_categories.Columns[0].Visible = false;
            dgv_categories.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void rbtn_isIncome_CheckedChanged(object sender, EventArgs e)
        {
            if(this.rbtn_isIncome.Checked) this.dgv_categories.DataSource = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaReceita = true");
        }

        private void rbtn_isExpense_CheckedChanged(object sender, EventArgs e)
        {
            if(this.rbtn_isExpense.Checked) this.dgv_categories.DataSource = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaDespesa = true");
        }

        private void rbtn_isProduct_CheckedChanged(object sender, EventArgs e)
        {
            if(this.rbtn_isProduct.Checked) this.dgv_categories.DataSource = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaProduto = true");
        }

        private void pcb_btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txt_categoryName.Text.Trim())) MessageBox.Show("Informe o nome da categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Category category = new Category();
                category.nomeCategoria = txt_categoryName.Text.Trim();
                if (this.rbtn_isIncome.Checked) category.categoriaReceita = true;
                else if (this.rbtn_isExpense.Checked) category.categoriaDespesa = true;
                else if (this.rbtn_isProduct.Checked) category.categoriaProduto = true;
                if(Database.newCategory(category))
                {
                    MessageBox.Show("Categoria cadastrada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txt_categoryName.Clear();
                    this.txt_categoryName.PlaceholderText = "";
                    this.txt_categoryName.Focus();
                    if (this.rbtn_isIncome.Checked) this.dgv_categories.DataSource = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaReceita = true");
                    else if (this.rbtn_isExpense.Checked) this.dgv_categories.DataSource = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaDespesa = true");
                    else if (this.rbtn_isProduct.Checked) this.dgv_categories.DataSource = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaProduto = true");
                }
                else MessageBox.Show("Não foi possível cadastrar categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pcb_btnUpdateCategory_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txt_categoryName.Text.Trim())) MessageBox.Show("Informe o nome da categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Category category = new Category();
                category.nomeCategoria = txt_categoryName.Text.Trim();
                category.idCategoria = Convert.ToInt32(this.dgv_categories.SelectedRows[0].Cells[0].Value);
                if (Database.updateCategory(category))
                {
                    MessageBox.Show("Categoria atualizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txt_categoryName.Clear();
                    this.txt_categoryName.PlaceholderText = "";
                    this.txt_categoryName.Focus();
                    if (this.rbtn_isIncome.Checked) this.dgv_categories.DataSource = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaReceita = true");
                    else if (this.rbtn_isExpense.Checked) this.dgv_categories.DataSource = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaDespesa = true");
                    else if (this.rbtn_isProduct.Checked) this.dgv_categories.DataSource = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaProduto = true");
                }
                else MessageBox.Show("Não foi possível atualizar categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pcb_btnDeleteCategory_Click(object sender, EventArgs e)
        {
            Category category = new Category();
            category.idCategoria = Convert.ToInt32(this.dgv_categories.SelectedRows[0].Cells[0].Value);
            if (Database.deleteCategory(category))
            {
                MessageBox.Show("Categoria excluída com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (this.rbtn_isIncome.Checked) this.dgv_categories.DataSource = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaReceita = true");
                else if (this.rbtn_isExpense.Checked) this.dgv_categories.DataSource = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaDespesa = true");
                else if (this.rbtn_isProduct.Checked) this.dgv_categories.DataSource = Database.query("SELECT idCategoria, nomeCategoria AS 'Categoria:' FROM categoria WHERE categoriaProduto = true");
            }
            else MessageBox.Show("Não foi possível excluir categoria!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
