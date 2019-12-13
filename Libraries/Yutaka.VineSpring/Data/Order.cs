using System;

namespace Yutaka.VineSpring.Data
{
	#region public class Order
	public class Order
	{
		public DateTime CreatedOn { get; set; }
		public string Id { get; set; }
		public bool? IsCommitted { get; set; }
		public bool? IsCompliant { get; set; }
		public Customer Customer { get; set; }
		public DateTime? ShipDate { get; set; }
		public decimal? CalculatedShipping { get; set; }
		public decimal? FreightTax { get; set; }
		public decimal? Shipping { get; set; }
		public decimal? Subtotal { get; set; }
		public decimal? Tax { get; set; }
		public decimal? Total { get; set; }
		public Discount[] Discounts { get; set; }
		public Item[] Items { get; set; }
		public Note[] Notes { get; set; }
		public ShippingAddress ShippingAddress { get; set; }
		public ShippingMethod ShippingMethod { get; set; }
		public string AccountId { get; set; }
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
	}
	#endregion Order

	#region public class Customer, BillingAddress, Name
	public class Customer
	{
		public string OrderId { get; set; }
		public string Email { get; set; }
		public BillingAddress BillingAddress { get; set; }
		public DateTime? Dob { get; set; }
		public Name Name { get; set; }
		public string Phone { get; set; }
	}

	public class BillingAddress
	{
		public DateTime? CreatedOn { get; set; }
		public string Id { get; set; }
		public string OrderId { get; set; }
		public bool? IsInternational { get; set; }
		public string Name { get; set; }
		public string Organization { get; set; }
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string PostalCode { get; set; }
		public string Phone { get; set; }
		public string CustomerId { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; }
	}

	public class Name
	{
		public string OrderId { get; set; }
		public string First { get; set; }
		public string Middle { get; set; }
		public string Last { get; set; }
		public string Nick { get; set; }
	}
	#endregion Customer

	#region public class Discount
	public class Discount
	{
		public string OrderId { get; set; }
		public string Id { get; set; }
		public decimal? Benefit { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
	}
	#endregion Discount

	#region public class Item
	public class Item
	{
		public string OrderId { get; set; }
		public string Sku { get; set; }
		public decimal? Discount { get; set; }
		public decimal? Price { get; set; }
		public decimal? Subtotal { get; set; }
		public decimal? Total { get; set; }
		public int? Quantity { get; set; }
		public string Name { get; set; }
		public string ProductId { get; set; }
		public string ShipCompliantBrandKey { get; set; }
		public string ShipCompliantProductKey { get; set; }
	}
	#endregion Item

	#region public class Note
	public class Note
	{
		public DateTime? CreatedOn { get; set; }
		public string Email { get; set; }
		public string OrderId { get; set; }
		public string Message { get; set; }
	}
	#endregion Note

	#region public class ShippingAddress
	public class ShippingAddress
	{
		public DateTime? CreatedOn { get; set; }
		public string Id { get; set; }
		public string OrderId { get; set; }
		public bool? IsInternational { get; set; }
		public string Name { get; set; }
		public string Organization { get; set; }
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string PostalCode { get; set; }
		public string Phone { get; set; }
		public string CustomerId { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; }
	}
	#endregion ShippingAddress

	#region public class ShippingMethod, AlternateAddress
	public class ShippingMethod
	{
		public string OrderId { get; set; }
		public string Id { get; set; }
		public AlternateAddress AlternateAddress { get; set; }
		public bool? RequireShippingAddress { get; set; }
		public string CarrierCode { get; set; }
		public string Name { get; set; }
	}

	public class AlternateAddress
	{
		public string OrderId { get; set; }
		public bool? IsInternational { get; set; }
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string PostalCode { get; set; }
	}
	#endregion ShippingMethod
}