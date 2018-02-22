using System.Collections.ObjectModel;

namespace ServiceRequest.Models
{
    public class GroupedRequestGroupModel : ObservableCollection<SRiRequestGroupModel>
    {
        public string GroupName { get; set; }
    }
}
