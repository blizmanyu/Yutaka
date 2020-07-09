using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Yutaka.Core.CSharp
{
	public class Field
	{
		#region Fields
		protected static readonly Regex Tab = new Regex("\t", RegexOptions.Compiled);
		public string CurrentIndentation = "";
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
				sb.AppendFormat("{0}[DisplayName(\"{2}\")]{1}", CurrentIndentation, Environment.NewLine, DisplayName);
			if (!String.IsNullOrWhiteSpace(UIHint))
				sb.AppendFormat("{0}[UIHint(\"{2}\")]{1}", CurrentIndentation, Environment.NewLine, UIHint);

			sb.AppendFormat("{0}{1}{2}{3}{4}",
				CurrentIndentation, // {0}
				String.IsNullOrWhiteSpace(AccessLevel) ? "" : String.Format("{0} ", AccessLevel), // {1}
				String.IsNullOrWhiteSpace(Modifier) ? "" : String.Format("{0} ", Modifier), // {2}
				String.IsNullOrWhiteSpace(Type) ? "" : String.Format("{0} ", Type), // {3}
				Name // {4}
				);

			if (IsAutoImplemented)
				sb.Append(" { get; set; }");
			else {
				if (String.IsNullOrWhiteSpace(Getter)) {
					if (String.IsNullOrWhiteSpace(Setter))
						sb.Append(";");
					else {
						sb.AppendLine();
						sb.AppendFormat("{0}{{{1}", CurrentIndentation, Environment.NewLine);
						IncreaseIndent();
						sb.AppendFormat("{0}set {{{1}", CurrentIndentation, Environment.NewLine);
						sb.AppendLine(Setter);
						sb.AppendFormat("{0}}}{1}", CurrentIndentation, Environment.NewLine);
						DecreaseIndent();
						sb.AppendFormat("{0}}}{1}", CurrentIndentation, Environment.NewLine);
					}
				}

				else {
					sb.AppendLine();
					sb.AppendFormat("{0}{{{1}", CurrentIndentation, Environment.NewLine);
					IncreaseIndent();
					sb.AppendFormat("{0}get {{{1}", CurrentIndentation, Environment.NewLine);
					sb.AppendLine(Getter);
					sb.AppendFormat("{0}}}{1}", CurrentIndentation, Environment.NewLine);
					DecreaseIndent();

					if (!String.IsNullOrWhiteSpace(Setter)) {
						sb.AppendLine();
						sb.AppendFormat("{0}set {{{1}", CurrentIndentation, Environment.NewLine);
						sb.AppendLine(Setter);
						sb.AppendFormat("{0}}}{1}", CurrentIndentation, Environment.NewLine);
					}

					sb.AppendFormat("{0}}}{1}", CurrentIndentation, Environment.NewLine);
				}
			}

			return sb.ToString();
		}
	}
}