namespace ServiceRequest.Models
{
    public class SRiRequestGroupModel
    {
        public string GroupDetails { get; set; }
        public string TargetResponseDate { get; set; }
        public string UploadStatusImage { get; set; }
        public ShortIndexMapping ShortIndex { get; set; }
        public bool GrayLine { get; set; }

    }
}
