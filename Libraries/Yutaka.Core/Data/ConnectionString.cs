using System;
using System.Collections.Generic;

namespace Yutaka.Core.Data
{
	/// <summary>
	/// Represents a <see cref="ConnectionString"/>.
	/// </summary>
	public partial class ConnectionString
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConnectionString"/> class.
		/// </summary>
		public ConnectionString()
		{
			RawDataSetting = new Dictionary<string, string>();
		}

		/// <summary>
		/// Gets or sets the Name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the Connection String.
		/// </summary>
		public string ConnString { get; set; }

		/// <summary>
		/// Gets the raw DataSetting.
		/// </summary>
		public IDictionary<string, string> RawDataSetting { get; private set; }

		/// <summary>
		/// Indicates whether the entered information is valid.
		/// </summary>
		/// <returns></returns>
		public bool IsValid()
		{
			return !String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(ConnString);
		}
	}
}