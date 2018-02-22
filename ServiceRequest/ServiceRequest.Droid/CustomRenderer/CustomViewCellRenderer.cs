using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Droid.CustomRenderer;

using ServiceRequest.Custom;
using ServiceRequest.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;
using System;
using ServiceRequest.AppContext;

[assembly: ExportRenderer(typeof(SrViewCell), typeof(CustomViewCellRenderer))]
namespace ServiceRequest.Droid.CustomRenderer
{
    public class CustomViewCellRenderer : ViewCellRenderer
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Properties
        ///
        private List<SRiActionParagraph> Items { get; set; }

        public TableView TableView { get; set; }

        public ViewGroup ViewGroup { get; set; }

        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region private Properties
        private static int _firstIndex = -1;
        private static int _secondIndex = -1;

        private static int FirstIndex
        {
            get { return _firstIndex; }
            set { _firstIndex = value; }
        }

        private static int SecondIndex
        {
            get { return _secondIndex; }
            set { _secondIndex = value; }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Protected Functions
        protected override View GetCellCore(Cell item, View convertView, ViewGroup parent, Context context)
        {
            try
            {
                var cellcore = base.GetCellCore(item, convertView, parent, context);
                cellcore.Drag -= CellcoreOnDrag;
                cellcore.Drag += CellcoreOnDrag;
                return cellcore;
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
        private void CellcoreOnDrag(object sender, View.DragEventArgs args)
        {
            try
            {
                Items = AppData.PropertyModel.SelectedAction.Action.Paragraphs;
                switch (args.Event.Action)
                {
                    case DragAction.Started:
                        args.Handled = true;
                        break;
                    case DragAction.Entered:
                        args.Handled = true;
                        if (FirstIndex == -1)
                        {
                            FirstIndex = VisitActionDetailsPage.TblSection.IndexOf(Cell);
                        }
                        break;
                    case DragAction.Exited:
                        args.Handled = true;
                        break;
                    case DragAction.Drop:
                        args.Handled = true;
                        if (SecondIndex == -1)
                        {
                            SecondIndex = VisitActionDetailsPage.TblSection.IndexOf(Cell);
                        }
                        break;
                    case DragAction.Ended:
                        args.Handled = true;
                        if (FirstIndex != -1)
                        {
                            var firstType = Items[FirstIndex].ParagraphType;
                            var secondType = Items[SecondIndex].ParagraphType;
                            if (firstType == secondType)
                            {
                                var firstItem = Items[FirstIndex];
                                if (firstItem != null)
                                {
                                    Items.RemoveAt(FirstIndex);
                                    Items.Insert(SecondIndex, firstItem);
                                    VisitActionDetailsPage.CurrentInstance.RefreshList();
                                }
                            }
                        }
                        FirstIndex = -1;
                        SecondIndex = -1;
                        break;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}