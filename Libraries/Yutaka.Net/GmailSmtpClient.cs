﻿using System;
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
	}
}