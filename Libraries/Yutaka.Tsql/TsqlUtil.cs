﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text;

namespace Yutaka.Data
{
	public class TsqlUtil
	{
		#region Fields
		public DateTime CreateDate = DateTime.Now;
		public string Author = "";
		public string Database = "";
		public string DateFormat = "MMM d, yyyy";
		public string EditViewDescription = String.Format("Only use this on Edit and View pages where you're getting 1 record.\r\n--\t\t\t\tDo *not* use this on List or Search pages where you're pulling multiple rows. Use View [**ListView] instead.");
		public string ListViewDescription = String.Format("Optimized version that only gets the columns you need for List and Search pages.");
		public string Schema = "";
		public string Table = "";
		#endregion

		#region Constructors
		/// <summary>
		/// Creates an instance of <see cref="TsqlUtil"/>.
		/// </summary>
		public TsqlUtil() { }

		/// <summary>
		/// Creates an instance of <see cref="TsqlUtil"/>. Specifying the database name.
		/// </summary>
		/// <param name="database">The database name.</param>
		public TsqlUtil(string database)
		{
			if (String.IsNullOrWhiteSpace(database))
				throw new Exception("'database' is required. Exception thrown in Constructor TsqlUtil(string database).");
			else
				Database = database.Trim();
		}

		/// <summary>
		/// Creates an instance of <see cref="TsqlUtil"/>. Specifying the database and schema name.
		/// </summary>
		/// <param name="database">The database name.</param>
		/// <param name="schema">The schema name.</param>
		public TsqlUtil(string database, string schema)
		{
			if (String.IsNullOrWhiteSpace(database))
				throw new Exception("'database' is required. Exception thrown in Constructor TsqlUtil(string database, string schema).");
			else
				Database = database.Trim();

			if (String.IsNullOrWhiteSpace(schema))
				throw new Exception("'schema' is required. Exception thrown in Constructor TsqlUtil(string database, string schema).");
			else
				Schema = schema.Trim();
		}

		/// <summary>
		/// Creates an instance of <see cref="TsqlUtil"/>. Specifying the database, schema, and table name.
		/// </summary>
		/// <param name="database">The database name.</param>
		/// <param name="schema">The schema name.</param>
		/// <param name="table">The table name.</param>
		public TsqlUtil(string database, string schema, string table)
		{
			if (String.IsNullOrWhiteSpace(database))
				throw new Exception("'database' is required. Exception thrown in Constructor TsqlUtil(string database, string schema, string table).");
			else
				Database = database.Trim();

			if (String.IsNullOrWhiteSpace(schema))
				throw new Exception("'schema' is required. Exception thrown in Constructor TsqlUtil(string database, string schema, string table).");
			else
				Schema = schema.Trim();

			if (String.IsNullOrWhiteSpace(table))
				throw new Exception("'table' is required. Exception thrown in Constructor TsqlUtil(string database, string schema, string table).");
			else
				Table = table.Trim();
		}
		#endregion

		#region Utilities
		/// <summary>
		/// Scripts a CREATE PROCEDURE template.
		/// </summary>
		/// <returns></returns>
		protected string ScriptCreateProcedureTemplate(string description = "")
		{
			var sb = new StringBuilder(ScriptHeading(description));
			sb.AppendLine("CREATE PROCEDURE [_SCHEMA_].[_PROCEDURE_NAME_]");
			sb.AppendLine("_PARAMETERS_");
			sb.AppendLine("AS");
			sb.AppendLine("BEGIN");
			sb.AppendLine("\t-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.");
			sb.AppendLine("\tSET NOCOUNT ON;");
			sb.AppendLine();
			sb.AppendLine("_STATEMENT_CLAUSE_");
			sb.AppendLine("END");
			sb.AppendLine("GO");
			sb.AppendLine().AppendLine();

			return sb.ToString();
		}

