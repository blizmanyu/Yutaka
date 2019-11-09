using System;
using System.Runtime.InteropServices;
using System.Threading;

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

		#region Tests for FfmpegProcess
		private static void Test_CreateAnimatedGif()
		{
			try {
				var ffmpeg = new FfmpegProcess();
				//ffmpeg.CreateAnimatedGif();
				Thread.Sleep(1000);
			}

			catch (Exception ex) {
				var log = "";

				if (ex.InnerException == null)
					log = String.Format("{0}{2}{2}Exception thrown in Program.Test_CreateAnimatedGif(){2}{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					log = String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of Program.Test_CreateAnimatedGif(){2}{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);

				if (log.Contains("The system cannot find the file specified"))
					Console.Write("\nThe system can't find ffmpeg. Make sure it's installed on the system AND it's Path is in the Environment Variables.");
				else
					Console.Write("\n{0}", log);
			}


			// Define variables to track the peak
			// memory usage of the process.
			//long peakPagedMem = 0,
			//	 peakWorkingSet = 0,
			//	 peakVirtualMem = 0;
			//using (var ffmpeg = new FfmpegProcess()) {
			//	// Display the process statistics until
			//	// the user closes the program.
			//	do {
			//		if (!ffmpeg.HasExited) {
			//			// Refresh the current process property values.
			//			ffmpeg.Refresh();

			//			Console.WriteLine();

			//			// Display current process statistics.

			//			Console.WriteLine($"{ffmpeg} -");
			//			Console.WriteLine("-------------------------------------");

			//			Console.WriteLine($"  Physical memory usage     : {ffmpeg.WorkingSet64}");
			//			Console.WriteLine($"  Base priority             : {ffmpeg.BasePriority}");
			//			Console.WriteLine($"  Priority class            : {ffmpeg.PriorityClass}");
			//			Console.WriteLine($"  User processor time       : {ffmpeg.UserProcessorTime}");
			//			Console.WriteLine($"  Privileged processor time : {ffmpeg.PrivilegedProcessorTime}");
			//			Console.WriteLine($"  Total processor time      : {ffmpeg.TotalProcessorTime}");
			//			Console.WriteLine($"  Paged system memory size  : {ffmpeg.PagedSystemMemorySize64}");
			//			Console.WriteLine($"  Paged memory size         : {ffmpeg.PagedMemorySize64}");

			//			// Update the values for the overall peak memory statistics.
			//			peakPagedMem = ffmpeg.PeakPagedMemorySize64;
			//			peakVirtualMem = ffmpeg.PeakVirtualMemorySize64;
			//			peakWorkingSet = ffmpeg.PeakWorkingSet64;

			//			if (ffmpeg.Responding) {
			//				Console.WriteLine("Status = Running");
			//			}
			//			else {
			//				Console.WriteLine("Status = Not Responding");
			//			}
			//		}
			//	}
			//	while (!ffmpeg.WaitForExit(1000));


			//	Console.WriteLine();
			//	Console.WriteLine($"  Process exit code          : {ffmpeg.ExitCode}");

			//	// Display peak memory statistics for the process.
			//	Console.WriteLine($"  Peak physical memory usage : {peakWorkingSet}");
			//	Console.WriteLine($"  Peak paged memory usage    : {peakPagedMem}");
			//	Console.WriteLine($"  Peak virtual memory usage  : {peakVirtualMem}");
			//}
		}
		#endregion Tests for FfmpegProcess

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