using System;
using Foundation;
using UIKit;

namespace top25
{
	public class Podcast :NSObject
	{

		public NSString Title { get; set; }
		public NSString Summary { get; set; }
		public UIImage IconImage { get; set; }
		public NSString IconURLString { get; set; }
		public NSNumber Rank { get; set;}

		public Podcast (NSString title, NSString summary, NSString iconURLString, NSNumber rank)
		{
			Title = title;
			Summary = summary;
			IconURLString = iconURLString;
			IconImage = FromUrl (iconURLString);
			Rank = rank;
		}

		static UIImage FromUrl (NSString uri)
		{
			//			Console.WriteLine ("uri: {0}", uri);
			using (var url = new NSUrl (uri))
				//			Console.WriteLine ("url: {0}", url);
			using (var data = NSData.FromUrl (url))
				//				Console.WriteLine ("daya: {0}", data);
				if (data != null) {
					return UIImage.LoadFromData (data);
				} else {
					return new UIImage ();
				}
		}
	}
}

