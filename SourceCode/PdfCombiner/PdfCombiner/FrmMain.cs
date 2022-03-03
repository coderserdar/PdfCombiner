using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PdfCombiner
{
    public partial class FrmMain : Form
    {
        public XFont AppFont { get; set; }
        public const string AppTitle = "PDF Combiner";

        public FrmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This function is used to set some kind of parameters which can be used
        /// In PDF combination
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Shown(object sender, EventArgs e)
        {
            //var options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
            //AppFont = new XFont("Times New Roman", 12, XFontStyle.Regular, options);
            //lblDetails.Text = "Font Type: " + AppFont.FontFamily.Name + ", Font Size: " + AppFont.Size + ", Font Style: " + AppFont.Style;
        }

        /// <summary>
        /// It is used to add single or multiple PDF files to combine
        /// When you choose file or files
        /// The listbox in the form will be filled with the name of the files 
        /// Which you choose to combine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddFile_Click(object sender, EventArgs e)
        {
            using (var dialogAddFile = new OpenFileDialog())
            {
                InitializeFileDialog(dialogAddFile);
                var result = dialogAddFile.ShowDialog();
                if (result == DialogResult.OK && dialogAddFile.FileNames != null)
                {
                    foreach (var file in dialogAddFile.FileNames)
                    {
                        if (!lbFiles.Items.Contains(file))
                            lbFiles.Items.Add(file);
                    }
                }
            }
        }

        /// <summary>
        /// This function is used to set options of file dialog like
        /// Filter, start path etc.
        /// </summary>
        /// <param name="dialogAddFile">File Dialog</param>
        private static void InitializeFileDialog(OpenFileDialog dialogAddFile)
        {
            dialogAddFile.Multiselect = true;
            dialogAddFile.Filter = "pdf files (*.pdf)|*.pdf";
            dialogAddFile.InitialDirectory = Application.StartupPath;
            dialogAddFile.Title = "Select PDF File/s";
            dialogAddFile.DefaultExt = "pdf";
        }

        /// <summary>
        /// This function is used to clear the file list in listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearList_Click(object sender, EventArgs e)
        {
            lbFiles.Items.Clear();
        }

        /// <summary>
        /// This function is used to add PDF files in a folder which you choose in
        /// Folder Dialog recursively
        /// When you use it the added files will be seen in listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            using (var dialogAddFolder = new FolderBrowserDialog())
            {
                var result = dialogAddFolder.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrEmpty(dialogAddFolder.SelectedPath))
                {
                    var fileNames = Directory.GetFiles(dialogAddFolder.SelectedPath, "*.pdf", SearchOption.AllDirectories);
                    foreach (var file in fileNames)
                    {
                        if (!lbFiles.Items.Contains(file))
                            lbFiles.Items.Add(file);
                    }
                }
            }
        }

        //private void btnSetFont_Click(object sender, EventArgs e)
        //{
        //    using (var dialogSetFont = new FontDialog())
        //    {
        //        var result = dialogSetFont.ShowDialog();
        //        if (result == DialogResult.OK && dialogSetFont.Font != null)
        //        {
        //            var fontDialog = dialogSetFont.Font;
        //            var options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
        //            AppFont = new XFont(fontDialog.FontFamily.Name, fontDialog.Size, (XFontStyle)fontDialog.Style, options);
        //            lblDetails.Text = "Font Type: " + AppFont.FontFamily.Name + ", Font Size: " + AppFont.Size + ", Font Style: " + AppFont.Style;
        //        }
        //    }
        //}

        /// <summary>
        /// This function is used to take list of PDF files in listbox,
        /// Combine them with a single file in location which you choose in folder dialog
        /// And you can see the progress in progress bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCombine_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbFiles.Items.Count < 2)
                {
                    MessageBox.Show("There must be at least 2 PDF files to combine", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    using (var dialogExport = new FolderBrowserDialog())
                    {
                        var result = dialogExport.ShowDialog();
                        if (result == DialogResult.OK && !string.IsNullOrEmpty(dialogExport.SelectedPath))
                        {
                            var outputFileName = dialogExport.SelectedPath + "/" + Guid.NewGuid().ToString() + ".pdf";
                            var outputFile = new PdfDocument();
                            outputFile.Options.CompressContentStreams = true;
                            outputFile.Options.EnableCcittCompressionForBilevelImages = true;
                            outputFile.Options.FlateEncodeMode = PdfFlateEncodeMode.BestCompression;

                            for (int i = 0; i < lbFiles.Items.Count; i++)
                            {
                                var inputDocument = PdfReader.Open(lbFiles.Items[i].ToString(), PdfDocumentOpenMode.Import);
                                var count = inputDocument.PageCount;
                                for (int idx = 0; idx < count; idx++)
                                {
                                    var page = inputDocument.Pages[idx];
                                    outputFile.AddPage(page);
                                }
                                inputDocument.Close();

                                pbFiles.Value = ((i + 1) * 100 / lbFiles.Items.Count);
                                if (pbFiles.Value > pbFiles.Maximum)
                                    pbFiles.Value = pbFiles.Maximum;
                            }
                            outputFile.Save(outputFileName);
                            outputFile.Close();

                            MessageBox.Show(lbFiles.Items.Count + " PDF files successfully combined in " + outputFileName, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            pbFiles.Value = pbFiles.Minimum;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is an error while combining PDF files in list. Details: " + ex.GetAllMessages(), AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                pbFiles.Value = pbFiles.Minimum;
            }
        }

        private void lbFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == Keys.Delete.GetHashCode())
            {
                var allItems = new List<string>();
                var removedItems = new List<string>();
                var fileCount = lbFiles.SelectedItems.Count;
                for (int i = 0; i < lbFiles.SelectedItems.Count; i++)
                {
                    removedItems.Add(lbFiles.SelectedItems[i].ToString());
                }
                foreach (var item in lbFiles.Items)
                {
                    if (!removedItems.Any(j => j == item.ToString()))
                        allItems.Add(item.ToString());
                }
                lbFiles.Items.Clear();
                foreach (var item in allItems)
                {
                    lbFiles.Items.Add(item);
                }
                MessageBox.Show(fileCount + " files are deleted from the list.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
