using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idox.LGDP.Apps.ServiceRequest.Client.Classes
{
    public class PropertyComparer : IComparer<SRiProperty>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Compare
        /// 
        /// <summary>	Compares PremisesMapping x and y using the sort mode provided in the constructor.
        /// </summary>
        /// <param name="x">		Object x.</param>
        /// <param name="y">		Object y.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public int Compare(SRiProperty propX, SRiProperty propY)
        {
            int result;
            //
            switch (AppData.PropertyModel.Sort)
            {
                case SortMode.Alphabetical:
                    result = CompareAlpha(propX, propY);
                    break;
                case SortMode.AlphabeticalInverse:
                    result = CompareAlphaInverse(propX, propY);
                    break;
                case SortMode.Date:
                    result = CompareDate(propX, propY);
                    break;
                case SortMode.Distance:
                    result = CompareDistance(propX, propY);
                    break;
                default:
                    result = 0;
                    break;
            }
            //
            return result;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Private Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		CompareAlpha
        /// 
        /// <summary>	Compares SRiRecord x and y based on an ascending alphabetical sort order.
        /// </summary>
        /// <param name="x">		Object x.</param>
        /// <param name="y">		Object y.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        int CompareAlpha(SRiProperty x, SRiProperty y)
        {
            return StringComparer.CurrentCulture.Compare(x.TradeName, y.TradeName);
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		CompareAlphaInverse
        /// 
        /// <summary>	Compares SRiRecord x and y based on a descending alphabetical sort order.
        /// </summary>
        /// <param name="x">		Object x.</param>
        /// <param name="y">		Object y.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        int CompareAlphaInverse(SRiProperty x, SRiProperty y)
        {
            int result;
            //
            switch (CompareAlpha(x, y))
            {
                case 1:
                    result = -1;
                    break;
                case -1:
                    result = 1;
                    break;
                default:
                    result = 0;
                    break;
            }
            //
            return result;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		CompareDate
        /// 
        /// <summary>	Compares SRiRecord x and y based on the premises earliest due date.
        /// </summary>
        /// <param name="x">		Object x.</param>
        /// <param name="y">		Object y.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        int CompareDate(SRiProperty x, SRiProperty y)
        {
            int result;
            //
            if (x.EarliestScheduleDate.HasValue && y.EarliestScheduleDate.HasValue)
                result = DateTime.Compare(x.EarliestScheduleDate.Value, y.EarliestScheduleDate.Value);
            else if (x.EarliestScheduleDate.HasValue)
                result = 1;
            else if (y.EarliestScheduleDate.HasValue)
                result = -1;
            else
                result = 0;
            //
            return result;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		CompareDistance
        /// 
        /// <summary>	Compares SRiRecord x and y based on the premises distance from the user.
        /// </summary>
        /// <param name="x">		Object x.</param>
        /// <param name="y">		Object y.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        int CompareDistance(SRiProperty x, SRiProperty y)
        {
            int result;
            //
            if (x.DistanceFromUser > y.DistanceFromUser)
                result = 1;
            else if (y.DistanceFromUser > x.DistanceFromUser)
                result = -1;
            else
                result = 0;
            //
            return result;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
