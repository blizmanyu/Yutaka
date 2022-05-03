using System;

namespace Yutaka.Core
{
	public static class Extensions
	{
		/// <summary>
		/// Fast Contains method using <see cref="StringComparison.OrdinalIgnoreCase"/>.
		/// </summary>
		/// <param name="source">This <see cref="string"/>.</param>
		/// <param name="toCheck">The <see cref="string"/> to look for.</param>
		/// <returns></returns>
		public static bool FastContainsIgnoreCase(this string source, string toCheck)
		{
			if (source == null)
				return false;

			return source.FastContains(toCheck, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Fast Contains method with <see cref="StringComparison"/>.
		/// </summary>
		/// <param name="source">This <see cref="string"/>.</param>
		/// <param name="toCheck">The <see cref="string"/> to look for.</param>
		/// <param name="comp">The <see cref="StringComparison"/> to use.</param>
		/// <returns></returns>
		public static bool FastContains(this string source, string toCheck, StringComparison comp)
		{
			if (source == null)
				return false;

			return source.IndexOf(toCheck, comp) > -1;
		}
	}
}