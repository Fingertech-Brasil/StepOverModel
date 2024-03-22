using LanguageExt;
using LanguageExt.Pipes;
using Sig.DeviceAPI;
using Sig.SignAPI;
using Sig.SignAPI.Utils;
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
using System.Drawing.Printing;
using System.Security.Cryptography;
using System.Text;
using System.Management;

namespace StepOverModel
{
    public partial class Form1 : Form
    {
        // ------------------------------Variables and Objects-----------------------

        // number of pages
        int pages = 0;

        // Origin PDF file path
        string source;

        // Create objects of the DeviceAPI
        public static IDriver driverInterface = new Driver();
        public static IReadSignatureImageOptions imageOptions = driverInterface.ReadSignatureImageOptions;

        // Create objects of the SignAPI
        ISigning theSigningObject = null;

        // Create objects of the SetCertificate and create certificate path
        ISetCertificate setCertificate = driverInterface.SetCertificate;
        string certPath;

        // WMI query to monitor for device arrival events
        private ManagementEventWatcher arrivalWatcher;
        // WMI query to monitor for device removal events
        private ManagementEventWatcher removalWatcher;
        // Device is plugged
        private bool deviceArrival = false;

        // ------------------------------Methods For Forms---------------------------

        // Form is open
        public Form1()
        {
            InitializeComponent();

            // Set the device
            SearchDevice(deviceArrival);

            // Set the value of the scroll bar x
            sb_x.Value = int.Parse(tb_x.Text);
            sb_x.Maximum = 595 - int.Parse(tb_sigWidth.Text);

            // Create the tmp folder
            if (!Directory.Exists("tmp"))
            {
                Directory.CreateDirectory("tmp");
            }

            driverInterface.IsSignFinishedEnabled = false;

            // Check if the cache file exists
            if(File.Exists("cache.txt"))// Get saved certificate path and password
            {
                // Open the file to read from.
                using (StreamReader sr = File.OpenText("cache.txt"))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (s.Contains(".pfx"))
                        {
                            certPath = s;
                        }
                        else
                        {
                            // Convert the encrypted data to a byte array
                            byte[] encryptedPasswordBytes = Convert.FromBase64String(s);

                            // Define the entropy used during encryption (if any).
                            byte[] entropy = { 1, 2, 3, 4, 1, 2, 3, 4 }; // This should match the entropy used during encryption.

                            // Decrypt the byte array.
                            byte[] decryptedPasswordBytes = ProtectedData.Unprotect(encryptedPasswordBytes, entropy, DataProtectionScope.CurrentUser);

                            // Convert the decrypted byte array back to a string.
                            string decryptedPassword = Encoding.UTF8.GetString(decryptedPasswordBytes);

                            // Set the value of the text box passCert
                            tb_passCert.Text = decryptedPassword;
                        }
                    }
                }
            }

            // Initialize the WMI event watchers
            InitializeWmiWatchers();

