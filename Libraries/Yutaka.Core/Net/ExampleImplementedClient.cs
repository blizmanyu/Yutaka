using System.Threading.Tasks;

namespace Yutaka.Core.Net
{
	/// <summary>
	/// Example service client that implements BaseClient. Don't actually use this class in any production code.
	/// </summary>
	public class ExampleImplementedClient : BaseClient
	{
		public static readonly string endpoint = "https://www.example.com";

		public async Task<string> SendRequestToEndpoint()
		{
			return await Client.GetStringAsync(endpoint);
		}
	}
}