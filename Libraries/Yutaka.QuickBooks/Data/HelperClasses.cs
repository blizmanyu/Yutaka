namespace Yutaka.QuickBooks
{
	#region public class AccountRef
	public class AccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion AccountRef

	#region public class AdditionalContactRef
	public class AdditionalContactRef
	{
		public string ContactName;
		public string ContactValue;
	}
	#endregion

	#region public class AdditionalNotesRet
	public class AdditionalNotesRet
	{
		public int NodeID; //required
		public string Date; //required
		public string Note; //required
	}
	#endregion

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

	#region public class AssetAccountRef
	public class AssetAccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion AssetAccountRef

	#region public class BankAccountRef
	public class BankAccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion

	#region public class BillAddress
	public class BillAddress
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

	#region public class BillAddressBlock
	public class BillAddressBlock
	{
		public string Addr1;
		public string Addr2;
		public string Addr3;
		public string Addr4;
		public string Addr5;
	}
	#endregion

	#region public class ClassRef
	public class ClassRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion ClassRef

	#region public class COGSAccountRef
	public class COGSAccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion COGSAccountRef

	#region public class CreditCardAccountRef
	public class CreditCardAccountRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion CreditCardAccountRef

	#region public class CreditCardInfo
	public class CreditCardInfo
	{
		public string CreditCardNumber; 
		public int ExpirationMonth; 
		public int ExpirationYear; 
		public string NameOnCard; 
		public string CreditCardAddress;
		public string CreditCardPostalCode;

	}
	#endregion

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
		public string TxnAuthorizationTime; //required
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

	#region public class CustomerMsgRef
	public class CustomerMsgRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion

	#region public class CustomerSalesTaxCodeRef
	public class CustomerSalesTaxCodeRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion

	#region public class CustomerRef
	public class CustomerRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion CustomerRef

	#region public class CustomerTypeRef
	public class CustomerTypeRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion

	#region public class DataExtRet
	public class DataExtRet
	{
		public string OwnerID; 
		public string DataExtName; //required
		public string DataExtType; //required
		public string DataExtValue; //required
	}
	#endregion

	#region public class DepositToAccountRef
	public class DepositToAccountRef
	{
		public string ListID;
		public string FullName;
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

	#region public class EntityRef
	public class EntityRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion

	#region public class ErrorRecovery
	public class ErrorRecovery
	{
		public string ListID;
		public string TxnNumber;
		public string EditSequence;
		public string ExternalGUID;
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

	#region public class ItemLineRet
	public class ItemLineRet
	{
		public string TxnLineID;
		public ItemRef ItemRef;
		public InventorySiteRef InventorySiteRef;
		public InventorySiteLocationRef InventorySiteLocationRef;
		public string SerialNumber;
		public string LotNumber;
		public string Desc;
		public decimal? Quantity;
		public string UnitOfMeasure;
		public OverrideUOMSetRef OverrideUOMSetRef;
		public decimal? Cost;
		public decimal? Amount;
		public CustomerRef CustomerRef;
		public ClassRef ClassRef;
		public SalesTaxCodeRef SalesTaxCodeRef;
		public string BillableStatus;
		public SalesRepRef SalesRepRef;
		public DataExtRet DataExtRet;

		public ItemLineRet()
		{
			ItemRef = new ItemRef();
			InventorySiteRef = new InventorySiteRef();
			InventorySiteLocationRef = new InventorySiteLocationRef();
			OverrideUOMSetRef = new OverrideUOMSetRef();
			CustomerRef = new CustomerRef();
			ClassRef = new ClassRef();
			SalesTaxCodeRef = new SalesTaxCodeRef();
			SalesRepRef = new SalesRepRef();
			DataExtRet = new DataExtRet();
		}

	}
	#endregion

	#region public class ItemSalesTaxRef
	public class ItemSalesTaxRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion ItemSalesTaxRef

	#region public class JobTypeRef
	public class JobTypeRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion JobTypeRef

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

	#region public class PreferredPaymentMethodRef
	public class PreferredPaymentMethodRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion PreferredPaymentMethodRef

	#region public class PriceLevelRef
	public class PriceLevelRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion PriceLevelRef

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

	#region public class ShipAddress
	public class ShipAddress
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

	#region public class ShipAddressBlock
	public class ShipAddressBlock
	{
		public string Addr1;
		public string Addr2;
		public string Addr3;
		public string Addr4;
		public string Addr5;
	}
	#endregion

	#region public class ShipMethodRef
	public class ShipMethodRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion ShipMethodRef

	#region public class ShipToAddress
	public class ShipToAddress
	{
		public string Name; //required
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
		public bool? DefaultShipTo;
	}
	#endregion

	#region public class TaxLineInfoRet
	public class TaxLineInfoRet
	{
		public string TaxLineID;
		public string TaxLineName;
	}
	#endregion TaxLineInfoRet

	#region public class TemplateRef
	public class TemplateRef
	{
		public string ListID;
		public string FullName;
	}
	#endregion TemplateRef

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