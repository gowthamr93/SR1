using System;
using System.Collections.Generic;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Pages;
using ServiceRequest.Views;
using ServiceRequest.Views.PopUp;
using Xamarin.Forms.Maps;

namespace ServiceRequest.Custom
{
    public class CustomMap : Map
    {
        /// ------------------------------------------------------------------------------------------------
        /// <summary>  Custom Controller for the Map..
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<CustomPin> CustomPins { get; set; }

        public Action<List<CustomPin>> LoadPins;

        public Action ClearPins;

        public string MapTypeStyle { get; set; }
        public Action DestroyMap;

        public async void GetPositionOnClick(double latitude, double longitude)
        {
            if (FullMapView.AddNewCasePointOnMapView != null && FullMapView.AddNewCasePointOnMapView.NewCaseAddView == null && AppData.PropertyModel.SelectedProperty == null)
            {
                if (!AppContext.AppContext.LocationSelected)
                {
                    AppContext.AppContext.LocationSelected = true;
                    if (await SplitView.DisplayAlert("Location Selected", "Do you want to create a new Service Record in this location?", "Yes", "No"))
                    {
                        FullMapView.AddNewCasePointOnMapView.PostionSelected(latitude, longitude);
                    }
                    else
                    {
                        AppContext.AppContext.LocationSelected = false;
                        AppContext.AppContext.MapView?.ClearPin();
                    }
                }
            }
            else if (AddNewCaseView.EditLocInstance != null)
            {
                if (!AppContext.AppContext.LocationSelected)
                {
                    AppContext.AppContext.LocationSelected = true;
                    if (await SplitView.DisplayAlert("Location Selected", "Do you want to move the Service Record to this location?", "Yes", "No"))
                    {
                        AddNewCaseView.EditLocInstance.PostionSelected(latitude, longitude);
                    }
                    else
                    {
                        AppContext.AppContext.LocationSelected = false;
                        AppContext.AppContext.MapView?.ClearPin();
                    }
                }
            }
        }
		public Action DisposeMapControl;
    }
}