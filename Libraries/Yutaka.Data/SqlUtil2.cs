using System;
using System.Data;
using System.Data.SqlClient;

namespace Yutaka.Data
{
	public class SqlUtil2
	{
		#region Fields
		protected string ConnectionString;
		/// <summary>
		/// An SQL text command. (Default.)
		/// </summary>
		public static readonly CommandType CommandType_Text = CommandType.Text;
		/// <summary>
		/// The name of a stored procedure.
		/// </summary>
		public static readonly CommandType CommandType_StoredProcedure = CommandType.StoredProcedure;
		public static readonly DateTime SqlDateTime_MinValue = new DateTime(1753, 1, 1);
		public const CommandBehavior CommandBehavior_CloseConnection = CommandBehavior.CloseConnection;
		#endregion Fields

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="SqlUtil2"/> class with a connection string.
		/// </summary>
		/// <param name="connectionString">A valid connection string.</param>
		public SqlUtil2(string connectionString = null)
		{
			if (!String.IsNullOrWhiteSpace(connectionString))
				ConnectionString = connectionString;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlUtil2"/> class.
		/// </summary>
		/// <param name="server">The name or network address of the instance of SQL Server to connect to.</param>
		/// <param name="username">The username to be used when connecting to SQL Server.</param>
		/// <param name="password">The password for the SQL Server account.</param>
		public SqlUtil2(string server, string username, string password)
		{
			#region Check Input
			var log = "";

			if (String.IsNullOrWhiteSpace(server))
				log = String.Format("{0}'server' is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(username))
				log = String.Format("{0}'username' is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(password))
				log = String.Format("{0}'password' is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in Constructor SqlUtil2(string server, string username, string password).{1}", log, Environment.NewLine));
			#endregion Check Input

			ConnectionString = String.Format("data source={0};initial catalog=master;user id={1};password={2};asynchronous processing=True;multipleactiveresultsets=True;App=EntityFramework", server, username, password);
		}
		#endregion Constructor

		#region Public Methods
		/// <summary>
		/// Builds and returns a connection string from the given parameters.
		/// </summary>
		/// <param name="server">The name or network address of the instance of SQL Server to connect to.</param>
		/// <param name="username">The username to be used when connecting to SQL Server.</param>
		/// <param name="password">The password for the SQL Server account.</param>
		/// <param name="database">The name of the specific database to connect to. Default is [master].</param>
		/// <returns></returns>
		public string BuildConnectionString(string server = null, string username = null, string password = null, string database = null)
		{
			#region Check Input
			var log = "";

			if (String.IsNullOrWhiteSpace(server))
				log = String.Format("{0}'server' is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(username))
				log = String.Format("{0}'username' is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(password))
				log = String.Format("{0}'password' is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(database))
				database = "master";

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in BuildConnectionString(string server = null, string username = null, string password = null, string database = null).{1}", log, Environment.NewLine));
			#endregion Check Input

			return String.Format("data source={0};initial catalog={3};user id={1};password={2};asynchronous processing=True;multipleactiveresultsets=True;App=EntityFramework", server, username, password, database);
		}

		/// <summary>
		/// Checks whether or not we can connect and execute a simple query on the SQL Server.
		/// </summary>
		/// <returns>True if it can connect and execute. False otherwise.</returns>
		public bool CanExecute()
		{
			try {
				var result = ExecuteScalar("SELECT 1");

				if (result == null)
					return false;

				if ((int) result == 1)
					return true;

				return false;
			}

			catch (Exception) {
				return false;
			}
		}

		/// <summary>
		/// Executes a Transact-SQL statement against the connection.
		/// </summary>
		/// <param name="commandText">The Transact-SQL statement or stored procedure to execute.</param>
		/// <param name="commandType">One of the <see cref="CommandType"/> values.</param>
		/// <param name="parameters">The parameters of the Transact-SQL statement or stored procedure. The default is an empty collection.</param>
		public void ExecuteNonQuery(string commandText, CommandType commandType = CommandType.Text, params SqlParameter[] parameters)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(ConnectionString))
				log = String.Format("{0}'ConnectionString' is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(commandText))
				log = String.Format("{0}'commandText' is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in SqlUtil2.ExecuteNonQuery(string commandText, CommandType commandType, params SqlParameter[] parameters).{1}{1}", log, Environment.NewLine));
			#endregion Input Check

			try {
				using (var conn = new SqlConnection(ConnectionString)) {
					using (var cmd = new SqlCommand(commandText, conn)) {
						cmd.CommandType = commandType;
						cmd.Parameters.AddRange(parameters);
						conn.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}_PARAMS_Exception thrown in SqlUtil2.ExecuteNonQuery(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, commandText, commandType);
				else
					log = String.Format("{0}{2}_PARAMS_Exception thrown in INNER EXCEPTION of SqlUtil2.ExecuteNonQuery(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, commandText, commandType);

				log = log.Replace("_PARAMS_", ToString(parameters, true));
				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// Sends the CommandText to the Connection and builds a <see cref="SqlDataReader"/>.
		/// </summary>
		/// <param name="commandText">The Transact-SQL statement or stored procedure to execute.</param>
		/// <param name="commandType">One of the <see cref="CommandType"/> values.</param>
		/// <param name="parameters">The parameters of the Transact-SQL statement or stored procedure. The default is an empty collection.</param>
		/// <returns>A <see cref="SqlDataReader"/> object</returns>
		public SqlDataReader ExecuteReader(string commandText, CommandType commandType = CommandType.Text, params SqlParameter[] parameters)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(ConnectionString))
				log = String.Format("{0}'ConnectionString' is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(commandText))
				log = String.Format("{0}'commandText' is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in SqlUtil2.ExecuteReader(string commandText, CommandType commandType, params SqlParameter[] parameters).{1}{1}", log, Environment.NewLine));
			#endregion Input Check

			try {
				using (var conn = new SqlConnection(ConnectionString)) {
					using (var cmd = new SqlCommand(commandText, conn)) {
						cmd.CommandType = commandType;
						cmd.Parameters.AddRange(parameters);
						conn.Open();
						return cmd.ExecuteReader();
					}
				}
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}_PARAMS_Exception thrown in SqlUtil2.ExecuteReader(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, commandText, commandType);
				else
					log = String.Format("{0}{2}_PARAMS_Exception thrown in INNER EXCEPTION of SqlUtil2.ExecuteReader(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, commandText, commandType);

				log = log.Replace("_PARAMS_", ToString(parameters, true));
				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored. Returns a maximum of 2033 characters.
		/// </summary>
		/// <param name="commandText">The Transact-SQL statement or stored procedure to execute.</param>
		/// <param name="commandType">One of the <see cref="CommandType"/> values.</param>
		/// <param name="parameters">The parameters of the Transact-SQL statement or stored procedure. The default is an empty collection.</param>
		/// <returns>The first column of the first row in the result set, or a null reference if the result set is empty. Returns a maximum of 2033 characters.</returns>
		public object ExecuteScalar(string commandText, CommandType commandType = CommandType.Text, params SqlParameter[] parameters)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(ConnectionString))
				log = String.Format("{0}'ConnectionString' is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(commandText))
				log = String.Format("{0}'commandText' is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in SqlUtil2.ExecuteScalar(string commandText, CommandType commandType, params SqlParameter[] parameters).{1}{1}", log, Environment.NewLine));
			#endregion Input Check

			try {
				using (var conn = new SqlConnection(ConnectionString)) {
					using (var cmd = new SqlCommand(commandText, conn)) {
						cmd.CommandType = commandType;
						cmd.Parameters.AddRange(parameters);
						conn.Open();
						return cmd.ExecuteScalar();
					}
				}
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}_PARAMS_Exception thrown in SqlUtil2.ExecuteScalar(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, commandText, commandType);
				else
					log = String.Format("{0}{2}_PARAMS_Exception thrown in INNER EXCEPTION of SqlUtil2.ExecuteScalar(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, commandText, commandType);

				log = log.Replace("_PARAMS_", ToString(parameters, true));
				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// Excecutes a Stored Procedure.
		/// </summary>
		/// <param name="storProcName">The name of the Stored Procedure to execute.</param>
		/// <param name="parameters">The parameters of the stored procedure. The default is an empty collection.</param>
		public void ExecuteStoredProcedure(string storProcName, params SqlParameter[] parameters)
		{
			if (String.IsNullOrWhiteSpace(storProcName))
				return;

			ExecuteNonQuery(storProcName, CommandType_StoredProcedure, parameters);
		}

		/// <summary>
		/// Sends the CommandText to the Connection and loads the data into a <see cref="DataTable"/>.
		/// </summary>
		/// <param name="commandText">The Transact-SQL statement or stored procedure to execute.</param>
		/// <param name="commandType">One of the <see cref="CommandType"/> values.</param>
		/// <param name="parameters">The parameters of the Transact-SQL statement or stored procedure. The default is an empty collection.</param>
		/// <returns>A <see cref="DataTable"/> object</returns>
		public DataTable GetData(string commandText, CommandType commandType = CommandType.Text, params SqlParameter[] parameters)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(ConnectionString))
				log = String.Format("{0}'ConnectionString' is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(commandText))
				log = String.Format("{0}'commandText' is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in SqlUtil2.ExecuteReader(string commandText, CommandType commandType, params SqlParameter[] parameters).{1}{1}", log, Environment.NewLine));
			#endregion Input Check

			var dt = new DataTable();

			try {
				using (var conn = new SqlConnection(ConnectionString)) {
					using (var cmd = new SqlCommand(commandText, conn)) {
						cmd.CommandType = commandType;
						cmd.Parameters.AddRange(parameters);
						conn.Open();
						dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));
						return dt;
					}
				}
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}_PARAMS_Exception thrown in SqlUtil2.GetData(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, commandText, commandType);
				else
					log = String.Format("{0}{2}_PARAMS_Exception thrown in INNER EXCEPTION of SqlUtil2.GetData(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, commandText, commandType);

				log = log.Replace("_PARAMS_", ToString(parameters, true));
				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// Starts a SQL Agent job.
		/// </summary>
		/// <param name="jobId">The ID of the job to start.</param>
		/// <param name="stepName">Optional step name to start at.</param>
		public void StartJob(string jobId, string stepName = null)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(ConnectionString))
				log = String.Format("{0}'ConnectionString' is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(jobId))
				log = String.Format("{0}'jobId' is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in SqlUtil2.StartJob(string jobId, string stepName).{1}{1}", log, Environment.NewLine));
			#endregion Input Check

			try {
				using (var conn = new SqlConnection(ConnectionString)) {
					using (var cmd = new SqlCommand("msdb.dbo.sp_start_job", conn)) {
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@job_id", jobId);

						if (!String.IsNullOrWhiteSpace(stepName))
							cmd.Parameters.AddWithValue("@step_name", stepName);

						conn.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in SqlUtil2.StartJob(string jobId='{3}', string stepName='{4}').{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, jobId, stepName);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of SqlUtil2.StartJob(string jobId='{3}', string stepName='{4}').{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, jobId, stepName);

				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// Returns a string that represents all <see cref="SqlParameter.ParameterName"/>s and their <see cref="SqlParameter.Value"/>s.
		/// </summary>
		/// <param name="parameters">The array of <see cref="SqlParameter"/>s</param>
		/// <param name="appendNewline">True to append <see cref="Environment.NewLine"/> to the end. The default is False.</param>
		/// <returns>A string that represents all <see cref="SqlParameter.ParameterName"/>s and their <see cref="SqlParameter.Value"/>s.</returns>
		public string ToString(SqlParameter[] parameters, bool appendNewline = false)
		{
			if (parameters == null || parameters.Length < 1)
				return "";

			var str = "";

			foreach (var p in parameters)
				str = String.Format("{0}{1}: '{2}'; ", str, p.ParameterName, p.Value);

			str = str.Substring(0, str.Length - 2);

			if (appendNewline)
				return String.Format("{0}{1}", str, Environment.NewLine);

			return str;
		}

		/// <summary>
		/// Truncates a SQL table.
		/// </summary>
		/// <param name="table">The table to truncate.</param>
		public void TruncateTable(string table)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(ConnectionString))
				log = String.Format("{0}'ConnectionString' is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(table))
				log = String.Format("{0}'table' is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in SqlUtil2.TruncateTable(string table).{1}{1}", log, Environment.NewLine));
			#endregion Input Check

			var sql = String.Format("TRUNCATE TABLE [dbo].{0}", table);

			try {
				using (var conn = new SqlConnection(ConnectionString)) {
					using (var cmd = new SqlCommand(sql, conn)) {
						cmd.CommandType = CommandType.Text;
						conn.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in SqlUtil2.TruncateTable(string table='{3}').{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, table);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of SqlUtil2.TruncateTable(string table='{3}').{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, table);

				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// Truncates a SQL table. Specifying the schema and table.
		/// </summary>
		/// <param name="schema">The schema of the table. If null or empty, it will default to "dbo".</param>
		/// <param name="table">The table to truncate.</param>
		public void TruncateTable(string schema, string table)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(ConnectionString))
				log = String.Format("{0}'ConnectionString' is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(schema))
				schema = "[dbo]";
			if (String.IsNullOrWhiteSpace(table))
				log = String.Format("{0}'table' is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in SqlUtil2.TruncateTable(string schema, string table).{1}{1}", log, Environment.NewLine));
			#endregion Input Check

			var sql = String.Format("TRUNCATE TABLE {0}.{1}", schema, table);

			try {
				using (var conn = new SqlConnection(ConnectionString)) {
					using (var cmd = new SqlCommand(sql, conn)) {
						cmd.CommandType = CommandType.Text;
						conn.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in SqlUtil2.TruncateTable(string schema='{3}', string table='{4}').{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, schema, table);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of SqlUtil2.TruncateTable(string schema='{3}', string table='{4}').{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, schema, table);

				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// Truncates a SQL table. Specifying the database, schema, and table.
		/// </summary>
		/// <param name="database">The database of the table.</param>
		/// <param name="schema">The schema of the table. If null or empty, it will default to "dbo".</param>
		/// <param name="table">The table to truncate.</param>
		public void TruncateTable(string database, string schema, string table)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(ConnectionString))
				log = String.Format("{0}'ConnectionString' is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(database))
				log = String.Format("{0}'database' is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(schema))
				schema = "[dbo]";
			if (String.IsNullOrWhiteSpace(table))
				log = String.Format("{0}'table' is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in SqlUtil2.TruncateTable(string database, string schema, string table).{1}{1}", log, Environment.NewLine));
			#endregion Input Check

			var sql = String.Format("TRUNCATE TABLE {0}.{1}.{2}", database, schema, table);

			try {
				using (var conn = new SqlConnection(ConnectionString)) {
					using (var cmd = new SqlCommand(sql, conn)) {
						cmd.CommandType = CommandType.Text;
						conn.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in SqlUtil2.TruncateTable(string database='{3}', string schema='{4}', string table='{5}').{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, database, schema, table);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of SqlUtil2.TruncateTable(string database='{3}', string schema='{4}', string table='{5}').{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, database, schema, table);

				throw new Exception(log);
				#endregion Log
			}
		}
		#endregion Public Methods
	}
}