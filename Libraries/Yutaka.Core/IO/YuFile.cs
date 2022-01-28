using System;
using System.IO;

namespace Yutaka.Core.IO
{
	public class YuFile
	{
		#region Fields
		protected static readonly DateTime DateThreshold = new DateTime(1960, 1, 1);
		protected static readonly DateTime TwoYearsAgo = DateTime.Today.AddYears(-2);
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
	}
}