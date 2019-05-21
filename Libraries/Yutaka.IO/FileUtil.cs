using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Yutaka.IO
{
	public class FileUtil
	{
		#region Fields
		// Constants //
		const int DATE_TAKEN = 36867; // PropertyTagExifDTOrig //
		const int ONE_GIGABYTE = 1073741824; // Math.Pow(2, 30) //
		const int ONE_MEGABYTE = 1048576; // Math.Pow(2, 20) //
		const int FIVE_TWELVE_KB = 524288; // Math.Pow(2, 19) //
		const int ONE_KILOBYTE = 1024; // Math.Pow(2, 10) //
		const int BUFFER = FIVE_TWELVE_KB;

		// Config/Settings //
		private bool consoleOut = true;
		private string mode = "copy";

		// PIVs //
		public List<string> IgnoreListFolders;
		public List<string> IgnoreListFileMasks;
		private DateTime dateThreshold = new DateTime(1982, 1, 1);
		private HashSet<string> audioExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".aiff", ".m4a", ".mp3", ".au", ".ogg", ".wav", ".wma" };
		private HashSet<string> applicationExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".asdf", ".asdf", ".asdf" };
		private HashSet<string> archiveExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".rar", ".zip", ".7z", ".ace", ".arj", ".bz2", ".cab", ".gz", ".iso", ".jar", ".lz", ".lzh", ".tar", ".uue", ".xz", ".z", ".zipx", ".001" };
		private HashSet<string> documentExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".ppt", ".pptx" };
		private HashSet<string> imageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".ai", ".bmp", ".eps", ".gif", ".ico", ".jpg", ".jpeg", ".png", ".psd", ".tiff" };
		private HashSet<string> videoExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".3gp", ".avi", ".flv", ".m4v", ".mkv", ".mpg", ".mpeg", ".mp4", ".ogv", ".mov", ".webm", ".wmv" };
		#endregion Fields

		public FileUtil() {
			IgnoreListFolders = new List<string>();
			IgnoreListFileMasks = new List<string>();
		}

		#region Private Helpers
		private bool CopyFile(FileInfo source, string dest)
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

		private void DisplayException(Exception ex)
		{
			Console.Write("\n{0}", ex.Message);
			Console.Write("\n");
			Console.Write("\n{0}", ex.ToString());
		}

		private bool MoveFile(FileInfo source, string dest)
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
		#endregion Private Helpers

		#region Move
		/// <summary> Fast file move with big buffers
		/// </summary>
		/// <param name="source">Source file path</param> 
		/// <param name="destination">Destination file path</param> 
		public void FastMove(string source, string destination, bool deleteSource = true)
		{
			var array_length = BUFFER;
			var dataArray = new byte[array_length];

			using (var fsread = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.None, array_length)) {
				using (var bwread = new BinaryReader(fsread)) {
					using (var fswrite = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None, array_length)) {
						using (var bwwrite = new BinaryWriter(fswrite)) {
							for (; ; ) {
								var read = bwread.Read(dataArray, 0, array_length);
								if (0 == read)
									break;
								bwwrite.Write(dataArray, 0, read);
							}
						}
					}
				}
			}

			if (deleteSource)
				File.Delete(source);
		}

		/// <summary> Time the Move
		/// </summary> 
		/// <param name="source">Source file path</param> 
		/// <param name="destination">Destination file path</param> 
		public void MoveTime(string source, string destination, bool delete = true)
		{
			var start_time = DateTime.Now;
			FastMove(source, destination, delete);
			var milliseconds = 1 + (int) ((DateTime.Now - start_time).TotalMilliseconds);
			var size = new FileInfo(destination).Length;
			// size time in milliseconds per sec
			var tsize = size * 1000 / milliseconds;
			if (tsize > ONE_GIGABYTE) {
				tsize = tsize / ONE_GIGABYTE;
				Console.Write("\n{0} transferred at {1}gb/sec", source, tsize);
			}
			else if (tsize > ONE_MEGABYTE) {
				tsize = tsize / ONE_MEGABYTE;
				Console.Write("\n{0} transferred at {1}mb/sec", source, tsize);
			}
			else if (tsize > ONE_KILOBYTE) {
				tsize = tsize / ONE_KILOBYTE;
				Console.Write("\n{0} transferred at {1}kb/sec", source, tsize);
			}
			else
				Console.Write("\n{0} transferred at {1}b/sec", source, tsize);
		}

		// DRY is ignored in favor of performance. Any changes in this method should also be made to Move(string sourceFilePath, string destFilePath) //
		public void Move(FileInfo source, string destFilePath)
		{
			if (source == null)
				throw new Exception(String.Format("Exception thrown in FileUtil.Move(FileInfo source, string destFilePath){0}<source> is NULL", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(destFilePath))
				throw new Exception(String.Format("Exception thrown in FileUtil.Move(FileInfo source, string destFilePath){0}<destFilePath> is {1}", Environment.NewLine, destFilePath == null ? "NULL" : "Empty"));

			try {
				Directory.CreateDirectory(Path.GetDirectoryName(destFilePath));
				source.MoveTo(destFilePath);
			}

			catch (Exception ex) {
				throw new Exception(String.Format("Exception thrown in FileUtil.Move(FileInfo source, string destFilePath='{3}'){2}{0}{2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine, destFilePath));
			}
		}

		// DRY is ignored in favor of performance. Any changes in this method should also be made to Move(FileInfo source, string destFilePath) //
		public void Move(string sourceFilePath, string destFilePath)
		{
			if (String.IsNullOrWhiteSpace(sourceFilePath))
				throw new Exception(String.Format("Exception thrown in FileUtil.Move(string sourceFilePath, string destFilePath){0}<sourceFilePath> is {1}", Environment.NewLine, sourceFilePath == null ? "NULL" : "Empty"));
			if (String.IsNullOrWhiteSpace(destFilePath))
				throw new Exception(String.Format("Exception thrown in FileUtil.Move(string sourceFilePath, string destFilePath){0}<destFilePath> is {1}", Environment.NewLine, destFilePath == null ? "NULL" : "Empty"));

			try {
				Directory.CreateDirectory(Path.GetDirectoryName(destFilePath));
				new FileInfo(sourceFilePath).MoveTo(destFilePath);
			}

			catch (Exception ex) {
				throw new Exception(String.Format("Exception thrown in FileUtil.Move(string sourceFilePath='{3}', string destFilePath='{4}'){2}{0}{2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine, sourceFilePath, destFilePath));
			}
		}
		#endregion Move

		#region Public Methods
		public void CopyFile(FileInfo source, string dest, bool overwrite = false, TimestampOption tOption = TimestampOption.WindowsDefault)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			if (dest == null)
				throw new ArgumentNullException("dest");

			if (tOption == TimestampOption.WindowsDefault)
				source.CopyTo(dest, overwrite);

			else {
				var now = DateTime.Now;
				var creationTime = now;
				var lastAccessTime = now;
				var lastWriteTime = now;

				switch (tOption) {
					case TimestampOption.PreserveOriginal:
						creationTime = source.CreationTime;
						lastAccessTime = source.LastAccessTime;
						lastWriteTime = source.LastWriteTime;
						break;
					case TimestampOption.SetAllToMinDate:
						var minDate = GetMinTime(source);
						creationTime = minDate;
						lastAccessTime = minDate;
						lastWriteTime = minDate;
						break;
					case TimestampOption.SetAllToDateTaken:
						var dateTaken = GetMinTime(source);
						creationTime = dateTaken;
						lastAccessTime = dateTaken;
						lastWriteTime = dateTaken;
						break;
					default:
						throw new Exception(String.Format("{0} isn't a valid TimestampOption, or, it hasn't been implemented yet", tOption));
				}

				var newFile = source.CopyTo(dest, overwrite);

				if (newFile != null) {
					newFile.CreationTime = creationTime;
					newFile.LastWriteTime = lastAccessTime;
					newFile.LastAccessTime = lastWriteTime;
				}
			}
		}

		public void CopyFile(string source, string dest, bool overwrite = false, TimestampOption tOption = TimestampOption.WindowsDefault)
		{
			CopyFile(new FileInfo(source), dest, overwrite, tOption);
		}

		public void CopyFile(string source, string dest, bool delete = false)
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

		public void DeleteFiles(string folder, string extension)
		{
			if (String.IsNullOrEmpty(folder))
				throw new Exception("<folder> is required.");
			if (String.IsNullOrEmpty(extension))
				throw new Exception("<extension> is required.");

			try {
				extension = extension.ToUpper();

				if (!extension.StartsWith("."))
					extension = String.Format(".{0}", extension);

				var di = new DirectoryInfo(folder);
				var files = di.GetFiles();

				files.AsParallel().Where(f => f.Extension.ToUpper().Equals(extension)).ForAll((f) => f.Delete());
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in FileUtil.DeleteFiles(string folder='{3}', string extension='{4}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, folder, extension));

				throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of FileUtil.DeleteFiles(string folder='{3}', string extension='{4}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, folder, extension));
			}
		}

		/// <summary>
		/// Returns an IEnumerable of audio FileInfos that matches a specified search patthern and search subdirectory option.
		/// </summary>
		/// <param name="path">A string specifying the path on which to create the DirectoryInfo.</param>
		/// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions. The default pattern is "*", which returns all files.</param>
		/// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is TopDirectoryOnly.</param>
		/// <returns></returns>
		public IEnumerable<FileInfo> EnumerateAudioFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return new DirectoryInfo(path).EnumerateFiles(searchPattern, searchOption).Where(x => audioExtensions.Contains(x.Extension, StringComparer.OrdinalIgnoreCase));
		}

		/// <summary>
		/// Returns an IEnumerable of image FileInfos that matches a specified search patthern and search subdirectory option.
		/// </summary>
		/// <param name="path">A string specifying the path on which to create the DirectoryInfo.</param>
		/// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions. The default pattern is "*", which returns all files.</param>
		/// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is TopDirectoryOnly.</param>
		/// <returns></returns>
		public IEnumerable<FileInfo> EnumerateImageFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return new DirectoryInfo(path).EnumerateFiles(searchPattern, searchOption).Where(x => imageExtensions.Contains(x.Extension, StringComparer.OrdinalIgnoreCase));
		}

		/// <summary>
		/// Returns an IEnumerable of video FileInfos that matches a specified search patthern and search subdirectory option.
		/// </summary>
		/// <param name="path">A string specifying the path on which to create the DirectoryInfo.</param>
		/// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions. The default pattern is "*", which returns all files.</param>
		/// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is TopDirectoryOnly.</param>
		/// <returns></returns>
		public IEnumerable<FileInfo> EnumerateVideoFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return new DirectoryInfo(path).EnumerateFiles(searchPattern, searchOption).Where(x => videoExtensions.Contains(x.Extension, StringComparer.OrdinalIgnoreCase));
		}

		public void FixCreationTime(string root, string searchPattern = "*", int initialCapacity = 1024)
		{
			if (String.IsNullOrWhiteSpace(root))
				throw new Exception(String.Format("Exception thrown in FileUtil.FixCreationTime(string root, string searchPattern='*', int initialCapacity=1024){0}<root> is {1}", Environment.NewLine, root == null ? "NULL" : "Empty"));
			if (!Directory.Exists(root))
				throw new Exception(String.Format("Exception thrown in FileUtil.FixCreationTime(string root, string searchPattern='*', int initialCapacity=1024){0}Directory doesn't exist: {1}", Environment.NewLine, root));

			// Data structure to hold names of subfolders to be examined for files.
			var dirs = new Stack<string>(initialCapacity);
			dirs.Push(root);
			string currentDir;
			DirectoryInfo di;
			FileInfo fi;

			while (dirs.Count > 0) {
				currentDir = dirs.Pop();

				// Perform required action on each file  //
				try {
					foreach (var file in Directory.EnumerateFiles(currentDir, searchPattern)) {
						fi = new FileInfo(file);

						if (fi.CreationTime > fi.LastWriteTime)
							fi.CreationTime = fi.LastWriteTime;
					}
				}

				catch (UnauthorizedAccessException) {
					continue;
				}

				catch (DirectoryNotFoundException) {
					continue;
				}

				catch (PathTooLongException) {
					continue;
				}

				// Push subdirectories onto stack for traversal //
				try {
					foreach (var suDir in Directory.EnumerateDirectories(currentDir)) {
						dirs.Push(suDir);
						di = new DirectoryInfo(suDir);

						if (di.CreationTime > di.LastWriteTime)
							di.CreationTime = di.LastWriteTime;
					}
				}
				catch (UnauthorizedAccessException) {
					continue;
				}
				catch (DirectoryNotFoundException) {
					continue;
				}
				catch (PathTooLongException) {
					continue;
				}
			}
		}

		public string[] GetAllLines(string filePath, int maxLines = -1)
		{
			if (String.IsNullOrEmpty(filePath))
				throw new ArgumentNullException("filePath", "<filePath> is required.");

			try {
				if (maxLines < 1)
					maxLines = File.ReadLines(filePath).Count();

				var allLines = new string[maxLines];
				using (var sr = File.OpenText(filePath)) {
					var x = 0;
					while (!sr.EndOfStream) {
						allLines[x] = sr.ReadLine();
						x += 1;
					}
				}

				return allLines;
			}

			catch (Exception ex) {
				throw ex;
			}
		}

		public List<string> GetAllAudioFiles(string rootFolder, string[] ignoreFolders=null, int initialStackCapacity=200)
		{
			if (String.IsNullOrWhiteSpace(rootFolder))
				throw new Exception(String.Format("<rootFolder> is required.{0}Exception thrown in FileUtil.GetAllAudioFiles(string rootFolder, int initialStackCapacity)", Environment.NewLine));
			if (!Directory.Exists(rootFolder))
				throw new Exception(String.Format("Directory '{1}' doesn't exist.{0}Exception thrown in FileUtil.GetAllAudioFiles(string rootFolder, int initialStackCapacity)", Environment.NewLine, rootFolder));

			string[] audioExtensions = { "*.aiff", "*.m4a", "*.mp3", "*.au", "*.ogg", "*.wav", "*.wma" };
			var list = new List<string>();
			var dirs = new Stack<string>(initialStackCapacity);
			dirs.Push(rootFolder);

			while (dirs.Count > 0) {
				string[] subDirs;
				var currentDir = dirs.Pop();

				while (IsInIgnoreList(currentDir, ignoreFolders)) {
					currentDir = dirs.Pop();
				}

				try {
					for (int i = 0; i < audioExtensions.Length; i++) {
						list.AddRange(Directory.EnumerateFiles(currentDir, audioExtensions[i]));
					}
				}

				catch (UnauthorizedAccessException e) {
					Console.Write("\n{0}\ncurrentDir: {1}", e.Message, currentDir);
					continue;
				}

				catch (DirectoryNotFoundException e) {
					Console.Write("\n{0}\ncurrentDir: {1}", e.Message, currentDir);
					continue;
				}

				try {
					subDirs = Directory.GetDirectories(currentDir);
				}

				catch (UnauthorizedAccessException e) {
					Console.Write("\n{0}\ncurrentDir: {1}", e.Message, currentDir);
					continue;
				}
				catch (DirectoryNotFoundException e) {
					Console.Write("\n{0}\ncurrentDir: {1}", e.Message, currentDir);
					continue;
				}

				for (int i = 0; i < subDirs.Length; i++) {
					if (IsInIgnoreList(subDirs[i], ignoreFolders))
						continue;
					else
						dirs.Push(subDirs[i]);
				}

				if (dirs.Count > initialStackCapacity)
					Console.Write("\n******* dirs.Count: {0} *******", dirs.Count);
			}

			return list;
		}

		// Retrieves the datetime WITHOUT loading the whole image //
		public DateTime GetDateTakenFromImage(string path)
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

		public long GetDirectorySize(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories)
		{
			try {
				var files = Directory.EnumerateFiles(path, searchPattern, searchOption);
				long bytes = 0;

				foreach (var file in files)
					bytes += new FileInfo(file).Length;

				return bytes;
			}

			catch (Exception ex) {
				throw new Exception(String.Format("Exception thrown in FileUtil.GetDirectorySize(string path='{3}', string searchPattern='{4}', SearchOption searchOption='{5}'){2}{0}{2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine, path ?? "null", searchPattern ?? "null", searchOption));
			}
		}

		public List<string> GetFilesRecursive(string targetDirectory, string searchPattern = "*", int maxDepth = 7)
		{
			if (String.IsNullOrEmpty(targetDirectory))
				throw new ArgumentNullException("targetDirectory", "<targetDirectory> is required.");

			//Console.Write("\n");
			//Console.Write("\n==============================");
			//Console.Write("\ntargetDirectory: {0}", targetDirectory);
			//Console.Write("\n==============================");
			var files = new List<string>();
			if (maxDepth < 1)
				return files;

			try {
				files = Directory.EnumerateFiles(targetDirectory, searchPattern).ToList();
				for (int i = 0; i < files.Count; i++) {
					//Console.Write("\n{0}", files[i]);
				}
			}

			catch (Exception) {
				return files;
			}

			// Recurse into subdirectories of this directory //
			var subdirectories = Directory.GetDirectories(targetDirectory);
			for (int i = 0; i < subdirectories.Length; i++) {
				//Console.Write("\n");
				//Console.Write("\nsubDirectory: {0}", subdirectories[i]);
				var fInfo = new FileInfo(subdirectories[i]);
				var attributes = fInfo.Attributes;
				var isReadOnly = fInfo.IsReadOnly;
				if (isReadOnly)
					fInfo.Attributes = FileAttributes.Normal;

				files.AddRange(GetFilesRecursive(subdirectories[i], searchPattern, maxDepth - 1));

				if (isReadOnly)
					fInfo.Attributes = attributes;
			}

			return files;
		}

		/// <summary>
		/// Gets the MinDate based on CreationTime, LastAccessTime, and LastWriteTime. Sometimes a date can be "blank" if the source is from a removable media (flash drives,
		/// SD cards). In those cases, the date will be less than the dateThreshold of Jan 1, 1982, and will NOT return that date.
		/// </summary>
		/// <param name="fi">The FileInfo object</param>
		/// <returns></returns>
		public DateTime GetMinTime(FileInfo fi)
		{
			if (fi == null)
				throw new ArgumentNullException();
			if ((fi.CreationTime < dateThreshold) && (fi.LastAccessTime < dateThreshold) && (fi.LastWriteTime < dateThreshold))
				throw new Exception("Something's wrong with this file. All 3 dates are before Jan 1, 1982");

			var minTime = fi.CreationTime;
			var lastAccessTime = fi.LastAccessTime;
			var lastWriteTime = fi.LastWriteTime;

			if (minTime < dateThreshold || lastAccessTime < minTime)
				minTime = lastAccessTime;
			if (minTime < dateThreshold || lastWriteTime < minTime)
				minTime = lastWriteTime;

			if (minTime > dateThreshold)
				return minTime;

			throw new Exception("Couldn't determine MinTime");
		}

		/// <summary>
		/// Gets the MinDate based on CreationTime, LastAccessTime, and LastWriteTime. Sometimes a date can be "blank" if the source is from a removable media (flash drives,
		/// SD cards). In those cases, the date will be less than the dateThreshold of Jan 1, 1982, and will NOT return that date. 
		/// </summary>
		/// <remarks>This method calls GetMinTime(FileInfo fi), so if performance is crucial, you may want to refactor your code.</remarks>
		/// <param name="fi">path to the File</param>
		/// <returns></returns>
		public DateTime GetMinTime(string path)
		{
			return GetMinTime(new FileInfo(path));
		}

		public string GetNewestFile(string dirPath, string extension=null)
		{
			if (String.IsNullOrWhiteSpace(dirPath))
				throw new Exception(String.Format("<dirPath> is required.{0}Exception thrown in FileUtil.GetNewestFile(string dirPath, string extension){0}{0}", Environment.NewLine));

			try {
				if (String.IsNullOrWhiteSpace(extension))
					return new DirectoryInfo(dirPath).GetFiles().OrderByDescending(x => x.LastWriteTime).FirstOrDefault().FullName;

				return new DirectoryInfo(dirPath).GetFiles().Where(x => x.Extension.ToUpper().Contains(extension.ToUpper()))
																							 .OrderByDescending(y => y.LastWriteTime).FirstOrDefault().FullName;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in FileUtil.GetNewestFile(string dirPath='{3}', string extension='{4}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, dirPath, extension));

				throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of FileUtil.GetNewestFile(string dirPath='{3}', string extension='{4}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, dirPath, extension));
			}
		}

		public bool IsSameDate(FileInfo fi1, FileInfo fi2)
		{
			try {
				if (fi1.LastWriteTimeUtc == fi2.LastWriteTimeUtc)
					return true;

				return false;
			}

			catch (Exception ex) {
				throw ex;
			}
		}

		public bool IsSameDate(string path1, string path2)
		{
			return IsSameDate(new FileInfo(path1), new FileInfo(path2));
		}

		public bool IsSameFile(FileInfo fi1, FileInfo fi2)
		{
			try {
				if (IsSameDate(fi1, fi2) && IsSameSize(fi1, fi2))
					return true;

				return false;
			}

			catch (Exception ex) {
				throw ex;
			}
		}

		public bool IsSameFile(string path1, string path2)
		{
			return IsSameFile(new FileInfo(path1), new FileInfo(path2));
		}

		public bool IsSameSize(FileInfo fi1, FileInfo fi2)
		{
			try {
				if (fi1.Length == fi2.Length)
					return true;

				return false;
			}

			catch (Exception ex) {
				throw ex;
			}
		}

		public bool IsSameSize(string path1, string path2)
		{
			return IsSameSize(new FileInfo(path1), new FileInfo(path2));
		}

		public bool IsStringInArray(string str, string[] array)
		{
			if (str == null)
				throw new Exception(String.Format("<str> is required.{0}Exception thrown in FileUtil.IsStringInArray(string str, string[] array).{0}", Environment.NewLine));

			if (array == null || array.Length < 1)
				throw new Exception(String.Format("<array> is required.{0}Exception thrown in FileUtil.IsStringInArray(string str, string[] array).{0}", Environment.NewLine));

			if (array.Contains(str, StringComparer.OrdinalIgnoreCase))
				return true;

			return false;
		}

		public bool IsStringInList(string str, List<String> list)
		{
			if (str == null)
				throw new Exception(String.Format("<str> is required.{0}Exception thrown in FileUtil.IsStringInList(string str, List<String> list).{0}", Environment.NewLine));

			if (list == null || list.Count < 1)
				throw new Exception(String.Format("<list> is required.{0}Exception thrown in FileUtil.IsStringInList(string str, List<String> list).{0}", Environment.NewLine));

			if (list.Contains(str, StringComparer.OrdinalIgnoreCase))
				return true;

			return false;
		}

		public void Redate(string filename, DateTime dt)
		{
			if (String.IsNullOrWhiteSpace(filename))
				return;
			if (dt < dateThreshold)
				return;

			var fi = new FileInfo(filename);
			fi.CreationTime = dt;
			fi.LastAccessTime = dt;
			fi.LastWriteTime = dt;
			fi = null;
		}

		public void Write(object value, string path, bool append = true, Encoding encoding = null, int bufferSize = 65536)
		{
			#region Parameter Check
			if (value == null || String.IsNullOrWhiteSpace(value.ToString()))
				throw new Exception(String.Format("Exception thrown in FileUtil.Write(object value='{1}', string path='{2}', bool append='{3}', Encoding encoding='{4}', int bufferSize='{5}'){0}<value> is NULL or whitespace", Environment.NewLine, value, path, append, encoding, bufferSize));

			if (String.IsNullOrWhiteSpace(path))
				throw new Exception(String.Format("Exception thrown in FileUtil.Write(object value='{1}', string path='{2}', bool append='{3}', Encoding encoding='{4}', int bufferSize='{5}'){0}<path> is NULL or whitespace", Environment.NewLine, value, path, append, encoding, bufferSize));
			if (String.IsNullOrWhiteSpace(Path.GetDirectoryName(path)) || !Directory.Exists(Path.GetDirectoryName(path)))
				Directory.CreateDirectory(Path.GetDirectoryName(path));

			if (encoding == null)
				encoding = Encoding.Default;

			if (bufferSize < 4096)
				bufferSize = 4096;
			#endregion Parameter Check

			try {
				using (var sw = new StreamWriter(path, append, encoding, bufferSize))
					sw.Write(value);
			}

			catch (Exception ex) {
				throw new Exception(String.Format("Exception thrown in FileUtil.Write(object value='{3}', string path='{4}', bool append='{5}', Encoding encoding='{6}', int bufferSize='{7}'){2}{0}{2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine, value, path, append, encoding, bufferSize));
			}
		}
		#endregion Public Methods

		#region Enum
		public enum OverwriteOption { No, Yes, IfSourceIsNewer, IfSourceIsOlder, IsDifferentDate, IfSourceIsLarger, IfSourceIsSmaller, IfDifferentSize, IfDifferentDateOrDifferentSize, RenameAppendCurTime };
		public enum TimestampOption { WindowsDefault, PreserveOriginal, SetAllToMinDate, SetAllToDateTaken };
		#endregion Enum

		#region Deprecated
		[Obsolete("Deprecated on May 13, 2019. Use IsStringInArray(string str, string[] array) instead.")]
		public bool IsInIgnoreList(string str, string[] ignoreList)
		{
			if (String.IsNullOrWhiteSpace(str)) {
				if (ignoreList == null || ignoreList.Length < 1)
					throw new Exception(String.Format("<str> and <ignoreList> can't BOTH be empty.{0}Exception thrown in FileUtil.IsInIgnoreList(string str, string[] ignoreList)", Environment.NewLine));

				return false;
			}

			else {
				str = str.ToUpper();

				for (int i = 0; i < ignoreList.Length; i++)
					if (str.Contains(ignoreList[i].ToUpper()))
						return true;
			}

			return false;
		}

		[Obsolete("Deprecated on Nov 19, 2018. No alternate method exists.", true)]
		public List<string> EnumerateFilesStack(string targetDirectory, string searchPattern = "*", int initialCapacity = 100)
		{
			if (String.IsNullOrEmpty(targetDirectory))
				throw new ArgumentNullException("targetDirectory", "<targetDirectory> is required.");
			if (!Directory.Exists(targetDirectory))
				throw new ArgumentException();

			var list = new List<string>();
			var dirs = new Stack<string>(initialCapacity);

			dirs.Push(targetDirectory);

			while (dirs.Count > 0) {
				var currentDir = dirs.Pop();
				//Console.Write("\n");
				//Console.Write("\n==============================");
				//Console.Write("\ncurrentDir: {0}", currentDir);
				//Console.Write("\n==============================");
				string[] subDirs;
				try {
					subDirs = Directory.GetDirectories(currentDir);
				}
				// An UnauthorizedAccessException exception will be thrown if we do not have discovery permission on a folder or file //
				catch (UnauthorizedAccessException e) {
					Console.Write("\n{0}: {1}", e.Message, currentDir);
					continue;
				}
				catch (DirectoryNotFoundException e) {
					Console.Write("\n{0}: {1}", e.Message, currentDir);
					continue;
				}

				var files = new List<string>();
				try {
					files = Directory.EnumerateFiles(currentDir, searchPattern).ToList();
					list.AddRange(files);
				}

				catch (UnauthorizedAccessException e) {
					Console.Write("\n{0}: {1}", e.Message, currentDir);
					continue;
				}

				catch (DirectoryNotFoundException e) {
					Console.Write("\n{0}: {1}", e.Message, currentDir);
					continue;
				}

				// Perform the required action on each file here. Modify this block to perform your required task.
				for (int i = 0; i < files.Count; i++) {
					try {
						// Perform whatever action is required in your scenario.
						var fi = new FileInfo(files[i]);
						//Console.Write("\n{0}: {1}, {2}", fi.Name, fi.Length, fi.CreationTime);
					}
					catch (FileNotFoundException e) {
						Console.Write("\n{0}: {1}", e.Message, currentDir);
						continue;
					}
				}

				// Push the subdirectories onto the stack for traversal.
				for (int i = 0; i < subDirs.Length; i++)
					dirs.Push(subDirs[i]);
			}

			return list;
		}
		#endregion Deprecated
	}
}