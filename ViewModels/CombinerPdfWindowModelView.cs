using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Combiner_PDF.Support;
using GongSolutions.Wpf.DragDrop;

namespace Combiner_PDF.ViewModels
{
    public class CombinerPdfWindowModelView: Base.ModelView, IDropTarget
    {
        #region Fields

        #region Private
        private string pathToPdfDoc;
        private string namePdfDoc;
        private ImageSource iconDoc;
        private BitmapSource previewPdfDoc;
        private ObservableCollection<string> pathsToPdfDocs = new ObservableCollection<string>();
        private ObservableCollection<Models.PdfDoc> listOfPdfDocs = new ObservableCollection<Models.PdfDoc>();
        private bool isPdfDoc;
        private bool isActiveMerging;
        private bool isActiveAddingPdfDoc;
        private bool isActiveDeletingAllPdfDocs;
        private bool isVisiblePreview;
        private string statusMerging;
        #endregion

        #region Public

        #endregion

        #endregion

        #region Properties

        #region Private
        private ObservableCollection<string> PathsToPdfDocs
        {
            get => pathsToPdfDocs;
            set => Set(ref pathsToPdfDocs, value);
        }

        private bool IsPdfDocument
        {
            get => isPdfDoc;
            set => Set(ref isPdfDoc, value);
        }
        #endregion

        #region Public
        public string PathToPdfDoc
        {
            get => pathToPdfDoc;
            set => Set(ref pathToPdfDoc, value);
        }

        public string NamePdfDoc
        {
            get => namePdfDoc;
            set => Set(ref namePdfDoc, value);
        }

        public ImageSource IconDoc
        {
            get => iconDoc;
            set => Set(ref iconDoc, value);
        }

