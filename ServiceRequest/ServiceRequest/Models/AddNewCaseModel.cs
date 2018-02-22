using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ServiceRequest.Annotations;
using ServiceRequest.AppContext;

namespace ServiceRequest.Models
{
    public class AddNewCaseModel : INotifyPropertyChanged
    {
        private bool _status;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string NewCaseTradeName { get; set; }
        public string NewCaseAddressValue { get; set; }
        public bool ExistCaseAddress { get; set; }
        public bool ExistCaseRecievedDate { get; set; }
        public bool ExistCaseRecievedTime { get; set; }
        public bool ExistCaseDetails { get; set; }

        //public int NewCaseAddressIndex { get; set; }
        //public IList<string> NewCaseAddressItems { get; set; }
        public string NewCaseRequestTypeText { get; set; }
        public string NewInspectionTypeText { get; set; }
        //public IList<string> NewCaseRequestTypeItems { get; set; }
        public DateTime? NewCaseReceivedDate { get; set; }
        public DateTime? NewCaseReceivedMin { get; set; }
        public DateTime? NewCaseReceivedMax { get; set; }
        public TimeSpan? NewCaseReceivedTime { get; set; }
        public string NewCaseDetails { get; set; }

        public bool NewCaseIsOffline
        {
            get
            {
                if (Reachability.InternetConnectionStatus() == ReachabilityNetworkStatus.NotReachable)
                    _status = true;
                else
                    _status = false;
                return _status;
            }
            set { _status = value; }
        }

        public bool NewCaseIsOnline
        {
            get
            {
                if (Reachability.InternetConnectionStatus() != ReachabilityNetworkStatus.NotReachable)
                    _status = true;
                else
                    _status = false;

                return _status;
            }
            set { _status = value; }
        }

    }
}
