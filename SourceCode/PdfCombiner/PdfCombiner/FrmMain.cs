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
using iTextSharp.text.pdf;
using PdfDocument = PdfSharp.Pdf.PdfDocument;
using PdfReader = PdfSharp.Pdf.IO.PdfReader;
using static PdfCombiner.Enums;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PdfCombiner
{
    public partial class FrmMain : Form
    {
        private static ResourceManager _resource;

        // Culture lists whose resources have been added to project
        private static readonly List<string> LanguageList = new List<string>
            {"EN", "IN", "TR", "DE", "FR", "RU", "ES", "IT", "ZH", "AR", "NL", "PT", "ID", "JP", "BG"};

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
            MessageBox.Show(_resource.GetString("ThanksMessage"), _resource.GetString("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            // To allow dropping an item to the listbox
            lbFiles.AllowDrop = true;

            FillLanguageComboboxAndSetFirstLanguage(out var firstLanguage);

            _resource = new ResourceManager("PdfCombiner.Resources.AppResources-" + firstLanguage,
                Assembly.GetExecutingAssembly());
            cmbLanguage.SelectedIndex = cmbLanguage.FindStringExact(firstLanguage.ToUpper(new CultureInfo("en-US")));

            FormMembersNameInitialization();

        }

        /// <summary>
        /// This method is used to Fill the Language combobox
        /// And set initial value with
        /// The language computer used in
        /// </summary>
        /// <param name="firstLanguage">First Language Info</param>
        private void FillLanguageComboboxAndSetFirstLanguage(out string firstLanguage)
        {
            cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLanguage.Items.Clear();
            var cultureInfo = CultureInfo.InstalledUICulture;
            firstLanguage = string.Empty;
            foreach (var item in LanguageList)
            {
                if (cultureInfo.Name.Contains(item))
                    firstLanguage = item.ToLower(new CultureInfo("en-US"));
                cmbLanguage.Items.Add(item);
            }

            if (string.IsNullOrEmpty(firstLanguage))
                firstLanguage = "en";
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
            SetProgramLanguage();
        }

        /// <summary>
        /// This method is used to Set Program Language
        /// By the selection from Language Combobox
        /// Finally, with the selected language
        /// Form elements renamed with the selected language
        /// </summary>
        private void SetProgramLanguage()
        {
            try
            {
                _resource = new ResourceManager(
                    "PdfCombiner.Resources.AppResources-" +
                    cmbLanguage.SelectedItem.ToString().ToLower(new CultureInfo("en-US")),
                    Assembly.GetExecutingAssembly());
            }
            catch (Exception)
            {
                _resource = new ResourceManager("PdfCombiner.Resources.AppResources-en",
                    Assembly.GetExecutingAssembly());
            }
            finally
            {
                FormMembersNameInitialization();
            }
        }

        /// <summary>
        /// This method is used to change text of form members by the
        /// Resource file which is selected
        /// </summary>
        private void FormMembersNameInitialization()
        {
            if (_resource == null) return;
            if (ActiveForm != null) ActiveForm.Text = _resource.GetString("AppTitle") ?? string.Empty;
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

                AddPdfFilesToList(dialogAddFile.FileNames, ref addedFileCount);
                GenerateAddFileMessage(addedFileCount);
            }
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

                AddPdfFilesToList(fileNames, ref addedFileCount);
                GenerateAddFileMessage(addedFileCount);
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
            dialogAddFile.Filter = _resource.GetString("PdfFiles") + @" (*.pdf)|*.pdf";
            dialogAddFile.InitialDirectory = Application.StartupPath;
            dialogAddFile.Title = _resource.GetString("SelectPdfFile");
            dialogAddFile.DefaultExt = "PDF";
        }

        /// <summary>
        /// This method is used both adding files or folders to list
        /// </summary>
        /// <param name="fileNames">List Of File Names</param>
        /// <param name="addedFileCount">Added File Count For Progress Bar</param>
        private void AddPdfFilesToList(ICollection<string> fileNames, ref int addedFileCount)
        {
            foreach (var file in fileNames)
            {
                if (lbFiles.Items.Contains(file)) continue;
                lbFiles.Items.Add(file);
                addedFileCount++;

                pbFiles.Value = addedFileCount * 100 / fileNames.Count;
                if (pbFiles.Value > pbFiles.Maximum)
                    pbFiles.Value = pbFiles.Maximum;
                if (ActiveForm != null) ActiveForm.Text = @"%" + pbFiles.Value;
            }
        }

        /// <summary>
        /// This method is used to show info message after adding files or folders
        /// </summary>
        /// <param name="addedFileCount">Added File Count</param>
        private void GenerateAddFileMessage(int addedFileCount)
        {
            MessageBox.Show(addedFileCount + _resource.GetString("FileAddMessage"),
                _resource.GetString("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);

            pbFiles.Value = pbFiles.Minimum;
            if (ActiveForm != null) ActiveForm.Text = _resource.GetString("AppTitle");
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
                    GenerateCombineWarningMessage();
                else
                {
                    //using (var dialogExport = new FolderBrowserDialog())
                    using (var saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.InitialDirectory = @"C:\";
                        saveFileDialog.Title = "Save Combined Files";
                        saveFileDialog.CheckPathExists = true;
                        saveFileDialog.CheckFileExists = false;
                        saveFileDialog.DefaultExt = "pdf";
                        saveFileDialog.Filter = "PDF document (*.pdf)|*.pdf";
                        saveFileDialog.RestoreDirectory = true;
                        var result = saveFileDialog.ShowDialog();
                        if (result != DialogResult.OK || string.IsNullOrEmpty(saveFileDialog.FileName)) return;

                        SetInitialValuesForCombiningFiles(saveFileDialog, out var outputFileName, out var fileCount, out var combinedFiles);

                        //var result = dialogExport.ShowDialog();
                        //if (result != DialogResult.OK || string.IsNullOrEmpty(dialogExport.SelectedPath)) return;

                        //SetInitialValuesForCombiningFiles(dialogExport, out var outputFileName, out var fileCount, out var combinedFiles);

                        using (var outputFile = new PdfDocument())
                        {
                            SetOutputFilePropertiesPdfSharp(outputFile);

                            foreach (var t in lbFiles.Items)
                            {
                                try
                                {
                                    AddPdfContentToTargetPdfFilePdfSharp(t, outputFile, ref combinedFiles);
                                }
                                catch (Exception)
                                {
                                    fileCount--;
                                }

                                SetCombinationRatio(combinedFiles, fileCount);
                            }

                            SaveOutputPdfFile(outputFile, outputFileName);
                        }

                        GenerateCombineFileMessage(fileCount, outputFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                GenerateExceptionMessage(ex);
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
                    GenerateCombineWarningMessage();
                else
                {
                    using (var saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.InitialDirectory = @"C:\";
                        saveFileDialog.Title = "Save Combined Files";
                        saveFileDialog.CheckPathExists = true;
                        saveFileDialog.CheckFileExists = false;
                        saveFileDialog.DefaultExt = "pdf";
                        saveFileDialog.Filter = "PDF document (*.pdf)|*.pdf";
                        saveFileDialog.RestoreDirectory = true;
                        var result = saveFileDialog.ShowDialog();
                        if (result != DialogResult.OK || string.IsNullOrEmpty(saveFileDialog.FileName)) return;

                        SetInitialValuesForCombiningFiles(saveFileDialog, out var outputFileName, out var fileCount, out var combinedFiles);

                        var outputFile = new Document();
                        using (var outputFileStream = new FileStream(outputFileName, FileMode.Create))
                        {
                            using (var pdfWriter = new PdfCopy(outputFile, outputFileStream))
                            {
                                pdfWriter.SetFullCompression();
                                outputFile.Open();
                                foreach (var t in lbFiles.Items)
                                {
                                    try
                                    {
                                        AddPdfContentToTargetPdfFileITextSharp(t, pdfWriter, ref combinedFiles);
                                    }
                                    catch (Exception)
                                    {
                                        fileCount--;
                                    }

                                    SetCombinationRatio(combinedFiles, fileCount);
                                }

                                pdfWriter.Close();
                            }

                            outputFile.Close();
                        }

                        GenerateCombineFileMessage(fileCount, outputFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                GenerateExceptionMessage(ex);
            }
        }

        /// <summary>
        /// This method is used to
        /// Save Combined PDF File
        /// </summary>
        /// <param name="outputFile">Combined PDF File</param>
        /// <param name="outputFileName">Combined PDF File Path</param>
        private static void SaveOutputPdfFile(PdfDocument outputFile, string outputFileName)
        {
            outputFile.Save(outputFileName);
            outputFile.Close();
        }

        /// <summary>
        /// This method is used to take source pdf file content
        /// And add to the target combined pdf file
        /// </summary>
        /// <param name="t">Source PDF File Path</param>
        /// <param name="outputFile">Combined PDF File</param>
        /// <param name="combinedFiles">Combined File Count</param>
        private static void AddPdfContentToTargetPdfFilePdfSharp(object t, PdfDocument outputFile, ref int combinedFiles)
        {
            var inputDocument = PdfReader.Open(t.ToString(),
                PdfDocumentOpenMode.Import);
            var count = inputDocument.PageCount;
            for (var index = 0; index < count; index++)
            {
                var page = inputDocument.Pages[index];
                outputFile.AddPage(page);
            }

            inputDocument.Close();
            combinedFiles++;
        }

        /// <summary>
        /// This method is used to set combined PDF file
        /// Properties like Compression etc.
        /// </summary>
        /// <param name="outputFile">Combined PDF File</param>
        private static void SetOutputFilePropertiesPdfSharp(PdfDocument outputFile)
        {
            outputFile.Options.CompressContentStreams = true;
            outputFile.Options.EnableCcittCompressionForBilevelImages = true;
            outputFile.Options.FlateEncodeMode = PdfFlateEncodeMode.BestCompression;
        }

        /// <summary>
        /// This method is used to take source pdf file content
        /// And add to the target combined pdf file
        /// </summary>
        /// <param name="t">Source PDF File Path</param>
        /// <param name="pdfWriter">PDF Writer</param>
        /// <param name="combinedFiles">Combined File Count</param>
        private static void AddPdfContentToTargetPdfFileITextSharp(object t, PdfCopy pdfWriter, ref int combinedFiles)
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

        /// <summary>
        /// This method is used
        /// When user just wnat to combine 1 pdf file
        /// </summary>
        private static void GenerateCombineWarningMessage()
        {
            MessageBox.Show(_resource.GetString("CombineWarning"), _resource.GetString("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// This method is used to
        /// Show error message when exception occurs and set form title initial value
        /// </summary>
        /// <param name="ex">Exception information</param>
        private void GenerateExceptionMessage(Exception ex)
        {
            MessageBox.Show(_resource.GetString("CombineErrorMessage") + ex.GetAllMessages(), _resource.GetString("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            pbFiles.Value = pbFiles.Minimum;
            if (ActiveForm != null) ActiveForm.Text = _resource.GetString("AppTitle");
        }

        /// <summary>
        /// This method is used to set initial values of some properties
        /// Which are used in combining PDF files
        /// In both PdfSharp and ITextSharp
        /// </summary>
        /// <param name="dialogExport">Export Dialog Info</param>
        /// <param name="outputFileName">Output File Name</param>
        /// <param name="fileCount">Total File Count</param>
        /// <param name="combinedFiles">Combined Files Count</param>
        private void SetInitialValuesForCombiningFiles(FolderBrowserDialog dialogExport, out string outputFileName,
            out int fileCount, out int combinedFiles)
        {
            outputFileName = dialogExport.SelectedPath + "/" + Guid.NewGuid() + ".pdf";
            fileCount = lbFiles.Items.Count;
            combinedFiles = 0;
        }

        /// <summary>
        /// This method is used to set initial values of some properties
        /// Which are used in combining PDF files In both PdfSharp and ITextSharp
        /// </summary>
        /// <param name="dialogExport">Export Dialog Info</param>
        /// <param name="outputFileName">Output File Name</param>
        /// <param name="fileCount">Total File Count</param>
        /// <param name="combinedFiles">Combined Files Count</param>
        private void SetInitialValuesForCombiningFiles(SaveFileDialog dialogExport, out string outputFileName, out int fileCount, out int combinedFiles)
        {
            outputFileName = dialogExport.FileName;
            fileCount = lbFiles.Items.Count;
            combinedFiles = 0;
        }

        /// <summary>
        /// This method is used to set combination degree while combining PDF files
        /// And show this ratio on Form Caption
        /// </summary>
        /// <param name="combinedFiles">Combined File Count</param>
        /// <param name="fileCount">Total File Count</param>
        private void SetCombinationRatio(int combinedFiles, int fileCount)
        {
            pbFiles.Value = combinedFiles * 100 / fileCount;
            if (pbFiles.Value > pbFiles.Maximum)
                pbFiles.Value = pbFiles.Maximum;
            if (ActiveForm != null) ActiveForm.Text = @"%" + pbFiles.Value;
        }

        /// <summary>
        /// This method is used to set successfull combining file message
        /// And set value of Form Caption etc.
        /// </summary>
        /// <param name="fileCount">Total File Count</param>
        /// <param name="outputFileName">Output File Name</param>
        private void GenerateCombineFileMessage(int fileCount, string outputFileName)
        {
            MessageBox.Show(fileCount + _resource.GetString("CombineFileMessage") + outputFileName, _resource.GetString("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            pbFiles.Value = pbFiles.Minimum;
            if (ActiveForm != null) ActiveForm.Text = _resource.GetString("AppTitle");
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
                    ShowDeleteDialog(out var deleteDialog);
                    if (deleteDialog == DialogResult.Yes)
                        DeleteFilesFromListBox();
                }
                else
                    GenerateNoSelectedFileInListBoxMessage();
            }
            else
                GenerateNoFileInListBoxMessage();
        }

        /// <summary>
        /// This method is used in
        /// When there is no selected file in listbox
        /// </summary>
        private static void GenerateNoSelectedFileInListBoxMessage()
        {
            MessageBox.Show(_resource.GetString("NoSelectedFile"), _resource.GetString("AppTitle"),
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// This method is used to
        /// Show delete file dialog
        /// </summary>
        /// <param name="deleteDialog">Delete File Dialog</param>
        private void ShowDeleteDialog(out DialogResult deleteDialog)
        {
            deleteDialog = MessageBox.Show(_resource.GetString("DeleteWarning1") + lbFiles.SelectedItems.Count + _resource.GetString("DeleteWarning2"), _resource.GetString("AppTitle"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// This method is used to Delete selected files from ListBox
        /// This is used different ways in this app
        /// So we created this as a method
        /// </summary>
        private void DeleteFilesFromListBox()
        {
            var fileCount = lbFiles.SelectedItems.Count;
            var removedItems = (from object t in lbFiles.SelectedItems select t.ToString()).ToList();

            var allItems = (from object item in lbFiles.Items where removedItems.All(j => j != item.ToString()) select item.ToString()).ToList();

            lbFiles.Items.Clear();
            foreach (var item in allItems)
            {
                lbFiles.Items.Add(item);
            }

            GenerateDeleteItemMessage(fileCount);
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
            GenerateDeleteItemMessage(fileCount);
        }

        /// <summary>
        /// This method is used after deleting items from listbox
        /// </summary>
        /// <param name="fileCount">Deleted File Count Information</param>
        private static void GenerateDeleteItemMessage(int fileCount)
        {
            MessageBox.Show(fileCount + _resource.GetString("DeleteFileMessage"), _resource.GetString("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                lbFiles = SortItems(lbFiles, SortType.ByPath, OrderType.Ascending);
            // lbFiles = SortItemsByPath(lbFiles, OrderType.Ascending);
            else
                GenerateNoFileInListBoxMessage();
        }

        /// <summary>
        /// This method is used to order elements in listbox descending by full path
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void menuItemOrderByPathDescending_Click(object sender, EventArgs e)
        {
            if (lbFiles.Items.Count > 0)
                lbFiles = SortItems(lbFiles, SortType.ByPath, OrderType.Descending);
            // lbFiles = SortItemsByPath(lbFiles, OrderType.Descending);
            else
                GenerateNoFileInListBoxMessage();
        }

        /// <summary>
        /// This method is used to order elements in listbox ascending by file name
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void menuItemOrderByNameAscending_Click(object sender, EventArgs e)
        {
            if (lbFiles.Items.Count > 0)
                lbFiles = SortItems(lbFiles, SortType.ByName, OrderType.Ascending);
            // lbFiles = SortItemsByName(lbFiles, OrderType.Ascending);
            else
                GenerateNoFileInListBoxMessage();
        }

        /// <summary>
        /// This method is used to order elements in listbox descending by file name
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void menuItemOrderByNameDescending_Click(object sender, EventArgs e)
        {
            if (lbFiles.Items.Count > 0)
                lbFiles = SortItems(lbFiles, SortType.ByName, OrderType.Descending);
            // lbFiles = SortItemsByName(lbFiles, OrderType.Descending);
            else
                GenerateNoFileInListBoxMessage();
        }

        /// <summary>
        /// This method is used to generate message
        /// When there is no item in listbox
        /// </summary>
        private static void GenerateNoFileInListBoxMessage()
        {
            MessageBox.Show(_resource.GetString("NoFile"), _resource.GetString("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// This method is used to order items by full path in listbox
        /// And return them with the given order as parametre
        /// </summary>
        /// <param name="listBox">ListBox Info</param>
        /// <param name="sortType">Sort Type (By Name or Path)</param>
        /// <param name="orderType">Order Type is Ascending Or Not</param>
        private static ListBox SortItems(ListBox listBox, SortType sortType, OrderType orderType)
        {
            var items = listBox.Items.OfType<object>().ToList();
            listBox.Items.Clear();

            // Sort Items By Name
            if (sortType == SortType.ByName)
            {
                listBox.Items.AddRange(orderType == OrderType.Ascending
                    ? items.OrderBy(i => i).ToArray()
                    : items.OrderByDescending(i => i).ToArray());
            }
            // Sort Items By Path
            else
            {
                const char c = '\u005c';
                var fileList = (from item in items select item.ToString() into fullPath let list = fullPath.Split(c).ToList() where list.Count > 0 select new FileInfo { FilePath = fullPath, FileName = list[list.Count - 1] }).ToList();

                fileList = orderType == OrderType.Ascending
                    ? fileList.OrderBy(j => j.FileName).ThenBy(j => j.FilePath).ToList()
                    : fileList.OrderByDescending(j => j.FileName).ThenByDescending(j => j.FilePath).ToList();

                foreach (var item in fileList)
                    listBox.Items.Add(item.FilePath);
            }

            return listBox;
        }

        /// <summary>
        /// This method is used to order items by full path in listbox
        /// And return them with the given order as parametre
        /// </summary>
        /// <param name="listBox">ListBox Info</param>
        /// <param name="orderType">Order Type is Ascending Or Not</param>
        private static ListBox SortItemsByPath(ListBox listBox, OrderType orderType)
        {
            var items = listBox.Items.OfType<object>().ToList();
            listBox.Items.Clear();
            listBox.Items.AddRange(orderType == OrderType.Ascending
                ? items.OrderBy(i => i).ToArray()
                : items.OrderByDescending(i => i).ToArray());
            return listBox;
        }

        /// <summary>
        /// This method is used to order items by file name in listbox
        /// And return them with the given order as parametre
        /// </summary>
        /// <param name="listBox">ListBox Info</param>
        /// <param name="orderType">Order Type is Ascending Or Not</param>
        private static ListBox SortItemsByName(ListBox listBox, OrderType orderType)
        {
            var items = listBox.Items.OfType<object>().ToList();
            const char c = '\u005c';
            var fileList = (from item in items select item.ToString() into fullPath let list = fullPath.Split(c).ToList() where list.Count > 0 select new FileInfo { FilePath = fullPath, FileName = list[list.Count - 1] }).ToList();

            fileList = orderType == OrderType.Ascending
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
            if (e.Effect == DragDropEffects.Copy) return;

            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// This is used to give effect while dropping an item to the listbox
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>
        private void lbFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        /// <summary>
        /// This is used to drag and drop an item in listbox
        /// Which is used to order books and combine them with that order
        /// </summary>
        /// <param name="sender">The sender info (For example Main Form)</param>
        /// <param name="e">Event Arguments</param>m>
        private void lbFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
                string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);

                var errorMessages = new List<string>();
                foreach (string file in fileNames)
                {
                    if (Path.GetExtension(file) == ".pdf")
                        lbFiles.Items.Add(file);
                    else errorMessages.Add(file);
                }
                // Display messages if non-pdf files
                if (errorMessages.Count > 0)
                {
                    string messages = string.Join(Environment.NewLine, errorMessages);
                    MessageBox.Show($"Only PDF files are allowed, and these files are invalid.\n\r{messages}", "Invalid File Type");
                }
            }
            else if (e.Effect == DragDropEffects.Move)
            {
                var point = lbFiles.PointToClient(new Point(e.X, e.Y));
                var index = lbFiles.IndexFromPoint(point);
                if (index < 0) index = lbFiles.Items.Count - 1;
                // type of string because file names stored in string in listbox
                var data = e.Data.GetData(typeof(string));
                lbFiles.Items.Remove(data);
                lbFiles.Items.Insert(index, data);
            }
        }

        #endregion
    }
}