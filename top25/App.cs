using System;
using UIKit;
using Foundation;
using Newtonsoft.Json;

namespace top25
{
	public class App : NSObject
	{
		public NSString Title { get; set; }
		public NSString Summary { get; set; }
		public UIImage AppIcon { get; set; }
		public NSString AppIconURLString { get; set; }
		public NSNumber Rank { get; set;}
		public NSString URLString { get; set;}
		public App (NSString title, NSString summary, NSString appIconURLString, NSNumber rank, NSString urlString)
		{
			Title = title;
			Summary = summary;
			AppIcon = FromUrl (appIconURLString);
			AppIconURLString = appIconURLString;
			Rank = rank;
			URLString = urlString;
		}

		static UIImage FromUrl (NSString uri)
		{
//			Console.WriteLine ("uri: {0}", uri);
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

