using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yutaka.Core.CSharp;
using Yutaka.Data;

namespace Yutaka.Code
{
	public class Scripter
	{
		/// <summary>
		/// WIP: Do NOT use yet!
		/// </summary>
		/// <param name="columns"></param>
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
			var curSchema = "";
			var curTable = "";
			var schema = "";
			var table = "";
			var alias = "";

			columns = columns.OrderBy(x => x.TableSchema).ThenBy(x => x.TableName).ThenBy(x => x.OrdinalPosition).ToList();

			foreach (var tables in columns.GroupBy(x => new { x.TableSchema, x.TableName })) {
				schema = tables.Key.TableSchema;
				table = tables.Key.TableName;
				alias = table.Replace("_", "").Substring(0, 2).ToLower();

				method = new Method {
					AccessLevel = "public",
					Body = "",
					Modifier = null,
					Name = table,
					Parameters = String.Format("{0} {1}, out string response", table, alias),
					ReturnType = "bool",
				};

				foreach (var col in tables) {
					col.DumpToConsole();
				}

				finalScript.AppendLine(method.ToString());
			}

			return finalScript.ToString();



			foreach (var col in columns) {
				schema = col.TableSchema;
				table = col.TableName;
				alias = table.Replace("_", "").Substring(0, 2).ToLower();

				// its a new table //
				if (!table.Equals(curTable) || !schema.Equals(curSchema)) {
					// if null or whitespace, its the first iteration, so NOT will catch all other times its a new table //
					if (!String.IsNullOrWhiteSpace(curTable) || !String.IsNullOrWhiteSpace(curSchema)) {
						//script = script.Replace("_PARAMETERS_", parametersClause);
						//script = script.Replace("_STATEMENT_CLAUSE_", String.Format("{0}){1}{2})", insertClause, Environment.NewLine, valuesClause));
						method.Body = String.Format("{0}{3}{1}{3}{2}", checkInputBlock, tryBlock, catchBlock, Environment.NewLine);
						finalScript.Append(method);
					}

					method = new Method {
						AccessLevel = "public",
						Body = "",
						Modifier = "",
						Name = String.Format("TryInsert{0}", table),
						Parameters = String.Format("{0} {1}, out string response", table, alias),
						ReturnType = "bool",
					};

					curSchema = schema;
					curTable = table;
					//body = new StringBuilder();
					//body.AppendLine(String.Format("\t\tif ({0} == null) {", alias));
					//body.AppendLine("\t\t\treturn;");
				}

				if (col.IsIdentity || col.IsComputed)
					continue; // skip identity & computed columns

				//if (String.IsNullOrWhiteSpace(parametersClause))
				//	parametersClause = String.Format("{0}\t @{1} {2} = NULL", parametersClause, col.ColumnName, col.DataTypeFull);
				//else
				//	parametersClause = String.Format("{0}{3}\t,@{1} {2} = NULL", parametersClause, col.ColumnName, col.DataTypeFull, Environment.NewLine);

				//if (String.IsNullOrWhiteSpace(insertClause)) {
				//	insertClause = String.Format("\tINSERT INTO [{0}].[{1}]", schema, table);
				//	insertClause = String.Format("{0}{1}\t\t\t   ([{2}]", insertClause, Environment.NewLine, col.ColumnName);
				//}
				//else
				//	insertClause = String.Format("{0}{1}\t\t\t   ,[{2}]", insertClause, Environment.NewLine, col.ColumnName);

				//if (String.IsNullOrWhiteSpace(valuesClause)) {
				//	valuesClause = String.Format("\t\t VALUES");
				//	valuesClause = String.Format("{0}{1}\t\t\t   (@{2}", valuesClause, Environment.NewLine, col.ColumnName);
				//}
				//else
				//	valuesClause = String.Format("{0}{1}\t\t\t   ,@{2}", valuesClause, Environment.NewLine, col.ColumnName);
			}

			//script = script.Replace("_PARAMETERS_", parametersClause);
			//script = script.Replace("_STATEMENT_CLAUSE_", String.Format("{0}){1}{2})", insertClause, Environment.NewLine, valuesClause));
			finalScript.Append(method);



			return finalScript.ToString();
		}
	}
}