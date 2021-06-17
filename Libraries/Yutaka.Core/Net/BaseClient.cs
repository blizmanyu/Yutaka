using System;
using System.Net.Http;

namespace Yutaka.Core.Net
{
	public abstract class BaseClient : IDisposable
	{
		private static object _locker = new object();
		private static volatile HttpClient _client;

		protected static HttpClient Client
		{
			get {
				if (_client == null) {
					lock (_locker) {
						if (_client == null)
							_client = new HttpClient();
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