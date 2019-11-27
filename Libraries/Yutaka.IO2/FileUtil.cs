using System;
using System.IO;
using System.Linq;

namespace Yutaka.IO2
{
	public static class FileUtil
	{
		#region Fields
		public static readonly DateTime UNIX_TIME = new DateTime(1970, 1, 1);
		public static readonly int FIVE_HUNDRED_TWELVE_KB = (int) Math.Pow(2, 19);
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

			try {
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

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in FileUtil.FastCopy(string sourceFileName='{3}', string destFileName='{4}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, sourceFileName, destFileName);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of FileUtil.FastCopy(string sourceFileName='{3}', string destFileName='{4}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, sourceFileName, destFileName);

				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// Fast file move based on whether the source and destination are the same drive/volume or not. If &lt;destFileName&gt; exists, it will be overwritten.
		/// </summary>
		/// <param name="sourceFileName">The file to move.</param>
		/// <param name="destFileName">The name of the destination file. This cannot be a directory.</param>
		private static void FastMove(string sourceFileName, string destFileName)
		{
			try {
				if (Path.GetPathRoot(sourceFileName).ToUpper().Equals(Path.GetPathRoot(destFileName).ToUpper()))
					new FileInfo(sourceFileName).MoveTo(destFileName);
				else
					FastCopy(sourceFileName, destFileName);
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in FileUtil.FastMove(string sourceFileName='{3}', string destFileName='{4}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, sourceFileName, destFileName);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of FileUtil.FastMove(string sourceFileName='{3}', string destFileName='{4}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, sourceFileName, destFileName);

				throw new Exception(log);
				#endregion Log
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

			try {
				var destFileExists = File.Exists(destFileName);

				switch (overwriteOption) {
					#region case OverwriteOption.Overwrite:
					case OverwriteOption.Overwrite:
						FastCopy(sourceFileName, destFileName);
						break;
					#endregion
					#region case OverwriteOption.Skip:
					case OverwriteOption.Skip:
						if (!destFileExists)
							FastCopy(sourceFileName, destFileName);
						break;
					#endregion
					#region case OverwriteOption.KeepBoth:
					case OverwriteOption.KeepBoth:
						if (destFileExists)
							FastCopy(sourceFileName, AutoRename(destFileName));
						else
							FastCopy(sourceFileName, destFileName);
						break;
					#endregion
					#region case OverwriteOption.Smart:
					case OverwriteOption.Smart:
						if (destFileExists) {
							var fi1 = new FileInfo(sourceFileName);
							var fi2 = new FileInfo(destFileName);

							if (fi1.Length != fi2.Length)
								FastCopy(sourceFileName, AutoRename(destFileName));

							fi1 = null;
							fi2 = null;
						}

						else
							FastCopy(sourceFileName, destFileName);
						break;
					#endregion
					default:
						break;
				}
			}

			catch (Exception ex) {
				#region Log
				log = "";

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in FileUtil.Copy(string sourceFileName='{3}', string destFileName='{4}', OverwriteOption overwriteOption='{5}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, sourceFileName, destFileName, overwriteOption.ToString());
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of FileUtil.Copy(string sourceFileName='{3}', string destFileName='{4}', OverwriteOption overwriteOption='{5}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, sourceFileName, destFileName, overwriteOption.ToString());

				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// Permanently deletes the specified file.
		/// </summary>
		/// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
		public static void Delete(string path)
		{
			#region Input Check
			if (String.IsNullOrWhiteSpace(path))
				throw new Exception(String.Format("<path> is required.{0}Exception thrown in FileUtil.Delete(string path).{0}{0}", Environment.NewLine));
			if (!File.Exists(path))
				throw new Exception(String.Format("<path> doesn't exist.{0}Exception thrown in FileUtil.Delete(string path).{0}{0}", Environment.NewLine));
			#endregion Input Check

			try {
				new FileInfo(path).Delete();
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in FileUtil.Delete(string path='{3}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, path);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of FileUtil.Delete(string path='{3}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, path);

				throw new Exception(log);
				#endregion Log
			}
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
		/// Permanently deletes the specified file.
		/// </summary>
		/// <param name="path"></param>
		public static bool TryDelete(string path)
		{
			try {
				Delete(path);
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

		/// <summary>
		/// Moves a specified file to a new location, providing the option to specify a new file name.
		/// </summary>
		/// <param name="sourceFileName">The name of the file to move. Can include a relative or absolute path.</param>
		/// <param name="destFileName">The path to move the file to, which can specify a different file name.</param>
		/// <param name="overwriteOption">One of the enumeration values that specifies whether to overwrite or not if the destination file already exists.</param>
		public static void Move(string sourceFileName, string destFileName, OverwriteOption overwriteOption = OverwriteOption.Skip)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(sourceFileName))
				log = String.Format("{0}<sourceFileName> is required.{1}", log, Environment.NewLine);
			else if (!File.Exists(sourceFileName))
				log = String.Format("{0}File '{2}' doesn't exist.{1}", log, Environment.NewLine, sourceFileName);
			if (String.IsNullOrWhiteSpace(destFileName))
				log = String.Format("{0}<destFileName> is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in FileUtil.Move(string sourceFileName, string destFileName, OverwriteOption overwriteOption).{1}", log, Environment.NewLine));

			if (destFileName.ToUpper().Equals(sourceFileName.ToUpper()))
				return;
			#endregion Input Check

			try {
				if (Path.GetPathRoot(sourceFileName).ToUpper().Equals(Path.GetPathRoot(destFileName).ToUpper()))
					new FileInfo(sourceFileName).MoveTo(destFileName);
				else {
					if (TryCopy(sourceFileName, destFileName, overwriteOption))
						TryDelete(sourceFileName);
				}
			}

			catch (Exception ex) {
				#region Log
				log = "";

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in FileUtil.Move(string sourceFileName='{3}', string destFileName='{4}', OverwriteOption overwriteOption='{5}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, sourceFileName, destFileName, overwriteOption.ToString());
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of FileUtil.Move(string sourceFileName='{3}', string destFileName='{4}', OverwriteOption overwriteOption='{5}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, sourceFileName, destFileName, overwriteOption.ToString());

				throw new Exception(log);
				#endregion Log
			}
		}
		#endregion Public Methods
	}
}