using System;
using System.Net;
using System.Net.Http;

namespace Yutaka.Helcim
{
	public abstract class BaseClient : IDisposable
	{
		protected const SecurityProtocolType TLS13 = (SecurityProtocolType) 12288;
		private static object _locker = new object();
		private static volatile HttpClient _client;

		protected static HttpClient Client
		{
			get {
				if (_client == null) {
					lock (_locker) {
						if (_client == null) {
							try {
								ServicePointManager.SecurityProtocol |= TLS13 | SecurityProtocolType.Tls12;
								ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls11;
								ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls;
								ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
							}

							catch (NotSupportedException) {
								ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
								ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls;
								ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
							}

							_client = new HttpClient();
							_client.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/xml");
							_client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/x-www-form-urlencoded");
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