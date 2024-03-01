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
            bg_sign = new GroupBox();
            bt_saveImage = new Button();
            bt_StopSignature = new Button();
            pb_signature = new PictureBox();
            gb_PDF = new GroupBox();
            button1 = new Button();
            button2 = new Button();
            bt_loadPDF = new Button();
            pb_pdfView = new PictureBox();
            bg_sign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pb_signature).BeginInit();
            gb_PDF.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pb_pdfView).BeginInit();
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
            // bg_sign
            // 
            bg_sign.Controls.Add(bt_saveImage);
            bg_sign.Controls.Add(bt_StopSignature);
            bg_sign.Controls.Add(bt_StartSignature);
            bg_sign.Location = new Point(12, 12);
            bg_sign.Name = "bg_sign";
            bg_sign.Size = new Size(116, 111);
            bg_sign.TabIndex = 1;
            bg_sign.TabStop = false;
            bg_sign.Text = "Signature Options";
            // 
            // bt_saveImage
            // 
            bt_saveImage.Location = new Point(6, 51);
            bt_saveImage.Name = "bt_saveImage";
            bt_saveImage.Size = new Size(104, 23);
            bt_saveImage.TabIndex = 2;
            bt_saveImage.Text = "Save Image";
            bt_saveImage.UseVisualStyleBackColor = true;
            bt_saveImage.Click += bt_saveImage_Click;
            // 
            // bt_StopSignature
            // 
            bt_StopSignature.Location = new Point(6, 80);
            bt_StopSignature.Name = "bt_StopSignature";
            bt_StopSignature.Size = new Size(104, 23);
            bt_StopSignature.TabIndex = 1;
            bt_StopSignature.Text = "Stop Signature";
            bt_StopSignature.UseVisualStyleBackColor = true;
            bt_StopSignature.Click += bt_StopSignature_Click;
            // 
            // pb_signature
            // 
            pb_signature.BackColor = SystemColors.ControlLight;
            pb_signature.BorderStyle = BorderStyle.FixedSingle;
            pb_signature.Cursor = Cursors.Cross;
            pb_signature.Location = new Point(149, 12);
            pb_signature.Name = "pb_signature";
            pb_signature.Size = new Size(427, 191);
            pb_signature.TabIndex = 2;
            pb_signature.TabStop = false;
            // 
            // gb_PDF
            // 
            gb_PDF.Controls.Add(button1);
            gb_PDF.Controls.Add(button2);
            gb_PDF.Controls.Add(bt_loadPDF);
            gb_PDF.Location = new Point(12, 129);
            gb_PDF.Name = "gb_PDF";
            gb_PDF.Size = new Size(116, 111);
            gb_PDF.TabIndex = 3;
            gb_PDF.TabStop = false;
            gb_PDF.Text = "PDF Options";
            // 
            // button1
            // 
            button1.Location = new Point(6, 51);
            button1.Name = "button1";
            button1.Size = new Size(104, 23);
            button1.TabIndex = 2;
            button1.Text = "Save Image";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(6, 80);
            button2.Name = "button2";
            button2.Size = new Size(104, 23);
            button2.TabIndex = 1;
            button2.Text = "Stop Signature";
            button2.UseVisualStyleBackColor = true;
            // 
            // bt_loadPDF
            // 
            bt_loadPDF.Location = new Point(6, 22);
            bt_loadPDF.Name = "bt_loadPDF";
            bt_loadPDF.Size = new Size(104, 23);
            bt_loadPDF.TabIndex = 0;
            bt_loadPDF.Text = "Load PDF";
            bt_loadPDF.UseVisualStyleBackColor = true;
            bt_loadPDF.Click += bt_loadPDF_Click;
            // 
            // pb_pdfView
            // 
            pb_pdfView.BackColor = SystemColors.ControlLight;
            pb_pdfView.BorderStyle = BorderStyle.FixedSingle;
            pb_pdfView.Cursor = Cursors.Cross;
            pb_pdfView.Location = new Point(582, 12);
            pb_pdfView.Name = "pb_pdfView";
            pb_pdfView.Size = new Size(199, 306);
            pb_pdfView.TabIndex = 4;
            pb_pdfView.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(795, 325);
            Controls.Add(pb_pdfView);
            Controls.Add(gb_PDF);
            Controls.Add(pb_signature);
            Controls.Add(bg_sign);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "StepOverModel";
            bg_sign.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pb_signature).EndInit();
            gb_PDF.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pb_pdfView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button bt_StartSignature;
        private GroupBox bg_sign;
        private Button bt_StopSignature;
        private PictureBox pb_signature;
        private Button bt_saveImage;
        private GroupBox gb_PDF;
        private Button button1;
        private Button button2;
        private Button bt_loadPDF;
        private PictureBox pb_pdfView;
    }
}
