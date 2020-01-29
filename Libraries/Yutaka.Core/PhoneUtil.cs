using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka
{
	public static class PhoneUtil
	{
		//public static string Beautify(string phone)
		//{
		//	if (String.IsNullOrWhiteSpace(phone))
		//		return "";

		//	var minified = Minify(phone);
		//	var substring = minified.Substring(1);
		//	var minifiedLength = minified.Length;

		//	if (minifiedLength < 7)
		//		phone = minified;

		//	else if (minifiedLength == 7 && minified.All(Char.IsDigit))
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

		//	if (String.IsNullOrWhiteSpace(ext))
		//		return phone;

		//	return String.Format("{0} ext.{1}", phone, ext);
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

			var minified = phone.Replace("`", "").Replace("~", "").Replace("!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("=", "").Replace("{", "").Replace("[", "").Replace("}", "").Replace("]", "").Replace("|", "").Replace(@"\", "").Replace(":", "").Replace(";", "").Replace("\"", "").Replace("'", "").Replace("<", "").Replace(",", "").Replace(">", "").Replace(".", "").Replace("/", "").Replace(" ", "");

			while (minified.Contains(" "))
				minified = minified.Replace(" ", "");

			return minified;
		}
	}
}