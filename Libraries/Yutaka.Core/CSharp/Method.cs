using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.Core.CSharp
{
	public class Method
	{
		private static readonly string Space = " ";
		private static readonly string Tab = "\t";
		public string AccessLevel;
		public string Modifier;
		public string ReturnType;
		public string Name;
		public string[] Parameters;
		public string Body;

		public Method(string accessLevel = null, string modifier = null, string returnType = null, string name = null, string[] parameters = null, string body = null)
		{
			if (String.IsNullOrWhiteSpace(accessLevel))
				AccessLevel = "public";
			else
				AccessLevel = accessLevel.Trim().ToLower();

			if (String.IsNullOrWhiteSpace(modifier))
				Modifier = null;
			else
				Modifier = modifier.Trim().ToLower();

			if (String.IsNullOrWhiteSpace(returnType))
				ReturnType = "void";
			else
				ReturnType = returnType.Trim().ToLower();

			if (String.IsNullOrWhiteSpace(name))
				Name = null;
			else
				Name = name.Trim();

			if (parameters == null || parameters.Length == 0)
				Parameters = null;
			else
				Parameters = parameters;

			if (String.IsNullOrWhiteSpace(body))
				Body = null;
			else
				Body = body.Trim();
		}
	}
}