using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Yutaka.IO;
using Yutaka.VineSpring.Data;
using Yutaka.VineSpring.Data20191126;
using Yutaka.VineSpring.Data20200207;

namespace Yutaka.VineSpring
{
	/// <summary>
	/// A VineSpringClient class.
	/// </summary>
	public class VineSpringClient : BaseClient
	{
		#region Fields
		public const string TIME_FORMAT = @"yyyy-MM-ddT00:00:00.000Z";
		private static readonly DateTime DOB_THRESHOLD = DateTime.Now.AddYears(-100);
		public static readonly DateTime MIN_DATE = DateTime.Now.AddYears(-10);
		public static readonly DateTime MAX_DATE = DateTime.Now.AddYears(1);
		public string ApiKey;
		#endregion Fields

		#region Constructor
		public VineSpringClient(string apiKey = null)
		{
			if (String.IsNullOrWhiteSpace(apiKey))
				throw new Exception(String.Format("'apiKey' is required. Exception thrown in Constructor VineSpringClient(string apiKey = null).{0}", Environment.NewLine));

			ApiKey = apiKey;
		}
		#endregion Constructor

		#region Utilities
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

		#region Methods
		#region Clubs
		public async Task<string> ListAllClubs()
		{
			try {
				var endpoint = "clubs";
				Client.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", ApiKey);

				using (var response = await Client.GetAsync(endpoint)) {
					var responseData = await response.Content.ReadAsStringAsync();
					return responseData;
				}
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in VineSpringClient.ListAllClubs().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VineSpringClient.ListAllClubs().{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);

				Console.Write("\n{0}", log);
				throw new Exception(log);
				#endregion Log
			}
		}
		#endregion Clubs
		#endregion Methods
	}
}