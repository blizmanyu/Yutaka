using System;

namespace Yutaka.Utils
{
	[Obsolete("Deprecated Jan 9, 2020: Use Yutaka.DateTime.DateTimeUtil instead.", false)]
	public static class Util
	{
		private static readonly DateTime dateTimeThreshold = new DateTime(1900, 1, 1);
		private static readonly DateTime unixTime = new DateTime(1970, 1, 1);

		[Obsolete("Deprecated Jan 9, 2020: Use Yutaka.DateTime.DateTimeUtil.ConvertToRelativeTimeString(DateTime dt) instead.", false)]
		public static string GetRelativeDateTimeString(DateTime dt)
		{
			if (dt == null || dt < dateTimeThreshold)
				throw new Exception(String.Format("<dt> is required.{0}{0}Exception thrown in Util.GetRelativeDateTimeString(DateTime dt)", Environment.NewLine));

			try {
				if (dt.Date == DateTime.Now.Date) // Today //
					return String.Format("Today, {0}", dt.ToString("h:mmtt").ToLower());

				if (dt.Year == DateTime.Now.Year) // This Year //
					return String.Format("{0:MMM d}, {1}", dt, dt.ToString("h:mmtt").ToLower());

				return dt.ToString("M/d/yy, h:mmtt").ToLower();
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in Util.GetRelativeDateTimeString(DateTime dt='{3}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, dt));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of Util.GetRelativeDateTimeString(DateTime dt='{3}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, dt));
			}
		}

		[Obsolete("Deprecated Jan 9, 2020: Use Yutaka.DateTime.DateTimeUtil.ConvertToLocalTime(long googleInternalDate) instead.", false)]
		public static DateTime GoogleInternalDateToUtc(long internalDate)
		{
			if (internalDate < 0)
				throw new Exception(String.Format("<internalDate> is required.{0}{0}Exception thrown in Util.GoogleInternalDateToUtc(long internalDate)", Environment.NewLine));

			try {
				return unixTime.AddMilliseconds(internalDate);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in Util.GoogleInternalDateToUtc(long internalDate='{3}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, internalDate));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of Util.GoogleInternalDateToUtc(long internalDate='{3}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, internalDate));
			}
		}

		[Obsolete("Deprecated Jan 9, 2020: Use Yutaka.DateTime.DateTimeUtil.ConvertToUtcTime(long googleInternalDate) instead.", false)]
		public static DateTime GoogleInternalDateToLocalTime(long internalDate)
		{
			if (internalDate < 0)
				throw new Exception(String.Format("<internalDate> is required.{0}{0}Exception thrown in Util.GoogleInternalDateToLocalTime(long internalDate)", Environment.NewLine));

			try {
				return unixTime.AddMilliseconds(internalDate).ToLocalTime();
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in Util.GoogleInternalDateToLocalTime(long internalDate='{3}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, internalDate));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of Util.GoogleInternalDateToLocalTime(long internalDate='{3}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, internalDate));
			}
		}

		[Obsolete("Deprecated Jan 9, 2020: Use Yutaka.DateTime.DateTimeUtil.LocalTimeToGoogleInternalDate(DateTime dt) instead.", false)]
		public static long LocalTimeToGoogleInternalDate(DateTime dt)
		{
			if (dt == null || dt < dateTimeThreshold)
				throw new Exception(String.Format("<dt> is required.{0}{0}Exception thrown in Util.LocalTimeToGoogleInternalDate(DateTime dt)", Environment.NewLine));

			try {
				return (long) (dt.ToUniversalTime() - unixTime).TotalMilliseconds;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in Util.LocalTimeToGoogleInternalDate(DateTime dt='{3}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, dt));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of Util.LocalTimeToGoogleInternalDate(DateTime dt='{3}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, dt));
			}
		}

		[Obsolete("Deprecated Jan 9, 2020: Use Yutaka.DateTime.DateTimeUtil.UtcToGoogleInternalDate(DateTime dt) instead.", false)]
		public static long UtcToGoogleInternalDate(DateTime dt)
		{
			if (dt == null || dt < dateTimeThreshold)
				throw new Exception(String.Format("<dt> is required.{0}{0}Exception thrown in Util.UtcToGoogleInternalDate(DateTime dt)", Environment.NewLine));

			try {
				return (long) (dt - unixTime).TotalMilliseconds;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in Util.UtcToGoogleInternalDate(DateTime dt='{3}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, dt));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of Util.UtcToGoogleInternalDate(DateTime dt='{3}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, dt));
			}
		}
	}
}