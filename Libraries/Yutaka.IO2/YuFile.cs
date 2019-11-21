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

		/// <summary>
		/// Initializes a new instance of the <see cref="YuFile"/> class, which acts as a wrapper for a file path.
		/// </summary>
		/// <param name="filename">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
		public YuFile(string filename = null)
		{
			if (String.IsNullOrWhiteSpace(filename))
				throw new Exception(String.Format("<filename> is required. Exception thrown in constructor YuFile(string filename).{0}{0}", Environment.NewLine));

			try {
				var fi = new FileInfo(filename);
				fi.IsReadOnly = false;
				#region CreationTime = fi.CreationTime;
				try {
					CreationTime = fi.CreationTime;
				}
				catch (Exception) {
					CreationTime = new DateTime();
				}
				#endregion CreationTime = fi.CreationTime;
				LastAccessTime = fi.LastAccessTime;
				#region LastWriteTime = fi.LastWriteTime;
				try {
					LastWriteTime = fi.LastWriteTime;
				}
				catch (Exception) {
					LastWriteTime = new DateTime();
				}
				#endregion LastWriteTime = fi.LastWriteTime;
				DirectoryName = fi.DirectoryName;
				ExtensionOrig = fi.Extension;
				Extension = ExtensionOrig.ToLower();
				FullName = fi.FullName.Replace(ExtensionOrig, Extension);
				Size = fi.Length;
				Name = fi.Name.Replace(ExtensionOrig, Extension);
				fi = null;
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in constructor YuFile(string filename='{3}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, filename);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of constructor YuFile(string filename='{3}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, filename);

				Console.Write("\n{0}", log);
				#endregion Log
			}
		}
	}
}