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
            gb_sign = new GroupBox();
            bt_saveImage = new Button();
            bt_StopSignature = new Button();
            pb_signature = new PictureBox();
            gb_PDF = new GroupBox();
            label2 = new Label();
            label1 = new Label();
            lb_page = new Label();
            textBox1 = new TextBox();
            tb_page = new TextBox();
            bt_signPDFImg = new Button();
            label3 = new Label();
            bt_loadPDF = new Button();
            textBox2 = new TextBox();
            gb_coordSign = new GroupBox();
            lb_y = new Label();
            tb_y = new TextBox();
            lb_x = new Label();
            tb_x = new TextBox();
            webBrowser = new WebBrowser();
            pb_pdfView = new PictureBox();
            gb_sign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pb_signature).BeginInit();
            gb_PDF.SuspendLayout();
            gb_coordSign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pb_pdfView).BeginInit();
            SuspendLayout();
            // 
            // bt_StartSignature
            // 
            bt_StartSignature.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            bt_StartSignature.Location = new Point(6, 24);
            bt_StartSignature.Name = "bt_StartSignature";
            bt_StartSignature.Size = new Size(131, 33);
            bt_StartSignature.TabIndex = 0;
            bt_StartSignature.Text = "Start Signature";
            bt_StartSignature.UseVisualStyleBackColor = true;
            bt_StartSignature.Click += bt_StartSignature_Click;
            // 
            // gb_sign
            // 
            gb_sign.Controls.Add(bt_saveImage);
            gb_sign.Controls.Add(bt_StopSignature);
            gb_sign.Controls.Add(bt_StartSignature);
            gb_sign.Enabled = false;
            gb_sign.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            gb_sign.Location = new Point(12, 12);
            gb_sign.Name = "gb_sign";
            gb_sign.Size = new Size(142, 142);
            gb_sign.TabIndex = 1;
            gb_sign.TabStop = false;
            gb_sign.Text = "Signature Options";
            // 
            // bt_saveImage
            // 
            bt_saveImage.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            bt_saveImage.Location = new Point(6, 63);
            bt_saveImage.Name = "bt_saveImage";
            bt_saveImage.Size = new Size(131, 33);
            bt_saveImage.TabIndex = 2;
            bt_saveImage.Text = "Save Image";
            bt_saveImage.UseVisualStyleBackColor = true;
            bt_saveImage.Click += bt_saveImage_Click;
            // 
            // bt_StopSignature
            // 
            bt_StopSignature.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            bt_StopSignature.Location = new Point(6, 102);
            bt_StopSignature.Name = "bt_StopSignature";
            bt_StopSignature.Size = new Size(131, 33);
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
            pb_signature.Location = new Point(210, 12);
            pb_signature.Name = "pb_signature";
            pb_signature.Size = new Size(600, 300);
            pb_signature.TabIndex = 2;
            pb_signature.TabStop = false;
            // 
            // gb_PDF
            // 
            gb_PDF.Controls.Add(label2);
            gb_PDF.Controls.Add(label1);
            gb_PDF.Controls.Add(lb_page);
            gb_PDF.Controls.Add(textBox1);
            gb_PDF.Controls.Add(tb_page);
            gb_PDF.Controls.Add(bt_signPDFImg);
            gb_PDF.Controls.Add(label3);
            gb_PDF.Controls.Add(bt_loadPDF);
            gb_PDF.Controls.Add(textBox2);
            gb_PDF.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            gb_PDF.Location = new Point(12, 174);
            gb_PDF.Name = "gb_PDF";
            gb_PDF.Size = new Size(142, 213);
            gb_PDF.TabIndex = 3;
            gb_PDF.TabStop = false;
            gb_PDF.Text = "PDF Options";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(72, 182);
            label2.Name = "label2";
            label2.Size = new Size(20, 19);
            label2.TabIndex = 12;
            label2.Text = "Y:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(10, 173);
            label1.Name = "label1";
            label1.Size = new Size(56, 19);
            label1.TabIndex = 5;
            label1.Text = "PDF inf:";
            // 
            // lb_page
            // 
            lb_page.AutoSize = true;
            lb_page.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lb_page.Location = new Point(9, 64);
            lb_page.Name = "lb_page";
            lb_page.Size = new Size(42, 19);
            lb_page.TabIndex = 4;
            lb_page.Text = "Page:";
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Enabled = false;
            textBox1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(98, 182);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(38, 16);
            textBox1.TabIndex = 11;
            textBox1.TabStop = false;
            textBox1.Text = "842";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // tb_page
            // 
            tb_page.Location = new Point(98, 61);
            tb_page.Name = "tb_page";
            tb_page.Size = new Size(38, 25);
            tb_page.TabIndex = 3;
            tb_page.Text = "1";
            tb_page.TextAlign = HorizontalAlignment.Center;
            // 
            // bt_signPDFImg
            // 
            bt_signPDFImg.Enabled = false;
            bt_signPDFImg.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            bt_signPDFImg.Location = new Point(5, 90);
            bt_signPDFImg.Name = "bt_signPDFImg";
            bt_signPDFImg.Size = new Size(131, 33);
            bt_signPDFImg.TabIndex = 2;
            bt_signPDFImg.Text = "Sign img PDF";
            bt_signPDFImg.UseVisualStyleBackColor = true;
            bt_signPDFImg.Click += bt_signPDFImg_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(72, 153);
            label3.Name = "label3";
            label3.Size = new Size(20, 19);
            label3.TabIndex = 10;
            label3.Text = "X:";
            // 
            // bt_loadPDF
            // 
            bt_loadPDF.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            bt_loadPDF.Location = new Point(6, 22);
            bt_loadPDF.Name = "bt_loadPDF";
            bt_loadPDF.Size = new Size(131, 33);
            bt_loadPDF.TabIndex = 0;
            bt_loadPDF.Text = "Load PDF";
            bt_loadPDF.UseVisualStyleBackColor = true;
            bt_loadPDF.Click += bt_loadPDF_Click;
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Enabled = false;
            textBox2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            textBox2.Location = new Point(98, 153);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(38, 16);
            textBox2.TabIndex = 9;
            textBox2.TabStop = false;
            textBox2.Text = "595";
            textBox2.TextAlign = HorizontalAlignment.Center;
            // 
            // gb_coordSign
            // 
            gb_coordSign.Controls.Add(lb_y);
            gb_coordSign.Controls.Add(tb_y);
            gb_coordSign.Controls.Add(lb_x);
            gb_coordSign.Controls.Add(tb_x);
            gb_coordSign.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            gb_coordSign.Location = new Point(583, 318);
            gb_coordSign.Name = "gb_coordSign";
            gb_coordSign.Size = new Size(241, 69);
            gb_coordSign.TabIndex = 5;
            gb_coordSign.TabStop = false;
            gb_coordSign.Text = "Coord for signature";
            // 
            // lb_y
            // 
            lb_y.AutoSize = true;
            lb_y.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lb_y.Location = new Point(128, 32);
            lb_y.Name = "lb_y";
            lb_y.Size = new Size(20, 19);
            lb_y.TabIndex = 8;
            lb_y.Text = "Y:";
            // 
            // tb_y
            // 
            tb_y.Enabled = false;
            tb_y.Location = new Point(154, 29);
            tb_y.Name = "tb_y";
            tb_y.Size = new Size(47, 25);
            tb_y.TabIndex = 7;
            tb_y.Text = "4";
            tb_y.TextAlign = HorizontalAlignment.Center;
            // 
            // lb_x
            // 
            lb_x.AutoSize = true;
            lb_x.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lb_x.Location = new Point(6, 32);
            lb_x.Name = "lb_x";
            lb_x.Size = new Size(20, 19);
            lb_x.TabIndex = 6;
            lb_x.Text = "X:";
            // 
            // tb_x
            // 
            tb_x.Enabled = false;
            tb_x.Location = new Point(32, 29);
            tb_x.Name = "tb_x";
            tb_x.Size = new Size(47, 25);
            tb_x.TabIndex = 5;
            tb_x.Text = "440";
            tb_x.TextAlign = HorizontalAlignment.Center;
            // 
            // webBrowser
            // 
            webBrowser.Location = new Point(830, 12);
            webBrowser.Name = "webBrowser";
            webBrowser.Size = new Size(282, 375);
            webBrowser.TabIndex = 4;
            webBrowser.TabStop = false;
            // 
            // pb_pdfView
            // 
            pb_pdfView.BackColor = SystemColors.ControlLight;
            pb_pdfView.BorderStyle = BorderStyle.FixedSingle;
            pb_pdfView.Cursor = Cursors.Cross;
            pb_pdfView.Location = new Point(830, 12);
            pb_pdfView.Name = "pb_pdfView";
            pb_pdfView.Size = new Size(282, 375);
            pb_pdfView.TabIndex = 4;
            pb_pdfView.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1124, 400);
            Controls.Add(gb_coordSign);
            Controls.Add(pb_pdfView);
            Controls.Add(gb_PDF);
            Controls.Add(pb_signature);
            Controls.Add(gb_sign);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "StepOverModel";
            gb_sign.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pb_signature).EndInit();
            gb_PDF.ResumeLayout(false);
            gb_PDF.PerformLayout();
            gb_coordSign.ResumeLayout(false);
            gb_coordSign.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pb_pdfView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button bt_StartSignature;
        private GroupBox gb_sign;
        private Button bt_StopSignature;
        private PictureBox pb_signature;
        private Button bt_saveImage;
        private GroupBox gb_PDF;
        private Button bt_signPDFImg;
        private Button bt_loadPDF;
        private Label lb_page;
        private TextBox tb_page;
        private GroupBox gb_coordSign;
        private Label lb_y;
        private TextBox tb_y;
        private Label lb_x;
        private TextBox tb_x;
        private WebBrowser webBrowser;
        private PictureBox pb_pdfView;
        private Label label2;
        private Label label1;
        private TextBox textBox1;
        private Label label3;
        private TextBox textBox2;
    }
}
