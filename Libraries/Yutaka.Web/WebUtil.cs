using System.Web;
using Yutaka.Utils;

namespace Yutaka.Web
{
	public static class WebUtil
    {
		#region Methods
		public static void SetSessionVariables()
		{
			var Request = HttpContext.Current.Request;
			var uniqueId = Base36.UniqueID() + Base36.UniqueID(Request.UserHostAddress);
			var url = Request.Url;
			var referer = Request.UrlReferrer;
			var Session = HttpContext.Current.Session;

			if (Session["SessionId"] == null)
				Session["SessionId"] = uniqueId;
			if (Session["Orig_Referer"] == null)
				Session["Orig_Referer"] = referer == null ? "" : referer.AbsoluteUri;

			Session["Url"] = url == null ? "" : url.AbsoluteUri;
			Session["Referer"] = referer == null ? "" : referer.AbsoluteUri;
		}
		#endregion
	}
}