using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.FileUtil
{
    public class FileUtil
	{
		#region Public Methods
		public static bool IsSameDate(FileInfo fi1, FileInfo fi2)
		{
			try {
				if (fi1.LastWriteTimeUtc == fi2.LastWriteTimeUtc)
					return true;

				return false;
			}

			catch (Exception ex) {
				throw new Exception(Environment.NewLine + ex.Message + Environment.NewLine + ex.ToString());
			}
		}

		public static bool IsSameDate(string path1, string path2)
		{
			return IsSameDate(new FileInfo(path1), new FileInfo(path2));
		}

		public static bool IsSameFile(FileInfo fi1, FileInfo fi2)
		{
			try {
				if (IsSameDate(fi1, fi2) && IsSameSize(fi1, fi2))
					return true;

				return false;
			}

			catch (Exception ex) {
				throw new Exception(Environment.NewLine + ex.Message + Environment.NewLine + ex.ToString());
			}
		}

		public static bool IsSameFile(string path1, string path2)
		{
			return IsSameFile(new FileInfo(path1), new FileInfo(path2));
		}

		public static bool IsSameSize(FileInfo fi1, FileInfo fi2)
		{
			try {
				if (fi1.Length == fi2.Length)
					return true;

				return false;
			}

			catch (Exception ex) {
				throw new Exception(Environment.NewLine + ex.Message + Environment.NewLine + ex.ToString());
			}
		}

		public static bool IsSameSize(string path1, string path2)
		{
			return IsSameSize(new FileInfo(path1), new FileInfo(path2));
		}
		#endregion
	}
}