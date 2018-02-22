using System;
using MapKit;

namespace ServiceRequest.iOS
{
	public class CustomMKAnnotationView:MKAnnotationView
	{
		public string Id { get; set; }

		public string Url { get; set; }
		public static string Identifier;

		public CustomMKAnnotationView(IMKAnnotation annotation, string id)
			: base(annotation, id)
		{
		}


	}
}
