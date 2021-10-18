using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class CustomerRet
	{
		public string ListID; // required
		public DateTime TimeCreated; // required
		public DateTime TimeModified; // required
		public string EditSequence; // required 
		public string Name; //required
		public string FullName; //required
		public bool? IsActive;
		public ClassRef ClassRef;
		public ParentRef ParentRef;
		public int? Sublevel; //required
		public string CompanyName;
		public string Salutation;
		public string FirstName;
		public string MiddleName;
		public string LastName;
		public string JobTitle;
		public BillAddress BillAddress;
		public BillAddressBlock BillAddressBlock;
		public ShipAddress ShipAddress;
		public ShipAddressBlock ShipAddressBlock;
		public ShipToAddress ShipToAddress;
		public string Phone;
		public string AltPhone;
		public string Fax;
		public string Email;
		public string Cc;
		public string Contact;
		public string AltContact;
		public AdditionalContactRef AdditionalContactRef;
		public List<Customer_ContactsRet> ContactsRet;
		public CustomerTypeRef CustomerTypeRef;
		public TermsRef TermsRef;
		public SalesRepRef SalesRepRef;
		public decimal? Balance;
		public decimal? TotalBalance;
		public SalesTaxCodeRef SalesTaxCodeRef;
		public ItemSalesTaxRef ItemSalesTaxRef;
		public string SalesTaxCountry;
		public string ResaleNumber;
		public string AccountNumber;
		public decimal? CreditLimit;
		public PreferredPaymentMethodRef PreferredPaymentMethodRef;
		public CreditCardInfo CreditCardInfo;
		public string JobStatus;
		public DateTime JobStartDate;
		public DateTime JobProjectedEndDate;
		public DateTime JobEndDate;
		public JobTypeRef JobTypeRef;
		public string Notes;
		public AdditionalNotesRet AdditionalNotesRet;
		public string PreferredDeliveryMethod;
		public PriceLevelRef PriceLevelRef;
		public string ExternalGUID;
		public string TaxRegistrationNumber;
		public CurrencyRef CurrencyRef;
		public DataExtRet DataExtRet;

		public CustomerRet()
		{
			ClassRef = new ClassRef();
			ParentRef = new ParentRef();
			BillAddress = new BillAddress();
			BillAddressBlock = new BillAddressBlock();
			ShipAddress = new ShipAddress();
			ShipAddressBlock = new ShipAddressBlock();
			ShipToAddress = new ShipToAddress();
			AdditionalContactRef = new AdditionalContactRef();
			ContactsRet = new List<Customer_ContactsRet>();
			CustomerTypeRef = new CustomerTypeRef();
			TermsRef = new TermsRef();
			SalesRepRef = new SalesRepRef();
			SalesTaxCodeRef = new SalesTaxCodeRef();
			ItemSalesTaxRef = new ItemSalesTaxRef();
			PreferredPaymentMethodRef = new PreferredPaymentMethodRef();
			CreditCardInfo = new CreditCardInfo();
			JobTypeRef = new JobTypeRef();
			AdditionalNotesRet = new AdditionalNotesRet();
			PriceLevelRef = new PriceLevelRef();
			CurrencyRef = new CurrencyRef();
			DataExtRet = new DataExtRet();
			
		}
	}
}
