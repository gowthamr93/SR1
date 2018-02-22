using System.Collections.Generic;
using Android.Content;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Custom;
using ServiceRequest.Droid.CustomRenderer;
using ServiceRequest.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System;
using ServiceRequest.AppContext;
using System.Reflection;
using System.Linq;
using Android.Views;
using ListView = Android.Widget.ListView;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(SrTableView), typeof(CustomTableViewRenderer))]
namespace ServiceRequest.Droid.CustomRenderer
{
    public class CustomTableViewRenderer : TableViewRenderer
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// 
        private List<SRiActionParagraph> Items { get; set; }
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Protected Functions
        protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
        {
            try
            {
                base.OnElementChanged(e);
                Items = AppData.PropertyModel.SelectedAction.Action.Paragraphs;
                Control.ItemLongClick += (s, args) =>
                {
                    ClipData data = ClipData.NewPlainText("List", args.Position.ToString());
                    if (VisitActionDetailsPage.CurrentInstance.IsEditable)
                    {
                        if (VisitActionDetailsPage.CurrentInstance.SingleTypeCode != null)
                        {
                            if (!VisitActionDetailsPage.CurrentInstance.SingleTypeCode.Contains(Items[args.Position - 1].ParagraphType))
                            {
                                CustomDragShadowBuilder myShadownScreen = new CustomDragShadowBuilder(args.View);
                                args.View.StartDrag(data, myShadownScreen, null, 0);
                            }
                        }

                    }
                };
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        protected override TableViewModelRenderer GetModelRenderer(ListView listView, TableView view)
        {
            return new MyTableViewModelRenderer(Context, listView, view);
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        class MyTableViewModelRenderer : TableViewModelRenderer
        {
            //Info on private base class method
            private static readonly MethodInfo CellForPosition = typeof(TableViewModelRenderer)
                .GetMethod(nameof(GetCellForPosition), BindingFlags.NonPublic | BindingFlags.Instance, null, CallingConventions.Any,
                    new[] { typeof(int), typeof(bool).MakeByRefType(), typeof(bool).MakeByRefType() }, null);


            private readonly TableView _view;

            #region Public Function
            public MyTableViewModelRenderer(Context context, ListView listView, TableView view)
                : base(context, listView, view)
            {
                _view = view;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {

                var cellView = base.GetView(position, convertView, parent);
                try
                {
                    if (_view.Intent == TableIntent.Data)
                    {
                        var @params = new object[] { position, false, false };
                        var cell = CellForPosition.Invoke(this, @params) as Cell;   //Reflection to base method to determine cell type

                        bool shouldHide = (bool)@params[1] /*out isHeader*/
                                          && string.IsNullOrEmpty((cell as TextCell)?.Text);

                        //With view recycling, we need to ensure Visibility is set to the proper value
                        var visibility = shouldHide
                            ? ViewStates.Gone
                            : ViewStates.Visible;

                        cellView.Visibility = visibility;

                        var cellGroup = cellView as ViewGroup;
                        if (cellGroup != null)
                        {
                            foreach (var child in Enumerable.Range(0, cellGroup.ChildCount).Select(cellGroup.GetChildAt))
                                child.Visibility = visibility;
                        }
                    }

                    return cellView;
                }
                catch (Exception ex)
                {
                    LogTracking.LogTrace(ex.ToString());
                    return cellView;
                }
            }
            #endregion
        }
    }
}