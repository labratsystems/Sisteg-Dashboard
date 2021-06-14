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
    public partial class ConfigForm : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        protected internal int step = 0;
        protected internal AccountSettings accountSettings;
        protected internal CategorySettings categorySettings;

        public ConfigForm()
        {
            InitializeComponent();
            accountSettings = new AccountSettings(this) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            this.panel_steps.Controls.Add(accountSettings);
            accountSettings.Show();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handlerparam = base.CreateParams;
                handlerparam.ExStyle |= 0x02000000;
                return handlerparam;
            }
        }

        private void pcb_btnMain_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnMain.Image = Properties.Resources.btn_main_form_active;
        }

        private void pcb_btnMain_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnMain.Image = Properties.Resources.btn_main_form;
        }

        private void pcb_btnMain_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Main>().Count() == 0)
            {
                Main main = new Main();
                main.Show();
                this.Close();
            }
        }

        private void pcb_btnClient_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnClient.Image = Properties.Resources.btn_client_form_active;
        }

        private void pcb_btnClient_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnClient.Image = Properties.Resources.btn_client_form;
        }

        private void pcb_btnClient_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ClientForm>().Count() == 0)
            {
                ClientForm client = new ClientForm();
                client.Show();
                this.Close();
            }
        }

        private void pcb_btnProduct_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnProduct.Image = Properties.Resources.btn_product_form_active;
        }

        private void pcb_btnProduct_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnProduct.Image = Properties.Resources.btn_product_form;
        }

        private void pcb_btnProduct_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ProductForm>().Count() == 0)
            {
                ProductForm productForm = new ProductForm();
                productForm.Show();
                this.Close();
            }
        }

        private void pcb_btnBudget_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnBudget.Image = Properties.Resources.btn_budget_form_active;
        }

        private void pcb_btnBudget_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnBudget.Image = Properties.Resources.btn_budget_form;
        }

        private void pcb_btnBudget_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<BudgetForm>().Count() == 0)
            {
                BudgetForm budget = new BudgetForm();
                budget.Show();
                this.Close();
            }
        }

        private void pcb_btnGoForward_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnGoForward.Image = Properties.Resources.btn_go_forward_active;
        }

        private void pcb_btnGoForward_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnGoForward.Image = Properties.Resources.btn_go_forward;
        }

        private void pcb_btnGoBack_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnGoBack.Image = Properties.Resources.btn_go_back_active;
        }

        private void pcb_btnGoBack_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnGoBack.Image = Properties.Resources.btn_go_back;
        }

        private void pcb_btnGoForward_Click(object sender, EventArgs e)
        {
            step += 1;
            if (step == 1)
            {
                this.panel_steps.Controls.Remove(accountSettings);
                categorySettings = new CategorySettings(this) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Add(categorySettings);
                categorySettings.Show();
            }
        }

        private void pcb_btnGoBack_Click(object sender, EventArgs e)
        {
            step -= 1;
            if (step == 0)
            {
                this.panel_steps.Controls.Remove(categorySettings);
                accountSettings = new AccountSettings(this) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Add(accountSettings);
                accountSettings.Show();
            }
        }
    }
}
