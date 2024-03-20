namespace StepOverModel
{
    partial class SignInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tb_reason = new TextBox();
            tb_name = new TextBox();
            tb_location = new TextBox();
            tb_contactInfo = new TextBox();
            bt_sign = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            cb_by = new CheckBox();
            cb_reason = new CheckBox();
            cb_time = new CheckBox();
            cb_location = new CheckBox();
            SuspendLayout();
            // 
            // tb_reason
            // 
            tb_reason.Location = new Point(142, 79);
            tb_reason.Name = "tb_reason";
            tb_reason.Size = new Size(100, 23);
            tb_reason.TabIndex = 1;
            tb_reason.Text = "I agree to the terms";
            // 
            // tb_name
            // 
            tb_name.Location = new Point(142, 30);
            tb_name.Name = "tb_name";
            tb_name.Size = new Size(100, 23);
            tb_name.TabIndex = 0;
            tb_name.Text = "Signer";
            // 
            // tb_location
            // 
            tb_location.Location = new Point(142, 122);
            tb_location.Name = "tb_location";
            tb_location.Size = new Size(100, 23);
            tb_location.TabIndex = 2;
            tb_location.Text = "Here";
            // 
            // tb_contactInfo
            // 
            tb_contactInfo.Location = new Point(142, 171);
            tb_contactInfo.Name = "tb_contactInfo";
            tb_contactInfo.Size = new Size(100, 23);
            tb_contactInfo.TabIndex = 3;
            // 
            // bt_sign
            // 
            bt_sign.Location = new Point(152, 209);
            bt_sign.Name = "bt_sign";
            bt_sign.Size = new Size(75, 23);
            bt_sign.TabIndex = 4;
            bt_sign.Text = "Sign";
            bt_sign.UseVisualStyleBackColor = true;
            bt_sign.Click += bt_sign_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(145, 18);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 5;
            label1.Text = "Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(146, 67);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 6;
            label2.Text = "Reason";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(145, 110);
            label3.Name = "label3";
            label3.Size = new Size(53, 15);
            label3.TabIndex = 7;
            label3.Text = "Location";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(145, 159);
            label4.Name = "label4";
            label4.Size = new Size(70, 15);
            label4.TabIndex = 8;
            label4.Text = "ContactInfo";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(27, 9);
            label5.Name = "label5";
            label5.Size = new Size(95, 15);
            label5.TabIndex = 9;
            label5.Text = "Apparent in Sign";
            // 
            // cb_by
            // 
            cb_by.AutoSize = true;
            cb_by.Location = new Point(12, 32);
            cb_by.Name = "cb_by";
            cb_by.Size = new Size(81, 19);
            cb_by.TabIndex = 10;
            cb_by.Text = "Signed by:";
            cb_by.UseVisualStyleBackColor = true;
            // 
            // cb_reason
            // 
            cb_reason.AutoSize = true;
            cb_reason.Location = new Point(12, 81);
            cb_reason.Name = "cb_reason";
            cb_reason.Size = new Size(127, 19);
            cb_reason.TabIndex = 11;
            cb_reason.Text = "Reason for signing:";
            cb_reason.UseVisualStyleBackColor = true;
            // 
            // cb_time
            // 
            cb_time.AutoSize = true;
            cb_time.Location = new Point(12, 209);
            cb_time.Name = "cb_time";
            cb_time.Size = new Size(52, 19);
            cb_time.TabIndex = 13;
            cb_time.Text = "Time";
            cb_time.UseVisualStyleBackColor = true;
            // 
            // cb_location
            // 
            cb_location.AutoSize = true;
            cb_location.Location = new Point(12, 124);
            cb_location.Name = "cb_location";
            cb_location.Size = new Size(75, 19);
            cb_location.TabIndex = 12;
            cb_location.Text = "Location:";
            cb_location.UseVisualStyleBackColor = true;
            // 
            // SignInfo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(266, 253);
            Controls.Add(cb_time);
            Controls.Add(cb_location);
            Controls.Add(cb_reason);
            Controls.Add(cb_by);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(bt_sign);
            Controls.Add(tb_location);
            Controls.Add(tb_contactInfo);
            Controls.Add(tb_name);
            Controls.Add(tb_reason);
            Name = "SignInfo";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SignInfo";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tb_reason;
        private TextBox tb_name;
        private TextBox tb_location;
        private TextBox tb_contactInfo;
        private Button bt_sign;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private CheckBox cb_by;
        private CheckBox cb_reason;
        private CheckBox cb_time;
        private CheckBox cb_location;
    }
}