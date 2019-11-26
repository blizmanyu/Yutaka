using System;

namespace Yutaka.VineSpringV3.Data20191126
{
	public class Customer
	{
		public string Id { get; set; }
		public string AccountId { get; set; }
		public string AltEmail { get; set; }
		public string Company { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime CustomerSince { get; set; }
		public BillingAddress BillingAddress { get; set; }
		public DefaultAddress DefaultAddress { get; set; }
		public DateTime? Dob { get; set; }
		public string Email { get; set; }
		public string FullName { get; set; }
		public bool IsTaxExempt { get; set; }
		public string Password { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }
		public Name Name { get; set; }
		public string Phone { get; set; }
		public string Source { get; set; }
	}

	public class Name
	{
		public string First { get; set; }
		public string Last { get; set; }
		public string Middle { get; set; }
	}
}