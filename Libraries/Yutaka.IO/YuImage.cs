using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.IO
{
	public class YuImage
	{
		public DateTime CreationTime;
		public DateTime LastAccessTime;
		public DateTime LastWriteTime;
		public DateTime MinDateTime;
		public long Length;
		public string Extension;
		public string FullName;
		public string Name;
		public string NewPath;
		public string ParentDirectory;

		public YuImage(string filename)
		{
			var fi = new FileInfo(filename);
			CreationTime = fi.CreationTime;
			LastAccessTime = fi.LastAccessTime;
			LastWriteTime = fi.LastWriteTime;
			//MinDateTime = fi.MinDateTime;
			Length = fi.Length;
			Extension = fi.Extension;
			FullName = fi.FullName;
			Name = fi.Name;
			//NewPath = fi.NewPath;
			//ParentDirectory = fi.ParentDirectory;

			fi = null;
		}
	}
}