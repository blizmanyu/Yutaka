using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Yutaka.Data
{
	public class SqlUtil
	{
		public const CommandType STORED_PROCEDURE = CommandType.StoredProcedure;
		public const CommandType TABLE_DIRECT = CommandType.TableDirect;
		public const CommandType TEXT_COMM_TYPE = CommandType.Text;

		public SqlUtil() { }

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

					catch (Exception ex) {
						throw ex;
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

					catch (Exception ex) {
						throw ex;
					}
				}
			}
		}

		public Object ExecuteScalar(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
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

			catch (Exception ex) {
				var p = "";

				if (parameters != null && parameters.Length > 0) {
					for (int i = 0; i < parameters.Length; i++)
						p = String.Format("{0}{1}: {2}; ", p, parameters[i].ParameterName, parameters[i].Value);

					p = String.Format("{0}{1}", p, Environment.NewLine);
				}

				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{3}{2}Exception thrown in RcwEmailService.ExecuteScalar(string connectionString, string commandText='{4}', CommandType commandType='{5}'){2}{1}", ex.Message, ex.ToString(), Environment.NewLine, p, commandText, commandType));

				throw new Exception(String.Format("{0}{2}{3}{2}Exception thrown in INNER EXCEPTION of RcwEmailService.ExecuteScalar(string connectionString, string commandText='{4}', CommandType commandType='{5}'){2}{1}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, p, commandText, commandType));
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

		public void StartJob(string connectionString, string jobId)
		{
			using (var conn = new SqlConnection(connectionString)) {
				using (var cmd = new SqlCommand()) {
					try {
						cmd.Connection = conn;
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandText = "msdb.dbo.sp_start_job";
						cmd.Parameters.AddWithValue("@job_id", jobId);
						conn.Open();
						cmd.ExecuteNonQuery();
					}

					catch (Exception ex) {
						throw ex;
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

		public void TruncateTable(string connectionString, string database=null, string schema=null, string table=null)
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

		//			catch (Exception ex) {
		//				throw ex;
		//			}
		//		}
		//	}
		//}
		#endregion Commented Out Jan, 10, 2019
	}
}