using System;
using Idox.LGDP.Apps.ServiceRequest.Client;
using Xamarin.Forms;
using System.Collections.Generic;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views.ViewCells
{
    public partial class PropertyHistoryViewCell
    {
        ///-------------------------------------------------------------------------------------------------
        #region Private Variables
        ///
        private readonly int _counter;
        private Dictionary<string, string> _dictionary;
        ///
        #endregion
        ///-------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Constructor
        ///-------------------------------------------------------------------------------------------------
        ///
        public PropertyHistoryViewCell(int index)
        {
            try
            {
                InitializeComponent();
                OnLoad(index);
                foreach (var item in _dictionary)
                {
                    //counter is a loop variable for determine the intention of the foreach loop.
                    _counter++;
                    if (!String.IsNullOrEmpty(item.Value))
                    {
                        var glKeyValue = new Grid()
                        {
                            RowDefinitions =
                        {
                            new RowDefinition {Height= new GridLength(97, GridUnitType.Star) },
                            new RowDefinition {Height= new GridLength(3, GridUnitType.Star) },
                        },

                            ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(25, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(74, GridUnitType.Star) }
                        },

                        };
                        glKeyValue.HeightRequest = 40;
                        var lblKey = new Label
                        {
                            TextColor = Styles.MediumText,
                            VerticalOptions = LayoutOptions.Center
                        };
                        var lblValue = new Label
                        {
                            TextColor = Color.Black,
                            VerticalOptions = LayoutOptions.Center,
                            LineBreakMode = LineBreakMode.TailTruncation
                        };
                        lblKey.Text = item.Key;
                        lblValue.Text = item.Value;
                        glKeyValue.Children.Add(lblKey, 1, 0);
                        glKeyValue.Children.Add(lblValue, 2, 0);

                        //Boxview not added for the last row
                        if (_counter != _dictionary.Count)
                        {
                            var boxView = new BoxView
                            {
                                Color = Styles.GroupedTableSeparator,
                                HeightRequest = 0.5,
                                Margin = new Thickness(15, 0, 0, 0)
                            };
                            glKeyValue.Children.Add(boxView, 0, 1);
                            Grid.SetColumnSpan(boxView, 3);
                        }
                        Sl_History.Children.Add(glKeyValue);
                    }
                }
                Sl_HistoryList.Children.Add(Sl_History);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// 
        /// -------------------------------------------------------------------------------------------------
        /// -------------------------------------------------------------------------------------------------
        ///  ------------------------------------------------------------------------------------------------
        ///  Name            OnLoad
        ///  
        ///  <summary>       Loads the data on loading.
        ///  </summary>
        ///  <param name="index"></param>
        /// ------------------------------------------------------------------------------------------------
        private void OnLoad(int index)
        {
            try
            {
                var propertyDetails = AppData.PropertyModel.SelectedProperty.GetPropertyHistory(index);
                // ReSharper disable once InvertIf
                if (propertyDetails != null)
                {
                    _dictionary = new Dictionary<string, string>
                {
                    {"Modules", propertyDetails.SubSys},
                    {"System", propertyDetails.SystemID},
                    {"Reference", propertyDetails.RefVal},
                    {"Description", propertyDetails.Description},
                    {"Status", propertyDetails.Status}
                };
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
