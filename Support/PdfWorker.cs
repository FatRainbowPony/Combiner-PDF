using Combiner_PDF.Models;
using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combiner_PDF.Support
{
    public static class PdfWorker
    {
        public static string GetPathToPdfDoc()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Добавить PDF-документ";
            openFileDialog.Filter = "Файлы PDF (*.pdf)|*.pdf";
            openFileDialog.DefaultExt = "pdf";

            string pathToPdfDoc = "";

            var result = openFileDialog.ShowDialog();

            if (result == true)
            {
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    pathToPdfDoc = openFileDialog.FileName;
                }
            }

            return pathToPdfDoc;
        }

        private static string SetPathToPdfDoc()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Сохранить PDF-документ";
            saveFileDialog.Filter = "Файлы PDF (*.pdf)|*.pdf";
            saveFileDialog.DefaultExt = "pdf";

            string pathToPdfDoc = "";

            var result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    pathToPdfDoc = saveFileDialog.FileName;
                }
            }

            return pathToPdfDoc;
        }

        public static void MergePdfDocuments(ObservableCollection<string> pathsToPdfDocuments)
        {
            var outputFilePath = SetPathToPdfDoc();
            
            if (!string.IsNullOrEmpty(outputFilePath))
            {
                PdfDocument outputPdfDocument = new PdfDocument();

                foreach (string pathToPdfDocument in pathsToPdfDocuments)
                {
                    PdfDocument inputPdfDocument = PdfReader.Open(pathToPdfDocument, PdfDocumentOpenMode.Import);
                    outputPdfDocument.Version = inputPdfDocument.Version;

                    foreach (PdfPage page in inputPdfDocument.Pages)
                    {
                        outputPdfDocument.AddPage(page);
                    }
                }

                outputPdfDocument.Save(outputFilePath);
            }
        }
    }
}