            // Events
            SubscribeToEvents();
        }

        // Form is closed
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save the certificate path and password
            // if certPath is empty, return
            if (!string.IsNullOrEmpty(certPath))
            {
                // Create a file to write to.
                string filepath = "cache.txt";

                // Create a byte array for additional entropy when using Protect method
                byte[] entropy = { 1, 2, 3, 4, 1, 2, 3, 4 };

                // Encrypt the password
                byte[] encryptedPassword = ProtectedData.Protect(Encoding.UTF8.GetBytes(tb_passCert.Text), entropy, DataProtectionScope.CurrentUser);

                // Convert the encrypted data to a string
                string encryptedPasswordString = Convert.ToBase64String(encryptedPassword);

                // Combine the certPath and passCert
                string combinedStrings = certPath + Environment.NewLine + encryptedPasswordString;

                // Verify if the file exists
                if (!File.Exists(filepath))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(filepath))
                    {
                        sw.WriteLine(combinedStrings);
                    }
                }
                else
                {
                    // Open the file to read from.
                    using (StreamReader sr = File.OpenText(filepath))
                    {
                        // Verify if the certPath and passCert already modified
                        bool change = false;

                        string s = "";
                        // Count the lines
                        int line = 0;

                        // Read the file
                        while ((s = sr.ReadLine()) != null)
                        {
                            if (s != tb_passCert.Text && s.Contains(".pfx") || s != certPath && line == 1)
                            {
                                change = true;
                            }
                            line++;
                        }
                        sr.Close();

                        // If the certPath and passCert already modified
                        if (change)
                        {
                            File.WriteAllText(filepath, combinedStrings);
                        }
                    }
                }
            }

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

            // Stop the WMI event watchers
            arrivalWatcher.Stop();
            arrivalWatcher.Dispose();

            removalWatcher.Stop();
            removalWatcher.Dispose();
        }

        // ------------------------------Methods For Events--------------------------

        // Subscribe to events
        private void SubscribeToEvents()
        {
            // Event for signature image changed
            driverInterface.SignImgChanged += (object sender, EventArgs e) =>
            {
                pb_signature.Invoke((MethodInvoker)(() => UpdateSignatureImage()));
            };
        }

        // Initialize WMI event watchers
        private void InitializeWmiWatchers()
        {
            // WMI query to monitor for device arrival events
            string arrivalQuery = "SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2";
            arrivalWatcher = new ManagementEventWatcher(arrivalQuery);
            arrivalWatcher.EventArrived += ArrivalEventArrived;
            arrivalWatcher.Start();

            // WMI query to monitor for device removal events
            string removalQuery = "SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3";
            removalWatcher = new ManagementEventWatcher(removalQuery);
            removalWatcher.EventArrived += RemovalEventArrived;
            removalWatcher.Start();
        }

        // ------------------------------Methods Called------------------------------

        // Error Message
        private void ShowErrorMessage(Error error)
        {
            if (error != Error.SUCCESS)
                MessageBox.Show("Error: \n" + error.ToString());
        }

        // Convert PDF to TIFF
        public void ConvertPDFtoImg(string source, int page)
        {
            // clear the pb_pdfView.image if have content
            if (pb_pdfView.Image != null)
            {
                pb_pdfView.Image.Dispose();
            }

            // If use the professional version, set the license key
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

                // Dispose the document
                document.Unload();
            }
        }

        // Get the destination path
        private string GetDestinPath()
        {
            string destSource = "";
            // Create variable to number of the signature if necessary
            int number = 2;

            // Check if source file is unsigned and if exist a signed file
            if (!source.Contains("_signed") && !File.Exists(source.Replace(".pdf", "_signed.pdf")))
            {
                destSource = source.Replace(".pdf", "_signed.pdf");
            }
            else // If the file is signed
            {
                if (source.Contains("_signed.pdf")) // If the file is signed and dont have a number in the end
                {
                    // Check if the file with the number 2 dont exists
                    if (!File.Exists(source.Replace("_signed.pdf", "_signed" + number + ".pdf")))
                    {
                        destSource = source.Replace("_signed.pdf", "_signed" + number + ".pdf");
                    }
                    else // If the file with the number 2 exists
                    {
                        number++; // Increment the number
                        // Check if the file with the next number exists
                        while (File.Exists(source.Replace("_signed.pdf", "_signed" + number + ".pdf")))
                        {
                            number++;
                        }
                        destSource = source.Replace("_signed.pdf", "_signed" + number + ".pdf");
                    }
                }
                else if (!source.Contains("_signed")) // If the file is signed and have a number in the end
                {
                    // Check if the file with the number 2 dont exists and if the file with the number 2 exists increment the number
                    // And check if the file with the next number exists
                    while (File.Exists(source.Replace(".pdf", "_signed" + number + ".pdf")))
                    {
                        number++;
                    }
                    destSource = source.Replace(".pdf", "_signed" + number + ".pdf");
                }
                else // If the file is signed and have a number in the end
                {
                    string letter = source[^5..]; // Get the number of the file
                    while (File.Exists(source.Replace(letter, number + ".pdf"))) // Check the file with the number exists, ascending the number sequentially
                    {
                        number++;
                    }
                    destSource = source.Replace(letter, number + ".pdf");
                }
            }
            return destSource; // Old path + _Signed
        }

        // ----------Usb----------

        // For the usb arrival and removal events
        private void ArrivalEventArrived(object sender, EventArgs e)
        {
            // USB device was plugged in
            SearchDevice(deviceArrival);
        }

        private void RemovalEventArrived(object sender, EventArgs e)
        {
            // USB device was removed
            SearchDevice(deviceArrival);
        }

        // Search the device
        private void SearchDevice(bool plugged)
        {
            string[]? deviceNames;

            int guiOption = 0;

            // Filter the device
            FilterDeviceKind DeviceFilter = FilterDeviceKind.dkStepOver;
            driverInterface.DeviceSearch(out deviceNames, guiOption, DeviceFilter);
            if (deviceNames.Length == 0)
            {
                // Show the message
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => lb_DeviceInfo.Text = ("No device found!" +
                                                                       "\nSignature options is disable.")));
                }
                else
                {
                    lb_DeviceInfo.Text = ("No device found!" +
                                          "\nSignature options is disable.");
                }

                // Disable the buttons
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => gb_sign.Enabled = false));
                    this.Invoke(new Action(() => bt_StopSignature.Enabled = false));
                    this.Invoke(new Action(() => bt_saveImage.Enabled = false));
                    this.Invoke(new Action(() => bt_lineColor.Enabled = false));
                    this.Invoke(new Action(() => lb_deviceArrival.Text = Char.ConvertFromUtf32(0x274C)));
                    this.Invoke(new Action(() => lb_deviceArrival.ForeColor = Color.Red));
                    
                    if (bt_signPDFImg.Enabled)
                    {
                        this.Invoke(new Action(() => bt_signPDF.Enabled = false));
                    }
                }
                else
                {
                    gb_sign.Enabled = false;
                    bt_StopSignature.Enabled = false;
                    bt_saveImage.Enabled = false;
                    bt_lineColor.Enabled = false;
                    lb_deviceArrival.Text = Char.ConvertFromUtf32(0x274C);
                    lb_deviceArrival.ForeColor = Color.Red;

                    if (bt_signPDFImg.Enabled)
                    {
                        bt_signPDF.Enabled = false;
                    }
                }
                deviceArrival = false;
            }
            else if (!deviceArrival)
            {
                // Set the device
                Error r = driverInterface.SetDevice(deviceNames[0]);
                if (r == Error.SUCCESS)
                {
                    // Activate buttons
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => gb_sign.Enabled = true));
                        this.Invoke(new Action(() => bt_StopSignature.Enabled = false));
                        this.Invoke(new Action(() => bt_saveImage.Enabled = false));
                        this.Invoke(new Action(() => bt_lineColor.Enabled = true));
                        this.Invoke(new Action(() => lb_deviceArrival.Text = Char.ConvertFromUtf32(0x2714) + Char.ConvertFromUtf32(0xFE0F)));
                        this.Invoke(new Action(() => lb_deviceArrival.ForeColor = Color.Green));

                        if (bt_signPDFImg.Enabled)
                        {
                            this.Invoke(new Action(() => bt_signPDF.Enabled = true));
                        }
                    }
                    else
                    {
                        gb_sign.Enabled = true;
                        bt_StopSignature.Enabled = false;
                        bt_saveImage.Enabled = false;
                        bt_lineColor.Enabled = true;
                        lb_deviceArrival.Text = Char.ConvertFromUtf32(0x2714) + Char.ConvertFromUtf32(0xFE0F);
                        lb_deviceArrival.ForeColor = Color.Green;

                        if (bt_signPDFImg.Enabled)
                        {
                            bt_signPDF.Enabled = true;
                        }
                    }
                    deviceArrival = true;
                    
                }

                // Load the license
                r = driverInterface.LoadLicense("License/FingerTech.xml");
                ShowErrorMessage(r);

                // Show the device info
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => lb_DeviceInfo.Text = ("Device Info: " + deviceNames[0])));
                }
                else
                {
                    lb_DeviceInfo.Text = ("Device Info: " + deviceNames[0]);
                }
            }
        }

        // ------------------------------Methods For Singing--------------------------

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

        // Button to modify the color of the signature line
        private void bt_lineColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = imageOptions.LineColor;
            colorDialog.ShowDialog();
            imageOptions.LineColor = colorDialog.Color;
        }

        // Update signature image in picture box
        void UpdateSignatureImage()
        {
            Error error = driverInterface.ReadSignatureImage(pb_signature.Width, pb_signature.Height, out Bitmap bitmap); // API which gets the Signature image

            bool source_is_wider = (float)bitmap.Width / bitmap.Height > (float)pb_signature.Width / pb_signature.Height;
            var resized = new Bitmap(pb_signature.Width, pb_signature.Height);
            var g = Graphics.FromImage(resized);
            g.Clear(Color.Transparent); // Set the background color to transparent

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
            pb_signPrev.Image = resized;
        }

        // ------------------------------Methods For manipulate PDF----------------------

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

                // Convert the pages to images
                try
                {
                    ConvertPDFtoImg(source, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: \n" + ex.Message);

                    // Clear the picture box and disable the buttons
                    pb_pdfView.Image = null;
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

                    return;
                }

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

                if (bt_StartSignature.Enabled)
                {
                    bt_signPDF.Enabled = true;
                }
            }
        }

        // Button to sign PDF file with image
        private void bt_signPDFImg_Click(object sender, EventArgs e)
        {
            // Create variable to destination PDF file path
            string destSource = GetDestinPath();

            // Create variable to signature image path
            string imgpath = "";

            // Check if have the signature image in the picture box
            if (pb_signature.Image != null)
            {
                // Check if user like to use the current signature
                DialogResult imageUse = MessageBox.Show("Use current signature?", "Signature", MessageBoxButtons.YesNo);

                if (imageUse == DialogResult.Yes)
                {
                    // Save the temporary signature image
                    pb_signature.Image.Save("tmp/Sign.bmp");
                    imgpath = "tmp/Sign.bmp";
                }
            }
            else
            {
                // Select the image to add to the PDF file
                OpenFileDialog openFileDialogImage = new OpenFileDialog();
                openFileDialogImage.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png";
                openFileDialogImage.Title = "Select an sign Image File";
                openFileDialogImage.ShowDialog();
                imgpath = openFileDialogImage.FileName;
            }

            if (imgpath != "")
            {
                // Modify the PDF file
                iText.Kernel.Pdf.PdfDocument pdfDocument = new iText.Kernel.Pdf.PdfDocument(new PdfReader(source), new PdfWriter(destSource));

                // Document object to modify the PDF file
                Document document = new Document(pdfDocument);

                // Add the image
                ImageData imageData = ImageDataFactory.Create(imgpath);
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
                bt_signPDF.Enabled = false;
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
                source = "";

            }
            else
            {
                MessageBox.Show("No image to sign");
            }
        }

        // ------------------------------Methods For sign PDF------------------------------

        private async void bt_signPDF_Click(object sender, EventArgs e)
        {
            // Create variable to destination PDF file path
            string destSource = GetDestinPath();

            // Create variable to document octets
            byte[] documentOctets = File.ReadAllBytes(source);

            // Set the certificate
            if (certPath != "" && tb_passCert.Text != "")
            {
                Error r = setCertificate.FromPKCS12File(certPath, tb_passCert.Text);
                ShowErrorMessage(r);
            }

            // Set the object of the Signing
            using (theSigningObject = await SigningFactory.StartAsync(driverInterface, documentOctets)) // Auto-Dispose in the end
            {
                // Get the device properties
                IDeviceProperties deviceProperties = driverInterface.DeviceProperties;

                // Set the coordinate of the signature
                FieldPosition fieldPosition = new FieldPosition((int.Parse(tb_a4y.Text) - int.Parse(tb_y.Text)),
                                                                int.Parse(tb_x.Text),
                                                                (int.Parse(tb_a4y.Text) - int.Parse(tb_y.Text) - int.Parse(tb_sigHeight.Text)),
                                                                int.Parse(tb_x.Text) + int.Parse(tb_sigWidth.Text));

                // Get the page number
                fieldPosition.PageNumber = int.Parse(tb_page.Text) - 1;

                // Show the signInfo form
                SignInfo signInfo = new SignInfo();
                signInfo.ShowDialog();

                if (!signInfo.Ok)
                {
                    MessageBox.Show("signature canceled");
                    signInfo.Dispose();
                    return;
                }

                // Set the device signature image
                signInfo.behaviour.UseRectangle(Sig.SignAPI.Rectangle.FromBottomLeft(0, 0, deviceProperties.Hardware.DisplayWidth, deviceProperties.Hardware.DisplayHeight)); // Set the SigImg size in device

                // Set the device end sign in 3 seconds
                driverInterface.IsSignFinishedEnabled = true;

                // Sign the PDF file
                _ = await theSigningObject.SignAsync(signInfo.signatureInfo.Name, fieldPosition, signInfo.signatureInfo, signInfo.behaviour);

                // Download the signed PDF file
                byte[] signedDocument = await theSigningObject.Client.DownloadPdfAsync();
                File.WriteAllBytes(destSource, signedDocument);

                // Restore the device end sign
                driverInterface.IsSignFinishedEnabled = false;

                // Get new imag and Disable the buttons
                ConvertPDFtoImg(destSource, int.Parse(tb_page.Text) - 1);
                bt_signPDFImg.Enabled = false;
                bt_signPDF.Enabled = false;
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
                source = "";
            }

        }

        // ------------------------------Methods For Certificate------------------------------

        // Get the certificate path
        private void bt_setCert_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PFX Files|*.pfx";
            openFileDialog.Title = "Select a PFX File";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                certPath = openFileDialog.FileName;
            }
        }

        // ------------------------------Methods For PDF Page----------------------

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

            // Focus the Sign PDF button
            bt_signPDFImg.Focus();

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

            // Focus the Sign PDF button
            bt_signPDFImg.Focus();

            // Update the img
            ConvertPDFtoImg(source, currentPage);
        }

        // ------------------------------Methods For Sign Coordnates----------------------

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
            ScaleSign();
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
            else if (int.Parse(tb_sigHeight.Text) > int.Parse(tb_a4y.Text))
            {
                tb_sigHeight.Text = tb_a4y.Text;
            }
            else if (int.Parse(tb_sigHeight.Text) < 0)
            {
                tb_sigHeight.Text = "0";
            }
            // Adjust the value of the text box y
            ScaleSign();
            tb_y_TextChanged(sender, e);
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

        // ------------------------------Methods For Sign Preview Scale and Coordinate----------------------

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
