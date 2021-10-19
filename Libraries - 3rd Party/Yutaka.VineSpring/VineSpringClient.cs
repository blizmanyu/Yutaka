using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Yutaka.IO;
using Yutaka.VineSpring.Domain.Club;

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
		#region Clubs
		private async Task<string> ListAllClubs()
		{
			try {
				var endpoint = "clubs";
				Client.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", ApiKey);

				using (var response = await Client.GetAsync(endpoint)) {
					return await response.Content.ReadAsStringAsync();
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

		private async Task<string> ListAllClubMemberships(string clubId)
		{
			#region Check Input
			var log = "";

			if (String.IsNullOrWhiteSpace(clubId))
				log = String.Format("{0}'clubId' is required. Exception thrown in VineSpringClient.ListAllClubMemberships(string clubId).{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				Console.Write("\n{0}", log);
				throw new Exception(log);
			}
			#endregion Check Input

			try {
				var endpoint = String.Format("clubs/{0}/clubMemberships", clubId);
				Client.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", ApiKey);

				using (var response = await Client.GetAsync(endpoint)) {
					return await response.Content.ReadAsStringAsync();
				}
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in VineSpringClient.ListAllClubMemberships(string clubId='{3}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, clubId);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VineSpringClient.ListAllClubMemberships(string clubId='{3}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, clubId);

				Console.Write("\n{0}", log);
				throw new Exception(log);
				#endregion Log
			}
		}
		#endregion Clubs

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
		public IList<Club> GetAllClubs()
		{
			try {
				var list = new List<Club>();
				var response = ListAllClubs();
				var clubs = JsonConvert.DeserializeObject<List<Club>>(response.Result);

				foreach (var club in clubs)
					list.Add(club);

				return list;
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in VineSpringClient.GetAllClubs().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VineSpringClient.GetAllClubs().{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);

				Console.Write("\n{0}", log);
				throw new Exception(log);
				#endregion Log
			}
		}
		#endregion Clubs
		#endregion Methods
	}
}