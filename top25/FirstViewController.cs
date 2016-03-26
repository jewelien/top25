using System;
using UIKit;
using Foundation;

namespace top25
{
	public partial class FirstViewController : UIViewController
	{

		UITableView appsTableView { get; set; }

		public FirstViewController (IntPtr handle) : base (handle)
		{
		}
			
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			appsTableView = new UITableView (new CoreGraphics.CGRect (0, 20, View.Bounds.Width, View.Bounds.Height - 70), UITableViewStyle.Grouped);
			appsTableView.Source = new AppsTableSource ();
			Add (appsTableView);

			NSNotificationCenter.DefaultCenter.AddObserver ((NSString)"getAppsSuccess", reloadTable);
			NSNotificationCenter.DefaultCenter.AddObserver ((NSString)"getAppsFailed", alertErrorFetchingApps);
			NSNotificationCenter.DefaultCenter.AddObserver ((NSString)"appInfoTapped", infoTappedAlert);
		}

		void reloadTable (NSNotification notification)
		{
			InvokeOnMainThread (() => {
				appsTableView.ReloadData ();
			});
		}

		void alertErrorFetchingApps (NSNotification notification)
		{
			InvokeOnMainThread (() => {
				Boolean isSelectedTab = this.TabBarController.SelectedIndex == 0;
				Console.WriteLine ("isAppTabSelected {0}", isSelectedTab);

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

		void infoTappedAlert (NSNotification notification)
		{
			App selectedApp = (App)notification.Object;
			UIAlertController okAlertController = UIAlertController.Create (selectedApp.Title, selectedApp.Summary, UIAlertControllerStyle.Alert);
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

