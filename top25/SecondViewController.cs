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

			UIImage podcastImage = UIImage.FromBundle("Podcast.png");
			podcastTabBarItem.Image = GlobalMethods.SharedInstance.ResizeImage(podcastImage, 30, 30);

			NSNotificationCenter.DefaultCenter.AddObserver ((NSString)"getPodcastsSuccess", reloadTable);
			NSNotificationCenter.DefaultCenter.AddObserver ((NSString)"getPodcastsFailed", alertErrorFetchingPodcasts);
			NSNotificationCenter.DefaultCenter.AddObserver ((NSString)"podcastInfoTapped", infoTappedAlert);
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
					PresentViewController(GlobalMethods.SharedInstance.genericErrorAlertController(), true, null);
				}

			});
		}

		void infoTappedAlert (NSNotification notification)
		{
			Content selectedPodcast = (Content)notification.Object;
			UIAlertController okAlertController = UIAlertController.Create (selectedPodcast.Title, selectedPodcast.Summary, UIAlertControllerStyle.Alert);
			okAlertController.AddAction(UIAlertAction.Create("close", UIAlertActionStyle.Default, null));
			PresentViewController (okAlertController, true, null);
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

