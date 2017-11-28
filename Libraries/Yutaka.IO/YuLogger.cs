using System;
using System.Text;

namespace Yutaka.IO
{
	public class YuLogger
	{
		#region Fields
		// Constants //
		const string EN_US = @"M/d/yyyy h:mmtts";
		const string TIMESTAMP = @"[HH:mm:ss]";

		// PIVs //
		private DateTime StartTime;
		private StringBuilder Summary;
		private bool LogAll, LogErrors, ConsoleOut;
		private string FileName, FileNameErrors;
		#endregion

		#region Private Helpers
		#endregion

		#region Public Methods
		public YuLogger(string fileName="log <TIMESTAMP>.txt", bool logAll=true, bool logErrors=true, bool consoleOut=false)
		{
			StartTime = DateTime.Now;

			if (String.IsNullOrEmpty(fileName) || fileName == "log <TIMESTAMP>.txt")
				FileName = "log " + StartTime.ToString("yyyy MMdd HHmm ssff") + ".txt";
			else
				FileName = fileName;

			FileNameErrors = FileName.Replace(".txt", " Errors.txt");
			LogAll = logAll;
			LogErrors = logErrors;
			ConsoleOut = consoleOut;

			if (LogAll || LogErrors || ConsoleOut) {
				Summary = new StringBuilder();

			}
		}

		public void Log(string message)
		{
			if (String.IsNullOrEmpty(message))
				return;

			if (ConsoleOut)
				Console.WriteLine(message);

			if (LogAll || LogErrors) {
				var now = DateTime.Now;
				var temp = new StringBuilder();
				temp.Append(now.ToString(TIMESTAMP) + " " + message);
				// Write timestamp+message to file //
			}
		}

		public void Log(StringBuilder message)
		{
			if (message == null || String.IsNullOrEmpty(message.ToString()))
				return;

			Log(message.ToString());
		}

		public void End()
		{
			if (Summary == null || String.IsNullOrEmpty(Summary.ToString()))
				return;

			var endTime = DateTime.Now;
			var ts = endTime - StartTime;
		}
		#endregion
	}
}