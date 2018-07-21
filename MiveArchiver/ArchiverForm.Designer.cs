namespace MiveArchiver
{
    partial class ArchiverForm
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
            this.btnUnzip = new System.Windows.Forms.Button();
            this.btnZip = new System.Windows.Forms.Button();
            this.opdFileDialogZip = new System.Windows.Forms.OpenFileDialog();
            this.opdFileDialogUnzip = new System.Windows.Forms.OpenFileDialog();
            this.tbZip = new System.Windows.Forms.TextBox();
            this.tbUnzip = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUnzip
            // 
            this.btnUnzip.Location = new System.Drawing.Point(422, 76);
            this.btnUnzip.Name = "btnUnzip";
            this.btnUnzip.Size = new System.Drawing.Size(88, 20);
            this.btnUnzip.TabIndex = 1;
            this.btnUnzip.Text = "Unzip";
            this.btnUnzip.UseVisualStyleBackColor = true;
            this.btnUnzip.Click += new System.EventHandler(this.btnUnzip_Click);
            // 
            // btnZip
            // 
            this.btnZip.Location = new System.Drawing.Point(422, 30);
            this.btnZip.Name = "btnZip";
            this.btnZip.Size = new System.Drawing.Size(88, 20);
            this.btnZip.TabIndex = 2;
            this.btnZip.Text = "Zip";
            this.btnZip.UseVisualStyleBackColor = true;
            this.btnZip.Click += new System.EventHandler(this.btnZip_Click);
            // 
            // opdFileDialogZip
            // 
            this.opdFileDialogZip.FileName = "File name";
            // 
            // opdFileDialogUnzip
            // 
            this.opdFileDialogUnzip.FileName = "File name";
            // 
            // tbZip
            // 
            this.tbZip.Location = new System.Drawing.Point(14, 30);
            this.tbZip.Name = "tbZip";
            this.tbZip.ReadOnly = true;
            this.tbZip.Size = new System.Drawing.Size(387, 20);
            this.tbZip.TabIndex = 3;
            // 
            // tbUnzip
            // 
            this.tbUnzip.Location = new System.Drawing.Point(14, 76);
            this.tbUnzip.Name = "tbUnzip";
            this.tbUnzip.ReadOnly = true;
            this.tbUnzip.Size = new System.Drawing.Size(387, 20);
            this.tbUnzip.TabIndex = 4;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(14, 124);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(387, 23);
            this.progressBar.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(422, 124);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ArchiverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 172);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.tbUnzip);
            this.Controls.Add(this.tbZip);
            this.Controls.Add(this.btnZip);
            this.Controls.Add(this.btnUnzip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ArchiverForm";
            this.Text = "Mive Archiver";
            this.Load += new System.EventHandler(this.ArchiverForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnUnzip;
        private System.Windows.Forms.Button btnZip;
        private System.Windows.Forms.OpenFileDialog opdFileDialogZip;
        private System.Windows.Forms.OpenFileDialog opdFileDialogUnzip;
        private System.Windows.Forms.TextBox tbZip;
        private System.Windows.Forms.TextBox tbUnzip;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnCancel;
    }
}

