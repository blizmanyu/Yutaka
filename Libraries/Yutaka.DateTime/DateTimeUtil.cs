using System;

namespace Yutaka
{
	public static class DateTimeUtil
	{
		#region Fields
		private static readonly char[] separator = new char[] { '/' };
		public static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		public static readonly DateTime UNIX_EPOCH_LOCAL = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
		public static readonly DateTime UNIX_EPOCH_UNSPECIFIED = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
		#endregion Fields

		#region Methods
		/// <summary>
		/// Calculates a person's age. Optionally specify an &lt;endDate&gt; if you don't want to calculate based on <see cref="DateTime.Today"/>. This assumes you are
		/// looking for the Western idea of age and not using East Asian reckoning.
		/// </summary>
		/// <param name="birthdate">The peron's birthdate.</param>
		/// <param name="endDate">The date to base the calculation on. If null, it will default to <see cref="DateTime.Today"/>.</param>
		/// <returns>The person's age.</returns>
		/// <seealso cref="https://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c"/>
		public static int CalculateAge(DateTime birthdate, DateTime? endDate = null)
		{
			if (endDate == null)
				endDate = DateTime.Today;

			var age = endDate.Value.Year - birthdate.Year;
			// Go back to the year the person was born in case of a leap year
			if (birthdate.Date > endDate.Value.AddYears(-age))
				age--;

			return age;
		}

		#region ConvertToLocalTime() Overloads
		/// <summary>
		/// Converts the date and time to current local date and time
		/// </summary>
		/// <param name="dt">The date and time (respesents local system time or UTC time) to convert.</param>
		/// <returns>A DateTime value that represents time that corresponds to the dateTime parameter in customer time zone.</returns>
		public static DateTime ConvertToLocalTime(DateTime dt)
		{
			return ConvertToLocalTime(dt, dt.Kind);
		}

		/// <summary>
		/// Converts the date and time to current local date and time
		/// </summary>
		/// <param name="dt">The date and time (respesents local system time or UTC time) to convert.</param>
		/// <param name="sourceDateTimeKind">The source datetimekind</param>
		/// <returns>A DateTime value that represents time that corresponds to the dateTime parameter in customer time zone.</returns>
		public static DateTime ConvertToLocalTime(DateTime dt, DateTimeKind sourceDateTimeKind)
		{
			dt = DateTime.SpecifyKind(dt, sourceDateTimeKind);
			return TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.Local);
		}

		/// <summary>
		/// Converts the date and time to current local date and time
		/// </summary>
		/// <param name="dt">The dateTime to convert.</param>
		/// <param name="sourceTimeZone">The time zone of dateTime.</param>
		/// <returns>A DateTime value that represents time that corresponds to the dateTime parameter in customer time zone.</returns>
		public static DateTime ConvertToLocalTime(DateTime dt, TimeZoneInfo sourceTimeZone)
		{
			return ConvertToLocalTime(dt, sourceTimeZone, TimeZoneInfo.Local);
		}

