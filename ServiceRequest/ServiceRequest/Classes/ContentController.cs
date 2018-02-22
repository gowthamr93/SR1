using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.ViewModels;
using ServiceRequest.Pages;
using ServiceRequest.AppContext;
using Xamarin.Forms;

namespace ServiceRequest.Classes
{
	public class ContentController : IContentController
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Clear
		/// 
		/// <summary>	Clears all content saved on the device.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public async Task<bool> Clear()
		{
			try
			{
				// It's only necessary for the properties collection to be deleted.
				await FileSystem.DeleteAsync(AppData.CPINFO);
				await FileSystem.DeleteAsync(AppData.LICASE);
				await FileSystem.DeleteAsync(AppData.HISTORY);
				await FileSystem.DeleteAsync(AppData.NOTES);
				await FileSystem.DeleteAsync(AppData.RECORDS);
				await FileSystem.DeleteAsync(AppData.VISITS);
				return true;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
				return false;
			}
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		LoadCaseData
		/// 
		/// <summary>	Loads the cached property collection from the device.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public async Task<bool> LoadCaseData()
		{
			try
			{
				OnSiteConfigCache config;
				SRiCPInfoData cpinfoData;
				SRiLICaseData licaseData;
				SRiCNApplPropData historyData;
				SRiPRNotePadData notesData;
				SRiRecordData recordData;
				SRiVisitData visitData;
				OnSiteDocumentCache docsData;
				FileSystemArgs response, responseInfo;
				Exception error;
				List<SRiProperty> properties;
				//
				// Load the config first and then the data. Return true if both load successfully.
				response = Task.Run(() => FileSystem.ReadText(AppData.CONFIG)).Result;
				//response = await FileSystem.ReadText(AppData.CONFIG).Result;
				if (response.Error == null)
				{
					config = OnSiteConfigCache.FromJson(response.TextContents, out error);
					if (error == null && config != null)
					{
						AppData.ConfigModel.Add(config.Configs, true);

						responseInfo = await FileSystem.ReadText(AppData.CPINFO);
						cpinfoData = SRiCPInfoData.FromJson(responseInfo.TextContents, out error);
						if (error != null) return false;

						responseInfo = await FileSystem.ReadText(AppData.LICASE);
						licaseData = SRiLICaseData.FromJson(responseInfo.TextContents, out error);
						if (error != null) return false;

						// licaseData = SRiLICaseData.FromJson(FileSystem.ReadText(AppData.LICASE).TextContents, out error);
						//if (error != null) return false;

						responseInfo = await FileSystem.ReadText(AppData.HISTORY);
						historyData = SRiCNApplPropData.FromJson(responseInfo.TextContents, out error);
						if (error != null) return false;

						//historyData = SRiCNApplPropData.FromJson(FileSystem.ReadText(AppData.HISTORY).TextContents, out error);
						//if (error != null) return false;

						responseInfo = await FileSystem.ReadText(AppData.NOTES);
						notesData = SRiPRNotePadData.FromJson(responseInfo.TextContents, out error);
						if (error != null) return false;

						//notesData = SRiPRNotePadData.FromJson(FileSystem.ReadText(AppData.NOTES).TextContents, out error);
						//if (error != null) return false;

						responseInfo = await FileSystem.ReadText(AppData.RECORDS);
						recordData = SRiRecordData.FromJson(responseInfo.TextContents, out error);
						if (error != null) return false;

						//recordData = SRiRecordData.FromJson(FileSystem.ReadText(AppData.RECORDS).TextContents, out error);
						//if (error != null) return false;

						responseInfo = await FileSystem.ReadText(AppData.VISITS);
						visitData = SRiVisitData.FromJson(responseInfo.TextContents, out error);
						if (error != null) return false;

						//visitData = SRiVisitData.FromJson(FileSystem.ReadText(AppData.VISITS).TextContents, out error);
						//if (error != null) return false;

						responseInfo = await FileSystem.ReadText(AppData.DOCS);
						docsData = OnSiteDocumentCache.FromJson(responseInfo.TextContents, out error);
						if (error != null) return false;

						//docsData = OnSiteDocumentCache.FromJson(FileSystem.ReadText(AppData.DOCS).TextContents, out error);
						//if (error != null || docsData == null) return false;
						//Added to avoid error on no saced content for existing user
						if (recordData != null && visitData != null)
						{
							recordData.AddVisits(visitData.Visits);
							properties = recordData.CreateProperties(cpinfoData, licaseData, historyData, notesData);
							if (AppData.Environment != OnSiteEnvironments.Sales)
								properties.AddDocumentData(docsData.DocumentData);
							AppData.LastSync = recordData.LastSync;
							AppData.PropertyModel.Update(properties, true);
						}
					}
				}
				//
				return false;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
				return false;
			}
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		LoadUser
		/// 
		/// <summary>	Attempts to load user data from the device, if at any stage of the process a fail is
		/// 			encountered, there is no error displayed and the user is instantiated as a new user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public async void LoadUser()
		{
			try
			{
				SRiUser user;
				FileSystemArgs readResponse;
				Exception error;
				//
				user = null;
				readResponse = Task.Run(() => FileSystem.ReadText(AppData.USER)).Result;
				//        readResponse =await FileSystem.ReadText(AppData.USER);
				if (readResponse.Error == null && readResponse.TextContents.Length > 0)
				{
					user = SRiUser.FromJson(readResponse.TextContents, out error);
					if (user != null)
					{
						switch (Device.OS)
						{
							case TargetPlatform.Android:
								if (AppData.Version.Text != user.Version.Text)
								{
									// Deletion happens when version differs
									//Files
									await FileSystem.DeleteAsync(AppData.CPINFO);
									await FileSystem.DeleteAsync(AppData.LICASE);
									await FileSystem.DeleteAsync(AppData.HISTORY);
									await FileSystem.DeleteAsync(AppData.NOTES);
									await FileSystem.DeleteAsync(AppData.RECORDS);
									await FileSystem.DeleteAsync(AppData.CONFIG);
									await FileSystem.DeleteAsync(AppData.USER);
									await FileSystem.DeleteAsync(AppData.DOCS);
									await FileSystem.DeleteAsync("error.txt");
									await FileSystem.DeleteAsync("log.txt");

									//Documents
									//foreach (var docs in Directory.GetFiles(await DependencyService.Get<IFileSystem>().GetRootDir()))
									//    await FileSystem.DeleteAsync(docs);
									//
									user = null;
								}
								else
								{
									user.LoginAction = LoginActions.Existing;
									user.IsValidated = false;
								}
								break;

							case TargetPlatform.Windows:
								user.LoginAction = LoginActions.Existing;
								user.IsValidated = false;
								break;
							case TargetPlatform.iOS:
								user.LoginAction = LoginActions.Existing;
								user.IsValidated = false;
								break;
						}
						AppData.Environment = user.Environment;
					}
					else user = new SRiUser();
				}
				else user = new SRiUser();
				//
				AppData.MainModel.CurrentUser = user;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
			}
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SaveCaseData
		/// 
		/// <summary>	Saves the cache string of the property collection to the device. Any error 
		/// 			encountered is displayed in an alert.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public async Task<bool> SaveCaseData()
		{
			try
			{
				Dictionary<string, string> cache;
				//
				OnSiteSettings.ShouldSerialize = ShouldSerializeRule.None;
				cache = AppData.PropertyModel.Cache;
				if (!FileSystemResponse(await FileSystem.WriteAsync(cache[AppData.CPINFO], AppData.CPINFO)))
					return false;
				if (!FileSystemResponse(await FileSystem.WriteAsync(cache[AppData.LICASE], AppData.LICASE)))
					return false;
				if (!FileSystemResponse(await FileSystem.WriteAsync(cache[AppData.HISTORY], AppData.HISTORY)))
					return false;
				if (!FileSystemResponse(await FileSystem.WriteAsync(cache[AppData.NOTES], AppData.NOTES)))
					return false;
				if (!FileSystemResponse(await FileSystem.WriteAsync(cache[AppData.RECORDS], AppData.RECORDS)))
					return false;
				if (!FileSystemResponse(await FileSystem.WriteAsync(cache[AppData.VISITS], AppData.VISITS)))
					return false;
				if (!FileSystemResponse(await FileSystem.WriteAsync(cache[AppData.DOCS], AppData.DOCS)))
					return false;
				//
				return true;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
				return false;
			}
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SaveConfig
		/// 
		/// <summary>	Saves the cache string of the config collection to the device. Any error 
		/// 			encountered is displayed in an alert.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public async Task<bool> SaveConfig()
		{
			try
			{
				return FileSystemResponse(await FileSystem.WriteAsync(AppData.ConfigModel.Cache, AppData.CONFIG));
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
				return false;
			}
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SaveUser
		/// 
		/// <summary>	Saves the serialised json of the user data to the device. Any error 
		/// 			encountered is displayed in an alert.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public async Task<bool> SaveUser()
		{
			try
			{
				return FileSystemResponse(await FileSystem.WriteAsync(AppData.MainModel.CurrentUser.ToJson(), AppData.USER));
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
				return false;
			}
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------

		/// ------------------------------------------------------------------------------------------------
		#region Private Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		FileSystemResponse
		/// 
		/// <summary>	Checks if there is an error in the response and displays an error message.
		/// </summary>
		/// <param name="response">		The file system response.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		private bool FileSystemResponse(FileSystemArgs response)
		{
			try
			{
				if (response.Error != null)
				{
					SplitView.DisplayAlert("File Error", response.Error.Message, "OK", null);
					return false;
				}
				return true;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
				return false;
			}
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
