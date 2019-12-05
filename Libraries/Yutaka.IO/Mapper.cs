using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Yutaka.IO
{
	public static class Mapper
	{
		public static void Map<TDest>(string sourceType)
		{
			var destType = typeof(TDest);
			var sb = new StringBuilder();
			sb.Append("\n");
			sb.Append(String.Format("\n\t\tprivate static {0} Map({1} x)", destType, sourceType));
			sb.Append("\n\t\t{");
			sb.Append(String.Format("\n\t\t\treturn new {0} {{", destType));

			foreach (var p in destType.GetProperties().ToDictionary(p => p.Name, p => p))
				sb.Append(String.Format("\n\t\t\t\t{0} = x.{0},", p.Key));

			sb.Append("\n\t\t\t};");
			sb.Append("\n\t\t}");
			sb.Append("\n");

			var filename = String.Format("{0:yyyy MMdd HHmm ssff}.cs", DateTime.Now);
			new FileUtil().Write(sb, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), filename));
		}
	}
}