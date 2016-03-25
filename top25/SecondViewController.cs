using System;
using UIKit;
using Foundation;

namespace top25
{
	public partial class SecondViewController : UIViewController
	{
		UITableView podcastsTableView { get; set; }

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
			NSNotificationCenter.DefaultCenter.AddObserver ((NSString)"getPodcastsFailed", alertErrorFetchingPodcasts);
		}

		void reloadTable (NSNotification notification)
		{
			InvokeOnMainThread (() => {
				podcastsTableView.ReloadData ();
			});
		}

		void alertErrorFetchingPodcasts (NSNotification notification)
		{
			InvokeOnMainThread (() => {
				
				Boolean isSelectedTab = this.TabBarController.SelectedIndex == 1;
				Console.WriteLine ("isPodcastTabSelected {0}", isSelectedTab);

				if (isSelectedTab) {
					UIAlertController alertController = UIAlertController.Create ("Notice", "Error pulling or updating data from apple. Please try again.", UIAlertControllerStyle.Alert);
					UIAlertAction okAction = UIAlertAction.Create ("OK", UIAlertActionStyle.Default, a => { 
						alertController.DismissViewController (true, null);
					});
					alertController.AddAction (okAction);
					PresentViewController(alertController, true, null);
				}

			});

		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

