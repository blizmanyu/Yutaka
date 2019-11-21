using System;
using System.IO;

namespace Yutaka.IO2
{
	public class YuFile
	{
		public static readonly DateTime UNIX_TIME = new DateTime(1970, 1, 1);
		public DateTime CreationTime;
		public DateTime LastAccessTime;
		public DateTime LastWriteTime;
		public string DirectoryName;
		protected string ExtensionOrig;
		public string Extension;
		public string FullName;
		public string Name;
		public long Size;
	}
}