using DeviceAPI_Csharp_DLL;
using LanguageExt;
using LanguageExt.Pipes;
using Sig.Crypto;
using Sig.DeviceAPI;
using Sig.PdfClient;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Pkcs;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;
using System.Windows.Forms;
using GemBox.Pdf;
using System.Windows;

namespace StepOverModel
{

    public partial class Form1 : Form
    {
        // number of pages
        int pages = 0;

        // Origin PDF file path
        string source;

        // Create an instance of the driver
        public static IDriver driverInterface = new Driver();

        // Create an instance of the IClient
        public IClient clientInterface = ClientFactory.BuildDefault();

        // Form is open
        public Form1(string deviceName0)
        {
            InitializeComponent();

            // Set the device
            Error r = driverInterface.SetDevice(deviceName0);
            if (r == Error.SUCCESS)
            {
                gb_sign.Enabled = true;
                bt_StopSignature.Enabled = false;
                bt_saveImage.Enabled = false;
            }

            // Subscribe to events
            SubscribeToEvents();

            // Set the value of the scroll bar x
            sb_x.Value = int.Parse(tb_x.Text);
            sb_x.Maximum = 595 - int.Parse(tb_sigWidth.Text);

            r = driverInterface.LoadLicense("License/FingerTech.xml");
            ShowErrorMessage(r);

            // Create the tmp folder
            if (!Directory.Exists("tmp"))
            {
                Directory.CreateDirectory("tmp");
            }
        }

        // Form is closed
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dispose the image
            if (pb_pdfView.Image != null)
            {
                pb_pdfView.Image.Dispose();
            }
            // Delete the "tmp/" folder and its contents
            if (Directory.Exists("tmp"))
            {
                Directory.Delete("tmp", true);
            }
        }


        // Error Message
        private void ShowErrorMessage(Error error)
        {
            if (error != Error.SUCCESS)
                System.Windows.Forms.MessageBox.Show("Error: \n" + error.ToString());
        }

        // Subscribe to events
        private void SubscribeToEvents()
        {
            // Event for button or sign event
            driverInterface.ButtonEvent += (object sender, ButtonEventArgs args) =>
            {
                Handlebuttonevent(args);        // User defined function
            };

            // Event for signature image changed
            driverInterface.SignImgChanged += (object sender, EventArgs e) =>
            {
                pb_signature.Invoke((MethodInvoker)(() => UpdateSignatureImage()));
            };

            // Event for signature mode stopped
            driverInterface.SignFinished += (object sender, EventArgs e) =>
            {

            };

        }

        // Convert PDF to TIFF
        public void ConvertPDFtoImg(string source, int page)
        {
            // clear the pb_pdfView.image if have content
            if (pb_pdfView.Image != null)
            {
                pb_pdfView.Image.Dispose();
            }

            // Se estiver usando a versão Professional, insira sua chave de serial abaixo.
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            // Carregue um documento PDF.
            using (GemBox.Pdf.PdfDocument document = GemBox.Pdf.PdfDocument.Load(source))
            {
                // Crie opções de salvamento de imagem.
                var imageOptions = new ImageSaveOptions(ImageSaveFormat.Bmp)
                {
                    PageNumber = page, // Selecione a primeira página do PDF.
                    Width = pb_pdfView.Width, // Set the FitToSizeWidth property to the width of pb_pdfView
                    Height = pb_pdfView.Height // Set the FitToSizeHeight property to the height of pb_pdfView
                };

                // Salve um documento PDF em um arquivo JPEG.
                document.Save("tmp/Output.bmp", imageOptions);

                // Load the image from the file and display it in the PictureBox
                pb_pdfView.Image = System.Drawing.Image.FromFile("tmp/Output.bmp");
            }
        }

        // Button to start signature
        private void bt_StartSignature_Click(object sender, EventArgs e)
        {
            Error r = driverInterface.StartSignatureMode(SignMode.StandardSign);
            ShowErrorMessage(r);

            bt_StopSignature.Enabled = true;
            bt_saveImage.Enabled = true;
        }

