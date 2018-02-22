using System;

namespace Idox.LGDP.Apps.Common.OnSite
{
    /// ----------------------------------------------------------------------------------------------------
    /// Name			LoginActions
    /// 
    /// <summary>		Different login scenarios.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum LoginActions
    {
        /// <summary>	No login action has been set.
        /// </summary>
        None,
        /// <summary>	The user is starting a new session for the first time or their details
        /// 			are different to the user from the previous session.
        /// </summary>
        New,
        /// <summary>	The user is starting a new session and they are the same user from the
        /// 			previous session.
        /// </summary>
        Existing,
        /// <summary>	The user is reactivating a dormant session.
        /// </summary>
        Unlocking
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name			OnSiteEnvironment
    /// 
    /// <summary>		The applications environments that can be ran.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum OnSiteEnvironments
    {
        NotSpecified,
        Dev,
        QA,
        Sales,
        Staging,
        Production,
        NoAuth
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SelectionMode
    /// 
    /// <summary>		Different types of selection modes.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum SelectionMode
    {
        None,
        Single,
        Multiple
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name		ShouldSerializeRule
    /// 
    /// <summary>	The different types of should serialize rules available for serialization.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum ShouldSerializeRule
    {
        None,
        Full,
        DataForAPI,
        ModifiedDiff
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SyncStatus
    /// 
    /// <summary>		The sync status of an item that can be updated to the API.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum SyncStatus
    {
        None = 0,
        ReadOnly = 1,
        Saved = 2,
        New = 3,
        Changed = 4
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name			UpdateType
    /// 
    /// <summary>		The type of update that can be applied to an item.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum UpdateType
    {
        None,
        New,
        Changed,
        Deleted
    }
}

