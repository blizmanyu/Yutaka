using System;
using System.IO;
using System.Linq;

namespace Yutaka.IO2
{
	public static class FileUtil
	{
		#region Fields
		const int FIVE_HUNDRED_TWELVE_KB = 524288;
		public static readonly DateTime UNIX_TIME = new DateTime(1970, 1, 1);
		#endregion Fields

		#region Utilities
		/// <summary>
		/// Fast file copy with big buffers. If &lt;destFileName&gt; exists, it will be overwritten.
		/// </summary>
		/// <param name="sourceFileName">The file to copy.</param>
		/// <param name="destFileName">The name of the destination file. This cannot be a directory.</param>
		/// <seealso cref="https://www.codeproject.com/Tips/777322/A-Faster-File-Copy"/>
		private static void FastCopy(string sourceFileName, string destFileName)
		{
			int read;
			var array_length = FIVE_HUNDRED_TWELVE_KB;
			var dataArray = new byte[array_length];

			using (var fsread = new FileStream(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.None, array_length)) {
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
		}

		/// <summary>
		/// Fast file move based on whether the source and destination are the same drive/volume or not. If &lt;destFileName&gt; exists, it will be overwritten.
		/// </summary>
		/// <param name="sourceFileName">The file to move.</param>
		/// <param name="destFileName">The name of the destination file. This cannot be a directory.</param>
		private static void FastMove(string sourceFileName, string destFileName)
		{
			if (Path.GetPathRoot(sourceFileName).ToUpper().Equals(Path.GetPathRoot(destFileName).ToUpper())) {
				if (File.Exists(destFileName))
					new FileInfo(destFileName).Delete();
				new FileInfo(sourceFileName).MoveTo(destFileName);
			}

			else {
				FastCopy(sourceFileName, destFileName);
				new FileInfo(sourceFileName).Delete();
			}
		}
		#endregion Utilities

		#region Public Methods
		/// <summary>
		/// Returns a new filename with " - Copy" appended to it.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static string AutoRename(string filename)
		{
			if (String.IsNullOrWhiteSpace(filename))
				throw new Exception(String.Format("<filename> is required.{0}", Environment.NewLine));

			var extension = Path.GetExtension(filename);

			while (File.Exists(filename))
				filename = filename.Replace(extension, String.Format(" - Copy{0}", extension));

			return filename;
		}

		/// <summary>
		/// Copies an existing file to a new file.
		/// </summary>
		/// <param name="sourceFileName">The file to copy.</param>
		/// <param name="destFileName">The name of the destination file. This cannot be a directory.</param>
		/// <param name="overwriteOption">One of the enumeration values that specifies whether to overwrite or not if the destination file already exists.</param>
		public static void Copy(string sourceFileName, string destFileName, OverwriteOption overwriteOption = OverwriteOption.Skip)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(sourceFileName))
				log = String.Format("{0}<sourceFileName> is required.{1}", log, Environment.NewLine);
			else if (!File.Exists(sourceFileName))
				log = String.Format("{0}<sourceFileName> doesn't exist.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(destFileName))
				log = String.Format("{0}<destFileName> is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in FileUtil.Copy(string sourceFileName, string destFileName, OverwriteOption overwriteOption).{1}", log, Environment.NewLine));

			if (destFileName.ToUpper().Equals(sourceFileName.ToUpper()))
				return;
			#endregion Input Check

