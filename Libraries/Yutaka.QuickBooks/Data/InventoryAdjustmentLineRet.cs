namespace Yutaka.QuickBooks
{
	public class InventoryAdjustmentLineRet
	{
		public string TxnLineID; // required
		public ItemRef ItemRef; // required
		public string SerialNumber;
		public string SerialNumberAddedOrRemoved;
		public string LotNumber;
		public InventorySiteLocationRef InventorySiteLocationRef;
		public decimal QuantityDifference; // required
		public decimal ValueDifference; // required

		public InventoryAdjustmentLineRet()
		{
			ItemRef = new ItemRef();
			InventorySiteLocationRef = new InventorySiteLocationRef();
		}
	}
}