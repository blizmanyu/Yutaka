using System;
using Interop.QBFC13;

namespace com.intuit.idn.samples
{
	public partial class Sample
	{
		public void DoCustomerQuery()
		{
			bool sessionBegun = false;
			bool connectionOpen = false;
			QBSessionManager sessionManager = null;

			try {
				//Create the session Manager object
				sessionManager = new QBSessionManager();

				//Create the message set request object to hold our request
				IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US",13,0);
				requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

				BuildCustomerQueryRq(requestMsgSet);

				//Connect to QuickBooks and begin a session
				sessionManager.OpenConnection("", "Sample Code from OSR");
				connectionOpen = true;
				sessionManager.BeginSession("", ENOpenMode.omDontCare);
				sessionBegun = true;

				//Send the request and get the response from QuickBooks
				IMsgSetResponse responseMsgSet = sessionManager.DoRequests(requestMsgSet);

				//End the session and close the connection to QuickBooks
				sessionManager.EndSession();
				sessionBegun = false;
				sessionManager.CloseConnection();
				connectionOpen = false;

				WalkCustomerQueryRs(responseMsgSet);
			}
			catch (Exception e) {
				MessageBox.Show(e.Message, "Error");
				if (sessionBegun) {
					sessionManager.EndSession();
				}
				if (connectionOpen) {
					sessionManager.CloseConnection();
				}
			}
		}

