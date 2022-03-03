using iTextSharp.text;
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
        public iTextSharp.text.Font AppFont { get; set; }
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
        /// This function is used to terminate application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            MessageBox.Show("Thanks for using this app", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
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
                var addedFileCount = 0;
                if (result == DialogResult.OK && dialogAddFile.FileNames != null)
                {
                    foreach (var file in dialogAddFile.FileNames)
                    {
                        if (!lbFiles.Items.Contains(file))
                        {
                            lbFiles.Items.Add(file);
                            addedFileCount++;
                        }
                    }
                    MessageBox.Show(addedFileCount + " file/s successfully added to the list", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            var fileCount = lbFiles.Items.Count;
            lbFiles.Items.Clear();
            MessageBox.Show(fileCount + " file/s successfully deleted from the list", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    var addedFileCount = 0;
                    foreach (var file in fileNames)
                    {
                        if (!lbFiles.Items.Contains(file))
                        {
                            lbFiles.Items.Add(file);
                            addedFileCount++;
                        }
                    }
                    MessageBox.Show(addedFileCount + " file/s successfully added to the list", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        #region Comment Block
        ///// <summary>
        ///// This function is used to set font for
        ///// Combining PDF files With iTextSharp NuGet Package
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnSetFont_Click(object sender, EventArgs e)
        //{
        //    using (var dialogSetFont = new FontDialog())
        //    {
        //        var result = dialogSetFont.ShowDialog();
        //        if (result == DialogResult.OK && dialogSetFont.Font != null)
        //        {
        //            var fontDialog = dialogSetFont.Font;
        //            AppFont = FontFactory.GetFont(fontDialog.FontFamily.Name, fontDialog.Size, BaseColor.BLACK);
        //            AppFont.SetStyle(fontDialog.Style.ToString());
        //            lblDetails.Text = "Font Type: " + AppFont.Family.ToString() + ", Font Size: " + AppFont.Size + ", Font Style: " + AppFont.Style;
        //        }
        //    }
        //}
        #endregion

        /// <summary>
        /// This function is used to delete files in listbox which are chosen
        /// If you press the Delete button in your keyboard
        /// The selected files will be deleted from the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                MessageBox.Show(fileCount + " file/s are deleted from the list.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// This function is used to take list of PDF files in listbox,
        /// Combine them with a single file in location which you choose in folder dialog
        /// And you can see the progress in progress bar
        /// It uses PdfSharp Nuget Package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCombinePdfSharp_Click(object sender, EventArgs e)
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
                            var fileCount = lbFiles.Items.Count;
                            var combinedFiles = 0;

                            for (int i = 0; i < lbFiles.Items.Count; i++)
                            {
                                try
                                {
                                    var inputDocument = PdfReader.Open(lbFiles.Items[i].ToString(), PdfDocumentOpenMode.Import);
                                    var count = inputDocument.PageCount;
                                    for (int idx = 0; idx < count; idx++)
                                    {
                                        var page = inputDocument.Pages[idx];
                                        outputFile.AddPage(page);
                                    }
                                    inputDocument.Close();
                                    combinedFiles++;
                                }
                                catch (Exception)
                                {
                                    fileCount--;
                                }

                                pbFiles.Value = (combinedFiles * 100 / fileCount);
                                if (pbFiles.Value > pbFiles.Maximum)
                                    pbFiles.Value = pbFiles.Maximum;
                            }
                            outputFile.Save(outputFileName);
                            outputFile.Close();

                            MessageBox.Show(fileCount + " PDF files successfully combined in " + outputFileName, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        /// <summary>
        /// This function is used to take list of PDF files in listbox,
        /// Combine them with a single file in location which you choose in folder dialog
        /// And you can see the progress in progress bar
        /// It uses iTextSharp Nuget Package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCombineITextSharp_Click(object sender, EventArgs e)
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
                            var fileCount = lbFiles.Items.Count;
                            var combinedFiles = 0;

                            var outputFile = new Document();
                            using (FileStream outputFileStream = new FileStream(outputFileName, FileMode.Create))
                            {
                                var pdfWriter = new iTextSharp.text.pdf.PdfCopy(outputFile, outputFileStream);
                                if (pdfWriter == null)
                                {
                                    return;
                                }
                                pdfWriter.SetFullCompression();
                                outputFile.Open();
                                for (int i = 0; i < lbFiles.Items.Count; i++)
                                {
                                    try
                                    {
                                        var pdfReader = new iTextSharp.text.pdf.PdfReader(lbFiles.Items[i].ToString());
                                        pdfReader.ConsolidateNamedDestinations();
                                        for (int j = 1; j <= pdfReader.NumberOfPages; j++)
                                        {
                                            var page = pdfWriter.GetImportedPage(pdfReader, j);
                                            pdfWriter.AddPage(page);
                                        }
                                        pdfReader.Close();
                                        combinedFiles++;
                                    }
                                    catch (Exception)
                                    {
                                        fileCount--;
                                    }

                                    pbFiles.Value = (combinedFiles * 100 / fileCount);
                                    if (pbFiles.Value > pbFiles.Maximum)
                                        pbFiles.Value = pbFiles.Maximum;
                                }
                                pdfWriter.Close();
                                outputFile.Close();
                            }

                            MessageBox.Show(fileCount + " PDF files successfully combined in " + outputFileName, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

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
    }
}
