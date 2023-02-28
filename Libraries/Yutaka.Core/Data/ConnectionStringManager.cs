using System;
using System.Collections.Generic;
using System.IO;

namespace Yutaka.Core.Data
{
	/// <summary>
	/// WIP: Do not use yet! Manager of connection strings
	/// </summary>
	public partial class ConnectionStringManager
	{
		protected const char SEPARATOR = ':';
		protected const string FILENAME = "ConnectionStrings.txt";
		protected const string FILEDIR = @"C:\";
		protected List<ConnectionString> ConnectionStrings = new List<ConnectionString>();

		/// <summary>
		/// Parse settings
		/// </summary>
		/// <param name="text">Text of connection strings file</param>
		/// <returns>Parsed connection strings</returns>
		protected virtual ConnectionString ParseConnectionString(string text)
		{
			var connectionString = new ConnectionString();

			if (String.IsNullOrWhiteSpace(text))
				return connectionString;

			//Old way of file reading. This leads to unexpected behavior when a user's FTP program transfers these files as ASCII (\r\n becomes \n).
			//var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var lines = new List<string>();

			using (var reader = new StringReader(text)) {
				string str;
				while ((str = reader.ReadLine()) != null)
					lines.Add(str);
			}

			int separatorIndex;
			string key, value;

			foreach (var line in lines) {
				connectionString = new ConnectionString();
				separatorIndex = line.IndexOf(SEPARATOR);

				if (separatorIndex == -1)
					continue;

				key = line.Substring(0, separatorIndex).Trim();
				value = line.Substring(separatorIndex + 1).Trim();

				connectionString.Name = key;
				connectionString.ConnString = value;
				connectionString.RawDataSetting.Add(key, value);
				ConnectionStrings.Add(connectionString);
			}

			return connectionString;
		}

		/// <summary>
		/// Convert connection strings to string representation
		/// </summary>
		/// <param name="settings">Settings</param>
		/// <returns>Text</returns>
		protected virtual string ComposeSettings(ConnectionString settings)
		{
			if (settings == null)
				return "";

			return string.Format("DataProvider: {0}{2}DataConnectionString: {1}{2}",
								 //settings.DataProvider,
								 //settings.DataConnectionString,
								 Environment.NewLine
				);
		}

		/// <summary>
		/// Load settings
		/// </summary>
		/// <param name="filePath">File path; pass null to use default connection strings file path</param>
		/// <returns></returns>
		public virtual ConnectionString LoadSettings(string filePath = null)
		{
			if (String.IsNullOrEmpty(filePath)) {
				filePath = Path.Combine(FILEDIR, FILENAME);
			}
			if (File.Exists(filePath)) {
				string text = File.ReadAllText(filePath);
				return ParseConnectionString(text);
			}

			return new ConnectionString();
		}

		/// <summary>
		/// Save settings to a file
		/// </summary>
		/// <param name="settings"></param>
		public virtual void SaveSettings(ConnectionString settings)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");

			string filePath = Path.Combine(FILEDIR, FILENAME);
			if (!File.Exists(filePath)) {
				using (File.Create(filePath)) {
					//we use 'using' to close the file after it's created
				}
			}

			var text = ComposeSettings(settings);
			File.WriteAllText(filePath, text);
		}
	}
}