using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Yutaka.FileProcessor
{
	class Program
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		static void Main(string[] args)
		{
			logger.Trace("Sample trace message");
			logger.Debug("Sample debug message");
			logger.Info("Sample informational message");
			logger.Warn("Sample warning message");
			logger.Error("Sample error message");
			logger.Fatal("Sample fatal error message");

			// alternatively you can call the Log() method
			// and pass log level as the parameter.
			logger.Log(LogLevel.Info, "Sample informational message");
		}
	}
}