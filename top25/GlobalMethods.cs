using System;
using UIKit;
using System.Drawing;

namespace top25
{
	public class GlobalMethods
	{
		private static GlobalMethods s_instance;
		public Boolean isAppsFirstLaunch;

		public static GlobalMethods SharedInstance {
			get {
				if (s_instance == null)
					s_instance = new GlobalMethods ();

				return s_instance; 
			}
		}

		public GlobalMethods ()
		{
		}

		public UIImage ResizeImage(UIImage sourceImage, float width, float height)
		{
			UIGraphics.BeginImageContext(new SizeF(width, height));
			sourceImage.Draw(new RectangleF(0, 0, width, height));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return resultImage;
		}

		public UIAlertController genericErrorAlertController () {
			UIAlertController alertController = UIAlertController.Create ("Notice", "Error pulling or updating data from apple. Please try again.", UIAlertControllerStyle.Alert);
			UIAlertAction okAction = UIAlertAction.Create ("OK", UIAlertActionStyle.Default, null);
			alertController.AddAction (okAction);
			return alertController;
		}
	}
}

