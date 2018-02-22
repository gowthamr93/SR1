using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Models;
using Plugin.Geolocator.Abstractions;
using Idox.LGDP.Apps.Common.OnSite;
using ServiceRequest.AppContext;

namespace ServiceRequest.ViewModels
{
    public class AddNewCaseViewModel : AddNewCaseModel
    {
        public AddNewCaseViewModel(string key = null)
        {
			try
			{
				if (NewCaseIsOffline)
				{

				}
				if (NewCaseIsOnline)
				{

				}
				ExistCaseAddress = true;
				ExistCaseDetails = true;
				ExistCaseRecievedDate = true;
				ExistCaseRecievedTime = true;
				if (key == "Existing")
				{
					ExistCaseAddress = false;
					ExistCaseDetails = false;
					ExistCaseRecievedDate = false;
					ExistCaseRecievedTime = false;
				}
			}
				catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
			}
        }
    }
}
