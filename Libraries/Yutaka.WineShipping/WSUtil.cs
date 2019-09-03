using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.WineShipping
{
	public class WSUtil
	{
		#region Fields
		public const string PRODUCTION_URL = @"https://wsservices-test.azurewebsites.net/";

		public Uri BaseUrl;
		public string UserKey;
		public string Password;
		public string CustomerNumber;
		#endregion Fields

	}
}