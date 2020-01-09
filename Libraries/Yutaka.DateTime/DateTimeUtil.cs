using System;

namespace Yutaka
{
	public static class DateTimeUtil
	{
		public static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		#region Methods
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
		/// <param name="dt">The date and time to convert.</param>
		/// <param name="sourceTimeZone">The time zone of dateTime.</param>
		/// <returns>A DateTime value that represents time that corresponds to the dateTime parameter in customer time zone.</returns>
		public static DateTime ConvertToLocalTime(DateTime dt, TimeZoneInfo sourceTimeZone)
		{
			return ConvertToLocalTime(dt, sourceTimeZone, TimeZoneInfo.Local);
		}

		/// <summary>
		/// Converts the date and time to current local date and time
		/// </summary>
		/// <param name="dt">The date and time to convert.</param>
		/// <param name="sourceTimeZone">The time zone of dateTime.</param>
		/// <param name="destinationTimeZone">The time zone to convert dateTime to.</param>
		/// <returns>A DateTime value that represents time that corresponds to the dateTime parameter in customer time zone.</returns>
		public static DateTime ConvertToLocalTime(DateTime dt, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
		{
			return TimeZoneInfo.ConvertTime(dt, sourceTimeZone, destinationTimeZone);
		}

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
		/// <param name="dt">The date and time to convert.</param>
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
		#endregion Methods
	}
}