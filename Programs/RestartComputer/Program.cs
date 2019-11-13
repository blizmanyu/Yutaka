using System;
using Yutaka.Diagnostics;

namespace RestartComputer
{
	class Program
	{
		private static string[] programs = new string[] { "Dynamics", "OUTLOOK", "Skype", };

		static void Main(string[] args)
		{
			if (ProcessUtil.GetUpTime() > ProcessUtil.FIVE_DAYS_IN_SECONDS) {
				var killCount = 0;
				var closeCount = 0;

				foreach (var program in programs) {
					if (ProcessUtil.CloseProgram(program) > 0) {
						Console.Write("\nClosed {0}", program);
						closeCount++;
					}
				}

				foreach (var program in programs) {
					if (ProcessUtil.KillProcess(program) > 0) {
						Console.Write("\nKilled {0}", program);
						killCount++;
					}
				}

				Console.Write("\nClosed {0} programs and killed {1}", closeCount, killCount);
				ProcessUtil.RestartComputer();
			}
		}
	}
}