using System;
using UIKit;
using Foundation;

namespace top25
{
	public class PodcastsTableSouce : UITableViewSource
	{
		private NSArray podcastsList {
			get {
				return PodcastController.SharedInstance.podcastsList;
			}
		}

		public PodcastsTableSouce ()
		{
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return 25;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell("podcastCell");
			cell = new UITableViewCell (UITableViewCellStyle.Subtitle, "podastCell");
			if (podcastsList != null && podcastsList.Count > 0) {
				uint indexRow = (uint)indexPath.Row;
				Podcast podcast = podcastsList.GetItem<Podcast>(indexRow);
				cell.TextLabel.Text = podcast.Title;
				cell.DetailTextLabel.Text = podcast.Summary;
				cell.ImageView.Image = podcast.IconImage;
			}
			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			uint indexRow = (uint)indexPath.Row;
			Podcast podcast = podcastsList.GetItem<Podcast> (indexRow);
			NSUrl podcastURL = new NSUrl (podcast.URLString);
			if (UIApplication.SharedApplication.CanOpenUrl(podcastURL)) {
				UIApplication.SharedApplication.OpenUrl (podcastURL);
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
			UIView tableHeaderView = new UIView (new CoreGraphics.CGRect (0, 0, screenWidth, 50));
			UILabel titleLabel = new UILabel (new CoreGraphics.CGRect (10,0, screenWidth - 55, tableHeaderView.Frame.Size.Height));
			NSString dateString = (NSString)(string.Format("fetched: {0}",PodcastController.SharedInstance.lastFetchedDateForPodcastsString));
			titleLabel.Text = dateString;
			tableHeaderView.Add (titleLabel);
			UIButton refreshButton = new UIButton (new CoreGraphics.CGRect (screenWidth - 40, tableHeaderView.Frame.Size.Height /2 -15, 30, 30));
			refreshButton.SetImage (UIImage.FromBundle("Refresh.png"), UIControlState.Normal);
			refreshButton.TouchUpInside += RefreshButton_TouchUpInside;;
			tableHeaderView.Add (refreshButton);
			return tableHeaderView;
		}

		void RefreshButton_TouchUpInside (object sender, EventArgs e)
		{
			NetworkController.getPodcasts();
		}
	}
}

