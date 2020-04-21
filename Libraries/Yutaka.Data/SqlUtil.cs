using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

namespace Yutaka.Data
{
	public class SqlUtil
	{
		#region Fields
		// CommandBehavior //
		/// <summary>
		/// When the command is executed, the associated <c>Connection</c> object is closed when the associated <c>DataReader</c> object is closed.
		/// </summary>
		public const CommandBehavior CommandBehavior_CloseConnection = CommandBehavior.CloseConnection;
		/// <summary>
		/// The query may return multiple result sets. Execution of the query may affect the database state. <c>Default</c> sets no <c>CommandBehavior</c> flags, so calling <c>ExecuteReader(CommandBehavior.Default)</c> is functionally equivalent to calling <c>ExecuteReader()</c>.
		/// </summary>
		public const CommandBehavior CommandBehavior_Default = CommandBehavior.Default;
		/// <summary>
		/// The query returns column and primary key information. The provider appends extra columns to the result set for existing primary key and timestamp columns.
		/// </summary>
		public const CommandBehavior CommandBehavior_KeyInfo = CommandBehavior.KeyInfo;
		/// <summary>
		/// The query returns column information only. When using <c>SchemaOnly</c>, the .NET Framework Data Provider for SQL Server precedes the statement being executed with SET FMTONLY ON.
		/// </summary>
		public const CommandBehavior CommandBehavior_SchemaOnly = CommandBehavior.SchemaOnly;
		/// <summary>
		/// Provides a way for the DataReader to handle rows that contain columns with large binary values. Rather than loading the entire row, SequentialAccess enables the DataReader to load data as a stream. You can then use the GetBytes or GetChars method to specify a byte location to start the read operation, and a limited buffer size for the data being returned.
		/// </summary>
		public const CommandBehavior CommandBehavior_SequentialAccess = CommandBehavior.SequentialAccess;
		/// <summary>
		/// The query returns a single result set.
		/// </summary>
		public const CommandBehavior CommandBehavior_SingleResult = CommandBehavior.SingleResult;
		/// <summary>
		/// The query is expected to return a single row of the first result set. Execution of the query may affect the database state. Some .NET Framework data providers may, but are not required to, use this information to optimize the performance of the command. When you specify SingleRow with the ExecuteReader() method of the OleDbCommand object, the .NET Framework Data Provider for OLE DB performs binding using the OLE DB IRow interface if it is available. Otherwise, it uses the IRowset interface. If your SQL statement is expected to return only a single row, specifying SingleRow can also improve application performance. It is possible to specify SingleRow when executing queries that are expected to return multiple result sets. In that case, where both a multi-result set SQL query and single row are specified, the result returned will contain only the first row of the first result set. The other result sets of the query will not be returned.
		/// </summary>
		public const CommandBehavior CommandBehavior_SingleRow = CommandBehavior.SingleRow;

		// CommandType //
		/// <summary>
		/// The name of a stored procedure.
		/// </summary>
		public const CommandType CommandType_StoredProcedure = CommandType.StoredProcedure;
		/// <summary>
		/// The name of a table.
		/// </summary>
		public const CommandType CommandType_TableDirect = CommandType.TableDirect;
		/// <summary>
		/// An SQL text command. (Default)
		/// </summary>
		public const CommandType CommandType_Text = CommandType.Text;
		public const CommandType STORED_PROCEDURE = CommandType.StoredProcedure;
		public const CommandType TABLE_DIRECT = CommandType.TableDirect;
		public const CommandType TEXT_COMM_TYPE = CommandType.Text;

		protected string ConnectionString;
		#endregion Fields

		#region Public Methods
		/// <summary>
		/// Checks whether or not we can connect and execute a simple query on the SQL Server.
		/// </summary>
		/// <param name="connectionString">The connection string used to open the SQL Server database.</param>
		/// <param name="retries">The number of retry attempts. Default is 1.</param>
		/// <returns>True if it can connect and execute. False otherwise.</returns>
		public bool CanExecute(string connectionString, int retries = 1)
		{
			if (String.IsNullOrWhiteSpace(connectionString))
				return false;

			try {
				var result = ExecuteScalar(connectionString, "SELECT 1");

				if (result == null)
					return false;

				if ((int) result == 1)
					return true;

				return false;
			}

			catch (Exception) {
				if (retries > 0) {
					Thread.Sleep(2200);
					return CanExecute(connectionString, --retries);
				}

				return false;
			}
		}

		public void ExecuteNonQuery(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			using (var conn = new SqlConnection(connectionString)) {
				using (var cmd = new SqlCommand(commandText, conn)) {
					cmd.CommandType = commandType;
					try {
						cmd.Parameters.AddRange(parameters);
						conn.Open();
						cmd.ExecuteNonQuery();
					}

					catch (Exception) {
						throw;
					}
				}
			}
		}