			if (File.Exists(destFileName)) {
				string ext, newExt;
				switch (overwriteOption) {
					#region case OverwriteOption.Overwrite:
					case OverwriteOption.Overwrite:
						FastCopy(sourceFileName, destFileName);
						return;
					#endregion
					#region case OverwriteOption.OverwriteIfSourceNewer:
					case OverwriteOption.OverwriteIfSourceNewer:
						if (new FileInfo(sourceFileName).LastWriteTime > new FileInfo(destFileName).LastWriteTime)
							FastCopy(sourceFileName, destFileName);
						return;
					#endregion
					#region case OverwriteOption.OverwriteIfDifferentSize:
					case OverwriteOption.OverwriteIfDifferentSize:
						if (new FileInfo(sourceFileName).Length != new FileInfo(destFileName).Length)
							FastCopy(sourceFileName, destFileName);
						return;
					#endregion
					#region case OverwriteOption.OverwriteIfDifferentSizeOrSourceNewer:
					case OverwriteOption.OverwriteIfDifferentSizeOrSourceNewer:
						var sourceFile = new FileInfo(sourceFileName);
						var destFile = new FileInfo(destFileName);
						if (sourceFile.Length != destFile.Length || sourceFile.LastWriteTime > destFile.LastWriteTime)
							FastCopy(sourceFileName, destFileName);
						sourceFile = null;
						destFile = null;
						return;
					#endregion
					#region case OverwriteOption.Rename:
					case OverwriteOption.Rename:
						ext = Path.GetExtension(destFileName);
						newExt = String.Format(" Copy{0}", ext);
						Copy(sourceFileName, destFileName.Replace(ext, newExt), overwriteOption);
						return;
					#endregion
					#region case OverwriteOption.RenameIfDifferentSize:
					case OverwriteOption.RenameIfDifferentSize:
						if (new FileInfo(sourceFileName).Length != new FileInfo(destFileName).Length) {
							ext = Path.GetExtension(destFileName);
							newExt = String.Format(" Copy{0}", ext);
							Copy(sourceFileName, destFileName.Replace(ext, newExt), overwriteOption);
						}
						return;
					#endregion
					#region case OverwriteOption.Skip:
					case OverwriteOption.Skip:
						return;
					#endregion
					default:
						throw new Exception(String.Format("Unsupported OverwriteOption.{0}", Environment.NewLine));
				}
			}

			else
				FastCopy(sourceFileName, destFileName);
		}

		/// <summary>
		/// Deletes all files that match a search pattern in a specified path, and optionally searches subdirectories. Returns the number of files deleted.
		/// </summary>
		/// <param name="folderPath">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
		/// <param name="searchPattern">The search string to match against the names of files in &lt;folderPath&gt;. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
		/// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories.</param>
		/// <returns>The number of files deleted.</returns>
		public static int Delete(string folderPath, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			#region Input Check
			if (String.IsNullOrWhiteSpace(folderPath))
				return 0;
			if (!Directory.Exists(folderPath))
				return 0;
			if (String.IsNullOrWhiteSpace(searchPattern))
				searchPattern = "*";
			#endregion Input Check

			var count = 0;

			Directory.EnumerateFiles(folderPath, searchPattern, searchOption).AsParallel().ForAll(path => {
				try {
					File.Delete(path);
					count++;
				}

				catch (Exception) { }
			});

			return count;
		}

