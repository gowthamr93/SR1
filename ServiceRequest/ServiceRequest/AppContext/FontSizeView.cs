using System;
using Xamarin.Forms;

namespace ServiceRequest.AppContext
{
    ///
    /// ------------------------------------------------------------------------------------------------
    /// <summary>  It contains all the fontsize for the specific platform
    /// </summary>
    /// ------------------------------------------------------------------------------------------------
    ///
    public class FontSizeView
    {
        //Micro
        public static readonly Double CustomFontSizeMiMi =
              Device.OnPlatform<Double>(0,
              Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
              Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
        public static readonly Double CustomFontSizeMiSm =
             Device.OnPlatform<Double>(0,
             Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
             Device.GetNamedSize(NamedSize.Small, typeof(Label)));
        public static readonly Double CustomFontSizeMiMe =
             Device.OnPlatform<Double>(0,
             Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
             Device.GetNamedSize(NamedSize.Medium, typeof(Label)));

        public static readonly Double CustomFontSizeMiLa =
             Device.OnPlatform<Double>(0,
             Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
             Device.GetNamedSize(NamedSize.Large, typeof(Label)));

        //Small
        public static readonly Double CustomFontSizeSmMi =
            Device.OnPlatform<Double>(0,
            Device.GetNamedSize(NamedSize.Small, typeof(Label)),
            Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
        public static readonly Double CustomFontSizeSmSm =
            Device.OnPlatform<Double>(0,
            Device.GetNamedSize(NamedSize.Small, typeof(Label)),
            Device.GetNamedSize(NamedSize.Small, typeof(Label)));
        public static readonly Double CustomFontSizeSmMe =
            Device.OnPlatform<Double>(0,
            Device.GetNamedSize(NamedSize.Small, typeof(Label)),
            Device.GetNamedSize(NamedSize.Medium, typeof(Label)));
        public static readonly Double CustomFontSizeSmLa =
            Device.OnPlatform<Double>(0,
            Device.GetNamedSize(NamedSize.Small, typeof(Label)),
            Device.GetNamedSize(NamedSize.Large, typeof(Label)));


        //Medium
        public static readonly Double CustomFontSizeMeMi =
            Device.OnPlatform<Double>(0,
            Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
        public static readonly Double CustomFontSizeMeSm =
            Device.OnPlatform<Double>(0,
            Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            Device.GetNamedSize(NamedSize.Small, typeof(Label)));
        public static readonly Double CustomFontSizeMeMe =
            Device.OnPlatform<Double>(0,
            Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            Device.GetNamedSize(NamedSize.Medium, typeof(Label)));

        public static readonly Double CustomFontSizeMeLa =
            Device.OnPlatform<Double>(0,
            Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            Device.GetNamedSize(NamedSize.Large, typeof(Label)));

        //Large
        public static readonly Double CustomFontSizeLaMi =
            Device.OnPlatform<Double>(0,
            Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
        public static readonly Double CustomFontSizeLaSm =
            Device.OnPlatform<Double>(0,
            Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            Device.GetNamedSize(NamedSize.Small, typeof(Label)));
        public static readonly Double CustomFontSizeLaMe =
            Device.OnPlatform<Double>(0,
            Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            Device.GetNamedSize(NamedSize.Medium, typeof(Label)));
        public static readonly Double CustomFontSizeLaLa =
            Device.OnPlatform<Double>(0,
            Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            Device.GetNamedSize(NamedSize.Large, typeof(Label)));


		//Added for ios by Praveen
		//Micro
		public static readonly Double CustomFontSizeMiMiMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeMiMiSm =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)));
		public static readonly Double CustomFontSizeMiMiMe =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)));
		public static readonly Double CustomFontSizeMiMiLa =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)));
		public static readonly Double CustomFontSizeMiSmMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeMiMeMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeMiLaMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeMiSmSm =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)));
		public static readonly Double CustomFontSizeMiMeMe =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)));
		public static readonly Double CustomFontSizeMiLaLa =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)));

		//Smalll
		public static readonly Double CustomFontSizeSmMiMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeSmMiSm =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)));
		public static readonly Double CustomFontSizeSmMiMe =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)));
		public static readonly Double CustomFontSizeSmMiLa =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)));
		public static readonly Double CustomFontSizeSmSmMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeSmSmMe =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)));
		public static readonly Double CustomFontSizeSmMeMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeSmLaMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeSmSmSm =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)));
		public static readonly Double CustomFontSizeSmMeMe =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)));
		public static readonly Double CustomFontSizeSmLaLa =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)));
		//Medium
		public static readonly Double CustomFontSizeMeMiMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeMeMiSm =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)));
		public static readonly Double CustomFontSizeMeMiMe =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)));
		public static readonly Double CustomFontSizeMeMiLa =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)));
		public static readonly Double CustomFontSizeMeSmMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeMeMeMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeMeLaMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeMeSmSm =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)));
		public static readonly Double CustomFontSizeMeMeSm =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)));

		public static readonly Double CustomFontSizeMeMeMe =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)));
		public static readonly Double CustomFontSizeMeLaLa =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)));
		//Large
		public static readonly Double CustomFontSizeLaMiMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeLaMiSm =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)));
		public static readonly Double CustomFontSizeLaMiMe =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)));
		public static readonly Double CustomFontSizeLaMiLa =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)));
		public static readonly Double CustomFontSizeLaSmMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeLaMeMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeLaLaMi =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Micro, typeof(Label)));
		public static readonly Double CustomFontSizeLaSmSm =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)));
		public static readonly Double CustomFontSizeLaMeMe =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)));
		public static readonly Double CustomFontSizeLaLaMe =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)));
		public static readonly Double CustomFontSizeLaLaLa =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)));

		public static readonly Double CustomFontSizeSmSmLa =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)));
		public static readonly Double CustomFontSizeLaLaSm =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Large, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)));

		public static readonly Double CustomFontSizeSmMeSm =
			  Device.OnPlatform<Double>(
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			  Device.GetNamedSize(NamedSize.Small, typeof(Label)));

    }
}
