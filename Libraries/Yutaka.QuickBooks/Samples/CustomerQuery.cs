using System;
using Interop.QBFC13;

namespace com.intuit.idn.samples
{
	public partial class Sample
	{
		public void DoQuery()
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

				BuildQueryRq(requestMsgSet);

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

				WalkQueryRs(responseMsgSet);
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

		void BuildQueryRq(IMsgSetRequest requestMsgSet)
		{
			IQuery QueryRq= requestMsgSet.AppendQueryRq();
			//Set attributes
			//Set field value for metaData
			QueryRq.metaData.SetValue("IQBENmetaDataType");
			//Set field value for iterator
			QueryRq.iterator.SetValue("IQBENiteratorType");
			//Set field value for iteratorID
			QueryRq.iteratorID.SetValue("IQBUUIDType");
			string ORListQueryElementType7493 = "ListIDList";
			if (ORListQueryElementType7493 == "ListIDList") {
				//Set field value for ListIDList
				//May create more than one of these if needed
				QueryRq.ORListQuery.ListIDList.Add("200000-1011023419");
			}
			if (ORListQueryElementType7493 == "FullNameList") {
				//Set field value for FullNameList
				//May create more than one of these if needed
				QueryRq.ORListQuery.FullNameList.Add("ab");
			}
			if (ORListQueryElementType7493 == "ListFilter") {
				//Set field value for MaxReturned
				QueryRq.ORListQuery.ListFilter.MaxReturned.SetValue(6);
				//Set field value for ActiveStatus
				QueryRq.ORListQuery.ListFilter.ActiveStatus.SetValue(ENActiveStatus.asActiveOnly[DEFAULT]);
				//Set field value for FromModifiedDate
				QueryRq.ORListQuery.ListFilter.FromModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				//Set field value for ToModifiedDate
				QueryRq.ORListQuery.ListFilter.ToModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				string ORNameFilterElementType7494 = "NameFilter";
				if (ORNameFilterElementType7494 == "NameFilter") {
					//Set field value for MatchCriterion
					QueryRq.ORListQuery.ListFilter.ORNameFilter.NameFilter.MatchCriterion.SetValue(ENMatchCriterion.mcStartsWith);
					//Set field value for Name
					QueryRq.ORListQuery.ListFilter.ORNameFilter.NameFilter.Name.SetValue("ab");
				}
				if (ORNameFilterElementType7494 == "NameRangeFilter") {
					//Set field value for FromName
					QueryRq.ORListQuery.ListFilter.ORNameFilter.NameRangeFilter.FromName.SetValue("ab");
					//Set field value for ToName
					QueryRq.ORListQuery.ListFilter.ORNameFilter.NameRangeFilter.ToName.SetValue("ab");
				}
				//Set field value for Operator
				QueryRq.ORListQuery.ListFilter.TotalBalanceFilter.Operator.SetValue(ENOperator.oLessThan);
				//Set field value for Amount
				QueryRq.ORListQuery.ListFilter.TotalBalanceFilter.Amount.SetValue(10.01);
				string ORCurrencyFilterElementType7495 = "ListIDList";
				if (ORCurrencyFilterElementType7495 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					QueryRq.ORListQuery.ListFilter.CurrencyFilter.ORCurrencyFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORCurrencyFilterElementType7495 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					QueryRq.ORListQuery.ListFilter.CurrencyFilter.ORCurrencyFilter.FullNameList.Add("ab");
				}
				string ORClassFilterElementType7496 = "ListIDList";
				if (ORClassFilterElementType7496 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					QueryRq.ORListQuery.ListFilter.ClassFilter.ORClassFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORClassFilterElementType7496 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					QueryRq.ORListQuery.ListFilter.ClassFilter.ORClassFilter.FullNameList.Add("ab");
				}
				if (ORClassFilterElementType7496 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					QueryRq.ORListQuery.ListFilter.ClassFilter.ORClassFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (ORClassFilterElementType7496 == "FullNameWithChildren") {
					//Set field value for FullNameWithChildren
					QueryRq.ORListQuery.ListFilter.ClassFilter.ORClassFilter.FullNameWithChildren.SetValue("ab");
				}
			}
			//Set field value for IncludeRetElementList
			//May create more than one of these if needed
			QueryRq.IncludeRetElementList.Add("ab");
			//Set field value for OwnerIDList
			//May create more than one of these if needed
			QueryRq.OwnerIDList.Add(Guid.NewGuid().ToString());
		}

		void WalkQueryRs(IMsgSetResponse responseMsgSet)
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
						if (responseType == ENResponseType.rtQueryRs) {
							//upcast to more specific type here, this is safe because we checked with response.Type check above
							IRetList Ret = (IRetList)response.Detail;
							WalkRet(Ret);
						}
					}
				}
			}
		}

		void WalkRet(IRetList Ret)
		{
			if (Ret == null) return;
			//Go through all the elements of IRetList
			//Get value of ListID
			string ListID7497 = (string)Ret.ListID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated7498 = (DateTime)Ret.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified7499 = (DateTime)Ret.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence7500 = (string)Ret.EditSequence.GetValue();
			//Get value of Name
			string Name7501 = (string)Ret.Name.GetValue();
			//Get value of FullName
			string FullName7502 = (string)Ret.FullName.GetValue();
			//Get value of IsActive
			if (Ret.IsActive != null) {
				bool IsActive7503 = (bool)Ret.IsActive.GetValue();
			}
			if (Ret.ClassRef != null) {
				//Get value of ListID
				if (Ret.ClassRef.ListID != null) {
					string ListID7504 = (string)Ret.ClassRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ClassRef.FullName != null) {
					string FullName7505 = (string)Ret.ClassRef.FullName.GetValue();
				}
			}
			if (Ret.ParentRef != null) {
				//Get value of ListID
				if (Ret.ParentRef.ListID != null) {
					string ListID7506 = (string)Ret.ParentRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ParentRef.FullName != null) {
					string FullName7507 = (string)Ret.ParentRef.FullName.GetValue();
				}
			}
			//Get value of Sublevel
			int Sublevel7508 = (int)Ret.Sublevel.GetValue();
			//Get value of CompanyName
			if (Ret.CompanyName != null) {
				string CompanyName7509 = (string)Ret.CompanyName.GetValue();
			}
			//Get value of Salutation
			if (Ret.Salutation != null) {
				string Salutation7510 = (string)Ret.Salutation.GetValue();
			}
			//Get value of FirstName
			if (Ret.FirstName != null) {
				string FirstName7511 = (string)Ret.FirstName.GetValue();
			}
			//Get value of MiddleName
			if (Ret.MiddleName != null) {
				string MiddleName7512 = (string)Ret.MiddleName.GetValue();
			}
			//Get value of LastName
			if (Ret.LastName != null) {
				string LastName7513 = (string)Ret.LastName.GetValue();
			}
			//Get value of JobTitle
			if (Ret.JobTitle != null) {
				string JobTitle7514 = (string)Ret.JobTitle.GetValue();
			}
			if (Ret.BillAddress != null) {
				//Get value of Addr1
				if (Ret.BillAddress.Addr1 != null) {
					string Addr17515 = (string)Ret.BillAddress.Addr1.GetValue();
				}
				//Get value of Addr2
				if (Ret.BillAddress.Addr2 != null) {
					string Addr27516 = (string)Ret.BillAddress.Addr2.GetValue();
				}
				//Get value of Addr3
				if (Ret.BillAddress.Addr3 != null) {
					string Addr37517 = (string)Ret.BillAddress.Addr3.GetValue();
				}
				//Get value of Addr4
				if (Ret.BillAddress.Addr4 != null) {
					string Addr47518 = (string)Ret.BillAddress.Addr4.GetValue();
				}
				//Get value of Addr5
				if (Ret.BillAddress.Addr5 != null) {
					string Addr57519 = (string)Ret.BillAddress.Addr5.GetValue();
				}
				//Get value of City
				if (Ret.BillAddress.City != null) {
					string City7520 = (string)Ret.BillAddress.City.GetValue();
				}
				//Get value of State
				if (Ret.BillAddress.State != null) {
					string State7521 = (string)Ret.BillAddress.State.GetValue();
				}
				//Get value of PostalCode
				if (Ret.BillAddress.PostalCode != null) {
					string PostalCode7522 = (string)Ret.BillAddress.PostalCode.GetValue();
				}
				//Get value of Country
				if (Ret.BillAddress.Country != null) {
					string Country7523 = (string)Ret.BillAddress.Country.GetValue();
				}
				//Get value of Note
				if (Ret.BillAddress.Note != null) {
					string Note7524 = (string)Ret.BillAddress.Note.GetValue();
				}
			}
			if (Ret.BillAddressBlock != null) {
				//Get value of Addr1
				if (Ret.BillAddressBlock.Addr1 != null) {
					string Addr17525 = (string)Ret.BillAddressBlock.Addr1.GetValue();
				}
				//Get value of Addr2
				if (Ret.BillAddressBlock.Addr2 != null) {
					string Addr27526 = (string)Ret.BillAddressBlock.Addr2.GetValue();
				}
				//Get value of Addr3
				if (Ret.BillAddressBlock.Addr3 != null) {
					string Addr37527 = (string)Ret.BillAddressBlock.Addr3.GetValue();
				}
				//Get value of Addr4
				if (Ret.BillAddressBlock.Addr4 != null) {
					string Addr47528 = (string)Ret.BillAddressBlock.Addr4.GetValue();
				}
				//Get value of Addr5
				if (Ret.BillAddressBlock.Addr5 != null) {
					string Addr57529 = (string)Ret.BillAddressBlock.Addr5.GetValue();
				}
			}
			if (Ret.ShipAddress != null) {
				//Get value of Addr1
				if (Ret.ShipAddress.Addr1 != null) {
					string Addr17530 = (string)Ret.ShipAddress.Addr1.GetValue();
				}
				//Get value of Addr2
				if (Ret.ShipAddress.Addr2 != null) {
					string Addr27531 = (string)Ret.ShipAddress.Addr2.GetValue();
				}
				//Get value of Addr3
				if (Ret.ShipAddress.Addr3 != null) {
					string Addr37532 = (string)Ret.ShipAddress.Addr3.GetValue();
				}
				//Get value of Addr4
				if (Ret.ShipAddress.Addr4 != null) {
					string Addr47533 = (string)Ret.ShipAddress.Addr4.GetValue();
				}
				//Get value of Addr5
				if (Ret.ShipAddress.Addr5 != null) {
					string Addr57534 = (string)Ret.ShipAddress.Addr5.GetValue();
				}
				//Get value of City
				if (Ret.ShipAddress.City != null) {
					string City7535 = (string)Ret.ShipAddress.City.GetValue();
				}
				//Get value of State
				if (Ret.ShipAddress.State != null) {
					string State7536 = (string)Ret.ShipAddress.State.GetValue();
				}
				//Get value of PostalCode
				if (Ret.ShipAddress.PostalCode != null) {
					string PostalCode7537 = (string)Ret.ShipAddress.PostalCode.GetValue();
				}
				//Get value of Country
				if (Ret.ShipAddress.Country != null) {
					string Country7538 = (string)Ret.ShipAddress.Country.GetValue();
				}
				//Get value of Note
				if (Ret.ShipAddress.Note != null) {
					string Note7539 = (string)Ret.ShipAddress.Note.GetValue();
				}
			}
			if (Ret.ShipAddressBlock != null) {
				//Get value of Addr1
				if (Ret.ShipAddressBlock.Addr1 != null) {
					string Addr17540 = (string)Ret.ShipAddressBlock.Addr1.GetValue();
				}
				//Get value of Addr2
				if (Ret.ShipAddressBlock.Addr2 != null) {
					string Addr27541 = (string)Ret.ShipAddressBlock.Addr2.GetValue();
				}
				//Get value of Addr3
				if (Ret.ShipAddressBlock.Addr3 != null) {
					string Addr37542 = (string)Ret.ShipAddressBlock.Addr3.GetValue();
				}
				//Get value of Addr4
				if (Ret.ShipAddressBlock.Addr4 != null) {
					string Addr47543 = (string)Ret.ShipAddressBlock.Addr4.GetValue();
				}
				//Get value of Addr5
				if (Ret.ShipAddressBlock.Addr5 != null) {
					string Addr57544 = (string)Ret.ShipAddressBlock.Addr5.GetValue();
				}
			}
			if (Ret.ShipToAddressList != null) {
				for (int i7545 = 0; i7545 < Ret.ShipToAddressList.Count; i7545++) {
					IShipToAddress ShipToAddress = Ret.ShipToAddressList.GetAt(i7545);
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
			if (Ret.Phone != null) {
				string Phone7558 = (string)Ret.Phone.GetValue();
			}
			//Get value of AltPhone
			if (Ret.AltPhone != null) {
				string AltPhone7559 = (string)Ret.AltPhone.GetValue();
			}
			//Get value of Fax
			if (Ret.Fax != null) {
				string Fax7560 = (string)Ret.Fax.GetValue();
			}
			//Get value of Email
			if (Ret.Email != null) {
				string Email7561 = (string)Ret.Email.GetValue();
			}
			//Get value of Cc
			if (Ret.Cc != null) {
				string Cc7562 = (string)Ret.Cc.GetValue();
			}
			//Get value of Contact
			if (Ret.Contact != null) {
				string Contact7563 = (string)Ret.Contact.GetValue();
			}
			//Get value of AltContact
			if (Ret.AltContact != null) {
				string AltContact7564 = (string)Ret.AltContact.GetValue();
			}
			if (Ret.AdditionalContactRefList != null) {
				for (int i7565 = 0; i7565 < Ret.AdditionalContactRefList.Count; i7565++) {
					IQBBaseRef QBBaseRef = Ret.AdditionalContactRefList.GetAt(i7565);
					//Get value of ContactName
					string ContactName7566 = (string)QBBaseRef.ContactName.GetValue();
					//Get value of ContactValue
					string ContactValue7567 = (string)QBBaseRef.ContactValue.GetValue();
				}
			}
			if (Ret.ContactsRetList != null) {
				for (int i7568 = 0; i7568 < Ret.ContactsRetList.Count; i7568++) {
					IContactsRet ContactsRet = Ret.ContactsRetList.GetAt(i7568);
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
			if (Ret.TypeRef != null) {
				//Get value of ListID
				if (Ret.TypeRef.ListID != null) {
					string ListID7582 = (string)Ret.TypeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.TypeRef.FullName != null) {
					string FullName7583 = (string)Ret.TypeRef.FullName.GetValue();
				}
			}
			if (Ret.TermsRef != null) {
				//Get value of ListID
				if (Ret.TermsRef.ListID != null) {
					string ListID7584 = (string)Ret.TermsRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.TermsRef.FullName != null) {
					string FullName7585 = (string)Ret.TermsRef.FullName.GetValue();
				}
			}
			if (Ret.SalesRepRef != null) {
				//Get value of ListID
				if (Ret.SalesRepRef.ListID != null) {
					string ListID7586 = (string)Ret.SalesRepRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.SalesRepRef.FullName != null) {
					string FullName7587 = (string)Ret.SalesRepRef.FullName.GetValue();
				}
			}
			//Get value of Balance
			if (Ret.Balance != null) {
				double Balance7588 = (double)Ret.Balance.GetValue();
			}
			//Get value of TotalBalance
			if (Ret.TotalBalance != null) {
				double TotalBalance7589 = (double)Ret.TotalBalance.GetValue();
			}
			if (Ret.SalesTaxCodeRef != null) {
				//Get value of ListID
				if (Ret.SalesTaxCodeRef.ListID != null) {
					string ListID7590 = (string)Ret.SalesTaxCodeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.SalesTaxCodeRef.FullName != null) {
					string FullName7591 = (string)Ret.SalesTaxCodeRef.FullName.GetValue();
				}
			}
			if (Ret.ItemSalesTaxRef != null) {
				//Get value of ListID
				if (Ret.ItemSalesTaxRef.ListID != null) {
					string ListID7592 = (string)Ret.ItemSalesTaxRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ItemSalesTaxRef.FullName != null) {
					string FullName7593 = (string)Ret.ItemSalesTaxRef.FullName.GetValue();
				}
			}
			//Get value of SalesTaxCountry
			if (Ret.SalesTaxCountry != null) {
				ENSalesTaxCountry SalesTaxCountry7594 = (ENSalesTaxCountry)Ret.SalesTaxCountry.GetValue();
			}
			//Get value of ResaleNumber
			if (Ret.ResaleNumber != null) {
				string ResaleNumber7595 = (string)Ret.ResaleNumber.GetValue();
			}
			//Get value of AccountNumber
			if (Ret.AccountNumber != null) {
				string AccountNumber7596 = (string)Ret.AccountNumber.GetValue();
			}
			//Get value of CreditLimit
			if (Ret.CreditLimit != null) {
				double CreditLimit7597 = (double)Ret.CreditLimit.GetValue();
			}
			if (Ret.PreferredPaymentMethodRef != null) {
				//Get value of ListID
				if (Ret.PreferredPaymentMethodRef.ListID != null) {
					string ListID7598 = (string)Ret.PreferredPaymentMethodRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.PreferredPaymentMethodRef.FullName != null) {
					string FullName7599 = (string)Ret.PreferredPaymentMethodRef.FullName.GetValue();
				}
			}
			if (Ret.CreditCardInfo != null) {
				//Get value of CreditCardNumber
				if (Ret.CreditCardInfo.CreditCardNumber != null) {
					string CreditCardNumber7600 = (string)Ret.CreditCardInfo.CreditCardNumber.GetValue();
				}
				//Get value of ExpirationMonth
				if (Ret.CreditCardInfo.ExpirationMonth != null) {
					int ExpirationMonth7601 = (int)Ret.CreditCardInfo.ExpirationMonth.GetValue();
				}
				//Get value of ExpirationYear
				if (Ret.CreditCardInfo.ExpirationYear != null) {
					int ExpirationYear7602 = (int)Ret.CreditCardInfo.ExpirationYear.GetValue();
				}
				//Get value of NameOnCard
				if (Ret.CreditCardInfo.NameOnCard != null) {
					string NameOnCard7603 = (string)Ret.CreditCardInfo.NameOnCard.GetValue();
				}
				//Get value of CreditCardAddress
				if (Ret.CreditCardInfo.CreditCardAddress != null) {
					string CreditCardAddress7604 = (string)Ret.CreditCardInfo.CreditCardAddress.GetValue();
				}
				//Get value of CreditCardPostalCode
				if (Ret.CreditCardInfo.CreditCardPostalCode != null) {
					string CreditCardPostalCode7605 = (string)Ret.CreditCardInfo.CreditCardPostalCode.GetValue();
				}
			}
			//Get value of JobStatus
			if (Ret.JobStatus != null) {
				ENJobStatus JobStatus7606 = (ENJobStatus)Ret.JobStatus.GetValue();
			}
			//Get value of JobStartDate
			if (Ret.JobStartDate != null) {
				DateTime JobStartDate7607 = (DateTime)Ret.JobStartDate.GetValue();
			}
			//Get value of JobProjectedEndDate
			if (Ret.JobProjectedEndDate != null) {
				DateTime JobProjectedEndDate7608 = (DateTime)Ret.JobProjectedEndDate.GetValue();
			}
			//Get value of JobEndDate
			if (Ret.JobEndDate != null) {
				DateTime JobEndDate7609 = (DateTime)Ret.JobEndDate.GetValue();
			}
			//Get value of JobDesc
			if (Ret.JobDesc != null) {
				string JobDesc7610 = (string)Ret.JobDesc.GetValue();
			}
			if (Ret.JobTypeRef != null) {
				//Get value of ListID
				if (Ret.JobTypeRef.ListID != null) {
					string ListID7611 = (string)Ret.JobTypeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.JobTypeRef.FullName != null) {
					string FullName7612 = (string)Ret.JobTypeRef.FullName.GetValue();
				}
			}
			//Get value of Notes
			if (Ret.Notes != null) {
				string Notes7613 = (string)Ret.Notes.GetValue();
			}
			if (Ret.AdditionalNotesRetList != null) {
				for (int i7614 = 0; i7614 < Ret.AdditionalNotesRetList.Count; i7614++) {
					IAdditionalNotesRet AdditionalNotesRet = Ret.AdditionalNotesRetList.GetAt(i7614);
					//Get value of NoteID
					int NoteID7615 = (int)AdditionalNotesRet.NoteID.GetValue();
					//Get value of Date
					DateTime Date7616 = (DateTime)AdditionalNotesRet.Date.GetValue();
					//Get value of Note
					string Note7617 = (string)AdditionalNotesRet.Note.GetValue();
				}
			}
			//Get value of PreferredDeliveryMethod
			if (Ret.PreferredDeliveryMethod != null) {
				ENPreferredDeliveryMethod PreferredDeliveryMethod7618 = (ENPreferredDeliveryMethod)Ret.PreferredDeliveryMethod.GetValue();
			}
			if (Ret.PriceLevelRef != null) {
				//Get value of ListID
				if (Ret.PriceLevelRef.ListID != null) {
					string ListID7619 = (string)Ret.PriceLevelRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.PriceLevelRef.FullName != null) {
					string FullName7620 = (string)Ret.PriceLevelRef.FullName.GetValue();
				}
			}
			//Get value of ExternalGUID
			if (Ret.ExternalGUID != null) {
				string ExternalGUID7621 = (string)Ret.ExternalGUID.GetValue();
			}
			//Get value of TaxRegistrationNumber
			if (Ret.TaxRegistrationNumber != null) {
				string TaxRegistrationNumber7622 = (string)Ret.TaxRegistrationNumber.GetValue();
			}
			if (Ret.CurrencyRef != null) {
				//Get value of ListID
				if (Ret.CurrencyRef.ListID != null) {
					string ListID7623 = (string)Ret.CurrencyRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.CurrencyRef.FullName != null) {
					string FullName7624 = (string)Ret.CurrencyRef.FullName.GetValue();
				}
			}
			if (Ret.DataExtRetList != null) {
				for (int i7625 = 0; i7625 < Ret.DataExtRetList.Count; i7625++) {
					IDataExtRet DataExtRet = Ret.DataExtRetList.GetAt(i7625);
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