using System;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using PopUpSample;
using Xamarin.Forms;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views.PopUp
{
    public partial class EnvironmentsView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        public EnvironmentsView(PopupLayouts parentPopup)
        {
            try
            {
                InitializeComponent();
                // Demo Environment Tapped.
                TapGestureRecognizer tapDemo = new TapGestureRecognizer();
                tapDemo.Tapped += (s, e) =>
                {
                    parentPopup.DismisPopup();
                    AppData.Environment = OnSiteEnvironments.Sales;
                };
                Gl_Demo.GestureRecognizers.Add(tapDemo);
                // Test Environment Tapped.
                TapGestureRecognizer tapTest = new TapGestureRecognizer();
                tapTest.Tapped += (s, e) =>
                {
                    parentPopup.DismisPopup();
                    AppData.Environment = OnSiteEnvironments.Staging;
                };
                Gl_Test.GestureRecognizers.Add(tapTest);
                // Dev Environment Tapped.
                TapGestureRecognizer tapDev = new TapGestureRecognizer();
                tapDev.Tapped += (s, e) =>
                {
                    parentPopup.DismisPopup();
                    AppData.Environment = OnSiteEnvironments.Dev;
                };
                Gl_Dev.GestureRecognizers.Add(tapDev);
                // Live Environment Tapped.
                TapGestureRecognizer tapLive = new TapGestureRecognizer();
                tapLive.Tapped += (s, e) =>
                {
                    parentPopup.DismisPopup();
                    AppData.Environment = OnSiteEnvironments.Production;
                };
                Gl_Live.GestureRecognizers.Add(tapLive);
                // QA Environment Tapped.
                TapGestureRecognizer tapQa = new TapGestureRecognizer();
                tapQa.Tapped += (s, e) =>
                {
                    parentPopup.DismisPopup();
                    AppData.Environment = OnSiteEnvironments.QA;
                };
                Gl_QA.GestureRecognizers.Add(tapQa);
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
