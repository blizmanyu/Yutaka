using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class ExpenseLineRet
	{
		public string TxnLineID; // required
		public AccountRef AccountRef;
		public decimal? Amount;
		public string Memo;
		public CustomerRef CustomerRef;
		public ClassRef ClassRef;
		public SalesTaxCodeRef SalesTaxCodeRef;
		public string BillableStatus;
		public SalesRepRef SalesRepRef;
		public DataExtRet DataExtRet;

		public ExpenseLineRet()
		{
			AccountRef = new AccountRef();
			CustomerRef = new CustomerRef();
			ClassRef = new ClassRef();
			SalesTaxCodeRef = new SalesTaxCodeRef();
			SalesRepRef = new SalesRepRef();
			DataExtRet = new DataExtRet();
		}
	}
}
