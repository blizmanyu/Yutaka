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
		protected const int PROPERTY_TAG_EXIF_DATE_TAKEN = 36867; // PropertyTagExifDTOrig // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.imaging.propertyitem.id //
		protected static readonly DateTime DateThreshold = new DateTime(1960, 1, 1);
		protected static readonly DateTime TwoYearsAgo = DateTime.Today.AddYears(-2);
		protected static readonly Regex Regex_Colon = new Regex(":", RegexOptions.Compiled);
		/// <summary>
		/// The path originally specified by the user, whether relative or absolute.
		/// </summary>
		protected string OriginalPath;

		public bool IsReadOnly { get; set; }
		public DateTime CreationTime { get; set; }
		public DateTime CreationTimeUtc { get; set; }
		public DateTime LastAccessTime { get; set; }
		public DateTime LastAccessTimeUtc { get; set; }
		public DateTime LastWriteTime { get; set; }
		public DateTime LastWriteTimeUtc { get; set; }
		#region public DateTime? DateTaken { }
		public DateTime? DateTaken {
			get {
				if (DateTaken == null)
					SetDateTaken();

				return DateTaken;
			} 

			private set { }
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
				#region CreationTimeUtc = fInfo.CreationTimeUtc;
				try {
					CreationTimeUtc = fInfo.CreationTimeUtc;
				}

				catch {
					try {
						CreationTimeUtc = fInfo.LastWriteTime;
					}

					catch {
						CreationTimeUtc = fInfo.LastAccessTime;
					}
				}
				#endregion
				LastAccessTime = fInfo.LastAccessTime;
				LastAccessTimeUtc = fInfo.LastAccessTimeUtc;
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
				#region LastWriteTimeUtc = fInfo.LastWriteTimeUtc;
				try {
					LastWriteTimeUtc = fInfo.LastWriteTimeUtc;
				}

				catch {
					try {
						LastWriteTimeUtc = fInfo.CreationTimeUtc;
					}

					catch {
						LastWriteTimeUtc = fInfo.LastAccessTime;
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
			Console.Write("\n  CreationTimeUtc: {0:yyyy-MM-dd HH:mm:ss.fff}", CreationTimeUtc);
			Console.Write("\n   LastAccessTime: {0:yyyy-MM-dd HH:mm:ss.fff}", LastAccessTime);
			Console.Write("\nLastAccessTimeUtc: {0:yyyy-MM-dd HH:mm:ss.fff}", LastAccessTimeUtc);
			Console.Write("\n    LastWriteTime: {0:yyyy-MM-dd HH:mm:ss.fff}", LastWriteTime);
			Console.Write("\n LastWriteTimeUtc: {0:yyyy-MM-dd HH:mm:ss.fff}", LastWriteTimeUtc);
			Console.Write("\n        DateTaken: {0:yyyy-MM-dd HH:mm:ss.fff}", DateTaken);
			Console.Write("\n             Size: {0:n0}", Size);
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
		#endregion
	}
}