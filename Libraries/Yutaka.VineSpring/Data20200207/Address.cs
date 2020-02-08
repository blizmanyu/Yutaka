using System;

namespace Yutaka.VineSpring.Data20200207
{
	public class Address
	{
		public DateTime? CreatedOn { get; set; }
		public string Id { get; set; }
		public bool? IsInternational { get; set; }
		public string Name { get; set; }
		public string Organization { get; set; }
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
		public string Phone { get; set; }
		public string CustomerId { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; }
	}
}