using System;
using UIKit;
using Foundation;


namespace top25
{
	public class NetworkController
	{
		public NetworkController ()
		{
		}

		public static void getContent(Content.ContentType typeOfContent)
		{
			NSString urlString;
			if (typeOfContent == Content.ContentType.Application) {
				urlString = (NSString)"http://ax.itunes.apple.com/WebObjects/MZStoreServices.woa/ws/RSS/topfreeapplications/limit=25/json";
			} else {
				urlString = (NSString)"https://itunes.apple.com/us/rss/toppodcasts/limit=25/json";
			}
			var config = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration("com.SimpleBackgroundTransfer.BackgroundSession");
			var session = NSUrlSession.FromConfiguration(config, (NSUrlSessionDelegate) new NetworkDelegate(), new NSOperationQueue());
			var url = NSUrl.FromString(urlString);
			var request = NSUrlRequest.FromUrl(url);
			var downloadTask = session.CreateDownloadTask (request);
			downloadTask.Resume ();
		}
	}
}

