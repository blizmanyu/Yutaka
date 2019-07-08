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
		public ClassRef ClassRef;
		public ParentRef ParentRef;
		public int Sublevel;
		public string ManufacturerPartNumber;
		public UnitOfMeasureSetRef UnitOfMeasureSetRef;
		public bool IsTaxIncluded;
		public SalesTaxCodeRef SalesTaxCodeRef;
		public SalesOrPurchase SalesOrPurchase;
		public SalesAndPurchase SalesAndPurchase;
		public string ExternalGUID;

		public ItemNonInventoryRet()
		{
			ClassRef = new ClassRef();
			ParentRef = new ParentRef();
			UnitOfMeasureSetRef = new UnitOfMeasureSetRef();
			SalesTaxCodeRef = new SalesTaxCodeRef();
			SalesOrPurchase = new SalesOrPurchase();
			SalesAndPurchase = new SalesAndPurchase();
		}
	}
}