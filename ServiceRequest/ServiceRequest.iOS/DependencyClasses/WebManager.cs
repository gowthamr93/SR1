using ServiceRequest.iOS;
using ServiceRequest.DependencyInterfaces;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(WebManager))]
namespace ServiceRequest.iOS
{
	public class WebManager : IWebManager
	{
		public void ClearCookies(string url)
		{
			// string yahooCookies = CookieManager.Instance.GetCookie(url);
			//CookieManager.Instance.RemoveAllCookie();
			NSHttpCookieStorage CookieStorage = NSHttpCookieStorage.SharedStorage;

			foreach (var cookie in CookieStorage.Cookies)
			{
				CookieStorage.DeleteCookie(cookie);
			}
		}
	}
}