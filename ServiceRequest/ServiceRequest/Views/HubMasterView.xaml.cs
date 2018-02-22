using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using ServiceRequest.Custom;
using ServiceRequest.Pages;
using ServiceRequest.Views.CaseListViewControl;
using ServiceRequest.Views.PopUp;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Forms.GoogleMaps;
using Pin = Xamarin.Forms.Maps.Pin;
using Position = Xamarin.Forms.Maps.Position;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client.Models;
using ServiceRequest.Classes;
using ServiceRequest.Views.ViewCells;

namespace ServiceRequest.Views
{
	public partial class HubMasterView : ContentView
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public variables and properties
		public SyncStatusView VwSyncStatus { get; set; }
		public static CaseListControl CaseListView;

		#endregion
		/// ------------------------------------------------------------------------------------------------

		/// ------------------------------------------------------------------------------------------------
		#region Private variables and properties       

		private List<SRiRequestGroup> _sRiRequestGroup;
		public List<SRiRequestGroup> LstSRiRequestGroups
		{
			get { return _sRiRequestGroup ?? (_sRiRequestGroup = new List<SRiRequestGroup>()); }
			set { _sRiRequestGroup = value; }
		}

		private SRiProperty _sriProperty;
		#endregion
		/// ------------------------------------------------------------------------------------------------

