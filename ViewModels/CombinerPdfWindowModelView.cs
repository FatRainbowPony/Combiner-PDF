using Combiner_PDF.Support;
using PdfSharp.Pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Combiner_PDF.ViewModels
{
    public class CombinerPdfWindowModelView: Base.ModelView
    {
        #region Fields

        #region Private
        private string pathToPdfDocument;
        private ImageSource iconDoc;
        private ObservableCollection<Models.PdfDoc> listOfPdfDocs = new ObservableCollection<Models.PdfDoc>();
        private bool isActiveMerging;
        private bool isActiveAddingPdfDoc;
        private bool isActiveDeletingAllPdfDocs;
        #endregion

        #region Public

        #endregion

        #endregion

        #region Properties

        #region Private

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
            if (!string.IsNullOrEmpty(this.PathToPdfDocument))
            {
                IsActiveAddingPdfDoc = true;
            }
            else
            {
                IsActiveAddingPdfDoc = false;
            }

            if (this.ListOfPdfDocs.Count > 1)
            {
                IsActiveMerging = true;
            }
            else
            {
                IsActiveMerging = false;
            }

            if (this.ListOfPdfDocs.Count > 0)
            {
                IsActiveDeletingAllPdfDocs = true;
            }
            else
            {
                IsActiveDeletingAllPdfDocs = false;
            }

            if (this.ListOfPdfDocs.Count > 0)
            {
                IsActiveDeletingAllPdfDocs = true;
            }
            else
            {
                IsActiveDeletingAllPdfDocs = false;
            }
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
                    this.PathToPdfDocument = PdfWorker.GetPathToPdfDoc();
                    this.IconDoc = IconWorker.FileToImageIconConverter(this.PathToPdfDocument);

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
                        var pathsToPdfDocuments = new ObservableCollection<string>();

                        foreach (var itemInParameter in (parameter as ObservableCollection<Models.PdfDoc>))
                        {
                            pathsToPdfDocuments.Add(itemInParameter.PathToPdfDocument);
                        }

                        PdfWorker.MergePdfDocuments(pathsToPdfDocuments);
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
                        PathToPdfDocument = this.PathToPdfDocument,
                        IconDoc = this.IconDoc
                        
                    };

                    this.ListOfPdfDocs.Add(newPdfDoc);
                    this.PathToPdfDocument = string.Empty;
                    this.IconDoc = default;

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
