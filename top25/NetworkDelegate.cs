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

		public override void DidFinishDownloading (NSUrlSession session, NSUrlSessionDownloadTask downloadTask, NSUrl location)
		{
			NSData data = NSFileManager.DefaultManager.Contents (location.Path);
			if (data == null) {
				alertUserOfError ();
				return;
			}
			NSError err;
			NSDictionary jsonData = (NSDictionary)NSJsonSerialization.Deserialize (data, NSJsonReadingOptions.MutableContainers, out err);
			if (err != null) {
				alertUserOfError ();
				Console.WriteLine ("error with data while json deserialization: \n \"{0}\" ", err);
				return;
			}
			NSDictionary feed = (NSDictionary)jsonData ["feed"];
			var feedToSave = jsonData ["feed"];
			NSArray entries = (NSArray)feed ["entry"];
			NSArray contentArray = updatedContentDictionaryArrayFromServerArray(entries);

			NSDictionary firstItem = entries.GetItem <NSDictionary>(0);
			NSDictionary content = (NSDictionary)firstItem ["im:contentType"];
			NSDictionary contentAttr = (NSDictionary)content ["attributes"];
			NSString contentType = (NSString)contentAttr["label"];
			if (contentType.IsEqual ((NSString)"Application")) {
				saveContentToFileOfType (contentArray, Content.ContentType.Application);
			} else {
				saveContentToFileOfType (contentArray, Content.ContentType.Podcast);
			}
		}

		void alertUserOfError()
		{
			NSNotificationCenter.DefaultCenter.PostNotificationName((NSString)"getAppsFailed", null);
			NSNotificationCenter.DefaultCenter.PostNotificationName((NSString)"getPodcastsFailed", null);
		}

		void saveContentToFileOfType(NSArray contentArray, Content.ContentType typeOfContent)
		{

			NSMutableDictionary dictionaryToSave = new NSMutableDictionary ();
			NSString dateString = (NSString)(string.Format("{0}", DateTime.Now.ToLocalTime()));
			NSString dateKey = (NSString)"FetchedDateString";
			NSString appsKey;
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			NSString filename;

			if (typeOfContent == Content.ContentType.Application) {
				appsKey = (NSString)"Apps";
				filename = (NSString)Path.Combine (documents, "app.json");
			} else {
				appsKey = (NSString)"Podcasts";
				filename = (NSString)Path.Combine (documents, "podcast.json");
			}

			dictionaryToSave.Add (dateKey, dateString);
			dictionaryToSave.Add (appsKey, contentArray);

			//Delete app file if existing
			File.Delete (filename);
			//Save apps to app file
			NSError error = new NSError();
			var jsonSerialized = NSJsonSerialization.Serialize(dictionaryToSave, NSJsonWritingOptions.PrettyPrinted, out error);
			if (error != null) {
				alertUserOfError ();
				Console.WriteLine ("error saving content to file: {0}, ContentType {1}", error, typeOfContent);
			}
			var jsonString = jsonSerialized.ToString ();
			File.WriteAllText(filename, jsonString);

			if (typeOfContent == Content.ContentType.Application) {
				NSNotificationCenter.DefaultCenter.PostNotificationName("updateAppsList", null);
			} else {
				NSNotificationCenter.DefaultCenter.PostNotificationName("updatePodcastsList", null);
			}

		}

		private static NSArray updatedContentDictionaryArrayFromServerArray(NSArray dictionaryArray)
		{
			NSMutableArray contentArray = new NSMutableArray ();
			for (int i = 0; i < 25; i++) {
				nuint index = (nuint)i;
				NSDictionary entry = dictionaryArray.GetItem<NSDictionary> (index);
				NSDictionary titleDict = (NSDictionary)entry ["im:name"];
				NSString title = (NSString)titleDict ["label"];
				NSString titleTest = (NSString)((NSDictionary)entry ["im:name"])["label"];
				Console.WriteLine ("titleTest: {0}", titleTest);
				NSDictionary summaryDict = (NSDictionary)entry ["summary"];
				NSString summary = (NSString)summaryDict ["label"];
				NSArray imagesArray = (NSArray)entry ["im:image"];
				NSDictionary imageDictionary = imagesArray.GetItem<NSDictionary> (0);
				NSString imageURLString = (NSString)imageDictionary ["label"];
				NSNumber rank = new NSNumber(i + 1);
				NSDictionary idDictionary = (NSDictionary)entry["id"];
				NSString linkToContent = (NSString)idDictionary["label"];

				NSMutableDictionary contentToDict = new NSMutableDictionary ();
				NSString titleKey = (NSString)"Title";
				NSString summaryKey = (NSString)"Summary";
				NSString urlKey = (NSString)"IconURLString";
				NSString rankKey = (NSString)"Rank";
				NSString contentURLKey = (NSString)"URLString";
				contentToDict.Add (titleKey, title);
				contentToDict.Add (summaryKey, summary);
				contentToDict.Add (urlKey, imageURLString);
				contentToDict.Add (rankKey, rank);
				contentToDict.Add (contentURLKey, linkToContent);

				contentArray.Add (contentToDict);
			};
			return contentArray;
		}
	
	}
}

