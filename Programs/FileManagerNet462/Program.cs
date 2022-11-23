using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NLog;
using Yutaka.IO2;

namespace FileManagerNet462
{
	class Program
	{
		// Config/Settings //
		private static bool consoleOut = true; // default = false //
		private static string Dest = @"ASDFG\";

		#region Fields
		// Const and readonlys //
		private const decimal TIME_FACTOR = 395189149.04562228086464351881m; // this is an arbitrary number based on average performance of my computer/drives.
		private const double ERROR_PERCENT_THRESHOLD = 0.07;
		private const int ERROR_COUNT_THRESHOLD = 7;
		private const string EMAIL_FROM = "from@server.com";
		private const string EMAIL_TO = "to@server.com";
		private const string PROGRAM_NAME = "FileManagerNet462";
		private const string TIMESTAMP = @"[HH:mm:ss] ";
		private static readonly DateTime startTime = DateTime.UtcNow;

		// Counters //
		private static int errorCount = 0;
		private static int processedCount = 0;
		private static int skippedCount = 0;
		private static int successCount = 0;
		private static int totalCount = 0;
		private static long totalSize = 0;

		// PIVs //
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static Stopwatch stopwatch = new Stopwatch();

		#region Static Externs
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		const int SW_HIDE = 0;
		#endregion
		#endregion

		static void Main(string[] args)
		{
			StartProgram();

			var deleteFiles = false;
			consoleOut = !deleteFiles;
			var sources = new string[] {
				@"asdfasdf\",
			};

			foreach (var source in sources)
				totalSize = FileUtil.GetDirectorySize(source, SearchOption.AllDirectories);

			if (deleteFiles) {
				if (totalSize > FileUtil.FOUR_GB) {
					Console.Write("\n******* Total Size is {0}! Estimated time is {1:n1}min. Press any key if you're certain you want to continue! *******", FileUtil.BytesToString(totalSize), totalSize / TIME_FACTOR);
					Console.ReadKey(true);
				}

				foreach (var source in sources)
					MoveAllFiles(source, Dest);
			}

			else {
				if (totalSize > FileUtil.TWO_GB) {
					Console.Write("\n******* Total Size is {0}! Estimated time is {1:n1}min. Press any key if you're certain you want to continue! *******", FileUtil.BytesToString(totalSize), totalSize / TIME_FACTOR);
					Console.ReadKey(true);
				}

				foreach (var source in sources)
					CopyAllFiles(source, Dest);
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

		#region StartProgram & EndProgram
		private static void HandleArgs(string[] args)
		{
			if (args == null || args.Length < 1)
			{
				; // if you want empty arguments to actually set default args, set them here.
			}

			else
			{
				var temp = String.Join(" ", args);

				if (temp.IndexOf("ASDFG", StringComparison.OrdinalIgnoreCase) > -1)
				{
					; // handle args like this
				}
			}
		}

		private static void StartProgram()
		{
			var log = String.Format("Starting {0} program", PROGRAM_NAME);
			logger.Info(log);

			if (consoleOut)
			{
				Console.Clear();
				Console.Write("{0}{1}", DateTime.Now.ToString(TIMESTAMP), log);
			}

			else
			{
				var handle = GetConsoleWindow();
				ShowWindow(handle, SW_HIDE); // hide window //
			}
		}

		private static void EndProgram()
		{
			var endTime = DateTime.UtcNow;
			var ts = endTime - startTime;
			var processedPerc = (double) processedCount / totalCount;
			var skippedPerc = (double) skippedCount / totalCount;
			var errorPerc = processedCount > 0 ? (double)errorCount / processedCount : (double)errorCount / totalCount;
			var successPerc = processedCount > 0 ? (double)successCount / processedCount : (double)successCount / totalCount;

			if (errorCount > ERROR_COUNT_THRESHOLD || errorPerc > ERROR_PERCENT_THRESHOLD) {
				logger.Error("The number of errors is above the threshold.");

				//if (errorCount > ERROR_COUNT_THRESHOLD && errorPerc > ERROR_PERCENT_THRESHOLD)
					//_smtpClient.TrySend(EMAIL_FROM, EMAIL_TO, PROGRAM_NAME, String.Format("Errors: {0} ({1})", errorCount, errorPerc.ToString("p")), out var response);
			}

			var log = new string[7];
			log[0] = "Ending program";

			if (ts.TotalSeconds < 61)
				log[1] = String.Format("It took {0} sec to complete", ts.ToString(@"s\.fff"));
			else if (ts.TotalMinutes < 61)
				log[1] = String.Format("It took {0}m {1}s to complete", ts.Minutes, ts.Seconds);
			else
				log[1] = String.Format("It took {0}h {1}m to complete", ts.Hours, ts.Minutes);

			log[2] = String.Format("    Total: {0,5:n0}", totalCount);

			if (totalCount > 0)
			{
				log[3] = String.Format("Processed: {0,5:n0} ({1,7})", processedCount, processedPerc.ToString("p").Replace(" ", ""));
				log[4] = String.Format("  Skipped: {0,5:n0} ({1,7})", skippedCount, skippedPerc.ToString("p").Replace(" ", ""));
				log[5] = String.Format("  Success: {0,5:n0} ({1,7})", successCount, successPerc.ToString("p").Replace(" ", ""));
				log[6] = String.Format("   Errors: {0,5:n0} ({1,7})", errorCount, errorPerc.ToString("p").Replace(" ", ""));
			}

			foreach (var l in log)
				logger.Info(l);

			logger.Info(Environment.NewLine + Environment.NewLine);

			if (consoleOut)
			{
				var timestamp = DateTime.Now.ToString(TIMESTAMP);
				Console.Write("\n");

				foreach (var l in log)
					Console.Write("\n{0}{1}", timestamp, l);

				Console.Write("\n\n. . . Press any key to close the program . . .\n\n");
				Console.ReadKey(true);
			}

			Environment.Exit(0); // in case you want to call this method outside of a standard successful program completion, this line will close the app //
		}
		#endregion StartProgram & EndProgram
	}
}