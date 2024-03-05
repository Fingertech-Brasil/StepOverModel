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

namespace StepOverModel
{

    public partial class Form1 : Form
    {
        // Origin PDF file path
        string source;

        // Create an instance of the driver
        public static IDriver driverInterface = new Driver();

        // Create an instance of the IClient
        //public IClient clientInterface = ClientFactory.BuildDefault();

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
        }

        // Error Message
        private void ShowErrorMessage(Error error)
        {
            if (error != Error.SUCCESS)
                MessageBox.Show("Error: \n" + error.ToString());
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
                bt_StopSignature_Click(sender, e);
            };

        }

        // Get the PDF preview
        private void GetPDFPreview(string docPath)
        {
            // Clear the web browser
            webBrowser.Dispose();
            webBrowser = new WebBrowser();
            // Set the web browser properties
            webBrowser.Dock = DockStyle.Fill;
            webBrowser.Navigate(docPath);
            pb_pdfView.Controls.Add(webBrowser);
        }

        // Button to start signature
        private void bt_StartSignature_Click(object sender, EventArgs e)
        {
            Error r = driverInterface.LoadLicense("C:\\Users\\Finger\\Desktop\\Project\\StepOverModel\\FingerTech.xml");
            ShowErrorMessage(r);

            r = driverInterface.StartSignatureMode(SignMode.StandardSign);
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
                MessageBox.Show("No signature image to save");
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
            DialogResult result = MessageBox.Show("Save without background?", "Save", MessageBoxButtons.YesNo);

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
            var dest_rect = new Rectangle(0, 0, pb_signature.Width, pb_signature.Height);
            Rectangle src_rect;

            if (source_is_wider)
            {
                float size_ratio = (float)pb_signature.Height / bitmap.Height;
                int sample_width = (int)(pb_signature.Width / size_ratio);
                src_rect = new Rectangle((bitmap.Width - sample_width) / 2, 0, sample_width, bitmap.Height);
            }
            else
            {
                float size_ratio = (float)pb_signature.Width / bitmap.Width;
                int sample_height = (int)(pb_signature.Height / size_ratio);
                src_rect = new Rectangle(0, (bitmap.Height - sample_height) / 2, bitmap.Width, sample_height);
            }
            g.DrawImage(bitmap, dest_rect, src_rect, GraphicsUnit.Pixel);
            g.Dispose();

            pb_signature.Image = resized;
        }

        // Handle button event
        private void Handlebuttonevent(ButtonEventArgs args)
        {
            // Show the button event
            MessageBox.Show("Button Event = " + args.ButtonKind.ToString());
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
                GetPDFPreview(source);
                bt_signPDFImg.Enabled = true;
                tb_x.Enabled = true;
                tb_y.Enabled = true;
                tb_sigWidth.Enabled = true;
                tb_sigHeight.Enabled = true;
                sb_x.Enabled = true;
                sb_y.Enabled = true;
            }
        }

        // Button to sign PDF file with image
        private void bt_signPDFImg_Click(object sender, EventArgs e)
        {
            // Set the destination PDF file path and name
            string destSource;
            if (source.Contains("_signed.pdf"))
            {
                // If the file is already signed, pick the file name, if have number, add 1
                int number = 2;
                while (File.Exists(source.Replace(".pdf", number + ".pdf"))) // Check if the file exists
                {
                    number++;
                }
                destSource = source.Replace(".pdf", number + ".pdf");
            }
            else
            {
                if (File.Exists(source.Replace(".pdf", "_signed.pdf")))
                {
                    int number = 2;
                    while (File.Exists(source.Replace(".pdf", number + ".pdf"))) // Check if the file exists
                    {
                        number++;
                    }
                    destSource = source.Replace(".pdf", "_signed" + number + ".pdf");
                }
                else
                {
                    destSource = source.Replace(".pdf", "_signed.pdf");
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
                PdfDocument pdfDocument = new PdfDocument(new PdfReader(source), new PdfWriter(destSource));

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
                GetPDFPreview(destSource);
                bt_signPDFImg.Enabled = false;
                tb_x.Enabled = false;
                tb_y.Enabled = false;
                tb_sigWidth.Enabled = false;
                tb_sigHeight.Enabled = false;
                sb_x.Enabled = false;
                sb_y.Enabled = false;
            }
            else
            {
                MessageBox.Show("No image to sign");
            }
        }

        // tb x adujstment
        private void tb_x_TextChanged(object sender, EventArgs e)
        {
            // Check the value of the text box
            if (string.IsNullOrEmpty(tb_x.Text) || !int.TryParse(tb_x.Text, out _))
            {
                tb_x.Text = "0";
            }
            else if (int.Parse(tb_x.Text) > (595 - int.Parse(tb_sigWidth.Text)))
            {
                tb_x.Text = Convert.ToString(595 - int.Parse(tb_sigWidth.Text));
            }
            else if (int.Parse(tb_x.Text) < 0)
            {
                tb_x.Text = "0";
            }

            // Set the value of the scroll bar x
            sb_x.Value = int.Parse(tb_x.Text);
            sb_x.Maximum = 595 - int.Parse(tb_sigWidth.Text);
        }

        // tb y adujstment
        private void tb_y_TextChanged(object sender, EventArgs e)
        {
            // Check the value of the text box
            if (string.IsNullOrEmpty(tb_y.Text) || !int.TryParse(tb_y.Text, out _))
            {
                tb_y.Text = "0";
            }
            else if (int.Parse(tb_y.Text) > (842 - int.Parse(tb_sigHeight.Text)))
            {
                tb_y.Text = Convert.ToString(842 - int.Parse(tb_sigHeight.Text));
            }
            else if (int.Parse(tb_y.Text) < 0)
            {
                tb_y.Text = "0";
            }

            // Set the value of the scroll bar y
            sb_y.Value = int.Parse(tb_y.Text);
            sb_y.Maximum = 842 - int.Parse(tb_sigHeight.Text);
        }

        // tb sigWidth adujstment
        private void tb_sigWidth_TextChanged(object sender, EventArgs e)
        {
            // Check the value of the text box
            if (string.IsNullOrEmpty(tb_sigWidth.Text) || !int.TryParse(tb_sigWidth.Text, out _))
            {
                tb_sigWidth.Text = "0";
            }
            else if (int.Parse(tb_sigWidth.Text) > 595)
            {
                tb_sigWidth.Text = "595";
            }
            else if (int.Parse(tb_sigWidth.Text) < 0)
            {
                tb_sigWidth.Text = "0";
            }
            // Adjust the value of the text box x
            tb_x_TextChanged(sender, e);
        }

        // tb sigHeight adujstment
        private void tb_sigHeight_TextChanged(object sender, EventArgs e)
        {
            // Check the value of the text box
            if (string.IsNullOrEmpty(tb_sigHeight.Text) || !int.TryParse(tb_sigHeight.Text, out _))
            {
                tb_sigHeight.Text = "0";
            }
            else if (int.Parse(tb_sigHeight.Text) > 842)
            {
                tb_sigHeight.Text = "842";
            }
            else if (int.Parse(tb_sigHeight.Text) < 0)
            {
                tb_sigHeight.Text = "0";
            }
            // Adjust the value of the text box y
            tb_y_TextChanged(sender, e);
        }

        private void sb_x_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            tb_x.Text = sb_x.Value.ToString();
        }

        private void sb_y_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            tb_y.Text = sb_y.Value.ToString();
        }
    }
}
