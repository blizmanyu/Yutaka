namespace Yutaka.VineSpringV3.Data20191126
{
	public class ShippingMethod
	{
		public string Name { get; set; }
		public string Id { get; set; }
		public bool RequireShippingAddress { get; set; }
		public AlternateAddress AlternateAddress { get; set; }
		public string CarrierCode { get; set; }
	}
}