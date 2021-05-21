using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class ARRefundCreditCardRet
	{
		public string TxnID;
		public DateTime TimeCreated;
		public DateTime TimeModified;
		public string EditSequence;
		public int? TxnNumber;
		public CustomerRef CustomerRef;
		public RefundFromAccountRef RefundFromAccountRef;
		public DateTime TxnDate;
		public string RefNumber;
		public decimal TotalAmount;
		public CurrencyRef CurrencyRef;
		public float ExchangeRate;
		public decimal TotalAmountInHomeCurrency;
		public Address Address;
		public AddressBlock AddressBlock;
		public PaymentMethodRef PaymentMethodRef;
		public string Memo;
		public List<ARRefundCreditCard_CreditCardTxnInfo> CreditCardTxnInfo;
		public string ExternalGUID;
		public RefundAppliedToTxnRet RefundAppliedToTxnRet;
		public DataExtRet DataExtRet;

		public ARRefundCreditCardRet()
		{
			CustomerRef = new CustomerRef();
			RefundFromAccountRef = new RefundFromAccountRef();
			CurrencyRef = new CurrencyRef();
			Address = new Address();
			AddressBlock = new AddressBlock();
			PaymentMethodRef = new PaymentMethodRef();
			CreditCardTxnInfo = new List<ARRefundCreditCard_CreditCardTxnInfo>();
			RefundAppliedToTxnRet = new RefundAppliedToTxnRet();
			DataExtRet = new DataExtRet();
		}
	}
}
