using System;

namespace Yutaka.Text
{
	public static class CSUtil
	{
		/// <summary>
		/// Generates method for Getting the entity by Id.
		/// </summary>
		/// <param name="table">The name of the table.</param>
		/// <returns></returns>
		public static string GenerateGetById(string table)
		{
			var alias = table.Replace("_", "").Substring(0, 1).ToLower();

			return String.Format(
				"\t\tpublic {0} Get{0}ById(int id){2}" +
				"\t\t{{{2}" +
				"\t\t\treturn (from {1} in dbcontext.{0}s{2}" +
				"\t\t\t\t\twhere id == {1}.Id{2}" +
				"\t\t\t\t\tselect {1}).FirstOrDefault();{2}" +
				"\t\t}}{2}{2}", table, alias, Environment.NewLine);
		}
	}
}