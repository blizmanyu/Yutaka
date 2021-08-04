using System;
using System.Net.Mail;
using System.Text;

namespace Yutaka.Core.Net
{
	public class Email
	{
		#region Fields
		/// <summary>
		/// Represents the address of an electronic mail sender or recipient.
		/// </summary>
		private MailAddress _mailAddress;

		/// <summary>
		/// Gets the email address specified when this instance was created.
		/// </summary>
		/// <remarks>The value returned by this property does not include the DisplayName information.</remarks>
		public string Address {
			get {
				return String.Format("{0}@{1}", User, Host);
			}
		}

		/// <summary>
		/// Gets the display name composed from the display name and address information specified when this instance was created.
		/// </summary>
		/// <remarks>
		/// Display names that contain non-ASCII characters are returned in human-readable form.
		/// Use the ToString method to get the encoded form of the DisplayName.
		/// Some software programs that are used to read email display the DisplayName property value instead of, or in addition to, the email address.
		/// </remarks>
		public string DisplayName {
			get {
				if (String.IsNullOrWhiteSpace(_mailAddress.DisplayName))
					return "";

				var temp = _mailAddress.DisplayName.Trim();

				while (temp.Contains("  "))
					temp = temp.Replace("  ", " ");

				return temp;
			}
		}

		/// <summary>
		/// Gets the host portion of the address specified when this instance was created.
		/// </summary>
		/// <remarks>
		/// In a typical email address, the host string includes all information following the "@" sign.
		/// For example, in "tsmith@contoso.com", the host is "contoso.com".
		/// </remarks>
		public string Host {
			get {
				if (String.IsNullOrWhiteSpace(_mailAddress.Host))
					return "";
				if (_mailAddress.Host.Equals(_mailAddress.Host.ToUpper()))
					return _mailAddress.Host.ToLower();

				return _mailAddress.Host;
			}
		}

		/// <summary>
		/// Gets the original email address that was passed to the <see cref="Email"/> constructor.
		/// </summary>
		/// <remarks>
		/// If the address specified to the constructor contained leading or trailing spaces, these spaces are preserved.
		/// The value returned by this property differs from ToString.
		/// </remarks>
		public string OriginalString { get; }

		/// <summary>
		/// Gets the user information from the address specified when this instance was created.
		/// </summary>
		/// <remarks>
		/// In a typical email address, the user string includes all information preceding the "@" sign.
		/// For example, in "tsmith@contoso.com", the user is "tsmith".
		/// </remarks>
		public string User {
			get {
				if (String.IsNullOrWhiteSpace(_mailAddress.User))
					return "";
				if (_mailAddress.User.Equals(_mailAddress.User.ToUpper()))
					return _mailAddress.User.ToLower();

				return _mailAddress.User;
			}
		}
		#endregion Fields

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="MailAddress"/> class using the specified address.
		/// </summary>
		/// <param name="address">A <see cref="string"/> that contains an email address.</param>
		public Email(string address)
		{
			#region Check Input
			if (String.IsNullOrWhiteSpace(address))
				throw new Exception(String.Format("'address' is required.{0}Exception thrown in constructor Email(string address).{0}", Environment.NewLine));
			else {
				OriginalString = address;

				if (address.IndexOf("undisclosed", StringComparison.OrdinalIgnoreCase) > -1)
					address = "Undisclosed Recipients <undisclosed@recipients>";
			}
			#endregion Check Input

			try {
				_mailAddress = new MailAddress(address);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in Constructor Email(string address='{3}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, address));
				else
					throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Constructor Email(string address='{3}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, address));
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Email"/> class using the specified address and display name.
		/// </summary>
		/// <param name="address">A <see cref="string"/> that contains an email address.</param>
		/// <param name="displayName">A <see cref="string"/> that contains the display name associated with address. This parameter can be null.</param>
		public Email(string address, string displayName)
		{
			#region Check Input
			if (String.IsNullOrWhiteSpace(address))
				throw new Exception(String.Format("'address' is required.{0}Exception thrown in constructor Email(string address, string displayName).{0}", Environment.NewLine));
			else {
				OriginalString = address;

				if (address.IndexOf("undisclosed", StringComparison.OrdinalIgnoreCase) > -1)
					address = "Undisclosed Recipients <undisclosed@recipients>";
			}
			#endregion Check Input

			try {
				_mailAddress = new MailAddress(address, displayName);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in Constructor Email(string address='{3}', string displayName='{4}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, address, displayName));
				else
					throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Constructor Email(string address='{3}', string displayName='{4}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, address, displayName));
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Email"/> class using the specified address, display name, and encoding.
		/// </summary>
		/// <param name="address">A <see cref="string"/> that contains an email address.</param>
		/// <param name="displayName">A <see cref="string"/> that contains the display name associated with address.</param>
		/// <param name="displayNameEncoding">The <see cref="Encoding"/> that defines the character set used for displayName.</param>
		public Email(string address, string displayName, Encoding displayNameEncoding)
		{
			#region Check Input
			var log = "";

			if (String.IsNullOrWhiteSpace(address))
				log = String.Format("{0}'address' is required.{1}", log, Environment.NewLine);
			else {
				OriginalString = address;

				if (address.IndexOf("undisclosed", StringComparison.OrdinalIgnoreCase) > -1)
					address = "Undisclosed Recipients <undisclosed@recipients>";
			}

			if (String.IsNullOrWhiteSpace(displayName))
				log = String.Format("{0}'displayName' is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in Email(string address, string displayName, Encoding displayNameEncoding).{1}", log, Environment.NewLine);
				throw new Exception(log);
			}
			#endregion Check Input

			try {
				_mailAddress = new MailAddress(address, displayName, displayNameEncoding);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in Constructor Email(string address='{3}', string displayName='{4}', Encoding displayNameEncoding='{5}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, address, displayName, displayNameEncoding));
				else
					throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Constructor Email(string address='{3}', string displayName='{4}', Encoding displayNameEncoding='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, address, displayName, displayNameEncoding));
			}
		}
		#endregion Constructors

		#region Methods
		public void Debug()
		{
			Console.Write("\n");
			Console.Write("\nOriginalString: {0}", OriginalString);
			Console.Write("\n   DisplayName: {0}", DisplayName);
			Console.Write("\n          User: {0}", User);
			Console.Write("\n          Host: {0}", Host);
			Console.Write("\n       Address: {0}", Address);
			Console.Write("\n      ToString: {0}", ToString());
			Console.Write("\n");
		}

		public override string ToString()
		{
			if (String.IsNullOrWhiteSpace(DisplayName))
				return Address;

			return String.Format("{0} <{1}>", DisplayName, Address);
		}
		#endregion
	}
}