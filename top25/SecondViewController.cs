using System;
using UIKit;
using Foundation;

namespace top25
{
	public partial class SecondViewController : UIViewController
	{
		UITableView podcastsTableView { get; set;}

		public SecondViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();	

			podcastsTableView = new UITableView (new CoreGraphics.CGRect (0, 20, View.Bounds.Width, View.Bounds.Height - 70), UITableViewStyle.Grouped);
			podcastsTableView.Source = new PodcastsTableSouce ();
			Add (podcastsTableView);

			NSNotificationCenter.DefaultCenter.AddObserver ((NSString)"getPodcastsSuccess", reloadTable);
		}

		void reloadTable (NSNotification notification)
		{
			InvokeOnMainThread (() => {
				podcastsTableView.ReloadData();
			});
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

