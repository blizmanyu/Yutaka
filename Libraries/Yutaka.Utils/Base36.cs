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

		public static long Decode(string input)
		{
			var CharList = input.Equals(input.ToUpper()) ? CharListUpper : CharListLower;
			var reversed = input.Reverse();
			long result = 0;
			var pos = 0;
			foreach (char c in reversed) {
				result += CharList.IndexOf(c) * (long) Math.Pow(36, pos);
				pos++;
			}
			return result;
		}

		public static string DecodeIP(string input)
		{
			if (String.IsNullOrWhiteSpace(input))
				return "";

			var decoded = Decode(input);
			var str = decoded.ToString("d12");
			var sb = new StringBuilder();
			sb.Append(int.Parse(str.Substring(0, 3))).Append(".");
			sb.Append(int.Parse(str.Substring(3, 3))).Append(".");
			sb.Append(int.Parse(str.Substring(6, 3))).Append(".");
			sb.Append(int.Parse(str.Substring(9, 3)));
			return sb.ToString();
		}

		public static string DumbDecode(string str)
		{
			if (String.IsNullOrWhiteSpace(str))
				return "";

			var sb = new StringBuilder();

			for (int i = 0; i < str.Length; i++)
				sb.Append((char) (str[i] - 2));

			return sb.ToString();
		}

		public static string DumbEncode(string str)
		{
			if (String.IsNullOrWhiteSpace(str))
				return "";

			var sb = new StringBuilder();

			for (int i = 0; i < str.Length; i++)
				sb.Append((char) (str[i] + 2));

			return sb.ToString();
		}

		public static string EncodeIP(string ip, bool lowerCase = true)
		{
			if (String.IsNullOrWhiteSpace(ip))
				return "";

			var sb = new StringBuilder();
			var split = ip.Split('.');

			for (int i = 0; i < split.Length; i++)
				sb.Append(split[i].PadLeft(3, '0'));

			return Encode(long.Parse(sb.ToString()), lowerCase);
		}

		#region Deprecated
		/// <summary>
		/// Deprecated on Jun 15, 2022. Use Yutaka.Core.Domain.Security.Base36.Encode(long input) instead.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="lowerCase"></param>
		/// <returns></returns>
		[Obsolete("Deprecated on Jun 15, 2022. Use Yutaka.Core.Domain.Security.Base36.Encode(long input) instead.", false)]
		public static string Encode(long input, bool lowerCase = true)
		{
			if (input < 0)
				throw new ArgumentOutOfRangeException("input", input, "input cannot be negative");
			if (input == 0)
				return "0";

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

		/// <summary>
		/// Deprecated on Jun 15, 2022. Use Yutaka.Core.Domain.Security.SecurityUtil.Encode(DateTime dt) instead.
		/// </summary>
		/// <param name="dt">The <see cref="DateTime"/> to use.</param>
		/// <param name="lowerCase"></param>
		/// <returns></returns>
		[Obsolete("Deprecated on Jun 15, 2022. Use Yutaka.Core.Domain.Security.SecurityUtil.Encode(DateTime dt) instead.", true)]
		public static string Encode(DateTime? dt = null, bool lowerCase = true)
		{
			if (dt == null)
				dt = DateTime.UtcNow;

			return Encode(long.Parse(dt.Value.ToString("yyyyMMddHHmmssfff")), lowerCase);
		}

		[Obsolete("Deprecated on Sep 30, 2019. Use Encode(DateTime? dt = null, bool lowerCase = true) instead.")]
		public static string GetUniqueId(DateTime? time = null)
		{
			if (time == null)
				time = DateTime.UtcNow;

			try {
				var num = ((DateTime) time).ToString("yyyyMMddHHmmssfff");
				return Encode(Int64.Parse(num));
			}

			catch (Exception ex) {
				throw new Exception(String.Format("Exception thrown in Base36.GetUniqueId(DateTime? time){2}{0}{2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine));
			}
		}

		[Obsolete("Deprecated on Mar 25, 2019. Use WebUtil.EncodeIp(string ipAddress) instead.")]
		public static string EncodeIp(string ipAddress)
		{
			if (String.IsNullOrWhiteSpace(ipAddress))
				throw new Exception(String.Format("Exception thrown in Base36.EncodeIp(string ipAddress){0}<ipAddress> is {1}", Environment.NewLine, ipAddress == null ? "NULL" : "Empty"));
			if (ipAddress.Length < 7 || 16 < ipAddress.Length)
				throw new Exception(String.Format("Exception thrown in Base36.EncodeIp(string ipAddress='{1}'){0}Only IPv4 address between 0.0.0.0 and 255.255.255.255 are allowed", Environment.NewLine, ipAddress));

			try {
				var sb = new StringBuilder();
				ipAddress.Split('.').ToList().ForEach(u => sb.Append(u.ToString().PadLeft(3, '0')));

				return Encode(Int64.Parse(sb.ToString()));
			}

			catch (Exception ex) {
				throw new Exception(String.Format("Exception thrown in Base36.EncodeIp(string ipAddress='{3}'){2}{0}{2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine, ipAddress));
			}
		}

		[Obsolete("Deprecated on Nov 16, 2018. Use EncodeIp(string ipAddress) instead.")]
		public static string GetUniqueIdByIP(string ipAddress)
		{
			if (String.IsNullOrWhiteSpace(ipAddress))
				throw new Exception(String.Format("Exception thrown in Base36.GetUniqueIdByIP(string ipAddress){0}<ipAddress> is {1}", Environment.NewLine, ipAddress == null ? "NULL" : "Empty"));

			return EncodeIp(ipAddress);
		}

		[Obsolete("Deprecated on Jun 28, 2018. Use GetUniqueId(DateTime? time = null) instead.")]
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

		[Obsolete("Deprecated on Jun 28, 2018. Use EncodeIp(string ipAddress) instead.")]
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
		#endregion Deprecated
	}
}