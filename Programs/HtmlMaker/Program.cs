using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using NLog;
using Yutaka.Core.Net;
using Yutaka.IO;

namespace HtmlMaker
{
	class Program
	{
		private const string ProgramName = "HtmlMaker";
		private const string SOURCE = @"ASDFG\";
		private static readonly bool consoleOut = true; // true/false
		private static readonly string GmailPassword = "PASSWORD";
		private static readonly string GmailUsername = "USERNAME";
		private static readonly string fromEmail = "from@server.com";
		private static readonly string toEmail = "to@server.com";

		#region Fields
		#region Static Externs
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		const int SW_HIDE = 0;
		#endregion

		private const double errorPercentThreshold = 0.07;
		private const int errorCountThreshold = 7;
		private const string TIMESTAMP = @"[HH:mm:ss] ";
		private const string CSS_RESET = "*{margin:0;outline:none;padding:0;text-decoration:none}*,:before,:after{-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box}html{margin:0;-webkit-text-size-adjust:none}ol,ul{list-style:none}a img{border:none}a:active{outline:none}input[type='button']::-moz-focus-inner,input[type='submit']::-moz-focus-inner,input[type='reset']::-moz-focus-inner,input[type='file'] > input[type='button']::-moz-focus-inner{margin:0;border:0;padding:0}input[type='button'],input[type='submit'],input[type='reset'],input[type='tel'],input[type='text'],input[type='email'],input[type='password'],textarea{border-radius:0}input[type='button'],input[type='submit'],input[type='reset']{-webkit-appearance:none}input:-webkit-autofill{-webkit-box-shadow:inset 0 0 0 1000px #fff}script{display:none!important}";
		private static readonly DateTime startTime = DateTime.UtcNow;

		private static int errorCount = 0;
		private static int processedCount = 0;
		private static int skippedCount = 0;
		private static int successCount = 0;
		private static int totalCount = 0;
		private static FileUtil _fileUtil = new FileUtil();
		private static GmailSmtpClient _smtpClient = new GmailSmtpClient(GmailUsername, GmailPassword);
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
		#endregion

		#region Utilities
		//private static string EnumerateFilesToHtml(string path)
		//{
		//	var videos = "<div style='margin:1rem auto;'>";
		//	var images = "<div style='margin:1rem auto;'>";

		//	foreach (var file in path) {
		//		if (Yutaka.Core.IO.FileUtil.is)
		//	}
		//}

		private static string GetHtmlGeneral()
		{
			return String.Format(
				"\t/** HTML General **/{0}" +
				"\tbody{{max-width:100%;overflow-x:hidden;background-color:#000;font:normal 1rem/1.5 Arial,Helvetica,sans-serif;color:#ccc}}{0}" +
				"\tb,h1,h2,h3,h4,h5,h6,strong{{color:#fff}}{0}" +
				"\ta{{color:inherit;cursor:pointer}}{0}" +
				"\ta:hover{{color:#fff;text-decoration:underline}}{0}" +
				"\ta img{{opacity:.99}}{0}" +
				"\timg{{max-width:100%;height:auto}}{0}" +
				"\ttable{{width:100%;border-collapse:collapse}}{0}" +
				"\tinput[type='tel'],input[type='text'],input[type='email'],input[type='password'],textarea,select{{height:2.5rem;border:1px solid #ddd;padding:8px;vertical-align:middle}}{0}" +
				"\tinput,textarea,select{{font-size:1.25rem;font-family:Arial,Helvetica,sans-serif;color:#777}}{0}" +
				"\ttextarea{{min-height:150px}}{0}" +
				"\tselect{{min-width:50px;height:2.5rem;padding:6px}}{0}" +
				"\tinput[type='tel']:focus,input[type='text']:focus,input[type='email']:focus,input[type='password']:focus,textarea:focus,select:focus{{border-color:#ccc;color:#444}}{0}" +
				"\tinput[type='checkbox'],input[type='radio'],input[type='checkbox'] + *,input[type='radio'] + *{{vertical-align:middle}}{0}" +
				"\tinput[type='button'],input[type='submit'],button,.button-1,.button-2{{cursor:pointer}}{0}" +
				"\tlabel,label + *{{vertical-align:middle}}", Environment.NewLine);
		}
		#endregion

		static void Main(string[] args)
		{
			StartProgram();

			try {
				Step1(SOURCE);
				Step2();
				Step3();
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

			EndProgram(0);
		}

		#region Methods
		private static void Step1(string root)
		{
			#region Trace then Check Input
			logger.Trace("Begin method Step1(string root).");
			var log = "";

			if (root == null)
				log = String.Format("{0}'root' is null.{1}", log, Environment.NewLine);
			else if (String.IsNullOrWhiteSpace(root))
				log = String.Format("{0}'root' is empty.{1}", log, Environment.NewLine);
			else if (!Directory.Exists(root))
				log = String.Format("{0}Directory '{2}' doesn't exist.{1}", log, Environment.NewLine, root);
			else
				root = root.Trim();

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in Step1(string root).{1}{1}", log, Environment.NewLine);
				logger.Error(log);

				if (consoleOut)
					Console.Write("\n{0}", log);

				return;
			}
			#endregion

			var html = "";
			html = String.Format("{0}<meta name='viewport' content='width=device-width, initial-scale=1'>{1}", html, Environment.NewLine);
			html = String.Format("{0}<style>{1}", html, Environment.NewLine);
			html = String.Format("{0}\t/** CSS RESET **/{1}", html, Environment.NewLine);
			html = String.Format("{0}\t{2}{1}", html, Environment.NewLine, CSS_RESET);
			html = String.Format("{0}{1}", html, Environment.NewLine);
			html = String.Format("{0}{2}{1}", html, Environment.NewLine, GetHtmlGeneral());
			html = String.Format("{0}</style>{1}", html, Environment.NewLine);
			html = String.Format("{0}<body>{1}", html, Environment.NewLine);
			html = String.Format("{0}\t<div style='margin:1rem auto;padding:1rem'>{1}", html, Environment.NewLine);

			foreach (var dir in Directory.EnumerateDirectories(root)) {
				try {
					html = String.Format("{0}\t\t<div><a href='{3}'>{2}</a></div>{1}", html, Environment.NewLine, dir, dir.Replace(root, ""));
					Step1Success.Add(dir);
				}

				catch (Exception ex) {
					Step1Failed.Add(dir);
					#region Logging
					if (ex.InnerException == null)
						log = String.Format("{0}{2}Exception thrown in Step1(string root).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
					else
						log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Step1(string root).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);

					logger.Error(log);

					if (consoleOut)
						Console.Write("\n{0}", log);
					#endregion
				}
			}

			html = String.Format("{0}\t</div>{1}", html, Environment.NewLine);
			html = String.Format("{0}</body>{1}", html, Environment.NewLine);






			var newPath = String.Format("{0}__.html", root);

			if (File.Exists(newPath))
				File.Delete(newPath);

			_fileUtil.Write(html, newPath, false, Encoding.Unicode);
			logger.Trace("End method Step1(string root).{0}", Environment.NewLine);
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

			if (errorCount > errorCountThreshold && errorPerc > errorPercentThreshold)
				_smtpClient.TrySend(fromEmail, toEmail, ProgramName, String.Format("Errors: {0} ({1})", errorCount, errorPerc.ToString("p")), out var response);

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

			if (exitCode != 0)
				Environment.Exit(exitCode);
		}
		#endregion
		#endregion
	}
}