        // Button to stop signature
        private void bt_StopSignature_Click(object sender, EventArgs e)
        {
            // Set value of the signature hash
            Random random = new Random();
            byte[] hash = new byte[32];
            random.NextBytes(hash);
            // Show de document hash in the LCD
            Error r = driverInterface.SetPreliminaryDocumentHash(hash); // API for   signing 
            if (r != Error.SUCCESS)
            {
                // do nothing
            }
            else
                r = driverInterface.SetFinalDocumentHash(hash, false); // API for  signing 

            if (r != Error.SUCCESS)
                ShowErrorMessage(r);

            driverInterface.StopSignatureCapture();
            driverInterface.ClearLcd();

            // Disable the button
            bt_StopSignature.Enabled = false;
        }

        // Button to save signature image
        private void bt_saveImage_Click(object sender, EventArgs e)
        {
            // Check if have signature image
            if (pb_signature.Image == null)
            {
                System.Windows.Forms.MessageBox.Show("No signature image to save");
                return;
            }
            // Show save file dialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "BMP Image|*.bmp|SOI Image|*.soi|JPEG Image|*.jpg|PNG Image|*.png";
            saveFileDialog.Title = "Save Signature Image";
            saveFileDialog.ShowDialog();

            // Get the selected file path and name
            string filePath = saveFileDialog.FileName;

            // If Cancel button was pressed, return
            if (string.IsNullOrEmpty(filePath))
                return;

            // Show a message box with the save options
            DialogResult result = System.Windows.Forms.MessageBox.Show("Save without background?", "Save", MessageBoxButtons.YesNo);

            // Check the result
            if (result == DialogResult.Yes)
            {
                // Check if a file was selected
                if (!string.IsNullOrEmpty(filePath))
                {
                    // Save the signature image
                    pb_signature.Image.Save(filePath);
                }
            }
            else if (result == DialogResult.No)
            {
                // Checl if a file was selected
                if (!string.IsNullOrEmpty(filePath))
                {
                    driverInterface.ReadSignatureImage(pb_signature.Width, pb_signature.Height, filePath);
                }
            }

            // Disable the button
            bt_saveImage.Enabled = false;
        }

        // Update signature image in picture box
        void UpdateSignatureImage()
        {
            Error error = driverInterface.ReadSignatureImage(pb_signature.Width, pb_signature.Height, out Bitmap bitmap); // API which gets the Signature image

            // ----------------- Application specific code to adjust size of the signature image to the picture box  can be ignored ---------------------------
            bool source_is_wider = (float)bitmap.Width / bitmap.Height > (float)pb_signature.Width / pb_signature.Height;
            var resized = new Bitmap(pb_signature.Width, pb_signature.Height);
            var g = Graphics.FromImage(resized);
            var dest_rect = new System.Drawing.Rectangle(0, 0, pb_signature.Width, pb_signature.Height);
            System.Drawing.Rectangle src_rect;

            if (source_is_wider)
            {
                float size_ratio = (float)pb_signature.Height / bitmap.Height;
                int sample_width = (int)(pb_signature.Width / size_ratio);
                src_rect = new System.Drawing.Rectangle((bitmap.Width - sample_width) / 2, 0, sample_width, bitmap.Height);
            }
            else
            {
                float size_ratio = (float)pb_signature.Width / bitmap.Width;
                int sample_height = (int)(pb_signature.Height / size_ratio);
                src_rect = new System.Drawing.Rectangle(0, (bitmap.Height - sample_height) / 2, bitmap.Width, sample_height);
            }
            g.DrawImage(bitmap, dest_rect, src_rect, GraphicsUnit.Pixel);
            g.Dispose();

            pb_signature.Image = resized;
        }

        // Handle button event
        private void Handlebuttonevent(ButtonEventArgs args)
        {
            // Show the button event
            System.Windows.Forms.MessageBox.Show("Button Event = " + args.ButtonKind.ToString());
        }

        // Load PDF file
        private void bt_loadPDF_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF Files|*.pdf";
            openFileDialog.Title = "Select a PDF File";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                // Set the origin PDF file path and save the signed PDF file path
                source = openFileDialog.FileName;
                // GetPDFPreview(source);

