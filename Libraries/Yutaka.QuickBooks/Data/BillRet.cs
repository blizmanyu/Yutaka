using System;

namespace Yutaka.QuickBooks
{
	public class BillRet
	{
		public string TxnID; // required
		public DateTime TimeCreated; // required
		public DateTime TimeModified; // required
		public string EditSequence; // required
		public int? TxnNumber;
		public VendorRef VendorRef;
		public VendorAddress VendorAddress;
		public APAccountRef APAccountRef;
		public DateTime TxnDate; // required
		public DateTime? DueDate;
		public decimal AmountDue; // required
		public CurrencyRef CurrencyRef;
		public decimal? ExchangeRate;
		public decimal? AmountDueInHomeCurrency;
		public string RefNumber;
		public TermsRef TermsRef;
		public string Memo;
		public bool? IsTaxIncluded;
		public SalesTaxCodeRef SalesTaxCodeRef;
		public bool? IsPaid;
		public string ExternalGUID;
		public object laskfjdlskdjf;
		//public object laskfjdlskdjf;
		//public object laskfjdlskdjf;
		//public object laskfjdlskdjf;
		//public object laskfjdlskdjf;
		//public object laskfjdlskdjf;
		//public object laskfjdlskdjf;

		public BillRet()
		{

		}
	}
}