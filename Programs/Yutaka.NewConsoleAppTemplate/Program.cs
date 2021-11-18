using System;
using System.Runtime.InteropServices;
using NLog;
using Yutaka.Core.Net;

namespace Yutaka.NewConsoleAppTemplate
{
	class Program
	{
		private static readonly bool consoleOut = true; // default = false //
		private static readonly string ProgramName = "NewConsoleAppTemplate";

		#region Fields
		#region Static Externs
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		const int SW_HIDE = 0;
		#endregion

		private static readonly DateTime startTime = DateTime.UtcNow;
		private static readonly double errorPercThreshold = 0.07;
		private static readonly int errorCountThreshold = 7;
		private static readonly string TIMESTAMP = @"[HH:mm:ss] ";
		private static readonly string fromEmail = "from@server.com";
		private static readonly string toEmail = "to@server.com";

		private static int errorCount = 0;
		private static int processedCount = 0;
		private static int skippedCount = 0;
		private static int successCount = 0;
		private static int totalCount = 0;
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static GmailSmtpClient client = new GmailSmtpClient();
		#endregion

		static void Main(string[] args)
		{
			StartProgram();

			try {
				Process();
			}

			catch (Exception ex) {
				++errorCount;
				#region Logging
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in Main().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Main().{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);

				if (consoleOut)
					Console.Write("\n{0}", log);

				logger.Error(log);
				#endregion
			}

			EndProgram();
		}

		private static void Process()
		{
			try {
				// TODO: Add logic here //

				++processedCount;
			}

			catch (Exception ex) {
				#region Log
				++errorCount;
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in Process().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Process().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);

				logger.Error(log);

				if (consoleOut)
					Console.Write("\n{0}", log);
				#endregion Log
			}
		}

		#region Methods

		#region StartProgram & EndProgram
		private static void HandleArgs(string[] args)
		{
			if (args == null || args.Length < 1) {
				; // if you want empty arguments to actually set default args, set them here.
			}

			else {
				var temp = String.Join(" ", args);

				if (temp.IndexOf("ASDFG", StringComparison.OrdinalIgnoreCase) > -1) {
					; // handle args like this
				}
			}
		}

		private static void StartProgram()
		{
			var log = String.Format("Starting {0} program", ProgramName);
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
			var endTime = DateTime.UtcNow;
			var ts = endTime - startTime;
			var processedPerc = (double) processedCount / totalCount;
			var skippedPerc = (double) skippedCount / totalCount;
			var errorPerc = processedCount > 0 ? (double) errorCount / processedCount : (double) errorCount / totalCount;
			var successPerc = processedCount > 0 ? (double) successCount / processedCount : (double) successCount / totalCount;

			if (errorCount > errorCountThreshold || errorPerc > errorPercThreshold) {
				logger.Error("The number of errors is above the threshold.");

				if (errorCount > errorCountThreshold && errorPerc > errorPercThreshold)
					client.TrySend(fromEmail, toEmail, ProgramName, String.Format("Errors: {0} ({1})", errorCount, errorPerc.ToString("p")), out var response);
			}

			var log = new string[7];
			log[0] = "Ending program";

			if (ts.TotalHours > 1)
				log[1] = String.Format("It took {0} to complete", ts.ToString(@"hh\:mm"));
			else if (ts.TotalMinutes > 1)
				log[1] = String.Format("It took {0} to complete", ts.ToString(@"mm\:ss"));
			else
				log[1] = String.Format("It took {0} to complete", ts.ToString(@"ss\.fff"));

			log[2] = String.Format("Total: {0:n0}", totalCount);

			if (totalCount > 0) {
				log[3] = String.Format("Processed: {0:n0} ({1})", processedCount, processedPerc.ToString("p").Replace(" ", ""));
				log[4] = String.Format("Skipped: {0} ({1})", skippedCount, skippedPerc.ToString("p").Replace(" ", ""));
				log[5] = String.Format("Success: {0} ({1})", successCount, successPerc.ToString("p").Replace(" ", ""));
				log[6] = String.Format("Errors: {0} ({1})", errorCount, errorPerc.ToString("p").Replace(" ", ""));
			}

			foreach (var l in log)
				logger.Info(l);

			logger.Info(Environment.NewLine + Environment.NewLine);

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