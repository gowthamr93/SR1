using Xamarin.Forms.Maps;

namespace ServiceRequest.Custom
{
    /// <summary>  Custom Pin that is used for the Map.
    /// </summary>
    /// ------------------------------------------------------------------------------------------------
    /// 
    public class CustomPin
    {

        public Pin Pin { get; set; }

        public string Id { get; set; }

        public string Url { get; set; }

        public string ImageUrl { get; set; }
    }
}
