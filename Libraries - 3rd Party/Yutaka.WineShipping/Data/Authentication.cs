using System;

namespace Yutaka.WineShipping
{
	public class Authentication
	{
		public string UserKey { get; set; }
		public string Password { get; set; }
		public string CustomerNo { get; set; }

		public string ToJson()
		{
			return String.Format("\"Authentication\": {{ \"UserKey\": \"{0}\", \"Password\": \"{1}\", \"CustomerNo\": \"{2}\" }}", UserKey, Password, CustomerNo);
		}
	}
}