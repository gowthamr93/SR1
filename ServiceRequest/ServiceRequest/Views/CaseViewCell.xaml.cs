using System;
using ServiceRequest.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using Xamarin.Forms;

namespace ServiceRequest.Views
{
    public partial class CaseViewCell : Grid
    {
        public CaseViewCell(SRiProperty data, int alphabets)
        {
          InitializeComponent();

            SetSyncStatus(data.Status);
            BindingContext = new
            {
                Alphabet = alphabets.IndexToAlphabet(),
                TradeName=data.TradeName!=null?data.TradeName:data.Address.Lines[0],
                Address = data.Address.ShortAddress.Split(Environment.NewLine.ToCharArray())[0],
                //ApplicationNo = data.TradeName,
            };
         TapGestures(alphabets);

        }

        private void TapGestures(int caseIndex)
        {
            
            var tapCaseCell = new TapGestureRecognizer();
            tapCaseCell.Tapped += (s, e) => HubMasterView._caseListView.OnCellTouchUpInside(caseIndex);
            Gl_CaseCell.GestureRecognizers.Add(tapCaseCell);
        }

        public void SetSyncStatus(SyncStatus status)
        {
            switch (status)
            {
                case SyncStatus.Changed:
                    Img_NewStatus.IsVisible = false;
                    Img_DoneUpload.IsVisible = false;
                    Img_PendingUpload.IsVisible = true;
                    break;
                case SyncStatus.New:
                    Img_NewStatus.IsVisible = true;
                    Img_DoneUpload.IsVisible = false;
                    Img_PendingUpload.IsVisible = false;
                    break;
                case SyncStatus.Saved:
                    Img_NewStatus.IsVisible = false;
                    Img_DoneUpload.IsVisible = true;
                    Img_PendingUpload.IsVisible = false;
                    break;
                default:
                    Img_NewStatus.IsVisible = false;
                    Img_DoneUpload.IsVisible = false;
                    Img_PendingUpload.IsVisible = false;
                    break;
            }
        }

    }
}
