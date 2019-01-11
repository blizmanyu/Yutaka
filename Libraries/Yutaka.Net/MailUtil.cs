using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Yutaka.Net
{
	public static class MailUtil
	{
		#region Methods
		public static string Clean(string email)
		{
			if (String.IsNullOrWhiteSpace(email))
				return "";

			while (email.Contains(" "))
				email.Replace(" ", "");

			if (email == email.ToUpper())
				email = email.ToLower();

			return email;
		}

		public static string DecodeEmail(string str)
		{
			if (String.IsNullOrWhiteSpace(str))
				return "";

			try {
				var sb = new StringBuilder();
				int c;

				for (int i = 0; i < str.Length; i++) {
					c = str[i] - 2;
					sb.Append((char) c);
				}

				return sb.ToString();
			}

			catch (Exception ex) {
				throw new Exception(String.Format("Exception thrown in WebHelper.DecodeEmail(string str='{3}'){2}{0}{2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine, str));
			}
		}

		public static string EncodeEmail(string email, bool cleanFirst = false)
		{
			if (String.IsNullOrWhiteSpace(email))
				return "";

			if (cleanFirst)
				email = Clean(email);

			try {
				var sb = new StringBuilder();
				int c;

				for (int i = 0; i < email.Length; i++) {
					c = email[i] + 2;
					sb.Append((char) c);
				}

				return sb.ToString();
			}

			catch (Exception ex) {
				throw new Exception(String.Format("Exception thrown in WebHelper.EncodeEmail(string email='{3}'){2}{0}{2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine, email));
			}
		}

		public static bool IsValid(string email, bool cleanFirst = false)
		{
			if (String.IsNullOrWhiteSpace(email))
				return false;

			if (cleanFirst)
				email = Clean(email);

			try {
				var v = new MailAddress(email);
				return true;
			}

			catch (Exception) {
				return false;
			}
		}

		#region Parse/TryParse
		public static MailAddress Parse(string email)
		{
			if (String.IsNullOrWhiteSpace(email))
				throw new Exception(String.Format("<email> is required.{0}{0}Exception thrown in MailUtil.Parse(string email)", Environment.NewLine));

			try {
				if (email.ToUpper().Contains("UNDISCLOSED"))
					return new MailAddress("undisclosed@recipients", "Undisclosed Recipients");

				return new MailAddress(Clean(email.Replace(";", "").Replace(":", "").Replace(",", "")));
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in MailUtil.Parse(string email='{3}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, email));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of MailUtil.Parse(string email='{3}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, email));
			}
		}
		#endregion Parse/TryParse

		public static Result Send(MailMessage message, string smtpHost, int smtpPort, string username, string password, SmtpDeliveryMethod smtpDeliveryMethod = SmtpDeliveryMethod.Network, bool enableSsl = true, bool useDefaultCredentials = false)
		{
			var result = new Result() {
				Success = false,
				Message = "",
				Exception = ""
			};

			using (var client = new SmtpClient(smtpHost, smtpPort)) {
				client.Credentials = new NetworkCredential(username, password);
				client.DeliveryMethod = smtpDeliveryMethod;
				client.EnableSsl = enableSsl;
				client.UseDefaultCredentials = false;

				try {
					client.Send(message);
					result.Success = true;
				}

				catch (Exception ex) {
					throw ex;
				}
			}

			return result;
		}

		public static Result Send(SmtpClient client, MailMessage message)
		{
			var result = new Result() {
				Success = false,
				Message = "",
				Exception = ""
			};

			try {
				client.Send(message);
				result.Success = true;
			}

			catch (Exception ex) {
				throw ex;
			}

			return result;
		}

		public static Result Send(SmtpClient client, string from, string to, string subject, string body, AttachmentCollection attachments=null, MailAddressCollection bcc = null, MailAddressCollection cc = null, bool isBodyHtml = true)
		{
			var result = new Result() {
				Success = false,
				Message = "",
				Exception = ""
			};

			using (var message = new MailMessage(from, to, subject, body)) {
				int i;

				if (attachments != null) {
					for (i = 0; i < attachments.Count; i++)
						message.Attachments.Add(attachments[i]);
				}

				if (bcc != null) {
					for (i = 0; i < bcc.Count; i++)
						message.Bcc.Add(bcc[i]);
				}

				if (cc != null) {
					for (i = 0; i < cc.Count; i++)
						message.CC.Add(cc[i]);
				}

				message.IsBodyHtml = isBodyHtml;

				try {
					client.Send(message);
					result.Success = true;
				}

				catch (Exception ex) {
					throw ex;
				}
			}

			return result;
		}

		public static Result Send(string smtpHost, int smtpPort, string username, string password, string from, string to, string subject, string body, SmtpDeliveryMethod smtpDeliveryMethod = SmtpDeliveryMethod.Network, bool enableSsl = true, bool useDefaultCredentials = false, AttachmentCollection attachments = null, MailAddressCollection bcc = null, MailAddressCollection cc = null, bool isBodyHtml = true)
		{
			var result = new Result() {
				Success = false,
				Message = "",
				Exception = ""
			};

			using (var message = new MailMessage(from, to, subject, body)) {
				int i;

				if (attachments != null) {
					for (i = 0; i < attachments.Count; i++)
						message.Attachments.Add(attachments[i]);
				}

				if (bcc != null) {
					for (i = 0; i < bcc.Count; i++)
						message.Bcc.Add(bcc[i]);
				}

				if (cc != null) {
					for (i = 0; i < cc.Count; i++)
						message.CC.Add(cc[i]);
				}

				message.IsBodyHtml = isBodyHtml;

				using (var client = new SmtpClient(smtpHost, smtpPort)) {
					client.Credentials = new NetworkCredential(username, password);
					client.DeliveryMethod = smtpDeliveryMethod;
					client.EnableSsl = enableSsl;
					client.UseDefaultCredentials = false;

					try {
						client.Send(message);
						result.Success = true;
					}

					catch (Exception ex) {
						throw ex;
					}
				}
			}

			return result;
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