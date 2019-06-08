using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Yutaka.IO
{
	public class YuImage
	{
		#region Fields
		const int PROPERTY_TAG_EXIF_DATE_TAKEN = 36867; // PropertyTagExifDTOrig //
		public DateTime CreationTime;
		public DateTime DateTaken;
		public DateTime LastAccessTime;
		public DateTime LastWriteTime;
		public DateTime MinDateTime;
		public DateTime MinDateTimeThreshold = new DateTime(1970, 1, 1); // based on Unix time //
		public long Size;
		public string DirectoryName;
		public string Extension;
		public string FullName;
		public string Name;
		public string NameWithoutExtension;
		public string NewFilename;
		public string NewFolder;
		public string ParentFolder;
		#endregion Fields

		public YuImage(string filename)
		{
			var fi = new FileInfo(filename);
			try {
				CreationTime = fi.CreationTime;
			}
			catch (Exception) {
				CreationTime = new DateTime();
			}
			LastAccessTime = fi.LastAccessTime;
			try {
				LastWriteTime = fi.LastWriteTime;
			}
			catch (Exception) {
				LastWriteTime = new DateTime();
			}
			Size = fi.Length;
			DirectoryName = fi.DirectoryName;
			Extension = fi.Extension.ToLower();
			FullName = fi.FullName;
			Name = fi.Name;
			NameWithoutExtension = Name.Replace(fi.Extension, "");
			ParentFolder = fi.Directory.Name;
			fi = null;

			SetDateTaken();
			SetMinDateTime();
			SetNewFolderAndFilename();
		}

		// Retrieves the datetime WITHOUT loading the whole image //
		private void SetDateTaken()
		{
			var r = new Regex(":");

			try {
				using (var fs = new FileStream(FullName, FileMode.Open, FileAccess.Read)) {
					using (var img = Image.FromStream(fs, false, false)) {
						var propItem = img.GetPropertyItem(PROPERTY_TAG_EXIF_DATE_TAKEN);
						var dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
						DateTaken = DateTime.Parse(dateTaken);
					}
				}
			}

			catch (Exception) {
				DateTaken = new DateTime();
			}
		}

		private void SetMinDateTime()
		{
			MinDateTime = DateTime.Now;

			if (DateTaken != null && MinDateTimeThreshold < DateTaken && DateTaken < MinDateTime)
				MinDateTime = DateTaken; // prioritize DateTaken //

			else {
				if (CreationTime != null && MinDateTimeThreshold < CreationTime && CreationTime < MinDateTime)
					MinDateTime = CreationTime;

				if (LastWriteTime != null && MinDateTimeThreshold < LastWriteTime && LastWriteTime < MinDateTime)
					MinDateTime = LastWriteTime;
			}
		}

		private void SetNewFolderAndFilename()
		{
			#region Case: 4 or more characters
			var specialFolders1 = new string[,] {
				// search term,			new folder name //
				{ @"MICHAEL CONTURSI", @"Michael Contursi\", },
				{ @"CONSUMER REPORT", @"Magazines\Consumer Reports\", },
				{ @"UNITED AIRLINES", @"yyyy\United Airlines\", },
				{ @"CLASH ROYALE", @"Games\Clash Royale\", },
				{ @"CONFIRMATION", @"Receipts\", },
				{ @"VIDEO PLAYER", @"z\Video Player\", },
				{ @"DRAGONFRUIT", @"z\DragonFruit\", },
				{ @"GROOMING", @"Grooming\", },
				{ @"WOMENS HEALTH", @"Magazines\Womens Health\", },
				{ @"MENS HEALTH", @"Magazines\Mens Health\", },
				{ @"PHILIPS HUE", @"Philips Hue\", },
				{ @"MAXIMUM PC", @"Magazines\Maximum PC\", },
				{ @"APARTMENT", @"Apartment\", },
				{ @"INSTAGRAM", @"z\Instagram\", },
				{ @"MAXIMUMPC", @"Magazines\Maximum PC\", },
				{ @"STEAMGIRL", @"z\Steamgirl\PARENTFOLDER\", },
				{ @"THANK YOU", @"Receipts\", },
				{ @"CHECKOUT", @"Receipts\", },
				{ @"ITINERAR", @"Itineraries\", },
				{ @"ME\USING", @"zMe\Using\", },
				{ @"MESSAGES", @"yyyy\Messsages\", },
				{ @"PATRICIA", @"z\Patricia\", },
				{ @"PC GAMER", @"Magazines\PC Gamer\", },
				{ @"SNAPCHAT", @"z\Snapchat\", },
				{ @"UNSPLASH", @"Unsplash\", },
				{ @"INVOICE", @"Invoices\", },
				{ @"ME\TEST", @"zMe\Test\", },
				{ @"SAMSUNG", @"yyyy\Samsung\", },
				{ @"POSE", @"Poses\", },
				{ @"OKCUPID", @"z\OkCupid\", },
				{ @"RECEIPT", @"Receipts\", },
				{ @"WELCOME", @"Receipts\", },
				// Less than 7 characters //
				{ @"BUMBLE", @"z\Bumble\", },
				{ @"CHROME", @"yyyy\Chrome\", },
				{ @"LONDON", @"z\London\", },
				{ @"NANAMI", @"Nanami\", },
				{ @"TATTOO", @"Tattoos\", },
				{ @"THANKS", @"Receipts\", },
				{ @"TIKTOK", @"z\TikTok\", },
				{ @"TINDER", @"z\Tinder\", },
				{ @"BIXBY", @"yyyy\Bixby\", },
				{ @"CHASE", @"yyyy\Chase\", },
				{ @"DELTA", @"yyyy\Delta\", },
				{ @"GMAIL", @"yyyy\Gmail\", },
				{ @"HAPPN", @"z\Happn\", },
				{ @"MAXIM", @"Magazines\Maxim\", },
				{ @"SARAH", @"z\Sarah\", },
				{ @"SHIRT", @"Shirts\", },
				{ @"SLEEP", @"yyyy\Sleep\", },
				{ @"ETNT", @"Magazines\ETNT\", },
				{ @"GAME", @"Games\", },
				{ @"IKEA", @"Ikea\", },
				{ @"LEAH", @"z\Leah\", },
				{ @"LINE", @"yyyy\Line\", },
				{ @"MAPS", @"yyyy\Maps\", },
				{ @"OLGA", @"z\Olga\", },
				{ @"TURO", @"yyyy\Turo\", },
				{ @"ZME", @"zMe\", },
				// leave screenshots last //
				{ @"SCREENSHOT", @"yyyy\Screenshots\", },
			};

			for (int i = 0; i < specialFolders1.Length/2; i++) {
				if (FullName.ToUpper().Contains(specialFolders1[i,0])) {
					if (specialFolders1[i, 1].StartsWith(@"yyyy\"))
						NewFolder = specialFolders1[i, 1].Replace("yyyy", MinDateTime.ToString("yyyy"));
					else if (specialFolders1[i, 1].Contains("PARENTFOLDER"))
						NewFolder = specialFolders1[i, 1].Replace("PARENTFOLDER", ParentFolder);
					else
						NewFolder = specialFolders1[i, 1];

					NewFilename = Name;
					return; // only match one, then return //
				}
			}
			#endregion Case: 4 or more characters

			#region Case: Less than 4 characters
			// Order these by string length, descending //
			var specialFolders2 = new string[,] {
				// search term, new folder name, new filename // null or empty filename will keep the original name (won't rename it) //
				{ "GQ", @"Magazines\GQ\", },
				{ "Me", @"zMe\", },
			};

			for (int i = 0; i < specialFolders2.Length / 2; i++) {
				if (FullName.Contains(String.Format(@"\{0}\", specialFolders2[i, 0])) || Name.StartsWith(String.Format("{0} ", specialFolders2[i,0]))) {
					if (specialFolders2[i, 1].StartsWith(@"yyyy\"))
						NewFolder = specialFolders2[i, 1].Replace("yyyy", MinDateTime.ToString("yyyy"));
					else
						NewFolder = specialFolders2[i, 1];

					NewFilename = Name;
					return; // only match one, then return //
				}
			}
			#endregion Case: Less than 4 characters

			#region Default: Everything else
			int year;
			if (ParentFolder.Equals("Camera") || ParentFolder.Equals("Images") || ParentFolder.Equals("Pictures") || ParentFolder.Equals("_Unprocessed") || ParentFolder.Equals("_Process These") || (int.TryParse(ParentFolder, out year) && (MinDateTimeThreshold.Year <= year && year <= DateTime.Now.Year)))
				NewFolder = String.Format(@"{0:yyyy}\", MinDateTime);
			else
				NewFolder = String.Format(@"{0:yyyy}\{1}\", MinDateTime, ParentFolder);

			NewFilename = Name;
			#endregion Default: Everything else
		}
	}
}