using System;
using System.Data;
using System.Data.SqlClient;

namespace Yutaka.Data
{
	public static class SqlUtil
	{
		#region Methods
		public static Result ExecuteNonQuery(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			var result = new Result() {
				Success = false,
				Message = "",
				Exception = ""
			};

			using (var conn = new SqlConnection(connectionString)) {
				using (var cmd = new SqlCommand(commandText, conn)) {
					cmd.CommandType = commandType;
					try {
						cmd.Parameters.AddRange(parameters);
						conn.Open();
						cmd.ExecuteNonQuery();
						result.Success = true;
					}

					catch (Exception ex) {
						result.Message = ex.Message;
						result.Exception = ex.ToString();
					}
				}
			}

			return result;
		}

		// This method should never actually be called. It is provided as an example only since the reader will get destroyed before returning. You
		// may want to use the GetData method instead.
		public static Result ExecuteReader(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			var result = new Result() {
				Success = false,
				Message = "",
				Exception = ""
			};

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
						result.Success = true;
					}

					catch (Exception ex) {
						result.Message = ex.Message;
						result.Exception = ex.ToString();
					}
				}
			}

			return result;
		}

		public static Result ExecuteScalar(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			var result = new Result() {
				Success = false,
				Message = "",
				Exception = ""
			};

			using (var conn = new SqlConnection(connectionString)) {
				using (var cmd = new SqlCommand(commandText, conn)) {
					cmd.CommandType = commandType;
					try {
						cmd.Parameters.AddRange(parameters);
						conn.Open();
						cmd.ExecuteScalar();
						result.Success = true;
					}

					catch (Exception ex) {
						result.Message = ex.Message;
						result.Exception = ex.ToString();
					}
				}
			}

			return result;
		}

		public static DataSet GetData(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			var ds = new DataSet();

			using (var conn = new SqlConnection(connectionString)) {
				using (var cmd = new SqlCommand(commandText, conn)) {
					cmd.CommandType = commandType;
					try {
						cmd.Parameters.AddRange(parameters);
						using (var adapter = new SqlDataAdapter(cmd)) {
							adapter.Fill(ds);
						}
						return ds;
					}

					catch (Exception) {
						return null;
					}
				}
			}
		}
		#endregion

		#region Struct
		public struct Result
		{
			public bool Success;
			public string Message;
			public string Exception;
		}
		#endregion
	}
}