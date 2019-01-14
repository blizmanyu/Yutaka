using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.Utils
{
	public static class Util
	{
		private static DateTime unixTime = new DateTime(1970, 1, 1);

		public static string GetRelativeDateTimeString(DateTime dt)
		{
			if (dt == null || dt < unixTime)
				throw new Exception(String.Format("<dt> is required.{0}{0}Exception thrown in Util.GetRelativeDateTimeString(DateTime dt)", Environment.NewLine));

			try {
				if (dt.Date == DateTime.Now.Date) // Today //
					return String.Format("Today, {0}", dt.ToString("h:mmtt").ToLower());

				if (dt.Year == DateTime.Now.Year) // This Year //
					return String.Format("{0:MMM d}, {1}", dt, dt.ToString("h:mmtt").ToLower());

				return String.Format("{0:M/d/yy}, {1}", dt, dt.ToString("h:mmtt").ToLower());
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in Util.GetRelativeDateTimeString(DateTime dt='{3}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, dt));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of Util.GetRelativeDateTimeString(DateTime dt='{3}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, dt));
			}
		}
	}
}