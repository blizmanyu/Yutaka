using System;
using System.Net.Http;

namespace Yutaka.Core.Net
{
	public class VineSpringClient : IDisposable
	{
		private static object _locker = new object();
		private static volatile HttpClient _client;
		public static readonly Uri BaseAddress = new Uri("https://api.vinespring.com/");

		protected static HttpClient Client
		{
			get {
				if (_client == null) {
					lock (_locker) {
						if (_client == null) {
							_client = new HttpClient { BaseAddress = BaseAddress };
							_client.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
						}
					}
				}

				return _client;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual void Dispose(bool disposing)
		{
			if (disposing) {
				if (_client != null)
					_client.Dispose();

				_client = null;
			}
		}
	}
}