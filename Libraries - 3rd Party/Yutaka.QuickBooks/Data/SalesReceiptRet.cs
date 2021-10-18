using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class SalesReceiptRet
	{
		public string TxnID; // required
		public DateTime TimeCreated; // required
		public DateTime TimeModified; // required
		public string EditSequence; // required 
		public int? TxnNumber;
		public CustomerRef CustomerRef;
		public ClassRef ClassRef;
		public TemplateRef TemplateRef;
		public DateTime TxnDate; // required
		public string RefNumber;
		public BillAddress BillAddress;
		public BillAddressBlock BillAddressBlock;
		public ShipAddress ShipAddress;
		public ShipAddressBlock ShipAddressBlock;
		public bool? IsPending;
		public string CheckNumber;
		public PaymentMethodRef PaymentMethodRef;
		public DateTime DueDate;
		public SalesRepRef SalesRepRef;
		public DateTime ShipDate;
		public ShipMethodRef ShipMethodRef;
		public string FOB;
		public decimal? Subtotal;
		public ItemSalesTaxRef ItemSalesTaxRef;
		public decimal? SalesTaxPercentage;
		public decimal? SalesTaxTotal;
		public decimal? TotalAmount;
		public CurrencyRef CurrencyRef;
		public decimal? ExchangeRate;
		public decimal? TotalAmountInHomeCurrency;
		public string Memo;
		public CustomerMsgRef CustomerMsgRef;
		public bool? IsToBePrinted;
		public bool? IsToBeEmailed;
		public bool? IsTaxIncluded;
		public CustomerSalesTaxCodeRef CustomerSalesTaxCodeRef;
		public DepositToAccountRef DepositToAccountRef;
		public List<CreditCardTxnInfo> CreditCardTxnInfo;
		public string Other;
		public string ExternalGUID;
		public DataExtRet DataExtRet;

		public SalesReceiptRet()
		{
			CustomerRef = new CustomerRef();
			ClassRef = new ClassRef();
			TemplateRef = new TemplateRef();
			BillAddress = new BillAddress();
			BillAddressBlock = new BillAddressBlock();
			ShipAddress = new ShipAddress();
			ShipAddressBlock = new ShipAddressBlock();
			PaymentMethodRef = new PaymentMethodRef();
			SalesRepRef = new SalesRepRef();
			ShipMethodRef = new ShipMethodRef();
			ItemSalesTaxRef = new ItemSalesTaxRef();
			CurrencyRef = new CurrencyRef();
			CustomerMsgRef = new CustomerMsgRef();
			CustomerSalesTaxCodeRef = new CustomerSalesTaxCodeRef();
			DepositToAccountRef = new DepositToAccountRef();
			CreditCardTxnInfo = new List<CreditCardTxnInfo>();
		}
	}
}
