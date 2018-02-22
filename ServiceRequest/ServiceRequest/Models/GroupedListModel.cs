namespace ServiceRequest.Models
{
    public class GroupedListModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string CodeSort => Code[0].ToString();
    }
}
