using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Yutaka.VineSpringV3
{
	public class V3Util
	{
		#region Fields
		public const string MOCK_SERVER_URL = @"https://private-anon-ba1d162474-vinespring.apiary-mock.com/";
		public const string DEBUGGIN_PROXY_URL  = @"https://private-anon-ba1d162474-vinespring.apiary-proxy.com/";
		public const string PRODUCTION_URL      = @"https://api.vinespring.com/";
		public const string TIME_FORMAT = @"yyyy-MM-ddT00:00.000Z";
		private readonly DateTime DOB_THRESHOLD;

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
			DOB_THRESHOLD = DateTime.Now.AddYears(-100);
		}
		#endregion Constructor

		#region Customers
		public async Task<string> CreateCustomer(Customer customer)
		{
			if (customer == null)
				throw new Exception(String.Format("<customer> is required. Exception thrown in V3Util.CreateCustomer(Customer customer).{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(customer.Email))
				throw new Exception(String.Format("<customer.Email> is required. Exception thrown in V3Util.CreateCustomer(Customer customer).{0}", Environment.NewLine));

			try {
				var str = String.Format("{{ \"email\": \"{0}\"", customer.Email);

				if (!String.IsNullOrWhiteSpace(customer.FullName))
					str = String.Format("{0}, \"fullName\": \"{1}\"", str, customer.FullName);
				if (!String.IsNullOrWhiteSpace(customer.AltEmail))
					str = String.Format("{0}, \"altEmail\": \"{1}\"", str, customer.AltEmail);
				if (!String.IsNullOrWhiteSpace(customer.Company))
					str = String.Format("{0}, \"company\": \"{1}\"", str, customer.Company);
				if (customer.DoB != null && customer.DoB > DOB_THRESHOLD)
					str = String.Format("{0}, \"dob\": \"{1}\"", str, customer.DoB.Value.ToString(TIME_FORMAT));
				if (customer.IsTaxExempt != null)
					str = String.Format("{0}, \"isTaxExempt\": \"{1}\"", str, customer.IsTaxExempt);
				if (!String.IsNullOrWhiteSpace(customer.Phone))
					str = String.Format("{0}, \"phone\": \"{1}\"", str, customer.Phone);
				if (!String.IsNullOrWhiteSpace(customer.Source))
					str = String.Format("{0}, \"source\": \"{1}\"", str, customer.Source);

				str = String.Format("{0} }}", str);
				Console.Write("\n{0}", str);

				using (var httpClient = new HttpClient { BaseAddress = BaseAddress }) {
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", ApiKey);
					using (var content = new StringContent(str, System.Text.Encoding.Default, "application/json")) {
						using (var response = await httpClient.PostAsync("customers", content)) {
							var responseData = await response.Content.ReadAsStringAsync();
							return responseData;
						}
					}
				}
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Email: {3}{2}Exception thrown in V3Util.CreateCustomer(Customer customer)", ex.Message, ex.ToString(), Environment.NewLine, customer.Email));
				else
					throw new Exception(String.Format("{0}{2}Email: {3}{2}Exception thrown in INNER EXCEPTION of V3Util.CreateCustomer(Customer customer)", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, customer.Email));
			}
		}

		public async Task<string> DeleteCustomer(string customerId)
		{
			if (String.IsNullOrWhiteSpace(customerId))
				throw new Exception(String.Format("<customerId> is required. Exception thrown in V3Util.DeleteCustomer(string customerId).{0}", Environment.NewLine));

			try {
				var str = String.Format("customers/{0}", customerId);
				Console.Write("\n{0}", str);

				using (var httpClient = new HttpClient { BaseAddress = BaseAddress }) {
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", ApiKey);
					using (var response = await httpClient.DeleteAsync(str)) {
						var responseData = await response.Content.ReadAsStringAsync();
						return responseData;
					}
				}
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in V3Util.DeleteCustomer(string customerId='{3}')", ex.Message, ex.ToString(), Environment.NewLine, customerId));
				else
					throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of V3Util.DeleteCustomer(string customerId='{3}')", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, customerId));
			}
		}

		public async Task<string> GetCustomer(string customerId)
		{
			if (String.IsNullOrWhiteSpace(customerId))
				throw new Exception(String.Format("<customerId> is required. Exception thrown in V3Util.GetCustomer(string customerId).{0}", Environment.NewLine));

			try {
				var str = String.Format("customers/{0}", customerId);
				Console.Write("\n{0}", str);

				using (var httpClient = new HttpClient { BaseAddress = BaseAddress }) {
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", ApiKey);
					using (var response = await httpClient.GetAsync(str)) {
						var responseData = await response.Content.ReadAsStringAsync();
						return responseData;
					}
				}
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in V3Util.GetCustomer(string customerId='{3}')", ex.Message, ex.ToString(), Environment.NewLine, customerId));
				else
					throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of V3Util.GetCustomer(string customerId='{3}')", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, customerId));
			}
		}

		public async Task<string> ResetPassword(string email)
		{
			if (String.IsNullOrWhiteSpace(email))
				throw new Exception(String.Format("<email> is required. Exception thrown in V3Util.ResetPassword(string email).{0}", Environment.NewLine));

			try {
				var str = String.Format("{{ \"email\": \"{0}\" }}", email);
				Console.Write("\n{0}", str);

				using (var httpClient = new HttpClient { BaseAddress = BaseAddress }) {
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", ApiKey);
					using (var content = new StringContent(str, System.Text.Encoding.Default, "application/json")) {
						using (var response = await httpClient.PostAsync("customers/passwordReset", content)) {
							var responseData = await response.Content.ReadAsStringAsync();
							return responseData;
						}
					}
				}
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Email: {3}{2}Exception thrown in V3Util.ResetPassword(string email)", ex.Message, ex.ToString(), Environment.NewLine, email));
				else
					throw new Exception(String.Format("{0}{2}Email: {3}{2}Exception thrown in INNER EXCEPTION of V3Util.ResetPassword(string email)", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, email));
			}
		}

		public async Task<string> UpdateCustomer(Customer customer)
		{
			if (customer == null)
				throw new Exception(String.Format("<customer> is required. Exception thrown in V3Util.UpdateCustomer(Customer customer).{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(customer.Id))
				throw new Exception(String.Format("<customer.Id> is required. Exception thrown in V3Util.UpdateCustomer(Customer customer).{0}", Environment.NewLine));

			try {
				var str = String.Format("{{ \"fullName\": \"{0}\"", customer.FullName);

				if (!String.IsNullOrWhiteSpace(customer.AltEmail))
					str = String.Format("{0}, \"altEmail\": \"{1}\"", str, customer.AltEmail);
				if (!String.IsNullOrWhiteSpace(customer.Company))
					str = String.Format("{0}, \"company\": \"{1}\"", str, customer.Company);
				if (customer.DoB != null && customer.DoB > DOB_THRESHOLD)
					str = String.Format("{0}, \"dob\": \"{1}\"", str, customer.DoB.Value.ToString(TIME_FORMAT));
				if (customer.IsTaxExempt != null)
					str = String.Format("{0}, \"isTaxExempt\": \"{1}\"", str, customer.IsTaxExempt);
				if (!String.IsNullOrWhiteSpace(customer.Phone))
					str = String.Format("{0}, \"phone\": \"{1}\"", str, customer.Phone);
				if (!String.IsNullOrWhiteSpace(customer.Source))
					str = String.Format("{0}, \"source\": \"{1}\"", str, customer.Source);

				str = String.Format("{0} }}", str);
				Console.Write("\n{0}", str);

				using (var httpClient = new HttpClient { BaseAddress = BaseAddress }) {
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", ApiKey);
					using (var content = new StringContent(str, System.Text.Encoding.Default, "application/json")) {
						var requestUri = new Uri(BaseAddress, String.Format("customers/{0}", customer.Id));
						using (var request = new HttpRequestMessage { Method = new HttpMethod("PATCH"), RequestUri = requestUri, Content = content }) {
							using (var response = await httpClient.SendAsync(request)) {
								var responseData = await response.Content.ReadAsStringAsync();
								return responseData;
							}
						}
					}
				}
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Id: {3}{2}Exception thrown in V3Util.UpdateCustomer(Customer customer)", ex.Message, ex.ToString(), Environment.NewLine, customer.Id));
				else
					throw new Exception(String.Format("{0}{2}Id: {3}{2}Exception thrown in INNER EXCEPTION of V3Util.UpdateCustomer(Customer customer)", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, customer.Id));
			}
		}
		#endregion Customers

		#region Orders
		#endregion Orders

		#region Products
		#endregion Products
	}
}