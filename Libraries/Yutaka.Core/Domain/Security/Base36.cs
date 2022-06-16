using System;
using System.Collections.Generic;
using System.Linq;

namespace Yutaka.Core.Domain.Security
{
	public static class Base36
	{
		const string CharList = "0123456789abcdefghijklmnopqrstuvwxyz";

		public static long Decode(string input)
		{
			var pos = 0;
			long result = 0;
			var reversed = input.Reverse();

			foreach (char c in reversed) {
				result += CharList.IndexOf(c) * (long) Math.Pow(36, pos);
				pos++;
			}

			return result;
		}

		public static string Encode(long input)
		{
			if (input < 0)
				throw new ArgumentOutOfRangeException("input", input, "input cannot be negative");
			if (input == 0)
				return "0";

			var result = new Stack<char>();
			char[] clistarr = CharList.ToCharArray();

			while (input != 0) {
				result.Push(clistarr[input % 36]);
				input /= 36;
			}

			return new string(result.ToArray());
		}
	}
}