using System;
using System.Net;

namespace Yutaka.Net
{
	class MyClient : WebClient
	{
		public bool HeadOnly { get; set; }

		protected override WebRequest GetWebRequest(Uri address)
		{
			var req = base.GetWebRequest(address);

			if (HeadOnly && req.Method == "GET")
				req.Method = "HEAD";

			return req;
		}
	}
}