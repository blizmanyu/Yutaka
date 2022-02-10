using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Yutaka.Core.Net;
using Yutaka.IO;

namespace Yutaka.Helcim
{
	public class HelcimClient : BaseClient
	{
		#region Fields
		public const string BASE_URL = "https://secure.myhelcim.com/api/";
		public bool Debug;
		public string ApiToken;
		public uint AccountId;
		#endregion Fields

		#region Constructor
		public HelcimClient(uint accountId, string apiToken)
		{
			if (String.IsNullOrWhiteSpace(apiToken))
				throw new Exception(String.Format("'apiToken' is required.{0}Exception thrown in Constructor HelcimClient(uint accountId, string apiToken).{0}", Environment.NewLine));

			Debug = false;
			AccountId = accountId;
			ApiToken = apiToken;
		}
		#endregion Constructor

		#region Utilities
		private string GetAuthenticationString()
		{
			var str = "{ ";
			str = String.Format("{0}\"accountId\": {1}, ", str, AccountId);
			str = String.Format("{0}\"apiToken\": \"{1}\", ", str, ApiToken);
			return str;
		}

		#region Writes
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

		public void WriteToFile(string response, bool pretty = true)
		{
			if (response == null || String.IsNullOrWhiteSpace(response))
				return;

			var filename = String.Format("{0}.json", DateTime.Now.ToString("yyyy MMdd HHmm ssff"));
			var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "JSON");
			Directory.CreateDirectory(folder);

			if (pretty) {
				dynamic parsedJson = JsonConvert.DeserializeObject(response);
				var formatted = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
				new FileUtil().Write(formatted, Path.Combine(folder, filename));
			}

			else
				new FileUtil().Write(response, Path.Combine(folder, filename));
		}

		public void WriteToFile(Task<string> response, bool pretty = true)
		{
			if (response == null || String.IsNullOrWhiteSpace(response.Result))
				return;

			var filename = String.Format("{0}.json", DateTime.Now.ToString("yyyy MMdd HHmm ssff"));
			var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "JSON");
			Directory.CreateDirectory(folder);

			if (pretty) {
				dynamic parsedJson = JsonConvert.DeserializeObject(response.Result);
				var formatted = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
				new FileUtil().Write(formatted, Path.Combine(folder, filename));
			}

			else
				new FileUtil().Write(response.Result, Path.Combine(folder, filename));
		}
		#endregion Writes
		#endregion Utilities

		#region Methods
		public async Task TestConnection()
		{
			try {
				var str = GetAuthenticationString();
				str = String.Format("{0}\"action\": \"connectionTest\" ", str);
				str = String.Format("{0} }}", str);
				#region Debug
				if (Debug)
					Console.Write("\n{0}", str);
				#endregion

				using (var content = new StringContent(str, System.Text.Encoding.Default, "application/json")) {
					using (var response = await Client.PostAsync(BASE_URL, content)) {
						string responseData = await response.Content.ReadAsStringAsync();
						#region Debug
						if (Debug)
							Console.Write("\n{0}", responseData);
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
				throw new Exception(log);
				#endregion Log
			}
		}
		#endregion Methods
	}
}