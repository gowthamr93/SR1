namespace ServiceRequest.AppContext
{
    /// ----------------------------------------------------------------------------------------------------
    /// Name		ReachabilityNetworkStatus
    /// 
    /// <summary>	The different types of Network status 
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum ReachabilityNetworkStatus
    {
        NotReachable,
        ReachableViaCarrierDataNetwork,
        ReachableViaWiFiNetwork
    }
    /// ----------------------------------------------------------------------------------------------------
    /// Name		ContentType
    /// 
    /// <summary>	The different types of API Content
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum ContentType
    {
        BinaryData,
        Json,
        Xml
    }
    /// ----------------------------------------------------------------------------------------------------
    /// Name		DocumentDownloadState
    /// 
    /// <summary>	The different types of Download State
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum DocumentDownloadState
    {
        Begin,
        Inprogress,
        Completed
    }
}
