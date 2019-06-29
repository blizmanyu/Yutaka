using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using NLog;
using Yutaka.IO;

namespace FileManager
{
	class Program
	{
		// Config/Settings //
		const string PROGRAM_NAME = "FileManager";
		private static bool consoleOut = true; // default = false //

		#region Fields
		#region Static Externs
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		const int SW_HIDE = 0;
		#endregion

		const string TIMESTAMP = @"[HH:mm:ss] ";
		private static DateTime startTime = DateTime.Now;
		private static FileUtil _fileUtil = new FileUtil();
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static Stopwatch stopwatch = new Stopwatch();
		private static int errorCount = 0;
		private static int totalCount = 0;
		private static int errorCountThreshold = 7;
		private static double errorPerThreshold = 0.07;
		#endregion

		static void Main(string[] args)
		{
			StartProgram();

			var dest = @"asdfasdf";

			EndProgram();
		}

		private static void Test_YuVideo(string source, string dest, bool deleteFile = false)
		{
			consoleOut = true;
			Directory.CreateDirectory(dest);

			TimeSpan ts, timeRemaining;
			YuVideo vid;
			var videoExtensions = new Regex(".3gp|.asf|.avi|.f4a|.f4b|.f4v|.flv|.m4a|.m4b|.m4r|.m4v|.mkv|.mov|.mp4|.mpeg|.mpg|.webm|.wma|.wmv", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
			var videos = Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories).Where(x => videoExtensions.IsMatch(Path.GetExtension(x))).ToList();
			var videosCount = videos.Count;
			long thisSize = 0;
			long totalSize = 0;
			long processedSize = 0;
			long unprocessedSize = 0;
			var bytesPerSec = 0.0;
			var totalSizeStr = "";
			var processedSizeStr = "";
			var unprocessedSizeStr = "";

			for (int i = 0; i < videosCount; i++) {
				totalSize += new FileInfo(videos[i]).Length;
				totalSizeStr = String.Format("{0:n2} GB", totalSize / 1073741824.0);
			}

			if (consoleOut)
				Console.Write("\ntotalSize: {0}", totalSizeStr);

			if (totalSize > 107374182400) {
				Console.Write("\n******* totalSize > 100 GB. Press any key if you're sure you want to continue *******");
				Console.ReadKey(true);
			}

			stopwatch.Restart();

			for (int i = 0; i < videosCount; i++) {
				vid = new YuVideo(videos[i]);
				thisSize = vid.Size;
				if (consoleOut) {
					Console.Write("\n");
					Console.Write("\n{0}/{1}) {2}", ++totalCount, videosCount, videos[i]);
					Console.Write("\n     CreationTime: {0}", vid.CreationTime);
					Console.Write("\n   LastAccessTime: {0}", vid.LastAccessTime);
					Console.Write("\n    LastWriteTime: {0}", vid.LastWriteTime);
					Console.Write("\n     MediaCreated: {0}", vid.MediaCreated);
					Console.Write("\n     DateReleased: {0}", vid.DateReleased);
					Console.Write("\n      MinDateTime: {0}", vid.MinDateTime);
					Console.Write("\n");
					Console.Write("\n   ParentFolder: {0}", vid.ParentFolder);
					Console.Write("\n   NewFolder: {0}", vid.NewFolder);
					Console.Write("\n   NewFilename: {0}", vid.NewFilename);
					Console.Write("\n   Size: {0:n2} GB", thisSize / 1073741824.0);
				}

				Directory.CreateDirectory(String.Format("{0}{1}", dest, vid.NewFolder));
				_fileUtil.Move(videos[i], String.Format("{0}{1}{2}", dest, vid.NewFolder, vid.NewFilename), deleteFile);
				_fileUtil.Redate(String.Format("{0}{1}{2}", dest, vid.NewFolder, vid.NewFilename), vid.MinDateTime);

				if (consoleOut) {
					ts = stopwatch.Elapsed;
					processedSize += thisSize;
					processedSizeStr = String.Format("{0:n2}", processedSize / 1073741824.0);
					unprocessedSize = totalSize - processedSize;
					unprocessedSizeStr = String.Format("{0:n2} GB", unprocessedSize / 1073741824.0);
					bytesPerSec = processedSize / ts.TotalSeconds;
					timeRemaining = TimeSpan.FromSeconds(unprocessedSize / bytesPerSec);
					Console.Write("\n");
					Console.Write("\n[{0:00}:{1:00}:{2:00}] Processed {3}/{4} ({5:p2})", ts.Hours, ts.Minutes, ts.Seconds, processedSizeStr, totalSizeStr, ((double) processedSize / totalSize));
					Console.Write("\n  MB per second: {0:n2}", bytesPerSec / 1024.0 / 1024.0);
					Console.Write("\n  Approx time remaining: {0:00}:{1:00}:{2:00}", timeRemaining.Hours, timeRemaining.Minutes, timeRemaining.Seconds);
				}
			}
		}

