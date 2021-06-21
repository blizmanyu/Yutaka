using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Yutaka.Utils;

namespace Yutaka.Text
{
	public static class TextUtil
	{
		private static readonly Regex RegexWhitespace = new Regex(@"\s+", RegexOptions.Compiled);
		private static readonly TextInfo CurrentCulture = CultureInfo.CurrentCulture.TextInfo;

		#region Methods
		#region Encode/Decode
		public static string Encode(string input)
		{
			if (String.IsNullOrEmpty(input))
				throw new ArgumentNullException("input", "<input> is required.");

			var sb = new StringBuilder();

			for (int i = 0; i < input.Length; i++)
				sb.Append((char) (input[i] + 2));

			return sb.ToString();
		}

		public static string Decode(string input)
		{
			if (String.IsNullOrEmpty(input))
				throw new ArgumentNullException("input", "<input> is required.");

			var sb = new StringBuilder();

			for (int i = 0; i < input.Length; i++)
				sb.Append((char) (input[i] - 2));

			return sb.ToString();
		}
		#endregion Encode/Decode

		#region Phone Utils
		public static string BeautifyPhone(string phone)
		{
			if (String.IsNullOrWhiteSpace(phone))
				return "";

			#region Handle Extensions
			var ext = "";

			if (phone.ToUpper().Contains("EXT.")) {
				var ind = phone.ToUpper().IndexOf("EXT.");
				Console.Write("\nind: {0}", ind);
				ext = phone.Substring(ind + 4, phone.Length - ind - 4).Trim();
				Console.Write("\next: {0}", ext);
				if (ind == 0)
					return String.Format("ext.{0}", ext);
				phone = phone.Substring(0, ind).Trim();
				Console.Write("\nphone: {0}", phone);
			}

			else if (phone.ToUpper().Contains("EXT")) {
				var ind = phone.ToUpper().IndexOf("EXT");
				Console.Write("\nind: {0}", ind);
				ext = phone.Substring(ind + 3, phone.Length - ind - 3).Trim();
				Console.Write("\next: {0}", ext);
				if (ind == 0)
					return String.Format("ext.{0}", ext);
				phone = phone.Substring(0, ind).Trim();
				Console.Write("\nphone: {0}", phone);
			}

			else if (phone.ToUpper().Contains("EX.")) {
				var ind = phone.ToUpper().IndexOf("EX.");
				Console.Write("\nind: {0}", ind);
				ext = phone.Substring(ind + 3, phone.Length - ind - 3).Trim();
				Console.Write("\next: {0}", ext);
				if (ind == 0)
					return String.Format("ext.{0}", ext);
				phone = phone.Substring(0, ind).Trim();
				Console.Write("\nphone: {0}", phone);
			}

			else if (phone.ToUpper().Contains("XT.")) {
				var ind = phone.ToUpper().IndexOf("XT.");
				Console.Write("\nind: {0}", ind);
				ext = phone.Substring(ind + 3, phone.Length - ind - 3).Trim();
				Console.Write("\next: {0}", ext);
				if (ind == 0)
					return String.Format("ext.{0}", ext);
				phone = phone.Substring(0, ind).Trim();
				Console.Write("\nphone: {0}", phone);
			}

			else if (phone.ToUpper().Contains("XT")) {
				var ind = phone.ToUpper().IndexOf("XT");
				Console.Write("\nind: {0}", ind);
				ext = phone.Substring(ind + 2, phone.Length - ind - 2).Trim();
				Console.Write("\next: {0}", ext);
				if (ind == 0)
					return String.Format("ext.{0}", ext);
				phone = phone.Substring(0, ind).Trim();
				Console.Write("\nphone: {0}", phone);
			}

			else if (phone.ToUpper().Contains("X.")) {
				var ind = phone.ToUpper().IndexOf("X.");
				Console.Write("\nind: {0}", ind);
				ext = phone.Substring(ind + 2, phone.Length - ind - 2).Trim();
				Console.Write("\next: {0}", ext);
				if (ind == 0)
					return String.Format("ext.{0}", ext);
				phone = phone.Substring(0, ind).Trim();
				Console.Write("\nphone: {0}", phone);
			}
			#endregion Handle Extensions

			var stripped = StripPhone(phone);
			var substring = stripped.Substring(1);
			var len = stripped.Length;

			if (len < 7)
				phone = stripped;

			else if (len == 7 && stripped.All(Char.IsDigit))
				phone = String.Format("{0}-{1}", stripped.Substring(0, 3), stripped.Substring(3, 4));

			else if (len == 10 && stripped.All(Char.IsDigit))
				phone = String.Format("({0}) {1}-{2}", stripped.Substring(0, 3), stripped.Substring(3, 3), stripped.Substring(6, 4));

			else if (len == 11) {
				if (stripped.All(Char.IsDigit))
					phone = String.Format("{0} ({1}) {2}-{3}", stripped.Substring(0, 1), stripped.Substring(1, 3), stripped.Substring(4, 3), stripped.Substring(7, 4));
				else if (substring.All(Char.IsDigit))
					phone = String.Format("{0}-{1}-{2}", stripped.Substring(0, 4), stripped.Substring(4, 3), stripped.Substring(7, 4));
			}

			else if (len == 12 && substring.All(Char.IsDigit))
				phone = String.Format("{0} ({1}) {2}-{3}", stripped.Substring(0, 2), stripped.Substring(2, 3), stripped.Substring(5, 3), stripped.Substring(8, 4));

			else if (len == 13 && substring.All(Char.IsDigit))
				phone = String.Format("{0} ({1}) {2}-{3}", stripped.Substring(0, 3), stripped.Substring(3, 3), stripped.Substring(6, 3), stripped.Substring(9, 4));

			if (String.IsNullOrWhiteSpace(ext))
				return phone;

			return String.Format("{0} ext.{1}", phone, ext);
		}

