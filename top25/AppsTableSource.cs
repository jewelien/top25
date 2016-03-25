using System;
using UIKit;
using Foundation;

namespace top25
{
	public class AppsTableSource : UITableViewSource
	{
		public NSArray appsList {
			get {
				return AppController.SharedInstance.appsList;
			}
		}

		public AppsTableSource ()
		{

		}


		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return 25;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell ("cell");
			if (cell == null) 
				cell = new UITableViewCell (UITableViewCellStyle.Subtitle, "cell");

			if (appsList != null && appsList.Count > 0) {
//				Console.WriteLine ("{0}", appsList);
				uint indexRow = (uint)indexPath.Row;
//				Console.WriteLine ("indexRow {0} count {1}", indexRow, appsList.Count);
				App app = appsList.GetItem<App> (indexRow);
//				Console.WriteLine ("App: {0}, title: {1}, summary:{2}, url:{3}", app, app.Title, app.Summary, app.AppIconURLString);
				cell.TextLabel.Text = app.Title;
				cell.DetailTextLabel.Text = app.Summary;
				cell.ImageView.Image = app.AppIcon;
			}
			return cell;
		}

		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			tableView.DeselectRow (indexPath, true);
		}

		public override string TitleForHeader (UITableView tableView, nint section)
		{
			NSString dateString = (NSString)(string.Format("last fetched: {0}",AppController.SharedInstance.lastFetchedDateForAppsString));
			return dateString;
		}

		public override UIView GetViewForHeader (UITableView tableView, nint section)
		{
			float screenWidth = (float) UIScreen.MainScreen.ApplicationFrame.Size.Width;
			UIView tableHeaderView = new UIView (new CoreGraphics.CGRect (0, 0, screenWidth, 50));
			UILabel titleLabel = new UILabel (new CoreGraphics.CGRect (10,0, screenWidth - 55, tableHeaderView.Frame.Size.Height));
//			titleLabel.BackgroundColor = UIColor.Yellow;
			NSString dateString = (NSString)(string.Format("fetched: {0}",AppController.SharedInstance.lastFetchedDateForAppsString));
			titleLabel.Text = dateString;
			tableHeaderView.Add (titleLabel);
			UIButton refreshButton = new UIButton (new CoreGraphics.CGRect (screenWidth - 40, tableHeaderView.Frame.Size.Height /2 -15, 30, 30));
			refreshButton.SetImage (UIImage.FromBundle("Refresh.png"), UIControlState.Normal);
			refreshButton.TouchUpInside += RefreshButton_TouchUpInside;
//			refreshButton.BackgroundColor = UIColor.Red;
			tableHeaderView.Add (refreshButton);
			return tableHeaderView;
		}

		void RefreshButton_TouchUpInside (object sender, EventArgs e)
		{
			NetworkController.getApps ();
		}
	}
}

