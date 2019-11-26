namespace Yutaka.VineSpring.Data20191126
{
	public class Item
	{
		public decimal Total { get; set; }
		public int Quantity { get; set; }
		public string ProductId { get; set; }
		public decimal Price { get; set; }
		public decimal Subtotal { get; set; }
		public bool IsTaxExempt { get; set; }
		public string Name { get; set; }
		public decimal Discount { get; set; }
		public string Sku { get; set; }
	}
}