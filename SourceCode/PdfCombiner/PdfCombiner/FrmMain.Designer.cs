namespace PdfCombiner
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.lblDetails = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOrderByPathAscending = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOrderByPathDescending = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOrderByNameAscending = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOrderByNameDescending = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCombinePdfSharp = new System.Windows.Forms.Button();
            this.btnAddFolder = new System.Windows.Forms.Button();
            this.btnAddFile = new System.Windows.Forms.Button();
            this.btnClearList = new System.Windows.Forms.Button();
            this.pbFiles = new System.Windows.Forms.ProgressBar();
            this.btnCombineITextSharp = new System.Windows.Forms.Button();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Location = new System.Drawing.Point(12, 65);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(0, 17);
            this.lblDetails.TabIndex = 6;
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemDelete,
            this.menuItemOrderByPathAscending,
            this.menuItemOrderByPathDescending,
            this.menuItemOrderByNameAscending,
            this.menuItemOrderByNameDescending});
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(277, 134);
            // 
            // menuItemDelete
            // 
            this.menuItemDelete.Image = ((System.Drawing.Image)(resources.GetObject("menuItemDelete.Image")));
            this.menuItemDelete.Name = "menuItemDelete";
            this.menuItemDelete.Size = new System.Drawing.Size(276, 26);
            this.menuItemDelete.Text = "Delete";
            this.menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
            // 
            // menuItemOrderByPathAscending
            // 
            this.menuItemOrderByPathAscending.Image = ((System.Drawing.Image)(resources.GetObject("menuItemOrderByPathAscending.Image")));
            this.menuItemOrderByPathAscending.Name = "menuItemOrderByPathAscending";
            this.menuItemOrderByPathAscending.Size = new System.Drawing.Size(276, 26);
            this.menuItemOrderByPathAscending.Text = "Order By Path (Ascending)";
            this.menuItemOrderByPathAscending.Click += new System.EventHandler(this.menuItemOrderByPathAscending_Click);
            // 
            // menuItemOrderByPathDescending
            // 
            this.menuItemOrderByPathDescending.Image = ((System.Drawing.Image)(resources.GetObject("menuItemOrderByPathDescending.Image")));
            this.menuItemOrderByPathDescending.Name = "menuItemOrderByPathDescending";
            this.menuItemOrderByPathDescending.Size = new System.Drawing.Size(276, 26);
            this.menuItemOrderByPathDescending.Text = "Order By Path (Descending)";
            this.menuItemOrderByPathDescending.Click += new System.EventHandler(this.menuItemOrderByPathDescending_Click);
            // 
            // menuItemOrderByNameAscending
            // 
            this.menuItemOrderByNameAscending.Image = ((System.Drawing.Image)(resources.GetObject("menuItemOrderByNameAscending.Image")));
            this.menuItemOrderByNameAscending.Name = "menuItemOrderByNameAscending";
            this.menuItemOrderByNameAscending.Size = new System.Drawing.Size(276, 26);
            this.menuItemOrderByNameAscending.Text = "Order By Name (Ascending)";
            this.menuItemOrderByNameAscending.Click += new System.EventHandler(this.menuItemOrderByNameAscending_Click);
            // 
            // menuItemOrderByNameDescending
            // 
            this.menuItemOrderByNameDescending.Image = ((System.Drawing.Image)(resources.GetObject("menuItemOrderByNameDescending.Image")));
            this.menuItemOrderByNameDescending.Name = "menuItemOrderByNameDescending";
            this.menuItemOrderByNameDescending.Size = new System.Drawing.Size(276, 26);
            this.menuItemOrderByNameDescending.Text = "Order By Name (Descending)";
            this.menuItemOrderByNameDescending.Click += new System.EventHandler(this.menuItemOrderByNameDescending_Click);
            // 
            // btnCombinePdfSharp
            // 
            this.btnCombinePdfSharp.Image = ((System.Drawing.Image)(resources.GetObject("btnCombinePdfSharp.Image")));
            this.btnCombinePdfSharp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCombinePdfSharp.Location = new System.Drawing.Point(436, 6);
            this.btnCombinePdfSharp.Name = "btnCombinePdfSharp";
            this.btnCombinePdfSharp.Size = new System.Drawing.Size(214, 37);
            this.btnCombinePdfSharp.TabIndex = 39;
            this.btnCombinePdfSharp.Text = "Combine Files (PdfSharp)";
            this.btnCombinePdfSharp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCombinePdfSharp.UseVisualStyleBackColor = true;
            this.btnCombinePdfSharp.Click += new System.EventHandler(this.btnCombinePdfSharp_Click);
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnAddFolder.Image")));
            this.btnAddFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddFolder.Location = new System.Drawing.Point(191, 6);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Size = new System.Drawing.Size(171, 37);
            this.btnAddFolder.TabIndex = 40;
            this.btnAddFolder.Text = "Add Folder";
            this.btnAddFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddFolder.UseVisualStyleBackColor = true;
            this.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);
            // 
            // btnAddFile
            // 
            this.btnAddFile.Image = ((System.Drawing.Image)(resources.GetObject("btnAddFile.Image")));
            this.btnAddFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddFile.Location = new System.Drawing.Point(6, 6);
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(179, 37);
            this.btnAddFile.TabIndex = 41;
            this.btnAddFile.Text = "Add Files";
            this.btnAddFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddFile.UseVisualStyleBackColor = true;
            this.btnAddFile.Click += new System.EventHandler(this.btnAddFile_Click);
            // 
            // btnClearList
            // 
            this.btnClearList.Image = ((System.Drawing.Image)(resources.GetObject("btnClearList.Image")));
            this.btnClearList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearList.Location = new System.Drawing.Point(6, 46);
            this.btnClearList.Name = "btnClearList";
            this.btnClearList.Size = new System.Drawing.Size(179, 37);
            this.btnClearList.TabIndex = 42;
            this.btnClearList.Text = "Clear File List";
            this.btnClearList.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClearList.UseVisualStyleBackColor = true;
            this.btnClearList.Click += new System.EventHandler(this.btnClearList_Click);
            // 
            // pbFiles
            // 
            this.pbFiles.Location = new System.Drawing.Point(191, 46);
            this.pbFiles.Name = "pbFiles";
            this.pbFiles.Size = new System.Drawing.Size(239, 37);
            this.pbFiles.TabIndex = 43;
            // 
            // btnCombineITextSharp
            // 
            this.btnCombineITextSharp.Image = ((System.Drawing.Image)(resources.GetObject("btnCombineITextSharp.Image")));
            this.btnCombineITextSharp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCombineITextSharp.Location = new System.Drawing.Point(436, 46);
            this.btnCombineITextSharp.Name = "btnCombineITextSharp";
            this.btnCombineITextSharp.Size = new System.Drawing.Size(214, 37);
            this.btnCombineITextSharp.TabIndex = 44;
            this.btnCombineITextSharp.Text = "Combine Files (iTextSharp)";
            this.btnCombineITextSharp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCombineITextSharp.UseVisualStyleBackColor = true;
            this.btnCombineITextSharp.Click += new System.EventHandler(this.btnCombineITextSharp_Click);
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(162)));
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.ItemHeight = 25;
            this.cmbLanguage.Items.AddRange(new object[] {
            "EN",
            "TR",
            "DE",
            "FR",
            "RU",
            "ES"});
            this.cmbLanguage.Location = new System.Drawing.Point(368, 7);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(62, 33);
            this.cmbLanguage.TabIndex = 45;
            this.cmbLanguage.Text = "Select Language";
            this.cmbLanguage.SelectedIndexChanged += new System.EventHandler(this.cmbLanguage_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblInfo);
            this.panel1.Controls.Add(this.cmbLanguage);
            this.panel1.Controls.Add(this.btnCombineITextSharp);
            this.panel1.Controls.Add(this.pbFiles);
            this.panel1.Controls.Add(this.btnClearList);
            this.panel1.Controls.Add(this.btnAddFile);
            this.panel1.Controls.Add(this.btnAddFolder);
            this.panel1.Controls.Add(this.btnCombinePdfSharp);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(666, 116);
            this.panel1.TabIndex = 12;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbFiles);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 116);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(666, 292);
            this.panel2.TabIndex = 13;
            // 
            // lbFiles
            // 
            this.lbFiles.AllowDrop = true;
            this.lbFiles.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lbFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.ItemHeight = 20;
            this.lbFiles.Location = new System.Drawing.Point(0, 0);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbFiles.Size = new System.Drawing.Size(666, 292);
            this.lbFiles.TabIndex = 5;
            this.lbFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbFiles_DragDrop);
            this.lbFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbFiles_DragEnter);
            this.lbFiles.DragOver += new System.Windows.Forms.DragEventHandler(this.lbFiles_DragOver);
            this.lbFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbFiles_KeyDown);
            this.lbFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbFiles_MouseDown);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(6, 90);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(272, 17);
            this.lblInfo.TabIndex = 46;
            this.lblInfo.Text = "Drop your pdf files below to combine them";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 408);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblDetails);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDF Combiner";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.menuStrip.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.ContextMenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuItemDelete;
        private System.Windows.Forms.ToolStripMenuItem menuItemOrderByPathAscending;
        private System.Windows.Forms.ToolStripMenuItem menuItemOrderByPathDescending;
        private System.Windows.Forms.ToolStripMenuItem menuItemOrderByNameAscending;
        private System.Windows.Forms.ToolStripMenuItem menuItemOrderByNameDescending;
        private System.Windows.Forms.Button btnCombinePdfSharp;
        private System.Windows.Forms.Button btnAddFolder;
        private System.Windows.Forms.Button btnAddFile;
        private System.Windows.Forms.Button btnClearList;
        private System.Windows.Forms.ProgressBar pbFiles;
        private System.Windows.Forms.Button btnCombineITextSharp;
        public System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.Label lblInfo;
    }
}

