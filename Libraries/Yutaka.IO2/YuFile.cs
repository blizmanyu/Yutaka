using System;
using System.IO;

namespace Yutaka.IO2
{
	public class YuFile
	{
		#region Fields
		public static readonly DateTime UNIX_TIME = new DateTime(1970, 1, 1);
		public static readonly int FIVE_HUNDRED_TWELVE_KB = (int) Math.Pow(2, 19);
		public DateTime CreationTime;
		public DateTime LastAccessTime;
		public DateTime LastWriteTime;
		public string DirectoryName;
		protected string ExtensionOrig;
		public string Extension;
		public string FullName;
		public string Name;
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
				Root = Path.GetPathRoot(FullName);
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
		#endregion Constructor

		#region Utilities
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
		#endregion Utilities

		#region Methods
		/// <summary>
		/// Copies an existing file to a new file.
		/// </summary>
		/// <param name="destFileName">The name of the new file to copy to.</param>
		/// <param name="overwriteOption">The <see cref="OverwriteOption"/> to use.</param>
		/// <returns></returns>
		public bool CopyTo(string destFileName, OverwriteOption overwriteOption = OverwriteOption.Skip)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(destFileName))
				log = String.Format("{0}<destFileName> is required.{1}", log, Environment.NewLine);
			if (FullName.ToUpper().Equals(destFileName.ToUpper()))
				log = String.Format("{0}<this> and <destFileName> are the same.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in YuFile.CopyTo(string destFileName, OverwriteOption overwriteOption).{1}", log, Environment.NewLine);
				Console.Write("\n{0}\n", log);
				return false;
			}
			#endregion Input Check

			try {
				var destFileExists = File.Exists(destFileName);

				switch (overwriteOption) {
					#region case OverwriteOption.Overwrite:
					case OverwriteOption.Overwrite:
						return FastCopyTo(destFileName);
					#endregion
					#region case OverwriteOption.Skip:
					case OverwriteOption.Skip:
						if (destFileExists)
							return false;

						return FastCopyTo(destFileName);
					#endregion
					#region case OverwriteOption.KeepBoth:
					case OverwriteOption.KeepBoth:
						if (destFileExists)
							return FastCopyTo(FileUtil.AutoRename(destFileName));

						return FastCopyTo(destFileName);
					#endregion
					#region case OverwriteOption.Smart:
					case OverwriteOption.Smart:
						if (destFileExists) {
							if (Size == new FileInfo(destFileName).Length)
								return true;

							return FastCopyTo(FileUtil.AutoRename(destFileName));
						}

						return FastCopyTo(destFileName);
					#endregion
					default:
						return false;
				}
			}

			catch (Exception ex) {
				#region Log
				log = "";

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in YuFile.CopyTo(string destFileName='{3}', OverwriteOption overwriteOption='{4}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, destFileName, overwriteOption.ToString());
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of YuFile.CopyTo(string destFileName='{3}', OverwriteOption overwriteOption='{4}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, destFileName, overwriteOption.ToString());

				Console.Write("\n{0}", log);
				#endregion Log

				return false;
			}
		}

		#region Overrides
		public override bool Equals(Object obj)
		{
			if ((obj == null) || !GetType().Equals(obj.GetType()))
				return false;

			return Size == ((YuFile) obj).Size;
		}

		public override int GetHashCode()
		{
			return Size.GetHashCode();
		}
		#endregion Overrides
		#endregion Methods
	}
}