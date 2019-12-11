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
		public bool? IsLockedForDraft { get; set; }
		public bool? IsV2Import { get; set; }
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
		public string CampaignId { get; set; }
		public string ComplianceDetail { get; set; }
		public string CustomerId { get; set; }
		public string FulfillmentDetail { get; set; }
		public string FulfillmentHouse { get; set; }
		public string FullName { get; set; }
		public string GiftMessage { get; set; }
		public string LockedReasonCode { get; set; }
		public string OrderNumber { get; set; }
		public string SalesRep { get; set; }
		public string ShipCompliantStatus { get; set; }
		public string ShippingInstructions { get; set; }
		public string Source { get; set; }
		public string Status { get; set; }
		public string TrackingNumber { get; set; }
		public string Type { get; set; }
		public string[] Tags { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; }
	}
	#endregion Order

	#region public class Customer
	public class Customer
	{
		public string Email { get; set; }
		public BillingAddress BillingAddress { get; set; }
		public DateTime? Dob { get; set; }
		public Name Name { get; set; }
		public string FullName { get; set; }
		public string Phone { get; set; }
	}

	#region public class Name
	public class Name
	{
		public string First { get; set; }
		public string Last { get; set; }
		public string Middle { get; set; }
		public string Nick { get; set; }
		public string Suffix { get; set; }
		public string Title { get; set; }
	}
	#endregion Name

	#region public class BillingAddress
	public class BillingAddress
	{
		public DateTime CreatedOn { get; set; }
		public string Id { get; set; }
		public bool? IsInternational { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string CustomerId { get; set; }
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string Name { get; set; }
		public string Organization { get; set; }
		public string Phone { get; set; }
		public string PostalCode { get; set; }
		public string State { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; }
	}
	#endregion BillingAddress
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
		public DateTime CreatedOn { get; set; }
		public string Email { get; set; }
		public string OrderId { get; set; }
		public string Message { get; set; }
	}
	#endregion Note

	#region public class ShippingMethod
	public class ShippingMethod
	{
		public string Id { get; set; }
		public AlternateAddress AlternateAddress { get; set; }
		public bool? IsAdminDefault { get; set; }
		public bool? RequireShippingAddress { get; set; }
		public Display Display { get; set; }
		public int? DisplayOrder { get; set; }
		public string CarrierCode { get; set; }
		public string Name { get; set; }
		public string ZoneTableId { get; set; }
	}

	#region public class Display
	public class Display
	{
		public bool? Admins { get; set; }
		public bool? ClubMembers { get; set; }
		public bool? Customers { get; set; }
	}
	#endregion Display

	#region public class AlternateAddress
	public class AlternateAddress
	{
		public bool? IsInternational { get; set; }
		public string Line1 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string PostalCode { get; set; }
	}
	#endregion AlternateAddress
	#endregion ShippingMethod

	#region public class ShippingAddress
	public class ShippingAddress
	{
		public DateTime? CreatedOn { get; set; }
		public string Id { get; set; }
		public bool? IsInternational { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string CustomerId { get; set; }
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string Name { get; set; }
		public string Organization { get; set; }
		public string Phone { get; set; }
		public string PostalCode { get; set; }
		public string State { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; }
	}
	#endregion ShippingAddress
}