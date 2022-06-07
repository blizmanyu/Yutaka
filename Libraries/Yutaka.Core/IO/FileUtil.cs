using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.FileIO;

namespace Yutaka.Core.IO
{
	public static class FileUtil
	{
		private static readonly string[] _sizes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB" };
		#region public static readonly HashSet<string> ImageExtensions & VideoExtensions
		public static readonly HashSet<string> ImageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
			".ai", ".bmp", ".eps", ".gif", ".ico", ".jpg", ".jpeg", ".png", ".psd", ".tiff", ".webp",
		};
		public static readonly HashSet<string> VideoExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
			".3gp", ".avi", ".flv", ".m4v", ".mkv", ".mpg", ".mpeg", ".mp4", ".ogv", ".mov", ".webm", ".wmv",
		};
		#endregion

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool DeleteFile(string lpFileName);

		/// <summary>
		/// Dumps the contents of a <see cref="DataTable"/> to console.
		/// </summary>
		/// <param name="dt">The <see cref="DataTable"/>.</param>
		public static void ConsoleOut(DataTable dt)
		{
			if (dt == null)
				Console.Write("\nThe DataTable is null.\n");

			else if (dt.Rows.Count < 1)
				Console.Write("\nThe DataTable is empty.\n");

			else {
				Console.Write("\n");
				var count = 0;

				foreach (DataRow row in dt.Rows) {
					Console.Write("{0,3})", ++count);

					foreach (var item in row.ItemArray)
						Console.Write("{0,10}", item);

					Console.Write("\n");
				}
			}
		}

		/// <summary>
		/// Checks if 2 files are different based on file Length and LastWriteTime only. Does not check filenames, checksums, CRC, or anything else.
		/// </summary>
		/// <param name="f1">File 1</param>
		/// <param name="f2">File 2</param>
		/// <returns></returns>
		public static bool IsDifferent(FileInfo f1, FileInfo f2)
		{
			#region Check Input
			var log = "";

			if (f1 == null)
				log = String.Format("{0}f1 is null.{1}", log, Environment.NewLine);
			if (f2 == null)
				log = String.Format("{0}f2 is null.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in FileUtil.IsDifferent(FileInfo f1, FileInfo f2).{1}", log, Environment.NewLine);
				Console.Write("\n{0}\n");
				throw new Exception(log);
			}
			#endregion

			if (f1.Length != f2.Length)
				return true;
			if (f1.LastWriteTime != f2.LastWriteTime)
				return true;

			return false;
		}

		/// <summary>
		/// Checks if 2 files are the same based on file Length and LastWriteTime only. Does not check filenames, checksums, CRC, or anything else.
		/// </summary>
		/// <param name="f1">File 1</param>
		/// <param name="f2">File 2</param>
		/// <returns></returns>
		public static bool IsSame(FileInfo f1, FileInfo f2)
		{
			#region Check Input
			var log = "";

			if (f1 == null)
				log = String.Format("{0}f1 is null.{1}", log, Environment.NewLine);
			if (f2 == null)
				log = String.Format("{0}f2 is null.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in FileUtil.IsSame(FileInfo f1, FileInfo f2).{1}", log, Environment.NewLine);
				Console.Write("\n{0}\n");
				throw new Exception(log);
			}
			#endregion

			if (f1.Length != f2.Length)
				return false;
			if (f1.LastWriteTime != f2.LastWriteTime)
				return false;

			return true;
		}

		/// <summary>
		/// Checks if 2 files are the same based on file Length and LastWriteTime only. Does not check filenames, checksums, CRC, or anything else.
		/// </summary>
		/// <param name="filePath">Path of file 1.</param>
		/// <param name="obj">Object containing size and LastWriteTime of file 2.</param>
		/// <returns></returns>
		public static bool IsSame(string filePath, YuFile obj)
		{
			#region Check Input
			var log = "";

			if (filePath == null)
				log = String.Format("{0}filePath is null.{1}", log, Environment.NewLine);
			else if (String.IsNullOrWhiteSpace(filePath))
				log = String.Format("{0}filePath is empty.{1}", log, Environment.NewLine);
			else if (!File.Exists(filePath))
				log = String.Format("{0}filePath '{2}' doesn't exist.{1}", log, Environment.NewLine, filePath);

			if (obj == null)
				log = String.Format("{0}obj is null.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in FileUtil.IsSame(string filePath, YuFile obj).{1}", log, Environment.NewLine);
				Console.Write("\n{0}\n");
				throw new Exception(log);
			}
			#endregion

			var fi = new FileInfo(filePath);

			if (fi.Length != obj.Size) {
				fi = null;
				return false;
			}

			if (fi.LastWriteTime != obj.LastWriteTime) {
				fi = null;
				return false;
			}

			fi = null;
			return true;
		}

		/// <summary>
		/// Checks if 2 files are the same based on file Length and LastWriteTime only. Does not check filenames, checksums, CRC, or anything else.
		/// </summary>
		/// <param name="filePath1">Path of file 1.</param>
		/// <param name="filePath2">Path of file 2.</param>
		/// <returns></returns>
		public static bool IsSame(string filePath1, string filePath2)
		{
			#region Check Input
			var log = "";

			if (filePath1 == null)
				log = String.Format("{0}filePath1 is null.{1}", log, Environment.NewLine);
			else if (String.IsNullOrWhiteSpace(filePath1))
				log = String.Format("{0}filePath1 is empty.{1}", log, Environment.NewLine);
			else if (!File.Exists(filePath1))
				log = String.Format("{0}filePath1 '{2}' doesn't exist.{1}", log, Environment.NewLine, filePath1);

			if (filePath2 == null)
				log = String.Format("{0}filePath2 is null.{1}", log, Environment.NewLine);
			else if (String.IsNullOrWhiteSpace(filePath2))
				log = String.Format("{0}filePath2 is empty.{1}", log, Environment.NewLine);
			else if (!File.Exists(filePath2))
				log = String.Format("{0}filePath2 '{2}' doesn't exist.{1}", log, Environment.NewLine, filePath2);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in FileUtil.IsSame(string filePath1, string filePath2).{1}", log, Environment.NewLine);
				Console.Write("\n{0}\n");
				throw new Exception(log);
			}
			#endregion

			return IsSame(new FileInfo(filePath1), new FileInfo(filePath2));
		}

		/// <summary>
		/// Sets the LastWriteTime of a file.
		/// </summary>
		/// <param name="fi">The file.</param>
		/// <param name="dt">The new datetime to set it as.</param>
		public static void SetLastWriteTime(FileInfo fi, DateTime dt)
		{
			#region Check Input
			var log = "";

			if (fi == null)
				log = String.Format("{0}fi is null.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in FileUtil.SetLastWriteTime(FileInfo fi, DateTime dt).{1}", log, Environment.NewLine);
				Console.Write("\n{0}\n");
				throw new Exception(log);
			}
			#endregion

			try {
				var isReadOnly = false;

				if (fi.IsReadOnly) {
					isReadOnly = true;
					fi.IsReadOnly = false;
					fi.Refresh();
				}

				fi.LastWriteTime = dt;

				if (isReadOnly)
					fi.IsReadOnly = true;
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in FileUtil.SetLastWriteTime(FileInfo fi='{3}', DateTime dt='{4}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, fi, dt);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of FileUtil.SetLastWriteTime(FileInfo fi='{3}', DateTime dt='{4}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, fi, dt);

				Console.Write("\n{0}", log);
				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// Sets the LastWriteTime of a file.
		/// </summary>
		/// <param name="filePath">The file path of the file.</param>
		/// <param name="dt">The new datetime to set it as.</param>
		public static void SetLastWriteTime(string filePath, DateTime dt)
		{
			#region Check Input
			var log = "";

			if (filePath == null)
				log = String.Format("{0}filePath is null.{1}", log, Environment.NewLine);
			else if (String.IsNullOrWhiteSpace(filePath))
				log = String.Format("{0}filePath is empty.{1}", log, Environment.NewLine);
			else if (!File.Exists(filePath))
				log = String.Format("{0}filePath '{2}' doesn't exist.{1}", log, Environment.NewLine, filePath);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in FileUtil.SetLastWriteTime(string filePath, DateTime dt).{1}", log, Environment.NewLine);
				Console.Write("\n{0}\n");
				throw new Exception(log);
			}
			#endregion

			SetLastWriteTime(new FileInfo(filePath), dt);
		}

		/// <summary>
		/// Converts a flat file to a <see cref="DataTable"/>.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="delimiter">The delimiter. The default is a comma.</param>
		/// <param name="hasFieldsEnclosedInQuotes">Whether the fields are enclosed in quotes or not (key distinguisher for CSV files).</param>
		/// <returns></returns>
		public static DataTable ToDataTable(string filePath, string delimiter = ",", bool hasFieldsEnclosedInQuotes = false)
		{
			#region Check Input
			var log = "";

			if (String.IsNullOrWhiteSpace(filePath))
				log = String.Format("{0}filePath is required.{1}", log, Environment.NewLine);
			else if (!File.Exists(filePath))
				log = String.Format("{0}filePath '{2}' does not exist.{1}", log, Environment.NewLine, filePath);

			if (String.IsNullOrEmpty(delimiter)) // whitespace is a valid delimiter, so only check IsNullOrEmpty
				log = String.Format("{0}delimiter is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in FileUtil.ToDataTable(string filePath, string delimiter, bool hasFieldsEnclosedInQuotes).{1}", log, Environment.NewLine);
				Console.Write("\n{0}", log);
				return null;
			}
			#endregion
			var dt = new DataTable();

			try {
				using (var tfp = new TextFieldParser(filePath)) {
					DataColumn datecolumn;
					tfp.SetDelimiters(new string[] { delimiter });
					tfp.HasFieldsEnclosedInQuotes = hasFieldsEnclosedInQuotes;
					var colFields = tfp.ReadFields();

					foreach (string column in colFields) {
						datecolumn = new DataColumn(column);
						datecolumn.AllowDBNull = true;
						dt.Columns.Add(datecolumn);
					}

					string[] fieldData;

					while (!tfp.EndOfData) {
						fieldData = tfp.ReadFields();

						for (int i = 0; i < fieldData.Length; i++) {
							if (fieldData[i].Equals("")) // make empty values null
								fieldData[i] = null;
						}

						dt.Rows.Add(fieldData);
					}
				}
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					Console.Write("{0}{2}Exception thrown in FileUtil.ToDataTable(string filePath='{3}', string delimiter='{4}', bool hasFieldsEnclosedInQuotes='{5}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, filePath, delimiter, hasFieldsEnclosedInQuotes);
				else
					Console.Write("{0}{2}Exception thrown in INNER EXCEPTION of FileUtil.ToDataTable(string filePath='{3}', string delimiter='{4}', bool hasFieldsEnclosedInQuotes='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, filePath, delimiter, hasFieldsEnclosedInQuotes);
				#endregion Log
				return null;
			}

			return dt;
		}

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
	}
}