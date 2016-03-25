using System;
using UIKit;
using Foundation;

namespace top25
{
	public partial class FirstViewController : UIViewController
	{


		public FirstViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
//			appsTableView = new UITableView(View.Bounds);
//			appsTableView.Source = new AppsTableSource();
//			appsTableView = new UITableView {
//				Frame = new CoreGraphics.CGRect (0, 20, View.Bounds.Width, View.Bounds.Height - 70),
//				Source = new AppsTableSource ()
//			};
			appsTableView = new UITableView (new CoreGraphics.CGRect (0, 20, View.Bounds.Width, View.Bounds.Height - 70), UITableViewStyle.Grouped);
			appsTableView.Source = new AppsTableSource ();
			Add (appsTableView);

			NSString getAppsSuccess = (NSString)"getAppsSuccess";
			NSNotificationCenter.DefaultCenter.AddObserver (getAppsSuccess, reloadTable);
		}

		void reloadTable (NSNotification notification)
		{
			InvokeOnMainThread (() => {
				appsTableView.ReloadData ();
			});
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

