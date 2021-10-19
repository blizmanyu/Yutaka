using System;

namespace Yutaka.VineSpring.Domain.Club
{
	public class ClubMembership
	{
		public bool IsGift { get; set; }
		public int ShipmentsCreated { get; set; }
		public DateTime CancelationOn { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime HoldOn { get; set; }
		public DateTime HoldUntil { get; set; }
		public DateTime MemberSince { get; set; }
		public DateTime UpdatedOn { get; set; }
		public object ShipmentsGifted { get; set; }
		public string AccountId { get; set; }
		public string CancelationReason { get; set; }
		public string CardId { get; set; }
		public string ClubId { get; set; }
		public string Custom1 { get; set; }
		public string Id { get; set; }
		public string PurchaserCustomerId { get; set; }
		public string RecipientCustomerId { get; set; }
		public string Salesrep { get; set; }
		public string ShippingAddressId { get; set; }
		public string ShippingMethodId { get; set; }
		public string Source { get; set; }
		public string Status { get; set; }
		public string UpdatedBy { get; set; }
		public Customernote CustomerNote { get; set; }
		public Note Note { get; set; }
	}

	public class Note
	{
		public string Message { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }
	}

	public class Customernote
	{
		public string Message { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }
	}
}