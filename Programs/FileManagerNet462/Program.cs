using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using NLog;
using Yutaka.IO2;

namespace FileManagerNet462
{
	class Program
	{
		// Config/Settings //
		const string PROGRAM_NAME = "FileManagerNet462";
		private const decimal TIME_FACTOR = 395189149.04562228086464351881m; // this is an arbitrary number based on average performance of my computer/drives.
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
		private static readonly DateTime startTime = DateTime.Now;
		private static readonly double errorPerThreshold = 0.07;
		private static readonly int errorCountThreshold = 7;
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static Stopwatch stopwatch = new Stopwatch();
		private static int errorCount = 0;
		private static int totalCount = 0;
		private static long totalSize = 0;
		#endregion

		static void Main(string[] args)
		{
			StartProgram();

			var deleteFiles = false;
			consoleOut = !deleteFiles;
			var dest = @"asdfasdf\";
			var sources = new string[] {
				@"asdfasdf\",
			};

			if (deleteFiles) {
				foreach (var source in sources) {
					totalSize = FileUtil.GetDirectorySize(source, SearchOption.AllDirectories);

					if (totalSize > FileUtil.FOUR_GB) {
						Console.Write("\n******* Total Size is {0}! Estimated time is {1:n1}min. Press any key if you're certain you want to continue! *******", FileUtil.BytesToString(totalSize), totalSize / TIME_FACTOR);
						Console.ReadKey(true);
					}

						MoveAllFiles(source, dest);
					}
			}

			else {
				foreach (var source in sources) {
					totalSize = FileUtil.GetDirectorySize(source, SearchOption.AllDirectories);

					if (totalSize > FileUtil.TWO_GB) {
						Console.Write("\n******* Total Size is {0}! Estimated time is {1:n1}min. Press any key if you're certain you want to continue! *******", FileUtil.BytesToString(totalSize), totalSize/TIME_FACTOR);
						Console.ReadKey(true);
					}

					CopyAllFiles(source, dest);
				}
			}

			EndProgram();
		}

		private static void CopyAllFiles(string source, string dest)
		{
			var count = FileUtil.DeleteAllCacheFiles(source, SearchOption.AllDirectories);
			Directory.CreateDirectory(dest);

			var bytesProcessed = 0L;
			YuFile fi;
			var files = Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories).ToList();

			for (var i = 0; i < files.Count; i++) {
				totalCount++;
				fi = new YuFile(files[i]);
				bytesProcessed += fi.Size;

				if (consoleOut)
					fi.Debug();

				Directory.CreateDirectory(Path.Combine(dest, fi.NewFolder));
				if (FileUtil.TryCopy(files[i], Path.Combine(dest, fi.NewFolder, fi.Name), OverwriteOption.RenameIfDifferentSize)) {
					Console.Write("\n{3}    {0}/{1} ({2})", FileUtil.BytesToString(bytesProcessed), FileUtil.BytesToString(totalSize), ((double) bytesProcessed / totalSize).ToString("p2"), source);

					if (FileUtil.TryRedate(Path.Combine(dest, fi.NewFolder, fi.Name), fi.MinDateTime))
						continue;
					else
						errorCount++;
				}

				else
					errorCount++;
			}

			count += FileUtil.DeleteAllCacheFiles(dest, SearchOption.AllDirectories);
			Console.Write("\n\nDeleted {0} cache files", count);
		}

		private static void MoveAllFiles(string source, string dest)
		{
			var count = FileUtil.DeleteAllCacheFiles(source, SearchOption.AllDirectories);
			Directory.CreateDirectory(dest);

			var bytesProcessed = 0L;
			YuFile fi;
			var files = Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories).ToList();

			for (var i = 0; i < files.Count; i++) {
				totalCount++;
				fi = new YuFile(files[i]);
				bytesProcessed += fi.Size;

				if (consoleOut)
					fi.Debug();

				Directory.CreateDirectory(Path.Combine(dest, fi.NewFolder));
				if (FileUtil.TryMove(files[i], Path.Combine(dest, fi.NewFolder, fi.Name), OverwriteOption.RenameIfDifferentSize)) {
					if (i % 10 == 0)
						Console.Write("\n{3}    {0}/{1} ({2})", FileUtil.BytesToString(bytesProcessed), FileUtil.BytesToString(totalSize), ((double) bytesProcessed / totalSize).ToString("p2"), source);

					if (FileUtil.TryRedate(Path.Combine(dest, fi.NewFolder, fi.Name), fi.MinDateTime))
						continue;
					else
						errorCount++;
				}

				else
					errorCount++;
			}

			count += FileUtil.DeleteAllCacheFiles(dest, SearchOption.AllDirectories);
			Console.Write("\n\nDeleted {0} cache files", count);
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

			var log = new string[5];
			log[0] = "Ending program";
			log[1] = String.Format("It took {0} to complete", ts.ToString(@"hh\:mm\:ss\.fff"));
			log[2] = String.Format("Total: {0}", totalCount);
			log[3] = String.Format("Total Size: {0} ({1:n0} bytes)", FileUtil.BytesToString(totalSize), totalSize);
			log[4] = String.Format("Errors: {0} ({1}){2}", errorCount, errorPer.ToString("P"), Environment.NewLine + Environment.NewLine);

			foreach (var v in log)
				logger.Info(v);

			var timestamp = DateTime.Now.ToString(TIMESTAMP);
			Console.Write("\n");
			foreach (var v in log)
				Console.Write("\n{0}{1}", timestamp, v);
			Console.Write("\n.... Press any key to close the program ....");
			Console.ReadKey(true);

			Environment.Exit(0); // in case you want to call this method outside of a standard successful program completion, this line will close the app //
		}
		#endregion Start & EndProgram
	}
}