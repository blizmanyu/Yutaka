using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yutaka.Utils
{
	public static class Base36
	{
		const string CharListLower = "0123456789abcdefghijklmnopqrstuvwxyz";
		const string CharListUpper = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		public static string Encode(long input, bool lowerCase = true)
		{
			if (input < 0)
				throw new ArgumentOutOfRangeException("input", input, "input cannot be negative");

			string CharList;
			if (lowerCase)
				CharList = CharListLower;
			else
				CharList = CharListUpper;

			char[] clistarr = CharList.ToCharArray();
			var result = new Stack<char>();
			while (input != 0) {
				result.Push(clistarr[input % 36]);
				input /= 36;
			}
			return new string(result.ToArray());
		}

		public static Int64 Decode(string input, bool lowerCase = true)
		{
			string CharList;
			if (lowerCase)
				CharList = CharListLower;
			else
				CharList = CharListUpper;

			var reversed = input.Reverse();
			long result = 0;
			var pos = 0;
			foreach (char c in reversed) {
				result += CharList.IndexOf(c) * (long) Math.Pow(36, pos);
				pos++;
			}
			return result;
		}

		public static string UniqueID(DateTime? time = null)
		{
			if (time == null)
				time = DateTime.UtcNow;

			try {
				var num = ((DateTime) time).ToString("yyyyMMddHHmmssfff");
				return Encode(Int64.Parse(num));
			}

			catch (Exception ex) {
				throw new Exception(String.Format("Exception thrown in Base36.UniqueID(DateTime? time){2}{0}{2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine));
			}
		}

		public static string UniqueID(string ip)
		{
			if (String.IsNullOrEmpty(ip))
				throw new ArgumentNullException("ip", "<ip> is required");
			if (ip.Length < 7 || 16 < ip.Length)
				throw new ArgumentOutOfRangeException("ip", ip, "<ip> must be a value between 0.0.0.0 and 255.255.255.255");

			try {
				var sb = new StringBuilder();
				ip.Split('.').ToList().ForEach(u => sb.Append(u.ToString().PadLeft(3, '0')));
				sb.Replace(".", "");

				return Encode(Int64.Parse(sb.ToString()));
			}

			catch (Exception ex) {
				throw new Exception(String.Format("Exception thrown in Base36.UniqueID(string ip={3}){2}{0}{2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine, ip));
			}
		}
	}
}