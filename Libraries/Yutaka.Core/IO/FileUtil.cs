using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.FileIO;

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
	}
}