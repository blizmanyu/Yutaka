using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Yutaka.IO
{
	public class YuImage
	{
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

		public YuImage(string filename)
		{
			var fi = new FileInfo(filename);
			CreationTime = fi.CreationTime;
			LastAccessTime = fi.LastAccessTime;
			LastWriteTime = fi.LastWriteTime;
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

			if (CreationTime != null && MinDateTimeThreshold < CreationTime && CreationTime < MinDateTime)
				MinDateTime = CreationTime;

			if (DateTaken != null && MinDateTimeThreshold < DateTaken && DateTaken < MinDateTime)
				MinDateTime = DateTaken;

			if (LastAccessTime != null && MinDateTimeThreshold < LastAccessTime && LastAccessTime < MinDateTime)
				MinDateTime = LastAccessTime;

			if (LastWriteTime != null && MinDateTimeThreshold < LastWriteTime && LastWriteTime < MinDateTime)
				MinDateTime = LastWriteTime;
		}

		private void SetNewFolderAndFilename()
		{
			#region Case: 7 or more characters
			var specialFolders1 = new string[,] {
				// search term, new folder name, new filename // null or empty filename will keep the original name (won't rename it) //
				{ "PHILIPS HUE", @"Philips Hue\", "" },
				{ "MICHAEL CONTURSI", @"Michael Contursi\", "" },
				{ "UNSPLASH", @"Unsplash\", "" },
				{ "RECEIPT", @"Receipts\", "" },
				{ "APARTMENT", @"Apartment\", "Apartment" },
				{ "CONSUMER REPORTS", @"Consumer Reports\", "Consumer Reports" },
				{ "FACEBOOK", @"Facebook\", "" },
				{ "GROOMING", @"Grooming\", "" },
				{ "MAGAZINE", @"Magazines\", "Magazine" },
				{ "MAXIMUM PC", @"Maximum PC\", "Maximum PC" },
				{ "MENS HEALTH", @"Mens Health\", "Mens Health" },
				{ @"ME\TEST", @"Me\Test\", "" },
				{ "OC FAIR", @"yyyy\OC Fair\", "" },
				{ "OKCUPID", @"OkCupid\", "OkCupid" },
				{ "PATRICIA", @"Patricia\", "Patricia" },
				{ "PC GAMER", @"PC Gamer\", "PC Gamer" },
				{ "SNAPCHAT", @"Snapchat\", "Snapchat" },
				{ "WOMENS HEALTH", @"Womens Health\", "Womens Health" },
				{ "SCREENSHOT", @"yyyy\Screenshots\", "Screenshot" }, // leave screenshots last //
			};

			for (int i = 0; i < specialFolders1.Length/3; i++) {
				if (FullName.ToUpper().Contains(specialFolders1[i,0])) {
					if (specialFolders1[i, 1].StartsWith(@"yyyy\"))
						NewFolder = specialFolders1[i, 1].Replace("yyyy", MinDateTime.ToString("yyyy"));
					else
						NewFolder = specialFolders1[i, 1];

					if (String.IsNullOrWhiteSpace(specialFolders1[i, 2]))
						NewFilename = Name;
					else
						NewFilename = String.Format("{0} {1:yyyy MMdd HHmm ssff}{2}", specialFolders1[i,2], MinDateTime, Extension);

					return; // only match one, then return //
				}
			}
			#endregion Case: 7 or more characters

			#region Case: Less than 7 characters
			// Order these by string length, descending //
			var specialFolders2 = new string[,] {
				// search term, new folder name, new filename // null or empty filename will keep the original name (won't rename it) //
				{ "Bumble", @"Bumble\", "Bumble" },
				{ "Cancun", @"yyyy\Cancun\", "" },
				{ "Design", @"Design\", "" },
				{ "London", @"London\", "London" },
				{ "Nanami", @"Nanami\", "Nanami" },
				{ "Tattoo", @"Tattoos\", "" },
				{ "TikTok", @"TikTok\", "TikTok" },
				{ "Tinder", @"Tinder\", "Tinder" },
				{ "Maxim", @"Maxim\", "Maxim" },
				{ "Shirt", @"Shirts\", "" },
				{ "ztest", @"ztest\", "" },
				{ "ETNT", @"ETNT\", "ETNT" },
				{ "Game", @"Games\", "" },
				{ "Ikea", @"Ikea\", "Ikea" },
				{ "Napa", @"yyyy\Napa\", "" },
				{ "Pose", @"Poses\", "" },
				{ "Woot", @"Woot\", "" },
				{ "Ga", @"Ga\", "" },
				{ "GQ", @"GQ\", "GQ" },
				{ "Me", @"Me\", "Me" },
			};

			for (int i = 0; i < specialFolders2.Length / 3; i++) {
				if (FullName.Contains(String.Format(@"\{0}\", specialFolders2[i, 0])) || Name.StartsWith(String.Format("{0} ", specialFolders2[i,0]))) {
					if (specialFolders2[i, 1].StartsWith(@"yyyy\"))
						NewFolder = specialFolders2[i, 1].Replace("yyyy", MinDateTime.ToString("yyyy"));
					else
						NewFolder = specialFolders2[i, 1];

					if (String.IsNullOrWhiteSpace(specialFolders2[i, 2]))
						NewFilename = Name;
					else
						NewFilename = String.Format("{0} {1:yyyy MMdd HHmm ssff}{2}", specialFolders2[i, 2], MinDateTime, Extension);

					return; // only match one, then return //
				}
			}
			#endregion Case: Less than 7 characters

			#region Default: Everything else
			int year;
			if (ParentFolder.Equals("Images") || ParentFolder.Equals("Pictures") || (int.TryParse(ParentFolder, out year) && (MinDateTimeThreshold.Year <= year && year <= DateTime.Now.Year)))
				NewFolder = String.Format(@"{0:yyyy}\", MinDateTime);
			else
				NewFolder = String.Format(@"{0:yyyy}\{1}", MinDateTime, ParentFolder);

			NewFilename = Name;
			#endregion Default: Everything else
		}
	}
}