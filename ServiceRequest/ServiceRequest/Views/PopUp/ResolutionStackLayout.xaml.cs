using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Classes;
using Xamarin.Forms;

namespace ServiceRequest.Views.PopUp
{
    public partial class ResolutionStackLayout
    {
        public ResolutionStackLayout(List<SRiResolution> resolution)
        {
            InitializeComponent();
            Lbl_Title.Text = resolution.Any(x => x.Override.HasValue) ? "Conflicts" : "Deletions";
            Lst_Conflict.ItemsSource = resolution;
        }
    }
}
