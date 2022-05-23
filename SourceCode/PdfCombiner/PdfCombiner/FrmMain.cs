using iTextSharp.text;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace PdfCombiner
{
    public partial class FrmMain : Form
    {
        private static ResourceManager _resource;

        // Culture lists whose resources have been added to project
        private static readonly List<string> _languageList = new List<string>
            {"EN", "TR", "DE", "FR", "RU", "ES", "IT", "ZH", "AR", "NL", "PT", "IN", "ID", "JP", "BG"};

        public FrmMain()
        {
            InitializeComponent();
        }

        #region Form Init, Language Settings and Finalize Methods

        /// <summary>
        /// This function is used to terminate application
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            MessageBox.Show(_resource.GetString("ThanksMessage"), _resource.GetString("AppTitle"), MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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
            cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLanguage.Items.Clear();
            var cultureInfo = CultureInfo.InstalledUICulture;
            var firstLanguage = string.Empty;
            foreach (var item in _languageList)
            {
                if (cultureInfo.Name.Contains(item))
                    firstLanguage = item.ToLower(new CultureInfo("en-US"));
                cmbLanguage.Items.Add(item);
            }
            if (string.IsNullOrEmpty(firstLanguage))
                firstLanguage = "tr";
            
            _resource = new ResourceManager("PdfCombiner.Resources.AppResources-" + firstLanguage,
                Assembly.GetExecutingAssembly());
            cmbLanguage.SelectedIndex = cmbLanguage.FindStringExact(firstLanguage.ToUpper(new CultureInfo("en-US")));
            
            FormMembersNameInitialization();
        }

        /// <summary>
        /// This function is used to set the _resource file
        /// And with this, you can see form elements, info messages etc. with the selected
        /// _resource file contents. That is used to multi language support
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLanguage.SelectedIndex == -1) return;
            try
            {
                _resource = new ResourceManager(
                    "PdfCombiner.Resources.AppResources-" +
                    cmbLanguage.SelectedItem.ToString().ToLower(new CultureInfo("en-US")),
                    Assembly.GetExecutingAssembly());
                FormMembersNameInitialization();
            }
            catch (Exception)
            {
                _resource = new ResourceManager("PdfCombiner.Resources.AppResources-en",
                    Assembly.GetExecutingAssembly());
                FormMembersNameInitialization();
            }
        }

        /// <summary>
        /// This method is used to change text of form members by the
        /// Resource file which is selected
        /// </summary>
        private void FormMembersNameInitialization()
        {
            ActiveForm.Text = _resource.GetString("AppTitle") ?? string.Empty;
            btnAddFile.Text = _resource.GetString("AddFile") ?? string.Empty;
            btnAddFolder.Text = _resource.GetString("AddFolder") ?? string.Empty;
            btnClearList.Text = _resource.GetString("ClearFileList") ?? string.Empty;
            btnCombineITextSharp.Text = _resource.GetString("CombineFilesITextSharp") ?? string.Empty;
            btnCombinePdfSharp.Text = _resource.GetString("CombineFilesPdfSharp") ?? string.Empty;
            menuItemDelete.Text = _resource.GetString("Delete") ?? string.Empty;
            menuItemOrderByPathAscending.Text = _resource.GetString("OrderByPathAscending") ?? string.Empty;
            menuItemOrderByPathDescending.Text = _resource.GetString("OrderByPathDescending") ?? string.Empty;
            menuItemOrderByNameAscending.Text = _resource.GetString("OrderByNameAscending") ?? string.Empty;
            menuItemOrderByNameDescending.Text = _resource.GetString("OrderByNameDescending") ?? string.Empty;
            cmbLanguage.Text = _resource.GetString("SelectLanguage") ?? string.Empty;
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
                if (result != DialogResult.OK || dialogAddFile.FileNames == null) return;
                foreach (var file in dialogAddFile.FileNames)
                {
                    if (lbFiles.Items.Contains(file)) continue;
                    lbFiles.Items.Add(file);
                    addedFileCount++;

                    pbFiles.Value = addedFileCount * 100 / dialogAddFile.FileNames.Length;
                    if (pbFiles.Value > pbFiles.Maximum)
                        pbFiles.Value = pbFiles.Maximum;
                    ActiveForm.Text = "%" + pbFiles.Value;
                }

                MessageBox.Show(addedFileCount + _resource.GetString("FileAddMessage"),
                    _resource.GetString("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                pbFiles.Value = pbFiles.Minimum;
                ActiveForm.Text = _resource.GetString("AppTitle");
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
            dialogAddFile.Filter = _resource.GetString("PdfFiles") + " (*.pdf)|*.pdf";
            dialogAddFile.InitialDirectory = Application.StartupPath;
            dialogAddFile.Title = _resource.GetString("SelectPdfFile");
            dialogAddFile.DefaultExt = "PDF";
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
                if (result != DialogResult.OK || string.IsNullOrEmpty(dialogAddFolder.SelectedPath)) return;
                var fileNames = Directory.GetFiles(dialogAddFolder.SelectedPath, "*.pdf",
                    SearchOption.AllDirectories);
                var addedFileCount = 0;
                foreach (var file in fileNames)
                {
                    if (lbFiles.Items.Contains(file)) continue;
                    lbFiles.Items.Add(file);
                    addedFileCount++;

                    pbFiles.Value = addedFileCount * 100 / fileNames.Length;
                    if (pbFiles.Value > pbFiles.Maximum)
                        pbFiles.Value = pbFiles.Maximum;
                    ActiveForm.Text = "%" + pbFiles.Value;
                }

                MessageBox.Show(addedFileCount + _resource.GetString("FileAddMessage"),
                    _resource.GetString("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                pbFiles.Value = pbFiles.Minimum;
                ActiveForm.Text = _resource.GetString("AppTitle");
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
                    MessageBox.Show(_resource.GetString("CombineWarning"), _resource.GetString("AppTitle"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    using (var dialogExport = new FolderBrowserDialog())
                    {
                        var result = dialogExport.ShowDialog();
                        if (result != DialogResult.OK || string.IsNullOrEmpty(dialogExport.SelectedPath)) return;
                        var outputFileName = dialogExport.SelectedPath + "/" + Guid.NewGuid() + ".pdf";
                        using (var outputFile = new PdfDocument())
                        {
                            outputFile.Options.CompressContentStreams = true;
                            outputFile.Options.EnableCcittCompressionForBilevelImages = true;
                            outputFile.Options.FlateEncodeMode = PdfFlateEncodeMode.BestCompression;
                            var fileCount = lbFiles.Items.Count;
                            var combinedFiles = 0;

                            foreach (var t in lbFiles.Items)
                            {
                                try
                                {
                                    var inputDocument = PdfReader.Open(t.ToString(),
                                        PdfDocumentOpenMode.Import);
                                    var count = inputDocument.PageCount;
                                    for (var idx = 0; idx < count; idx++)
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

                                pbFiles.Value = combinedFiles * 100 / fileCount;
                                if (pbFiles.Value > pbFiles.Maximum)
                                    pbFiles.Value = pbFiles.Maximum;
                                ActiveForm.Text = "%" + pbFiles.Value;
                            }

                            outputFile.Save(outputFileName);
                            outputFile.Close();

                            MessageBox.Show(fileCount + _resource.GetString("CombineFileMessage") + outputFileName,
                                _resource.GetString("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        pbFiles.Value = pbFiles.Minimum;
                        ActiveForm.Text = _resource.GetString("AppTitle");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_resource.GetString("CombineErrorMessage") + ex.GetAllMessages(),
                    _resource.GetString("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                pbFiles.Value = pbFiles.Minimum;
                ActiveForm.Text = _resource.GetString("AppTitle");
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
                    MessageBox.Show(_resource.GetString("CombineWarning"), _resource.GetString("AppTitle"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    using (var dialogExport = new FolderBrowserDialog())
                    {
                        var result = dialogExport.ShowDialog();
                        if (result != DialogResult.OK || string.IsNullOrEmpty(dialogExport.SelectedPath)) return;
                        var outputFileName = dialogExport.SelectedPath + "/" + Guid.NewGuid() + ".pdf";
                        var fileCount = lbFiles.Items.Count;
                        var combinedFiles = 0;

                        var outputFile = new Document();
                        using (var outputFileStream = new FileStream(outputFileName, FileMode.Create))
                        {
                            using (var pdfWriter = new iTextSharp.text.pdf.PdfCopy(outputFile, outputFileStream))
                            {
                                if (pdfWriter == null)
                                {
                                    return;
                                }

                                pdfWriter.SetFullCompression();
                                outputFile.Open();
                                foreach (var t in lbFiles.Items)
                                {
                                    try
                                    {
                                        var pdfReader =
                                            new iTextSharp.text.pdf.PdfReader(t.ToString());
                                        pdfReader.ConsolidateNamedDestinations();
                                        for (var j = 1; j <= pdfReader.NumberOfPages; j++)
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

                                    pbFiles.Value = combinedFiles * 100 / fileCount;
                                    if (pbFiles.Value > pbFiles.Maximum)
                                        pbFiles.Value = pbFiles.Maximum;
                                    ActiveForm.Text = "%" + pbFiles.Value;
                                }

                                pdfWriter.Close();
                            }

                            outputFile.Close();
                        }

                        MessageBox.Show(fileCount + _resource.GetString("CombineFileMessage") + outputFileName,
                            _resource.GetString("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                        pbFiles.Value = pbFiles.Minimum;
                        ActiveForm.Text = _resource.GetString("AppTitle");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_resource.GetString("CombineErrorMessage") + ex.GetAllMessages(),
                    _resource.GetString("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                pbFiles.Value = pbFiles.Minimum;
                ActiveForm.Text = _resource.GetString("AppTitle");
            }
        }

        #endregion

        #region Delete Item Methods

        /// <summary>
        /// This function is used to operate some process in files in listbox which are chosen
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void lbFiles_KeyDown(object sender, KeyEventArgs e)
        {
            // this is used to delete selected files in listbox when you press Delete key
            if (e.KeyValue == Keys.Delete.GetHashCode())
                DeleteFilesFromListBox();
            // This is used to select all items in listbox when you press Ctrl + A key combination
            if (!e.Control || e.KeyCode != Keys.A) return;
            for (var i = 0; i < lbFiles.Items.Count; i++)
            {
                lbFiles.SetSelected(i, true);
            }
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
                    var deleteDialog = MessageBox.Show(
                        _resource.GetString("DeleteWarning1") + lbFiles.SelectedItems.Count +
                        _resource.GetString("DeleteWarning2"), _resource.GetString("AppTitle"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (deleteDialog == DialogResult.Yes)
                        DeleteFilesFromListBox();
                }
                else
                    MessageBox.Show(_resource.GetString("NoSelectedFile"), _resource.GetString("AppTitle"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show(_resource.GetString("NoFile"), _resource.GetString("AppTitle"), MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
            foreach (var t in lbFiles.SelectedItems)
            {
                removedItems.Add(t.ToString());
            }

            foreach (var item in lbFiles.Items)
            {
                if (removedItems.All(j => j != item.ToString()))
                    allItems.Add(item.ToString());
            }

            lbFiles.Items.Clear();
            foreach (var item in allItems)
            {
                lbFiles.Items.Add(item);
            }

            MessageBox.Show(fileCount + _resource.GetString("DeleteFileMessage"), _resource.GetString("AppTitle"),
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            MessageBox.Show(fileCount + _resource.GetString("DeleteFileMessage"), _resource.GetString("AppTitle"),
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Order Item Methods

        /// <summary>
        /// This method is used to order elements in listbox ascending by full path
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void menuItemOrderByPathAscending_Click(object sender, EventArgs e)
        {
            if (lbFiles.Items.Count > 0)
                lbFiles = SortItemsByPath(lbFiles, true);
            else
                MessageBox.Show(_resource.GetString("NoFile"), _resource.GetString("AppTitle"), MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        /// <summary>
        /// This method is used to order elements in listbox descending by full path
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void menuItemOrderByPathDescending_Click(object sender, EventArgs e)
        {
            if (lbFiles.Items.Count > 0)
                lbFiles = SortItemsByPath(lbFiles, false);
            else
                MessageBox.Show(_resource.GetString("NoFile"), _resource.GetString("AppTitle"), MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        /// <summary>
        /// This method is used to order elements in listbox ascending by file name
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void menuItemOrderByNameAscending_Click(object sender, EventArgs e)
        {
            if (lbFiles.Items.Count > 0)
                lbFiles = SortItemsByName(lbFiles, true);
            else
                MessageBox.Show(_resource.GetString("NoFile"), _resource.GetString("AppTitle"), MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        /// <summary>
        /// This method is used to order elements in listbox descending by file name
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void menuItemOrderByNameDescending_Click(object sender, EventArgs e)
        {
            if (lbFiles.Items.Count > 0)
                lbFiles = SortItemsByName(lbFiles, false);
            else
                MessageBox.Show(_resource.GetString("NoFile"), _resource.GetString("AppTitle"), MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        /// <summary>
        /// This method is used to order items by full path in listbox
        /// And return them with the given order as parametre
        /// </summary>
        /// <param name="listBox">ListBox Info</param>
        /// <param name="ascending">Order Type is Ascending Or Not</param>
        private static ListBox SortItemsByPath(ListBox listBox, bool ascending)
        {
            var items = listBox.Items.OfType<object>().ToList();
            listBox.Items.Clear();
            listBox.Items.AddRange(ascending
                ? items.OrderBy(i => i).ToArray()
                : items.OrderByDescending(i => i).ToArray());
            return listBox;
        }

        /// <summary>
        /// This method is used to order items by file name in listbox
        /// And return them with the given order as parametre
        /// </summary>
        /// <param name="listBox">ListBox Info</param>
        /// <param name="ascending">Order Type is Ascending Or Not</param>
        private static ListBox SortItemsByName(ListBox listBox, bool ascending)
        {
            var fileList = new List<FileInfo>();
            var items = listBox.Items.OfType<object>().ToList();
            const char c = '\u005c';
            foreach (var item in items)
            {
                var fullPath = item.ToString();
                var list = fullPath.Split(c).ToList();
                if (list.Count <= 0) continue;
                var fileInfo = new FileInfo
                {
                    FilePath = fullPath,
                    FileName = list[list.Count - 1]
                };
                fileList.Add(fileInfo);
            }

            fileList = ascending
                ? fileList.OrderBy(j => j.FileName).ThenBy(j => j.FilePath).ToList()
                : fileList.OrderByDescending(j => j.FileName).ThenByDescending(j => j.FilePath).ToList();

            listBox.Items.Clear();
            foreach (var item in fileList)
                listBox.Items.Add(item.FilePath);

            return listBox;
        }

        #endregion

        #region Item Drag and Drop Methods

        /// <summary>
        /// This is used to control mouse actions and if that is not right click
        /// Start to drag and drop item in listbox
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void lbFiles_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right)) return;
            if (lbFiles.SelectedItem == null) return;
            lbFiles.DoDragDrop(lbFiles.SelectedItem, DragDropEffects.Move);
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
            var index = lbFiles.IndexFromPoint(point);
            if (index < 0) index = lbFiles.Items.Count - 1;
            // type of string because file names stored in string in listbox
            var data = e.Data.GetData(typeof(string));
            lbFiles.Items.Remove(data);
            lbFiles.Items.Insert(index, data);
        }

        #endregion
    }
}