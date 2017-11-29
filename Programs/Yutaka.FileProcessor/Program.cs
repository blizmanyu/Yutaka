using System;
using System.IO;
using System.Linq;
using NLog;
using Yutaka.IO;

namespace Yutaka.FileProcessor
{
	class Program
	{
		#region Config/Settings
		const string srcFolder = @"C:\";
		const string destFolder = @"C:\";
		const string destDrive = @"C:\";
		#endregion

		#region Fields
		private static DateTime startTime = DateTime.Now;
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static int totalCount = 0;
		private static int errorCount = 0;
		#endregion

		#region Private Helpers
		private static void Process()
		{
			var di = new DirectoryInfo(srcFolder);
			var files = di.EnumerateFiles("*", SearchOption.AllDirectories);
			totalCount = files.Count();

			foreach (var file in files) {
				try {
					FileUtil.CopyFile(file, String.Format(destFolder + file.Name), true, FileUtil.TimestampOption.PreserveOriginal);
				}

				catch (Exception ex) {
					errorCount++;
					logger.Error(ex.Message);
				}
			}
		}
		#endregion

		#region Methods
		static void Main(string[] args)
		{
			StartProgram();
			Process();
			EndProgram();
		}

		static void StartProgram()
		{
			logger.Trace("Starting program...");
		}

		static void EndProgram()
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
	}
}