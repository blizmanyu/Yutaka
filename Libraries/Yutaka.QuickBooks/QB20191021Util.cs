using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Interop.QBXMLRP2Lib;
using Yutaka.QuickBooks.Data;

namespace Yutaka.QuickBooks
{
	/// <summary>
	/// To support a new Action/Query, simply perform these 5 steps:
	/// 1) Add the Action to the Enum ActionType.
	/// 2) Look at the Response portion of the XMLOps from the API Docs to create a C# class that corresponds to the Ret object.
	/// 3) In the method BuildRequest(), add a case to build the request XML for your action. Make sure you read/handle the params object that comes
	///	   in for you specific action.
	///	4) If your action is a Query, add a case in the ProcessReturn() method to convert the response XML to a list of ret class you created in number (2).
	///	   Also do this if your action isn't a query, but you still want to see the returned object.
	///	5) Create a public method that takes in logically-named parameters that converts them to the more-general params object to pass into DoAction().
	///	   If you want your public method to return a list, "upcast" it to a list of your Ret objects instead of generic objects.
	/// </summary>
	/// 

	//@TODO Make sure all parameters are added into foreach

	public class QB20191021Util
	{
		#region protected enum ActionType { .... }
		protected enum ActionType
		{
			AccountAdd,
			AccountMod,
			AccountQuery,
			ARRefundCreditCardQuery,
			BillAdd,
			BillQuery,
			BillPaymentCheckAdd,
			BillPaymentCheckQuery,
			BillPaymentCreditCardAdd,
			BillPaymentCreditCardQuery,
			ChargeAdd,
			ChargeQuery,
			CheckAdd,
			CheckQuery,
			CreditCardChargeAdd,
			CreditCardChargeQuery,
			CreditCardCreditAdd,
			CreditCardCreditQuery,
			CustomerAdd,
			CustomeryQuery,
			DepositQuery,
			InventoryAdjustmentAdd,
			InventoryAdjustmentQuery,
			ItemNonInventoryQuery,
			ReceivePaymentQuery,
			SalesReceiptQuery,
		};
		#endregion protected enum ActionType { .... }

		#region Fields
		protected const string DEFAULT_APP_NAME = "QB20191021Util";
		protected const string QB_FORMAT = "yyyy-MM-ddTHH:mm:ssK";
		protected readonly DateTime MIN_DATE = DateTime.Now.AddYears(-10);
		protected readonly DateTime MAX_DATE = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59, 999, DateTimeKind.Local);
		protected RequestProcessor2 Rp;
		protected bool ConnectionOpen;
		protected bool SessionBegun;
		protected string SessionId;

		public bool Debug;
		#endregion Fields

		public QB20191021Util()
		{
			Debug = false;
			Rp = null;
			ConnectionOpen = false;
			SessionBegun = false;
			SessionId = null;
		}

		#region Utilities
		protected string BeautifyXml(string xml)
		{
			if (String.IsNullOrWhiteSpace(xml))
				return "";

			using (var sw = new StringWriter()) {
				var xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(xml);
				xmlDoc.Save(sw);

				return sw.ToString();
			}
		}

