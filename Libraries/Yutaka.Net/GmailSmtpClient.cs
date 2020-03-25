using System;
using System.Net;
using System.Net.Mail;

namespace Yutaka.Net
{
	public class GmailSmtpClient : SmtpClient
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GmailSmtpClient"/> class that sends email by using Gmail's SMTP server and port.
		/// </summary>
		/// <param name="username">The user name associated with the credentials.</param>
		/// <param name="password">The password for the user name associated with the credentials.</param>
		public GmailSmtpClient(string username, string password)
		{
			#region Input Check
			var log = "";

			if (String.IsNullOrWhiteSpace(username))
				log = String.Format("{0}<username> is required.{1}", log, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(password))
				log = String.Format("{0}<password> is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in Constructor GmailSmtpClient(string username, string password).{1}{1}", log, Environment.NewLine);
				throw new Exception(log);
			}
			#endregion Input Check

			UseDefaultCredentials = false; // always set this BEFORE setting Credentials //
			Credentials = new NetworkCredential(username, password);
			EnableSsl = true;
			Host = "smtp.gmail.com";
			Port = 587;
		}

		/// <summary>
		/// Sends the specified message to Gmail's SMTP server for delivery.
		/// </summary>
		/// <param name="message">A <see cref="MailMessage"/> that contains the message to send.</param>
		/// <param name="response">Response message containing the result.</param>
		/// <returns>True if succeeded. False otherwise.</returns>
		public bool TrySend(MailMessage message, out string response)
		{
			try {
				Send(message);
				response = "Success";
				return true;
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					response = String.Format("{0}{2}Exception thrown in GmailSmtpClient.TrySend(MailMessage message, out string response).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					response = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of GmailSmtpClient.TrySend(MailMessage message, out string response).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);
				#endregion Log

				return false;
			}
		}

		/// <summary>
		/// Sends the specified email message to Gmail's SMTP server for delivery. The message sender, recipients, subject, and message body are specified using <see cref="String"/> objects.
		/// </summary>
		/// <param name="from">A string that contains the address information of the message sender.</param>
		/// <param name="recipients">A string that contains the addresses that the message is sent to.</param>
		/// <param name="subject">A string that contains the subject line for the message.</param>
		/// <param name="body">A string that contains the message body.</param>
		/// <param name="response">The response containing the result with any <see cref="Exception"/> messages.</param>
		/// <returns>True if succeeded. False otherwise.</returns>
		public bool TrySend(string from, string recipients, string subject, string body, out string response)
		{
			#region Input Check
			response = "";

			if (String.IsNullOrWhiteSpace(from))
				response = String.Format("{0}<from> is required.{1}", response, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(recipients))
				response = String.Format("{0}<recipients> is required.{1}", response, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(subject) && String.IsNullOrWhiteSpace(body))
				response = String.Format("{0}<subject> OR <body> are required.{1}", response, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(response)) {
				response = String.Format("{0}Exception thrown in TrySend(string from, string recipients, string subject, string body, out string response).{1}{1}", response, Environment.NewLine);
				return false;
			}
			#endregion Input Check

			try {
				Send(from, recipients, subject, body);
				response = "Success";
				return true;
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					response = String.Format("{0}{2}Exception thrown in GmailSmtpClient.TrySend(string from='{3}', string recipients='{4}', string subject='{5}', string body='{6}', out string response).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, from, recipients, subject, body.Length > 200 ? body.Substring(0, 200) : body);
				else
					response = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of GmailSmtpClient.TrySend(string from='{3}', string recipients='{4}', string subject='{5}', string body='{6}', out string response).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, from, recipients, subject, body.Length > 200 ? body.Substring(0, 200) : body);
				#endregion Log

				return false;
			}
		}
	}
}