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
            bt_lineColor = new Button();
            bt_saveImage = new Button();
            bt_StopSignature = new Button();
            pb_signature = new PictureBox();
            gb_PDF = new GroupBox();
            bt_nextPage = new Button();
            bt_previousPage = new Button();
            lb_nPages = new Label();
            label2 = new Label();
            lb_pdfSize = new Label();
            lb_page = new Label();
            tb_a4y = new TextBox();
            tb_page = new TextBox();
            bt_signPDFImg = new Button();
            label3 = new Label();
            bt_loadPDF = new Button();
            tb_a4x = new TextBox();
            sb_x = new HScrollBar();
            lb_y = new Label();
            tb_y = new TextBox();
            lb_x = new Label();
            tb_x = new TextBox();
            webBrowser = new WebBrowser();
            pb_pdfView = new PictureBox();
            gb_signatureOptions = new GroupBox();
            sb_y = new HScrollBar();
            lb_sigHeight = new Label();
            lb_sigSize = new Label();
            tb_sigHeight = new TextBox();
            lb_sigWidth = new Label();
            tb_sigWidth = new TextBox();
            pb_signPrev = new PictureBox();
            gb_sign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pb_signature).BeginInit();
            gb_PDF.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pb_pdfView).BeginInit();
            gb_signatureOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pb_signPrev).BeginInit();
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
            gb_sign.Controls.Add(bt_lineColor);
            gb_sign.Controls.Add(bt_saveImage);
            gb_sign.Controls.Add(bt_StopSignature);
            gb_sign.Controls.Add(bt_StartSignature);
            gb_sign.Enabled = false;
            gb_sign.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            gb_sign.Location = new Point(12, 12);
            gb_sign.Name = "gb_sign";
            gb_sign.Size = new Size(142, 156);
            gb_sign.TabIndex = 1;
            gb_sign.TabStop = false;
            gb_sign.Text = "Signature Options";
            // 
            // bt_lineColor
            // 
            bt_lineColor.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            bt_lineColor.Location = new Point(36, 136);
            bt_lineColor.Name = "bt_lineColor";
            bt_lineColor.Size = new Size(72, 20);
            bt_lineColor.TabIndex = 15;
            bt_lineColor.Text = "Line Color";
            bt_lineColor.UseCompatibleTextRendering = true;
            bt_lineColor.UseVisualStyleBackColor = true;
            bt_lineColor.Click += bt_lineColor_Click;
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
            gb_PDF.Controls.Add(bt_nextPage);
            gb_PDF.Controls.Add(bt_previousPage);
            gb_PDF.Controls.Add(lb_nPages);
            gb_PDF.Controls.Add(label2);
            gb_PDF.Controls.Add(lb_pdfSize);
            gb_PDF.Controls.Add(lb_page);
            gb_PDF.Controls.Add(tb_a4y);
            gb_PDF.Controls.Add(tb_page);
            gb_PDF.Controls.Add(bt_signPDFImg);
            gb_PDF.Controls.Add(label3);
            gb_PDF.Controls.Add(bt_loadPDF);
            gb_PDF.Controls.Add(tb_a4x);
            gb_PDF.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            gb_PDF.Location = new Point(12, 174);
            gb_PDF.Name = "gb_PDF";
            gb_PDF.Size = new Size(142, 213);
            gb_PDF.TabIndex = 3;
            gb_PDF.TabStop = false;
            gb_PDF.Text = "PDF Options";
            // 
            // bt_nextPage
            // 
            bt_nextPage.FlatStyle = FlatStyle.Popup;
            bt_nextPage.Font = new Font("Segoe UI", 6.75F, FontStyle.Bold, GraphicsUnit.Point);
            bt_nextPage.Location = new Point(89, 60);
            bt_nextPage.Margin = new Padding(0);
            bt_nextPage.Name = "bt_nextPage";
            bt_nextPage.Size = new Size(16, 26);
            bt_nextPage.TabIndex = 15;
            bt_nextPage.Text = ">";
            bt_nextPage.UseVisualStyleBackColor = true;
            bt_nextPage.Visible = false;
            bt_nextPage.Click += bt_nextPage_Click;
            // 
            // bt_previousPage
            // 
            bt_previousPage.FlatStyle = FlatStyle.Popup;
            bt_previousPage.Font = new Font("Segoe UI", 6.75F, FontStyle.Bold, GraphicsUnit.Point);
            bt_previousPage.Location = new Point(53, 60);
            bt_previousPage.Margin = new Padding(0);
            bt_previousPage.Name = "bt_previousPage";
            bt_previousPage.Size = new Size(16, 26);
            bt_previousPage.TabIndex = 14;
            bt_previousPage.Text = "<";
            bt_previousPage.UseVisualStyleBackColor = true;
            bt_previousPage.Visible = false;
            bt_previousPage.Click += bt_previousPage_Click;
            // 
            // lb_nPages
            // 
            lb_nPages.AutoSize = true;
            lb_nPages.Location = new Point(101, 64);
            lb_nPages.Name = "lb_nPages";
            lb_nPages.Size = new Size(22, 19);
            lb_nPages.TabIndex = 13;
            lb_nPages.Text = "/0";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(16, 189);
            label2.Name = "label2";
            label2.Size = new Size(53, 19);
            label2.TabIndex = 12;
            label2.Text = "Height:";
            // 
            // lb_pdfSize
            // 
            lb_pdfSize.AutoSize = true;
            lb_pdfSize.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lb_pdfSize.Location = new Point(41, 152);
            lb_pdfSize.Name = "lb_pdfSize";
            lb_pdfSize.Size = new Size(64, 19);
            lb_pdfSize.TabIndex = 5;
            lb_pdfSize.Text = "PDF Size:";
            // 
            // lb_page
            // 
            lb_page.AutoSize = true;
            lb_page.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lb_page.Location = new Point(6, 64);
            lb_page.Name = "lb_page";
            lb_page.Size = new Size(42, 19);
            lb_page.TabIndex = 4;
            lb_page.Text = "Page:";
            // 
            // tb_a4y
            // 
            tb_a4y.BorderStyle = BorderStyle.None;
            tb_a4y.Enabled = false;
            tb_a4y.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            tb_a4y.Location = new Point(70, 192);
            tb_a4y.Name = "tb_a4y";
            tb_a4y.ReadOnly = true;
            tb_a4y.Size = new Size(38, 16);
            tb_a4y.TabIndex = 11;
            tb_a4y.TabStop = false;
            tb_a4y.Text = "842";
            tb_a4y.TextAlign = HorizontalAlignment.Center;
            // 
            // tb_page
            // 
            tb_page.Enabled = false;
            tb_page.Location = new Point(66, 61);
            tb_page.Name = "tb_page";
            tb_page.Size = new Size(26, 25);
            tb_page.TabIndex = 3;
            tb_page.Text = "0";
            tb_page.TextAlign = HorizontalAlignment.Center;
            tb_page.TextChanged += tb_page_TextChanged;
            // 
            // bt_signPDFImg
            // 
            bt_signPDFImg.Enabled = false;
            bt_signPDFImg.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            bt_signPDFImg.Location = new Point(6, 116);
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
            label3.Location = new Point(20, 170);
            label3.Name = "label3";
            label3.Size = new Size(49, 19);
            label3.TabIndex = 10;
            label3.Text = "Width:";
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
            // tb_a4x
            // 
            tb_a4x.BorderStyle = BorderStyle.None;
            tb_a4x.Enabled = false;
            tb_a4x.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            tb_a4x.Location = new Point(70, 173);
            tb_a4x.Name = "tb_a4x";
            tb_a4x.ReadOnly = true;
            tb_a4x.Size = new Size(38, 16);
            tb_a4x.TabIndex = 9;
            tb_a4x.TabStop = false;
            tb_a4x.Text = "595";
            tb_a4x.TextAlign = HorizontalAlignment.Center;
            // 
            // sb_x
            // 
            sb_x.Enabled = false;
            sb_x.LargeChange = 1;
            sb_x.Location = new Point(195, 51);
            sb_x.Maximum = 595;
            sb_x.Name = "sb_x";
            sb_x.RightToLeft = RightToLeft.No;
            sb_x.Size = new Size(96, 13);
            sb_x.TabIndex = 6;
            sb_x.Scroll += sb_x_Scroll;
            // 
            // lb_y
            // 
            lb_y.AutoSize = true;
            lb_y.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lb_y.Location = new Point(329, 29);
            lb_y.Name = "lb_y";
            lb_y.Size = new Size(20, 19);
            lb_y.TabIndex = 8;
            lb_y.Text = "Y:";
            // 
            // tb_y
            // 
            tb_y.Enabled = false;
            tb_y.Location = new Point(355, 26);
            tb_y.Name = "tb_y";
            tb_y.Size = new Size(47, 25);
            tb_y.TabIndex = 7;
            tb_y.Text = "0";
            tb_y.TextAlign = HorizontalAlignment.Center;
            tb_y.TextChanged += tb_y_TextChanged;
            // 
            // lb_x
            // 
            lb_x.AutoSize = true;
            lb_x.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lb_x.Location = new Point(207, 29);
            lb_x.Name = "lb_x";
            lb_x.Size = new Size(20, 19);
            lb_x.TabIndex = 6;
            lb_x.Text = "X:";
            // 
            // tb_x
            // 
            tb_x.Enabled = false;
            tb_x.Location = new Point(233, 26);
            tb_x.Name = "tb_x";
            tb_x.Size = new Size(47, 25);
            tb_x.TabIndex = 5;
            tb_x.Text = "0";
            tb_x.TextAlign = HorizontalAlignment.Center;
            tb_x.TextChanged += tb_x_TextChanged;
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
            pb_pdfView.Enabled = false;
            pb_pdfView.Location = new Point(830, 12);
            pb_pdfView.Name = "pb_pdfView";
            pb_pdfView.Size = new Size(282, 375);
            pb_pdfView.TabIndex = 4;
            pb_pdfView.TabStop = false;
            pb_pdfView.MouseClick += pb_pdfView_MouseClick;
            // 
            // gb_signatureOptions
            // 
            gb_signatureOptions.Controls.Add(sb_y);
            gb_signatureOptions.Controls.Add(sb_x);
            gb_signatureOptions.Controls.Add(lb_sigHeight);
            gb_signatureOptions.Controls.Add(lb_y);
            gb_signatureOptions.Controls.Add(lb_sigSize);
            gb_signatureOptions.Controls.Add(tb_y);
            gb_signatureOptions.Controls.Add(tb_sigHeight);
            gb_signatureOptions.Controls.Add(lb_x);
            gb_signatureOptions.Controls.Add(lb_sigWidth);
            gb_signatureOptions.Controls.Add(tb_x);
            gb_signatureOptions.Controls.Add(tb_sigWidth);
            gb_signatureOptions.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            gb_signatureOptions.Location = new Point(408, 318);
            gb_signatureOptions.Name = "gb_signatureOptions";
            gb_signatureOptions.Size = new Size(416, 70);
            gb_signatureOptions.TabIndex = 13;
            gb_signatureOptions.TabStop = false;
            gb_signatureOptions.Text = "PDF Signature Options";
            // 
            // sb_y
            // 
            sb_y.Enabled = false;
            sb_y.LargeChange = 1;
            sb_y.Location = new Point(317, 51);
            sb_y.Maximum = 595;
            sb_y.Name = "sb_y";
            sb_y.RightToLeft = RightToLeft.No;
            sb_y.Size = new Size(96, 13);
            sb_y.TabIndex = 13;
            sb_y.Scroll += sb_y_Scroll;
            // 
            // lb_sigHeight
            // 
            lb_sigHeight.AutoSize = true;
            lb_sigHeight.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lb_sigHeight.Location = new Point(69, 48);
            lb_sigHeight.Name = "lb_sigHeight";
            lb_sigHeight.Size = new Size(53, 19);
            lb_sigHeight.TabIndex = 12;
            lb_sigHeight.Text = "Height:";
            // 
            // lb_sigSize
            // 
            lb_sigSize.AutoSize = true;
            lb_sigSize.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lb_sigSize.Location = new Point(7, 35);
            lb_sigSize.Name = "lb_sigSize";
            lb_sigSize.Size = new Size(57, 19);
            lb_sigSize.TabIndex = 5;
            lb_sigSize.Text = "Sig Size:";
            // 
            // tb_sigHeight
            // 
            tb_sigHeight.BackColor = SystemColors.Window;
            tb_sigHeight.Enabled = false;
            tb_sigHeight.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            tb_sigHeight.Location = new Point(124, 47);
            tb_sigHeight.Name = "tb_sigHeight";
            tb_sigHeight.Size = new Size(38, 22);
            tb_sigHeight.TabIndex = 11;
            tb_sigHeight.TabStop = false;
            tb_sigHeight.Text = "77";
            tb_sigHeight.TextAlign = HorizontalAlignment.Center;
            tb_sigHeight.TextChanged += tb_sigHeight_TextChanged;
            // 
            // lb_sigWidth
            // 
            lb_sigWidth.AutoSize = true;
            lb_sigWidth.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lb_sigWidth.Location = new Point(69, 23);
            lb_sigWidth.Name = "lb_sigWidth";
            lb_sigWidth.Size = new Size(49, 19);
            lb_sigWidth.TabIndex = 10;
            lb_sigWidth.Text = "Width:";
            // 
            // tb_sigWidth
            // 
            tb_sigWidth.BackColor = SystemColors.Window;
            tb_sigWidth.Enabled = false;
            tb_sigWidth.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            tb_sigWidth.Location = new Point(124, 20);
            tb_sigWidth.Name = "tb_sigWidth";
            tb_sigWidth.Size = new Size(38, 22);
            tb_sigWidth.TabIndex = 9;
            tb_sigWidth.TabStop = false;
            tb_sigWidth.Text = "155";
            tb_sigWidth.TextAlign = HorizontalAlignment.Center;
            tb_sigWidth.TextChanged += tb_sigWidth_TextChanged;
            // 
            // pb_signPrev
            // 
            pb_signPrev.BackColor = Color.Transparent;
            pb_signPrev.BackgroundImageLayout = ImageLayout.None;
            pb_signPrev.Enabled = false;
            pb_signPrev.Image = Properties.Resources.signPrev;
            pb_signPrev.Location = new Point(831, 352);
            pb_signPrev.Name = "pb_signPrev";
            pb_signPrev.Size = new Size(73, 34);
            pb_signPrev.SizeMode = PictureBoxSizeMode.StretchImage;
            pb_signPrev.TabIndex = 14;
            pb_signPrev.TabStop = false;
            pb_signPrev.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1124, 400);
            Controls.Add(pb_signPrev);
            Controls.Add(gb_signatureOptions);
            Controls.Add(pb_pdfView);
            Controls.Add(gb_PDF);
            Controls.Add(pb_signature);
            Controls.Add(gb_sign);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "StepOverModel";
            FormClosing += Form1_FormClosing;
            gb_sign.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pb_signature).EndInit();
            gb_PDF.ResumeLayout(false);
            gb_PDF.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pb_pdfView).EndInit();
            gb_signatureOptions.ResumeLayout(false);
            gb_signatureOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pb_signPrev).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button bt_StartSignature;
        private Button bt_StopSignature;
        private Button bt_saveImage;
        private Button bt_signPDFImg;
        private Button bt_loadPDF;
        private Button bt_nextPage;
        private Button bt_previousPage;

        private GroupBox gb_sign;
        private GroupBox gb_PDF;
        private GroupBox gb_signatureOptions;

        private PictureBox pb_signature;
        private PictureBox pb_pdfView;
        private PictureBox pb_signPrev;

        private WebBrowser webBrowser;

        private HScrollBar sb_x;
        private HScrollBar sb_y;

        private Label lb_page;
        private Label lb_y;
        private Label lb_x;
        private Label label2;
        private Label lb_pdfSize;
        private Label label3;
        private Label lb_sigHeight;
        private Label lb_sigSize;
        private Label lb_sigWidth;
        private Label lb_nPages;

        private TextBox tb_page;
        private TextBox tb_y;
        private TextBox tb_x;
        private TextBox tb_a4y;
        private TextBox tb_a4x;
        private TextBox tb_sigHeight;
        private TextBox tb_sigWidth;
        private Button bt_lineColor;
    }
}
