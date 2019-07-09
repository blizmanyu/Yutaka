namespace Yutaka.QuickBooks
{
	#region public class AccountRef
	public class AccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion AccountRef

	#region public class ClassRef
	public class ClassRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion ClassRef

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

	#region public class SalesTaxCodeRef
	public class SalesTaxCodeRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion SalesTaxCodeRef

	#region public class UnitOfMeasureSetRef
	public class UnitOfMeasureSetRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion UnitOfMeasureSetRef
}