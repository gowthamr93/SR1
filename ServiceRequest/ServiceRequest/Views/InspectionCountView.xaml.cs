using System;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views
{
    public partial class InspectionCountView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private variables and properties
        private int _count;

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region public constructor
        public InspectionCountView()
        {
            try
            {
                InitializeComponent();
                Update();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region public functions
        ///
        /// Name                Update
        /// 
        /// <summary>           Updates the count of the Records.
        /// </summary>
        /// 
        public void Update()
        {
            try
            {
                _count = 0;
                foreach (var pMap in AppData.PropertyModel.Mappings)
                    foreach (var iMap in pMap.ActiveExpanded)
                        if (iMap.Type == MappingType.Record)
                            _count++;
                Lbl_Count.Text = _count.ToString();
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
