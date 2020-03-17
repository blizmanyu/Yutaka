using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Yutaka.IO2
{
	public class YuFile
	{
		#region Fields
		const int PROPERTY_TAG_EXIF_DATE_TAKEN = 36867; // PropertyTagExifDTOrig //
		private static readonly int FIVE_HUNDRED_TWELVE_KB = (int) Math.Pow(2, 19);
		private static readonly Regex Regex_Colon = new Regex(":", RegexOptions.Compiled);
		protected static readonly DateTime MaxDateTimeThreshold = DateTime.Now.AddDays(1);
		protected static readonly DateTime MinDateTimeThreshold = UNIX_TIME;
		#region protected static readonly string[][] SpecialFolders = new string[][] {
		protected static readonly string[][] SpecialFolders = new string[][] {
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
		#endregion protected static readonly string[][] SpecialFolders
		public static readonly DateTime UNIX_TIME = new DateTime(1970, 1, 1);
		public DateTime CreationTime;
		public DateTime? DateTaken;
		public DateTime LastAccessTime;
		public DateTime LastWriteTime;
		public DateTime MinDateTime;
		public string DirectoryName;
		protected string ExtensionOrig;
		public string Extension;
		public string FullName;
		public string Name;
		public string ParentFolder;
		public string NewFolder;
		public string Root;
		public long Size;
		#endregion Fields

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="YuFile"/> class, which acts as a wrapper for a file path.
		/// </summary>
		/// <param name="filename">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
		public YuFile(string filename = null)
		{
			if (String.IsNullOrWhiteSpace(filename))
				throw new Exception(String.Format("<filename> is required. Exception thrown in constructor YuFile(string filename).{0}{0}", Environment.NewLine));

			try {
				var isReadOnly = false;
				var fi = new FileInfo(filename);

				if (fi.IsReadOnly) {
					isReadOnly = true;
					fi.IsReadOnly = false;
					fi.Refresh();
				}

				#region CreationTime = fi.CreationTime;
				try {
					CreationTime = fi.CreationTime;
				}
				catch (Exception) {
					try {
						CreationTime = fi.LastWriteTime;
					}
					catch (Exception) {
						CreationTime = fi.LastAccessTime;
					}
				}
				#endregion CreationTime = fi.CreationTime;
				LastAccessTime = fi.LastAccessTime;
				#region LastWriteTime = fi.LastWriteTime;
				try {
					LastWriteTime = fi.LastWriteTime;
				}
				catch (Exception) {
					try {
						LastWriteTime = fi.CreationTime;
					}
					catch (Exception) {
						LastWriteTime = fi.LastAccessTime;
					}
				}
				#endregion LastWriteTime = fi.LastWriteTime;
				DirectoryName = fi.DirectoryName;
				ExtensionOrig = fi.Extension;
				Extension = ExtensionOrig.ToLower();
				FullName = fi.FullName.Replace(ExtensionOrig, Extension);
				Size = fi.Length;
				Name = fi.Name.Replace(ExtensionOrig, Extension);
				NewFolder = "";
				ParentFolder = fi.Directory.Name;
				Root = Path.GetPathRoot(FullName);

				SetDateTaken();
				SetMinDateTime();
				SetNewFolder();

				if (isReadOnly) {
					fi.IsReadOnly = true;
					fi.Refresh();
				}

				fi = null;
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in Constructor YuFile(string filename='{3}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, filename);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Constructor YuFile(string filename='{3}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, filename);

				Console.Write("\n{0}", log);
				#endregion Log
			}
		}
		#endregion Constructor

		#region Utilities
		/// <summary>
		/// Dumps all field values to Console.
		/// </summary>
		public void Debug()
		{
			Console.Write("\n");
			Console.Write("\n   CreationTime: {0}", CreationTime);
			Console.Write("\n      DateTaken: {0}", DateTaken);
			Console.Write("\n LastAccessTime: {0}", LastAccessTime);
			Console.Write("\n  LastWriteTime: {0}", LastWriteTime);
			Console.Write("\n    MinDateTime: {0}", MinDateTime);
			Console.Write("\n  DirectoryName: {0}", DirectoryName);
			Console.Write("\n  ExtensionOrig: {0}", ExtensionOrig);
			Console.Write("\n      Extension: {0}", Extension);
			Console.Write("\n       FullName: {0}", FullName);
			Console.Write("\n           Name: {0}", Name);
			Console.Write("\n   ParentFolder: {0}", ParentFolder);
			Console.Write("\n      NewFolder: {0}", NewFolder);
			Console.Write("\n           Root: {0}", Root);
			Console.Write("\n           Size: {0}", Size);
			Console.Write("\n");
		}

		/// <summary>
		/// Fast file copy with big buffers. If &lt;destFileName&gt; exists, it will be overwritten.
		/// </summary>
		/// <param name="destFileName">The name of the destination file. This cannot be a directory.</param>
		/// <seealso cref="https://www.codeproject.com/Tips/777322/A-Faster-File-Copy"/>
		protected bool FastCopyTo(string destFileName)
		{
			int read;
			var array_length = FIVE_HUNDRED_TWELVE_KB;
			var dataArray = new byte[array_length];

			try {
				using (var fsread = new FileStream(FullName, FileMode.Open, FileAccess.Read, FileShare.None, array_length)) {
					using (var bwread = new BinaryReader(fsread)) {
						using (var fswrite = new FileStream(destFileName, FileMode.Create, FileAccess.Write, FileShare.None, array_length)) {
							using (var bwwrite = new BinaryWriter(fswrite)) {
								for (; ; ) {
									read = bwread.Read(dataArray, 0, array_length);
									if (0 == read)
										break;
									bwwrite.Write(dataArray, 0, read);
								}
							}
						}
					}
				}

				return true;
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in YuFile.FastCopyTo(string destFileName='{3}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, destFileName);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of YuFile.FastCopyTo(string destFileName='{3}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, destFileName);

				Console.Write("\n{0}", log);
				#endregion Log

				return false;
			}
		}

		/// <summary>
		/// Sets the DateTaken WITHOUT loading the whole image.
		/// </summary>
		/// <returns></returns>
		protected void SetDateTaken()
		{
			try {
				using (var fs = new FileStream(FullName, FileMode.Open, FileAccess.Read)) {
					using (var img = Image.FromStream(fs, false, false)) {
						var propItem = img.GetPropertyItem(PROPERTY_TAG_EXIF_DATE_TAKEN);
						var dateTaken = Regex_Colon.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);

						if (DateTime.TryParse(dateTaken, out var result))
							DateTaken = result;
						else
							DateTaken = null;
					}
				}
			}

			catch (Exception) {
				DateTaken = null;
			}
		}

		/// <summary>
		/// Sets MinDateTime. Prioritize DateTaken if its valid.
		/// </summary>
		/// <returns></returns>
		protected void SetMinDateTime()
		{
			MinDateTime = MaxDateTimeThreshold;

			if (DateTaken != null && MinDateTimeThreshold < DateTaken && DateTaken < MinDateTime)
				MinDateTime = DateTaken.Value; // prioritize DateTaken //

			else {
				if (MinDateTimeThreshold < CreationTime && CreationTime < MinDateTime)
					MinDateTime = CreationTime;
				if (MinDateTimeThreshold < LastWriteTime && LastWriteTime < MinDateTime)
					MinDateTime = LastWriteTime;
				if (MinDateTimeThreshold < LastAccessTime && LastAccessTime < MinDateTime)
					MinDateTime = LastAccessTime;
			}
		}

		protected void SetNewFolder()
		{
			int result;
			var fullnameUpper = FullName.ToUpper();
			var parentFolderUpper = ParentFolder.ToUpper();
			var bypassList = new List<string> { "_PROCESS THESE", "_TEST", "_UNPROCESSED", "100ANDRO", "101_PANA", "102_PANA", "103_PANA", "APPS", "CAMERA", "CAMERA ROLL", "DOCUMENTS", "DOWNLOAD", "DOWNLOADS", "GAMES", "IMAGES", "OLD", "PICTURES", "SCREENSHOT", "SCREENSHOTS", "TEST", "XPERIA TL", };

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
		#endregion Utilities

		#region Methods
		/// <summary>
		/// Copies an existing file to a new file.
		/// </summary>
		/// <param name="destFileName">The name of the new file to copy to.</param>
		/// <param name="overwriteOption">One of the enumeration values that specifies whether to overwrite or not if the destination file already exists.</param>
		public void CopyTo(string destFileName, OverwriteOption overwriteOption = OverwriteOption.Skip)
		{
			#region Input Check
			if (String.IsNullOrWhiteSpace(destFileName))
				throw new Exception(String.Format("<destFileName> is required.{0}", Environment.NewLine));
			if (FullName.ToUpper().Equals(destFileName.ToUpper()))
				return;
			#endregion Input Check

			try {
				var destFileExists = File.Exists(destFileName);

				switch (overwriteOption) {
					#region case OverwriteOption.Overwrite:
					case OverwriteOption.Overwrite:
						FastCopyTo(destFileName);
						break;
					#endregion
					#region case OverwriteOption.Skip:
					case OverwriteOption.Skip:
						if (!destFileExists)
							FastCopyTo(destFileName);
						break;
					#endregion
					#region case OverwriteOption.KeepBoth:
					case OverwriteOption.KeepBoth:
						if (destFileExists)
							FastCopyTo(FileUtil.AutoRename(destFileName));
						else
							FastCopyTo(destFileName);
						break;
					#endregion
					#region case OverwriteOption.Smart:
					case OverwriteOption.Smart:
						if (destFileExists) {
							if (!this.Equals(new YuFile(destFileName)))
								FastCopyTo(FileUtil.AutoRename(destFileName));
						}

						else
							FastCopyTo(destFileName);
						break;
					#endregion
					default:
						break;
				}
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in YuFile.CopyTo(string destFileName='{3}', OverwriteOption overwriteOption='{4}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, destFileName, overwriteOption.ToString());
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of YuFile.CopyTo(string destFileName='{3}', OverwriteOption overwriteOption='{4}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, destFileName, overwriteOption.ToString());

				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// Permanently deletes the file.
		/// </summary>
		public void Delete()
		{
			try {
				new FileInfo(FullName).Delete();
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in YuFile.Delete(){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of YuFile.Delete(){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);

				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// Copies an existing file to a new file.
		/// </summary>
		/// <param name="destFileName">The name of the new file to copy to.</param>
		/// <param name="overwriteOption">One of the enumeration values that specifies whether to overwrite or not if the destination file already exists.</param>
		public bool TryCopyTo(string destFileName, OverwriteOption overwriteOption = OverwriteOption.Skip)
		{
			try {
				CopyTo(destFileName, overwriteOption);
				return true;
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in YuFile.TryCopyTo(string destFileName='{3}', OverwriteOption overwriteOption='{4}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, destFileName, overwriteOption.ToString());
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of YuFile.TryCopyTo(string destFileName='{3}', OverwriteOption overwriteOption='{4}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, destFileName, overwriteOption.ToString());

				Console.Write("\n{0}", log);
				#endregion Log

				return false;
			}
		}

		/// <summary>
		/// Permanently deletes the file.
		/// </summary>
		public bool TryDelete()
		{
			try {
				Delete();
				return true;
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in YuFile.TryDelete(){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of YuFile.TryDelete(){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);

				Console.Write("\n{0}", log);
				#endregion Log

				return false;
			}
		}

		#region Overrides
		public override bool Equals(Object obj)
		{
			if (obj == null || !GetType().Equals(obj.GetType()))
				return false;

			return Size == ((YuFile) obj).Size;
		}

		public override int GetHashCode()
		{
			return Size.GetHashCode();
		}

		public static bool operator ==(YuFile x, YuFile y)
		{
			return x.Equals(y);
		}

		public static bool operator !=(YuFile x, YuFile y)
		{
			return !x.Equals(y);
		}

		public override string ToString()
		{
			return FullName;
		}
		#endregion Overrides
		#endregion Methods
	}
}