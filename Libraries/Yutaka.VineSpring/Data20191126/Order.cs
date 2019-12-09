using System;

namespace Yutaka.VineSpring.Data20191126
{
	public class Order
	{
		public DateTime CreatedOn { get; set; }
		public string Id { get; set; }
		public bool? IsCommitted { get; set; }
		public bool? IsCompliant { get; set; }
		public DateTime? ShipDate { get; set; }
		public decimal? CalculatedShipping { get; set; }
		public decimal? FreightTax { get; set; }
		public decimal? Shipping { get; set; }
		public decimal? Subtotal { get; set; }
		public decimal? Tax { get; set; }
		public decimal? Total { get; set; }
		public string AccountId { get; set; }
		public string CampaignId { get; set; }
		public string ComplianceDetail { get; set; }
		public string CustomerId { get; set; }
		public string FulfillmentDetail { get; set; }
		public string FulfillmentHouse { get; set; }
		public string FullName { get; set; }
		public string OrderNumber { get; set; }
		public string SalesRep { get; set; }
		public string ShipCompliantStatus { get; set; }
		public string Source { get; set; }
		public string Status { get; set; }
		public string TrackingNumber { get; set; }
		public string Type { get; set; }
		public string[] Tags { get; set; }
		public Customer Customer { get; set; }
		public Discount[] Discounts { get; set; }
		public OrderItem[] Items { get; set; }
		public Note[] Notes { get; set; }
		public ShippingAddress ShippingAddress { get; set; }
		public ShippingMethod ShippingMethod { get; set; }
	}

	public class OrderItem
	{
		public string OrderId { get; set; }
		public string ProductId { get; set; }
		public int? Quantity { get; set; }
		public decimal? Discount { get; set; }
		public decimal? Price { get; set; }
		public decimal? Subtotal { get; set; }
		public decimal? Total { get; set; }
		public string Name { get; set; }
		public string ShipCompliantBrandKey { get; set; }
		public string ShipCompliantProductKey { get; set; }
		public string Sku { get; set; }
	}

	public class Note
	{
		public DateTime CreatedOn { get; set; }
		public string Email { get; set; }
		public string OrderId { get; set; }
		public string Message { get; set; }
	}

	public class Discount
	{
		public string Id { get; set; }
		public string OrderId { get; set; }
		public decimal? Benefit { get; set; }
		public string Type { get; set; }
		public string Name { get; set; }
	}
}