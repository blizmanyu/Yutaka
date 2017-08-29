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

			foreach (var p in processes) {
				p.Kill();
				p.Close();
			}
		}
		#endregion

		#region Methods
		public static void EndProcessesByName(string processName, bool forceKill = true)
		{
			var processes = Process.GetProcessesByName(processName);

			if (processes == null || processes.Length < 1) {
				return;
			}

			foreach (var p in processes) {
				p.CloseMainWindow();
				p.Close();
			}

			Thread.Sleep(2000);

			if (forceKill)
				KillProcessesByName(processName);
		}

		public static void RestartComputer(string remoteCompName = null)
		{
			var args = "/r /f /t 30";

			if (remoteCompName != null)
				args = string.Format(@"{0} /m \\{1}", args, remoteCompName);

			var psi = new ProcessStartInfo("shutdown", args);
			psi.CreateNoWindow = true;
			psi.UseShellExecute = false;
			Process.Start(psi);
		}
		#endregion
	}
}