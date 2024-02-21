using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Xml.Linq;

namespace StepOverModel
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            bt_StartSignature = new Button();
            groupBox1 = new GroupBox();
            bt_StopSignature = new Button();
            pb_signature = new PictureBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pb_signature).BeginInit();
            SuspendLayout();
            // 
            // bt_StartSignature
            // 
            bt_StartSignature.Location = new Point(6, 22);
            bt_StartSignature.Name = "bt_StartSignature";
            bt_StartSignature.Size = new Size(104, 23);
            bt_StartSignature.TabIndex = 0;
            bt_StartSignature.Text = "Start Signature";
            bt_StartSignature.UseVisualStyleBackColor = true;
            bt_StartSignature.Click += bt_StartSignature_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(bt_StopSignature);
            groupBox1.Controls.Add(bt_StartSignature);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(116, 85);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Signature Options";
            // 
            // bt_StopSignature
            // 
            bt_StopSignature.Location = new Point(6, 51);
            bt_StopSignature.Name = "bt_StopSignature";
            bt_StopSignature.Size = new Size(104, 23);
            bt_StopSignature.TabIndex = 1;
            bt_StopSignature.Text = "Cancel Signature";
            bt_StopSignature.UseVisualStyleBackColor = true;
            bt_StopSignature.Click += bt_StopSignature_Click;
            // 
            // pb_signature
            // 
            pb_signature.BackColor = SystemColors.ControlLight;
            pb_signature.BorderStyle = BorderStyle.FixedSingle;
            pb_signature.Cursor = Cursors.Cross;
            pb_signature.Location = new Point(172, 12);
            pb_signature.Name = "pb_signature";
            pb_signature.Size = new Size(442, 222);
            pb_signature.TabIndex = 2;
            pb_signature.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(638, 263);
            Controls.Add(pb_signature);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "StepOverModel";
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pb_signature).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button bt_StartSignature;
        private GroupBox groupBox1;
        private Button bt_StopSignature;
        private PictureBox pb_signature;
    }
}
