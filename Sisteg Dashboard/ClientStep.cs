using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iText;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Sisteg_Dashboard
{
    public partial class ClientStep : Form
    {
        //DECLARAÇÃO DE VARIÁVEIS
        protected internal DataTable clientStepDataTable, budgetStepDataTable;
        protected internal BudgetForm clientStepBudgetForm;
        protected internal int budgetNumber = 0;
        public static string path = System.Environment.CurrentDirectory;

        //INICIA INSTÂNCIA DO PAINEL, POPULANDO O COMBOBOX DE LISTAGEM DE CLIENTES
        public ClientStep(BudgetForm budgetForm, DataTable dataTable)
        {
            InitializeComponent();
            //Popula o combobox de clientes
            clientStepDataTable = Database.query("SELECT cliente.nomeCliente FROM cliente ORDER BY cliente.nomeCliente;");
            for (int i = 0; i < clientStepDataTable.Rows.Count; i++) this.cbb_clientName.Items.Insert(i, " " + clientStepDataTable.Rows[i].ItemArray[0].ToString());
            
            clientStepBudgetForm = budgetForm;

            //Cliente já tem orçamento
            //Usuário avançou e retornou a este formulário
            if (dataTable != null)
            {
                clientStepDataTable = dataTable;
                this.cbb_clientName.SelectedIndex = this.cbb_clientName.FindString(" " + clientStepDataTable.Rows[0].ItemArray[1].ToString());
                if (clientStepBudgetForm.selectedIndex != -1) this.cbb_budgetNumber.SelectedIndex = clientStepBudgetForm.selectedIndex;
                budgetStepDataTable = Database.query("SELECT * FROM orcamento WHERE idCliente = " + clientStepDataTable.Rows[0].ItemArray[0]);
                for (int i = 0; i < budgetStepDataTable.Rows.Count; i++)
                {
                    if (clientStepBudgetForm.budgetNumber == Convert.ToInt32(budgetStepDataTable.Rows[i].ItemArray[0]))
                    {
                        this.cbb_budgetNumber.SelectedIndex = this.cbb_budgetNumber.FindString(" " + (i + 1).ToString());
                        this.isBudgetConfirmed(i);
                    }
                }

                //Esconde os controles do formulário caso nenhum orçamento esteja selecionado
                if (this.cbb_budgetNumber.SelectedIndex == -1)
                {
                    this.pcb_btnConfirmBudget.Visible = false;
                    this.pcb_btnPrint.Visible = false;
                    this.pcb_btnSendBudget.Visible = false;
                    this.ckb_technicalReport.Visible = false;
                    this.lbl_technicalReport.Visible = false;
                }
            }
            //Cliente não tem orçamento
            else
            {
                //Esconde os controles do formulário
                this.cbb_budgetNumber.Visible = false;
                this.clientStepBudgetForm.pcb_btnEdit.Visible = false;
                this.pcb_btnConfirmBudget.Visible = false;
                this.pcb_btnPrint.Visible = false;
                this.pcb_btnSendBudget.Visible = false;
                this.ckb_technicalReport.Visible = false;
                this.lbl_technicalReport.Visible = false;
            }
        }

        //EVITA TREMULAÇÃO DE COMPONENTES
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handlerparam = base.CreateParams;
                handlerparam.ExStyle |= 0x02000000;
                return handlerparam;
            }
        }

        //FUNÇÃO QUE CHECA SE O ORÇAMENTO FOI CONFIRMADO
        public void isBudgetConfirmed(int i)
        {
            //Mostra o controle escondido no formulário
            this.clientStepBudgetForm.pcb_btnEdit.Visible = true;

            //Orçamento não confirmado
            if (!Convert.ToBoolean(budgetStepDataTable.Rows[i].ItemArray[6]))
            {
                this.pcb_btnConfirmBudget.Visible = true;
                this.pcb_btnPrint.Location = new System.Drawing.Point(6, 219);
            }
            //Orçamento confirmado
            else this.pcb_btnPrint.Location = new System.Drawing.Point(6, 143);

            //Mostra os controles escondidos do formulário
            this.pcb_btnPrint.Visible = true;
            this.pcb_btnSendBudget.Visible = true;
        }

        //FUNÇÃO QUE LISTA OS TELEFONES DO CLIENTE NA TABELA DE CLIENTES
        private void listTelephones(DataTable telephoneDataTable, Table table, Style titleStyle, Style bodyStyle)
        {
            foreach (DataRow dataRow in telephoneDataTable.Rows)
            {
                DataTable telephoneTypeDataTable = Database.query("SELECT numeroTelefone FROM telefone WHERE idCliente = " + this.clientStepDataTable.Rows[0].ItemArray[0] + " AND tipoTelefone = '" + dataRow.ItemArray[3] + "';");
                if (telephoneTypeDataTable.Rows.Count == 1)
                {
                    if (this.clientStepDataTable.Columns.Contains("Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":"))
                    {
                        this.clientStepDataTable.Rows[0]["Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":"] = dataRow.ItemArray[4];
                        break;
                    }
                    else
                    {
                        this.clientStepDataTable.Columns.Add("Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":", typeof(string));
                        this.clientStepDataTable.Rows[0]["Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":"] = dataRow.ItemArray[4];
                        table.AddCell(new Cell(1, 1).Add(new Paragraph("Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":")).AddStyle(titleStyle));
                        table.AddCell(new Cell(1, 7).Add(new Paragraph(dataRow.ItemArray[4].ToString())).AddStyle(bodyStyle).SetTextAlignment(TextAlignment.LEFT));
                    }
                }
                else if (telephoneTypeDataTable.Rows.Count > 1)
                {
                    if (this.clientStepDataTable.Columns.Contains("Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":"))
                    {
                        this.clientStepDataTable.Rows[0]["Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":"] = dataRow.ItemArray[4];
                        break;
                    }
                    else
                    {
                        string numbers = null;
                        int j = 0;
                        foreach (DataRow dataRowType in telephoneTypeDataTable.Rows)
                        {
                            numbers += dataRowType.ItemArray[0].ToString();
                            if (j != Convert.ToInt32(telephoneTypeDataTable.Rows.Count - 1)) numbers += "; ";
                            j++;
                        }
                        this.clientStepDataTable.Columns.Add("Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":", typeof(string));
                        this.clientStepDataTable.Rows[0]["Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":"] = numbers;
                        table.AddCell(new Cell(1, 1).Add(new Paragraph("Telefone " + dataRow.ItemArray[3].ToString().ToLower() + ":")).AddStyle(titleStyle));
                        table.AddCell(new Cell(1, 7).Add(new Paragraph(numbers.ToString())).AddStyle(bodyStyle).SetTextAlignment(TextAlignment.LEFT));
                    }
                }
            }
        }

        //MENU DO PRIMEIRO PASSO DO PAINEL DE ORÇAMENTOS

        //CONFIRMAR ORÇAMENTO
        private void pcb_btnConfirmBudget_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnConfirmBudget.Image = Properties.Resources.btn_confirmBudget_active;
        }

        private void pcb_btnConfirmBudget_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnConfirmBudget.Image = Properties.Resources.btn_confirmBudget;
        }

        private void pcb_btnConfirmBudget_Click(object sender, EventArgs e)
        {
            if(this.cbb_budgetNumber.SelectedIndex == -1) MessageBox.Show("Selecione um orçamento, para poder confirmá-lo!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Budget budget = new Budget();
                budget.numeroOrcamento = budgetNumber;
                budget.orcamentoConfirmado = true;
                if (Database.confirmBudget(budget))
                {
                    MessageBox.Show("Orçamento confirmado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.pcb_btnConfirmBudget.Visible = false;
                    this.pcb_btnPrint.Location = new System.Drawing.Point(6, 143);
                }
                else MessageBox.Show("[ERRO] Não foi possível confirmar orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //GERAR PDF DO ORÇAMENTO
        private void pcb_btnPrint_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnPrint.Image = Properties.Resources.btn_print_active;
        }

        private void pcb_btnPrint_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnPrint.Image = Properties.Resources.btn_print;
        }

        private void pcb_btnPrint_Click(object sender, EventArgs e)
        {
            if(this.cbb_clientName.SelectedIndex == -1 || this.budgetNumber == 0)
            {
                MessageBox.Show("Selecione um cliente e um orçamento para imprimi-lo.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.sfd_saveBudget.Filter = "Portable Document File (.pdf)|*.pdf";

                //Abrir a janela "Salvar arquivo"
                if (this.sfd_saveBudget.ShowDialog() == DialogResult.Cancel) return;
                else
                {
                    using (PdfWriter pdfWriter = new PdfWriter(sfd_saveBudget.FileName, new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)))
                    {
                        //Document
                        var pdfDocument = new PdfDocument(pdfWriter);
                        var document = new Document(pdfDocument, PageSize.A4);

                        //Font
                        string avengeance = path + @"\assets\fonts\avengeance.ttf";

                        PdfFont pdfFontBody = PdfFontFactory.CreateFont(FontConstants.COURIER);
                        PdfFont pdfFontCompanyName = PdfFontFactory.CreateFont(avengeance, true);
                        PdfFont pdfFontTitle = PdfFontFactory.CreateFont(FontConstants.TIMES_ROMAN);

                        //Color
                        DeviceRgb deviceRgbTitle = new DeviceRgb(190, 255, 255);

                        //Styles
                        Style companyStyle = new Style();
                        companyStyle.SetBorder(Border.NO_BORDER).SetFont(pdfFontCompanyName).SetFontSize(48).SetPaddingBottom(-20).SetTextAlignment(TextAlignment.CENTER);

                        Style titleStyle = new Style();
                        titleStyle.SetBorder(Border.NO_BORDER).SetFont(pdfFontTitle).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER);

                        Style bodyStyle = new Style();
                        bodyStyle.SetBorder(Border.NO_BORDER).SetFont(pdfFontBody).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER);

                        //Table 1
                        float[] columnWidth = new float[] { 140, 140, 140, 140, 140, 140, 140, 140 };

                        Table headerTable = new Table(columnWidth);

                        //Row 1
                        headerTable.AddCell(new Cell(6, 4).Add(new Paragraph()).SetBorder(Border.NO_BORDER));
                        headerTable.AddCell(new Cell(1, 4).Add(new Paragraph("Sisteg")).AddStyle(companyStyle));

                        //Row 2
                        headerTable.AddCell(new Cell(1, 4).Add(new Paragraph("SEGURANÇA ELETRÔNICA")).AddStyle(titleStyle));

                        //Row 3
                        headerTable.AddCell(new Cell(1, 4).Add(new Paragraph("Rua professora Angelina de Felice Mesanelli, 152")).AddStyle(bodyStyle));

                        //Row 4
                        headerTable.AddCell(new Cell(1, 4).Add(new Paragraph("Jd. Victório Lucato - Limeira - SP")).AddStyle(bodyStyle));

                        //Row 5
                        headerTable.AddCell(new Cell(1, 2).Add(new Paragraph("Telefone:")).AddStyle(titleStyle));
                        headerTable.AddCell(new Cell(1, 2).Add(new Paragraph("(19) 98881-1660")).AddStyle(bodyStyle));

                        //Row 6
                        headerTable.AddCell(new Cell(1, 2).Add(new Paragraph("CNPJ:")).AddStyle(titleStyle));
                        headerTable.AddCell(new Cell(1, 2).Add(new Paragraph("23.524.384/0001-47")).AddStyle(bodyStyle));

                        DataTable budgetDataTable = Database.query("SELECT * FROM orcamento WHERE numeroOrcamento = " + this.budgetNumber);

                        document.Add(headerTable);

                        //Table 2
                        float[] clientColumnWidth = new float[] { 160, 140, 140, 140, 140, 140, 140, 120 };

                        Table clientTable = new Table(clientColumnWidth);

                        //Row 7
                        if (this.ckb_technicalReport.Checked) headerTable.AddCell(new Cell(1, 8).Add(new Paragraph("LAUDO TÉCNICO:")).AddStyle(titleStyle).SetFontSize(12));
                        else clientTable.SetMarginTop(20);

                        //Row 1
                        clientTable.AddCell(new Cell(1, 1).Add(new Paragraph("PEDIDO:")).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                        clientTable.AddCell(new Cell(1, 1).Add(new Paragraph(budgetDataTable.Rows[0].ItemArray[0].ToString())).AddStyle(bodyStyle).SetBackgroundColor(deviceRgbTitle).SetTextAlignment(TextAlignment.LEFT));
                        clientTable.AddCell(new Cell(1, 4).Add(new Paragraph()).SetBorder(Border.NO_BORDER).SetBackgroundColor(deviceRgbTitle));
                        clientTable.AddCell(new Cell(1, 1).Add(new Paragraph("DATA:")).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                        clientTable.AddCell(new Cell(1, 1).Add(new Paragraph(Convert.ToDateTime(budgetDataTable.Rows[0].ItemArray[2]).ToShortDateString())).AddStyle(bodyStyle).SetBackgroundColor(deviceRgbTitle).SetTextAlignment(TextAlignment.LEFT));

                        //Row 2
                        clientTable.AddCell(new Cell(1, 1).Add(new Paragraph("Nome:")).AddStyle(titleStyle));
                        clientTable.AddCell(new Cell(1, 7).Add(new Paragraph(clientStepDataTable.Rows[0].ItemArray[1].ToString())).AddStyle(bodyStyle).SetTextAlignment(TextAlignment.LEFT));

                        //Row 3
                        clientTable.AddCell(new Cell(1, 1).Add(new Paragraph("Endereço:")).AddStyle(titleStyle));
                        clientTable.AddCell(new Cell(1, 7).Add(new Paragraph(clientStepDataTable.Rows[0].ItemArray[2].ToString() + ", " + clientStepDataTable.Rows[0].ItemArray[3].ToString())).AddStyle(bodyStyle).SetTextAlignment(TextAlignment.LEFT));

                        //Row 4
                        clientTable.AddCell(new Cell(1, 1).Add(new Paragraph("Cidade:")).AddStyle(titleStyle));
                        clientTable.AddCell(new Cell(1, 5).Add(new Paragraph(clientStepDataTable.Rows[0].ItemArray[4].ToString())).AddStyle(bodyStyle).SetTextAlignment(TextAlignment.LEFT));
                        clientTable.AddCell(new Cell(1, 1).Add(new Paragraph("Estado:")).AddStyle(titleStyle));
                        clientTable.AddCell(new Cell(1, 1).Add(new Paragraph(clientStepDataTable.Rows[0].ItemArray[5].ToString())).AddStyle(bodyStyle).SetTextAlignment(TextAlignment.LEFT));

                        DataTable telephoneDataTable = Database.query("SELECT * FROM telefone WHERE idCliente = " + clientStepDataTable.Rows[0].ItemArray[0] + " ORDER BY tipoTelefone ASC;");
                        this.listTelephones(telephoneDataTable, clientTable, titleStyle, bodyStyle);

                        clientTable.SetBorder(new SolidBorder(deviceRgbTitle, 1));

                        document.Add(clientTable);

                        //Table 3
                        float[] budgetedProductColumnWidth = new float[] { 62, 112, 162, 112, 112 };

                        Table budgetedProductTable = new Table(budgetedProductColumnWidth);

                        DataTable productStepBudgetedProduct = Database.query("SELECT produtoOrcado.idProdutoOrcado, produtoOrcado.item AS 'Item:', produtoOrcado.quantidadeProduto AS 'Quantidade:', produto.nomeProduto AS 'Nome do produto:', produto.valorUnitario AS 'Valor unitário:', produtoOrcado.valorTotal AS 'Valor total:' FROM produtoOrcado JOIN produto ON produtoOrcado.idProduto = produto.idProduto WHERE produtoOrcado.numeroOrcamento = " + this.budgetNumber + " ORDER BY produtoOrcado.item;");

                        budgetedProductTable.SetMarginTop(20);

                        //Row 1
                        budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph("Item:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                        budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph("Quantidade:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                        budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph("Nome do produto:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                        budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph("Valor unitário:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                        budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph("Valor total:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));

                        int j = 0;
                        foreach (DataRow dataRow in productStepBudgetedProduct.Rows)
                        {
                            for (int i = 1; i < 6; i++)
                            {
                                if (j == productStepBudgetedProduct.Rows.Count - 1)
                                {
                                    if (i == 4 || i == 5) budgetedProductTable.AddCell(new Cell().Add(new Paragraph(Convert.ToDecimal(dataRow.ItemArray[i]).ToString("C"))).AddStyle(bodyStyle).SetBorderBottom(new SolidBorder(deviceRgbTitle, 1)));
                                    else budgetedProductTable.AddCell(new Cell().Add(new Paragraph(dataRow.ItemArray[i].ToString())).AddStyle(bodyStyle).SetBorderBottom(new SolidBorder(deviceRgbTitle, 1)));
                                }
                                else
                                {
                                    if (i == 4 || i == 5) budgetedProductTable.AddCell(new Cell().Add(new Paragraph(Convert.ToDecimal(dataRow.ItemArray[i]).ToString("C"))).AddStyle(bodyStyle));
                                    else budgetedProductTable.AddCell(new Cell().Add(new Paragraph(dataRow.ItemArray[i].ToString())).AddStyle(bodyStyle));
                                }
                            }
                            j++;
                        }

                        budgetedProductTable.AddCell(new Cell(1, 3).Add(new Paragraph()).SetBorder(Border.NO_BORDER).SetBackgroundColor(deviceRgbTitle));
                        budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph("Valor do trabalho:")).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                        budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph(Convert.ToDecimal(budgetDataTable.Rows[0].ItemArray[3]).ToString("C"))).AddStyle(bodyStyle).SetBackgroundColor(deviceRgbTitle));

                        budgetedProductTable.AddCell(new Cell(1, 2).Add(new Paragraph("Condigação de pagamento:")).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                        budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph(budgetDataTable.Rows[0].ItemArray[5].ToString())).AddStyle(bodyStyle).SetBackgroundColor(deviceRgbTitle));
                        budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph("TOTAL R$:")).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                        budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph(Convert.ToDecimal(budgetDataTable.Rows[0].ItemArray[4]).ToString("C"))).AddStyle(bodyStyle).SetBackgroundColor(deviceRgbTitle));

                        budgetedProductTable.SetBorder(new SolidBorder(deviceRgbTitle, 1));

                        document.Add(budgetedProductTable);

                        document.Close();

                        System.Diagnostics.Process.Start(this.sfd_saveBudget.FileName);
                    }
                }
            }
        }

        //ENVIAR ORÇAMENTO VIA WHATSAPP
        private void pcb_btnSendBudget_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnSendBudget.Image = Properties.Resources.btn_sendBudget_active;
        }

        private void pcb_btnSendBudget_MouseLeave(object sender, EventArgs e)
        {
            this.pcb_btnSendBudget.Image = Properties.Resources.btn_sendBudget;
        }

        private void pcb_btnSendBudget_Click(object sender, EventArgs e)
        {
            /*string id = "AC874c2cf569ea94a03ecfd2719609f523";
            string token = "306c7a60d2ec2a331fd9351fb2b4a935";
            TwilioClient.Init(id, token);
            var message = MessageResource.Create(
                body: "Olá, aqui é da Sisteg",
                from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                to: new Twilio.Types.PhoneNumber("whatsapp:+5519987213927")
           );*/

            try
            {
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    using (MailMessage mailMessage = new MailMessage())
                    {
                        this.ofd_sendBudget.Filter = "Portable Document File (.pdf)|*.pdf";

                        //Abrir a janela "Salvar arquivo"
                        if (this.ofd_sendBudget.ShowDialog() == DialogResult.Cancel) return;
                        else
                        {
                            //Servidor SMTP
                            if (this.clientStepDataTable.Rows[0].ItemArray[7].ToString().Contains("gmail")) smtpClient.Host = "smtp.gmail.com";
                            else smtpClient.Host = "smtp-mail.outlook.com";
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = new NetworkCredential("wrpblimeira@hotmail.com", "Wesley20082000");
                            smtpClient.Port = 587;
                            smtpClient.EnableSsl = true;

                            //Email (Mensagem)
                            mailMessage.From = new MailAddress("wrpblimeira@hotmail.com");
                            mailMessage.To.Add(clientStepDataTable.Rows[0].ItemArray[7].ToString());
                            mailMessage.Subject = "Orçamento";
                            mailMessage.IsBodyHtml = false;
                            //mailMessage.Body = "Olá, aqui é da Sisteg";

                            //Anexo do email
                            mailMessage.Attachments.Add(new Attachment(this.ofd_sendBudget.FileName));

                            //Enviar email
                            smtpClient.Send(mailMessage);
                            MessageBox.Show("Email enviado!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }   
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Erro: " + exception.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //BUSCAR CLIENTE
        private void txt_searchClient_TextChange(object sender, EventArgs e)
        {
            if(this.txt_searchClient.Text.Trim() != null)
            {
                DataTable searchClientDataTable = Database.query("SELECT cliente.nomeCliente FROM cliente WHERE cliente.nomeCliente LIKE '%" + this.txt_searchClient.Text.Trim() + "%' ORDER BY cliente.nomeCliente;");
                this.cbb_clientName.Items.Clear();
                for (int i = 0; i < searchClientDataTable.Rows.Count; i++) this.cbb_clientName.Items.Insert(i, " " + searchClientDataTable.Rows[i].ItemArray[0].ToString());
            }
        }

        //SELECIONAR CLIENTE
        private void cbb_clientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Esconde os controles do formulário
            this.clientStepBudgetForm.pcb_btnEdit.Visible = false;
            this.pcb_btnConfirmBudget.Visible = false;
            this.pcb_btnPrint.Visible = false;
            this.pcb_btnSendBudget.Visible = false;

            //Todos os dados do cliente selecionado
            clientStepDataTable = Database.query("SELECT * FROM cliente WHERE cliente.nomeCliente = '" + cbb_clientName.SelectedItem.ToString().Trim() + "';");
            //Busca os orçamentos do cliente
            budgetStepDataTable = Database.query("SELECT orcamento.* FROM orcamento INNER JOIN cliente ON cliente.idCliente = orcamento.idCliente WHERE cliente.nomeCliente = '" + cbb_clientName.SelectedItem.ToString().Trim() + "';");

            //Popula o combobox de orçamentos do cliente
            this.cbb_budgetNumber.Items.Clear();
            for (int i = 0; i < Convert.ToInt32(budgetStepDataTable.Rows.Count); i++) if (!this.cbb_budgetNumber.Items.Contains(" " + (i + 1).ToString())) this.cbb_budgetNumber.Items.Insert(i, " " + (i + 1).ToString());

            //Cliente tem orçamento
            if (budgetStepDataTable.Rows.Count > 0)
            {                
                if (Convert.ToInt32(budgetStepDataTable.Rows.Count) == 1)
                {
                    //Cliente tem apenas um orçamento
                    this.cbb_budgetNumber.Visible = false;
                    this.ckb_technicalReport.Location = new System.Drawing.Point(13, 59);
                    this.lbl_technicalReport.Location = new System.Drawing.Point(50, 59);
                    this.ckb_technicalReport.Visible = true;
                    this.lbl_technicalReport.Visible = true;
                    this.cbb_budgetNumber.SelectedIndex = 0;
                    this.budgetNumber = Convert.ToInt32(budgetStepDataTable.Rows[0].ItemArray[0]);

                    //Função que checa se o orçamento foi confirmado
                    this.isBudgetConfirmed(0);
                }
                else
                {
                    //Cliente tem mais de um orçamento
                    this.cbb_budgetNumber.Visible = true;
                    this.ckb_technicalReport.Location = new System.Drawing.Point(13, 107);
                    this.lbl_technicalReport.Location = new System.Drawing.Point(50, 107);
                    this.ckb_technicalReport.Visible = true;
                    this.lbl_technicalReport.Visible = true;
                    this.cbb_budgetNumber.SelectedIndex = -1;
                    this.cbb_budgetNumber.Text = " Número do orçamento";
                }
            }
            //Cliente não tem orçamento
            else
            {
                this.cbb_budgetNumber.Visible = false;
                this.ckb_technicalReport.Visible = true;
                this.lbl_technicalReport.Visible = true;
            }
        }

        //SELECIONAR ORÇAMENTO DO CLIENTE
        private void cbb_budgetNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            for(int i = 0; i < budgetStepDataTable.Rows.Count; i++) 
            { 
                if(this.cbb_budgetNumber.SelectedIndex != -1)
                {
                    if ((i + 1) == Convert.ToInt32(this.cbb_budgetNumber.SelectedItem.ToString().Trim()))
                    {
                        //Número do orçamento (chave primária) no banco de dados
                        budgetNumber = Convert.ToInt32(budgetStepDataTable.Rows[i].ItemArray[0]);
                        clientStepBudgetForm.budgetNumber = this.budgetNumber;

                        //Função que checa se o orçamento foi confirmado
                        this.isBudgetConfirmed(0);
                        if (string.IsNullOrEmpty(clientStepDataTable.Rows[0].ItemArray[7].ToString()))
                        {
                            this.pcb_btnSendBudget.Visible = false;
                            MessageBox.Show("Este cliente não tem endereço de e-mail!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }
    }
}
