using System;
using System.IO;

namespace Yutaka.IO2
{
	public static class FileUtil
	{
		public static readonly DateTime UNIX_TIME = new DateTime(1970, 1, 1);

		/// <summary>
		/// WIP: Do not use yet!
		/// </summary>
		/// <param name="sourceFileName">The file to copy.</param>
		/// <param name="destFileName">The name of the destination file. This cannot be a directory.</param>
		/// <param name="overwriteOption"></param>
		/// <returns></returns>
		public static bool Copy(string sourceFileName, string destFileName, OverwriteOption overwriteOption = OverwriteOption.Skip)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(sourceFileName))
				log = String.Format("{0}<sourceFileName> is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(destFileName))
				log = String.Format("{0}<destFileName> is required.{1}", log, Environment.NewLine);
			if (destFileName.Equals(sourceFileName))
				log = String.Format("{0}<sourceFileName> and <destFileName> are the same.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in FileUtil.Copy(string sourceFileName, string destFileName, OverwriteOption overwriteOption).{1}", log, Environment.NewLine);
				Console.Write("\n{0}\n", log);
				return false;
			}
			#endregion Input Check

			try {
				if (File.Exists(destFileName)) {
					switch (overwriteOption) {
						case OverwriteOption.Overwrite:
							File.Copy(sourceFileName, destFileName, true);
							return true;
						case OverwriteOption.Skip:
							File.Copy(sourceFileName, destFileName, false);
							return true;
						case OverwriteOption.KeepBoth:
							//File.Copy(sourceFileName, destFileName, false);
							return true;
						case OverwriteOption.OverwriteIfDifferent:
							//File.Copy(sourceFileName, destFileName, false);
							return true;
						default:
							return false;
					}
				}

				File.Copy(sourceFileName, destFileName);
				return true;
			}

			catch (Exception ex) {
				#region Log
				log = "";

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in FileUtil.Copy(string sourceFileName='{3}', string destFileName='{4}', OverwriteOption overwriteOption='{5}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, sourceFileName, destFileName, overwriteOption.ToString());
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of FileUtil.Copy(string sourceFileName='{3}', string destFileName='{4}', OverwriteOption overwriteOption='{5}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, sourceFileName, destFileName, overwriteOption.ToString());

				Console.Write("\n{0}", log);
				#endregion Log

				return false;
			}
		}
	}
}