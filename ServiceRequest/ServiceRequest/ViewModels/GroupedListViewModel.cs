using ServiceRequest.AppContext;
using ServiceRequest.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ServiceRequest.ViewModels
{
    public static class GroupedListViewModel
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        private const string SEPERATOR = " - ";
        private static int _count;
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Variables
        public static ObservableCollection<Grouping<string, GroupedListModel>> GroupedList { get; set; }
        public static int Count
        {
            get { return _count; }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Static Constructor
        static GroupedListViewModel()
        {
            try
            {
                GroupedList = new ObservableCollection<Grouping<string, GroupedListModel>>();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Functions

        /// <summary>
        /// Return the Key and Value pair list when called.
        /// </summary>
        /// <param name="keyValuePairs">The KeyValue Pairs</param>
        /// <returns></returns>
        public static ObservableCollection<Grouping<string, GroupedListModel>> ToGroupedList(
            this List<KeyValuePair<string, string>> keyValuePairs)
        {
            try
            {
                var groupedListModels = new ObservableCollection<GroupedListModel>();
                foreach (var keyValuePair in keyValuePairs)
                {
                    string description;

                    if (!AppContext.AppContext.IsTypeList)
                        description = keyValuePair.Key + SEPERATOR + keyValuePair.Value;
                    else
                        description = keyValuePair.Value;

                    groupedListModels.Add(new GroupedListModel()
                    {
                        Code = keyValuePair.Key,
                        Description = description
                    });
                }

                var sorted = from groupedModel in groupedListModels
                             orderby groupedModel.Code
                             group groupedModel by groupedModel.CodeSort into groupedList
                             select new Grouping<string, GroupedListModel>(groupedList.Key, groupedList);
                _count = sorted.Count() + groupedListModels.Count;
                GroupedList = AppContext.AppContext.IsParalist ? new ObservableCollection<Grouping<string, GroupedListModel>>(sorted.Where(aitem=> aitem.Key !="U")) : new ObservableCollection<Grouping<string, GroupedListModel>>(sorted);              
                return GroupedList;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Return the selected item index from grouped list
        /// </summary>
        /// <param name="selectedItem">Object of selected Item as GroupedListModel</param>
        /// <returns></returns>
        public static int SelectedIndex(GroupedListModel selectedItem)
        {
            int count = 0;

            var selectedParent = GroupedList.FirstOrDefault(x => x.Contains(selectedItem));

            var parentIndex = GroupedList.IndexOf(selectedParent);

            for (int i = 0; i < parentIndex; i++)
            {
                count += GroupedList[i].Count;
            }

            var index = (from groupedlist in GroupedList
                         from item in groupedlist
                         where item.Code == selectedItem.Code
                         select groupedlist.IndexOf(item)).FirstOrDefault();

            count += index;

            return count;
        }

    }

    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; set; }

        public Grouping(K key, IEnumerable<T> items)
        {
            try
            {
                Key = key;
                foreach (var item in items)
                    Items.Add(item);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
    }
    #endregion
        /// ------------------------------------------------------------------------------------------------
}
