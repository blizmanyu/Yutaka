using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using Rcw.Sql.Ironman.RcwConfiguration.Services;

namespace Yutaka.Utils
{
	public class MailHelper
	{
		private static RcwConfigurationService _rcwConfigurationService = new RcwConfigurationService();

		#region Methods
		public static bool IsValid(string email)
		{
			if (String.IsNullOrEmpty(email))
				return false;

			try {
				var addr = new MailAddress(email);
				return (addr.Address == email);
			}

			catch (Exception) {
				return false;
			}
		}

		public static string Clean(string email)
		{
			if (String.IsNullOrEmpty(email))
				return "";

			while (email.Contains(" "))
				email = email.Replace(" ", "");

			if (email == email.ToUpper())
				return email.ToLower();

			return email;
		}

		public static Result Send(string from, string recipients, string subject, string body, bool isBodyHtml = true)
		{
			using (var msg = new MailMessage(from, recipients, subject, body)) {
				msg.IsBodyHtml = isBodyHtml;
				return Send(msg);
			}
		}

		public static Result Send(MailMessage msg)
		{
			var result = new Result();

			var from = msg.From.Address.ToLower();
			var email = _rcwConfigurationService.GetEmailAccountByEmail(from);

			if (email == null || email.Id < 0) {
				result.ErrorCode = -10;
				var sb = new StringBuilder();
				sb.Append("The email you're trying to use <" + email + "> is not known within the system. If it should be, tell IT to enter it in for you.");
				result.Message = sb.ToString();
				return result;
			}

			using (var client = new SmtpClient(email.Host, email.Port)) {
				client.Credentials = new NetworkCredential(email.Username, email.Password);
				client.EnableSsl = email.EnableSsl;

				try {
					client.Send(msg);
					result.ErrorCode = 10;
					result.Message = "Success!";
				}

				catch (Exception ex) {
					result.ErrorCode = -20;
					var sb = new StringBuilder();
					sb.Append(ex.Message + "\n\n" + ex.ToString());
					result.Message = sb.ToString();
				}
			}

			return result;
		}
		#endregion

		#region Structs
		/// <summary>
		/// <code>ErrorCode</code> should be negative for errors and positive for success.
		/// </summary>
		public struct Result
		{
			public int ErrorCode { get; set; }
			public string Message { get; set; }
		}
		#endregion
	}
}