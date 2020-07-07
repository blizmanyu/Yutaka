using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Yutaka.Core.CSharp
{
	/// <summary>
	/// WIP: Do NOT use yet!
	/// </summary>
	public class Class
	{
		#region Fields
		protected static readonly Regex Tab = new Regex("\t", RegexOptions.Compiled);
		protected static string CurrentIndentation = "";
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

		#region Non-Public Methods
		protected void DecreaseIndent()
		{
			if (String.IsNullOrEmpty(CurrentIndentation))
				CurrentIndentation = "";
			else
				CurrentIndentation = Tab.Replace(CurrentIndentation, "", 1);
		}

		protected void IncreaseIndent()
		{
			CurrentIndentation = String.Format("{0}\t", CurrentIndentation);
		}
		#endregion Non-Public Methods

		#region Public Methods
		/// <summary>
		/// WIP: Do NOT use yet!
		/// </summary>
		public override string ToString()
		{
			var sb = new StringBuilder();
			CurrentIndentation = "";

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
				IncreaseIndent();
			}

			#region Class
			sb.AppendFormat("{0}{1}{2}class {3}{4}{5}",
				CurrentIndentation, // {0}
				String.IsNullOrWhiteSpace(AccessLevel) ? "" : String.Format("{0} ", AccessLevel), // {1}
				String.IsNullOrWhiteSpace(Modifier) ? "" : String.Format("{0} ", Modifier), // {2}
				Name, // {3}
				String.IsNullOrWhiteSpace(BaseClass) ? "" : String.Format(" : {0}", BaseClass), // {4}
				Environment.NewLine // {5}
				);

			sb.AppendFormat("{0}{{{1}", CurrentIndentation, Environment.NewLine);
			IncreaseIndent();

			#region Constructor
			sb.AppendFormat("{0}public {2}() {{ }}{1}", CurrentIndentation, Environment.NewLine, Name);
			sb.AppendLine();
			#endregion Constructor

			#region Fields
			if (Fields != null && Fields.Count > 0) {
				var lastField = Fields.Last();

				foreach (var field in Fields) {
					sb.AppendFormat("{0}{2}{1}", CurrentIndentation, Environment.NewLine, field);
					if (!field.Equals(lastField))
						sb.AppendLine();
				}
			}
			#endregion Fields

			#region Methods
			if (Methods != null && Methods.Count > 0) {
				var lastMethod = Methods.Last();
				sb.AppendLine();

				foreach (var method in Methods) {
					sb.AppendFormat("{0}{2}{1}", CurrentIndentation, Environment.NewLine, method);
					if (!method.Equals(lastMethod))
						sb.AppendLine();
				}
			}
			#endregion Methods

			DecreaseIndent();
			sb.AppendFormat("{0}}}{1}", CurrentIndentation, Environment.NewLine);
			#endregion Class

			if (!String.IsNullOrWhiteSpace(Namespace))
				sb.AppendFormat("}}{0}", Environment.NewLine);
			#endregion Namespace

			return sb.ToString();
		}
		#endregion Public Methods
	}
}