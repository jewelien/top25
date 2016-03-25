using System;
using Foundation;
using System.IO;

namespace top25
{
	public class PodcastController
	{
		public NSArray podcastsList { get; set; }
		public NSString lastFetchedDateForPodcastsString { get; set;}

		private static PodcastController s_instance;

		public static PodcastController SharedInstance {
			get {
				if (s_instance == null)
					s_instance = new PodcastController ();

				return s_instance; 
			}
		}

		public PodcastController ()
		{
			NSNotificationCenter.DefaultCenter.AddObserver ((NSString)"updatePodcastsList", updatePodcastsArrayFromFileWithDictionaryArray);
		}

		void updatePodcastsArrayFromFileWithDictionaryArray(NSNotification notification)
		{
			loadPodcastsFromFile ();
		}

		public void loadPodcastsFromFile()
		{
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			var filename = Path.Combine (documents, "podcast.json");
			NSData data = NSFileManager.DefaultManager.Contents (filename);
			if (data == null) {
				return;
			}
			NSError err;
			NSDictionary jsonDictionary = (NSDictionary)NSJsonSerialization.Deserialize (data, NSJsonReadingOptions.MutableContainers, out err);
			NSString fetchedDateString = (NSString)jsonDictionary["FetchedDateString"];
			this.lastFetchedDateForPodcastsString = fetchedDateString;

			NSArray podcastsDictionaryArray = (NSArray)jsonDictionary ["Podcasts"];

			NSMutableArray podcastsArray = new NSMutableArray ();
			for (int i = 0; i < 25; i++) {
				nuint index = (nuint)i;
				NSDictionary podcastDictionary = podcastsDictionaryArray.GetItem<NSDictionary> (index);
				NSString title = (NSString)podcastDictionary ["Title"];
				NSString summary = (NSString)podcastDictionary ["Summary"];
				NSString imageURLString = (NSString)podcastDictionary ["IconURLString"];
				NSNumber rankFromDict = (NSNumber)podcastDictionary ["Rank"];
				NSString urlString = (NSString)podcastDictionary["URLString"];
				Podcast newPodcast = new Podcast (title, summary, imageURLString, rankFromDict, urlString);
				podcastsArray.Add (newPodcast);
			} ;
			this.podcastsList = podcastsArray;
			NSNotificationCenter.DefaultCenter.PostNotificationName("getPodcastsSuccess", null);
		}
	}
}

