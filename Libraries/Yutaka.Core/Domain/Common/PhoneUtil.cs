﻿using System;
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
		private static readonly TextInfo EnglishUS = new CultureInfo("en-US", false).TextInfo;

		/// <summary>
		/// WIP: Do not use yet.
		/// </summary>
		/// <param name="phone"></param>
		/// <returns></returns>
		public static string Beautify(string phone)
		{
			if (String.IsNullOrWhiteSpace(phone))
				return "";

			phone = Minify(phone);
			var split = Split(phone);
			var number = split[0];
			var extension = split[1];
			var result = "";

			if (number.StartsWith("+")) {
				number = number.Replace("+", "");

				if (number.Length < 10)
					return phone;
				if (number.Length == 10)
					number = Regex.Replace(number, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
			}



			return phone;
		}

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
			phone = Split(phone)[1];

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
				number = number.Replace(PLUS, "");
				result[1] = number;
			}

			if (number.IndexOf("ext.", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, @"ext\.", RegexOptions.IgnoreCase);
			else if (number.IndexOf("ext", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, "ext", RegexOptions.IgnoreCase);

			else if (number.IndexOf("ex.", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, @"ex\.", RegexOptions.IgnoreCase);
			else if (number.IndexOf("ex ", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, "ex ", RegexOptions.IgnoreCase);

			else if (number.IndexOf("xt.", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, @"xt\.", RegexOptions.IgnoreCase);
			else if (number.IndexOf("xt ", StringComparison.OrdinalIgnoreCase) > -1)
				split = Regex.Split(number, "xt ", RegexOptions.IgnoreCase);

			if (split == null || split.Length < 2)
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
	}
}