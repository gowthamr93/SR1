using System;
using System.Threading.Tasks;
using Plugin.Connectivity;
using ServiceRequest.Pages;
using ServiceRequest.Views;
using Xamarin.Forms;

namespace ServiceRequest.AppContext
{
    public class Reachability
    {
        /// ------------------------------------------------------------------------------------------------
        #region Static Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// <summary>  It is used to check the network status and update it.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 
        static Reachability()
        {
            try
            {
                HostName = "www.google.com";
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Static Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		HostName
        /// 
        /// <summary>	Gets and sets the HostName.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string HostName
        {
            get;
            set;
        }

        public static Plugin.Connectivity.Abstractions.ConnectionType CurrentConnection;
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		InternetConnectionStatus
        /// 
        /// <summary>	Gets the Network status for the internet connection.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static ReachabilityNetworkStatus InternetConnectionStatus()
        {           
                foreach (var item in CrossConnectivity.Current.ConnectionTypes)
                {
                    CurrentConnection = item;
                    if (!CrossConnectivity.Current.IsConnected)
                        return ReachabilityNetworkStatus.NotReachable;
                    else if ((CurrentConnection == Plugin.Connectivity.Abstractions.ConnectionType.Desktop) ||
                             (CurrentConnection == Plugin.Connectivity.Abstractions.ConnectionType.Cellular))
                        return ReachabilityNetworkStatus.ReachableViaCarrierDataNetwork;

                    else if (CurrentConnection == Plugin.Connectivity.Abstractions.ConnectionType.WiFi)
                        return ReachabilityNetworkStatus.ReachableViaWiFiNetwork;
                    else
                        return ReachabilityNetworkStatus.NotReachable;                
            }
            return ReachabilityNetworkStatus.NotReachable;
        }
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnNetworkChanged
        /// 
        /// <summary>	Executes when the network type is changed.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static void OnNetworkChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            try
            {
                LoginPage login = new LoginPage();
                SplitView.Instace().UpdateStatus();
                login.MakeReady();
                if ((Application.Current.MainPage.ToString() == "ServiceRequest.Pages.SplitView") && (InternetConnectionStatus() == ReachabilityNetworkStatus.NotReachable))
                {
                    DocumentListView.StopProcess?.Invoke();
                    //PropertySummary.ShowMatchAddress.Invoke(false);
                }
                else
                {
                    //PropertySummary.ShowMatchAddress.Invoke(true);
                }
                FullMapView.NewCaseAddView?.LoadButton();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		IsHostReachable
        /// 
        /// <summary>	Is the host reachable with the current network configuration.
        /// </summary>
        /// <param name="host">			The host.</param>
        /// 
        /// <returns>	If it's reachable.
        /// </returns>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static async Task<bool> IsHostReachable(string host)
        {
            try
            {
                if (string.IsNullOrEmpty(host))
                    return false;
                return await CrossConnectivity.Current.IsReachable(host);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
