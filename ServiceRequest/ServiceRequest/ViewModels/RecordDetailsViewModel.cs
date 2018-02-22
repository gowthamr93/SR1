using System;
using System.Collections.Generic;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using Idox.LGDP.Apps.Common.OnSite;
using Xamarin.Forms;

namespace ServiceRequest.ViewModels
{
    /// ------------------------------------------------------------------------------------------------
    #region Key value pair function
    /// <summary>
    /// To set the key value pair as list
    /// </summary>
    public class RecordSummaryList : List<KeyValuePair<string, string>>
    {
        public void Add(string key, string value)
        {
            //if (key != null && value != null)
            if (key != null)
                Add(new KeyValuePair<string, string>(key, value));
        }
    }
    #endregion
    /// ------------------------------------------------------------------------------------------------

    /// ------------------------------------------------------------------------------------------------
    #region Record summary
    /// <summary>
    /// To create the record summary list as key value pair
    /// </summary>
    public class CreateRecordList
    {
        public bool ImagesShow { get; set; }
        public string Title { get; set; }
        public RecordSummaryList Details { get; set; }
        public CreateRecordList()
        {
            try
            {
                ImagesShow = false;
                Title = "RECORD SUMMARY";
                Details = new RecordSummaryList();
                Details.AddRange(AppData.PropertyModel.SelectedRecord.Record.Record.BasicDetails);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }

        }

    }
    #endregion
    /// ------------------------------------------------------------------------------------------------

    /// ------------------------------------------------------------------------------------------------
    #region Visit list  
    /// <summary>
    /// To create the visits list as key value pair
    /// </summary>      
    public class CreateVisitsList
    {
        public string VisitTypeDescription { get; set; }
        public string ScheduleDate { get; set; }
        public string CompletedDate { get; set; }
        public int VisitListIndex { get; set; }

    }
    #endregion
    /// ------------------------------------------------------------------------------------------------

    /// ------------------------------------------------------------------------------------------------
    #region Customer list
    /// <summary>
    /// To set the customer list as key value pair
    /// </summary>
    public class CreateCustomerList
    {
        public string Title { get; set; }
        public RecordSummaryList Details { get; set; }
        public CreateCustomerList()
        {
			try
			{
				//CustomerList = new List<RecordSummaryList>();           
				Title = "CUSTOMERS";
				Details = new RecordSummaryList();
				for (int i = 0; i < AppData.PropertyModel.SelectedRecord.CustomerMappings.Count; i++)
				{
					if (Device.OS == TargetPlatform.Android)
					{
						if (AppData.PropertyModel.SelectedRecord.CustomerMappings[i].FieldName == "Address")
						{
							var address = AppData.PropertyModel.SelectedRecord.CustomerMappings[i].FieldValue?.Replace("\r",
								Environment.NewLine);
							AppData.PropertyModel.SelectedRecord.CustomerMappings[i].FieldValue = address;
						}
					}
					Details.Add(AppData.PropertyModel.SelectedRecord.CustomerMappings[i].FieldName, AppData.PropertyModel.SelectedRecord.CustomerMappings[i].FieldValue);
				}
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
			}
        }
    }
    #endregion
    /// ------------------------------------------------------------------------------------------------

    /// ------------------------------------------------------------------------------------------------
    #region VisitsAction data
    public class VisitsActionData
    {
        public string ActionTypeDescription { get; set; }
        public string CompletedDate { get; set; }
        public int ActionIndex { get; set; }
    }
    #endregion
    /// ------------------------------------------------------------------------------------------------
}
