using System;

namespace Yutaka.Core.Domain.Security
{
	public static class SecurityUtil
	{
		/// <summary>
		/// Base36 encodes UtcNow and returns it as a string.
		/// </summary>
		/// <returns></returns>
		public static string Encode()
		{
			return Encode(DateTime.UtcNow);
		}

		/// <summary>
		/// Base36 encodes UtcNow, appends the userId, and returns it as a string.
		/// </summary>
		/// <param name="userId">The user ID to append.</param>
		/// <returns></returns>
		public static string Encode(int userId)
		{
			return String.Format("{0}_{1}", Encode(DateTime.UtcNow), userId);
		}

		/// <summary>
		/// Base36 encodes the given <see cref="DateTime"/> and returns it as a string.
		/// </summary>
		/// <param name="dt">The <see cref="DateTime"/> to use.</param>
		/// <returns></returns>
		public static string Encode(DateTime dt)
		{
			return Base36.Encode(long.Parse(dt.ToString("yyyyMMddHHmmssfff")));
		}

		/// <summary>
		/// Base36 encodes the given <see cref="DateTime"/>, appends the userId, and returns it as a string.
		/// </summary>
		/// <param name="dt">The <see cref="DateTime"/> to use.</param>
		/// <param name="userId">The user ID to append.</param>
		/// <returns></returns>
		public static string Encode(DateTime dt, int userId)
		{
			return String.Format("{0}_{1}", Encode(dt), userId);
		}
	}
}