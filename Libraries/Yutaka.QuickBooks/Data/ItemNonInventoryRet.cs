using System;

namespace Yutaka.QuickBooks
{
	public class ItemNonInventoryRet
	{
		public string ListID;
		public DateTime TimeCreated;
		public DateTime TimeModified;
		public string EditSequence;
		public string Name;
		public string FullName;
		public string BarCodeValue;
		public bool IsActive;
		public string ClassRef_ListID;
		public string ClassRef_FullName;
		public string ParentRef_ListID;
		public string ParentRef_FullName;
		public int Sublevel;
		public string ManufacturerPartNumber;
		public string UnitOfMeasureSetRef_ListID;
		public string UnitOfMeasureSetRefFullName;
		public bool IsTaxIncluded;
		public string SalesTaxCodeRef_ListID;
		public string SalesTaxCodeRef_FullName;
		public string SalesOrPurchase_Desc;
		public decimal SalesOrPurchase_Price;
		public decimal SalesOrPurchase_PricePercent;
		public string SalesOrPurchase_AccountRef_ListID;
		public string SalesOrPurchase_AccountRef_FullName;
		public string SalesAndPurchase_SalesDesc;
		public decimal SalesAndPurchase_SalesPrice;
		public string SalesAndPurchase_IncomeAccountRef_ListID;
		public string SalesAndPurchase_IncomeAccountRef_FullName;
		public string SalesAndPurchase_PurchaseDesc;
		public decimal SalesAndPurchase_PurchaseCost;
		public string SalesAndPurchase_PurchaseTaxCodeRef_ListID;
		public string SalesAndPurchase_PurchaseTaxCodeRef_FullName;
		public string SalesAndPurchase_ExpenseAccountRef_ListID;
		public string SalesAndPurchase_ExpenseAccountRef_FullName;
		public string SalesAndPurchase_PrefVendorRef_ListID;
		public string SalesAndPurchase_PrefVendorRef_FullName;
		public string ExternalGUID;
	}
}