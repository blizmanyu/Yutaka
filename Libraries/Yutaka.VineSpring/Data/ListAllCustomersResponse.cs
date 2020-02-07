using System;

namespace Yutaka.VineSpring.Data
{
	public class ListAllCustomersResponse : ListAllResponse
	{
		public Customer[] Customers { get; set; }
	}

	public class Customer
	{
		public DateTime updatedOn { get; set; }
		public string accountId { get; set; }
		public bool isTaxExempt { get; set; }
		public string updatedBy { get; set; }
		public string source { get; set; }
		public string fullName { get; set; }
		public string email { get; set; }
		public Name name { get; set; }
		public string authToken { get; set; }
		public DateTime customerSince { get; set; }
		public string company { get; set; }
		public DateTime createdOn { get; set; }
		public string id { get; set; }
		public float lifetimeValue { get; set; }
		public string lastName { get; set; }
		public string firstName { get; set; }
		public Defaultaddress defaultAddress { get; set; }
		public DateTime dob { get; set; }
		public string phone { get; set; }
		public string merchantId { get; set; }
	}

	public class Name
	{
		public string last { get; set; }
		public string first { get; set; }
		public string middle { get; set; }
	}

	public class Defaultaddress
	{
		public string updatedBy { get; set; }
		public string city { get; set; }
		public string postalCode { get; set; }
		public string name { get; set; }
		public string customerId { get; set; }
		public DateTime updatedOn { get; set; }
		public string id { get; set; }
		public string state { get; set; }
		public DateTime createdOn { get; set; }
		public string line1 { get; set; }
		public bool isInternational { get; set; }
		public string phone { get; set; }
		public string organization { get; set; }
		public string country { get; set; }
		public string line2 { get; set; }
	}
}