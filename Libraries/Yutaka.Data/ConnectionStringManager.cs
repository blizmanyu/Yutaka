using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Yutaka.Data
{
	/// <summary>
	/// Manager of connection strings
	/// </summary>
	public class ConnectionStringManager
	{
		private const string FOLDER = @"DontMoveOrDelete\";
		private const string FOLDER1 = @"D:\" + FOLDER;
		private const string FOLDER2 = @"E:\" + FOLDER;
		private const string FOLDER3 = @"C:\" + FOLDER;
		private const string FILENAME = "ConnectionStrings.txt";
		private List<SqlConnectionStringBuilder> ConnectionStrings;

		public ConnectionStringManager(string filePath = null)
		{
			ConnectionStrings = new List<SqlConnectionStringBuilder>();
			LoadConnectionStrings(filePath);
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

		//public string GetConnectionString(string server=null, string database=null)
		//{
		//	if (String.IsNullOrWhiteSpace(server))
		//		server = "localhost";
		//	if (String.IsNullOrWhiteSpace(database))
		//		throw new Exception(String.Format("<database> is required.{0}Exception thrown in ConnectionStringManager.GetConnectionString(string server, string database){0}", Environment.NewLine));

		//	try {
		//		var query = from x in ConnectionStrings
		//					where x.DataSource
		//	}

		//	catch (Exception ex) {

		//	}
		//}

		/// <summary>
		/// Load connection strings
		/// </summary>
		/// <param name="filePath">File path; pass null to use default ConnectionStrings file path</param>
		/// <returns></returns>
		public void LoadConnectionStrings(string filePath = null)
		{
			if (String.IsNullOrWhiteSpace(filePath)) {
				if (Directory.Exists(FOLDER1))
					filePath = Path.Combine(FOLDER1, FILENAME);
				else if (Directory.Exists(FOLDER2))
					filePath = Path.Combine(FOLDER2, FILENAME);
				else if (Directory.Exists(FOLDER3))
					filePath = Path.Combine(FOLDER3, FILENAME);
				else
					throw new Exception(String.Format("Can't find default ConnectionStrings file.{0}Exception thrown in ConnectionStringManager.LoadConnectionStrings(string filePath){0}", Environment.NewLine));
			}

			if (File.Exists(filePath)) {
				ConnectionStrings = new List<SqlConnectionStringBuilder>();
				var text = File.ReadAllText(filePath);
				ParseConnectionStrings(text);
			}
		}
	}
}