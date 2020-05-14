using System;
using System.Runtime.InteropServices;
using NLog;

namespace Yutaka.NewConsoleAppTemplate
{
	class Program
	{
		// Config/Settings //
		const string PROGRAM_NAME = "NewConsoleAppTemplate";
		private static bool consoleOut = true; // default = false //

		#region Fields
		#region Static Externs
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		const int SW_HIDE = 0;
		#endregion

		private static readonly DateTime startTime = DateTime.Now;
		private static readonly double errorPerThreshold = 0.07;
		private static readonly int errorCountThreshold = 7;
		private static readonly string TIMESTAMP = @"[HH:mm:ss] ";
		private static int errorCount = 0;
		private static int totalCount = 0;
		private static Logger logger = LogManager.GetCurrentClassLogger();
		#endregion

		static void Main(string[] args)
		{
			StartProgram();
			Process();
			EndProgram();
		}

		private static void Process()
		{

		}

		#region Methods

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
			var errorPer = (double) errorCount/totalCount;

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
			log[3] = String.Format("Errors: {0} ({1}){2}{2}", errorCount, errorPer.ToString("P"), Environment.NewLine);

			foreach (var l in log)
				logger.Info(l);

			if (consoleOut) {
				var timestamp = DateTime.Now.ToString(TIMESTAMP);
				Console.Write("\n");

				foreach (var l in log)
					Console.Write("\n{0}{1}", timestamp, l);

				Console.Write("\n.... Press any key to close the program ....");
				Console.ReadKey(true);
			}

			Environment.Exit(0); // in case you want to call this method outside of a standard successful program completion, this line will close the app //
		}
		#endregion StartProgram & EndProgram
		#endregion Methods
	}
}