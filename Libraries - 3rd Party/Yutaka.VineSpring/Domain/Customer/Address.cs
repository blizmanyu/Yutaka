using System;
using Newtonsoft.Json;

namespace Yutaka.VineSpring.Domain.Customer
{
	public class Address
	{
		[JsonProperty("isInternational")]
		public bool IsInternational { get; set; }

		[JsonProperty("updatedOn")]
		public DateTime UpdatedOn { get; set; }

		[JsonProperty("label")]
		public string Label { get; set; }

		[JsonProperty("line1")]
		public string Line1 { get; set; }

		[JsonProperty("line2")]
		public string Line2 { get; set; }

		[JsonProperty("updatedBy")]
		public string UpdatedBy { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("city")]
		public string City { get; set; }

		[JsonProperty("createdOn")]
		public DateTime CreatedOn { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("phone")]
		public string Phone { get; set; }

		[JsonProperty("postalCode")]
		public string PostalCode { get; set; }

		[JsonProperty("customerId")]
		public string CustomerId { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("organization")]
		public string Organization { get; set; }
	}
}