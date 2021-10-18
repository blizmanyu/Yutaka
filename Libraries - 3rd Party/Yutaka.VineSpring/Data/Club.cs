using System;

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

		public void DumpToConsole()
		{
			Console.Write("\n");
			Console.Write("\nAllowCustomerHold: {0}", AllowCustomerHold);
			Console.Write("\nMaxHoldDuration: {0}", MaxHoldDuration);
			Console.Write("\nAccountId: {0}", AccountId);
			Console.Write("\nId: {0}", Id);
			Console.Write("\nMaxHoldPeriod: {0}", MaxHoldPeriod);
			Console.Write("\nName: {0}", Name);
			Console.Write("\nSignupDetails: {0}", SignupDetails);
			Console.Write("\nType: {0}", Type);
			Console.Write("\nDiscounts: {0}", String.Join(", ", Discounts));
			Console.Write("\n");
		}
	}

	public class Amount { }

	public class Quantity { }

	public class Shipping
	{
		public string ConfigId { get; set; }
	}

	public class Taxrate { }
}