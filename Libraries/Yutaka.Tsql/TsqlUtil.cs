using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Yutaka.Data
{
	public class TsqlUtil
	{
		#region Fields
		public DateTime CreateDate;
		public string Author;
		public string CreateDateStr;
		public string Database;
		public string DateFormat;
		public string Description;
		#endregion Fields

		#region Constructor
		/// <summary>
		/// Creates an instance of TsqlUtil.
		/// </summary>
		/// <param name="database">The database.</param>
		/// <param name="author">The author's name.</param>
		/// <param name="createDate">The Create date you want to set this as. The default is Today.</param>
		/// <param name="dateFormat">The date format you want to use for the Create date and Modified date. Default is "MMM dd, yyyy".</param>
		/// <param name="description">The description. Default is "-".</param>
		public TsqlUtil(string database = null, string author = null, DateTime? createDate = null, string dateFormat = null, string description = null)
		{
			if (String.IsNullOrWhiteSpace(database))
				throw new ArgumentException("<database> is required.", "database");
			else
				Database = database.Trim();

			if (String.IsNullOrWhiteSpace(author))
				Author = "";
			else
				Author = author.Trim();

			if (createDate == null)
				CreateDate = DateTime.Today;
			else
				CreateDate = createDate.Value;

			if (String.IsNullOrWhiteSpace(dateFormat))
				CreateDateStr = CreateDate.ToString(@"MMM dd, yyyy");
			else
				CreateDateStr = CreateDate.ToString(dateFormat);

			if (String.IsNullOrWhiteSpace(description))
				Description = "-";
			else
				Description = description.Trim();
		}
		#endregion Constructor

		#region Utilities
		/// <summary>
		/// Scripts a CREATE PROCEDURE template.
		/// </summary>
		/// <returns></returns>
		protected string ScriptCreateProcedureTemplate()
		{
			var sb = new StringBuilder(ScriptHeading());
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
		protected string ScriptCreateViewTemplate()
		{
			var sb = new StringBuilder(ScriptHeading());
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
		protected string ScriptHeading()
		{
			var sb = new StringBuilder();
			sb.AppendLine(String.Format("USE [{0}]", Database));
			sb.AppendLine(String.Format("GO"));
			sb.AppendLine(String.Format("SET ANSI_NULLS ON"));
			sb.AppendLine(String.Format("GO"));
			sb.AppendLine(String.Format("SET QUOTED_IDENTIFIER ON"));
			sb.AppendLine(String.Format("GO"));
			sb.AppendLine(String.Format("-- ============================================="));
			sb.AppendLine(String.Format("-- Author:      {0}", Author));
			sb.AppendLine(String.Format("-- Create date: {0}", CreateDateStr));
			sb.AppendLine(String.Format("-- Modified:    {0}", CreateDateStr));
			sb.AppendLine(String.Format("-- Description: {0}", Description));
			sb.AppendLine(String.Format("-- ============================================="));

			return sb.ToString();
		}
		#endregion Utilities

		#region Public Methods
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
							Column col;
							while (reader.Read()) {
								col = new Column {
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
								};

								if (col.DataType.Equals("varchar") || col.DataType.Equals("nvarchar"))
									col.DataTypeFull = String.Format("{0}({1})", col.DataType, col.CharacterMaximumLength);
								else if (col.DataType.Equals("decimal"))
									col.DataTypeFull = String.Format("{0}({1},{2})", col.DataType, col.NumericPrecision, col.NumericScale);
								else
									col.DataTypeFull = col.DataType;

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
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of TsqlUtil.GetColumnsInformation(string connectionString, string database='{3}', string schema='{4}', string table='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, database, schema, table);

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
			sb.Append(ScriptCreateViewEdit(columns));
			sb.Append(ScriptCreateViewList(columns));
			sb.AppendLine("-- =============================================");
			sb.AppendLine("-- STORED PROCEDURES");
			sb.AppendLine("-- =============================================");
			sb.Append(ScriptCreateProcedureDelete(columns));
			sb.Append(ScriptCreateProcedureInsert(columns));
			sb.Append(ScriptCreateProcedureRestore(columns));
			sb.Append(ScriptCreateProcedureUpdate(columns));
			return sb.ToString();
		}

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

			var parametersClause = "";
			var updateClause = "";
			var setClause = "";
			var whereClause = "";
			var schema = "";
			var table = "";
			var findId = true;
			var isFirstCol = true;

			foreach (var col in columns) {
				if (isFirstCol) {
					schema = col.TableSchema;
					table = col.TableName;
					script = script.Replace("_SCHEMA_", schema);
					script = script.Replace("_PROCEDURE_NAME_", String.Format("{0}Delete", table));
					updateClause = String.Format("\tUPDATE [{0}].[{1}]", schema, table);
					isFirstCol = false;
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
				return String.Format("-- This table doesn't contain any 'Delete' columns --{0}{0}{0}", Environment.NewLine);

			script = script.Replace("_PARAMETERS_", parametersClause);
			script = script.Replace("_STATEMENT_CLAUSE_", String.Format("{0}{1}{2}{1}{3}", updateClause, Environment.NewLine, setClause, whereClause));
			return script.ToString();
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

			var parametersClause = "";
			var insertClause = "";
			var valuesClause = "";
			var schema = "";
			var table = "";
			var isFirstCol = true;

			foreach (var col in columns) {
				if (isFirstCol) {
					schema = col.TableSchema;
					table = col.TableName;
					script = script.Replace("_SCHEMA_", schema);
					script = script.Replace("_PROCEDURE_NAME_", String.Format("{0}Insert", table));
					isFirstCol = false;
				}

				if (col.ColumnName.Equals("Ident"))
					continue; // skip identity columns

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
			return script.ToString();
		}

		/// <summary>
		/// Generates script text to create a SQL Stored Procedure used to Restore.
		/// </summary>
		/// <param name="columns">The list of all columns from a table.</param>
		/// <returns></returns>
		public string ScriptCreateProcedureRestore(IList<Column> columns)
		{
			var script = new StringBuilder(ScriptCreateProcedureTemplate());

			if (columns == null || columns.Count < 1)
				return script.ToString();

			var parametersClause = "";
			var updateClause = "";
			var setClause = "";
			var whereClause = "";
			var schema = "";
			var table = "";
			var findId = true;
			var isFirstCol = true;

			foreach (var col in columns) {
				if (isFirstCol) {
					schema = col.TableSchema;
					table = col.TableName;
					script = script.Replace("_SCHEMA_", schema);
					script = script.Replace("_PROCEDURE_NAME_", String.Format("{0}Restore", table));
					updateClause = String.Format("\tUPDATE [{0}].[{1}]", schema, table);
					isFirstCol = false;
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

				else if (col.ColumnName.StartsWith("Delete")) {
					if (String.IsNullOrWhiteSpace(parametersClause))
						parametersClause = String.Format("\t @{0} {1} = NULL", col.ColumnName, col.DataTypeFull);
					else
						parametersClause = String.Format("{2}{3}\t,@{0} {1} = NULL", col.ColumnName, col.DataTypeFull, parametersClause, Environment.NewLine);

					if (String.IsNullOrWhiteSpace(setClause))
						setClause = String.Format("\t   SET [{0}] = NULL", col.ColumnName);
					else
						setClause = String.Format("{1}{2}\t\t  ,[{0}] = NULL", col.ColumnName, setClause, Environment.NewLine);
				}
			}

			if (String.IsNullOrWhiteSpace(setClause))
				return String.Format("-- This table doesn't contain any 'Delete' columns --{0}{0}{0}", Environment.NewLine);

			script = script.Replace("_PARAMETERS_", parametersClause);
			script = script.Replace("_STATEMENT_CLAUSE_", String.Format("{0}{1}{2}{1}{3}", updateClause, Environment.NewLine, setClause, whereClause));
			return script.ToString();
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

			var parametersClause = "";
			var updateClause = "";
			var setClause = "";
			var whereClause = "";
			var schema = "";
			var table = "";
			var findId = true;
			var isFirstCol = true;

			foreach (var col in columns) {
				if (isFirstCol) {
					schema = col.TableSchema;
					table = col.TableName;
					script = script.Replace("_SCHEMA_", schema);
					script = script.Replace("_PROCEDURE_NAME_", String.Format("{0}Update", table));
					updateClause = String.Format("\tUPDATE [{0}].[{1}]", schema, table);
					isFirstCol = false;
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
						if (String.IsNullOrWhiteSpace(setClause))
							setClause = String.Format("\t   SET [{0}] = (CASE WHEN @{0} is null THEN [{0}] ELSE @{0} END)", col.ColumnName);
						else
							setClause = String.Format("{0}{1}\t\t  ,[{2}] = (CASE WHEN @{2} is null THEN [{2}] ELSE @{2} END)", setClause, Environment.NewLine, col.ColumnName);
					}
				}
			}

			script = script.Replace("_PARAMETERS_", parametersClause);
			script = script.Replace("_STATEMENT_CLAUSE_", String.Format("{0}{1}{2}{1}{3}", updateClause, Environment.NewLine, setClause, whereClause));
			return script.ToString();
		}

		/// <summary>
		/// Generates script text to create a SQL View used for Editing.
		/// </summary>
		/// <param name="columns">The list of all columns from a table.</param>
		/// <returns></returns>
		public string ScriptCreateViewEdit(IList<Column> columns)
		{
			var script = new StringBuilder(ScriptCreateViewTemplate());

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
					script = script.Replace("_VIEW_NAME_", String.Format("{0}Edit", table));
					isFirstCol = false;
				}

				if (String.IsNullOrWhiteSpace(selectClause))
					selectClause = String.Format("{0}\t SELECT [{1}] = {2}.[{1}]", selectClause, col.ColumnName, alias);
				else
					selectClause = String.Format("{0}{1}\t\t   ,[{2}] = {3}.[{2}]", selectClause, Environment.NewLine, col.ColumnName, alias);

				if (String.IsNullOrWhiteSpace(fromClause))
					fromClause = String.Format("\t   FROM [{0}].[{1}].[{2}] {3} WITH (NOLOCK)", Database, schema, table, alias);

				if (col.ColumnName.Equals("CreatedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[CreatedByFullName] = cr.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] cr WITH (NOLOCK) ON cr.[Id] = {2}.[CreatedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.Equals("UpdatedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[UpdatedByFullName] = up.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] up WITH (NOLOCK) ON up.[Id] = {2}.[UpdatedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.Equals("ModifiedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[UpdatedByFullName] = up.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] up WITH (NOLOCK) ON up.[Id] = {2}.[ModifiedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.Equals("DeletedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[DeletedByFullName] = de.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] de WITH (NOLOCK) ON de.[Id] = {2}.[DeletedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.EndsWith("Id") && !col.ColumnName.Equals("Id") && !col.ColumnName.Equals("UniqueId")) {
					joinedAlias = col.ColumnName.Replace("_", "").Substring(0, 2).ToLower();
					selectClause = String.Format("{0}{1}\t\t   ,{2}.*", selectClause, Environment.NewLine, joinedAlias);
					fromClause = String.Format("{0}{1}  LEFT JOIN [{2}].[{3}].[{4}] {5} WITH (NOLOCK) ON {5}.[Id] = {6}.[{7}]", fromClause, Environment.NewLine, Database, schema, col.ColumnName.Replace("Id", ""), joinedAlias, alias, col.ColumnName);
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
		public string ScriptCreateViewList(IList<Column> columns)
		{
			var script = new StringBuilder(ScriptCreateViewTemplate());

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
					script = script.Replace("_VIEW_NAME_", String.Format("{0}List", table));
					isFirstCol = false;
				}

				if (String.IsNullOrWhiteSpace(selectClause))
					selectClause = String.Format("{0}\t SELECT [{1}] = {2}.[{1}]", selectClause, col.ColumnName, alias);
				else
					selectClause = String.Format("{0}{1}\t\t   ,[{2}] = {3}.[{2}]", selectClause, Environment.NewLine, col.ColumnName, alias);

				if (String.IsNullOrWhiteSpace(fromClause))
					fromClause = String.Format("\t   FROM [{0}].[{1}].[{2}] {3} WITH (NOLOCK)", Database, schema, table, alias);

				if (col.ColumnName.Equals("CreatedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[CreatedByFullName] = cr.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] cr WITH (NOLOCK) ON cr.[Id] = {2}.[CreatedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.Equals("UpdatedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[UpdatedByFullName] = up.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] up WITH (NOLOCK) ON up.[Id] = {2}.[UpdatedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.Equals("ModifiedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[UpdatedByFullName] = up.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] up WITH (NOLOCK) ON up.[Id] = {2}.[ModifiedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.Equals("DeletedById")) {
					selectClause = String.Format("{0}{1}\t\t   ,[DeletedByFullName] = de.[FullName]", selectClause, Environment.NewLine);
					fromClause = String.Format("{0}{1}  LEFT JOIN [IntranetData].[dbo].[CustomerExtra] de WITH (NOLOCK) ON de.[Id] = {2}.[DeletedById]", fromClause, Environment.NewLine, alias);
				}

				else if (col.ColumnName.EndsWith("Id") && !col.ColumnName.Equals("Id") && !col.ColumnName.Equals("UniqueId")) {
					joinedAlias = col.ColumnName.Replace("_", "").Substring(0, 2).ToLower();
					selectClause = String.Format("{0}{1}\t\t   ,{2}.*", selectClause, Environment.NewLine, joinedAlias);
					fromClause = String.Format("{0}{1}  LEFT JOIN [{2}].[{3}].[{4}] {5} WITH (NOLOCK) ON {5}.[Id] = {6}.[{7}]", fromClause, Environment.NewLine, Database, schema, col.ColumnName.Replace("Id", ""), joinedAlias, alias, col.ColumnName);
				}
			}

			script = script.Replace("_SELECT_CLAUSE_", selectClause);
			script = script.Replace("_FROM_CLAUSE_", fromClause);
			return script.ToString();
		}
		#endregion Public Methods
	}
}