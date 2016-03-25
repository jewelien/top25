using System;
using System.IO;
using UIKit;
using Newtonsoft.Json;
using Foundation;
using System.Collections;
using System.Linq;

namespace top25
{
	public class AppController
	{
		public NSArray appsList { get; set; }
		public NSString lastFetchedDateForAppsString { get; set;}

		private static AppController s_instance;

		public static AppController SharedInstance {
			get {
				if (s_instance == null)
					s_instance = new AppController ();

				return s_instance; 
			}
		}

		private AppController ()
		{
			NSString updateAppsKey = (NSString)"updateAppsList";
			NSNotificationCenter.DefaultCenter.AddObserver(updateAppsKey, updateAppsArrayFromFileWithDictionaryArray);
		}

		void updateAppsArrayFromFileWithDictionaryArray (NSNotification notification)
		{
			loadAppsFromFile ();
		}

		public void loadAppsFromFile()
		{
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			var filename = Path.Combine (documents, "app.json");
			NSData data = NSFileManager.DefaultManager.Contents (filename);
			if (data == null) {
				return;
			}
			NSError err;
			NSDictionary jsonDictionary = (NSDictionary)NSJsonSerialization.Deserialize (data, NSJsonReadingOptions.MutableContainers, out err);
			//			NSArray jsonArray = (NSArray)NSJsonSerialization.Deserialize (data, NSJsonReadingOptions.MutableContainers, out err);
			//			Console.WriteLine("date {0}", jsonDictionary["FetchedDateString"]);
			NSString fetchedDateString = (NSString)jsonDictionary["FetchedDateString"];
			this.lastFetchedDateForAppsString = fetchedDateString;

			//			Console.WriteLine("apps {0}", jsonArray["Apps"]);
			NSArray appsDictionaryArray = (NSArray)jsonDictionary ["Apps"];

			NSMutableArray appsArray = new NSMutableArray ();
			for (int i = 0; i < 25; i++) {
				nuint index = (nuint)i;
				NSDictionary appDictionary = appsDictionaryArray.GetItem<NSDictionary> (index);
				NSString title = (NSString)appDictionary ["Title"];
				NSString summary = (NSString)appDictionary ["Summary"];
				NSString imageURLString = (NSString)appDictionary ["IconURLString"];
				NSNumber rankFromDict = (NSNumber)appDictionary ["Rank"];
				//				int rank = rankFromDict.Int32Value;
				//				Console.WriteLine ("Create app, title:{0}, sumamry:{1}, url{2}, rank {3}", title, summary, imageURLString, rank);
				App newApp = new App (title, summary, imageURLString, rankFromDict);
				//				Console.WriteLine ("APP {0}, title:{1}, sumamry:{2}, url{3}", newApp, newApp.Title, newApp.Summary, newApp.AppIconURLString);
				appsArray.Add (newApp);
			} ;
			//			Console.WriteLine ("APPSARRAY {0}", appsArray);
			this.appsList = appsArray;
			NSNotificationCenter.DefaultCenter.PostNotificationName("getAppsSuccess", null);
		}

	}
}