		/// <summary>
		/// Moves a specified file to a new location, providing the option to specify a new file name.
		/// </summary>
		/// <param name="sourceFileName">The name of the file to move. Can include a relative or absolute path.</param>
		/// <param name="destFileName">The new path and name for the file.</param>
		/// <param name="overwriteOption">One of the enumeration values that specifies whether to overwrite or not if the destination file already exists.</param>
		public static void Move(string sourceFileName, string destFileName, OverwriteOption overwriteOption = OverwriteOption.Skip)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(sourceFileName))
				log = String.Format("{0}<sourceFileName> is required.{1}", log, Environment.NewLine);
			else if (!File.Exists(sourceFileName))
				log = String.Format("{0}<sourceFileName> doesn't exist.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(destFileName))
				log = String.Format("{0}<destFileName> is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in FileUtil.Move(string sourceFileName, string destFileName, OverwriteOption overwriteOption).{1}", log, Environment.NewLine));

			if (destFileName.ToUpper().Equals(sourceFileName.ToUpper()))
				return;
			#endregion Input Check

			if (File.Exists(destFileName)) {
				string ext, newExt;
				switch (overwriteOption) {
					#region case OverwriteOption.Overwrite:
					case OverwriteOption.Overwrite:
						FastMove(sourceFileName, destFileName);
						return;
					#endregion
					#region case OverwriteOption.OverwriteIfSourceNewer:
					case OverwriteOption.OverwriteIfSourceNewer:
						if (new FileInfo(sourceFileName).LastWriteTime > new FileInfo(destFileName).LastWriteTime)
							FastMove(sourceFileName, destFileName);
						return;
					#endregion
					#region case OverwriteOption.OverwriteIfDifferentSize:
					case OverwriteOption.OverwriteIfDifferentSize:
						if (new FileInfo(sourceFileName).Length != new FileInfo(destFileName).Length)
							FastMove(sourceFileName, destFileName);
						return;
					#endregion
					#region case OverwriteOption.OverwriteIfDifferentSizeOrSourceNewer:
					case OverwriteOption.OverwriteIfDifferentSizeOrSourceNewer:
						var sourceFile = new FileInfo(sourceFileName);
						var destFile = new FileInfo(destFileName);
						if (sourceFile.Length != destFile.Length || sourceFile.LastWriteTime > destFile.LastWriteTime)
							FastMove(sourceFileName, destFileName);
						sourceFile = null;
						destFile = null;
						return;
					#endregion
					#region case OverwriteOption.Rename:
					case OverwriteOption.Rename:
						ext = Path.GetExtension(destFileName);
						newExt = String.Format(" Copy{0}", ext);
						Move(sourceFileName, destFileName.Replace(ext, newExt), overwriteOption);
						return;
					#endregion
					#region case OverwriteOption.RenameIfDifferentSize:
					case OverwriteOption.RenameIfDifferentSize:
						if (new FileInfo(sourceFileName).Length != new FileInfo(destFileName).Length) {
							ext = Path.GetExtension(destFileName);
							newExt = String.Format(" Copy{0}", ext);
							Move(sourceFileName, destFileName.Replace(ext, newExt), overwriteOption);
						}
						return;
					#endregion
					#region case OverwriteOption.Skip:
					case OverwriteOption.Skip:
						return;
					#endregion
					default:
						throw new Exception(String.Format("Unsupported OverwriteOption.{0}", Environment.NewLine));
				}
			}

			else
				FastMove(sourceFileName, destFileName);
		}

		/// <summary>
		/// Copies an existing file to a new file.
		/// </summary>
		/// <param name="sourceFileName">The file to copy.</param>
		/// <param name="destFileName">The name of the destination file. This cannot be a directory.</param>
		/// <param name="overwriteOption">One of the enumeration values that specifies whether to overwrite or not if the destination file already exists.</param>
		public static bool TryCopy(string sourceFileName, string destFileName, OverwriteOption overwriteOption = OverwriteOption.Skip)
		{
			try {
				Copy(sourceFileName, destFileName, overwriteOption);
				return true;
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in FileUtil.TryCopy(string sourceFileName='{3}', string destFileName='{4}', OverwriteOption overwriteOption='{5}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, sourceFileName, destFileName, overwriteOption.ToString());
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of FileUtil.TryCopy(string sourceFileName='{3}', string destFileName='{4}', OverwriteOption overwriteOption='{5}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, sourceFileName, destFileName, overwriteOption.ToString());

				Console.Write("\n{0}", log);
				#endregion Log

				return false;
			}
		}

		/// <summary>
		/// Deletes the specified file.
		/// </summary>
		/// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
		public static bool TryDelete(string path)
		{
			try {
				new FileInfo(path).Delete();
				return true;
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in FileUtil.TryDelete(string path='{3}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, path);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of FileUtil.TryDelete(string path='{3}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, path);

				Console.Write("\n{0}", log);
				#endregion Log

				return false;
			}
		}
		#endregion Public Methods
	}
}