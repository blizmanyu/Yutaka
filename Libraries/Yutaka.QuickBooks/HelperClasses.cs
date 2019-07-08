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
		public IncomeAccountRef IncomeAccountRef = new IncomeAccountRef();
		public PurchaseTaxCodeRef PurchaseTaxCodeRef = new PurchaseTaxCodeRef();
		public ExpenseAccountRef ExpenseAccountRef = new ExpenseAccountRef();
		public PrefVendorRef PrefVendorRef = new PrefVendorRef();
		public string SalesDesc;
		public string PurchaseDesc;
		public decimal SalesPrice;
		public decimal PurchaseCost;
	}
	#endregion SalesAndPurchase

	#region public class SalesOrPurchase
	public class SalesOrPurchase
	{
		public AccountRef AccountRef = new AccountRef();
		public string Desc;
		public decimal Price;
		public decimal PricePercent;
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