using System;
using System.Data;
using System.Data.SqlClient;

namespace Yutaka.Utils
{
	public class SqlHelper
	{
		#region Fields
		public const CommandType STORED_PROCEDURE = CommandType.StoredProcedure;
		public const CommandType TABLE_DIRECT = CommandType.TableDirect;
		public const CommandType TEXT = CommandType.Text;
		const string SERVER_DB = "server=localhost; database=";
		const string UID_PWD = "uid=sa; pwd=";
		const string IRONMAN_PASSWORD = "access";
		const string HULK_PASSWORD = "dhsSG2j36GDjsASgL";

		public const string Goldmine = SERVER_DB + "Goldmine; " + UID_PWD + IRONMAN_PASSWORD;
		public const string Intranet = SERVER_DB + "Intranet; " + UID_PWD + IRONMAN_PASSWORD;
		public const string wwwSPs = SERVER_DB + "wwwSPs; " + UID_PWD + IRONMAN_PASSWORD;

		public const string DB_IRONMAN = SERVER_DB + "IRONMAN; " + UID_PWD + HULK_PASSWORD;
		public const string DB_NOPCOMMERCE_RCW = SERVER_DB + "NopCommerce_RCW; " + UID_PWD + HULK_PASSWORD;
		public const string DB_NTA = SERVER_DB + "NTA; " + UID_PWD + HULK_PASSWORD;
		public const string DB_RCW201412 = SERVER_DB + "RCW201412; " + UID_PWD + HULK_PASSWORD;
		public const string DB_RCWSHARED = SERVER_DB + "RCWShared; " + UID_PWD + HULK_PASSWORD;
		public const string DB_TRANSFER = SERVER_DB + "Transfer; " + UID_PWD + HULK_PASSWORD;
		public const string DB_WW2_RCWF = SERVER_DB + "ww2.rcwfinancial.com; " + UID_PWD + HULK_PASSWORD;
		public const string DB_RCW_V370 = SERVER_DB + "www.rarecoinwholesalers.com_v3.70; " + UID_PWD + HULK_PASSWORD;
		public const string DB_RCWF_V360 = SERVER_DB + "www.rcwfinancial.com_v3.60; " + UID_PWD + HULK_PASSWORD;
		#endregion

		#region Methods
		public static string ExecuteNonQuery(string connectionString, string query, CommandType commandType = STORED_PROCEDURE, params SqlParameter[] parameters)
		{
			if (String.IsNullOrEmpty(connectionString) || String.IsNullOrEmpty(query))
				return "Connection string or Query is NULL or Empty.";

			using (var conn = new SqlConnection(connectionString)) {
				using (var cmd = new SqlCommand(query, conn)) {
					cmd.CommandType = commandType;
					cmd.Parameters.AddRange(parameters);
					try {
						conn.Open();
						cmd.ExecuteNonQuery();
						return "";
					}

					catch (Exception ex) {
						var errorMsg = "";
						errorMsg += "\n" + ex.Message;
						errorMsg += "\n";
						errorMsg += "\n" + ex.ToString();
#if DEBUG
						Console.Write(errorMsg);
#endif
						return errorMsg;
					}
				}
			}
		}

		public static string ExecuteReader(string connectionString, string query, CommandType commandType = STORED_PROCEDURE, params SqlParameter[] parameters)
		{
			if (String.IsNullOrEmpty(connectionString) || String.IsNullOrEmpty(query))
				return "Connection string or Query is NULL or Empty.";

			using (var conn = new SqlConnection(connectionString)) {
				using (var cmd = new SqlCommand(query, conn)) {
					cmd.CommandType = commandType;
					cmd.Parameters.AddRange(parameters);
					try {
						conn.Open();
						using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
							if (reader.HasRows) {
								while (reader.Read()) {
									// DO SOMETHING HERE //
								}
							}
							return "";
						}
					}
					catch (Exception ex) {
						var errorMsg = "";
						errorMsg += "\n" + ex.Message;
						errorMsg += "\n";
						errorMsg += "\n" + ex.ToString();
#if DEBUG
						Console.Write(errorMsg);
#endif
						return errorMsg;
					}
				}
			}
		}
		#endregion
	}
}