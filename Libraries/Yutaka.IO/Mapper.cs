using System;
using System.Linq;
using System.Text;

namespace Yutaka.IO
{
	public static class Mapper
	{
		public static string Map<TDest>(string sourceType)
		{
			var destType = typeof(TDest);
			var destTypeName = destType.Name;
			var sb = new StringBuilder();
			sb.Append("\n");
			sb.Append(String.Format("\t\tprivate static {0} To{0}({1} x)", destTypeName, sourceType));
			sb.Append("\n\t\t{");
			sb.Append("\n\t\t\tif (x == null)");
			sb.Append("\n\t\t\t\treturn null;");
			sb.Append("\n");
			sb.Append(String.Format("\n\t\t\treturn new {0} {{", destTypeName));

			foreach (var p in destType.GetProperties().ToDictionary(p => p.Name, p => p))
				sb.Append(String.Format("\n\t\t\t\t{0} = x.{0} ?? null,", p.Key));

			sb.Append("\n\t\t\t};");
			sb.Append("\n\t\t}");
			sb.Append("\n");

			return sb.ToString();
		}
	}
}