		/// <summary>
		/// Converts the date and time to current local date and time
		/// </summary>
		/// <param name="dt">The dateTime to convert.</param>
		/// <param name="sourceTimeZone">The time zone of dateTime.</param>
		/// <param name="destinationTimeZone">The time zone to convert dateTime to.</param>
		/// <returns>A DateTime value that represents time that corresponds to the dateTime parameter in customer time zone.</returns>
		public static DateTime ConvertToLocalTime(DateTime dt, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
		{
			return TimeZoneInfo.ConvertTime(dt, sourceTimeZone, destinationTimeZone);
		}

		public static DateTime ConvertToLocalTime(long googleInternalDate)
		{
			return UNIX_EPOCH.AddMilliseconds(googleInternalDate).ToLocalTime();
		}
		#endregion ConvertToLocalTime()

		#region ConvertToUtcTime() Overloads
		/// <summary>
		/// Converts the date and time to Coordinated Universal Time (UTC)
		/// </summary>
		/// <param name="dt">The date and time (respesents local system time or UTC time) to convert.</param>
		/// <returns>A DateTime value that represents the Coordinated Universal Time (UTC) that corresponds to the dateTime parameter. The DateTime value's Kind property is always set to DateTimeKind.Utc.</returns>
		public static DateTime ConvertToUtcTime(DateTime dt)
		{
			return ConvertToUtcTime(dt, dt.Kind);
		}

		/// <summary>
		/// Converts the date and time to Coordinated Universal Time (UTC)
		/// </summary>
		/// <param name="dt">The date and time (respesents local system time or UTC time) to convert.</param>
		/// <param name="sourceDateTimeKind">The source datetimekind</param>
		/// <returns>A DateTime value that represents the Coordinated Universal Time (UTC) that corresponds to the dateTime parameter. The DateTime value's Kind property is always set to DateTimeKind.Utc.</returns>
		public static DateTime ConvertToUtcTime(DateTime dt, DateTimeKind sourceDateTimeKind)
		{
			dt = DateTime.SpecifyKind(dt, sourceDateTimeKind);
			return TimeZoneInfo.ConvertTimeToUtc(dt);
		}

		/// <summary>
		/// Converts the date and time to Coordinated Universal Time (UTC)
		/// </summary>
		/// <param name="dt">The dateTime to convert.</param>
		/// <param name="sourceTimeZone">The time zone of dateTime.</param>
		/// <returns>A DateTime value that represents the Coordinated Universal Time (UTC) that corresponds to the dateTime parameter. The DateTime value's Kind property is always set to DateTimeKind.Utc.</returns>
		public static DateTime ConvertToUtcTime(DateTime dt, TimeZoneInfo sourceTimeZone)
		{
			if (sourceTimeZone.IsInvalidTime(dt)) {
				//could not convert
				return dt;
			}

			return TimeZoneInfo.ConvertTimeToUtc(dt, sourceTimeZone);
		}

		public static DateTime ConvertToUtcTime(long googleInternalDate)
		{
			return UNIX_EPOCH.AddMilliseconds(googleInternalDate);
		}
		#endregion ConvertToUtcTime()

		#region ConvertToRelativeTimeString() Overloads
		/// <summary>
		/// Converts the date and time to current local date and time
		/// </summary>
		/// <param name="dt">The date and time (respesents local system time or UTC time) to convert.</param>
		/// <returns>A string that represents time that corresponds to the dateTime parameter in local time zone.</returns>
		public static string ConvertToRelativeTimeString(DateTime dt)
		{
			return ConvertToRelativeTimeString(dt, dt.Kind);
		}

		/// <summary>
		/// Converts the date and time to current local date and time
		/// </summary>
		/// <param name="dt">The date and time (respesents local system time or UTC time) to convert.</param>
		/// <param name="sourceDateTimeKind">The source datetimekind</param>
		/// <returns>A string that represents time that corresponds to the dateTime parameter in local time zone.</returns>
		public static string ConvertToRelativeTimeString(DateTime dt, DateTimeKind sourceDateTimeKind)
		{
			dt = ConvertToLocalTime(dt, sourceDateTimeKind);
			var time = dt.ToString("h:mmtt").ToLower();

			if (dt.Date == DateTime.Today)
				return String.Format("Today, {0}", time);

			if (dt.Year == DateTime.Now.Year) // This Year //
				return String.Format("{0:MMM d}, {1}", dt, time);

			return String.Format("{0:M/d/yy}, {1}", dt, time);
		}

		/// <summary>
		/// Converts the date and time to current local date and time
		/// </summary>
		/// <param name="dt">The dateTime to convert.</param>
		/// <param name="sourceTimeZone">The time zone of dateTime.</param>
		/// <returns>A string that represents time that corresponds to the dateTime parameter in local time zone.</returns>
		public static string ConvertToRelativeTimeString(DateTime dt, TimeZoneInfo sourceTimeZone)
		{
			return ConvertToRelativeTimeString(dt, sourceTimeZone, TimeZoneInfo.Local);
		}

		/// <summary>
		/// Converts the date and time to current local date and time
		/// </summary>
		/// <param name="dt">The dateTime to convert.</param>
		/// <param name="sourceTimeZone">The time zone of dateTime.</param>
		/// <param name="destinationTimeZone">The time zone to convert dateTime to.</param>
		/// <returns>A string that represents time that corresponds to the dateTime parameter in local time zone.</returns>
		public static string ConvertToRelativeTimeString(DateTime dt, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
		{
			dt = ConvertToLocalTime(dt, sourceTimeZone, destinationTimeZone);
			var time = dt.ToString("h:mmtt").ToLower();
			var todayAtDestination = ConvertToLocalTime(DateTime.Today, TimeZoneInfo.Local, destinationTimeZone);
			var thisYearAtDestination = todayAtDestination.Year;

			if (dt.Date == todayAtDestination)
				return String.Format("Today, {0}", time);

			if (dt.Year == thisYearAtDestination) // This Year //
				return String.Format("{0:MMM d}, {1}", dt, time);

			return String.Format("{0:M/d/yy}, {1}", dt, time);
		}
		#endregion ConvertToRelativeTimeString()

		public static DateTime GetBeginningOfWeek()
		{
			var today = DateTime.Today;
			return today.AddDays(DayOfWeek.Sunday - today.DayOfWeek);
		}

		public static DateTime GetEndOfWeek()
		{
			var today = DateTime.Today;
			return today.AddDays(DayOfWeek.Saturday - today.DayOfWeek);
		}

		public static DateTime GetBeginningOfMonth()
		{
			var today = DateTime.Today;
			return new DateTime(today.Year, today.Month, 1);
		}

		public static DateTime GetEndOfMonth()
		{
			var today = DateTime.Today;
			return new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);
		}

		public static DateTime GetBeginningOfQuarter()
		{
			var today = DateTime.Today;
			var quarterNumber = (today.Month - 1) / 3 + 1;
			return new DateTime(today.Year, (quarterNumber - 1) * 3 + 1, 1);
		}

		public static DateTime GetEndOfQuarter()
		{
			var today = DateTime.Today;
			var quarterNumber = (today.Month - 1) / 3 + 1;
			return new DateTime(today.Year, (quarterNumber - 1) * 3 + 1, 1).AddMonths(3).AddDays(-1);
		}

		public static DateTime GetBeginningOfYear()
		{
			return new DateTime(DateTime.Today.Year, 1, 1);
		}

		public static DateTime GetEndOfYear()
		{
			return new DateTime(DateTime.Today.Year, 12, 31);
		}

		#region Google Time
		public static long LocalTimeToGoogleInternalDate(DateTime dt)
		{
			return (long) (dt.ToUniversalTime() - UNIX_EPOCH).TotalMilliseconds;
		}

		public static long UtcToGoogleInternalDate(DateTime dt)
		{
			return (long) (dt - UNIX_EPOCH).TotalMilliseconds;
		}
		#endregion Google Time

		/// <summary>
		/// A more complex TryParse method for US-based date formats. Will detect M, MM, d, dd, yy, and yyyy separated by '/' that is used in the US.
		/// </summary>
		/// <param name="date">The date as a string.</param>
		/// <param name="result">The resulting DateTime if successful. Null if unsuccessful.</param>
		/// <returns>True if succeeded. False otherwise.</returns>
		public static bool TryParse(string date, out DateTime? result)
		{
			result = null;

			if (String.IsNullOrWhiteSpace(date))
				return false;
			else {
				if (DateTime.TryParse(date, out var temp)) {
					result = temp;
					return true;
				}

				else {
					if (date.Contains("/")) {
						var split = date.Split(separator, StringSplitOptions.RemoveEmptyEntries);

						if (split == null || split.Length != 3)
							return false;
						else {
							// Check month //
							if (String.IsNullOrWhiteSpace(split[0]))
								return false;
							if (int.TryParse(split[0], out var month)) {
								if (0 < month && month < 13) {
									// Check day //
									if (String.IsNullOrWhiteSpace(split[1]))
										return false;
									if (int.TryParse(split[1], out var day)) {
										if (0 < day && day < 32) {
											// Check year //
											if (String.IsNullOrWhiteSpace(split[2]))
												return false;
											if (int.TryParse(split[2], out var year)) {
												if (-1 < year && year < 100) {
													result = new DateTime(year + 2000, month, day);
													return true;
												}

												if (1900 < year && year < 10000) {
													result = new DateTime(year, month, day);
													return true;
												}

												return false;
											}
										}

										return false;
									}
								}

								return false;
							}
						}
					}

					return false;
				}
			}
		}
		#endregion Methods
	}
}