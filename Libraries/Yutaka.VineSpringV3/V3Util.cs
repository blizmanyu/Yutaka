using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Yutaka.VineSpringV3
{
	public class V3Util
	{
		#region Fields
		const string MOCK_SERVER_URL	= @"https://private-anon-ba1d162474-vinespring.apiary-mock.com/";
		const string DEBUGGIN_PROXY_URL	= @"https://private-anon-ba1d162474-vinespring.apiary-proxy.com/";
		const string PRODUCTION_URL		= @"https://api.vinespring.com/";

		public Uri BaseAddress;
		public string ApiKey;
		#endregion Fields

		#region Constructor
		public V3Util(string apiKey = null, string baseAddress = null)
		{
			if (String.IsNullOrWhiteSpace(apiKey))
				throw new Exception(String.Format("<apiKey> is required. Exception thrown in Constructor V3Util(string apiKey = null, Uri baseAddress = null).{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(baseAddress))
				baseAddress = MOCK_SERVER_URL;

			ApiKey = apiKey;
			BaseAddress = new Uri(baseAddress);
		}
		#endregion Constructor

		#region Customers
		public async Task<bool> CreateCustomer(Customer customer)
		{
			if (customer == null)
				throw new Exception(String.Format("<apiKey> is required. Exception thrown in V3Util.CreateCustomer(Customer customer).{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(customer.Email))
				throw new Exception(String.Format("<customer.Email> is required. Exception thrown in V3Util.CreateCustomer(Customer customer).{0}", Environment.NewLine));

			try {
				using (var httpClient = new HttpClient { BaseAddress = BaseAddress }) {
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", ApiKey);
					using (var content = new StringContent(String.Format("{ \"email\": \"{0}\",  \"fullName\": \"{1}\",  \"altEmail\": \"{2}\",  \"company\": \"{3}\",  \"dob\": \"{4}\",  \"isTaxExempt\": {5},  \"phone\": \"{6}\",  \"source\": \"{7}\" }", customer.Email, customer.FullName, customer.AltEmail, customer.Company, customer.DoB, customer.IsTaxExempt, customer.Phone, customer.Source), System.Text.Encoding.Default, "application/json")) {
						using (var response = await httpClient.PostAsync("customers", content)) {
							var responseData = await response.Content.ReadAsStringAsync();
						}
					}
				}

				return true;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Email: {3}{2}Exception thrown in V3Util.CreateCustomer(Customer customer)", ex.Message, ex.ToString(), Environment.NewLine, customer.Email));
				else
					throw new Exception(String.Format("{0}{2}Email: {3}{2}Exception thrown in INNER EXCEPTION of V3Util.CreateCustomer(Customer customer)", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, customer.Email));
			}
		}
		#endregion Customers
	}
}