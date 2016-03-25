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

		public static void getApps () 
		{
			var config = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration("com.SimpleBackgroundTransfer.BackgroundSession");
			var session = NSUrlSession.FromConfiguration(config, (NSUrlSessionDelegate) new NetworkDelegate(), new NSOperationQueue());
			var url = NSUrl.FromString("http://ax.itunes.apple.com/WebObjects/MZStoreServices.woa/ws/RSS/topfreeapplications/limit=25/json");
			var request = NSUrlRequest.FromUrl(url);
			var downloadTask = session.CreateDownloadTask (request);
			downloadTask.Resume ();
		}

		public static void getPodcasts ()
		{
			var config = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration("com.SimpleBackgroundTransfer.BackgroundSession");
			var session = NSUrlSession.FromConfiguration(config, (NSUrlSessionDelegate) new NetworkDelegate(), new NSOperationQueue());
			var url = NSUrl.FromString("https://itunes.apple.com/us/rss/toppodcasts/limit=25/json");
			var request = NSUrlRequest.FromUrl(url);
			var downloadTask = session.CreateDownloadTask (request);
			downloadTask.Resume ();
		}
	}
}

