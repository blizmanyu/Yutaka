using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.WineShipping.Data
{
	public class AuthenticationDetails
	{
		public string UserKey { get; set; }
		public string Password { get; set; }
		public string CustomerNo { get; set; }

		public string ToJson()
		{
			return String.Format("\"AuthenticationDetails\": {{ \"UserKey\": \"{0}\", \"Password\": \"{1}\", \"CustomerNo\": \"{2}\" }}", UserKey, Password, CustomerNo);
		}
	}
}
