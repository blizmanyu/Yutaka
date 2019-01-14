using System;
using System.Collections.Generic;
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
				email = email.Replace(" ", "");

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

		/// <summary>
		/// Converts a string of email adresses into a List of MailAddress objects. Comma separated is the default, but can be changed by specifying the separator parameter.
		/// </summary>
		/// <param name="emails">List of comma-separated emails. Semi-colon is supported if you specify it in the separator parameter</param>
		/// <param name="separator">Use this parameter if your list is semi-colon delimited. Comma and semi-colon are the only supported separators.</param>
		/// <param name="maxEmails">Max # of emails to convert from the string in case your list is pulled from a Mailing List that contains hundreds (or even thousands) of emails that you want to limited. If you don't want to cap the list, use 0 or a negative number.</param>
		/// <returns></returns>
		public static List<MailAddress> ConvertStringToMailAddresses(string emails, char separator=',', int maxEmails=10)
		{
			if (String.IsNullOrWhiteSpace(emails))
				throw new Exception(String.Format("<emails> is required.{0}{0}Exception thrown in MailUtil.ConvertStringToMailAddresses(string emails, char separator)", Environment.NewLine));

			var list = new List<MailAddress>();

			try {
				// Remove use of separator char that isn't being used as a separator  //
				switch (separator) {
					case ',':
						#region Case ','
						if (emails.Contains(",")) {
							emails = emails.Replace(">, ", "REPLACE_ME");
							emails = emails.Replace(".com, ", "REPLACE_DOT_COM");
							emails = emails.Replace(".net, ", "REPLACE_DOT_NET");
							emails = emails.Replace(".edu, ", "REPLACE_DOT_EDU");
							emails = emails.Replace(".org, ", "REPLACE_DOT_ORG");
							emails = emails.Replace(".gov, ", "REPLACE_DOT_GOV");
							emails = emails.Replace(".ca, ", "REPLACE_DOT_CA");
							emails = emails.Replace(".us, ", "REPLACE_DOT_US");
							emails = emails.Replace(".ch, ", "REPLACE_DOT_CH");
							emails = emails.Replace(".uk, ", "REPLACE_DOT_UK");
							emails = emails.Replace(".au, ", "REPLACE_DOT_AU");
							emails = emails.Replace(".de, ", "REPLACE_DOT_DE");
							emails = emails.Replace(".mil, ", "REPLACE_DOT_MIL");
							emails = emails.Replace(".biz, ", "REPLACE_DOT_BIZ");
							emails = emails.Replace(".it, ", "REPLACE_DOT_IT");
							emails = emails.Replace(".es, ", "REPLACE_DOT_ES");
							emails = emails.Replace(".fr, ", "REPLACE_DOT_FR");
							emails = emails.Replace(".in, ", "REPLACE_DOT_IN");
							emails = emails.Replace(".at, ", "REPLACE_DOT_AT");
							emails = emails.Replace(".nz, ", "REPLACE_DOT_NZ");
							emails = emails.Replace(".ru, ", "REPLACE_DOT_RU");
							emails = emails.Replace(".sg, ", "REPLACE_DOT_SG");
							emails = emails.Replace(".mx, ", "REPLACE_DOT_MX");
							emails = emails.Replace(".nl, ", "REPLACE_DOT_NL");
							emails = emails.Replace(".jp, ", "REPLACE_DOT_JP");
							emails = emails.Replace(",", "");
							emails = emails.Replace("REPLACE_ME", ">, ");
							emails = emails.Replace("REPLACE_DOT_COM", ".com, ");
							emails = emails.Replace("REPLACE_DOT_NET", ".net, ");
							emails = emails.Replace("REPLACE_DOT_EDU", ".edu, ");
							emails = emails.Replace("REPLACE_DOT_ORG", ".org, ");
							emails = emails.Replace("REPLACE_DOT_GOV", ".gov, ");
							emails = emails.Replace("REPLACE_DOT_CA", ".ca, ");
							emails = emails.Replace("REPLACE_DOT_US", ".us, ");
							emails = emails.Replace("REPLACE_DOT_CH", ".ch, ");
							emails = emails.Replace("REPLACE_DOT_UK", ".uk, ");
							emails = emails.Replace("REPLACE_DOT_AU", ".au, ");
							emails = emails.Replace("REPLACE_DOT_DE", ".de, ");
							emails = emails.Replace("REPLACE_DOT_MIL", ".mil, ");
							emails = emails.Replace("REPLACE_DOT_BIZ", ".biz, ");
							emails = emails.Replace("REPLACE_DOT_IT", ".it, ");
							emails = emails.Replace("REPLACE_DOT_ES", ".es, ");
							emails = emails.Replace("REPLACE_DOT_FR", ".fr, ");
							emails = emails.Replace("REPLACE_DOT_IN", ".in, ");
							emails = emails.Replace("REPLACE_DOT_AT", ".at, ");
							emails = emails.Replace("REPLACE_DOT_NZ", ".nz, ");
							emails = emails.Replace("REPLACE_DOT_RU", ".ru, ");
							emails = emails.Replace("REPLACE_DOT_SG", ".sg, ");
							emails = emails.Replace("REPLACE_DOT_MX", ".mx, ");
							emails = emails.Replace("REPLACE_DOT_NL", ".nl, ");
							emails = emails.Replace("REPLACE_DOT_JP", ".jp, ");
						}
						#endregion Case ","
						break;
					case ';':
						#region Case ';'
						if (emails.Contains(";")) {
							emails = emails.Replace(">; ", "REPLACE_ME");
							emails = emails.Replace(".com; ", "REPLACE_DOT_COM");
							emails = emails.Replace(".net; ", "REPLACE_DOT_NET");
							emails = emails.Replace(".edu; ", "REPLACE_DOT_EDU");
							emails = emails.Replace(".org; ", "REPLACE_DOT_ORG");
							emails = emails.Replace(".gov; ", "REPLACE_DOT_GOV");
							emails = emails.Replace(".ca; ", "REPLACE_DOT_CA");
							emails = emails.Replace(".us; ", "REPLACE_DOT_US");
							emails = emails.Replace(".ch; ", "REPLACE_DOT_CH");
							emails = emails.Replace(".uk; ", "REPLACE_DOT_UK");
							emails = emails.Replace(".au; ", "REPLACE_DOT_AU");
							emails = emails.Replace(".de; ", "REPLACE_DOT_DE");
							emails = emails.Replace(".mil; ", "REPLACE_DOT_MIL");
							emails = emails.Replace(".biz; ", "REPLACE_DOT_BIZ");
							emails = emails.Replace(".it; ", "REPLACE_DOT_IT");
							emails = emails.Replace(".es; ", "REPLACE_DOT_ES");
							emails = emails.Replace(".fr; ", "REPLACE_DOT_FR");
							emails = emails.Replace(".in; ", "REPLACE_DOT_IN");
							emails = emails.Replace(".at; ", "REPLACE_DOT_AT");
							emails = emails.Replace(".nz; ", "REPLACE_DOT_NZ");
							emails = emails.Replace(".ru; ", "REPLACE_DOT_RU");
							emails = emails.Replace(".sg; ", "REPLACE_DOT_SG");
							emails = emails.Replace(".mx; ", "REPLACE_DOT_MX");
							emails = emails.Replace(".nl; ", "REPLACE_DOT_NL");
							emails = emails.Replace(".jp; ", "REPLACE_DOT_JP");
							emails = emails.Replace(";", "");
							emails = emails.Replace("REPLACE_ME", ">; ");
							emails = emails.Replace("REPLACE_DOT_COM", ".com; ");
							emails = emails.Replace("REPLACE_DOT_NET", ".net; ");
							emails = emails.Replace("REPLACE_DOT_EDU", ".edu; ");
							emails = emails.Replace("REPLACE_DOT_ORG", ".org; ");
							emails = emails.Replace("REPLACE_DOT_GOV", ".gov; ");
							emails = emails.Replace("REPLACE_DOT_CA", ".ca; ");
							emails = emails.Replace("REPLACE_DOT_US", ".us; ");
							emails = emails.Replace("REPLACE_DOT_CH", ".ch; ");
							emails = emails.Replace("REPLACE_DOT_UK", ".uk; ");
							emails = emails.Replace("REPLACE_DOT_AU", ".au; ");
							emails = emails.Replace("REPLACE_DOT_DE", ".de; ");
							emails = emails.Replace("REPLACE_DOT_MIL", ".mil; ");
							emails = emails.Replace("REPLACE_DOT_BIZ", ".biz; ");
							emails = emails.Replace("REPLACE_DOT_IT", ".it; ");
							emails = emails.Replace("REPLACE_DOT_ES", ".es; ");
							emails = emails.Replace("REPLACE_DOT_FR", ".fr; ");
							emails = emails.Replace("REPLACE_DOT_IN", ".in; ");
							emails = emails.Replace("REPLACE_DOT_AT", ".at; ");
							emails = emails.Replace("REPLACE_DOT_NZ", ".nz; ");
							emails = emails.Replace("REPLACE_DOT_RU", ".ru; ");
							emails = emails.Replace("REPLACE_DOT_SG", ".sg; ");
							emails = emails.Replace("REPLACE_DOT_MX", ".mx; ");
							emails = emails.Replace("REPLACE_DOT_NL", ".nl; ");
							emails = emails.Replace("REPLACE_DOT_JP", ".jp; ");
						}
						#endregion Case ";"
						break;
					default:
						throw new Exception(String.Format("Unsupported <separator>.{0}{0}Exception thrown in MailUtil.ConvertStringToMailAddresses(string emails='{1}', char separator='{2}')", Environment.NewLine, emails, separator));
				}

				if (emails.Contains(separator.ToString())) { // multiple emails //
					var array = emails.Split(separator);

					for (int i=0; i<array.Length; i++) {
						if (array[i].ToUpper().Contains("UNDISCLOSED"))
							list.Add(new MailAddress("undisclosed@recipients", "Undisclosed Recipients"));
						else {
							try {
								list.Add(new MailAddress(array[i].Replace(";", "").Replace(":", "").Replace(",", "")));
							}

							catch (Exception ex) {
								if (ex.InnerException == null)
									Console.Write("{0}{2}{2}Exception thrown in MailUtil.ConvertStringToMailAddresses(string emails='{3}', char separator='{4}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, emails, separator);

								Console.Write("{0}{2}{2}Exception thrown in INNER EXCEPTION of MailUtil.ConvertStringToMailAddresses(string emails='{3}', char separator='{4}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, emails, separator);
							}
						}

						if (maxEmails > 0 && list.Count >= maxEmails)
							return list;
					}
				}

				else { // single email //
					if (emails.ToUpper().Contains("UNDISCLOSED"))
						list.Add(new MailAddress("undisclosed@recipients", "Undisclosed Recipients"));
					else {
						try {
							list.Add(new MailAddress(emails.Replace(";", "").Replace(":", "").Replace(",", "")));
						}

						catch (Exception ex) {
							if (ex.InnerException == null)
								Console.Write("{0}{2}{2}Exception thrown in MailUtil.ConvertStringToMailAddresses(string emails='{3}', char separator='{4}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, emails, separator);

							Console.Write("{0}{2}{2}Exception thrown in INNER EXCEPTION of MailUtil.ConvertStringToMailAddresses(string emails='{3}', char separator='{4}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, emails, separator);
						}
					}
				}

				return list;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in MailUtil.ConvertStringToMailAddresses(string emails='{3}', char separator='{4}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, emails, separator));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of MailUtil.ConvertStringToMailAddresses(string emails='{3}', char separator='{4}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, emails, separator));
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