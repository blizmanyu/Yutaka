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

		// WIP: do NOT use yet!! //
		private void SetNewFolderAndFilename()
		{
			#region Case: 7 or more characters
			#region Case: 7 or more characters > Default
			var specialFolders = new string[] {
				"2018 06 Cancun",
				"2018 06 Napa",
				"Apartment",
				"Consumer Reports",
				"Facebook",
				"Grooming",
				"Maximum PC",
				"Mens Health",
				"OC Fair",
				"OkCupid",
				"Patricia",
				"PC Gamer",
				"Philips Hue",
				"Snapchat",
				"Unsplash",
				"Womens Health",
			};

			for (int i = 0; i < specialFolders.Length; i++) {
				if (FullName.Contains(specialFolders[i])) {
					if (specialFolders[i].Equals("OC Fair") || specialFolders[i].Equals("2018 06 Napa") || specialFolders[i].Equals("2018 06 Cancun")) // special case for Screenshots //
						NewFolder = String.Format(@"{0:yyyy}\{1}", MinDateTime, specialFolders[i]);
					else
						NewFolder = specialFolders[i];

					NewFilename = String.Format("{0} {1:yyyy MMdd HHmm ssff}.{2}", specialFolders[i], MinDateTime, Extension);
					return; // only match one, then return //
				}
			}
			#endregion Case: 7 or more characters > Default

			#region Case: 7 or more characters > Plural folder, Singular filename
			var pluralSingular1 = new string[] {
				"Magazine",
				"Receipt",
				"Screenshot",
				"Tattoo",
			};

			for (int i = 0; i < pluralSingular1.Length; i++) {
				if (FullName.Contains(pluralSingular1[i])) {
					NewFolder = String.Format("{0}s", pluralSingular1[i]);
					NewFilename = String.Format("{0} {1:yyyy MMdd HHmm ssff}.{2}", pluralSingular1[i], MinDateTime, Extension);
					return; // only match one, then return //
				}
			}
			#endregion Case: 7 or more characters > Plural folder, Singular filename
			#endregion Case: 7 or more characters

			#region Case: Less than 7 characters
			#region Case: Less than 7 characters > Default
			// Order these by string length, descending //
			var specialFolders2 = new string[] {
				"Bumble",
				"Cancun",
				"Design",
				"London",
				"Nanami",
				"TikTok",
				"Tinder",
				"Maxim",
				"ztest",
				"ETNT",
				"Ikea",
				"Napa",
				"Woot",
				"GQ",
				"Me",
			};

			for (int i = 0; i < specialFolders2.Length; i++) {
				if (Name.StartsWith(String.Format("{0} ", specialFolders2[i])) || FullName.Contains(String.Format(@"\{0}\", specialFolders2[i]))) {
					if (specialFolders2[i].Equals("Cancun") || specialFolders2[i].Equals("Napa")) // special cases //
						NewFolder = String.Format(@"{0:yyyy}\{1}", MinDateTime, specialFolders2[i]);
					else
						NewFolder = specialFolders2[i];

					NewFilename = String.Format("{0} {1:yyyy MMdd HHmm ssff}.{2}", specialFolders2[i], MinDateTime, Extension);
					return; // only match one, then return //
				}
			}
			#endregion Case: Less than 7 characters > Default

			#region Case: Less than 7 characters > Plural folder, Singular filename
			var pluralSingular2 = new string[] {
				"Tattoo",
				"Shirt",
				"Game",
				"Pose",
			};

			for (int i = 0; i < pluralSingular2.Length; i++) {
				if (FullName.Contains(pluralSingular2[i])) {
					NewFolder = String.Format("{0}s", pluralSingular2[i]);
					NewFilename = String.Format("{0} {1:yyyy MMdd HHmm ssff}.{2}", pluralSingular2[i], MinDateTime, Extension);
					return; // only match one, then return //
				}
			}
			#endregion Case: Less than 7 characters > Plural folder, Singular filename
			#endregion Case: Less than 7 characters

			#region Default: Everything else
			NewFolder = MinDateTime.ToString("yyyy");
			NewFilename = Name;
			#endregion Default: Everything else
		}
	}
}