using System;
namespace Idox.LGDP.Apps.ServiceRequest.Client
{
    /// ----------------------------------------------------------------------------------------------------
    /// Name		CodeLists
    /// 
    /// <summary>	The different code lists available in the app.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum CodeLists
    {
        RequestKind,
        RequestType,
        InspectionType,
        VisitType,
        ActionType
    }
    /// ----------------------------------------------------------------------------------------------------
    /// Name			FieldType
    /// 
    /// <summary>		Different field types used by the FieldMapping.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum FieldType
    {
        CustomerHeader,
        CustomerField
    }
    /// ----------------------------------------------------------------------------------------------------
    /// Name			FilterMode
    /// 
    /// <summary>		Different filter types for the main menu.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum FilterMode
    {
        All = 0,
        Complete = 1,
        Incomplete = 2
    }
    /// ----------------------------------------------------------------------------------------------------
    /// Name			MappingType
    /// 
    /// <summary>		The different model that are mapped by an IndexMapping.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum MappingType
    {
        None,
        Property,
        RequestGroup,
        Record,
        Inspection,
        Visit,
        Action,
        Treatment
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name			PropertyType
    /// 
    /// <summary>		All property types available for update calls in the application.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum PropertyType
    {
        CacheLoaded,
        Config,
        Documents,
        PropertyData,
        PropertyFilters,
        PropertyList,
        SearchResults,
        SelectedProperty,
        SelectedRecord,
        SelectedVisit,
        SelectedAction,
        User,
        Environment
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name		RestContentType
    /// 
    /// <summary>	Determines the different types of content for rest requests.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum RestContentType
    {
        None,
        BinaryData,
        Json,
        Xml
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SortMode
    /// 
    /// <summary>		Different types of sort mode available for the main menu.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum SortMode
    {
        None,
        Alphabetical,
        AlphabeticalInverse,
        Date,
        Distance
    }
    /// 
	/// ----------------------------------------------------------------------------------------------------
	/// Name			PopupType
	/// 
	/// <summary>		Different types of Popup available for the Popup Layout.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	///
    public enum PopupType
    {
        Center,
        Relative,
        Margin
    }
    /// 
	/// ----------------------------------------------------------------------------------------------------
	/// Name			TrianglePoistion
	/// 
	/// <summary>		Different types of Position available for the display trianlge of relative Popup.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	///
    public enum TrianglePoistion
    {
        Above,
        Below,
    }
    ///
    /// ----------------------------------------------------------------------------------------------------
    /// Name            CellTypes
    /// 
    /// <summary>       Different types of cell types available for the paragraphs.
    /// </summary>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum CellTypes
    {
        Normal,
        Custom,
        Input
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name        SRiResolutionIssue
    /// 
    /// <summary>   Different types of resolution issues.
    /// </summary>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public enum SRiResolutionIssue
    {
        None,
        Conflict,
        Deletion
    }
}

