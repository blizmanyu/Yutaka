using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yutaka.Utils;

namespace Yutaka.Text
{
	public static class TextUtil
	{
		public static string ConvertIpToBase36(string ip, bool lowerCase=true)
		{
			if (ip == null)
				throw new ArgumentNullException(ip);
			if (ip.Length < 7 || ip.Length > 16)
				throw new ArgumentOutOfRangeException(ip, ip, "Enter a value between 0.0.0.0 and 255.255.255.255");

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
				throw new ArgumentNullException(ip);
			if (ip.Length < 7 || ip.Length > 16)
				throw new ArgumentOutOfRangeException(ip, ip, "Enter a value between 0.0.0.0 and 255.255.255.255");

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