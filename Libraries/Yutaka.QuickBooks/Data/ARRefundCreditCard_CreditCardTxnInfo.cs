namespace Yutaka.QuickBooks.Data
{
	public class ARRefundCreditCard_CreditCardTxnInfo
	{
		public CreditCardTxnInputInfo CreditCardTxnInputInfo; // required
		public CreditCardTxnResultInfo CreditCardTxnResultInfo; // required

		public ARRefundCreditCard_CreditCardTxnInfo()
		{
			CreditCardTxnInputInfo = new CreditCardTxnInputInfo();
			CreditCardTxnResultInfo = new CreditCardTxnResultInfo();
		}
	}
}