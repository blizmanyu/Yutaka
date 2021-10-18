using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class BillPaymentCreditCardRet
	{
		public string TxnID;
		public DateTime TimeCreated;
		public DateTime TimeModified;
		public string EditSequence;
		public int? TxnNumber;
		public PayeeEntityRef PayeeEntityRef;
		public APAccountRef APAccountRef;
		public DateTime TxnDate;
		public CreditCardAccountRef CreditCardAccountRef;
		public decimal? Amount;
		public CurrencyRef CurrencyRef;
		public decimal? ExchangeRate;
		public decimal? AmountInHomeCurrency;
		public string RefNumber;
		public string Memo;
		public string ExternalGUID;
		public List<AppliedToTxnRet> AppliedToTxnRet;
		public DataExtRet DataExtRet;
	}
}
