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
	}
}