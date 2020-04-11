using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Yutaka.IO2
{
	public class YuFile
	{
		#region Fields
		public const decimal ONE_KB = 1024m;
		public const decimal ONE_MB = 1048576m;
		public const decimal ONE_GB = 1073741824m;
		public const decimal ONE_TB = 1099511627776m;
		public const decimal ONE_PB = 1125899906842624m;
		public static readonly DateTime UnixTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		public static readonly DateTime UnixTimeUnspecified = new DateTime(1970, 1, 1);
		protected const int FIVE_HUNDRED_TWELVE_KB = 524288;
		protected const int PROPERTY_TAG_EXIF_DATE_TAKEN = 36867; // PropertyTagExifDTOrig // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.imaging.propertyitem.id //
		protected const string FORMAT = @"yyyy-MM-dd HH:mm:ss.fff";
		protected static readonly DateTime MaxDateTimeThreshold = DateTime.Now.AddDays(1);
		protected static readonly DateTime MinDateTimeThreshold = UnixTimeUnspecified;
		protected static readonly DateTime OldThreshold = DateTime.Now.AddYears(-10);
		protected static readonly DateTime ReallyOldThreshold = DateTime.Now.AddYears(-20);
		protected static readonly Regex Regex_Colon = new Regex(":", RegexOptions.Compiled);
		protected static readonly string[] DefaultCameraFolders = { "_PROCESS THESE", "_TEST", "_UNPROCESSED", "100ANDRO", "101_PANA", "102_PANA", "103_PANA", "APPS", "CAMERA", "CAMERA ROLL", "DOCUMENTS", "DOWNLOAD", "DOWNLOADS", "GAMES", "IMAGES", "OLD", "PICTURES", "SCREENSHOT", "SCREENSHOTS", "TEST", "XPERIA TL", };
		#region protected static readonly string[] SpecialFolders = { "Tattoos", "Shirts", "Poses", "zMe", };
		protected static readonly string[] SpecialFolders = {
			"Tattoos",
			"Shirts",
			"Poses",
			"zMe",
		};
		#endregion SpecialFolders
		protected string DateTakenStr;
		protected string ExtensionOrig;
		public DateTime CreationTime;
		public DateTime? DateTaken;
		public DateTime LastAccessTime;
		public DateTime LastWriteTime;
		public DateTime MinDateTime;
		public long Size;
		public string DirectoryName;
		public string Extension;
		public string FullName;
		public string Name;
		public string NewFolder;
		public string ParentFolder;
		public string Root;
		#endregion Fields

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="YuFile"/> class, which acts as a wrapper for a file path.
		/// </summary>
		/// <param name="filename">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
		public YuFile(string filename = null)
		{
			if (String.IsNullOrWhiteSpace(filename))
				throw new Exception(String.Format("<filename> is required.{0}Exception thrown in Constructor YuFile(string filename).{0}{0}", Environment.NewLine));

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
				Name = fi.Name.Replace(ExtensionOrig, Extension);
				NewFolder = "";
				ParentFolder = fi.Directory.Name;
				Root = Path.GetPathRoot(FullName);
				Size = fi.Length;

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
		/// Converts bytes to a more human-readable unit.
		/// </summary>
		/// <param name="bytes">The bytes to convert.</param>
		/// <returns></returns>
		protected string BytesToString(long bytes)
		{
			if (bytes == 0)
				return "0 bytes";
			if (bytes < 1000)
				return String.Format("{0} bytes", bytes);

			string unit;
			decimal temp;
			var decimalBytes = (decimal) bytes;

			#region KB
			if (bytes < 1023488) { // 999.5 KiB //
				temp = decimalBytes / ONE_KB;
				unit = "KB";

				if (temp < 10)
					return String.Format("{0:n2} {1}", temp, unit);
				if (temp < 100)
					return String.Format("{0:n1} {1}", temp, unit);

				return String.Format("{0:f0} {1}", temp, unit);
			}
			#endregion KB

			#region MB
			if (bytes < 1048051712) { // 999.5 MiB //
				temp = decimalBytes / ONE_MB;
				unit = "MB";

				if (temp < 10)
					return String.Format("{0:n2} {1}", temp, unit);
				if (temp < 100)
					return String.Format("{0:n1} {1}", temp, unit);

				return String.Format("{0:f0} {1}", temp, unit);
			}
			#endregion MB

			#region GB
			if (bytes < 1073204953088) { // 999.5 GiB //
				temp = decimalBytes / ONE_GB;
				unit = "GB";

				if (temp < 10)
					return String.Format("{0:n2} {1}", temp, unit);
				if (temp < 100)
					return String.Format("{0:n1} {1}", temp, unit);

				return String.Format("{0:f0} {1}", temp, unit);
			}
			#endregion GB

			#region TB
			if (bytes < 1098961871962112) { // 999.5 TiB //
				temp = decimalBytes / ONE_TB;
				unit = "TB";

				if (temp < 10)
					return String.Format("{0:n2} {1}", temp, unit);
				if (temp < 100)
					return String.Format("{0:n1} {1}", temp, unit);

				return String.Format("{0:f0} {1}", temp, unit);
			}
			#endregion TB

			temp = decimalBytes / ONE_PB;
			unit = "PB";

			if (temp < 10)
				return String.Format("{0:n2} {1}", temp, unit);
			if (temp < 100)
				return String.Format("{0:n1} {1}", temp, unit);

			return String.Format("{0:f0} {1}", temp, unit);
		}

		/// <summary>
		/// Dumps all field values to Console.
		/// </summary>
		public void Debug()
		{
			Console.Write("\n");
			Console.Write("\n   CreationTime: {0}", CreationTime.ToString(FORMAT));
			Console.Write("\n      DateTaken: {0:yyyy-MM-dd HH:mm:ss.fff}", DateTaken);
			Console.Write("\n   DateTakenStr: {0}", DateTakenStr);
			Console.Write("\n LastAccessTime: {0}", LastAccessTime.ToString(FORMAT));
			Console.Write("\n  LastWriteTime: {0}", LastWriteTime.ToString(FORMAT));
			Console.Write("\n    MinDateTime: {0}", MinDateTime.ToString(FORMAT));
			Console.Write("\n  DirectoryName: {0}", DirectoryName);
			Console.Write("\n  ExtensionOrig: {0}", ExtensionOrig);
			Console.Write("\n      Extension: {0}", Extension);
			Console.Write("\n       FullName: {0}", FullName);
			Console.Write("\n           Name: {0}", Name);
			Console.Write("\n   ParentFolder: {0}", ParentFolder);
			Console.Write("\n      NewFolder: {0}", NewFolder);
			Console.Write("\n           Root: {0}", Root);
			Console.Write("\n           Size: {0}", BytesToString(Size));
			Console.Write("\n");
		}

		/// <summary>
		/// Fast file copy with big buffers. If &lt;destFileName&gt; exists, it will be overwritten.
		/// </summary>
		/// <param name="destFileName">The name of the destination file. This cannot be a directory.</param>
		/// <seealso cref="https://www.codeproject.com/Tips/777322/A-Faster-File-Copy"/>
		protected void FastCopyTo(string destFileName)
		{
			int read;
			var array_length = FIVE_HUNDRED_TWELVE_KB;
			var dataArray = new byte[array_length];

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
		}

		/// <summary>
		/// Fast file move with big buffers. If &lt;destFileName&gt; exists, it will be overwritten.
		/// </summary>
		/// <param name="destFileName">The name of the destination file. This cannot be a directory.</param>
		protected void FastMoveTo(string destFileName)
		{
			if (Root.ToUpper().Equals(Path.GetPathRoot(destFileName).ToUpper())) {
				if (File.Exists(destFileName))
					new FileInfo(destFileName).Delete();
				new FileInfo(FullName).MoveTo(destFileName);
			}

			else {
				FastCopyTo(destFileName);
				TryDelete();
			}
		}

		/// <summary>
		/// Sets the DateTaken WITHOUT loading the whole image.
		/// </summary>
		protected void SetDateTaken()
		{
			try {
				using (var fs = new FileStream(FullName, FileMode.Open, FileAccess.Read)) {
					using (var img = Image.FromStream(fs, false, false)) {
						var propItem = img.GetPropertyItem(PROPERTY_TAG_EXIF_DATE_TAKEN);
						var dateTaken = Regex_Colon.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
						DateTakenStr = dateTaken ?? "";

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
			//var parentFolderUpper = ParentFolder.ToUpper();
			//var bypassList = new List<string> { "_PROCESS THESE", "_TEST", "_UNPROCESSED", "100ANDRO", "101_PANA", "102_PANA", "103_PANA", "APPS", "CAMERA", "CAMERA ROLL", "DOCUMENTS", "DOWNLOAD", "DOWNLOADS", "GAMES", "IMAGES", "OLD", "PICTURES", "SCREENSHOT", "SCREENSHOTS", "TEST", "XPERIA TL", };

			if (SpecialFolders.Contains(ParentFolder))
				NewFolder = String.Format(@"{0}\", ParentFolder);
			else {
				if (MinDateTime < ReallyOldThreshold)
					NewFolder = @"ReallyOld\";
				else if (MinDateTime < OldThreshold)
					NewFolder = @"Old\";

				NewFolder = String.Format(@"{0}{1}\", NewFolder, MinDateTime.Year);

				if (DefaultCameraFolders.Contains(ParentFolder, StringComparer.OrdinalIgnoreCase) || int.TryParse(ParentFolder, out var result))
					return;
				else
					NewFolder = String.Format(@"{0}{1}\", NewFolder, ParentFolder);
			}

			#region Special Folders
			//for (int i = 0; i < SpecialFolders.Length; i++) {
			//	if (Regex.IsMatch(FullName, String.Format(@"[^a-zA-Z]{0}[^a-zA-Z]", SpecialFolders[i][0]), RegexOptions.IgnoreCase)) {
			//		if (SpecialFolders[i][1].ToUpper().Contains(parentFolderUpper))
			//			NewFolder = String.Format("{0}{1}", NewFolder, SpecialFolders[i][1]);
			//		else if (bypassList.Exists(x => x.Equals(parentFolderUpper) || int.TryParse(ParentFolder, out result)))
			//			NewFolder = String.Format("{0}{1}", NewFolder, SpecialFolders[i][1]);
			//		else
			//			NewFolder = String.Format(@"{0}{1}{2}\", NewFolder, SpecialFolders[i][1], ParentFolder);

			//		return; // only match one, then return //
			//	}
			//}
			//#endregion Special Folders

			//#region Default: Everything else
			//if (bypassList.Exists(x => x.Equals(parentFolderUpper)) || int.TryParse(ParentFolder, out result))
			//	;
			//else
			//	NewFolder = String.Format(@"{0}{1}\", NewFolder, ParentFolder);
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

			if (File.Exists(destFileName)) {
				string ext, newExt;
				switch (overwriteOption) {
					#region case OverwriteOption.Overwrite:
					case OverwriteOption.Overwrite:
						FastCopyTo(destFileName);
						return;
					#endregion
					#region case OverwriteOption.OverwriteIfSourceNewer:
					case OverwriteOption.OverwriteIfSourceNewer:
						if (LastWriteTime > new FileInfo(destFileName).LastWriteTime)
							FastCopyTo(destFileName);
						return;
					#endregion
					#region case OverwriteOption.OverwriteIfDifferentSize:
					case OverwriteOption.OverwriteIfDifferentSize:
						if (Size != new FileInfo(destFileName).Length)
							FastCopyTo(destFileName);
						return;
					#endregion
					#region case OverwriteOption.OverwriteIfDifferentSizeOrSourceNewer:
					case OverwriteOption.OverwriteIfDifferentSizeOrSourceNewer:
						var destFile = new FileInfo(destFileName);
						if (Size != destFile.Length || LastWriteTime > destFile.LastWriteTime)
							FastCopyTo(destFileName);
						destFile = null;
						return;
					#endregion
					#region case OverwriteOption.Rename:
					case OverwriteOption.Rename:
						ext = Path.GetExtension(destFileName);
						newExt = String.Format(" Copy{0}", ext);
						CopyTo(destFileName.Replace(ext, newExt), overwriteOption);
						return;
					#endregion
					#region case OverwriteOption.RenameIfDifferentSize:
					case OverwriteOption.RenameIfDifferentSize:
						if (Size != new FileInfo(destFileName).Length) {
							ext = Path.GetExtension(destFileName);
							newExt = String.Format(" Copy{0}", ext);
							CopyTo(destFileName.Replace(ext, newExt), overwriteOption);
						}
						return;
					#endregion
					#region case OverwriteOption.Skip:
					case OverwriteOption.Skip:
						return;
					#endregion
					default:
						throw new Exception(String.Format("Unsupported OverwriteOption.{0}", Environment.NewLine));
				}
			}

			else
				FastCopyTo(destFileName);
		}

		/// <summary>
		/// Moves a specified file to a new location, providing the option to specify a new file name.
		/// </summary>
		/// <param name="destFileName">The path to move the file to, which can specify a different file name.</param>
		/// <param name="overwriteOption">One of the enumeration values that specifies whether to overwrite or not if the destination file already exists.</param>
		public void MoveTo(string destFileName, OverwriteOption overwriteOption = OverwriteOption.Skip)
		{
			#region Input Check
			if (String.IsNullOrWhiteSpace(destFileName))
				throw new Exception(String.Format("<destFileName> is required.{0}", Environment.NewLine));
			if (FullName.ToUpper().Equals(destFileName.ToUpper()))
				return;
			#endregion Input Check

			if (File.Exists(destFileName)) {
				string ext, newExt;
				switch (overwriteOption) {
					#region case OverwriteOption.Overwrite:
					case OverwriteOption.Overwrite:
						FastMoveTo(destFileName);
						return;
					#endregion
					#region case OverwriteOption.OverwriteIfSourceNewer:
					case OverwriteOption.OverwriteIfSourceNewer:
						if (LastWriteTime > new FileInfo(destFileName).LastWriteTime)
							FastMoveTo(destFileName);
						return;
					#endregion
					#region case OverwriteOption.OverwriteIfDifferentSize:
					case OverwriteOption.OverwriteIfDifferentSize:
						if (Size != new FileInfo(destFileName).Length)
							FastMoveTo(destFileName);
						return;
					#endregion
					#region case OverwriteOption.OverwriteIfDifferentSizeOrSourceNewer:
					case OverwriteOption.OverwriteIfDifferentSizeOrSourceNewer:
						var destFile = new FileInfo(destFileName);
						if (Size != destFile.Length || LastWriteTime > destFile.LastWriteTime)
							FastMoveTo(destFileName);
						destFile = null;
						return;
					#endregion
					#region case OverwriteOption.Rename:
					case OverwriteOption.Rename:
						ext = Path.GetExtension(destFileName);
						newExt = String.Format(" Copy{0}", ext);
						MoveTo(destFileName.Replace(ext, newExt), overwriteOption);
						return;
					#endregion
					#region case OverwriteOption.RenameIfDifferentSize:
					case OverwriteOption.RenameIfDifferentSize:
						if (Size != new FileInfo(destFileName).Length) {
							ext = Path.GetExtension(destFileName);
							newExt = String.Format(" Copy{0}", ext);
							MoveTo(destFileName.Replace(ext, newExt), overwriteOption);
						}
						return;
					#endregion
					#region case OverwriteOption.Skip:
					case OverwriteOption.Skip:
						return;
					#endregion
					default:
						throw new Exception(String.Format("Unsupported OverwriteOption.{0}", Environment.NewLine));
				}
			}

			else
				FastMoveTo(destFileName);
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
				new FileInfo(FullName).Delete();
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

		/// <summary>
		/// Moves a specified file to a new location, providing the option to specify a new file name.
		/// </summary>
		/// <param name="destFileName">The path to move the file to, which can specify a different file name.</param>
		/// <param name="overwriteOption">One of the enumeration values that specifies whether to overwrite or not if the destination file already exists.</param>
		/// <returns>True if the move succeeded. False otherwise.</returns>
		public bool TryMoveTo(string destFileName, OverwriteOption overwriteOption = OverwriteOption.Skip)
		{
			try {
				MoveTo(destFileName, overwriteOption);
				return true;
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in YuFile.TryMoveTo(string destFileName='{3}', OverwriteOption overwriteOption='{4}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, destFileName, overwriteOption.ToString());
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of YuFile.TryMoveTo(string destFileName='{3}', OverwriteOption overwriteOption='{4}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, destFileName, overwriteOption.ToString());

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