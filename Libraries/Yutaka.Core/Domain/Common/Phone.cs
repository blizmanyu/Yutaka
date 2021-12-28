﻿using System;
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

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Phone"/> class.
		/// </summary>
		public Phone()
		{
			Label = null;
			Number = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phone"/> class. Specifying the phone number.
		/// </summary>
		/// <param name="number">The phone number.</param>
		public Phone(string number)
		{
			if (String.IsNullOrWhiteSpace(number))
				throw new Exception("'number' is required.");

			Label = null;
			Number = number;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phone"/> class. Specifying the phone number and label.
		/// </summary>
		/// <param name="number">The phone number.</param>
		/// <param name="label">The label of the phone number.</param>
		public Phone(string number, string label)
		{
			if (String.IsNullOrWhiteSpace(number))
				throw new Exception("'number' is required.");

			Label = label;
			Number = number;
		}
		#endregion Constructors
	}
}