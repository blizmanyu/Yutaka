using System;
using System.Data;
using System.Data.SqlClient;

namespace Yutaka.Data
{
	public class SqlUtil2
	{
		#region Fields
		#region CommandTypes
		/// <summary>
		/// An SQL text command. (Default.)
		/// </summary>
		public static readonly CommandType CommandTypeText = CommandType.Text;
		/// <summary>
		/// The name of a stored procedure.
		/// </summary>
		public static readonly CommandType CommandTypeStoredProcedure = CommandType.StoredProcedure;
		/// <summary>
		/// The name of a table.
		/// </summary>
		public static readonly CommandType CommandTypeTableDirect = CommandType.TableDirect;
		#endregion CommandTypes

		protected string ConnectionString;
		#endregion Fields

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="SqlUtil2"/> class with a connection string.
		/// </summary>
		/// <param name="connectionString">A valid connection string.</param>
		public SqlUtil2(string connectionString = null)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(connectionString))
				log = String.Format("<connectionString> is required.{0}", Environment.NewLine);
			else if (connectionString.Length < 9)
				log = String.Format("'{0}' is an invalid connection string.{1}", connectionString, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in Constructor SqlUtil2(string connectionString).{1}", log, Environment.NewLine));
			#endregion Input Check

			ConnectionString = connectionString;
		}
		#endregion Constructor

		#region Public Methods
		/// <summary>
		/// Executes a Transact-SQL statement against the connection.
		/// </summary>
		/// <param name="commandText">The Transact-SQL statement or stored procedure to execute.</param>
		/// <param name="commandType">One of the <see cref="CommandType"/> values.</param>
		/// <param name="parameters">The parameters of the Transact-SQL statement or stored procedure. The default is an empty collection.</param>
		public void ExecuteNonQuery(string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(commandText))
				log = String.Format("{0}<commandText> is required.{1}", log, Environment.NewLine);

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
		public SqlDataReader ExecuteReader(string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(commandText))
				log = String.Format("{0}<commandText> is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in SqlUtil2.ExecuteReader(string commandText, CommandType commandType, params SqlParameter[] parameters).{1}{1}", log, Environment.NewLine));
			#endregion Input Check

			try {
				using (var conn = new SqlConnection(ConnectionString)) {
					using (var cmd = new SqlCommand(commandText, conn)) {
						cmd.CommandType = commandType;
						cmd.Parameters.AddRange(parameters);
						conn.Open();
						// When using CommandBehavior.CloseConnection, the connection will be closed when the IDataReader is closed.  
						return cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
		public object ExecuteScalar(string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(commandText))
				log = String.Format("{0}<commandText> is required.{1}", log, Environment.NewLine);

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
		#endregion Public Methods
	}
}