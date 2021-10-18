using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class Deposit_DepositLineRet
	{
		public string TxnType;
		public string TxnID;
		public string TxnLineID;
		public string PaymentTxnLineID;
		public EntityRef EntityRef;
		public AccountRef AccountRef;
		public string Memo;
		public string CheckNumber;
		public PaymentMethodRef PaymentMethodRef;
		public ClassRef ClassRef;
		public decimal? Amount;

		public Deposit_DepositLineRet()
		{
			EntityRef = new EntityRef();
			AccountRef = new AccountRef();
			PaymentMethodRef = new PaymentMethodRef();
			ClassRef = new ClassRef();
		}
	}
}
