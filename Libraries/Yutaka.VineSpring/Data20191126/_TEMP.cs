using System;

public class Rootobject
{
	public bool hasMore { get; set; }
	public string paginationKey { get; set; }
	public Order[] orders { get; set; }
}

public class Order
{
	public string status { get; set; }
	public float tax { get; set; }
	public string fulfillmentHouse { get; set; }
	public float shipping { get; set; }
	public Shippingmethod shippingMethod { get; set; }
	public string id { get; set; }
	public bool isCommitted { get; set; }
	public string campaignId { get; set; }
	public DateTime shipDate { get; set; }
	public bool isCompliant { get; set; }
	public string accountId { get; set; }
	public string shipCompliantStatus { get; set; }
	public float total { get; set; }
	public string fullName { get; set; }
	public Item[] items { get; set; }
	public float freightTax { get; set; }
	public string orderNumber { get; set; }
	public int subtotal { get; set; }
	public Shippingaddress shippingAddress { get; set; }
	public DateTime createdOn { get; set; }
	public string trackingNumber { get; set; }
	public float calculatedShipping { get; set; }
	public Customer customer { get; set; }
	public Discount[] discounts { get; set; }
	public string customerId { get; set; }
	public string type { get; set; }
	public string fulfillmentDetail { get; set; }
	public string salesRep { get; set; }
	public string[] tags { get; set; }
	public string complianceDetail { get; set; }
	public string source { get; set; }
	public Note[] notes { get; set; }
}

public class Shippingmethod
{
	public string name { get; set; }
	public string id { get; set; }
	public bool requireShippingAddress { get; set; }
	public Alternateaddress alternateAddress { get; set; }
	public string carrierCode { get; set; }
}

public class Shippingaddress
{
	public string updatedBy { get; set; }
	public string city { get; set; }
	public bool isInternational { get; set; }
	public string phone { get; set; }
	public string postalCode { get; set; }
	public string name { get; set; }
	public string customerId { get; set; }
	public DateTime updatedOn { get; set; }
	public string id { get; set; }
	public string state { get; set; }
	public DateTime createdOn { get; set; }
	public string line1 { get; set; }
	public string country { get; set; }
	public string line2 { get; set; }
	public string organization { get; set; }
}

public class Customer
{
	public Name name { get; set; }
	public Billingaddress billingAddress { get; set; }
	public string phone { get; set; }
	public DateTime dob { get; set; }
	public string email { get; set; }
}

public class Name
{
	public string last { get; set; }
	public string first { get; set; }
	public string middle { get; set; }
	public string nick { get; set; }
}

public class Billingaddress
{
	public string updatedBy { get; set; }
	public string city { get; set; }
	public bool isInternational { get; set; }
	public string phone { get; set; }
	public string postalCode { get; set; }
	public string name { get; set; }
	public string customerId { get; set; }
	public DateTime updatedOn { get; set; }
	public string id { get; set; }
	public string state { get; set; }
	public DateTime createdOn { get; set; }
	public string line1 { get; set; }
	public string country { get; set; }
	public string line2 { get; set; }
	public string organization { get; set; }
}

public class Item
{
	public string shipCompliantBrandKey { get; set; }
	public int total { get; set; }
	public int quantity { get; set; }
	public string productId { get; set; }
	public int price { get; set; }
	public float subtotal { get; set; }
	public string name { get; set; }
	public float discount { get; set; }
	public string shipCompliantProductKey { get; set; }
	public string sku { get; set; }
}

public class Discount
{
	public string name { get; set; }
	public string id { get; set; }
	public string type { get; set; }
	public float benefit { get; set; }
}

public class Note
{
	public string message { get; set; }
	public DateTime createdOn { get; set; }
	public string email { get; set; }
}