		// This method should never actually be called. It is provided as an example only since the reader will get destroyed before returning. You
		// may want to use the GetData method instead.
		public void ExecuteReader(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			using (var conn = new SqlConnection(connectionString)) {
				using (var cmd = new SqlCommand(commandText, conn)) {
					cmd.CommandType = commandType;
					try {
						cmd.Parameters.AddRange(parameters);
						conn.Open();
						using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
							while (reader.Read())
								Console.WriteLine(String.Format("{0}", reader[0]));
						}
					}

					catch (Exception) {
						throw;
					}
				}
			}
		}

		public object ExecuteScalar(string connectionString, string commandText, CommandType commandType = CommandType.Text, params SqlParameter[] parameters)
		{
			try {
				using (var conn = new SqlConnection(connectionString)) {
					using (var cmd = new SqlCommand(commandText, conn)) {
						cmd.CommandType = commandType;
						cmd.Parameters.AddRange(parameters);
						conn.Open();
						return cmd.ExecuteScalar();
					}
				}
			}

			catch (SqlException ex) {
				var log = String.Format("Exception thrown in SqlUtil.ExecuteScalar().{0}", Environment.NewLine);

				for (int i = 0; i < ex.Errors.Count; i++) {
					log = String.Format("{0}Index #{1}{2}", log, i, Environment.NewLine);
					log = String.Format("{0}Message: {1}{2}", log, ex.Errors[i].Message, Environment.NewLine);
					log = String.Format("{0}LineNumber: {1}{2}", log, ex.Errors[i].LineNumber, Environment.NewLine);
					log = String.Format("{0}Source: {1}{2}", log, ex.Errors[i].Source, Environment.NewLine);
					log = String.Format("{0}Procedure: {1}{2}", log, ex.Errors[i].Procedure, Environment.NewLine);
				}

				throw new Exception(log);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in SqlUtil.ExecuteScalar().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine));
				else
					throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of SqlUtil.ExecuteScalar().{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine));
			}
		}

		public DataSet GetData(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			var ds = new DataSet();

			try {
				using (var conn = new SqlConnection(connectionString)) {
					using (var cmd = new SqlCommand(commandText, conn)) {
						cmd.CommandType = commandType;
						cmd.Parameters.AddRange(parameters);
						using (var adapter = new SqlDataAdapter(cmd)) {
							adapter.Fill(ds);
						}
						return ds;
					}
				}
			}

			catch (Exception ex) {
				var p = "";

				if (parameters != null && parameters.Length > 0) {
					for (int i = 0; i < parameters.Length; i++)
						p = String.Format("{0}{1}: {2}; ", p, parameters[i].ParameterName, parameters[i].Value);

					p = String.Format("{0}{1}", p, Environment.NewLine);
				}

				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{3}{2}Exception thrown in SqlUtil.GetData(string connectionString, string commandText='{4}', CommandType commandType='{5}'){2}{1}", ex.Message, ex.ToString(), Environment.NewLine, p, commandText, commandType));

				throw new Exception(String.Format("{0}{2}{3}{2}Exception thrown in INNER EXCEPTION of SqlUtil.GetData(string connectionString, string commandText='{4}', CommandType commandType='{5}'){2}{1}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, p, commandText, commandType));
			}
		}

		public void StartJob(string connectionString, string jobId, string stepName = null)
		{
			using (var conn = new SqlConnection(connectionString)) {
				using (var cmd = new SqlCommand()) {
					try {
						cmd.Connection = conn;
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandText = "msdb.dbo.sp_start_job";
						cmd.Parameters.AddWithValue("@job_id", jobId);

						if (!String.IsNullOrWhiteSpace(stepName))
							cmd.Parameters.AddWithValue("@step_name", stepName);

						conn.Open();
						cmd.ExecuteNonQuery();
					}

					catch (Exception) {
						throw;
					}
				}
			}
		}

		public void ToCsv(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			try {
				using (var conn = new SqlConnection(connectionString)) {
					using (var cmd = new SqlCommand(commandText, conn)) {
						cmd.CommandType = commandType;
						cmd.Parameters.AddRange(parameters);
						conn.Open();
						using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
							using (var fs = new StreamWriter(String.Format(@"C:\TEMP\{0:yyyy MMdd HHmm ssff}.csv", DateTime.Now))) {
								// Loop through the fields and add headers
								var line = "";
								string name, value;

								for (int i = 0; i < reader.FieldCount; i++) {
									name = reader.GetName(i);

									if (name.Contains("\""))
										name = name.Replace("\"", "'");
									if (name.Contains(","))
										name = String.Format("\"{0}\"", name);

									line = String.Format("{0}{1},", line, name);
								}

								fs.WriteLine(line);

								// Loop through the rows and output the data
								while (reader.Read()) {
									line = "";

									for (int i = 0; i < reader.FieldCount; i++) {
										value = reader[i].ToString();

										if (value.Contains("\""))
											value = value.Replace("\"", "'");
										if (value.Contains(","))
											value = String.Format("\"{0}\"", value);

										line = String.Format("{0}{1},", line, value);
									}

									fs.WriteLine(line);
								}
							}
						}
					}
				}
			}

			catch (Exception ex) {
				var p = "";

				if (parameters != null && parameters.Length > 0) {
					for (int i = 0; i < parameters.Length; i++)
						p = String.Format("{0}{1}: {2}; ", p, parameters[i].ParameterName, parameters[i].Value);

					p = String.Format("{0}{1}", p, Environment.NewLine);
				}

				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{3}{2}Exception thrown in SqlUtil.ToCsv(string connectionString, string commandText='{4}', CommandType commandType='{5}'){2}{1}", ex.Message, ex.ToString(), Environment.NewLine, p, commandText, commandType));

				throw new Exception(String.Format("{0}{2}{3}{2}Exception thrown in INNER EXCEPTION of SqlUtil.ToCsv(string connectionString, string commandText='{4}', CommandType commandType='{5}'){2}{1}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, p, commandText, commandType));
			}
		}

		public void ToXls(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			try {
				using (var conn = new SqlConnection(connectionString)) {
					using (var cmd = new SqlCommand(commandText, conn)) {
						cmd.CommandType = commandType;
						cmd.Parameters.AddRange(parameters);
						conn.Open();
						using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
							using (var fs = new StreamWriter(String.Format(@"C:\TEMP\{0:yyyy MMdd HHmm ssff}.xls", DateTime.Now))) {
								// Loop through the fields and add headers
								var line = "";

								for (int i = 0; i < reader.FieldCount; i++)
									line = String.Format("{0}{1}\t", line, reader.GetName(i));

								fs.WriteLine(line);

								// Loop through the rows and output the data
								while (reader.Read()) {
									line = "";

									for (int i = 0; i < reader.FieldCount; i++)
										line = String.Format("{0}{1}\t", line, reader[i]);

									fs.WriteLine(line);
								}
							}
						}
					}
				}
			}

			catch (Exception ex) {
				var p = "";

				if (parameters != null && parameters.Length > 0) {
					for (int i = 0; i < parameters.Length; i++)
						p = String.Format("{0}{1}: {2}; ", p, parameters[i].ParameterName, parameters[i].Value);

					p = String.Format("{0}{1}", p, Environment.NewLine);
				}

				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{3}{2}Exception thrown in SqlUtil.ToXls(string connectionString, string commandText='{4}', CommandType commandType='{5}'){2}{1}", ex.Message, ex.ToString(), Environment.NewLine, p, commandText, commandType));

				throw new Exception(String.Format("{0}{2}{3}{2}Exception thrown in INNER EXCEPTION of SqlUtil.ToXls(string connectionString, string commandText='{4}', CommandType commandType='{5}'){2}{1}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, p, commandText, commandType));
			}
		}

		public void TruncateTable(string connectionString, string database = null, string schema = null, string table = null)
		{
			if (String.IsNullOrWhiteSpace(connectionString))
				throw new Exception(String.Format("<connectionString> is required.{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(database))
				database = "";
			if (String.IsNullOrWhiteSpace(schema))
				schema = "dbo";
			if (String.IsNullOrWhiteSpace(table))
				throw new Exception(String.Format("<table> is required.{0}", Environment.NewLine));

			try {
				var sql = "TRUNCATE TABLE ";

				if (!String.IsNullOrWhiteSpace(database))
					sql = String.Format("{0}{1}.", sql, database);

				sql = String.Format("{0}{1}.{2}", sql, schema, table);

				Console.Write("\n{0}", sql);

				using (var conn = new SqlConnection(connectionString)) {
					using (var cmd = new SqlCommand(sql, conn)) {
						conn.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in SqlUtil.TruncateTable(string connectionString, string database='{3}', string schema='{4}', string table='{5}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, database, schema, table));

				throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of SqlUtil.TruncateTable(string connectionString, string database='{3}', string schema='{4}', string table='{5}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, database, schema, table));
			}
		}

		#region Deprecated
		[Obsolete("Deprecated Dec 26, 2019. Use CanExecute(string connectionString, int retries = 1) instead.", true)]
		public bool IsServerConnected(string connectionString)
		{
			using (var conn = new SqlConnection(connectionString)) {
				try {
					conn.Open();
					return true;
				}
				catch (SqlException) {
					return false;
				}
			}
		}
		#endregion Deprecated

		#region Commented Out Jan, 10, 2019
		//public void ExecuteScalar(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
		//{
		//	using (var conn = new SqlConnection(connectionString)) {
		//		using (var cmd = new SqlCommand(commandText, conn)) {
		//			cmd.CommandType = commandType;
		//			try {
		//				cmd.Parameters.AddRange(parameters);
		//				conn.Open();
		//				cmd.ExecuteScalar();
		//			}

				//catch (Exception) {
				//	throw;
		//			}
		//		}
		//	}
		//}
		#endregion Commented Out Jan, 10, 2019
		#endregion Public Methods
	}
}