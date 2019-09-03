using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

		#region Methods
		public void DisplayResponse(Task<string> response, bool pretty = true)
		{
			if (response == null || String.IsNullOrWhiteSpace(response.Result))
				return;

			if (pretty) {
				dynamic parsedJson = JsonConvert.DeserializeObject(response.Result);
				var formatted = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
				Console.Write("\n{0}", formatted);
			}

			else
				Console.Write("\n{0}", response.Result);
		}

		public async Task<string> GetInventoryStatus(string warehouse=null, string[] itemNumbers=null, bool? includeTotalRecordCount=null, int? skip=null, int? top=null)
		{
			if (includeTotalRecordCount == null)
				includeTotalRecordCount = true;

			try {
				var endpoint = "api/Inventory/GetStatus";
				var request = new InventoryStatusRequest {
					Authentication = new Authentication { UserKey = UserKey, Password = Password, CustomerNo = CustomerNumber, },
					Warehouse = warehouse,
					ItemNumbers = itemNumbers,
					IncludeTotalRecordCount = includeTotalRecordCount,
					Skip = skip,
					Top = top,
				};


				using (var httpClient = new HttpClient { BaseAddress = BaseUrl }) {
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
					using (var content = new StringContent(request.ToJson(), Encoding.Default, "application/json")) {
						using (var response = await httpClient.PostAsync(endpoint, content)) {
							var responseData = await response.Content.ReadAsStringAsync();
							return responseData;
						}
					}
				}
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in V3Util.GetInventoryStatus(string warehouse='{3}', string[] itemNumbers='{4}', bool? includeTotalRecordCount='{5}', int? skip='{6}', int? top='{7}')", ex.Message, ex.ToString(), Environment.NewLine, warehouse, String.Join(",", itemNumbers), includeTotalRecordCount, skip, top));
				else
					throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of V3Util.GetInventoryStatus(string warehouse='{3}', string[] itemNumbers='{4}', bool? includeTotalRecordCount='{5}', int? skip='{6}', int? top='{7}')", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, warehouse, String.Join(",", itemNumbers), includeTotalRecordCount, skip, top));
			}
		}

		#endregion Methods

	}
}