using System;
using Foundation;
using System.IO;

namespace top25
{
	public class ContentController
	{

		public NSArray podcastsList { get; set; }
		public NSArray appsList { get; set; }
		public NSString lastFetchedDateForPodcastsString { get; set;}
		public NSString lastFetchedDateForAppsString { get; set;}

		private static ContentController s_instance;
		public static ContentController SharedInstance {
			get {
				if (s_instance == null)
					s_instance = new ContentController ();

				return s_instance; 
			}
		}

		public ContentController ()
		{
			NSNotificationCenter.DefaultCenter.AddObserver((NSString)"updateAppsList", updateAppsArrayFromFileWithDictionaryArray);
			NSNotificationCenter.DefaultCenter.AddObserver ((NSString)"updatePodcastsList", updatePodcastsArrayFromFileWithDictionaryArray);
		}

		void updateAppsArrayFromFileWithDictionaryArray (NSNotification notification)
		{
			loadFromFileContentTypeOf (Content.ContentType.Application);
		}

		void updatePodcastsArrayFromFileWithDictionaryArray(NSNotification notification)
		{
			loadFromFileContentTypeOf (Content.ContentType.Podcast);
		}

		public void loadFromFileContentTypeOf(Content.ContentType typeOfContent)
		{
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			NSString filename = (NSString)"";
			if (typeOfContent == Content.ContentType.Application) {
				filename = (NSString)Path.Combine (documents, "app.json");
			} else if (typeOfContent == Content.ContentType.Podcast) {
				filename = (NSString)Path.Combine (documents, "podcast.json");
			}
			NSData data = NSFileManager.DefaultManager.Contents (filename);
			if (data == null) {
				return;
			}

			NSError err;
			NSDictionary jsonDictionary = (NSDictionary)NSJsonSerialization.Deserialize (data, NSJsonReadingOptions.MutableContainers, out err);
			NSString fetchedDateString = (NSString)jsonDictionary["FetchedDateString"];
			NSArray contentDictionaryArray = new NSArray ();
			if (typeOfContent == Content.ContentType.Application) {
				this.lastFetchedDateForAppsString = fetchedDateString;
				contentDictionaryArray = (NSArray)jsonDictionary ["Apps"];
			} else if (typeOfContent == Content.ContentType.Podcast) {
				this.lastFetchedDateForPodcastsString = fetchedDateString;
				contentDictionaryArray = (NSArray)jsonDictionary ["Podcasts"];
			}
				
			NSMutableArray contentArray = new NSMutableArray ();
			for (int i = 0; i < 25; i++) {
				nuint index = (nuint)i;
				NSDictionary contentDictionary = contentDictionaryArray.GetItem<NSDictionary> (index);
				NSString title = (NSString)contentDictionary ["Title"];
				NSString summary = (NSString)contentDictionary ["Summary"];
				NSString imageURLString = (NSString)contentDictionary ["IconURLString"];
				NSNumber rankFromDict = (NSNumber)contentDictionary ["Rank"];
				NSString urlString = (NSString)contentDictionary["URLString"];
				Content newContent = new Content(title, summary, imageURLString, rankFromDict, urlString, typeOfContent);
				contentArray.Add (newContent);
			} ;

			if (typeOfContent == Content.ContentType.Application) {
				this.appsList = contentArray;
				NSNotificationCenter.DefaultCenter.PostNotificationName("getAppsSuccess", null);
			} else if (typeOfContent == Content.ContentType.Podcast) {
				this.podcastsList = contentArray;
				NSNotificationCenter.DefaultCenter.PostNotificationName("getPodcastsSuccess", null);
			}
		}
	}
}

