using System;
using System.Collections.Generic;
using System.Text;
using Yutaka.Data;

namespace Yutaka.Text
{
	public static class CSUtil
	{
		/// <summary>
		/// Calls all code generating methods in this class and returns them as one string.
		/// </summary>
		/// <param name="columns">The list of all columns from a table.</param>
		/// <returns></returns>
		public static string GenerateAll(IList<Column> columns)
		{
			if (columns == null || columns.Count < 1)
				return "";

			var table = columns[0].TableName;
			var sb = new StringBuilder();
			sb.Append(String.Format("\t\t#region {0}{1}", table, Environment.NewLine));
			sb.Append(GenerateGetById(columns)).Append(Environment.NewLine);
			//sb.Append(GenerateSearch(columns)).Append(Environment.NewLine);
			//sb.Append(GenerateSearch(columns)).Append(Environment.NewLine);
			sb.Append(GenerateSearch(columns));
			sb.Append(String.Format("\t\t#endregion {0}{1}", table, Environment.NewLine));
			return sb.ToString();
		}

		/// <summary>
		/// Generates method for Getting the entity by Id.
		/// </summary>
		/// <param name="table">The name of the table.</param>
		/// <returns></returns>
		public static string GenerateGetById(IList<Column> columns)
		{
			var table = "";
			var idCol = "";
			var dataType = "";

			foreach (var col in columns) {
				if (col.ColumnName.Equals("Id") || col.ColumnName.Equals("UniqueId")) {
					table = col.TableName;
					idCol = col.ColumnName;
					dataType = col.DataType;
					break;
				}
			}

			var alias = table.Replace("_", "").Substring(0, 1).ToLower();

			if (dataType.Equals("int")) {
				return String.Format(
					"\t\tpublic {0} Get{0}ById(int id){3}" +
					"\t\t{{{3}" +
					"\t\t\treturn (from {1} in dbcontext.{0}s{3}" +
					"\t\t\t\t\twhere id == {1}.{2}{3}" +
					"\t\t\t\t\tselect {1}).FirstOrDefault();{3}" +
					"\t\t}}{3}", table, alias, idCol, Environment.NewLine);
			}

			return String.Format(
				"\t\tpublic {0} Get{0}ById(string id){3}" +
				"\t\t{{{3}" +
				"\t\t\treturn (from {1} in dbcontext.{0}s{3}" +
				"\t\t\t\t\twhere id.Equals({1}.{2}){3}" +
				"\t\t\t\t\tselect {1}).FirstOrDefault();{3}" +
				"\t\t}}{3}", table, alias, idCol, Environment.NewLine);
		}

		/// <summary>
		/// Generates method for searching the entity.
		/// </summary>
		/// This is WIP.
		/// <param name="table">The name of the table.</param>
		/// <returns></returns>
		public static string GenerateSearch(IList<Column> columns)
		{
			var table = "";
			var idCol = "";
			var dataType = "";

			foreach (var col in columns) {
				table = col.TableName;
				idCol = col.ColumnName;
				dataType = col.DataType;
				break;
			}

			var alias = table.Replace("_", "").Substring(0, 1).ToLower();

			return String.Format(
				"\t\tpublic IList<{0}List> Search{0}(){3}" +
				"\t\t{{{3}" +
				"\t\t\tvar query = from {1} in dbcontext.{0}Lists{3}" +
				"\t\t\t\t\t\tselect {1};{3}" +
				"{3}" +
				"\t\t\treturn query.ToList();{3}" +
				"\t\t}}{3}", table, alias, idCol, Environment.NewLine);
		}

