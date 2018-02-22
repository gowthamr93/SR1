using System.Collections.Generic;
using ServiceRequest.Pages;
using Xamarin.Forms;

namespace ServiceRequest
{
	public static class PageNavigation
	{
		public static Stack<ContentPage> _mainPageStack;
		#region Static construtor
		static PageNavigation()
		{
			_mainPageStack = new Stack<ContentPage>();
		}
		#endregion

		#region Public Functions.

		/// <summary>
		/// 
		/// Pushes the Page to the stack.
		/// </summary>
		/// <param name="toPage"></param>
		public static void PushMainPage(ContentPage toPage)
		{
			if (Device.OS == TargetPlatform.iOS)
			{
				_mainPageStack.Push(toPage);
				Application.Current.MainPage = toPage;
			}
			else
			{
                _mainPageStack.Push(toPage);
                Application.Current.MainPage = toPage;
            }

		}
		/// <summary>
		/// To get the current Page instance.
		/// </summary>
		/// <returns></returns>
		public static ContentPage GetCurrentPage()
		{
			return _mainPageStack.Peek();
		}

		/// <summary>
		/// To Pop the Current Page from the stack.
		/// </summary>
		public static void PopMainPage()
		{
			if (Device.OS == TargetPlatform.iOS)
			{
				_mainPageStack.Pop();
				Application.Current.MainPage = _mainPageStack.Peek();
			}
			else
			{
                _mainPageStack.Pop();
                Application.Current.MainPage = _mainPageStack.Peek();
            }
		}

		/// <summary>
		/// To Get the current stack.
		/// </summary>
		public static Stack<ContentPage> GetStack()
		{
			return _mainPageStack;
		}

		#endregion
	}
}
