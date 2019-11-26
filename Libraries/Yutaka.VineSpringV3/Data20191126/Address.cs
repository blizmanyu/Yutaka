using System;

namespace Yutaka.VineSpringV3.Data20191126
{
	public class AlternateAddress : Address { }
	public class BillingAddress : Address { }
	public class DefaultAddress : Address { }
	public class ShippingAddress : Address { }

	public class Address
	{
		public string Country { get; set; }
		public string UpdatedBy { get; set; }
		public string City { get; set; }
		public bool? IsInternational { get; set; }
		public string PostalCode { get; set; }
		public string Name { get; set; }
		public string CustomerId { get; set; }
		public string State { get; set; }
		public string Id { get; set; }
		public DateTime UpdatedOn { get; set; }
		public DateTime CreatedOn { get; set; }
		public string Line1 { get; set; }
		public string Phone { get; set; }
		public string Organization { get; set; }
		public string Line2 { get; set; }
	}
}