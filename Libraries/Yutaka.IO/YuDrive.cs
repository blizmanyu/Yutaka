using System;
using System.IO;
using System.Linq;

namespace Yutaka.IO
{
	public class YuDrive
	{
		protected const decimal ONE_KB = 1024m;
		protected const decimal ONE_MB = 1048576m;
		protected const decimal ONE_GB = 1073741824m;
		protected const decimal ONE_TB = 1099511627776m;
		protected const decimal ONE_PB = 1125899906842624m;

		public void Debug()
		{
			var drives = DriveInfo.GetDrives();

			Console.Write("\n======= Local Disks =============");
			foreach (var drive in drives.Where(x => x.DriveType.ToString().Equals("Fixed"))) {
				Console.Write("\n       DriveFormat: {0}", drive.DriveFormat);
				Console.Write("\n         DriveType: {0}", drive.DriveType);
				Console.Write("\n         GetType(): {0}", drive.GetType());
				Console.Write("\n           IsReady: {0}", drive.IsReady);
				Console.Write("\n              Name: {0}", drive.Name);
				Console.Write("\n     RootDirectory: {0}", drive.RootDirectory);
				Console.Write("\n        ToString(): {0}", drive.ToString());
				Console.Write("\nAvailableFreeSpace: {0}", drive.AvailableFreeSpace);
				Console.Write("\n    TotalFreeSpace: {0}", drive.TotalFreeSpace);
				Console.Write("\n         TotalSize: {0}", drive.TotalSize);
				Console.Write("\n       VolumeLabel: {0}", drive.VolumeLabel);
				Console.Write("\n");
			}

			Console.Write("\n======= Network Locations =======");
			foreach (var drive in drives.Where(x => x.DriveType.ToString().Equals("Network"))) {
				try {
					Console.Write("\n       DriveFormat: {0}", drive.DriveFormat);
					Console.Write("\n         DriveType: {0}", drive.DriveType);
					Console.Write("\n         GetType(): {0}", drive.GetType());
					Console.Write("\n           IsReady: {0}", drive.IsReady);
					Console.Write("\n              Name: {0}", drive.Name);
					Console.Write("\n     RootDirectory: {0}", drive.RootDirectory);
					Console.Write("\n        ToString(): {0}", drive.ToString());
					Console.Write("\nAvailableFreeSpace: {0}", drive.AvailableFreeSpace);
					Console.Write("\n    TotalFreeSpace: {0}", drive.TotalFreeSpace);
					Console.Write("\n         TotalSize: {0}", drive.TotalSize);
					Console.Write("\n       VolumeLabel: {0}", drive.VolumeLabel);
					Console.Write("\n");
				}

				catch (Exception) { continue; }
			}
		}

		public string ToFriendlyUnits(long bytes)
		{
			if (bytes == 0)
				return "0 bytes";
			if (bytes < 1000)
				return String.Format("{0} bytes", bytes);

			string unit;
			decimal temp;
			var decimalBytes = (decimal) bytes;

			#region KB
			if (bytes < 1023488) { // 999.5 KiB //
				temp = decimalBytes / ONE_KB;
				unit = "KB";

				if (temp < 10)
					return String.Format("{0:n2} {1}", temp, unit);
				if (temp < 100)
					return String.Format("{0:n1} {1}", temp, unit);

				return String.Format("{0:f0} {1}", temp, unit);
			}
			#endregion KB

			#region MB
			if (bytes < 1048051712) { // 999.5 MiB //
				temp = decimalBytes / ONE_MB;
				unit = "MB";

				if (temp < 10)
					return String.Format("{0:n2} {1}", temp, unit);
				if (temp < 100)
					return String.Format("{0:n1} {1}", temp, unit);

				return String.Format("{0:f0} {1}", temp, unit);
			}
			#endregion MB

			#region GB
			if (bytes < 1073204953088) { // 999.5 GiB //
				temp = decimalBytes / ONE_GB;
				unit = "GB";

				if (temp < 10)
					return String.Format("{0:n2} {1}", temp, unit);
				if (temp < 100)
					return String.Format("{0:n1} {1}", temp, unit);

				return String.Format("{0:f0} {1}", temp, unit);
			}
			#endregion GB

			#region TB
			if (bytes < 1098961871962112) { // 999.5 TiB //
				temp = decimalBytes / ONE_TB;
				unit = "TB";

				if (temp < 10)
					return String.Format("{0:n2} {1}", temp, unit);
				if (temp < 100)
					return String.Format("{0:n1} {1}", temp, unit);

				return String.Format("{0:f0} {1}", temp, unit);
			}
			#endregion TB

			temp = decimalBytes / ONE_PB;
			unit = "PB";

			if (temp < 10)
				return String.Format("{0:n2} {1}", temp, unit);
			if (temp < 100)
				return String.Format("{0:n1} {1}", temp, unit);

			return String.Format("{0:f0} {1}", temp, unit);
		}

		public void WriteToConsole()
		{
			long used;
			var driveCount = 0;
			var drives = DriveInfo.GetDrives();

			Console.Write("\n======= Local Disks =============");
			foreach (var drive in drives.Where(x => x.DriveType.ToString().Equals("Fixed"))) {
				used = drive.TotalSize - drive.TotalFreeSpace;
				Console.Write("\n{0}) '{1}' ({2})", ++driveCount, drive.VolumeLabel, drive.Name);
				Console.Write("\n Used space: {0} ({1:p})", ToFriendlyUnits(used), (decimal) used / drive.TotalSize);
				Console.Write("\n Free space: {0} ({1:p})", ToFriendlyUnits(drive.TotalFreeSpace), (decimal) drive.TotalFreeSpace / drive.TotalSize);
				Console.Write("\n   Capacity: {0}", ToFriendlyUnits(drive.TotalSize));
				Console.Write("\n");
			}

			Console.Write("\n======= Network Locations =======");
			foreach (var drive in drives.Where(x => x.DriveType.ToString().Equals("Network"))) {
				try {
					used = drive.TotalSize - drive.TotalFreeSpace;
					Console.Write("\n{0}) '{1}' ({2})", ++driveCount, drive.VolumeLabel, drive.Name);
					Console.Write("\n Used space: {0} ({1:p})", ToFriendlyUnits(used), (decimal) used / drive.TotalSize);
					Console.Write("\n Free space: {0} ({1:p})", ToFriendlyUnits(drive.TotalFreeSpace), (decimal) drive.TotalFreeSpace / drive.TotalSize);
					Console.Write("\n   Capacity: {0}", ToFriendlyUnits(drive.TotalSize));
					Console.Write("\n");
				}

				catch (Exception) { continue; }
			}
		}
	}
}