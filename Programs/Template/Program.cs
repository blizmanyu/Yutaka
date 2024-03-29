﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NLog;
using Yutaka.Core.Net;

namespace Template
{
	class Program
	{
		// Config/Settings //
		private const string EMAIL_TO = "to@server.com";
		private static readonly bool consoleOut = true; // true/false

		#region Fields
		// Constants & Readonlys //
		private const int ERROR_COUNT_THRESHOLD = 7;
		private const double ERROR_PERCENT_THRESHOLD = 0.07;
		private const string EMAIL_FROM = "from@server.com";
		private const string GMAIL_PASSWORD = "PASSWORD";
		private const string GMAIL_USERNAME = "USERNAME";
		private const string PROGRAM_NAME = "REPLACE_THIS";
		private const string TIMESTAMP = @"[HH:mm:ss] ";
		private static readonly DateTime startTime = DateTime.UtcNow;

		// Counters //
		private static int errorCount = 0;
		private static int processedCount = 0;
		private static int skippedCount = 0;
		private static int successCount = 0;
		private static int totalCount = 0;

		// PIVs //
		private static GmailSmtpClient _smtpClient = new GmailSmtpClient(GMAIL_USERNAME, GMAIL_PASSWORD);
		private static List<object> Step1Failed = new List<object>();
		private static List<object> Step1Skipped = new List<object>();
		private static List<object> Step1Success = new List<object>();
		private static List<object> Step2Failed = new List<object>();
		private static List<object> Step2Skipped = new List<object>();
		private static List<object> Step2Success = new List<object>();
		private static List<object> Step3Failed = new List<object>();
		private static List<object> Step3Skipped = new List<object>();
		private static List<object> Step3Success = new List<object>();
		private static Logger logger = LogManager.GetCurrentClassLogger();

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

			try {
				Step1();
				Step2();
				Step3();
				EndProgram(0);
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
				EndProgram(1);
			}
		}

		#region Methods
		private static void Step1()
		{
			#region Trace then Check Input
			logger.Trace("Begin method Step1().");
			var log = "";

			// Example only //
			//if (str == null)
			//	log = String.Format("{0}'str' is null.{1}", log, Environment.NewLine);
			//else if (String.IsNullOrWhiteSpace(str))
			//	log = String.Format("{0}'str' is empty.{1}", log, Environment.NewLine);
			//else
			//	str = str.Trim();

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in Step1().{1}{1}", log, Environment.NewLine);
				logger.Error(log);

				if (consoleOut)
					Console.Write("\n{0}", log);

				return;
			}
			#endregion

			var list = new List<object>();

			foreach (var item in list) {
				try {
					if (item != item) {
						Step1Skipped.Add(item);
						continue;
					}

					Step1Success.Add(item);
				}

				catch (Exception ex) {
					Step1Failed.Add(item);
					#region Logging
					if (ex.InnerException == null)
						log = String.Format("{0}{2}Exception thrown in Step1().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
					else
						log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Step1().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);

					logger.Error(log);

					if (consoleOut)
						Console.Write("\n{0}", log);
					#endregion
				}
			}

