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

		public V3Util(string apiKey = null, string baseAddress = null)
		{
			if (String.IsNullOrWhiteSpace(apiKey))
				throw new Exception("<apiKey> is required. Exception thrown in Constructor V3Util(string apiKey = null, Uri baseAddress = null).");
			if (String.IsNullOrWhiteSpace(baseAddress))
				baseAddress = MOCK_SERVER_URL;

			ApiKey = apiKey;
			BaseAddress = new Uri(baseAddress);
		}

		#region Customers
		public async Task<bool> CreateCustomer(Customer customer)
		{
			try {
				using (var httpClient = new HttpClient { BaseAddress = BaseAddress }) {
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", ApiKey);
					using (var content = new StringContent("{  }", System.Text.Encoding.Default, "application/json")) {
						using (var response = await httpClient.PostAsync("customers", content)) {
							string responseData = await response.Content.ReadAsStringAsync();
						}
					}
				}

				return true;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in V3Util.CreateCustomer(Customer customer)", ex.Message, ex.ToString(), Environment.NewLine));

				return false;
			}
		}
		#endregion Customers
	}
}