using ServiceRequest.AppContext;
using System;

namespace ServiceRequest.Models
{
    public class ShortIndexMapping
    {
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ShortIndexMapping
        /// 
        /// <summary>	Creates a new instance of the ShortIndexMapping class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public ShortIndexMapping(int section, int requestGroupIndex, int recordIndex, int propertyIndex)
        {
            try
            {
                Section = section;
                RequestGroupIndex = requestGroupIndex;
                Row = RecordIndex = recordIndex;
                PropertyIndex = propertyIndex;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Properties
        public int RequestGroupIndex { get; private set; }
        public int RecordIndex { get; private set; }
        public int Section { get; private set; }
        public int Row { get; private set; }
        public int PropertyIndex { get; private set; }
        #endregion
    }

}
