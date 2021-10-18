using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class Deposit_CashBackInfoRet
	{
		public string TxnLineID; //required
		public AccountRef AccountRef; //required
		public string Memo;
		public decimal? Amount;

		public Deposit_CashBackInfoRet()
		{
			AccountRef = new AccountRef();
		}

	}
}
