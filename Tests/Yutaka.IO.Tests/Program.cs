using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using NLog;

namespace Yutaka.IO.Tests
{
	class Program
	{
		// Config/Settings //
		const string PROGRAM_NAME = "Yutaka.IO.Tests";
		const string ROOT_FOLDER = @"E:\Office\Videos\";
		const string DEST_FOLDER = @"E:\Office\Processed\";
		const string SEARCH_PATTERN = "*";
		private static bool consoleOut = true; // default = false //

		#region Fields
		#region Static Externs
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		const int SW_HIDE = 0;
		#endregion

		// Constants //
		const string TIMESTAMP = @"[HH:mm:ss] ";

		// PIVs //
		private static DateTime startTime = DateTime.Now;
		private static FileUtil _fileUtil = new FileUtil();
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static int errorCount = 0;
		private static int totalCount = 0;
		private static int errorCountThreshold = 7;
		private static double errorPerThreshold = 0.07;
		#endregion

		static void Main(string[] args)
		{
			StartProgram();
			Test_Delete();
			EndProgram();
		}

		#region Tests for FileUtil
		// TODO: Include immediate parent folder in dest folder //
		// TODO: Handle general Exception (in FileUtil.cs as well) //
		// TODO: See what happens when file already exists at dest //
		private static void FixCreationTimeThenMove(string root, string searchPattern = "*", int initialCapacity = 1024)
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
				logger.Info("******* currentDir: {0} *******", currentDir);

				// Perform required action on each file  //
				try {
					foreach (var file in Directory.EnumerateFiles(currentDir, searchPattern)) {
						totalCount++;
						logger.Info("file: {0}", file);
						fi = new FileInfo(file);

						if (fi.CreationTime > fi.LastWriteTime)
							fi.CreationTime = fi.LastWriteTime;

						var year = fi.CreationTime.Year;
						_fileUtil.Move(fi, String.Format("{0}{1}\\{2}", DEST_FOLDER, year, fi.Name));
					}
				}

				catch (UnauthorizedAccessException ex) {
					errorCount++;
					logger.Error(ex.Message);
					continue;
				}

				catch (DirectoryNotFoundException ex) {
					errorCount++;
					logger.Error(ex.Message);
					continue;
				}

				catch (PathTooLongException ex) {
					errorCount++;
					logger.Error(ex.Message);
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
				catch (UnauthorizedAccessException ex) {
					errorCount++;
					logger.Error(ex.Message);
					continue;
				}
				catch (DirectoryNotFoundException ex) {
					errorCount++;
					logger.Error(ex.Message);
					continue;
				}
				catch (PathTooLongException ex) {
					errorCount++;
					logger.Error(ex.Message);
					continue;
				}
			}
		}

		private static void Test_Delete()
		{
			int count;
			string searchPattern;
			SearchOption searchOption;
			var folder = @"C:\TEMP\TEMP\";

			#region Test 1
			searchPattern = "*.png";
			searchOption = SearchOption.TopDirectoryOnly;
			count = _fileUtil.Delete(folder, searchPattern, searchOption);
			Console.Write("\nDeleted {0} files.", count);
			#endregion Test 1

			#region Test 2
			searchPattern = "*.gif";
			searchOption = SearchOption.TopDirectoryOnly;
			count = _fileUtil.Delete(folder, searchPattern, searchOption);
			Console.Write("\nDeleted {0} files.", count);
			#endregion Test 2

			#region Test 3
			searchPattern = "*";
			searchOption = SearchOption.TopDirectoryOnly;
			count = _fileUtil.Delete(folder, searchPattern, searchOption);
			Console.Write("\nDeleted {0} files.", count);
			#endregion Test 3

			#region Test 4
			searchPattern = "*.png";
			searchOption = SearchOption.AllDirectories;
			count = _fileUtil.Delete(folder, searchPattern, searchOption);
			Console.Write("\nDeleted {0} files.", count);
			#endregion Test 4

			#region Test 5
			searchPattern = "*.gif";
			searchOption = SearchOption.AllDirectories;
			count = _fileUtil.Delete(folder, searchPattern, searchOption);
			Console.Write("\nDeleted {0} files.", count);
			#endregion Test 5

			#region Test 6
			searchPattern = "*";
			searchOption = SearchOption.AllDirectories;
			count = _fileUtil.Delete(folder, searchPattern, searchOption);
			Console.Write("\nDeleted {0} files.", count);
			#endregion Test 6
		}
		#endregion Tests for FileUtil

		#region Misc Tests
		private static void Process()
		{
			Console.Write("\n{0}", new DateTime());
		}
		#endregion Misc Tests

		#region StartProgram & EndProgram
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

			if (errorCount > errorCountThreshold && errorPer > errorPerThreshold)
				logger.Error("The number of errors is above the threshold.");

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
				Console.Write("\n");
				Console.Write("\n{0}{1}", DateTime.Now.ToString(TIMESTAMP), log[0]);
				Console.Write("\n{0}{1}", DateTime.Now.ToString(TIMESTAMP), log[1]);
				Console.Write("\n{0}{1}", DateTime.Now.ToString(TIMESTAMP), log[2]);
				Console.Write("\n{0}{1}", DateTime.Now.ToString(TIMESTAMP), log[3]);
				Console.Write("\n.... Press any key to close the program ....");
				Console.ReadKey(true);
			}

			Environment.Exit(0); // in case you want to call this method outside of a standard successful program completion, this line will close the app //
		}
		#endregion StartProgram & EndProgram
	}
}