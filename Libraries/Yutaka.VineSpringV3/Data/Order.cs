using System;
using System.Collections.Generic;

namespace Yutaka.VineSpringV3
{
	public class Display
	{
		public bool clubMembers { get; set; }
		public bool admins { get; set; }
		public bool customers { get; set; }
	}

	public class ShippingMethod
	{
		public string zoneTableId { get; set; }
		public bool isAdminDefault { get; set; }
		public Display display { get; set; }
		public string carrierCode { get; set; }
		public int displayOrder { get; set; }
		public string name { get; set; }
		public string id { get; set; }
		public bool requireShippingAddress { get; set; }
	}

	public class Item
	{
		public decimal total { get; set; }
		public int quantity { get; set; }
		public string productId { get; set; }
		public int price { get; set; }
		public decimal subtotal { get; set; }
		public string name { get; set; }
		public int discount { get; set; }
		public string sku { get; set; }
	}

	public class ShippingAddress : Address
	{
		public string city { get; set; }
		public bool isInternational { get; set; }
		public string postalCode { get; set; }
		public string customerId { get; set; }
		public string name { get; set; }
		public string id { get; set; }
		public string state { get; set; }
		public string line1 { get; set; }
	}

	public class BillingAddress : Address
	{
		public string city { get; set; }
		public bool isInternational { get; set; }
		public string postalCode { get; set; }
		public string customerId { get; set; }
		public string name { get; set; }
		public string id { get; set; }
		public string state { get; set; }
		public string line1 { get; set; }
	}

	public class Note
	{
		public string message { get; set; }
		public DateTime createdOn { get; set; }
		public string email { get; set; }
	}

	public class Order
	{
		public string fulfillmentDetail { get; set; }
		public DateTime updatedOn { get; set; }
		public string status { get; set; }
		public decimal tax { get; set; }
		public string salesRep { get; set; }
		public string updatedBy { get; set; }
		public string fulfillmentHouse { get; set; }
		public int shipping { get; set; }
		public ShippingMethod shippingMethod { get; set; }
		public string lockedReasonCode { get; set; }
		public List<Note> notes { get; set; }
		public bool isLockedForDraft { get; set; }
		public bool isCommitted { get; set; }
		public string id { get; set; }
		public List<object> tags { get; set; }
		public DateTime? shipDate { get; set; }
		public bool isCompliant { get; set; }
		public string accountId { get; set; }
		public string shipCompliantStatus { get; set; }
		public bool isV2Import { get; set; }
		public decimal total { get; set; }
		public string fullName { get; set; }
		public List<Item> items { get; set; }
		public decimal freightTax { get; set; }
		public string orderNumber { get; set; }
		public decimal subtotal { get; set; }
		public ShippingAddress shippingAddress { get; set; }
		public DateTime createdOn { get; set; }
		public int calculatedShipping { get; set; }
		public Customer customer { get; set; }
		public List<object> discounts { get; set; }
		public string type { get; set; }
		public string customerId { get; set; }
		public string shippingInstructions { get; set; }
	}
}