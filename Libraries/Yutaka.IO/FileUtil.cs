using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Yutaka.IO
{
	public static class FileUtil
	{
		#region Fields
		// Constants //
		const int DATE_TAKEN = 36867; // PropertyTagExifDTOrig //
		const int ONE_GIGABYTE = 1073741824; // Math.Pow(2, 30) //
		const int ONE_MEGABYTE =    1048576; // Math.Pow(2, 20) //
		const int FIVE_TWELVE_KB =   524288; // Math.Pow(2, 19) //
		const int ONE_KILOBYTE =       1024; // Math.Pow(2, 10) //
		const int BUFFER = FIVE_TWELVE_KB;

		// Config/Settings //
		private static bool consoleOut = true;
		private static string mode = "copy";

		// PIVs //
		private static DateTime dateThreshold = new DateTime(1982, 1, 1);
		#endregion

		#region Private Helpers
		private static bool CopyFile(FileInfo source, string dest)
		{
			try {
				source.CopyTo(dest);
				return true;
			}

			catch (Exception ex) {
				if (consoleOut)
					DisplayException(ex);
				return false;
			}
		}

		private static void DisplayException(Exception ex)
		{
			Console.Write("\n{0}", ex.Message);
			Console.Write("\n");
			Console.Write("\n{0}", ex.ToString());
		}

		// Retrieves the datetime WITHOUT loading the whole image //
		private static DateTime GetDateTakenFromImage(string path)
		{
			var r = new Regex(":");

			try {
				using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
					using (var myImage = Image.FromStream(fs, false, false)) {
						var propItem = myImage.GetPropertyItem(DATE_TAKEN);
						var dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
						return DateTime.Parse(dateTaken);
					}
				}
			}

			catch (Exception) {
				return dateThreshold;
			}
		}

		private static DateTime GetMinTime(FileInfo fi)
		{
			if (fi == null)
				return dateThreshold;

			var creationTime = fi.CreationTime;
			var lastAccessTime = fi.LastAccessTime;
			var lastWriteTime = fi.LastWriteTime;
			var minTime = DateTime.MaxValue;

			if (creationTime < minTime)
				minTime = creationTime;
			if (lastAccessTime < minTime)
				minTime = lastAccessTime;
			if (lastWriteTime < minTime)
				minTime = lastWriteTime;

			if (minTime > dateThreshold)
				return minTime;

			return dateThreshold;
		}

		private static bool MoveFile(FileInfo source, string dest)
		{
			try {
				source.MoveTo(dest);
				return true;
			}

			catch (Exception ex) {
				if (consoleOut)
					DisplayException(ex);
				return false;
			}
		}
		#endregion

		#region Public Methods
        public static void CopyFile(string source, string dest, bool delete = false)
        {
            if (String.IsNullOrEmpty(source))
                throw new Exception("<source> can't be empty");

            if (String.IsNullOrEmpty(dest))
                throw new Exception("<dest> can't be empty");

            var destInfo = new FileInfo(dest);
            var sourceInfo = new FileInfo(source);
            var sourceCreationTime = sourceInfo.CreationTime;
            var sourceLastAccessTime = sourceInfo.LastAccessTime;
            var sourceLastWriteTime = sourceInfo.LastWriteTime;
            var sourceLength = sourceInfo.Length;

            Console.Write("\nCreationTime: {0}", sourceCreationTime);
            Console.Write("\nLastAccessTime: {0}", sourceLastAccessTime);
            Console.Write("\nLastWriteTime: {0}", sourceLastWriteTime);
            Console.Write("\nLength: {0} bytes", sourceLength);

            if (destInfo.Exists)
            {
                var destLength = dest.Length;
                if (sourceLength == destLength)
                {
                    Console.Write("\nExact file exists already.");
                    return;
                }
                else
                {
                    CopyFile(sourceInfo, dest + "2");
                }
            }

            else /*!destInfo.Exists*/
            {
                CopyFile(sourceInfo, dest);
            }
        }

		/// <summary> Fast file move with big buffers
		/// </summary>
		/// <param name="source">Source file path</param> 
		/// <param name="destination">Destination file path</param> 
		public static void FastMove(string source, string destination, bool delete = true)
		{
			var array_length = BUFFER;
			var dataArray = new byte[array_length];

			using (var fsread = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.None, array_length)) {
				using (var bwread = new BinaryReader(fsread)) {
					using (var fswrite = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None, array_length)) {
						using (var bwwrite = new BinaryWriter(fswrite)) {
							for (; ; ) {
								var read = bwread.Read(dataArray, 0, array_length);
								if (0 == read)
									break;
								bwwrite.Write(dataArray, 0, read);
							}
						}
					}
				}
			}

			if (delete)
				File.Delete(source);
		}

        public static bool IsSameDate(FileInfo fi1, FileInfo fi2)
        {
            try
            {
                if (fi1.LastWriteTimeUtc == fi2.LastWriteTimeUtc)
                    return true;

                return false;
            }

            catch (Exception ex)
            {
                throw new Exception(Environment.NewLine + ex.Message + Environment.NewLine + ex.ToString());
            }
        }

        public static bool IsSameDate(string path1, string path2)
        {
            return IsSameDate(new FileInfo(path1), new FileInfo(path2));
        }

        public static bool IsSameFile(FileInfo fi1, FileInfo fi2)
        {
            try
            {
                if (IsSameDate(fi1, fi2) && IsSameSize(fi1, fi2))
                    return true;

                return false;
            }

            catch (Exception ex)
            {
                throw new Exception(Environment.NewLine + ex.Message + Environment.NewLine + ex.ToString());
            }
        }

        public static bool IsSameFile(string path1, string path2)
        {
            return IsSameFile(new FileInfo(path1), new FileInfo(path2));
        }

        public static bool IsSameSize(FileInfo fi1, FileInfo fi2)
        {
            try
            {
                if (fi1.Length == fi2.Length)
                    return true;

                return false;
            }

            catch (Exception ex)
            {
                throw new Exception(Environment.NewLine + ex.Message + Environment.NewLine + ex.ToString());
            }
        }

        public static bool IsSameSize(string path1, string path2)
        {
            return IsSameSize(new FileInfo(path1), new FileInfo(path2));
        }

        /// <summary> Time the Move
        /// </summary> 
        /// <param name="source">Source file path</param> 
        /// <param name="destination">Destination file path</param> 
        public static void MoveTime(string source, string destination, bool delete = true)
        {
            var start_time = DateTime.Now;
            FastMove(source, destination, delete);
            var milliseconds = 1 + (int)((DateTime.Now - start_time).TotalMilliseconds);
            var size = new FileInfo(destination).Length;
            // size time in milliseconds per sec
            var tsize = size * 1000 / milliseconds;
            if (tsize > ONE_GIGABYTE)
            {
                tsize = tsize / ONE_GIGABYTE;
                Console.Write("\n{0} transferred at {1}gb/sec", source, tsize);
            }
            else if (tsize > ONE_MEGABYTE)
            {
                tsize = tsize / ONE_MEGABYTE;
                Console.Write("\n{0} transferred at {1}mb/sec", source, tsize);
            }
            else if (tsize > ONE_KILOBYTE)
            {
                tsize = tsize / ONE_KILOBYTE;
                Console.Write("\n{0} transferred at {1}kb/sec", source, tsize);
            }
            else
                Console.Write("\n{0} transferred at {1}b/sec", source, tsize);
        }
        #endregion
	}
}