using System;
using System.Net.Mail;

namespace Yutaka.Core.Net
{
	public class Email
	{
		#region Fields
		/// <summary>
		/// Represents the address of an electronic mail sender or recipient.
		/// </summary>
		private MailAddress MailAddress;
		/// <summary>
		/// Gets the email address specified when this instance was created.
		/// </summary>
		/// <remarks>The value returned by this property does not include the DisplayName information.</remarks>
		public string Address { get; }
		/// <summary>
		/// Gets the display name composed from the display name and address information specified when this instance was created.
		/// </summary>
		/// <remarks>
		/// Display names that contain non-ASCII characters are returned in human-readable form.
		/// Use the ToString method to get the encoded form of the DisplayName.
		/// Some software programs that are used to read email display the DisplayName property value instead of, or in addition to, the email address.
		/// </remarks>
		public string DisplayName { get; }
		/// <summary>
		/// Gets the host portion of the address specified when this instance was created.
		/// </summary>
		/// <remarks>
		/// In a typical email address, the host string includes all information following the "@" sign.
		/// For example, in "tsmith@contoso.com", the host is "contoso.com".
		/// </remarks>
		public string Host { get; }
		/// <summary>
		/// Gets the user information from the address specified when this instance was created.
		/// </summary>
		/// <remarks>
		/// In a typical email address, the user string includes all information preceding the "@" sign.
		/// For example, in "tsmith@contoso.com", the user is "tsmith".
		/// </remarks>
		public string User { get; }
		#endregion Fields

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="MailAddress"/> class using the specified address.
		/// </summary>
		/// <param name="address">A <see cref="string"/> that contains an email address.</param>
		public Email(string address)
		{
			#region Check Input
			if (address == null)
				throw new ArgumentNullException("address");
			else if (String.IsNullOrWhiteSpace(address))
				throw new ArgumentException("<address> is empty.");
			else if (address.IndexOf("undisclosed", StringComparison.OrdinalIgnoreCase) > -1)
				address = "Undisclosed Recipients <undisclosed@recipients>";
			else
				address = address.Trim();
			#endregion Check Input

			var email = new MailAddress(address);
			DisplayName = email.DisplayName.Trim();
			Host = email.Host.Trim();
			User = email.User.Trim();

			if (Host.Equals(Host.ToUpper()))
				Host = Host.ToLower();
			if (User.Equals(User.ToUpper()))
				User = User.ToLower();

			Address = String.Format("{0}@{1}", User, Host);
		}
		#endregion Constructors
	}
}