		/// ------------------------------------------------------------------------------------------------
		#region Public constructor   
		public HubMasterView()
		{
			try
			{
				InitializeComponent();
				CaseListView = new CaseListControl();
				VwSyncStatus = new SyncStatusView();
				VwSyncStatus.UpdateDateTime();
				//SyncStatusUpdate();

				Gl_Footer.Children.Add(VwSyncStatus, 1, 0);
				AppData.PropertyModel.PropertyUpdated += PropertyUpdated;
				TapGestures();
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}

		#endregion
		/// ------------------------------------------------------------------------------------------------

		/// ------------------------------------------------------------------------------------------------
		#region Public functions
		/// <summary>
		/// Check the Sync
		/// </summary>
		public async void SyncCheck()
		{
			try
			{
				if (await SplitView.DisplayAlert("Sync",
						 "All changes saved on the device will be sent to the Uniform Cloud, any " +
						 "Cases with no further inspections pending will be removed and any new for the officer " +
						 "will be retreived.\r\nDo you wish to continue?",
						 "Sync Now", "Cancel"))
				{
					if (AppContext.AppContext.LstSriproperty.Any(x => x.Status == SyncStatus.New))
					{
						if (await HasNoLatLong() || await HasNoUPRN())
						{
							return;
						}
						else
						{
							SyncBegin();
						}
					}
					else
					{
						SyncBegin();
					}
				}
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}

		private async Task<bool> HasNoUPRN()
		{
			try
			{
				bool flag = false;
				foreach (var prop in AppContext.AppContext.LstSriproperty)
				{
					if (prop.Status == SyncStatus.New && prop.RequestGroups[0].Records[0].Record.UPRN == null)
					{
						if (await SplitView.DisplayAlert("New Case Has Unvalidated Address",
							"The address for " + prop.Address.ShortAddress +
							" was not validated and so does not have a UPRN. Are you sure you want to sync?", "Yes", "No"))
						{
							flag = false;
							break;
						}
						else
						{
							flag = true;
							break;
						}
					}
				}
				return flag;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
				return false;
			}
		}

		private async Task<bool> HasNoLatLong()
		{
			try
			{
				bool flag = false;
				foreach (var prop in AppContext.AppContext.LstSriproperty)
				{
					if (prop.Status == SyncStatus.New)
					{
						if ((prop.RequestGroups[0].Records[0].Record.Latitude == null && prop.RequestGroups[0].Records[0].Record.Longitude == null)
							|| (prop.RequestGroups[0].Records[0].Record.Latitude == 0 && prop.RequestGroups[0].Records[0].Record.Longitude == 0))
						{
							await SplitView.Instace().DisplayAlert("New Case has No Location for Case", "The address for " + prop.Address.ShortAddress + " does not have a location. Please correct this and try again", "Ok");
							flag = true;
							break;
						}
					}
				}
				return flag;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
				return false;
			}
		}


		/// <summary>
		/// Filter the data based on complete incomplete and all
		/// </summary>
		public async void Filter_List()
		{
			try
			{
				List<PropertyMapping> p_Map = AppData.PropertyModel.Mappings;
				//
				AppContext.AppContext.LstSriproperty.Clear();

				if (AppContext.AppContext.LstGooglePin != null)//Android to clear pin
					AppContext.AppContext.LstGooglePin.Clear();

				if (AppContext.AppContext.LstCustomPin != null)//Windows Platform
					AppContext.AppContext.LstCustomPin.Clear();
				SplitView.MapView.ClearPin();

				// SplitView.HubMaster.FindByName<Grid>("Gl_CaseList").Children.Clear();
				CaseListView.Clear();

				if (p_Map.Count > 0)
				{
					SplitView.HubMaster.FindByName<Label>("LblSearchHint").IsVisible = false;
					for (int i = 0; i < p_Map.Count; i++)
					{
						_sriProperty = AppData.PropertyModel.PropertyFromMapping(p_Map[i]);
						LstSRiRequestGroups = _sriProperty.RequestGroups;      //Assign the sriproperty requestgroup to temp requestgroup list        
						LoadRequestGrp();
						AppContext.AppContext.LstSriproperty.Add(_sriProperty);
						Load(i);
					}
					CaseListView.CloseAllEntries();
					if (Device.OS == TargetPlatform.Android)
					{
						var formsMap = (AndroidMapView)SplitView.MapView;
						SplitView.Fullmapview.FindByName<Grid>("GlMapView").Children.Clear();
						SplitView.Fullmapview.FindByName<Grid>("GlMapView").Children.Add(formsMap, 0, 0);
						SplitView.MapView.LoadPins(AppContext.AppContext.LstGooglePin);
					}
					else
					{
						SplitView.MapView.LoadPins(AppContext.AppContext.LstCustomPin);
					}
					SplitView.HubMaster.FindByName<Grid>("Gl_CaseList").Children.Add(CaseListView, 0, 0);
				}
				else
				{
					await SplitView.Instace().Clear();
					//Added for IDXSR-260
					if (Device.OS == TargetPlatform.Android)
					{
						var formsMap = (AndroidMapView)SplitView.MapView;
						SplitView.Fullmapview.FindByName<Grid>("GlMapView").Children.Clear();
						SplitView.Fullmapview.FindByName<Grid>("GlMapView").Children.Add(formsMap, 0, 0);
						SplitView.MapView.LoadPins(AppContext.AppContext.LstGooglePin);
					}
					else
					{
						SplitView.MapView.LoadPins(AppContext.AppContext.LstCustomPin);
					}
				}
				GC.Collect();
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}

		#endregion
		/// ------------------------------------------------------------------------------------------------

		/// ------------------------------------------------------------------------------------------------
		#region Private Functions

		/// <summary>
		/// Synchronize data
		/// </summary>
		private async void SyncBegin()
		{
			try
			{
				AppData.SyncInProgress = true;
				VwSyncStatus.SyncStarted();
				// Clear the request mappings and remove all the sections from the table.
				AppData.PropertyModel.Mappings.Clear();
				// await Task.Delay(800);
				if (await AppData.API.Sync())
				{
					//Modified for IDXSR-343 on 10.05.2017
					AppData.PropertyModel.SelectedProperty = null;
					await ReloadPropertyData();
					SyncFinish(true);
				}
				else
				{
					//if syn fails, trigeer the propertyviewmodel to repopulate the data in memory
					AppData.PropertyModel.Update(new List<SRiProperty>());
					SyncFinish(false);
					//
					//Call Resolution Manager when conflicts occurs
					if (API.m_Resolution?.Count > 0)
					{
						if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
							await Task.Delay(1500);
						else
						{
							await Task.Delay(800);
						}
						SplitView.CenterPopupContent.ShowPopupCenter(new ResolutionView(API.m_Resolution), 0.5, "Adding Resolution");
						API.m_Resolution.Clear();
					}
					//
				}
				////show the Plus symbol 
				//FullMapView.ShowHideAdd.Invoke(true);
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}

		public async Task ReloadPropertyData()
		{
			try
			{
				await SplitView.Instace().Clear();
				//on loading property list record progress must stopp
				AppContext.AppContext.NewRecordInProgress = false;
				FullMapView.AddNewCasePointOnMapView = null;
				//
				AppData.PropertyModel.UpdatePropertyList();
				Filter_List();
				SplitView.Instace().FilterCheckAvailable(true); // To Enable Filter Image
				SplitView.HubMaster.FindByName<Label>("LblSearchHint").IsVisible = false;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
			}
		}

		/// <summary>
		/// To Check the syncStaus
		/// </summary>
		/// <param name="passed"></param>
		private void SyncFinish(bool passed)
		{
			try
			{
				if (!AppData.SyncInProgress)
				{
					VwSyncStatus.SyncStopped(AppData.SyncInProgress);
				}
				else
				{
					VwSyncStatus.SyncStopped(passed);
				}
				//else
				//{
				//    SplitView.Instace()
				//        .DisplayAlert("Sync Timed Out", "The sync has ran for too long and has timed out.", "Ok");
				//}

				AppData.SyncInProgress = false;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}

		/// <summary>
		/// On tapped the action label
		/// </summary>
		private void TapGestures()
		{
			try
			{
				TapGestureRecognizer actionTapGesture = new TapGestureRecognizer();
				actionTapGesture.Tapped += (s, e) =>
				{
					OnActionTapped();
				};
				Lbl_Action.GestureRecognizers.Add(actionTapGesture);

				var tapLblDeleteVisit = new TapGestureRecognizer();
				tapLblDeleteVisit.Tapped += (s, e) => OnLblDeleteVisitTapped();
				LblDeleteVisit.GestureRecognizers.Add(tapLblDeleteVisit);
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}

		/// Name		OnActionTapped
		/// 
		/// <summary>
		/// Handles operation for tapping on Action.
		/// </summary>
		private async void OnActionTapped()
		{
			try
			{
				if (AppData.SyncInProgress)
					await SplitView.DisplayAlert("Sync In Progress", "Please wait for the sync to finish before taking further actions.", "OK", null);
				else if (AppContext.AppContext.NewRecordInProgress)
					await SplitView.DisplayAlert("New Record creation is in progress", "Please save or cancel new sevice record to proceed", "OK", null);
				else
				{
					if (Reachability.InternetConnectionStatus() != ReachabilityNetworkStatus.NotReachable)
						SplitView.PopupContent.ShowPopupRelative(new ActionView(SplitView.PopupContent), Lbl_Action, 200, 100, true, "");
					else
						await SplitView.DisplayAlert("Offline", "It is not possible to complete this action while in offline mode.\rConnect the device to a network and try again.", "OK", null);
				}
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}

		private async void OnLblDeleteVisitTapped()
		{
			try
			{
				if (await LockScreen.ToDisplayAlert(SplitView.Instace(), "Delete Service Record", "Are you sure you want to delete this service record?", "Yes", "No"))
					InspectionCellViewCell.OnDelete(InspectionCellView.CurrentUnifiedItem);
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}

		}

		public void MakeDeleteVisible(bool status)
		{
			LblDeleteVisit.IsVisible = status;
		}

		/// <summary>
		/// fire on Property type updated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PropertyUpdated(object sender, PropertyUpdatedEventArgs e)
		{
			try
			{
				if (e.PropertyType == PropertyType.PropertyFilters)
				{
					Filter_List();
					SplitView.InspectionCount?.Update();
				}
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}

		/// <summary>
		/// To check the record has completed date or not
		/// </summary>
		/// <param name="record"></param>
		/// <returns></returns>
		public bool CheckComplete(SRiRecord record)
		{

			bool incomplete;
			//
			incomplete = false;
			try
			{
				foreach (var i in record.Inspections)
				{
					foreach (var v in i.Visits)
						if (!v.Visit.DateVisit.HasValue)
						{
							incomplete = true;
							break;
						}
					if (incomplete) break;
				}
				return !incomplete;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
				return incomplete;
			}
		}

		/// <summary>
		/// Load the data while update the propertytype propertyfilters
		/// </summary>
		/// l
		/// <param name="index"></param>
		private void Load(int index)
		{
			try
			{
				AppContext.AppContext.CaseCell = new CaseCellList(_sriProperty, index);
				AppContext.AppContext.InspectionCell = new InspectionCellView(index);
				CaseListView.Add(AppContext.AppContext.CaseCell, AppContext.AppContext.InspectionCell);

				switch (Device.OS)

				{
					case TargetPlatform.Android:
						if (AppContext.AppContext.LstGooglePin == null)
							AppContext.AppContext.LstGooglePin = new List<Xamarin.Forms.GoogleMaps.Pin>();
						if (_sriProperty.HasValidCoords && _sriProperty.Longitude != null && _sriProperty.Latitude != null)
						{
							AppContext.AppContext.LstGooglePin.Add(new Xamarin.Forms.GoogleMaps.Pin
							{
								Position =
									new Xamarin.Forms.GoogleMaps.Position((double)_sriProperty.Latitude,
										(double)_sriProperty.Longitude),
								Address = _sriProperty.Address.RawAddress,
								Label = index.IndexToAlphabet(),
								Icon = BitmapDescriptorFactory.FromView(new PinView(index.IndexToAlphabet()))
							});

						}
						break;
					case TargetPlatform.iOS:
					case TargetPlatform.Windows:
						if (AppContext.AppContext.LstCustomPin == null)
							AppContext.AppContext.LstCustomPin = new List<CustomPin>();
						if (_sriProperty.HasValidCoords && _sriProperty.Longitude != null && _sriProperty.Latitude != null)
						{
							AppContext.AppContext.LstCustomPin.Add(new CustomPin
							{
								Pin = new Pin
								{
									Position = new Position((double)_sriProperty.Latitude, _sriProperty.Longitude.Value),
									Address = _sriProperty.Address.RawAddress,
									Label = index.IndexToAlphabet(),
								}

							});
						}
						break;
				}
				FullMapView.IsLocationVisible = false;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
			}
		}

		public void LoadRequestGrp()
		{
			try
			{
				int index = 0;
				List<SRiRequestGroup> templist = new List<SRiRequestGroup>();//Temporary request group 
				var isComplete = false;
				foreach (var tempSripropertyReq in LstSRiRequestGroups)
				{
					if (AppData.PropertyModel.FilterByType == null || tempSripropertyReq.GroupType.Equals(AppData.PropertyModel.FilterByType))
					{
						//Check//
						if (tempSripropertyReq.Records.Count == 1)
						{
							tempSripropertyReq.TempRecords = null;
							foreach (var tempRequestgrpRecords in tempSripropertyReq.Records)
							{
								switch (AppData.PropertyModel.Filter)
								{
									case FilterMode.All:
										if (AppData.PropertyModel.FilterByType != null)//To check the filter by request type or not
										{
											templist.Add(tempSripropertyReq);
											isComplete = true;
										}
										break;
									case FilterMode.Complete:
										if (!CheckComplete(tempRequestgrpRecords.Record))
											templist.Add(tempSripropertyReq);
										else if (AppData.PropertyModel.FilterByType != null)//To check the filter by request type or not
										{
											templist.Add(tempSripropertyReq);
											isComplete = true;
										}
										break;
									case FilterMode.Incomplete:
										if (CheckComplete(tempRequestgrpRecords.Record))
											templist.Add(tempSripropertyReq);
										else if (AppData.PropertyModel.FilterByType != null)//To check the filter by request type or not
										{
											templist.Add(tempSripropertyReq);
											isComplete = true;
										}
										break;
								}
								if (isComplete) //if filter by request type break the loop to avoid unneccesary looping
									break;
							}
							if (templist.Count > 0)
							{
								LstSRiRequestGroups = !isComplete ? LstSRiRequestGroups.Except(templist).ToList() : templist;
							}

						}
						////check//
						///// if the requesttype has multiple records...

						else
						{
							List<SRiRecordMeta> temprecord = new List<SRiRecordMeta>();
							tempSripropertyReq.TempRecords = tempSripropertyReq.Records;
							foreach (var t2 in tempSripropertyReq.Records)
							{
								switch (AppData.PropertyModel.Filter)
								{
									case FilterMode.All:
										break;
									case FilterMode.Complete:
										if (!CheckComplete(t2.Record))
											temprecord.Add(t2);

										break;
									case FilterMode.Incomplete:
										if (CheckComplete(t2.Record))
											temprecord.Add(t2);
										break;
								}
							}
							LstSRiRequestGroups[index].TempRecords = LstSRiRequestGroups[index].TempRecords.Except(temprecord).ToList();
							if (AppData.PropertyModel.FilterByType != null)
							{
								LstSRiRequestGroups = LstSRiRequestGroups.GetRange(index, 1);//to set the particular request group only.
								isComplete = true;
							}
						}
					}
					if (isComplete)
						break;

					index++;
				}
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
