namespace MyTiskTask4
{
    partial class FormNewUser
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtLogin;
        private TextBox txtPassword;
        private Button btnOk;
        private Button btnCancel;
        private Label label1;
        private Label label2;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtLogin = new TextBox();
            txtPassword = new TextBox();
            btnOk = new Button();
            btnCancel = new Button();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();

            // txtLogin
            txtLogin.Location = new System.Drawing.Point(120, 30);
            txtLogin.Name = "txtLogin";
            txtLogin.Size = new System.Drawing.Size(200, 27);
            txtLogin.TabIndex = 0;

            // txtPassword
            txtPassword.Location = new System.Drawing.Point(120, 70);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new System.Drawing.Size(200, 27);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;

            // btnOk
            btnOk.Location = new System.Drawing.Point(120, 120);
            btnOk.Name = "btnOk";
            btnOk.Size = new System.Drawing.Size(95, 30);
            btnOk.TabIndex = 2;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;

            // btnCancel
            btnCancel.Location = new System.Drawing.Point(225, 120);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(95, 30);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;

            // label1
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(20, 33);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(56, 20);
            label1.Text = "Имя:";

            // label2
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(20, 73);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(63, 20);
            label2.Text = "Пароль:";

            // FormNewUser
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(370, 180);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(txtPassword);
            Controls.Add(txtLogin);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormNewUser";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Новый пользователь";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}