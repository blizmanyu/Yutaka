using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yutaka.Core.CSharp;
using Yutaka.Data;

namespace Yutaka.Code
{
	public class Scripter
	{
		/// <summary>
		/// Separates all columns by table, then runs all Script methods on each. Wraps each table in a #region for organization.
		/// </summary>
		/// <param name="columns">The columns.</param>
		/// <returns></returns>
		public string ScriptAll(IList<Column> columns)
		{
			#region Input Check
			var log = "";

			if (columns == null)
				log = String.Format("{0}<columns> is null.{1}", log, Environment.NewLine);
			else if (columns.Count == 0)
				log = String.Format("{0}<columns> is empty.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in Scripter.ScriptAll(IList<Column> columns).{1}{1}", log, Environment.NewLine);
				return log;
			}
			#endregion

			var finalScript = new StringBuilder();
			var table = "";

			columns = columns.OrderBy(x => x.TableSchema).ThenBy(x => x.TableName).ThenBy(x => x.OrdinalPosition).ToList();
			var grouped = columns.GroupBy(x => new { x.TableCatalog, x.TableSchema, x.TableName }).ToList();
			var last = grouped.Last();

			foreach (var tables in grouped) {
				table = tables.Key.TableName;
				finalScript.AppendLine(String.Format("\t\t#region {0}", table));
				//finalScript.Append(ScriptTryInsertMethod(tables.ToList()));
				finalScript.Append(ScriptModel(tables.ToList()));
				finalScript.AppendLine(String.Format("\t\t#endregion {0}", table));

				if (!last.Equals(tables))
					finalScript.AppendLine();
			}

			return finalScript.ToString();
		}

		/// <summary>
		/// Creates script for an MVC Model.
		/// </summary>
		/// <param name="columns">The columns information for each table.</param>
		/// <returns></returns>
		public string ScriptModel(IList<Column> columns)
		{
			#region Input Check
			var log = "";

			if (columns == null)
				log = String.Format("{0}<columns> is null.{1}", log, Environment.NewLine);
			else if (columns.Count == 0)
				log = String.Format("{0}<columns> is empty.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in Scripter.ScriptModel(IList<Column> columns).{1}{1}", log, Environment.NewLine);
				return log;
			}
			#endregion

			var finalScript = new StringBuilder();
			Class cl = null;
			Field field = null;
			var table = "";

			columns = columns.OrderBy(x => x.TableSchema).ThenBy(x => x.TableName).ThenBy(x => x.OrdinalPosition).ToList();

			foreach (var tables in columns.GroupBy(x => new { x.TableCatalog, x.TableSchema, x.TableName })) {
				table = tables.Key.TableName.Replace(".", "_");

				cl = new Class {
					AccessLevel = "public",
					BaseClass = null,
					Fields = new List<Field>(),
					Methods = new List<Method>(),
					Modifier = null,
					Name = String.Format("{0}Model", table),
					Namespace = String.Format("Yutaka.Models.{0}s", table),
					Usings = new List<string>(),
				};

				foreach (var col in tables) {
					field = new Field {
						AccessLevel = "public",
						DisplayName = col.ColumnName,
						Getter = null,
						IsAutoImplemented = true,
						Modifier = null,
						Name = col.ColumnName,
						Setter = null,
						Type = null,
						UIHint = null,
					};

					switch (col.DataType) {
						#region case "bit":
						case "bit":
							if (col.IsNullable)
								field.Type = "bool?";
							else
								field.Type = "bool";
							break;
						#endregion case "bit":
						#region case "datetime":
						case "datetime":
							if (col.IsNullable) {
								field.Type = "DateTime?";
								field.UIHint = "DateTimeNullable";
							}
							else
								field.Type = "DateTime";
							break;
						#endregion case "datetime":
						#region case "decimal" & "numeric":
						case "decimal":
						case "numeric":
							if (col.IsNullable)
								field.Type = "decimal?";
							else
								field.Type = "decimal";
							break;
						#endregion case "decimal" & "numeric":
						#region case "int":
						case "int":
							if (col.IsNullable)
								field.Type = "int?";
							else
								field.Type = "int";
							break;
						#endregion case "int":
						#region case "nvarchar" & "varchar":
						case "nvarchar":
						case "varchar":
							field.Type = "string";
							break;
						#endregion case "nvarchar" & "varchar":
						default:
							if (col.IsNullable)
								field.Type = String.Format("{0}?", col.DataType);
							else
								field.Type = col.DataType;
							break;
					}

					cl.Fields.Add(field);
				}

				finalScript.Append(cl.ToString());
			}

