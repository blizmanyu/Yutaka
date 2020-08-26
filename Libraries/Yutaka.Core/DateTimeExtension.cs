using System;

namespace Yutaka
{
	public static class DateTimeExtension
	{
		public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Sunday)
		{
			var diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
			return dt.AddDays(-diff).Date;
		}

		public static DateTime EndOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Sunday)
		{
			return StartOfWeek(dt, startOfWeek).AddDays(6);
		}

		public static DateTime StartOfYear(this DateTime dt)
		{
			return new DateTime(dt.Year, 1, 1);
		}

		public static DateTime EndOfYear(this DateTime dt)
		{
			return new DateTime(dt.Year, 12, 31);
		}
	}
}