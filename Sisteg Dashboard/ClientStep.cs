using System;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Sisteg_Dashboard
{
    public partial class ClientStep : Form
    {
        //INICIA INSTÂNCIA DO PAINEL, POPULANDO O COMBOBOX DE LISTAGEM DE CLIENTES
        public ClientStep(BudgetForm budgetForm, DataTable dataTable)
        {
            InitializeComponent();
            Globals.budgetForm = budgetForm;

            //Popula o combobox de clientes
            Globals.clientStepDataTable = Database.query("SELECT cliente.nomeCliente FROM cliente ORDER BY cliente.nomeCliente;");
            for (int i = 0; i < Globals.clientStepDataTable.Rows.Count; i++) this.cbb_clientName.Items.Insert(i, " " + Globals.clientStepDataTable.Rows[i].ItemArray[0].ToString().Trim());

            //Cliente já tem orçamento
            //Usuário avançou e retornou a este formulário
            if (dataTable != null)
            {
                Globals.clientStepDataTable = dataTable;
                this.cbb_clientName.SelectedIndex = this.cbb_clientName.FindString(" " + Globals.clientStepDataTable.Rows[0].ItemArray[1].ToString().Trim());
                //if (Globals.budgetForm.selectedIndex != -1) this.cbb_budgetNumber.SelectedIndex = Globals.budgetForm.selectedIndex;
                Globals.budgetStepDataTable = Database.query("SELECT * FROM orcamento WHERE idCliente = " + Globals.clientStepDataTable.Rows[0].ItemArray[0]);
                for (int i = 0; i < Globals.budgetStepDataTable.Rows.Count; i++)
                {
                    if (Globals.numeroOrcamento == Convert.ToInt32(Globals.budgetStepDataTable.Rows[i].ItemArray[0]))
                    {
                        this.cbb_budgetNumber.SelectedIndex = this.cbb_budgetNumber.FindString(" " + (i + 1).ToString().Trim());
                        this.isBudgetConfirmed(i);
                    }
                }

                //Esconde os controles do formulário caso nenhum orçamento esteja selecionado
                if (this.cbb_budgetNumber.SelectedIndex == -1)
                {
                    this.lbl_btnConfirmBudgetTag.Visible = false;
                    this.pcb_btnConfirmBudget.Visible = false;
                    this.lbl_btnPrintBudgetTag.Visible = false;
                    this.pcb_btnPrint.Visible = false;
                    this.lbl_btnSendBudgetTag.Visible = false;
                    this.pcb_btnSendBudget.Visible = false;
                    this.ckb_technicalReport.Visible = false;
                    this.lbl_technicalReport.Visible = false;
                    this.pcb_btnEndBudget.Visible = false;
                    this.lbl_btnEndBudgetTag.Visible = false;
                }
            }
            //Cliente não tem orçamento
            else
            {
                //Esconde os controles do formulário
                this.cbb_budgetNumber.Visible = false;
                Globals.budgetForm.lbl_btnEditTag.Visible = false;
                Globals.budgetForm.pcb_btnEdit.Visible = false;
                this.lbl_btnConfirmBudgetTag.Visible = false;
                this.pcb_btnConfirmBudget.Visible = false;
                this.lbl_btnPrintBudgetTag.Visible = false;
                this.pcb_btnPrint.Visible = false;
                this.lbl_btnSendBudgetTag.Visible = false;
                this.pcb_btnSendBudget.Visible = false;
                this.ckb_technicalReport.Visible = false;
                this.lbl_technicalReport.Visible = false;
                this.pcb_btnEndBudget.Visible = false;
                this.lbl_btnEndBudgetTag.Visible = false;
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

        //FUNÇÕES

        //Função que checa se o orçamento foi confirmado
        public void isBudgetConfirmed(int i)
        {
            //Mostra o controle escondido no formulário
            Globals.budgetForm.lbl_btnEditTag.Visible = true;
            Globals.budgetForm.pcb_btnEdit.Visible = true;

            //Orçamento não confirmado
            if (!Convert.ToBoolean(Globals.budgetStepDataTable.Rows[i].ItemArray[6]))
            {
                this.lbl_btnConfirmBudgetTag.Visible = true;
                this.pcb_btnConfirmBudget.Visible = true;
            }

            //Mostra os controles escondidos do formulário
            this.lbl_btnPrintBudgetTag.Visible = true;
            this.pcb_btnPrint.Visible = true;
            this.lbl_btnSendBudgetTag.Visible = true;
            this.pcb_btnSendBudget.Visible = true;
            this.lbl_btnEndBudgetTag.Visible = true;
            this.pcb_btnEndBudget.Visible = true;
        }

        //Função que lista os telefones do cliente na tabela de clientes
        private void listTelephones(DataTable telephoneDataTable, Table table, Style titleStyle, Style bodyStyle)
        {
            foreach (DataRow dataRow in telephoneDataTable.Rows)
            {
                DataTable telephoneTypeDataTable = Database.query("SELECT numeroTelefone FROM telefone WHERE idCliente = " + Globals.clientStepDataTable.Rows[0].ItemArray[0] + " AND tipoTelefone = '" + dataRow.ItemArray[3] + "';");
                if (telephoneTypeDataTable.Rows.Count == 1)
                {
                    if (Globals.clientStepDataTable.Columns.Contains("Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":"))
                    {
                        Globals.clientStepDataTable.Rows[0]["Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":"] = dataRow.ItemArray[4];
                        break;
                    }
                    else
                    {
                        Globals.clientStepDataTable.Columns.Add("Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":", typeof(string));
                        Globals.clientStepDataTable.Rows[0]["Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":"] = dataRow.ItemArray[4];
                        table.AddCell(new Cell(1, 1).Add(new Paragraph("Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":")).AddStyle(titleStyle));
                        table.AddCell(new Cell(1, 7).Add(new Paragraph(dataRow.ItemArray[4].ToString().Trim())).AddStyle(bodyStyle).SetTextAlignment(TextAlignment.LEFT));
                    }
                }
                else if (telephoneTypeDataTable.Rows.Count > 1)
                {
                    if (Globals.clientStepDataTable.Columns.Contains("Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":"))
                    {
                        Globals.clientStepDataTable.Rows[0]["Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":"] = dataRow.ItemArray[4];
                        break;
                    }
                    else
                    {
                        string numbers = null;
                        int j = 0;
                        foreach (DataRow dataRowType in telephoneTypeDataTable.Rows)
                        {
                            numbers += dataRowType.ItemArray[0].ToString().Trim();
                            if (j != Convert.ToInt32(telephoneTypeDataTable.Rows.Count - 1)) numbers += "; ";
                            j++;
                        }
                        Globals.clientStepDataTable.Columns.Add("Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":", typeof(string));
                        Globals.clientStepDataTable.Rows[0]["Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":"] = numbers;
                        table.AddCell(new Cell(1, 1).Add(new Paragraph("Telefone " + dataRow.ItemArray[3].ToString().Trim().ToLower() + ":")).AddStyle(titleStyle));
                        table.AddCell(new Cell(1, 7).Add(new Paragraph(numbers.ToString().Trim())).AddStyle(bodyStyle).SetTextAlignment(TextAlignment.LEFT));
                    }
                }
            }
        }

        //Função que confirma orçamento
        private void confirmBudget()
        {
            if (this.cbb_budgetNumber.SelectedIndex == -1) MessageBox.Show("Selecione um orçamento, para poder confirmá-lo!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                Budget budget = new Budget();
                budget.NumeroOrcamento = Globals.numeroOrcamento;
                budget.OrcamentoConfirmado = true;
                if (Database.confirmBudget(budget))
                {
                    MessageBox.Show("Orçamento confirmado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.lbl_btnConfirmBudgetTag.Visible = false;
                    this.pcb_btnConfirmBudget.Visible = false;
                    this.lbl_btnPrintBudgetTag.Location = new System.Drawing.Point(89, 171);
                    this.pcb_btnPrint.Location = new System.Drawing.Point(6, 143);
                }
                else MessageBox.Show("[ERRO] Não foi possível confirmar orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Função que gera um PDF do orçamento
        private void printBudget()
        {
            if (this.cbb_clientName.SelectedIndex == -1 || Globals.numeroOrcamento == 0) MessageBox.Show("Selecione um cliente e um orçamento para imprimi-lo.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                this.sfd_saveBudget.Filter = "Portable Document File (.pdf)|*.pdf";

                //Abrir a janela "Salvar arquivo"
                if (this.sfd_saveBudget.ShowDialog() == DialogResult.Cancel) return;
                else
                {
                    try
                    {
                        using (PdfWriter pdfWriter = new PdfWriter(sfd_saveBudget.FileName, new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)))
                        {
                            //Document
                            var pdfDocument = new PdfDocument(pdfWriter);
                            var document = new Document(pdfDocument, PageSize.A4);

                            //Font
                            string avengeance = Globals.path + @"\assets\fonts\avengeance.ttf";

                            //Image
                            string logo = Globals.path + @"\assets\img\icon\logo.jpg";
                            iText.IO.Image.ImageData imageData = iText.IO.Image.ImageDataFactory.Create(logo);
                            Image image = new Image(imageData);
                            image.SetWidth(150).SetHeight(150);

                            PdfFont pdfFontBody = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.COURIER);
                            PdfFontFactory.EmbeddingStrategy embeddingStrategy = new PdfFontFactory.EmbeddingStrategy();
                            PdfFont pdfFontCompanyName = PdfFontFactory.CreateFont(avengeance, embeddingStrategy); //PdfFontFactory.CreateFont(avengeance, true);
                            PdfFont pdfFontTitle = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.TIMES_ROMAN);

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
                            headerTable.AddCell(new Cell(6, 4).Add(image).SetBorder(Border.NO_BORDER).SetPaddingLeft(60).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER));
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

                            DataTable budgetDataTable = Database.query("SELECT * FROM orcamento WHERE numeroOrcamento = " + Globals.numeroOrcamento);

                            //Table 2
                            float[] clientColumnWidth = new float[] { 160, 140, 140, 140, 140, 140, 140, 120 };

                            Table clientTable = new Table(clientColumnWidth);

                            //Row 7
                            if (this.ckb_technicalReport.Checked) headerTable.AddCell(new Cell(1, 8).Add(new Paragraph("LAUDO TÉCNICO:")).AddStyle(titleStyle).SetFontSize(12));
                            else clientTable.SetMarginTop(20);

                            document.Add(headerTable);

                            //Row 1
                            clientTable.AddCell(new Cell(1, 1).Add(new Paragraph("PEDIDO:")).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                            clientTable.AddCell(new Cell(1, 1).Add(new Paragraph(budgetDataTable.Rows[0].ItemArray[0].ToString())).AddStyle(bodyStyle).SetBackgroundColor(deviceRgbTitle).SetTextAlignment(TextAlignment.LEFT));
                            clientTable.AddCell(new Cell(1, 4).Add(new Paragraph()).SetBorder(Border.NO_BORDER).SetBackgroundColor(deviceRgbTitle));
                            clientTable.AddCell(new Cell(1, 1).Add(new Paragraph("DATA:")).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                            clientTable.AddCell(new Cell(1, 1).Add(new Paragraph(Convert.ToDateTime(budgetDataTable.Rows[0].ItemArray[2]).ToShortDateString().Trim())).AddStyle(bodyStyle).SetBackgroundColor(deviceRgbTitle).SetTextAlignment(TextAlignment.LEFT));

                            //Row 2
                            clientTable.AddCell(new Cell(1, 1).Add(new Paragraph("Nome:")).AddStyle(titleStyle));
                            clientTable.AddCell(new Cell(1, 7).Add(new Paragraph(Globals.clientStepDataTable.Rows[0].ItemArray[1].ToString().Trim())).AddStyle(bodyStyle).SetTextAlignment(TextAlignment.LEFT));

                            //Row 3
                            clientTable.AddCell(new Cell(1, 1).Add(new Paragraph("Endereço:")).AddStyle(titleStyle));
                            clientTable.AddCell(new Cell(1, 7).Add(new Paragraph(Globals.clientStepDataTable.Rows[0].ItemArray[2].ToString().Trim() + ", " + Globals.clientStepDataTable.Rows[0].ItemArray[3].ToString())).AddStyle(bodyStyle).SetTextAlignment(TextAlignment.LEFT));

                            //Row 4
                            clientTable.AddCell(new Cell(1, 1).Add(new Paragraph("Cidade:")).AddStyle(titleStyle));
                            clientTable.AddCell(new Cell(1, 5).Add(new Paragraph(Globals.clientStepDataTable.Rows[0].ItemArray[4].ToString().Trim())).AddStyle(bodyStyle).SetTextAlignment(TextAlignment.LEFT));
                            clientTable.AddCell(new Cell(1, 1).Add(new Paragraph("Estado:")).AddStyle(titleStyle));
                            clientTable.AddCell(new Cell(1, 1).Add(new Paragraph(Globals.clientStepDataTable.Rows[0].ItemArray[5].ToString().Trim())).AddStyle(bodyStyle).SetTextAlignment(TextAlignment.LEFT));

                            DataTable telephoneDataTable = Database.query("SELECT * FROM telefone WHERE idCliente = " + Globals.clientStepDataTable.Rows[0].ItemArray[0] + " ORDER BY tipoTelefone ASC;");
                            this.listTelephones(telephoneDataTable, clientTable, titleStyle, bodyStyle);

                            clientTable.SetBorder(new SolidBorder(deviceRgbTitle, 1));

                            document.Add(clientTable);

                            //Table 3
                            float[] budgetedProductColumnWidth = new float[] { 62, 112, 162, 112, 112 };

                            Table budgetedProductTable = new Table(budgetedProductColumnWidth);

                            DataTable productStepBudgetedProduct = Database.query("SELECT produtoOrcado.idProdutoOrcado, produtoOrcado.item AS 'Item:', produtoOrcado.quantidadeProduto AS 'Quantidade:', produto.nomeProduto AS 'Nome do produto:', produto.valorUnitario AS 'Valor unitário:', produtoOrcado.valorTotal AS 'Valor total:' FROM produtoOrcado JOIN produto ON produtoOrcado.idProduto = produto.idProduto WHERE produtoOrcado.numeroOrcamento = " + Globals.numeroOrcamento + " ORDER BY produtoOrcado.item;");

                            budgetedProductTable.SetMarginTop(20);

                            //Row 1
                            if(productStepBudgetedProduct.Rows.Count > 0)
                            {
                                budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph("Item:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                                budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph("Quantidade:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                                budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph("Nome do produto:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                                budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph("Valor unitário:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                                budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph("Valor total:")).SetBorder(Border.NO_BORDER).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                            }

                            int j = 0;
                            foreach (DataRow dataRow in productStepBudgetedProduct.Rows)
                            {
                                for (int i = 1; i < 6; i++)
                                {
                                    if (j == productStepBudgetedProduct.Rows.Count - 1)
                                    {
                                        if (i == 4 || i == 5) budgetedProductTable.AddCell(new Cell().Add(new Paragraph(Convert.ToDecimal(dataRow.ItemArray[i]).ToString("C").Trim())).AddStyle(bodyStyle).SetBorderBottom(new SolidBorder(deviceRgbTitle, 1)));
                                        else budgetedProductTable.AddCell(new Cell().Add(new Paragraph(dataRow.ItemArray[i].ToString().Trim())).AddStyle(bodyStyle).SetBorderBottom(new SolidBorder(deviceRgbTitle, 1)));
                                    }
                                    else
                                    {
                                        if (i == 4 || i == 5) budgetedProductTable.AddCell(new Cell().Add(new Paragraph(Convert.ToDecimal(dataRow.ItemArray[i]).ToString("C").Trim())).AddStyle(bodyStyle));
                                        else budgetedProductTable.AddCell(new Cell().Add(new Paragraph(dataRow.ItemArray[i].ToString().Trim())).AddStyle(bodyStyle));
                                    }
                                }
                                j++;
                            }

                            budgetedProductTable.AddCell(new Cell(1, 3).Add(new Paragraph()).SetBorder(Border.NO_BORDER).SetBackgroundColor(deviceRgbTitle));
                            budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph("Valor do trabalho:")).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                            budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph(Convert.ToDecimal(budgetDataTable.Rows[0].ItemArray[3]).ToString("C").Trim())).AddStyle(bodyStyle).SetBackgroundColor(deviceRgbTitle));

                            budgetedProductTable.AddCell(new Cell(1, 2).Add(new Paragraph("Condigação de pagamento:")).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                            budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph(budgetDataTable.Rows[0].ItemArray[5].ToString().Trim())).AddStyle(bodyStyle).SetBackgroundColor(deviceRgbTitle));
                            budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph("TOTAL R$:")).AddStyle(titleStyle).SetBackgroundColor(deviceRgbTitle));
                            budgetedProductTable.AddCell(new Cell(1, 1).Add(new Paragraph(Convert.ToDecimal(budgetDataTable.Rows[0].ItemArray[4]).ToString("C").Trim())).AddStyle(bodyStyle).SetBackgroundColor(deviceRgbTitle));

                            budgetedProductTable.SetBorder(new SolidBorder(deviceRgbTitle, 1));

                            document.Add(budgetedProductTable);

                            document.Close();

                            System.Diagnostics.Process.Start(this.sfd_saveBudget.FileName);
                        }
                    }catch(Exception exception)
                    {
                        MessageBox.Show("[ERRO] " + exception.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //Função que envia orçamento via e-mail
        private void sendBudget()
        {
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
                            if (Globals.clientStepDataTable.Rows[0].ItemArray[7].ToString().Trim().Contains("gmail")) smtpClient.Host = "smtp.gmail.com";
                            else smtpClient.Host = "smtp-mail.outlook.com";
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = new NetworkCredential("jpblimeira@hotmail.com", "brasil2012");
                            smtpClient.Port = 587;
                            smtpClient.EnableSsl = true;

                            //Email (Mensagem)
                            mailMessage.From = new MailAddress("jpblimeira@hotmail.com", "Sisteg - Segurança eletrônica");
                            mailMessage.To.Add(Globals.clientStepDataTable.Rows[0].ItemArray[7].ToString());
                            mailMessage.Subject = "Orçamento";
                            mailMessage.IsBodyHtml = false;

                            //Anexo do email
                            mailMessage.Attachments.Add(new Attachment(this.ofd_sendBudget.FileName));

                            //Enviar email
                            smtpClient.Send(mailMessage);
                            MessageBox.Show("E-mail enviado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("[ERRO] " + exception.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Função que encerra orçamento
        private void endBudget()
        {
            Budget budget = new Budget();
            budget.NumeroOrcamento = Globals.numeroOrcamento;

            //Lista os clientes que estão em divida
            bool allParcelsPaid = true;
            bool allIncomesPaid = true;
            bool allBudgetsPaid = true;
            DataTable budgetsDataTable = Database.query("SELECT numeroOrcamento FROM orcamento JOIN cliente ON orcamento.idCliente = cliente.idCliente WHERE cliente.nomeCliente = '" + cbb_clientName.SelectedItem.ToString().Trim() + "';");
            foreach (DataRow budgetsDataRow in budgetsDataTable.Rows)
            {
                //Cliente tem orçamento
                DataTable incomesDataTable = Database.query("SELECT idReceita, recebimentoConfirmado, repetirParcelarReceita FROM receita WHERE numeroOrcamento = " + budgetsDataRow.ItemArray[0]);
                foreach (DataRow incomesDataRow in incomesDataTable.Rows)
                {
                    if (Convert.ToBoolean(incomesDataRow.ItemArray[2]))
                    {
                        //Receita parcelada
                        DataTable parcelsDataTable = Database.query("SELECT idParcela, recebimentoConfirmado FROM parcela WHERE idReceita = " + incomesDataRow.ItemArray[0]);
                        foreach (DataRow parcelsDataRow in parcelsDataTable.Rows)
                        {
                            if (Convert.ToBoolean(parcelsDataRow.ItemArray[1])) continue; //Parcela paga
                            else
                            {
                                //Existe parcela pendente
                                allParcelsPaid = false;
                                allIncomesPaid = false;
                                allBudgetsPaid = false;
                                break;
                            }
                        }
                    }

                    if (Convert.ToBoolean(incomesDataRow.ItemArray[1])) continue; //Parcela paga
                    else
                    {
                        //Existe receita pendente
                        allIncomesPaid = false;
                        allBudgetsPaid = false;
                        break;
                    }
                }
            }

            if (!(allParcelsPaid && allIncomesPaid && allBudgetsPaid)) MessageBox.Show("[ERRO] Não é possível encerrar o orçamento porque o cliente está em dívida!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (Database.deleteAllBudgetedProducts(budget))
            {
                if (Database.deleteBudget(budget))
                {
                        MessageBox.Show("Orçamento encerrado com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.cbb_clientName.SelectedIndex = -1;
                        this.cbb_clientName.Text = "Cliente";
                }
                else MessageBox.Show("[ERRO] Não foi possível encerrar o orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else MessageBox.Show("[ERRO] Não foi possível encerrar o orçamento!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //MENU DO PRIMEIRO PASSO DO PAINEL DE ORÇAMENTOS

        //Confirmar orçamento
        private void pcb_btnConfirmBudget_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnConfirmBudget.BackgroundImage = Properties.Resources.btn_confirm_budget_active;
        }

        private void pcb_btnConfirmBudget_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnConfirmBudgetTag.ClientRectangle.Contains(lbl_btnConfirmBudgetTag.PointToClient(Cursor.Position))) this.pcb_btnConfirmBudget.BackgroundImage = Properties.Resources.btn_confirm_budget;
        }

        private void pcb_btnConfirmBudget_Click(object sender, EventArgs e)
        {
            this.confirmBudget();
        }

        private void lbl_btnConfirmBudgetTag_Click(object sender, EventArgs e)
        {
            this.confirmBudget();
        }

        //Gerar PDF do orçamento
        private void pcb_btnPrint_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnPrint.BackgroundImage = Properties.Resources.btn_print_active;
        }

        private void pcb_btnPrint_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnPrintBudgetTag.ClientRectangle.Contains(lbl_btnPrintBudgetTag.PointToClient(Cursor.Position))) this.pcb_btnPrint.BackgroundImage = Properties.Resources.btn_print;
        }

        private void pcb_btnPrint_Click(object sender, EventArgs e)
        {
            this.printBudget();
        }

        private void lbl_btnPrintBudgetTag_Click(object sender, EventArgs e)
        {
            this.printBudget();
        }

        //Enviar orçamento via e-mail
        private void pcb_btnSendBudget_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnSendBudget.BackgroundImage = Properties.Resources.btn_send_email_active;
        }

        private void pcb_btnSendBudget_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnSendBudgetTag.ClientRectangle.Contains(lbl_btnSendBudgetTag.PointToClient(Cursor.Position))) this.pcb_btnSendBudget.BackgroundImage = Properties.Resources.btn_send_email;
        }

        private void pcb_btnSendBudget_Click(object sender, EventArgs e)
        {
            this.sendBudget();
        }

        private void lbl_btnSendBudgetTag_Click(object sender, EventArgs e)
        {
            this.sendBudget();
        }

        //Encerrar orçamento
        private void pcb_btnEndBudget_MouseEnter(object sender, EventArgs e)
        {
            this.pcb_btnEndBudget.BackgroundImage = Properties.Resources.btn_end_budget_active;
        }

        private void pcb_btnEndBudget_MouseLeave(object sender, EventArgs e)
        {
            if (!lbl_btnEndBudgetTag.ClientRectangle.Contains(lbl_btnEndBudgetTag.PointToClient(Cursor.Position))) this.pcb_btnEndBudget.BackgroundImage = Properties.Resources.btn_end_budget;
        }

        private void pcb_btnEndBudget_Click(object sender, EventArgs e)
        {
            this.endBudget();
        }

        private void lbl_btnEndBudgetTag_Click(object sender, EventArgs e)
        {
            this.endBudget();
        }

        //BUSCAR CLIENTE
        private void txt_searchClient_TextChange(object sender, EventArgs e)
        {
            if(this.txt_searchClient.Text.Trim() != null)
            {
                DataTable searchClientDataTable = Database.query("SELECT cliente.nomeCliente FROM cliente WHERE cliente.nomeCliente LIKE '%" + this.txt_searchClient.Text.Trim() + "%' ORDER BY cliente.nomeCliente;");
                this.cbb_clientName.Items.Clear();
                for (int i = 0; i < searchClientDataTable.Rows.Count; i++) this.cbb_clientName.Items.Insert(i, " " + searchClientDataTable.Rows[i].ItemArray[0].ToString().Trim());
            }
        }

        //SELECIONAR CLIENTE
        private void cbb_clientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Esconde os controles do formulário
            Globals.budgetForm.lbl_btnEditTag.Visible = false;
            Globals.budgetForm.pcb_btnEdit.Visible = false;
            this.lbl_btnConfirmBudgetTag.Visible = false;
            this.pcb_btnConfirmBudget.Visible = false;
            this.lbl_btnPrintBudgetTag.Visible = false;
            this.pcb_btnPrint.Visible = false;
            this.lbl_btnSendBudgetTag.Visible = false;
            this.pcb_btnSendBudget.Visible = false;
            this.ckb_technicalReport.Visible = false;
            this.lbl_technicalReport.Visible = false;
            this.pcb_btnEndBudget.Visible = false;
            this.lbl_btnEndBudgetTag.Visible = false;

            if(this.cbb_clientName.SelectedIndex != -1)
            {
                //Todos os dados do cliente selecionado
                Globals.clientStepDataTable = Database.query("SELECT * FROM cliente WHERE cliente.nomeCliente = '" + cbb_clientName.SelectedItem.ToString().Trim() + "';");
                //Busca os orçamentos do cliente
                Globals.budgetStepDataTable = Database.query("SELECT orcamento.* FROM orcamento INNER JOIN cliente ON cliente.idCliente = orcamento.idCliente WHERE cliente.nomeCliente = '" + cbb_clientName.SelectedItem.ToString().Trim() + "';");

                //Popula o combobox de orçamentos do cliente
                this.cbb_budgetNumber.Items.Clear();
                for (int i = 0; i < Convert.ToInt32(Globals.budgetStepDataTable.Rows.Count); i++) if (!this.cbb_budgetNumber.Items.Contains(" " + (i + 1).ToString().Trim())) this.cbb_budgetNumber.Items.Insert(i, " " + (i + 1).ToString().Trim());

                //Cliente tem orçamento
                if (Globals.budgetStepDataTable.Rows.Count > 0)
                {
                    if (Convert.ToInt32(Globals.budgetStepDataTable.Rows.Count) == 1)
                    {
                        //Cliente tem apenas um orçamento
                        this.cbb_budgetNumber.Visible = false;
                        this.ckb_technicalReport.Location = new System.Drawing.Point(13, 59);
                        this.lbl_technicalReport.Location = new System.Drawing.Point(50, 59);
                        this.ckb_technicalReport.Visible = true;
                        this.lbl_technicalReport.Visible = true;
                        this.cbb_budgetNumber.SelectedIndex = 0;
                        Globals.numeroOrcamento = Convert.ToInt32(Globals.budgetStepDataTable.Rows[0].ItemArray[0]);

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
                    this.ckb_technicalReport.Visible = false;
                    this.lbl_technicalReport.Visible = false;
                }
            }

        }

        //SELECIONAR ORÇAMENTO DO CLIENTE
        private void cbb_budgetNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            for(int i = 0; i < Globals.budgetStepDataTable.Rows.Count; i++) 
            { 
                if(this.cbb_budgetNumber.SelectedIndex != -1)
                {
                    if ((i + 1) == Convert.ToInt32(this.cbb_budgetNumber.SelectedItem.ToString().Trim()))
                    {
                        //Número do orçamento (chave primária) no banco de dados
                        Globals.numeroOrcamento = Convert.ToInt32(Globals.budgetStepDataTable.Rows[i].ItemArray[0]);

                        //Função que checa se o orçamento foi confirmado
                        this.isBudgetConfirmed(0);
                        if (string.IsNullOrEmpty(Globals.clientStepDataTable.Rows[0].ItemArray[7].ToString()))
                        {
                            this.lbl_btnSendBudgetTag.Visible = false;
                            this.pcb_btnSendBudget.Visible = false;
                            MessageBox.Show("Este cliente não tem endereço de e-mail!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }
    }
}
