namespace Yutaka.QuickBooks
{
	#region public class AccountRef
	public class AccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion AccountRef

	#region public class APAccountRef
	public class APAccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion APAccountRef

	#region public class ClassRef
	public class ClassRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion ClassRef

	#region public class CurrencyRef
	public class CurrencyRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion CurrencyRef

	#region public class CustomerRef
	public class CustomerRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion CustomerRef

	#region public class ExpenseAccountRef
	public class ExpenseAccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion ExpenseAccountRef

	#region public class IncomeAccountRef
	public class IncomeAccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion IncomeAccountRef

	#region public class ParentRef
	public class ParentRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion ParentRef

	#region public class PrefVendorRef
	public class PrefVendorRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion PrefVendorRef

	#region public class PurchaseTaxCodeRef
	public class PurchaseTaxCodeRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion PurchaseTaxCodeRef

	#region public class SalesAndPurchase
	public class SalesAndPurchase
	{
		public string SalesDesc;
		public decimal? SalesPrice;
		public IncomeAccountRef IncomeAccountRef;
		public string PurchaseDesc;
		public decimal? PurchaseCost;
		public PurchaseTaxCodeRef PurchaseTaxCodeRef;
		public ExpenseAccountRef ExpenseAccountRef;
		public PrefVendorRef PrefVendorRef;

		public SalesAndPurchase()
		{
			IncomeAccountRef = new IncomeAccountRef();
			PurchaseTaxCodeRef = new PurchaseTaxCodeRef();
			ExpenseAccountRef = new ExpenseAccountRef();
			PrefVendorRef = new PrefVendorRef();
		}
	}
	#endregion SalesAndPurchase

	#region public class SalesOrPurchase
	public class SalesOrPurchase
	{
		public string Desc;
		public decimal? Price;
		public decimal? PricePercent;
		public AccountRef AccountRef;

		public SalesOrPurchase()
		{
			AccountRef = new AccountRef();
		}
	}
	#endregion SalesOrPurchase

	#region public class SalesRepRef
	public class SalesRepRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion SalesRepRef

	#region public class SalesTaxCodeRef
	public class SalesTaxCodeRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion SalesTaxCodeRef

	#region public class TermsRef
	public class TermsRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion TermsRef

	#region public class UnitOfMeasureSetRef
	public class UnitOfMeasureSetRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion UnitOfMeasureSetRef

	#region public class VendorAddress
	public class VendorAddress
	{
		public string Addr1;
		public string Addr2;
		public string Addr3;
		public string Addr4;
		public string Addr5;
		public string City;
		public string State;
		public string PostalCode;
		public string Country;
		public string Note;
	}
	#endregion VendorAddress

	#region public class VendorRef
	public class VendorRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion VendorRef
}