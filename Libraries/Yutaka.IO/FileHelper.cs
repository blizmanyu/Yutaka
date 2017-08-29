using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Yutaka.IO
{
	public class FileHelper
	{
		#region Fields
		// Constants //
		const int DATE_TAKEN = 36867; // PropertyTagExifDTOrig

		// Config/Settings //
		private static bool consoleOut = true;
		private static string mode = "copy";

		// PIVs //
		private static DateTime dateThreshold = new DateTime(1982, 1, 1);
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

		private static DateTime GetMinTime(FileInfo fi)
		{
			if (fi == null)
				return dateThreshold;

			var creationTime = fi.CreationTime;
			var lastAccessTime = fi.LastAccessTime;
			var lastWriteTime = fi.LastWriteTime;
			var minTime = DateTime.MaxValue;

			if (creationTime < minTime)
				minTime = creationTime;
			if (lastAccessTime < minTime)
				minTime = lastAccessTime;
			if (lastWriteTime < minTime)
				minTime = lastWriteTime;

			if (minTime > dateThreshold)
				return minTime;

			return dateThreshold;
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
		#endregion
	}
}