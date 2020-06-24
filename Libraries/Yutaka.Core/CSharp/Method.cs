using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.Core.CSharp
{
	public class Method
	{
		public string AccessLevel;
		public string Modifier;
		public string ReturnType;
		public string Name;
		public string[] Parameters;

		public Method(string accessLevel = null, string modifier = null, string returnType = null, string name = null, string[] parameters = null)
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
				throw new ArgumentNullException("name", "<name> is required.");
			else
				Name = name.Trim();

			if (parameters == null || parameters.Length == 0)
				Parameters = null;
			else
				Parameters = parameters;
		}
	}
}