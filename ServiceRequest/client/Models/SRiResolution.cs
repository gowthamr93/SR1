using System;
using System.Collections.Generic;
using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
    public class SRiResolution
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private Action m_oResolveAction;
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructors
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SRiResolution
        /// 
        /// <summary>	Creates a new instance of the SRiResolution class.
        /// </summary>
        /// <param name="sent">			The visit that was sent to the API.</param>
        /// <param name="returned">		The visit that was returned from the API in conflict.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiResolution(SRiVisitMeta sent, SRiVisitMeta returned)
        {
            Title = sent.VisitTypeDescription;
            Description = sent.Visit.DateVisit.ToString("dd/MM/yy HH:mm", "Completed ", "Not complete");
            Type = "VISIT";
            Selected = false;
            CaseID= AppData.PropertyModel.GetRefVal(sent) ?? "No Case ID";
            TradeName = AppData.PropertyModel.GetTradeName(sent) ?? "No Trading Name";
            //
            // There is only a resolve action required for a conflict.
            if (returned.QueryState.ToUpper().Equals("C"))
            {
                Issue = SRiResolutionIssue.Conflict;
                m_oResolveAction = () =>
                {
                    // Make sure the visit stays as changed and update the version to match the returned.
                    sent.Status = SyncStatus.Changed;
                    sent.Version = returned.Version;
                    sent.Visit.UpdateVersions(returned.Visit);
                    AppData.PropertyModel.Update(sent, sent);
                };
            }
            else if (returned.QueryState.ToUpper().Equals("D"))
            {
                Issue = SRiResolutionIssue.Deletion;
                m_oResolveAction = () =>
                {
                    AppData.PropertyModel.Delete(sent);
                };
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SRiResolution
        /// 
        /// <summary>	Creates a new instance of the SRiResolution class.
        /// </summary>
        /// <param name="sent">			The record that was sent to the API.</param>
        /// <param name="returned">		The record that was returned from the API in conflict.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiResolution(SRiRecordMeta sent, SRiRecordMeta returned)
        {
            Title = sent.RequestTypeDescription;
            Description = sent.Record.Name?? "No Value";
            Type = "SRREC";
            Selected = false;
            CaseID = returned.Record.RefVal ?? "No Case ID";
            TradeName = returned.Record.TradeName ?? "No Trading Name";
            //
            if (returned.QueryState.ToUpper().Equals("C"))
            {
                Issue = SRiResolutionIssue.Conflict;
                m_oResolveAction = () =>
                {
                    // Make sure the record stays as changed, but update the UVersion to match the returned.
                    sent.Record.Status = SyncStatus.Changed;
                    sent.Version = returned.Version;
                    sent.Record.UpdateVersions(returned.Record);
                    AppData.PropertyModel.Update(sent, sent);
                };
            }
            else if (returned.QueryState.ToUpper().Equals("D"))
            {
                Issue = SRiResolutionIssue.Deletion;
                m_oResolveAction = () =>
                {
                    AppData.PropertyModel.Delete(sent);
                };
            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        public string Description
        {
            get;
            private set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Issue
        /// 
        /// <summary>	Gets and sets the issue that requires resolving.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiResolutionIssue Issue
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Resolve
        /// 
        /// <summary>	The resolve action that carries out the unique resolution for this instance.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void Resolve()
        {
            if (Selected && m_oResolveAction != null)
                m_oResolveAction();
        }
        /// 
        public string Title
        {
            get;
            private set;
        }

        public bool Selected
        {
            get;
            set;
        }

        public string Type
        {
            get;
            private set;
        }

        public string CaseID
        {
            get;
            private set;
        }
        public string TradeName
        {
            get;
            private set;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
