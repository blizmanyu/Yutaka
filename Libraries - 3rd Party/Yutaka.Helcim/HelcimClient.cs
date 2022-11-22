using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Yutaka.IO;

namespace Yutaka.Helcim
{
	public class HelcimClient : BaseClient
	{
		#region Fields
		// Constants and readonlys //
		public const string BASE_URL = "https://secure.myhelcim.com/api/";
		private static readonly string LogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HelcimClient");

		// Properties //
		public bool Debug;
		public string ApiToken;
		public uint AccountId;
		private readonly string Timestamp;

		// PIVs //
		private static FileUtil fileUtil = new FileUtil();
		#endregion

		#region Constructor
		public HelcimClient(uint accountId, string apiToken)
		{
			if (String.IsNullOrWhiteSpace(apiToken))
				throw new Exception(String.Format("'apiToken' is required.{0}Exception thrown in Constructor HelcimClient(uint accountId, string apiToken).{0}", Environment.NewLine));

			Debug = false;
			Timestamp = DateTime.Now.ToString("yyyy MMdd HHmm ssff");
			AccountId = accountId;
			ApiToken = apiToken;
		}
		#endregion

		#region Utilities
		private string GetAuthenticationString()
		{
			var str = "{ ";
			str = String.Format("{0}\"accountId\": {1}, ", str, AccountId);
			str = String.Format("{0}\"apiToken\": \"{1}\", ", str, ApiToken);
			return str;
		}

		private void AddHeaders()
		{
			Client.DefaultRequestHeaders.TryAddWithoutValidation("account-id", AccountId.ToString());
			Client.DefaultRequestHeaders.TryAddWithoutValidation("api-token", ApiToken);
		}

		#region Writes
		public void JsonToFile(string response, bool pretty = true)
		{
			if (response == null || String.IsNullOrWhiteSpace(response))
				return;

			var filename = String.Format("{0}.json", Timestamp);
			Directory.CreateDirectory(LogPath);

			if (pretty) {
				dynamic parsedJson = JsonConvert.DeserializeObject(response);
				var formatted = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
				fileUtil.Write(formatted, Path.Combine(LogPath, filename));
			}

			else
				fileUtil.Write(response, Path.Combine(LogPath, filename));

			fileUtil.Write(Environment.NewLine, Path.Combine(LogPath, filename));
		}

		public void WriteToConsole(Task<string> response, bool pretty = true)
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

		public void WriteToFile(Task<string> response, bool pretty = true)
		{
			if (response == null || String.IsNullOrWhiteSpace(response.Result))
				return;

			var filename = String.Format("{0}.xml", Timestamp);
			Directory.CreateDirectory(LogPath);

			if (pretty) {
				dynamic parsedJson = JsonConvert.DeserializeObject(response.Result);
				var formatted = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
				fileUtil.Write(formatted, Path.Combine(LogPath, filename));
			}

			else
				fileUtil.Write(response.Result, Path.Combine(LogPath, filename));
		}

		public void XmlToFile(string response)
		{
			if (response == null || String.IsNullOrWhiteSpace(response))
				return;

			var filename = String.Format("{0}.xml", Timestamp);
			Directory.CreateDirectory(LogPath);
			fileUtil.Write(response, Path.Combine(LogPath, filename));
			fileUtil.Write(Environment.NewLine, Path.Combine(LogPath, filename));
		}
		#endregion Writes
		#endregion