			logger.Trace("End method Step1().{0}", Environment.NewLine);
		}

		private static void Step2()
		{
			#region Trace then Check Input
			logger.Trace("Begin method Step2().");
			var log = "";

			// Example only //
			//if (str == null)
			//	log = String.Format("{0}'str' is null.{1}", log, Environment.NewLine);
			//else if (String.IsNullOrWhiteSpace(str))
			//	log = String.Format("{0}'str' is empty.{1}", log, Environment.NewLine);
			//else
			//	str = str.Trim();

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in Step2().{1}{1}", log, Environment.NewLine);
				logger.Error(log);

				if (consoleOut)
					Console.Write("\n{0}", log);

				return;
			}
			#endregion

			var list = new List<object>();

			foreach (var item in list) {
				try {
					if (item != item) {
						Step2Skipped.Add(item);
						continue;
					}

					Step2Success.Add(item);
				}

				catch (Exception ex) {
					Step2Failed.Add(item);
					#region Logging
					if (ex.InnerException == null)
						log = String.Format("{0}{2}Exception thrown in Step2().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
					else
						log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Step2().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);

					logger.Error(log);

					if (consoleOut)
						Console.Write("\n{0}", log);
					#endregion
				}
			}

			logger.Trace("End method Step2().{0}", Environment.NewLine);
		}

		private static void Step3()
		{
			#region Trace then Check Input
			logger.Trace("Begin method Step3().");
			var log = "";

			// Example only //
			//if (str == null)
			//	log = String.Format("{0}'str' is null.{1}", log, Environment.NewLine);
			//else if (String.IsNullOrWhiteSpace(str))
			//	log = String.Format("{0}'str' is empty.{1}", log, Environment.NewLine);
			//else
			//	str = str.Trim();

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in Step3().{1}{1}", log, Environment.NewLine);
				logger.Error(log);

				if (consoleOut)
					Console.Write("\n{0}", log);

				return;
			}
			#endregion

			var list = new List<object>();

			foreach (var item in list) {
				try {
					if (item != item) {
						Step3Skipped.Add(item);
						continue;
					}

					Step3Success.Add(item);
				}

				catch (Exception ex) {
					Step3Failed.Add(item);
					#region Logging
					if (ex.InnerException == null)
						log = String.Format("{0}{2}Exception thrown in Step3().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
					else
						log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Step3().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);

					logger.Error(log);

					if (consoleOut)
						Console.Write("\n{0}", log);
					#endregion
				}
			}

			logger.Trace("End method Step3().{0}", Environment.NewLine);
		}

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

		/// <summary>
		/// Ends the program and returns an exit code to the operating system.
		/// </summary>
		/// <param name="exitCode">The exit code to return to the operating system. Use 0 (zero) to indicate that the process completed successfully.</param>
		private static void EndProgram(int exitCode)
		{
			var endTime = DateTime.UtcNow;
			var ts = endTime - startTime;
			var processedPerc = (double) processedCount / totalCount;
			var skippedPerc = (double) skippedCount / totalCount;
			var errorPerc = processedCount > 0 ? (double) errorCount / processedCount : (double) errorCount / totalCount;
			var successPerc = processedCount > 0 ? (double) successCount / processedCount : (double) successCount / totalCount;

			if (errorCount > ERROR_COUNT_THRESHOLD && errorPerc > ERROR_PERCENT_THRESHOLD)
				_smtpClient.TrySend(EMAIL_FROM, EMAIL_TO, PROGRAM_NAME, String.Format("Errors: {0} ({1})", errorCount, errorPerc.ToString("p")), out var response);

			var log = new string[7];
			log[0] = "Ending program";

			if (ts.TotalSeconds < 61)
				log[1] = String.Format("It took {0} sec to complete", ts.ToString(@"s\.fff"));
			else if (ts.TotalMinutes < 61)
				log[1] = String.Format("It took {0}m {1}s to complete", ts.Minutes, ts.Seconds);
			else
				log[1] = String.Format("It took {0}h {1}m to complete", ts.Hours, ts.Minutes);

			log[2] = String.Format("    Total: {0,5:n0}", totalCount);

			if (totalCount > 0) {
				log[3] = String.Format("Processed: {0,5:n0} ({1,7})", processedCount, processedPerc.ToString("p").Replace(" ", ""));
				log[4] = String.Format("  Skipped: {0,5:n0} ({1,7})", skippedCount, skippedPerc.ToString("p").Replace(" ", ""));
				log[5] = String.Format("  Success: {0,5:n0} ({1,7})", successCount, successPerc.ToString("p").Replace(" ", ""));
				log[6] = String.Format("   Errors: {0,5:n0} ({1,7})", errorCount, errorPerc.ToString("p").Replace(" ", ""));
			}

			foreach (var l in log)
				logger.Info(l);

			logger.Info(Environment.NewLine + Environment.NewLine);

			if (consoleOut) {
				var timestamp = DateTime.Now.ToString(TIMESTAMP);
				Console.Write("\n");

				foreach (var l in log)
					Console.Write("\n{0}{1}", timestamp, l);

				Console.Write("\n\n. . . Press any key to close the program . . .\n\n");
				Console.ReadKey(true);
			}

			Environment.Exit(exitCode);
		}
		#endregion
		#endregion
	}
}