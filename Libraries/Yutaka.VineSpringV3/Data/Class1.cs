using System;

public class Rootobject
{
	public bool hasMore { get; set; }
	public string paginationKey { get; set; }
	public Order[] orders { get; set; }
}

public class Order
{
	public DateTime shipDate { get; set; }
	public bool isCompliant { get; set; }
	public string accountId { get; set; }
	public string complianceDetail { get; set; }
	public string status { get; set; }
	public float tax { get; set; }
	public float total { get; set; }
	public string fullName { get; set; }
	public Items items { get; set; }
	public string fulfillmentHouse { get; set; }
	public int shipping { get; set; }
	public float freightTax { get; set; }
	public string orderNumber { get; set; }
	public Shippingmethod shippingMethod { get; set; }
	public float subtotal { get; set; }
	public Shippingaddress shippingAddress { get; set; }
	public DateTime createdOn { get; set; }
	public int calculatedShipping { get; set; }
	public Customer customer { get; set; }
	public bool isCommitted { get; set; }
	public string id { get; set; }
	public string type { get; set; }
	public string customerId { get; set; }
}

public class Items
{
	public float total { get; set; }
	public int quantity { get; set; }
	public string productId { get; set; }
	public float price { get; set; }
	public float subtotal { get; set; }
	public bool isTaxExempt { get; set; }
	public string name { get; set; }
	public int discount { get; set; }
	public string sku { get; set; }
}

public class Shippingmethod
{
	public string name { get; set; }
	public string id { get; set; }
	public bool requireShippingAddress { get; set; }
	public Alternateaddress alternateAddress { get; set; }
}

public class Alternateaddress
{
	public string country { get; set; }
	public string state { get; set; }
	public string city { get; set; }
	public string line1 { get; set; }
	public string postalCode { get; set; }
	public string carrierCode { get; set; }
}

public class Shippingaddress
{
	public string country { get; set; }
	public string state { get; set; }
	public string city { get; set; }
	public string line1 { get; set; }
	public string postalCode { get; set; }
}

public class Customer
{
	public object name { get; set; }
	public string last { get; set; }
	public string first { get; set; }
	public string email { get; set; }
	public Billingaddress billingAddress { get; set; }
}

public class Billingaddress
{
	public string country { get; set; }
	public string updatedBy { get; set; }
	public string city { get; set; }
	public bool isInternational { get; set; }
	public string postalCode { get; set; }
	public string name { get; set; }
	public string customerId { get; set; }
	public string state { get; set; }
	public string id { get; set; }
	public DateTime updatedOn { get; set; }
	public DateTime createdOn { get; set; }
	public string line1 { get; set; }
}
