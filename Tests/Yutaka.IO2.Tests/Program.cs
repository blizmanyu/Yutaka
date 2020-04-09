using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Yutaka.IO2.Tests
{
	class Program
	{
		private static readonly bool consoleOut = true; // default = false //

		#region Fields
		#region Static Externs
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		const int SW_HIDE = 0;
		#endregion

		// Constants //
		const string PROGRAM_NAME = "Yutaka.Diagnostics.Tests";
		const string TIMESTAMP = @"[HH:mm:ss] ";
		private const decimal ONE_KB = 1024m;
		private const decimal NNF_KB = 1023488m;
		private const decimal ONE_MB = 1048576m;
		private const decimal NNF_MB = 1048051712m;
		private const decimal ONE_GB = 1073741824m;
		private const decimal NNF_GB = 1073204953088m;
		private const decimal ONE_TB = 1099511627776m;
		private const decimal NNF_TB = 1098961871962112m;
		private const decimal ONE_PB = 1125899906842624m;

		// PIVs //
		private static DateTime startTime = DateTime.Now;
		private static int errorCount = 0;
		private static int totalCount = 0;
		private static int errorCountThreshold = 7;
		private static double errorPerThreshold = 0.07;
		#endregion

		static void Main(string[] args)
		{
			StartProgram();
			Test_GetDirectorySize();
			EndProgram();
		}

		#region Tests for FileUtil
		// Created Apr 8, 2020, Modified: Apr 8, 2020 //
		private static void Test_GetDirectorySize()
		{
			var tests = new string[] {
				@"asdfasdf",
				@"asdfasdf",
			};

			long result;

			foreach (var test in tests) {
				Console.Write("\n");
				Console.Write("\n{0}) {1}", ++totalCount, test);
				result = FileUtil.GetDirectorySize(test, SearchOption.AllDirectories);
				Console.Write("\n  Size: {0} ({1:n0} bytes)", FileUtil.BytesToString(result), result);
				Console.Write("\n");
			}
		}

		// Created Apr 6, 2020, Modified: Apr 6, 2020 //
		private static void Test_BytesToString()
		{
			var tests = new long[] { 999, 1000, 1001, 1023, 1024, 1025, 9999, 10000, 10001, 99999, 100000, 100001, 999999, 1000000, 1000001, 1023487, 1023488, 1023489, 1048575, 1048576, 1048577, 9999999, 10000000, 10000001, 99999999, 100000000, 100000001, 999999999, 1000000000, 1000000001, 1048051711, 1048051712, 1048051713, 1073741823, 1073741824, 1073741825, 9999999999, 10000000000, 10000000001, 99999999999, 100000000000, 100000000001, 999999999999, 1000000000000, 1000000000001, 1073204953087, 1073204953088, 1073204953089, 1099511627775, 1099511627776, 1099511627777, 9999999999999, 10000000000000, 10000000000001, 99999999999999, 100000000000000, 100000000000001, 999999999999999, 1000000000000000, 1000000000000001, 1098961871962111, 1098961871962112, 1098961871962113, 1125899906842623, 1125899906842624, 1125899906842625, 9999999999999999, 10000000000000000, 10000000000000001, 99999999999999999, 100000000000000000, 100000000000000001, 999999999999999999, 1000000000000000000, 1000000000000000001, 1152921504606846975, 1152921504606846976, 1152921504606846977, };
			Console.Write("\n");
			foreach (var test in tests)
				Console.Write("\n{0}) {1} ({2:n0})", ++totalCount, FileUtil.BytesToString(test), test);
		}

		// Created Apr 6, 2020, Modified: Apr 6, 2020 //
		private static void BytesToStringHelper()
		{
			var list = new List<decimal>();
			var test1 = 1000m;
			var test2 = 1024m;
			while (0 < test1 && test1 < long.MaxValue) {
				list.Add(test1 - 1);
				list.Add(test1);
				list.Add(test1 + 1);
				if (0 < test2 && test2 < long.MaxValue) {
					list.Add(test2 - 1);
					list.Add(test2);
					list.Add(test2 + 1);
					test2 *= 1024m;
				}
				test1 *= 10m;
			}

			decimal temp;
			temp = ONE_KB * 999.5m;
			list.Add(temp - 1);
			list.Add(temp);
			list.Add(temp + 1);
			temp = ONE_MB * 999.5m;
			list.Add(temp - 1);
			list.Add(temp);
			list.Add(temp + 1);
			temp = ONE_GB * 999.5m;
			list.Add(temp - 1);
			list.Add(temp);
			list.Add(temp + 1);
			temp = ONE_TB * 999.5m;
			list.Add(temp - 1);
			list.Add(temp);
			list.Add(temp + 1);

			list = list.OrderBy(x => x).ToList();
			Console.Write("\n");
			Console.Write("{0}", String.Join(", ", list));
		}

		// Created Apr 2, 2020, Modified: Apr 2, 2020 //
		private static void Test_DeleteAllCacheFiles()
		{
			var folderPath = @"C:\TEMP\TEMP";
			var count = FileUtil.DeleteAllCacheFiles(folderPath, SearchOption.AllDirectories);
			Console.Write("\nDeleted {0} file(s).", count);
		}

		// Created Mar 20, 2020, Modified: Mar 20, 2020 //
		private static void Test_TryWrite()
		{
			object value;
			string path;
			var testFolder = @"C:\temp";

			value = "";
			path = "";
			Console.Write("\n{0}) path: {1}", ++totalCount, path);
			if (!FileUtil.TryWrite(value, path))
				errorCount++;

			value = "test 2020 0320 0126 0001";
			path = Path.Combine(testFolder, String.Format("{0}.txt", value));
			Console.Write("\n{0}) path: {1}", ++totalCount, path);
			if (!FileUtil.TryWrite(value, path))
				errorCount++;

			value = "test 2020 0320 0126 0010";
			path = Path.Combine(testFolder, @"1\2\3", String.Format("{0}.txt", value));
			Console.Write("\n{0}) path: {1}", ++totalCount, path);
			if (!FileUtil.TryWrite(value, path))
				errorCount++;
		}

		// Created Mar 20, 2020, Modified: Mar 20, 2020 //
		private static void Test_Write()
		{
			object value;
			string path;
			var testFolder = @"C:\temp";

			totalCount++;
			value = "test 2020 0320 0101 0001";
			path = Path.Combine(testFolder, String.Format("{0}.txt", value));
			Console.Write("\npath: {0}", path);
			FileUtil.Write(value, path);

			totalCount++;
			value = "test 2020 0320 0101 0010";
			path = Path.Combine(testFolder, @"1\2\3", String.Format("{0}.txt", value));
			Console.Write("\npath: {0}", path);
			FileUtil.Write(value, path);
		}

		// Created Mar 19, 2020, Modified: Mar 19, 2020 //
		private static void Test_Redate()
		{
			var filename = @"asdf";
			var dt = new DateTime();
			totalCount++;
			if (!FileUtil.TryRedate(filename, dt))
				errorCount++;

			filename = @"asdf";
			dt = DateTime.Now;
			totalCount++;
			if (!FileUtil.TryRedate(filename, dt))
				errorCount++;
		}

		// Created Dec 5, 2019, Modified: Dec 5, 2019 //
		private static void DeleteAllThumbnailCache()
		{
			var folders = new string[] {
				@"C:\",
			};

			int deletedThisIter;
			var totalDeleted = 0;

			foreach (var folder in folders) {
				deletedThisIter = 0;
				Console.Write("\n");
				Console.Write("\n{0}) {1}", ++totalCount, folder);
				deletedThisIter += FileUtil.Delete(folder, ".ds_store", SearchOption.AllDirectories);
				deletedThisIter += FileUtil.Delete(folder, "thumbs*.db", SearchOption.AllDirectories);
				deletedThisIter += FileUtil.Delete(folder, "desktop*.ini", SearchOption.AllDirectories);
				totalDeleted += deletedThisIter;
				Console.Write("\n   Deleted {0} files", deletedThisIter);
			}

			Console.Write("\n\nTotal Deleted: {0}", totalDeleted);
		}

		private static void Test_FastCopy()
		{
			var source = @"C:\TEMP\test.txt";
			var dest = @"C:\TEMP\TEMP\test.txt";
			//FileUtil.FastCopy(source, dest);
		}
		#endregion Tests for FileUtil

		#region Tests for YuFile
		private static void Test_Debug()
		{
			var path = @"Q:\DCIM\Screenshots\";

			foreach (var test in Directory.EnumerateFiles(path)) {
				Console.Write("\n");
				Console.Write("\n{0}) {1}", ++totalCount, test);
				new YuFile(test).Debug();
			}
		}
		#endregion Tests for YuFile

		#region Tests for YuImage
		// Created Mar 20, 2020, Modified: Mar 20, 2020 //
		private static void Test_YuImage()
		{
			var tests = new string[] {
				@"C:\Pictures\2020\jacket1.jpg",
				@"C:\Pictures\2020\jacket2.jpg",
				@"C:\Pictures\2020\jacket3.jpg",
			};

			YuImage img;

			foreach (var test in tests) {
				Console.Write("\n{0}) {1}", ++totalCount, test);
				img = new YuImage(test);
				img.Debug();
				Console.Write("\n");
			}
		}
		#endregion Tests for YuImage

		#region Misc Tests
		private static void Test_FileInfo_MoveTo()
		{
			var file1 = @"C:\TEMP\test file 1.txt";
			var file2 = @"C:\TEMP\test file 2.txt";

			try {
				new FileInfo(file1).MoveTo(file2);
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in Test_FileInfo_MoveTo(){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Test_FileInfo_MoveTo(){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);

				Console.Write("\n{0}", log);
				#endregion Log
			}
		}

		private static void Test_Path_GetPathRoot()
		{
			var file1 = @"c:\alsdkfj\";
			var file2 = @"C:\alsdkjf\";
			Console.Write("\n{0}", Path.GetPathRoot(file1));
			Console.Write("\n{0}", Path.GetPathRoot(file2));
		}
		#endregion Misc Tests

		#region Start & EndProgram
		static void StartProgram()
		{
			var log = String.Format("Starting {0} program", PROGRAM_NAME);

			if (consoleOut) {
				Console.Clear();
				Console.Write("{0}{1}", DateTime.Now.ToString(TIMESTAMP), log);
			}

			else {
				var handle = GetConsoleWindow();
				ShowWindow(handle, SW_HIDE); // hide window //
			}
		}

		static void EndProgram()
		{
			var endTime = DateTime.Now;
			var ts = endTime - startTime;
			var errorPer = (double) errorCount / totalCount;

			if (errorCount > errorCountThreshold && errorPer > errorPerThreshold)
				Console.Write("\nThe number of errors is above the threshold.");

			var log = new string[4];
			log[0] = "Ending program";
			log[1] = String.Format("It took {0} to complete", ts.ToString(@"hh\:mm\:ss\.fff"));
			log[2] = String.Format("Total: {0}", totalCount);
			log[3] = String.Format("Errors: {0} ({1}){2}", errorCount, errorPer.ToString("P"), Environment.NewLine + Environment.NewLine);

			if (consoleOut) {
				Console.Write("\n\n\n\n\n\n\n****************************");
				for (int i = 0; i < log.Length; i++) {
					Console.Write("\n{0}{1}", DateTime.Now.ToString(TIMESTAMP), log[i]);
				}
				Console.Write("\n.... Press any key to close the program ....");
				Console.ReadKey(true);
			}

			Environment.Exit(0); // in case you want to call this method outside of a standard successful program completion, this line will close the app //
		}
		#endregion Start & EndProgram

		#region Commented Out
		// Created Nov 21, 2019, Modified: Nov 21, 2019 //
		//private static void Test_AutoRename()
		//{
		//	var tests = new string[] {
		//		@"C:\TEMP\test.txt",
		//		@"C:\TEMP\TEMP\test.txt",
		//	};

		//	foreach (var test in tests) {
		//		Console.Write("\n");
		//		Console.Write("\n{0}) {1}", ++totalCount, test);
		//		Console.Write("\n   {0}", FileUtil.AutoRename(test));
		//	}
		//}

		// Created Nov 21, 2019, Modified: Nov 21, 2019 //
		#endregion Commented Out
	}
}