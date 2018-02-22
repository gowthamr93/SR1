using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using ServiceRequest.Models;
using ServiceRequest.Pages;
using Xamarin.Forms;

namespace ServiceRequest.Views.ViewCells
{
    public partial class InspectionCellViewCell : ViewCell
    {
        public InspectionCellViewCell()
        {
            InitializeComponent();
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		OnBindingContextChanged
        /// 
        /// <summary>
        /// Bind the updated value
        /// </summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        protected override void OnBindingContextChanged()
        {
            try
            {
                base.OnBindingContextChanged();
                var model = (SRiRequestGroupModel)BindingContext;
                if (model != null)
                {
                    if (model.UploadStatusImage == "new_upload.png")
                    {
                        if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.iOS)
                        {
                            var deleteAction = new MenuItem { Text = "Delete", IsDestructive = true };
                            deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
                            deleteAction.Clicked += (s, e) => OnDelete(model);
                            this.ContextActions.Add(deleteAction);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        public static async void OnDelete(SRiRequestGroupModel model)
        {
            try
            {
                var pMap = AppContext.AppContext.InspectionCell.PropertyMappingFromIndex(model.ShortIndex);
                foreach (var iMap in pMap.ActiveExpanded)
                {
                    if (iMap.Type == MappingType.Record)
                    {
                        var record = AppData.PropertyModel.EntityFromMapping(iMap) as SRiRecordMeta;
                        if (record != null && record.Record.Details.Equals(model.GroupDetails))
                        {
                            AppData.PropertyModel.Delete(record);
                            AppData.PropertyModel.SelectedProperty = null;
                            break;
                        }
                    }
                }
                await SplitView.HubMaster.ReloadPropertyData();
                SplitView.HubMaster.MakeDeleteVisible(false);
                SplitView.InspectionCount?.Update();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}", ex.ToString()));
            }
        }
    }
}
