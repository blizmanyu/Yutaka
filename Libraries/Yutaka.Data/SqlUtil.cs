using System;
using System.Data;
using System.Data.SqlClient;

namespace Yutaka.Data
{
	public abstract class SqlUtil
	{
		public const CommandType STORED_PROCEDURE = CommandType.StoredProcedure;
		public const CommandType TABLE_DIRECT = CommandType.TableDirect;
		public const CommandType TEXT_COMM_TYPE = CommandType.Text;

		public virtual void ExecuteNonQuery(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
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
		public virtual void ExecuteReader(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
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

		public virtual Object ExecuteScalar(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
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

		public virtual DataSet GetData(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
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
					throw new Exception(String.Format("{0}{2}{3}{2}Exception thrown in RcwEmailService.GetData(string connectionString, string commandText='{4}', CommandType commandType='{5}'){2}{1}", ex.Message, ex.ToString(), Environment.NewLine, p, commandText, commandType));

				throw new Exception(String.Format("{0}{2}{3}{2}Exception thrown in INNER EXCEPTION of RcwEmailService.GetData(string connectionString, string commandText='{4}', CommandType commandType='{5}'){2}{1}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, p, commandText, commandType));
			}
		}

		public virtual bool IsServerConnected(string connectionString)
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

		#region Commented Out Jan, 10, 2019
		//public virtual void ExecuteScalar(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
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