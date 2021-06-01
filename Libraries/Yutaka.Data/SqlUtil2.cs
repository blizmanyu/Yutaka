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
		#endregion Constructor

		#region Public Methods
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

			catch (SqlException ex) {
				#region Log
				for (int i = 0; i < ex.Errors.Count; i++) {
					log = String.Format("{0}Index #{1}{2}" +
						"Message: {3}{2}" +
						"LineNumber: {4}{2}" +
						"Source: {5}{2}" +
						"Procedure: {6}{2}", log, i, Environment.NewLine, ex.Errors[i].Message, ex.Errors[i].LineNumber, ex.Errors[i].Source, ex.Errors[i].Procedure);
				}

				log = String.Format("{0}{2}{1}_PARAMS_Exception thrown in SqlUtil2.ExecuteNonQuery(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{2}{2}", ex.Message, log, Environment.NewLine, commandText, commandType);

				if (parameters == null || parameters.Length < 1)
					log = log.Replace("_PARAMS_", "");
				else {
					var ps = "";

					foreach (var p in parameters)
						ps = String.Format("{0}{1}: '{2}'; ", ps, p.ParameterName, p.Value ?? "NULL");

					ps = String.Format("{0}{1}", ps, Environment.NewLine);
					log = log.Replace("_PARAMS_", ps);
				}

				throw new Exception(log);
				#endregion Log
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}_PARAMS_Exception thrown in SqlUtil2.ExecuteNonQuery(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, commandText, commandType);
				else
					log = String.Format("{0}{2}_PARAMS_Exception thrown in INNER EXCEPTION of SqlUtil2.ExecuteNonQuery(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, commandText, commandType);

				if (parameters == null || parameters.Length < 1)
					log = log.Replace("_PARAMS_", "");
				else {
					var ps = "";

					foreach (var p in parameters)
						ps = String.Format("{0}{1}: '{2}'; ", ps, p.ParameterName, p.Value ?? "NULL");

					ps = String.Format("{0}{1}", ps, Environment.NewLine);
					log = log.Replace("_PARAMS_", ps);
				}

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

			catch (SqlException ex) {
				#region Log
				for (int i = 0; i < ex.Errors.Count; i++) {
					log = String.Format("{0}Index #{1}{2}" +
						"Message: {3}{2}" +
						"LineNumber: {4}{2}" +
						"Source: {5}{2}" +
						"Procedure: {6}{2}", log, i, Environment.NewLine, ex.Errors[i].Message, ex.Errors[i].LineNumber, ex.Errors[i].Source, ex.Errors[i].Procedure);
				}

				log = String.Format("{0}{2}{1}_PARAMS_Exception thrown in SqlUtil2.ExecuteReader(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{2}{2}", ex.Message, log, Environment.NewLine, commandText, commandType);

				if (parameters == null || parameters.Length < 1)
					log = log.Replace("_PARAMS_", "");
				else {
					var ps = "";

					foreach (var p in parameters)
						ps = String.Format("{0}{1}: '{2}'; ", ps, p.ParameterName, p.Value ?? "NULL");

					ps = String.Format("{0}{1}", ps, Environment.NewLine);
					log = log.Replace("_PARAMS_", ps);
				}

				throw new Exception(log);
				#endregion Log
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}_PARAMS_Exception thrown in SqlUtil2.ExecuteReader(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, commandText, commandType);
				else
					log = String.Format("{0}{2}_PARAMS_Exception thrown in INNER EXCEPTION of SqlUtil2.ExecuteReader(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, commandText, commandType);

				if (parameters == null || parameters.Length < 1)
					log = log.Replace("_PARAMS_", "");
				else {
					var ps = "";

					foreach (var p in parameters)
						ps = String.Format("{0}{1}: '{2}'; ", ps, p.ParameterName, p.Value ?? "NULL");

					ps = String.Format("{0}{1}", ps, Environment.NewLine);
					log = log.Replace("_PARAMS_", ps);
				}

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

			catch (SqlException ex) {
				#region Log
				for (int i = 0; i < ex.Errors.Count; i++) {
					log = String.Format("{0}Index #{1}{2}" +
						"Message: {3}{2}" +
						"LineNumber: {4}{2}" +
						"Source: {5}{2}" +
						"Procedure: {6}{2}", log, i, Environment.NewLine, ex.Errors[i].Message, ex.Errors[i].LineNumber, ex.Errors[i].Source, ex.Errors[i].Procedure);
				}

				log = String.Format("{0}{2}{1}_PARAMS_Exception thrown in SqlUtil2.ExecuteScalar(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{2}{2}", ex.Message, log, Environment.NewLine, commandText, commandType);

				if (parameters == null || parameters.Length < 1)
					log = log.Replace("_PARAMS_", "");
				else {
					var ps = "";

					foreach (var p in parameters)
						ps = String.Format("{0}{1}: '{2}'; ", ps, p.ParameterName, p.Value ?? "NULL");

					ps = String.Format("{0}{1}", ps, Environment.NewLine);
					log = log.Replace("_PARAMS_", ps);
				}

				throw new Exception(log);
				#endregion Log
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}_PARAMS_Exception thrown in SqlUtil2.ExecuteScalar(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, commandText, commandType);
				else
					log = String.Format("{0}{2}_PARAMS_Exception thrown in INNER EXCEPTION of SqlUtil2.ExecuteScalar(string commandText='{3}', CommandType commandType='{4}', params SqlParameter[] parameters).{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, commandText, commandType);

				if (parameters == null || parameters.Length < 1)
					log = log.Replace("_PARAMS_", "");
				else {
					var ps = "";

					foreach (var p in parameters)
						ps = String.Format("{0}{1}: '{2}'; ", ps, p.ParameterName, p.Value ?? "NULL");

					ps = String.Format("{0}{1}", ps, Environment.NewLine);
					log = log.Replace("_PARAMS_", ps);
				}

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

			catch (SqlException ex) {
				#region Log
				for (int i = 0; i < ex.Errors.Count; i++) {
					log = String.Format("{0}Index #{1}{2}" +
						"Message: {3}{2}" +
						"LineNumber: {4}{2}" +
						"Source: {5}{2}" +
						"Procedure: {6}{2}", log, i, Environment.NewLine, ex.Errors[i].Message, ex.Errors[i].LineNumber, ex.Errors[i].Source, ex.Errors[i].Procedure);
				}

				log = String.Format("{0}{2}{1}Exception thrown in SqlUtil2.StartJob(string jobId='{3}', string stepName='{4}').{2}{2}", ex.Message, log, Environment.NewLine, jobId, stepName);
				throw new Exception(log);
				#endregion Log
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
		/// <returns>A string that represents all <see cref="SqlParameter.ParameterName"/>s and their <see cref="SqlParameter.Value"/>s.</returns>
		public string ToString(SqlParameter[] parameters)
		{
			if (parameters == null || parameters.Length < 1)
				return "";

			var str = "";

			foreach (var p in parameters)
				str = String.Format("{0}{1}: '{2}'; ", str, p.ParameterName, p.Value);

			return str.Substring(0, str.Length - 2);
		}

		/// <summary>
		/// Truncates a SQL table.
		/// </summary>
		/// <param name="database">Optional database of the table, but beware because it will run on the "current" database.</param>
		/// <param name="schema">The schema of the table. If null or empty, it will default to "dbo".</param>
		/// <param name="table">The table to truncate.</param>
		public void TruncateTable(string database = null, string schema = null, string table = null)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(ConnectionString))
				log = String.Format("{0}'ConnectionString' is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(schema))
				schema = "dbo";
			if (String.IsNullOrWhiteSpace(table))
				log = String.Format("{0}'table' is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in SqlUtil2.TruncateTable(string database, string schema, string table).{1}{1}", log, Environment.NewLine));
			#endregion Input Check

			var sql = String.Format("TRUNCATE TABLE {0}.{1}", schema, table);

			if (!String.IsNullOrWhiteSpace(database))
				sql = sql.Replace("TABLE ", String.Format("TABLE {0}.", database));

			try {
				using (var conn = new SqlConnection(ConnectionString)) {
					using (var cmd = new SqlCommand(sql, conn)) {
						cmd.CommandType = CommandType.Text;
						conn.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}

			catch (SqlException ex) {
				#region Log
				for (int i = 0; i < ex.Errors.Count; i++) {
					log = String.Format("{0}Index #{1}{2}" +
						"Message: {3}{2}" +
						"LineNumber: {4}{2}" +
						"Source: {5}{2}" +
						"Procedure: {6}{2}", log, i, Environment.NewLine, ex.Errors[i].Message, ex.Errors[i].LineNumber, ex.Errors[i].Source, ex.Errors[i].Procedure);
				}

				log = String.Format("{0}{2}{1}Exception thrown in SqlUtil2.TruncateTable(string database='{3}', string schema='{4}', string table='{5}').{2}{2}", ex.Message, log, Environment.NewLine, database, schema, table);
				throw new Exception(log);
				#endregion Log
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