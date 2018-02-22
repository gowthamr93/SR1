using System;
using System.Collections.Generic;
using System.Linq;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Custom;
using ServiceRequest.Pages;
using ServiceRequest.ViewModels;
using Xamarin.Forms;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views.ViewCells
{
    public partial class ParagraphInputViewCell
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables and Properties
        ///
        private readonly WrapLayoutOld _container;
        private bool _isChanged;
        private List<string> _insertValues, _splittedString;
        private SRiActionParagraph _sRiActionParagraph;
        private readonly ScrollView _scroll;
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public variables and Properties
        ///
        public List<HyperLinkLabel> LstHyperLinkLabels { get; set; }
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        ///
        public ParagraphInputViewCell()
        {
            try
            {
                InitializeComponent();
                TapGestureRecognizer tapDelete = new TapGestureRecognizer();
                Img_Delete.GestureRecognizers.Add(tapDelete);
                tapDelete.Tapped += OnDeleteTapped;

                BindingContextChanged += OnBindingContextChanged;
                _isChanged = true;
                _insertValues = new List<string>();
                _splittedString = new List<string>();
                LstHyperLinkLabels = new List<HyperLinkLabel>();

                _container = new WrapLayoutOld
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    Orientation = StackOrientation.Horizontal,
                    //Padding = 10,
                    Spacing = 5,
                    IsClippedToBounds = true,
                };
                _scroll = new ScrollView
                {
                    Orientation = ScrollOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Content = _container,
                    IsClippedToBounds = true,
                };

                //StackLayout stack = new StackLayout
                //{
                //    Orientation = StackOrientation.Vertical,
                //    VerticalOptions = LayoutOptions.FillAndExpand,
                //    HorizontalOptions = LayoutOptions.FillAndExpand
                //};
                //ScrollView scroll = new ScrollView
                //{
                //    Orientation = ScrollOrientation.Vertical,
                //    Content = _container

                //};

                //stack.Children.Add(scroll);

                Gl_Main.Children.Add(_scroll, 2, 0);

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnBindingContextChanged
        /// 
        /// <summary>
        /// Bind the values
        /// </summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_isChanged)
                {
                    _sRiActionParagraph = BindingContext as SRiActionParagraph;


                    if (_sRiActionParagraph != null)
                    {
                        _insertValues = _sRiActionParagraph.InsertsList;
                        _splittedString = _sRiActionParagraph.PlainText.Split(new[] { VisitActionDetailsPage.INSERT_MARKER }, StringSplitOptions.None).ToList();
                        SetValues();
                    }
                }
                _isChanged = false;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// ------------------------------------------------------------------------------------------------
        /// Name		SetValues
        /// 
        /// <summary>
        /// Given input will update
        /// </summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void SetValues()
        {
            try
            {
                while (_insertValues.Count < _splittedString.Count - 1)
                    _insertValues.Add(ParagraphViewModel.DEFAULT_VALUE_TEXT);
                RefreshText();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        /// ------------------------------------------------------------------------------------------------
        /// Name		RefreshText
        /// 
        /// <summary>
        /// Refresh the text added new
        /// </summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void RefreshText()
        {
            try
            {
                for (int i = 0; i < _splittedString.Count; i++)
                {
                    _container.Children.Add(new Label() { Text = _splittedString[i], TextColor = Color.Black, LineBreakMode = LineBreakMode.TailTruncation });

                    if (i != _splittedString.Count - 1)
                    {
                        if (i < _insertValues.Count)
                        {
                            var hyperlinkLabel = new HyperLinkLabel($"{_insertValues[i]}")
                            {
                                TextChanged = () =>
                                {
                                    _sRiActionParagraph.InsertsList = LstHyperLinkLabels.Select(x => x.GetText).ToList();
                                    _sRiActionParagraph.Modified = true;
                                }
                            };
                            LstHyperLinkLabels.Add(hyperlinkLabel);
                            _container.Children.Add(hyperlinkLabel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// ------------------------------------------------------------------------------------------------
        /// Name            OnDeleteTapped
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// <summary>       Delete the view cell.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void OnDeleteTapped(object sender, EventArgs e)
        {
            try
            {
                if (_sRiActionParagraph != null)
                {
                    VisitActionDetailsPage.CurrentInstance.OnDelete(_sRiActionParagraph);
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
