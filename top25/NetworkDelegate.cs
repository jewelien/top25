using System;
using Foundation;
using System.IO;
using UIKit;
using Newtonsoft.Json;
using System.Collections;

namespace top25
{
	public class NetworkDelegate : NSUrlSessionDownloadDelegate
	{
		public NetworkDelegate ()
		{
		}
			
		//		public override void DidWriteData (NSUrlSession session, NSUrlSessionDownloadTask downloadTask, long bytesWritten, long totalBytesWritten, long totalBytesExpectedToWrite)
		//		{
		//			Console.WriteLine (string.Format ("URLSession: {0}  downloadTask: {1}   bytesWritten:{0}    totalBytesWritten:{0}   bytesExpected:{0}", session, downloadTask, bytesWritten, totalBytesWritten, totalBytesExpectedToWrite ));
		////			Console.WriteLine (string.Format ("DownloadTask: {0}  progress: {1}", downloadTask, progress));
		////			InvokeOnMainThread( () => {
		////				// update UI with progress bar, if desired
		////			});
		//		}

		public override void DidFinishDownloading (NSUrlSession session, NSUrlSessionDownloadTask downloadTask, NSUrl location)
		{
//			Console.WriteLine (string.Format ("DownloadTask: {0}  location: {1}", downloadTask, location));
			NSData data = NSFileManager.DefaultManager.Contents (location.Path);
			if (data == null) {
				return;
			}
			NSError err;
			NSDictionary jsonData = (NSDictionary)NSJsonSerialization.Deserialize (data, NSJsonReadingOptions.MutableContainers, out err);
			if (err != null) {
				return;
			}
			Console.WriteLine ("jsonData: {0}", jsonData["feed"]);
			NSDictionary feed = (NSDictionary)jsonData ["feed"];
			var feedToSave = jsonData ["feed"];
			//save to file with date pulled
			NSArray entries = (NSArray)feed ["entry"];
//			Console.WriteLine ("entries: {0}", entries);

			NSArray contentArray = updatedContentDictionaryArrayFromServerArray(entries);
//			Console.WriteLine ("contentArray {0}", appNSArray);

			NSDictionary firstItem = entries.GetItem <NSDictionary>(0);
			NSDictionary content = (NSDictionary)firstItem ["im:contentType"];
			NSDictionary contentAttr = (NSDictionary)content ["attributes"];
			NSString contentType = (NSString)contentAttr["label"];
//			Console.WriteLine ("isEqual: {0}",contentType.IsEqual((NSString)"Application") );
			if (contentType.IsEqual ((NSString)"Application")) {
				saveAppsToFile (contentArray);
			} else {
				savePodcastsToFile (contentArray);
			}
		}

		void saveAppsToFile (NSArray appsArray) 
		{
			NSMutableDictionary dictionaryToSave = new NSMutableDictionary ();
			NSString dateString = (NSString)(string.Format("{0}", DateTime.Now.ToLocalTime()));
			NSString dateKey = (NSString)"FetchedDateString";
			NSString appsKey = (NSString)"Apps";
			dictionaryToSave.Add (dateKey, dateString);
			dictionaryToSave.Add (appsKey, appsArray);
//			Console.WriteLine ("dictToSave: {0}", dictionaryToSave);

			var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			var filename = Path.Combine (documents, "app.json");
			//Delete app file if existing
			File.Delete (filename);
			//Save apps to app file
			NSError error = new NSError();
			var jsonSerialized = NSJsonSerialization.Serialize(dictionaryToSave, NSJsonWritingOptions.PrettyPrinted, out error);
			var jsonString = jsonSerialized.ToString ();
			File.WriteAllText(filename, jsonString);

			NSNotificationCenter.DefaultCenter.PostNotificationName("updateAppsList", null);
		}

		void savePodcastsToFile (NSArray podcastsArray)
		{
			NSMutableDictionary dictionaryToSave = new NSMutableDictionary ();
			NSString dateString = (NSString)(string.Format("{0}", DateTime.Now.ToLocalTime()));
			NSString dateKey = (NSString)"FetchedDateString";
			NSString appsKey = (NSString)"Podcasts";
			dictionaryToSave.Add (dateKey, dateString);
			dictionaryToSave.Add (appsKey, podcastsArray);
//			Console.WriteLine ("dictToSave: {0}", dictionaryToSave);

			var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			var filename = Path.Combine (documents, "podcast.json");
			//Delete app file if existing
			File.Delete (filename);
			//Save apps to app file
			NSError error = new NSError();
			var jsonSerialized = NSJsonSerialization.Serialize(dictionaryToSave, NSJsonWritingOptions.PrettyPrinted, out error);
			var jsonString = jsonSerialized.ToString ();
			File.WriteAllText(filename, jsonString);

			NSNotificationCenter.DefaultCenter.PostNotificationName("updatePodcastsList", null);
		}

		private static NSArray updatedContentDictionaryArrayFromServerArray(NSArray dictionaryArray)
		{
			NSMutableArray contentArray = new NSMutableArray ();
			for (int i = 0; i < 25; i++) {
				nuint index = (nuint)i;
				NSDictionary entry = dictionaryArray.GetItem<NSDictionary> (index);
				NSDictionary titleDict = (NSDictionary)entry ["im:name"];
				NSString title = (NSString)titleDict ["label"];
				NSDictionary summaryDict = (NSDictionary)entry ["summary"];
				NSString summary = (NSString)summaryDict ["label"];
				NSArray imagesArray = (NSArray)entry ["im:image"];
				NSDictionary imageDictionary = imagesArray.GetItem<NSDictionary> (0);
				NSString imageURLString = (NSString)imageDictionary ["label"];
				NSNumber rank = new NSNumber(i + 1);
				//				Console.WriteLine (string.Format (@"title: {0}, summary:{1}, url:{2}", title, summary, imageURLString));
//				App newApp = new App (title, summary, imageURLString, rank);

				NSMutableDictionary contentToDict = new NSMutableDictionary ();
				NSString titleKey = (NSString)"Title";
				NSString summaryKey = (NSString)"Summary";
				NSString urlKey = (NSString)"IconURLString";
				NSString rankKey = (NSString)"Rank";
				contentToDict.Add (titleKey, title);
				contentToDict.Add (summaryKey, summary);
				contentToDict.Add (urlKey, imageURLString);
				contentToDict.Add (rankKey, rank);
//				Console.WriteLine ("appValidJSON? : {0}", NSJsonSerialization.IsValidJSONObject(serialized));

				contentArray.Add (contentToDict);
			};
			return contentArray;
		}
	
	}
}