		[Obsolete("Deprecated Jan 28, 2020. Use PhoneUtil.IsValid() instead.", false)]
		public static bool IsValidPhone(string phone)
		{
			if (String.IsNullOrWhiteSpace(phone))
				return false;

			phone = StripPhone(phone);

			if (phone.Length < 10)
				return false;

			if (phone.Contains("0000000") || phone.Contains("1111111") || phone.Contains("2222222") || phone.Contains("3333333") ||
				phone.Contains("4444444") || phone.Contains("5555555") || phone.Contains("6666666") || phone.Contains("7777777") ||
				phone.Contains("8888888") || phone.Contains("9999999") || phone.Contains("12345678") || phone.Contains("98765432"))
				return false;

			return true;
		}

		[Obsolete("Deprecated Jan 28, 2020. Use PhoneUtil.Minify() instead.", false)]
		public static string StripPhone(string phone)
		{
			if (String.IsNullOrWhiteSpace(phone))
				return "";

			var stripped = phone.Replace("~", "").Replace("`", "").Replace("!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("=", "").Replace("{", "").Replace("[", "").Replace("}", "").Replace("]", "").Replace("|", "").Replace(@"\", "").Replace(":", "").Replace(";", "").Replace("\"", "").Replace("'", "").Replace("<", "").Replace(",", "").Replace(">", "").Replace(".", "").Replace("/", "").Trim();

			while (stripped.Contains(" "))
				stripped = stripped.Replace(" ", "");

			return stripped;
		}
		#endregion Phone Utils

		public static string BeautifyJson(string str)
		{
			if (String.IsNullOrWhiteSpace(str))
				return "";

			return str.Replace("{", "{\n  ").Replace("\":\"", "\": \"").Replace("\",\"", "\",\n  \"").Replace("}", "\n}");
		}

		public static Int64 ConvertIpToInt64(string ip)
		{
			if (String.IsNullOrEmpty(ip))
				throw new ArgumentNullException("ip", "<ip> is required.");
			if (ip.Length < 7 || ip.Length > 16)
				throw new ArgumentOutOfRangeException("ip", ip, "<ip> must be a value between 0.0.0.0 and 255.255.255.255");

			var sb = new StringBuilder();
			ip.Split('.').ToList().ForEach(u => sb.Append(u.ToString().PadLeft(3, '0')));
			sb.Replace(".", "");

			Int64 result;
			if (Int64.TryParse(sb.ToString(), out result))
				return result;

			return -1;
		}

		public static Int64 ConvertTimeToInt64(DateTime? time = null)
		{
			if (time == null)
				time = DateTime.Now;

			Int64 result;
			if (Int64.TryParse(((DateTime) time).ToString("yyyyMMddHHmmssfff"), out result))
				return result;

			return -1;
		}

		public static string ConvertIpToBase36(string ip, bool lowerCase = true)
		{
			if (String.IsNullOrEmpty(ip))
				throw new ArgumentNullException("ip", "<ip> is required.");
			if (ip.Length < 7 || ip.Length > 16)
				throw new ArgumentOutOfRangeException("ip", ip, "<ip> must be a value between 0.0.0.0 and 255.255.255.255");

			var sb = new StringBuilder();
			ip.Split('.').ToList().ForEach(u => sb.Append(u.ToString().PadLeft(3, '0')));
			sb.Replace(".", "");

			Int64 result;
			if (Int64.TryParse(sb.ToString(), out result))
				return Base36.Encode(result, lowerCase);

			return ip;
		}

