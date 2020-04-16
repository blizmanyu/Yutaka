using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Yutaka.Data
{
	public class TsqlUtil
	{
		public string DateFormat = @"MMM dd, yyyy";

		/// <summary>
		/// Gets Columns information from INFORMATION_SCHEMA.
		/// </summary>
		/// <param name="connectionString">The connection used to open the SQL Server database.</param>
		/// <param name="database">The database to query.</param>
		/// <param name="schema">Specify a schema. To get from all schema, leave blank.</param>
		/// <param name="table">Specify a table. To get from all tables, leave blank.</param>
		public IList<Column> GetColumnsInformation(string connectionString, string database, string schema = null, string table = null)
		{
			var list = new List<Column>();
			var where = "";
			var query = String.Format(
				"SELECT [TABLE_CATALOG]" +
				"      ,[TABLE_SCHEMA]" +
				"      ,[TABLE_NAME]" +
				"      ,[COLUMN_NAME]" +
				"      ,[ORDINAL_POSITION]" +
				"      ,[COLUMN_DEFAULT]" +
				"      ,[IS_NULLABLE]" +
				"      ,[DATA_TYPE]" +
				"      ,[CHARACTER_MAXIMUM_LENGTH]" +
				"      ,[CHARACTER_OCTET_LENGTH]" +
				"      ,[NUMERIC_PRECISION]" +
				"      ,[NUMERIC_PRECISION_RADIX]" +
				"      ,[NUMERIC_SCALE]" +
				"      ,[DATETIME_PRECISION]" +
				"      ,[CHARACTER_SET_CATALOG]" +
				"      ,[CHARACTER_SET_SCHEMA]" +
				"      ,[CHARACTER_SET_NAME]" +
				"      ,[COLLATION_CATALOG]" +
				"      ,[COLLATION_SCHEMA]" +
				"      ,[COLLATION_NAME]" +
				"      ,[DOMAIN_CATALOG]" +
				"      ,[DOMAIN_SCHEMA]" +
				"      ,[DOMAIN_NAME]" +
				"  FROM [{0}].[INFORMATION_SCHEMA].[COLUMNS]", database);

			if (!String.IsNullOrWhiteSpace(schema))
				where = String.Format(" WHERE [TABLE_SCHEMA] = '{0}'", schema);

			if (!String.IsNullOrWhiteSpace(table)) {
				if (String.IsNullOrWhiteSpace(where))
					where = String.Format(" WHERE [TABLE_NAME] = '{0}'", table);
				else
					where = String.Format("{0}   AND [TABLE_NAME] = '{1}'", where, table);
			}

			try {
				using (var conn = new SqlConnection(connectionString)) {
					using (var cmd = new SqlCommand(String.Format("{0}{1}", query, where), conn)) {
						cmd.CommandType = CommandType.Text;
						conn.Open();
						using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
							while (reader.Read()) {
								list.Add(new Column {
									TableCatalog = reader["TABLE_CATALOG"] is DBNull ? "" : reader["TABLE_CATALOG"].ToString(),
									TableSchema = reader["TABLE_SCHEMA"] is DBNull ? "" : reader["TABLE_SCHEMA"].ToString(),
									TableName = reader["TABLE_NAME"] is DBNull ? "" : reader["TABLE_NAME"].ToString(),
									ColumnName = reader["COLUMN_NAME"] is DBNull ? "" : reader["COLUMN_NAME"].ToString(),
									OrdinalPosition = reader["ORDINAL_POSITION"] is DBNull ? -1 : (int) reader["ORDINAL_POSITION"],
									ColumnDefault = reader["COLUMN_DEFAULT"]?.ToString(),
									IsNullable = reader["IS_NULLABLE"] is DBNull ? true : reader["IS_NULLABLE"].Equals("YES") ? true : false,
									DataType = reader["DATA_TYPE"] is DBNull ? "" : reader["DATA_TYPE"].ToString(),
									CharacterMaximumLength = reader["CHARACTER_MAXIMUM_LENGTH"] is DBNull ? -1 : (int) reader["CHARACTER_MAXIMUM_LENGTH"],
									CharacterOctetLength = reader["CHARACTER_OCTET_LENGTH"] is DBNull ? -1 : (int) reader["CHARACTER_OCTET_LENGTH"],
									NumericPrecision = reader["NUMERIC_PRECISION"] is DBNull ? -1 : int.Parse(reader["NUMERIC_PRECISION"].ToString()),
									NumericPrecisionRadix = reader["NUMERIC_PRECISION_RADIX"] is DBNull ? -1 : int.Parse(reader["NUMERIC_PRECISION_RADIX"].ToString()),
									NumericScale = reader["NUMERIC_SCALE"] is DBNull ? -1 : (int) reader["NUMERIC_SCALE"],
									DatetimePrecision = reader["DATETIME_PRECISION"] is DBNull ? -1 : int.Parse(reader["DATETIME_PRECISION"].ToString()),
									CharacterSetCatalog = reader["CHARACTER_SET_CATALOG"]?.ToString(),
									CharacterSetSchema = reader["CHARACTER_SET_SCHEMA"]?.ToString(),
									CharacterSetName = reader["CHARACTER_SET_NAME"]?.ToString(),
									CollationCatalog = reader["COLLATION_CATALOG"]?.ToString(),
									CollationSchema = reader["COLLATION_SCHEMA"]?.ToString(),
									CollationName = reader["COLLATION_NAME"]?.ToString(),
									DomainCatalog = reader["DOMAIN_CATALOG"]?.ToString(),
									DomainSchema = reader["DOMAIN_SCHEMA"]?.ToString(),
									DomainName = reader["DOMAIN_NAME"]?.ToString(),
								});
							}
						}
					}
				}
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in TsqlUtil.GetColumnsInformation(string connectionString, string database='{3}', string schema='{4}', string table='{5}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, database, schema, table);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of TsqlUtil.GetColumnsInformation(string connectionString, string database='{3}', string schema='{4}', string table='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, database, schema, table);

				Console.Write("\n{0}", log);
				throw new Exception(log);
				#endregion Log
			}

			return list;
		}

		/// <summary>
		/// Returns a script for creating a Sql View List.
		/// </summary>
		/// <param name="columns">The list of all columns from a table.</param>
		/// <returns></returns>
		public string ScriptTableAsCreateViewList(IList<Column> columns)
		{
			if (columns == null || columns.Count < 1)
				return "";

			var today = DateTime.Today.ToString(DateFormat);
			var script = "";
			var database = "";
			var schema = "";
			var table = "";
			var alias = "";
			var scriptIntro = true;

			foreach (var col in columns) {
				if (scriptIntro) {
					database = col.TableCatalog ?? "";
					schema = col.TableSchema ?? "";
					table = col.TableName ?? "";
					alias = table.Replace("_", "").Substring(0, 2).ToLower();
					script = String.Format("{0}USE [{2}]{1}", script, Environment.NewLine, database);
					script = String.Format("{0}GO{1}", script, Environment.NewLine);
					script = String.Format("{0}SET ANSI_NULLS ON{1}", script, Environment.NewLine);
					script = String.Format("{0}GO{1}", script, Environment.NewLine);
					script = String.Format("{0}SET QUOTED_IDENTIFIER ON{1}", script, Environment.NewLine);
					script = String.Format("{0}GO{1}", script, Environment.NewLine);
					script = String.Format("{0}-- ============================================={1}", script, Environment.NewLine);
					script = String.Format("{0}-- Author:      Yutaka Blizman{1}", script, Environment.NewLine);
					script = String.Format("{0}-- Create date: {2}{1}", script, Environment.NewLine, today);
					script = String.Format("{0}-- Modified:    {2}{1}", script, Environment.NewLine, today);
					script = String.Format("{0}-- Description: -{1}", script, Environment.NewLine);
					script = String.Format("{0}-- ============================================={1}", script, Environment.NewLine);
					script = String.Format("{0}CREATE VIEW [{2}].[{3}List] AS ({1}", script, Environment.NewLine, schema, table);
					script = String.Format("{0}     SELECT [{2}] = {3}.[{2}]{1}", script, Environment.NewLine, col.ColumnName, alias);
					scriptIntro = false;
				}

				else {
					script = String.Format("{0}           ,[{2}] = {3}.[{2}]{1}", script, Environment.NewLine, col.ColumnName, alias);

					if (col.ColumnName.Equals("CreatedById")) {
						script = String.Format("{0}           ,[CreatedByAlias] = cr.[Alias]{1}", script, Environment.NewLine);
						script = String.Format("{0}           ,[CreatedByUserName] = cr.[UserName]{1}", script, Environment.NewLine);
						script = String.Format("{0}           ,[CreatedByFullName] = cr.[FullName]{1}", script, Environment.NewLine);
					}

					else if (col.ColumnName.Equals("UpdatedById")) {
						script = String.Format("{0}           ,[UpdatedByAlias] = up.[Alias]{1}", script, Environment.NewLine);
						script = String.Format("{0}           ,[UpdatedByUserName] = up.[UserName]{1}", script, Environment.NewLine);
						script = String.Format("{0}           ,[UpdatedByFullName] = up.[FullName]{1}", script, Environment.NewLine);
					}

					else if (col.ColumnName.Equals("DeletedById")) {
						script = String.Format("{0}           ,[DeletedByAlias] = de.[Alias]{1}", script, Environment.NewLine);
						script = String.Format("{0}           ,[DeletedByUserName] = de.[UserName]{1}", script, Environment.NewLine);
						script = String.Format("{0}           ,[DeletedByFullName] = de.[FullName]{1}", script, Environment.NewLine);
					}
				}
			}

			script = String.Format("{0}       FROM [{2}].[{3}].[{4}] {5} WITH (NOLOCK){1}", script, Environment.NewLine, database, schema, table, alias);
			script = String.Format("{0}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] cr WITH (NOLOCK) ON cr.[Id] = {2}.[CreatedById]{1}", script, Environment.NewLine, alias);
			script = String.Format("{0}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] up WITH (NOLOCK) ON up.[Id] = {2}.[UpdatedById]{1}", script, Environment.NewLine, alias);
			script = String.Format("{0}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] de WITH (NOLOCK) ON de.[Id] = {2}.[DeletedById]{1}", script, Environment.NewLine, alias);
			script = String.Format("{0}){1}", script, Environment.NewLine);
			return script;
		}
	}
}