using System;

namespace Yutaka.VineSpring.Data20191126
{
	public class Order
	{
		public string Status { get; set; }
		public decimal Tax { get; set; }
		public string SalesRep { get; set; }
		public string FulfillmentHouse { get; set; }
		public decimal Shipping { get; set; }
		public ShippingMethod ShippingMethod { get; set; }
		public Note[] Notes { get; set; }
		public string Id { get; set; }
		public bool IsCommitted { get; set; }
		public DateTime? ShipDate { get; set; }
		public bool IsCompliant { get; set; }
		public string AccountId { get; set; }
		public string ShipCompliantStatus { get; set; }
		public decimal Total { get; set; }
		public string Source { get; set; }
		public string FullName { get; set; }
		public Item[] Items { get; set; }
		public decimal FreightTax { get; set; }
		public string OrderNumber { get; set; }
		public decimal Subtotal { get; set; }
		public ShippingAddress ShippingAddress { get; set; }
		public DateTime CreatedOn { get; set; }
		public decimal CalculatedShipping { get; set; }
		public Customer Customer { get; set; }
		public Discount[] Discounts { get; set; }
		public string CustomerId { get; set; }
		public string Type { get; set; }
		public string FulfillmentDetail { get; set; }
		public string TrackingNumber { get; set; }
		public string[] Tags { get; set; }
		public string CampaignId { get; set; }
		public string ComplianceDetail { get; set; }
	}
}