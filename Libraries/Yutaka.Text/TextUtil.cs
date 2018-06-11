using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Yutaka.Utils;

namespace Yutaka.Text
{
	public static class TextUtil
	{
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
			if (phone == null)
				return "";

			var stripped = StripPhone(phone);
			var substring = stripped.Substring(1);

			if (stripped.Length == 10 && stripped.All(Char.IsDigit))
				return String.Format("({0}) {1}-{2}", stripped.Substring(0, 3), stripped.Substring(3, 3), stripped.Substring(6, 4));
			if (stripped.Length == 11 && stripped.StartsWith("1") && substring.All(Char.IsDigit))
				return String.Format("{0} ({1}) {2}-{3}", stripped.Substring(0, 1), stripped.Substring(1, 3), stripped.Substring(4, 3), stripped.Substring(7, 4));
			if (stripped.Length == 12 && substring.All(Char.IsDigit))
				return String.Format("{0} ({1}) {2}-{3}", stripped.Substring(0, 2), stripped.Substring(2, 3), stripped.Substring(5, 3), stripped.Substring(8, 4));
			if (stripped.Length == 13 && substring.All(Char.IsDigit))
				return String.Format("{0} ({1}) {2}-{3}", stripped.Substring(0, 3), stripped.Substring(3, 3), stripped.Substring(6, 3), stripped.Substring(9, 4));

			return phone;
		}

		public static string StripPhone(string phone)
		{
			if (phone == null)
				return "";

			var stripped = phone.Trim().Replace("~", "").Replace("`", "").Replace("!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("=", "").Replace("{", "").Replace("[", "").Replace("}", "").Replace("]", "").Replace("|", "").Replace(@"\", "").Replace(":", "").Replace(";", "").Replace("\"", "").Replace("'", "").Replace("<", "").Replace(",", "").Replace(">", "").Replace(".", "").Replace("/", "").Trim();
			while (stripped.Contains(" "))
				stripped = stripped.Replace(" ", "");

			return stripped;
		}
		#endregion Phone Utils

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

		public static string ConvertIpToBase36(string ip, bool lowerCase=true)
		{
			if (String.IsNullOrEmpty(ip))
				throw new ArgumentNullException("ip", "<ip> is required.");
			if (ip.Length < 7 || ip.Length > 16)
				throw new ArgumentOutOfRangeException("ip", ip, "<ip> must be a value between 0.0.0.0 and 255.255.255.255");

			var sb = new StringBuilder();
			ip.Split('.').ToList().ForEach(u => sb.Append(u.ToString().PadLeft(3, '0')));
			sb.Replace(".","");

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

		public static string FormatCommonLabels(string label)
		{
			if (String.IsNullOrWhiteSpace(label))
				return "";

			label = label.Trim();
			var labelUpper = label.ToUpper();
			string[] commonLabels = { "HOME", "WORK", "OFFICE", "OTHER", "MOBILE", "CELL", "MAIN", "PAGER" };

			if (label.Length > 7 && (label == labelUpper || label == label.ToLower()))
				return TextUtil.ToTitleCase(label);

			for (int i = 0; i < commonLabels.Length; i++) {
				if (labelUpper == commonLabels[i])
					return TextUtil.ToTitleCase(label);
			}

			return label;
		}

		public static string ToTitleCase(string str)
		{
			if (str == null)
				return "";

			return new CultureInfo("en-US", false).TextInfo.ToTitleCase(str);
		}
	}
}