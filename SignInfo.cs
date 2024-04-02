using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;
using Sig.PdfClient;
using Sig.SignAPI;

namespace StepOverModel
{
    public partial class SignInfo : Form
    {
        // ------------------------------Variables and Objects-----------------------

        // DefaultTextBuilder is a class from the Sig.SignAPI.Stamp namespace
        public Behaviour behaviour;

        // SignatureInfo is a class from the Sig.PdfClient namespace
        public SignatureInfo signatureInfo = new SignatureInfo(DateTimeOffset.Now);

        // Ok is a public field that is used to determine if the user has clicked the "Sign" button
        public bool Ok = false;

        public SignInfo()
        {
            InitializeComponent();
            tb_name.Leave += TbName_Leave; // Attach event handler to the Leave event of tb_name
        }

        // ------------------------------Event Handlers-----------------------

        // Event handler for the Leave event of tb_name
        private void TbName_Leave(object sender, EventArgs e)
        {
            string text = tb_name.Text;

            // Capitalize the first letter of each word in the text
            if (!string.IsNullOrEmpty(text))
            {
                string[] words = text.Split(' ');
                StringBuilder result = new StringBuilder();
                foreach (string word in words)
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        char firstChar = char.ToUpper(word[0]);
                        string restOfWord = word.Substring(1);
                        string capitalizedWord = firstChar + restOfWord;
                        result.Append(capitalizedWord + " ");
                    }
                }
                tb_name.Text = result.ToString().TrimEnd();
            }
        }

        private void tb_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tb_reason.Focus();
            }
        }

        private void tb_reason_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tb_location.Focus();
            }
        }

        private void tb_location_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tb_contactInfo.Focus();
            }
        }

        private void tb_contactInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bt_sign_Click(sender,e);
            }
        }

        // ------------------------------Methods For Buttons-----------------------

        private void bt_sign_Click(object sender, EventArgs e)
        {
            // Check if all fields are filled
            if (tb_reason.Text == "" || tb_name.Text == "" || tb_location.Text == "")
            {
                MessageBox.Show("All fields must be filled! Less contact info");
                return;
            }
            signatureInfo.Name = tb_name.Text;
            signatureInfo.Reason = tb_reason.Text;
            signatureInfo.Location = tb_location.Text;

            behaviour = Behaviour.GetDefault().StampWithTextBuilder(Sig.SignAPI.Stamp.DefaultTextBuilder.Get(cb_by.Checked, cb_reason.Checked, cb_location.Checked, cb_time.Checked));

            // Optional field
            if (tb_contactInfo.Text != "")
            {
                signatureInfo.ContactInfo = tb_contactInfo.Text;
            }

            Ok = true;
            this.Close();
        }
    }
}
