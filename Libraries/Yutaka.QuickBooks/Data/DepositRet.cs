using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class DepositRet
	{
		public string TxnID; // required
		public DateTime TimeCreated; // required
		public DateTime TimeModified; // required
		public string EditSequence; // required 
		public int? TxnNumber;
		public DateTime TxnDate; //required
		public DepositToAccountRef DepositToAccountRef; //required
		public string Memo;
		public decimal? DepositTotal;
		public CurrencyRef CurrencyRef;
		public decimal? ExchangeRate;
		public decimal? DepositTotalInHomeCurrency;
		public List<Deposit_CashBackInfoRet> CashBackInfoRets;
		public string ExternalGUID;
		public List<Deposit_DepositLineRet> DepositLineRets;
		public DataExtRet DataExtRet;

		public DepositRet()
		{
			DepositToAccountRef = new DepositToAccountRef();
			CurrencyRef = new CurrencyRef();
			CashBackInfoRets = new List<Deposit_CashBackInfoRet>();
			DepositLineRets = new List<Deposit_DepositLineRet>();
			DataExtRet = new DataExtRet();
		}
	}
}
