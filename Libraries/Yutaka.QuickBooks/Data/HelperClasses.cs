namespace Yutaka.QuickBooks
{
	#region public class AccountRef
	public class AccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion AccountRef

	#region public class Address
	public class Address
	{
		public string Addr1;
		public string Addr2;
		public string Addr3;
		public string Addr4;
		public string Addr5;
		public string City;
		public string State;
		public string PostalCode;
		public string Country;
		public string Note;
	}
	#endregion

	#region public class AddressBlock
	public class AddressBlock
	{
		public string Addr1;
		public string Addr2;
		public string Addr3;
		public string Addr4;
		public string Addr5;
	}
	#endregion

	#region public class APAccountRef
	public class APAccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion APAccountRef

	#region public class ARAccountRef
	public class ARAccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion

	#region public class BankAccountRef
	public class BankAccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion

	#region public class ClassRef
	public class ClassRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion ClassRef


	#region public class CreditCardTxnInputInfo
	public class CreditCardTxnInputInfo
	{
		public string CreditCardNumber; //required
		public int ExpirationMonth; //required
		public int ExpirationYear; //required
		public string NameOnCard; //required
		public string CreditCardAddress;
		public string CreditCardPostalCode;
		public string CommercialCardCode;
		public string TransactionMode;
		public string CreditCardTxnType;
	}
	#endregion

	#region public class CreditCardTxnResultInfo
	public class CreditCardTxnResultInfo 
	{
		public int ResultCode; //Required
		public string ResultMessage; //Required
		public string CreditCardTransID; //Required
		public string MerchantAccountNumber; //Required
		public string AuthorizationCode;
		public string AVSStreet;
		public string AVSZip;
		public string CardSecurityCodeMatch;
		public string ReconBatchId;
		public int PaymentGroupingCode;
		public string PaymentStatus; // required
		public string TxnAuthorization; //required
		public int TxnAuthorizationStamp;
		public string ClientTransID;
	}
	#endregion

	#region public class CurrencyRef
	public class CurrencyRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion CurrencyRef

	#region public class CustomerRef
	public class CustomerRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion CustomerRef

	#region public class DataExtRet
	public class DataExtRet
	{
		public string OwnerID; //guid type in xml
		public string DataExtName; //required
		public string DataExtType; //required
		public string DataExtValue; //required
	}
	#endregion

	#region public class DiscountAccountRef
	public class DiscountAccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion

	#region public class DiscountClassRef
	public class DiscountClassRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion

	#region public class ExpenseAccountRef
	public class ExpenseAccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion ExpenseAccountRef

	#region public class IncomeAccountRef
	public class IncomeAccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion IncomeAccountRef

	#region public class InventorySiteLocationRef
	public class InventorySiteLocationRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion InventorySiteLocationRef

	#region public class InventorySiteRef
	public class InventorySiteRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion InventorySiteRef

	#region public class ItemRef
	public class ItemRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion ItemRef

	#region public class LinkedTxn

	public class LinkedTxn
	{
		public string TxnID; //required
		public string TxnType; //required
		public string TxnDate; //required
		public string RefNumber;
		public string LinkType;
		public decimal? Amount; //required

	}
	#endregion

	#region public class OverrideUOMSetRef
	public class OverrideUOMSetRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion

	#region public class ParentRef
	public class ParentRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion ParentRef

	#region public class PayeeEntityRef
	public class PayeeEntityRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion

	#region public class PaymentMethodRef
	public class PaymentMethodRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion ParentRef

	#region public class PrefVendorRef
	public class PrefVendorRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion PrefVendorRef

	#region public class PurchaseTaxCodeRef
	public class PurchaseTaxCodeRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion PurchaseTaxCodeRef

	#region public class RefundAppliedToTxnRet
	public class RefundAppliedToTxnRet
	{
		public string TxnID; //required
		public string TxnType; //required
		public string TxnDate; //datetype in xml
		public string RefNumber;
		public decimal CreditRemaining;
		public decimal RefundAmmount;
		public decimal CreditRemainingInHomecurrency;
		public decimal RefundAmountInHomeCurrency;
	}
	#endregion

	#region public class RefundFromAccountRef
	public class RefundFromAccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion

	#region public class SalesAndPurchase
	public class SalesAndPurchase
	{
		public string SalesDesc;
		public decimal? SalesPrice;
		public IncomeAccountRef IncomeAccountRef;
		public string PurchaseDesc;
		public decimal? PurchaseCost;
		public PurchaseTaxCodeRef PurchaseTaxCodeRef;
		public ExpenseAccountRef ExpenseAccountRef;
		public PrefVendorRef PrefVendorRef;

		public SalesAndPurchase()
		{
			IncomeAccountRef = new IncomeAccountRef();
			PurchaseTaxCodeRef = new PurchaseTaxCodeRef();
			ExpenseAccountRef = new ExpenseAccountRef();
			PrefVendorRef = new PrefVendorRef();
		}
	}
	#endregion SalesAndPurchase

	#region public class SalesOrPurchase
	public class SalesOrPurchase
	{
		public string Desc;
		public decimal? Price;
		public decimal? PricePercent;
		public AccountRef AccountRef;

		public SalesOrPurchase()
		{
			AccountRef = new AccountRef();
		}
	}
	#endregion SalesOrPurchase

	#region public class SalesRepRef
	public class SalesRepRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion SalesRepRef

	#region public class SalesTaxCodeRef
	public class SalesTaxCodeRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion SalesTaxCodeRef

	#region public class TermsRef
	public class TermsRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion TermsRef

	#region public class UnitOfMeasureSetRef
	public class UnitOfMeasureSetRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion UnitOfMeasureSetRef

	#region public class VendorAddress
	public class VendorAddress
	{
		public string Addr1;
		public string Addr2;
		public string Addr3;
		public string Addr4;
		public string Addr5;
		public string City;
		public string State;
		public string PostalCode;
		public string Country;
		public string Note;
	}
	#endregion VendorAddress

	#region public class VendorRef
	public class VendorRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion VendorRef
}