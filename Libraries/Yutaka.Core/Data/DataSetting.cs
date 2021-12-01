using System;
using System.Collections.Generic;

namespace Yutaka.Core.Data
{
	/// <summary>
	/// Represents a <see cref="DataSetting"/> (connection string information).
	/// </summary>
	public partial class DataSetting
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataSetting"/> class.
		/// </summary>
		public DataSetting()
		{
			RawDataSetting = new Dictionary<string, string>();
		}

		/// <summary>
		/// Gets or sets the Data Provider.
		/// </summary>
		public string DataProvider { get; set; }

		/// <summary>
		/// Gets or sets the Connection String.
		/// </summary>
		public string DataConnectionString { get; set; }

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
			return !String.IsNullOrEmpty(DataProvider) && !String.IsNullOrEmpty(DataConnectionString);
		}
	}
}