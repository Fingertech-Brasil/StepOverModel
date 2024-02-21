using DeviceAPI_Csharp_DLL;
using LanguageExt;
using Sig.Crypto;
using Sig.DeviceAPI;
using System.Runtime.InteropServices;

namespace StepOverModel
{
    
    public partial class Form1 : Form
    {
        // Create an instance of the driver
        public static IDriver driverInterface = new Driver();

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
            // Event for signature image changed
            driverInterface.SignImgChanged += (object sender, EventArgs e) =>
            {
                pb_signature.Invoke((MethodInvoker)(() => UpdateSignatureImage()));
            };

            // Event for signature finished
            driverInterface.SignFinished += (object sender, EventArgs e) =>
            {
                Random random = new Random();
                byte[] hash = new byte[32];

                Error r = driverInterface.SetPreliminaryDocumentHash(hash); // API for   signing 
                if (r != Error.SUCCESS)
                {
                    // do nothing
                }
                else
                    r = driverInterface.SetFinalDocumentHash(hash, false); // API for  signing 

                if (r != Error.SUCCESS)
                    MessageBox.Show(r.ToString(), "Warning");
            };
        }

        // Button to start signature
        private void bt_StartSignature_Click(object sender, EventArgs e)
        {
            Error r = driverInterface.StartSignatureMode(SignMode.StandardSign);
            ShowErrorMessage(r);
        }

        // Button to stop signature
        private void bt_StopSignature_Click(object sender, EventArgs e)
        {
            sopadDLL.SOPAD_stopCapture();
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
            //---------------------------------------------------------------------------------------------------------------------------------------------------
            //pb_signature.Image = bitmap;
        }
    }
}
