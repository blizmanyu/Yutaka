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