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
			// Ordering is important here. Don't re-order without realizing the repercussions //
			if (Now < April1.AddDays(-14))
				return April1;
			if (Now < May1.AddDays(-14))
				return May1;
			if (Now < October1.AddDays(-14))
				return October1;

			return April1.AddYears(1);
		}
	}
}