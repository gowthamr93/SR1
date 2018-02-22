using System;
using Android.Webkit;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.Droid.DependencyClasses;
using ServiceRequest.AppContext;

[assembly: Xamarin.Forms.Dependency(typeof(WebManager))]
namespace ServiceRequest.Droid.DependencyClasses
{
    public class WebManager : IWebManager
    {
        /// ------------------------------------------------------------------------------------------------
        /// <summary>  Clears the cookies from the Application.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 

        public void ClearCookies(string url)
        {
            try
            {
                CookieManager.Instance.GetCookie(url);
                CookieManager.Instance.RemoveAllCookie();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
    }
}