		void BuildCustomerQueryRq(IMsgSetRequest requestMsgSet)
		{
			ICustomerQuery CustomerQueryRq= requestMsgSet.AppendCustomerQueryRq();
			//Set attributes
			//Set field value for metaData
			CustomerQueryRq.metaData.SetValue("IQBENmetaDataType");
			//Set field value for iterator
			CustomerQueryRq.iterator.SetValue("IQBENiteratorType");
			//Set field value for iteratorID
			CustomerQueryRq.iteratorID.SetValue("IQBUUIDType");
			string ORCustomerListQueryElementType7493 = "ListIDList";
			if (ORCustomerListQueryElementType7493 == "ListIDList") {
				//Set field value for ListIDList
				//May create more than one of these if needed
				CustomerQueryRq.ORCustomerListQuery.ListIDList.Add("200000-1011023419");
			}
			if (ORCustomerListQueryElementType7493 == "FullNameList") {
				//Set field value for FullNameList
				//May create more than one of these if needed
				CustomerQueryRq.ORCustomerListQuery.FullNameList.Add("ab");
			}
			if (ORCustomerListQueryElementType7493 == "CustomerListFilter") {
				//Set field value for MaxReturned
				CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.MaxReturned.SetValue(6);
				//Set field value for ActiveStatus
				CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.ActiveStatus.SetValue(ENActiveStatus.asActiveOnly[DEFAULT]);
				//Set field value for FromModifiedDate
				CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.FromModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				//Set field value for ToModifiedDate
				CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.ToModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				string ORNameFilterElementType7494 = "NameFilter";
				if (ORNameFilterElementType7494 == "NameFilter") {
					//Set field value for MatchCriterion
					CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.ORNameFilter.NameFilter.MatchCriterion.SetValue(ENMatchCriterion.mcStartsWith);
					//Set field value for Name
					CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.ORNameFilter.NameFilter.Name.SetValue("ab");
				}
				if (ORNameFilterElementType7494 == "NameRangeFilter") {
					//Set field value for FromName
					CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.ORNameFilter.NameRangeFilter.FromName.SetValue("ab");
					//Set field value for ToName
					CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.ORNameFilter.NameRangeFilter.ToName.SetValue("ab");
				}
				//Set field value for Operator
				CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.TotalBalanceFilter.Operator.SetValue(ENOperator.oLessThan);
				//Set field value for Amount
				CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.TotalBalanceFilter.Amount.SetValue(10.01);
				string ORCurrencyFilterElementType7495 = "ListIDList";
				if (ORCurrencyFilterElementType7495 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.CurrencyFilter.ORCurrencyFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORCurrencyFilterElementType7495 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.CurrencyFilter.ORCurrencyFilter.FullNameList.Add("ab");
				}
				string ORClassFilterElementType7496 = "ListIDList";
				if (ORClassFilterElementType7496 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.ClassFilter.ORClassFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORClassFilterElementType7496 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.ClassFilter.ORClassFilter.FullNameList.Add("ab");
				}
				if (ORClassFilterElementType7496 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.ClassFilter.ORClassFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (ORClassFilterElementType7496 == "FullNameWithChildren") {
					//Set field value for FullNameWithChildren
					CustomerQueryRq.ORCustomerListQuery.CustomerListFilter.ClassFilter.ORClassFilter.FullNameWithChildren.SetValue("ab");
				}
			}
			//Set field value for IncludeRetElementList
			//May create more than one of these if needed
			CustomerQueryRq.IncludeRetElementList.Add("ab");
			//Set field value for OwnerIDList
			//May create more than one of these if needed
			CustomerQueryRq.OwnerIDList.Add(Guid.NewGuid().ToString());
		}

		void WalkCustomerQueryRs(IMsgSetResponse responseMsgSet)
		{
			if (responseMsgSet == null) return;
			IResponseList responseList = responseMsgSet.ResponseList;
			if (responseList == null) return;
			//if we sent only one request, there is only one response, we'll walk the list for this sample
			for (int i = 0; i < responseList.Count; i++) {
				IResponse response = responseList.GetAt(i);
				//check the status code of the response, 0=ok, >0 is warning
				if (response.StatusCode >= 0) {
					//the request-specific response is in the details, make sure we have some
					if (response.Detail != null) {
						//make sure the response is the type we're expecting
						ENResponseType responseType = (ENResponseType)response.Type.GetValue();
						if (responseType == ENResponseType.rtCustomerQueryRs) {
							//upcast to more specific type here, this is safe because we checked with response.Type check above
							ICustomerRetList CustomerRet = (ICustomerRetList)response.Detail;
							WalkCustomerRet(CustomerRet);
						}
					}
				}
			}
		}

		void WalkCustomerRet(ICustomerRetList CustomerRet)
		{
			if (CustomerRet == null) return;
			//Go through all the elements of ICustomerRetList
			//Get value of ListID
			string ListID7497 = (string)CustomerRet.ListID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated7498 = (DateTime)CustomerRet.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified7499 = (DateTime)CustomerRet.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence7500 = (string)CustomerRet.EditSequence.GetValue();
			//Get value of Name
			string Name7501 = (string)CustomerRet.Name.GetValue();
			//Get value of FullName
			string FullName7502 = (string)CustomerRet.FullName.GetValue();
			//Get value of IsActive
			if (CustomerRet.IsActive != null) {
				bool IsActive7503 = (bool)CustomerRet.IsActive.GetValue();
			}
			if (CustomerRet.ClassRef != null) {
				//Get value of ListID
				if (CustomerRet.ClassRef.ListID != null) {
					string ListID7504 = (string)CustomerRet.ClassRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CustomerRet.ClassRef.FullName != null) {
					string FullName7505 = (string)CustomerRet.ClassRef.FullName.GetValue();
				}
			}
			if (CustomerRet.ParentRef != null) {
				//Get value of ListID
				if (CustomerRet.ParentRef.ListID != null) {
					string ListID7506 = (string)CustomerRet.ParentRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CustomerRet.ParentRef.FullName != null) {
					string FullName7507 = (string)CustomerRet.ParentRef.FullName.GetValue();
				}
			}
			//Get value of Sublevel
			int Sublevel7508 = (int)CustomerRet.Sublevel.GetValue();
			//Get value of CompanyName
			if (CustomerRet.CompanyName != null) {
				string CompanyName7509 = (string)CustomerRet.CompanyName.GetValue();
			}
			//Get value of Salutation
			if (CustomerRet.Salutation != null) {
				string Salutation7510 = (string)CustomerRet.Salutation.GetValue();
			}
			//Get value of FirstName
			if (CustomerRet.FirstName != null) {
				string FirstName7511 = (string)CustomerRet.FirstName.GetValue();
			}
			//Get value of MiddleName
			if (CustomerRet.MiddleName != null) {
				string MiddleName7512 = (string)CustomerRet.MiddleName.GetValue();
			}
			//Get value of LastName
			if (CustomerRet.LastName != null) {
				string LastName7513 = (string)CustomerRet.LastName.GetValue();
			}
			//Get value of JobTitle
			if (CustomerRet.JobTitle != null) {
				string JobTitle7514 = (string)CustomerRet.JobTitle.GetValue();
			}
			if (CustomerRet.BillAddress != null) {
				//Get value of Addr1
				if (CustomerRet.BillAddress.Addr1 != null) {
					string Addr17515 = (string)CustomerRet.BillAddress.Addr1.GetValue();
				}
				//Get value of Addr2
				if (CustomerRet.BillAddress.Addr2 != null) {
					string Addr27516 = (string)CustomerRet.BillAddress.Addr2.GetValue();
				}
				//Get value of Addr3
				if (CustomerRet.BillAddress.Addr3 != null) {
					string Addr37517 = (string)CustomerRet.BillAddress.Addr3.GetValue();
				}
				//Get value of Addr4
				if (CustomerRet.BillAddress.Addr4 != null) {
					string Addr47518 = (string)CustomerRet.BillAddress.Addr4.GetValue();
				}
				//Get value of Addr5
				if (CustomerRet.BillAddress.Addr5 != null) {
					string Addr57519 = (string)CustomerRet.BillAddress.Addr5.GetValue();
				}
				//Get value of City
				if (CustomerRet.BillAddress.City != null) {
					string City7520 = (string)CustomerRet.BillAddress.City.GetValue();
				}
				//Get value of State
				if (CustomerRet.BillAddress.State != null) {
					string State7521 = (string)CustomerRet.BillAddress.State.GetValue();
				}
				//Get value of PostalCode
				if (CustomerRet.BillAddress.PostalCode != null) {
					string PostalCode7522 = (string)CustomerRet.BillAddress.PostalCode.GetValue();
				}
				//Get value of Country
				if (CustomerRet.BillAddress.Country != null) {
					string Country7523 = (string)CustomerRet.BillAddress.Country.GetValue();
				}
				//Get value of Note
				if (CustomerRet.BillAddress.Note != null) {
					string Note7524 = (string)CustomerRet.BillAddress.Note.GetValue();
				}
			}
			if (CustomerRet.BillAddressBlock != null) {
				//Get value of Addr1
				if (CustomerRet.BillAddressBlock.Addr1 != null) {
					string Addr17525 = (string)CustomerRet.BillAddressBlock.Addr1.GetValue();
				}
				//Get value of Addr2
				if (CustomerRet.BillAddressBlock.Addr2 != null) {
					string Addr27526 = (string)CustomerRet.BillAddressBlock.Addr2.GetValue();
				}
				//Get value of Addr3
				if (CustomerRet.BillAddressBlock.Addr3 != null) {
					string Addr37527 = (string)CustomerRet.BillAddressBlock.Addr3.GetValue();
				}
				//Get value of Addr4
				if (CustomerRet.BillAddressBlock.Addr4 != null) {
					string Addr47528 = (string)CustomerRet.BillAddressBlock.Addr4.GetValue();
				}
				//Get value of Addr5
				if (CustomerRet.BillAddressBlock.Addr5 != null) {
					string Addr57529 = (string)CustomerRet.BillAddressBlock.Addr5.GetValue();
				}
			}
			if (CustomerRet.ShipAddress != null) {
				//Get value of Addr1
				if (CustomerRet.ShipAddress.Addr1 != null) {
					string Addr17530 = (string)CustomerRet.ShipAddress.Addr1.GetValue();
				}
				//Get value of Addr2
				if (CustomerRet.ShipAddress.Addr2 != null) {
					string Addr27531 = (string)CustomerRet.ShipAddress.Addr2.GetValue();
				}
				//Get value of Addr3
				if (CustomerRet.ShipAddress.Addr3 != null) {
					string Addr37532 = (string)CustomerRet.ShipAddress.Addr3.GetValue();
				}
				//Get value of Addr4
				if (CustomerRet.ShipAddress.Addr4 != null) {
					string Addr47533 = (string)CustomerRet.ShipAddress.Addr4.GetValue();
				}
				//Get value of Addr5
				if (CustomerRet.ShipAddress.Addr5 != null) {
					string Addr57534 = (string)CustomerRet.ShipAddress.Addr5.GetValue();
				}
				//Get value of City
				if (CustomerRet.ShipAddress.City != null) {
					string City7535 = (string)CustomerRet.ShipAddress.City.GetValue();
				}
				//Get value of State
				if (CustomerRet.ShipAddress.State != null) {
					string State7536 = (string)CustomerRet.ShipAddress.State.GetValue();
				}
				//Get value of PostalCode
				if (CustomerRet.ShipAddress.PostalCode != null) {
					string PostalCode7537 = (string)CustomerRet.ShipAddress.PostalCode.GetValue();
				}
				//Get value of Country
				if (CustomerRet.ShipAddress.Country != null) {
					string Country7538 = (string)CustomerRet.ShipAddress.Country.GetValue();
				}
				//Get value of Note
				if (CustomerRet.ShipAddress.Note != null) {
					string Note7539 = (string)CustomerRet.ShipAddress.Note.GetValue();
				}
			}
			if (CustomerRet.ShipAddressBlock != null) {
				//Get value of Addr1
				if (CustomerRet.ShipAddressBlock.Addr1 != null) {
					string Addr17540 = (string)CustomerRet.ShipAddressBlock.Addr1.GetValue();
				}
				//Get value of Addr2
				if (CustomerRet.ShipAddressBlock.Addr2 != null) {
					string Addr27541 = (string)CustomerRet.ShipAddressBlock.Addr2.GetValue();
				}
				//Get value of Addr3
				if (CustomerRet.ShipAddressBlock.Addr3 != null) {
					string Addr37542 = (string)CustomerRet.ShipAddressBlock.Addr3.GetValue();
				}
				//Get value of Addr4
				if (CustomerRet.ShipAddressBlock.Addr4 != null) {
					string Addr47543 = (string)CustomerRet.ShipAddressBlock.Addr4.GetValue();
				}
				//Get value of Addr5
				if (CustomerRet.ShipAddressBlock.Addr5 != null) {
					string Addr57544 = (string)CustomerRet.ShipAddressBlock.Addr5.GetValue();
				}
			}
			if (CustomerRet.ShipToAddressList != null) {
				for (int i7545 = 0; i7545 < CustomerRet.ShipToAddressList.Count; i7545++) {
					IShipToAddress ShipToAddress = CustomerRet.ShipToAddressList.GetAt(i7545);
					//Get value of Name
					string Name7546 = (string)ShipToAddress.Name.GetValue();
					//Get value of Addr1
					if (ShipToAddress.Addr1 != null) {
						string Addr17547 = (string)ShipToAddress.Addr1.GetValue();
					}
					//Get value of Addr2
					if (ShipToAddress.Addr2 != null) {
						string Addr27548 = (string)ShipToAddress.Addr2.GetValue();
					}
					//Get value of Addr3
					if (ShipToAddress.Addr3 != null) {
						string Addr37549 = (string)ShipToAddress.Addr3.GetValue();
					}
					//Get value of Addr4
					if (ShipToAddress.Addr4 != null) {
						string Addr47550 = (string)ShipToAddress.Addr4.GetValue();
					}
					//Get value of Addr5
					if (ShipToAddress.Addr5 != null) {
						string Addr57551 = (string)ShipToAddress.Addr5.GetValue();
					}
					//Get value of City
					if (ShipToAddress.City != null) {
						string City7552 = (string)ShipToAddress.City.GetValue();
					}
					//Get value of State
					if (ShipToAddress.State != null) {
						string State7553 = (string)ShipToAddress.State.GetValue();
					}
					//Get value of PostalCode
					if (ShipToAddress.PostalCode != null) {
						string PostalCode7554 = (string)ShipToAddress.PostalCode.GetValue();
					}
					//Get value of Country
					if (ShipToAddress.Country != null) {
						string Country7555 = (string)ShipToAddress.Country.GetValue();
					}
					//Get value of Note
					if (ShipToAddress.Note != null) {
						string Note7556 = (string)ShipToAddress.Note.GetValue();
					}
					//Get value of DefaultShipTo
					if (ShipToAddress.DefaultShipTo != null) {
						bool DefaultShipTo7557 = (bool)ShipToAddress.DefaultShipTo.GetValue();
					}
				}
			}
			//Get value of Phone
			if (CustomerRet.Phone != null) {
				string Phone7558 = (string)CustomerRet.Phone.GetValue();
			}
			//Get value of AltPhone
			if (CustomerRet.AltPhone != null) {
				string AltPhone7559 = (string)CustomerRet.AltPhone.GetValue();
			}
			//Get value of Fax
			if (CustomerRet.Fax != null) {
				string Fax7560 = (string)CustomerRet.Fax.GetValue();
			}
			//Get value of Email
			if (CustomerRet.Email != null) {
				string Email7561 = (string)CustomerRet.Email.GetValue();
			}
			//Get value of Cc
			if (CustomerRet.Cc != null) {
				string Cc7562 = (string)CustomerRet.Cc.GetValue();
			}
			//Get value of Contact
			if (CustomerRet.Contact != null) {
				string Contact7563 = (string)CustomerRet.Contact.GetValue();
			}
			//Get value of AltContact
			if (CustomerRet.AltContact != null) {
				string AltContact7564 = (string)CustomerRet.AltContact.GetValue();
			}
			if (CustomerRet.AdditionalContactRefList != null) {
				for (int i7565 = 0; i7565 < CustomerRet.AdditionalContactRefList.Count; i7565++) {
					IQBBaseRef QBBaseRef = CustomerRet.AdditionalContactRefList.GetAt(i7565);
					//Get value of ContactName
					string ContactName7566 = (string)QBBaseRef.ContactName.GetValue();
					//Get value of ContactValue
					string ContactValue7567 = (string)QBBaseRef.ContactValue.GetValue();
				}
			}
			if (CustomerRet.ContactsRetList != null) {
				for (int i7568 = 0; i7568 < CustomerRet.ContactsRetList.Count; i7568++) {
					IContactsRet ContactsRet = CustomerRet.ContactsRetList.GetAt(i7568);
					//Get value of ListID
					string ListID7569 = (string)ContactsRet.ListID.GetValue();
					//Get value of TimeCreated
					DateTime TimeCreated7570 = (DateTime)ContactsRet.TimeCreated.GetValue();
					//Get value of TimeModified
					DateTime TimeModified7571 = (DateTime)ContactsRet.TimeModified.GetValue();
					//Get value of EditSequence
					string EditSequence7572 = (string)ContactsRet.EditSequence.GetValue();
					//Get value of Contact
					if (ContactsRet.Contact != null) {
						string Contact7573 = (string)ContactsRet.Contact.GetValue();
					}
					//Get value of Salutation
					if (ContactsRet.Salutation != null) {
						string Salutation7574 = (string)ContactsRet.Salutation.GetValue();
					}
					//Get value of FirstName
					string FirstName7575 = (string)ContactsRet.FirstName.GetValue();
					//Get value of MiddleName
					if (ContactsRet.MiddleName != null) {
						string MiddleName7576 = (string)ContactsRet.MiddleName.GetValue();
					}
					//Get value of LastName
					if (ContactsRet.LastName != null) {
						string LastName7577 = (string)ContactsRet.LastName.GetValue();
					}
					//Get value of JobTitle
					if (ContactsRet.JobTitle != null) {
						string JobTitle7578 = (string)ContactsRet.JobTitle.GetValue();
					}
					if (ContactsRet.AdditionalContactRefList != null) {
						for (int i7579 = 0; i7579 < ContactsRet.AdditionalContactRefList.Count; i7579++) {
							IQBBaseRef QBBaseRef = ContactsRet.AdditionalContactRefList.GetAt(i7579);
							//Get value of ContactName
							string ContactName7580 = (string)QBBaseRef.ContactName.GetValue();
							//Get value of ContactValue
							string ContactValue7581 = (string)QBBaseRef.ContactValue.GetValue();
						}
					}
				}
			}
			if (CustomerRet.CustomerTypeRef != null) {
				//Get value of ListID
				if (CustomerRet.CustomerTypeRef.ListID != null) {
					string ListID7582 = (string)CustomerRet.CustomerTypeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CustomerRet.CustomerTypeRef.FullName != null) {
					string FullName7583 = (string)CustomerRet.CustomerTypeRef.FullName.GetValue();
				}
			}
			if (CustomerRet.TermsRef != null) {
				//Get value of ListID
				if (CustomerRet.TermsRef.ListID != null) {
					string ListID7584 = (string)CustomerRet.TermsRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CustomerRet.TermsRef.FullName != null) {
					string FullName7585 = (string)CustomerRet.TermsRef.FullName.GetValue();
				}
			}
			if (CustomerRet.SalesRepRef != null) {
				//Get value of ListID
				if (CustomerRet.SalesRepRef.ListID != null) {
					string ListID7586 = (string)CustomerRet.SalesRepRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CustomerRet.SalesRepRef.FullName != null) {
					string FullName7587 = (string)CustomerRet.SalesRepRef.FullName.GetValue();
				}
			}
			//Get value of Balance
			if (CustomerRet.Balance != null) {
				double Balance7588 = (double)CustomerRet.Balance.GetValue();
			}
			//Get value of TotalBalance
			if (CustomerRet.TotalBalance != null) {
				double TotalBalance7589 = (double)CustomerRet.TotalBalance.GetValue();
			}
			if (CustomerRet.SalesTaxCodeRef != null) {
				//Get value of ListID
				if (CustomerRet.SalesTaxCodeRef.ListID != null) {
					string ListID7590 = (string)CustomerRet.SalesTaxCodeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CustomerRet.SalesTaxCodeRef.FullName != null) {
					string FullName7591 = (string)CustomerRet.SalesTaxCodeRef.FullName.GetValue();
				}
			}
			if (CustomerRet.ItemSalesTaxRef != null) {
				//Get value of ListID
				if (CustomerRet.ItemSalesTaxRef.ListID != null) {
					string ListID7592 = (string)CustomerRet.ItemSalesTaxRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CustomerRet.ItemSalesTaxRef.FullName != null) {
					string FullName7593 = (string)CustomerRet.ItemSalesTaxRef.FullName.GetValue();
				}
			}
			//Get value of SalesTaxCountry
			if (CustomerRet.SalesTaxCountry != null) {
				ENSalesTaxCountry SalesTaxCountry7594 = (ENSalesTaxCountry)CustomerRet.SalesTaxCountry.GetValue();
			}
			//Get value of ResaleNumber
			if (CustomerRet.ResaleNumber != null) {
				string ResaleNumber7595 = (string)CustomerRet.ResaleNumber.GetValue();
			}
			//Get value of AccountNumber
			if (CustomerRet.AccountNumber != null) {
				string AccountNumber7596 = (string)CustomerRet.AccountNumber.GetValue();
			}
			//Get value of CreditLimit
			if (CustomerRet.CreditLimit != null) {
				double CreditLimit7597 = (double)CustomerRet.CreditLimit.GetValue();
			}
			if (CustomerRet.PreferredPaymentMethodRef != null) {
				//Get value of ListID
				if (CustomerRet.PreferredPaymentMethodRef.ListID != null) {
					string ListID7598 = (string)CustomerRet.PreferredPaymentMethodRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CustomerRet.PreferredPaymentMethodRef.FullName != null) {
					string FullName7599 = (string)CustomerRet.PreferredPaymentMethodRef.FullName.GetValue();
				}
			}
			if (CustomerRet.CreditCardInfo != null) {
				//Get value of CreditCardNumber
				if (CustomerRet.CreditCardInfo.CreditCardNumber != null) {
					string CreditCardNumber7600 = (string)CustomerRet.CreditCardInfo.CreditCardNumber.GetValue();
				}
				//Get value of ExpirationMonth
				if (CustomerRet.CreditCardInfo.ExpirationMonth != null) {
					int ExpirationMonth7601 = (int)CustomerRet.CreditCardInfo.ExpirationMonth.GetValue();
				}
				//Get value of ExpirationYear
				if (CustomerRet.CreditCardInfo.ExpirationYear != null) {
					int ExpirationYear7602 = (int)CustomerRet.CreditCardInfo.ExpirationYear.GetValue();
				}
				//Get value of NameOnCard
				if (CustomerRet.CreditCardInfo.NameOnCard != null) {
					string NameOnCard7603 = (string)CustomerRet.CreditCardInfo.NameOnCard.GetValue();
				}
				//Get value of CreditCardAddress
				if (CustomerRet.CreditCardInfo.CreditCardAddress != null) {
					string CreditCardAddress7604 = (string)CustomerRet.CreditCardInfo.CreditCardAddress.GetValue();
				}
				//Get value of CreditCardPostalCode
				if (CustomerRet.CreditCardInfo.CreditCardPostalCode != null) {
					string CreditCardPostalCode7605 = (string)CustomerRet.CreditCardInfo.CreditCardPostalCode.GetValue();
				}
			}
			//Get value of JobStatus
			if (CustomerRet.JobStatus != null) {
				ENJobStatus JobStatus7606 = (ENJobStatus)CustomerRet.JobStatus.GetValue();
			}
			//Get value of JobStartDate
			if (CustomerRet.JobStartDate != null) {
				DateTime JobStartDate7607 = (DateTime)CustomerRet.JobStartDate.GetValue();
			}
			//Get value of JobProjectedEndDate
			if (CustomerRet.JobProjectedEndDate != null) {
				DateTime JobProjectedEndDate7608 = (DateTime)CustomerRet.JobProjectedEndDate.GetValue();
			}
			//Get value of JobEndDate
			if (CustomerRet.JobEndDate != null) {
				DateTime JobEndDate7609 = (DateTime)CustomerRet.JobEndDate.GetValue();
			}
			//Get value of JobDesc
			if (CustomerRet.JobDesc != null) {
				string JobDesc7610 = (string)CustomerRet.JobDesc.GetValue();
			}
			if (CustomerRet.JobTypeRef != null) {
				//Get value of ListID
				if (CustomerRet.JobTypeRef.ListID != null) {
					string ListID7611 = (string)CustomerRet.JobTypeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CustomerRet.JobTypeRef.FullName != null) {
					string FullName7612 = (string)CustomerRet.JobTypeRef.FullName.GetValue();
				}
			}
			//Get value of Notes
			if (CustomerRet.Notes != null) {
				string Notes7613 = (string)CustomerRet.Notes.GetValue();
			}
			if (CustomerRet.AdditionalNotesRetList != null) {
				for (int i7614 = 0; i7614 < CustomerRet.AdditionalNotesRetList.Count; i7614++) {
					IAdditionalNotesRet AdditionalNotesRet = CustomerRet.AdditionalNotesRetList.GetAt(i7614);
					//Get value of NoteID
					int NoteID7615 = (int)AdditionalNotesRet.NoteID.GetValue();
					//Get value of Date
					DateTime Date7616 = (DateTime)AdditionalNotesRet.Date.GetValue();
					//Get value of Note
					string Note7617 = (string)AdditionalNotesRet.Note.GetValue();
				}
			}
			//Get value of PreferredDeliveryMethod
			if (CustomerRet.PreferredDeliveryMethod != null) {
				ENPreferredDeliveryMethod PreferredDeliveryMethod7618 = (ENPreferredDeliveryMethod)CustomerRet.PreferredDeliveryMethod.GetValue();
			}
			if (CustomerRet.PriceLevelRef != null) {
				//Get value of ListID
				if (CustomerRet.PriceLevelRef.ListID != null) {
					string ListID7619 = (string)CustomerRet.PriceLevelRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CustomerRet.PriceLevelRef.FullName != null) {
					string FullName7620 = (string)CustomerRet.PriceLevelRef.FullName.GetValue();
				}
			}
			//Get value of ExternalGUID
			if (CustomerRet.ExternalGUID != null) {
				string ExternalGUID7621 = (string)CustomerRet.ExternalGUID.GetValue();
			}
			//Get value of TaxRegistrationNumber
			if (CustomerRet.TaxRegistrationNumber != null) {
				string TaxRegistrationNumber7622 = (string)CustomerRet.TaxRegistrationNumber.GetValue();
			}
			if (CustomerRet.CurrencyRef != null) {
				//Get value of ListID
				if (CustomerRet.CurrencyRef.ListID != null) {
					string ListID7623 = (string)CustomerRet.CurrencyRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CustomerRet.CurrencyRef.FullName != null) {
					string FullName7624 = (string)CustomerRet.CurrencyRef.FullName.GetValue();
				}
			}
			if (CustomerRet.DataExtRetList != null) {
				for (int i7625 = 0; i7625 < CustomerRet.DataExtRetList.Count; i7625++) {
					IDataExtRet DataExtRet = CustomerRet.DataExtRetList.GetAt(i7625);
					//Get value of OwnerID
					if (DataExtRet.OwnerID != null) {
						string OwnerID7626 = (string)DataExtRet.OwnerID.GetValue();
					}
					//Get value of DataExtName
					string DataExtName7627 = (string)DataExtRet.DataExtName.GetValue();
					//Get value of DataExtType
					ENDataExtType DataExtType7628 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
					//Get value of DataExtValue
					string DataExtValue7629 = (string)DataExtRet.DataExtValue.GetValue();
				}
			}
		}
	}
}