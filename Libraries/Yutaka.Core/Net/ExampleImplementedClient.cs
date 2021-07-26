using System.Threading.Tasks;

namespace Yutaka.Core.Net
{
	/// <summary>
	/// Example service client that implements BaseClient. Don't actually use this class in any production code.
	/// </summary>
	public class ExampleImplementedClient : BaseClient
	{
		public static readonly string endpoint = "https://www.google.com";

		public async Task SendRequestToEndpoint()
		{
			for (var count = 0; count < 10; count++)
				await Client.GetStringAsync(endpoint);
		}
	}
}