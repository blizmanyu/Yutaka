using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Yutaka.Web
{
	public class YuSession
	{
		public DateTime CreatedOnUtc;
		public string Ip;
		public string UniqueId;
		protected HttpRequest Request;
		protected HttpSessionState Session;

		public YuSession()
		{
			CreatedOnUtc = DateTime.UtcNow;
			Ip = HttpContext.Current.Request.UserHostAddress ?? "";
		}
	}
}