using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Yutaka.Diagnostics
{
	public static class ProcessHelper
	{
		#region DLL Imports
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
			string lpszWindow);

		[DllImport("user32.dll")]
		public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
		#endregion DLL Imports

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

		private static void RefreshTrayArea(IntPtr windowHandle)
		{
			const uint wmMousemove = 0x0200;
			RECT rect;
			GetClientRect(windowHandle, out rect);
			for (var x = 0; x < rect.right; x += 5)
				for (var y = 0; y < rect.bottom; y += 5)
					SendMessage(windowHandle, wmMousemove, 0, (y << 16) + x);
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

		public static void RefreshTrayArea()
		{
			IntPtr systemTrayContainerHandle = FindWindow("Shell_TrayWnd", null);
			IntPtr systemTrayHandle = FindWindowEx(systemTrayContainerHandle, IntPtr.Zero, "TrayNotifyWnd", null);
			IntPtr sysPagerHandle = FindWindowEx(systemTrayHandle, IntPtr.Zero, "SysPager", null);
			IntPtr notificationAreaHandle = FindWindowEx(sysPagerHandle, IntPtr.Zero, "ToolbarWindow32", "Notification Area");
			if (notificationAreaHandle == IntPtr.Zero) {
				notificationAreaHandle = FindWindowEx(sysPagerHandle, IntPtr.Zero, "ToolbarWindow32",
					"User Promoted Notification Area");
				IntPtr notifyIconOverflowWindowHandle = FindWindow("NotifyIconOverflowWindow", null);
				IntPtr overflowNotificationAreaHandle = FindWindowEx(notifyIconOverflowWindowHandle, IntPtr.Zero,
			"ToolbarWindow32", "Overflow Notification Area");
				RefreshTrayArea(overflowNotificationAreaHandle);
			}
			RefreshTrayArea(notificationAreaHandle);
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

		public static int StartProcess(string fileName, string args=null, bool redirectStandardOutput = true, bool redirectStandardError = true, bool useShellExecute = false, bool createNoWindow = true)
		{
			if (String.IsNullOrWhiteSpace(fileName))
				throw new Exception(String.Format("<fileName> is required.{0}Exception thrown in ProcessHelper.StartProcess(string fileName, string args, bool redirectStandardOutput, bool redirectStandardError, bool useShellExecute, bool createNoWindow).{0}{0}", Environment.NewLine));

			try {
				using (var p = new Process()) {
					p.StartInfo.FileName = fileName;
					p.StartInfo.Arguments = args;
					p.StartInfo.RedirectStandardOutput = redirectStandardOutput;
					p.StartInfo.RedirectStandardError = redirectStandardError;
					p.StartInfo.UseShellExecute = useShellExecute;
					p.StartInfo.CreateNoWindow = createNoWindow;
					//Console.Write("\nParent Name: {0}", Directory.GetParent(source).Name);
					//Console.Write("\nParent FullName: {0}", Directory.GetParent(source).FullName);
					//p.StartInfo.WorkingDirectory = Directory.GetParent(source).FullName;
					p.Start();
					p.WaitForExit();

					try {
						return p.ExitCode;
					}
					catch (Exception) {
						return -1;
					}
				}
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in ProcessHelper.StartProcess(string fileName='{3}', string args'{4}', bool redirectStandardOutput, bool redirectStandardError, bool useShellExecute, bool createNoWindow).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, fileName, args));

				throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of ProcessHelper.StartProcess(string fileName='{3}', string args'{4}', bool redirectStandardOutput, bool redirectStandardError, bool useShellExecute, bool createNoWindow).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, fileName, args));
			}
		}
		#endregion
	}
}