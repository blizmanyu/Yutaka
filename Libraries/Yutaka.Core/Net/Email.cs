using System;
using System.Net.Mail;

namespace Yutaka.Core.Net
{
	public class Email
	{
		#region Fields
		public string Address;
		public string DisplayName;
		public string Host;
		public string User;
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