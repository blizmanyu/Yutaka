﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Yutaka.Diagnostics
{
	public static class ProcessUtil
	{
		#region Fields
		const uint wmMousemove = 0x0200;
		public const int ONE_DAY_IN_SECONDS = 86400;
		public const int TWO_DAYS_IN_SECONDS = 172800;
		public const int THREE_DAYS_IN_SECONDS = 259200;
		public const int FOUR_DAYS_IN_SECONDS = 345600;
		public const int FIVE_DAYS_IN_SECONDS = 432000;
		public const int TWENTY_ONE_HOURS = 75600;
		public const int DEFAULT_SLEEPTIME = 2500;

		[StructLayout(LayoutKind.Sequential)]
		private struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}
		#endregion Fields

		#region Utilities
		#region DLL Imports
		[DllImport("user32.dll")]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll")]
		private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
			string lpszWindow);

		[DllImport("user32.dll")]
		private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
		#endregion DLL Imports

		/// <summary>
		/// Frees all the resources that are associated with these components.
		/// </summary>
		/// <param name="processes">The list of <see cref="Process"/>es to free.</param>
		private static void FreeResources(List<Process> processes)
		{
			if (processes == null || processes.Count < 1)
				return;

			foreach (var process in processes) {
				try {
					process.Close();
				}
				catch { }
			}
		}

		private static void RefreshTrayArea(IntPtr windowHandle)
		{
			GetClientRect(windowHandle, out var rect);
			for (var x = 0; x < rect.right; x += 5)
				for (var y = 0; y < rect.bottom; y += 5)
					SendMessage(windowHandle, wmMousemove, 0, (y << 16) + x);
		}
		#endregion Utilities

		#region Public Methods
		/// <summary>
		/// Closes a process that has a user interface by sending a close message to its main window, then frees all the resources that are associated with this component.
		/// </summary>
		/// <param name="programName">The friendly name of the process.</param>
		/// <returns>The number of processes closed.</returns>
		public static int CloseProgram(string programName)
		{
			if (String.IsNullOrWhiteSpace(programName))
				return 0;

			var count = 0;
			var processes = new List<Process>();
			programName = programName.ToUpper();

			foreach (var process in Process.GetProcesses()) {
				if (process.ProcessName.ToUpper().StartsWith(programName)) {
					processes.Add(process);
					try {
						process.CloseMainWindow();
						++count;
					}
					catch { }
				}
			}

			Thread.Sleep(DEFAULT_SLEEPTIME);
			FreeResources(processes);
			Thread.Sleep(DEFAULT_SLEEPTIME);
			KillProcess(programName);
			return count;
		}

		/// <summary>
		/// Gets the System Uptime in seconds.
		/// </summary>
		/// <returns></returns>
		public static float GetUpTime()
		{
			using (var uptime = new PerformanceCounter("System", "System Up Time")) {
				uptime.NextValue(); // call this an extra time before reading its value
				return uptime.NextValue();
			}
		}

		/// <summary>
		/// Immediately stops the associated process.
		/// </summary>
		/// <param name="processName">The friendly name of the process.</param>
		/// <returns>The number of processes killed.</returns>
		public static int KillProcess(string processName)
		{
			if (String.IsNullOrWhiteSpace(processName))
				return 0;

			var count = 0;
			var processes = new List<Process>();
			processName = processName.ToUpper();

			foreach (var process in Process.GetProcesses()) {
				if (process.ProcessName.ToUpper().StartsWith(processName)) {
					processes.Add(process);
					try {
						process.Kill();
						++count;
					} catch { }
				}
			}

			Thread.Sleep(DEFAULT_SLEEPTIME);
			FreeResources(processes);
			return count;
		}

		public static void RefreshTrayArea()
		{
			var systemTrayContainerHandle = FindWindow("Shell_TrayWnd", null);
			var systemTrayHandle = FindWindowEx(systemTrayContainerHandle, IntPtr.Zero, "TrayNotifyWnd", null);
			var sysPagerHandle = FindWindowEx(systemTrayHandle, IntPtr.Zero, "SysPager", null);
			var notificationAreaHandle = FindWindowEx(sysPagerHandle, IntPtr.Zero, "ToolbarWindow32", "Notification Area");

			if (notificationAreaHandle == IntPtr.Zero) {
				notificationAreaHandle = FindWindowEx(sysPagerHandle, IntPtr.Zero, "ToolbarWindow32", "User Promoted Notification Area");
				var notifyIconOverflowWindowHandle = FindWindow("NotifyIconOverflowWindow", null);
				var overflowNotificationAreaHandle = FindWindowEx(notifyIconOverflowWindowHandle, IntPtr.Zero, "ToolbarWindow32", "Overflow Notification Area");
				RefreshTrayArea(overflowNotificationAreaHandle);
			}

			RefreshTrayArea(notificationAreaHandle);
		}

		/// <summary>
		/// Restarts the calling computer.
		/// </summary>
		/// <param name="waitTime">The number of seconds to wait before restarting. Default is 60 seconds. If waitTime is less than 0, it will ignore and use the default waitTime.</param>
		public static void RestartComputer(int waitTime = 60)
		{
			if (waitTime < 0)
				waitTime = 60;

			RestartComputer(true, waitTime, null, false);
		}

		public static void RestartComputer(bool force = true, int waitTime = 60, string remoteCompName = null, bool createWindow = false)
		{
			var args = "-r";

			if (force)
				args = String.Format("{0} -f", args);
			if (waitTime > -1)
				args = String.Format("{0} -t {1}", args, waitTime);
			if (!String.IsNullOrWhiteSpace(remoteCompName))
				args = String.Format(@"{0} -m \\{1}", args, remoteCompName);

			Process.Start(new ProcessStartInfo("shutdown", args) {
				CreateNoWindow = !createWindow,
				UseShellExecute = false,
			});
		}

		/// <summary>
		/// Restarts the calling computer if and only if the UpTime is greater than &lt;uptime&gt;.
		/// </summary>
		/// <param name="waitTime">The number of seconds to wait before restarting. Default is 60 seconds. If waitTime is less than 0, it will ignore and use the default waitTime.</param>
		/// <param name="uptime">Restarts only if the current Uptime is greater than &lt;uptime&gt;. Default is 75600 (21 hours).</param>
		public static void RestartComputerIfUptimeGreaterThan(int waitTime = 60, float uptime = TWENTY_ONE_HOURS)
		{
			if (waitTime < 0)
				waitTime = 60;

			if (GetUpTime() > uptime)
				RestartComputer(waitTime);
		}

		public static int StartProcess(string fileName, string args = null, bool redirectStandardOutput = true, bool redirectStandardError = true, bool useShellExecute = false, bool createNoWindow = true)
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
		#endregion Public Methods
	}
}