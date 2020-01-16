// 
// Copyright (c) Yutaka Blizman
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Yutaka Blizman nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

namespace Yutaka
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Defines available date ranges.
	/// </summary>
	public sealed class DateRange : IComparable, IEquatable<DateRange>, IConvertible
	{
		/// <summary>
		/// ThisWeek date range.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Type is immutable")]
		public static readonly DateRange ThisWeek = new DateRange("This Week", 0);

		/// <summary>
		/// Last7Days date range.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Type is immutable")]
		public static readonly DateRange Last7Days = new DateRange("Last 7 Days", 1);

		/// <summary>
		/// ThisMonth date range.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Type is immutable")]
		public static readonly DateRange ThisMonth = new DateRange("This Month", 2);

		/// <summary>
		/// Last30Days date range.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Type is immutable")]
		public static readonly DateRange Last30Days = new DateRange("Last 30 Days", 3);

		/// <summary>
		/// ThisQuarter date range.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Type is immutable")]
		public static readonly DateRange ThisQuarter = new DateRange("This Quarter", 4);

		/// <summary>
		/// Last3Months date range.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Type is immutable")]
		public static readonly DateRange Last3Months = new DateRange("Last 3 Months", 5);

		/// <summary>
		/// ThisYear date range.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Type is immutable")]
		public static readonly DateRange ThisYear = new DateRange("This Year", 6);

		/// <summary>
		/// Last12Months date range.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Type is immutable")]
		public static readonly DateRange Last12Months = new DateRange("Last 12 Months", 7);

		/// <summary>
		/// AllTime date range.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Type is immutable")]
		public static readonly DateRange AllTime = new DateRange("All Time", 8);

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Type is immutable")]
		private static readonly IList<DateRange> allDateRanges = new List<DateRange> { ThisWeek, Last7Days, ThisMonth, Last30Days, ThisQuarter, Last3Months, ThisYear, Last12Months, AllTime }.AsReadOnly();

		/// <summary>
		/// Gets all the available date ranges (ThisWeek, Last7Days, ThisMonth, Last30Days, ThisQuarter, Last3Months, ThisYear, Last12Months, AllTime).
		/// </summary>
		public static IEnumerable<DateRange> AllDateRanges => allDateRanges;

		private readonly int _ordinal;
		private readonly string _name;

		/// <summary>
		/// Initializes a new instance of <see cref="DateRange"/>.
		/// </summary>
		/// <param name="name">The date range name.</param>
		/// <param name="ordinal">The date range ordinal number.</param>
		private DateRange(string name, int ordinal)
		{
			_name = name;
			_ordinal = ordinal;
		}

		/// <summary>
		/// Gets the name of the date range.
		/// </summary>
		public string Name => _name;

		internal static DateRange MaxDateRange => AllTime;
		internal static DateRange MinDateRange => ThisWeek;

		/// <summary>
		/// Gets the ordinal of the date range.
		/// </summary>
		public int Ordinal => _ordinal;

		/// <summary>
		/// Compares two <see cref="DateRange"/> objects and returns a value indicating whether the first one is equal to the second one.
		/// </summary>
		/// <param name="range1">The first range.</param>
		/// <param name="range2">The second range.</param>
		/// <returns>The value of <c>range1.Ordinal == range2.Ordinal</c>.</returns>
		public static bool operator ==(DateRange range1, DateRange range2)
		{
			if (ReferenceEquals(range1, null))
				return ReferenceEquals(range2, null);

			if (ReferenceEquals(range2, null))
				return false;

			return range1.Ordinal == range2.Ordinal;
		}

		/// <summary>
		/// Compares two <see cref="DateRange"/> objects and returns a value indicating whether the first one is not equal to the second one.
		/// </summary>
		/// <param name="range1">The first range.</param>
		/// <param name="range2">The second range.</param>
		/// <returns>The value of <c>range1.Ordinal != range2.Ordinal</c>.</returns>
		public static bool operator !=(DateRange range1, DateRange range2)
		{
			if (ReferenceEquals(range1, null))
				return !ReferenceEquals(range2, null);

			if (ReferenceEquals(range2, null))
				return true;

			return range1.Ordinal != range2.Ordinal;
		}

		/// <summary>
		/// Compares two <see cref="DateRange"/> objects and returns a value indicating whether the first one is greater than the second one.
		/// </summary>
		/// <param name="range1">The first range.</param>
		/// <param name="range2">The second range.</param>
		/// <returns>The value of <c>range1.Ordinal &gt; range2.Ordinal</c>.</returns>
		public static bool operator >(DateRange range1, DateRange range2)
		{
			if (range1 == null) { throw new ArgumentNullException(nameof(range1)); }
			if (range2 == null) { throw new ArgumentNullException(nameof(range2)); }

			return range1.Ordinal > range2.Ordinal;
		}

		/// <summary>
		/// Compares two <see cref="DateRange"/> objects and returns a value indicating whether the first one is greater than or equal to the second one.
		/// </summary>
		/// <param name="range1">The first range.</param>
		/// <param name="range2">The second range.</param>
		/// <returns>The value of <c>range1.Ordinal &gt;= range2.Ordinal</c>.</returns>
		public static bool operator >=(DateRange range1, DateRange range2)
		{
			if (range1 == null) { throw new ArgumentNullException(nameof(range1)); }
			if (range2 == null) { throw new ArgumentNullException(nameof(range2)); }

			return range1.Ordinal >= range2.Ordinal;
		}

		/// <summary>
		/// Compares two <see cref="DateRange"/> objects and returns a value indicating whether the first one is less than the second one.
		/// </summary>
		/// <param name="range1">The first range.</param>
		/// <param name="range2">The second range.</param>
		/// <returns>The value of <c>range1.Ordinal &lt; range2.Ordinal</c>.</returns>
		public static bool operator <(DateRange range1, DateRange range2)
		{
			if (range1 == null) { throw new ArgumentNullException(nameof(range1)); }
			if (range2 == null) { throw new ArgumentNullException(nameof(range2)); }

			return range1.Ordinal < range2.Ordinal;
		}

		/// <summary>
		/// Compares two <see cref="DateRange"/> objects and returns a value indicating whether the first one is less than or equal to the second one.
		/// </summary>
		/// <param name="range1">The first range.</param>
		/// <param name="range2">The second range.</param>
		/// <returns>The value of <c>range1.Ordinal &lt;= range2.Ordinal</c>.</returns>
		public static bool operator <=(DateRange range1, DateRange range2)
		{
			if (range1 == null) { throw new ArgumentNullException(nameof(range1)); }
			if (range2 == null) { throw new ArgumentNullException(nameof(range2)); }

			return range1.Ordinal <= range2.Ordinal;
		}

		/// <summary>
		/// Gets the <see cref="DateRange"/> that corresponds to the specified ordinal.
		/// </summary>
		/// <param name="ordinal">The ordinal.</param>
		/// <returns>The <see cref="DateRange"/> instance. For 0 it returns <see cref="DateRange.ThisWeek"/>, 1 gives <see cref="DateRange.Last7Days"/> and so on.</returns>
		public static DateRange FromOrdinal(int ordinal)
		{
			switch (ordinal) {
				case 0:
					return ThisWeek;
				case 1:
					return Last7Days;
				case 2:
					return ThisMonth;
				case 3:
					return Last30Days;
				case 4:
					return ThisQuarter;
				case 5:
					return Last3Months;
				case 6:
					return ThisYear;
				case 7:
					return Last12Months;
				case 8:
					return AllTime;

				default:
					throw new ArgumentException("Invalid ordinal.");
			}
		}

		/// <summary>
		/// Returns the <see cref="T:Yutaka.DateRange"/> that corresponds to the supplied <see langword="string" />.
		/// </summary>
		/// <param name="rangeName">The textual representation of the date range.</param>
		/// <returns>The enumeration value.</returns>
		public static DateRange FromString(string rangeName)
		{
			if (rangeName == null)
				throw new ArgumentNullException(nameof(rangeName));

			if (rangeName.Equals("This Week", StringComparison.OrdinalIgnoreCase))
				return ThisWeek;

			if (rangeName.Equals("Last 7 Days", StringComparison.OrdinalIgnoreCase))
				return Last7Days;

			if (rangeName.Equals("This Month", StringComparison.OrdinalIgnoreCase))
				return ThisMonth;

			if (rangeName.Equals("Last 30 Days", StringComparison.OrdinalIgnoreCase))
				return Last30Days;

			if (rangeName.Equals("This Quarter", StringComparison.OrdinalIgnoreCase))
				return ThisQuarter;

			if (rangeName.Equals("Last 3 Months", StringComparison.OrdinalIgnoreCase))
				return Last3Months;

			if (rangeName.Equals("This Year", StringComparison.OrdinalIgnoreCase))
				return ThisYear;

			if (rangeName.Equals("Last 12 Months", StringComparison.OrdinalIgnoreCase))
				return Last12Months;

			if (rangeName.Equals("All Time", StringComparison.OrdinalIgnoreCase))
				return AllTime;

			throw new ArgumentException($"Unknown date range: {rangeName}");
		}

		/// <summary>
		/// Returns a string representation of the date range.
		/// </summary>
		/// <returns>Log range name.</returns>
		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return Ordinal;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>Value of <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			DateRange other = obj as DateRange;
			if ((object) other == null) {
				return false;
			}

			return Ordinal == other.Ordinal;
		}

		/// <summary>
		/// Determines whether the specified <see cref="Yutaka.DateRange"/> instance is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="Yutaka.DateRange"/> to compare with this instance.</param>
		/// <returns>Value of <c>true</c> if the specified <see cref="Yutaka.DateRange"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public bool Equals(DateRange other)
		{
			return other != null && Ordinal == other.Ordinal;
		}

		/// <summary>
		/// Compares the range to the other <see cref="DateRange"/> object.
		/// </summary>
		/// <param name="obj">The object object.</param>
		/// <returns>A value less than zero when this logger's <see cref="Ordinal"/> is less than the other logger's ordinal, 0 when they are equal and greater than zero when this ordinal is greater than the other ordinal.
		/// </returns>
		public int CompareTo(object obj)
		{
			if (obj == null)
				throw new ArgumentNullException(nameof(obj));

			// The code below does NOT account if the casting to DateRange returns null. This is because as this class is sealed and does not provide any public constructors it is impossible to create a invalid instance.
			DateRange range = (DateRange)obj;
			return Ordinal - range.Ordinal;
		}

		#region Implementation of IConvertible
		TypeCode IConvertible.GetTypeCode()
		{
			return TypeCode.Object;
		}

		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(_ordinal);
		}

		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(_ordinal);
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(_ordinal);
		}

		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return _ordinal;
		}

		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(_ordinal);
		}

		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(_ordinal);
		}

		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(_ordinal);
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(_ordinal);
		}

		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(_ordinal);
		}

		string IConvertible.ToString(IFormatProvider provider)
		{
			return _name;
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			if (conversionType == typeof(string))
				return Name;
			else
				return Convert.ChangeType(_ordinal, conversionType, provider);
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(_ordinal);
		}

		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(_ordinal);
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(_ordinal);
		}
		#endregion
	}

	public static class DateRangeExtension
	{
		/// <summary>
		/// Gets the StartDate of the specified <see cref="T:Yutaka.DateRange"/>.
		/// </summary>
		/// <param name="range">The <see cref="T:Yutaka.DateRange"/> to get the StartDate from.</param>
		/// <returns>A <see langword="DateTime" /> object representing the start date of the <see cref="T:Yutaka.DateRange"/>.</returns>
		public static DateTime StartDate(this DateRange range)
		{
			switch (range.Name) {
				case "This Week":
					return DateTimeUtil.GetBeginningOfWeek();
				case "Last 7 Days":
					return DateTime.Today.AddDays(-7);
				case "This Month":
					return DateTimeUtil.GetBeginningOfMonth();
				case "Last 30 Days":
					return DateTime.Today.AddDays(-30);
				case "This Quarter":
					return DateTimeUtil.GetBeginningOfQuarter();
				case "Last 3 Months":
					return DateTime.Today.AddMonths(-3);
				case "This Year":
					return DateTimeUtil.GetBeginningOfYear();
				case "Last 12 Months":
					return DateTime.Today.AddMonths(-12);
				case "All Time":
					return new DateTime(2000, 1, 1);

				default:
					throw new ArgumentException("Invalid DateRange.");
			}
		}

		/// <summary>
		/// Gets the EndDate of the specified <see cref="T:Yutaka.DateRange"/>.
		/// </summary>
		/// <param name="range">The <see cref="T:Yutaka.DateRange"/> to get the EndDate from.</param>
		/// <returns>A <see langword="DateTime" /> object representing the end date of the <see cref="T:Yutaka.DateRange"/>.</returns>
		public static DateTime EndDate(this DateRange range)
		{
			switch (range.Name) {
				case "This Week":
					return DateTimeUtil.GetEndOfWeek();
				case "This Month":
					return DateTimeUtil.GetEndOfMonth();
				case "This Quarter":
					return DateTimeUtil.GetEndOfQuarter();
				case "This Year":
				case "All Time":
					return DateTimeUtil.GetEndOfYear();
				case "Last 7 Days":
				case "Last 30 Days":
				case "Last 3 Months":
				case "Last 12 Months":
					return DateTime.Today;

				default:
					throw new ArgumentException("Invalid DateRange.");
			}
		}
	}
}