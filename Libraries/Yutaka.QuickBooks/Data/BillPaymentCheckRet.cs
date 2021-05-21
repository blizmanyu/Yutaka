﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class BillPaymentCheckRet
	{
		public string TxnID; 
		public DateTime TimeCreated;
		public DateTime TimeModified; 
		public string EditSequence; 
		public int? TxnNumber;
		public PayeeEntityRef PayeeEntityRef;
		public APAccountRef APAccountRef;
		public DateTime TxnDate;
		public BankAccountRef BankAccountRef;
		public decimal? Amount;
		public CurrencyRef CurrencyRef;
		public decimal? ExchangeRate;
		public decimal? AmountDueInHomeCurrency;
		public string RefNumber;
		public string Memo;
		public Address Address;
		public AddressBlock AddressBlock;
		public bool? IsToBePrinted;
		public string ExternalGUID;
		public List<BillPaymentCheck_AppliedToTxnRet> AppliedToTxnRets;
		public DataExtRet DataExtRet;

	}
}
