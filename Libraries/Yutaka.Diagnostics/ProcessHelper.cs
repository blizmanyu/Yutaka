using System.Diagnostics;
using System.Threading;

namespace Yutaka.Diagnostics
{
	public static class ProcessHelper
	{
		#region Private Helpers
		private static void KillProcessesByName(string processName)
		{
			var processes = Process.GetProcessesByName(processName);

			if (processes == null || processes.Length < 1)
				return;

			for (int i = 0; i < processes.Length; i++) {
				processes[i].Kill();
				processes[i].Close();
			}
		}
		#endregion

		#region Methods
		public static void EndProcessesByName(string processName, bool forceKill = true)
		{
			var processes = Process.GetProcessesByName(processName);

			if (processes == null || processes.Length < 1)
				return;

			for (int i = 0; i < processes.Length; i++) {
				processes[i].CloseMainWindow();
				processes[i].Close();
			}

			Thread.Sleep(2200);

			if (forceKill)
				KillProcessesByName(processName);
		}

		public static void RestartComputer(string remoteCompName = null)
		{
			var args = "/r /f /t 30";

			if (!string.IsNullOrEmpty(remoteCompName))
				args = string.Format(@"{0} /m \\{1}", args, remoteCompName);

			var psi = new ProcessStartInfo("shutdown", args);
			psi.CreateNoWindow = true;
			psi.UseShellExecute = false;
			Process.Start(psi);
		}

		public static void RestartComputer(bool force = true, int waitTime = 30, string remoteCompName = null)
		{
			var args = "/r ";

			if (force)
				args += "/f ";
			if (waitTime > 0)
				args += string.Format("/t {0} ", waitTime);
			if (!string.IsNullOrEmpty(remoteCompName))
				args += string.Format(@"/m \\{1}", remoteCompName);

			var psi = new ProcessStartInfo("shutdown", args);
			psi.CreateNoWindow = true;
			psi.UseShellExecute = false;
			Process.Start(psi);
		}
		#endregion
	}
}