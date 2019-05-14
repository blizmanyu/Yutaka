using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
		public DateTime NullDateTimeThreshold = new DateTime(1970, 1, 1); // based on Unix time //
		public long Length;
		public string Extension;
		public string FullName;
		public string Name;
		public string NewFolder;
		public string ParentFolder;

		public YuImage(string filename)
		{
			var fi = new FileInfo(filename);
			CreationTime = fi.CreationTime;
			LastAccessTime = fi.LastAccessTime;
			LastWriteTime = fi.LastWriteTime;
			Length = fi.Length;
			Extension = fi.Extension;
			FullName = fi.FullName;
			Name = fi.Name;
			ParentFolder = fi.Directory.Name;
			fi = null;

			DateTaken = GetDateTaken(filename);
			MinDateTime = GetMinDateTime();
			NewFolder = GetNewPath();
		}

		// Retrieves the datetime WITHOUT loading the whole image //
		public DateTime GetDateTaken(string path)
		{
			var r = new Regex(":");

			try {
				using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
					using (var myImage = Image.FromStream(fs, false, false)) {
						var propItem = myImage.GetPropertyItem(PROPERTY_TAG_EXIF_DATE_TAKEN);
						var dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
						return DateTime.Parse(dateTaken);
					}
				}
			}

			catch (Exception) {
				return new DateTime();
			}
		}

		public DateTime GetMinDateTime()
		{
			var minDateTime = new DateTime();

			// TODO //

			return minDateTime;
		}

		public string GetNewPath()
		{
			return null;
		}
	}
}