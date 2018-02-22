using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.AppContext;
using ServiceRequest.UWP.DependencyClasses;

[assembly: Xamarin.Forms.Dependency(typeof(InputDialog))]
namespace ServiceRequest.UWP.DependencyClasses
{
    public class InputDialog : IInputDialog
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables and Properties
        /// ------------------------------------------------------------------------------------------------
        private TextBox _entry;
        private ContentDialog _dialog;
        /// ------------------------------------------------------------------------------------------------
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Methods
        /// ------------------------------------------------------------------------------------------------
        /// 

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ShowDialog
        /// 
        /// <summary>	Displaying a dialog to get the input string.</summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        public async Task<string> ShowDialog(string inputText)
        {
            try
            {
                string returnText = string.Empty;

                _dialog = new ContentDialog()
                {
                    Title = "Enter the Text",
                };

                var panel = new StackPanel();

                _entry = new TextBox()
                {
                    Text = inputText
                };

                panel.Children.Add(_entry);
                _dialog.Content = panel;

                // Add Buttons
                _dialog.PrimaryButtonText = "OK";

                _entry.TextChanged += EntryTextChanged;
                _dialog.IsPrimaryButtonEnabled = false;

                _dialog.PrimaryButtonClick += delegate
                {
                    returnText = _entry.Text;
                };

                _dialog.SecondaryButtonText = "Cancel";
                _dialog.SecondaryButtonClick += delegate
                {
                    returnText = string.Empty;
                };

                // Show Dialog
                var result = await _dialog.ShowAsync();
                if (result == ContentDialogResult.None)
                {
                    returnText = _entry.Text;
                }

                return returnText;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions
        /// ------------------------------------------------------------------------------------------------
        /// 

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		EntryTextChanged
        /// 
        /// <summary>	Text changed event to enable the submit button.</summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void EntryTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_entry.Text))
                {
                    _dialog.IsPrimaryButtonEnabled = false;
                }
                else
                {
                    _dialog.IsPrimaryButtonEnabled = true;
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
