using System;
using System.Net.Mail;

namespace Yutaka.Core
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
		public Email(string address = null)
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