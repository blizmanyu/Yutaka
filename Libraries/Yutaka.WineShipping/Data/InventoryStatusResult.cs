namespace Yutaka.WineShipping
{
	public class InventoryStatusResult
	{
		public int? TotalRecordCount { get; set; }
		public int? Skip { get; set; }
		public int? Top { get; set; }
		public bool? MoreRecords { get; set; }
		public WarehouseInventoryStatus[] WarehouseInventoryStatuses { get; set; }
	}

	public class WarehouseInventoryStatus
	{
		public string CustomerNo { get; set; }
		public string CustomerName { get; set; }
		public string ItemNo { get; set; }
		public string ItemDescription { get; set; }
		public string ItemUnit { get; set; }
		public string InventoryWarehouse { get; set; }
		public string InventoryStatus { get; set; }
		public int? OnHandQuantity { get; set; }
		public int? ReservedQuantity { get; set; }
		public int? AvailableQuantity { get; set; }
		public int? BackOrderQuantity { get; set; }
		public int? OpenPOQuantity { get; set; }
	}
}