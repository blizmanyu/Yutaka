using System;
using System.Linq;
using System.Text;
using Yutaka.Utils;

namespace Yutaka.Text
{
	public static class TextUtil
	{
		public static string ConvertIpToBase36(string ip, bool lowerCase=true)
		{
			if (ip == null)
				Console.Write("\n[{0}||Error] ip can't be NULL", DateTime.Now.ToString("HH:mm:ss.fff"));
			if (ip.Length < 7 || ip.Length > 16)
				Console.Write("\n[{0}||Error] ip: {1} - Enter a value between 0.0.0.0 and 255.255.255.255", DateTime.Now.ToString("HH:mm:ss.fff"), ip);

			var sb = new StringBuilder();
			ip.Split('.').ToList().ForEach(u => sb.Append(u.ToString().PadLeft(3, '0')));
			sb.Replace(".","");

			Int64 result;
			if (Int64.TryParse(sb.ToString(), out result))
				return Base36.Encode(result, lowerCase);

			return ip;
		}

		public static Int64 ConvertIpToInt64(string ip)
		{
			if (ip == null)
				Console.Write("\n[{0}||Error] ip can't be NULL", DateTime.Now.ToString("HH:mm:ss.fff"));
			if (ip.Length < 7 || ip.Length > 16)
				Console.Write("\n[{0}||Error] ip: {1} - Enter a value between 0.0.0.0 and 255.255.255.255", DateTime.Now.ToString("HH:mm:ss.fff"), ip);

			var sb = new StringBuilder();
			ip.Split('.').ToList().ForEach(u => sb.Append(u.ToString().PadLeft(3, '0')));
			sb.Replace(".", "");

			Int64 result;
			if (Int64.TryParse(sb.ToString(), out result))
				return result;

			return -1;
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

		public static Int64 ConvertTimeToInt64(DateTime? time=null)
		{
			if (time == null)
				time = DateTime.Now;

			Int64 result;
			if (Int64.TryParse(((DateTime) time).ToString("yyyyMMddHHmmssfff"), out result))
				return result;

			return -1;
		}
	}
}