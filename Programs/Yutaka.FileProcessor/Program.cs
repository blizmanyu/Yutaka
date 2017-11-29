using System;
using NLog;

namespace Yutaka.FileProcessor
{
	class Program
	{
		#region Config/Settings
		#endregion

		#region Fields
		private static DateTime startTime = DateTime.Now;
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static int totalCount = 0;
		private static int errorCount = 0;
		#endregion

		static void Main(string[] args)
		{
			StartProgram();
			Process();
			EndProgram();
		}

		#region Private Helpers
		private static void Process()
		{
			logger.Trace("Sample trace message");
		}

		#region Start & EndProgram
		private static void StartProgram()
		{
			logger.Trace("Starting program...");
		}

		private static void EndProgram()
		{
			var endTime = DateTime.Now;
			var ts = endTime - startTime;
			var errorPercent = 0.0;

			if (errorCount > 0 && totalCount > 0)
				errorPercent = (double) errorCount / totalCount;

			logger.Trace("=====================================");
			logger.Trace("Program took {0} to complete", ts.ToString(@"hh\:mm\:ss\.fff"));
			logger.Trace("Total Count: {0}", totalCount);
			logger.Trace("Error Count: {0}", errorCount);
			if (errorPercent > 0)
				logger.Trace("Error Percent: {0:p1}", errorPercent);
			logger.Trace("====================================={0}", Environment.NewLine);
		}
		#endregion
		#endregion
	}
}