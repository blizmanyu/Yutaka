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

		/// <summary>
		/// Converts the value of the current <see cref="DateTime"/> object to its equivalent long datetime string representation.
		/// </summary>
		/// <param name="dt">The DateTime to convert.</param>
		/// <returns></returns>
		public static string ToRelativeDateTimeString(this DateTime dt)
		{
			if (dt.Year == DateTime.Today.Year)
				return String.Format("{0:MMM d}, {1}", dt, dt.ToShorterTimeString());

			return String.Format("{0:MMM d, yyyy}, {1}", dt, dt.ToShorterTimeString());
		}

		/// <summary>
		/// Converts the value of the current <see cref="DateTime"/> object to its equivalent short time string representation.
		/// </summary>
		/// <param name="dt">The DateTime to convert.</param>
		/// <returns></returns>
		public static string ToShorterTimeString(this DateTime dt)
		{
			return dt.ToString("h:mmtt").ToLower();
		}
	}
}