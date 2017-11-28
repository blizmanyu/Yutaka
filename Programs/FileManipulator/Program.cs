using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FileManipulator
{
	class Program
	{
		#region Fields
		protected enum Mode { Copy, Move };

		private static HashSet<string> imageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
			".jpg", ".jpeg", ".nef", ".png", ".svg"
		};

		// Constants //
		const int ONE_MEGABYTE = 1048576;
		const int FIVE_TWELVE_KILOBYTES = 524288;
		const int BUFFER_SIZE = ONE_MEGABYTE;
		// Flags //
		static readonly bool consoleOut   = true;
		static readonly bool consoleOutSummary   = true;
		static readonly bool DEBUG        = false;
		static readonly bool deleteAfter  = false;
		protected Mode mode = Mode.Copy;
		static readonly string CopyOrMove = "Copy";
		// Folders //
		const string srcFolder = @"D:\DCIM\Camera\";
		const string destDrive = @"C:\";
		const string backupsFolder = @"S:\Backups\";
		const string officeFolder = @"S:\Office\";
		const string privateFolder = @"S:\Private\";
		const string downloadsFolder = @"C:\Downloads\";
		const string picturesFolder = destDrive + @"Pictures\";
		const string ikeaFolder = picturesFolder + @"Ikea\";
		const string magazinesFolder = picturesFolder + @"Magazines\";
		const string mensHealthFolder = picturesFolder + @"Mens Health\";
		const string screenshotFolder = picturesFolder + @"Screenshots\";
		const string tattoosFolder = picturesFolder + @"Tattoos\";
		const string londonFolder = picturesFolder + @"zLond\";
		const string meFolder = picturesFolder + @"zMe\";
		const string patriciaFolder = picturesFolder + @"zPatr\";
		const string testFolder = picturesFolder + @"zTest\";
		const string posesFolder = picturesFolder + @"Poses\";
		const string skyfallFolder = destDrive + @"Skyfall\";
		const string videosFolder = destDrive + @"Videos\";
		private static readonly string[] folders = { picturesFolder, magazinesFolder, ikeaFolder, mensHealthFolder, screenshotFolder, londonFolder, meFolder, patriciaFolder, videosFolder, tattoosFolder, posesFolder };

		// Patterns //
		const string datePattern = @"M/d/yyyy h:mmtt";
		const string digitsPattern = @"\d+";
		const string yrMonDayHrMinSecMil = @"^20\d{14}";
		const string yrMonDayHrMinSec = @"^20\d{12}";
		const string yrMonDayHrMin = @"^20\d{10}";
		const string yrMonDayHr = @"^20\d{8}";
		const string yrMonDay = @"^20\d{6}";
		const string yrMon = @"^20\d{4}";
		const string yr = @"^20\d{2}";

		private static bool destDriveIsC = false;
		private static DateTime dateTimeThreshold = new DateTime(1982, 1, 1);
		private static DateTime nullDateTime = new DateTime(1982, 1, 1);
		private static DateTime startTime = DateTime.Now;
		private static DateTime firstOfYear = new DateTime(startTime.Year, 1, 1);
		private static DateTime twoMonthsAgo;
		private static DateTime lastMonth;
		private static DateTime endTime;
		private static TimeSpan ts;
		private static DateTime newDate = new DateTime(2017, 1, 8, 15, 0, 0); // used for SetDateTaken method //
		private static string thisYearFolder1;
		private static string lastMonthFolder;
		#endregion

		static void Main(string[] args)
		{
			StartProgram();
			//Process();
			//ChangeDateTakens();
			CheckFolders();
			ProcessFiles();
			//RedateByFilename();
			//RenameDownloadParts();
			//FillCreateDate();
			//TestFindDuplicateFiles();
			EndProgram();
		}

		#region Process 2017.01.03
		private static void Process()
		{
			// Fields //
			var source = @"C:\Pictures\2016\";
			var di = new DirectoryInfo(source);

			// Step 1: Get all files //
			var images = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Where(x => imageExtensions.Contains(x.Extension));
			Console.Write("\n\n{0}", images.Count());

			// Step 2: Get Dates //
			foreach (var img in images) {
				Console.Write("\n{0}", img.FullName);
				var date = GetDate(img);
			}


			// Step 3: Determine Destination //

			// Step 4: Copy/Move //

			// Step 5: Re-date //

		}

		private static DateTime GetDate(FileInfo fi)
		{
			var result = dateTimeThreshold;

			if (fi == null)
				return result;

			var dateTakenFromImage = GetDateTakenFromImage(fi.FullName);
			//var dateTaken = GetDateTakenFromFilename(path);


			return result;
		}
		#endregion

		#region Change Date Taken 2017.01.08
		private static void ChangeDateTakens()
		{
			var di = new DirectoryInfo(srcFolder);
			var images = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Where(x => imageExtensions.Contains(x.Extension));
			Thread.Sleep(1000);

			foreach (var img in images) {
				SetDateTaken(img.FullName);
				newDate = newDate.AddSeconds(1);
			}
		}

		private static void SetDateTaken(string path)
		{
			try {
				using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite)) {
					using (var myImage = Image.FromStream(fs, false, false)) {
						var propItem = myImage.GetPropertyItem(36867);
						Console.Write("\npropItem: {0}", Encoding.UTF8.GetString(propItem.Value));
						Console.Write("\nnewDate: {0}", newDate.ToString("yyyy:MM:dd HH:mm:ss"));
						propItem.Value = Encoding.UTF8.GetBytes(newDate.ToString("yyyy:MM:dd HH:mm:ss"));
						myImage.SetPropertyItem(propItem);

						if (path.EndsWith(".jpg"))
							path = path.Replace(".jpg", "b.jpg");
						if (path.EndsWith(".JPG"))
							path = path.Replace(".JPG", "b.jpg");
						
						myImage.Save(path);
					}
				}
			}
			catch (Exception ex) {
				Console.Write("\nError: SetDateTaken({0}):", path);
				Console.Write("\n{0}", ex.Message);
				Console.Write("\n{0}", ex.ToString());
			}
		}
		#endregion

		#region Duplicate File Finder 2016.12.27
		private static void TestFindDuplicateFiles()
		{
			var dirInfo = new DirectoryInfo(privateFolder);
			var files = dirInfo.EnumerateFiles("*.*", SearchOption.AllDirectories);
			//var files2 = dirInfo2.EnumerateFiles("*.*", SearchOption.AllDirectories).Where(s => !String.IsNullOrEmpty(s.Extension) && s.Length > 2048000);
			//var files3 = dirInfo3.EnumerateFiles("*.*", SearchOption.AllDirectories).Where(s => !String.IsNullOrEmpty(s.Extension) && s.Length > 2048000);
			FindDuplicateFiles(files);
			//var videos = dirInfo.EnumerateFiles("*.*", SearchOption.AllDirectories).Where(s => s.Extension.ToLower() == ".3gp" ||
			//																				   s.Extension.ToLower() == ".avi" ||
			//																				   s.Extension.ToLower() == ".flv" ||
			//																				   s.Extension.ToLower() == ".mkv" ||
			//																				   s.Extension.ToLower() == ".mov" ||
			//																				   s.Extension.ToLower() == ".mp4" ||
			//																				   s.Extension.ToLower() == ".mpg" ||
			//																				   s.Extension.ToLower() == ".mpeg" ||
			//																				   s.Extension.ToLower() == ".wmv");
			//FindDuplicateFiles(videos);
		}

		private static void FindDuplicateFiles(IEnumerable<FileInfo> files)
		{
			if (files == null || files.FirstOrDefault() == null) {
				Console.Write("\n** File list is null or empty!! **");
				return;
			}

			var duplicateGroups = files.GroupBy(f => f.Length).Where(group => group.Count() > 1);
			long totalFileSizes = 0;

			foreach (var group in duplicateGroups) {
				Console.WriteLine("Files with filesize {0}", group.Key);
				long length = 0;
				foreach (var file in group) {
					if (length == 0)
						totalFileSizes += file.Length;
					Console.WriteLine("  {0}", file.FullName);
				}
			}

			var kb = totalFileSizes/1024;
			var mb = kb/1024;
			var gb = mb/1024;
			Console.Write("\ntotalFileSizes: {0} bytes", totalFileSizes);
			Console.Write("\nkb: {0} kb", kb);
			Console.Write("\nmb: {0} mb", mb);
			Console.Write("\ngb: {0} gb", gb);
		}
		#endregion

		private static void RedateByFilename()
		{
			var files = Directory.EnumerateFiles(srcFolder, "*.*", SearchOption.AllDirectories).Where(s => s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".jpeg") || s.ToLower().EndsWith(".png") || s.ToLower().EndsWith(".3gp") || s.ToLower().EndsWith(".mov") || s.ToLower().EndsWith(".mp4"));
			foreach (var pic in files) {
				DateTime dateTaken = nullDateTime;
				dateTaken = GetDateTakenFromFilename(pic);
				if (dateTaken > nullDateTime)
					SetTimes(pic, dateTaken);
			}
		}

		#region FastMove - http://www.codeproject.com/Tips/777322/A-Faster-File-Copy
		/// <summary> Time the Move
		/// </summary> 
		/// <param name="source">Source file path</param> 
		/// <param name="dest">Destination file path</param> 
		public static void MoveTime(string source, string destination)
		{
			var start_time = DateTime.Now;
			FastMove(source, destination);
			var size = new FileInfo(destination).Length;
			var milliseconds = 1 + (int) ((DateTime.Now - start_time).TotalMilliseconds);
			// size time in milliseconds per hour
			long tsize = size * 3600000 / milliseconds;
			tsize = tsize / (int) Math.Pow(2, 30);
			if (consoleOut) { Console.WriteLine(tsize + "GB/hour"); }
		}

		/// <summary> Fast file move with big buffers
		/// </summary>
		/// <param name="source">Source file path</param> 
		/// <param name="dest">Destination file path</param> 
		static void FastMove(string source, string dest)
		{
			var buffer = BUFFER_SIZE;
			byte[] dataArray = new byte[buffer];
			using (var fsread = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.None, buffer)) {
				using (var bwread = new BinaryReader(fsread)) {
					using (var fswrite = new FileStream(dest, FileMode.Create, FileAccess.Write, FileShare.None, buffer)) {
						using (var bwwrite = new BinaryWriter(fswrite)) {
							for (; ; ) {
								var read = bwread.Read(dataArray, 0, buffer);
								if (0 == read)
									break;
								bwwrite.Write(dataArray, 0, read);
							}
						}
					}
				}
			}
			if (deleteAfter)
				File.Delete(source);
		}
		#endregion

		private static void FillCreateDate()
		{
			var pictures = Directory.EnumerateFiles(srcFolder, "*.*", SearchOption.AllDirectories).Where(s => !s.Contains(@"\emulated\0\Android\") && !s.ToLower().Contains("thumbnail") && !s.ToLower().Contains(@"\emulated\0\amazonmp3\") && !s.ToLower().Contains(@"\emulated\0\com.zinio.mobile.android.reader\") && (s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".jpeg") || s.ToLower().EndsWith(".png") || s.ToLower().EndsWith(".nef") || s.ToLower().EndsWith(".svg")));
			foreach (var pic in pictures) {
				var date = GetMinDate(pic);
				SetTimes(pic, date);

				//var file = new FileInfo(pic);
				//var createTime = file.CreationTime;

				//if (createTime == null || createTime < nullDateTime) {
				//	var min = file.LastAccessTime;
				//	var writeTime = file.LastWriteTime;

				//	if (writeTime < min)
				//		min = writeTime;

				//	File.SetCreationTime(pic, min);
				//}
			}
		}

		private static void RenameDownloadParts()
		{
			var files = Directory.EnumerateFiles(downloadsFolder, "*.*", SearchOption.AllDirectories).Where(x => x.ToLower().EndsWith(".part"));
			foreach (var file in files) {
				var nameWithoutExt = Path.GetFileNameWithoutExtension(file);
				var nameWithoutExt2 = Path.GetFileNameWithoutExtension(nameWithoutExt);
				var ext = Path.GetExtension(file);
				var ext2 = Path.GetExtension(nameWithoutExt);
				var fullExt = ext2 + ext;
				var newFile = downloadsFolder + nameWithoutExt2 + "INCOMPLETE" + ext2;

				if (DEBUG) {
					Console.Write("\n");
					Console.Write("\nFile: {0}", file);
					Console.Write("\nNew:  {0}", newFile);
				}

				if (File.Exists(newFile)) {
					// check filesize //
					var curFileSize = new FileInfo(file).Length;
					var newFileSize = new FileInfo(newFile).Length;

					if (curFileSize > newFileSize) {
						File.Delete(newFile);
						File.Move(file, newFile);
					}
					else /* curFileSize <= newFileSize */
						File.Delete(file);
				}

				else
					File.Move(file, newFile);
			}
		}

		private static void CheckFolders()
		{
			foreach (string folder in folders)
				if (!Directory.Exists(folder))
					Directory.CreateDirectory(folder);

			if (destDrive.Substring(0, 1).ToLower() == "c")
				destDriveIsC = true;

			//if (destDriveIsC) {
			//	if (startTime.Month > 3) { // Apr ~ Dec //
			//		twoMonthsAgo = startTime.AddMonths(-2);
			//		twoMonthsAgo = new DateTime(twoMonthsAgo.Year, twoMonthsAgo.Month, 1);
			//		lastMonth = startTime.AddMonths(-1);
			//		lastMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1);
			//		thisYearFolder1 = picturesFolder + twoMonthsAgo.Year + @"\01 Jan ~ " + twoMonthsAgo.ToString("MMM") + @"\";
			//		lastMonthFolder = picturesFolder + lastMonth.Year + @"\" + lastMonth.ToString("MM") + " " + lastMonth.ToString("MMM") + @" ~\";
			//	}

			//	else if (startTime.Month < 3) { // Jan & Feb //
			//		twoMonthsAgo = firstOfYear;
			//		lastMonth = firstOfYear;
			//		thisYearFolder1 = picturesFolder + twoMonthsAgo.Year + @"\";
			//		lastMonthFolder = picturesFolder + lastMonth.Year + @"\";
			//	}

			//	else /* startTime.Month == March */ {
			//		twoMonthsAgo = startTime.AddMonths(-2);
			//		twoMonthsAgo = new DateTime(twoMonthsAgo.Year, twoMonthsAgo.Month, 1);
			//		lastMonth = startTime.AddMonths(-1);
			//		lastMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1);
			//		thisYearFolder1 = picturesFolder + twoMonthsAgo.Year + @"\01 Jan\";
			//		lastMonthFolder = picturesFolder + lastMonth.Year + @"\02 Feb ~\";
			//	}

			//	if (!Directory.Exists(thisYearFolder1))
			//		Directory.CreateDirectory(thisYearFolder1);

			//	if (!Directory.Exists(lastMonthFolder))
			//		Directory.CreateDirectory(lastMonthFolder);
			//}
		}

		private static void ProcessFiles()
		{
			// Pictures //
			var pictures = Directory.EnumerateFiles(srcFolder, "*.*", SearchOption.AllDirectories).Where(s => !s.Contains(@"\emulated\0\Android\") && !s.ToLower().Contains("thumbnail") && !s.ToLower().Contains(@"\emulated\0\amazonmp3\") && !s.ToLower().Contains(@"\emulated\0\com.zinio.mobile.android.reader\") && (s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".jpeg") || s.ToLower().EndsWith(".png") || s.ToLower().EndsWith(".nef") || s.ToLower().EndsWith(".svg")));
			foreach (var pic in pictures) {
				var dateTaken = DetermineDateTaken(pic);
				var dest = DetermineDestination(pic, dateTaken);

				if (dateTaken > nullDateTime && !String.IsNullOrEmpty(dest)) {
					var returnedDest = RenameAndMoveFile(pic, dest);
					SetTimes(returnedDest, dateTaken);
				}
			}

			// Videos //
			var videos = Directory.EnumerateFiles(srcFolder, "*.*", SearchOption.AllDirectories).Where(s => s.ToLower().EndsWith(".3gp") || s.ToLower().EndsWith(".mov") || s.ToLower().EndsWith(".mp4"));
			foreach (var vid in videos) {
				DateTime dateTaken = DetermineDateTaken(vid);
				string dest = DetermineDestination(vid, dateTaken);

				if (dateTaken > nullDateTime && !String.IsNullOrEmpty(dest)) {
					string returnedDest = RenameAndMoveFile(vid, dest);
					SetTimes(returnedDest, dateTaken);
				}
			}
		}

		#region DetermineDateTaken
		private static DateTime DetermineDateTaken(string path)
		{
			DateTime dateTaken = nullDateTime;

			// Priority 1: Date Taken //
			dateTaken = GetDateTakenFromImage(path);
			if (dateTaken > nullDateTime)
				return dateTaken;

			// Priority 2: Date in filename //
			dateTaken = GetDateTakenFromFilename(path);
			if (dateTaken > nullDateTime)
				return dateTaken;

			// Priority 3: MinDate out of the 3 file dates //
			return GetMinDate(path);
		}

		// Retrieves the datetime WITHOUT loading the whole image //
		private static DateTime GetDateTakenFromImage(string path)
		{
			Regex r = new Regex(":");
			try {
				using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
					using (Image myImage = Image.FromStream(fs, false, false)) {
						PropertyItem propItem = myImage.GetPropertyItem(36867);
						string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
						return DateTime.Parse(dateTaken);
					}
				}
			}
			catch (Exception) {
				return nullDateTime;
			}
		}

		private static DateTime GetDateTakenFromFilename(string path)
		{
			var filename = Path.GetFileName(path).Replace(" ", "").Replace("-", "").Replace("_", "");
			var digits = Regex.Match(filename, digitsPattern);
			var digitsValue = digits.Value;
			DateTime test;

			try {
				if (Regex.IsMatch(digitsValue, yrMonDayHrMinSecMil)) {
					test = DateTime.Parse(digitsValue.Substring(0, 4) + "-" + digitsValue.Substring(4, 2) + "-" + digitsValue.Substring(6, 2) + " " + digitsValue.Substring(8, 2) + ":" + digitsValue.Substring(10, 2) + ":" + digitsValue.Substring(12, 2) + "." + digitsValue.Substring(14, 2));
					if (test < DateTime.Now)
						return test;
				}
				if (Regex.IsMatch(digitsValue, yrMonDayHrMinSec)) {
					test = DateTime.Parse(digitsValue.Substring(0, 4) + "-" + digitsValue.Substring(4, 2) + "-" + digitsValue.Substring(6, 2) + " " + digitsValue.Substring(8, 2) + ":" + digitsValue.Substring(10, 2) + ":" + digitsValue.Substring(12, 2));
					if (test < DateTime.Now)
						return test;
				}
				if (Regex.IsMatch(digitsValue, yrMonDayHrMin)) {
					test = DateTime.Parse(digitsValue.Substring(0, 4) + "-" + digitsValue.Substring(4, 2) + "-" + digitsValue.Substring(6, 2) + " " + digitsValue.Substring(8, 2) + ":" + digitsValue.Substring(10, 2));
					if (test < DateTime.Now)
						return test;
				}
				if (Regex.IsMatch(digitsValue, yrMonDayHr)) {
					test = DateTime.Parse(digitsValue.Substring(0, 4) + "-" + digitsValue.Substring(4, 2) + "-" + digitsValue.Substring(6, 2) + " " + digitsValue.Substring(8, 2) + ":00");
					if (test < DateTime.Now)
						return test;
				}
				if (Regex.IsMatch(digitsValue, yrMonDay)) {
					test = DateTime.Parse(digitsValue.Substring(0, 4) + "-" + digitsValue.Substring(4, 2) + "-" + digitsValue.Substring(6, 2));
					if (test < DateTime.Now)
						return test;
				}
				if (Regex.IsMatch(digitsValue, yrMon)) {
					test = DateTime.Parse(digitsValue.Substring(0, 4) + "-" + digitsValue.Substring(4, 2) + "-01");
					if (test < DateTime.Now)
						return test;
				}

				return nullDateTime;
			}

			catch (Exception e) {
				if (consoleOut) { Console.Write("\n{0}", e.ToString()); }
				return nullDateTime;
			}
		}

		private static DateTime GetMinDate(string path)
		{
			var file = new FileInfo(path);
			var createTime = file.CreationTime;
			var accessTime = file.LastAccessTime;
			var writeTime = file.LastWriteTime;
			var min = nullDateTime;

			if (createTime == null || createTime < min) {
				if (accessTime < writeTime)
					return accessTime;
				else
					return writeTime;
			}

			else {
				min = createTime;

				if (accessTime < min)
					min = accessTime;

				if (writeTime < min)
					min = writeTime;

				return min;
			}
		}
		#endregion

		private static string DetermineDestination(string path, DateTime dateTaken)
		{
			string filename = Path.GetFileNameWithoutExtension(path);
			string ext = Path.GetExtension(path).ToLower();

			// order by string length //
			if (filename.Contains("Consumer Reports ")) // 17 chars //
				return magazinesFolder + filename + ext;
			if (filename.Contains("Womens Health ")) // 14 chars //
				return mensHealthFolder + filename + ext;
			if (path.Contains(@"\Poses\") || filename.Contains("Poses ")) // 5 chars //
				return posesFolder + "Poses " + dateTaken.ToString("yyyy MMdd HHmm ssff") + ext;
			if (path.Contains(@"\Screenshot\") || path.Contains(@"\Screenshots\") || filename.Contains("Screenshot ")) // 11 chars //
				return screenshotFolder + "Screenshot " + dateTaken.ToString("yyyy MMdd HHmm ssff") + ext;
			if (path.Contains(@"\Mens Health\")) // 11 chars //
				return mensHealthFolder + filename + ext;
			if (path.Contains("MaximumPC ")) // 10 chars //
				return magazinesFolder + filename + ext;
			if (path.ToLower().Contains("screenshot")) // 10 chars //
				return screenshotFolder + "Screenshot " + dateTaken.ToString("yyyy MMdd HHmm ssff") + ext;
			if (path.Contains("PC Gamer ")) // 9 chars //
				return magazinesFolder + filename + ext;
			if (path.Contains(@"\zPatr\") || filename.Contains("Patricia ")) // 9 chars //
				return patriciaFolder + "Patricia " + dateTaken.ToString("yyyy MMdd HHmm ssff") + ext;
			if (path.Contains(@"\zLond\") || filename.Contains("London ")) // 7 chars //
				return londonFolder + "London " + dateTaken.ToString("yyyy MMdd HHmm ssff") + ext;
			if (path.ToLower().Contains("skyfall")) // 7 chars //
				return skyfallFolder + filename + ext;
			if (filename.Contains("Maxim ")) // 6 chars //
				return magazinesFolder + filename + ext;
			if (path.Contains(@"\Tattoos\") || filename.Contains("tattoo")) // 6 chars //
				return tattoosFolder + filename + ext;
			if (filename.Contains("ETNT ")) // 5 chars //
				return magazinesFolder + filename + ext;
			if (path.ToLower().Contains("ztest")) // 5 chars //
				return testFolder + "Test " + dateTaken.ToString("yyyy MMdd HHmm ssff") + ext;
			if (path.Contains(@"\Ikea\") || filename.Contains("Ikea ")) // 5 chars //
				return ikeaFolder + "Ikea " + dateTaken.ToString("yyyy MMdd HHmm ssff") + ext;
			if (path.Contains(@"\zMe\") || filename.Contains("Me ")) // 3 chars //
				return meFolder + "Me " + dateTaken.ToString("yyyy MMdd HHmm ssff") + ext;
			if (filename.Contains("MH ")) // 3 chars //
				return mensHealthFolder + filename + ext;
			if (path.Contains("GQ ")) // 3 chars //
				return magazinesFolder + filename + ext;

			return picturesFolder + dateTaken.Year.ToString() + @"\" + filename + ext;
		}

		private static string RenameAndMoveFile(string sourceFile, string destFile, char renamer = 'a')
		{
			if (consoleOut) {
				Console.Write("\n\nsourceFile: {0}", sourceFile);
				Console.Write("\n  destFile: {0}", destFile);
			}

			// Make sure it's not trying to move the same file onto itself because this method deletes first //
			if (sourceFile.ToLower() != destFile.ToLower()) {
				if (consoleOut) {
					Console.Write("\n  sourceFile != destFile. Checking if file exists...");
				}
				if (File.Exists(destFile)) {
					if (consoleOut) {
						Console.Write("\n  destFile exists. Comparing file sizes...");
					}
					long sourceFileSize = new FileInfo(sourceFile).Length;
					if (consoleOut) {
						Console.Write("\n  sourceFileSize: {0}", sourceFileSize);
					}
					long destFileSize = new FileInfo(destFile).Length;
					if (consoleOut) {
						Console.Write("\n    destFileSize: {0}", destFileSize);
					}
					if (sourceFileSize == destFileSize) {
						if (CopyOrMove == "Move") {
							if (consoleOut) {
								Console.Write("\n  sourceFileSize == destFileSize. Deleting destFile...");
							}
							File.Delete(destFile);
							File.Move(sourceFile, destFile);
						}
						return destFile;
					}
					else {
						string ext = Path.GetExtension(destFile);
						return RenameAndMoveFile(sourceFile, destFile.Replace(ext, "") + renamer + ext, ++renamer);
					}
				}

				if (CopyOrMove == "Copy") {
					if (consoleOut)
						Console.Write("\n  Copying file...");
					File.Copy(sourceFile, destFile);
				}

				else if (CopyOrMove == "Move") {
					if (consoleOut)
						Console.Write("\n  Moving file...");
					File.Move(sourceFile, destFile);
				}
			}

			return destFile;
		}

		private static void SetTimes(string path, DateTime time)
		{
			File.SetCreationTime(path, time);
			File.SetLastAccessTime(path, time);
			File.SetLastWriteTime(path, time);
		}

		private static void ReDateOnly()
		{
			var files = Directory.EnumerateFiles(srcFolder, "*.*", SearchOption.AllDirectories).Where(s => s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".jpeg") || s.ToLower().EndsWith(".png") || s.ToLower().EndsWith(".3gp") || s.ToLower().EndsWith(".mov") || s.ToLower().EndsWith(".mp4"));
			foreach (var pic in files) {
				DateTime dateTaken = nullDateTime;
				dateTaken = DetermineDateTaken(pic);
				if (dateTaken > nullDateTime)
					SetTimes(pic, dateTaken);
			}
		}

		#region Start & End Program
		private static void StartProgram()
		{
			Console.Write("\nFile Modifier started at {0}", startTime.ToString(datePattern).ToLower());
			Console.Write("\nconsoleOut: {0}", consoleOut.ToString().ToUpper());
			Console.Write("\n");
		}

		private static void EndProgram()
		{
			endTime = DateTime.Now;
			ts = endTime - startTime;

			if (consoleOut || consoleOutSummary) {
				Console.Write("\n");
				Console.Write("\nProgram ended at: {0}", endTime.ToString(datePattern).ToLower());
				Console.Write("\nIt took: ");
				if (ts.TotalMinutes >= 60)
					Console.Write("{0}hr ", ts.Hours);
				if (ts.TotalSeconds >= 60)
					Console.Write("{0}min ", ts.Minutes);
				Console.Write("{0}sec to complete", ts.Seconds);
				Console.Write("\n");
				Console.Write("\n... Program has finished. Press any key to close ...");
				Console.ReadKey(true);
			}
		}
		#endregion
	}
}
