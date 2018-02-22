using ServiceRequest.AppContext;
using ServiceRequest.Pages;
using ServiceRequest.Views.PopUp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ServiceRequest.Views.ViewCells
{
    public partial class ParagraphViewCell
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private variable and Properties

        private GroupedListView _groupedListPopupView;
        private TypedParagraph _typedPara;

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Variables and Properties
        public static Action CellTextUpdated;
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        public ParagraphViewCell()
        {
            try
            {
                InitializeComponent();
                var typeGestureRecognizer = new TapGestureRecognizer();
                Lbl_Type.GestureRecognizers.Add(typeGestureRecognizer);

                typeGestureRecognizer.Tapped += (sender, args) =>
                {
                    var objSender = sender as Label;
                    AppContext.AppContext.IsTypeList = true;
                    UpdateCellText(sender, ParagraphView.ParaViewModel.TypeList, 250, 150);
                    AppContext.AppContext.IsTypeList = false;
                    if (objSender != null) objSender.PropertyChanged += Lbl_TypePropertyChanged;
                };

                var paragraphGestureRecognizer = new TapGestureRecognizer();
                Lbl_Paragraph.GestureRecognizers.Add(paragraphGestureRecognizer);

                paragraphGestureRecognizer.Tapped += (sender, args) =>
                {
                    var objSender = sender as Label;
                    int width = (int)(SplitView.Instace().Width * 0.4);
                    AppContext.AppContext.IsParalist = true;
                    UpdateCellText(sender, ParagraphView.ParaViewModel.GetParaDescList(ParagraphView.ParaViewModel.ParagraphList), 100, width);
                    AppContext.AppContext.IsParalist = false;
                    if (objSender != null) objSender.PropertyChanged += Lbl_ParagraphPropertyChanged;
                };
                var imgDeleteGestureRecognizer = new TapGestureRecognizer();
                Img_Delete.GestureRecognizers.Add(imgDeleteGestureRecognizer);

                imgDeleteGestureRecognizer.Tapped += OnDelete;
                _typedPara = null;
                BindingContextChanged += OnBindingContextChanged;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions

        private void UpdateCellText(object sender, List<KeyValuePair<string, string>> listData, int height, int width)
        {
            try
            {
                var objSender = sender as Label;
                _groupedListPopupView = new GroupedListView(listData, sender);
                
                height = listData.Count * 85 + 5;

                if (height > 450)
                {
                    height = 450;
                }
                Task.Delay(300);
				if (Device.OS == TargetPlatform.iOS)
				{
					ParagraphView.CurrentInstance.Drawpopup();
					ParagraphView.RelativePopup.ShowInstacePopup(ParagraphView.CurrentInstance, ParagraphView.RelativePopup,
					                                             _groupedListPopupView, objSender, width, height, true, "");
					ParagraphView.RelativePopup._triangleImage.Source = "";
				}
				else
				{
					VisitActionDetailsPage.RelativePopup.ShowPopupRelative(_groupedListPopupView, objSender, width, height, true, "");
					VisitActionDetailsPage.RelativePopup._triangleImage.Source = "";
				}
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            try
            {
                var objsender = (TypedParagraph)BindingContext;
                if (objsender != null)
                    _typedPara = objsender;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private void Lbl_TypePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
				if (e.PropertyName.Equals("Text") && _typedPara !=null)
                {
                    ParagraphView.ParaViewModel.SetType(_groupedListPopupView.SelectedIndex(), ParagraphView.CurrentInstance.SelectedIndex(_typedPara));
                    CellTextUpdated?.Invoke();
                    Img_Delete.Source = "bin.png";
                }
                Img_Delete.IsVisible = true;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private void Lbl_ParagraphPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
				if (e.PropertyName.Equals("Text"))
                {
                    ParagraphView.ParaViewModel.SetParagraph(_groupedListPopupView.SelectedIndex(), ParagraphView.CurrentInstance.SelectedIndex(_typedPara));
                    CellTextUpdated?.Invoke();
                    Img_Delete.Source = "bin.png";
                }
                // Img_Delete.IsVisible = true;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private void OnDelete(object sender, EventArgs e)
        {
            try
            {
               
                if (_typedPara != null)
                {
                    if ((_typedPara.Type != "Type") || (_typedPara.Paragraph.ParagraphText != "Paragraph"))
                    {
                        ParagraphView.Delete.Invoke(_typedPara);
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                // Debug.WriteLine(" Error : {0}", ex.Message);
            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
