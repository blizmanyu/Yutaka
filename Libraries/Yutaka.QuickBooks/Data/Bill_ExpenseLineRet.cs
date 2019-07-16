namespace Yutaka.QuickBooks
{
	public class Bill_ExpenseLineRet
	{
		public string TxnID; // required
		public string TxnLineID; // required
		public AccountRef AccountRef;
		public decimal? Amount;
		public string Memo;
		public CustomerRef CustomerRef;
		public ClassRef ClassRef;
		public SalesTaxCodeRef SalesTaxCodeRef;
		public string BillableStatus;
		public SalesRepRef SalesRepRef;

		public Bill_ExpenseLineRet()
		{
			AccountRef = new AccountRef();
			CustomerRef = new CustomerRef();
			ClassRef = new ClassRef();
			SalesTaxCodeRef = new SalesTaxCodeRef();
			SalesRepRef = new SalesRepRef();
		}
	}
}