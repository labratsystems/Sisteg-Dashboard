using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace Sisteg_Dashboard
{
    public partial class ConfigForm : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        Bitmap backGround, backGroundTemp;

        //EVITA TREMULAÇÃO DOS COMPONENTES
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handlerparam = base.CreateParams;
                handlerparam.ExStyle |= 0x02000000;
                return handlerparam;
            }
        }

        private void initialize()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            backGroundTemp = new Bitmap(Properties.Resources.empty_bg);
            backGround = new Bitmap(backGroundTemp, backGroundTemp.Width, backGroundTemp.Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.DrawImageUnscaled(backGround, 0, 0);
            base.OnPaint(e);
        }

        //INICIA INSTÂNCIA DO FORMULÁRIO
        public ConfigForm()
        {
            InitializeComponent();
            initialize();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Globals.step = 0;
            Globals.accountSettings = new AccountSettings(this) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            this.panel_steps.Controls.Add(Globals.accountSettings);
            Globals.accountSettings.Show();
        }

        //MENU DE NAVEGAÇÃO DA APLICAÇÃO

        //Formulário Painel principal
        private void pcb_btnMain_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnMain.BackgroundImage = Properties.Resources.btn_main_form_active;
        }

        private void pcb_btnMain_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_mainTag.ClientRectangle.Contains(lbl_mainTag.PointToClient(Cursor.Position))) this.pcb_btnMain.BackgroundImage = Properties.Resources.btn_main_form;
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

        private void lbl_mainTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Main>().Count() == 0)
            {
                Main main = new Main();
                main.Show();
                this.Close();
            }
        }

        //Formulário Cliente
        private void pcb_btnClient_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnClient.BackgroundImage = Properties.Resources.btn_client_form_active;
        }

        private void pcb_btnClient_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_clientTag.ClientRectangle.Contains(lbl_clientTag.PointToClient(Cursor.Position))) this.pcb_btnClient.BackgroundImage = Properties.Resources.btn_client_form;
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

        private void lbl_clientTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ClientForm>().Count() == 0)
            {
                ClientForm client = new ClientForm();
                client.Show();
                this.Close();
            }
        }

        //Formulário Produto
        private void pcb_btnProduct_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnProduct.BackgroundImage = Properties.Resources.btn_product_form_active;
        }

        private void pcb_btnProduct_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_productTag.ClientRectangle.Contains(lbl_productTag.PointToClient(Cursor.Position))) this.pcb_btnProduct.BackgroundImage = Properties.Resources.btn_product_form;
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

        private void lbl_productTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ProductForm>().Count() == 0)
            {
                ProductForm productForm = new ProductForm();
                productForm.Show();
                this.Close();
            }
        }

        //Formulário Orçamento
        private void pcb_btnBudget_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnBudget.BackgroundImage = Properties.Resources.btn_budget_form_active;
        }

        private void pcb_btnBudget_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_budgetTag.ClientRectangle.Contains(lbl_budgetTag.PointToClient(Cursor.Position))) this.pcb_btnBudget.BackgroundImage = Properties.Resources.btn_budget_form;
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

        private void lbl_budgetTag_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<BudgetForm>().Count() == 0)
            {
                BudgetForm budget = new BudgetForm();
                budget.Show();
                this.Close();
            }
        }

        //ENCERRAR APLICAÇÃO
        private void pcb_appClose_Click(object sender, EventArgs e)
        {
            if (((DialogResult)MessageBox.Show("Tem certeza que deseja encerrar a aplicação?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)).ToString().ToUpper() == "YES") Application.Exit();
        }

        //MINIMIZAR APLICAÇÃO
        private void pcb_minimizeProgram_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //AVANÇAR
        private void pcb_btnGoForward_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnGoForward.BackgroundImage = Properties.Resources.btn_go_forward_active;
        }

        private void pcb_btnGoForward_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnGoForwardTag.ClientRectangle.Contains(lbl_btnGoForwardTag.PointToClient(Cursor.Position))) this.pcb_btnGoForward.BackgroundImage = Properties.Resources.btn_go_forward;
        }

        private void pcb_btnGoForward_Click(object sender, EventArgs e)
        {
            Globals.step += 1;
            if (Globals.step == 1)
            {
                this.panel_steps.Controls.Remove(Globals.accountSettings);
                Globals.categorySettings = new CategorySettings(this) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Add(Globals.categorySettings);
                Globals.categorySettings.Show();
            }
        }

        private void lbl_btnGoForwardTag_Click(object sender, EventArgs e)
        {
            Globals.step += 1;
            if (Globals.step == 1)
            {
                this.panel_steps.Controls.Remove(Globals.accountSettings);
                Globals.categorySettings = new CategorySettings(this) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Add(Globals.categorySettings);
                Globals.categorySettings.Show();
            }
        }

        //VOLTAR
        private void pcb_btnGoBack_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnGoBack.BackgroundImage = Properties.Resources.btn_go_back_active;
        }

        private void pcb_btnGoBack_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnGoBackTag.ClientRectangle.Contains(lbl_btnGoBackTag.PointToClient(Cursor.Position))) this.pcb_btnGoBack.BackgroundImage = Properties.Resources.btn_go_back;
        }
        private void pcb_btnGoBack_Click(object sender, EventArgs e)
        {
            Globals.step -= 1;
            if (Globals.step == 0)
            {
                this.panel_steps.Controls.Remove(Globals.categorySettings);
                Globals.accountSettings = new AccountSettings(this) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Add(Globals.accountSettings);
                Globals.accountSettings.Show();
            }
        }

        private void lbl_btnGoBackTag_Click(object sender, EventArgs e)
        {
            Globals.step -= 1;
            if (Globals.step == 0)
            {
                this.panel_steps.Controls.Remove(Globals.categorySettings);
                Globals.accountSettings = new AccountSettings(this) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                this.panel_steps.Controls.Add(Globals.accountSettings);
                Globals.accountSettings.Show();
            }
        }
    }
}
