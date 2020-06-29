using System;
using System.Collections.Generic;
using System.Text;

namespace Yutaka.Core.CSharp
{
	public class Region
	{
		public List<Method> Methods;
		public string Name;

		public Region(string name = null)
		{
			if (String.IsNullOrWhiteSpace(name))
				Name = null;
			else
				Name = name.Trim();

			Methods = new List<Method>();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			if (String.IsNullOrWhiteSpace(Name))
				sb.AppendLine("\t\t#region");
			else
				sb.AppendLine(String.Format("\t\t#region {0}", Name));

			if (Methods == null || Methods.Count < 1)
				sb.AppendLine("\t\t// No methods //");
			else {
				for (int i = 0; i < Methods.Count; i++) {
					sb.AppendLine(Methods[i].ToString());

					if (i < Methods.Count - 1)
						sb.AppendLine();
				}
			}

			if (String.IsNullOrWhiteSpace(Name))
				sb.AppendLine("\t\t#endregion");
			else
				sb.AppendLine(String.Format("\t\t#endregion {0}", Name));

			return sb.ToString();
		}
	}
}