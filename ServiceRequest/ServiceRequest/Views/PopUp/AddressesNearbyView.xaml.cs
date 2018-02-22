using Idox.LGDP.Apps.ServiceRequest.Client;
using Idox.LGDP.Apps.ServiceRequest.Client.Models;
using ServiceRequest.Models;
using ServiceRequest.Pages;
using ServiceRequest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views.PopUp
{
    public partial class AddressesNearbyView : ContentView
    {
        private AddNewCaseView AddNewCaseInstance { get; set; }
		public const int CellHeight = 40;
		public static int Groupedcount;
        public AddressesNearbyView(AddNewCaseView addCaseInstance, SRiUtilityAddresses addresses)
        {
			try
			{
				InitializeComponent();
				AppContext.AppContext.IsTypeList = true;
				var fAddresses = addresses.Addresses.OrderBy(x => x.FormattedAddress()).Select(x => new KeyValuePair<string, string>(x.FormattedAddress(), x.FormattedAddress())).ToList().ToGroupedList();
				AppContext.AppContext.IsTypeList = false;
				AddNewCaseInstance = addCaseInstance;
				Lstvw_Addresses.ItemsSource = fAddresses;
				Groupedcount = fAddresses.Count;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
			}
        }

        private async void AddressesItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = e.Item as GroupedListModel;
            SplitView.PopupContent.DismisPopup();
            AddNewCaseInstance.UpdateFieldAddress(selectedItem.Code);
        }
    }
}
