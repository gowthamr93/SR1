using System;
using Windows.Web.Http;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.UWP.DependencyClasses;
using ServiceRequest.AppContext;

[assembly: Xamarin.Forms.Dependency(typeof(WebManager))]
namespace ServiceRequest.UWP.DependencyClasses
{
    class WebManager : IWebManager
    {
        public void ClearCookies(string url)
        {
            try
            {
                Windows.Web.Http.Filters.HttpBaseProtocolFilter myFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
                var cookieManager = myFilter.CookieManager;
                HttpCookieCollection myCookieJar = cookieManager.GetCookies(new Uri(url));
                foreach (HttpCookie cookie in myCookieJar)
                {
                    cookieManager.DeleteCookie(cookie);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
    }
}
