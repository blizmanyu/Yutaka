using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace Yutaka.Data
{
	/// <summary>
	/// Manager of connection strings
	/// </summary>
	public class ConnectionStringManager
	{
		protected const string FOLDER1 = @"D:\NeverMoveOrDelete\";
		protected const string FOLDER2 = @"E:\NeverMoveOrDelete\";
		protected const string FOLDER3 = @"C:\NeverMoveOrDelete\";
		protected const string filename = "ConnectionStrings.txt";
		protected List<SqlConnectionStringBuilder> ConnectionStrings = new List<SqlConnectionStringBuilder>();

		public ConnectionStringManager()
		{

		}

		/// <summary>
		/// Parse connection strings
		/// </summary>
		/// <param name="text">Text of ConnectionStrings file</param>
		protected void ParseConnectionStrings(string text)
		{
			if (String.IsNullOrWhiteSpace(text))
				return;

			var connectionStrings = new List<string>();

			using (var reader = new StringReader(text)) {
				string str;
				while ((str = reader.ReadLine()) != null)
					connectionStrings.Add(str);
			}

			for (int i = 0; i < connectionStrings.Count; i++) {
				if (String.IsNullOrWhiteSpace(connectionStrings[i]))
					continue;
				ConnectionStrings.Add(new SqlConnectionStringBuilder(connectionStrings[i]));
			}
		}

		/// <summary>
		/// Load connection strings
		/// </summary>
		/// <param name="filePath">File path; pass null to use default ConnectionStrings file path</param>
		/// <returns></returns>
		public void LoadConnectionStrings(string filePath = null)
		{
			if (String.IsNullOrWhiteSpace(filePath)) {
				if (Directory.Exists(FOLDER1))
					filePath = Path.Combine(FOLDER1, filename);
				else if (Directory.Exists(FOLDER2))
					filePath = Path.Combine(FOLDER2, filename);
				else if (Directory.Exists(FOLDER3))
					filePath = Path.Combine(FOLDER3, filename);
				else
					throw new Exception(String.Format("Can't find default ConnectionStrings file.{0}Exception thrown in ConnectionStringManager.LoadConnectionStrings(string filePath){0}", Environment.NewLine));
			}

			if (File.Exists(filePath)) {
				ConnectionStrings.Clear();
				var text = File.ReadAllText(filePath);
				ParseConnectionStrings(text);
			}
		}
	}
}