		private static void Test_YuImage(string source, string dest, bool deleteFile = false)
		{
			consoleOut = !deleteFile;
			Directory.CreateDirectory(dest);

			YuImage img;
			var imageExtensions = new Regex(".ai|.bmp|.exif|.gif|.jpg|.jpeg|.nef|.png|.psd|.svg|.tiff|.webp", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
			var images = Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories).Where(x => imageExtensions.IsMatch(Path.GetExtension(x))).ToList();
			var imagesCount = images.Count;

			for (int i = 0; i < imagesCount; i++) {
				img = new YuImage(images[i]);
				if (consoleOut) {
					Console.Write("\n");
					Console.Write("\n{0}/{1} ({2})", ++totalCount, imagesCount, ((double) totalCount / imagesCount).ToString("p2"));
					Console.Write("\n{0}", images[i]);
					Console.Write("\n     CreationTime: {0}", img.CreationTime);
					Console.Write("\n        DateTaken: {0}", img.DateTaken);
					Console.Write("\n   LastAccessTime: {0}", img.LastAccessTime);
					Console.Write("\n    LastWriteTime: {0}", img.LastWriteTime);
					Console.Write("\n      MinDateTime: {0}", img.MinDateTime);
					Console.Write("\n");
					Console.Write("\n   DirectoryName: {0}", img.DirectoryName);
					Console.Write("\n   ParentFolder: {0}", img.ParentFolder);
					Console.Write("\n   NewFolder: {0}", img.NewFolder);
					Console.Write("\n   NewFilename: {0}", img.NewFilename);
				}

				Directory.CreateDirectory(String.Format("{0}{1}", dest, img.NewFolder));
				_fileUtil.Move(images[i], String.Format("{0}{1}{2}", dest, img.NewFolder, img.NewFilename), deleteFile);
				_fileUtil.Redate(String.Format("{0}{1}{2}", dest, img.NewFolder, img.NewFilename), img.MinDateTime);
			}

			var count = _fileUtil.DeleteAllThumbsDb(source);
			Console.Write("\n\nDeleted {0} 'Thumbs.db's.", count);
		}

		#region Start & EndProgram
		private static void StartProgram()
		{
			var log = String.Format("Starting {0} program", PROGRAM_NAME);
			logger.Info(log);

			if (consoleOut) {
				Console.Clear();
				Console.Write("{0}{1}", DateTime.Now.ToString(TIMESTAMP), log);
			}

			else {
				var handle = GetConsoleWindow();
				ShowWindow(handle, SW_HIDE); // hide window //
			}
		}

		private static void EndProgram()
		{
			var endTime = DateTime.Now;
			var ts = endTime - startTime;
			var errorPer = (double) errorCount / totalCount;

			if (errorCount > errorCountThreshold || errorPer > errorPerThreshold) {
				logger.Error("The number of errors is above the threshold.");

				if (errorCount > errorCountThreshold && errorPer > errorPerThreshold) {
					//MailUtil.Send("fromEmail", "fromEmail", PROGRAM_NAME, String.Format("Errors: {0} ({1})", errorCount, errorPer.ToString("P")));
				}
			}

			var log = new string[4];
			log[0] = "Ending program";
			log[1] = String.Format("It took {0} to complete", ts.ToString(@"hh\:mm\:ss\.fff"));
			log[2] = String.Format("Total: {0}", totalCount);
			log[3] = String.Format("Errors: {0} ({1}){2}", errorCount, errorPer.ToString("P"), Environment.NewLine + Environment.NewLine);

			logger.Info(log[0]);
			logger.Info(log[1]);
			logger.Info(log[2]);
			logger.Info(log[3]);

			if (consoleOut) {
				var timestamp = DateTime.Now.ToString(TIMESTAMP);
				Console.Write("\n");
				Console.Write("\n{0}{1}", timestamp, log[0]);
				Console.Write("\n{0}{1}", timestamp, log[1]);
				Console.Write("\n{0}{1}", timestamp, log[2]);
				Console.Write("\n{0}{1}", timestamp, log[3]);
				Console.Write("\n.... Press any key to close the program ....");
				Console.ReadKey(true);
			}

			Environment.Exit(0); // in case you want to call this method outside of a standard successful program completion, this line will close the app //
		}
		#endregion Start & EndProgram
	}
}