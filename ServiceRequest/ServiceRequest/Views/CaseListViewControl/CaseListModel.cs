using Xamarin.Forms;

namespace ServiceRequest.Views.CaseListViewControl
{
    public class CaseListModel
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Properties
        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        public View View { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public View Cell { get; set; }

        /// <summary>
        /// Gets or sets the size of the original.
        /// </summary>
        /// <value>The size of the original.</value>
        public Size OriginalSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is open.
        /// </summary>
        /// <value><c>true</c> if this instance is open; otherwise, <c>false</c>.</value>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Get or set index of Case Entry
        /// </summary>
	    public int Index { get; set; }
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
