using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Yutaka.IO;
using Yutaka.WineShipping.Data;

namespace Yutaka.WineShipping
{
	public class WSUtil
	{
		#region Fields
		public const int DEFAULT_TOP = 480;
		public const string PRODUCTION_URL = @"https://services.wineshipping.com/";
		public const string TEST_URL = @"https://wsservices-test.azurewebsites.net/";
		public Uri BaseUrl;
		public string UserKey;
		public string Password;
		public string CustomerNumber;
		private bool debug = true; //default should be false

		#endregion Fields

		#region Constructor
		public WSUtil(string userKey=null, string password=null, string customerNumber=null, string baseUrl=null)
		{
			if (String.IsNullOrWhiteSpace(userKey))
				throw new Exception(String.Format("<userKey> is required. Exception thrown in Constructor WSUtil(string baseUrl, string userKey, string password, string customerNumber).{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(password))
				throw new Exception(String.Format("<password> is required. Exception thrown in Constructor WSUtil(string baseUrl, string userKey, string password, string customerNumber).{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(customerNumber))
				throw new Exception(String.Format("<customerNumber> is required. Exception thrown in Constructor WSUtil(string baseUrl, string userKey, string password, string customerNumber).{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(baseUrl))
				baseUrl = TEST_URL;

			BaseUrl = new Uri(baseUrl);
			UserKey = userKey;
			Password = password;
			CustomerNumber = customerNumber;
		}
		#endregion Constructor

		#region Utilities
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

		public int? ItemUnitToBottlesPerUnit(string itemUnit)
		{
			if (String.IsNullOrWhiteSpace(itemUnit))
				return 1;

			itemUnit = itemUnit.ToUpper();

			switch (itemUnit) {
				#region case 1.5L
				case "1500":
					return 6;
				case "1500-1-W":
					return 1;
				#endregion case 1.5L
				#region case 3L
				case "3000":
				case "3000-1-W":
					return 1;
				#endregion case 3L
				#region case 750mL
				case "750":
					return 12;
				case "750-3":
					return 3;
				case "750-3-W":
					return 3;
				#endregion case 750mL
				#region case 9L
				case "9000":
					return 1;
				#endregion case 9L
				default:
					return 1;
			}
		}

		public int? ItemUnitToBottlesPerCase(string itemUnit)
		{
			if (String.IsNullOrWhiteSpace(itemUnit))
				return 1;

			itemUnit = itemUnit.ToUpper();

			switch (itemUnit) {
				#region case 1.5L
				case "1500":
				case "1500-1-W":
					return 6;
				#endregion case 1.5L
				#region case 3L
				case "3000":
				case "3000-1-W":
					return 3;
				#endregion case 3L
				#region case 750mL
				case "750":
				case "750-3":
				case "750-3-W":
					return 12;
				#endregion case 750mL
				#region case 9L
				case "9000":
					return 1;
				#endregion case 9L
				default:
					return 1;
			}
		}

		public void WriteToFile(Task<string> response, bool pretty = true)
		{
			if (response == null || String.IsNullOrWhiteSpace(response.Result))
				return;

			var filename = String.Format("{0}.json", DateTime.Now.ToString("yyyy MMdd HHmm ssff"));

			if (pretty) {
				dynamic parsedJson = JsonConvert.DeserializeObject(response.Result);
				var formatted = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
				new FileUtil().Write(formatted, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), filename));
			}

			else
				new FileUtil().Write(response.Result, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), filename));
		}
		#endregion Utilities

		#region Methods - API Calls
		/// <summary>
		/// Gets Inventory information using the API call /api/Inventory/GetStatus.
		/// </summary>
		/// <param name="warehouse">Optional: A warehouse code to return related inventory records for a specific Wineshipping warehouse. If omitted, the operation will return inventory records for all warehouses.</param>
		/// <param name="itemNumbers">Optional: And array of items to query. If omitted, returns inventory records for all items in the warehouse specified.</param>
		/// <param name="includeTotalRecordCount">Whether to return the total number of records found or not.</param>
		/// <param name="skip"></param>
		/// <param name="top">Maximum number of records to return. Default is set in the DEFAULT_TOP field.</param>
		/// <returns></returns>
		public async Task<string> GetInventoryStatus(string warehouse=null, string[] itemNumbers=null, bool? includeTotalRecordCount=null, int? skip=null, int? top=null)
		{
			if (includeTotalRecordCount == null)
				includeTotalRecordCount = true;
			if (skip == null)
				skip = 0;
			if (top == null)
				top = DEFAULT_TOP;

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
					throw new Exception(String.Format("{0}{2}Exception thrown in WSUtil.GetInventoryStatus(string warehouse='{3}', string[] itemNumbers='{4}', bool? includeTotalRecordCount='{5}', int? skip='{6}', int? top='{7}')", ex.Message, ex.ToString(), Environment.NewLine, warehouse, String.Join(",", itemNumbers), includeTotalRecordCount, skip, top));
				else
					throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of WSUtil.GetInventoryStatus(string warehouse='{3}', string[] itemNumbers='{4}', bool? includeTotalRecordCount='{5}', int? skip='{6}', int? top='{7}')", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, warehouse, String.Join(",", itemNumbers), includeTotalRecordCount, skip, top));
			}
		}

		/// <summary>
		/// Use this operation to retrieve a list of on-hold orders for a customer. 
		/// </summary>
		/// <returns>Successful execution of this method will generate a list of on hold orders from Wineshipping's system and if no orders are on hold, HTTP status code Not Found response will be returned.</returns>
		public async Task<string> GetOrdersOnHold(string userKey, string password, string customerNo)
		{
			if (String.IsNullOrWhiteSpace(userKey))
				throw new Exception(String.Format("Exception thrown in WSUtil.GetOrdersOnHold(string userKey, string password, string customerNo). userKey is required."));

			if (String.IsNullOrWhiteSpace(password))
				throw new Exception(String.Format("Exception thrown in WSUtil.GetOrdersOnHold(string userKey, string password, string customerNo). password is required."));

			if (String.IsNullOrWhiteSpace(customerNo))
				throw new Exception(String.Format("Exception thrown in WSUtil.GetOrdersOnHold(string userKey, string password, string customerNo). customerNo is required."));


			try {
				var endpoint = "api/SalesOrder/GetOrdersOnHold";
				var request = String.Format("{{ \"UserKey\" : \"{0}\", \"Password\" : \"{1}\", \"CustomerNo\" : \"{2}\" }}", userKey, password, customerNo);

				if (debug) {
					var m = MethodBase.GetCurrentMethod();
					var currentMethod = m.ReflectedType.Name;
					Console.Write(String.Format("\n Executing: {0} => {1}", endpoint, m.ReflectedType.Name));
					Console.WriteLine(String.Format("\n Request: {0}", request));
				}

				using (var httpClient = new HttpClient { BaseAddress = BaseUrl }) {
					httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
					using (var content = new StringContent(request, Encoding.Default, "application/json")) {
						using (var response = await httpClient.PostAsync(endpoint, content)) {
							var responseData = await response.Content.ReadAsStringAsync();
							return responseData;
						}
					}
				}
			}
			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in WSUtil.GetOrdersOnHold(string userKey={3}. string password={4}. string customerNo={5})", ex.Message, ex.ToString(), Environment.NewLine, userKey, password, customerNo));
				else
					throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of WSUtil.GetOrdersOnHold(string userKey={3}. string password={4}. string customerNo={5})", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, userKey, password, customerNo));
			}


		}

