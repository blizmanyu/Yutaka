using System;
using System.Collections.Generic;
using System.Linq;

namespace Yutaka.Utils
{
	public static class Base36
	{
		const string CharList = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		public static string Encode(long input)
		{
			if (input < 0)
				throw new ArgumentOutOfRangeException("input", input, "input cannot be negative");

			char[] clistarr = CharList.ToCharArray();
			var result = new Stack<char>();
			while (input != 0) {
				result.Push(clistarr[input % 36]);
				input /= 36;
			}
			return new string(result.ToArray());
		}

		public static Int64 Decode(string input)
		{
			var reversed = input.ToUpper().Reverse();
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