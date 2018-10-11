using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NLog;
using Yutaka.IO;
using Yutaka.Text;
using Yutaka.Images;

namespace Yutaka.Tests
{
	static class Program
	{
		#region Fields
		#region Static Externs
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		const int SW_HIDE = 0;
		#endregion

		// Constants //
		const string PROGRAM_NAME = "Yutaka.Tests";
		const string TIMESTAMP = @"[HH:mm:ss] ";

		// PIVs //
		private static DateTime startTime = DateTime.Now;
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static bool consoleOut = true; // default = false //
		private static int errorCount = 0;
		private static int totalCount = 0;
		private static int errorCountThreshold = 7;
		private static double errorPerThreshold = 0.07;
		#endregion

		static void Main(string[] args)
		{
			StartProgram();
			Test_FileUtil_GetDirectorySize();
			EndProgram();
		}

		#region Test FileUtil.GetDirectorySize()
		private static void Test_FileUtil_GetDirectorySize()
		{
			var tests = new string[] {
				@"D:\_DeleteAfter2018_0822\Private\IT\backups\RCW_Imports(dev)\",
				@"D:\_DeleteAfter2018_0822\Public\Jeff's Ads\IT\cdrive clement\RCW_Imports\",
				@"D:\Departments\DevIT\Projects\RCW_Imports\",
			};

			for (int i = 0; i < tests.Length; i++) {
				Console.Write("\n\nTest: {0}\n  DirectorySize: {1}", tests[i], FileUtil.GetDirectorySize(tests[i]));
			}
		}
		#endregion Test FileUtil.GetDirectorySize()

		#region Test ImageUtil.ExistsAndValidByUrl()
		static void Test_ImageUtil_ExistsAndValidByUrl()
		{
			var tests = new string[] {
				"http://www.rarecoinwholesalers.com/Content/Images/Coins/134917fcs.jpg",
				"http://www.rarecoinwholesalers.com/Content/Images/Coins/134917fcs.JPEG",
				"http://www.rarecoinwholesalers.com/Content/Images/Coins/134917fcs.asdf",
			};

			for (int i = 0; i < tests.Length; i++) {
				Console.Write("\n\nTest: {0}\n  ExistsAndValidByUrl: {1}", tests[i], ImageUtil.ExistsAndValidByUrl(tests[i]));
			}
		}
		#endregion Test ImageUtil.ExistsAndValidByUrl()

		#region Methods
		static void StartProgram()
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

		static void EndProgram()
		{
			var endTime = DateTime.Now;
			var ts = endTime - startTime;
			var errorPer = (double) errorCount/totalCount;

			if (errorCount > errorCountThreshold && errorPer > errorPerThreshold)
				logger.Error("The number of errors is above the threshold.");

			var log = new string[4];
			log[0] = "Ending program";
			log[1] = String.Format("It took {0} to complete", ts.ToString(@"hh\:mm\:ss\.fff"));
			log[2] = String.Format("Total: {0}", totalCount);
			log[3] = String.Format("Errors: {0} ({1}){2}", errorCount, errorPer.ToString("P"), Environment.NewLine + Environment.NewLine);

			for (int i=0; i<log.Length; i++) {
				logger.Info(log[i]);
			}

			if (consoleOut) {
				Console.Write("\n");
				for (int i = 0; i < log.Length; i++) {
					Console.Write("\n{0}{1}", DateTime.Now.ToString(TIMESTAMP), log[i]);
				}
				Console.Write("\n.... Press any key to close the program ....");
				Console.ReadKey(true);
			}

			Environment.Exit(0); // in case you want to call this method outside of a standard successful program completion, this line will close the app //
		}
		#endregion
	}
}