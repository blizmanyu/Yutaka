using System;
using System.IO;
using System.Runtime.InteropServices;
using Yutaka.IO2;

namespace Yutaka.IO2.Tests
{
	class Program
	{
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
		const string PROGRAM_NAME = "Yutaka.Diagnostics.Tests";
		const string TIMESTAMP = @"[HH:mm:ss] ";

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
			Test_Debug();
			EndProgram();
		}

		#region Tests for FileUtil
		// Created Nov 21, 2019, Modified: Nov 21, 2019 //
		private static void Test_AutoRename()
		{
			var tests = new string[] {
				@"C:\TEMP\test.txt",
				@"C:\TEMP\TEMP\test.txt",
			};

			foreach (var test in tests) {
				Console.Write("\n");
				Console.Write("\n{0}) {1}", ++totalCount, test);
				Console.Write("\n   {0}", FileUtil.AutoRename(test));
			}
		}

		// Created Nov 21, 2019, Modified: Nov 21, 2019 //
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
			var path = @"C:\TEMP\";

			foreach (var test in Directory.EnumerateFiles(path)) {
				Console.Write("\n");
				Console.Write("\n{0}) {1}", ++totalCount, test);
				new YuFile(test).Debug();
			}
		}
		#endregion Tests for YuFile

		#region Misc Tests
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
	}
}