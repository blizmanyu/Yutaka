using System;
using System.Collections.Generic;
using System.Text;

namespace Yutaka.Core.CSharp
{
	/// <summary>
	/// WIP: Do NOT use yet!
	/// </summary>
	public class Class
	{
		#region Fields
		public List<Field> Fields;
		public List<Method> Methods;
		public List<string> Usings;
		public string AccessLevel;
		public string BaseClass;
		public string Modifier;
		public string Name;
		public string Namespace;
		#endregion Fields

		/// <summary>
		/// WIP: Do NOT use yet!
		/// </summary>
		public Class()
		{
			Fields = new List<Field>();
			Methods = new List<Method>();
			Usings = new List<string>();
		}

		/// <summary>
		/// WIP: Do NOT use yet!
		/// </summary>
		public override string ToString()
		{
			var sb = new StringBuilder();

			#region Usings
			if (Usings != null && Usings.Count > 0) {
				foreach (var u in Usings)
					sb.AppendLine(String.Format("using {0};", u));

				sb.AppendLine();
			}
			#endregion Usings

			#region Namespace
			if (!String.IsNullOrWhiteSpace(Namespace)) {
				sb.AppendLine(String.Format("namespace {0}", Namespace));
				sb.AppendLine("{");
				sb.Append("\t");
			}

			#region Class
			if (!String.IsNullOrWhiteSpace(AccessLevel))
				sb.Append(String.Format("{0} ", AccessLevel));
			if (!String.IsNullOrWhiteSpace(Modifier))
				sb.Append(String.Format("{0} ", Modifier));

			sb.Append(String.Format("class {0}", Name));

			if (!String.IsNullOrWhiteSpace(BaseClass))
				sb.Append(String.Format(" : {0}", BaseClass));

			sb.AppendLine();
			sb.AppendLine("{");
			#endregion Class





			if (!String.IsNullOrWhiteSpace(Namespace))
				sb.AppendLine("}");
			#endregion Namespace

			return sb.ToString();
		}
	}
}