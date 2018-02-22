using Idox.LGDP.Apps.Common.OnSite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Views.PopUp;
using ServiceRequest.AppContext;
using modExtensions = Idox.LGDP.Apps.Common.OnSite.modExtensions;

namespace ServiceRequest.ViewModels
{
    public class ParagraphViewModel : INotifyPropertyChanged
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Variables and Properties
        /// <summary>
        /// Code to display for custom paragraph
        /// </summary>
        public const string CustomCode = "+";

        /// <summary>
        /// Code to display for custom paragraph
        /// </summary>
        public const string CUSTOM_PLACEHOLDER = "Insert custom text";
        public const string TAP_CUSTOM_PLACEHOLDER = "Tap to Insert Custom Text";
        public const string DEFAULT_VALUE_TEXT = "Value";
        public List<KeyValuePair<string, string>> TypeList
        {
            get
            {
                if (_mTypeList == null)
                {
                    _mTypeList = new List<KeyValuePair<string, string>>();
                    foreach (var character in modExtensions.Alphabet.ToList())
                    {
                        _mTypeList.Add(new KeyValuePair<string, string>(character, character));
                    }
                }
                return _mTypeList;
            }

        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Variables and Properties
        /// <summary>
        /// Length of start of paragraph to show in list picker and this modal
        /// </summary>
        private const int ParaDescLen = 90;

        /// <summary>
        /// Gets the paragraph description.
        /// </summary>
        /// <returns>A short human readable paragraph description.</returns>
        /// <summary>
        /// Cache of the Type list
        /// </summary>
        private List<KeyValuePair<string, string>> _mTypeList;

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Functions
        /// <summary>
        /// Gets the List of paragraph description.
        /// </summary>
        /// <returns>A List of short human readable paragraph description.</returns>
        /// <param name="paraList">KeyvaluePair.</param>
        public List<KeyValuePair<string, string>> GetParaDescList(List<KeyValuePair<string, OnSiteConfigPara>> paraList)
        {
            try
            {
                var paragraphDescList = new List<KeyValuePair<string, string>>();

                foreach (var para in paraList)
                {
                    paragraphDescList.Add(new KeyValuePair<string, string>(para.Value.Code, GetParaDesc(para)));
                }
                return paragraphDescList;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }

        }

        private List<KeyValuePair<string, OnSiteConfigPara>> _mParagraphList;
        public List<KeyValuePair<string, OnSiteConfigPara>> ParagraphList
        {
            get
            {
                if (_mParagraphList == null)
                {
                    _mParagraphList = new List<KeyValuePair<string, OnSiteConfigPara>>();
                    _mParagraphList.Add(new KeyValuePair<string, OnSiteConfigPara>("", new OnSiteConfigPara() { Code = CustomCode, ParagraphText = CUSTOM_PLACEHOLDER }));
                    foreach (var item in AppData.ConfigModel.ParagraphList(AppData.PropertyModel.SelectedVisit.Visit.Organisation, AppData.PropertyModel.SelectedVisit.GroupMod))
                    {
                        _mParagraphList.Add(new KeyValuePair<string, OnSiteConfigPara>("", item));
                    }
                }
                return _mParagraphList;
            }
        }

        /// <summary>
        /// Sets the paragraph type for use when a user selects a paragraph type from the list.
        /// </summary>
        /// <param name="listIndex">List index.</param>
        /// <param name="rowIndex">Table row index.</param>
        public void SetType(int listIndex, int rowIndex)
        {
            try
            {
                SelectedParagraphs[rowIndex].Type = modExtensions.ToAlphaIndex(listIndex);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }

        }

        /// <summary>
        /// Sets the paragraph for use when a user selects a paragraph from the list.
        /// </summary>
        /// <param name="listIndex">List index.</param>
        /// <param name="rowIndex">Table row index.</param>
        public void SetParagraph(int listIndex, int rowIndex)
        {
            try
            {
                var el = ParagraphList.ElementAt(listIndex);
                SelectedParagraphs[rowIndex].Key = el.Key;
                SelectedParagraphs[rowIndex].Paragraph = el.Value;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }

        }

        /// <summary>
        /// The selected paragraphs.
        /// </summary>
        public ObservableCollection<TypedParagraph> SelectedParagraphs = new ObservableCollection<TypedParagraph>();

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions

        /// <summary>
        /// Gets the List of paragraph description.
        /// </summary>
        /// <returns>A List of short human readable paragraph description.</returns>
        /// <param name="para">KeyvaluePair.</param>


        private string GetParaDesc(KeyValuePair<string, OnSiteConfigPara> para)
        {
            try
            {
                string p = para.Value.ParagraphPlainText;
                if (p == null) return null;
                return para.Value.Code + " - " + p.Substring(0, Math.Min(p.Length, ParaDescLen));
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }

        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