		#region Methods
		/// <summary>
		/// This API call lets you retrieve the customer's detailed information, including any cards in the vault associated with the customer.
		/// The response is an XML of the processed sale.
		/// </summary>
		/// <param name="customerCode">The customer ID code. CST1263</param>
		/// <returns></returns>
		public async Task<string> GetCustomer(string customerCode)
		{
			#region Check Input
			var log = "";

			if (String.IsNullOrWhiteSpace(customerCode))
				log = String.Format("{0}customerCode is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				Console.Write("\n{0}", log);
				XmlToFile(log);
				return null;

			}
			#endregion

			try {
				var thisAction = "customer/view";
				var action = String.Format("{0}{1}", BASE_URL, thisAction);

				using (var requestBody = new StringContent(String.Format("customerCode={0}", customerCode), System.Text.Encoding.Default, "application/x-www-form-urlencoded")) {
					AddHeaders();
					using (var response = await Client.PostAsync(action, requestBody)) {
						string responseData = await response.Content.ReadAsStringAsync();
						//Thread.Sleep(1000);
						#region Debug
						if (Debug) {
							Console.Write("\n{0}", responseData);
							XmlToFile(responseData);
						}
						#endregion

						return responseData;
					}
				}
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in HelcimClient.GetCustomer(string customerCode='{3}'", ex.Message, ex.ToString(), Environment.NewLine, customerCode);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of HelcimClient.GetCustomer(string customerCode='{3}'", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, customerCode);

				Console.Write("\n{0}", log);
				XmlToFile(log);
				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// This API call lets you retrieve the customer's detailed information, including any cards in the vault associated with the customer.
		/// The response is an XML of the processed sale.
		/// </summary>
		/// <returns></returns>
		public async Task<string> ListCustomers()
		{
			#region Check Input
			var log = "";
			#endregion

			try {
				var thisAction = "customer/search";
				var action = String.Format("{0}{1}", BASE_URL, thisAction);

				using (var content = new StringContent(action, System.Text.Encoding.Default, "application/x-www-form-urlencoded")) {
					AddHeaders();
					using (var response = await Client.PostAsync(action, content)) {
						string responseData = await response.Content.ReadAsStringAsync();
						//Thread.Sleep(1000);
						#region Debug  

						if (Debug) {
							Console.Write("\n{0}", responseData);
							XmlToFile(responseData);
						}
						#endregion
						return responseData;
					}
				}
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in HelcimClient.ListCustomers().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					log = String.Format("{0}{2}Exception thrown in HelcimClient.ListCustomers().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);

				Console.Write("\n{0}", log);
				XmlToFile(log);
				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// This API call lets you process a purchase transaction for a customer that has already been saved in your Helcim account.
		/// The response is an XML of the processed sale.
		/// </summary>
		/// <param name="customerCode">The customer ID code.</param>
		/// <param name="amount">The amount of the transaction. Must be at least $1.</param>
		/// <param name="comments">Optional comments added to the transaction.</param>
		/// <returns></returns>
		public async Task Purchase(string customerCode, decimal amount, string comments = null)
		{
			#region Check Input
			var log = "";

			if (String.IsNullOrWhiteSpace(customerCode))
				log = String.Format("{0}customerCode is required.{1}", log, Environment.NewLine);
			if (amount < 1)
				log = String.Format("{0}amount must be at least $1.{1}", log, Environment.NewLine);
			if (comments != null)
				comments = comments.Trim();

			if (!String.IsNullOrWhiteSpace(log)) {
				Console.Write("\n{0}", log);
				XmlToFile(log);
				return;
			}
			#endregion

			try {
				var str = GetAuthenticationString();
				str = String.Format("{0}\"transactionType\": \"purchase\", ", str);
				str = String.Format("{0}\"customerCode\": \"{1}\", ", str, customerCode);
				str = String.Format("{0}\"amount\": {1}, ", str, amount);

				if (!String.IsNullOrWhiteSpace(comments))
					str = String.Format("{0}\"comments\": \"{1}\" ", str, comments);

				str = String.Format("{0} }}", str);
				#region Debug
				if (Debug) {
					Console.Write("\n{0}", str);
					JsonToFile(str);
				}
				#endregion

				using (var content = new StringContent(str, System.Text.Encoding.Default, "application/json")) {
					using (var response = await Client.PostAsync(BASE_URL, content)) {
						string responseData = await response.Content.ReadAsStringAsync();
						Thread.Sleep(1000);
						#region Debug
						if (Debug) {
							Console.Write("\n{0}", responseData);
							XmlToFile(responseData);
						}
						#endregion
					}
				}
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in HelcimClient.Purchase(string customerCode='{3}', decimal amount='{4}', string comments='{5}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, customerCode, amount, comments);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of HelcimClient.Purchase(string customerCode='{3}', decimal amount='{4}', string comments='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, customerCode, amount, comments);

				Console.Write("\n{0}", log);
				XmlToFile(log);
				throw new Exception(log);
				#endregion Log
			}
		}

		/// <summary>
		/// This API call lets you do a connection test. The response is an XML of the result of the connection test.
		/// </summary>
		/// <returns></returns>
		public async Task TestConnection()
		{
			try {
				var str = GetAuthenticationString();
				str = String.Format("{0}\"action\": \"connectionTest\" ", str);
				str = String.Format("{0} }}", str);
				#region Debug
				if (Debug) {
					Console.Write("\n{0}", str);
					JsonToFile(str);
				}
				#endregion

				using (var content = new StringContent(str, System.Text.Encoding.Default, "application/json")) {
					using (var response = await Client.PostAsync(BASE_URL, content)) {
						string responseData = await response.Content.ReadAsStringAsync();
						#region Debug
						if (Debug) {
							Console.Write("\n{0}", responseData);
							XmlToFile(responseData);
						}
						#endregion
					}
				}
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in HelcimClient.TestConnection().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of HelcimClient.TestConnection().{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);

				Console.Write("\n{0}", log);
				XmlToFile(log);
				throw new Exception(log);
				#endregion Log
			}
		}
		#endregion
	}
}