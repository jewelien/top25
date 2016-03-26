// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace top25
{
	[Register ("SecondViewController")]
	partial class SecondViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITabBarItem podcastTabBarItem { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (podcastTabBarItem != null) {
				podcastTabBarItem.Dispose ();
				podcastTabBarItem = null;
			}
		}
	}
}
