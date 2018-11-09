using System;
using System.Net;
using System.Net.Mail;

namespace Yutaka.Net
{
	public static class MailUtil
	{
		#region Methods
		public static string Clean(string email)
		{
			if (email == null)
				return "";

			while (email.Contains(" "))
				email.Replace(" ", "");

			if (email == email.ToUpper())
				email = email.ToLower();

			return email;
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