		/// <summary>
		/// This operation provides sales order and associated package tracking information and their status if available. This operation accepts a customer number and order number to locate the order information within the Wineshipping system.
		/// </summary>
		/// <param name="orderNo">The Order Number</param>
		/// <returns> The result will include individual packages, associated tracking numbers, carrier status, and the shipping address information.</returns>
		public async Task<string> GetDetails(string userKey, string password, string customerNo, string orderNo)
		{
			if (String.IsNullOrWhiteSpace(userKey))
				throw new Exception(String.Format("Exception thrown in WSUtil.GetDetails(string userKey, string password, string customerNo, string orderNo). userKey is required."));

			if (String.IsNullOrWhiteSpace(password))
				throw new Exception(String.Format("Exception thrown in WSUtil.GetDetails(string userKey, string password, string customerNo, string orderNo). password is required."));

			if (String.IsNullOrWhiteSpace(customerNo))
				throw new Exception(String.Format("Exception thrown in WSUtil.GetDetails(string userKey, string password, string customerNo, string orderNo). customerNo is required."));

			if (String.IsNullOrWhiteSpace(orderNo))
				throw new Exception(String.Format("Exception thrown in WSUtil.GetDetails(string userKey, string password, string customerNo, string orderNo). OrderNo is required."));

			try {
				var endpoint = "api/Tracking/GetDetails";

				var request = new TrackingDetailRequest {
					AuthenticationDetails = new AuthenticationDetails { UserKey = userKey, Password = password, CustomerNo = customerNo, },
					OrderNo = orderNo
				};

				if (debug) {
					var m = MethodBase.GetCurrentMethod();
					var currentMethod = m.ReflectedType.Name;
					Console.Write(String.Format("\n Executing: {0} => {1}", endpoint, m.ReflectedType.Name));
					Console.WriteLine(String.Format("\n Request: {0}", request.ToJson()));
				}

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
					throw new Exception(String.Format("{0}{2}Exception thrown in WSUtil.GetDetails(string userKey={3}. string password={4}. string customerNo={5} string orderNo={6})", ex.Message, ex.ToString(), Environment.NewLine, userKey, password, customerNo,  orderNo));
				else
					throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of WSUtil.GetDetails(string userKey={3}. string password={4}. string customerNo={5} string orderNo={6})", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, userKey, password, customerNo, orderNo));
			}
		}


		#endregion Methods - API Calls
	}
}