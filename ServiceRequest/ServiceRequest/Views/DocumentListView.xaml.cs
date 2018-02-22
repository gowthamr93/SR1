using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceRequest.Pages;
using ServiceRequest.Views.PopUp;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using ServiceRequest.ViewModels;
using ServiceRequest.AppContext;
using Idox.LGDP.Apps.ServiceRequest.Client;
using System.Diagnostics;
using ServiceRequest.DependencyInterfaces;
using Idox.LGDP.Apps.Common.OnSite;
using System.Linq;

namespace ServiceRequest.Views
{
    public partial class DocumentListView: ContentView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        readonly DocumentViewModel _documentListModel;
        ObservableCollection<DocumentViewModel> _lstDocuments;
        public static Action StopProcess;
        private static bool _sortDocType;
       
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        public DocumentListView()
        {
            try
            {
                InitializeComponent();
                Lbl_CaseName.Text = "Property Summary";
                _documentListModel = new DocumentViewModel();
                //On Back Tapped
                var onBackTapped = new TapGestureRecognizer();
                onBackTapped.Tapped += BackImageTapped;
                Img_Back.GestureRecognizers.Add(onBackTapped);
                Lbl_CaseName.GestureRecognizers.Add(onBackTapped);
                Lstvw_Documents.ItemTapped += OnTapDocumentItem;
                Loading();
                PkrRefVal.SelectedIndexChanged += FilterbyRefval;
                StopProcess += OnStopProcess;
                AddNewImageView.DocumentAdded += OnDocumentAdded;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private  void FilterbyRefval(object sender, EventArgs e)
        {
            FilterList();
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------
       
        /// ------------------------------------------------------------------------------------------------
        #region Private Functions
        ///-------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        /// Name		BackImageTapped
        /// 
        /// <summary> handles the navigation to the previous page.
        /// </summary>
        /// /// <param name="sender">  </param>
        /// <param name="e">event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async void BackImageTapped(object sender, EventArgs e)
        {
            try
            {
                await SplitView.Instace().PopRightContent();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private async void FilterList()
        {
            string item = PkrRefVal.Items[PkrRefVal.SelectedIndex];
            if (item != "All")
            {
                foreach (var record in AppData.PropertyModel.SelectedProperty.RequestGroups)
                {
                    foreach (var res in record.Records)
                    {
                        if (res.Record.RefVal == item)
                            DocumentViewModel.RefVal = res.Record.KeyVal;
                        break;
                    }
                }

                _lstDocuments = new ObservableCollection<DocumentViewModel>(await _documentListModel.GetFilteredDocumentList());
                Lstvw_Documents.ItemsSource = _lstDocuments;
            }
            else
            {
                _lstDocuments = new ObservableCollection<DocumentViewModel>(await _documentListModel.GetDocumentList());
                Lstvw_Documents.ItemsSource = _lstDocuments;
            }
        }
        private void OnDocumentAdded(OnSiteDocument document)
        {
            try
            {
                _lstDocuments.Add(new DocumentViewModel
                {
                    DocumentName = document.FileName,
                    SriDocument = document,
                    DocumentSize = (document.Size.HasValue) ? document.Size.Value.ToByteSizeString() : "",
                    ArrowImage = "chevron.png",
                    BackgroundColor = Color.White,
                    IsBtnVisible = false,
                    IsDownloaded = true,
                    PendingImage = DocumentViewModel.PENDINGIMAGE,
                    //date = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private async void OnDownload(object sender, EventArgs e)
        {
            try
            {
                if (Reachability.InternetConnectionStatus() != ReachabilityNetworkStatus.NotReachable)
                {
                    var objSender = (Button)sender;
                    DocumentViewModel documentListViewModel = (DocumentViewModel)objSender.BindingContext;
                    if (documentListViewModel != null)
                    {
                        int selectedItemIndex = _lstDocuments.IndexOf(documentListViewModel);
                        documentListViewModel.DownloadState = DocumentDownloadState.Inprogress;
                        _lstDocuments[selectedItemIndex] = documentListViewModel;
                        if (await AppData.PropertyModel.GetDocument(documentListViewModel.SriDocument))
                        {
                            documentListViewModel.DownloadState = DocumentDownloadState.Completed;
                        }
                        else
                        {
                            documentListViewModel.DownloadState = DocumentDownloadState.Begin;
                        }
                        if (_lstDocuments.Count != 0)
                            _lstDocuments[selectedItemIndex] = documentListViewModel;
                    }
                }
                else
                {
                    await SplitView.DisplayAlert("No network", " Check network before downloading document", "OK",null);
                }

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void OnStopProcess()
        {
            try
            {
                foreach (var item in _lstDocuments)
                {
                    if (item.DownloadState == DocumentDownloadState.Inprogress)
                        item.DownloadState = DocumentDownloadState.Begin;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private async void Loading()
        {
            try
            {
                _lstDocuments = new ObservableCollection<DocumentViewModel>(await _documentListModel.GetDocumentList());
                Lstvw_Documents.ItemsSource = _lstDocuments;
                if (_lstDocuments.Count == 0)
                {
                    lbl_nodata.IsVisible = true;
                    Sl_Documents.IsVisible = false;
                }
                else
                {
                    Sl_Documents.IsVisible = true;
                    lbl_nodata.IsVisible = false;
                }
                LoadPickerData();

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private void LoadPickerData()
        {
            PkrRefVal.Items.Add("All");
            foreach (var record in AppData.PropertyModel.SelectedProperty.RequestGroups)
            {
                foreach (var res in record.Records)
                {
                    PkrRefVal.Items.Add(res.Record.RefVal);
                }
            }
            PkrRefVal.SelectedIndex = 0;
        }

        private async void OnTapDocumentItem(object sender, EventArgs e)
        {
            try
            {
				DocumentViewModel model = (DocumentViewModel)Lstvw_Documents.SelectedItem;
                if (model.IsDownloaded)
                {
                    await DocumentView(model.DocumentName);

			
                }
				((ListView)sender).SelectedItem = null;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private async Task DocumentView(string document)
        {
            try
            {
                string docUrl = await FileSystem.GetFullPath(document);
                Debug.WriteLine(docUrl);
                await DependencyService.Get<IFileViewer>().OpenFile(docUrl);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }

        }

        /// Name		OnSort
		/// ------------------------------------------------------------------------------------------------
		/// <summary> Handles Sorting of document in Document List.
		/// </summary>
		/// <param name="sender">  </param>
		/// <param name="e">event arguments</param>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		private async void OnSort(object sender, EventArgs e)
        {
            if(PkrRefVal.SelectedIndex ==0)
            _lstDocuments = new ObservableCollection<DocumentViewModel>(await _documentListModel.GetDocumentList());
            else
            {
               FilterList();
            }
            _sortDocType = !_sortDocType;

            if (_sortDocType)
            {
                Btn_Sort.Text = "Document Type";
                _lstDocuments = new ObservableCollection<DocumentViewModel>(_lstDocuments.OrderBy(x => x.ImagePath).ToList());
                Lstvw_Documents.ItemsSource = _lstDocuments;
            }
            else
            {
                Btn_Sort.Text = "Date/Index";
                Lstvw_Documents.ItemsSource = _lstDocuments;
            }
            if (!string.IsNullOrEmpty(SrchBar.Text))
                SrchBar.Text = null;

        }

        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