        public BitmapSource PreviewPdfDoc
        {
            get => previewPdfDoc;
            set => Set(ref previewPdfDoc, value);
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

        public bool IsVisiblePreview
        {
            get => isVisiblePreview;
            set => Set(ref isVisiblePreview, value);
        }

        public string StatusMerging
        {
            get => statusMerging;
            set => Set(ref statusMerging, value);
        }
        #endregion

        #endregion

        #region Constructors

        #region Private

        #endregion

        #region Public
        public CombinerPdfWindowModelView()
        {
            StatusMerging = "Ожидание документов для объединения";

            IsVisiblePreview = false;
        }
        #endregion

        #endregion

        #region Methods

        #region Private
        private bool IsNewPathToPdfDocCorrect()
        {
            if (!string.IsNullOrEmpty(PathToPdfDoc))
            {
                if (!string.IsNullOrWhiteSpace(PathToPdfDoc))
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckAbilityUnlockBuuttons()
        {
            if (!string.IsNullOrEmpty(PathToPdfDoc))
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

        private void CheckStatusMerging()
        {
            if (ListOfPdfDocs.Count > 1)
            {
                StatusMerging = "Добавленные документы могут быть объединены";
            }
            else
            {
                StatusMerging = "Ожидание документов для объединения";
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

                PathsToPdfDocs.Clear();

                foreach (var dragFile in dragFilesList)
                {
                    if (Path.GetExtension(dragFile).ToLower() == ".pdf")
                    {
                        IsPdfDocument = true;

                        PathsToPdfDocs.Add(Path.GetFullPath(dragFile));
                    }
                }

                if (IsPdfDocument == true && PathsToPdfDocs != null)
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
                    var sortPathsToPdfDocs = PathsToPdfDocs.Distinct();

                    foreach (var sortPathToPdfDoc in sortPathsToPdfDocs)
                    {
                        var newPdfDoc = new Models.PdfDoc
                        {
                            PathToPdfDoc = sortPathToPdfDoc,
                            IconDoc = IconWorker.FileToImageIconConverter(sortPathToPdfDoc)
                        };

                        ListOfPdfDocs.Add(newPdfDoc);
                    }

                    PathsToPdfDocs.Clear();

                    CheckAbilityUnlockBuuttons();
                    CheckStatusMerging();
                }
            }
        }

        void IDropTarget.DragEnter(IDropInfo dropInfo)
        {
            //
        }

        void IDropTarget.DragLeave(IDropInfo dropInfo)
        {
            //
        }
        #endregion

        #region Public

        #endregion

        #endregion

        #region Commands

        #region Private

        #endregion

        #region Public
        public ICommand GetPathToPdfDocComm
        {
            get
            {
                return new Commands.VMCommands((obj) =>
                {
                    PathToPdfDoc = PdfWorker.GetPathToPdfDoc();
                    IconDoc = IconWorker.FileToImageIconConverter(PathToPdfDoc);

                    CheckAbilityUnlockBuuttons();
                    CheckStatusMerging();
                }, (obj) => { return true; });
            }
        }

        public ICommand MergePdfDocsComm
        {
            get
            {
                return new Commands.VMCommands(parameter =>
                {
                    if (parameter is ObservableCollection<Models.PdfDoc>)
                    {
                        try
                        {
                            var isFinishedMerging = false;
                            var pathsToPdfDocs = new ObservableCollection<string>();

                            foreach (var itemInParameter in (parameter as ObservableCollection<Models.PdfDoc>))
                            {
                                pathsToPdfDocs.Add(itemInParameter.PathToPdfDoc);
                            }

                            if (isFinishedMerging == false)
                            {
                                StatusMerging = "В процессе объединения";
                            }

                            isFinishedMerging = PdfWorker.MergePdfDocuments(pathsToPdfDocs, out string path);

                            if (isFinishedMerging == true)
                            {
                                StatusMerging = "Объединение закончено";

                                string args = string.Format("/e, /select, \"{0}\"", path);

                                ProcessStartInfo info = new ProcessStartInfo();
                                info.FileName = "Explorer";
                                info.Arguments = args;

                                Process.Start(info);
                            }
                            else
                            {
                                CheckStatusMerging();
                            }
                        }
                        catch(Exception exception)
                        {
                            if (exception is FileNotFoundException)
                            {
                                MessageBox.Show("Случилась непредвиденная ошибка\r\nВозможно некоторые добавленные файлы не существуют", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);

                                ListOfPdfDocs.Clear();

                                CheckStatusMerging();
                            }

                            if (exception is IOException)
                            {
                                MessageBox.Show("Случилась непредвиденная ошибка\r\nВозможно файл с таким именем используется другим приложением", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);

                                CheckStatusMerging();
                            }
                        }
                    }

                    CheckAbilityUnlockBuuttons();
                }, (obj) => { return true; });
            }
        }

        public ICommand AddPdfDocComm
        {
            get
            {
                return new Commands.VMCommands((obj) =>
                {
                    var newPdfDoc = new Models.PdfDoc
                    {
                        PathToPdfDoc = PathToPdfDoc,
                        IconDoc = IconDoc                      
                    };

                    ListOfPdfDocs.Add(newPdfDoc);
                    PathToPdfDoc = string.Empty;

                    CheckAbilityUnlockBuuttons();
                    CheckStatusMerging();
                }, (obj) => { return IsNewPathToPdfDocCorrect(); });
            }
        }

        public ICommand DeleteAllPdfDocsComm
        {
            get
            {
                return new Commands.VMCommands((obj) =>
                {
                    ListOfPdfDocs.Clear();

                    CheckAbilityUnlockBuuttons();
                    CheckStatusMerging();
                }, (obj) => { return true; });
            }
        }

        public ICommand MoveUpPdfDocComm
        {
            get
            {
                return new Commands.VMCommands(parameter =>
                {
                    if (parameter is Models.PdfDoc)
                    {
                        var index = ListOfPdfDocs.IndexOf(parameter as Models.PdfDoc);

                        if (ListOfPdfDocs.Count > 1)
                        {
                            if (ListOfPdfDocs.First() != parameter as Models.PdfDoc)
                            {
                                ListOfPdfDocs.Remove(parameter as Models.PdfDoc);
                                ListOfPdfDocs.Insert(index - 1, parameter as Models.PdfDoc);

                            }
                        }
                    }

                    CheckAbilityUnlockBuuttons();
                    CheckStatusMerging();
                }, (obj) => true);
            }
        }

        public ICommand MoveDownPdfDocComm
        {
            get
            {
                return new Commands.VMCommands(parameter =>
                {
                    if (parameter is Models.PdfDoc)
                    {
                        var index = ListOfPdfDocs.IndexOf(parameter as Models.PdfDoc);

                        if (ListOfPdfDocs.Count > 1)
                        {
                            if (ListOfPdfDocs.Last() != parameter as Models.PdfDoc)
                            {
                                ListOfPdfDocs.Remove(parameter as Models.PdfDoc);
                                ListOfPdfDocs.Insert(index + 1, parameter as Models.PdfDoc);
                            }
                        }
                    }

                    CheckAbilityUnlockBuuttons();
                    CheckStatusMerging();
                }, (obj) => true);
            }
        }

        public ICommand DeletePdfDocComm
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
                    CheckStatusMerging();
                }, (obj) => true);
            }
        }

        public ICommand OpenPreviewPdfDocComm
        {
            get
            {
                return new Commands.VMCommands(parameter =>
                {
                    if (parameter is Models.PdfDoc)
                    {
                        try
                        {
                            NamePdfDoc = Path.GetFileName((parameter as Models.PdfDoc).PathToPdfDoc);

                            PreviewPdfDoc = BitmapWorker.ToBitmapSource(PdfiumViewer.Core.PdfDocument.Load((parameter as Models.PdfDoc).PathToPdfDoc).Render(0, 400, 550, 1920, 1080, false));

                            IsVisiblePreview = true;
                        }
                        catch{}
                    }
                }, (obj) => true);
            }
        }

        public ICommand ClosePreviewPdfDocComm
        {
            get
            {
                return new Commands.VMCommands(parameter =>
                {
                    if (parameter is Models.PdfDoc)
                    {
                        NamePdfDoc = string.Empty;
                        
                        PreviewPdfDoc = default;

                        IsVisiblePreview = false;
                    }
                }, (obj) => true);
            }
        }
        #endregion

        #endregion
    }
}
