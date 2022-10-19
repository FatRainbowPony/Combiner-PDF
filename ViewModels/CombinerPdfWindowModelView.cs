using Combiner_PDF.Support;
using GongSolutions.Wpf.DragDrop;
using PdfSharp.Pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Combiner_PDF.ViewModels
{
    public class CombinerPdfWindowModelView: Base.ModelView, IDropTarget
    {
        #region Fields

        #region Private
        private string pathToPdfDocument;
        private ImageSource iconDoc;
        private ObservableCollection<string> pathsToPdfDocuments = new ObservableCollection<string>();
        private ObservableCollection<Models.PdfDoc> listOfPdfDocs = new ObservableCollection<Models.PdfDoc>();
        private bool isPdfDocument;
        private bool isActiveMerging;
        private bool isActiveAddingPdfDoc;
        private bool isActiveDeletingAllPdfDocs;
        #endregion

        #region Public

        #endregion

        #endregion

        #region Properties

        #region Private
        private ObservableCollection<string> PathsToPdfDocuments
        {
            get => pathsToPdfDocuments;
            set => Set(ref pathsToPdfDocuments, value);
        }

        private bool IsPdfDocument
        {
            get => isPdfDocument;
            set => Set(ref isPdfDocument, value);
        }
        #endregion

        #region Public
        public string PathToPdfDocument
        {
            get => pathToPdfDocument;
            set => Set(ref pathToPdfDocument, value);
        }

        public ImageSource IconDoc
        {
            get => iconDoc;
            set => Set(ref iconDoc, value);
        }

        public ObservableCollection<Models.PdfDoc> ListOfPdfDocs
        {
            get => listOfPdfDocs;
            set => Set(ref listOfPdfDocs, value);
        }

        public bool IsActiveMerging
        {
            get => isActiveMerging;
            set => Set(ref isActiveMerging, value);
        }

        public bool IsActiveAddingPdfDoc
        {
            get => isActiveAddingPdfDoc;
            set => Set(ref isActiveAddingPdfDoc, value);
        }

        public bool IsActiveDeletingAllPdfDocs
        {
            get => isActiveDeletingAllPdfDocs;
            set => Set(ref isActiveDeletingAllPdfDocs, value);
        }
        #endregion

        #endregion

        #region Methods

        #region Private
        private bool IsNewPathToPdfDocumentCorrect()
        {
            if (!string.IsNullOrEmpty(PathToPdfDocument))
            {
                if (!string.IsNullOrWhiteSpace(PathToPdfDocument))
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckAbilityUnlockBuuttons()
        {
            if (!string.IsNullOrEmpty(PathToPdfDocument))
            {
                IsActiveAddingPdfDoc = true;
            }
            else
            {
                IsActiveAddingPdfDoc = false;
            }

            if (ListOfPdfDocs.Count > 1)
            {
                IsActiveMerging = true;
            }
            else
            {
                IsActiveMerging = false;
            }

            if (ListOfPdfDocs.Count > 0)
            {
                IsActiveDeletingAllPdfDocs = true;
            }
            else
            {
                IsActiveDeletingAllPdfDocs = false;
            }

            if (ListOfPdfDocs.Count > 0)
            {
                IsActiveDeletingAllPdfDocs = true;
            }
            else
            {
                IsActiveDeletingAllPdfDocs = false;
            }
        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is Models.PdfDoc)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
            else
            {
                IsPdfDocument = false;

                var dragFilesList = (dropInfo.Data as DataObject).GetFileDropList();

                PathsToPdfDocuments.Clear();

                foreach (var dragFile in dragFilesList)
                {
                    if (Path.GetExtension(dragFile).ToLower() == ".pdf")
                    {
                        IsPdfDocument = true;

                        PathsToPdfDocuments.Add(Path.GetFullPath(dragFile));
                    }
                }

                if (IsPdfDocument == true && PathsToPdfDocuments != null)
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is Models.PdfDoc)
            {
                ListOfPdfDocs.Remove((Models.PdfDoc)dropInfo.Data);

                try
                {
                    ListOfPdfDocs.Insert(dropInfo.InsertIndex, (Models.PdfDoc)dropInfo.Data);
                }
                catch
                {
                    ListOfPdfDocs.Add((Models.PdfDoc)dropInfo.Data);
                }
            }
            else
            {
                if (IsPdfDocument == true)
                {
                    var sortPathsToPdfDocuments = PathsToPdfDocuments.Distinct();

                    foreach (var sortPathToPdfDocument in sortPathsToPdfDocuments)
                    {
                        var newPdfDoc = new Models.PdfDoc
                        {
                            PathToPdfDocument = sortPathToPdfDocument,
                            IconDoc = IconWorker.FileToImageIconConverter(sortPathToPdfDocument)
                        };

                        ListOfPdfDocs.Add(newPdfDoc);
                    }

                    PathsToPdfDocuments.Clear();

                    CheckAbilityUnlockBuuttons();
                }
            }
        }

        void IDropTarget.DragLeave(IDropInfo dropInfo)
        {
            //
        }

        void IDropTarget.DragEnter(IDropInfo dropInfo)
        {
            //
        }
        #endregion

        #region Public

        #endregion

        #endregion

        #region Commands

        #region Private
        public ICommand GetPathToPdfComm
        {
            get
            {
                return new Commands.VMCommands((obj) =>
                {
                    PathToPdfDocument = PdfWorker.GetPathToPdfDoc();
                    IconDoc = IconWorker.FileToImageIconConverter(PathToPdfDocument);

                    CheckAbilityUnlockBuuttons();
                }, (obj) => { return true; });
            }
        }

        public ICommand MergePdfDocumentsComm
        {
            get
            {
                return new Commands.VMCommands(parameter =>
                {
                    if (parameter is ObservableCollection<Models.PdfDoc>)
                    {
                        try
                        {
                            var pathsToPdfDocuments = new ObservableCollection<string>();

                            foreach (var itemInParameter in (parameter as ObservableCollection<Models.PdfDoc>))
                            {
                                pathsToPdfDocuments.Add(itemInParameter.PathToPdfDocument);
                            }

                            PdfWorker.MergePdfDocuments(pathsToPdfDocuments);
                        }
                        catch
                        {
                            MessageBox.Show("\tСлучилась непредвиденная ошибка\t \tВозможно некоторые добавленные файлы не существуют\t", "Ошибка", 
                                MessageBoxButton.OK, MessageBoxImage.Error);

                            ListOfPdfDocs.Clear();
                        }
                    }

                    CheckAbilityUnlockBuuttons();
                }, (obj) => { return true; });
            }
        }

        public ICommand AddPdfDocumentComm
        {
            get
            {
                return new Commands.VMCommands((obj) =>
                {
                    var newPdfDoc = new Models.PdfDoc
                    {
                        PathToPdfDocument = PathToPdfDocument,
                        IconDoc = IconDoc                      
                    };

                    ListOfPdfDocs.Add(newPdfDoc);
                    PathToPdfDocument = string.Empty;

                    CheckAbilityUnlockBuuttons();
                }, (obj) => { return IsNewPathToPdfDocumentCorrect(); });
            }
        }

        public ICommand DeletePdfDocumentComm
        {
            get
            {
                return new Commands.VMCommands(parameter =>
                {
                    if (parameter is Models.PdfDoc)
                    {
                        ListOfPdfDocs.Remove(parameter as Models.PdfDoc);
                    }

                    CheckAbilityUnlockBuuttons();
                }, (obj) => true);
            }
        }

        public ICommand DeleteAllPdfDocumnets
        {
            get
            {
                return new Commands.VMCommands((obj) =>
                {
                    ListOfPdfDocs.Clear();

                    CheckAbilityUnlockBuuttons();
                }, (obj) => { return true; });
            }
        }
        #endregion

        #region Public

        #endregion

        #endregion
    }
}
