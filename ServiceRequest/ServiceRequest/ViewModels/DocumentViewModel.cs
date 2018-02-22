using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using  System.Linq;

namespace ServiceRequest.ViewModels
{
    public sealed class DocumentViewModel
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Properties

        private bool _mIsDownloaded;
        private bool _mIsVisibleDownloading;
        private DocumentDownloadState _downloadState;

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Properties
        public const string PENDINGIMAGE = "pending_upload.png";
        public const string CLOUDIMAGE = "uploaded.png";

        public delegate DocumentViewModel StateChangedEventargument(DocumentViewModel sender, DocumentDownloadState state);
        public event StateChangedEventargument StateChanged;
        public DocumentDownloadState DownloadState
        {
            get { return _downloadState; }
            set
            {
                if (_downloadState != value)
                {
                    _downloadState = value;
                    StateChanged?.Invoke(this, DownloadState);
                }
            }
        }

        public OnSiteDocument SriDocument { get; set; }
        public string DocumentSize { get; set; }
        public string ArrowImage { get; set; }
        public string PendingImage { get; set; }
        public string DocumentName { get; set; }
        public Color BackgroundColor { get; set; }
        public bool IsBtnVisible { get; set; }

        public string ImagePath { get; set; }

        public static string RefVal { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsVisible_Downloading
        {
            get { return _mIsVisibleDownloading; }
            set
            {
                _mIsVisibleDownloading = value;
                OnPropertyChanged(nameof(IsVisible_Downloading));
            }
        }
        public bool IsDownloaded
        {
            get { return _mIsDownloaded; }
            set
            {
                _mIsDownloaded = value;
                OnPropertyChanged(nameof(IsDownloaded));
            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Constructor

        public DocumentViewModel()
        {
            try
            {
                StateChanged += OnStateChanged;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Functions

        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetDocumentList
        /// 
        /// <summary>	Gets the list of Documents form the API.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        public async Task<List<DocumentViewModel>> GetDocumentList()
        {
            try
            {
                List<DocumentViewModel> lstvwDocuments = new List<DocumentViewModel>();
                DocumentViewModel documentModel;
                if (AppData.PropertyModel.SelectedProperty?.Documents != null)
                    foreach (var data in AppData.PropertyModel.SelectedProperty.Documents)
                    {
                        foreach (var meta in data.EntityDocumentLists)
                        {
                            foreach (var document in meta.Documents)
                            {
                                if (!document.Extension.Contains("."))
                                    document.Extension = CheckExtension(document.Extension);
                                if (await FileSystem.Exists(document.FileName))
                                {
                                    documentModel = new DocumentViewModel
                                    {
                                        DocumentName = document.FileName,
                                        SriDocument = document,
                                        DocumentSize = (document.Size.HasValue) ? document.Size.Value.ToByteSizeString() : "",
                                        ArrowImage = "chevron.png",
                                        BackgroundColor = Color.White,
                                        IsBtnVisible = false,
                                        IsDownloaded = true,
                                        DownloadState = DocumentDownloadState.Completed,
                                        PendingImage = DocSyncStatus(document),
                                        ImagePath = document.Extension.GetDocumentType()
                                    };
                                }
                                else
                                {
                                    documentModel = new DocumentViewModel
                                    {
                                        DocumentName = document.FileName,
                                        SriDocument = document,
                                        DocumentSize = (document.Size.HasValue) ? document.Size.Value.ToByteSizeString() : "",
                                        ArrowImage = "",
                                        BackgroundColor = Styles.WindowBackground,
                                        IsBtnVisible = true,
                                        IsDownloaded = false,
                                        DownloadState = DocumentDownloadState.Begin,
                                        PendingImage = DocSyncStatus(document),
                                        ImagePath = document.Extension.GetDocumentType()
                                    };
                                }                               
                                lstvwDocuments.Add(documentModel);

                               lstvwDocuments = lstvwDocuments.OrderBy(e => e.PendingImage).ToList();
                            }
                        }
                    }
                return lstvwDocuments;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }


        public async Task<List<DocumentViewModel>> GetFilteredDocumentList()
        {
            try
            {
                List<DocumentViewModel> lstvwDocuments = new List<DocumentViewModel>();
                DocumentViewModel documentModel;
                if (AppData.PropertyModel.SelectedProperty?.Documents != null)
                    foreach (var data in AppData.PropertyModel.SelectedProperty.Documents)
                    {
                        foreach (var meta in data.EntityDocumentLists)
                        {
                            if (meta.EntityKey.Equals(RefVal))
                            {
                                foreach (var document in meta.Documents)
                                {
                                    if (!document.Extension.Contains("."))
                                        document.Extension = CheckExtension(document.Extension);
                                    if (await FileSystem.Exists(document.FileName))
                                    {
                                        documentModel = new DocumentViewModel
                                        {
                                            DocumentName = document.FileName,
                                            SriDocument = document,
                                            DocumentSize =
                                                (document.Size.HasValue) ? document.Size.Value.ToByteSizeString() : "",
                                            ArrowImage = "chevron.png",
                                            BackgroundColor = Color.White,
                                            IsBtnVisible = false,
                                            IsDownloaded = true,
                                            DownloadState = DocumentDownloadState.Completed,
                                            PendingImage = DocSyncStatus(document),
                                            ImagePath = document.Extension.GetDocumentType()
                                        };
                                    }
                                    else
                                    {
                                        documentModel = new DocumentViewModel
                                        {
                                            DocumentName = document.FileName,
                                            SriDocument = document,
                                            DocumentSize =
                                                (document.Size.HasValue) ? document.Size.Value.ToByteSizeString() : "",
                                            ArrowImage = "",
                                            BackgroundColor = Styles.WindowBackground,
                                            IsBtnVisible = true,
                                            IsDownloaded = false,
                                            DownloadState = DocumentDownloadState.Begin,
                                            PendingImage = DocSyncStatus(document),
                                            ImagePath = document.Extension.GetDocumentType()
                                        };
                                    }
                                    lstvwDocuments.Add(documentModel);

                                    lstvwDocuments = lstvwDocuments.OrderBy(e => e.PendingImage).ToList();
                                }
                            }
                        }
                    }
                return lstvwDocuments;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions

        private string CheckExtension( string extension)
         {
            try
            {
                var aStr = extension.ToString();
                var result = aStr.Insert(0, ".");
                return result;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            try
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		DocSyncStatus
        /// 
        /// <summary>	Loads sync upload image
        /// </summary>
        /// <param>	</param>
        /// <param name="document"></param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        private static string DocSyncStatus(OnSiteDocument document)
        {
            try
            {
                string image;
                switch (document.Status)
                {
                    case SyncStatus.Changed:
                        image = PENDINGIMAGE;
                        break;
                    case SyncStatus.Saved:
                        image = CLOUDIMAGE;
                        break;
                    default:
                        image = string.Empty;
                        break;
                }
                return image;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        private DocumentViewModel OnStateChanged(DocumentViewModel document, DocumentDownloadState state)
        {
            try
            {
                switch (state)
                {
                    case DocumentDownloadState.Begin:
                        document.ArrowImage = "";
                        document.BackgroundColor = Styles.WindowBackground;
                        document.IsBtnVisible = true;
                        document.IsDownloaded = false;
                        document.IsVisible_Downloading = false;

                        break;
                    case DocumentDownloadState.Inprogress:
                        document.IsVisible_Downloading = true;
                        document.IsBtnVisible = false;
                        break;
                    case DocumentDownloadState.Completed:
                        document.ArrowImage = "chevron.png";
                        document.BackgroundColor = Color.White;
                        document.IsBtnVisible = false;
                        document.IsDownloaded = true;
                        document.IsVisible_Downloading = false;

                        break;
                }
                return document;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
