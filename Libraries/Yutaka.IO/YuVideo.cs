using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Yutaka.IO
{
	public class YuVideo
	{
		#region Fields
		private const int MEDIA_CREATED_FIELD = 208;
		private const int DATE_RELEASED_FIELD = 209;
		private readonly Guid CLSID_Shell = Guid.Parse("13709620-C279-11CE-A49E-444553540000");
		private char[] charactersToRemove = new char[] { (char) 8206, (char) 8207 };
		public DateTime CreationTime;
		public DateTime DateReleased;
		public DateTime LastAccessTime;
		public DateTime LastWriteTime;
		public DateTime MediaCreated;
		public DateTime MinDateTime;
		public DateTime MinDateTimeThreshold = new DateTime(1970, 1, 1); // based on Unix time //
		#region public string[][] SpecialFolders = new string[][] {
		public string[][] SpecialFolders = new string[][] {
			new string[] { "babystepsanddownwarddogs", @"zz\BareSolesBearSoul\", },
			new string[] { "PreciousO23_Bucket", @"zz\Olga\", },
			new string[] { "BareSolesBearSoul", @"zz\BareSolesBearSoul\", },
			new string[] { "Consumer Reports", @"Documents\Consumer Reports\", },
			new string[] { "ConsumerReports", @"Documents\Consumer Reports\", },
			new string[] { "United Airlines", @"Documents\Itineraries\", },
			new string[] { "Clash of Clans", @"Games\Clash of Clans\", },
			new string[] { "UnitedAirlines", @"Documents\Itineraries\", },
			new string[] { "Brian Viveros", @"Brian Viveros\", },
			new string[] { "Clash Royale", @"Games\Clash Royale\", },
			new string[] { "ClashOfClans", @"Games\Clash of Clans\", },
			new string[] { "Confirmation", @"Documents\Receipts\", },
			new string[] { "Registration", @"Documents\Registrations\", },
			new string[] { "Video Player", @"zz\Video Player\", },
			new string[] { "ClashRoyale", @"Games\Clash Royale\", },
			new string[] { "DragonFruit", @"zz\DragonFruit\", },
			new string[] { "Itineraries", @"Documents\Itineraries\", },
			new string[] { "Grooming", @"Grooming\", }, // higher priority //
			new string[] { "Womens Health", @"Documents\Womens Health\", },
			new string[] { "WomensHealth", @"Documents\Womens Health\", },
			new string[] { "Men's Health", @"Documents\Mens Health\", },
			new string[] { "Mens Health", @"Documents\Mens Health\", },
			new string[] { "MensHealth", @"Documents\Mens Health\", },
			new string[] { "Philips Hue", @"Philips Hue\", },
			new string[] { "Castle Age", @"Games\Castle Age\", },
			new string[] { "Green Card", @"Documents\Green Card\", },
			new string[] { "Maximum PC", @"Documents\Maximum PC\", },
			new string[] { "PhilipsHue", @"Philips Hue\", },
			new string[] { "US Airways", @"Documents\Itineraries\", },
			new string[] { "CastleAge", @"Games\Castle Age\", },
			new string[] { "Fantasica", @"Games\Fantasica\", },
			new string[] { "GreenCard", @"Documents\GreenCard\", },
			new string[] { "Instagram", @"zz\Instagram\", },
			new string[] { "Itinerary", @"Documents\Itineraries\", },
			new string[] { "MaximumPC", @"Documents\Maximum PC\", },
			new string[] { "Messenger", @"Apps\Messenger\", },
			new string[] { "Steamgirl", @"zz\Steamgirl\", },
			new string[] { "Thank You", @"Documents\Receipts\", },
			new string[] { "USAirways", @"Documents\Itineraries\", },
			new string[] { "Cash App", @"Apps\Cash App\", },
			new string[] { "Checkout", @"Documents\Receipts\", },
			new string[] { "Facebook", @"Apps\Facebook\", },
			new string[] { "Invoices", @"Documents\Invoices\", },
			new string[] { "Messages", @"Apps\Messages\", },
			new string[] { "Passport", @"Documents\Passport\", },
			new string[] { "Patricia", @"zz\Patricia\", },
			new string[] { "PC Gamer", @"Documents\PC Gamer\", },
			new string[] { "Receipts", @"Documents\Receipts\", },
			new string[] { "Snapchat", @"zz\Snapchat\", },
			new string[] { "ThankYou", @"Documents\Receipts\", },
			new string[] { "Unsplash", @"Unsplash\", },
			new string[] { "WhatsApp", @"Apps\WhatsApp\", },
			new string[] { "Bitmoji", @"Apps\Bitmoji\", },
			new string[] { "CashApp", @"Apps\Cash App\", },
			new string[] { "Invoice", @"Documents\Invoices\", },
			new string[] { "License", @"Documents\License\", },
			new string[] { "Netflix", @"Apps\Netflix\", },
			new string[] { "OkCupid", @"zz\OkCupid\", },
			new string[] { "P Shots", @"zz\P Shots\", },
			new string[] { "Pandora", @"Apps\Pandora\", },
			new string[] { "PCGamer", @"Documents\PC Gamer\", },
			new string[] { "Receipt", @"Documents\Receipts\", },
			new string[] { "Samsung", @"Apps\Samsung\", },
			new string[] { "Spotify", @"Apps\Spotify\", },
			new string[] { "Tattoos", @"Tattoos\", },
			new string[] { "Twitter", @"Apps\Twitter\", },
			new string[] { "Vanessa", @"zz\Vanessa\", },
			new string[] { "Welcome", @"Documents\Receipts\", },
			new string[] { "YouTube", @"Apps\YouTube\", },
			new string[] { "Amazon", @"Documents\Amazon\", },
			new string[] { "Poses", @"Poses\", }, // higher priority //
			new string[] { "Bumble", @"zz\Bumble\", },
			new string[] { "Chrome", @"Apps\Chrome\", },
			new string[] { "London", @"zz\London\", },
			new string[] { "Nanami", @"Nanami\", },
			new string[] { "Shirts", @"Shirts\", },
			new string[] { "Tattoo", @"Tattoos\", },
			new string[] { "Thanks", @"Documents\Receipts\", },
			new string[] { "TikTok", @"Apps\TikTok\", },
			new string[] { "Tinder", @"zz\Tinder\", },
			new string[] { "Alekz", @"zz\Alekz\", },
			new string[] { "Bixby", @"Apps\Bixby\", },
			new string[] { "Chase", @"Documents\Chase\", },
			new string[] { "Delta", @"Documents\Itineraries\", },
			new string[] { "Gmail", @"Apps\Gmail\", },
			new string[] { "Happn", @"zz\Happn\", },
			new string[] { "Maxim", @"Documents\Maxim\", },
			new string[] { "Sarah", @"zz\Sarah\", },
			new string[] { "Scans", @"Documents\Scans\", },
			new string[] { "Shirt", @"Shirts\", },
			new string[] { "Sleep", @"Apps\Sleep\", },
			new string[] { "Alex", @"zz\Alekz\", },
			new string[] { "Arpy", @"zz\Arpy\", },
			new string[] { "ETNT", @"Documents\ETNT\", },
			new string[] { "FICO", @"Documents\FICO\", },
			new string[] { "Ikea", @"Documents\Ikea\", },
			new string[] { "JFul", @"!JFul\", },
			new string[] { "Leah", @"zz\Leah\", },
			new string[] { "Line", @"Apps\Line\", },
			new string[] { "Maps", @"Apps\Maps\", },
			new string[] { "Marc", @"zz\Olga\", },
			new string[] { "Mely", @"zz\Mely\", },
			new string[] { "Olga", @"zz\Olga\", },
			new string[] { "Pose", @"Poses\", },
			new string[] { "Scan", @"Documents\Scans\", },
			new string[] { "Turo", @"Apps\Turo\", },
			new string[] { "Uber", @"Apps\Uber\", },
			new string[] { "Car", @"Documents\Car\", },
			new string[] { "Jas", @"zz\Jas\", },
			new string[] { "zMe", @"zMe\", },
			new string[] { "GQ", @"Documents\GQ\", },
			new string[] { "Me", @"zMe\", },
			new string[] { "MH", @"Documents\Mens Health\", },
			// Keep these last //
			new string[] { "zz", @"zz\", },
			new string[] { "Screenshots", @"Documents\Screenshots\", },
			new string[] { "Screenshot", @"Documents\Screenshots\", },
			new string[] { "Documents", @"Documents\", },
			new string[] { "Games", @"Games\", },
			new string[] { "Apps", @"Apps\", },
		};
		#endregion public string[][] SpecialFolders
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

		public YuVideo(string filename)
		{
			var fi = new FileInfo(filename);
			fi.IsReadOnly = false;
			CreationTime = fi.CreationTime;
			LastAccessTime = fi.LastAccessTime;
			LastWriteTime = fi.LastWriteTime;
			Size = fi.Length;
			DirectoryName = fi.DirectoryName;
			Extension = fi.Extension;
			FullName = fi.FullName.Replace(Extension, Extension.ToLower());
			Name = fi.Name.Replace(Extension, Extension.ToLower());
			NameWithoutExtension = fi.Name.Replace(Extension, "");
			NewFilename = Name;
			NewFolder = "";
			ParentFolder = fi.Directory.Name;
			fi = null;

			SetMediaCreated();
			SetDateReleased();
			SetMinDateTime();
			SetNewFolderAndFilename();
		}

		private void SetMediaCreated()
		{
			try {
				dynamic shell = Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_Shell));
				var folder = shell.NameSpace(DirectoryName);
				var file = folder.ParseName(Name);
				var label = folder.GetDetailsOf(null, MEDIA_CREATED_FIELD);

				if (label.ToUpper().Equals("MEDIA CREATED")) {
					var value = folder.GetDetailsOf(file, MEDIA_CREATED_FIELD).Trim();

					// Removing the suspect characters
					foreach (char c in charactersToRemove)
						value = value.Replace((c).ToString(), "").Trim();

					// If the value string is empty, return DateTime.MinValue, otherwise return the "Media Created" date
					MediaCreated = String.IsNullOrWhiteSpace(value) ? DateTime.MinValue : DateTime.Parse(value);
				}

				else {
					Console.Write("\n**********");
					Console.Write("\n{0} is NOT the Media Created field", MEDIA_CREATED_FIELD);
					Console.Write("\n**********");
					MediaCreated = new DateTime();
				}
			}

			catch (Exception) {
				MediaCreated = new DateTime();
			}
		}

		private void SetDateReleased()
		{
			try {
				dynamic shell = Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_Shell));
				var folder = shell.NameSpace(DirectoryName);
				var file = folder.ParseName(Name);
				var label = folder.GetDetailsOf(null, DATE_RELEASED_FIELD);

				if (label.ToUpper().Equals("DATE RELEASED")) {
					var value = folder.GetDetailsOf(file, DATE_RELEASED_FIELD).Trim();

					// Removing the suspect characters
					foreach (char c in charactersToRemove)
						value = value.Replace((c).ToString(), "").Trim();

					// If the value string is empty, return DateTime.MinValue, otherwise return the "Media Created" date
					DateReleased = String.IsNullOrWhiteSpace(value) ? DateTime.MinValue : DateTime.Parse(value);
				}

				else {
					Console.Write("\n**********");
					Console.Write("\n{0} is NOT the Date Relased field", DATE_RELEASED_FIELD);
					Console.Write("\n**********");
					DateReleased = new DateTime();
				}
			}

			catch (Exception) {
				DateReleased = new DateTime();
			}
		}

		private void SetMinDateTime()
		{
			MinDateTime = DateTime.Now;

			if (MediaCreated != null && MinDateTimeThreshold < MediaCreated && MediaCreated < MinDateTime)
				MinDateTime = MediaCreated; // prioritize MediaCreated //

			else {
				if (CreationTime != null && MinDateTimeThreshold < CreationTime && CreationTime < MinDateTime)
					MinDateTime = CreationTime;

				if (DateReleased != null && MinDateTimeThreshold < DateReleased && DateReleased < MinDateTime)
					MinDateTime = DateReleased;

				if (LastWriteTime != null && MinDateTimeThreshold < LastWriteTime && LastWriteTime < MinDateTime)
					MinDateTime = LastWriteTime;
			}
		}

		private void SetNewFolderAndFilename()
		{
			int result;
			var fullnameUpper = FullName.ToUpper();
			var parentFolderUpper = ParentFolder.ToUpper();
			var bypassList = new List<string> { "_TEST", "ANIME", "CAMERA", "DOWNLOAD", "DOWNLOADS", "MOVIES", "MUSIC VIDEOS", "OLD", "TEST", "TV", "VIDEOS", };

			#region Special Folders
			for (int i = 0; i < SpecialFolders.Length; i++) {
				if (Regex.IsMatch(FullName, String.Format(@"[^a-zA-Z]{0}[^a-zA-Z]", SpecialFolders[i][0]), RegexOptions.IgnoreCase)) {
					if (SpecialFolders[i][1].ToUpper().Contains(parentFolderUpper))
						NewFolder = SpecialFolders[i][1];
					else if (bypassList.Exists(x => x.Equals(parentFolderUpper) || int.TryParse(ParentFolder, out result)))
						NewFolder = SpecialFolders[i][1];
					else
						NewFolder = String.Format(@"{0}{1}\", SpecialFolders[i][1], ParentFolder);

					return; // only match one, then return //
				}
			}
			#endregion Special Folders

			#region Default: Everything else
			if (bypassList.Exists(x => x.Equals(parentFolderUpper)) || int.TryParse(ParentFolder, out result))
				NewFolder = String.Format(@"{0}{1}\", NewFolder, MinDateTime.Year);
			else
				NewFolder = String.Format(@"{0}{1}\{2}\", NewFolder, MinDateTime.Year, ParentFolder);
			#endregion Default: Everything else
		}
	}
}