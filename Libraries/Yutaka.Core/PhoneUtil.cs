using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka
{
	public static class PhoneUtil
	{
		/// <summary>
		/// Removes all non-alphanumerics from a phone number except for '+'.
		/// </summary>
		/// <param name="phone"></param>
		/// <returns></returns>
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