using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Yutaka.IO
{
	public static class FileUtil
	{
		#region Fields
		// Constants //
		const int DATE_TAKEN = 36867; // PropertyTagExifDTOrig //
		const int ONE_GIGABYTE = 1073741824; // Math.Pow(2, 30) //
		const int ONE_MEGABYTE =    1048576; // Math.Pow(2, 20) //
		const int FIVE_TWELVE_KB =   524288; // Math.Pow(2, 19) //
		const int ONE_KILOBYTE =       1024; // Math.Pow(2, 10) //
		const int BUFFER = FIVE_TWELVE_KB;

		// Config/Settings //
		private static bool consoleOut = true;
		private static string mode = "copy";

		// PIVs //
		private static DateTime dateThreshold = new DateTime(1982, 1, 1);
		private static HashSet<string> audioExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".aiff", ".m4a", ".mp3", ".au", ".ogg", ".wav", ".wma" };
		private static HashSet<string> applicationExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".asdf", ".asdf", ".asdf" };
		private static HashSet<string> archiveExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".rar", ".zip", ".7z", ".ace", ".arj", ".bz2", ".cab", ".gz", ".iso", ".jar", ".lz", ".lzh", ".tar", ".uue", ".xz", ".z", ".zipx", ".001" };
		private static HashSet<string> documentExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".ppt", ".pptx" };
		private static HashSet<string> imageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".ai", ".bmp", ".eps", ".gif", ".ico", ".jpg", ".jpeg", ".png", ".psd", ".tiff" };
		private static HashSet<string> videoExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".3gp", ".avi", ".flv", ".m4v", ".mkv", ".mpg", ".mpeg", ".mp4", ".ogv", ".mov", ".webm", ".wmv" };
		#endregion

		#region Private Helpers
		private static bool CopyFile(FileInfo source, string dest)
		{
			try {
				source.CopyTo(dest);
				return true;
			}

			catch (Exception ex) {
				if (consoleOut)
					DisplayException(ex);
				return false;
			}
		}

		private static void DisplayException(Exception ex)
		{
			Console.Write("\n{0}", ex.Message);
			Console.Write("\n");
			Console.Write("\n{0}", ex.ToString());
		}

		// Retrieves the datetime WITHOUT loading the whole image //
		private static DateTime GetDateTakenFromImage(string path)
		{
			var r = new Regex(":");

			try {
				using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
					using (var myImage = Image.FromStream(fs, false, false)) {
						var propItem = myImage.GetPropertyItem(DATE_TAKEN);
						var dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
						return DateTime.Parse(dateTaken);
					}
				}
			}

			catch (Exception) {
				return dateThreshold;
			}
		}

		private static bool MoveFile(FileInfo source, string dest)
		{
			try {
				source.MoveTo(dest);
				return true;
			}

			catch (Exception ex) {
				if (consoleOut)
					DisplayException(ex);
				return false;
			}
		}
		#endregion

		#region Public Methods
		public static void CopyFile(string source, string dest, bool delete = false)
		{
			if (String.IsNullOrEmpty(source))
				throw new Exception("<source> can't be empty");

			if (String.IsNullOrEmpty(dest))
				throw new Exception("<dest> can't be empty");

			var destInfo = new FileInfo(dest);
			var sourceInfo = new FileInfo(source);
			var sourceCreationTime = sourceInfo.CreationTime;
			var sourceLastAccessTime = sourceInfo.LastAccessTime;
			var sourceLastWriteTime = sourceInfo.LastWriteTime;
			var sourceLength = sourceInfo.Length;

			Console.Write("\nCreationTime: {0}", sourceCreationTime);
			Console.Write("\nLastAccessTime: {0}", sourceLastAccessTime);
			Console.Write("\nLastWriteTime: {0}", sourceLastWriteTime);
			Console.Write("\nLength: {0} bytes", sourceLength);

			if (destInfo.Exists) {
				var destLength = dest.Length;
				if (sourceLength == destLength) {
					Console.Write("\nExact file exists already.");
					return;
				}
				else {
					CopyFile(sourceInfo, dest + "2");
				}
			}

			else /*!destInfo.Exists*/ {
				CopyFile(sourceInfo, dest);
			}
		}

		/// <summary>
		/// Returns an IEnumerable of audio FileInfos that matches a specified search patthern and search subdirectory option.
		/// </summary>
		/// <param name="path">A string specifying the path on which to create the DirectoryInfo.</param>
		/// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions. The default pattern is "*", which returns all files.</param>
		/// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is TopDirectoryOnly.</param>
		/// <returns></returns>
		public static IEnumerable<FileInfo> EnumerateAudioFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return new DirectoryInfo(path).EnumerateFiles(searchPattern, searchOption).Where(x => audioExtensions.Contains(x.Extension, StringComparer.OrdinalIgnoreCase));
		}

		/// <summary>
		/// Returns an IEnumerable of image FileInfos that matches a specified search patthern and search subdirectory option.
		/// </summary>
		/// <param name="path">A string specifying the path on which to create the DirectoryInfo.</param>
		/// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions. The default pattern is "*", which returns all files.</param>
		/// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is TopDirectoryOnly.</param>
		/// <returns></returns>
		public static IEnumerable<FileInfo> EnumerateImageFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return new DirectoryInfo(path).EnumerateFiles(searchPattern, searchOption).Where(x => imageExtensions.Contains(x.Extension, StringComparer.OrdinalIgnoreCase));
		}

		/// <summary>
		/// Returns an IEnumerable of video FileInfos that matches a specified search patthern and search subdirectory option.
		/// </summary>
		/// <param name="path">A string specifying the path on which to create the DirectoryInfo.</param>
		/// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions. The default pattern is "*", which returns all files.</param>
		/// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is TopDirectoryOnly.</param>
		/// <returns></returns>
		public static IEnumerable<FileInfo> EnumerateVideoFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return new DirectoryInfo(path).EnumerateFiles(searchPattern, searchOption).Where(x => videoExtensions.Contains(x.Extension, StringComparer.OrdinalIgnoreCase));
		}

		/// <summary> Fast file move with big buffers
		/// </summary>
		/// <param name="source">Source file path</param> 
		/// <param name="destination">Destination file path</param> 
		public static void FastMove(string source, string destination, bool delete = true)
		{
			var array_length = BUFFER;
			var dataArray = new byte[array_length];

			using (var fsread = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.None, array_length)) {
				using (var bwread = new BinaryReader(fsread)) {
					using (var fswrite = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None, array_length)) {
						using (var bwwrite = new BinaryWriter(fswrite)) {
							for (; ; ) {
								var read = bwread.Read(dataArray, 0, array_length);
								if (0 == read)
									break;
								bwwrite.Write(dataArray, 0, read);
							}
						}
					}
				}
			}

			if (delete)
				File.Delete(source);
		}

		/// <summary>
		/// Gets the MinDate based on CreationTime, LastAccessTime, and LastWriteTime. Sometimes a date can be "blank" if the source is from a removable media (flash drives,
		/// SD cards). In those cases, the date will be less than the dateThreshold of Jan 1, 1982, and will NOT return that date.
		/// </summary>
		/// <param name="fi">The FileInfo object</param>
		/// <returns></returns>
		public static DateTime GetMinTime(FileInfo fi)
		{
			if (fi == null)
				throw new ArgumentNullException();
			if ((fi.CreationTime < dateThreshold) && (fi.LastAccessTime < dateThreshold) && (fi.LastWriteTime < dateThreshold))
				throw new Exception("Something's wrong with this file. All 3 dates are before Jan 1, 1982");

			var minTime = fi.CreationTime;
			var lastAccessTime = fi.LastAccessTime;
			var lastWriteTime = fi.LastWriteTime;

			if (minTime < dateThreshold || lastAccessTime < minTime)
				minTime = lastAccessTime;
			if (minTime < dateThreshold || lastWriteTime < minTime)
				minTime = lastWriteTime;

			if (minTime > dateThreshold)
				return minTime;

			throw new Exception("Couldn't determine MinTime");
		}

		/// <summary>
		/// Gets the MinDate based on CreationTime, LastAccessTime, and LastWriteTime. Sometimes a date can be "blank" if the source is from a removable media (flash drives,
		/// SD cards). In those cases, the date will be less than the dateThreshold of Jan 1, 1982, and will NOT return that date. 
		/// </summary>
		/// <remarks>This method calls GetMinTime(FileInfo fi), so if performance is crucial, you may want to refactor your code.</remarks>
		/// <param name="fi">path to the File</param>
		/// <returns></returns>
		public static DateTime GetMinTime(string path)
		{
			return GetMinTime(new FileInfo(path));
		}

		public static bool IsSameDate(FileInfo fi1, FileInfo fi2)
		{
			try {
				if (fi1.LastWriteTimeUtc == fi2.LastWriteTimeUtc)
					return true;

				return false;
			}

			catch (Exception ex) {
				throw ex;
			}
		}

		public static bool IsSameDate(string path1, string path2)
		{
			return IsSameDate(new FileInfo(path1), new FileInfo(path2));
		}

		public static bool IsSameFile(FileInfo fi1, FileInfo fi2)
		{
			try {
				if (IsSameDate(fi1, fi2) && IsSameSize(fi1, fi2))
					return true;

				return false;
			}

			catch (Exception ex) {
				throw ex;
			}
		}

		public static bool IsSameFile(string path1, string path2)
		{
			return IsSameFile(new FileInfo(path1), new FileInfo(path2));
		}

		public static bool IsSameSize(FileInfo fi1, FileInfo fi2)
		{
			try {
				if (fi1.Length == fi2.Length)
					return true;

				return false;
			}

			catch (Exception ex) {
				throw ex;
			}
		}

		public static bool IsSameSize(string path1, string path2)
		{
			return IsSameSize(new FileInfo(path1), new FileInfo(path2));
		}

		/// <summary> Time the Move
		/// </summary> 
		/// <param name="source">Source file path</param> 
		/// <param name="destination">Destination file path</param> 
		public static void MoveTime(string source, string destination, bool delete = true)
		{
			var start_time = DateTime.Now;
			FastMove(source, destination, delete);
			var milliseconds = 1 + (int) ((DateTime.Now - start_time).TotalMilliseconds);
			var size = new FileInfo(destination).Length;
			// size time in milliseconds per sec
			var tsize = size * 1000 / milliseconds;
			if (tsize > ONE_GIGABYTE) {
				tsize = tsize / ONE_GIGABYTE;
				Console.Write("\n{0} transferred at {1}gb/sec", source, tsize);
			}
			else if (tsize > ONE_MEGABYTE) {
				tsize = tsize / ONE_MEGABYTE;
				Console.Write("\n{0} transferred at {1}mb/sec", source, tsize);
			}
			else if (tsize > ONE_KILOBYTE) {
				tsize = tsize / ONE_KILOBYTE;
				Console.Write("\n{0} transferred at {1}kb/sec", source, tsize);
			}
			else
				Console.Write("\n{0} transferred at {1}b/sec", source, tsize);
		}
		#endregion
	}
}