		/// <summary>
		/// Scripts a CREATE VIEW template.
		/// </summary>
		/// <returns></returns>
		protected string ScriptCreateViewTemplate(string description = "")
		{
			var sb = new StringBuilder(ScriptHeading(description));
			sb.AppendLine("CREATE VIEW [_SCHEMA_].[_VIEW_NAME_] AS (");
			sb.AppendLine("_SELECT_CLAUSE_");
			sb.AppendLine("_FROM_CLAUSE_");
			sb.AppendLine(")");
			sb.AppendLine("GO");
			sb.AppendLine().AppendLine();

			return sb.ToString();
		}

		/// <summary>
		/// Scripts the heading part of almost all T-Sql script.
		/// </summary>
		/// <returns>The heading part of the script.</returns>
		protected string ScriptHeading(string description = "")
		{
			var sb = new StringBuilder();
			sb.AppendLine(String.Format("USE [{0}]", Database));
			sb.AppendLine(String.Format("GO"));
			sb.AppendLine(String.Format("SET ANSI_NULLS ON"));
			sb.AppendLine(String.Format("GO"));
			sb.AppendLine(String.Format("SET QUOTED_IDENTIFIER ON"));
			sb.AppendLine(String.Format("GO"));
			sb.AppendLine(String.Format("-- ============================================="));
			sb.AppendLine(String.Format("-- Author:\t\t{0}",	Author));
			sb.AppendLine(String.Format("-- Create date:\t{0}",	CreateDate.ToString("MMM d, yyyy")));
			sb.AppendLine(String.Format("-- Modified:\t{0}",	CreateDate.ToString("MMM d, yyyy")));
			sb.AppendLine(String.Format("-- Description:\t{0}", description));
			sb.AppendLine(String.Format("-- ============================================="));

			return sb.ToString();
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Gets Columns information from INFORMATION_SCHEMA and sys.columns.
		/// </summary>
		/// <param name="connectionString">The connection used to open the SQL Server database.</param>
		/// <param name="database">The database to query.</param>
		/// <param name="schema">Specify a schema. To get from all schema, leave blank.</param>
		/// <param name="table">Specify a table. To get from all tables, leave blank.</param>
		public IList<Column> GetColumnsInformation(string connectionString, string database, string schema = null, string table = null)
		{
			Column col;
			var list = new List<Column>();
			var where = "";
			var select = new StringBuilder();
			select.AppendLine("SELECT [TABLE_CATALOG] = c.[TABLE_CATALOG]");
			select.AppendLine("      ,[TABLE_SCHEMA] = c.[TABLE_SCHEMA]");
			select.AppendLine("      ,[TABLE_NAME] = c.[TABLE_NAME]");
			select.AppendLine("      ,[COLUMN_NAME] = c.[COLUMN_NAME]");
			select.AppendLine("      ,[ORDINAL_POSITION] = c.[ORDINAL_POSITION]");
			select.AppendLine("      ,[COLUMN_DEFAULT] = c.[COLUMN_DEFAULT]");
			select.AppendLine("      ,[IS_NULLABLE] = c.[IS_NULLABLE]");
			select.AppendLine("      ,[IsIdentity] = sc.[is_identity]");
			select.AppendLine("      ,[IsComputed] = sc.[is_computed]");
			select.AppendLine("      ,[DATA_TYPE] = c.[DATA_TYPE]");
			select.AppendLine("      ,[CHARACTER_MAXIMUM_LENGTH] = c.[CHARACTER_MAXIMUM_LENGTH]");
			select.AppendLine("      ,[CHARACTER_OCTET_LENGTH] = c.[CHARACTER_OCTET_LENGTH]");
			select.AppendLine("      ,[NUMERIC_PRECISION] = c.[NUMERIC_PRECISION]");
			select.AppendLine("      ,[NUMERIC_PRECISION_RADIX] = c.[NUMERIC_PRECISION_RADIX]");
			select.AppendLine("      ,[NUMERIC_SCALE] = c.[NUMERIC_SCALE]");
			select.AppendLine("      ,[DATETIME_PRECISION] = c.[DATETIME_PRECISION]");
			select.AppendLine("      ,[CHARACTER_SET_CATALOG] = c.[CHARACTER_SET_CATALOG]");
			select.AppendLine("      ,[CHARACTER_SET_SCHEMA] = c.[CHARACTER_SET_SCHEMA]");
			select.AppendLine("      ,[CHARACTER_SET_NAME] = c.[CHARACTER_SET_NAME]");
			select.AppendLine("      ,[COLLATION_CATALOG] = c.[COLLATION_CATALOG]");
			select.AppendLine("      ,[COLLATION_SCHEMA] = c.[COLLATION_SCHEMA]");
			select.AppendLine("      ,[COLLATION_NAME] = c.[COLLATION_NAME]");
			select.AppendLine("      ,[DOMAIN_CATALOG] = c.[DOMAIN_CATALOG]");
			select.AppendLine("      ,[DOMAIN_SCHEMA] = c.[DOMAIN_SCHEMA]");
			select.AppendLine("      ,[DOMAIN_NAME] = c.[DOMAIN_NAME]");
			select.AppendLine(String.Format("  FROM [{0}].[sys].[columns] sc", database));
			select.AppendLine(String.Format("  JOIN [{0}].[sys].[tables] t ON t.[object_id] = sc.[object_id]", database));
			select.AppendLine(String.Format("  JOIN [{0}].[INFORMATION_SCHEMA].[COLUMNS] c ON c.[COLUMN_NAME] = sc.[name] AND c.[TABLE_NAME] = t.[name]", database));

			if (!String.IsNullOrWhiteSpace(table)) {
				if (String.IsNullOrWhiteSpace(where))
					where = String.Format(" WHERE c.[TABLE_NAME] = '{0}'", table);
				else
					where = String.Format("{1}{2}   AND c.[TABLE_NAME] = '{0}'", table, where, Environment.NewLine);
			}

			if (!String.IsNullOrWhiteSpace(schema)) {
				if (String.IsNullOrWhiteSpace(where))
					where = String.Format(" WHERE c.[TABLE_SCHEMA] = '{0}'", schema);
				else
					where = String.Format("{1}{2}   AND c.[TABLE_SCHEMA] = '{0}'", schema, where, Environment.NewLine);
			}

			try {
				using (var conn = new SqlConnection(connectionString)) {
					using (var cmd = new SqlCommand(String.Format("{0}{1}", select, where), conn)) {
						cmd.CommandType = CommandType.Text;
						conn.Open();
						using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
							while (reader.Read()) {
								col = new Column {
									TableCatalog = reader["TABLE_CATALOG"]?.ToString(),
									TableSchema = reader["TABLE_SCHEMA"]?.ToString(),
									TableName = reader["TABLE_NAME"]?.ToString(),
									ColumnName = reader["COLUMN_NAME"]?.ToString(),
									OrdinalPosition = reader["ORDINAL_POSITION"] is DBNull ? -1 : (int) reader["ORDINAL_POSITION"],
									ColumnDefault = reader["COLUMN_DEFAULT"]?.ToString(),
									IsNullable = reader["IS_NULLABLE"] is DBNull || reader["IS_NULLABLE"].ToString().Equals("YES") ? true : false,
									IsIdentity = reader["IsIdentity"] is DBNull ? true : (bool) reader["IsIdentity"],
									IsComputed = reader["IsComputed"] is DBNull ? true : (bool) reader["IsComputed"],
									DataType = reader["DATA_TYPE"]?.ToString(),
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
								};

								list.Add(col);
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
				else {
					if (ex.InnerException.Message.Contains("system cannot find the file specified"))
						log = String.Format("{0}. Make sure the Connection String is correct.{2}Exception thrown in INNER EXCEPTION of TsqlUtil.GetColumnsInformation(string connectionString, string database='{3}', string schema='{4}', string table='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, database, schema, table);
					else
						log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of TsqlUtil.GetColumnsInformation(string connectionString, string database='{3}', string schema='{4}', string table='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, database, schema, table);
				}

				Console.Write("\n{0}", log);
				throw new Exception(log);
				#endregion Log
			}

			return list;
		}

		/// <summary>
		/// Calls all script generating method in this class and returns them as one string.
		/// </summary>
		/// <param name="columns">The list of all columns from a table.</param>
		/// <returns></returns>
		public string ScriptAll(IList<Column> columns)
		{
			if (columns == null || columns.Count < 1)
				return "";

			var sb = new StringBuilder();
			sb.AppendLine("-- =============================================");
			sb.AppendLine("-- VIEWS");
			sb.AppendLine("-- =============================================");
			sb.Append(ScriptCreateEditView(columns));
			sb.Append(ScriptCreateListView(columns));
			sb.AppendLine("-- =============================================");
			sb.AppendLine("-- STORED PROCEDURES");
			sb.AppendLine("-- =============================================");
			sb.Append(ScriptCreateProcedureDelete(columns));
			sb.Append(ScriptCreateProcedureInsert(columns));
			sb.Append(ScriptCreateProcedureUpdate(columns));
			return sb.ToString();
		}

		#region Stored Procedure
		/// <summary>
		/// Generates script text to create a SQL Stored Procedure used to Delete.
		/// </summary>
		/// <param name="columns">The list of all columns from a table.</param>
		/// <returns></returns>
		public string ScriptCreateProcedureDelete(IList<Column> columns)
		{
			var script = new StringBuilder(ScriptCreateProcedureTemplate());

			if (columns == null || columns.Count < 1)
				return script.ToString();

			columns = columns.OrderBy(x => x.TableSchema).ThenBy(x => x.TableName).ThenBy(x => x.OrdinalPosition).ToList();
			var result = new StringBuilder();
			var curSchema = "";
			var curTable = "";
			var parametersClause = "";
			var updateClause = "";
			var setClause = "";
			var whereClause = "";
			var schema = "";
			var table = "";
			var findId = true;

			foreach (var col in columns) {
				schema = col.TableSchema;
				table = col.TableName;

				// its a new table //
				if (!table.Equals(curTable) || !schema.Equals(curSchema)) {
					// if null or whitespace, its the first iteration, so NOT will catch all other times its a new table //
					if (!String.IsNullOrWhiteSpace(curTable) || !String.IsNullOrWhiteSpace(curSchema)) {
						if (String.IsNullOrWhiteSpace(setClause))
							result.Append(String.Format("-- Table [{0}].[{1}] doesn't contain any 'Delete' columns --{2}{2}{2}", curSchema, curTable, Environment.NewLine));
						else {
							if (String.IsNullOrWhiteSpace(whereClause))
								whereClause = "\t WHERE [Asdfg] = @Asdfg";

							script = script.Replace("_PARAMETERS_", parametersClause);
							script = script.Replace("_STATEMENT_CLAUSE_", String.Format("{0}{1}{2}{1}{3}", updateClause, Environment.NewLine, setClause, whereClause));
							result.Append(script);
						}
					}

					script = new StringBuilder(ScriptCreateProcedureTemplate());
					curSchema = schema;
					curTable = table;
					parametersClause = "";
					updateClause = "";
					setClause = "";
					whereClause = "";
					findId = true;

					script = script.Replace("_SCHEMA_", schema);
					script = script.Replace("_PROCEDURE_NAME_", String.Format("{0}Delete", table));
					updateClause = String.Format("\tUPDATE [{0}].[{1}]", schema, table);
				}

				if (col.ColumnName.Equals("Id")) {
					if (findId) {
						if (String.IsNullOrWhiteSpace(parametersClause))
							parametersClause = String.Format("\t @{0} {1} = NULL", col.ColumnName, col.DataTypeFull);
						else
							parametersClause = String.Format("{2}{3}\t,@{0} {1} = NULL", col.ColumnName, col.DataTypeFull, parametersClause, Environment.NewLine);

						whereClause = String.Format("\t WHERE [{0}] = @{0}", col.ColumnName);
						findId = false;
					}
				}

				else if (col.ColumnName.Equals("Ident")) {
					if (findId) {
						if (String.IsNullOrWhiteSpace(parametersClause))
							parametersClause = String.Format("\t @{0} {1} = NULL", col.ColumnName, col.DataTypeFull);
						else
							parametersClause = String.Format("{2}{3}\t,@{0} {1} = NULL", col.ColumnName, col.DataTypeFull, parametersClause, Environment.NewLine);

						whereClause = String.Format("\t WHERE [{0}] = @{0}", col.ColumnName);
						findId = false;
					}
				}

				else if (col.ColumnName.Equals("UniqueId")) {
					if (findId) {
						if (String.IsNullOrWhiteSpace(parametersClause))
							parametersClause = String.Format("\t @{0} {1} = NULL", col.ColumnName, col.DataTypeFull);
						else
							parametersClause = String.Format("{2}{3}\t,@{0} {1} = NULL", col.ColumnName, col.DataTypeFull, parametersClause, Environment.NewLine);

						whereClause = String.Format("\t WHERE [{0}] = @{0}", col.ColumnName);
						findId = false;
					}
				}

				else if (col.ColumnName.Equals("Active") || col.ColumnName.Equals("IsActive") || col.ColumnName.Equals("Published") || col.ColumnName.StartsWith("Delete")) {
					if (String.IsNullOrWhiteSpace(parametersClause))
						parametersClause = String.Format("\t @{0} {1} = NULL", col.ColumnName, col.DataTypeFull);
					else
						parametersClause = String.Format("{2}{3}\t,@{0} {1} = NULL", col.ColumnName, col.DataTypeFull, parametersClause, Environment.NewLine);

					if (String.IsNullOrWhiteSpace(setClause))
						setClause = String.Format("\t   SET [{0}] = @{0}", col.ColumnName);
					else
						setClause = String.Format("{1}{2}\t\t  ,[{0}] = @{0}", col.ColumnName, setClause, Environment.NewLine);
				}
			}

			if (String.IsNullOrWhiteSpace(setClause))
				result.Append(String.Format("-- Table [{0}].[{1}] doesn't contain any 'Delete' columns --{2}{2}{2}", schema, table, Environment.NewLine));
			else {
				if (String.IsNullOrWhiteSpace(whereClause))
					whereClause = "\t WHERE [Asdfg] = @Asdfg";

				script = script.Replace("_PARAMETERS_", parametersClause);
				script = script.Replace("_STATEMENT_CLAUSE_", String.Format("{0}{1}{2}{1}{3}", updateClause, Environment.NewLine, setClause, whereClause));
				result.Append(script);
			}

			return result.ToString();
		}

		/// <summary>
		/// Generates script text to create a SQL Stored Procedure used to Insert.
		/// </summary>
		/// <param name="columns">The list of all columns from a table.</param>
		/// <returns></returns>
		public string ScriptCreateProcedureInsert(IList<Column> columns)
		{
			var script = new StringBuilder(ScriptCreateProcedureTemplate());

			if (columns == null || columns.Count < 1)
				return script.ToString();

			columns = columns.OrderBy(x => x.TableSchema).ThenBy(x => x.TableName).ThenBy(x => x.OrdinalPosition).ToList();
			var result = new StringBuilder();
			var curSchema = "";
			var curTable = "";
			var parametersClause = "";
			var insertClause = "";
			var valuesClause = "";
			var schema = "";
			var table = "";

			foreach (var col in columns) {
				schema = col.TableSchema;
				table = col.TableName;

				// its a new table //
				if (!table.Equals(curTable) || !schema.Equals(curSchema)) {
					// if null or whitespace, its the first iteration, so NOT will catch all other times its a new table //
					if (!String.IsNullOrWhiteSpace(curTable) || !String.IsNullOrWhiteSpace(curSchema)) {
						script = script.Replace("_PARAMETERS_", parametersClause);
						script = script.Replace("_STATEMENT_CLAUSE_", String.Format("{0}){1}{2})", insertClause, Environment.NewLine, valuesClause));
						result.Append(script);
					}

					script = new StringBuilder(ScriptCreateProcedureTemplate());
					curSchema = schema;
					curTable = table;
					parametersClause = "";
					insertClause = "";
					valuesClause = "";

					script = script.Replace("_SCHEMA_", schema);
					script = script.Replace("_PROCEDURE_NAME_", String.Format("{0}Insert", table));
				}

				if (col.IsIdentity || col.IsComputed)
					continue; // skip identity & computed columns

				if (String.IsNullOrWhiteSpace(parametersClause))
					parametersClause = String.Format("{0}\t @{1} {2} = NULL", parametersClause, col.ColumnName, col.DataTypeFull);
				else
					parametersClause = String.Format("{0}{3}\t,@{1} {2} = NULL", parametersClause, col.ColumnName, col.DataTypeFull, Environment.NewLine);

				if (String.IsNullOrWhiteSpace(insertClause)) {
					insertClause = String.Format("\tINSERT INTO [{0}].[{1}]", schema, table);
					insertClause = String.Format("{0}{1}\t\t\t   ([{2}]", insertClause, Environment.NewLine, col.ColumnName);
				}
				else
					insertClause = String.Format("{0}{1}\t\t\t   ,[{2}]", insertClause, Environment.NewLine, col.ColumnName);

				if (String.IsNullOrWhiteSpace(valuesClause)) {
					valuesClause = String.Format("\t\t VALUES");
					valuesClause = String.Format("{0}{1}\t\t\t   (@{2}", valuesClause, Environment.NewLine, col.ColumnName);
				}
				else
					valuesClause = String.Format("{0}{1}\t\t\t   ,@{2}", valuesClause, Environment.NewLine, col.ColumnName);
			}

			script = script.Replace("_PARAMETERS_", parametersClause);
			script = script.Replace("_STATEMENT_CLAUSE_", String.Format("{0}){1}{2})", insertClause, Environment.NewLine, valuesClause));
			result.Append(script);
			return result.ToString();
		}

		/// <summary>
		/// Generates script text to create a SQL Stored Procedure used to Update.
		/// </summary>
		/// This method still has room for improvement. It should ignore [Id] and [UniqueId] from the SET clause.
		/// <param name="columns">The list of all columns from a table.</param>
		/// <returns></returns>
		public string ScriptCreateProcedureUpdate(IList<Column> columns)
		{
			var script = new StringBuilder(ScriptCreateProcedureTemplate());

			if (columns == null || columns.Count < 1)
				return script.ToString();

			columns = columns.OrderBy(x => x.TableSchema).ThenBy(x => x.TableName).ThenBy(x => x.OrdinalPosition).ToList();
			var result = new StringBuilder();
			var curSchema = "";
			var curTable = "";
			var parametersClause = "";
			var updateClause = "";
			var setClause = "";
			var whereClause = "";
			var schema = "";
			var table = "";
			var findId = true;

			foreach (var col in columns) {
				schema = col.TableSchema;
				table = col.TableName;

				// its a new table //
				if (!table.Equals(curTable) || !schema.Equals(curSchema)) {
					// if null or whitespace, its the first iteration, so NOT will catch all other times its a new table //
					if (!String.IsNullOrWhiteSpace(curTable) || !String.IsNullOrWhiteSpace(curSchema)) {
						if (String.IsNullOrWhiteSpace(whereClause))
							whereClause = "\t WHERE [Asdfg] = @Asdfg";

						script = script.Replace("_PARAMETERS_", parametersClause);
						script = script.Replace("_STATEMENT_CLAUSE_", String.Format("{0}{1}{2}{1}{3}", updateClause, Environment.NewLine, setClause, whereClause));
						result.Append(script);
					}

					script = new StringBuilder(ScriptCreateProcedureTemplate());
					curSchema = schema;
					curTable = table;
					parametersClause = "";
					updateClause = "";
					setClause = "";
					whereClause = "";
					findId = true;

					script = script.Replace("_SCHEMA_", schema);
					script = script.Replace("_PROCEDURE_NAME_", String.Format("{0}Update", table));
					updateClause = String.Format("\tUPDATE [{0}].[{1}]", schema, table);
				}

				if (col.ColumnName.StartsWith("Create") || col.ColumnName.StartsWith("InsertedOn"))
					continue;
				else {
					if (String.IsNullOrWhiteSpace(parametersClause))
						parametersClause = String.Format("\t @{0} {1} = NULL", col.ColumnName, col.DataTypeFull);
					else
						parametersClause = String.Format("{2}{3}\t,@{0} {1} = NULL", col.ColumnName, col.DataTypeFull, parametersClause, Environment.NewLine);

					if (col.ColumnName.Equals("Id") || col.ColumnName.Equals("Ident") || col.ColumnName.Equals("UniqueId")) {
						if (findId) {
							whereClause = String.Format("\t WHERE [{0}] = @{0}", col.ColumnName);
							findId = false;
						}
					}

					else {
						if (col.IsIdentity || col.IsComputed)
							continue; // skip identity & computed columns

						if (String.IsNullOrWhiteSpace(setClause))
							setClause = String.Format("\t   SET [{0}] = (CASE WHEN @{0} is null THEN [{0}] ELSE @{0} END)", col.ColumnName);
						else
							setClause = String.Format("{0}{1}\t\t  ,[{2}] = (CASE WHEN @{2} is null THEN [{2}] ELSE @{2} END)", setClause, Environment.NewLine, col.ColumnName);
					}
				}
			}

			if (String.IsNullOrWhiteSpace(whereClause))
				whereClause = "\t WHERE [Asdfg] = @Asdfg";

			script = script.Replace("_PARAMETERS_", parametersClause);
			script = script.Replace("_STATEMENT_CLAUSE_", String.Format("{0}{1}{2}{1}{3}", updateClause, Environment.NewLine, setClause, whereClause));
			result.Append(script);
			return result.ToString();
		}
		#endregion

		#region View
		/// <summary>
		/// Generates script text to create a SQL View used for Editing.
		/// </summary>
		/// <param name="columns">The list of all columns from a table.</param>
		/// <returns></returns>
		public string ScriptCreateEditView(IList<Column> columns)
		{
			var script = new StringBuilder(ScriptCreateViewTemplate(EditViewDescription));

			if (columns == null || columns.Count < 1)
				return script.ToString();

			var selectClause = "";
			var fromClause = "";
			var schema = "";
			var table = "";
			var alias = "";
			var joinedAlias = "";
			var isFirstCol = true;

			foreach (var col in columns) {
				if (isFirstCol) {
					schema = col.TableSchema;
					table = col.TableName;
					alias = table.Replace("_", "").Substring(0, 2).ToLower();
					script = script.Replace("_SCHEMA_", schema);
					script = script.Replace("_VIEW_NAME_", String.Format("{0}EditView", table));
					isFirstCol = false;
				}

				if (String.IsNullOrWhiteSpace(selectClause))
					selectClause = String.Format("{0}\t SELECT [{1}] = {2}.[{1}]", selectClause, col.ColumnName, alias);
				else
					selectClause = String.Format("{0}{1}\t\t   ,[{2}] = {3}.[{2}]", selectClause, Environment.NewLine, col.ColumnName, alias);

				if (String.IsNullOrWhiteSpace(fromClause))
					fromClause = String.Format("\t   FROM [{0}].[{1}].[{2}] {3}", Database, schema, table, alias);

				if (col.ColumnName.Equals("CreatedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[CreatedByFullName] = cr.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] cr ON cr.[Id] = {2}.[CreatedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.Equals("UpdatedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[UpdatedByFullName] = up.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] up ON up.[Id] = {2}.[UpdatedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.Equals("ModifiedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[UpdatedByFullName] = up.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] up ON up.[Id] = {2}.[ModifiedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.Equals("DeletedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[DeletedByFullName] = de.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] de ON de.[Id] = {2}.[DeletedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.EndsWith("Id") && !col.ColumnName.Equals("Id") && !col.ColumnName.Equals("UniqueId")) {
					joinedAlias = col.ColumnName.Replace("_", "").Substring(0, 2).ToLower();
					selectClause = String.Format("{0}{1}\t\t   ,{2}.*", selectClause, Environment.NewLine, joinedAlias);
					fromClause = String.Format("{0}{1}  LEFT JOIN [{2}].[{3}].[{4}] {5} ON {5}.[Id] = {6}.[{7}]", fromClause, Environment.NewLine, Database, schema, col.ColumnName.Replace("Id", ""), joinedAlias, alias, col.ColumnName);
				}
			}

			script = script.Replace("_SELECT_CLAUSE_", selectClause);
			script = script.Replace("_FROM_CLAUSE_", fromClause);
			return script.ToString();
		}

		/// <summary>
		/// Generates script text to create a SQL View used for Listing.
		/// </summary>
		/// <param name="columns">The list of all columns from a table.</param>
		/// <returns></returns>
		public string ScriptCreateListView(IList<Column> columns)
		{
			var script = new StringBuilder(ScriptCreateViewTemplate(ListViewDescription));

			if (columns == null || columns.Count < 1)
				return script.ToString();

			var selectClause = "";
			var fromClause = "";
			var schema = "";
			var table = "";
			var alias = "";
			var joinedAlias = "";
			var isFirstCol = true;

			foreach (var col in columns) {
				if (isFirstCol) {
					schema = col.TableSchema;
					table = col.TableName;
					alias = table.Replace("_", "").Substring(0, 2).ToLower();
					script = script.Replace("_SCHEMA_", schema);
					script = script.Replace("_VIEW_NAME_", String.Format("{0}ListView", table));
					isFirstCol = false;
				}

				if (String.IsNullOrWhiteSpace(selectClause))
					selectClause = String.Format("{0}\t SELECT [{1}] = {2}.[{1}]", selectClause, col.ColumnName, alias);
				else
					selectClause = String.Format("{0}{1}\t\t   ,[{2}] = {3}.[{2}]", selectClause, Environment.NewLine, col.ColumnName, alias);

				if (String.IsNullOrWhiteSpace(fromClause))
					fromClause = String.Format("\t   FROM [{0}].[{1}].[{2}] {3}", Database, schema, table, alias);

				if (col.ColumnName.Equals("CreatedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[CreatedByFullName] = cr.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] cr ON cr.[Id] = {2}.[CreatedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.Equals("UpdatedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[UpdatedByFullName] = up.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] up ON up.[Id] = {2}.[UpdatedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.Equals("ModifiedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[UpdatedByFullName] = up.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] up ON up.[Id] = {2}.[ModifiedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.Equals("DeletedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[DeletedByFullName] = de.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] de ON de.[Id] = {2}.[DeletedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.EndsWith("Id") && !col.ColumnName.Equals("Id") && !col.ColumnName.Equals("UniqueId")) {
					joinedAlias = col.ColumnName.Replace("_", "").Substring(0, 2).ToLower();
					selectClause = String.Format("{0}{1}\t\t   ,{2}.*", selectClause, Environment.NewLine, joinedAlias);
					fromClause = String.Format("{0}{1}  LEFT JOIN [{2}].[{3}].[{4}] {5} ON {5}.[Id] = {6}.[{7}]", fromClause, Environment.NewLine, Database, schema, col.ColumnName.Replace("Id", ""), joinedAlias, alias, col.ColumnName);
				}
			}

			script = script.Replace("_SELECT_CLAUSE_", selectClause);
			script = script.Replace("_FROM_CLAUSE_", fromClause);
			return script.ToString();
		}
		#endregion
		#endregion
	}
}