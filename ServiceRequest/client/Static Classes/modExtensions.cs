using System;
using System.Collections.Generic;

using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client.Models;


namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	public static class modExtensions
	{
        /// -----------------------------------------------------------------------------------------------
        /// Name        FormattedAddress
        /// 
        /// <summary>   Formats the address from the ordnance survey to remove the comma 
        ///             between house number and street name.
        /// </summary>
        /// <param name="address">      The ordnance survey address model.</param>
        /// 
        /// <returns>   Formatted address string.
        /// </returns>
        /// -----------------------------------------------------------------------------------------------
        ///
        public static string FormattedAddress(this SRiUtilityAddress address)
        {
            int index;
            //
            index = address.Address.IndexOf(',');
            if (index != -1)
                return address.Address.Remove(index, 1);
            else
                return address.Address;
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        SetNewOrChanged
        /// 
        /// <summary>   Returns what the status should be if setting to New or Changed.
        /// </summary>
        /// <param name="s">        The current status.</param>
        /// <param name="status">   The status being set.</param>
        /// 
        /// <remarks>   Stops a status of New becoming Changed, or vis versa.
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static SyncStatus SetNewOrChanged(this SyncStatus s, SyncStatus status)
        {
            if (status == SyncStatus.New || status == SyncStatus.Changed &&
                s == SyncStatus.None || s == SyncStatus.ReadOnly || s == SyncStatus.Saved)
                s = status;
            //
            return s;
        }


        public static void AddDocumentData(this List<SRiProperty> properties, List<OnSiteDocumentData> docData)
		{
			foreach (var data in docData)
				properties.AddDocumentData(data);
		}

		public static void AddDocumentData(this List<SRiProperty> properties, OnSiteDocumentData data)
		{
			bool done;
			//
			foreach (var docMeta in data.EntityDocumentLists)
			{
				done = false;
				foreach (var p in properties)
				{
					// Add all documents to all properties in sales environment.
					if (AppData.Environment == OnSiteEnvironments.Sales)
						p.Documents.AddMeta(data, docMeta);
					else
					{
						foreach (var rg in p.RequestGroups)
						{
							foreach (var r in rg.Records)
							{
								if (docMeta.EntityKey.Equals(r.Record.EntityKey))
								{
									p.Documents.AddMeta(data, docMeta);
									done = true;
									break;
								}
								foreach (var i in r.Record.Inspections)
								{
									foreach (var v in i.Visits)
										if (docMeta.EntityKey.Equals(v.Visit.EntityKey))
										{
											p.Documents.AddMeta(data, docMeta);
											done = true;
											break;
										}
									if (done) break;
								}
							}
							if (done) break;
						}
						if (done) break;
						//
						foreach (var cp in p.CPInfos)
							if (docMeta.EntityKey.Equals(cp.CPInfo.EntityKey))
							{
								p.Documents.AddMeta(data, docMeta);
								done = true;
								break;
							}
						if (done) break;
						//
						foreach (var li in p.LICases)
							if (docMeta.EntityKey.Equals(li.LICase.EntityKey))
							{
								p.Documents.AddMeta(data, docMeta);
								done = true;
								break;
							}
						if (done) break;
					}
				}
			}
		}

		public static void UpdateDocStatus(this List<SRiProperty> properties, OnSiteDocumentCache docCache)
		{
			foreach (var cacheData in docCache.DocumentData)
				foreach (var p in properties)
					foreach (var docData in p.Documents)
						if (docData.Entity.Equals(cacheData.Entity))
							foreach (var cacheMeta in cacheData.EntityDocumentLists)
								foreach (var docMeta in docData.EntityDocumentLists)
									if (docMeta.EntityKey.Equals(cacheMeta.EntityKey))
										foreach (var cacheDoc in cacheMeta.Documents)
											foreach (var doc in docMeta.Documents)
												if (doc.Id.Equals(cacheDoc.Id))
												{
													doc.Status = cacheDoc.Status;
													break;
												}
		}

		public static void AddMeta(this List<OnSiteDocumentData> docs, OnSiteDocumentData docData, OnSiteDocumentMeta docMeta)
		{
			bool dataExists;
			//
			dataExists = false;
			foreach (var data in docs)
				if (data.Entity.Equals(docData.Entity))
				{
					data.AddDocument(docMeta);
					dataExists = true;
					break;
				}
			//
			if (!dataExists)
				docs.Add(new OnSiteDocumentData()
				{
					AppModule = docData.AppModule,
					Entity = docData.Entity,
					EntityDocumentLists = new List<OnSiteDocumentMeta>() { docMeta }
				});
		}

		public static void AddDocument(this OnSiteDocumentData data, OnSiteDocumentMeta docMeta)
		{
			bool metaExists;
			//
			metaExists = false;
			foreach (var meta in data.EntityDocumentLists)
				if (meta.EntityKey.Equals(docMeta.EntityKey))
				{
					foreach (var doc in docMeta.Documents)
						meta.AddDocument(doc);
					//
					metaExists = true;
					break;
				}
			//
			if (!metaExists)
				data.EntityDocumentLists.Add(docMeta);
		}

		public static void AddDocument(this OnSiteDocumentMeta meta, OnSiteDocument doc)
		{
			bool docExists;
			//
			docExists = false;
                for (int d = 0; d < meta.Documents.Count; d++)
                //if (doc.Id != null)
                //{
                    if (meta.Documents[d].Id.Equals(doc.Id))
                    {
                        doc.Status = meta.Documents[d].Status;
                        meta.Documents[d] = doc;
                        docExists = true;
                        break;
                    }

                    //
                    if (!docExists)
                        meta.Documents.Add(doc);
           //}
		}

		public static void SetSavedStatus(this SRiProperty p, SRiProperty p2)
		{
			foreach (var rg in p.RequestGroups)
				foreach (var rg2 in p2.RequestGroups)
					if (rg.GroupType.Equals(rg2.GroupType))
					{
						rg.SetSavedStatus(rg2);
						break;
					}
		}

		public static void SetSavedStatus(this SRiRequestGroup rg,  SRiRequestGroup rg2)
		{
			foreach (var r in rg.Records)
				foreach (var r2 in rg2.Records)
					if (r.Record.EntityKey.Equals(r2.Record.EntityKey))
					{
						r.SetSavedStatus(r2);
						break;
					}
		}

		public static void SetSavedStatus(this SRiRecordMeta r, SRiRecordMeta r2)
		{
			r.Record.Status = r2.Record.Status;
			//
			foreach (var i in r.Record.Inspections)
				foreach (var i2 in r2.Record.Inspections)
					if (i.KeyVal.Equals(i2.KeyVal))
					{
						i.SetSavedStatus(i2);
						break;
					}
		}

		public static void SetSavedStatus(this SRiInspection i, SRiInspection i2)
		{
			foreach (var v in i.Visits)
				foreach (var v2 in i2.Visits)
					if (v.Visit.EntityKey.Equals(v2.Visit.EntityKey))
					{
						v.SetSavedStatus(v2);
						break;
					}
		}

        public static string OfficerCode(this SRiUser user, string organisation)
        {
            string officerCode;
            //
            officerCode = user.IdoxId;
            foreach (OnSiteOfficer off in AppData.ConfigModel.OfficerList(organisation))
                if (off.IdoxId.ToLower().Equals(user.IdoxId.ToLower()))
                {
                    officerCode = off.OfficerCode;
                    break;
                }
            //
            return officerCode;
        }

        public static void SetSavedStatus(this SRiVisitMeta v, SRiVisitMeta v2)
		{
			v.Visit.Status = v2.Visit.Status;
		}

	    public static string GetDocumentName(this string fileExtension)
	    {
	            string type = "";
	            //
	            switch (fileExtension)
	            {

                case ".gif":
                case ".jpeg":
                case ".jpg":
                case ".png":
                case ".tiff":
                case ".tif":
                case ".bmp":
                    type = "Photo";
                    break;
                case ".mp3":
                case ".mp4":
                case ".wav":
                    type = "Audio";
                    break;
                //case ".txt":
	               // case ".csv":
                //    Type = "txt.png";
	               //     break;
	               // case ".doc":
	               // case ".docx":
                //    Type = "doc.png";
	               //     break;
	               
                //    case ".pdf":
                //    Type = "pdf.png";
	               //     break;
	               
	              
	               // case ".xls":
	               // case ".xlsx":
                //    Type = "xls.png";
	               //     break;
	               // default:
                //    Type = "others.png";
	               //     break;
	            }
	         return type;
	    }
	}
}
