# PDF Combiner

![GitHub stars](https://img.shields.io/github/stars/coderserdar/PdfCombiner?style=social) ![GitHub forks](https://img.shields.io/github/forks/coderserdar/PdfCombiner?style=social) ![GitHub watchers](https://img.shields.io/github/watchers/coderserdar/PdfCombiner?style=social) ![GitHub repo size](https://img.shields.io/github/repo-size/coderserdar/PdfCombiner?style=plastic) ![GitHub language count](https://img.shields.io/github/languages/count/coderserdar/PdfCombiner?style=plastic) ![GitHub top language](https://img.shields.io/github/languages/top/coderserdar/PdfCombiner?style=plastic) ![GitHub last commit](https://img.shields.io/github/last-commit/coderserdar/PdfCombiner?color=red&style=plastic) ![GitHub issues](https://img.shields.io/github/issues/ss34nirala34/PdfCombiner)

This is a **Windows Form** app which is used to combine multiple **PDF** files in a single PDF file where you locate it with the app.

|  Programming Language  |  .NET Version  | Development Platform |     NuGet Packages     |
|------------------------|----------------|----------------------|------------------------|
|          *C#*          |      *4.8*     | *Visual Studio 2022* | *iTextSharp, PdfSharp* |

<img src="https://github.com/ss34nirala34/PdfCombiner/blob/main/Screenshots/Main/MainScreen.png?raw=true">

I use both [iTextSharp](https://www.nuget.org/packages/iTextSharp/) and [PdfSharp](http://www.pdfsharp.net) *NuGet* packages. You can prefer both options to combine your PDF files. 
I tested this app a lot, but if you test and send me issues I will be so appreciated to you. 
This app is written in **C#** programming language and in **Visual Studio 2022**. 

In this app you can do;

 - Add PDF files with File Dialog (you can add multiple files)
 - Add PDF files within a selected Folder with Folder Dialog
 	+ You will add the PDF files recursively, it adds the files in subfolders.
 - In file list (With right click, you can see menu items for)
    + *Deleting selected files*
    + *Selecting all files*
    + Ordering files by path
       * *Ascending* or *descending*
    + Ordering files by file name
       * *Ascending* or *descending*
 - You can order the books in listbox with *drag drop*
 - Delete the files which you don't want to combine in your file list
 - Combine files via **PdfSharp**
 - Combine files via **iTextSharp**
 - Multi Language support for **English**, **Turkish**, **German**, **French**, **Spanish**, **Russian**, **Italian**, **Chinese (Basic)**, **Arabic**, **Dutch**, **Portuguese**, **Bengali**, **Indonesian**, **Indian**, **Japanese**
 - Look the process status in the progress bar (Adding files or combining files)
 - Info messages after every operation in app
   
# Documentation and Screenshots

You can look up the source code's documentation in [Documentation](https://github.com/coderserdar/PdfCombiner/blob/main/Documentation/PdfCombiner.pdf) section. In this *PDF* file you can analyze source code. This PDF file supports Hyperlink, so you can go to a specific function easily. I try to write detailed comments in functions.

And you can look up screenshots like 

**v4**
Consider the following revised version v4:

 - Upgraded to the .NET 4.8 framework to facilitate the addition of new features and support future development and enhancements.
 - Now compatible with Visual Studio 2022.
 - Introduced a new application icon.
 - Implemented a docking feature to enable the adjustment of controls in full-screen mode.
 
<table>
   <tr>
      <td><img src="https://github.com/ss34nirala34/PdfCombiner/blob/main/Screenshots/v4/App_Screens_01.png?raw=true"></td>
      <td><img src="https://github.com/ss34nirala34/PdfCombiner/blob/main/Screenshots/v4/App_Screens_02.png?raw=true"></td>
   </tr>
</table>

**v3**

<table>
   <tr>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v3/App_Screens_01.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v3/App_Screens_02.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v3/App_Screens_03.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v3/App_Screens_04.png?raw=true"></td>
   </tr>
   <tr>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v3/App_Screens_05.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v3/App_Screens_06.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v3/App_Screens_07.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v3/App_Screens_08.png?raw=true"></td>
   </tr>
   <tr>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v3/App_Screens_09.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v3/App_Screens_10.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v3/App_Screens_11.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v3/App_Screens_12.png?raw=true"></td>
   </tr>
</table>

**v2**

<table>
   <tr>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v2/App_Screens_01.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v2/App_Screens_02.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v2/App_Screens_03.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v2/App_Screens_04.png?raw=true"></td>
   </tr>
   <tr>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v2/App_Screens_05.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v2/App_Screens_06.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v2/App_Screens_07.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v2/App_Screens_08.png?raw=true"></td>
   </tr>
</table>

**v1**

<table>
   <tr>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v1/App_Screens_01.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v1/App_Screens_02.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v1/App_Screens_03.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v1/App_Screens_04.png?raw=true"></td>
   </tr>
   <tr>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v1/App_Screens_05.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v1/App_Screens_06.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/v1/App_Screens_07.png?raw=true"></td>
   </tr>
</table>