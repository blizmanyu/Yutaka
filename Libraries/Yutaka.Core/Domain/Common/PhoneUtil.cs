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
		private const string PLUS = "+";
		private const int MIN_PHONE_LENGTH = 10;
		private const int MAX_PHONE_LENGTH = 17;
		private static readonly TextInfo EnglishUS = new CultureInfo("en-US", false).TextInfo;

		/// <summary>
		/// Converts a string of numbers into its "pretty"-formatted version [ex.: (000) 000-0000]. If the number is invalid, returns the original string as-is.
		/// </summary>
		/// <param name="phone">The phone number to beautify.</param>
		/// <returns></returns>
		public static string Beautify(string phone)
		{
			if (String.IsNullOrWhiteSpace(phone))
				return "";

			phone = Minify(phone);
			var split = Split(phone);
			var plus = split[0];
			var number = split[1];
			var extension = split[2];

			if (number.Length < MIN_PHONE_LENGTH || MAX_PHONE_LENGTH < number.Length)
				return phone;
			if (number.Length == 10)
				number = Regex.Replace(number, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
			else if (number.Length == 11)
				number = Regex.Replace(number, @"(\d{1})(\d{3})(\d{3})(\d{4})", "$1 ($2) $3-$4");
			else if (number.Length == 12)
				number = Regex.Replace(number, @"(\d{2})(\d{3})(\d{3})(\d{4})", "$1 ($2) $3-$4");
			else if (number.Length == 13)
				number = Regex.Replace(number, @"(\d{3})(\d{3})(\d{3})(\d{4})", "$1 ($2) $3-$4");

			else if (number.Length == 14)
				number = Regex.Replace(number, @"(\d{3})(\d{4})(\d{3})(\d{4})", "$1 ($2) $3-$4");
			else if (number.Length == 15)
				number = Regex.Replace(number, @"(\d{3})(\d{4})(\d{4})(\d{4})", "$1 ($2) $3-$4");
			else if (number.Length == 16)
				number = Regex.Replace(number, @"(\d{4})(\d{4})(\d{4})(\d{4})", "$1 ($2) $3-$4");

			else if (number.Length == 17)
				number = Regex.Replace(number, @"(\d{3})(\d{3})(\d{4})(\d{3})(\d{4})", "$1-$2 ($3) $4-$5");

			if (String.IsNullOrWhiteSpace(extension))
				return String.Format("{0}{1}", plus, number);

			return String.Format("{0}{1} ext.{2}", plus, number, extension);
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

			var split = Split(phone);
			var plus = split[0];
			var number = split[1];
			var extension = split[2];

			number = new string(number.Where(c => char.IsDigit(c)).ToArray());

			if (String.IsNullOrWhiteSpace(number))
				return "";
			if (String.IsNullOrWhiteSpace(extension))
				return String.Format("{0}{1}", plus, number);

			return String.Format("{0}{1}ex.{2}", plus, number, extension.Trim());
		}

		/// <summary>
		/// Attempts to detect an Extension, as well as a leading "+" from international numbers. Always returns a string[3] with
		/// plus, the number, the extension, in that order.
		/// </summary>
		/// <param name="phone">The phone number to split.</param>
		/// <returns>A string[] containing the phone number and extension.</returns>
		public static string[] Split(string phone)
		{
			if (String.IsNullOrWhiteSpace(phone))
				return new string[] { "", "", "" };

			string[] split = null;
			var result = new string[3] { "", phone, "" };
			var number = phone.Trim();

			if (number.StartsWith(PLUS)) {
				result[0] = PLUS;
				number = number.Substring(1);
				result[1] = number;
			}

			if (number.IndexOf("ext.", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, @"ext\.", RegexOptions.IgnoreCase);

			// 3 chars //
			else if (number.IndexOf("ext", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, "ext", RegexOptions.IgnoreCase);
			else if (number.IndexOf("ex.", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, @"ex\.", RegexOptions.IgnoreCase);
			else if (number.IndexOf("xt.", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, @"xt\.", RegexOptions.IgnoreCase);

			// 2 chars //
			else if (number.IndexOf("ex", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, "ex", RegexOptions.IgnoreCase);
			else if (number.IndexOf("xt", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, "xt", RegexOptions.IgnoreCase);
			else if (number.IndexOf("e.", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, @"e\.", RegexOptions.IgnoreCase);
			else if (number.IndexOf("x.", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, @"x\.", RegexOptions.IgnoreCase);

			// 1 char //
			else if (number.IndexOf("e", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, "e", RegexOptions.IgnoreCase);
			else if (number.IndexOf("x", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, "x", RegexOptions.IgnoreCase);

			if (split == null || split.Length < 2 || String.IsNullOrWhiteSpace(split[1]))
				return result;

			result[1] = split[0];
			result[2] = split[1];
			return result;
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

		/// <summary>
		/// Validates a phone number. If a phone is invalid, the error message will be contained in &lt;errorMessage&gt;. Otherwise, it will be <see cref="String.Empty"/>.
		/// </summary>
		/// <param name="phone">The phone number to validate.</param>
		/// <param name="errorMessage">The error message, if any.</param>
		/// <returns></returns>
		public static bool Validate(string phone, out string errorMessage)
		{
			if (String.IsNullOrWhiteSpace(phone)) {
				errorMessage = "Phone is required.";
				return false;
			}

			var number = Split(phone)[1];
			number = new string(number.Where(c => char.IsDigit(c)).ToArray());

			if (String.IsNullOrWhiteSpace(number)) {
				errorMessage = "Phone is required.";
				return false;
			}

			if (number.Length < 7) {
				errorMessage = "Phone is too short.";
				return false;
			}

			if (number.Length < MIN_PHONE_LENGTH) {
				errorMessage = "Phone is too short. Make sure you include an Area Code. If you're using an international number, please include your Country Code.";
				return false;
			}

			if (number.Length > MAX_PHONE_LENGTH) {
				errorMessage = "Phone is too long.";
				return false;
			}

			if (number.Contains("0000000") || number.Contains("1111111") || number.Contains("2222222") || number.Contains("3333333") ||
				number.Contains("4444444") || number.Contains("5555555") || number.Contains("6666666") || number.Contains("7777777") ||
				number.Contains("8888888") || number.Contains("9999999") || number.Contains("12345678") || number.Contains("98765432")) {
				errorMessage = "Phone is invalid. Please enter a real phone number.";
				return false;
			}

			errorMessage = "";
			return true;
		}
	}
}