using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yutaka.VineSpring.Domain.Customer
{
	#region public class ListAllCustomersResponse { ... }
	public class ListAllCustomersResponse
	{
		[JsonProperty("customers")]
		public List<Customer> Customers { get; set; }

		[JsonProperty("hasMore")]
		public bool HasMore { get; set; }

		[JsonProperty("paginationKey")]
		public string PaginationKey { get; set; }

		[JsonProperty("total")]
		public int Total { get; set; }
	}
	#endregion

	#region public class Customer { ... }
	public class Customer
	{
		[JsonProperty("updatedOn")]
		public DateTime UpdatedOn { get; set; }

		[JsonProperty("accountId")]
		public string AccountId { get; set; }

		[JsonProperty("updatedBy")]
		public string UpdatedBy { get; set; }

		[JsonProperty("source")]
		public string Source { get; set; }

		[JsonProperty("fullName")]
		public string FullName { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("name")]
		public Name Name { get; set; }

		[JsonProperty("authToken")]
		public string AuthToken { get; set; }

		[JsonProperty("customerSince")]
		public DateTime CustomerSince { get; set; }

		[JsonProperty("defaultAddress")]
		public DefaultAddress DefaultAddress { get; set; }

		[JsonProperty("createdOn")]
		public DateTime CreatedOn { get; set; }

		[JsonProperty("dob")]
		public DateTime Dob { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("phone")]
		public string Phone { get; set; }

		[JsonProperty("merchantId")]
		public string MerchantId { get; set; }

		[JsonProperty("lifetimeValue")]
		public decimal? LifetimeValue { get; set; }

		[JsonProperty("company")]
		public string Company { get; set; }

		[JsonProperty("lastName")]
		public string LastName { get; set; }

		[JsonProperty("isTaxExempt")]
		public bool? IsTaxExempt { get; set; }

		[JsonProperty("firstName")]
		public string FirstName { get; set; }

		[JsonProperty("altEmail")]
		public string AltEmail { get; set; }
	}
	#endregion

	#region public class Name { ... }
	public class Name
	{
		[JsonProperty("last")]
		public string Last { get; set; }

		[JsonProperty("first")]
		public string First { get; set; }

		[JsonProperty("middle")]
		public string Middle { get; set; }
	}
	#endregion

	#region public class DefaultAddress { ... }
	public class DefaultAddress
	{
		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("updatedBy")]
		public string UpdatedBy { get; set; }

		[JsonProperty("city")]
		public string City { get; set; }

		[JsonProperty("postalCode")]
		public string PostalCode { get; set; }

		[JsonProperty("updatedOn")]
		public DateTime UpdatedOn { get; set; }

		[JsonProperty("createdOn")]
		public DateTime CreatedOn { get; set; }

		[JsonProperty("isInternational")]
		public bool IsInternational { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("customerId")]
		public string CustomerId { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("line2")]
		public string Line2 { get; set; }

		[JsonProperty("line1")]
		public string Line1 { get; set; }

		[JsonProperty("label")]
		public string Label { get; set; }

		[JsonProperty("phone")]
		public string Phone { get; set; }

		[JsonProperty("organization")]
		public string Organization { get; set; }
	}
	#endregion
}