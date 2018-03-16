using System.Web.Mvc;
using Yutaka.Utils;

namespace Yutaka.Web
{
    public class WebUtil : Controller
    {
		public WebUtil() { }

		#region Methods
		public void SetSessionVariables()
		{
			var uniqueId = Base36.UniqueID();
			var url = Request.Url;
			var referer = Request.UrlReferrer;

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