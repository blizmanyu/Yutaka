using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class ChargeRet
	{
		public string TxnID; // required
		public DateTime TimeCreated; // required
		public DateTime TimeModified; // required
		public string EditSequence; // required
		public int? TxnNumber;
		public CustomerRef CustomerRef; //required
		public DateTime TxnDate;
		public string RefNumber;
		public ItemRef ItemRef;
		public InventorySiteRef InventorySiteRef;
		public InventorySiteLocationRef InventorySiteLocationRef;
		public int Quantity;
		public string UnitOfMeasure;
		public OverrideUOMSetRef OverrideUOMSetRef;
		public float? Rate;
		public decimal? Amount;
		public decimal? BalanceRemaining;
		public string Desc;
		public ARAccountRef ARAccountRef;
		public ClassRef ClassRef;
		public DateTime BilledDate;
		public DateTime DueDate;
		public bool? IsPaid;
		public string ExternalGUID;
		public LinkedTxn LinkedTxn;
		public DataExtRet DataExtRet;

		public ChargeRet()
		{
			CustomerRef = new CustomerRef();
			ItemRef = new ItemRef();
			InventorySiteRef = new InventorySiteRef();
			InventorySiteLocationRef = new InventorySiteLocationRef();
			OverrideUOMSetRef = new OverrideUOMSetRef();
			ARAccountRef = new ARAccountRef();
			ClassRef = new ClassRef();
			LinkedTxn = new LinkedTxn();
			DataExtRet = new DataExtRet();

		}
	}
}