                // Initialize the PDF file
                iText.Kernel.Pdf.PdfDocument pdfDocument = new iText.Kernel.Pdf.PdfDocument(new PdfReader(source));

                // Get Number of pages and set the number of pages for the text box
                pages = pdfDocument.GetNumberOfPages();
                tb_page.Text = "1";
                lb_nPages.Text = "/ " + pages;

                // Get the PDF file size
                iText.Kernel.Geom.Rectangle pageSize = pdfDocument.GetFirstPage().GetPageSize();
                tb_a4x.Text = ((int)Math.Round(pageSize.GetWidth())).ToString();
                tb_a4y.Text = ((int)Math.Round(pageSize.GetHeight())).ToString();

                // Dispose the pdf document
                pdfDocument.Close();

                // Convert the pages to images
                ConvertPDFtoImg(source, 0);

                // Att tb_x, tb_y, tb_sigWidth, tb_sigHeight
                tb_x_TextChanged(sender, e);
                tb_y_TextChanged(sender, e);
                tb_sigWidth_TextChanged(sender, e);
                tb_sigHeight_TextChanged(sender, e);

                // Enable the button to sign PDF file with image
                bt_signPDFImg.Enabled = true;
                tb_x.Enabled = true;
                tb_y.Enabled = true;
                tb_sigWidth.Enabled = true;
                tb_sigHeight.Enabled = true;
                sb_x.Enabled = true;
                sb_y.Enabled = true;
                pb_pdfView.Enabled = true;
                pb_signPrev.Visible = true;

