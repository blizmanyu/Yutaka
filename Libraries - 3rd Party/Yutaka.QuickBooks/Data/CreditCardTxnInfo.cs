using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class CreditCardTxnInfo
	{
		public CreditCardTxnInputInfo CreditCardTxnInputInfo; // required
		public CreditCardTxnResultInfo CreditCardTxnResultInfo; // required

		public CreditCardTxnInfo()
		{
			CreditCardTxnInputInfo = new CreditCardTxnInputInfo();
			CreditCardTxnResultInfo = new CreditCardTxnResultInfo();
		}
	}
}
