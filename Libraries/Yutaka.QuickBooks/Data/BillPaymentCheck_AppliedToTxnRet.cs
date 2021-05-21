﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class BillPaymentCheck_AppliedToTxnRet
	{
		public string TxnID; // required
		public string TxnLineID; // required
		public DateTime TxnDate;
		public string RefNumber;
		public decimal? BalanceRemaining;
		public decimal? Amount;
		public decimal? DiscountAmount;
		public DiscountAccountRef DiscountAccountRef;
		public DiscountClassRef DiscountClassRef;
		public LinkedTxn LinkedTxn;

		public BillPaymentCheck_AppliedToTxnRet()
		{
			DiscountAccountRef = new DiscountAccountRef();
			DiscountClassRef = new DiscountClassRef();
			LinkedTxn = new LinkedTxn();

		}
	}
}