                if (pages == 1)
                {
                    bt_previousPage.Visible = false;
                    bt_nextPage.Visible = false;
                }
                else
                {
                    tb_page.Enabled = true;
                    bt_previousPage.Visible = false;
                    bt_nextPage.Visible = true;
                }


            }
        }

        // Button to sign PDF file with image
        private void bt_signPDFImg_Click(object sender, EventArgs e)
        {
            // Set the destination PDF file path and name
            string destSource;
            int number = 2;
            if (!source.Contains("_signed") && !File.Exists(source.Replace(".pdf", "_signed.pdf")))
            {
                destSource = source.Replace(".pdf", "_signed.pdf");
            }
            else
            {
                if (source.Contains("_signed.pdf"))
                {
                    if (!File.Exists(source.Replace("_signed.pdf", "_signed" + number + ".pdf")))
                    {
                        destSource = source.Replace("_signed.pdf", "_signed" + number + ".pdf");
                    }
                    else
                    {
                        number++;
                        while (File.Exists(source.Replace("_signed.pdf", "_signed" + number + ".pdf")))
                        {
                            number++;
                        }
                        destSource = source.Replace("_signed.pdf", "_signed" + number + ".pdf");
                    }
                }
                else if (!source.Contains("_signed"))
                {
                    while (File.Exists(source.Replace(".pdf", "_signed" + number + ".pdf")))
                    {
                        number++;
                    }
                    destSource = source.Replace(".pdf", "_signed" + number + ".pdf");
                }
                else
                {
                    string letter = source[^5..];
                    while (File.Exists(source.Replace(letter, number + ".pdf")))
                    {
                        number++;
                    }
                    destSource = source.Replace(letter, number + ".pdf");
                }
            }
            // Check if have the signature image in the picture box
            if (pb_signature.Image != null)
            {
                // Check if user like to use the current signature
                DialogResult imageUse = System.Windows.Forms.MessageBox.Show("Use current signature?", "Signature", MessageBoxButtons.YesNo);

                if (imageUse == DialogResult.Yes)
                {
                    // Save the temporary signature image
                    pb_signature.Image.Save("tmp/Sign.bmp");
                }
            }

            // Select the image to add to the PDF file
            OpenFileDialog openFileDialogImage = new OpenFileDialog();
            openFileDialogImage.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png";
            openFileDialogImage.Title = "Select an sign Image File";
            openFileDialogImage.ShowDialog();

            if (openFileDialogImage.FileName != "")
            {
                // Modify the PDF file
                iText.Kernel.Pdf.PdfDocument pdfDocument = new iText.Kernel.Pdf.PdfDocument(new PdfReader(source), new PdfWriter(destSource));

                // Document object to modify the PDF file
                Document document = new Document(pdfDocument);

                // Add the image
                ImageData imageData = ImageDataFactory.Create(openFileDialogImage.FileName);
                iText.Layout.Element.Image signImage = new iText.Layout.Element.Image(imageData);
                signImage.ScaleAbsolute(Convert.ToInt32(tb_sigWidth.Text), Convert.ToInt32(tb_sigHeight.Text));

                // Set the position of the image
                signImage.SetFixedPosition(Convert.ToInt32(tb_page.Text), Convert.ToInt32(tb_x.Text), Convert.ToInt32(tb_y.Text));

                // Add the image to the PDF file
                document.Add(signImage);

                // Close the document
                document.Close();
                ConvertPDFtoImg(destSource, int.Parse(tb_page.Text) - 1);
                bt_signPDFImg.Enabled = false;
                tb_page.Enabled = false;
                tb_x.Enabled = false;
                tb_y.Enabled = false;
                tb_sigWidth.Enabled = false;
                tb_sigHeight.Enabled = false;
                sb_x.Enabled = false;
                sb_y.Enabled = false;
                bt_previousPage.Visible = false;
                bt_nextPage.Visible = false;
                pb_pdfView.Enabled = false;
                pb_signPrev.Visible = false;
                

                // Clear source
                source = null;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No image to sign");
            }
        }

        // Limit the value of the text box page
        private void tb_page_TextChanged(object sender, EventArgs e)
        {
            // Check the value of the text box
            if (string.IsNullOrEmpty(tb_page.Text) || !int.TryParse(tb_page.Text, out _))
            {
                tb_page.Text = "1";
            }
            else if (int.Parse(tb_page.Text) > pages)
            {
                tb_page.Text = pages.ToString();
            }
            else if (int.Parse(tb_page.Text) < 1)
            {
                tb_page.Text = "1";
            }
        }

        // Button to go to the previous page
        private void bt_previousPage_Click(object sender, EventArgs e)
        {
            int currentPage = int.Parse(tb_page.Text);
            if (currentPage > 1)
            {
                tb_page.Text = (currentPage - 1).ToString();
            }
            if (currentPage - 1 == 1)
            {
                bt_previousPage.Visible = false;
                bt_nextPage.Visible = true;
            }

            // Update the img
            ConvertPDFtoImg(source, currentPage - 2);
        }

        // Button to go to the next page
        private void bt_nextPage_Click(object sender, EventArgs e)
        {
            int currentPage = int.Parse(tb_page.Text);
            if (currentPage < pages)
            {
                tb_page.Text = (currentPage + 1).ToString();
            }
            if (currentPage + 1 == pages)
            {
                bt_nextPage.Visible = false;
                bt_previousPage.Visible = true;
            }

            // Update the img
            ConvertPDFtoImg(source, currentPage);
        }

        // tb x adujstment
        private void tb_x_TextChanged(object sender, EventArgs e)
        {
            // Check the value of the text box
            if (string.IsNullOrEmpty(tb_x.Text) || !int.TryParse(tb_x.Text, out _))
            {
                tb_x.Text = "0";
            }
            else if (int.Parse(tb_x.Text) > ((int)Math.Round(float.Parse(tb_a4x.Text)) - (int)Math.Round(float.Parse(tb_sigWidth.Text))))
            {
                tb_x.Text = Convert.ToString((int)Math.Round(float.Parse(tb_a4x.Text)) - (int)Math.Round(float.Parse(tb_sigWidth.Text)));
            }
            else if (int.Parse(tb_x.Text) < 0)
            {
                tb_x.Text = "0";
            }

            // Set the value of the scroll bar x
            sb_x.Value = (int)Math.Round(float.Parse(tb_x.Text));
            sb_x.Maximum = (int)Math.Round(float.Parse(tb_a4x.Text)) - (int)Math.Round(float.Parse(tb_sigWidth.Text));

            CoordnateSign();
        }

        // tb y adujstment
        private void tb_y_TextChanged(object sender, EventArgs e)
        {
            // Check the value of the text box
            if (string.IsNullOrEmpty(tb_y.Text) || !int.TryParse(tb_y.Text, out _))
            {
                tb_y.Text = "0";
            }
            else if ((int)Math.Round(float.Parse(tb_y.Text)) > ((int)Math.Round(float.Parse(tb_a4y.Text)) - (int)Math.Round(float.Parse(tb_sigHeight.Text))))
            {
                tb_y.Text = Convert.ToString((int)Math.Round(float.Parse(tb_a4y.Text)) - (int)Math.Round(float.Parse(tb_sigHeight.Text)));
            }
            else if ((int)Math.Round(float.Parse(tb_y.Text)) < 0)
            {
                tb_y.Text = "0";
            }

            // Set the value of the scroll bar y
            sb_y.Value = (int)Math.Round(float.Parse(tb_y.Text));
            sb_y.Maximum = (int)Math.Round(float.Parse(tb_a4y.Text)) - (int)Math.Round(float.Parse(tb_sigHeight.Text));

            CoordnateSign();
        }

        // tb sigWidth adujstment
        private void tb_sigWidth_TextChanged(object sender, EventArgs e)
        {
            // Check the value of the text box
            if (string.IsNullOrEmpty(tb_sigWidth.Text) || !int.TryParse(tb_sigWidth.Text, out _))
            {
                tb_sigWidth.Text = "0";
            }
            else if (int.Parse(tb_sigWidth.Text) > int.Parse(tb_a4x.Text))
            {
                tb_sigWidth.Text = tb_a4x.Text;
            }
            else if (int.Parse(tb_sigWidth.Text) < 0)
            {
                tb_sigWidth.Text = "0";
            }
            // Adjust the value of the text box x
            tb_x_TextChanged(sender, e);
            ScaleSign();
        }

        // tb sigHeight adujstment
        private void tb_sigHeight_TextChanged(object sender, EventArgs e)
        {
            // Check the value of the text box
            if (string.IsNullOrEmpty(tb_sigHeight.Text) || !int.TryParse(tb_sigHeight.Text, out _))
            {
                tb_sigHeight.Text = "0";
            }
            else if (int.Parse(tb_sigHeight.Text) > int.Parse(tb_a4y.Text))
            {
                tb_sigHeight.Text = tb_a4y.Text;
            }
            else if (int.Parse(tb_sigHeight.Text) < 0)
            {
                tb_sigHeight.Text = "0";
            }
            // Adjust the value of the text box y
            tb_y_TextChanged(sender, e);
            ScaleSign();
        }

        // Scroll bar x adujstment
        private void sb_x_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            tb_x.Text = sb_x.Value.ToString();
        }

        // Scroll bar y adujstment
        private void sb_y_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            tb_y.Text = sb_y.Value.ToString();
        }

        // Scale function for the sign preview
        private void ScaleSign()
        {
            pb_signPrev.Width = (int.Parse(tb_sigWidth.Text) * pb_pdfView.Width) / int.Parse(tb_a4x.Text);
            pb_signPrev.Height = (int.Parse(tb_sigHeight.Text) * pb_pdfView.Height) / int.Parse(tb_a4y.Text);
        }

        // Coordinate function for the sign preview
        private void CoordnateSign()
        {
            pb_signPrev.Location = new System.Drawing.Point(((int.Parse(tb_x.Text) * pb_pdfView.Width) / int.Parse(tb_a4x.Text)) + pb_pdfView.Location.X, ((-int.Parse(tb_y.Text) * pb_pdfView.Height) / int.Parse(tb_a4y.Text)) + ((pb_pdfView.Location.Y + pb_pdfView.Height) - pb_signPrev.Height));
        }

        // Mouse click event for the sign preview
        private void pb_pdfView_MouseClick(object sender, MouseEventArgs e)
        {
            tb_x.Text = ((e.X * int.Parse(tb_a4x.Text)) / pb_pdfView.Width).ToString();
            tb_y.Text = (((-e.Y * int.Parse(tb_a4y.Text)) / pb_pdfView.Height) + (int.Parse(tb_a4y.Text))).ToString();
        }

    }
}
