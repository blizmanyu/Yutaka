using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class AccountRet
	{
		public string ListID;
		/// <summary>
		/// required
		/// </summary>
		public DateTime TimeCreated;
		/// <summary>
		/// required
		/// </summary>
		public DateTime TimeModified;
		/// <summary>
		/// required
		/// </summary>
		public string EditSequence; 
		/// <summary>
		/// required
		/// </summary>
		public string Name; 
		public string FullName;
		public bool? IsActive;
		public ParentRef ParentRef;
		/// <summary>
		/// required
		/// </summary>
		public int? Sublevel;
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

		public AccountRet()
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
