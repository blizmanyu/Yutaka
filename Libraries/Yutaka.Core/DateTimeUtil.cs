using System;

namespace Yutaka.Core
{
	public partial class DateTimeUtil
	{
		#region Fields
		public static readonly DateTime Now = DateTime.Now;
		public static readonly DateTime April1 = new DateTime(Now.Year, 4, 1);
		public static readonly DateTime May1 = new DateTime(Now.Year, 5, 1);
		public static readonly DateTime October1 = new DateTime(Now.Year, 10, 1);
		#endregion

		/// <summary>
		/// Gets an Estimated Ship Date based on the current date.
		/// </summary>
		/// <returns></returns>
		public static DateTime GetShipDate()
		{
			return DateTime.Today.AddDays(30);
		}

		/// <summary>
		/// Converts the <see cref="DateTime"/> for Edit and Detail pages.
		/// </summary>
		/// <param name="dt">The date and time to convert.</param>
		/// <returns></returns>
		public static string ToEditPageTemplate(DateTime dt)
		{
			var today = DateTime.Today;
			var tt = dt.ToString("tt").ToLower();

			if (dt.Date == today)
				return String.Format("Today, {0:MMM d, yyyy, h:mm}{1}", dt, tt);

			if (dt.Date == today.AddDays(-1))
				return String.Format("Yesterday, {0:MMM d, yyyy, h:mm}{1}", dt, tt);

			return String.Format("{0:MMM d, yyyy, h:mm}{1}", dt, tt);
		}

		/// <summary>
		/// Converts the <see cref="DateTime"/> to the format that Gmail uses.
		/// </summary>
		/// <param name="dt">The date and time to convert.</param>
		/// <returns></returns>
		public static string ToGmailStyle(DateTime dt)
		{
			var today = DateTime.Today;

			if (dt.Date == today)
				return String.Format("{0:h:mm}{1}", dt, dt.ToString("tt").ToLower()); // 1:01pm

			if (dt.Year == today.Year)
				return String.Format("{0:MMM d}", dt); // Jan 1

			return String.Format("{0:M/d/yy}", dt); // 1/1/21
		}

		/// <summary>
		/// Converts the <see cref="DateTime"/> to the format that Gmail uses. This method includes &lt;span title&gt; for expanded hover text.
		/// </summary>
		/// <param name="dt">The date and time to convert.</param>
		/// <returns></returns>
		public static string ToGmailStyleTemplate(DateTime dt)
		{
			var today = DateTime.Today;
			var tt = dt.ToString("tt").ToLower();

			if (dt.Date == today)
				return String.Format("<span title='Today, {0:MMM d, yyyy, h:mm}{1}'>{0:h:mm}{1}</span>", dt, tt);

			if (dt.Date == today.AddDays(-1))
				return String.Format("<span title='Yesterday, {0:MMM d, yyyy, h:mm}{1}'>{0:MMM d}</span>", dt, tt);

			if (dt.Year == today.Year)
				return String.Format("<span title='{0:MMM d, yyyy, h:mm}{1}'>{0:MMM d}</span>", dt, tt);

			return String.Format("<span title='{0:MMM d, yyyy, h:mm}{1}'>{0:M/d/yy}</span>", dt, tt);
		}

		/// <summary>
		/// Converts the datetime to the long-form U.S. format. Example: "Jan 1, 2009, 1:01 PM".
		/// </summary>
		/// <param name="dt">The date and time to convert.</param>
		/// <returns>A string that represents date that corresponds to the dateTime parameter.</returns>
		public static string ToLongDateTimeString(DateTime dt)
		{
			return String.Format("{0:MMM d, yyyy, h:mm tt}", dt);
		}

		/// <summary>
		/// Converts the datetime to a relative date. If the date is today, it actually displays the time.
		/// </summary>
		/// <param name="dt">The date and time to convert.</param>
		/// <returns>A string that represents date that corresponds to the dateTime parameter.</returns>
		public static string ToRelativeDateString(DateTime dt)
		{
			var today = DateTime.Today;

			if (dt.Date == today)
				return String.Format("{0:h:mm tt}", dt); // 1:01 PM

			if (dt.Date == today.AddDays(-1))
				return "Yesterday";

			if (dt.Year == today.Year)
				return String.Format("{0:MMM d}", dt); // Jan 1

			return String.Format("{0:M/d/yy}", dt); // 1/1/21
		}

		/// <summary>
		/// Converts the datetime to a relative date and time.
		/// </summary>
		/// <param name="dt">The date and time to convert.</param>
		/// <returns>A string that represents time that corresponds to the dateTime parameter.</returns>
		public static string ToRelativeDateTimeString(DateTime dt)
		{
			var today = DateTime.Today;

			if (dt.Date == today)
				return String.Format("Today, {0:h:mm tt}", dt); // Today, 1:01 PM

			if (dt.Date == today.AddDays(-1))
				return String.Format("Yesterday, {0:h:mm tt}", dt); // Yesterday, 1:01 PM

			if (dt.Year == today.Year)
				return String.Format("{0:MMM d, h:mm tt}", dt); // Jan 1, 1:01 PM

			return String.Format("{0:M/d/yy, h:mm tt}", dt); // 1/1/21, 1:01 PM
		}

		/// <summary>
		/// Converts a <see cref="DateTime"/> to an HTML span snippet.
		/// </summary>
		/// <param name="dt">The date and time to convert.</param>
		/// <returns></returns>
		public static string ToTemplate(DateTime dt)
		{
			var today = DateTime.Today;

			if (dt.Date == today)
				return String.Format("<span title='Today, {0:MMM d, h:mm tt}'>{0:h:mm tt}</span>", dt);

			if (dt.Date == today.AddDays(-1))
				return String.Format("<span title='Yesterday, {0:MMM d, h:mm tt}'>{0:MMM d}</span>", dt);

			if (dt.Year == today.Year)
				return String.Format("<span title='{0:MMM d, h:mm tt}'>{0:MMM d}</span>", dt);

			return String.Format("<span title='{0:MMM d, yyyy h:mm tt}'>{0:M/d/yy}</span>", dt);
		}
	}
}