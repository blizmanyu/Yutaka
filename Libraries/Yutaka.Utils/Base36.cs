using System;
using System.Collections.Generic;
using System.Linq;

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
	}
}