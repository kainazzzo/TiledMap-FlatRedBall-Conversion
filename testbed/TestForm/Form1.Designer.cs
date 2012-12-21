namespace TestForm
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.tmxFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tmxFilename = new System.Windows.Forms.TextBox();
            this.tmxButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tmxDestinationFolder = new System.Windows.Forms.TextBox();
            this.tmxDestinationButton = new System.Windows.Forms.Button();
            this.tmxConvertToScnx = new System.Windows.Forms.Button();
            this.tmxConvertToShcx = new System.Windows.Forms.Button();
            this.tmxConvertToNntx = new System.Windows.Forms.Button();
            this.tmxFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tmxCSVButton = new System.Windows.Forms.Button();
            this.tmxLayerCSVButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "TMX File:";
            // 
            // tmxFileDialog
            // 
            this.tmxFileDialog.DefaultExt = "tmx";
            this.tmxFileDialog.Filter = "TMX File|*.tmx";
            this.tmxFileDialog.Title = "Choose a TMX File to Open";
            this.tmxFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.TmxFiledialogOk);
            // 
            // tmxFilename
            // 
            this.tmxFilename.Location = new System.Drawing.Point(124, 12);
            this.tmxFilename.Name = "tmxFilename";
            this.tmxFilename.Size = new System.Drawing.Size(354, 20);
            this.tmxFilename.TabIndex = 1;
            // 
            // tmxButton
            // 
            this.tmxButton.Location = new System.Drawing.Point(484, 9);
            this.tmxButton.Name = "tmxButton";
            this.tmxButton.Size = new System.Drawing.Size(24, 23);
            this.tmxButton.TabIndex = 2;
            this.tmxButton.Text = "...";
            this.tmxButton.UseVisualStyleBackColor = true;
            this.tmxButton.Click += new System.EventHandler(this.tmxButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Destination Folder:";
            // 
            // tmxDestinationFolder
            // 
            this.tmxDestinationFolder.Location = new System.Drawing.Point(124, 46);
            this.tmxDestinationFolder.Name = "tmxDestinationFolder";
            this.tmxDestinationFolder.Size = new System.Drawing.Size(354, 20);
            this.tmxDestinationFolder.TabIndex = 4;
            // 
            // tmxDestinationButton
            // 
            this.tmxDestinationButton.Location = new System.Drawing.Point(484, 43);
            this.tmxDestinationButton.Name = "tmxDestinationButton";
            this.tmxDestinationButton.Size = new System.Drawing.Size(24, 23);
            this.tmxDestinationButton.TabIndex = 5;
            this.tmxDestinationButton.Text = "...";
            this.tmxDestinationButton.UseVisualStyleBackColor = true;
            this.tmxDestinationButton.Click += new System.EventHandler(this.tmxDestinationButton_Click);
            // 
            // tmxConvertToScnx
            // 
            this.tmxConvertToScnx.Location = new System.Drawing.Point(228, 103);
            this.tmxConvertToScnx.Name = "tmxConvertToScnx";
            this.tmxConvertToScnx.Size = new System.Drawing.Size(75, 23);
            this.tmxConvertToScnx.TabIndex = 6;
            this.tmxConvertToScnx.Text = "SCNX";
            this.tmxConvertToScnx.UseVisualStyleBackColor = true;
            this.tmxConvertToScnx.Click += new System.EventHandler(this.tmxConvertToScnx_Click);
            // 
            // tmxConvertToShcx
            // 
            this.tmxConvertToShcx.Location = new System.Drawing.Point(390, 103);
            this.tmxConvertToShcx.Name = "tmxConvertToShcx";
            this.tmxConvertToShcx.Size = new System.Drawing.Size(75, 23);
            this.tmxConvertToShcx.TabIndex = 7;
            this.tmxConvertToShcx.Text = "SHCX";
            this.tmxConvertToShcx.UseVisualStyleBackColor = true;
            this.tmxConvertToShcx.Click += new System.EventHandler(this.tmxConvertToShcx_Click);
            // 
            // tmxConvertToNntx
            // 
            this.tmxConvertToNntx.Location = new System.Drawing.Point(309, 103);
            this.tmxConvertToNntx.Name = "tmxConvertToNntx";
            this.tmxConvertToNntx.Size = new System.Drawing.Size(75, 23);
            this.tmxConvertToNntx.TabIndex = 8;
            this.tmxConvertToNntx.Text = "NNTX";
            this.tmxConvertToNntx.UseVisualStyleBackColor = true;
            this.tmxConvertToNntx.Click += new System.EventHandler(this.tmxConvertToNntx_Click);
            // 
            // tmxFolderDialog
            // 
            this.tmxFolderDialog.Description = "Choose the destination folder";
            // 
            // tmxCSVButton
            // 
            this.tmxCSVButton.Location = new System.Drawing.Point(147, 103);
            this.tmxCSVButton.Name = "tmxCSVButton";
            this.tmxCSVButton.Size = new System.Drawing.Size(75, 23);
            this.tmxCSVButton.TabIndex = 9;
            this.tmxCSVButton.Text = "Tile CSV";
            this.tmxCSVButton.UseVisualStyleBackColor = true;
            this.tmxCSVButton.Click += new System.EventHandler(this.tmxCSVButton_Click);
            // 
            // tmxLayerCSVButton
            // 
            this.tmxLayerCSVButton.Location = new System.Drawing.Point(66, 103);
            this.tmxLayerCSVButton.Name = "tmxLayerCSVButton";
            this.tmxLayerCSVButton.Size = new System.Drawing.Size(75, 23);
            this.tmxLayerCSVButton.TabIndex = 10;
            this.tmxLayerCSVButton.Text = "Layer CSV";
            this.tmxLayerCSVButton.UseVisualStyleBackColor = true;
            this.tmxLayerCSVButton.Click += new System.EventHandler(this.tmxLayerCSVButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 138);
            this.Controls.Add(this.tmxLayerCSVButton);
            this.Controls.Add(this.tmxCSVButton);
            this.Controls.Add(this.tmxConvertToNntx);
            this.Controls.Add(this.tmxConvertToShcx);
            this.Controls.Add(this.tmxConvertToScnx);
            this.Controls.Add(this.tmxDestinationButton);
            this.Controls.Add(this.tmxDestinationFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tmxButton);
            this.Controls.Add(this.tmxFilename);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog tmxFileDialog;
        private System.Windows.Forms.TextBox tmxFilename;
        private System.Windows.Forms.Button tmxButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tmxDestinationFolder;
        private System.Windows.Forms.Button tmxDestinationButton;
        private System.Windows.Forms.Button tmxConvertToScnx;
        private System.Windows.Forms.Button tmxConvertToShcx;
        private System.Windows.Forms.Button tmxConvertToNntx;
        private System.Windows.Forms.FolderBrowserDialog tmxFolderDialog;
        private System.Windows.Forms.Button tmxCSVButton;
        private System.Windows.Forms.Button tmxLayerCSVButton;
    }
}

