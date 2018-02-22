using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using ServiceRequest.Pages;
using ServiceRequest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Idox.LGDP.Apps.Common.OnSite;
using Xamarin.Forms;

namespace ServiceRequest.Views
{
    public partial class ContactDetailsView : ContentView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        ///
        private List<SRiContact> Contact { get; set; }
        private RecordSummaryList ContactsDetails { get; set; }
        private static CreateCustomerList _customerLists { get; set; }
        private string phone = "";
        private string mobile = "";
        private string name = "";
        private OnSiteConfigCodeList ContactTypes { get; set; }
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Variables
        /// ------------------------------------------------------------------------------------------------
        public static Action<RecordSummaryList> UpdateContact { get; set; }
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Constructror
        /// ------------------------------------------------------------------------------------------------
        ///
        public ContactDetailsView(RecordSummaryList contactList)
        {
            try
            {
                InitializeComponent();
                ContactsDetails = contactList;
                Lbl_Moblie.TextChanged += Lbl_Moblie_TextChanged;
                Lbl_Phone.TextChanged += Lbl_Phone_TextChanged;
                Lbl_Name.TextChanged += Lbl_Name_TextChanged;
                if (ContactsDetails.Count > 0)
                {
                    foreach (var item in ContactsDetails)
                    {
                        if (item.Key == "Name")
                            Lbl_Name.Text = item.Value;
                        if (item.Key == "Mobile")
                            Lbl_Moblie.Text = item.Value;
                        if (item.Key == "Phone")
                            Lbl_Phone.Text = item.Value;
                        if (item.Key == "Email")
                            Lbl_Email.Text = item.Value;
                        if (item.Key == "Address")
                            EdAddress.Text = item.Value;
                    }
                }
                Contact = new List<SRiContact>();
                LoadPickerData();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void LoadPickerData()
        {
            try
            {
                ContactTypes = AppData.ConfigModel.ContactTypes(AppData.PropertyModel.SelectedProperty.Organisation);
                foreach (var res in ContactTypes.OrderBy(x=>x.Description).ToList())
                {
                    Pkr_CustomerType.Items.Add(res.Description);
                }
                if (AppData.PropertyModel.SelectedRecord.Record.Record.Customers.Count > 0)
                    Pkr_CustomerType.SelectedIndex =
                        Pkr_CustomerType.Items.IndexOf(
                            AppData.PropertyModel.SelectedRecord.Record.Record.Customers[0].CustomerTypeDescription);
                else
                    Pkr_CustomerType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }

        }



        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions and Methods
        ///
        private void OnSaveTapped(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Lbl_Name.Text))
                {
                    if (!string.IsNullOrWhiteSpace(Lbl_Email.Text))
                    {
                        if (Regex.IsMatch(Lbl_Email.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                        {
                            SaveDetails();
                        }
                        else
                        {
                            if (Device.OS == TargetPlatform.iOS)
                            {
                                DependencyService.Get<IDisplayAlertPopup>().ValidMail();
                            }
                            else
                                SplitView.Instace().DisplayAlert("Wrong E-Mail Format", "Please Enter a vaild Mail-id", "OK");
                        }
                    }
                    else
                    {
                        SaveDetails();
                    }
                }
                else
                {
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        DependencyService.Get<IDisplayAlertPopup>().Filldetails();
                    }
                    else
                        SplitView.Instace().DisplayAlert("Warning", "Please fill the name", "OK");
                }



            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void SaveDetails()
        {
            try
            {
                SRiCustomer customer = new SRiCustomer();
                if (ContactsDetails.Count == 0)
                {
                    customer.Name = Lbl_Name.Text;
                    customer.RecordKeyVal = "";
                    customer.KeyVal = "";
                    customer.Address = EdAddress.Text;
                    Contact = new List<SRiContact>()
        {
            new SRiContact()
            {
                ContactType = "PHONEH",
                ContactDescription = "Phone",
                ContactAddress = Lbl_Phone.Text,
                SRRecKeyVal = "",
                CustomerKeyVal = "",
                KeyVal = ""
            },
            new SRiContact()
            {
                ContactType = "EMAIL",
                ContactDescription = "Email",
                ContactAddress = Lbl_Email.Text,
                SRRecKeyVal = "",
                CustomerKeyVal = "",
                KeyVal = ""
            },
            new SRiContact()
            {
                ContactType = "MOBILE",
                ContactDescription = "Mobile",
                ContactAddress = Lbl_Moblie.Text,
                SRRecKeyVal = "",
                CustomerKeyVal = "",
                KeyVal = ""
            }
        };
                    customer.CustomerType = ContactTypes.Where(x => x.Description == Pkr_CustomerType.Items[Pkr_CustomerType.SelectedIndex]).Select(x => x.Code).FirstOrDefault();
                    customer.Contacts = Contact;
                   
                    AppData.PropertyModel.SelectedRecord.Record.Record.Customers.Add(customer);
                }
                else
                {
                    if (AppData.PropertyModel.SelectedRecord.Record.Record.Customers.Count > 0)
                    {
                        foreach (var item in AppData.PropertyModel.SelectedRecord.Record.Record.Customers)
                        {
                            item.Name = Lbl_Name.Text;
                            item.Address = EdAddress.Text;
                            item.CustomerType =
                                ContactTypes.Where(
                                    x => x.Description == Pkr_CustomerType.Items[Pkr_CustomerType.SelectedIndex])
                                    .Select(x => x.Code)
                                    .FirstOrDefault();
                            foreach (var contcat in item.Contacts)
                            {
                                if (contcat.ContactType == "PHONEH")
                                    contcat.ContactAddress = Lbl_Phone.Text;
                                if (contcat.ContactType == "EMAIL")
                                    contcat.ContactAddress = Lbl_Email.Text;
                                if (contcat.ContactType == "MOBILE")
                                    contcat.ContactAddress = Lbl_Moblie.Text;
                            }
                        }
                    }
                }
                AppData.PropertyModel.SelectedRecord.CreateMappings();
                Exception error;
                AppData.PropertyModel.SaveCustomer(out error);
                _customerLists = new CreateCustomerList();
                ContactsDetails = _customerLists.Details;
                // update UI in Listview                      
                SplitView.CenterPopupContent.DismisPopup();
                UpdateContact?.Invoke(ContactsDetails);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        private void OnCancelTapped(object sender, EventArgs e)
        {
            try
            {
                SplitView.CenterPopupContent.DismisPopup();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private void Lbl_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var sb = new StringBuilder();
                if (Lbl_Name.Text.Length <= 50)
                {
                    if (Regex.Match(Lbl_Name.Text, @"^[a-zA-Z_ \\s]*$").Success)
                    {
                        foreach (char c in Lbl_Name.Text)
                        {
                            sb.Append(c);
                        }
                    }
                    else
                    {
                        sb.Append(name);
                        if (Device.OS == TargetPlatform.iOS)
                        {
                            DependencyService.Get<IDisplayAlertPopup>().InvalidCharacter();
                        }
                        else
                            SplitView.Instace().DisplayAlert("Warning", "You have entered invalid character", "OK");
                    }
                }
                else
                {
                    sb.Append(name);
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        DependencyService.Get<IDisplayAlertPopup>().ExceedLength();
                    }
                    else
                        SplitView.Instace().DisplayAlert("Warning", "Length should not exceed 50 digits", "OK");
                }
                Lbl_Name.Text = sb.ToString();
                name = Lbl_Name.Text;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void Lbl_Phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var sb = new StringBuilder();
                if (Lbl_Phone.Text.Length <= 12)
                {
                    if (Regex.Match(Lbl_Phone.Text, @"^[0-9_ ]*$").Success)
                    {
                        foreach (char text in Lbl_Phone.Text)
                        {
                            sb.Append(text);
                        }
                    }
                    else
                    {
                        sb.Append(phone);
                        if (Device.OS == TargetPlatform.iOS)
                        {
                            DependencyService.Get<IDisplayAlertPopup>().OnlyDigits();
                        }
                        else
                            SplitView.Instace().DisplayAlert("Warning", "Phone numbers can not contain letters or special characters", "OK");
                    }
                }
                else
                {
                    sb.Append(phone);
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        DependencyService.Get<IDisplayAlertPopup>().Exceed12();
                    }
                    else
                        SplitView.Instace().DisplayAlert("Warning", "Length should not exceed 12 digits", "OK");
                }
                Lbl_Phone.Text = sb.ToString();
                phone = Lbl_Phone.Text;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void Lbl_Moblie_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var sb = new StringBuilder();
                if (Lbl_Moblie.Text.Length <= 12)
                {
                    if (Regex.Match(Lbl_Moblie.Text, @"^[0-9_ ]*$").Success)
                    {
                        foreach (char text in Lbl_Moblie.Text)
                        {
                            sb.Append(text);
                        }
                    }
                    else
                    {
                        sb.Append(mobile);
                        if (Device.OS == TargetPlatform.iOS)
                        {
                            DependencyService.Get<IDisplayAlertPopup>().OnlyDigits();
                        }
                        else
                            SplitView.Instace().DisplayAlert("Warning", "Phone numbers can not contain letters or special characters", "OK");
                    }
                }
                else
                {
                    sb.Append(mobile);
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        DependencyService.Get<IDisplayAlertPopup>().Exceed12();
                    }
                    else
                        SplitView.Instace().DisplayAlert("Warning", "Length should not exceed 12 digits", "OK");
                }
                Lbl_Moblie.Text = sb.ToString();
                mobile = Lbl_Moblie.Text;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------



    }
}
