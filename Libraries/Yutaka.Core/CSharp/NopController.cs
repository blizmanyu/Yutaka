using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yutaka.Core.CSharp
{
	public class NopController : Class
	{
		#region Fields
		#endregion Fields

		public NopController()
		{
			Fields = new List<Field>();
			Methods = new List<Method>();
			Usings = new List<string>();
		}

		#region Public Methods
		public override string ToString()
		{
			var sb = new StringBuilder();
			CurrentIndentation = "";

			#region Usings
			if (Fields.Any(x => x.Type.StartsWith("DateTime"))) {
				if (!Usings.Contains("System"))
					Usings.Add("System");
			}

			if (Fields.Any(x => !String.IsNullOrWhiteSpace(x.DisplayName))) {
				if (!Usings.Contains("System.ComponentModel"))
					Usings.Add("System.ComponentModel");
			}

			if (Fields.Any(x => !String.IsNullOrWhiteSpace(x.UIHint))) {
				if (!Usings.Contains("System.ComponentModel.DataAnnotations"))
					Usings.Add("System.ComponentModel.DataAnnotations");
			}

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
					field.CurrentIndentation = CurrentIndentation;
					sb.AppendLine(field.ToString());
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