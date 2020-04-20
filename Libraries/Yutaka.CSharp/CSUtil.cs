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
	}
}