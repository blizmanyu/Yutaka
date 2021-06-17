using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class CreditCardCreditRet
	{
		public string TxnID; // required
		public DateTime TimeCreated; // required
		public DateTime TimeModified; // required
		public string EditSequence; // required 
		public int? TxnNumber;
		public AccountRef AccountRef;
		public PayeeEntityRef PayeeEntityRef;
		public DateTime TxnDate;
		public decimal? Amount;
		public CurrencyRef CurrencyRef;
		public decimal? ExchangeRate;
		public decimal? AmountInHomeCurrency;
		public string RefNumber;
		public string Memo;
		public bool? IsTaxIncluded;
		public SalesTaxCodeRef SalesTaxCodeRef;
		public string ExternalGUID;
		public List<CreditCard_ExpenseLineRet> CreditCardCharge_ExpenseLineRet;
		public DataExtRet DataExtRet;

		public CreditCardCreditRet()
		{
			AccountRef = new AccountRef();
			PayeeEntityRef = new PayeeEntityRef();
			CurrencyRef = new CurrencyRef();
			SalesTaxCodeRef = new SalesTaxCodeRef();
			CreditCardCharge_ExpenseLineRet = new List<CreditCard_ExpenseLineRet>();
			DataExtRet = new DataExtRet();
		}
	}
}
