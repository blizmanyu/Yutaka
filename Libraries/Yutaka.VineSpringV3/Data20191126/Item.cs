namespace Yutaka.VineSpringV3.Data20191126
{
	public class Item
	{
		public string ProductId { get; set; }
		public string ShipCompliantBrandKey { get; set; }
		public decimal Total { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal Subtotal { get; set; }
		public string Name { get; set; }
		public decimal? Discount { get; set; }
		public string ShipCompliantProductKey { get; set; }
		public string Sku { get; set; }
	}
}