		/// <summary>
		/// WIP: Do not use yet! Generates method for Updating.
		/// </summary>
		/// <param name="columns">The list of Columns Information.</param>
		/// <returns></returns>
		public static string GenerateTryUpdate(IList<Column> columns)
		{
			var database = "";
			var table = "";
			var alias = "";
			var idCol = "";
			var dataType = "";
			var parameters = new StringBuilder();
			var isFirstCol = true;
			var findID = true;

			foreach (var col in columns) {
				if (isFirstCol) {
					database = col.TableCatalog;
					table = col.TableName;

					if (table.Substring(0, 1).Equals("_"))
						table = table.Substring(1);

					alias = String.Format("{0}{1}", table.Substring(0, 1).ToLower(), table.Substring(1));
					isFirstCol = false;
				}

				if (findID && (col.ColumnName.Equals("Id") || col.ColumnName.Equals("UniqueId"))) {
					idCol = col.ColumnName;
					dataType = col.DataType;
					findID = false;
				}

				parameters.Append(String.Format("\t\t\t\t\tnew SqlParameter(\"@{0}\", {1}.{0}", col.ColumnName, alias));

				if (col.IsNullable)
					parameters.Append(" ?? null");
				
				parameters.Append(String.Format("),{0}", Environment.NewLine));
			}

			var sb = new StringBuilder();
			sb.Append(String.Format("\t\tpublic bool TryUpdate{0}({0} {1}, out string response)", table, alias)).Append(Environment.NewLine);
			sb.Append("\t\t{").Append(Environment.NewLine);
			sb.Append("\t\t\t#region Input Check").Append(Environment.NewLine);
			sb.Append("\t\t\tresponse = \"\";").Append(Environment.NewLine);
			sb.Append(Environment.NewLine);
			sb.Append(String.Format("\t\t\tif ({0} == null)", alias)).Append(Environment.NewLine);
			sb.Append(String.Format("\t\t\t\tresponse = String.Format(\"{{0}}<{0}> is null.{{1}}\", response, Environment.NewLine);", alias)).Append(Environment.NewLine);

			if (dataType.Equals("int")) {
				sb.Append(String.Format("\t\t\telse if ({0}.{1} < 1)", alias, idCol)).Append(Environment.NewLine);
				sb.Append(String.Format("\t\t\t\tresponse = String.Format(\"{{0}}{0}.{1} is invalid.{{2}}\", response, Environment.NewLine);", alias, idCol)).Append(Environment.NewLine);
			}
			else {
				sb.Append(String.Format("\t\t\telse if (String.IsNullOrWhiteSpace({0}.{1}))", alias, idCol)).Append(Environment.NewLine);
				sb.Append(String.Format("\t\t\t\tresponse = String.Format(\"{{0}}{0}.{1} is NULL or whitespace.{{2}}\", response, Environment.NewLine);", alias, idCol)).Append(Environment.NewLine);
			}

			sb.Append(Environment.NewLine);
			sb.Append("\t\t\tif (!String.IsNullOrWhiteSpace(response)) {").Append(Environment.NewLine);
			sb.Append(String.Format("\t\t\t\tresponse = String.Format(\"{{0}}Exception thrown in {0}Service.TryUpdate{1}({1} {2}, out string response).{{1}}{{1}}\", response, Environment.NewLine);", database, table, alias)).Append(Environment.NewLine);
			sb.Append("\t\t\t\treturn false;").Append(Environment.NewLine);
			sb.Append("\t\t\t}").Append(Environment.NewLine);
			sb.Append("\t\t\t#endregion Input Check").Append(Environment.NewLine);
			sb.Append(Environment.NewLine);
			sb.Append("\t\t\ttry {").Append(Environment.NewLine);
			sb.Append(String.Format("\t\t\t\tvar storProc = \"[dbo].[{0}Update]\";", table)).Append(Environment.NewLine);
			sb.Append("\t\t\t\tSqlParameter[] parameters = {").Append(Environment.NewLine);
			sb.Append(parameters);
			sb.Append("\t\t\t\t};").Append(Environment.NewLine);
			sb.Append("\t\t\t\tExecuteNonQuery(storProc, STORED_PROCEDURE, parameters);").Append(Environment.NewLine);
			sb.Append("\t\t\t\treturn true;").Append(Environment.NewLine);
			sb.Append("\t\t\t}").Append(Environment.NewLine);
			sb.Append(Environment.NewLine);
			sb.Append("\t\t\tcatch (Exception ex) {").Append(Environment.NewLine);
			sb.Append("\t\t\t\t#region Log").Append(Environment.NewLine);
			sb.Append("\t\t\t\tif (ex.InnerException == null)").Append(Environment.NewLine);
			sb.Append(String.Format("\t\t\t\t\tresponse = String.Format(\"{{0}}{{2}}Exception thrown in {0}Service.TryUpdateChatter(Chatter chatter='{{3}}', out string response).{{2}}{{1}}{{2}}{{2}}\", ex.Message, ex.ToString(), Environment.NewLine, chatter.Id);", database)).Append(Environment.NewLine);
			sb.Append("\t\t\t\telse").Append(Environment.NewLine);
			sb.Append(String.Format("\t\t\t\t\tresponse = String.Format(\"{{0}}{{2}}Exception thrown in INNER EXCEPTION of {0}Service.TryUpdateChatter(Chatter chatter='{{3}}', out string response).{{2}}{{1}}{{2}}{{2}}\", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, chatter.Id);", database)).Append(Environment.NewLine);
			sb.Append(Environment.NewLine);
			sb.Append("\t\t\t\tLog(response);").Append(Environment.NewLine);
			sb.Append("\t\t\t\t#endregion Log").Append(Environment.NewLine);
			sb.Append(Environment.NewLine);
			sb.Append("\t\t\t\treturn false;").Append(Environment.NewLine);
			sb.Append("\t\t\t}").Append(Environment.NewLine);
			sb.Append("\t\t}").Append(Environment.NewLine);
			return sb.ToString();
		}
	}
}