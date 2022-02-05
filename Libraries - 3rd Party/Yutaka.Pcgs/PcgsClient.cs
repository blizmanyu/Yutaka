using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Yutaka.IO;

namespace Yutaka.Pcgs
{
	public class PcgsClient : BaseClient
	{
		#region Fields
		public string ApiToken;
		#endregion

		#region Constructor
		public PcgsClient(string apiToken = null)
		{
			if (String.IsNullOrWhiteSpace(apiToken))
				throw new Exception(String.Format("'apiToken' is required.{0}Exception thrown in Constructor PcgsClient(string apiToken = null).{0}", Environment.NewLine));

			ApiToken = apiToken;
		}
		#endregion

		#region Utilities
		public async Task<string> GetCoinFactsByCertNo(string certNo)
		{
			#region Check Input
			var log = "";

			if (String.IsNullOrWhiteSpace(certNo))
				log = String.Format("{0}'certNo' is required.{1}Exception thrown in PcgsClient.GetCoinFactsByCertNo(string certNo).{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				Console.Write("\n{0}", log);
				throw new Exception(log);
			}
			#endregion Check Input

			try {
				var endpoint = String.Format("GetCoinFactsByCertNo/{0}", certNo);
				Client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", ApiToken);

				using (var response = await Client.GetAsync(endpoint)) {
					return await response.Content.ReadAsStringAsync();
				}
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in PcgsClient.GetCoinFactsByCertNo().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of PcgsClient.GetCoinFactsByCertNo().{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);

				Console.Write("\n{0}", log);
				throw new Exception(log);
				#endregion Log
			}
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
		#endregion

		#region Methods

		#endregion
	}
}