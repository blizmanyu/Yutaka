using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Yutaka.Core.IO
{
	public class YuFile
	{
		#region Fields
		public const decimal ONE_KB = 1024m;
		public const decimal ONE_MB = 1048576m;
		public const decimal ONE_GB = 1073741824m;
		public const decimal ONE_TB = 1099511627776m;
		public const decimal ONE_PB = 1125899906842624m;
		protected const int PROPERTY_TAG_EXIF_DATE_TAKEN = 36867; // PropertyTagExifDTOrig // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.imaging.propertyitem.id //
		protected static readonly DateTime MinDateThreshold = new DateTime(1960, 1, 1);
		protected static readonly DateTime TwoYearsAgo = DateTime.Today.AddYears(-2);
		protected static readonly Regex Regex_Colon = new Regex(":", RegexOptions.Compiled);
		/// <summary>
		/// The path originally specified by the user, whether relative or absolute.
		/// </summary>
		protected string OriginalPath;
		protected DateTime? _dateTaken;
		protected DateTime? _minDate;
		#endregion

		#region Properties
		public bool IsReadOnly { get; set; }
		public DateTime CreationTime { get; set; }
		public DateTime LastAccessTime { get; set; }
		public DateTime LastWriteTime { get; set; }
		#region public DateTime? DateTaken { }
		public DateTime? DateTaken {
			get {
				if (_dateTaken == null)
					SetDateTaken();

				return _dateTaken;
			} 
		}
		#endregion
		#region public DateTime MinDate { }
		public DateTime MinDate
		{
			get {
				if (_minDate == null)
					SetMinDate();

				return _minDate.Value;
			}
		}
		#endregion
		/// <summary>
		/// Gets the size, in bytes, of the current file.
		/// </summary>
		public long Size { get; }
		/// <summary>
		/// Gets the extension part of the file name, including the leading dot . even if it is the entire file name, or an empty string if no extension is present.
		/// </summary>
		public string Extension { get; }
		/// <summary>
		/// Gets a string representing the directory's full path.
		/// </summary>
		public string FullDirectoryPath { get; }
		/// <summary>
		/// Gets the full path of the file.
		/// </summary>
		public string FullPath { get; }
		/// <summary>
		/// Gets the name of the file.
		/// </summary>
		public string Name { get; }
		public string ParentDirectory { get; }
		public string Root { get; }
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="YuFile"/> class, which acts as a wrapper for a file path.
		/// </summary>
		/// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
		public YuFile(string fileName = null)
		{
			if (fileName == null)
				throw new Exception(String.Format("fileName is null.{0}Exception thrown in Constructor YuFile(string fileName).{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(fileName))
				throw new Exception(String.Format("fileName is empty.{0}Exception thrown in Constructor YuFile(string fileName).{0}", Environment.NewLine));

			FileInfo fInfo;

			try {
				OriginalPath = fileName;
				fInfo = new FileInfo(fileName);
				IsReadOnly = fInfo.IsReadOnly;
				#region CreationTime = fInfo.CreationTime;
				try {
					CreationTime = fInfo.CreationTime;
				}

				catch {
					try {
						CreationTime = fInfo.LastWriteTime;
					}

					catch {
						CreationTime = fInfo.LastAccessTime;
					}
				}
				#endregion
				LastAccessTime = fInfo.LastAccessTime;
				#region LastWriteTime = fInfo.LastWriteTime;
				try {
					LastWriteTime = fInfo.LastWriteTime;
				}

				catch {
					try {
						LastWriteTime = fInfo.CreationTime;
					}

					catch {
						LastWriteTime = fInfo.LastAccessTime;
					}
				}
				#endregion
				Size = fInfo.Length;
				Extension = fInfo.Extension;
				FullDirectoryPath = fInfo.DirectoryName;
				FullPath = fInfo.FullName;
				Name = fInfo.Name;

				fInfo = null;
			}

			catch (Exception ex) {
				fInfo = null;
				#region Log
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in Constructor YuFile(string fileName='{3}').{0}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, fileName));
				else
					throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Constructor YuFile(string fileName='{3}').{0}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, fileName));
				#endregion
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Dumps all field values to Console.
		/// </summary>
		public void DumpToConsole()
		{
			Console.Write("\n");
			Console.Write("\n     OriginalPath: {0}", OriginalPath);
			Console.Write("\n       IsReadOnly: {0}", IsReadOnly);
			Console.Write("\n     CreationTime: {0:yyyy-MM-dd HH:mm:ss.fff}", CreationTime);
			Console.Write("\n   LastAccessTime: {0:yyyy-MM-dd HH:mm:ss.fff}", LastAccessTime);
			Console.Write("\n    LastWriteTime: {0:yyyy-MM-dd HH:mm:ss.fff}", LastWriteTime);
			Console.Write("\n        DateTaken: {0:yyyy-MM-dd HH:mm:ss.fff}", DateTaken);
			Console.Write("\n          MinDate: {0:yyyy-MM-dd HH:mm:ss.fff}", MinDate);
			Console.Write("\n             Size: {0:n0} bytes", Size);
			Console.Write("\n        Extension: {0}", Extension);
			Console.Write("\nFullDirectoryPath: {0}", FullDirectoryPath);
			Console.Write("\n         FullPath: {0}", FullPath);
			Console.Write("\n             Name: {0}", Name);
			Console.Write("\n  ParentDirectory: {0}", ParentDirectory);
			Console.Write("\n             Root: {0}", Root);
			Console.Write("\n");
		}

		/// <summary>
		/// Sets the DateTaken WITHOUT loading the whole image.
		/// </summary>
		protected void SetDateTaken()
		{
			try {
				using (var fs = new FileStream(FullPath, FileMode.Open, FileAccess.Read)) {
					using (var img = Image.FromStream(fs, false, false)) {
						var propItem = img.GetPropertyItem(PROPERTY_TAG_EXIF_DATE_TAKEN);
						var dateTaken = Regex_Colon.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);

						if (DateTime.TryParse(dateTaken, out var result))
							_dateTaken = result;
						else
							_dateTaken = null;
					}
				}
			}

			catch (Exception) {
				_dateTaken = null;
			}
		}

		/// <summary>
		/// Determines the MinDate, then sets it.
		/// </summary>
		protected void SetMinDate()
		{
			_minDate = DateTime.Now;

			if (MinDateThreshold < CreationTime && CreationTime < _minDate)
				_minDate = CreationTime;
			if (MinDateThreshold < LastWriteTime && LastWriteTime < _minDate)
				_minDate = LastWriteTime;
			if (MinDateThreshold < LastAccessTime && LastAccessTime < _minDate)
				_minDate = LastAccessTime;

			/// On most devices, the DateTaken field doesn't have millisecond precision, so it rounds to the nearest second.
			/// So if the difference is less than 1 second, the current MinDate carries a more precise DateTime.
			if (DateTaken.HasValue && MinDateThreshold < DateTaken && DateTaken < _minDate) {
				var diff = _minDate.Value - DateTaken.Value;

				if (diff.TotalSeconds < -.999 || .999 < diff.TotalSeconds)
					_minDate = DateTaken.Value;
			}
		}

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
		#endregion
	}
}