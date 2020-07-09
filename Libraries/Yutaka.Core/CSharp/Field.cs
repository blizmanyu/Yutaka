using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Yutaka.Core.CSharp
{
	public class Field
	{
		#region Fields
		protected static readonly Regex Tab = new Regex("\t", RegexOptions.Compiled);
		protected string CurrentIndentation = "";
		public bool IsAutoImplemented;
		public string AccessLevel;
		public string DisplayName;
		public string Getter;
		public string Modifier;
		public string Name;
		public string Setter;
		public string Type;
		public string UIHint;
		#endregion Fields

		public Field(string accessLevel = null, string modifier = null, string type = null, string name = null, string displayName = null, string uIHint = null, bool isAutoImplemented = true, string getter = null, string setter = null)
		{
			if (String.IsNullOrWhiteSpace(accessLevel))
				AccessLevel = "public";
			else
				AccessLevel = accessLevel.Trim().ToLower();

			if (String.IsNullOrWhiteSpace(modifier))
				Modifier = null;
			else
				Modifier = modifier.Trim().ToLower();

			if (String.IsNullOrWhiteSpace(type))
				Type = null;
			else
				Type = type.Trim();

			if (String.IsNullOrWhiteSpace(name))
				Name = null;
			else
				Name = name.Trim();

			if (String.IsNullOrWhiteSpace(displayName))
				DisplayName = null;
			else
				DisplayName = displayName.Trim();

			if (String.IsNullOrWhiteSpace(uIHint))
				UIHint = null;
			else
				UIHint = uIHint.Trim();

			IsAutoImplemented = isAutoImplemented;

			if (String.IsNullOrWhiteSpace(getter))
				Getter = null;
			else
				Getter = getter.Trim();

			if (String.IsNullOrWhiteSpace(setter))
				Setter = null;
			else
				Setter = setter.Trim();
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

		public override string ToString()
		{
			var sb = new StringBuilder();

			if (!String.IsNullOrWhiteSpace(DisplayName))
				sb.AppendLine(String.Format("\t\t[DisplayName(\"{0}\")]", DisplayName));
			if (!String.IsNullOrWhiteSpace(UIHint))
				sb.AppendLine(String.Format("\t\t[UIHint(\"{0}\")]", UIHint));

			sb.Append("\t\t");

			if (!String.IsNullOrWhiteSpace(AccessLevel))
				sb.Append(String.Format("{0} ", AccessLevel));
			if (!String.IsNullOrWhiteSpace(Modifier))
				sb.Append(String.Format("{0} ", Modifier));
			if (!String.IsNullOrWhiteSpace(Type))
				sb.Append(String.Format("{0} ", Type));
			if (!String.IsNullOrWhiteSpace(Name))
				sb.Append(String.Format("{0}", Name));

			if (IsAutoImplemented)
				sb.AppendLine(" { get; set; }");
			else {
				if (String.IsNullOrWhiteSpace(Getter)) {
					if (String.IsNullOrWhiteSpace(Setter))
						sb.AppendLine(";");
					else {
						sb.AppendLine();
						sb.AppendLine("\t\t{");
						sb.AppendLine("\t\t\tset {");
						sb.AppendLine(Setter);
						sb.AppendLine("\t\t\t}");
						sb.AppendLine("\t\t}");
					}
				}

				else {
					sb.AppendLine();
					sb.AppendLine("\t\t{");
					sb.AppendLine("\t\t\tget {");
					sb.AppendLine(Getter);
					sb.AppendLine("\t\t\t}");

					if (!String.IsNullOrWhiteSpace(Setter)) {
						sb.AppendLine();
						sb.AppendLine("\t\t\tset {");
						sb.AppendLine(Setter);
						sb.AppendLine("\t\t\t}");
					}

					sb.AppendLine("\t\t}");
				}
			}

			return sb.ToString();
		}
	}
}