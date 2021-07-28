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
		public static int TryDelete(string path)
		{
			if (String.IsNullOrWhiteSpace(path)) {
				Console.Write("{0}'path' is null or whitespace.{0}Exception thrown in FileUtil.TryDelete(string path).{0}", Environment.NewLine);
				return 0;
			}

			try {
				DeleteFile(path);
				return 1;
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					Console.Write("{0}{2}Exception thrown in FileUtil.TryDelete(string path='{3}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, path);
				else
					Console.Write("{0}{2}Exception thrown in INNER EXCEPTION of FileUtil.TryDelete(string path='{3}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, path);
				#endregion Log

				return 0;
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