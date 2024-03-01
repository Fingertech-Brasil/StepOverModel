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
using iTextSharp.text.pdf;
using System.Windows.Forms;

namespace StepOverModel
{

    public partial class Form1 : Form
    {
        // Create an instance of the driver
        public static IDriver driverInterface = new Driver();

        // Check if have signature image
        public bool haveSignatureImage = false;

        // Create an instance of the IClient
        //public IClient clientInterface = ClientFactory.BuildDefault();

        public Form1(string deviceName0)
        {
            InitializeComponent();

            // Set the device
            driverInterface.SetDevice(deviceName0);

            // Subscribe to events
            SubscribeToEvents();
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
        }

        private double CalculateDPI(double width, double height)
        {
            // Get tge Screen Dimensions
            Screen screen = Screen.FromControl(this);

            return Math.Sqrt((width*height)/(screen.Bounds.Width*screen.Bounds.Height));
        }

        // Button to start signature
        private void bt_StartSignature_Click(object sender, EventArgs e)
        {
            Error r = driverInterface.LoadLicense("C:\\Users\\Finger\\Desktop\\Project\\StepOverModel\\FingerTech.xml");
            ShowErrorMessage(r);

            r = driverInterface.StartSignatureMode(SignMode.StandardSign);
            ShowErrorMessage(r);
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

            haveSignatureImage = true;

            driverInterface.ClearLcd();
        }

        private void bt_saveImage_Click(object sender, EventArgs e)
        {
            // Check if have signature image
            if (pb_signature.Image == null && !haveSignatureImage)
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
                //// Open the selected pdf to stream
                //FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);

                //// Load the PDF file into the client
                //clientInterface.LoadPdfAsync(fs);

                //// Close the file stream
                //fs.Close();

                //// Get the PDF dimensions and number of pages
                //Task<IEnumerable<IPageDimension>> pageDimensions = clientInterface.GetPagesAsync();

            }
        }
    }
}
