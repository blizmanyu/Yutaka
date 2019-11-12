using System;
using System.Runtime.InteropServices;
using Yutaka.IO;

namespace Yutaka.Diagnostics.Tests
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
			Test_CreateAnimatedGif();
			EndProgram();
		}

		#region Tests for FfmpegUtil
		private static void Test_CreateAnimatedGif()
		{
			TimeSpan startTime;
			var length = TimeSpan.FromMilliseconds(10000);
			var source = @"asdf";
			var fps = 24;
			var width = 960;

			var _fileUtil = new FileUtil();
			_fileUtil.CreateGalleryHtml(@"C:\TEMP");
			return;

			startTime = TimeSpan.FromMilliseconds(10000);
			using (var proc1 = FfmpegUtil.StartCreatingPalette(startTime, length, source, fps, width, true)) {
				proc1.WaitForExit();

				using (var proc2 = FfmpegUtil.StartCreatingAnimatedGif(startTime, length, source, true)) {
					proc2.WaitForExit();

					using (var proc3 = FfmpegUtil.StartCreatingThumbnail(startTime, length, source, true)) {
						proc3.WaitForExit();
					}
				}
			}

			startTime = TimeSpan.FromMilliseconds(20000);
			using (var proc1 = FfmpegUtil.StartCreatingPalette(startTime, length, source, fps, width, true)) {
				proc1.WaitForExit();

				using (var proc2 = FfmpegUtil.StartCreatingAnimatedGif(startTime, length, source, true)) {
					proc2.WaitForExit();

					using (var proc3 = FfmpegUtil.StartCreatingThumbnail(startTime, length, source, true)) {
						proc3.WaitForExit();
					}
				}
			}

			startTime = TimeSpan.FromMilliseconds(30000);
			using (var proc1 = FfmpegUtil.StartCreatingPalette(startTime, length, source, fps, width, true)) {
				proc1.WaitForExit();

				using (var proc2 = FfmpegUtil.StartCreatingAnimatedGif(startTime, length, source, true)) {
					proc2.WaitForExit();

					using (var proc3 = FfmpegUtil.StartCreatingThumbnail(startTime, length, source, true)) {
						proc3.WaitForExit();
					}
				}
			}
		}
		#endregion Tests for FfmpegUtil

		#region Tests for ProcessUtil
		private static void Test_GetUpTime()
		{
			Console.Write("\nUp Time: {0}", ProcessUtil.GetUpTime());
		}

		private static void Test_RestartComputer()
		{
			ProcessUtil.RestartComputer();
			ProcessUtil.RestartComputer(force: false);
			ProcessUtil.RestartComputer(waitTime: 61);
			ProcessUtil.RestartComputer(remoteCompName: "laksjdf");
			ProcessUtil.RestartComputer(createWindow: true);
		}
		#endregion Tests for ProcessUtil

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