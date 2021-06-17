using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class AccountAddRet
	{
		public string ListID;
		public DateTime TimeCreated; // required
		public DateTime TimeModified; // required
		public string EditSequence; // required
		public string Name; //required
		public string FullName;
		public bool? IsActive;
		public ParentRef ParentRef;
		public int? Sublevel; //required
		public string AccountType;
		public string SpecialAccountType;
		public bool? IsTaxAccount;
		public string AccountNumber;
		public string BankNumber;
		public string Desc;
		public decimal? Balance;
		public decimal? TotalBalance;
		public SalesTaxCodeRef SalesTaxCodeRef;
		public TaxLineInfoRet TaxLineInfoRet;
		public string CashFlowClassification;
		public CurrencyRef CurrencyRef;
		public DataExtRet DataExtRet;
		public ErrorRecovery ErrorRecovery;

		public AccountAddRet()
		{
			ParentRef = new ParentRef();
			SalesTaxCodeRef = new SalesTaxCodeRef();
			TaxLineInfoRet = new TaxLineInfoRet();
			CurrencyRef = new CurrencyRef();
			DataExtRet = new DataExtRet();
			ErrorRecovery = new ErrorRecovery();
		}
	}
}
