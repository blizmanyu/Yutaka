using System;
using System.Collections.Generic;
using System.Text;

namespace Yutaka.Core.Domain.Common
{
	public class Phone
	{
		#region Fields
		protected string AreaCode;
		protected string BaseNumber;
		protected string Extension;

		public string Label;
		public string Number;
		/// <summary>
		/// Gets the minified phone number.
		/// </summary>
		public string NumberMinified { get; }
		/// <summary>
		/// Gets the "pretty" formatted phone number.
		/// </summary>
		public string NumberPretty { get; }
		#endregion Fields
	}
}