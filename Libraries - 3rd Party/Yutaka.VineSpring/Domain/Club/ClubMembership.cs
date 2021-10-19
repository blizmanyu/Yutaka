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

		public void DumpToConsole()
		{
			Console.Write("\n");
			Console.Write("\nIsGift: {0}", IsGift);
			Console.Write("\nShipmentsCreated: {0}", ShipmentsCreated);
			Console.Write("\nCancelationOn: {0}", CancelationOn);
			Console.Write("\nCreatedOn: {0}", CreatedOn);
			Console.Write("\nHoldOn: {0}", HoldOn);
			Console.Write("\nHoldUntil: {0}", HoldUntil);
			Console.Write("\nMemberSince: {0}", MemberSince);
			Console.Write("\nUpdatedOn: {0}", UpdatedOn);
			Console.Write("\nShipmentsGifted: {0}", ShipmentsGifted);
			Console.Write("\nAccountId: {0}", AccountId);
			Console.Write("\nCancelationReason: {0}", CancelationReason);
			Console.Write("\nCardId: {0}", CardId);
			Console.Write("\nClubId: {0}", ClubId);
			Console.Write("\nCustom1: {0}", Custom1);
			Console.Write("\nId: {0}", Id);
			Console.Write("\nPurchaserCustomerId: {0}", PurchaserCustomerId);
			Console.Write("\nRecipientCustomerId: {0}", RecipientCustomerId);
			Console.Write("\nSalesrep: {0}", Salesrep);
			Console.Write("\nShippingAddressId: {0}", ShippingAddressId);
			Console.Write("\nShippingMethodId: {0}", ShippingMethodId);
			Console.Write("\nSource: {0}", Source);
			Console.Write("\nStatus: {0}", Status);
			Console.Write("\nUpdatedBy: {0}", UpdatedBy);
			if (CustomerNote != null)
				CustomerNote.DumpToConsole();
			if (Note != null)
				Note.DumpToConsole();
			Console.Write("\n");
		}
	}

	public class Note
	{
		public string Message { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }

		public void DumpToConsole()
		{
			Console.Write("\nNote Message: {0}", Message);
			Console.Write("\nNote UpdatedBy: {0}", UpdatedBy);
			Console.Write("\nNote UpdatedOn: {0}", UpdatedOn);
		}
	}

	public class Customernote
	{
		public string Message { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }

		public void DumpToConsole()
		{
			Console.Write("\nCustomernote Message: {0}", Message);
			Console.Write("\nCustomernote UpdatedBy: {0}", UpdatedBy);
			Console.Write("\nCustomernote UpdatedOn: {0}", UpdatedOn);
		}
	}
}