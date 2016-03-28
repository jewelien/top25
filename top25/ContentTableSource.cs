using System;
using Foundation;
using UIKit;

namespace top25
{
	public class ContentTableSource : UITableViewSource
	{
		Content.ContentType contentType;
		UIView tableHeaderView;
		UIButton refreshButton;
		UIActivityIndicatorView indicatorView;
		private NSArray contentList {
			get {
				if (contentType == Content.ContentType.Application) {
					return ContentController.SharedInstance.appsList;
				} else {
					return ContentController.SharedInstance.podcastsList;
				}
			}
		}
		private NSString lastFetchedDateString {
			get {
				if (contentType == Content.ContentType.Application) {
					return (NSString)(string.Format("fetched: {0}",ContentController.SharedInstance.lastFetchedDateForAppsString));
				} else {
					return (NSString)(string.Format("fetched: {0}",ContentController.SharedInstance.lastFetchedDateForPodcastsString));
				}
			}
		}

		public ContentTableSource (Content.ContentType typeOfContent)
		{
			contentType = typeOfContent;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return 25;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 80;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell ("cell");
			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Subtitle, "cell");
			}
			if (contentList != null && contentList.Count > 0) {
				uint indexRow = (uint)indexPath.Row;

				Content contentAtIndex = contentList.GetItem<Content> (indexRow);
				cell.TextLabel.Text = (NSString)string.Format("{0}. {1}",contentAtIndex.Rank, contentAtIndex.Title);
				cell.DetailTextLabel.Text = contentAtIndex.Summary;
				cell.ImageView.Image = contentAtIndex.IconImage;
				cell.Accessory = UITableViewCellAccessory.DetailButton;
				cell.DetailTextLabel.Lines = 3;
			}
			return cell;
		}

		public override void AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
		{
			uint indexRow = (uint)indexPath.Row;
			Content app = contentList.GetItem<Content> (indexRow);
			if (contentType == Content.ContentType.Application) {
				NSNotificationCenter.DefaultCenter.PostNotificationName ("appInfoTapped", app);
			} else {
				NSNotificationCenter.DefaultCenter.PostNotificationName ("podcastInfoTapped", app);
			}
		}

		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			uint indexRow = (uint)indexPath.Row;
			Content app = contentList.GetItem<Content> (indexRow);
			NSUrl appURL = new NSUrl (app.URLString);
			if (UIApplication.SharedApplication.CanOpenUrl(appURL)) {
				UIApplication.SharedApplication.OpenUrl (appURL);
			};

			tableView.DeselectRow (indexPath, true);
		}

		public override nfloat GetHeightForHeader (UITableView tableView, nint section)
		{
			return 50;
		}

		public override UIView GetViewForHeader (UITableView tableView, nint section)
		{
			float screenWidth = (float) UIScreen.MainScreen.ApplicationFrame.Size.Width;
			tableHeaderView = new UIView (new CoreGraphics.CGRect (0, 0, screenWidth, 50));
			UILabel titleLabel = new UILabel (new CoreGraphics.CGRect (10,0, screenWidth - 55, tableHeaderView.Frame.Size.Height));
			titleLabel.Text = lastFetchedDateString;
			tableHeaderView.Add (titleLabel);

			refreshButton = new UIButton (new CoreGraphics.CGRect (screenWidth - 40, tableHeaderView.Frame.Size.Height /2 -15, 30, 30));
			refreshButton.SetImage (UIImage.FromBundle("Refresh.png"), UIControlState.Normal);
			refreshButton.TouchUpInside += RefreshButton_TouchUpInside;
			tableHeaderView.Add (refreshButton);
			if (indicatorView != null && indicatorView.IsAnimating) {
				indicatorView.StopAnimating ();
			}

			return tableHeaderView;
		}

		void RefreshButton_TouchUpInside (object sender, EventArgs e)
		{
			float screenWidth = (float) UIScreen.MainScreen.ApplicationFrame.Size.Width;
			UIImage noImage = new UIImage ();
			refreshButton.SetImage (noImage, UIControlState.Normal);

			indicatorView = new UIActivityIndicatorView (frame: new CoreGraphics.CGRect (screenWidth - 40, tableHeaderView.Frame.Size.Height /2 -15, 30, 30));
			indicatorView.Color = UIColor.DarkGray;
			tableHeaderView.Add (indicatorView);
			indicatorView.StartAnimating ();

			if (contentType == Content.ContentType.Application) {
				NetworkController.getContent (Content.ContentType.Application);
			} else {
				NetworkController.getContent (Content.ContentType.Podcast);
			}

		}
	}
}

