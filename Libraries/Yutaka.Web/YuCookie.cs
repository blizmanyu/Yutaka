using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Yutaka.Web
{
	/// <summary>
	/// WIP: Do not use yet.
	/// </summary>
	public class YuCookie
	{
		HttpRequest Request;
		HttpResponse Response;
		HttpSessionState Session;

		public YuCookie()
		{
			Session = HttpContext.Current.Session;
			Response = HttpContext.Current.Response;
			Request = HttpContext.Current.Request;
			var cookie = Request.Cookies.Get("YuCookie");

			// Check if cookie exists in the current request.
			if (cookie == null) {
				sb.Append("Cookie was not received from the client. ");
				sb.Append("Creating cookie to add to the response. <br/>");
				// Create cookie.
				cookie = new HttpCookie("DateCookieExample");
				// Set value of cookie to current date time.
				cookie.Value = DateTime.Now.ToString();
				// Set cookie to expire in 10 minutes.
				cookie.Expires = DateTime.Now.AddMinutes(10d);
				// Insert the cookie in the current HttpResponse.
				Response.Cookies.Add(cookie);
			}
			else {
				sb.Append("Cookie retrieved from client. <br/>");
				sb.Append("Cookie Name: " + cookie.Name + "<br/>");
				sb.Append("Cookie Value: " + cookie.Value + "<br/>");
				sb.Append("Cookie Expiration Date: " +
					cookie.Expires.ToString() + "<br/>");
			}
		}
	}
}
