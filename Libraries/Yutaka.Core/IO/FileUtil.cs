using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.Core.IO
{
	public static class FileUtil
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool DeleteFile(string lpFileName);

		private static readonly string[] _sizes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB" };

		/// <summary>
		/// Tries to deletes the specified file.
		/// </summary>
		/// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
		/// <returns></returns>
		public static bool TryDelete(string path)
		{
			if (String.IsNullOrWhiteSpace(path))
				return true;

			try {
				return DeleteFile(path);
			}

			catch {
				return false;
			}
		}

		//public static string BytesToString(long bytes)
		//{
		//	var order = 0;

		//	while (bytes >= 1024 && order < _sizes.Length - 1) {
		//		++order;
		//		bytes = bytes / 1024;
		//	}

		//	// Adjust the format string to your preferences. For example "{0:0.#}{1}" would
		//	// show a single decimal place, and no space.
		//	string result = String.Format("{0:0.##} {1}", bytes, _sizes[order]);
		//}
	}
}