		protected void BuildRequest(XmlDocument doc, XmlElement parent, ActionType actionType, params KeyValuePair<string, object>[] parameters)
		{
			var request = doc.CreateElement(String.Format("{0}Rq", actionType.ToString()));
			parent.AppendChild(request);

			#region XmlElements
			XmlElement 
				AccountAdd,
				AccountMod,
				AccountQuery,
				AccountRef,
				Address,
				AdditionalContactRef,
				AdditionalNotes,
				APAccountRef,
				ApplyCheckToTxnAdd,
				ARAccountRef,
				AppliedToTxnRet,
				BankAccountRef,
				BillAdd,
				BillAddress,
				BillPaymentCheckAdd,
				BillPaymentCreditCardAdd,
				ChargeAdd,
				CheckAdd,
				Contacts,
				CreditCardAccountRef,
				CreditCardChargeAdd,
				CreditCardCreditAdd,
				CreditCardInfo,
				ClassRef,
				CurrencyRef,
				CustomerAdd,
				CustomerRef,
				CustomerTypeRef,
				DataExt,
				DiscountAccountRef,
				DiscountClassRef,
				ExpenseLineAdd,
				InventoryAdjustmentAdd, 
				InventoryAdjustmentLineAdd, 
				InventorySiteRef,
				InventorySiteLocationRef,
				ItemLineAdd,
				ItemRef,
				ItemSalesTaxRef,
				JobTypeRef,
				NameFilter,
				OverrideItemAccountRef,
				ParentRef,
				PriceLevelRef,
				QuantityAdjustment,
				ModifiedDateRangeFilter,
				PayeeEntityRef,
				PreferredPaymentMethodRef,
				SalesRepRef,
				SalesTaxCodeRef, 
				SetCredit,
				ShipAddress,
				ShipToAddress,
				TermsRef,
				VendorAddress,
				VendorRef;

			#endregion

			#region Go through parameters

			#region Simple Element Parameters
			var accountRefListId = "";
			var dtFrom = "";
			var dtTo = "";
			var quantityDifference = 0;
			//new simple elements, unsure of default values for bool/int values
			var accountNumber = "";
			var accountRefFullName = "";
			var accountType = "";
			var altContact = "";
			var altPhone = "";
			var amount = "";
			var bankNumber = "";
			var billableStatus = "";
			var billedDate = "";
			var cc = "";
			var companyName = "";
			var contact = "";
			var cost = "";
			var creditLimit = "";
			var desc = "";
			var dueDate = "";
			var editSequence = "";
			var email = "";
			var exchangeRate ="";
			var externalGUID = "";
			var fax = "";
			var firstName = "";
			var isActive = "";
			var isTaxIncluded = false;
			var isToBePrinted = false;
			var jobDesc = "";
			var jobEndDate = "";
			var jobProjectedEndDate = "";
			var jobStatus = "";
			var jobStartDate = "";
			var jobTitle = "";
			var lastName = "";
			var listID = "";
			var linkToTxnId = "";
			var matchCriterion = "";
			var memo = "";
			var middleName = "";
			var name = "";
			var notes = "";
			var openBalance = "";
			var openBalanceDate = "";
			var optionForPriceRuleConflict = "";
			var paymentAmount = 0;
			var phone = "";
			var preferredDeliveryMethod = "";
			var quantity = 0;
			var rate = 0;
			var refNumber = "";
			var resaleNumber = "";
			var salutation = "";
			var salesTaxCountry = "";
			var serialNumber = "";
			var taxLineID = "";
			var taxRegistrationNumber = "";
			var txnDate = "";
			var txnId = "";
			var unitOfMeasure = "";
			#endregion

			#region AdditionalContact
			var additionalContactRefName = "";
			var additionalContactRefValue = "";
			#endregion

			#region AdditionalNotes
			var additionalNotesNote = "";
			#endregion

			#region Address
			var addr1 = "";
			var addr2 = "";
			var addr3 = "";
			var addr4 = "";
			var addr5 = "";
			var city = "";
			var country = "";
			var state = "";
			var postalCode = "";
			var note = "";
			#endregion

			#region ShipAddress
			var shipAddr1 = "";
			var shipAddr2 = "";
			var shipAddr3 = "";
			var shipAddr4 = "";
			var shipAddr5 = "";
			var shipCity = "";
			var shipState = "";
			var shipPostalCode = "";
			var shipCountry = "";
			var shipNote = "";
			#endregion
			
			#region BillAddress
			var billAddr1 = "";
			var billAddr2 = "";
			var billAddr3 = "";
			var billAddr4 = "";
			var billAddr5 = "";
			var billCity = "";
			var billState = "";
			var billPostalCode = "";
			var billCountry = "";
			var billNote = "";
			#endregion

			#region Ship To Address
			var shipToAddr1 = "";
			var shipToAddr2 = "";
			var shipToAddr3 = "";
			var shipToAddr4 = "";
			var shipToAddr5 = "";
			var shipToCity = "";
			var shipToState = "";
			var shipToPostalCode = "";
			var shipToCountry = "";
			var shipToNote = "";
			var shipToDefaultShipTo = "";
			#endregion

			#region VendorRef
			var vendorRefListId = "";
			var vendorRefFullName = "";
			var vendorRefAddr1 = "";
			var vendorRefAddr2 = "";
			var vendorRefAddr3 = "";
			var vendorRefAddr4 = "";
			var vendorRefAddr5 = "";
			var vendorRefCity = "";
			var vendorRefState = "";
			var vendorRefPostalCode = "";
			var vendorRefCountry = "";
			var vendorRefNote = "";
			#endregion

			#region APAccountRef 
			var apAccountRefListId = "";
			var apAccountRefFullName = "";
			#endregion

			#region ApplyCheckToTxnAdd
			var applyCheckToTxnAddTxnID = "";
			var applyCheckToTxnAddAmount = "";
			#endregion

			#region ARAccountRef 
			var arAccountRefListId = "";
			var arAccountRefFullName = "";
			#endregion

			#region TermsRef
			var termsRefListId = "";
			var termsRefFullName = "";
			#endregion

			#region SalesTaxCodeRef 
			var salesTaxCodeRefListId = "";
			var salesTaxCodeRefFullName = "";
			#endregion

			#region ItemRef
			var itemRefListId = "";
			var itemRefFullName = "";
			#endregion

			#region InventorySiteRef
			var inventorySiteRefListId = "";
			var inventorySiteRefFullName = "";
			#endregion

			#region InventorySiteLocationRef
			var inventorySiteLocationRefListId = "";
			var inventorySiteLocationRefFullName = "";
			#endregion

			#region CustomerRef
			var customerRefListId = "";
			var customerRefFullName = "";
			#endregion

			#region ClassRef
			var classRefListId = "";
			var classRefFullName = "";
			#endregion

			#region BankAccountRef
			var bankAccountRefListId = "";
			var bankAccountRefFullName = "";
			#endregion

			#region AppliedToTxnAdd
			var appliedToTxnAdd_TxnID = "";
			var appliedToTxnAdd_PaymentAmount = "";
			var appliedToTxnAdd_SetCredit_CreditTxnId = "";
			var appliedToTxnAdd_SetCredit_AppliedAmount = 0;
			var appliedToTxnAdd_SetCredit_Override = false;

			var appliedToTxnAdd_DiscountAmount = 0;
			var appliedToTxnAdd_DiscountAccountRefListId = "";
			var appliedToTxnAdd_DiscountAccountRefFullName = "";
			var appliedToTxnAdd_DiscountClassRefListId = "";
			var appliedToTxnAdd_DiscountClassRefFullName = "";
			#endregion

			#region PayeeEntityRef
			var payeeEntityRefListId = "";
			var payeeEntityRefFullName = "";
			#endregion

			#region CreditCardAccountRef
			var creditCardAccountRefListId = "";
			var creditCardAccountRefFullName = "";
			#endregion

			#region OverrideItemAccountRef
			var overrideItemAccountRefListId = "";
			var overrideItemAccountRefFullName = "";
			#endregion

			#region SalesRepRef 
			var salesRepRefListId = "";
			var salesRepRefFullName = "";
			#endregion

			#region ParentRef 
			var parentRefListId = "";
			var parentRefFullName = "";
			#endregion

			#region DataExt
			var dataExtOwnerId = "";
			var dataExtName = "";
			var dataExtValue = "";
			#endregion

			#region CustomerTypeRef
			var customerTypeRefListId = "";
			var customerTypeRefFullName = "";
			#endregion

			#region ItemSalesTaxRef
			var itemSalesTaxRefListId = "";
			var itemSalesTaxRefFullName = "";
			#endregion

			#region PreferredPaymentMethodRef
			var preferredPaymentMethodRefListId = "";
			var preferredPaymentMethodRefFullName = "";
			#endregion

			#region CreditCardInfo
			var creditCardNumber = "";
			var expirationMonth = "";
			var expirationYear = "";
			var nameOnCard = "";
			var creditCardAddress = "";
			var creditCardPostalCode = "";
			#endregion

			#region JobTypeRef
			var jobTypeRefListId = "";
			var jobTypeRefFullName = "";
			#endregion

			#region PriceLevelRef
			var priceLevelRefListId = "";
			var priceLevelRefFullName = "";
			#endregion

			#region CurrencyRef
			var currencyRefListId = "";
			var currencyRefFullName = "";
			#endregion

			foreach (var param in parameters) {
				switch (param.Key) {
					case "accountType":
						accountType = param.Value.ToString();
						break;
					case "accountRefListId":
						accountRefListId = param.Value.ToString();
						break;
					case "apAccountRefListId":
						apAccountRefListId = param.Value.ToString();
						break;
					case "apAccountRefFullName":
						apAccountRefFullName = param.Value.ToString();
						break;
					case "bankAccountRefListId":
						apAccountRefListId = param.Value.ToString();
						break;
					case "bankAccountRefFullName":
						apAccountRefFullName = param.Value.ToString();
						break;
					case "bankNumber":
						bankNumber = param.Value.ToString();
						break;
					case "billableStatus":
						billableStatus = param.Value.ToString();
						break;
					case "classRefListId":
						classRefListId = param.Value.ToString();
						break;
					case "classRefFullName":
						classRefFullName = param.Value.ToString();
						break;
					case "dueDate":
						dueDate = param.Value.ToString();
						break;
					case "dtFrom":
						dtFrom = param.Value.ToString();
						break;
					case "dtTo":
						dtTo = param.Value.ToString();
						break;
					case "exchangeRate":
						exchangeRate = param.Value.ToString();
						break;
					case "externalGUID":
						externalGUID = param.Value.ToString();
						break;
					case "inventorySiteLocationRefListId":
						inventorySiteLocationRefListId = param.Value.ToString();
						break;
					case "inventorySiteLocationRefFullName":
						inventorySiteLocationRefFullName = param.Value.ToString();
						break;
					case "inventorySiteRefListId":
						inventorySiteRefListId = param.Value.ToString();
						break;
					case "inventorySiteRefFullName":
						inventorySiteRefFullName = param.Value.ToString();
						break;
					case "itemRefListId":
						itemRefListId = param.Value.ToString();
						break;
					case "itemRefFullName":
						itemRefFullName = param.Value.ToString();
						break;
					case "isTaxIncluded":
						isTaxIncluded = (bool) param.Value;
						break;
					case "linkToTxnId":
						linkToTxnId = param.Value.ToString();
						break;
					case "memo":
						memo = param.Value.ToString();
						break;
					case "quantityDifference":
						quantityDifference = (int) param.Value;
						break;
					case "refNumber":
						refNumber = param.Value.ToString();
						break;
					case "serialNumber":
						serialNumber = param.Value.ToString();
						break;
					case "taxLineID":
						taxLineID = param.Value.ToString();
						break;
					case "txnDate":
						txnDate = param.Value.ToString();
						break;
					default:
						break;
				}
			}

			#endregion Go through parameters

			switch (actionType) {

				#region AccountAdd
				case ActionType.AccountAdd:
					AccountAdd = doc.CreateElement("AccountAdd");
					request.AppendChild(AccountAdd);

					AccountAdd.AppendChild(MakeSimpleElem(doc, "Name", name));
					AccountAdd.AppendChild(MakeSimpleElem(doc, "IsActive", isActive));

					#region ParentRef
					ParentRef = doc.CreateElement("ParentRef");
					AccountAdd.AppendChild(ParentRef);
					ParentRef.AppendChild(MakeSimpleElem(doc, "ListID", parentRefListId));
					ParentRef.AppendChild(MakeSimpleElem(doc, "FullName", parentRefFullName));
					#endregion

					AccountAdd.AppendChild(MakeSimpleElem(doc, "AccountType", accountType));
					AccountAdd.AppendChild(MakeSimpleElem(doc, "AccountNumber", accountNumber));
					AccountAdd.AppendChild(MakeSimpleElem(doc, "BankNumber", bankNumber));
					AccountAdd.AppendChild(MakeSimpleElem(doc, "Desc", desc));
					AccountAdd.AppendChild(MakeSimpleElem(doc, "OpenBalance", openBalance));
					AccountAdd.AppendChild(MakeSimpleElem(doc, "OpenBalanceDate", openBalanceDate));

					#region SalesTaxCodeRef
					SalesTaxCodeRef = doc.CreateElement("SalesTaxCodeRef");
					AccountAdd.AppendChild(SalesTaxCodeRef);
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "ListID", salesTaxCodeRefListId));
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "FullName", salesTaxCodeRefFullName));
					#endregion

					AccountAdd.AppendChild(MakeSimpleElem(doc, "TaxLineID", taxLineID));

					#region CurrencyRef
					CurrencyRef = doc.CreateElement("CurrencyRef");
					AccountAdd.AppendChild(CurrencyRef);
					CurrencyRef.AppendChild(MakeSimpleElem(doc, "ListID", currencyRefListId));
					CurrencyRef.AppendChild(MakeSimpleElem(doc, "FullName", currencyRefFullName));
					#endregion

					break;
				#endregion

				#region AccountMod
				case ActionType.AccountMod:
					AccountMod = doc.CreateElement("AccountMod");
					request.AppendChild(AccountMod);

					AccountMod.AppendChild(MakeSimpleElem(doc, "ListID", listID));
					AccountMod.AppendChild(MakeSimpleElem(doc, "EditSequence", editSequence));

					#region ParentRef
					ParentRef = doc.CreateElement("ParentRef");
					AccountMod.AppendChild(ParentRef);
					ParentRef.AppendChild(MakeSimpleElem(doc, "ListID", parentRefListId));
					ParentRef.AppendChild(MakeSimpleElem(doc, "FullName", parentRefFullName));
					#endregion

					AccountMod.AppendChild(MakeSimpleElem(doc, "AccountType", accountType));
					AccountMod.AppendChild(MakeSimpleElem(doc, "AccountNumber", accountNumber));
					AccountMod.AppendChild(MakeSimpleElem(doc, "BankNumber", bankNumber));
					AccountMod.AppendChild(MakeSimpleElem(doc, "Desc", desc));
					AccountMod.AppendChild(MakeSimpleElem(doc, "OpenBalance", openBalance));
					AccountMod.AppendChild(MakeSimpleElem(doc, "OpenBalanceDate", openBalanceDate));

					#region SalesTaxCodeRef
					SalesTaxCodeRef = doc.CreateElement("SalesTaxCodeRef");
					AccountMod.AppendChild(SalesTaxCodeRef);
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "ListID", salesTaxCodeRefListId));
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "FullName", salesTaxCodeRefFullName));
					#endregion

					AccountMod.AppendChild(MakeSimpleElem(doc, "TaxLineID", taxLineID));

					#region CurrencyRef
					CurrencyRef = doc.CreateElement("CurrencyRef");
					AccountMod.AppendChild(CurrencyRef);
					CurrencyRef.AppendChild(MakeSimpleElem(doc, "ListID", currencyRefListId));
					CurrencyRef.AppendChild(MakeSimpleElem(doc, "FullName", currencyRefFullName));
					#endregion
					break;
				#endregion

				#region AccountQuery
				case ActionType.AccountQuery:
					NameFilter = doc.CreateElement("NameFilter");
					request.AppendChild(NameFilter);
					NameFilter.AppendChild(MakeSimpleElem(doc, "MatchCriterion", matchCriterion));
					NameFilter.AppendChild(MakeSimpleElem(doc, "Name", name));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion

				#region ARRefundCreditCardQuery
				case ActionType.ARRefundCreditCardQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion

				// ExpenseLineAdd, ItemLineAdd, ItemGroupLineAdd  may be added but are omitted // 
				#region BillAdd
				case ActionType.BillAdd:
					BillAdd = doc.CreateElement("BillAdd");
					request.AppendChild(BillAdd);

					#region VendorRef
					VendorRef = doc.CreateElement("VendorRef");
					BillAdd.AppendChild(VendorRef);
					VendorRef.AppendChild(MakeSimpleElem(doc, "ListID", vendorRefListId));
					VendorRef.AppendChild(MakeSimpleElem(doc, "FullName", vendorRefFullName));

					#endregion

					#region VendorAddress
					VendorAddress = doc.CreateElement("VendorAddress");
					BillAdd.AppendChild(VendorAddress);
					VendorRef.AppendChild(MakeSimpleElem(doc, "Addr1", vendorRefAddr1));
					VendorRef.AppendChild(MakeSimpleElem(doc, "Addr2", vendorRefAddr2));
					VendorRef.AppendChild(MakeSimpleElem(doc, "Addr3", vendorRefAddr3));
					VendorRef.AppendChild(MakeSimpleElem(doc, "Addr4", vendorRefAddr4));
					VendorRef.AppendChild(MakeSimpleElem(doc, "Addr5", vendorRefAddr5));
					VendorRef.AppendChild(MakeSimpleElem(doc, "City", vendorRefCity));
					VendorRef.AppendChild(MakeSimpleElem(doc, "State", vendorRefState));
					VendorRef.AppendChild(MakeSimpleElem(doc, "PostalCode", vendorRefPostalCode));
					VendorRef.AppendChild(MakeSimpleElem(doc, "Country", vendorRefCountry));
					VendorRef.AppendChild(MakeSimpleElem(doc, "Note", vendorRefNote));
					#endregion

					#region APAccountRef
					APAccountRef = doc.CreateElement("APAccountRef");
					BillAdd.AppendChild(APAccountRef);
					APAccountRef.AppendChild(MakeSimpleElem(doc, "ListID", apAccountRefListId));
					APAccountRef.AppendChild(MakeSimpleElem(doc, "FullName", apAccountRefFullName));
					#endregion

					BillAdd.AppendChild(MakeSimpleElem(doc, "TxnDate", txnDate));
					BillAdd.AppendChild(MakeSimpleElem(doc, "DueDate", dueDate));
					BillAdd.AppendChild(MakeSimpleElem(doc, "RefNumber", refNumber));

					#region TermsRef
					TermsRef = doc.CreateElement("TermsRef");
					BillAdd.AppendChild(TermsRef);
					TermsRef.AppendChild(MakeSimpleElem(doc, "ListID", termsRefListId));
					TermsRef.AppendChild(MakeSimpleElem(doc, "FullName", termsRefFullName));
					#endregion

					BillAdd.AppendChild(MakeSimpleElem(doc, "Memo", memo));

					#region SalesTaxCodeRef
					SalesTaxCodeRef = doc.CreateElement("SalesTaxCodeRef");
					BillAdd.AppendChild(SalesTaxCodeRef);
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "ListID", salesTaxCodeRefListId));
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "FullName", salesTaxCodeRefFullName));
					#endregion

					BillAdd.AppendChild(MakeSimpleElem(doc, "ExchangeRate", exchangeRate));
					BillAdd.AppendChild(MakeSimpleElem(doc, "ExternalGUID", externalGUID));
					BillAdd.AppendChild(MakeSimpleElem(doc, "LinkToTxnID", linkToTxnId));

					break;
				#endregion

				#region BillQuery
				case ActionType.BillQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion
				
				//must include either PaymentAmount, SetCredit, or DiscountAmount (or more than one of these) within AppliedToTxnAdd
				#region BillPaymentCheckAdd
				case ActionType.BillPaymentCheckAdd:
					BillPaymentCheckAdd = doc.CreateElement("BillPaymentCheckAdd");
					request.AppendChild(BillPaymentCheckAdd);

					#region PayeeEntityRef
					PayeeEntityRef = doc.CreateElement("PayeeEntityRef");
					BillPaymentCheckAdd.AppendChild(PayeeEntityRef);
					PayeeEntityRef.AppendChild(MakeSimpleElem(doc, "ListID", payeeEntityRefListId));
					PayeeEntityRef.AppendChild(MakeSimpleElem(doc, "FullName", payeeEntityRefFullName));
					#endregion

					#region APAccountRef
					APAccountRef = doc.CreateElement("APAccountRef");
					BillPaymentCheckAdd.AppendChild(APAccountRef);
					APAccountRef.AppendChild(MakeSimpleElem(doc, "ListID", apAccountRefListId));
					APAccountRef.AppendChild(MakeSimpleElem(doc, "FullName", apAccountRefFullName));
					#endregion

					BillPaymentCheckAdd.AppendChild(MakeSimpleElem(doc, "TxnDate", txnDate));

					#region BankAccountRef
					BankAccountRef = doc.CreateElement("BankAccountRef");
					BillPaymentCheckAdd.AppendChild(BankAccountRef);
					BankAccountRef.AppendChild(MakeSimpleElem(doc, "ListID", bankAccountRefListId));
					BankAccountRef.AppendChild(MakeSimpleElem(doc, "FullName", bankAccountRefFullName));
					#endregion

					BillPaymentCheckAdd.AppendChild(MakeSimpleElem(doc, "IsToBePrinted", isToBePrinted.ToString()));
					BillPaymentCheckAdd.AppendChild(MakeSimpleElem(doc, "RefNumber", refNumber));
					BillPaymentCheckAdd.AppendChild(MakeSimpleElem(doc, "Memo", memo));
					BillPaymentCheckAdd.AppendChild(MakeSimpleElem(doc, "ExchangeRate", exchangeRate));
					BillPaymentCheckAdd.AppendChild(MakeSimpleElem(doc, "ExternalGUID", externalGUID));

					#region AppliedToTxnRet
					AppliedToTxnRet = doc.CreateElement("AppliedToTxnRet");
					BillPaymentCheckAdd.AppendChild(AppliedToTxnRet);

					AppliedToTxnRet.AppendChild(MakeSimpleElem(doc, "TxnID", appliedToTxnAdd_TxnID));
					AppliedToTxnRet.AppendChild(MakeSimpleElem(doc, "PaymentAmount", appliedToTxnAdd_PaymentAmount.ToString()));

					SetCredit = doc.CreateElement("SetCredit");
					AppliedToTxnRet.AppendChild(SetCredit);
					SetCredit.AppendChild(MakeSimpleElem(doc, "CreditTxnId", appliedToTxnAdd_SetCredit_CreditTxnId));
					SetCredit.AppendChild(MakeSimpleElem(doc, "AppliedAmount", appliedToTxnAdd_SetCredit_AppliedAmount.ToString()));
					SetCredit.AppendChild(MakeSimpleElem(doc, "CreditTxnId", appliedToTxnAdd_SetCredit_Override.ToString()));

					AppliedToTxnRet.AppendChild(MakeSimpleElem(doc, "DiscountAmount", appliedToTxnAdd_DiscountAmount.ToString()));

					DiscountAccountRef = doc.CreateElement("DiscountAccountRef");
					BillPaymentCheckAdd.AppendChild(DiscountAccountRef);
					DiscountAccountRef.AppendChild(MakeSimpleElem(doc, "ListID", appliedToTxnAdd_DiscountAccountRefListId));
					DiscountAccountRef.AppendChild(MakeSimpleElem(doc, "FullName", appliedToTxnAdd_DiscountAccountRefFullName));

					DiscountClassRef = doc.CreateElement("DiscountClassRef");
					BillPaymentCheckAdd.AppendChild(DiscountClassRef);
					DiscountClassRef.AppendChild(MakeSimpleElem(doc, "ListID", appliedToTxnAdd_DiscountClassRefListId));
					DiscountClassRef.AppendChild(MakeSimpleElem(doc, "FullName", appliedToTxnAdd_DiscountClassRefFullName));

					#endregion

					break;
				#endregion

				#region BillPaymentCheckQuery
				case ActionType.BillPaymentCheckQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion

				//BillPaymentCreditCardAdd request must include either PaymentAmount, SetCredit, or DiscountAmount (or more than one of these)
				#region BillPaymentCreditCardAdd
				case ActionType.BillPaymentCreditCardAdd:
					BillPaymentCreditCardAdd = doc.CreateElement("BillPaymentCreditCardAdd");
					request.AppendChild(BillPaymentCreditCardAdd);

					#region PayeeEntityRef
					PayeeEntityRef = doc.CreateElement("PayeeEntityRef");
					BillPaymentCreditCardAdd.AppendChild(PayeeEntityRef);
					PayeeEntityRef.AppendChild(MakeSimpleElem(doc, "ListID", payeeEntityRefListId));
					PayeeEntityRef.AppendChild(MakeSimpleElem(doc, "FullName", payeeEntityRefFullName));
					#endregion

					#region APAccountRef
					APAccountRef = doc.CreateElement("APAccountRef");
					BillPaymentCreditCardAdd.AppendChild(APAccountRef);
					APAccountRef.AppendChild(MakeSimpleElem(doc, "ListID", apAccountRefListId));
					APAccountRef.AppendChild(MakeSimpleElem(doc, "FullName", apAccountRefFullName));
					#endregion

					BillPaymentCreditCardAdd.AppendChild(MakeSimpleElem(doc, "TxnDate", txnDate));

					#region CreditCardAccountRef
					CreditCardAccountRef = doc.CreateElement("CreditCardAccountRef");
					BillPaymentCreditCardAdd.AppendChild(CreditCardAccountRef);
					CreditCardAccountRef.AppendChild(MakeSimpleElem(doc, "ListID", creditCardAccountRefListId));
					CreditCardAccountRef.AppendChild(MakeSimpleElem(doc, "FullName", creditCardAccountRefFullName));
					#endregion

					BillPaymentCreditCardAdd.AppendChild(MakeSimpleElem(doc, "RefNumber", refNumber));
					BillPaymentCreditCardAdd.AppendChild(MakeSimpleElem(doc, "Memo", memo));
					BillPaymentCreditCardAdd.AppendChild(MakeSimpleElem(doc, "ExchangeRate", exchangeRate));
					BillPaymentCreditCardAdd.AppendChild(MakeSimpleElem(doc, "ExternalGUID", externalGUID));

					#region AppliedToTxnRet
					AppliedToTxnRet = doc.CreateElement("AppliedToTxnRet");
					BillPaymentCreditCardAdd.AppendChild(AppliedToTxnRet);

					AppliedToTxnRet.AppendChild(MakeSimpleElem(doc, "TxnID", appliedToTxnAdd_TxnID));
					AppliedToTxnRet.AppendChild(MakeSimpleElem(doc, "PaymentAmount", appliedToTxnAdd_PaymentAmount.ToString()));

					SetCredit = doc.CreateElement("SetCredit");
					AppliedToTxnRet.AppendChild(SetCredit);
					SetCredit.AppendChild(MakeSimpleElem(doc, "CreditTxnId", appliedToTxnAdd_SetCredit_CreditTxnId));
					SetCredit.AppendChild(MakeSimpleElem(doc, "AppliedAmount", appliedToTxnAdd_SetCredit_AppliedAmount.ToString()));
					SetCredit.AppendChild(MakeSimpleElem(doc, "CreditTxnId", appliedToTxnAdd_SetCredit_Override.ToString()));

					AppliedToTxnRet.AppendChild(MakeSimpleElem(doc, "DiscountAmount", appliedToTxnAdd_DiscountAmount.ToString()));

					DiscountAccountRef = doc.CreateElement("DiscountAccountRef");
					BillPaymentCreditCardAdd.AppendChild(DiscountAccountRef);
					DiscountAccountRef.AppendChild(MakeSimpleElem(doc, "ListID", appliedToTxnAdd_DiscountAccountRefListId));
					DiscountAccountRef.AppendChild(MakeSimpleElem(doc, "FullName", appliedToTxnAdd_DiscountAccountRefFullName));

					DiscountClassRef = doc.CreateElement("DiscountClassRef");
					BillPaymentCreditCardAdd.AppendChild(DiscountClassRef);
					DiscountClassRef.AppendChild(MakeSimpleElem(doc, "ListID", appliedToTxnAdd_DiscountClassRefListId));
					DiscountClassRef.AppendChild(MakeSimpleElem(doc, "FullName", appliedToTxnAdd_DiscountClassRefFullName));

					#endregion AppliedToTxnRet

					break;
				#endregion

				#region BillPaymentCreditCardQuery
				case ActionType.BillPaymentCreditCardQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion

				#region ChargeAdd
				case ActionType.ChargeAdd:
					ChargeAdd = doc.CreateElement("ChargeAdd");
					request.AppendChild(ChargeAdd);

					#region CustomerRef
					CustomerRef = doc.CreateElement("CustomerRef");
					ChargeAdd.AppendChild(CustomerRef);
					CustomerRef.AppendChild(MakeSimpleElem(doc, "ListID", customerRefListId));
					CustomerRef.AppendChild(MakeSimpleElem(doc, "FullName", customerRefFullName));
					#endregion

					ChargeAdd.AppendChild(MakeSimpleElem(doc, "TxnDate", txnDate));
					ChargeAdd.AppendChild(MakeSimpleElem(doc, "RefNumber", refNumber));

					#region ItemRef
					ItemRef = doc.CreateElement("ItemRef");
					ChargeAdd.AppendChild(ItemRef);
					ItemRef.AppendChild(MakeSimpleElem(doc, "ListID", itemRefListId));
					ItemRef.AppendChild(MakeSimpleElem(doc, "FullName", itemRefFullName));
					#endregion

					#region InventorySiteRef
					InventorySiteRef = doc.CreateElement("InventorySiteRef");
					ChargeAdd.AppendChild(InventorySiteRef);
					InventorySiteRef.AppendChild(MakeSimpleElem(doc, "ListID", inventorySiteRefListId));
					InventorySiteRef.AppendChild(MakeSimpleElem(doc, "FullName", inventorySiteRefFullName));
					#endregion

					#region InventorySiteLocationRef
					InventorySiteLocationRef = doc.CreateElement("InventorySiteLocationRef");
					ChargeAdd.AppendChild(InventorySiteLocationRef);
					InventorySiteLocationRef.AppendChild(MakeSimpleElem(doc, "ListID", inventorySiteLocationRefListId));
					InventorySiteLocationRef.AppendChild(MakeSimpleElem(doc, "FullName", inventorySiteLocationRefFullName));
					#endregion

					ChargeAdd.AppendChild(MakeSimpleElem(doc, "Quantity", quantity.ToString()));
					ChargeAdd.AppendChild(MakeSimpleElem(doc, "UnitOfMeasure", unitOfMeasure));
					ChargeAdd.AppendChild(MakeSimpleElem(doc, "Rate", rate.ToString()));
					ChargeAdd.AppendChild(MakeSimpleElem(doc, "OptionForPriceRuleConflict", optionForPriceRuleConflict));
					ChargeAdd.AppendChild(MakeSimpleElem(doc, "Amount", amount));
					ChargeAdd.AppendChild(MakeSimpleElem(doc, "Desc",desc));

					#region ARAccountRef
					ARAccountRef = doc.CreateElement("ARAccountRef");
					ChargeAdd.AppendChild(ARAccountRef);
					ARAccountRef.AppendChild(MakeSimpleElem(doc, "ListID", arAccountRefListId));
					ARAccountRef.AppendChild(MakeSimpleElem(doc, "FullName", arAccountRefFullName));
					#endregion

					#region ClassRef
					ClassRef = doc.CreateElement("ClassRef");
					ChargeAdd.AppendChild(ClassRef);
					ClassRef.AppendChild(MakeSimpleElem(doc, "ListID", classRefListId));
					ClassRef.AppendChild(MakeSimpleElem(doc, "FullName", classRefFullName));
					#endregion

					ChargeAdd.AppendChild(MakeSimpleElem(doc, "BilledDate", billedDate));
					ChargeAdd.AppendChild(MakeSimpleElem(doc, "DueDate", dueDate));

					#region OverrideItemAccountRef
					OverrideItemAccountRef = doc.CreateElement("OverrideItemAccountRef");
					ChargeAdd.AppendChild(OverrideItemAccountRef);
					OverrideItemAccountRef.AppendChild(MakeSimpleElem(doc, "ListID", overrideItemAccountRefListId));
					OverrideItemAccountRef.AppendChild(MakeSimpleElem(doc, "FullName", overrideItemAccountRefFullName));
					#endregion

					ChargeAdd.AppendChild(MakeSimpleElem(doc, "ExternalGUID", externalGUID));

					break;
				#endregion

				#region ChargeQuery
				case ActionType.ChargeQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					break;
				#endregion
				
				//May include Line items, have to check this 
				#region CheckAdd
				case ActionType.CheckAdd:
					CheckAdd = doc.CreateElement("CheckAdd");
					request.AppendChild(CheckAdd);

					#region AccountRef
					AccountRef = doc.CreateElement("AccountRef");
					CheckAdd.AppendChild(AccountRef);
					AccountRef.AppendChild(MakeSimpleElem(doc, "ListID", accountRefListId));
					AccountRef.AppendChild(MakeSimpleElem(doc, "FullName", accountRefFullName));
					#endregion

					#region PayeeEntityRef
					PayeeEntityRef = doc.CreateElement("PayeeEntityRef");
					CheckAdd.AppendChild(PayeeEntityRef);
					PayeeEntityRef.AppendChild(MakeSimpleElem(doc, "ListID", itemRefListId));
					PayeeEntityRef.AppendChild(MakeSimpleElem(doc, "FullName", itemRefFullName));
					#endregion

					CheckAdd.AppendChild(MakeSimpleElem(doc, "RefNumber", refNumber));
					CheckAdd.AppendChild(MakeSimpleElem(doc, "TxnDate", txnDate));
					CheckAdd.AppendChild(MakeSimpleElem(doc, "Memo", memo));

					#region Address
					Address = doc.CreateElement("Address");
					request.AppendChild(Address);
					Address.AppendChild(MakeSimpleElem(doc, "Addr1", addr1));
					Address.AppendChild(MakeSimpleElem(doc, "Addr2", addr2));
					Address.AppendChild(MakeSimpleElem(doc, "Addr3", addr3));
					Address.AppendChild(MakeSimpleElem(doc, "Addr4", addr4));
					Address.AppendChild(MakeSimpleElem(doc, "Addr5", addr5));
					Address.AppendChild(MakeSimpleElem(doc, "City", city));
					Address.AppendChild(MakeSimpleElem(doc, "State", state));
					Address.AppendChild(MakeSimpleElem(doc, "PostalCode", postalCode));
					Address.AppendChild(MakeSimpleElem(doc, "Country", country));
					Address.AppendChild(MakeSimpleElem(doc, "Note", note));
					#endregion

					CheckAdd.AppendChild(MakeSimpleElem(doc, "IsToBePrinted", isToBePrinted.ToString()));
					CheckAdd.AppendChild(MakeSimpleElem(doc, "IsTaxIncluded", isTaxIncluded.ToString()));

					#region SalesTaxCodeRef
					SalesTaxCodeRef = doc.CreateElement("SalesTaxCodeRef");
					CheckAdd.AppendChild(SalesTaxCodeRef);
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "ListID", salesTaxCodeRefListId));
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "FullName", salesTaxCodeRefFullName));
					#endregion

					CheckAdd.AppendChild(MakeSimpleElem(doc, "ExchangeRate", exchangeRate));
					CheckAdd.AppendChild(MakeSimpleElem(doc, "ExternalGUID", externalGUID));

					#region ApplyCheckToTxnAdd
					ApplyCheckToTxnAdd = doc.CreateElement("ApplyCheckToTxnAdd");
					CheckAdd.AppendChild(SalesTaxCodeRef);
					ApplyCheckToTxnAdd.AppendChild(MakeSimpleElem(doc, "ListID", applyCheckToTxnAddTxnID));
					ApplyCheckToTxnAdd.AppendChild(MakeSimpleElem(doc, "FullName", applyCheckToTxnAddAmount));
					#endregion


					break;
				#endregion

				#region CheckQuery
				case ActionType.CheckQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion

				//Can I use the same variables within the same case from the parent node and another within the child?
				//Unsure of whats required: ExpenseLines, Item Lines 
				#region CreditCardChargeAdd
				case ActionType.CreditCardChargeAdd:
					CreditCardChargeAdd = doc.CreateElement("CreditCardChargeAdd");
					request.AppendChild(CreditCardChargeAdd);

					#region AccountRef
					AccountRef = doc.CreateElement("AccountRef");
					CreditCardChargeAdd.AppendChild(AccountRef);
					AccountRef.AppendChild(MakeSimpleElem(doc, "ListID", accountRefListId));
					AccountRef.AppendChild(MakeSimpleElem(doc, "FullName", accountRefFullName));
					#endregion

					#region PayeeEntityRef
					PayeeEntityRef = doc.CreateElement("PayeeEntityRef");
					CreditCardChargeAdd.AppendChild(PayeeEntityRef);
					PayeeEntityRef.AppendChild(MakeSimpleElem(doc, "ListID", payeeEntityRefListId));
					PayeeEntityRef.AppendChild(MakeSimpleElem(doc, "FullName", payeeEntityRefFullName));
					#endregion 

					CreditCardChargeAdd.AppendChild(MakeSimpleElem(doc, "TxnDate", txnDate));
					CreditCardChargeAdd.AppendChild(MakeSimpleElem(doc, "RefNumber", refNumber));
					CreditCardChargeAdd.AppendChild(MakeSimpleElem(doc, "Memo", memo));
					CreditCardChargeAdd.AppendChild(MakeSimpleElem(doc, "IsTaxIncluded", isTaxIncluded.ToString()));

					#region SalesTaxCodeRef
					SalesTaxCodeRef = doc.CreateElement("SalesTaxCodeRef");
					CreditCardChargeAdd.AppendChild(SalesTaxCodeRef);
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "ListID", salesTaxCodeRefListId));
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "FullName", salesTaxCodeRefFullName));
					#endregion

					CreditCardChargeAdd.AppendChild(MakeSimpleElem(doc, "ExchangeRate", exchangeRate));
					CreditCardChargeAdd.AppendChild(MakeSimpleElem(doc, "ExternalGUID", externalGUID));

					#region ExpenseLineAdd	
					ExpenseLineAdd = doc.CreateElement("ExpenseLineAdd");
					CreditCardChargeAdd.AppendChild(ExpenseLineAdd);

					AccountRef = doc.CreateElement("AccountRef");
					ExpenseLineAdd.AppendChild(AccountRef);

					ExpenseLineAdd.AppendChild(MakeSimpleElem(doc, "Amount", amount));
					ExpenseLineAdd.AppendChild(MakeSimpleElem(doc, "Memo", memo));

					#region CustomerRef
					CustomerRef = doc.CreateElement("CustomerRef");
					ExpenseLineAdd.AppendChild(CustomerRef);
					CustomerRef.AppendChild(MakeSimpleElem(doc, "ListID", customerRefListId));
					CustomerRef.AppendChild(MakeSimpleElem(doc, "FullName", customerRefFullName));
					#endregion

					#region ClassRef
					ClassRef = doc.CreateElement("ClassRef");
					ExpenseLineAdd.AppendChild(ClassRef);
					ClassRef.AppendChild(MakeSimpleElem(doc, "ListID", classRefListId));
					ClassRef.AppendChild(MakeSimpleElem(doc, "FullName", classRefFullName));
					#endregion

					#region SalesTaxCodeRef
					SalesTaxCodeRef = doc.CreateElement("SalesTaxCodeRef");
					ExpenseLineAdd.AppendChild(SalesTaxCodeRef);
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "ListID", salesTaxCodeRefListId));
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "FullName", salesTaxCodeRefFullName));
					#endregion

					ExpenseLineAdd.AppendChild(MakeSimpleElem(doc, "BillableStatus", billableStatus));

					#region SalesRepRef
					SalesRepRef = doc.CreateElement("SalesRepRef");
					ExpenseLineAdd.AppendChild(SalesRepRef);
					SalesRepRef.AppendChild(MakeSimpleElem(doc, "ListID", salesRepRefListId));
					SalesRepRef.AppendChild(MakeSimpleElem(doc, "FullName", salesRepRefFullName));
					#endregion

					#region DataExt
					DataExt = doc.CreateElement("DataExt");
					ExpenseLineAdd.AppendChild(DataExt);
					DataExt.AppendChild(MakeSimpleElem(doc, "OwnerID", dataExtOwnerId));
					DataExt.AppendChild(MakeSimpleElem(doc, "DataExtName", dataExtName));
					DataExt.AppendChild(MakeSimpleElem(doc, "DataExtValue", dataExtValue));
					#endregion

					#endregion ExpenseLineAdd

					break;
				#endregion

				#region CreditCardChargeQuery
				case ActionType.CreditCardChargeQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion

				#region CreditCardCreditAdd
				case ActionType.CreditCardCreditAdd:
					CreditCardCreditAdd = doc.CreateElement("CreditCardCreditAdd");
					request.AppendChild(CreditCardCreditAdd);

					#region AccountRef
					AccountRef = doc.CreateElement("AccountRef");
					CreditCardCreditAdd.AppendChild(AccountRef);
					AccountRef.AppendChild(MakeSimpleElem(doc, "ListID", accountRefListId));
					AccountRef.AppendChild(MakeSimpleElem(doc, "FullName", accountRefFullName));
					#endregion

					#region PayeeEntityRef
					PayeeEntityRef = doc.CreateElement("PayeeEntityRef");
					CreditCardCreditAdd.AppendChild(PayeeEntityRef);
					PayeeEntityRef.AppendChild(MakeSimpleElem(doc, "ListID", payeeEntityRefListId));
					PayeeEntityRef.AppendChild(MakeSimpleElem(doc, "FullName", payeeEntityRefFullName));
					#endregion

					CreditCardCreditAdd.AppendChild(MakeSimpleElem(doc, "TxnDate", txnDate));
					CreditCardCreditAdd.AppendChild(MakeSimpleElem(doc, "RefNumber", refNumber));
					CreditCardCreditAdd.AppendChild(MakeSimpleElem(doc, "Memo", memo));
					CreditCardCreditAdd.AppendChild(MakeSimpleElem(doc, "IsTaxIncluded", isTaxIncluded.ToString()));

					#region SalesTaxCodeRef
					SalesTaxCodeRef = doc.CreateElement("SalesTaxCodeRef");
					CreditCardCreditAdd.AppendChild(SalesTaxCodeRef);
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "ListID", salesTaxCodeRefListId));
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "FullName", salesTaxCodeRefFullName));
					#endregion

					CreditCardCreditAdd.AppendChild(MakeSimpleElem(doc, "ExchangeRate", exchangeRate));
					CreditCardCreditAdd.AppendChild(MakeSimpleElem(doc, "ExternalGUID", externalGUID));

					#region ExpenseLineAdd	
					ExpenseLineAdd = doc.CreateElement("ExpenseLineAdd");
					CreditCardCreditAdd.AppendChild(ExpenseLineAdd);

					AccountRef = doc.CreateElement("AccountRef");
					ExpenseLineAdd.AppendChild(AccountRef);

					ExpenseLineAdd.AppendChild(MakeSimpleElem(doc, "Amount", amount));
					ExpenseLineAdd.AppendChild(MakeSimpleElem(doc, "Memo", memo));

					#region CustomerRef
					CustomerRef = doc.CreateElement("CustomerRef");
					ExpenseLineAdd.AppendChild(CustomerRef);
					CustomerRef.AppendChild(MakeSimpleElem(doc, "ListID", customerRefListId));
					CustomerRef.AppendChild(MakeSimpleElem(doc, "FullName", customerRefFullName));
					#endregion

					#region ClassRef
					ClassRef = doc.CreateElement("ClassRef");
					ExpenseLineAdd.AppendChild(ClassRef);
					ClassRef.AppendChild(MakeSimpleElem(doc, "ListID", classRefListId));
					ClassRef.AppendChild(MakeSimpleElem(doc, "FullName", classRefFullName));
					#endregion

					#region SalesTaxCodeRef
					SalesTaxCodeRef = doc.CreateElement("SalesTaxCodeRef");
					ExpenseLineAdd.AppendChild(SalesTaxCodeRef);
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "ListID", salesTaxCodeRefListId));
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "FullName", salesTaxCodeRefFullName));
					#endregion

					ExpenseLineAdd.AppendChild(MakeSimpleElem(doc, "BillableStatus", billableStatus));

					#region SalesRepRef
					SalesRepRef = doc.CreateElement("SalesRepRef");
					ExpenseLineAdd.AppendChild(SalesRepRef);
					SalesRepRef.AppendChild(MakeSimpleElem(doc, "ListID", salesRepRefListId));
					SalesRepRef.AppendChild(MakeSimpleElem(doc, "FullName", salesRepRefFullName));
					#endregion

					#region DataExt
					DataExt = doc.CreateElement("DataExt");
					ExpenseLineAdd.AppendChild(DataExt);
					DataExt.AppendChild(MakeSimpleElem(doc, "OwnerID", dataExtOwnerId));
					DataExt.AppendChild(MakeSimpleElem(doc, "DataExtName", dataExtName));
					DataExt.AppendChild(MakeSimpleElem(doc, "DataExtValue", dataExtValue));
					#endregion

					#endregion ExpenseLineAdd
					break;
				#endregion

				#region CreditCardCreditQuery
				case ActionType.CreditCardCreditQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion

				#region CustomerAdd
				case ActionType.CustomerAdd:
					CustomerAdd = doc.CreateElement("CustomerAdd");
					request.AppendChild(CustomerAdd);

					CustomerAdd.AppendChild(MakeSimpleElem(doc, "Name", name));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "IsActive", isActive));

					#region ClassRef
					ClassRef = doc.CreateElement("ClassRef");
					CustomerAdd.AppendChild(ClassRef);
					ClassRef.AppendChild(MakeSimpleElem(doc, "ListID", classRefListId));
					ClassRef.AppendChild(MakeSimpleElem(doc, "FullName", classRefFullName));
					#endregion

					#region ParentRef
					ParentRef = doc.CreateElement("ParentRef");
					CustomerAdd.AppendChild(ParentRef);
					ParentRef.AppendChild(MakeSimpleElem(doc, "ListID", parentRefListId));
					ParentRef.AppendChild(MakeSimpleElem(doc, "FullName", parentRefFullName));
					#endregion

					CustomerAdd.AppendChild(MakeSimpleElem(doc, "CompanyName", companyName));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "Salutation", salutation));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "FirstName", firstName));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "MiddleName", middleName));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "LastName", lastName));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "JobTitle", jobTitle));

					#region BillAddress
					BillAddress = doc.CreateElement("BillAddress");
					request.AppendChild(BillAddress);
					BillAddress.AppendChild(MakeSimpleElem(doc, "Addr1", addr1));
					BillAddress.AppendChild(MakeSimpleElem(doc, "Addr2", addr2));
					BillAddress.AppendChild(MakeSimpleElem(doc, "Addr3", addr3));
					BillAddress.AppendChild(MakeSimpleElem(doc, "Addr4", addr4));
					BillAddress.AppendChild(MakeSimpleElem(doc, "Addr5", addr5));
					BillAddress.AppendChild(MakeSimpleElem(doc, "City", city));
					BillAddress.AppendChild(MakeSimpleElem(doc, "State", state));
					BillAddress.AppendChild(MakeSimpleElem(doc, "PostalCode", postalCode));
					BillAddress.AppendChild(MakeSimpleElem(doc, "Country", country));
					BillAddress.AppendChild(MakeSimpleElem(doc, "Note", note));
					#endregion

					#region ShipAddress
					ShipAddress = doc.CreateElement("ShipAddress");
					request.AppendChild(ShipAddress);
					ShipAddress.AppendChild(MakeSimpleElem(doc, "Addr1", shipAddr1));
					ShipAddress.AppendChild(MakeSimpleElem(doc, "Addr2", shipAddr2));
					ShipAddress.AppendChild(MakeSimpleElem(doc, "Addr3", shipAddr3));
					ShipAddress.AppendChild(MakeSimpleElem(doc, "Addr4", shipAddr4));
					ShipAddress.AppendChild(MakeSimpleElem(doc, "Addr5", shipAddr5));
					ShipAddress.AppendChild(MakeSimpleElem(doc, "City", shipCity));
					ShipAddress.AppendChild(MakeSimpleElem(doc, "State", shipState));
					ShipAddress.AppendChild(MakeSimpleElem(doc, "PostalCode", shipPostalCode));
					ShipAddress.AppendChild(MakeSimpleElem(doc, "Country", shipCountry));
					ShipAddress.AppendChild(MakeSimpleElem(doc, "Note", shipNote));
					#endregion

					#region ShipToAddress
					ShipToAddress = doc.CreateElement("ShipToAddress");
					request.AppendChild(ShipToAddress);
					ShipToAddress.AppendChild(MakeSimpleElem(doc, "Addr1", shipToAddr1));
					ShipToAddress.AppendChild(MakeSimpleElem(doc, "Addr2", shipToAddr2));
					ShipToAddress.AppendChild(MakeSimpleElem(doc, "Addr3", shipToAddr3));
					ShipToAddress.AppendChild(MakeSimpleElem(doc, "Addr4", shipToAddr4));
					ShipToAddress.AppendChild(MakeSimpleElem(doc, "Addr5", shipToAddr5));
					ShipToAddress.AppendChild(MakeSimpleElem(doc, "City", shipToCity));
					ShipToAddress.AppendChild(MakeSimpleElem(doc, "State", shipToState));
					ShipToAddress.AppendChild(MakeSimpleElem(doc, "PostalCode", shipToPostalCode));
					ShipToAddress.AppendChild(MakeSimpleElem(doc, "Country", shipToCountry));
					ShipToAddress.AppendChild(MakeSimpleElem(doc, "Note", shipToNote));
					ShipToAddress.AppendChild(MakeSimpleElem(doc, "DefaultShipTo", shipToDefaultShipTo));
					#endregion

					CustomerAdd.AppendChild(MakeSimpleElem(doc, "Phone", phone));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "AltPhone", altPhone));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "Fax", fax));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "Email", email));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "Cc", cc));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "Contact", contact));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "AltContact", altContact));

					#region AdditionalContactRef
					AdditionalContactRef = doc.CreateElement("AdditionalContactRef");
					CustomerAdd.AppendChild(AdditionalContactRef);
					AdditionalContactRef.AppendChild(MakeSimpleElem(doc, "ContactName", additionalContactRefName));
					AdditionalContactRef.AppendChild(MakeSimpleElem(doc, "ContactValue", additionalContactRefValue));
					#endregion

					#region Contacts
					Contacts = doc.CreateElement("Contacts");
					CustomerAdd.AppendChild(Contacts);
					Contacts.AppendChild(MakeSimpleElem(doc, "Salutation", salutation));
					Contacts.AppendChild(MakeSimpleElem(doc, "FirstName", firstName));
					Contacts.AppendChild(MakeSimpleElem(doc, "MiddleName", middleName));
					Contacts.AppendChild(MakeSimpleElem(doc, "LastName", lastName));
					Contacts.AppendChild(MakeSimpleElem(doc, "JobTitle", jobTitle));
					AdditionalContactRef = doc.CreateElement("AdditionalContactRef");
					Contacts.AppendChild(AdditionalContactRef);
					AdditionalContactRef.AppendChild(MakeSimpleElem(doc, "ContactName", additionalContactRefName));
					AdditionalContactRef.AppendChild(MakeSimpleElem(doc, "ContactValue", additionalContactRefValue));
					#endregion

					#region CustomerTypeRef
					CustomerTypeRef = doc.CreateElement("CustomerTypeRef");
					CustomerAdd.AppendChild(CustomerTypeRef);
					CustomerTypeRef.AppendChild(MakeSimpleElem(doc, "ListID", customerTypeRefListId));
					CustomerTypeRef.AppendChild(MakeSimpleElem(doc, "FullName", customerTypeRefFullName));
					#endregion

					#region TermsRef
					TermsRef = doc.CreateElement("TermsRef");
					CustomerAdd.AppendChild(TermsRef);
					TermsRef.AppendChild(MakeSimpleElem(doc, "ListID", termsRefListId));
					TermsRef.AppendChild(MakeSimpleElem(doc, "FullName", termsRefFullName));
					#endregion

					#region SalesRepRef
					SalesRepRef = doc.CreateElement("SalesRepRef");
					CustomerAdd.AppendChild(SalesRepRef);
					SalesRepRef.AppendChild(MakeSimpleElem(doc, "ListID", salesRepRefListId));
					SalesRepRef.AppendChild(MakeSimpleElem(doc, "FullName", salesRepRefFullName));
					#endregion

					CustomerAdd.AppendChild(MakeSimpleElem(doc, "OpenBalance", openBalance));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "OpenBalanceDate", openBalanceDate));

					#region SalesTaxCodeRef
					SalesTaxCodeRef = doc.CreateElement("SalesTaxCodeRef");
					CustomerAdd.AppendChild(SalesTaxCodeRef);
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "ListID", salesTaxCodeRefListId));
					SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "FullName", salesTaxCodeRefFullName));
					#endregion

					#region ItemSalesTaxRef
					ItemSalesTaxRef = doc.CreateElement("ItemSalesTaxRef");
					CustomerAdd.AppendChild(ItemSalesTaxRef);
					ItemSalesTaxRef.AppendChild(MakeSimpleElem(doc, "ListID", itemSalesTaxRefListId));
					ItemSalesTaxRef.AppendChild(MakeSimpleElem(doc, "FullName", itemSalesTaxRefFullName));
					#endregion

					CustomerAdd.AppendChild(MakeSimpleElem(doc, "SalesTaxCountry", salesTaxCountry));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "ResaleNumber", resaleNumber));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "AccountNumber", accountNumber));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "CreditLimit", creditLimit));

					#region PreferredPaymentMethod
					PreferredPaymentMethodRef = doc.CreateElement("PreferredPaymentMethodRef");
					CustomerAdd.AppendChild(PreferredPaymentMethodRef);
					PreferredPaymentMethodRef.AppendChild(MakeSimpleElem(doc, "ListID", preferredPaymentMethodRefListId));
					PreferredPaymentMethodRef.AppendChild(MakeSimpleElem(doc, "FullName", preferredPaymentMethodRefFullName));
					#endregion

					#region CreditCardInfo
					CreditCardInfo = doc.CreateElement("CreditCardInfo");
					CustomerAdd.AppendChild(CreditCardInfo);
					CreditCardInfo.AppendChild(MakeSimpleElem(doc, "CreditCardNumber", creditCardNumber));
					CreditCardInfo.AppendChild(MakeSimpleElem(doc, "ExpirationMonth", expirationMonth));
					CreditCardInfo.AppendChild(MakeSimpleElem(doc, "ExpirationYear", expirationYear));
					CreditCardInfo.AppendChild(MakeSimpleElem(doc, "NameOnCard", nameOnCard));
					CreditCardInfo.AppendChild(MakeSimpleElem(doc, "CreditCardAddress", creditCardAddress));
					CreditCardInfo.AppendChild(MakeSimpleElem(doc, "CreditCardPostalCode", creditCardPostalCode));
					#endregion

					CustomerAdd.AppendChild(MakeSimpleElem(doc, "JobStatus", jobStatus));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "CreditLimit", creditLimit));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "CreditLimit", creditLimit));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "CreditLimit", creditLimit));

					#region JobTypeRef
					JobTypeRef = doc.CreateElement("JobTypeRef");
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "ListID", jobTypeRefListId));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "FullName", jobTypeRefFullName));
					#endregion

					CustomerAdd.AppendChild(MakeSimpleElem(doc, "Notes", notes));

					#region AdditionalNotes
					AdditionalNotes = doc.CreateElement("AdditionalNotes");
					CustomerAdd.AppendChild(AdditionalNotes);
					AdditionalNotes.AppendChild(MakeSimpleElem(doc, "Note", additionalNotesNote));
					#endregion

					CustomerAdd.AppendChild(MakeSimpleElem(doc, "PreferredDeliveryMethod", preferredDeliveryMethod));

					#region PriceLevelRef
					PriceLevelRef = doc.CreateElement("PriceLevelRef");
					CustomerAdd.AppendChild(PriceLevelRef);
					PriceLevelRef.AppendChild(MakeSimpleElem(doc, "ListID", priceLevelRefListId));
					PriceLevelRef.AppendChild(MakeSimpleElem(doc, "FullName", priceLevelRefFullName));
					#endregion

					CustomerAdd.AppendChild(MakeSimpleElem(doc, "ExternalGUID", externalGUID));
					CustomerAdd.AppendChild(MakeSimpleElem(doc, "TaxRegistrationNumber", taxRegistrationNumber));

					#region CurrencyRef
					CurrencyRef = doc.CreateElement("CurrencyRef");
					CustomerAdd.AppendChild(CurrencyRef);
					CurrencyRef.AppendChild(MakeSimpleElem(doc, "ListID", currencyRefListId));
					CurrencyRef.AppendChild(MakeSimpleElem(doc, "FullName", currencyRefFullName));
					#endregion

					break;

				#endregion

				#region CustomerQuery
				case ActionType.CustomeryQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					break;
				#endregion

				#region DepositQuery
				case ActionType.DepositQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion

				#region InventoryAdjustmentAdd
				case ActionType.InventoryAdjustmentAdd:
					InventoryAdjustmentAdd = doc.CreateElement("InventoryAdjustmentAdd");
					request.AppendChild(InventoryAdjustmentAdd);

					AccountRef = doc.CreateElement("AccountRef");
					InventoryAdjustmentAdd.AppendChild(AccountRef);
					AccountRef.AppendChild(MakeSimpleElem(doc, "ListID", accountRefListId));

					InventoryAdjustmentLineAdd = doc.CreateElement("InventoryAdjustmentLineAdd");
					InventoryAdjustmentAdd.AppendChild(InventoryAdjustmentLineAdd);
					ItemRef = doc.CreateElement("ItemRef");
					InventoryAdjustmentLineAdd.AppendChild(ItemRef);
					ItemRef.AppendChild(MakeSimpleElem(doc, "FullName", itemRefFullName));
					QuantityAdjustment = doc.CreateElement("QuantityAdjustment");
					InventoryAdjustmentLineAdd.AppendChild(QuantityAdjustment);
					QuantityAdjustment.AppendChild(MakeSimpleElem(doc, "QuantityDifference", quantityDifference.ToString()));
					break;
				#endregion InventoryAdjustmentAdd

				#region InventoryAdjustmentQuery
				case ActionType.InventoryAdjustmentQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion InventoryAdjustmentQuery

				#region ItemNonInventoryQeury
				case ActionType.ItemNonInventoryQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					break;
				#endregion 

				#region ReceivePaymentQuery
				case ActionType.ReceivePaymentQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion InventoryAdjustmentQuery

				#region SalesReceiptQuery
				case ActionType.SalesReceiptQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion SalesReceiptQuery

				default:
					break;
			}
		}

		// This is the main method that actually sends the request to QuickBooks. Most of the other methods are called from within this method.
		protected List<object> DoAction(ActionType actionType, params KeyValuePair<string, object>[] parameters)
		{
			if (actionType < 0)
				throw new Exception(String.Format("<actionType> is required.{0}Exception thrown in QB20191021Util.DoAction(ActionType actionType, DateTime? startTime, DateTime? endTime).{0}", Environment.NewLine));

			try {
				if (!ConnectionOpen || !SessionBegun || String.IsNullOrWhiteSpace(SessionId))
					OpenConnection();

				//Create the message set request object to hold our request
				var requestXmlDoc = new XmlDocument();

				//Add the prolog processing instructions
				requestXmlDoc.AppendChild(requestXmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null));
				requestXmlDoc.AppendChild(requestXmlDoc.CreateProcessingInstruction("qbxml", "version=\"13.0\""));

				//Create the outer request envelope tag
				var outer = requestXmlDoc.CreateElement("QBXML");
				requestXmlDoc.AppendChild(outer);

				//Create the inner request envelope & any needed attributes
				var inner = requestXmlDoc.CreateElement("QBXMLMsgsRq");
				outer.AppendChild(inner);
				inner.SetAttribute("onError", "stopOnError");
				BuildRequest(requestXmlDoc, inner, actionType, parameters);

				if (Debug)
					File.WriteAllText(String.Format(@"C:\TEMP\{0}Request.xml", actionType.ToString()), BeautifyXml(requestXmlDoc.OuterXml));

				//Send the request and get the response from QuickBooks
				var responseStr = Rp.ProcessRequest(SessionId, requestXmlDoc.OuterXml);

				if (Debug)
					File.WriteAllText(String.Format(@"C:\TEMP\{0}Response.xml", actionType.ToString()), BeautifyXml(responseStr));

				if (actionType.ToString().EndsWith("Query"))
					return ProcessResponse(actionType, responseStr);

				return new List<object>();
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in QBV20191021Util.DoAction(ActionType actionType='{3}', params KeyValuePair<string, object>[] parameters='{4}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, actionType.ToString(), String.Join(", ", parameters));
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of QBV20191021Util.DoAction(ActionType actionType='{3}', params KeyValuePair<string, object>[] parameters='{4}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, actionType.ToString(), String.Join(", ", parameters));

				if (Debug)
					Console.Write("\n{0}", log);
				#endregion Log

				return new List<object>();
			}
		}

		protected XmlElement MakeSimpleElem(XmlDocument doc, string tagName, string tagVal)
		{
			var elem = doc.CreateElement(tagName);
			elem.InnerText = tagVal;
			return elem;
		}

		protected List<object> ProcessResponse(ActionType actionType, string response)
		{
			#region Input Validation
			if (String.IsNullOrWhiteSpace(response))
				return null;
			if (actionType < 0)
				throw new Exception(String.Format("<actionType> is required.{0}Exception thrown in QB20191021Util.ProcessResponse(ActionType actionType, string response).{0}", Environment.NewLine));
			#endregion Input Validation

			var responseXmlDoc = new XmlDocument();
			responseXmlDoc.LoadXml(response);
			var QueryRsList = responseXmlDoc.GetElementsByTagName(String.Format("{0}Rs", actionType.ToString()));

			if (QueryRsList.Count == 1) {
				var responseNode = QueryRsList.Item(0);
				// Check the status code, info, and severity
				var rsAttributes = responseNode.Attributes;
				var statusCode = rsAttributes.GetNamedItem("statusCode").Value;
				var statusSeverity = rsAttributes.GetNamedItem("statusSeverity").Value;
				var statusMessage = rsAttributes.GetNamedItem("statusMessage").Value;

				if (Debug)
					Console.Write("\n{0} {1}", statusCode, statusMessage);

				// status code = 0 all OK, > 0 is warning
				if (Convert.ToInt32(statusCode) > -1)
					return ProcessReturn(actionType, responseNode);
			}

			return new List<object>();
		}

		protected List<object> ProcessReturn(ActionType actionType, XmlNode responseNode)
		{
			if (actionType < 0)
				throw new Exception(String.Format("<actionType> is required.{0}Exception thrown in QB20191021Util.ProcessReturn(ActionType actionType).{0}", Environment.NewLine));
			if (responseNode == null || String.IsNullOrWhiteSpace(responseNode.ToString()))
				return null;

			XmlNode Item, ItemRef, LineItem;
			XmlNodeList ItemList, LineItemList;
			var list = new List<object>();

			switch (actionType) {

				#region Account
				case ActionType.AccountAdd:
				case ActionType.AccountMod:
				case ActionType.AccountQuery:
					AccountRet accountRet;
					ItemList = responseNode.SelectNodes("//AccountRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						accountRet = new AccountRet {
							ListID = Item.SelectSingleNode("ListID") == null ? null : Item.SelectSingleNode("ListID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							Name = Item.SelectSingleNode("Name") == null ? null : Item.SelectSingleNode("Name").InnerText,
							FullName = Item.SelectSingleNode("FullName") == null ? null : Item.SelectSingleNode("FullName").InnerText,
							IsActive = Item.SelectSingleNode("IsActive") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsActive").InnerText),
							Sublevel = Item.SelectSingleNode("Sublevel") == null ? (int?) null : int.Parse(Item.SelectSingleNode("Sublevel").InnerText),
							AccountType = Item.SelectSingleNode("AccountType") == null ? null : Item.SelectSingleNode("AccountType").InnerText,
							SpecialAccountType = Item.SelectSingleNode("SpecialAccountType") == null ? null : Item.SelectSingleNode("SpecialAccountType").InnerText,
							IsTaxAccount = Item.SelectSingleNode("IsTaxAccount") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsTaxAccount").InnerText),
							AccountNumber = Item.SelectSingleNode("AccountNumber") == null ? null : Item.SelectSingleNode("AccountNumber").InnerText,
							BankNumber = Item.SelectSingleNode("BankNumber") == null ? null : Item.SelectSingleNode("BankNumber").InnerText,
							Desc = Item.SelectSingleNode("Desc") == null ? null : Item.SelectSingleNode("Desc").InnerText,
							Balance = Item.SelectSingleNode("Balance") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("Balance").InnerText),
							TotalBalance = Item.SelectSingleNode("TotalBalance") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("TotalBalance").InnerText),
							CashFlowClassification = Item.SelectSingleNode("CashFlowClassification") == null ? null : Item.SelectSingleNode("CashFlowClassification").InnerText,
						};

						#region ParentRef
						if (Item.SelectSingleNode("ParentRef") == null) {
							accountRet.ParentRef.ListID = null;
							accountRet.ParentRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("ParentRef");
							accountRet.ParentRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							accountRet.ParentRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion ParentRef

						#region SalesTaxCodeRef
						if (Item.SelectSingleNode("SalesTaxCodeRef") == null) {
							accountRet.SalesTaxCodeRef.ListID = null;
							accountRet.SalesTaxCodeRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("SalesTaxCodeRef");
							accountRet.SalesTaxCodeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							accountRet.SalesTaxCodeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion SalesTaxCodeRef

						#region TaxLineInfoRet
						if (Item.SelectSingleNode("TaxLineInfoRet") == null) {
							accountRet.TaxLineInfoRet.TaxLineID = null;
							accountRet.TaxLineInfoRet.TaxLineName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("TaxLineInfoRet");
							accountRet.TaxLineInfoRet.TaxLineID = ItemRef.SelectSingleNode("TaxLineID") == null ? null : ItemRef.SelectSingleNode("TaxLineID").InnerText;
							accountRet.TaxLineInfoRet.TaxLineName = ItemRef.SelectSingleNode("TaxLineName") == null ? null : ItemRef.SelectSingleNode("TaxLineName").InnerText;
						}
						#endregion TaxLineInfoRet

						#region CurrencyRef
						if (Item.SelectSingleNode("CurrencyRef") == null) {
							accountRet.CurrencyRef.ListID = null;
							accountRet.CurrencyRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("CurrencyRef");
							accountRet.CurrencyRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							accountRet.CurrencyRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion CurrencyRef

						#region DataExtRet
						if (Item.SelectSingleNode("DataExtRet") == null) {
							accountRet.DataExtRet.OwnerID = null;
							accountRet.DataExtRet.DataExtName = null;
							accountRet.DataExtRet.DataExtType = null;
							accountRet.DataExtRet.DataExtValue = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("DataExtRet");
							accountRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
							accountRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
							accountRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
							accountRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
						}
						#endregion

						#region ErrorRecovery
						if (Item.SelectSingleNode("ErrorRecovery") == null) {
							accountRet.ErrorRecovery.ListID = null;
							accountRet.ErrorRecovery.TxnNumber = null;
							accountRet.ErrorRecovery.EditSequence = null;
							accountRet.ErrorRecovery.ExternalGUID = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("ErrorRecovery");
							accountRet.ErrorRecovery.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							accountRet.ErrorRecovery.TxnNumber = ItemRef.SelectSingleNode("TxnNumber") == null ? null : ItemRef.SelectSingleNode("TxnNumber").InnerText;
							accountRet.ErrorRecovery.EditSequence = ItemRef.SelectSingleNode("EditSequence") == null ? null : ItemRef.SelectSingleNode("EditSequence").InnerText;
							accountRet.ErrorRecovery.ExternalGUID = ItemRef.SelectSingleNode("ExternalGUID") == null ? null : ItemRef.SelectSingleNode("ExternalGUID").InnerText;
						}
						#endregion

						list.Add(accountRet);
					}
					break;
				#endregion

				#region ARRefundCreditCardQuery
				case ActionType.ARRefundCreditCardQuery:
			ARRefundCreditCardRet aRRefundCreditCardRet;
			CreditCardTxnInfo aRRefundCreditCardTxnInfo;
			ItemList = responseNode.SelectNodes("//ARRefundCreditCardRet");
			for (int i = 0; i < ItemList.Count; i++) {
				Item = ItemList.Item(i);
				aRRefundCreditCardRet = new ARRefundCreditCardRet {
					TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
					TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
					TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
					EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
					TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
					TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
					RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
					TotalAmount = Item.SelectSingleNode("TotalAmount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("TotalAmount").InnerText),
					ExchangeRate = Item.SelectSingleNode("ExchangeRate") == null ? 0 : float.Parse(Item.SelectSingleNode("ExchangeRate").InnerText),
					TotalAmountInHomeCurrency = Item.SelectSingleNode("TotalAmountInHomeCurrency") == null ? 0 : decimal.Parse(Item.SelectSingleNode("TotalAmountInHomeCurrency").InnerText),
					Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
					ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
				};

				#region CustomerRef
				if (Item.SelectSingleNode("CustomerRef") == null) {
					aRRefundCreditCardRet.CustomerRef.ListID = null;
					aRRefundCreditCardRet.CustomerRef.FullName = null;
				}

				else {
					ItemRef = Item.SelectSingleNode("CustomerRef");
					aRRefundCreditCardRet.CustomerRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
					aRRefundCreditCardRet.CustomerRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
				}
				#endregion CustomerRef

				#region RefundFromAccountRef
				if (Item.SelectSingleNode("RefundFromAccountRef") == null) {
					aRRefundCreditCardRet.RefundFromAccountRef.ListID = null;
					aRRefundCreditCardRet.RefundFromAccountRef.FullName = null;
				}
				else {
					ItemRef = Item.SelectSingleNode("RefundFromAccountRef");
					aRRefundCreditCardRet.RefundFromAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
					aRRefundCreditCardRet.RefundFromAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
				}
				#endregion

				#region CurrencyRef
				if (Item.SelectSingleNode("CurrencyRef") == null) {
					aRRefundCreditCardRet.CurrencyRef.ListID = null;
					aRRefundCreditCardRet.CurrencyRef.FullName = null;
				}
				else {
					ItemRef = Item.SelectSingleNode("CurrencyRef");
					aRRefundCreditCardRet.CurrencyRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
					aRRefundCreditCardRet.CurrencyRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
				}
				#endregion

				#region Address
				if (Item.SelectSingleNode("Address") == null) {
					aRRefundCreditCardRet.Address.Addr1 = null;
					aRRefundCreditCardRet.Address.Addr2 = null;
					aRRefundCreditCardRet.Address.Addr3 = null;
					aRRefundCreditCardRet.Address.Addr4 = null;
					aRRefundCreditCardRet.Address.Addr5 = null;
					aRRefundCreditCardRet.Address.City = null;
					aRRefundCreditCardRet.Address.State = null;
					aRRefundCreditCardRet.Address.PostalCode = null;
					aRRefundCreditCardRet.Address.Country = null;
					aRRefundCreditCardRet.Address.Note = null;

				}
				else {
					ItemRef = Item.SelectSingleNode("Address");
					aRRefundCreditCardRet.Address.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
					aRRefundCreditCardRet.Address.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
					aRRefundCreditCardRet.Address.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
					aRRefundCreditCardRet.Address.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
					aRRefundCreditCardRet.Address.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
					aRRefundCreditCardRet.Address.City = ItemRef.SelectSingleNode("City") == null ? null : ItemRef.SelectSingleNode("City").InnerText;
					aRRefundCreditCardRet.Address.State = ItemRef.SelectSingleNode("State") == null ? null : ItemRef.SelectSingleNode("State").InnerText;
					aRRefundCreditCardRet.Address.PostalCode = ItemRef.SelectSingleNode("PostalCode") == null ? null : ItemRef.SelectSingleNode("PostalCode").InnerText;
					aRRefundCreditCardRet.Address.Country = ItemRef.SelectSingleNode("Country") == null ? null : ItemRef.SelectSingleNode("Country").InnerText;
					aRRefundCreditCardRet.Address.Note = ItemRef.SelectSingleNode("Note") == null ? null : ItemRef.SelectSingleNode("Note").InnerText;
				}
				#endregion

				#region AddressBlock
				if (Item.SelectSingleNode("AddressBlock") == null) {
					aRRefundCreditCardRet.AddressBlock.Addr1 = null;
					aRRefundCreditCardRet.AddressBlock.Addr2 = null;
					aRRefundCreditCardRet.AddressBlock.Addr3 = null;
					aRRefundCreditCardRet.AddressBlock.Addr4 = null;
					aRRefundCreditCardRet.AddressBlock.Addr5 = null;			
				}
				else {
					ItemRef = Item.SelectSingleNode("AddressBlock");
					aRRefundCreditCardRet.AddressBlock.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
					aRRefundCreditCardRet.AddressBlock.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
					aRRefundCreditCardRet.AddressBlock.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
					aRRefundCreditCardRet.AddressBlock.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
					aRRefundCreditCardRet.AddressBlock.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
				}
				#endregion

				#region PaymentMethodRef
				if (Item.SelectSingleNode("PaymentMethodRef") == null) {
					aRRefundCreditCardRet.PaymentMethodRef.ListID = null;
					aRRefundCreditCardRet.PaymentMethodRef.FullName = null;
				}
				else {
					ItemRef = Item.SelectSingleNode("PaymentMethodRef");
					aRRefundCreditCardRet.PaymentMethodRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
					aRRefundCreditCardRet.PaymentMethodRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
				}
				#endregion
				
				#region CreditCardTxnInfo List

				LineItemList = Item.SelectNodes("CreditCardTxnInfo");
				for (int j = 0; j < LineItemList.Count; j++) {
					LineItem = LineItemList.Item(j);
					aRRefundCreditCardTxnInfo = new CreditCardTxnInfo();

							#region CreditCardTxnInputInfo
							if (LineItem.SelectSingleNode("CreditCardTxnInputInfo") == null) {
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardNumber = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationMonth = 0;
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationYear = 0;
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.NameOnCard = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType = null;
					}
					else {
						ItemRef = LineItem.SelectSingleNode("CreditCardTxnInputInfo");
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardNumber = ItemRef.SelectSingleNode("CreditCardNumber") == null ? null : ItemRef.SelectSingleNode("CreditCardNumber").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationMonth = ItemRef.SelectSingleNode("ExpirationMonth") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("ExpirationMonth").InnerText);
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationYear = ItemRef.SelectSingleNode("ExpirationYear") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("ExpirationYear").InnerText);
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.NameOnCard = ItemRef.SelectSingleNode("NameOnCard") == null ? null : ItemRef.SelectSingleNode("NameOnCard").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress = ItemRef.SelectSingleNode("CreditCardAddress") == null ? null : ItemRef.SelectSingleNode("CreditCardAddress").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode = ItemRef.SelectSingleNode("CreditCardPostalCode") == null ? null : ItemRef.SelectSingleNode("CreditCardPostalCode").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode = ItemRef.SelectSingleNode("CommercialCardCode") == null ? null : ItemRef.SelectSingleNode("CommercialCardCode").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode = ItemRef.SelectSingleNode("TransactionMode") == null ? null : ItemRef.SelectSingleNode("TransactionMode").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType = ItemRef.SelectSingleNode("CreditCardTxnType") == null ? null : ItemRef.SelectSingleNode("CreditCardTxnType").InnerText;
							}
							#endregion

							#region CreditCardTxnResultInfo
							if(LineItem.SelectSingleNode("CreditCardTxnResultInfo") == null) {
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.ResultCode = 0;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.ResultMessage = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.CreditCardTransID = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.MerchantAccountNumber = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchId = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode = 0;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.PaymentStatus = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationTime = null;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp = 0;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp = 0;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID = null;

							}
							else {
								ItemRef = LineItem.SelectSingleNode("CreditCardTxnResultInfo");
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.ResultCode = ItemRef.SelectSingleNode("ResultCode") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("ResultCode").InnerText);
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.ResultMessage = ItemRef.SelectSingleNode("ResultMessage") == null ? null : ItemRef.SelectSingleNode("ResultMessage").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.CreditCardTransID =  ItemRef.SelectSingleNode("CreditCardTransID") == null ? null : ItemRef.SelectSingleNode("CreditCardTransID").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.MerchantAccountNumber = ItemRef.SelectSingleNode("MerchantAccountNumber") == null ? null : ItemRef.SelectSingleNode("MerchantAccountNumber").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode = ItemRef.SelectSingleNode("AuthorizationCode") == null ? null : ItemRef.SelectSingleNode("AuthorizationCode").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet = ItemRef.SelectSingleNode("AVSStreet") == null ? null : ItemRef.SelectSingleNode("AVSStreet").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip = ItemRef.SelectSingleNode("AVSZip") == null ? null : ItemRef.SelectSingleNode("AVSZip").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch = ItemRef.SelectSingleNode("CardSecurityCodeMatch") == null ? null : ItemRef.SelectSingleNode("CardSecurityCodeMatch").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchId = ItemRef.SelectSingleNode("ReconBatchId") == null ? null : ItemRef.SelectSingleNode("ReconBatchId").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode = ItemRef.SelectSingleNode("PaymentGroupingCode") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("PaymentGroupingCode").InnerText);
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.PaymentStatus = ItemRef.SelectSingleNode("PaymentStatus") == null ? null : ItemRef.SelectSingleNode("PaymentStatus").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationTime = ItemRef.SelectSingleNode("TxnAuthorizationTime") == null ? null : ItemRef.SelectSingleNode("TxnAuthorizationTime").InnerText;
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp = ItemRef.SelectSingleNode("TxnAuthorizationStamp") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("TxnAuthorizationStamp").InnerText);
								aRRefundCreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID = ItemRef.SelectSingleNode("ClientTransID") == null ? null : ItemRef.SelectSingleNode("ClientTransID").InnerText;
							}

							#endregion

							aRRefundCreditCardRet.CreditCardTxnInfo.Add(aRRefundCreditCardTxnInfo);
							
						}
						#endregion

				list.Add(aRRefundCreditCardRet);
			}
			break;
				#endregion

				#region BillPaymentCheck
				case ActionType.BillPaymentCheckAdd:
				case ActionType.BillPaymentCheckQuery:
					BillPaymentCheckRet billPaymentCheckRet;
					AppliedToTxnRet billPaymentCheck_appliedToTxnRet;
					ItemList = responseNode.SelectNodes("//BillPaymentCheckRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						billPaymentCheckRet = new BillPaymentCheckRet {
							TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
							TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
							Amount = Item.SelectSingleNode("Amount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("Amount ").InnerText),
							ExchangeRate = Item.SelectSingleNode("ExchangeRate") == null ? 0 : decimal.Parse(Item.SelectSingleNode("ExchangeRate ").InnerText),
							AmountDueInHomeCurrency = Item.SelectSingleNode("AmountDueInHomeCurrency") == null ? 0 : decimal.Parse(Item.SelectSingleNode("AmountDueInHomeCurrency").InnerText),
							RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
							Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
							IsToBePrinted = Item.SelectSingleNode("IsToBePrinted") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsToBePrinted").InnerText),
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
						};

						#region PayeeEntityRef
						if (Item.SelectSingleNode("PayeeEntityRef") == null) {
							billPaymentCheckRet.PayeeEntityRef.ListID = null;
							billPaymentCheckRet.PayeeEntityRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("PayeeEntityRef");
							billPaymentCheckRet.PayeeEntityRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billPaymentCheckRet.PayeeEntityRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region APAccountRef
						if (Item.SelectSingleNode("APAccountRef") == null) {
							billPaymentCheckRet.APAccountRef.ListID = null;
							billPaymentCheckRet.APAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("APAccountRef");
							billPaymentCheckRet.APAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billPaymentCheckRet.APAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region BankAccountRef
						if (Item.SelectSingleNode("BankAccountRef") == null) {
							billPaymentCheckRet.BankAccountRef.ListID = null;
							billPaymentCheckRet.BankAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("BankAccountRef");
							billPaymentCheckRet.BankAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billPaymentCheckRet.BankAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CurrencyRef
						if (Item.SelectSingleNode("CurrencyRef") == null) {
							billPaymentCheckRet.CurrencyRef.ListID = null;
							billPaymentCheckRet.CurrencyRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CurrencyRef");
							billPaymentCheckRet.CurrencyRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billPaymentCheckRet.CurrencyRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region Address
						if (Item.SelectSingleNode("Address") == null) {
							billPaymentCheckRet.Address.Addr1 = null;
							billPaymentCheckRet.Address.Addr2 = null;
							billPaymentCheckRet.Address.Addr3 = null;
							billPaymentCheckRet.Address.Addr4 = null;
							billPaymentCheckRet.Address.Addr5 = null;
							billPaymentCheckRet.Address.City = null;
							billPaymentCheckRet.Address.State = null;
							billPaymentCheckRet.Address.PostalCode = null;
							billPaymentCheckRet.Address.Country = null;
							billPaymentCheckRet.Address.Note = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("Address");
							billPaymentCheckRet.Address.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							billPaymentCheckRet.Address.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							billPaymentCheckRet.Address.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							billPaymentCheckRet.Address.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							billPaymentCheckRet.Address.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
							billPaymentCheckRet.Address.City = ItemRef.SelectSingleNode("City") == null ? null : ItemRef.SelectSingleNode("City").InnerText;
							billPaymentCheckRet.Address.State = ItemRef.SelectSingleNode("State") == null ? null : ItemRef.SelectSingleNode("State").InnerText;
							billPaymentCheckRet.Address.PostalCode = ItemRef.SelectSingleNode("PostalCode") == null ? null : ItemRef.SelectSingleNode("PostalCode").InnerText;
							billPaymentCheckRet.Address.Country = ItemRef.SelectSingleNode("Country") == null ? null : ItemRef.SelectSingleNode("Country").InnerText;
							billPaymentCheckRet.Address.Note = ItemRef.SelectSingleNode("Note") == null ? null : ItemRef.SelectSingleNode("Note").InnerText;
						}
						#endregion

						#region AddressBlock
						if (Item.SelectSingleNode("AddressBlock") == null) {
							billPaymentCheckRet.AddressBlock.Addr1 = null;
							billPaymentCheckRet.AddressBlock.Addr2 = null;
							billPaymentCheckRet.AddressBlock.Addr3 = null;
							billPaymentCheckRet.AddressBlock.Addr4 = null;
							billPaymentCheckRet.AddressBlock.Addr5 = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("AddressBlock");
							billPaymentCheckRet.AddressBlock.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							billPaymentCheckRet.AddressBlock.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							billPaymentCheckRet.AddressBlock.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							billPaymentCheckRet.AddressBlock.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							billPaymentCheckRet.AddressBlock.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
						}
						#endregion

						#region DataExtRet
						if (Item.SelectSingleNode("DataExtRet") == null) {
							billPaymentCheckRet.DataExtRet.OwnerID = null;
							billPaymentCheckRet.DataExtRet.DataExtName = null;
							billPaymentCheckRet.DataExtRet.DataExtType = null;
							billPaymentCheckRet.DataExtRet.DataExtValue = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("DataExtRet");
							billPaymentCheckRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
							billPaymentCheckRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
							billPaymentCheckRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
							billPaymentCheckRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
						}
						#endregion

						#region AppliedToTxnRet LineItems
						LineItemList = Item.SelectNodes("AppliedToTxnRet");

						for (int j = 0; j < LineItemList.Count; j++) {
							LineItem = LineItemList.Item(j);
							billPaymentCheck_appliedToTxnRet = new AppliedToTxnRet {
								TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
								TxnType = LineItem.SelectSingleNode("TxnType") == null ? null : LineItem.SelectSingleNode("TxnType").InnerText,
								TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
								RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
								BalanceRemaining = Item.SelectSingleNode("BalanceRemaining") == null ? 0 : decimal.Parse(Item.SelectSingleNode("BalanceRemaining").InnerText),
								Amount = Item.SelectSingleNode("Amount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("Amount").InnerText),
								DiscountAmount = Item.SelectSingleNode("DiscountAmount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("DiscountAmount").InnerText),
							};

							#region DiscountAccountRef
							if (LineItem.SelectSingleNode("DiscountAccountRef") == null) {
								billPaymentCheck_appliedToTxnRet.DiscountAccountRef.ListID = null;
								billPaymentCheck_appliedToTxnRet.DiscountAccountRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("DiscountAccountRef");
								billPaymentCheck_appliedToTxnRet.DiscountAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								billPaymentCheck_appliedToTxnRet.DiscountAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region DiscountClassRef
							if (LineItem.SelectSingleNode("DiscountClassRef") == null) {
								billPaymentCheck_appliedToTxnRet.DiscountClassRef.ListID = null;
								billPaymentCheck_appliedToTxnRet.DiscountClassRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("DiscountClassRef");
								billPaymentCheck_appliedToTxnRet.DiscountClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								billPaymentCheck_appliedToTxnRet.DiscountClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region LinkedTxn
							if (Item.SelectSingleNode("LinkedTxn") == null) {
								billPaymentCheck_appliedToTxnRet.LinkedTxn.TxnID = null;
								billPaymentCheck_appliedToTxnRet.LinkedTxn.TxnType = null;
								billPaymentCheck_appliedToTxnRet.LinkedTxn.TxnDate = null;
								billPaymentCheck_appliedToTxnRet.LinkedTxn.RefNumber = null;
								billPaymentCheck_appliedToTxnRet.LinkedTxn.LinkType = null;
								billPaymentCheck_appliedToTxnRet.LinkedTxn.Amount = null;

							}
							else {
								ItemRef = Item.SelectSingleNode("LinkedTxn");
								billPaymentCheck_appliedToTxnRet.LinkedTxn.TxnID = ItemRef.SelectSingleNode("TxnID") == null ? null : ItemRef.SelectSingleNode("TxnID").InnerText;
								billPaymentCheck_appliedToTxnRet.LinkedTxn.TxnType = ItemRef.SelectSingleNode("TxnType") == null ? null : ItemRef.SelectSingleNode("TxnType").InnerText;
								billPaymentCheck_appliedToTxnRet.LinkedTxn.TxnDate = ItemRef.SelectSingleNode("TxnDate") == null ? null : ItemRef.SelectSingleNode("TxnDate").InnerText;
								billPaymentCheck_appliedToTxnRet.LinkedTxn.RefNumber = ItemRef.SelectSingleNode("RefNumber") == null ? null : ItemRef.SelectSingleNode("RefNumber").InnerText;
								billPaymentCheck_appliedToTxnRet.LinkedTxn.LinkType = ItemRef.SelectSingleNode("LinkType") == null ? null : ItemRef.SelectSingleNode("LinkType").InnerText;
								billPaymentCheck_appliedToTxnRet.LinkedTxn.Amount = ItemRef.SelectSingleNode("Amount") == null ? (decimal?) null : decimal.Parse(ItemRef.SelectSingleNode("Amount").InnerText);
							}
							#endregion


							billPaymentCheckRet.AppliedToTxnRet.Add(billPaymentCheck_appliedToTxnRet);
						}
						#endregion LineItems

						list.Add(billPaymentCheckRet);
					}

						break;
				#endregion

				#region BillPaymentCreditCard
				case ActionType.BillPaymentCreditCardAdd:
				case ActionType.BillPaymentCreditCardQuery:
					BillPaymentCreditCardRet BillPaymentCCRet;
					AppliedToTxnRet billPaymentCC_AppliedToTxnRet;
					ItemList = responseNode.SelectNodes("//BillPaymentCreditCardRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						BillPaymentCCRet = new BillPaymentCreditCardRet {
							TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
							TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
							Amount = Item.SelectSingleNode("Amount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("Amount ").InnerText),
							ExchangeRate = Item.SelectSingleNode("ExchangeRate") == null ? 0 : decimal.Parse(Item.SelectSingleNode("ExchangeRate").InnerText),
							AmountInHomeCurrency = Item.SelectSingleNode("AmountInHomeCurrency") == null ? 0 : decimal.Parse(Item.SelectSingleNode("AmountInHomeCurrency").InnerText),
							RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
							Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
						};

						#region PayeeEntityRef
						if (Item.SelectSingleNode("PayeeEntityRef") == null) {
							BillPaymentCCRet.PayeeEntityRef.ListID = null;
							BillPaymentCCRet.PayeeEntityRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("PayeeEntityRef");
							BillPaymentCCRet.PayeeEntityRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							BillPaymentCCRet.PayeeEntityRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region APAccountRef
						if (Item.SelectSingleNode("APAccountRef") == null) {
							BillPaymentCCRet.APAccountRef.ListID = null;
							BillPaymentCCRet.APAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("APAccountRef");
							BillPaymentCCRet.APAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							BillPaymentCCRet.APAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CreditCardAccountRef
						if (Item.SelectSingleNode("CreditCardAccountRef") == null) {
							BillPaymentCCRet.CreditCardAccountRef.ListID = null;
							BillPaymentCCRet.CreditCardAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CreditCardAccountRef");
							BillPaymentCCRet.CreditCardAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							BillPaymentCCRet.CreditCardAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CurrencyRef
						if (Item.SelectSingleNode("CurrencyRef") == null) {
							BillPaymentCCRet.CurrencyRef.ListID = null;
							BillPaymentCCRet.CurrencyRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CurrencyRef");
							BillPaymentCCRet.CurrencyRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							BillPaymentCCRet.CurrencyRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region AppliedToTxnRet LineItems
						LineItemList = Item.SelectNodes("AppliedToTxnRet");

						for (int j = 0; j < LineItemList.Count; j++) {
							LineItem = LineItemList.Item(j);
							billPaymentCC_AppliedToTxnRet = new AppliedToTxnRet {
								TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
								TxnType = LineItem.SelectSingleNode("TxnType") == null ? null : LineItem.SelectSingleNode("TxnType").InnerText,
								TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
								RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
								BalanceRemaining = Item.SelectSingleNode("BalanceRemaining") == null ? 0 : decimal.Parse(Item.SelectSingleNode("BalanceRemaining").InnerText),
								Amount = Item.SelectSingleNode("Amount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("Amount").InnerText),
								DiscountAmount = Item.SelectSingleNode("DiscountAmount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("DiscountAmount").InnerText),
							};

							#region DiscountAccountRef
							if (LineItem.SelectSingleNode("DiscountAccountRef") == null) {
								billPaymentCC_AppliedToTxnRet.DiscountAccountRef.ListID = null;
								billPaymentCC_AppliedToTxnRet.DiscountAccountRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("DiscountAccountRef");
								billPaymentCC_AppliedToTxnRet.DiscountAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								billPaymentCC_AppliedToTxnRet.DiscountAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region DiscountClassRef
							if (LineItem.SelectSingleNode("DiscountClassRef") == null) {
								billPaymentCC_AppliedToTxnRet.DiscountClassRef.ListID = null;
								billPaymentCC_AppliedToTxnRet.DiscountClassRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("DiscountClassRef");
								billPaymentCC_AppliedToTxnRet.DiscountClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								billPaymentCC_AppliedToTxnRet.DiscountClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region LinkedTxn
							if (Item.SelectSingleNode("LinkedTxn") == null) {
								billPaymentCC_AppliedToTxnRet.LinkedTxn.TxnID = null;
								billPaymentCC_AppliedToTxnRet.LinkedTxn.TxnType = null;
								billPaymentCC_AppliedToTxnRet.LinkedTxn.TxnDate = null;
								billPaymentCC_AppliedToTxnRet.LinkedTxn.RefNumber = null;
								billPaymentCC_AppliedToTxnRet.LinkedTxn.LinkType = null;
								billPaymentCC_AppliedToTxnRet.LinkedTxn.Amount = null;

							}
							else {
								ItemRef = Item.SelectSingleNode("LinkedTxn");
								billPaymentCC_AppliedToTxnRet.LinkedTxn.TxnID = ItemRef.SelectSingleNode("TxnID") == null ? null : ItemRef.SelectSingleNode("TxnID").InnerText;
								billPaymentCC_AppliedToTxnRet.LinkedTxn.TxnType = ItemRef.SelectSingleNode("TxnType") == null ? null : ItemRef.SelectSingleNode("TxnType").InnerText;
								billPaymentCC_AppliedToTxnRet.LinkedTxn.TxnDate = ItemRef.SelectSingleNode("TxnDate") == null ? null : ItemRef.SelectSingleNode("TxnDate").InnerText;
								billPaymentCC_AppliedToTxnRet.LinkedTxn.RefNumber = ItemRef.SelectSingleNode("RefNumber") == null ? null : ItemRef.SelectSingleNode("RefNumber").InnerText;
								billPaymentCC_AppliedToTxnRet.LinkedTxn.LinkType = ItemRef.SelectSingleNode("LinkType") == null ? null : ItemRef.SelectSingleNode("LinkType").InnerText;
								billPaymentCC_AppliedToTxnRet.LinkedTxn.Amount = ItemRef.SelectSingleNode("Amount") == null ? (decimal?) null : decimal.Parse(ItemRef.SelectSingleNode("Amount").InnerText);
							}
							#endregion

							BillPaymentCCRet.AppliedToTxnRet.Add(billPaymentCC_AppliedToTxnRet);
						}
						#endregion LineItems

						#region DataExtRet
						if (Item.SelectSingleNode("DataExtRet") == null) {
							BillPaymentCCRet.DataExtRet.OwnerID = null;
							BillPaymentCCRet.DataExtRet.DataExtName = null;
							BillPaymentCCRet.DataExtRet.DataExtType = null;
							BillPaymentCCRet.DataExtRet.DataExtValue = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("DataExtRet");
							BillPaymentCCRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
							BillPaymentCCRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
							BillPaymentCCRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
							BillPaymentCCRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
						}
						#endregion

						list.Add(BillPaymentCCRet);
					}

					break;
				#endregion

				#region Bill
				case ActionType.BillAdd:
				case ActionType.BillQuery:
					BillRet billRet;
					Bill_ExpenseLineRet billLineRet;
					ItemList = responseNode.SelectNodes("//BillRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						billRet = new BillRet {
							TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
							TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
							DueDate = DateTime.Parse(Item.SelectSingleNode("DueDate").InnerText),
							AmountDue = Item.SelectSingleNode("AmountDue") == null ? 0 : decimal.Parse(Item.SelectSingleNode("AmountDue").InnerText),
							ExchangeRate = Item.SelectSingleNode("ExchangeRate") == null ? 0 : decimal.Parse(Item.SelectSingleNode("ExchangeRate").InnerText),
							AmountDueInHomeCurrency = Item.SelectSingleNode("AmountDueInHomeCurrency") == null ? 0 : decimal.Parse(Item.SelectSingleNode("AmountDueInHomeCurrency ").InnerText),
							RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
							Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
							IsTaxIncluded = Item.SelectSingleNode("IsTaxIncluded") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsTaxIncluded").InnerText),
							IsPaid = Item.SelectSingleNode("IsPaid") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsPaid").InnerText),
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
							OpenAmount = Item.SelectSingleNode("OpenAmount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("OpenAmount").InnerText),
						};

						#region VendorRef
						if (Item.SelectSingleNode("VendorRef") == null) {
							billRet.VendorRef.ListID = null;
							billRet.VendorRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("VendorRef");
							billRet.VendorRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billRet.VendorRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion VendorRef

						#region VendorAddress
						if (Item.SelectSingleNode("VendorAddress") == null) {
							billRet.VendorAddress.Addr1 = null;
							billRet.VendorAddress.Addr2 = null;
							billRet.VendorAddress.Addr3 = null;
							billRet.VendorAddress.Addr4 = null;
							billRet.VendorAddress.Addr5 = null;
							billRet.VendorAddress.City = null;
							billRet.VendorAddress.State = null;
							billRet.VendorAddress.PostalCode = null;
							billRet.VendorAddress.Country = null;
							billRet.VendorAddress.Note = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("VendorAddress");
							billRet.VendorAddress.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							billRet.VendorAddress.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							billRet.VendorAddress.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							billRet.VendorAddress.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							billRet.VendorAddress.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
							billRet.VendorAddress.City = ItemRef.SelectSingleNode("City") == null ? null : ItemRef.SelectSingleNode("City").InnerText;
							billRet.VendorAddress.State = ItemRef.SelectSingleNode("State") == null ? null : ItemRef.SelectSingleNode("State").InnerText;
							billRet.VendorAddress.PostalCode = ItemRef.SelectSingleNode("PostalCode") == null ? null : ItemRef.SelectSingleNode("PostalCode").InnerText;
							billRet.VendorAddress.Country = ItemRef.SelectSingleNode("Country") == null ? null : ItemRef.SelectSingleNode("Country").InnerText;
							billRet.VendorAddress.Note = ItemRef.SelectSingleNode("Note") == null ? null : ItemRef.SelectSingleNode("Note").InnerText;
						}
						#endregion

						#region APAccountRef
						if (Item.SelectSingleNode("APAccountRef") == null) {
							billRet.APAccountRef.ListID = null;
							billRet.APAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("APAccountRef");
							billRet.APAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billRet.APAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CurrencyRef
						if (Item.SelectSingleNode("CurrencyRef") == null) {
							billRet.CurrencyRef.ListID = null;
							billRet.CurrencyRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("CurrencyRef");
							billRet.CurrencyRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billRet.CurrencyRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region TermsRef
						if (Item.SelectSingleNode("TermsRef") == null) {
							billRet.TermsRef.ListID = null;
							billRet.TermsRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("TermsRef");
							billRet.TermsRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billRet.TermsRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region SalesTaxCodeRef
						if (Item.SelectSingleNode("SalesTaxCodeRef") == null) {
							billRet.SalesTaxCodeRef.ListID = null;
							billRet.SalesTaxCodeRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("SalesTaxCodeRef");
							billRet.SalesTaxCodeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billRet.SalesTaxCodeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region LinkedTxn
						if (Item.SelectSingleNode("LinkedTxn") == null) {
							billRet.LinkedTxn.TxnID = null;
							billRet.LinkedTxn.TxnType = null;
							billRet.LinkedTxn.TxnDate = null;
							billRet.LinkedTxn.RefNumber = null;
							billRet.LinkedTxn.LinkType = null;
							billRet.LinkedTxn.Amount = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("LinkedTxn");
							billRet.LinkedTxn.TxnID = ItemRef.SelectSingleNode("TxnID") == null ? null : ItemRef.SelectSingleNode("TxnID").InnerText;
							billRet.LinkedTxn.TxnType = ItemRef.SelectSingleNode("TxnType") == null ? null : ItemRef.SelectSingleNode("TxnType").InnerText;
							billRet.LinkedTxn.TxnDate = ItemRef.SelectSingleNode("TxnDate") == null ? null : ItemRef.SelectSingleNode("TxnDate").InnerText;
							billRet.LinkedTxn.RefNumber = ItemRef.SelectSingleNode("RefNumber") == null ? null : ItemRef.SelectSingleNode("RefNumber").InnerText;
							billRet.LinkedTxn.LinkType = ItemRef.SelectSingleNode("LinkType") == null ? null : ItemRef.SelectSingleNode("LinkType").InnerText;
							billRet.LinkedTxn.Amount = ItemRef.SelectSingleNode("Amount") == null ? (decimal?) null : decimal.Parse(ItemRef.SelectSingleNode("Amount").InnerText);
						}
						#endregion

						#region DataExtRet
						if (Item.SelectSingleNode("DataExtRet") == null) {
							billRet.DataExtRet.OwnerID = null;
							billRet.DataExtRet.DataExtName = null;
							billRet.DataExtRet.DataExtType = null;
							billRet.DataExtRet.DataExtValue = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("DataExtRet");
							billRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
							billRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
							billRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
							billRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
						}
						#endregion

						#region LineItems
						LineItemList = Item.SelectNodes("Bill_ExperienceLineRet");

						for (int j = 0; j < LineItemList.Count; j++) {
							LineItem = LineItemList.Item(j);
							billLineRet = new Bill_ExpenseLineRet {
								TxnLineID = LineItem.SelectSingleNode("TxnLineID") == null ? null : LineItem.SelectSingleNode("TxnLineID").InnerText,
								Amount = LineItem.SelectSingleNode("Amount") == null ? (decimal?) null : decimal.Parse(LineItem.SelectSingleNode("Amount").InnerText),
								Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
								BillableStatus = LineItem.SelectSingleNode("BillableStatus") == null ? null : LineItem.SelectSingleNode("BillableStatus").InnerText,
							};

							#region AccountRef
							if (LineItem.SelectSingleNode("AccountRef") == null) {
								billLineRet.AccountRef.ListID = null;
								billLineRet.AccountRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("AccountRef");
								billLineRet.AccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								billLineRet.AccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion 

							#region AccountRef
							if (LineItem.SelectSingleNode("AccountRef") == null) {
								billLineRet.AccountRef.ListID = null;
								billLineRet.AccountRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("AccountRef");
								billLineRet.AccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								billLineRet.AccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion 

							#region CustomerRef
							if (Item.SelectSingleNode("CustomerRef") == null) {
								billLineRet.CustomerRef.ListID = null;
								billLineRet.CustomerRef.FullName = null;
							}

							else {
								ItemRef = Item.SelectSingleNode("CustomerRef");
								billLineRet.CustomerRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								billLineRet.CustomerRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region ClassRef
							if (Item.SelectSingleNode("ClassRef") == null) {
								billLineRet.ClassRef.ListID = null;
								billLineRet.ClassRef.FullName = null;
							}

							else {
								ItemRef = Item.SelectSingleNode("ClassRef");
								billLineRet.ClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								billLineRet.ClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region SalesTaxCodeRef
							if (Item.SelectSingleNode("SalesTaxCodeRef") == null) {
								billLineRet.SalesTaxCodeRef.ListID = null;
								billLineRet.SalesTaxCodeRef.FullName = null;
							}

							else {
								ItemRef = Item.SelectSingleNode("SalesTaxCodeRef");
								billLineRet.SalesTaxCodeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								billLineRet.SalesTaxCodeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region DataExtRet
							if (Item.SelectSingleNode("DataExtRet") == null) {
								billLineRet.DataExtRet.OwnerID = null;
								billLineRet.DataExtRet.DataExtName = null;
								billLineRet.DataExtRet.DataExtType = null;
								billLineRet.DataExtRet.DataExtValue = null;
							}
							else {
								ItemRef = Item.SelectSingleNode("DataExtRet");
								billLineRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
								billLineRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
								billLineRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
								billLineRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
							}
							#endregion

							billRet.ExpenseLines.Add(billLineRet);
						}
						#endregion LineItems

						list.Add(billRet);
					}
					break;

				#endregion

				#region ChargeQuery
				case ActionType.ChargeAdd:
				case ActionType.ChargeQuery:
					ChargeRet chargeRet;
					ItemList = responseNode.SelectNodes("//ChargeRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);

						chargeRet = new ChargeRet {
							TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
							RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
							Rate = Item.SelectSingleNode("Rate") == null ? (float?) null : float.Parse(Item.SelectSingleNode("Rate").InnerText),
							Amount = Item.SelectSingleNode("Amount") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("Amount").InnerText),
							BalanceRemaining = Item.SelectSingleNode("BalanceRemaining ") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("BalanceRemaining ").InnerText),
							Desc = Item.SelectSingleNode("Desc") == null ? null : Item.SelectSingleNode("Desc").InnerText,
							BilledDate = DateTime.Parse(Item.SelectSingleNode("BilledDate").InnerText),
							DueDate = DateTime.Parse(Item.SelectSingleNode("DueDate").InnerText),
							IsPaid = Item.SelectSingleNode("IsPaid") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsPaid").InnerText),
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
						};


						#region CustomerRef
						if (Item.SelectSingleNode("CustomerRef") == null) {
							chargeRet.CustomerRef.ListID = null;
							chargeRet.CustomerRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CustomerRef");
							chargeRet.CustomerRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							chargeRet.CustomerRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region ItemRef
						if (Item.SelectSingleNode("ItemRef") == null) {
							chargeRet.ItemRef.ListID = null;
							chargeRet.ItemRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("ItemRef");
							chargeRet.ItemRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							chargeRet.ItemRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region InventorySiteRef
						if (Item.SelectSingleNode("InventorySiteRef") == null) {
							chargeRet.InventorySiteRef.ListID = null;
							chargeRet.InventorySiteRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("InventorySiteRef");
							chargeRet.InventorySiteRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							chargeRet.InventorySiteRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region InventorySiteLocationRef
						if (Item.SelectSingleNode("InventorySiteLocationRef") == null) {
							chargeRet.InventorySiteLocationRef.ListID = null;
							chargeRet.InventorySiteLocationRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("InventorySiteLocationRef");
							chargeRet.InventorySiteLocationRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							chargeRet.InventorySiteLocationRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region OverrideUOMSetRef
						if (Item.SelectSingleNode("OverrideUOMSetRef") == null) {
							chargeRet.OverrideUOMSetRef.ListID = null;
							chargeRet.OverrideUOMSetRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("OverrideUOMSetRef");
							chargeRet.OverrideUOMSetRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							chargeRet.OverrideUOMSetRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region ARAccountRef
						if (Item.SelectSingleNode("ARAccountRef") == null) {
							chargeRet.ARAccountRef.ListID = null;
							chargeRet.ARAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("ARAccountRef");
							chargeRet.ARAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							chargeRet.ARAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region ClassRef
						if (Item.SelectSingleNode("ClassRef") == null) {
							chargeRet.ClassRef.ListID = null;
							chargeRet.ClassRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("ClassRef");
							chargeRet.ClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							chargeRet.ClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region LinkedTxn
						if (Item.SelectSingleNode("LinkedTxn") == null) {
							chargeRet.LinkedTxn.TxnID = null;
							chargeRet.LinkedTxn.TxnType = null;
							chargeRet.LinkedTxn.TxnDate = null;
							chargeRet.LinkedTxn.RefNumber = null;
							chargeRet.LinkedTxn.LinkType = null;
							chargeRet.LinkedTxn.Amount = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("LinkedTxn");
							chargeRet.LinkedTxn.TxnID = ItemRef.SelectSingleNode("TxnID") == null ? null : ItemRef.SelectSingleNode("TxnID").InnerText;
							chargeRet.LinkedTxn.TxnType = ItemRef.SelectSingleNode("TxnType") == null ? null : ItemRef.SelectSingleNode("TxnType").InnerText;
							chargeRet.LinkedTxn.TxnDate = ItemRef.SelectSingleNode("TxnDate") == null ? null : ItemRef.SelectSingleNode("TxnDate").InnerText;
							chargeRet.LinkedTxn.RefNumber = ItemRef.SelectSingleNode("RefNumber") == null ? null : ItemRef.SelectSingleNode("RefNumber").InnerText;
							chargeRet.LinkedTxn.LinkType = ItemRef.SelectSingleNode("LinkType") == null ? null : ItemRef.SelectSingleNode("LinkType").InnerText;
							chargeRet.LinkedTxn.Amount = ItemRef.SelectSingleNode("Amount") == null ? (decimal?) null : decimal.Parse(ItemRef.SelectSingleNode("Amount").InnerText);
						}
						#endregion

						#region DataExtRet
						if (Item.SelectSingleNode("DataExtRet") == null) {
							chargeRet.DataExtRet.OwnerID = null;
							chargeRet.DataExtRet.DataExtName = null;
							chargeRet.DataExtRet.DataExtType = null;
							chargeRet.DataExtRet.DataExtValue = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("DataExtRet");
							chargeRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
							chargeRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
							chargeRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
							chargeRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
						}
						#endregion
					}
					break;
				#endregion

				#region CheckQuery
				case ActionType.CheckAdd:
				case ActionType.CheckQuery:
					CheckRet checkRet;
					Check_ExpenseLineRet expenseLineRet;
					ItemList = responseNode.SelectNodes("//CheckRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						checkRet = new CheckRet {
							TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
							RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
							TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
							Amount = Item.SelectSingleNode("Amount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("Amount ").InnerText),
							ExchangeRate = Item.SelectSingleNode("ExchangeRate") == null ? 0 : decimal.Parse(Item.SelectSingleNode("ExchangeRate ").InnerText),
							AmountInHomeCurrency = Item.SelectSingleNode("AmountInHomeCurrency") == null ? 0 : decimal.Parse(Item.SelectSingleNode("AmountInHomeCurrency").InnerText),
							Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
							IsToBePrinted = Item.SelectSingleNode("IsToBePrinted") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsToBePrinted").InnerText),
							IsTaxIncluded = Item.SelectSingleNode("IsTaxIncluded") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsTaxIncluded").InnerText),
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
						};

						#region AccountRef
						if (Item.SelectSingleNode("AccountRef") == null) {
							checkRet.AccountRef.ListID = null;
							checkRet.AccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("AccountRef");
							checkRet.AccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							checkRet.AccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region PayeeEntityRef
						if (Item.SelectSingleNode("PayeeEntityRef") == null) {
							checkRet.PayeeEntityRef.ListID = null;
							checkRet.PayeeEntityRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("PayeeEntityRef");
							checkRet.PayeeEntityRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							checkRet.PayeeEntityRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CurrencyRef
						if (Item.SelectSingleNode("CurrencyRef") == null) {
							checkRet.CurrencyRef.ListID = null;
							checkRet.CurrencyRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CurrencyRef");
							checkRet.CurrencyRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							checkRet.CurrencyRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region Address
						if (Item.SelectSingleNode("Address") == null) {
							checkRet.Address.Addr1 = null;
							checkRet.Address.Addr2 = null;
							checkRet.Address.Addr3 = null;
							checkRet.Address.Addr4 = null;
							checkRet.Address.Addr5 = null;
							checkRet.Address.City = null;
							checkRet.Address.State = null;
							checkRet.Address.PostalCode = null;
							checkRet.Address.Country = null;
							checkRet.Address.Note = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("Address");
							checkRet.Address.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							checkRet.Address.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							checkRet.Address.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							checkRet.Address.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							checkRet.Address.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
							checkRet.Address.City = ItemRef.SelectSingleNode("City") == null ? null : ItemRef.SelectSingleNode("City").InnerText;
							checkRet.Address.State = ItemRef.SelectSingleNode("State") == null ? null : ItemRef.SelectSingleNode("State").InnerText;
							checkRet.Address.PostalCode = ItemRef.SelectSingleNode("PostalCode") == null ? null : ItemRef.SelectSingleNode("PostalCode").InnerText;
							checkRet.Address.Country = ItemRef.SelectSingleNode("Country") == null ? null : ItemRef.SelectSingleNode("Country").InnerText;
							checkRet.Address.Note = ItemRef.SelectSingleNode("Note") == null ? null : ItemRef.SelectSingleNode("Note").InnerText;
						}
						#endregion

						#region AddressBlock
						if (Item.SelectSingleNode("AddressBlock") == null) {
							checkRet.AddressBlock.Addr1 = null;
							checkRet.AddressBlock.Addr2 = null;
							checkRet.AddressBlock.Addr3 = null;
							checkRet.AddressBlock.Addr4 = null;
							checkRet.AddressBlock.Addr5 = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("AddressBlock");
							checkRet.AddressBlock.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							checkRet.AddressBlock.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							checkRet.AddressBlock.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							checkRet.AddressBlock.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							checkRet.AddressBlock.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
						}
						#endregion

						#region SalesTaxCodeRef
						if (Item.SelectSingleNode("SalesTaxCodeRef") == null) {
							checkRet.CurrencyRef.ListID = null;
							checkRet.CurrencyRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("SalesTaxCodeRef");
							checkRet.SalesTaxCodeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							checkRet.SalesTaxCodeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region LinkedTxn
						if (Item.SelectSingleNode("LinkedTxn") == null) {
							checkRet.LinkedTxn.TxnID = null;
							checkRet.LinkedTxn.TxnType = null;
							checkRet.LinkedTxn.TxnDate = null;
							checkRet.LinkedTxn.RefNumber = null;
							checkRet.LinkedTxn.LinkType = null;
							checkRet.LinkedTxn.Amount = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("LinkedTxn");
							checkRet.LinkedTxn.TxnID = ItemRef.SelectSingleNode("TxnID") == null ? null : ItemRef.SelectSingleNode("TxnID").InnerText;
							checkRet.LinkedTxn.TxnType = ItemRef.SelectSingleNode("TxnType") == null ? null : ItemRef.SelectSingleNode("TxnType").InnerText;
							checkRet.LinkedTxn.TxnDate = ItemRef.SelectSingleNode("TxnDate") == null ? null : ItemRef.SelectSingleNode("TxnDate").InnerText;
							checkRet.LinkedTxn.RefNumber = ItemRef.SelectSingleNode("RefNumber") == null ? null : ItemRef.SelectSingleNode("RefNumber").InnerText;
							checkRet.LinkedTxn.LinkType = ItemRef.SelectSingleNode("LinkType") == null ? null : ItemRef.SelectSingleNode("LinkType").InnerText;
							checkRet.LinkedTxn.Amount = ItemRef.SelectSingleNode("Amount") == null ? (decimal?) null : decimal.Parse(ItemRef.SelectSingleNode("Amount").InnerText);
						}
						#endregion

						#region DataExtRet
						if (Item.SelectSingleNode("DataExtRet") == null) {
							checkRet.DataExtRet.OwnerID = null;
							checkRet.DataExtRet.DataExtName = null;
							checkRet.DataExtRet.DataExtType = null;
							checkRet.DataExtRet.DataExtValue = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("DataExtRet");
							checkRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
							checkRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
							checkRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
							checkRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
						}
						#endregion

						#region Expense LineItems Return
						LineItemList = Item.SelectNodes("ExpenseLineRet");

						for (int j = 0; j < LineItemList.Count; j++) {
							LineItem = LineItemList.Item(j);
							expenseLineRet = new Check_ExpenseLineRet {
								TxnLineID = LineItem.SelectSingleNode("TxnLineID") == null ? null : LineItem.SelectSingleNode("TxnLineID").InnerText,
								Amount = Item.SelectSingleNode("Amount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("Amount").InnerText),
								Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
								BillableStatus = Item.SelectSingleNode("BillableStatus") == null ? null : Item.SelectSingleNode("BillableStatus").InnerText,
							};

							#region AccountRef
							if (LineItem.SelectSingleNode("AccountRef") == null) {
								expenseLineRet.AccountRef.ListID = null;
								expenseLineRet.AccountRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("AccountRef");
								expenseLineRet.AccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								expenseLineRet.AccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region CustomerRef
							if (LineItem.SelectSingleNode("CustomerRef") == null) {
								expenseLineRet.CustomerRef.ListID = null;
								expenseLineRet.CustomerRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("CustomerRef");
								expenseLineRet.CustomerRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								expenseLineRet.CustomerRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region ClassRef
							if (LineItem.SelectSingleNode("ClassRef") == null) {
								expenseLineRet.ClassRef.ListID = null;
								expenseLineRet.ClassRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("ClassRef");
								expenseLineRet.ClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								expenseLineRet.ClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region SalesTaxCodeRef
							if (LineItem.SelectSingleNode("SalesTaxCodeRef") == null) {
								expenseLineRet.SalesTaxCodeRef.ListID = null;
								expenseLineRet.SalesTaxCodeRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("SalesTaxCodeRef");
								expenseLineRet.SalesTaxCodeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								expenseLineRet.SalesTaxCodeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region SalesRepRef
							if (LineItem.SelectSingleNode("SalesRepRef") == null) {
								expenseLineRet.SalesRepRef.ListID = null;
								expenseLineRet.SalesRepRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("SalesRepRef");
								expenseLineRet.SalesRepRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								expenseLineRet.SalesRepRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region DataExtRet
							if (Item.SelectSingleNode("DataExtRet") == null) {
								expenseLineRet.DataExtRet.OwnerID = null;
								expenseLineRet.DataExtRet.DataExtName = null;
								expenseLineRet.DataExtRet.DataExtType = null;
								expenseLineRet.DataExtRet.DataExtValue = null;
							}
							else {
								ItemRef = Item.SelectSingleNode("DataExtRet");
								expenseLineRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
								expenseLineRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
								expenseLineRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
								expenseLineRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
							}
							#endregion

							checkRet.Check_ExpenseLineRet.Add(expenseLineRet);
						}
						#endregion LineItems

						list.Add(checkRet);
					}

					break;
				#endregion

				#region CreditCardCharge
				case ActionType.CreditCardChargeAdd:
				case ActionType.CreditCardChargeQuery:
					CreditCardChargeRet creditCardChargeRet;
					CreditCard_ExpenseLineRet creditCardCharge_ExpenseLineRet;
					ItemList = responseNode.SelectNodes("//CreditCardChargeRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						creditCardChargeRet = new CreditCardChargeRet {
							TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
							ExchangeRate = Item.SelectSingleNode("ExchangeRate") == null ? 0 : decimal.Parse(Item.SelectSingleNode("ExchangeRate ").InnerText),
							AmountInHomeCurrency = Item.SelectSingleNode("AmountInHomeCurrency") == null ? 0 : decimal.Parse(Item.SelectSingleNode("AmountInHomeCurrency").InnerText),
							Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
							IsTaxIncluded = Item.SelectSingleNode("IsTaxIncluded") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsTaxIncluded").InnerText),
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
						};

						#region AccountRef
						if (Item.SelectSingleNode("AccountRef") == null) {
							creditCardChargeRet.AccountRef.ListID = null;
							creditCardChargeRet.AccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("AccountRef");
							creditCardChargeRet.AccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							creditCardChargeRet.AccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region PayeeEntityRef
						if (Item.SelectSingleNode("PayeeEntityRef") == null) {
							creditCardChargeRet.PayeeEntityRef.ListID = null;
							creditCardChargeRet.PayeeEntityRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("PayeeEntityRef");
							creditCardChargeRet.PayeeEntityRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							creditCardChargeRet.PayeeEntityRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CurrencyRef
						if (Item.SelectSingleNode("CurrencyRef") == null) {
							creditCardChargeRet.CurrencyRef.ListID = null;
							creditCardChargeRet.CurrencyRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CurrencyRef");
							creditCardChargeRet.CurrencyRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							creditCardChargeRet.CurrencyRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region SalesTaxCodeRef
						if (Item.SelectSingleNode("SalesTaxCodeRef") == null) {
							creditCardChargeRet.CurrencyRef.ListID = null;
							creditCardChargeRet.CurrencyRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("SalesTaxCodeRef");
							creditCardChargeRet.SalesTaxCodeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							creditCardChargeRet.SalesTaxCodeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region Expense LineItems Return
						LineItemList = Item.SelectNodes("ExpenseLineRet");

						for (int j = 0; j < LineItemList.Count; j++) {
							LineItem = LineItemList.Item(j);
							creditCardCharge_ExpenseLineRet = new CreditCard_ExpenseLineRet {
								TxnLineID = LineItem.SelectSingleNode("TxnLineID") == null ? null : LineItem.SelectSingleNode("TxnLineID").InnerText,
								Amount = Item.SelectSingleNode("Amount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("Amount").InnerText),
								Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
								BillableStatus = Item.SelectSingleNode("BillableStatus") == null ? null : Item.SelectSingleNode("BillableStatus").InnerText,
							};

							#region AccountRef
							if (LineItem.SelectSingleNode("AccountRef") == null) {
								creditCardCharge_ExpenseLineRet.AccountRef.ListID = null;
								creditCardCharge_ExpenseLineRet.AccountRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("AccountRef");
								creditCardCharge_ExpenseLineRet.AccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								creditCardCharge_ExpenseLineRet.AccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region CustomerRef
							if (LineItem.SelectSingleNode("CustomerRef") == null) {
								creditCardCharge_ExpenseLineRet.CustomerRef.ListID = null;
								creditCardCharge_ExpenseLineRet.CustomerRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("CustomerRef");
								creditCardCharge_ExpenseLineRet.CustomerRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								creditCardCharge_ExpenseLineRet.CustomerRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region ClassRef
							if (LineItem.SelectSingleNode("ClassRef") == null) {
								creditCardCharge_ExpenseLineRet.ClassRef.ListID = null;
								creditCardCharge_ExpenseLineRet.ClassRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("ClassRef");
								creditCardCharge_ExpenseLineRet.ClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								creditCardCharge_ExpenseLineRet.ClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region SalesTaxCodeRef
							if (LineItem.SelectSingleNode("SalesTaxCodeRef") == null) {
								creditCardCharge_ExpenseLineRet.SalesTaxCodeRef.ListID = null;
								creditCardCharge_ExpenseLineRet.SalesTaxCodeRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("SalesTaxCodeRef");
								creditCardCharge_ExpenseLineRet.SalesTaxCodeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								creditCardCharge_ExpenseLineRet.SalesTaxCodeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region SalesRepRef
							if (LineItem.SelectSingleNode("SalesRepRef") == null) {
								creditCardCharge_ExpenseLineRet.SalesRepRef.ListID = null;
								creditCardCharge_ExpenseLineRet.SalesRepRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("SalesRepRef");
								creditCardCharge_ExpenseLineRet.SalesRepRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								creditCardCharge_ExpenseLineRet.SalesRepRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region DataExtRet
							if (Item.SelectSingleNode("DataExtRet") == null) {
								creditCardCharge_ExpenseLineRet.DataExtRet.OwnerID = null;
								creditCardCharge_ExpenseLineRet.DataExtRet.DataExtName = null;
								creditCardCharge_ExpenseLineRet.DataExtRet.DataExtType = null;
								creditCardCharge_ExpenseLineRet.DataExtRet.DataExtValue = null;
							}
							else {
								ItemRef = Item.SelectSingleNode("DataExtRet");
								creditCardCharge_ExpenseLineRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
								creditCardCharge_ExpenseLineRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
								creditCardCharge_ExpenseLineRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
								creditCardCharge_ExpenseLineRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
							}
							#endregion

							creditCardChargeRet.CreditCardCharge_ExpenseLineRet.Add(creditCardCharge_ExpenseLineRet);
						}
						#endregion LineItems

						#region DataExtRet
						if (Item.SelectSingleNode("DataExtRet") == null) {
							creditCardChargeRet.DataExtRet.OwnerID = null;
							creditCardChargeRet.DataExtRet.DataExtName = null;
							creditCardChargeRet.DataExtRet.DataExtType = null;
							creditCardChargeRet.DataExtRet.DataExtValue = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("DataExtRet");
							creditCardChargeRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
							creditCardChargeRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
							creditCardChargeRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
							creditCardChargeRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
						}
						#endregion

						list.Add(creditCardChargeRet);
					}

					break;
				#endregion

				#region CreditCardCredit
				case ActionType.CreditCardCreditAdd:
				case ActionType.CreditCardCreditQuery:
					CreditCardCreditRet creditCardCreditRet;
					CreditCard_ExpenseLineRet creditCardCredit_ExpenseLineRet;
					ItemList = responseNode.SelectNodes("//CreditCardChargeRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						creditCardCreditRet = new CreditCardCreditRet {
							TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
							ExchangeRate = Item.SelectSingleNode("ExchangeRate") == null ? 0 : decimal.Parse(Item.SelectSingleNode("ExchangeRate").InnerText),
							AmountInHomeCurrency = Item.SelectSingleNode("AmountInHomeCurrency") == null ? 0 : decimal.Parse(Item.SelectSingleNode("AmountInHomeCurrency").InnerText),
							Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
							IsTaxIncluded = Item.SelectSingleNode("IsTaxIncluded") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsTaxIncluded").InnerText),
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
						};

						#region AccountRef
						if (Item.SelectSingleNode("AccountRef") == null) {
							creditCardCreditRet.AccountRef.ListID = null;
							creditCardCreditRet.AccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("AccountRef");
							creditCardCreditRet.AccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							creditCardCreditRet.AccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region PayeeEntityRef
						if (Item.SelectSingleNode("PayeeEntityRef") == null) {
							creditCardCreditRet.PayeeEntityRef.ListID = null;
							creditCardCreditRet.PayeeEntityRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("PayeeEntityRef");
							creditCardCreditRet.PayeeEntityRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							creditCardCreditRet.PayeeEntityRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CurrencyRef
						if (Item.SelectSingleNode("CurrencyRef") == null) {
							creditCardCreditRet.CurrencyRef.ListID = null;
							creditCardCreditRet.CurrencyRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CurrencyRef");
							creditCardCreditRet.CurrencyRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							creditCardCreditRet.CurrencyRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region SalesTaxCodeRef
						if (Item.SelectSingleNode("SalesTaxCodeRef") == null) {
							creditCardCreditRet.CurrencyRef.ListID = null;
							creditCardCreditRet.CurrencyRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("SalesTaxCodeRef");
							creditCardCreditRet.SalesTaxCodeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							creditCardCreditRet.SalesTaxCodeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region Expense LineItems Return
						LineItemList = Item.SelectNodes("ExpenseLineRet");

						for (int j = 0; j < LineItemList.Count; j++) {
							LineItem = LineItemList.Item(j);
							creditCardCredit_ExpenseLineRet = new CreditCard_ExpenseLineRet {
								TxnLineID = LineItem.SelectSingleNode("TxnLineID") == null ? null : LineItem.SelectSingleNode("TxnLineID").InnerText,
								Amount = Item.SelectSingleNode("Amount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("Amount").InnerText),
								Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
								BillableStatus = Item.SelectSingleNode("BillableStatus") == null ? null : Item.SelectSingleNode("BillableStatus").InnerText,
							};

							#region AccountRef
							if (LineItem.SelectSingleNode("AccountRef") == null) {
								creditCardCredit_ExpenseLineRet.AccountRef.ListID = null;
								creditCardCredit_ExpenseLineRet.AccountRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("AccountRef");
								creditCardCredit_ExpenseLineRet.AccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								creditCardCredit_ExpenseLineRet.AccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region CustomerRef
							if (LineItem.SelectSingleNode("CustomerRef") == null) {
								creditCardCredit_ExpenseLineRet.CustomerRef.ListID = null;
								creditCardCredit_ExpenseLineRet.CustomerRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("CustomerRef");
								creditCardCredit_ExpenseLineRet.CustomerRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								creditCardCredit_ExpenseLineRet.CustomerRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region ClassRef
							if (LineItem.SelectSingleNode("ClassRef") == null) {
								creditCardCredit_ExpenseLineRet.ClassRef.ListID = null;
								creditCardCredit_ExpenseLineRet.ClassRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("ClassRef");
								creditCardCredit_ExpenseLineRet.ClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								creditCardCredit_ExpenseLineRet.ClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region SalesTaxCodeRef
							if (LineItem.SelectSingleNode("SalesTaxCodeRef") == null) {
								creditCardCredit_ExpenseLineRet.SalesTaxCodeRef.ListID = null;
								creditCardCredit_ExpenseLineRet.SalesTaxCodeRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("SalesTaxCodeRef");
								creditCardCredit_ExpenseLineRet.SalesTaxCodeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								creditCardCredit_ExpenseLineRet.SalesTaxCodeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region SalesRepRef
							if (LineItem.SelectSingleNode("SalesRepRef") == null) {
								creditCardCredit_ExpenseLineRet.SalesRepRef.ListID = null;
								creditCardCredit_ExpenseLineRet.SalesRepRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("SalesRepRef");
								creditCardCredit_ExpenseLineRet.SalesRepRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								creditCardCredit_ExpenseLineRet.SalesRepRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region DataExtRet
							if (Item.SelectSingleNode("DataExtRet") == null) {
								creditCardCredit_ExpenseLineRet.DataExtRet.OwnerID = null;
								creditCardCredit_ExpenseLineRet.DataExtRet.DataExtName = null;
								creditCardCredit_ExpenseLineRet.DataExtRet.DataExtType = null;
								creditCardCredit_ExpenseLineRet.DataExtRet.DataExtValue = null;
							}
							else {
								ItemRef = Item.SelectSingleNode("DataExtRet");
								creditCardCredit_ExpenseLineRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
								creditCardCredit_ExpenseLineRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
								creditCardCredit_ExpenseLineRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
								creditCardCredit_ExpenseLineRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
							}
							#endregion

							creditCardCreditRet.CreditCardCharge_ExpenseLineRet.Add(creditCardCredit_ExpenseLineRet);
						}
						#endregion LineItems

						#region DataExtRet
						if (Item.SelectSingleNode("DataExtRet") == null) {
							creditCardCreditRet.DataExtRet.OwnerID = null;
							creditCardCreditRet.DataExtRet.DataExtName = null;
							creditCardCreditRet.DataExtRet.DataExtType = null;
							creditCardCreditRet.DataExtRet.DataExtValue = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("DataExtRet");
							creditCardCreditRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
							creditCardCreditRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
							creditCardCreditRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
							creditCardCreditRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
						}
						#endregion

						list.Add(creditCardCreditRet);
					}

					break;
				#endregion

				#region CustomeryQuery
				case ActionType.CustomerAdd:
				case ActionType.CustomeryQuery:
					CustomerRet customerRet;
					Customer_ContactsRet customerContactsRet;
					ItemList = responseNode.SelectNodes("//CustomerRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						customerRet = new CustomerRet {
							ListID = Item.SelectSingleNode("ListID") == null ? null : Item.SelectSingleNode("ListID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							Name = Item.SelectSingleNode("Name") == null ? null : Item.SelectSingleNode("Name").InnerText,
							FullName = Item.SelectSingleNode("FullName") == null ? null : Item.SelectSingleNode("FullName").InnerText,
							IsActive = Item.SelectSingleNode("IsActive") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsActive").InnerText),
							Sublevel = Item.SelectSingleNode("Sublevel") == null ? (int?) null : int.Parse(Item.SelectSingleNode("Sublevel").InnerText),
							CompanyName = Item.SelectSingleNode("CompanyName") == null ? null : Item.SelectSingleNode("CompanyName").InnerText,
							Salutation = Item.SelectSingleNode("Salutation") == null ? null : Item.SelectSingleNode("Salutation").InnerText,
							FirstName = Item.SelectSingleNode("FirstName") == null ? null : Item.SelectSingleNode("FirstName").InnerText,
							MiddleName = Item.SelectSingleNode("MiddleName") == null ? null : Item.SelectSingleNode("MiddleName").InnerText,
							LastName = Item.SelectSingleNode("LastName") == null ? null : Item.SelectSingleNode("LastName").InnerText,
							Phone = Item.SelectSingleNode("Phone") == null ? null : Item.SelectSingleNode("Phone").InnerText,
							AltPhone = Item.SelectSingleNode("AltPhone") == null ? null : Item.SelectSingleNode("AltPhone").InnerText,
							Fax = Item.SelectSingleNode("Fax") == null ? null : Item.SelectSingleNode("Fax").InnerText,
							Email = Item.SelectSingleNode("Email") == null ? null : Item.SelectSingleNode("Email").InnerText,
							Cc = Item.SelectSingleNode("Cc") == null ? null : Item.SelectSingleNode("Cc").InnerText,
							Contact = Item.SelectSingleNode("Contact") == null ? null : Item.SelectSingleNode("Contact").InnerText,
							AltContact = Item.SelectSingleNode("AltContact") == null ? null : Item.SelectSingleNode("AltContact").InnerText,
							Balance = Item.SelectSingleNode("Balance") == null ? 0 : decimal.Parse(Item.SelectSingleNode("Balance").InnerText),
							TotalBalance = Item.SelectSingleNode("TotalBalance") == null ? 0 : decimal.Parse(Item.SelectSingleNode("TotalBalance").InnerText),
							SalesTaxCountry = Item.SelectSingleNode("SalesTaxCountry") == null ? null : Item.SelectSingleNode("SalesTaxCountry").InnerText,
							ResaleNumber = Item.SelectSingleNode("ResaleNumber") == null ? null : Item.SelectSingleNode("ResaleNumber").InnerText,
							AccountNumber = Item.SelectSingleNode("AccountNumber") == null ? null : Item.SelectSingleNode("AccountNumber").InnerText,
							CreditLimit = Item.SelectSingleNode("CreditLimit") == null ? 0 : decimal.Parse(Item.SelectSingleNode("CreditLimit").InnerText),
							JobStatus = Item.SelectSingleNode("JobStatus") == null ? null : Item.SelectSingleNode("JobStatus").InnerText,
							JobStartDate = DateTime.Parse(Item.SelectSingleNode("JobStartDate").InnerText),
							JobProjectedEndDate = DateTime.Parse(Item.SelectSingleNode("JobProjectedEndDate").InnerText),
							JobEndDate = DateTime.Parse(Item.SelectSingleNode("JobEndDate").InnerText),
							Notes = Item.SelectSingleNode("Notes") == null ? null : Item.SelectSingleNode("Notes").InnerText,
							PreferredDeliveryMethod = Item.SelectSingleNode("PreferredDeliveryMethod") == null ? null : Item.SelectSingleNode("PreferredDeliveryMethod").InnerText,
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
							TaxRegistrationNumber = Item.SelectSingleNode("TaxRegistrationNumber") == null ? null : Item.SelectSingleNode("TaxRegistrationNumber").InnerText,
						};

						#region ClassRef
						if (Item.SelectSingleNode("ClassRef") == null) {
							customerRet.ClassRef.ListID = null;
							customerRet.ClassRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("ClassRef");
							customerRet.ClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							customerRet.ClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region ParentRef
						if (Item.SelectSingleNode("ParentRef") == null) {
							customerRet.ParentRef.ListID = null;
							customerRet.ParentRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("ParentRef");
							customerRet.ParentRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							customerRet.ParentRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region BillAddress
						if (Item.SelectSingleNode("BillAddress") == null) {
							customerRet.BillAddress.Addr1 = null;
							customerRet.BillAddress.Addr2 = null;
							customerRet.BillAddress.Addr3 = null;
							customerRet.BillAddress.Addr4 = null;
							customerRet.BillAddress.Addr5 = null;
							customerRet.BillAddress.City = null;
							customerRet.BillAddress.State = null;
							customerRet.BillAddress.PostalCode = null;
							customerRet.BillAddress.Country = null;
							customerRet.BillAddress.Note = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("BillAddress");
							customerRet.BillAddress.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							customerRet.BillAddress.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							customerRet.BillAddress.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							customerRet.BillAddress.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							customerRet.BillAddress.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
							customerRet.BillAddress.City = ItemRef.SelectSingleNode("City") == null ? null : ItemRef.SelectSingleNode("City").InnerText;
							customerRet.BillAddress.State = ItemRef.SelectSingleNode("State") == null ? null : ItemRef.SelectSingleNode("State").InnerText;
							customerRet.BillAddress.PostalCode = ItemRef.SelectSingleNode("PostalCode") == null ? null : ItemRef.SelectSingleNode("PostalCode").InnerText;
							customerRet.BillAddress.Country = ItemRef.SelectSingleNode("Country") == null ? null : ItemRef.SelectSingleNode("Country").InnerText;
							customerRet.BillAddress.Note = ItemRef.SelectSingleNode("Note") == null ? null : ItemRef.SelectSingleNode("Note").InnerText;
						}
						#endregion

						#region BillAddressBlock
						if (Item.SelectSingleNode("BillAddressBlock") == null) {
							customerRet.BillAddressBlock.Addr1 = null;
							customerRet.BillAddressBlock.Addr2 = null;
							customerRet.BillAddressBlock.Addr3 = null;
							customerRet.BillAddressBlock.Addr4 = null;
							customerRet.BillAddressBlock.Addr5 = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("BillAddressBlock");
							customerRet.BillAddressBlock.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							customerRet.BillAddressBlock.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							customerRet.BillAddressBlock.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							customerRet.BillAddressBlock.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							customerRet.BillAddressBlock.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
						}
						#endregion

						#region ShipAddress
						if (Item.SelectSingleNode("ShipAddress") == null) {
							customerRet.ShipAddress.Addr1 = null;
							customerRet.ShipAddress.Addr2 = null;
							customerRet.ShipAddress.Addr3 = null;
							customerRet.ShipAddress.Addr4 = null;
							customerRet.ShipAddress.Addr5 = null;
							customerRet.ShipAddress.City = null;
							customerRet.ShipAddress.State = null;
							customerRet.ShipAddress.PostalCode = null;
							customerRet.ShipAddress.Country = null;
							customerRet.ShipAddress.Note = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("ShipAddress");
							customerRet.ShipAddress.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							customerRet.ShipAddress.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							customerRet.ShipAddress.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							customerRet.ShipAddress.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							customerRet.ShipAddress.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
							customerRet.ShipAddress.City = ItemRef.SelectSingleNode("City") == null ? null : ItemRef.SelectSingleNode("City").InnerText;
							customerRet.ShipAddress.State = ItemRef.SelectSingleNode("State") == null ? null : ItemRef.SelectSingleNode("State").InnerText;
							customerRet.ShipAddress.PostalCode = ItemRef.SelectSingleNode("PostalCode") == null ? null : ItemRef.SelectSingleNode("PostalCode").InnerText;
							customerRet.ShipAddress.Country = ItemRef.SelectSingleNode("Country") == null ? null : ItemRef.SelectSingleNode("Country").InnerText;
							customerRet.ShipAddress.Note = ItemRef.SelectSingleNode("Note") == null ? null : ItemRef.SelectSingleNode("Note").InnerText;
						}
						#endregion

						#region ShipAddressBlock
						if (Item.SelectSingleNode("ShipAddressBlock") == null) {
							customerRet.ShipAddressBlock.Addr1 = null;
							customerRet.ShipAddressBlock.Addr2 = null;
							customerRet.ShipAddressBlock.Addr3 = null;
							customerRet.ShipAddressBlock.Addr4 = null;
							customerRet.ShipAddressBlock.Addr5 = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("ShipAddressBlock");
							customerRet.ShipAddressBlock.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							customerRet.ShipAddressBlock.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							customerRet.ShipAddressBlock.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							customerRet.ShipAddressBlock.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							customerRet.ShipAddressBlock.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
						}
						#endregion

						#region ShipToAddress
						if (Item.SelectSingleNode("ShipToAddress") == null) {
							customerRet.ShipToAddress.Addr1 = null;
							customerRet.ShipToAddress.Addr2 = null;
							customerRet.ShipToAddress.Addr3 = null;
							customerRet.ShipToAddress.Addr4 = null;
							customerRet.ShipToAddress.Addr5 = null;
							customerRet.ShipToAddress.City = null;
							customerRet.ShipToAddress.State = null;
							customerRet.ShipToAddress.PostalCode = null;
							customerRet.ShipToAddress.Country = null;
							customerRet.ShipToAddress.Note = null;
							customerRet.ShipToAddress.DefaultShipTo = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("ShipToAddress");
							customerRet.ShipToAddress.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							customerRet.ShipToAddress.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							customerRet.ShipToAddress.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							customerRet.ShipToAddress.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							customerRet.ShipToAddress.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
							customerRet.ShipToAddress.City = ItemRef.SelectSingleNode("City") == null ? null : ItemRef.SelectSingleNode("City").InnerText;
							customerRet.ShipToAddress.State = ItemRef.SelectSingleNode("State") == null ? null : ItemRef.SelectSingleNode("State").InnerText;
							customerRet.ShipToAddress.PostalCode = ItemRef.SelectSingleNode("PostalCode") == null ? null : ItemRef.SelectSingleNode("PostalCode").InnerText;
							customerRet.ShipToAddress.Country = ItemRef.SelectSingleNode("Country") == null ? null : ItemRef.SelectSingleNode("Country").InnerText;
							customerRet.ShipToAddress.Note = ItemRef.SelectSingleNode("Note") == null ? null : ItemRef.SelectSingleNode("Note").InnerText;
							customerRet.ShipToAddress.DefaultShipTo = ItemRef.SelectSingleNode("DefaultShipTo") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("DefaultShipTo").InnerText);
						}
						#endregion

						#region AdditionalContactRef
						if (Item.SelectSingleNode("AdditionalContactRef") == null) {
							customerRet.AdditionalContactRef.ContactName = null;
							customerRet.AdditionalContactRef.ContactValue = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("AdditionalContactRef");
							customerRet.AdditionalContactRef.ContactName = ItemRef.SelectSingleNode("ContactName") == null ? null : ItemRef.SelectSingleNode("ContactName").InnerText;
							customerRet.AdditionalContactRef.ContactValue = ItemRef.SelectSingleNode("ContactValue") == null ? null : ItemRef.SelectSingleNode("ContactValue").InnerText;
						}
						#endregion

						#region ContactsRet LineItems
						LineItemList = Item.SelectNodes("ContactsRet");

						for (int j = 0; j < LineItemList.Count; j++) {
							LineItem = LineItemList.Item(j);
							customerContactsRet = new Customer_ContactsRet {
								ListID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
								TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
								TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
								EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
								Contact = Item.SelectSingleNode("Contact") == null ? null : Item.SelectSingleNode("Contact").InnerText,
								Salutation = Item.SelectSingleNode("Salutation") == null ? null : Item.SelectSingleNode("Salutation").InnerText,
								FirstName = Item.SelectSingleNode("FirstName") == null ? null : Item.SelectSingleNode("FirstName").InnerText,
								MiddleName = Item.SelectSingleNode("MiddleName") == null ? null : Item.SelectSingleNode("MiddleName").InnerText,
								LastName = Item.SelectSingleNode("LastName") == null ? null : Item.SelectSingleNode("LastName").InnerText,
								JobTitle = Item.SelectSingleNode("JobTitle") == null ? null : Item.SelectSingleNode("JobTitle").InnerText,
							};

							#region AdditionalContactRef
							if (LineItem.SelectSingleNode("AdditionalContactRef") == null) {
								customerContactsRet.AdditionalContactRef.ContactName = null;
								customerContactsRet.AdditionalContactRef.ContactValue = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("AdditionalContactRef");
								customerContactsRet.AdditionalContactRef.ContactName = ItemRef.SelectSingleNode("ContactName") == null ? null : ItemRef.SelectSingleNode("ContactName").InnerText;
								customerContactsRet.AdditionalContactRef.ContactValue = ItemRef.SelectSingleNode("ContactValue") == null ? null : ItemRef.SelectSingleNode("ContactValue").InnerText;
							}
							#endregion


							customerRet.ContactsRet.Add(customerContactsRet);
						}
						#endregion LineItems

						#region CustomerTypeRef
						if (Item.SelectSingleNode("CustomerTypeRef") == null) {
							customerRet.CustomerTypeRef.ListID = null;
							customerRet.CustomerTypeRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CustomerTypeRef");
							customerRet.CustomerTypeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							customerRet.CustomerTypeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region TermsRef
						if (Item.SelectSingleNode("TermsRef") == null) {
							customerRet.TermsRef.ListID = null;
							customerRet.TermsRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("TermsRef");
							customerRet.TermsRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							customerRet.TermsRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region SalesRepRef
						if (Item.SelectSingleNode("SalesRepRef") == null) {
							customerRet.SalesRepRef.ListID = null;
							customerRet.SalesRepRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("SalesRepRef");
							customerRet.SalesRepRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							customerRet.SalesRepRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region SalesTaxCodeRef
						if (Item.SelectSingleNode("SalesTaxCodeRef") == null) {
							customerRet.SalesTaxCodeRef.ListID = null;
							customerRet.SalesTaxCodeRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("SalesTaxCodeRef");
							customerRet.SalesTaxCodeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							customerRet.SalesTaxCodeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region ItemSalesTaxRef
						if (Item.SelectSingleNode("ItemSalesTaxRef") == null) {
							customerRet.ItemSalesTaxRef.ListID = null;
							customerRet.ItemSalesTaxRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("ItemSalesTaxRef");
							customerRet.ItemSalesTaxRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							customerRet.ItemSalesTaxRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region PreferredPaymentMethodRef
						if (Item.SelectSingleNode("PreferredPaymentMethodRef") == null) {
							customerRet.PreferredPaymentMethodRef.ListID = null;
							customerRet.PreferredPaymentMethodRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("PreferredPaymentMethodRef");
							customerRet.PreferredPaymentMethodRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							customerRet.PreferredPaymentMethodRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CreditCardInfo
						if (Item.SelectSingleNode("CreditCardInfo") == null) {
							customerRet.CreditCardInfo.CreditCardNumber = null;
							customerRet.CreditCardInfo.ExpirationMonth = 0;
							customerRet.CreditCardInfo.ExpirationYear = 0;
							customerRet.CreditCardInfo.NameOnCard = null;
							customerRet.CreditCardInfo.CreditCardAddress = null;
							customerRet.CreditCardInfo.CreditCardPostalCode = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("CreditCardInfo");
							customerRet.CreditCardInfo.CreditCardNumber = ItemRef.SelectSingleNode("CreditCardNumber") == null ? null : ItemRef.SelectSingleNode("CreditCardNumber").InnerText;
							customerRet.CreditCardInfo.ExpirationMonth = ItemRef.SelectSingleNode("ExpirationMonth") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("ExpirationMonth").InnerText);
							customerRet.CreditCardInfo.ExpirationYear = ItemRef.SelectSingleNode("ExpirationYear") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("ExpirationYear").InnerText);
							customerRet.CreditCardInfo.NameOnCard = ItemRef.SelectSingleNode("NameOnCard") == null ? null : ItemRef.SelectSingleNode("NameOnCard").InnerText;
							customerRet.CreditCardInfo.CreditCardAddress = ItemRef.SelectSingleNode("CreditCardAddress") == null ? null : ItemRef.SelectSingleNode("CreditCardAddress").InnerText;
							customerRet.CreditCardInfo.CreditCardPostalCode = ItemRef.SelectSingleNode("CreditCardPostalCode") == null ? null : ItemRef.SelectSingleNode("CreditCardPostalCode").InnerText;
						}
						#endregion

						#region JobTypeRef
						if (Item.SelectSingleNode("JobTypeRef") == null) {
							customerRet.JobTypeRef.ListID = null;
							customerRet.JobTypeRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("JobTypeRef");
							customerRet.JobTypeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							customerRet.JobTypeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region AdditionalNotesRet
						if (Item.SelectSingleNode("AdditionalNotesRet") == null) {
							customerRet.AdditionalNotesRet.NodeID = 0;
							customerRet.AdditionalNotesRet.Date = null;
							customerRet.AdditionalNotesRet.Note = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("AdditionalNotesRet");
							customerRet.AdditionalNotesRet.NodeID = ItemRef.SelectSingleNode("NodeID") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("NodeID").InnerText);
							customerRet.AdditionalNotesRet.Date = ItemRef.SelectSingleNode("Date") == null ? null : ItemRef.SelectSingleNode("Date").InnerText;
							customerRet.AdditionalNotesRet.Note = ItemRef.SelectSingleNode("Note") == null ? null : ItemRef.SelectSingleNode("Note").InnerText;
						}
						#endregion

						#region PriceLevelRef
						if (Item.SelectSingleNode("PriceLevelRef") == null) {
							customerRet.PriceLevelRef.ListID = null;
							customerRet.PriceLevelRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("PriceLevelRef");
							customerRet.PriceLevelRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							customerRet.PriceLevelRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CurrencyRef
						if (Item.SelectSingleNode("CurrencyRef") == null) {
							customerRet.CurrencyRef.ListID = null;
							customerRet.CurrencyRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CurrencyRef");
							customerRet.CurrencyRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							customerRet.CurrencyRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region DataExtRet
						if (Item.SelectSingleNode("DataExtRet") == null) {
							customerRet.DataExtRet.OwnerID = null;
							customerRet.DataExtRet.DataExtName = null;
							customerRet.DataExtRet.DataExtType = null;
							customerRet.DataExtRet.DataExtValue = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("DataExtRet");
							customerRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
							customerRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
							customerRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
							customerRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
						}
						#endregion

						list.Add(customerRet);
					}
					break;
				#endregion

				#region DepositQuery
				case ActionType.DepositQuery:
					DepositRet depositRet;
					Deposit_CashBackInfoRet depositCashBackRet;
					Deposit_DepositLineRet depositLineRet;

					ItemList = responseNode.SelectNodes("//DepositRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						depositRet = new DepositRet {
							TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
							Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
							DepositTotal = Item.SelectSingleNode("DepositTotal") == null ? 0 : decimal.Parse(Item.SelectSingleNode("DepositTotal").InnerText),
							ExchangeRate = Item.SelectSingleNode("ExchangeRate") == null ? 0 : decimal.Parse(Item.SelectSingleNode("ExchangeRate").InnerText),
							DepositTotalInHomeCurrency = Item.SelectSingleNode("DepositTotalInHomeCurrency") == null ? 0 : decimal.Parse(Item.SelectSingleNode("DepositTotalInHomeCurrency").InnerText),
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
						};

						#region DepositToAccountRef
						if (Item.SelectSingleNode("DepositToAccountRef") == null) {
							depositRet.DepositToAccountRef.ListID = null;
							depositRet.DepositToAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("DepositToAccountRef");
							depositRet.DepositToAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							depositRet.DepositToAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CurrencyRef
						if (Item.SelectSingleNode("CurrencyRef") == null) {
							depositRet.CurrencyRef.ListID = null;
							depositRet.CurrencyRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CurrencyRef");
							depositRet.CurrencyRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							depositRet.CurrencyRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region ListItems Return

						#region CashBackInfoRet

						LineItemList = Item.SelectNodes("CashBackInfoRet");

						for (int j = 0; j < LineItemList.Count; j++) {
							LineItem = LineItemList.Item(j);
							depositCashBackRet = new Deposit_CashBackInfoRet {
								TxnLineID = LineItem.SelectSingleNode("TxnLineID") == null ? null : LineItem.SelectSingleNode("TxnLineID").InnerText,
								Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
								Amount = Item.SelectSingleNode("Amount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("Amount").InnerText),

							};

							#region AccountRef
							if (LineItem.SelectSingleNode("AccountRef") == null) {
								depositCashBackRet.AccountRef.ListID = null;
								depositCashBackRet.AccountRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("AccountRef");
								depositCashBackRet.AccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								depositCashBackRet.AccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion



							depositRet.CashBackInfoRets.Add(depositCashBackRet);
						}
						#endregion

						#region DepositLineRets

						LineItemList = Item.SelectNodes("DepositLineRets");
						for (int k = 0; k < LineItemList.Count; k++) {
							LineItem = LineItemList.Item(k);

							depositLineRet = new Deposit_DepositLineRet {
								TxnType = Item.SelectSingleNode("TxnType") == null ? null : Item.SelectSingleNode("TxnType").InnerText,
								TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
								TxnLineID = Item.SelectSingleNode("TxnLineID") == null ? null : Item.SelectSingleNode("TxnLineID").InnerText,
								PaymentTxnLineID = Item.SelectSingleNode("PaymentTxnLineID") == null ? null : Item.SelectSingleNode("PaymentTxnLineID").InnerText,
								Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
								CheckNumber = Item.SelectSingleNode("CheckNumber") == null ? null : Item.SelectSingleNode("CheckNumber").InnerText

							};

							#region EntityRef
							if (LineItem.SelectSingleNode("EntityRef") == null) {
								depositLineRet.EntityRef.ListID = null;
								depositLineRet.EntityRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("EntityRef");
								depositLineRet.EntityRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								depositLineRet.EntityRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region AccountRef
							if (LineItem.SelectSingleNode("AccountRef") == null) {
								depositLineRet.AccountRef.ListID = null;
								depositLineRet.AccountRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("AccountRef");
								depositLineRet.AccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								depositLineRet.AccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region PaymentMethodRef
							if (LineItem.SelectSingleNode("PaymentMethodRef") == null) {
								depositLineRet.PaymentMethodRef.ListID = null;
								depositLineRet.PaymentMethodRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("PaymentMethodRef");
								depositLineRet.PaymentMethodRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								depositLineRet.PaymentMethodRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region ClassRef
							if (LineItem.SelectSingleNode("ClassRef") == null) {
								depositLineRet.ClassRef.ListID = null;
								depositLineRet.ClassRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("ClassRef");
								depositLineRet.ClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								depositLineRet.ClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							depositRet.DepositLineRets.Add(depositLineRet);
						}
						#endregion


						#endregion ListItems

						#region DataExtRet
						if (Item.SelectSingleNode("DataExtRet") == null) {
							depositRet.DataExtRet.OwnerID = null;
							depositRet.DataExtRet.DataExtName = null;
							depositRet.DataExtRet.DataExtType = null;
							depositRet.DataExtRet.DataExtValue = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("DataExtRet");
							depositRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
							depositRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
							depositRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
							depositRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
						}
						#endregion

						list.Add(depositRet);
					}

					break;
				#endregion

				#region InventoryAdjustment
				case ActionType.InventoryAdjustmentAdd:
				case ActionType.InventoryAdjustmentQuery:
					InventoryAdjustmentRet ent;
					InventoryAdjustmentLineRet entLine;
					ItemList = responseNode.SelectNodes("//InventoryAdjustmentRet");

					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						ent = new InventoryAdjustmentRet {
							TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
							TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
							RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
							Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
						};

						#region AccountRef
						if (Item.SelectSingleNode("AccountRef") == null) {
							ent.AccountRef.ListID = null;
							ent.AccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("AccountRef");
							ent.AccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							ent.AccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion AccountRef

						#region InventorySiteRef
						if (Item.SelectSingleNode("InventorySiteRef") == null) {
							ent.InventorySiteRef.ListID = null;
							ent.InventorySiteRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("InventorySiteRef");
							ent.InventorySiteRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							ent.InventorySiteRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion InventorySiteRef

						#region CustomerRef
						if (Item.SelectSingleNode("CustomerRef") == null) {
							ent.CustomerRef.ListID = null;
							ent.CustomerRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CustomerRef");
							ent.CustomerRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							ent.CustomerRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion CustomerRef

						#region ClassRef
						if (Item.SelectSingleNode("ClassRef") == null) {
							ent.ClassRef.ListID = null;
							ent.ClassRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("ClassRef");
							ent.ClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							ent.ClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion ClassRef

						#region LineItems
						LineItemList = Item.SelectNodes("InventoryAdjustmentLineRet");

						for (int j = 0; j < LineItemList.Count; j++) {
							LineItem = LineItemList.Item(j);
							entLine = new InventoryAdjustmentLineRet {
								TxnLineID = LineItem.SelectSingleNode("TxnLineID") == null ? null : LineItem.SelectSingleNode("TxnLineID").InnerText,
								SerialNumber = LineItem.SelectSingleNode("SerialNumber") == null ? null : LineItem.SelectSingleNode("SerialNumber").InnerText,
								SerialNumberAddedOrRemoved = LineItem.SelectSingleNode("SerialNumberAddedOrRemoved") == null ? null : LineItem.SelectSingleNode("SerialNumberAddedOrRemoved").InnerText,
								LotNumber = LineItem.SelectSingleNode("LotNumber") == null ? null : LineItem.SelectSingleNode("LotNumber").InnerText,
								QuantityDifference = decimal.Parse(LineItem.SelectSingleNode("QuantityDifference").InnerText),
								ValueDifference = decimal.Parse(LineItem.SelectSingleNode("ValueDifference").InnerText),
							};

							#region ItemRef
							if (LineItem.SelectSingleNode("ItemRef") == null) {
								entLine.ItemRef.ListID = null;
								entLine.ItemRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("ItemRef");
								entLine.ItemRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								entLine.ItemRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion ItemRef

							#region InventorySiteLocationRef
							if (LineItem.SelectSingleNode("InventorySiteLocationRef") == null) {
								entLine.InventorySiteLocationRef.ListID = null;
								entLine.InventorySiteLocationRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("InventorySiteLocationRef");
								entLine.InventorySiteLocationRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								entLine.InventorySiteLocationRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion InventorySiteLocationRef

							ent.LineItems.Add(entLine);
						}
						#endregion LineItems

						list.Add(ent);
					}

					break;
				#endregion InventoryAdjustment

				#region ItemNonInventory
				case ActionType.ItemNonInventoryQuery:
					ItemNonInventoryRet itemNonInventoryRet;
					ItemList = responseNode.SelectNodes("//ItemInventoryRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						itemNonInventoryRet = new ItemNonInventoryRet {
							ListID = Item.SelectSingleNode("ListID") == null ? null : Item.SelectSingleNode("ListID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							Name = Item.SelectSingleNode("Name") == null ? null : Item.SelectSingleNode("Name").InnerText,
							FullName = Item.SelectSingleNode("FullName") == null ? null : Item.SelectSingleNode("FullName").InnerText,
							BarCodeValue = Item.SelectSingleNode("BarCodeValue") == null ? null : Item.SelectSingleNode("BarCodeValue").InnerText,
							IsActive = Item.SelectSingleNode("IsActive") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsActive").InnerText),
							Sublevel = Item.SelectSingleNode("Sublevel") == null ? (int?) null : int.Parse(Item.SelectSingleNode("Sublevel").InnerText),
							ManufacturerPartNumber = Item.SelectSingleNode("ManufacturerPartNumber") == null ? null : Item.SelectSingleNode("ManufacturerPartNumber").InnerText,
							IsTaxIncluded = Item.SelectSingleNode("IsTaxIncluded") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsTaxIncluded").InnerText),
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
						};

						#region ClassRef
						if (Item.SelectSingleNode("ClassRef") == null) {
							itemNonInventoryRet.ClassRef.ListID = null;
							itemNonInventoryRet.ClassRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("ClassRef");
							itemNonInventoryRet.ClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							itemNonInventoryRet.ClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion AccountRef

						#region ParentRef
						if (Item.SelectSingleNode("ParentRef") == null) {
							itemNonInventoryRet.ParentRef.ListID = null;
							itemNonInventoryRet.ParentRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("ParentRef");
							itemNonInventoryRet.ParentRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							itemNonInventoryRet.ParentRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region UnitOfMeasureSetRef
						if (Item.SelectSingleNode("UnitOfMeasureSetRef") == null) {
							itemNonInventoryRet.UnitOfMeasureSetRef.ListID = null;
							itemNonInventoryRet.UnitOfMeasureSetRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("UnitOfMeasureSetRef");
							itemNonInventoryRet.UnitOfMeasureSetRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							itemNonInventoryRet.UnitOfMeasureSetRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region SalesTaxCodeRef
						if (Item.SelectSingleNode("SalesTaxCodeRef") == null) {
							itemNonInventoryRet.SalesTaxCodeRef.ListID = null;
							itemNonInventoryRet.SalesTaxCodeRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("SalesTaxCodeRef");
							itemNonInventoryRet.SalesTaxCodeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							itemNonInventoryRet.SalesTaxCodeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region SalesOrPurchase
						if (Item.SelectSingleNode("SalesOrPurchase") == null) {
							itemNonInventoryRet.SalesOrPurchase.Desc = null;
							itemNonInventoryRet.SalesOrPurchase.Price = null;
							itemNonInventoryRet.SalesOrPurchase.PricePercent = null;
							itemNonInventoryRet.SalesOrPurchase.AccountRef.ListID = null;
							itemNonInventoryRet.SalesOrPurchase.AccountRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("SalesOrPurchase");
							itemNonInventoryRet.SalesOrPurchase.Desc = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							itemNonInventoryRet.SalesOrPurchase.Price = ItemRef.SelectSingleNode("Price") == null ? 0 : decimal.Parse(ItemRef.SelectSingleNode("Price").InnerText);
							itemNonInventoryRet.SalesOrPurchase.PricePercent = ItemRef.SelectSingleNode("PricePercent") == null ? 0 : decimal.Parse(ItemRef.SelectSingleNode("PricePercent").InnerText);
							itemNonInventoryRet.SalesOrPurchase.AccountRef.ListID = ItemRef.SelectSingleNode("AccountRef/ListID") == null ? null : ItemRef.SelectSingleNode("AccountRef/ListID").InnerText;
							itemNonInventoryRet.SalesOrPurchase.AccountRef.FullName = ItemRef.SelectSingleNode("AccountRef/FullName") == null ? null : ItemRef.SelectSingleNode("AccountRef/FullName").InnerText;
						}
						#endregion

						#region SalesAndPurchase
						if (Item.SelectSingleNode("SalesAndPurchase") == null) {
							itemNonInventoryRet.SalesAndPurchase.SalesDesc = null;
							itemNonInventoryRet.SalesAndPurchase.SalesPrice = null;
							itemNonInventoryRet.SalesAndPurchase.IncomeAccountRef.ListID = null;
							itemNonInventoryRet.SalesAndPurchase.IncomeAccountRef.FullName = null;
							itemNonInventoryRet.SalesAndPurchase.PurchaseDesc = null;
							itemNonInventoryRet.SalesAndPurchase.PurchaseCost = null;
							itemNonInventoryRet.SalesAndPurchase.PurchaseTaxCodeRef.ListID = null;
							itemNonInventoryRet.SalesAndPurchase.PurchaseTaxCodeRef.FullName = null;
							itemNonInventoryRet.SalesAndPurchase.ExpenseAccountRef.ListID = null;
							itemNonInventoryRet.SalesAndPurchase.ExpenseAccountRef.FullName = null;
							itemNonInventoryRet.SalesAndPurchase.PrefVendorRef.ListID = null;
							itemNonInventoryRet.SalesAndPurchase.PrefVendorRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("SalesAndPurchase");
							itemNonInventoryRet.SalesAndPurchase.SalesDesc = ItemRef.SelectSingleNode("SalesDesc") == null ? null : ItemRef.SelectSingleNode("SalesDesc").InnerText;
							itemNonInventoryRet.SalesAndPurchase.SalesPrice = ItemRef.SelectSingleNode("SalesPrice") == null ? 0 : decimal.Parse(ItemRef.SelectSingleNode("SalesPrice").InnerText);
							itemNonInventoryRet.SalesAndPurchase.IncomeAccountRef.ListID = ItemRef.SelectSingleNode("IncomeAccountRef/ListID") == null ? null : ItemRef.SelectSingleNode("IncomeAccountRef/ListID").InnerText;
							itemNonInventoryRet.SalesAndPurchase.IncomeAccountRef.FullName = ItemRef.SelectSingleNode("IncomeAccountRef/FullName") == null ? null : ItemRef.SelectSingleNode("IncomeAccountRef/FullName").InnerText;
							itemNonInventoryRet.SalesAndPurchase.PurchaseDesc = ItemRef.SelectSingleNode("PurchaseDesc") == null ? null : ItemRef.SelectSingleNode("PurchaseDesc").InnerText;
							itemNonInventoryRet.SalesAndPurchase.PurchaseCost = ItemRef.SelectSingleNode("PurchaseCost") == null ? 0 : decimal.Parse(ItemRef.SelectSingleNode("PurchaseCost").InnerText);
							itemNonInventoryRet.SalesAndPurchase.PurchaseTaxCodeRef.ListID = ItemRef.SelectSingleNode("PurchaseTaxCodeRef/ListID") == null ? null : ItemRef.SelectSingleNode("PurchaseTaxCodeRef/ListID").InnerText;
							itemNonInventoryRet.SalesAndPurchase.PurchaseTaxCodeRef.FullName = ItemRef.SelectSingleNode("PurchaseTaxCodeRef/FullName") == null ? null : ItemRef.SelectSingleNode("PurchaseTaxCodeRef/FullName").InnerText;
							itemNonInventoryRet.SalesAndPurchase.ExpenseAccountRef.ListID = ItemRef.SelectSingleNode("ExpenseAccountRef/ListID") == null ? null : ItemRef.SelectSingleNode("ExpenseAccountRef/ListID").InnerText;
							itemNonInventoryRet.SalesAndPurchase.ExpenseAccountRef.FullName = ItemRef.SelectSingleNode("ExpenseAccountRef/FullName") == null ? null : ItemRef.SelectSingleNode("ExpenseAccountRef/FullName").InnerText;
							itemNonInventoryRet.SalesAndPurchase.PrefVendorRef.ListID = ItemRef.SelectSingleNode("PrefVendorRef/ListID") == null ? null : ItemRef.SelectSingleNode("PrefVendorRef/ListID").InnerText;
							itemNonInventoryRet.SalesAndPurchase.PrefVendorRef.FullName = ItemRef.SelectSingleNode("PrefVendorRef/FullName") == null ? null : ItemRef.SelectSingleNode("PrefVendorRef/FullName").InnerText;
						}
						#endregion

						#region DataExtRet
						if (Item.SelectSingleNode("DataExtRet") == null) {
							itemNonInventoryRet.DataExtRet.OwnerID = null;
							itemNonInventoryRet.DataExtRet.DataExtName = null;
							itemNonInventoryRet.DataExtRet.DataExtType = null;
							itemNonInventoryRet.DataExtRet.DataExtValue = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("DataExtRet");
							itemNonInventoryRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
							itemNonInventoryRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
							itemNonInventoryRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
							itemNonInventoryRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
						}
						#endregion
					}
					break;
				#endregion

				#region ReceivePaymentQuery
				case ActionType.ReceivePaymentQuery:
					ReceivePaymentRet receivePaymentRet;
					CreditCardTxnInfo receivePayment_CreditCardTxnInfo;
					AppliedToTxnRet receivePayment_AppliedToTxnRet;
					ItemList = responseNode.SelectNodes("//DepositRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						receivePaymentRet = new ReceivePaymentRet {
							TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
							TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
							RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
							TotalAmount = Item.SelectSingleNode("TotalAmount") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("TotalAmount").InnerText),
							ExchangeRate = Item.SelectSingleNode("ExchangeRate") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("ExchangeRate").InnerText),
							TotalAmountInHomeCurrency = Item.SelectSingleNode("TotalAmountInHomeCurrency") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("TotalAmountInHomeCurrency").InnerText),
							Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
							UnusedPayment = Item.SelectSingleNode("UnusedPayment") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("UnusedPayment").InnerText),
							UnusedCredits = Item.SelectSingleNode("UnusedCredits") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("UnusedCredits").InnerText),
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
						};

						#region CustomerRef
						if (Item.SelectSingleNode("CustomerRef") == null) {
							receivePaymentRet.CustomerRef.ListID = null;
							receivePaymentRet.CustomerRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CustomerRef");
							receivePaymentRet.CustomerRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							receivePaymentRet.CustomerRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region ARAccountRef
						if (Item.SelectSingleNode("ARAccountRef") == null) {
							receivePaymentRet.ARAccountRef.ListID = null;
							receivePaymentRet.ARAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("ARAccountRef");
							receivePaymentRet.ARAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							receivePaymentRet.ARAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CurrencyRef
						if (Item.SelectSingleNode("CurrencyRef") == null) {
							receivePaymentRet.CurrencyRef.ListID = null;
							receivePaymentRet.CurrencyRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CurrencyRef");
							receivePaymentRet.CurrencyRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							receivePaymentRet.CurrencyRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region PaymentMethodRef
						if (Item.SelectSingleNode("PaymentMethodRef") == null) {
							receivePaymentRet.PaymentMethodRef.ListID = null;
							receivePaymentRet.PaymentMethodRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("PaymentMethodRef");
							receivePaymentRet.PaymentMethodRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							receivePaymentRet.PaymentMethodRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region DepositToAccountRef
						if (Item.SelectSingleNode("DepositToAccountRef") == null) {
							receivePaymentRet.DepositToAccountRef.ListID = null;
							receivePaymentRet.DepositToAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("DepositToAccountRef");
							receivePaymentRet.DepositToAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							receivePaymentRet.DepositToAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CreditCardTxnInfo List

						LineItemList = Item.SelectNodes("CreditCardTxnInfo");
						for (int j = 0; j < LineItemList.Count; j++) {
							LineItem = LineItemList.Item(j);
							receivePayment_CreditCardTxnInfo = new CreditCardTxnInfo();

							#region CreditCardTxnInputInfo
							if (LineItem.SelectSingleNode("CreditCardTxnInputInfo") == null) {
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardNumber = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationMonth = 0;
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationYear = 0;
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.NameOnCard = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType = null;
							}
							else {
								ItemRef = LineItem.SelectSingleNode("CreditCardTxnInputInfo");
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardNumber = ItemRef.SelectSingleNode("CreditCardNumber") == null ? null : ItemRef.SelectSingleNode("CreditCardNumber").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationMonth = ItemRef.SelectSingleNode("ExpirationMonth") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("ExpirationMonth").InnerText);
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationYear = ItemRef.SelectSingleNode("ExpirationYear") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("ExpirationYear").InnerText);
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.NameOnCard = ItemRef.SelectSingleNode("NameOnCard") == null ? null : ItemRef.SelectSingleNode("NameOnCard").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress = ItemRef.SelectSingleNode("CreditCardAddress") == null ? null : ItemRef.SelectSingleNode("CreditCardAddress").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode = ItemRef.SelectSingleNode("CreditCardPostalCode") == null ? null : ItemRef.SelectSingleNode("CreditCardPostalCode").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode = ItemRef.SelectSingleNode("CommercialCardCode") == null ? null : ItemRef.SelectSingleNode("CommercialCardCode").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode = ItemRef.SelectSingleNode("TransactionMode") == null ? null : ItemRef.SelectSingleNode("TransactionMode").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType = ItemRef.SelectSingleNode("CreditCardTxnType") == null ? null : ItemRef.SelectSingleNode("CreditCardTxnType").InnerText;
							}
							#endregion

							#region CreditCardTxnResultInfo
							if (LineItem.SelectSingleNode("CreditCardTxnResultInfo") == null) {
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.ResultCode = 0;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.ResultMessage = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.CreditCardTransID = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.MerchantAccountNumber = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchId = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode = 0;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentStatus = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationTime = null;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp = 0;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID = null;

							}
							else {
								ItemRef = LineItem.SelectSingleNode("CreditCardTxnResultInfo");
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.ResultCode = ItemRef.SelectSingleNode("ResultCode") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("ResultCode").InnerText);
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.ResultMessage = ItemRef.SelectSingleNode("ResultMessage") == null ? null : ItemRef.SelectSingleNode("ResultMessage").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.CreditCardTransID = ItemRef.SelectSingleNode("CreditCardTransID") == null ? null : ItemRef.SelectSingleNode("CreditCardTransID").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.MerchantAccountNumber = ItemRef.SelectSingleNode("MerchantAccountNumber") == null ? null : ItemRef.SelectSingleNode("MerchantAccountNumber").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode = ItemRef.SelectSingleNode("AuthorizationCode") == null ? null : ItemRef.SelectSingleNode("AuthorizationCode").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet = ItemRef.SelectSingleNode("AVSStreet") == null ? null : ItemRef.SelectSingleNode("AVSStreet").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip = ItemRef.SelectSingleNode("AVSZip") == null ? null : ItemRef.SelectSingleNode("AVSZip").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch = ItemRef.SelectSingleNode("CardSecurityCodeMatch") == null ? null : ItemRef.SelectSingleNode("CardSecurityCodeMatch").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchId = ItemRef.SelectSingleNode("ReconBatchId") == null ? null : ItemRef.SelectSingleNode("ReconBatchId").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode = ItemRef.SelectSingleNode("PaymentGroupingCode") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("PaymentGroupingCode").InnerText);
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentStatus = ItemRef.SelectSingleNode("PaymentStatus") == null ? null : ItemRef.SelectSingleNode("PaymentStatus").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationTime = ItemRef.SelectSingleNode("TxnAuthorizationTime") == null ? null : ItemRef.SelectSingleNode("TxnAuthorizationTime").InnerText;
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp = ItemRef.SelectSingleNode("TxnAuthorizationStamp") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("TxnAuthorizationStamp").InnerText);
								receivePayment_CreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID = ItemRef.SelectSingleNode("ClientTransID") == null ? null : ItemRef.SelectSingleNode("ClientTransID").InnerText;
							}

							#endregion

							receivePaymentRet.CreditCardTxnInfo.Add(receivePayment_CreditCardTxnInfo);

						}
						#endregion

						#region AppliedToTxnRet LineItems
						LineItemList = Item.SelectNodes("AppliedToTxnRet");

						for (int k = 0; k < LineItemList.Count; k++) {
							LineItem = LineItemList.Item(k);
							receivePayment_AppliedToTxnRet = new AppliedToTxnRet {
								TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
								TxnType = LineItem.SelectSingleNode("TxnLineID") == null ? null : LineItem.SelectSingleNode("TxnLineID").InnerText,
								TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
								RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
								BalanceRemaining = Item.SelectSingleNode("BalanceRemaining") == null ? 0 : decimal.Parse(Item.SelectSingleNode("BalanceRemaining").InnerText),
								Amount = Item.SelectSingleNode("Amount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("Amount").InnerText),
								DiscountAmount = Item.SelectSingleNode("DiscountAmount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("DiscountAmount").InnerText),
							};

							#region DiscountAccountRef
							if (LineItem.SelectSingleNode("DiscountAccountRef") == null) {
								receivePayment_AppliedToTxnRet.DiscountAccountRef.ListID = null;
								receivePayment_AppliedToTxnRet.DiscountAccountRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("DiscountAccountRef");
								receivePayment_AppliedToTxnRet.DiscountAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								receivePayment_AppliedToTxnRet.DiscountAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region DiscountClassRef
							if (LineItem.SelectSingleNode("DiscountClassRef") == null) {
								receivePayment_AppliedToTxnRet.DiscountClassRef.ListID = null;
								receivePayment_AppliedToTxnRet.DiscountClassRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("DiscountClassRef");
								receivePayment_AppliedToTxnRet.DiscountClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								receivePayment_AppliedToTxnRet.DiscountClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region LinkedTxn
							if (Item.SelectSingleNode("LinkedTxn") == null) {
								receivePayment_AppliedToTxnRet.LinkedTxn.TxnID = null;
								receivePayment_AppliedToTxnRet.LinkedTxn.TxnType = null;
								receivePayment_AppliedToTxnRet.LinkedTxn.TxnDate = null;
								receivePayment_AppliedToTxnRet.LinkedTxn.RefNumber = null;
								receivePayment_AppliedToTxnRet.LinkedTxn.LinkType = null;
								receivePayment_AppliedToTxnRet.LinkedTxn.Amount = null;

							}
							else {
								ItemRef = Item.SelectSingleNode("LinkedTxn");
								receivePayment_AppliedToTxnRet.LinkedTxn.TxnID = ItemRef.SelectSingleNode("TxnID") == null ? null : ItemRef.SelectSingleNode("TxnID").InnerText;
								receivePayment_AppliedToTxnRet.LinkedTxn.TxnType = ItemRef.SelectSingleNode("TxnType") == null ? null : ItemRef.SelectSingleNode("TxnType").InnerText;
								receivePayment_AppliedToTxnRet.LinkedTxn.TxnDate = ItemRef.SelectSingleNode("TxnDate") == null ? null : ItemRef.SelectSingleNode("TxnDate").InnerText;
								receivePayment_AppliedToTxnRet.LinkedTxn.RefNumber = ItemRef.SelectSingleNode("RefNumber") == null ? null : ItemRef.SelectSingleNode("RefNumber").InnerText;
								receivePayment_AppliedToTxnRet.LinkedTxn.LinkType = ItemRef.SelectSingleNode("LinkType") == null ? null : ItemRef.SelectSingleNode("LinkType").InnerText;
								receivePayment_AppliedToTxnRet.LinkedTxn.Amount = ItemRef.SelectSingleNode("Amount") == null ? (decimal?) null : decimal.Parse(ItemRef.SelectSingleNode("Amount").InnerText);
							}
							#endregion


							receivePaymentRet.AppliedToTxnRet.Add(receivePayment_AppliedToTxnRet);
						}
						#endregion AppliedToTxnRet LineItems

						list.Add(receivePaymentRet);
					}
					break;
				#endregion

				#region SalesReceiptQuery
				case ActionType.SalesReceiptQuery:
					SalesReceiptRet salesReceiptRet;
					CreditCardTxnInfo salesReceiptRet_CreditCardTxnInfo;
					ItemList = responseNode.SelectNodes("//CustomerRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						salesReceiptRet = new SalesReceiptRet {
							TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
							TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
							RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
							IsPending = Item.SelectSingleNode("IsPending") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsPending").InnerText),
							CheckNumber = Item.SelectSingleNode("CheckNumber") == null ? null : Item.SelectSingleNode("CheckNumber").InnerText,
							DueDate = DateTime.Parse(Item.SelectSingleNode("DueDate").InnerText),
							ShipDate = DateTime.Parse(Item.SelectSingleNode("ShipDate").InnerText),
							FOB = Item.SelectSingleNode("FOB") == null ? null : Item.SelectSingleNode("FOB").InnerText,
							Subtotal = Item.SelectSingleNode("Subtotal") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("Subtotal").InnerText),
							SalesTaxPercentage = Item.SelectSingleNode("SalesTaxPercentage") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("SalesTaxPercentage").InnerText),
							SalesTaxTotal = Item.SelectSingleNode("SalesTaxTotal") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("SalesTaxTotal").InnerText),
							TotalAmount = Item.SelectSingleNode("TotalAmount") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("TotalAmount").InnerText),
							ExchangeRate = Item.SelectSingleNode("ExchangeRate") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("ExchangeRate").InnerText),
							TotalAmountInHomeCurrency = Item.SelectSingleNode("TotalAmountInHomeCurrency") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("TotalAmountInHomeCurrency").InnerText),
							Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
							IsToBePrinted = Item.SelectSingleNode("IsToBePrinted") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsToBePrinted").InnerText),
							IsToBeEmailed = Item.SelectSingleNode("IsToBeEmailed") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsToBeEmailed").InnerText),
							IsTaxIncluded = Item.SelectSingleNode("IsTaxIncluded") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsTaxIncluded").InnerText),
							Other = Item.SelectSingleNode("Other") == null ? null : Item.SelectSingleNode("Other").InnerText,
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
						};

						#region CustomerRef
						if (Item.SelectSingleNode("CustomerRef") == null) {
							salesReceiptRet.CustomerRef.ListID = null;
							salesReceiptRet.CustomerRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CustomerRef");
							salesReceiptRet.CustomerRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							salesReceiptRet.CustomerRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region ClassRef
						if (Item.SelectSingleNode("ClassRef") == null) {
							salesReceiptRet.ClassRef.ListID = null;
							salesReceiptRet.ClassRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("ClassRef");
							salesReceiptRet.ClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							salesReceiptRet.ClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region TemplateRef
						if (Item.SelectSingleNode("TemplateRef") == null) {
							salesReceiptRet.TemplateRef.ListID = null;
							salesReceiptRet.TemplateRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("TemplateRef");
							salesReceiptRet.TemplateRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							salesReceiptRet.TemplateRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region BillAddress
						if (Item.SelectSingleNode("BillAddress") == null) {
							salesReceiptRet.BillAddress.Addr1 = null;
							salesReceiptRet.BillAddress.Addr2 = null;
							salesReceiptRet.BillAddress.Addr3 = null;
							salesReceiptRet.BillAddress.Addr4 = null;
							salesReceiptRet.BillAddress.Addr5 = null;
							salesReceiptRet.BillAddress.City = null;
							salesReceiptRet.BillAddress.State = null;
							salesReceiptRet.BillAddress.PostalCode = null;
							salesReceiptRet.BillAddress.Country = null;
							salesReceiptRet.BillAddress.Note = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("BillAddress");
							salesReceiptRet.BillAddress.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							salesReceiptRet.BillAddress.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							salesReceiptRet.BillAddress.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							salesReceiptRet.BillAddress.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							salesReceiptRet.BillAddress.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
							salesReceiptRet.BillAddress.City = ItemRef.SelectSingleNode("City") == null ? null : ItemRef.SelectSingleNode("City").InnerText;
							salesReceiptRet.BillAddress.State = ItemRef.SelectSingleNode("State") == null ? null : ItemRef.SelectSingleNode("State").InnerText;
							salesReceiptRet.BillAddress.PostalCode = ItemRef.SelectSingleNode("PostalCode") == null ? null : ItemRef.SelectSingleNode("PostalCode").InnerText;
							salesReceiptRet.BillAddress.Country = ItemRef.SelectSingleNode("Country") == null ? null : ItemRef.SelectSingleNode("Country").InnerText;
							salesReceiptRet.BillAddress.Note = ItemRef.SelectSingleNode("Note") == null ? null : ItemRef.SelectSingleNode("Note").InnerText;
						}
						#endregion

						#region BillAddressBlock
						if (Item.SelectSingleNode("BillAddressBlock") == null) {
							salesReceiptRet.BillAddressBlock.Addr1 = null;
							salesReceiptRet.BillAddressBlock.Addr2 = null;
							salesReceiptRet.BillAddressBlock.Addr3 = null;
							salesReceiptRet.BillAddressBlock.Addr4 = null;
							salesReceiptRet.BillAddressBlock.Addr5 = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("BillAddressBlock");
							salesReceiptRet.BillAddressBlock.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							salesReceiptRet.BillAddressBlock.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							salesReceiptRet.BillAddressBlock.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							salesReceiptRet.BillAddressBlock.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							salesReceiptRet.BillAddressBlock.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
						}
						#endregion

						#region ShipAddress
						if (Item.SelectSingleNode("ShipAddress") == null) {
							salesReceiptRet.ShipAddress.Addr1 = null;
							salesReceiptRet.ShipAddress.Addr2 = null;
							salesReceiptRet.ShipAddress.Addr3 = null;
							salesReceiptRet.ShipAddress.Addr4 = null;
							salesReceiptRet.ShipAddress.Addr5 = null;
							salesReceiptRet.ShipAddress.City = null;
							salesReceiptRet.ShipAddress.State = null;
							salesReceiptRet.ShipAddress.PostalCode = null;
							salesReceiptRet.ShipAddress.Country = null;
							salesReceiptRet.ShipAddress.Note = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("ShipAddress");
							salesReceiptRet.ShipAddress.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							salesReceiptRet.ShipAddress.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							salesReceiptRet.ShipAddress.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							salesReceiptRet.ShipAddress.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							salesReceiptRet.ShipAddress.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
							salesReceiptRet.ShipAddress.City = ItemRef.SelectSingleNode("City") == null ? null : ItemRef.SelectSingleNode("City").InnerText;
							salesReceiptRet.ShipAddress.State = ItemRef.SelectSingleNode("State") == null ? null : ItemRef.SelectSingleNode("State").InnerText;
							salesReceiptRet.ShipAddress.PostalCode = ItemRef.SelectSingleNode("PostalCode") == null ? null : ItemRef.SelectSingleNode("PostalCode").InnerText;
							salesReceiptRet.ShipAddress.Country = ItemRef.SelectSingleNode("Country") == null ? null : ItemRef.SelectSingleNode("Country").InnerText;
							salesReceiptRet.ShipAddress.Note = ItemRef.SelectSingleNode("Note") == null ? null : ItemRef.SelectSingleNode("Note").InnerText;
						}
						#endregion

						#region ShipAddressBlock
						if (Item.SelectSingleNode("ShipAddressBlock") == null) {
							salesReceiptRet.ShipAddressBlock.Addr1 = null;
							salesReceiptRet.ShipAddressBlock.Addr2 = null;
							salesReceiptRet.ShipAddressBlock.Addr3 = null;
							salesReceiptRet.ShipAddressBlock.Addr4 = null;
							salesReceiptRet.ShipAddressBlock.Addr5 = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("ShipAddressBlock");
							salesReceiptRet.ShipAddressBlock.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							salesReceiptRet.ShipAddressBlock.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							salesReceiptRet.ShipAddressBlock.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							salesReceiptRet.ShipAddressBlock.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							salesReceiptRet.ShipAddressBlock.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
						}
						#endregion

						#region PaymentMethodRef
						if (Item.SelectSingleNode("PaymentMethodRef") == null) {
							salesReceiptRet.PaymentMethodRef.ListID = null;
							salesReceiptRet.PaymentMethodRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("PaymentMethodRef");
							salesReceiptRet.PaymentMethodRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							salesReceiptRet.PaymentMethodRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region SalesRepRef
						if (Item.SelectSingleNode("SalesRepRef") == null) {
							salesReceiptRet.SalesRepRef.ListID = null;
							salesReceiptRet.SalesRepRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("SalesRepRef");
							salesReceiptRet.SalesRepRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							salesReceiptRet.SalesRepRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region ShipMethodRef
						if (Item.SelectSingleNode("ShipMethodRef") == null) {
							salesReceiptRet.ShipMethodRef.ListID = null;
							salesReceiptRet.ShipMethodRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("ShipMethodRef");
							salesReceiptRet.ShipMethodRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							salesReceiptRet.ShipMethodRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region ItemSalesTaxRef
						if (Item.SelectSingleNode("ItemSalesTaxRef") == null) {
							salesReceiptRet.ItemSalesTaxRef.ListID = null;
							salesReceiptRet.ItemSalesTaxRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("ItemSalesTaxRef");
							salesReceiptRet.ItemSalesTaxRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							salesReceiptRet.ItemSalesTaxRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CurrencyRef
						if (Item.SelectSingleNode("CurrencyRef") == null) {
							salesReceiptRet.CurrencyRef.ListID = null;
							salesReceiptRet.CurrencyRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CurrencyRef");
							salesReceiptRet.CurrencyRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							salesReceiptRet.CurrencyRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CustomerMsgRef
						if (Item.SelectSingleNode("CustomerMsgRef") == null) {
							salesReceiptRet.CustomerMsgRef.ListID = null;
							salesReceiptRet.CustomerMsgRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CustomerMsgRef");
							salesReceiptRet.CustomerMsgRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							salesReceiptRet.CustomerMsgRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CustomerSalesTaxCodeRef
						if (Item.SelectSingleNode("CustomerSalesTaxCodeRef") == null) {
							salesReceiptRet.CustomerSalesTaxCodeRef.ListID = null;
							salesReceiptRet.CustomerSalesTaxCodeRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CustomerSalesTaxCodeRef");
							salesReceiptRet.CustomerSalesTaxCodeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							salesReceiptRet.CustomerSalesTaxCodeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region DepositToAccountRef
						if (Item.SelectSingleNode("DepositToAccountRef") == null) {
							salesReceiptRet.DepositToAccountRef.ListID = null;
							salesReceiptRet.DepositToAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("DepositToAccountRef");
							salesReceiptRet.DepositToAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							salesReceiptRet.DepositToAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CreditCardTxnInfo List

						LineItemList = Item.SelectNodes("CreditCardTxnInfo");
						for (int j = 0; j < LineItemList.Count; j++) {
							LineItem = LineItemList.Item(j);
							salesReceiptRet_CreditCardTxnInfo = new CreditCardTxnInfo();

							#region CreditCardTxnInputInfo
							if (LineItem.SelectSingleNode("CreditCardTxnInputInfo") == null) {
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardNumber = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationMonth = 0;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationYear = 0;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.NameOnCard = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType = null;
							}
							else {
								ItemRef = LineItem.SelectSingleNode("CreditCardTxnInputInfo");
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardNumber = ItemRef.SelectSingleNode("CreditCardNumber") == null ? null : ItemRef.SelectSingleNode("CreditCardNumber").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationMonth = ItemRef.SelectSingleNode("ExpirationMonth") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("ExpirationMonth").InnerText);
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationYear = ItemRef.SelectSingleNode("ExpirationYear") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("ExpirationYear").InnerText);
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.NameOnCard = ItemRef.SelectSingleNode("NameOnCard") == null ? null : ItemRef.SelectSingleNode("NameOnCard").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress = ItemRef.SelectSingleNode("CreditCardAddress") == null ? null : ItemRef.SelectSingleNode("CreditCardAddress").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode = ItemRef.SelectSingleNode("CreditCardPostalCode") == null ? null : ItemRef.SelectSingleNode("CreditCardPostalCode").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode = ItemRef.SelectSingleNode("CommercialCardCode") == null ? null : ItemRef.SelectSingleNode("CommercialCardCode").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode = ItemRef.SelectSingleNode("TransactionMode") == null ? null : ItemRef.SelectSingleNode("TransactionMode").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType = ItemRef.SelectSingleNode("CreditCardTxnType") == null ? null : ItemRef.SelectSingleNode("CreditCardTxnType").InnerText;
							}
							#endregion

							#region CreditCardTxnResultInfo
							if (LineItem.SelectSingleNode("CreditCardTxnResultInfo") == null) {
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.ResultCode = 0;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.ResultMessage = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.CreditCardTransID = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.MerchantAccountNumber = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchId = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode = 0;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentStatus = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationTime = null;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp = 0;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID = null;

							}
							else {
								ItemRef = LineItem.SelectSingleNode("CreditCardTxnResultInfo");
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.ResultCode = ItemRef.SelectSingleNode("ResultCode") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("ResultCode").InnerText);
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.ResultMessage = ItemRef.SelectSingleNode("ResultMessage") == null ? null : ItemRef.SelectSingleNode("ResultMessage").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.CreditCardTransID = ItemRef.SelectSingleNode("CreditCardTransID") == null ? null : ItemRef.SelectSingleNode("CreditCardTransID").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.MerchantAccountNumber = ItemRef.SelectSingleNode("MerchantAccountNumber") == null ? null : ItemRef.SelectSingleNode("MerchantAccountNumber").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode = ItemRef.SelectSingleNode("AuthorizationCode") == null ? null : ItemRef.SelectSingleNode("AuthorizationCode").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet = ItemRef.SelectSingleNode("AVSStreet") == null ? null : ItemRef.SelectSingleNode("AVSStreet").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip = ItemRef.SelectSingleNode("AVSZip") == null ? null : ItemRef.SelectSingleNode("AVSZip").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch = ItemRef.SelectSingleNode("CardSecurityCodeMatch") == null ? null : ItemRef.SelectSingleNode("CardSecurityCodeMatch").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchId = ItemRef.SelectSingleNode("ReconBatchId") == null ? null : ItemRef.SelectSingleNode("ReconBatchId").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode = ItemRef.SelectSingleNode("PaymentGroupingCode") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("PaymentGroupingCode").InnerText);
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentStatus = ItemRef.SelectSingleNode("PaymentStatus") == null ? null : ItemRef.SelectSingleNode("PaymentStatus").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationTime = ItemRef.SelectSingleNode("TxnAuthorizationTime") == null ? null : ItemRef.SelectSingleNode("TxnAuthorizationTime").InnerText;
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp = ItemRef.SelectSingleNode("TxnAuthorizationStamp") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("TxnAuthorizationStamp").InnerText);
								salesReceiptRet_CreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID = ItemRef.SelectSingleNode("ClientTransID") == null ? null : ItemRef.SelectSingleNode("ClientTransID").InnerText;
							}

							#endregion

							salesReceiptRet.CreditCardTxnInfo.Add(salesReceiptRet_CreditCardTxnInfo);

						}
						#endregion

						list.Add(salesReceiptRet);
					}
					break;
				#endregion

				default:
					break;
			}

			return list;
		}
		#endregion Utilities

		#region Public Methods

		#region Connection
		public void CloseConnection()
		{
			if (SessionBegun) {
				Rp.EndSession(SessionId);
				SessionBegun = false;
			}

			if (ConnectionOpen) {
				Rp.CloseConnection();
				ConnectionOpen = false;
			}
		}

		public bool OpenConnection(string appId = null, string appName = null, QBXMLRPConnectionType connPref = QBXMLRPConnectionType.localQBD, string qbFileName = null, QBFileMode reqFileMode = QBFileMode.qbFileOpenDoNotCare)
		{
			#region Input Validation
			if (String.IsNullOrWhiteSpace(appId))
				appId = DEFAULT_APP_NAME;
			if (String.IsNullOrWhiteSpace(appName))
				appName = DEFAULT_APP_NAME;
			if (connPref < 0)
				connPref = QBXMLRPConnectionType.localQBD;
			if (String.IsNullOrWhiteSpace(qbFileName))
				qbFileName = "";
			if (reqFileMode < 0)
				reqFileMode = QBFileMode.qbFileOpenDoNotCare;
			#endregion Input Validation

			try {
				Rp = new RequestProcessor2();
				Rp.OpenConnection2(appId, appName, connPref);
				ConnectionOpen = true;
				SessionId = Rp.BeginSession(qbFileName, reqFileMode);
				SessionBegun = true;
				return true;
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in QBV20191021Util.OpenConnection(string appId='{3}', string appName='{4}', QBXMLRPConnectionType connPref='{5}', string qbFileName='{6}', QBFileMode reqFileMode='{7}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, appId, appName, connPref.ToString(), qbFileName, reqFileMode.ToString());
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of QBV20191021Util.OpenConnection(string appId='{3}', string appName='{4}', QBXMLRPConnectionType connPref='{5}', string qbFileName='{6}', QBFileMode reqFileMode='{7}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, appId, appName, connPref.ToString(), qbFileName, reqFileMode.ToString());

				if (Debug)
					Console.Write("\n{0}", log);
				#endregion Log

				if (SessionBegun) {
					Rp.EndSession(SessionId);
					SessionBegun = false;
				}

				if (ConnectionOpen) {
					Rp.CloseConnection();
					ConnectionOpen = false;
				}

				return false;
			}
		}
		#endregion Connection

		#region AccountAdd
		public bool AddAccountAdjustment(string name, string accountType)
		{
			#region Input Validation
			var errorMsg = "";

			if (String.IsNullOrWhiteSpace(name))
				errorMsg = String.Format("{0}<name> is required.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(accountType))
				errorMsg = String.Format("{0}<accountType> is required.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg)) {
				Console.Write("\n{0}Error in QB20191021Util.AddAccountAdjustment().{1}", errorMsg, Environment.NewLine);
				return false;
			}
			#endregion Input Validation

			try {
				var parameters = new KeyValuePair<string, object>[] {
					new KeyValuePair<string, object>("name", name),
					new KeyValuePair<string, object>("accountType", accountType),

				};

				var result = DoAction(ActionType.AccountAdd, parameters);
				return true;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					errorMsg = String.Format("{0}{2}Exception in QB20191021Util.AddAccountAdjustment(string name='{3}', string accountType='{4}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine,name, accountType);
				else
					errorMsg = String.Format("{0}{2}Exception in INNER EXCEPTION of QB20191021Util.AddAccountAdjustment(string name='{3}', string accountType='{4}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine,name, accountType);

				Console.Write("\n{0}", errorMsg);
				return false;
			}
		}
		#endregion

		#region AccountMod
		public bool ModAccountAdjustment(string listID, string editSequence)
		{
			#region Input Validation
			var errorMsg = "";

			if (String.IsNullOrWhiteSpace(listID))
				errorMsg = String.Format("{0}<listID> is required.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(editSequence))
				errorMsg = String.Format("{0}<editSequence> is required.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg)) {
				Console.Write("\n{0}Error in QB20191021Util.ModAccountAdjustment().{1}", errorMsg, Environment.NewLine);
				return false;
			}
			#endregion Input Validation

			try {
				var parameters = new KeyValuePair<string, object>[] {
					new KeyValuePair<string, object>("listID", listID),
					new KeyValuePair<string, object>("editSequence", editSequence),

				};

				var result = DoAction(ActionType.AccountMod, parameters);
				return true;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					errorMsg = String.Format("{0}{2}Exception in QB20191021Util.ModAccountAdjustment(string name='{3}', string accountType='{4}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, listID, editSequence);
				else
					errorMsg = String.Format("{0}{2}Exception in INNER EXCEPTION of QB20191021Util.ModAccountAdjustment(string name='{3}', string accountType='{4}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, listID, editSequence);

				Console.Write("\n{0}", errorMsg);
				return false;
			}
		}
		#endregion

		#region AccountQuery
		public List<AccountRet> GetAccountAdjustments(string matchCriterion, string name)
		{
			#region Input Validation
			var errorMsg = "";

			if (String.IsNullOrWhiteSpace(matchCriterion))
				errorMsg = String.Format("{0}<matchCriterion> is required.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(name))
				errorMsg = String.Format("{0}<name> is required.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg)) {
				Console.Write("\n{0}Error in QB20191021Util.GetAllAccountAdjustments().{1}", errorMsg, Environment.NewLine);
				return new List<AccountRet>();
			}
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("matchCriterion", matchCriterion),
				new KeyValuePair<string, object>("name", name),
			};

			var list = new List<AccountRet>();
			var rets = DoAction(ActionType.AccountQuery, parameters);

			foreach (AccountRet ret in rets)
				list.Add(ret);

			return list;


		}
		#endregion


		#region ARRefundCreditCardQuery
		public List<ARRefundCreditCardRet> GetAllArrRefundCreditCardAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<ARRefundCreditCardRet>();
			var rets = DoAction(ActionType.ARRefundCreditCardQuery, parameters);

			foreach (ARRefundCreditCardRet ret in rets)
				list.Add(ret);

			return list;
		}
		#endregion

		#region BillPaymentCheckAdd
		public bool AddBillPaymentCheckAdjustment(string payeeEntityRefListId, string appliedToTxnAddTxnID, int appliedToTxnAddTxnIDPaymentAmount, string appliedToTxnAddSetCreditAppliedAmount)
		{
			#region Input Validation
			var errorMsg = "";

			if (String.IsNullOrWhiteSpace(payeeEntityRefListId))
				errorMsg = String.Format("{0}<payeeEntityRefListId> is required.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(appliedToTxnAddTxnID))
				errorMsg = String.Format("{0}<appliedToTxnAddTxnID> is required.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(appliedToTxnAddTxnIDPaymentAmount.ToString()))
				errorMsg = String.Format("{0}<appliedToTxnAddTxnIDPaymentAmount> is required.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(appliedToTxnAddSetCreditAppliedAmount))
				errorMsg = String.Format("{0}<appliedToTxnAddSetCreditAppliedAmount> is required.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg)) {
				Console.Write("\n{0}Error in QB20191021Util.AddBillPaymentCheckAdjustment(string payeeEntityRefListId, string appliedToTxnAddTxnID, int appliedToTxnAddTxnIDPaymentAmount, string appliedToTxnAddSetCreditAppliedAmount).{1}", errorMsg, Environment.NewLine);
				return false;
			}
			#endregion Input Validation

			try {
				var parameters = new KeyValuePair<string, object>[] {
					new KeyValuePair<string, object>("payeeEntityRefListId", payeeEntityRefListId),
					new KeyValuePair<string, object>("appliedToTxnAddTxnID", appliedToTxnAddTxnID),
					new KeyValuePair<string, object>("appliedToTxnAddTxnIDPaymentAmount", appliedToTxnAddTxnIDPaymentAmount),
					new KeyValuePair<string, object>("appliedToTxnAddSetCreditAppliedAmount", appliedToTxnAddSetCreditAppliedAmount),

				};

				var result = DoAction(ActionType.BillPaymentCheckAdd, parameters);
				return true;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					errorMsg = String.Format("{0}{2}Exception in QB20191021Util.AddBillPaymentCheckAdjustment(string accountRefListId='{3}', string itemRefFullName='{4}', int quantityDifference='{5}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, payeeEntityRefListId, appliedToTxnAddTxnID, appliedToTxnAddTxnIDPaymentAmount, appliedToTxnAddSetCreditAppliedAmount);
				else
					errorMsg = String.Format("{0}{2}Exception in INNER EXCEPTION of QB20191021Util.AddBillPaymentCheckAdjustment(string accountRefListId='{3}', string itemRefFullName='{4}', int quantityDifference='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, payeeEntityRefListId, appliedToTxnAddTxnID, appliedToTxnAddTxnIDPaymentAmount, appliedToTxnAddSetCreditAppliedAmount);

				Console.Write("\n{0}", errorMsg);
				return false;
			}
		}
		#endregion

		#region BillPaymentCheckQuery
		public List<BillPaymentCheckRet> GetAllBillPaymentCheckAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<BillPaymentCheckRet>();
			var rets = DoAction(ActionType.BillPaymentCheckQuery, parameters);

			foreach (BillPaymentCheckRet ret in rets)
				list.Add(ret);

			return list;
		}

		#endregion

		#region BillPaymentCreditCardAdd
		public bool AddBillPaymentCreditCardAdjustment(string payeeEntityRefListId,string creditCardAccountRefListId, string appliedToTxnAddTxnID, int appliedToTxnAddTxnIDPaymentAmount, string appliedToTxnAddSetCreditAppliedAmount)
		{
			#region Input Validation
			var errorMsg = "";

			if (String.IsNullOrWhiteSpace(payeeEntityRefListId))
				errorMsg = String.Format("{0}<accountRefListId> is required.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(creditCardAccountRefListId))
				errorMsg = String.Format("{0}<creditCardAccountRefListId> is required.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(appliedToTxnAddTxnID))
				errorMsg = String.Format("{0}<appliedToTxnAddTxnID> is required.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(appliedToTxnAddTxnIDPaymentAmount.ToString()))
				errorMsg = String.Format("{0}<appliedToTxnAddTxnIDPaymentAmount> is required.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(appliedToTxnAddSetCreditAppliedAmount))
				errorMsg = String.Format("{0}<appliedToTxnAddSetCreditAppliedAmount> is required.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg)) {
				Console.Write("\n{0}Error in QB20191021Util.AddBillPaymentCreditCardAdjustment(string payeeEntityRefListId,string creditCardAccountRefListId, string appliedToTxnAddTxnID, int appliedToTxnAddTxnIDPaymentAmount, string appliedToTxnAddSetCreditAppliedAmount).{1}", errorMsg, Environment.NewLine);
				return false;
			}
			#endregion Input Validation

			try {
				var parameters = new KeyValuePair<string, object>[] {
					new KeyValuePair<string, object>("payeeEntityRefListId", payeeEntityRefListId),
					new KeyValuePair<string, object>("creditCardAccountRefListId", creditCardAccountRefListId),
					new KeyValuePair<string, object>("appliedToTxnAddTxnID", appliedToTxnAddTxnID),
					new KeyValuePair<string, object>("appliedToTxnAddTxnIDPaymentAmount", appliedToTxnAddTxnIDPaymentAmount),
					new KeyValuePair<string, object>("appliedToTxnAddSetCreditAppliedAmount", appliedToTxnAddSetCreditAppliedAmount),

				};

				var result = DoAction(ActionType.BillPaymentCreditCardAdd, parameters);
				return true;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					errorMsg = String.Format("{0}{2}Exception in QB20191021Util.AddBillPaymentCreditCardAdjustment(string payeeEntityRefListId ='{3}',string creditCardAccountRefListId ='{4}', string appliedToTxnAddTxnID='{5}', int appliedToTxnAddTxnIDPaymentAmount='{6}', string appliedToTxnAddSetCreditAppliedAmount='{7}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, payeeEntityRefListId, creditCardAccountRefListId, appliedToTxnAddTxnID, appliedToTxnAddTxnIDPaymentAmount, appliedToTxnAddSetCreditAppliedAmount);
				else
					errorMsg = String.Format("{0}{2}Exception in INNER EXCEPTION of QB20191021Util.AddBillPaymentCreditCardAdjustment(string accountRefListId='{3}', string itemRefFullName='{4}', int quantityDifference='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, payeeEntityRefListId, creditCardAccountRefListId, appliedToTxnAddTxnID, appliedToTxnAddTxnIDPaymentAmount, appliedToTxnAddSetCreditAppliedAmount);

				Console.Write("\n{0}", errorMsg);
				return false;
			}
		}
		#endregion

		#region BillPaymentCreditCardQuery
		public List<BillPaymentCreditCardRet> GetAllBillPaymentCreditCardAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<BillPaymentCreditCardRet>();
			var rets = DoAction(ActionType.BillPaymentCreditCardQuery, parameters);

			foreach (BillPaymentCreditCardRet ret in rets)
				list.Add(ret);

			return list;
		}

		#endregion
		
		#region BillAdd
		public bool AddBillAdjustment(string vendorRefListId)
		{
			#region Input Validation
			var errorMsg = "";

			if (String.IsNullOrWhiteSpace(vendorRefListId))
				errorMsg = String.Format("{0}<vendorRefListId> is required.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg)) {
				Console.Write("\n{0}Error in QB20191021Util.AddBillAdjustment(string vendorRefListId).{1}", errorMsg, Environment.NewLine);
				return false;
			}
			#endregion Input Validation

			try {
				var parameters = new KeyValuePair<string, object>[] {
					new KeyValuePair<string, object>("vendorRefListId", vendorRefListId),
				};

				var result = DoAction(ActionType.BillAdd, parameters);
				return true;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					errorMsg = String.Format("{0}{2}Exception in QB20191021Util.AddBillAdjustment(string vendorRefListId='{3}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, vendorRefListId);
				else
					errorMsg = String.Format("{0}{2}Exception in INNER EXCEPTION of QB20191021Util.AddBillAdjustment(string vendorRefListId='{3}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, vendorRefListId);

				Console.Write("\n{0}", errorMsg);
				return false;
			}
		}
		#endregion

		#region BillQuery
		public List<BillRet> GetAllBillAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<BillRet>();
			var rets = DoAction(ActionType.BillQuery, parameters);

			foreach (BillRet ret in rets)
				list.Add(ret);

			return list;
		}

		#endregion

		#region ChargeAdd
		/// <summary>
		/// if AccountRef refers to an A/P account, CustomerRef must refer to a vendor (not to a customer).
		/// </summary>
		/// <param name="customerRefListId">CustomerRef property: List Id</param>
		/// <returns></returns>
		public bool AddChargeAdjustment(string customerRefListId)
		{
			#region Input Validation
			var errorMsg = "";

			if (String.IsNullOrWhiteSpace(customerRefListId))
				errorMsg = String.Format("{0}<customerRefListId> is required.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg)) {
				Console.Write("\n{0}Error in QB20191021Util.AddChargeAdjustment(string customerRefListId).{1}", errorMsg, Environment.NewLine);
				return false;
			}
			#endregion Input Validation

			try {
				var parameters = new KeyValuePair<string, object>[] {
					new KeyValuePair<string, object>("customerRefListId", customerRefListId),
				};

				var result = DoAction(ActionType.ChargeAdd, parameters);
				return true;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					errorMsg = String.Format("{0}{2}Exception in QB20191021Util.AddChargeAdjustment(string customerRefListId='{3}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, customerRefListId);
				else
					errorMsg = String.Format("{0}{2}Exception in INNER EXCEPTION of QB20191021Util.AddChargeAdjustment(string customerRefListId='{3}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, customerRefListId);

				Console.Write("\n{0}", errorMsg);
				return false;
			}
		}
		#endregion

		#region ChargeQuery
		public List<ChargeRet> GetAllChargeAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<ChargeRet>();
			var rets = DoAction(ActionType.ChargeQuery, parameters);

			foreach (ChargeRet ret in rets)
				list.Add(ret);

			return list;
		}

		#endregion

		#region CheckAdd
		public bool AddCheckAdjustment(string accountRefListId)
		{
			#region Input Validation
			var errorMsg = "";

			if (String.IsNullOrWhiteSpace(accountRefListId))
				errorMsg = String.Format("{0}<accountRefListId> is required.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg)) {
				Console.Write("\n{0}Error in QB20191021Util.AddCheckAdjustment(string accountRefListId).{1}", errorMsg, Environment.NewLine);
				return false;
			}
			#endregion Input Validation

			try {
				var parameters = new KeyValuePair<string, object>[] {
					new KeyValuePair<string, object>("accountRefListId", accountRefListId),

				};

				var result = DoAction(ActionType.CheckAdd, parameters);
				return true;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					errorMsg = String.Format("{0}{2}Exception in QB20191021Util.AddCheckAdjustment(string accountRefListId='{3}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, accountRefListId);
				else
					errorMsg = String.Format("{0}{2}Exception in INNER EXCEPTION of QB20191021UtilAddCheckAdjustment(string accountRefListId='{3}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, accountRefListId);

				Console.Write("\n{0}", errorMsg);
				return false;
			}
		}
		#endregion

		#region CheckQuery
		public List<CheckRet> GetAllCheckAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<CheckRet>();
			var rets = DoAction(ActionType.CheckQuery, parameters);

			foreach (CheckRet ret in rets)
				list.Add(ret);

			return list;
		}

		#endregion

		#region CreditCardChargeAdd
		public bool AddCreditCardChargeAdjustment(string accountRefListId)
		{
			#region Input Validation
			var errorMsg = "";

			if (String.IsNullOrWhiteSpace(accountRefListId))
				errorMsg = String.Format("{0}<accountRefListId> is required.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg)) {
				Console.Write("\n{0}Error in QB20191021Util.AddCheckAdjustment(string accountRefListId).{1}", errorMsg, Environment.NewLine);
				return false;
			}
			#endregion Input Validation

			try {
				var parameters = new KeyValuePair<string, object>[] {
					new KeyValuePair<string, object>("accountRefListId", accountRefListId),
				};

				var result = DoAction(ActionType.CreditCardChargeAdd, parameters);
				return true;
			}
			catch (Exception ex) {
				if (ex.InnerException == null)
					errorMsg = String.Format("{0}{2}Exception in QB20191021Util.AddCreditCardChargeAdjustment(string accountRefListId='{3}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, accountRefListId);
				else
					errorMsg = String.Format("{0}{2}Exception in INNER EXCEPTION of QB20191021UtilAddCreditCardChargeAdjustment(string accountRefListId='{3}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, accountRefListId);

				Console.Write("\n{0}", errorMsg);
				return false;
			}
		}
		#endregion

		#region CreditCardChargeQuery
		public List<CreditCardChargeRet> GetAllCreditCardChargeAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<CreditCardChargeRet>();
			var rets = DoAction(ActionType.CreditCardChargeQuery, parameters);

			foreach (CreditCardChargeRet ret in rets)
				list.Add(ret);

			return list;
		}
		#endregion

		#region CreditCardCreditAdd
		public bool AddCreditCardCreditAdjustment(string accountRefListId)
		{
			#region Input Validation
			var errorMsg = "";

			if (String.IsNullOrWhiteSpace(accountRefListId))
				errorMsg = String.Format("{0}<accountRefListId> is required.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg)) {
				Console.Write("\n{0}Error in QB20191021Util.AddCheckAdjustment(string accountRefListId).{1}", errorMsg, Environment.NewLine);
				return false;
			}
			#endregion Input Validation

			try {
				var parameters = new KeyValuePair<string, object>[] {
					new KeyValuePair<string, object>("accountRefListId", accountRefListId),

				};

				var result = DoAction(ActionType.CreditCardCreditAdd, parameters);
				return true;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					errorMsg = String.Format("{0}{2}Exception in QB20191021Util.AddCreditCardChargeAdjustment(string accountRefListId='{3}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, accountRefListId);
				else
					errorMsg = String.Format("{0}{2}Exception in INNER EXCEPTION of QB20191021UtilAddCreditCardChargeAdjustment(string accountRefListId='{3}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, accountRefListId);

				Console.Write("\n{0}", errorMsg);
				return false;
			}
		}
		#endregion

		#region CreditCardCreditQuery
		public List<CreditCardCreditRet> GetAllCreditCardCreditAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<CreditCardCreditRet>();
			var rets = DoAction(ActionType.CreditCardCreditQuery, parameters);

			foreach (CreditCardCreditRet ret in rets)
				list.Add(ret);

			return list;
		}
		#endregion

		#region CustomerAdd
		public bool AddCustomerAdjustment(string name)
		{
			#region Input Validation
			var errorMsg = "";

			if (String.IsNullOrWhiteSpace(name))
				errorMsg = String.Format("{0}<name> is required.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg)) {
				Console.Write("\n{0}Error in QB20191021Util.AddCheckAdjustment(string name).{1}", errorMsg, Environment.NewLine);
				return false;
			}
			#endregion Input Validation

			try {
				var parameters = new KeyValuePair<string, object>[] {
					new KeyValuePair<string, object>("name", name),
				};

				var result = DoAction(ActionType.CustomerAdd, parameters);
				return true;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					errorMsg = String.Format("{0}{2}Exception in QB20191021Util.AddCustomerAdjustments(string name='{3}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, name);
				else
					errorMsg = String.Format("{0}{2}Exception in INNER EXCEPTION of QB20191021UtilAddCustomerAdjustments(string name='{3}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, name);

				Console.Write("\n{0}", errorMsg);
				return false;
			}
		}
		#endregion

		#region CustomerQuery
		public List<CustomerRet> GetAllCustomerAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<CustomerRet>();
			var rets = DoAction(ActionType.CustomeryQuery, parameters);

			foreach (CustomerRet ret in rets)
				list.Add(ret);

			return list;
		}

		#endregion

		#region DepositQuery
		public List<DepositRet> GetAllDepositAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<DepositRet>();
			var rets = DoAction(ActionType.DepositQuery, parameters);

			foreach (DepositRet ret in rets)
				list.Add(ret);

			return list;
		}

		#endregion

		#region InventoryAdjustment
		public bool AddInventoryAdjustment(string accountRefListId, string itemRefFullName, int quantityDifference)
		{
			#region Input Validation
			var errorMsg = "";

			if (String.IsNullOrWhiteSpace(accountRefListId))
				errorMsg = String.Format("{0}<accountRefListId> is required.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(itemRefFullName))
				errorMsg = String.Format("{0}<itemRefFullName> is required.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg)) {
				Console.Write("\n{0}Error in QB20191021Util.AddInventoryAdjustment(string accountRefListId, string itemRefFullName, int quantityDifference).{1}", errorMsg, Environment.NewLine);
				return false;
			}
			#endregion Input Validation

			try {
				var parameters = new KeyValuePair<string, object>[] {
					new KeyValuePair<string, object>("accountRefListId", accountRefListId),
					new KeyValuePair<string, object>("itemRefFullName", itemRefFullName),
					new KeyValuePair<string, object>("quantityDifference", quantityDifference),
				};

				var result = DoAction(ActionType.InventoryAdjustmentAdd, parameters);
				return true;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					errorMsg = String.Format("{0}{2}Exception in QB20191021Util.AddInventoryAdjustment(string accountRefListId='{3}', string itemRefFullName='{4}', int quantityDifference='{5}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, accountRefListId, itemRefFullName, quantityDifference);
				else
					errorMsg = String.Format("{0}{2}Exception in INNER EXCEPTION of QB20191021Util.AddInventoryAdjustment(string accountRefListId='{3}', string itemRefFullName='{4}', int quantityDifference='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, accountRefListId, itemRefFullName, quantityDifference);

				Console.Write("\n{0}", errorMsg);
				return false;
			}
		}

		public List<InventoryAdjustmentRet> GetAllInventoryAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<InventoryAdjustmentRet>();
			var rets = DoAction(ActionType.InventoryAdjustmentQuery, parameters);

			foreach (InventoryAdjustmentRet ret in rets)
				list.Add(ret);

			return list;
		}
		#endregion InventoryAdjustment

		#region ItemNonInventory
		public List<ItemNonInventoryRet> GetAllItemNonInventoryAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<ItemNonInventoryRet>();
			var rets = DoAction(ActionType.ItemNonInventoryQuery, parameters);

			foreach (ItemNonInventoryRet ret in rets)
				list.Add(ret);

			return list;
		}

		#endregion

		#region ReceivePaymentQuery
		public List<ReceivePaymentRet> GetAllReceivePaymentAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<ReceivePaymentRet>();
			var rets = DoAction(ActionType.ReceivePaymentQuery, parameters);

			foreach (ReceivePaymentRet ret in rets)
				list.Add(ret);

			return list;
		}

		#endregion

		#region SalesReceiptQuery
		public List<SalesReceiptRet> GetAllSalesReceiptAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<SalesReceiptRet>();
			var rets = DoAction(ActionType.SalesReceiptQuery, parameters);

			foreach (SalesReceiptRet ret in rets)
				list.Add(ret);

			return list;
		}

		#endregion

		#endregion Public Methods
	}
}