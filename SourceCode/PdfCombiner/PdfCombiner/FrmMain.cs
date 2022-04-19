using iTextSharp.text;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PdfCombiner
{
    public partial class FrmMain : Form
    {
        public const string AppTitle = "PDF Combiner";

        public FrmMain()
        {
            InitializeComponent();
        }

        #region Form Init And Finalize Methods

        /// <summary>
        /// This function is used to terminate application
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            MessageBox.Show("Thanks for using this app", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

        /// <summary>
        /// This function is used to assign menu to listbox 
        /// When form is shown
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void FrmMain_Shown(object sender, EventArgs e)
        {
            lbFiles.ContextMenuStrip = menuStrip;
        }

        #endregion

        #region Add Item Methods

        /// <summary>
        /// It is used to add single or multiple PDF files to combine
        /// When you choose file or files
        /// The listbox in the form will be filled with the name of the files 
        /// Which you choose to combine
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
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
        /// This function is used to add PDF files in a folder which you choose in
        /// Folder Dialog recursively
        /// When you use it the added files will be seen in listbox
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
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

        #endregion

        #region Combine PDF Methods

        /// <summary>
        /// This function is used to take list of PDF files in listbox,
        /// Combine them with a single file in location which you choose in folder dialog
        /// And you can see the progress in progress bar
        /// It uses PdfSharp Nuget Package
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void btnCombinePdfSharp_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbFiles.Items.Count < 2)
                    MessageBox.Show("There must be at least 2 PDF files to combine", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    using (var dialogExport = new FolderBrowserDialog())
                    {
                        var result = dialogExport.ShowDialog();
                        if (result == DialogResult.OK && !string.IsNullOrEmpty(dialogExport.SelectedPath))
                        {
                            var outputFileName = dialogExport.SelectedPath + "/" + Guid.NewGuid().ToString() + ".pdf";
                            using (var outputFile = new PdfDocument())
                            {
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
                                    ActiveForm.Text = "%" + pbFiles.Value;
                                }
                                outputFile.Save(outputFileName);
                                outputFile.Close();

                                MessageBox.Show(fileCount + " PDF files successfully combined in " + outputFileName, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            pbFiles.Value = pbFiles.Minimum;
                            ActiveForm.Text = AppTitle;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is an error while combining PDF files in list. Details: " + ex.GetAllMessages(), AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                pbFiles.Value = pbFiles.Minimum;
                ActiveForm.Text = AppTitle;
            }
        }

        /// <summary>
        /// This function is used to take list of PDF files in listbox,
        /// Combine them with a single file in location which you choose in folder dialog
        /// And you can see the progress in progress bar
        /// It uses iTextSharp Nuget Package
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void btnCombineITextSharp_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbFiles.Items.Count < 2)
                    MessageBox.Show("There must be at least 2 PDF files to combine", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                using (var pdfWriter = new iTextSharp.text.pdf.PdfCopy(outputFile, outputFileStream))
                                {
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
                                        ActiveForm.Text = "%" + pbFiles.Value;
                                    }
                                    pdfWriter.Close();
                                }
                                outputFile.Close();
                            }

                            MessageBox.Show(fileCount + " PDF files successfully combined in " + outputFileName, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            pbFiles.Value = pbFiles.Minimum;
                            ActiveForm.Text = AppTitle;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is an error while combining PDF files in list. Details: " + ex.GetAllMessages(), AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                pbFiles.Value = pbFiles.Minimum;
                ActiveForm.Text = AppTitle;
            }
        }

        #endregion

        #region Delete Item Methods

        /// <summary>
        /// This function is used to delete files in listbox which are chosen
        /// If you press the Delete button in your keyboard
        /// The selected files will be deleted from the list
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void lbFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == Keys.Delete.GetHashCode())
                DeleteFilesFromListBox();
        }

        /// <summary>
        /// This is used to delete selected items in listbox
        /// When menu item Delete is clicked and opened dialog 
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void menuItemDelete_Click(object sender, EventArgs e)
        {
            if (lbFiles.Items.Count > 0)
            {
                if (lbFiles.SelectedItems.Count > 0)
                {
                    var deleteDialog = new DialogResult();
                    deleteDialog = MessageBox.Show("You are about to delete " + lbFiles.SelectedItems.Count + " files in listbox. Are you sure?", AppTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (deleteDialog == DialogResult.Yes)
                        DeleteFilesFromListBox();
                }
                else
                    MessageBox.Show("There is no selected files in list.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("There is no files in list.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// This method is used to Delete selected files from ListBox
        /// This is used different ways in this app
        /// So we created this as a method
        /// </summary>
        private void DeleteFilesFromListBox()
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

        /// <summary>
        /// This function is used to clear the file list in listbox
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void btnClearList_Click(object sender, EventArgs e)
        {
            var fileCount = lbFiles.Items.Count;
            lbFiles.Items.Clear();
            MessageBox.Show(fileCount + " file/s successfully deleted from the list", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Order Item Methods

        /// <summary>
        /// This method is used to order elements in listbox ascending
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void menuItemOrderAscending_Click(object sender, EventArgs e)
        {
            if (lbFiles.Items.Count > 0)
                lbFiles = SortItems(lbFiles, true);
            else
                MessageBox.Show("There is no files in list.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// This method is used to order elements in listbox descending by name
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void menuItemOrderDescending_Click(object sender, EventArgs e)
        {
            if (lbFiles.Items.Count > 0)
                lbFiles = SortItems(lbFiles, false);
            else
                MessageBox.Show("There is no files in list.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// This method is used to order items in listbox
        /// And return them with the given order as parametre
        /// </summary>
        /// <param name="listBox">ListBox Info</param>
        /// <param name="ascending">Order Type is Ascending Or Not</param>
        private static ListBox SortItems(ListBox listBox, bool ascending)
        {
            List<object> items;
            items = listBox.Items.OfType<object>().ToList();
            listBox.Items.Clear();
            if (ascending)
                listBox.Items.AddRange(items.OrderBy(i => i).ToArray());
            else
                listBox.Items.AddRange(items.OrderByDescending(i => i).ToArray());
            return listBox;
        }

        #endregion

        #region Drag Drop Item For Order Methods

        /// <summary>
        /// This is used to control mouse actions and if that is not right click
        /// Start to drag and drop item in listbox
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void lbFiles_MouseDown(object sender, MouseEventArgs e)
        {
            if (!e.Button.Equals(MouseButtons.Right))
            {
                if (lbFiles.SelectedItem == null) return;
                lbFiles.DoDragDrop(lbFiles.SelectedItem, DragDropEffects.Move);
            }
        }

        /// <summary>
        /// This is used to give effect while dragging and dropping an item in listbox
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void lbFiles_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// This is used to drag and drop an item in listbox
        /// Which is used to order books and combine them with that order
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>m>
        private void lbFiles_DragDrop(object sender, DragEventArgs e)
        {
            var point = lbFiles.PointToClient(new Point(e.X, e.Y));
            var index = this.lbFiles.IndexFromPoint(point);
            if (index < 0) index = this.lbFiles.Items.Count - 1;
            // type of string because file names stored in string in listbox
            var data = e.Data.GetData(typeof(string));
            this.lbFiles.Items.Remove(data);
            this.lbFiles.Items.Insert(index, data);
        }
        #endregion
    }
}
