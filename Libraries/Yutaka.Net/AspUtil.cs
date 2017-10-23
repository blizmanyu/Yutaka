using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Rcw.Settings;
using Yutaka.Utils;

namespace Yutaka.Net
{
	public static class AspUtil
	{
		#region Fields
		const string DB_RCWShared = ConnectionStrings.DB_RCWShared;
		private static readonly CommandType STORED_PROCEDURE = CommandType.StoredProcedure;
		#endregion

		#region Private Helpers
		private static int ExecuteNonQuery(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			try {
				using (var conn = new SqlConnection(connectionString)) {
					using (var cmd = new SqlCommand(commandText, conn)) {
						cmd.CommandType = commandType;
						cmd.Parameters.AddRange(parameters);

						conn.Open();
						return cmd.ExecuteNonQuery();
					}
				}
			}

			catch (Exception ex) {
				string body = ex.Message + "\n\nConnection String: " + connectionString + "\nCommand Text: " + commandText + "\nCommand Type: " + commandType + "\nParameters:\n";
				for (int i=0; i < parameters.Length; i++)
					body += parameters[i].ParameterName + " = " + parameters[i].Value + "\n";
				return -1;
			}
		}
		#endregion

		#region Public Methods
		public static bool IsUrlValid(string url)
		{
			if (url == null)
				return false;

			using (var client = new MyClient()) {
				client.HeadOnly = true;

				try {
					var str = client.DownloadString(url);
					return true;
				}

				catch (Exception) {
					return false;
				}
			}
		}

		public static string GetUniqueId(DateTime time)
		{
			if (time == null)
				time = DateTime.Now;

			var num = time.ToString("yyyyMMddHHmmssfff");
			return Base36.Encode(Int64.Parse(num));
		}

		public static void InsertLead(string uniqueId = "", string name = "", string email = "", string phone = "", string leadStatus = "", string leadSource = "", string leadProvider = "", string campaignSource = "", string notes = "", string ip = "", string url = "", string referer = "", string browser = "", string userAgent = "", string createdBy = "", DateTime createdOn = new DateTime())
		{
			#region Handle Input
			var Request = HttpContext.Current.Request;

			if (createdOn == new DateTime())
				createdOn = DateTime.Now;
			if (String.IsNullOrEmpty(uniqueId))
				uniqueId = GetUniqueId(createdOn);
			if (String.IsNullOrEmpty(leadStatus))
				leadStatus = "Open";
			if (String.IsNullOrEmpty(leadSource))
				leadSource = "Web";
			if (String.IsNullOrEmpty(leadProvider) && leadSource == "Web")
				leadProvider = Request.Url.Host ?? "";
			if (String.IsNullOrEmpty(ip))
				ip = Request.UserHostAddress ?? "";
			if (String.IsNullOrEmpty(url))
				url = Request.UrlReferrer.AbsoluteUri ?? "";
			if (String.IsNullOrEmpty(browser))
				browser = Request.Browser.Browser ?? "";
			if (String.IsNullOrEmpty(userAgent))
				userAgent = Request.UserAgent ?? "";
			if (String.IsNullOrEmpty(createdBy))
				createdBy = "Website";
			#endregion

			#region Insert Lead
			var storProc = "dbo.InsertLead";

			SqlParameter[] parameters = { new SqlParameter("@UniqueID", uniqueId),
										  new SqlParameter("@Name", name),
										  new SqlParameter("@Email", email),
										  new SqlParameter("@Phone", phone),
										  new SqlParameter("@LeadStatus", leadStatus),
										  new SqlParameter("@LeadSource", leadSource),
										  new SqlParameter("@LeadProvider", leadProvider),
										  new SqlParameter("@CampaignSource", campaignSource),
										  new SqlParameter("@Notes ", notes),
										  new SqlParameter("@IP", ip),
										  new SqlParameter("@URL", url),
										  new SqlParameter("@Referer", referer),
										  new SqlParameter("@Browser", browser),
										  new SqlParameter("@UserAgent", userAgent),
										  new SqlParameter("@CreatedBy", createdBy),
										  new SqlParameter("@CreatedOn", createdOn)};

			ExecuteNonQuery(DB_RCWShared, storProc, STORED_PROCEDURE, parameters);
			#endregion
		}
		#endregion
	}
}