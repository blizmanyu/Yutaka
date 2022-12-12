using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using NLog;
using Yutaka.Core.IO;
using Yutaka.Core.Net;

namespace HtmlMaker
{
	class Program
	{
		private static readonly bool consoleOut = true; // true/false
		private static readonly bool ltr = true; // true/false
		private const string SOURCE = @"ASDFG\";
		private const string MAX_WIDTH = "600px";

		#region Fields
		#region Static Externs
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		const int SW_HIDE = 0;
		#endregion

		private const double errorPercentThreshold = 0.07;
		private const int CAPACITY = 100;
		private const int errorCountThreshold = 7;
		private const string FILENAME = "__.html";
		private const string ProgramName = "HtmlMaker";
		private const string TIMESTAMP = @"[HH:mm:ss] ";

		private static readonly DateTime startTime = DateTime.UtcNow;
		private static readonly Regex OneDigit = new Regex(@"^\d{1}$", RegexOptions.Compiled);
		private static readonly Regex TwoDigits = new Regex(@"^\d{2}$", RegexOptions.Compiled);
		private static readonly string fromEmail = "from@server.com";
		private static readonly string GmailPassword = "PASSWORD";
		private static readonly string GmailUsername = "USERNAME";
		private static readonly string toEmail = "to@server.com";

		private static int errorCount = 0;
		private static int processedCount = 0;
		private static int skippedCount = 0;
		private static int successCount = 0;
		private static int totalCount = 0;
		private static GmailSmtpClient _smtpClient = new GmailSmtpClient(GmailUsername, GmailPassword);
		private static List<object> TraverseTreeFailed = new List<object>();
		private static List<object> TraverseTreeSkipped = new List<object>();
		private static List<object> TraverseTreeSuccess = new List<object>();
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
			var sb = new StringBuilder();
			sb.AppendFormat("/** HTML General **/{0}", Environment.NewLine);
			sb.AppendFormat("body{{max-width:100%;overflow-x:hidden;background-color:#000;font:normal 1rem/1.5 Arial,Helvetica,sans-serif;color:#ccc}}{0}", Environment.NewLine);
			sb.AppendFormat("b,h1,h2,h3,h4,h5,h6,strong{{color:#fff}}{0}", Environment.NewLine);
			sb.AppendFormat("a{{color:inherit;cursor:pointer}}{0}", Environment.NewLine);
			sb.AppendFormat("a:hover{{color:#fff;text-decoration:underline}}{0}", Environment.NewLine);
			sb.AppendFormat("a img{{opacity:.99}}{0}", Environment.NewLine);
			sb.AppendFormat("img{{margin:1px auto;width:100%;max-width:{0};height:auto;display:inline-block;vertical-align:middle}}{1}", MAX_WIDTH, Environment.NewLine);
			//sb.AppendFormat("img{{max-width:100%;height:auto;display:inline-block;vertical-align:middle}}{0}", Environment.NewLine);
			//sb.AppendFormat("img{{max-width:100%;max-height:94vh;width:auto;display:inline-block;vertical-align:middle}}{0}", Environment.NewLine);
			sb.AppendFormat("table{{width:100%;border-collapse:collapse}}{0}", Environment.NewLine);
			sb.AppendFormat("input[type='tel'],input[type='text'],input[type='email'],input[type='password'],textarea,select{{height:2.5rem;border:1px solid #ddd;padding:8px;vertical-align:middle}}{0}", Environment.NewLine);
			sb.AppendFormat("input,textarea,select{{font-size:1.25rem;font-family:Arial,Helvetica,sans-serif;color:#777}}{0}", Environment.NewLine);
			sb.AppendFormat("textarea{{min-height:150px}}{0}", Environment.NewLine);
			sb.AppendFormat("select{{min-width:50px;height:2.5rem;padding:6px}}{0}", Environment.NewLine);
			sb.AppendFormat("input[type='tel']:focus,input[type='text']:focus,input[type='email']:focus,input[type='password']:focus,textarea:focus,select:focus{{border-color:#ccc;color:#444}}{0}", Environment.NewLine);
			sb.AppendFormat("input[type='checkbox'],input[type='radio'],input[type='checkbox'] + *,input[type='radio'] + *{{vertical-align:middle}}{0}", Environment.NewLine);
			sb.AppendFormat("input[type='button'],input[type='submit'],button,.button,.button-1,.button-2{{cursor:pointer}}{0}", Environment.NewLine);
			sb.AppendFormat("label,label + *{{vertical-align:middle}}{0}", Environment.NewLine);
			return sb.ToString();
		}

