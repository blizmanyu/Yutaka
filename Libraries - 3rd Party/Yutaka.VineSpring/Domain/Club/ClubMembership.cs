using System;

namespace Yutaka.VineSpring.Domain.Club
{
	public class ClubMembership
	{
		public bool IsGift { get; set; }
		public CustomerNote CustomerNote { get; set; }
		public DateTime CancelationOn { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime HoldOn { get; set; }
		public DateTime HoldUntil { get; set; }
		public DateTime MemberSince { get; set; }
		public DateTime UpdatedOn { get; set; }
		public int ShipmentsCreated { get; set; }
		public Note Note { get; set; }
		public ShipmentsGifted ShipmentsGifted { get; set; }
		public string AccountId { get; set; }
		public string CancelationReason { get; set; }
		public string CardId { get; set; }
		public string ClubId { get; set; }
		public string Custom1 { get; set; }
		public string Id { get; set; }
		public string PurchaserCustomerId { get; set; }
		public string RecipientCustomerId { get; set; }
		public string SalesRep { get; set; }
		public string ShippingAddressId { get; set; }
		public string ShippingMethodId { get; set; }
		public string Source { get; set; }
		public string Status { get; set; }
		public string UpdatedBy { get; set; }
		public Trustee Trustee { get; set; }

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
			Console.Write("\nSalesrep: {0}", SalesRep);
			Console.Write("\nShippingAddressId: {0}", ShippingAddressId);
			Console.Write("\nShippingMethodId: {0}", ShippingMethodId);
			Console.Write("\nSource: {0}", Source);
			Console.Write("\nStatus: {0}", Status);
			Console.Write("\nUpdatedBy: {0}", UpdatedBy);
			if (CustomerNote != null)
				CustomerNote.DumpToConsole();
			if (Note != null)
				Note.DumpToConsole();
			if (Trustee != null)
				Trustee.DumpToConsole();
			Console.Write("\n");
		}
	}

	public class CustomerNote
	{
		public DateTime CreatedOn { get; set; }
		public string CreatedBy { get; set; }
		public string Message { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }

		public void DumpToConsole()
		{
			Console.Write("\nCustomerNote:");
			Console.Write("\n    CreatedOn: {0}", CreatedOn);
			Console.Write("\n    CreatedBy: {0}", CreatedBy);
			Console.Write("\n      Message: {0}", Message);
			Console.Write("\n    UpdatedBy: {0}", UpdatedBy);
			Console.Write("\n    UpdatedOn: {0}", UpdatedOn);
		}
	}

	public class Note
	{
		public DateTime CreatedOn { get; set; }
		public DateTime UpdatedOn { get; set; }
		public string CreatedBy { get; set; }
		public string Message { get; set; }
		public string UpdatedBy { get; set; }

		public void DumpToConsole()
		{
			Console.Write("\nNote:");
			Console.Write("\n    CreatedOn: {0}", CreatedOn);
			Console.Write("\n    CreatedBy: {0}", CreatedBy);
			Console.Write("\n      Message: {0}", Message);
			Console.Write("\n    UpdatedBy: {0}", UpdatedBy);
			Console.Write("\n    UpdatedOn: {0}", UpdatedOn);
		}
	}

	public class ShipmentsGifted { }

	public class Trustee
	{
		public string Email { get; set; }
		public string FullName { get; set; }

		public void DumpToConsole()
		{
			Console.Write("\nTrustee:");
			Console.Write("\n       Email: {0}", Email);
			Console.Write("\n    FullName: {0}", FullName);
		}
	}
}