using System;
using Foundation;
using UIKit;

namespace top25
{
	public class Content : NSObject
	{
		public NSString Title { get; set; }
		public NSString Summary { get; set; }
		public UIImage IconImage { get; set; }
		public NSString IconURLString { get; set; }
		public NSNumber Rank { get; set; }
		public NSString URLString { get; set; }

		public enum ContentType {
			Application = 0,
			Podcast = 1
		}
		public ContentType TypeOfContent { get; set;}

		public Content (NSString title, NSString summary, NSString iconURLString, NSNumber rank, NSString urlString, ContentType typeOfContent)
		{
			Title = title;
			Summary = summary;
			IconURLString = iconURLString;
			IconImage = FromUrl (iconURLString);
			Rank = rank;
			URLString = urlString;
			TypeOfContent = typeOfContent;
		}

		static UIImage FromUrl (NSString uri)
		{
			using (var url = new NSUrl (uri))
			using (var data = NSData.FromUrl (url))
				if (data != null) {
					return UIImage.LoadFromData (data);
				} else {
					return new UIImage ();
				}
		}
	}
}

