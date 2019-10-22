using System;
using System.Collections.Generic;

namespace Yutaka.QuickBooks
{
	public class InventoryAdjustmentRet
	{
		public string TxnID; // required
		public DateTime TimeCreated; // required
		public DateTime TimeModified; // required
		public string EditSequence; // required
		public int? TxnNumber;
		public AccountRef AccountRef; // required
		public InventorySiteRef InventorySiteRef;
		public DateTime TxnDate; // required
		public string RefNumber;
		public CustomerRef CustomerRef;
		public ClassRef ClassRef;
		public string Memo;
		public string ExternalGUID;
		public List<InventoryAdjustmentLineRet> LineItems;

		public InventoryAdjustmentRet()
		{
			AccountRef = new AccountRef();
			InventorySiteRef = new InventorySiteRef();
			CustomerRef = new CustomerRef();
			ClassRef = new ClassRef();
			LineItems = new List<InventoryAdjustmentLineRet>();
		}
	}
}