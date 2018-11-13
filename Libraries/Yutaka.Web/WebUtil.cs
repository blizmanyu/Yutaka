using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using Yutaka.Utils;

namespace Yutaka.Web
{
	public static class WebUtil
	{
		// Fields //
		private static List<string> bots = new List<string> { "bot","crawler","spider","80legs","baidu","yahoo! slurp","ia_archiver","mediapartners-google","lwp-trivial","nederland.zoek","ahoy","anthill","appie","arale","araneo","http",".com","ariadne","atn_worldwide","atomz","bjaaland","ukonline","calif","combine","cosmos","cusco","cyberspyder","digger","grabber","downloadexpress","ecollector","ebiness","esculapio","esther","felix ide","hamahakki","kit-fireball","fouineur","freecrawl","desertrealm","gcreep","golem","griffon","gromit","gulliver","gulper","whowhere","havindex","hotwired","htdig","ingrid","informant","inspectorwww","iron33","teoma","ask jeeves","jeeves","image.kapsi.net","kdd-explorer","label-grabber","larbin","linkidator","linkwalker","lockon","marvin","mattie","mediafox","merzscope","nec-meshexplorer","udmsearch","moget","motor","muncher","muninn","muscatferret","mwdsearch","sharp-info-agent","webmechanic","netscoop","newscan-online","objectssearch","orbsearch","packrat","pageboy","parasite","patric","pegasus","phpdig","piltdownman","pimptrain","plumtreewebaccessor","getterrobo-plus","raven","roadrunner","robbie","robocrawl","robofox","webbandit","scooter","search-au","searchprocess","senrigan","shagseeker","site valet","skymob","slurp","snooper","speedy","curl_image_client","suke","www.sygol.com","tach_bw","templeton","titin","topiclink","udmsearch","urlck","valkyrie libwww-perl","verticrawl","victoria","webscout","voyager","crawlpaper","webcatcher","t-h-u-n-d-e-r-s-t-o-n-e","webmoose","pagesinventory","webquest","webreaper","webwalker","winona","occam","robi","fdse","jobo","rhcs","gazz","dwcp","yeti","fido","wlm","wolp","wwwc","xget","legs","curl","webs","wget","sift","cmc" };
		// Updated Aug 1, 2014 // Retrieved Apr 5, 2018 //
		private static Regex MobileCheck = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
		private static Regex MobileVersionCheck = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

		// Methods //
		public static bool IsBot()
		{
			try {
				var Request = HttpContext.Current.Request;

				if (Request.Browser.Crawler)
					return true;

				var userAgent = (Request.UserAgent ?? "").ToLower();

				if (userAgent.Length < 20)
					return true;
				if (bots.Exists(b => userAgent.Contains(b)))
					return true;

				return false;
			}

			catch (Exception ex) {
				throw new Exception(String.Format("Exception thrown in WebUtil.IsBot(){2}{0}{2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine));
			}
		}

		public static bool IsMobileDevice()
		{
			var Request = HttpContext.Current.Request;

			if (Request != null && Request.ServerVariables["HTTP_USER_AGENT"] != null) {
				var userAgent = Request.ServerVariables["HTTP_USER_AGENT"].ToString();

				if (userAgent.Length < 4)
					return false;

				if (MobileCheck.IsMatch(userAgent) || MobileVersionCheck.IsMatch(userAgent.Substring(0, 4)))
					return true;
			}

			return false;
		}

		public static bool IsMobileDevice(string userAgent)
		{
			if (String.IsNullOrWhiteSpace(userAgent) || userAgent.Length < 4)
				return false;

			if (MobileCheck.IsMatch(userAgent) || MobileVersionCheck.IsMatch(userAgent.Substring(0, 4)))
				return true;

			return false;
		}

		public static void SetSessionVariables()
		{
			try {
				var Request = HttpContext.Current.Request;
				var Session = HttpContext.Current.Session;

				if (Session["Id"] == null || String.IsNullOrWhiteSpace(Session["Id"].ToString()))
					Session["Id"] = String.Format("{0}-{1}", Base36.GetUniqueId(), Base36.GetUniqueIdByIP(Request.UserHostAddress));

				if (Session["Source"] == null)
					Session["Source"] = Request.UrlReferrer == null ? "" : Request.UrlReferrer.AbsoluteUri ?? "";

				if (Session["IsMobileDevice"] == null || String.IsNullOrWhiteSpace(Session["IsMobileDevice"].ToString()))
					Session["IsMobileDevice"] = IsMobileDevice(Request.UserAgent ?? "");

				if (Session["Url"] == null || String.IsNullOrWhiteSpace(Session["Url"].ToString())) {
					Session["Url"] = Request.Url == null ? "" : Request.Url.AbsoluteUri ?? "";
					Session["Referer"] = Request.UrlReferrer == null ? "" : Request.UrlReferrer.AbsoluteUri ?? "";
				}

				else {
					Session["Referer"] = Session["Url"].ToString();
					Session["Url"] = Request.Url == null ? "" : Request.Url.AbsoluteUri ?? "";
				}
			}

			catch (Exception ex) {
				throw new Exception(String.Format("Exception thrown in WebUtil.SetSessionVariables(){2}{0}{2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine));
			}
		}

		[Obsolete("Deprecated. Use IsBot() instead.")]
		public static bool IsBot(string userAgent)
		{
			if (String.IsNullOrWhiteSpace(userAgent) || userAgent.Length < 20)
				return true;

			try {
				userAgent = userAgent.ToLower();

				if (bots.Exists(b => userAgent.Contains(b)))
					return true;

				return false;
			}

			catch (Exception ex) {
				throw new Exception(String.Format("Exception thrown in WebUtil.IsBot(string userAgent='{3}'){2}{0}{2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine, userAgent));
			}
		}
	}
}