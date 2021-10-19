using System;

namespace Yutaka.VineSpring.Domain.Club
{
	public class ClubMembership
	{
		public DateTime updatedOn { get; set; }
		public string accountId { get; set; }
		public string purchaserCustomerId { get; set; }
		public string status { get; set; }
		public string updatedBy { get; set; }
		public string source { get; set; }
		public int shipmentsCreated { get; set; }
		public string clubId { get; set; }
		public string cardId { get; set; }
		public bool isGift { get; set; }
		public string recipientCustomerId { get; set; }
		public DateTime createdOn { get; set; }
		public string id { get; set; }
		public string shippingAddressId { get; set; }
		public DateTime memberSince { get; set; }
		public DateTime holdOn { get; set; }
		public Note note { get; set; }
		public DateTime cancelationOn { get; set; }
		public DateTime holdUntil { get; set; }
		public string cancelationReason { get; set; }
		public object shipmentsGifted { get; set; }
		public string shippingMethodId { get; set; }
		public string salesrep { get; set; }
		public Customernote customerNote { get; set; }
		public string custom1 { get; set; }
	}

	public class Note
	{
		public string message { get; set; }
		public string updatedBy { get; set; }
		public DateTime updatedOn { get; set; }
	}

	public class Customernote
	{
		public string message { get; set; }
		public string updatedBy { get; set; }
		public DateTime updatedOn { get; set; }
	}
}