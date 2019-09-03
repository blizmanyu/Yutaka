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

		#region Constructor
		public WSUtil(string baseUrl=null, string userKey=null, string password=null, string customerNumber=null)
		{
			if (String.IsNullOrWhiteSpace(baseUrl))
				baseUrl = PRODUCTION_URL;
			if (String.IsNullOrWhiteSpace(userKey))
				throw new Exception(String.Format("<userKey> is required. Exception thrown in Constructor WSUtil(string baseUrl, string userKey, string password, string customerNumber).{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(password))
				throw new Exception(String.Format("<password> is required. Exception thrown in Constructor WSUtil(string baseUrl, string userKey, string password, string customerNumber).{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(customerNumber))
				throw new Exception(String.Format("<customerNumber> is required. Exception thrown in Constructor WSUtil(string baseUrl, string userKey, string password, string customerNumber).{0}", Environment.NewLine));

			BaseUrl = new Uri(baseUrl);
			UserKey = userKey;
			Password = password;
			CustomerNumber = customerNumber;
		}
		#endregion Constructor

	}
}