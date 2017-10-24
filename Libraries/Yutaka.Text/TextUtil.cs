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

			return null;
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
	}
}