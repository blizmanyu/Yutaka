using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class ReceivePaymentRet 
	{
		public string TxnID; // required
		public DateTime TimeCreated; // required
		public DateTime TimeModified; // required
		public string EditSequence; // required 
		public int? TxnNumber;
		public CustomerRef CustomerRef;
		public ARAccountRef ARAccountRef;
		public DateTime TxnDate;
		public string RefNumber;
		public decimal? TotalAmount;
		public CurrencyRef CurrencyRef;
		public decimal? ExchangeRate;
		public decimal? TotalAmountInHomeCurrency;
		public PaymentMethodRef PaymentMethodRef;
		public string Memo;
		public DepositToAccountRef DepositToAccountRef;
		public List<CreditCardTxnInfo> CreditCardTxnInfo;
		public decimal? UnusedPayment;
		public decimal? UnusedCredits;
		public string ExternalGUID;
		public List<AppliedToTxnRet> AppliedToTxnRet;
		public DataExtRet DataExtRet;

		public ReceivePaymentRet()
		{
			CustomerRef = new CustomerRef();
			ARAccountRef = new ARAccountRef();
			CurrencyRef = new CurrencyRef();
			PaymentMethodRef = new PaymentMethodRef();
			DepositToAccountRef = new DepositToAccountRef();
			CreditCardTxnInfo = new List<CreditCardTxnInfo>();
			AppliedToTxnRet = new List<AppliedToTxnRet>();
			DataExtRet = new DataExtRet();
		}
	}
}
