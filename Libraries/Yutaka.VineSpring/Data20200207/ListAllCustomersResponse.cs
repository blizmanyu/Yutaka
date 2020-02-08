using System;

namespace Yutaka.VineSpring.Data20200207
{
	public class ListAllCustomersResponse : ListAllResponse
	{
		public Customer[] Customers { get; set; }
	}

	public class Customer
	{
		public DateTime? CreatedOn { get; set; }
		public string Id { get; set; }
		public bool? IsTaxExempt { get; set; }
		public DateTime? CustomerSince { get; set; }
		public DateTime? Dob { get; set; }
		public decimal? LifetimeValue { get; set; }
		public DefaultAddress DefaultAddress { get; set; }
		public Name Name { get; set; }
		public string AccountId { get; set; }
		public string AuthToken { get; set; }
		public string Company { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName { get; set; }
		public string MerchantId { get; set; }
		public string Phone { get; set; }
		public string Source { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; }
	}

	public class Name
	{
		public string First { get; set; }
		public string Middle { get; set; }
		public string Last { get; set; }
	}

	public class DefaultAddress
	{
		public DateTime? CreatedOn { get; set; }
		public string Id { get; set; }
		public bool? IsInternational { get; set; }
		public string Name { get; set; }
		public string Organization { get; set; }
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
		public string Phone { get; set; }
		public string CustomerId { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; }
	}
}