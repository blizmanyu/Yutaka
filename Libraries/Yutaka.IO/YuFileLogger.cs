using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Yutaka.IO
{
	public class YuFileLogger
	{
		#region Fields
		// Constants //
		const string EN_US = @"M/d/yyyy h:mmtt";
		const string TIMESTAMP = @"[HH:mm:ss] ";

		// Private //
		private DateTime startTime;
		private Stopwatch stopwatch;
		private StringBuilder log;
		private StringBuilder summary;

		// Public //
		public enum Mode { FullLog, ErrorsOnly, SummaryOnly };
		public Mode LogMode { get; set; }
		public string FileName { get; set; }
		#endregion

		#region Ctr
		public YuFileLogger(string filename = @"C:\Logs\log<TIMESTAMP>.txt", Mode mode = Mode.FullLog)
		{
			if (filename.Contains(@"C:\Logs\"))
				Directory.CreateDirectory(@"C:\Logs\");

			if (filename.Contains("<TIMESTAMP>")) {
				var timestamp = DateTime.Now.ToString("yyyy MMdd HHmm ssff");
				filename = filename.Replace("<TIMESTAMP>", timestamp);
			}

			FileName = filename;
			File.Create(FileName);
			stopwatch = new Stopwatch();
			LogMode = mode;
		}
		#endregion

		#region Methods
		public bool Start()
		{
			try {
				startTime = DateTime.Now;
				stopwatch.Start();
				log = new StringBuilder();
				summary = new StringBuilder();
				summary.AppendLine("======= Summary =======");
				summary.AppendFormat("Log started at: {0}", startTime.ToString(EN_US)).AppendLine();
				return true;
			}

			catch (Exception ex) {
				Console.Write("\nCould not start FileLogger: {0}\n{1}", ex.Message, ex.ToString());
				return false;
			}
		}

		public void Log(string msg)
		{
			try {
				var ts = stopwatch.Elapsed;
				var timestamp = String.Format("[{0:00}:{1:00}:{2:00}.{3:00}]", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
				using (var sw = File.AppendText(FileName)) {
					sw.WriteLine("{0} {1}", timestamp, msg);
				}
			}

			catch (Exception ex) {
				Console.Write("\nException thrown in Log('{0}')\n{1}\n{2}", msg, ex.Message, ex.ToString());
			}
		}

		public void Stop()
		{
			try {
				stopwatch.Stop();
				var ts = stopwatch.Elapsed;
				var timestamp = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
				summary.AppendFormat("This log was capturing for {0}", timestamp).AppendLine();
				summary.AppendLine("=======================");

				using (var sw = File.AppendText(FileName)) {
					sw.WriteLine("This log was capturing for {0}", timestamp);
				}
			}

			catch (Exception ex) {
				Console.Write("\nCould not start FileLogger: {0}\n{1}", ex.Message, ex.ToString());
			}
		}
		#endregion
	}
}