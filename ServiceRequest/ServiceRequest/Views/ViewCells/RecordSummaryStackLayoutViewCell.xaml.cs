using ServiceRequest.AppContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ServiceRequest.Views.ViewCells
{
    public partial class RecordSummaryStackLayoutViewCell : ViewCell
    {
        public RecordSummaryStackLayoutViewCell()
        {
            InitializeComponent();
        }

        protected void OnBindingContextChanged()
        {
			try
			{
				base.OnBindingContextChanged();
				if (Lbl_Address.Text == "Edit Record")
					Lbl_Address.TextColor = Styles.MainAccent;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
			}
        }
    }
}
