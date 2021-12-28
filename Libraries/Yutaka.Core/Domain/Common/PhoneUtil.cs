using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Yutaka.Core.Domain.Common
{
	public static class PhoneUtil
	{
		private static readonly Regex TenDigits = new Regex(@"\d{10}", RegexOptions.Compiled);
		private static readonly Regex Whitespace = new Regex(@"\s+", RegexOptions.Compiled);

		//public static string Beautify(string phone)
		//{
		//	if (String.IsNullOrWhiteSpace(phone))
		//		return "";

		//	var minified = Minify(phone);
		//	var minifiedLength = minified.Length;

		//	if (minifiedLength < 7)
		//		return phone;

		//	var result = "";

		//	if (phone.StartsWith("+")) {
		//		result = "+";
		//		minified = minified.Substring(1);
		//	}

		//	#region Check for extension
		//	var ext = "";
		//	string[] split;
		//	minified = minified.ToUpper();

		//	if (minified.Contains("EXT.")) {
		//		split = minified.Split(new string[] { "EXT." }, StringSplitOptions.None);
		//		minified = split[0].Trim();
		//		ext = split[1].Trim();
		//	}

		//	else if (minified.Contains("EXT")) {
		//		split = minified.Split(new string[] { "EXT" }, StringSplitOptions.None);
		//		minified = split[0].Trim();
		//		ext = split[1].Trim();
		//	}

		//	else if (minified.Contains("EX.")) {
		//		split = minified.Split(new string[] { "EX." }, StringSplitOptions.None);
		//		minified = split[0].Trim();
		//		ext = split[1].Trim();
		//	}

		//	else if (minified.Contains("XT.")) {
		//		split = minified.Split(new string[] { "XT." }, StringSplitOptions.None);
		//		minified = split[0].Trim();
		//		ext = split[1].Trim();
		//	}

		//	else if (minified.Contains("EX")) {
		//		split = minified.Split(new string[] { "EX" }, StringSplitOptions.None);
		//		minified = split[0].Trim();
		//		ext = split[1].Trim();
		//	}

		//	else if (minified.Contains("XT")) {
		//		split = minified.Split(new string[] { "XT" }, StringSplitOptions.None);
		//		minified = split[0].Trim();
		//		ext = split[1].Trim();
		//	}

		//	else if (minified.Contains("E.")) {
		//		split = minified.Split(new string[] { "E." }, StringSplitOptions.None);
		//		minified = split[0].Trim();
		//		ext = split[1].Trim();
		//	}

		//	else if (minified.Contains("X.")) {
		//		split = minified.Split(new string[] { "X." }, StringSplitOptions.None);
		//		minified = split[0].Trim();
		//		ext = split[1].Trim();
		//	}
		//	#endregion Check for extension

		//	if (minifiedLength == 7 && minified.All(Char.IsDigit))
		//		phone = String.Format("{0}-{1}", minified.Substring(0, 3), minified.Substring(3, 4));

		//	else if (minifiedLength == 10 && minified.All(Char.IsDigit))
		//		phone = String.Format("({0}) {1}-{2}", minified.Substring(0, 3), minified.Substring(3, 3), minified.Substring(6, 4));

		//	else if (minifiedLength == 11) {
		//		if (minified.All(Char.IsDigit))
		//			phone = String.Format("{0} ({1}) {2}-{3}", minified.Substring(0, 1), minified.Substring(1, 3), minified.Substring(4, 3), minified.Substring(7, 4));
		//		else if (substring.All(Char.IsDigit))
		//			phone = String.Format("{0}-{1}-{2}", minified.Substring(0, 4), minified.Substring(4, 3), minified.Substring(7, 4));
		//	}

		//	else if (minifiedLength == 12 && substring.All(Char.IsDigit))
		//		phone = String.Format("{0} ({1}) {2}-{3}", minified.Substring(0, 2), minified.Substring(2, 3), minified.Substring(5, 3), minified.Substring(8, 4));

		//	else if (minifiedLength == 13 && substring.All(Char.IsDigit))
		//		phone = String.Format("{0} ({1}) {2}-{3}", minified.Substring(0, 3), minified.Substring(3, 3), minified.Substring(6, 3), minified.Substring(9, 4));

		//	result = String.Format("{0}{1}", result, minified);

		//	if (String.IsNullOrWhiteSpace(ext))
		//		return result;

		//	return String.Format("{0} ext.{1}", result, ext);
		//}

		/// <summary>
		/// Checks whether a phone number is valid or not. General criteria is at least 10 characters and doesn't contain a sequence of similar/bogus
		/// numbers.
		/// </summary>
		/// <param name="phone">The phone number to check.</param>
		/// <returns>true if valid. Otherwise false.</returns>
		public static bool IsValid(string phone)
		{
			if (String.IsNullOrWhiteSpace(phone))
				return false;

			phone = Minify(phone);

			if (phone.Length < 10)
				return false;

			if (phone.Contains("0000000") || phone.Contains("1111111") || phone.Contains("2222222") || phone.Contains("3333333") ||
				phone.Contains("4444444") || phone.Contains("5555555") || phone.Contains("6666666") || phone.Contains("7777777") ||
				phone.Contains("8888888") || phone.Contains("9999999") || phone.Contains("12345678") || phone.Contains("98765432"))
				return false;

			if (!TenDigits.IsMatch(phone))
				return false;

			return true;
		}

		/// <summary>
		/// Removes all non-alphanumerics from a phone number except for '+'.
		/// </summary>
		/// <param name="phone">The phone number to minify.</param>
		/// <returns>The minified phone number.</returns>
		public static string Minify(string phone)
		{
			if (String.IsNullOrWhiteSpace(phone))
				return "";

			var startsWithPlus = false;
			var hasExtension = false;
			var split = SplitExtension(phone);

			if (!String.IsNullOrWhiteSpace(split[0])) {
				if (split[0].StartsWith("+"))
					startsWithPlus = true;

				split[0] = Whitespace.Replace(split[0], "");
				split[0] = split[0].Replace("`", "").Replace("~", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("=", "").Replace("+", "").Replace("{", "").Replace("[", "").Replace("}", "").Replace("]", "").Replace("|", "").Replace(@"\", "").Replace(":", "").Replace(";", "").Replace("\"", "").Replace("'", "").Replace("<", "").Replace(",", "").Replace(">", "").Replace(".", "").Replace("/", "");

				if (startsWithPlus)
					split[0] = String.Format("+{0}", split[0]);
			}

			if (!String.IsNullOrWhiteSpace(split[1])) {
				hasExtension = true;
				split[1] = Whitespace.Replace(split[1], "");
				split[1] = split[1].Replace("`", "").Replace("~", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("=", "").Replace("+", "").Replace("{", "").Replace("[", "").Replace("}", "").Replace("]", "").Replace("|", "").Replace(@"\", "").Replace(":", "").Replace(";", "").Replace("\"", "").Replace("'", "").Replace("<", "").Replace(",", "").Replace(">", "").Replace(".", "").Replace("/", "");
			}

			if (hasExtension)
				return String.Format("{0}ext{1}", split[0], split[1]);

			return split[0];
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

			var lower = phone.ToLower();

			if (lower.Contains("ext."))
				return lower.Split(new string[] { "ext." }, StringSplitOptions.None);
			if (lower.Contains("ext"))
				return lower.Split(new string[] { "ext" }, StringSplitOptions.None);
			if (lower.Contains("ex."))
				return lower.Split(new string[] { "ex." }, StringSplitOptions.None);
			if (lower.Contains("ex "))
				return lower.Split(new string[] { "ex " }, StringSplitOptions.None);
			if (lower.Contains("xt."))
				return lower.Split(new string[] { "xt." }, StringSplitOptions.None);
			if (lower.Contains("xt"))
				return lower.Split(new string[] { "xt" }, StringSplitOptions.None);
			if (lower.Contains("e."))
				return lower.Split(new string[] { "e." }, StringSplitOptions.None);
			if (lower.Contains("x."))
				return lower.Split(new string[] { "x." }, StringSplitOptions.None);

			return new string[] { phone, "" };
		}
	}
}