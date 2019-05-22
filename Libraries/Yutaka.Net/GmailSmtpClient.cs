using System;
using System.Net;
using System.Net.Mail;

namespace Yutaka.Net
{
	public class GmailSmtpClient : SmtpClient
	{
		public GmailSmtpClient(string username, string password)
		{
			if (String.IsNullOrWhiteSpace(username))
				throw new Exception(String.Format("<username> is required. Exception thrown in GmailSmtpClient Constructor.{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(password))
				throw new Exception(String.Format("<password> is required. Exception thrown in GmailSmtpClient Constructor.{0}", Environment.NewLine));

			UseDefaultCredentials = false; // always set this BEFORE setting Credentials //
			Credentials = new NetworkCredential(username, password);
			EnableSsl = true;
			Host = "smtp.gmail.com";
			Port = 587;
		}
	}
}