
namespace Pack
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnLoadFiles = new System.Windows.Forms.Button();
            this.OFD = new System.Windows.Forms.OpenFileDialog();
            this.btnUnloadFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoadFiles
            // 
            this.btnLoadFiles.Location = new System.Drawing.Point(12, 12);
            this.btnLoadFiles.Name = "btnLoadFiles";
            this.btnLoadFiles.Size = new System.Drawing.Size(165, 23);
            this.btnLoadFiles.TabIndex = 0;
            this.btnLoadFiles.Text = "Pack files";
            this.btnLoadFiles.UseVisualStyleBackColor = true;
            this.btnLoadFiles.Click += new System.EventHandler(this.btnLoadFiles_Click);
            // 
            // OFD
            // 
            this.OFD.Multiselect = true;
            // 
            // btnUnloadFile
            // 
            this.btnUnloadFile.Location = new System.Drawing.Point(12, 41);
            this.btnUnloadFile.Name = "btnUnloadFile";
            this.btnUnloadFile.Size = new System.Drawing.Size(165, 23);
            this.btnUnloadFile.TabIndex = 1;
            this.btnUnloadFile.Text = "Unpack file";
            this.btnUnloadFile.UseVisualStyleBackColor = true;
            this.btnUnloadFile.Click += new System.EventHandler(this.btnUnloadFile_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(189, 75);
            this.Controls.Add(this.btnUnloadFile);
            this.Controls.Add(this.btnLoadFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "Packer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadFiles;
        private System.Windows.Forms.OpenFileDialog OFD;
        private System.Windows.Forms.Button btnUnloadFile;
    }
}

