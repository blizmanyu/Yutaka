using System;

namespace Yutaka.VineSpring.Data20191126
{
	public class Customer
	{
		public string Id;
		public string AccountId;
		public DateTime CreatedOn;
		public DateTime? CustomerSince;
		public Address DefaultAddress;
		public DateTime? DoB;
		public string Email;
		public string FullName;
		public string AltEmail;
		public string Company;
		public bool? IsTaxExempt;
		public string UpdatedBy;
		public DateTime? UpdatedOn;
		public Name Name;
		public string Phone;
		public string Source;
	}
}