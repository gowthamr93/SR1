using System;
using ServiceRequest.ViewModels;
using Xamarin.Forms;
using ServiceRequest.AppContext;
using ServiceRequest.Pages;
using Idox.LGDP.Apps.ServiceRequest.Client;
using Idox.LGDP.Apps.Common.OnSite;
using System.Collections.Generic;
using System.Linq;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Pages;
using ServiceRequest.Views.ViewCells;
using Idox.LGDP.Apps.Common.OnSite;

namespace ServiceRequest.Views
{
    public partial class RecordSummaryStackLayout
    {
        Label Lbl_Contacts;
        public AddNewCaseView UpdateNewCase { get; set; }
        RecordSummaryList ContactsList { get; set; }
        /// ------------------------------------------------------------------------------------------------
        #region public constructor
        public RecordSummaryStackLayout(object classname)
        {
            try
            {
                InitializeComponent();
                ContactsList = new RecordSummaryList();
                var a = classname.GetType();
                //To Check the recordsummary or customerlist data
                if (a.Name == "CreateRecordList")
                {
                    var recordSummary = (CreateRecordList)classname;
                    BindingContext = recordSummary;
                    Lstvw_Main.ItemsSource = recordSummary.Details;
                    if (AppData.PropertyModel.SelectedProperty.Status == SyncStatus.New)
                    {
                        Gl_Footer.IsVisible = true;
                        Sl_Main.HeightRequest = recordSummary.Details.Count * Device.OnPlatform<int>(36, 35, 35) + 40;
                    }
                    else
                    {
                        Sl_Main.HeightRequest = recordSummary.Details.Count * Device.OnPlatform<int>(36, 35, 35);
                        Gl_Footer.IsVisible = false;
                    }
                    TapGestureRecognizer tapFilter = new TapGestureRecognizer();
                    tapFilter.Tapped += (s, e) =>
                    {
                        EditRecordTapped();
                    };
                    Gl_Footer.GestureRecognizers.Add(tapFilter);
                }

                if (a.Name == "CreateCustomerList")
                {
                    Gl_Footer.IsVisible = false;
                    var customersDetails = (CreateCustomerList)classname;
                    BindingContext = customersDetails;
                    Lstvw_Main.ItemsSource = customersDetails.Details;
                    string address = null;
                    foreach (var item in customersDetails.Details)
                    {
                        if (item.Key == "Address")
                        {
                            address = item.Value;
                        }
                    }
                    if (!(customersDetails.Details.Count > 0))
                    {
                        Lstvw_Main.HeightRequest = Device.OnPlatform<double>(8, 8, 8);
                    }
                    else if (string.IsNullOrWhiteSpace(address))
                    {
                        Lstvw_Main.HeightRequest = customersDetails.Details.Count * Device.OnPlatform<int>(36, 35, 35);
                        if (Device.OS == TargetPlatform.Android)
                        {
                            Sl_Main.HeightRequest = Lstvw_Main.HeightRequest + customersDetails.Details.Count(x => x.Key == "Customer type") * Device.OnPlatform<int>(45, 40, 52);
                        }
                    }
                    else
                    {
                        Lstvw_Main.HeightRequest = customersDetails.Details.Count * Device.OnPlatform<int>(36, 35, 35) + Device.OnPlatform<int>(55, 0, 55);
                        if (Device.OS == TargetPlatform.Android)
                        {
                            Sl_Main.HeightRequest = Lstvw_Main.HeightRequest + customersDetails.Details.Count(x => x.Key == "Customer type") * Device.OnPlatform<int>(45, 40, 52);
                        }
                    }
                    if (AppData.PropertyModel.SelectedProperty.Status == SyncStatus.New)
                    {
                        Lbl_Contacts = new Label
                        {
                            //FontSize = Device.OnPlatform (Device.GetNamedSize(NamedSize.Small, typeof(Label)), Device.GetNamedSize(NamedSize.Small, typeof(Label)), Device.GetNamedSize(NamedSize.Micro, typeof(Label))),
                            VerticalOptions = LayoutOptions.Start,
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            HorizontalTextAlignment = TextAlignment.Start,
                            Margin = new Thickness(20, 0, 0, 10),
                            TextColor = Styles.MainAccent,
                        };
                        Sl_Main.Children.Add(Lbl_Contacts);
                        TapGestureRecognizer CustomerTapped = new TapGestureRecognizer();
                        CustomerTapped.Tapped += CustomerTapped_Tapped;
                        Lbl_Contacts.GestureRecognizers.Add(CustomerTapped);
                        ContactsList = customersDetails.Details;
                        Lbl_Contacts.Text = ContactsList.Count <= 0 ? " + Add Contact Details" : "Edit Contact Details";
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private async void EditRecordTapped()
        {
            //Open Exist Add Case View
            await SplitView.Instace().PushRightContent(FullMapView.NewCaseAddView = new AddNewCaseView());
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// 
        #region private function
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ContactList
        /// 
        /// <summary>	Update the ContactList in Add/Edit Contact details.
        /// </summary>
        /// <param name="detailsList">		Contact details to dispaly.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        public void ContactList(RecordSummaryList detailsList)
        {
            try
            {
                ContactsList = detailsList;
                Lstvw_Main.ItemsSource = ContactsList;
                string address = null;
                foreach (var item in ContactsList)
                {
                    if (item.Key == "Address")
                    {
                        address = item.Value;
                    }
                }
                if (address != null || address != "")
                {
                    Lstvw_Main.HeightRequest = ContactsList.Count * Device.OnPlatform<int>(36, 35, 35) + Device.OnPlatform<int>(55, 0, 55);
                }
                else
                    Lstvw_Main.HeightRequest = detailsList.Count * Device.OnPlatform<int>(36, 35, 35);
                Lbl_Contacts.Text = "Edit Contact Details";
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// <summary>
        /// To Add/Edit Contact details for New Case
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        private void CustomerTapped_Tapped(object sender, EventArgs e)
        {
            try
            {
                ContactDetailsView.UpdateContact = ContactList;
                SplitView.CenterPopupContent.ShowPopupCenter(new ContactDetailsView(ContactsList), 0.5);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// <summary>
        /// To make the selected list null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnSelectList(object sender, EventArgs e)
        {
            try
            {
                ((ListView)sender).SelectedItem = null;
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
