using System;

namespace Yutaka.VineSpring.Data20191126
{
	public class AlternateAddress : Address { }
	public class BillingAddress : Address { }
	public class ShippingAddress : Address { }

	public class Address
	{
		public string Id;
		public string City;
		public string Country;
		public DateTime CreatedOn;
		public string CustomerId;
		public bool? IsInternational;
		public string Line1;
		public string Line2;
		public string Name;
		public string PostalCode;
		public string State;
		public string UpdatedBy;
		public DateTime UpdatedOn;
	}
}