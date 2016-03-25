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

		public App (NSString title, NSString summary, NSString appIconURLString, NSNumber rank)
		{
//			Console.WriteLine ("title:{0}    summary:{1}    appIconURLString:{2}", title, summary, appIconURLString);
			Title = title;
			Summary = summary;
			AppIcon = FromUrl (appIconURLString);
			AppIconURLString = appIconURLString;
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

