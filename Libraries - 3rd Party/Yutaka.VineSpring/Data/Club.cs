namespace Yutaka.VineSpring.Data
{
	public class Club
	{
		public bool AllowCustomerHold { get; set; }
		public int MaxHoldDuration { get; set; }
		public string AccountId { get; set; }
		public string Id { get; set; }
		public string MaxHoldPeriod { get; set; }
		public string Name { get; set; }
		public string SignupDetails { get; set; }
		public string Type { get; set; }
		public string[] Discounts { get; set; }
		public Amount Amount { get; set; }
		public Quantity Quantity { get; set; }
		public Shipping Shipping { get; set; }
		public Taxrate TaxRate { get; set; }
	}

	public class Amount { }

	public class Quantity { }

	public class Shipping
	{
		public string ConfigId { get; set; }
	}

	public class Taxrate { }
}