namespace Yutaka.VineSpring.Data20191126
{
	public class ShippingMethod
	{
		public string Name { get; set; }
		public string Id { get; set; }
		public bool? RequireShippingAddress { get; set; }
		public AlternateAddress AlternateAddress { get; set; }
		public string CarrierCode { get; set; }
	}

	public class AlternateAddress
	{
		public string ShippingMethodId { get; set; }
		public bool? IsInternational { get; set; }
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string PostalCode { get; set; }
	}
}