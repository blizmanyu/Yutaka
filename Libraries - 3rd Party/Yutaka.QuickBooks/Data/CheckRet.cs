using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class CheckRet
	{
		public string TxnID; // required
		public DateTime TimeCreated; // required
		public DateTime TimeModified; // required
		public string EditSequence; // required
		public int? TxnNumber;
		public AccountRef AccountRef;
		public PayeeEntityRef PayeeEntityRef;
		public string RefNumber;
		public DateTime TxnDate;
		public decimal? Amount;
		public CurrencyRef CurrencyRef;
		public decimal? ExchangeRate;
		public decimal? AmountInHomeCurrency;
		public string Memo;
		public Address Address;
		public AddressBlock AddressBlock;
		public bool? IsToBePrinted;
		public bool? IsTaxIncluded;
		public SalesTaxCodeRef SalesTaxCodeRef;
		public string ExternalGUID;
		public LinkedTxn LinkedTxn;
		public List<Check_ExpenseLineRet> Check_ExpenseLineRet;
		public DataExtRet DataExtRet;

		public CheckRet()
		{
			AccountRef = new AccountRef();
			PayeeEntityRef = new PayeeEntityRef();
			CurrencyRef = new CurrencyRef();
			Address = new Address();
			AddressBlock = new AddressBlock();
			SalesTaxCodeRef = new SalesTaxCodeRef();
			LinkedTxn = new LinkedTxn();
			Check_ExpenseLineRet = new List<Check_ExpenseLineRet>();
			DataExtRet = new DataExtRet();
		}

	}
}
