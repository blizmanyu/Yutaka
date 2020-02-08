using System;

namespace Yutaka.VineSpring.Data20200207
{
	public class Customer
	{
		public DateTime? CreatedOn { get; set; }
		public string Id { get; set; }
		public bool? IsTaxExempt { get; set; }
		public CustomerDefaultAddress DefaultAddress { get; set; }
		public CustomerName Name { get; set; }
		public DateTime? CustomerSince { get; set; }
		public DateTime? Dob { get; set; }
		public decimal? LifetimeValue { get; set; }
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

	public class CustomerName
	{
		public string First { get; set; }
		public string Middle { get; set; }
		public string Last { get; set; }
	}

	public class CustomerDefaultAddress : Address { }
}