			return finalScript.ToString();
		}

		/// <summary>
		/// Creates script for a TryInsert method.
		/// </summary>
		/// <param name="columns">The columns information for each table.</param>
		/// <returns></returns>
		public string ScriptTryInsertMethod(IList<Column> columns)
		{
			#region Input Check
			var log = "";

			if (columns == null)
				log = String.Format("{0}<columns> is null.{1}", log, Environment.NewLine);
			else if (columns.Count == 0)
				log = String.Format("{0}<columns> is empty.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in Scripter.ScriptTryInsertMethod(IList<Column> columns).{1}{1}", log, Environment.NewLine);
				return log;
			}
			#endregion

			var finalScript = new StringBuilder();
			var checkInputBlock = new StringBuilder();
			var tryBlock = new StringBuilder();
			var catchBlock = new StringBuilder();
			Method method = null;
			var database = "";
			var schema = "";
			var table = "";
			var alias = "";

			columns = columns.OrderBy(x => x.TableSchema).ThenBy(x => x.TableName).ThenBy(x => x.OrdinalPosition).ToList();

			foreach (var tables in columns.GroupBy(x => new { x.TableCatalog, x.TableSchema, x.TableName })) {
				database = tables.Key.TableCatalog;
				schema = tables.Key.TableSchema;
				table = tables.Key.TableName.Replace(".", "_");
				alias = table.Replace("_", "").Substring(0, 2).ToLower();

				// check for C# keywords //
				if (alias.Equals("in"))
					alias = table.Replace("_", "").Substring(0, 3).ToLower();

				#region Check Input Block
				checkInputBlock = new StringBuilder();
				checkInputBlock.AppendLine("\t\t\t#region Check Input");
				checkInputBlock.AppendLine("\t\t\tresponse = \"\";");
				checkInputBlock.AppendLine();
				checkInputBlock.AppendLine(String.Format("\t\t\tif ({0} == null)", alias));
				checkInputBlock.AppendLine(String.Format("\t\t\t\tresponse = String.Format(\"{{0}}<{0}> is required.{{1}}\", response, Environment.NewLine);", alias));
				checkInputBlock.AppendLine();
				checkInputBlock.AppendLine("\t\t\tif (!String.IsNullOrWhiteSpace(response)) {");
				checkInputBlock.AppendLine(String.Format("\t\t\t\tresponse = String.Format(\"{{0}}Exception thrown in {0}Service.TryInsert{1}({1} {2}, out string response).{{1}}\", response, Environment.NewLine); ", database, table, alias));
				checkInputBlock.AppendLine("\t\t\t\treturn false;");
				checkInputBlock.AppendLine("\t\t\t}");
				checkInputBlock.AppendLine("\t\t\t#endregion");
				#endregion Check Input Block

				#region Open Try Block
				tryBlock = new StringBuilder();
				tryBlock.AppendLine("\t\t\ttry {");
				tryBlock.AppendLine(String.Format("\t\t\t\tvar storProc = \"[dbo].[{0}Insert]\";", table));
				tryBlock.AppendLine("\t\t\t\tSqlParameter[] parameters = {");
				#endregion Open Try Block

				foreach (var col in tables) {
					if (!col.IsIdentity && !col.IsComputed) {
						tryBlock.Append(String.Format("\t\t\t\t\tnew SqlParameter(\"@{0}\", {1}.{0}", col.ColumnName, alias));

						if (col.IsNullable) {
							if (col.ColumnName.StartsWith("Delete"))
								tryBlock.AppendLine(" ?? null),");
							else if (col.DataType.Equals("bit") || col.DataType.Equals("datetime"))
								tryBlock.AppendLine(" ?? null),");
							else if (col.DataType.Equals("int") || col.DataType.Equals("decimal") || col.DataType.Equals("numeric"))
								tryBlock.AppendLine(" ?? -1),");
							else
								tryBlock.AppendLine(" ?? \"\"),");
						}

						else if (col.DataType.Equals("varchar") || col.DataType.Equals("nvarchar"))
							tryBlock.AppendLine(" ?? \"\"),");
						else
							tryBlock.AppendLine("),");
					}
				}

				#region Close Try Block
				tryBlock.AppendLine("\t\t\t\t};");
				tryBlock.AppendLine("\t\t\t\tExecuteStoredProcedure(storProc, parameters);");
				tryBlock.AppendLine("\t\t\t\treturn true;");
				tryBlock.AppendLine("\t\t\t}");
				#endregion Close Try Block

				#region Catch Block
				catchBlock = new StringBuilder();
				catchBlock.AppendLine("\t\t\tcatch (Exception ex) {");
				catchBlock.AppendLine("\t\t\t\t#region Log");
				catchBlock.AppendLine("\t\t\t\tif (ex.InnerException == null)");
				catchBlock.AppendLine(String.Format("\t\t\t\t\tresponse = String.Format(\"{{0}}{{2}}Exception thrown in {0}Service.TryInsert{1}({1} {2}, out string response).{{2}}{{1}}{{2}}{{2}}\", ex.Message, ex.ToString(), Environment.NewLine);", database, table, alias));
				catchBlock.AppendLine("\t\t\t\telse");
				catchBlock.AppendLine(String.Format("\t\t\t\t\tresponse = String.Format(\"{{0}}{{2}}Exception thrown in INNER EXCEPTION of {0}Service.TryInsert{1}({1} {2}, out string response).{{2}}{{1}}{{2}}{{2}}\", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);", database, table, alias));
				catchBlock.AppendLine("\t\t\t\t#endregion Log");
				catchBlock.AppendLine();
				catchBlock.AppendLine("\t\t\t\treturn false;");
				catchBlock.AppendLine("\t\t\t}");
				#endregion Catch Block

				method = new Method {
					AccessLevel = "public",
					Body = "",
					Modifier = null,
					Name = String.Format("TryInsert{0}", table),
					Parameters = String.Format("{0} {1}, out string response", table, alias),
					ReturnType = "bool",
				};

				if (!String.IsNullOrWhiteSpace(checkInputBlock.ToString()))
					method.Body = String.Format("{0}{1}{2}", method.Body, checkInputBlock, Environment.NewLine);
				if (!String.IsNullOrWhiteSpace(tryBlock.ToString()))
					method.Body = String.Format("{0}{1}{2}", method.Body, tryBlock, Environment.NewLine);
				if (!String.IsNullOrWhiteSpace(catchBlock.ToString()))
					method.Body = String.Format("{0}{1}", method.Body, catchBlock);

				finalScript.Append(method.ToString());
			}

			return finalScript.ToString();
		}
	}
}