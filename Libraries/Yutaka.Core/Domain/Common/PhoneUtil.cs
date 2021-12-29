using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Yutaka.Core.Domain.Common
{
	public static class PhoneUtil
	{
		private static readonly TextInfo EnglishUS = new CultureInfo("en-US", false).TextInfo;

		/// <summary>
		/// Checks whether a phone number is valid or not. General criteria is at least 10 characters and doesn't contain a sequence
		/// of similar/bogus numbers.
		/// </summary>
		/// <param name="phone">The phone number to check.</param>
		/// <returns>true if valid. Otherwise false.</returns>
		public static bool IsValid(string phone)
		{
			if (String.IsNullOrWhiteSpace(phone))
				return false;

			phone = Minify(phone);
			phone = SplitExtension(phone)[0];

			if (phone.Length < 10 || 20 < phone.Length)
				return false;

			if (phone.Contains("0000000") || phone.Contains("1111111") || phone.Contains("2222222") || phone.Contains("3333333") ||
				phone.Contains("4444444") || phone.Contains("5555555") || phone.Contains("6666666") || phone.Contains("7777777") ||
				phone.Contains("8888888") || phone.Contains("9999999") || phone.Contains("12345678") || phone.Contains("98765432"))
				return false;

			return true;
		}

		/// <summary>
		/// Removes all non-numbers from a phone number. If an extension is detected, it will return the number, "ex." then the extension with no spaces.
		/// </summary>
		/// <param name="phone">The phone number to minify.</param>
		/// <returns>The minified phone number.</returns>
		public static string Minify(string phone)
		{
			if (String.IsNullOrWhiteSpace(phone))
				return "";

			var split = SplitExtension(phone);
			var number = split[0];
			var extension = split[1];

			number = new string(number.Where(c => char.IsDigit(c)).ToArray());

			if (String.IsNullOrWhiteSpace(number))
				return "";
			if (String.IsNullOrWhiteSpace(extension))
				return number;

			return String.Format("{0}ex.{1}", number, extension.Trim());
		}

		/// <summary>
		/// Attempts to detect extensions within a string and splits it if it finds it.
		/// </summary>
		/// <param name="phone">The phone number to split.</param>
		/// <returns>A string[] containing the phone number and extension.</returns>
		public static string[] SplitExtension(string phone)
		{
			if (String.IsNullOrWhiteSpace(phone))
				return new string[] { "", "" };

			if (phone.IndexOf("ext.", StringComparison.OrdinalIgnoreCase) > -1)
				return Regex.Split(phone, @"ext\.", RegexOptions.IgnoreCase);
			if (phone.IndexOf("ext", StringComparison.OrdinalIgnoreCase) > -1)
				return Regex.Split(phone, "ext", RegexOptions.IgnoreCase);

			if (phone.IndexOf("ex.", StringComparison.OrdinalIgnoreCase) > -1)
				return Regex.Split(phone, @"ex\.", RegexOptions.IgnoreCase);
			if (phone.IndexOf("ex ", StringComparison.OrdinalIgnoreCase) > -1)
				return Regex.Split(phone, "ex ", RegexOptions.IgnoreCase);

			if (phone.IndexOf("xt.", StringComparison.OrdinalIgnoreCase) > -1)
				return Regex.Split(phone, @"xt\.", RegexOptions.IgnoreCase);
			if (phone.IndexOf("xt ", StringComparison.OrdinalIgnoreCase) > -1)
				return Regex.Split(phone, "xt ", RegexOptions.IgnoreCase);

			return new string[] { phone, "" };
		}

		/// <summary>
		/// Returns the standardized format of a phone label.
		/// </summary>
		/// <param name="label">The string to standardize.</param>
		/// <returns></returns>
		public static string StandardizeLabel(string label)
		{
			if (String.IsNullOrWhiteSpace(label))
				return "";

			label = label.Trim();

			if (label.Equals(label.ToLowerInvariant()))
				return EnglishUS.ToTitleCase(label);

			return label;
		}
	}
}