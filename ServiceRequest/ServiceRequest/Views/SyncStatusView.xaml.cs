using System;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views
{
    public partial class SyncStatusView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SyncStatusView"/> class.
        /// </summary>
        public SyncStatusView()
        {
            try
            {
                InitializeComponent();
                Acin_Syncing.IsRunning = false;
                Acin_Syncing.IsVisible = false;
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
        /// Name		SyncStarted
        /// 
        /// <summary>
        /// Sets text when sync in progress.
        /// </summary>
        public void SyncStarted()
        {
            try
            {
                Lbl_SyncStatus.Text = "Syncing...";
                Lbl_SyncDateTime.IsVisible = false;
                SetIndicator(true);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SyncStopped
        /// 
        /// <summary>
        /// Sets text when sync is completed.
        /// </summary>
		/// ------------------------------------------------------------------------------------------------
        ///
        public void SyncStopped(bool success)
        {
            try
            {
                Lbl_SyncStatus.Text = "Last Sync";
                Lbl_SyncDateTime.IsVisible = true;
                SetIndicator(false);
                if (success)
                {
                    AppData.LastSync = DateTime.Now;
                    UpdateDateTime();
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UpdateDateTime
        /// 
        /// <summary>
        /// Updates the data and time for last sync.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        ///
        public void UpdateDateTime()
        {
            try
            {
                if (AppData.LastSync.HasValue)
                {
                    var time = AppData.LastSync.Value.ToString("HH:mm");
                    if (AppData.LastSync.Value.Date == DateTime.Now.Date)
                        Lbl_SyncDateTime.Text = string.Format("today @ {0}", time);
                    else if (AppData.LastSync.Value.AddDays(1).Date == DateTime.Now.Date)
                        Lbl_SyncDateTime.Text = string.Format("yesterday @ {0}", time);
                    else if (AppData.LastSync.Value.AddDays(7).Date > DateTime.Now.Date)
                        Lbl_SyncDateTime.Text = string.Format("{0} @ {1}", AppData.LastSync.Value.DayOfWeek, time);
                    else
                        Lbl_SyncDateTime.Text = string.Format("{0} @ {1}", AppData.LastSync.Value.ToString("dd MMM"), time);
                }
                else
                    Lbl_SyncDateTime.Text = "never";
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SetIndicator
        /// 
        /// <summary>
        /// Toggles between show and hide of activity indicator.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions
        private void SetIndicator(bool isLoading)
        {
            try
            {
                Acin_Syncing.IsVisible = Acin_Syncing.IsRunning = isLoading;
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
