﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Runtime.InteropServices;
using NLog;
using Yutaka.Core.Domain.Common;
using Yutaka.Core.IO;
using Yutaka.Core.Net;

namespace Yutaka.Core.Tests
{
	class Program
	{
		#region Fields
		#region Static Externs
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		const int SW_HIDE = 0;
		#endregion

		private static readonly bool consoleOut = true; // default = false //
		private static readonly DateTime startTime = DateTime.Now;
		private static readonly double errorPerThreshold = 0.07;
		private static readonly int errorCountThreshold = 7;
		private static readonly string ProgramName = "Yutaka.Core.Tests";
		private static readonly string TIMESTAMP = @"[HH:mm:ss] ";
		private static readonly string DcRcwIp = "192.168.4.6";
		private static readonly string BlackPantherIp = "192.168.4.28";
		private static readonly string WebHostIp = "69.16.238.147";

		private static int errorCount = 0;
		private static int processedCount = 0;
		private static int skippedCount = 0;
		private static int successCount = 0;
		private static int totalCount = 0;
		private static Logger logger = LogManager.GetCurrentClassLogger();
		#endregion

		static void Main(string[] args)
		{
			StartProgram();
			string log;

			try {
				Test_GetSystemTimeZones();
				//Test_PhoneUtil_Minify();
				//Test_PhoneUtil_Split();
			}

			catch (Exception ex) {
				++errorCount;
				#region Logging
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

		#region General
		private static void Test_GetSystemTimeZones()
		{
			Console.WriteLine();

			foreach (var v in TimeZoneInfo.GetSystemTimeZones())
				Console.Write("\n{0}", v.Id);

			Console.WriteLine();
		}

		private static void Test_DateTimeUtil_ToTemplate()
		{
			var dateTimeUtil = new DateTimeUtil();
			DateTime[] tests = {
				DateTime.Now,
				DateTime.Now.AddDays(-1),
				DateTime.Now.AddMonths(-1),
				DateTime.Now.AddYears(-1),
			};

			foreach (var test in tests) {
				Console.Write("\n");
				Console.Write("\n{0,2}) '{1}'", ++totalCount, test);
				Console.Write("\n{0}", DateTimeUtil.ToTemplate(test));
				Console.Write("\n");
			}
		}
		#endregion

		#region Core.Domain.Common Tests
		private static void Test_PhoneUtil_Beautify()
		{
			string[] tests = {
				null, "", " ",
				"asdfg",

				"1234567",
				"+1234567",
				"1234567ext.200",
				"+1234567ext.200",

				"1234567890",
				"+1234567890",
				"1234567890ext.200",
				"+1234567890ext.200",

				"12345678901",
				"+12345678901",
				"12345678901ext.200",
				"+12345678901ext.200",

				"123456789012",
				"+123456789012",
				"123456789012ext.200",
				"+123456789012ext.200",

				"1234567890123",
				"+1234567890123",
				"1234567890123ext.200",
				"+1234567890123ext.200",

				"12345678901234",
				"+12345678901234",
				"12345678901234ext.200",
				"+12345678901234ext.200",

				"123456789012345",
				"+123456789012345",
				"123456789012345ext.200",
				"+123456789012345ext.200",

				"1234567890123456",
				"+1234567890123456",
				"1234567890123456ext.200",
				"+1234567890123456ext.200",

				"12345678901234567",
				"+12345678901234567",
				"12345678901234567ext.200",
				"+12345678901234567ext.200",

				"123456789012345678",
				"+123456789012345678",
				"123456789012345678ext.200",
				"+123456789012345678ext.200",
			};

			foreach (var test in tests) {
				Console.Write("\n");
				Console.Write("\n{0,2}) '{1}'", ++totalCount, test ?? "NULL");
				Console.Write("\n  Minified: '{0}'", PhoneUtil.Minify(test));
				Console.Write("\nBeautified: '{0}'", PhoneUtil.Beautify(test));
				Console.Write("\n");
			}
		}

		private static void Test_PhoneUtil_Minify()
		{
			string[] tests = {
				null, "", " ",
				"asdfg",
				"(123) 456-7890",
				"1 (234) 567-8901",
				"123.456.7890",
				"1.234.567.8901",
				"+1 (234) 567-8901",
				"+123.456.7890",
				"+1.234.567.8901",
				"1 (234) 567-8901 ext. 200",
				"123.456.7890 ex. 210",
				"1.234.567.8901 x.220",
			};

			string[] split;

			foreach (var test in tests) {
				Console.Write("\n");
				Console.Write("\n{0,2}) '{1}'", ++totalCount, test ?? "NULL");
				split = PhoneUtil.Split(test);
				Console.Write("\nSplit: '{0}' || '{1}' || '{2}'", split[0], split[1], split[2]);
				Console.Write("\nMinified: '{0}'", PhoneUtil.Minify(test));
				Console.Write("\n");
			}
		}

		private static void Test_PhoneUtil_Split()
		{
			string[] tests = {
				null, "", " ",
				#region Tests starting with '+'
				"+",
				"+asdfg",
				"+9496791222",
				"+9496791222ext.200",
				"+9496791222ext200",
				"+9496791222ex.200",
				"+9496791222ex 200",
				"+9496791222xt.200",
				"+9496791222xt 200",
				"+9496791222 x.200",
				"+9496791222 x200",
				"+9496791222e.200",
				"+9496791222x.200",
				"+9496791222e200",
				"+9496791222x200",
				#endregion Tests starting with '+'
				#region Tests NOT starting with '+'
				"asdfg",
				"9496791222",
				"9496791222ext.200",
				"9496791222ext200",
				"9496791222ex.200",
				"9496791222ex 200",
				"9496791222xt.200",
				"9496791222xt 200",
				"9496791222 x.200",
				"9496791222 x200",
				"9496791222e.200",
				"9496791222x.200",
				"9496791222e200",
				"9496791222x200",
				#endregion Tests NOT starting with '+'
			};

			string[] split;

			foreach (var test in tests) {
				Console.Write("\n");
				Console.Write("\n{0,2}) '{1}'", ++totalCount, test ?? "NULL");
				split = PhoneUtil.Split(test);
				Console.Write("\n  '{0}' || '{1}' || '{2}'", split[0], split[1], split[2]);
				Console.Write("\n");
			}
		}
		#endregion Core.Domain.Common Tests

		#region Core.IO Tests
		// Modified Sep 30, 2020 // Created Sep 29, 2020 //
		private static void Test_EnumerateImageFiles()
		{
			var path = @"C:\";
			int i;
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			stopWatch.Stop();

			//foreach (var file in Directory.EnumerateFiles(path, pattern, SearchOption.AllDirectories))
			//	Console.Write("\n{0}) {1}", ++totalCount, file);

			i = 0;
			stopWatch.Restart();

			foreach (var file in DirectoryUtil.EnumerateImageFiles(path))
				Console.Write("\n{0}) {1}", ++totalCount, file);

			Console.Write("\nCount: {0}", i);
			stopWatch.Stop();
			Console.Write("\n{0:n0}ms", stopWatch.ElapsedMilliseconds);
		}

		// Modified Sep 30, 2020 // Created Sep 29, 2020 //
		private static void Test_EnumerateFiles()
		{
			var path = @"C:\";
			var pattern = "*";
			int i;
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			stopWatch.Stop();

			//foreach (var file in Directory.EnumerateFiles(path, pattern, SearchOption.AllDirectories))
			//	Console.Write("\n{0}) {1}", ++totalCount, file);

			i = 0;
			stopWatch.Restart();

			foreach (var file in DirectoryUtil.EnumerateFiles(path, pattern))
				++i;

			Console.Write("\nCount: {0}", i);
			stopWatch.Stop();
			Console.Write("\n{0:n0}ms", stopWatch.ElapsedMilliseconds);
		}
		#endregion Core.IO Tests

		#region Core.Net Tests
		// Modified Aug 3, 2021 // Created Aug 3, 2021 //
		private static void TestMailAddress()
		{
			string[] tests = {
				"  YBLIZMAN  @  RCW1.COM  ",
				"  YUTAKA BLIZMAN <YBLIZMAN  @  RCW1.COM>  ",
				"  USER  @HOST  ",
				"  \"DISPLAY NAME\" <  USER  @HOST>  ",
				"  \"DISPLAY NAME\"  USER  @HOST  ",
				"  DISPLAY NAME <  USER  @HOST>  ",
				"  \"USER  NAME\"  @HOST  ",
				"  USER...NAME..  @HOST  ",
				"  <  USER  @[MY DOMAIN]>  ",
				"  (COMMENT)\"DISPLAY NAME\"(COMMENT)<  (COMMENT)USER(COMMENT)  @(COMMENT)DOMAIN(COMMENT)>(COMMENT)  ",
			};

			Email email;
			MailAddress mailAddress;

			foreach (var test in tests) {
				mailAddress = new MailAddress(test);
				email = new Email(test);
				Console.Write("\n");
				Console.Write("\n{0}) '{1}'", ++totalCount, test);
				email.Debug();
				Console.Write("\nDisplayName: '{0}'", mailAddress.DisplayName);
				Console.Write("\n       User: '{0}'", mailAddress.User);
				Console.Write("\n       Host: '{0}'", mailAddress.Host);
				Console.Write("\n    Address: '{0}'", mailAddress.Address);
				Console.Write("\n   ToString: '{0}'", mailAddress);
				Console.Write("\n   ToString: '{0}'", mailAddress.ToString());
				Console.Write("\n");
			}
		}

		// Modified Jul 26, 2021 // Created Jul 26, 2021 //
		private static async void Test_ExampleImplementedClientAsync()
		{
			var client = new ExampleImplementedClient();
			Console.Write("\n\n{0}", await client.SendRequestToEndpoint());
		}

		// Modified Feb 8, 2021 // Created Feb 8, 2021 //
		private static void Test_PingUtil_ArePingable()
		{
			string[] tests = { DcRcwIp, BlackPantherIp, WebHostIp, /*"192.168.4.254",*/ };
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			stopWatch.Stop();
			stopWatch.Restart();
			Console.Write("\nArePingable: {0}", PingUtil.ArePingable(tests));
			stopWatch.Stop();
			Console.Write("\n{0:n0}ms", stopWatch.ElapsedMilliseconds);
		}
		#endregion Core.Net Tests

		#region StartProgram & EndProgram
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
			var endTime = DateTime.Now;
			var ts = endTime - startTime;
			var processedPer = (double) processedCount / totalCount;
			var skippedPer = (double) skippedCount / totalCount;
			var errorPer = processedCount > 0 ? (double) errorCount / processedCount : (double) errorCount / totalCount;
			var successPer = processedCount > 0 ? (double) successCount / processedCount : (double) successCount / totalCount;

			if (errorCount > errorCountThreshold || errorPer > errorPerThreshold) {
				logger.Error("The number of errors is above the threshold.");

				if (errorCount > errorCountThreshold && errorPer > errorPerThreshold) {
					//MailUtil.Send("fromEmail", "fromEmail", ProgramName, String.Format("Errors: {0} ({1})", errorCount, errorPer.ToString("P")));
				}
			}

			var log = new string[7];
			log[0] = "Ending program";
			log[1] = String.Format("It took {0} to complete", ts.ToString(@"hh\:mm\:ss\.fff"));
			log[2] = String.Format("Total: {0:n0}", totalCount);
			log[3] = String.Format("Processed: {0:n0} ({1})", processedCount, processedPer.ToString("p"), Environment.NewLine);
			log[4] = String.Format("Skipped: {0} ({1})", skippedCount, skippedPer.ToString("p"), Environment.NewLine);
			log[5] = String.Format("Success: {0} ({1})", successCount, successPer.ToString("p"), Environment.NewLine);
			log[6] = String.Format("Errors: {0} ({1}){2}{2}", errorCount, errorPer.ToString("p"), Environment.NewLine);

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
	}
}