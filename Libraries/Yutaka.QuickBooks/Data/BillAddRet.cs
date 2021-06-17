using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class BillAddRet
	{
		public string TxnID;
		public DateTime TimeCreated;
		public DateTime TimeModified;
		public string EditSequence;
		public int? TxnNumber;
		public VendorRef VendorRef;
		public VendorAddress VendorAddress;
		public APAccountRef APAccountRef;
		public DateTime TxnDate;
		public DateTime DueDate;
		public decimal? AmountDue;
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
		public LinkedTxn LinkedTxn;
		public List<ExpenseLineRet> ExpenseLineRet;
		public List<ItemLineRet> ItemLineRet;
		public decimal? OpenAmount;
		public DataExtRet DataExtRet;

		public BillAddRet()
		{
			VendorRef = new VendorRef();
			VendorAddress = new VendorAddress();
			APAccountRef = new APAccountRef();
			CurrencyRef = new CurrencyRef();
			TermsRef = new TermsRef();
			SalesTaxCodeRef = new SalesTaxCodeRef();
			LinkedTxn = new LinkedTxn();
			ExpenseLineRet = new List<ExpenseLineRet>();
			ItemLineRet = new List<ItemLineRet>();
			DataExtRet = new DataExtRet();
		}
	}

}