		private static string GetHeader()
		{
			var sb = new StringBuilder();

			if (ltr)
				sb.AppendFormat("<html>{0}", Environment.NewLine);
			else
				sb.AppendFormat("<html dir='rtl'>{0}", Environment.NewLine);

			sb.AppendFormat("<head>{0}", Environment.NewLine);
			sb.AppendFormat("<meta name='viewport' content='width=device-width, initial-scale=1'>{0}", Environment.NewLine);
			sb.AppendFormat("<style>{0}", Environment.NewLine);
			sb.AppendFormat("/** CSS RESET **/{0}", Environment.NewLine);
			sb.AppendFormat("*{{margin:0;outline:none;padding:0;text-decoration:none}}*,:before,:after{{-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box}}html{{margin:0;-webkit-text-size-adjust:none}}ol,ul{{list-style:none}}a img{{border:none}}a:active{{outline:none}}input[type='button']::-moz-focus-inner,input[type='submit']::-moz-focus-inner,input[type='reset']::-moz-focus-inner,input[type='file'] > input[type='button']::-moz-focus-inner{{margin:0;border:0;padding:0}}input[type='button'],input[type='submit'],input[type='reset'],input[type='tel'],input[type='text'],input[type='email'],input[type='password'],textarea{{border-radius:0}}input[type='button'],input[type='submit'],input[type='reset']{{-webkit-appearance:none}}input:-webkit-autofill{{-webkit-box-shadow:inset 0 0 0 1000px #fff}}script{{display:none!important}}{0}", Environment.NewLine);
			sb.AppendFormat(Environment.NewLine);
			sb.Append(GetHtmlGeneral());
			sb.AppendFormat(".video-container {{ height:90vh }}{0}", Environment.NewLine);
			sb.AppendFormat("video,iframe {{ width:100%;height:auto;max-height:100% }}{0}", Environment.NewLine);
			sb.AppendFormat("@media all and (min-width: 1024px) {{{0}", Environment.NewLine);
			//sb.AppendFormat("img {{ width:50% }}{0}", Environment.NewLine);
			sb.AppendFormat("}}{0}", Environment.NewLine);
			sb.AppendFormat("</style>{0}", Environment.NewLine);

			sb.AppendFormat("</head>{0}", Environment.NewLine);
			return sb.ToString();
		}

		private static string DirectoriesToHtml(string[] subDirs)
		{
			if (subDirs == null || subDirs.Length < 1)
				return "";

			var sb = new StringBuilder();
			sb.AppendFormat("<div style='margin:0 auto 1rem;padding:1rem;'>{0}", Environment.NewLine);

			foreach (var dir in subDirs)
				sb.AppendFormat("<a href=\"{1}/{2}\">/ {1} /</a><br/>{0}", Environment.NewLine, dir.Split('\\').Last(), FILENAME);

			sb.AppendFormat("</div>{0}", Environment.NewLine);
			return sb.ToString();
		}

