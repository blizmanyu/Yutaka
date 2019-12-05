using System;
using System.Threading;
using Yutaka.Diagnostics;

namespace RestartComputer
{
	class Program
	{
		private static readonly string[] programs = new string[] { "Dynamics", "OUTLOOK", "Skype", };

		static void Main(string[] args)
		{
			if (DateTime.Now.Month == 12 && DateTime.Now.Day == 31)
				return; // don't run on New Years Eve //

			if (ProcessUtil.GetUpTime() > ProcessUtil.FIVE_DAYS_IN_SECONDS) {
				var killCount = 0;
				var closeCount = 0;

				foreach (var program in programs) {
					try {
						closeCount += ProcessUtil.CloseProgram(program);
					}

					catch (Exception) { }
				}

				if (closeCount > 0) {
					ProcessUtil.RefreshTrayArea();
					Thread.Sleep(ProcessUtil.DEFAULT_SLEEP_TIME);
				}

				foreach (var program in programs) {
					try {
						killCount += ProcessUtil.KillProcess(program);
					}

					catch (Exception) { }
				}

				if (killCount > 0) {
					ProcessUtil.RefreshTrayArea();
					Thread.Sleep(ProcessUtil.DEFAULT_SLEEP_TIME);
				}

				Console.Write("\nClosed {0} programs and killed {1}", closeCount, killCount);
				ProcessUtil.RestartComputer();
			}
		}
	}
}