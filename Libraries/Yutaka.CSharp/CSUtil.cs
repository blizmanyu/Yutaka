using System;
using System.Collections.Generic;
using Yutaka.Data;

namespace Yutaka.Text
{
	public static class CSUtil
	{
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
					"\t\t}}{3}{3}", table, alias, idCol, Environment.NewLine);
			}

			return String.Format(
				"\t\tpublic {0} Get{0}ById(string id){3}" +
				"\t\t{{{3}" +
				"\t\t\treturn (from {1} in dbcontext.{0}s{3}" +
				"\t\t\t\t\twhere id.Equals({1}.{2}){3}" +
				"\t\t\t\t\tselect {1}).FirstOrDefault();{3}" +
				"\t\t}}{3}{3}", table, alias, idCol, Environment.NewLine);
		}
	}
}