		private static string FilesToHtml(string[] files)
		{
			if (files == null || files.Length < 1)
				return "";

			Match oneDigitMatch, twoDigitMatch;
			string filename, extension, filenameWithoutExtension;
			var sb = new StringBuilder();

			#region Videos
			sb.AppendFormat("<div style='margin:0 auto 1rem;font-size:0'>{0}", Environment.NewLine);

			foreach (var file in files) {
				if (FileUtil.IsVideoFile(file)) {
					filename = Path.GetFileName(file);
					filenameWithoutExtension = Path.GetFileNameWithoutExtension(file);
					extension = Path.GetExtension(file).Replace(".", "");

					if (OneDigit.Match(filenameWithoutExtension).Success)
						logger.Trace(" OneDigit:  {0}", filenameWithoutExtension);
					else if (TwoDigits.Match(filenameWithoutExtension).Success)
						logger.Trace("TwoDigits: {0}", filenameWithoutExtension);

					sb.AppendFormat("<div class='video-container'><video controls><source src=\"{1}\" type='video/{2}'></video></div>{0}", Environment.NewLine, filename, extension);
				}
			}

			sb.AppendFormat("</div>{0}", Environment.NewLine);
			#endregion

			#region Images
			sb.AppendFormat("<div style='margin:0 auto 1rem;font-size:0;text-align:center'>{0}", Environment.NewLine);

			foreach (var file in files) {
				if (FileUtil.IsImageFile(file)) {
					filename = Path.GetFileName(file);
					filenameWithoutExtension = Path.GetFileNameWithoutExtension(file);
					oneDigitMatch = OneDigit.Match(filenameWithoutExtension);
					twoDigitMatch = TwoDigits.Match(filenameWithoutExtension);

					if (oneDigitMatch.Success) {
						logger.Trace(" OneDigit:  {0}", filenameWithoutExtension);

					}
					else if (twoDigitMatch.Success)
						logger.Trace("TwoDigits: {0}", filenameWithoutExtension);

					sb.AppendFormat("<img src=\"{1}\" />{0}", Environment.NewLine, filename);
				}
			}

			sb.AppendFormat("</div>{0}", Environment.NewLine);
			#endregion

			return sb.ToString();
		}
		#endregion

		static void Main(string[] args)
		{
			StartProgram();

			try {
				TraverseTree(SOURCE);
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
		private static void TraverseTree(string root)
		{
			#region Trace then Check Input
			logger.Trace("Begin method TraverseTree(string root).");
			var log = "";

			if (root == null)
				log = String.Format("{0}'root' is null.{1}", log, Environment.NewLine);
			else if (String.IsNullOrWhiteSpace(root))
				log = String.Format("{0}'root' is empty.{1}", log, Environment.NewLine);
			else if (!Directory.Exists(root))
				log = String.Format("{0}root '{2}' doesn't exist.{1}", log, Environment.NewLine, root);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in TraverseTree(string root).{1}{1}", log, Environment.NewLine);
				logger.Error(log);

				if (consoleOut)
					Console.Write("\n{0}", log);

				return;
			}
			#endregion

			var dirs = new Stack<string>(CAPACITY);
			dirs.Push(root);
			string html, currentDir;
			string[] files, subDirs;

			while (dirs.Count > 0) {
				#region Html Header
				html = "";
				html = GetHeader();
				html = String.Format("{0}<body>{1}", html, Environment.NewLine);
				#endregion
				currentDir = dirs.Pop();

				try {
					subDirs = Directory.GetDirectories(currentDir);
					html = String.Format("{0}{1}", html, DirectoriesToHtml(subDirs));
				}

				catch (UnauthorizedAccessException e) {
					TraverseTreeSkipped.Add(e.Message);
					continue;
				}
				catch (DirectoryNotFoundException e) {
					TraverseTreeSkipped.Add(e.Message);
					continue;
				}

				try {
					files = Directory.GetFiles(currentDir);
					html = String.Format("{0}{1}", html, FilesToHtml(files));
				}

				catch (UnauthorizedAccessException e) {
					TraverseTreeSkipped.Add(e.Message);
					continue;
				}

				catch (DirectoryNotFoundException e) {
					TraverseTreeSkipped.Add(e.Message);
					continue;
				}

				foreach (var str in subDirs)
					dirs.Push(str);

				html = String.Format("{0}</body>{1}</html>", html, Environment.NewLine);
				var newPath = String.Format(@"{0}\{1}", currentDir, FILENAME);

				if (File.Exists(newPath))
					File.Delete(newPath);

				try {
					FileUtil.Write(html, newPath, false, Encoding.Unicode);
					TraverseTreeSuccess.Add(currentDir);
				}

				catch (Exception ex) {
					#region Log
					if (ex.InnerException == null)
						log = String.Format("{0}{1}", ex.Message, Environment.NewLine);
					else
						log = String.Format("{0}{1}", ex.InnerException.Message, Environment.NewLine);

					logger.Error(log);

					if (consoleOut)
						Console.Write("\n{0}", log);
					#endregion
					TraverseTreeFailed.Add(newPath);
				}
			}

			logger.Trace("End method TraverseTree(string root).{0}", Environment.NewLine);
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
			LogManager.Flush();

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