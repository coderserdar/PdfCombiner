# PDF Combiner

![GitHub stars](https://img.shields.io/github/stars/coderserdar/PdfCombiner?style=social) ![GitHub forks](https://img.shields.io/github/forks/coderserdar/PdfCombiner?style=social) ![GitHub watchers](https://img.shields.io/github/watchers/coderserdar/PdfCombiner?style=social) ![GitHub repo size](https://img.shields.io/github/repo-size/coderserdar/PdfCombiner?style=plastic) ![GitHub language count](https://img.shields.io/github/languages/count/coderserdar/PdfCombiner?style=plastic) ![GitHub top language](https://img.shields.io/github/languages/top/coderserdar/PdfCombiner?style=plastic) ![GitHub last commit](https://img.shields.io/github/last-commit/coderserdar/PdfCombiner?color=red&style=plastic)

This is a **Windows Form** app which is used to combine multiple **PDF** files in a single PDF file where you locate it with the app.
I use both [iTextSharp](https://www.nuget.org/packages/iTextSharp/) and [PdfSharp](http://www.pdfsharp.net) *NuGet* packages. You can prefer 1 of 2 options to combine your PDF files. 
I tested this app a lot, but if you test and send me issues I will be so appreciated to you. 
This app is written in **C#** programming language and in **Visual Studio 2017**. 

In this app you can do;

 - Add PDF files with File Dialog (you can add multiple files)
 - Add PDF files within a selected Folder with Folder Dialog
 	+ You will add the PDF files recursively, it adds the files in subfolders.
 - Delete the files which you don't wnat to combine in your file list
 - Combine files via **PdfSharp**
 - Combine files via **iTextSharp**
 - Look the process statues in the progress bar
 - Info messages after every operation in app
   
# Documentation and Screenshots

You can look up the source code's documentation in [Documentation](https://github.com/coderserdar/PdfCombiner/blob/main/Documentation/PdfCombiner.pdf) section. In this *PDF* file you can analyze source code. This PDF file supports Hyperlink, so you can go to a specific function easily. I try to write detailed comments in functions.

And you can look up screenshots like 

<table>
   <tr>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/App_Screens_01.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/App_Screens_02.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/App_Screens_03.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/App_Screens_04.png?raw=true"></td>
   </tr>
   <tr>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/App_Screens_05.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/App_Screens_06.png?raw=true"></td>
      <td><img src="https://github.com/coderserdar/PdfCombiner/blob/main/Screenshots/App_Screens_07.png?raw=true"></td>
   </tr>
</table>