		public static string ConvertTimeToBase36(DateTime? time = null, bool lowerCase = true)
		{
			if (time == null)
				time = DateTime.Now;

			var timeStr = ((DateTime) time).ToString("yyyyMMddHHmmssfff");

			Int64 result;
			if (Int64.TryParse(timeStr, out result))
				return Base36.Encode(result, lowerCase);

			return timeStr;
		}

		public static string DecodeBase64(string str)
		{
			if (String.IsNullOrWhiteSpace(str))
				return "";

			var incoming = str.Replace('_', '/').Replace('-', '+');

			switch (str.Length % 4) {
				case 2:
					incoming += "==";
					break;
				case 3:
					incoming += "=";
					break;
			}

			var bytes = Convert.FromBase64String(incoming);
			return Encoding.ASCII.GetString(bytes);
		}

		public static string FormatCommonLabels(string label)
		{
			if (String.IsNullOrWhiteSpace(label))
				return "";

			string[] commonLabels = { "HOME", "WORK", "OFFICE", "OTHER", "MOBILE", "CELL", "MAIN", "FAX", "PAGER" };
			label = label.Trim();
			var labelUpper = label.ToUpper();

			for (int i = 0; i < commonLabels.Length; i++) {
				if (labelUpper.Equals(commonLabels[i]))
					return ToTitleCase(label, 0);
			}

			return ToTitleCase(label, 0);
		}

		public static string GetPlainTextFromHtml(string html)
		{
			if (String.IsNullOrWhiteSpace(html))
				return "";

			var htmlTagPattern = "<.*?>";
			var regexCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
			html = regexCss.Replace(html, "");
			html = Regex.Replace(html, htmlTagPattern, " ");
			html = Regex.Replace(html, @"^\s+$[\r\n]*", " ", RegexOptions.Multiline);
			html = html.Replace("&nbsp;", " ").Trim();

			while (html.Contains("  "))
				html = html.Replace("  ", " ");

			return html;
		}

		public static string RemoveExcessWhitespace(string input)
		{
			if (String.IsNullOrWhiteSpace(input))
				return "";

			input = input.Trim();

			while (input.Contains("\r\n\r\n\r\n"))
				input = input.Replace("\r\n\r\n\r\n", "\r\n\r\n");

			while (input.Contains("\r\r\r"))
				input = input.Replace("\r\r\r", "\r\r");

			while (input.Contains("\n\n\n"))
				input = input.Replace("\n\n\n", "\n\n");

			while (input.Contains("\t\t"))
				input = input.Replace("\t\t", "\t");

			while (input.Contains("  "))
				input = input.Replace("  ", " ");

			return input.Trim();
		}

		public static string ReplaceWhitespace(string input, string replacement)
		{
			return RegexWhitespace.Replace(input, replacement);
		}

		public static string StripExcessWhitespace(string str)
		{
			if (String.IsNullOrWhiteSpace(str))
				return "";

			str = str.Replace("\n", " ").Replace("\r", " ").Replace("\t", " ").Trim();

			while (str.Contains("  "))
				str = str.Replace("  ", " ");

			return str;
		}

		/// <summary>
		/// Converts the specified string to title case.
		/// </summary>
		/// <param name="str">The string to convert to title case.</param>
		/// <param name="ifLengthGreaterThan">Only converts if a word within the string has a length greater than &lt;ifLengthGreaterThan&gt;.</param>
		/// <returns>The specified string converted to title case.</returns>
		public static string ToTitleCase(string str, int ifLengthGreaterThan = 3)
		{
			if (String.IsNullOrWhiteSpace(str))
				return "";
			if (str.Equals(str.ToLower()))
				return CurrentCulture.ToTitleCase(str);

			var split = str.Split(' ');

			for (int i = 0; i < split.Length; i++) {
				if (split[i].Equals(split[i].ToLower()))
					split[i] = CurrentCulture.ToTitleCase(split[i]);

				else if (split[i].Length > ifLengthGreaterThan && split[i].Equals(split[i].ToUpper()))
					split[i] = CurrentCulture.ToTitleCase(split[i].ToLower());
			}

			return String.Join(" ", split);
		}
		#endregion Methods
	}
}