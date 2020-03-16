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
		public DateTime DateTaken;
		public DateTime LastAccessTime;
		public DateTime LastWriteTime;
		public DateTime MaxDateTime;
		public DateTime MinDateTime;
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
				Size = fi.Length;
				Name = fi.Name.Replace(ExtensionOrig, Extension);
				Root = Path.GetPathRoot(FullName);

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
		/// Dumps all field values to Console.
		/// </summary>
		public void Debug()
		{
			Console.Write("\n");
			Console.Write("\n   CreationTime: {0}", CreationTime);
			Console.Write("\n LastAccessTime: {0}", LastAccessTime);
			Console.Write("\n  LastWriteTime: {0}", LastWriteTime);
			Console.Write("\n  DirectoryName: {0}", DirectoryName);
			Console.Write("\n  ExtensionOrig: {0}", ExtensionOrig);
			Console.Write("\n      Extension: {0}", Extension);
			Console.Write("\n       FullName: {0}", FullName);
			Console.Write("\n           Name: {0}", Name);
			Console.Write("\n           Root: {0}", Root);
			Console.Write("\n           Size: {0}", Size);
			Console.Write("\n");
		}

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
		/// <param name="overwriteOption">One of the enumeration values that specifies whether to overwrite or not if the destination file already exists.</param>
		public void CopyTo(string destFileName, OverwriteOption overwriteOption = OverwriteOption.Skip)
		{
			#region Input Check
			if (String.IsNullOrWhiteSpace(destFileName))
				throw new Exception(String.Format("<destFileName> is required.{0}", Environment.NewLine));
			if (FullName.ToUpper().Equals(destFileName.ToUpper()))
				return;
			#endregion Input Check

			try {
				var destFileExists = File.Exists(destFileName);

				switch (overwriteOption) {
					#region case OverwriteOption.Overwrite:
					case OverwriteOption.Overwrite:
						FastCopyTo(destFileName);
						break;
					#endregion
					#region case OverwriteOption.Skip:
					case OverwriteOption.Skip:
						if (!destFileExists)
							FastCopyTo(destFileName);
						break;
					#endregion
					#region case OverwriteOption.KeepBoth:
					case OverwriteOption.KeepBoth:
						if (destFileExists)
							FastCopyTo(FileUtil.AutoRename(destFileName));
						else
							FastCopyTo(destFileName);
						break;
					#endregion
					#region case OverwriteOption.Smart:
					case OverwriteOption.Smart:
						if (destFileExists) {
							if (!this.Equals(new YuFile(destFileName)))
								FastCopyTo(FileUtil.AutoRename(destFileName));
						}

						else
							FastCopyTo(destFileName);
						break;
					#endregion
					default:
						break;
				}
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in YuFile.CopyTo(string destFileName='{3}', OverwriteOption overwriteOption='{4}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, destFileName, overwriteOption.ToString());
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of YuFile.CopyTo(string destFileName='{3}', OverwriteOption overwriteOption='{4}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, destFileName, overwriteOption.ToString());

				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// Permanently deletes the file.
		/// </summary>
		public void Delete()
		{
			try {
				new FileInfo(FullName).Delete();
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in YuFile.Delete(){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of YuFile.Delete(){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);

				throw new Exception(log);
				#endregion Log
			}
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
				Delete();
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