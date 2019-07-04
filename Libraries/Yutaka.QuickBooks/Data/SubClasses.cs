namespace Yutaka.QuickBooks
{
	public class AccountRef
	{
		public string ListID;
		public string FullName;
	}

	public class ClassRef
	{
		public string ListID;
		public string FullName;
	}

	public class ParentRef
	{
		public string ListID;
		public string FullName;
	}

	public class UnitOfMeasureSetRef
	{
		public string ListID;
		public string FullName;
	}

	public class SalesTaxCodeRef
	{
		public string ListID;
		public string FullName;
	}

	public class SalesOrPurchase
	{
		public AccountRef AccountRef;
		public string Desc;
		public decimal Price;
		public decimal PricePercent;
		public SalesOrPurchase() { AccountRef = new AccountRef(); }
	}
}