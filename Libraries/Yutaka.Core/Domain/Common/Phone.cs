using System;

namespace Yutaka.Core.Domain.Common
{
	public class Phone
	{
		#region Fields
		public string Label;
		public string Number;

		/// <summary>
		/// Gets the minified phone number.
		/// </summary>
		public string NumberMinified
		{
			get {
				if (String.IsNullOrWhiteSpace(Number))
					return "";

				return PhoneUtil.Minify(Number);
			}
		}

		/// <summary>
		/// Gets the "pretty" formatted phone number.
		/// </summary>
		public string NumberPretty
		{
			get {
				if (String.IsNullOrWhiteSpace(Number))
					return "";

				return PhoneUtil.Beautify(Number);
			}
		}
		#endregion Fields

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Phone"/> class.
		/// </summary>
		public Phone()
		{
			Label = "";
			Number = "";
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phone"/> class. Specifying the phone number.
		/// </summary>
		/// <param name="number">The phone number.</param>
		public Phone(string number)
		{
			if (String.IsNullOrWhiteSpace(number))
				throw new Exception("'number' is required.");

			Label = "";
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
			if (String.IsNullOrWhiteSpace(label))
				throw new Exception("'label' is required.");

			Label = label;
			Number = number;
		}
		#endregion Constructors

		#region Methods
		public override bool Equals(Object obj)
		{
			return obj is Phone && this == (Phone) obj;
		}

		public override int GetHashCode()
		{
			return NumberMinified.GetHashCode();
		}

		public static bool operator ==(Phone x, Phone y)
		{
			return x.NumberMinified == y.NumberMinified;
		}

		public static bool operator !=(Phone x, Phone y)
		{
			return !(x == y);
		}

		public override string ToString()
		{
			return NumberPretty;
		}
		#endregion Methods
	}
}