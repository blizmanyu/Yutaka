using System;
using Interop.QBFC13;

namespace com.intuit.idn.samples
{
	public partial class Sample
	{
		public void DoCreditMemoQuery()
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

				BuildCreditMemoQueryRq(requestMsgSet);

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

				WalkCreditMemoQueryRs(responseMsgSet);
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

		void BuildCreditMemoQueryRq(IMsgSetRequest requestMsgSet)
		{
			ICreditMemoQuery CreditMemoQueryRq= requestMsgSet.AppendCreditMemoQueryRq();
			//Set attributes
			//Set field value for metaData
			CreditMemoQueryRq.metaData.SetValue("IQBENmetaDataType");
			//Set field value for iterator
			CreditMemoQueryRq.iterator.SetValue("IQBENiteratorType");
			//Set field value for iteratorID
			CreditMemoQueryRq.iteratorID.SetValue("IQBUUIDType");
			string ORTxnQueryElementType6301 = "TxnIDList";
			if (ORTxnQueryElementType6301 == "TxnIDList") {
				//Set field value for TxnIDList
				//May create more than one of these if needed
				CreditMemoQueryRq.ORTxnQuery.TxnIDList.Add("200000-1011023419");
			}
			if (ORTxnQueryElementType6301 == "RefNumberList") {
				//Set field value for RefNumberList
				//May create more than one of these if needed
				CreditMemoQueryRq.ORTxnQuery.RefNumberList.Add("ab");
			}
			if (ORTxnQueryElementType6301 == "RefNumberCaseSensitiveList") {
				//Set field value for RefNumberCaseSensitiveList
				//May create more than one of these if needed
				CreditMemoQueryRq.ORTxnQuery.RefNumberCaseSensitiveList.Add("ab");
			}
			if (ORTxnQueryElementType6301 == "TxnFilter") {
				//Set field value for MaxReturned
				CreditMemoQueryRq.ORTxnQuery.TxnFilter.MaxReturned.SetValue(6);
				string ORDateRangeFilterElementType6302 = "ModifiedDateRangeFilter";
				if (ORDateRangeFilterElementType6302 == "ModifiedDateRangeFilter") {
					//Set field value for FromModifiedDate
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.ModifiedDateRangeFilter.FromModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
					//Set field value for ToModifiedDate
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.ModifiedDateRangeFilter.ToModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				}
				if (ORDateRangeFilterElementType6302 == "TxnDateRangeFilter") {
					string ORTxnDateRangeFilterElementType6303 = "TxnDateFilter";
					if (ORTxnDateRangeFilterElementType6303 == "TxnDateFilter") {
						//Set field value for FromTxnDate
						CreditMemoQueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.FromTxnDate.SetValue(DateTime.Parse("12/15/2007"));
						//Set field value for ToTxnDate
						CreditMemoQueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.ToTxnDate.SetValue(DateTime.Parse("12/15/2007"));
					}
					if (ORTxnDateRangeFilterElementType6303 == "DateMacro") {
						//Set field value for DateMacro
						CreditMemoQueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.DateMacro.SetValue(ENDateMacro.dmAll);
					}
				}
				string OREntityFilterElementType6304 = "ListIDList";
				if (OREntityFilterElementType6304 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.ListIDList.Add("200000-1011023419");
				}
				if (OREntityFilterElementType6304 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.FullNameList.Add("ab");
				}
				if (OREntityFilterElementType6304 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (OREntityFilterElementType6304 == "FullNameWithChildren") {
					//Set field value for FullNameWithChildren
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.FullNameWithChildren.SetValue("ab");
				}
				string ORAccountFilterElementType6305 = "ListIDList";
				if (ORAccountFilterElementType6305 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORAccountFilterElementType6305 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.FullNameList.Add("ab");
				}
				if (ORAccountFilterElementType6305 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (ORAccountFilterElementType6305 == "FullNameWithChildren") {
					//Set field value for FullNameWithChildren
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.FullNameWithChildren.SetValue("ab");
				}
				string ORRefNumberFilterElementType6306 = "RefNumberFilter";
				if (ORRefNumberFilterElementType6306 == "RefNumberFilter") {
					//Set field value for MatchCriterion
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberFilter.MatchCriterion.SetValue(ENMatchCriterion.mcStartsWith);
					//Set field value for RefNumber
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberFilter.RefNumber.SetValue("ab");
				}
				if (ORRefNumberFilterElementType6306 == "RefNumberRangeFilter") {
					//Set field value for FromRefNumber
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberRangeFilter.FromRefNumber.SetValue("ab");
					//Set field value for ToRefNumber
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberRangeFilter.ToRefNumber.SetValue("ab");
				}
				string ORCurrencyFilterElementType6307 = "ListIDList";
				if (ORCurrencyFilterElementType6307 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.CurrencyFilter.ORCurrencyFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORCurrencyFilterElementType6307 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					CreditMemoQueryRq.ORTxnQuery.TxnFilter.CurrencyFilter.ORCurrencyFilter.FullNameList.Add("ab");
				}
			}
			//Set field value for IncludeLineItems
			CreditMemoQueryRq.IncludeLineItems.SetValue(true);
			//Set field value for IncludeLinkedTxns
			CreditMemoQueryRq.IncludeLinkedTxns.SetValue(true);
			//Set field value for IncludeRetElementList
			//May create more than one of these if needed
			CreditMemoQueryRq.IncludeRetElementList.Add("ab");
			//Set field value for OwnerIDList
			//May create more than one of these if needed
			CreditMemoQueryRq.OwnerIDList.Add(Guid.NewGuid().ToString());
		}

		void WalkCreditMemoQueryRs(IMsgSetResponse responseMsgSet)
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
						if (responseType == ENResponseType.rtCreditMemoQueryRs) {
							//upcast to more specific type here, this is safe because we checked with response.Type check above
							ICreditMemoRetList CreditMemoRet = (ICreditMemoRetList)response.Detail;
							WalkCreditMemoRet(CreditMemoRet);
						}
					}
				}
			}
		}

		void WalkCreditMemoRet(ICreditMemoRetList CreditMemoRet)
		{
			if (CreditMemoRet == null) return;
			//Go through all the elements of ICreditMemoRetList
			//Get value of TxnID
			string TxnID6308 = (string)CreditMemoRet.TxnID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated6309 = (DateTime)CreditMemoRet.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified6310 = (DateTime)CreditMemoRet.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence6311 = (string)CreditMemoRet.EditSequence.GetValue();
			//Get value of TxnNumber
			if (CreditMemoRet.TxnNumber != null) {
				int TxnNumber6312 = (int)CreditMemoRet.TxnNumber.GetValue();
			}
			//Get value of ListID
			if (CreditMemoRet.CustomerRef.ListID != null) {
				string ListID6313 = (string)CreditMemoRet.CustomerRef.ListID.GetValue();
			}
			//Get value of FullName
			if (CreditMemoRet.CustomerRef.FullName != null) {
				string FullName6314 = (string)CreditMemoRet.CustomerRef.FullName.GetValue();
			}
			if (CreditMemoRet.ClassRef != null) {
				//Get value of ListID
				if (CreditMemoRet.ClassRef.ListID != null) {
					string ListID6315 = (string)CreditMemoRet.ClassRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CreditMemoRet.ClassRef.FullName != null) {
					string FullName6316 = (string)CreditMemoRet.ClassRef.FullName.GetValue();
				}
			}
			if (CreditMemoRet.ARAccountRef != null) {
				//Get value of ListID
				if (CreditMemoRet.ARAccountRef.ListID != null) {
					string ListID6317 = (string)CreditMemoRet.ARAccountRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CreditMemoRet.ARAccountRef.FullName != null) {
					string FullName6318 = (string)CreditMemoRet.ARAccountRef.FullName.GetValue();
				}
			}
			if (CreditMemoRet.TemplateRef != null) {
				//Get value of ListID
				if (CreditMemoRet.TemplateRef.ListID != null) {
					string ListID6319 = (string)CreditMemoRet.TemplateRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CreditMemoRet.TemplateRef.FullName != null) {
					string FullName6320 = (string)CreditMemoRet.TemplateRef.FullName.GetValue();
				}
			}
			//Get value of TxnDate
			DateTime TxnDate6321 = (DateTime)CreditMemoRet.TxnDate.GetValue();
			//Get value of RefNumber
			if (CreditMemoRet.RefNumber != null) {
				string RefNumber6322 = (string)CreditMemoRet.RefNumber.GetValue();
			}
			if (CreditMemoRet.BillAddress != null) {
				//Get value of Addr1
				if (CreditMemoRet.BillAddress.Addr1 != null) {
					string Addr16323 = (string)CreditMemoRet.BillAddress.Addr1.GetValue();
				}
				//Get value of Addr2
				if (CreditMemoRet.BillAddress.Addr2 != null) {
					string Addr26324 = (string)CreditMemoRet.BillAddress.Addr2.GetValue();
				}
				//Get value of Addr3
				if (CreditMemoRet.BillAddress.Addr3 != null) {
					string Addr36325 = (string)CreditMemoRet.BillAddress.Addr3.GetValue();
				}
				//Get value of Addr4
				if (CreditMemoRet.BillAddress.Addr4 != null) {
					string Addr46326 = (string)CreditMemoRet.BillAddress.Addr4.GetValue();
				}
				//Get value of Addr5
				if (CreditMemoRet.BillAddress.Addr5 != null) {
					string Addr56327 = (string)CreditMemoRet.BillAddress.Addr5.GetValue();
				}
				//Get value of City
				if (CreditMemoRet.BillAddress.City != null) {
					string City6328 = (string)CreditMemoRet.BillAddress.City.GetValue();
				}
				//Get value of State
				if (CreditMemoRet.BillAddress.State != null) {
					string State6329 = (string)CreditMemoRet.BillAddress.State.GetValue();
				}
				//Get value of PostalCode
				if (CreditMemoRet.BillAddress.PostalCode != null) {
					string PostalCode6330 = (string)CreditMemoRet.BillAddress.PostalCode.GetValue();
				}
				//Get value of Country
				if (CreditMemoRet.BillAddress.Country != null) {
					string Country6331 = (string)CreditMemoRet.BillAddress.Country.GetValue();
				}
				//Get value of Note
				if (CreditMemoRet.BillAddress.Note != null) {
					string Note6332 = (string)CreditMemoRet.BillAddress.Note.GetValue();
				}
			}
			if (CreditMemoRet.BillAddressBlock != null) {
				//Get value of Addr1
				if (CreditMemoRet.BillAddressBlock.Addr1 != null) {
					string Addr16333 = (string)CreditMemoRet.BillAddressBlock.Addr1.GetValue();
				}
				//Get value of Addr2
				if (CreditMemoRet.BillAddressBlock.Addr2 != null) {
					string Addr26334 = (string)CreditMemoRet.BillAddressBlock.Addr2.GetValue();
				}
				//Get value of Addr3
				if (CreditMemoRet.BillAddressBlock.Addr3 != null) {
					string Addr36335 = (string)CreditMemoRet.BillAddressBlock.Addr3.GetValue();
				}
				//Get value of Addr4
				if (CreditMemoRet.BillAddressBlock.Addr4 != null) {
					string Addr46336 = (string)CreditMemoRet.BillAddressBlock.Addr4.GetValue();
				}
				//Get value of Addr5
				if (CreditMemoRet.BillAddressBlock.Addr5 != null) {
					string Addr56337 = (string)CreditMemoRet.BillAddressBlock.Addr5.GetValue();
				}
			}
			if (CreditMemoRet.ShipAddress != null) {
				//Get value of Addr1
				if (CreditMemoRet.ShipAddress.Addr1 != null) {
					string Addr16338 = (string)CreditMemoRet.ShipAddress.Addr1.GetValue();
				}
				//Get value of Addr2
				if (CreditMemoRet.ShipAddress.Addr2 != null) {
					string Addr26339 = (string)CreditMemoRet.ShipAddress.Addr2.GetValue();
				}
				//Get value of Addr3
				if (CreditMemoRet.ShipAddress.Addr3 != null) {
					string Addr36340 = (string)CreditMemoRet.ShipAddress.Addr3.GetValue();
				}
				//Get value of Addr4
				if (CreditMemoRet.ShipAddress.Addr4 != null) {
					string Addr46341 = (string)CreditMemoRet.ShipAddress.Addr4.GetValue();
				}
				//Get value of Addr5
				if (CreditMemoRet.ShipAddress.Addr5 != null) {
					string Addr56342 = (string)CreditMemoRet.ShipAddress.Addr5.GetValue();
				}
				//Get value of City
				if (CreditMemoRet.ShipAddress.City != null) {
					string City6343 = (string)CreditMemoRet.ShipAddress.City.GetValue();
				}
				//Get value of State
				if (CreditMemoRet.ShipAddress.State != null) {
					string State6344 = (string)CreditMemoRet.ShipAddress.State.GetValue();
				}
				//Get value of PostalCode
				if (CreditMemoRet.ShipAddress.PostalCode != null) {
					string PostalCode6345 = (string)CreditMemoRet.ShipAddress.PostalCode.GetValue();
				}
				//Get value of Country
				if (CreditMemoRet.ShipAddress.Country != null) {
					string Country6346 = (string)CreditMemoRet.ShipAddress.Country.GetValue();
				}
				//Get value of Note
				if (CreditMemoRet.ShipAddress.Note != null) {
					string Note6347 = (string)CreditMemoRet.ShipAddress.Note.GetValue();
				}
			}
			if (CreditMemoRet.ShipAddressBlock != null) {
				//Get value of Addr1
				if (CreditMemoRet.ShipAddressBlock.Addr1 != null) {
					string Addr16348 = (string)CreditMemoRet.ShipAddressBlock.Addr1.GetValue();
				}
				//Get value of Addr2
				if (CreditMemoRet.ShipAddressBlock.Addr2 != null) {
					string Addr26349 = (string)CreditMemoRet.ShipAddressBlock.Addr2.GetValue();
				}
				//Get value of Addr3
				if (CreditMemoRet.ShipAddressBlock.Addr3 != null) {
					string Addr36350 = (string)CreditMemoRet.ShipAddressBlock.Addr3.GetValue();
				}
				//Get value of Addr4
				if (CreditMemoRet.ShipAddressBlock.Addr4 != null) {
					string Addr46351 = (string)CreditMemoRet.ShipAddressBlock.Addr4.GetValue();
				}
				//Get value of Addr5
				if (CreditMemoRet.ShipAddressBlock.Addr5 != null) {
					string Addr56352 = (string)CreditMemoRet.ShipAddressBlock.Addr5.GetValue();
				}
			}
			//Get value of IsPending
			if (CreditMemoRet.IsPending != null) {
				bool IsPending6353 = (bool)CreditMemoRet.IsPending.GetValue();
			}
			//Get value of PONumber
			if (CreditMemoRet.PONumber != null) {
				string PONumber6354 = (string)CreditMemoRet.PONumber.GetValue();
			}
			if (CreditMemoRet.TermsRef != null) {
				//Get value of ListID
				if (CreditMemoRet.TermsRef.ListID != null) {
					string ListID6355 = (string)CreditMemoRet.TermsRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CreditMemoRet.TermsRef.FullName != null) {
					string FullName6356 = (string)CreditMemoRet.TermsRef.FullName.GetValue();
				}
			}
			//Get value of DueDate
			if (CreditMemoRet.DueDate != null) {
				DateTime DueDate6357 = (DateTime)CreditMemoRet.DueDate.GetValue();
			}
			if (CreditMemoRet.SalesRepRef != null) {
				//Get value of ListID
				if (CreditMemoRet.SalesRepRef.ListID != null) {
					string ListID6358 = (string)CreditMemoRet.SalesRepRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CreditMemoRet.SalesRepRef.FullName != null) {
					string FullName6359 = (string)CreditMemoRet.SalesRepRef.FullName.GetValue();
				}
			}
			//Get value of FOB
			if (CreditMemoRet.FOB != null) {
				string FOB6360 = (string)CreditMemoRet.FOB.GetValue();
			}
			//Get value of ShipDate
			if (CreditMemoRet.ShipDate != null) {
				DateTime ShipDate6361 = (DateTime)CreditMemoRet.ShipDate.GetValue();
			}
			if (CreditMemoRet.ShipMethodRef != null) {
				//Get value of ListID
				if (CreditMemoRet.ShipMethodRef.ListID != null) {
					string ListID6362 = (string)CreditMemoRet.ShipMethodRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CreditMemoRet.ShipMethodRef.FullName != null) {
					string FullName6363 = (string)CreditMemoRet.ShipMethodRef.FullName.GetValue();
				}
			}
			//Get value of Subtotal
			if (CreditMemoRet.Subtotal != null) {
				double Subtotal6364 = (double)CreditMemoRet.Subtotal.GetValue();
			}
			if (CreditMemoRet.ItemSalesTaxRef != null) {
				//Get value of ListID
				if (CreditMemoRet.ItemSalesTaxRef.ListID != null) {
					string ListID6365 = (string)CreditMemoRet.ItemSalesTaxRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CreditMemoRet.ItemSalesTaxRef.FullName != null) {
					string FullName6366 = (string)CreditMemoRet.ItemSalesTaxRef.FullName.GetValue();
				}
			}
			//Get value of SalesTaxPercentage
			if (CreditMemoRet.SalesTaxPercentage != null) {
				double SalesTaxPercentage6367 = (double)CreditMemoRet.SalesTaxPercentage.GetValue();
			}
			//Get value of SalesTaxTotal
			if (CreditMemoRet.SalesTaxTotal != null) {
				double SalesTaxTotal6368 = (double)CreditMemoRet.SalesTaxTotal.GetValue();
			}
			//Get value of TotalAmount
			if (CreditMemoRet.TotalAmount != null) {
				double TotalAmount6369 = (double)CreditMemoRet.TotalAmount.GetValue();
			}
			//Get value of CreditRemaining
			if (CreditMemoRet.CreditRemaining != null) {
				double CreditRemaining6370 = (double)CreditMemoRet.CreditRemaining.GetValue();
			}
			if (CreditMemoRet.CurrencyRef != null) {
				//Get value of ListID
				if (CreditMemoRet.CurrencyRef.ListID != null) {
					string ListID6371 = (string)CreditMemoRet.CurrencyRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CreditMemoRet.CurrencyRef.FullName != null) {
					string FullName6372 = (string)CreditMemoRet.CurrencyRef.FullName.GetValue();
				}
			}
			//Get value of ExchangeRate
			if (CreditMemoRet.ExchangeRate != null) {
				IQBFloatType ExchangeRate6373 = (IQBFloatType)CreditMemoRet.ExchangeRate.GetValue();
			}
			//Get value of CreditRemainingInHomeCurrency
			if (CreditMemoRet.CreditRemainingInHomeCurrency != null) {
				double CreditRemainingInHomeCurrency6374 = (double)CreditMemoRet.CreditRemainingInHomeCurrency.GetValue();
			}
			//Get value of Memo
			if (CreditMemoRet.Memo != null) {
				string Memo6375 = (string)CreditMemoRet.Memo.GetValue();
			}
			if (CreditMemoRet.CustomerMsgRef != null) {
				//Get value of ListID
				if (CreditMemoRet.CustomerMsgRef.ListID != null) {
					string ListID6376 = (string)CreditMemoRet.CustomerMsgRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CreditMemoRet.CustomerMsgRef.FullName != null) {
					string FullName6377 = (string)CreditMemoRet.CustomerMsgRef.FullName.GetValue();
				}
			}
			//Get value of IsToBePrinted
			if (CreditMemoRet.IsToBePrinted != null) {
				bool IsToBePrinted6378 = (bool)CreditMemoRet.IsToBePrinted.GetValue();
			}
			//Get value of IsToBeEmailed
			if (CreditMemoRet.IsToBeEmailed != null) {
				bool IsToBeEmailed6379 = (bool)CreditMemoRet.IsToBeEmailed.GetValue();
			}
			//Get value of IsTaxIncluded
			if (CreditMemoRet.IsTaxIncluded != null) {
				bool IsTaxIncluded6380 = (bool)CreditMemoRet.IsTaxIncluded.GetValue();
			}
			if (CreditMemoRet.CustomerSalesTaxCodeRef != null) {
				//Get value of ListID
				if (CreditMemoRet.CustomerSalesTaxCodeRef.ListID != null) {
					string ListID6381 = (string)CreditMemoRet.CustomerSalesTaxCodeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (CreditMemoRet.CustomerSalesTaxCodeRef.FullName != null) {
					string FullName6382 = (string)CreditMemoRet.CustomerSalesTaxCodeRef.FullName.GetValue();
				}
			}
			//Get value of Other
			if (CreditMemoRet.Other != null) {
				string Other6383 = (string)CreditMemoRet.Other.GetValue();
			}
			//Get value of ExternalGUID
			if (CreditMemoRet.ExternalGUID != null) {
				string ExternalGUID6384 = (string)CreditMemoRet.ExternalGUID.GetValue();
			}
			if (CreditMemoRet.LinkedTxnList != null) {
				for (int i6385 = 0; i6385 < CreditMemoRet.LinkedTxnList.Count; i6385++) {
					ILinkedTxn LinkedTxn = CreditMemoRet.LinkedTxnList.GetAt(i6385);
					//Get value of TxnID
					string TxnID6386 = (string)LinkedTxn.TxnID.GetValue();
					//Get value of TxnType
					ENTxnType TxnType6387 = (ENTxnType)LinkedTxn.TxnType.GetValue();
					//Get value of TxnDate
					DateTime TxnDate6388 = (DateTime)LinkedTxn.TxnDate.GetValue();
					//Get value of RefNumber
					if (LinkedTxn.RefNumber != null) {
						string RefNumber6389 = (string)LinkedTxn.RefNumber.GetValue();
					}
					//Get value of LinkType
					if (LinkedTxn.LinkType != null) {
						ENLinkType LinkType6390 = (ENLinkType)LinkedTxn.LinkType.GetValue();
					}
					//Get value of Amount
					double Amount6391 = (double)LinkedTxn.Amount.GetValue();
				}
			}
			if (CreditMemoRet.ORCreditMemoLineRetList != null) {
				for (int i6392 = 0; i6392 < CreditMemoRet.ORCreditMemoLineRetList.Count; i6392++) {
					IORCreditMemoLineRet ORCreditMemoLineRet6393 = CreditMemoRet.ORCreditMemoLineRetList.GetAt(i6392);
					if (ORCreditMemoLineRet6393.CreditMemoLineRet != null) {
						if (ORCreditMemoLineRet6393.CreditMemoLineRet != null) {
							//Get value of TxnLineID
							string TxnLineID6394 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.TxnLineID.GetValue();
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.ItemRef != null) {
								//Get value of ListID
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.ItemRef.ListID != null) {
									string ListID6395 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.ItemRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.ItemRef.FullName != null) {
									string FullName6396 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.ItemRef.FullName.GetValue();
								}
							}
							//Get value of Desc
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.Desc != null) {
								string Desc6397 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.Desc.GetValue();
							}
							//Get value of Quantity
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.Quantity != null) {
								int Quantity6398 = (int)ORCreditMemoLineRet6393.CreditMemoLineRet.Quantity.GetValue();
							}
							//Get value of UnitOfMeasure
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.UnitOfMeasure != null) {
								string UnitOfMeasure6399 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.UnitOfMeasure.GetValue();
							}
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.OverrideUOMSetRef != null) {
								//Get value of ListID
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.OverrideUOMSetRef.ListID != null) {
									string ListID6400 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.OverrideUOMSetRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.OverrideUOMSetRef.FullName != null) {
									string FullName6401 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.OverrideUOMSetRef.FullName.GetValue();
								}
							}
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.ORRate != null) {
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.ORRate.Rate != null) {
									//Get value of Rate
									if (ORCreditMemoLineRet6393.CreditMemoLineRet.ORRate.Rate != null) {
										double Rate6403 = (double)ORCreditMemoLineRet6393.CreditMemoLineRet.ORRate.Rate.GetValue();
									}
								}
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.ORRate.RatePercent != null) {
									//Get value of RatePercent
									if (ORCreditMemoLineRet6393.CreditMemoLineRet.ORRate.RatePercent != null) {
										double RatePercent6404 = (double)ORCreditMemoLineRet6393.CreditMemoLineRet.ORRate.RatePercent.GetValue();
									}
								}
							}
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.ClassRef != null) {
								//Get value of ListID
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.ClassRef.ListID != null) {
									string ListID6405 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.ClassRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.ClassRef.FullName != null) {
									string FullName6406 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.ClassRef.FullName.GetValue();
								}
							}
							//Get value of Amount
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.Amount != null) {
								double Amount6407 = (double)ORCreditMemoLineRet6393.CreditMemoLineRet.Amount.GetValue();
							}
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.InventorySiteRef != null) {
								//Get value of ListID
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.InventorySiteRef.ListID != null) {
									string ListID6408 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.InventorySiteRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.InventorySiteRef.FullName != null) {
									string FullName6409 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.InventorySiteRef.FullName.GetValue();
								}
							}
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.InventorySiteLocationRef != null) {
								//Get value of ListID
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.InventorySiteLocationRef.ListID != null) {
									string ListID6410 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.InventorySiteLocationRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.InventorySiteLocationRef.FullName != null) {
									string FullName6411 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.InventorySiteLocationRef.FullName.GetValue();
								}
							}
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.ORSerialLotNumber != null) {
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.ORSerialLotNumber.SerialNumber != null) {
									//Get value of SerialNumber
									if (ORCreditMemoLineRet6393.CreditMemoLineRet.ORSerialLotNumber.SerialNumber != null) {
										string SerialNumber6413 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.ORSerialLotNumber.SerialNumber.GetValue();
									}
								}
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.ORSerialLotNumber.LotNumber != null) {
									//Get value of LotNumber
									if (ORCreditMemoLineRet6393.CreditMemoLineRet.ORSerialLotNumber.LotNumber != null) {
										string LotNumber6414 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.ORSerialLotNumber.LotNumber.GetValue();
									}
								}
							}
							//Get value of ServiceDate
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.ServiceDate != null) {
								DateTime ServiceDate6415 = (DateTime)ORCreditMemoLineRet6393.CreditMemoLineRet.ServiceDate.GetValue();
							}
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.SalesTaxCodeRef != null) {
								//Get value of ListID
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.SalesTaxCodeRef.ListID != null) {
									string ListID6416 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.SalesTaxCodeRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.SalesTaxCodeRef.FullName != null) {
									string FullName6417 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.SalesTaxCodeRef.FullName.GetValue();
								}
							}
							//Get value of Other1
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.Other1 != null) {
								string Other16418 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.Other1.GetValue();
							}
							//Get value of Other2
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.Other2 != null) {
								string Other26419 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.Other2.GetValue();
							}
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo != null) {
								//Get value of CreditCardNumber
								string CreditCardNumber6420 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardNumber.GetValue();
								//Get value of ExpirationMonth
								int ExpirationMonth6421 = (int)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationMonth.GetValue();
								//Get value of ExpirationYear
								int ExpirationYear6422 = (int)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationYear.GetValue();
								//Get value of NameOnCard
								string NameOnCard6423 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.NameOnCard.GetValue();
								//Get value of CreditCardAddress
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress != null) {
									string CreditCardAddress6424 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress.GetValue();
								}
								//Get value of CreditCardPostalCode
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode != null) {
									string CreditCardPostalCode6425 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode.GetValue();
								}
								//Get value of CommercialCardCode
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode != null) {
									string CommercialCardCode6426 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode.GetValue();
								}
								//Get value of TransactionMode
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode != null) {
									ENTransactionMode TransactionMode6427 = (ENTransactionMode)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode.GetValue();
								}
								//Get value of CreditCardTxnType
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType != null) {
									ENCreditCardTxnType CreditCardTxnType6428 = (ENCreditCardTxnType)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType.GetValue();
								}
								//Get value of ResultCode
								int ResultCode6429 = (int)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ResultCode.GetValue();
								//Get value of ResultMessage
								string ResultMessage6430 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ResultMessage.GetValue();
								//Get value of CreditCardTransID
								string CreditCardTransID6431 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.CreditCardTransID.GetValue();
								//Get value of MerchantAccountNumber
								string MerchantAccountNumber6432 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.MerchantAccountNumber.GetValue();
								//Get value of AuthorizationCode
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode != null) {
									string AuthorizationCode6433 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode.GetValue();
								}
								//Get value of AVSStreet
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet != null) {
									ENAVSStreet AVSStreet6434 = (ENAVSStreet)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet.GetValue();
								}
								//Get value of AVSZip
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip != null) {
									ENAVSZip AVSZip6435 = (ENAVSZip)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip.GetValue();
								}
								//Get value of CardSecurityCodeMatch
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch != null) {
									ENCardSecurityCodeMatch CardSecurityCodeMatch6436 = (ENCardSecurityCodeMatch)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch.GetValue();
								}
								//Get value of ReconBatchID
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchID != null) {
									string ReconBatchID6437 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchID.GetValue();
								}
								//Get value of PaymentGroupingCode
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode != null) {
									int PaymentGroupingCode6438 = (int)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode.GetValue();
								}
								//Get value of PaymentStatus
								ENPaymentStatus PaymentStatus6439 = (ENPaymentStatus)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentStatus.GetValue();
								//Get value of TxnAuthorizationTime
								DateTime TxnAuthorizationTime6440 = (DateTime)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationTime.GetValue();
								//Get value of TxnAuthorizationStamp
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp != null) {
									int TxnAuthorizationStamp6441 = (int)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp.GetValue();
								}
								//Get value of ClientTransID
								if (ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID != null) {
									string ClientTransID6442 = (string)ORCreditMemoLineRet6393.CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID.GetValue();
								}
							}
							if (ORCreditMemoLineRet6393.CreditMemoLineRet.DataExtRetList != null) {
								for (int i6443 = 0; i6443 < ORCreditMemoLineRet6393.CreditMemoLineRet.DataExtRetList.Count; i6443++) {
									IDataExtRet DataExtRet = ORCreditMemoLineRet6393.CreditMemoLineRet.DataExtRetList.GetAt(i6443);
									//Get value of OwnerID
									if (DataExtRet.OwnerID != null) {
										string OwnerID6444 = (string)DataExtRet.OwnerID.GetValue();
									}
									//Get value of DataExtName
									string DataExtName6445 = (string)DataExtRet.DataExtName.GetValue();
									//Get value of DataExtType
									ENDataExtType DataExtType6446 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
									//Get value of DataExtValue
									string DataExtValue6447 = (string)DataExtRet.DataExtValue.GetValue();
								}
							}
						}
					}
					if (ORCreditMemoLineRet6393.CreditMemoLineGroupRet != null) {
						if (ORCreditMemoLineRet6393.CreditMemoLineGroupRet != null) {
							//Get value of TxnLineID
							string TxnLineID6448 = (string)ORCreditMemoLineRet6393.CreditMemoLineGroupRet.TxnLineID.GetValue();
							//Get value of ListID
							if (ORCreditMemoLineRet6393.CreditMemoLineGroupRet.ItemGroupRef.ListID != null) {
								string ListID6449 = (string)ORCreditMemoLineRet6393.CreditMemoLineGroupRet.ItemGroupRef.ListID.GetValue();
							}
							//Get value of FullName
							if (ORCreditMemoLineRet6393.CreditMemoLineGroupRet.ItemGroupRef.FullName != null) {
								string FullName6450 = (string)ORCreditMemoLineRet6393.CreditMemoLineGroupRet.ItemGroupRef.FullName.GetValue();
							}
							//Get value of Desc
							if (ORCreditMemoLineRet6393.CreditMemoLineGroupRet.Desc != null) {
								string Desc6451 = (string)ORCreditMemoLineRet6393.CreditMemoLineGroupRet.Desc.GetValue();
							}
							//Get value of Quantity
							if (ORCreditMemoLineRet6393.CreditMemoLineGroupRet.Quantity != null) {
								int Quantity6452 = (int)ORCreditMemoLineRet6393.CreditMemoLineGroupRet.Quantity.GetValue();
							}
							//Get value of UnitOfMeasure
							if (ORCreditMemoLineRet6393.CreditMemoLineGroupRet.UnitOfMeasure != null) {
								string UnitOfMeasure6453 = (string)ORCreditMemoLineRet6393.CreditMemoLineGroupRet.UnitOfMeasure.GetValue();
							}
							if (ORCreditMemoLineRet6393.CreditMemoLineGroupRet.OverrideUOMSetRef != null) {
								//Get value of ListID
								if (ORCreditMemoLineRet6393.CreditMemoLineGroupRet.OverrideUOMSetRef.ListID != null) {
									string ListID6454 = (string)ORCreditMemoLineRet6393.CreditMemoLineGroupRet.OverrideUOMSetRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORCreditMemoLineRet6393.CreditMemoLineGroupRet.OverrideUOMSetRef.FullName != null) {
									string FullName6455 = (string)ORCreditMemoLineRet6393.CreditMemoLineGroupRet.OverrideUOMSetRef.FullName.GetValue();
								}
							}
							//Get value of IsPrintItemsInGroup
							bool IsPrintItemsInGroup6456 = (bool)ORCreditMemoLineRet6393.CreditMemoLineGroupRet.IsPrintItemsInGroup.GetValue();
							//Get value of TotalAmount
							double TotalAmount6457 = (double)ORCreditMemoLineRet6393.CreditMemoLineGroupRet.TotalAmount.GetValue();
							if (ORCreditMemoLineRet6393.CreditMemoLineGroupRet.CreditMemoLineRetList != null) {
								for (int i6458 = 0; i6458 < ORCreditMemoLineRet6393.CreditMemoLineGroupRet.CreditMemoLineRetList.Count; i6458++) {
									ICreditMemoLineRet CreditMemoLineRet = ORCreditMemoLineRet6393.CreditMemoLineGroupRet.CreditMemoLineRetList.GetAt(i6458);
									//Get value of TxnLineID
									string TxnLineID6459 = (string)CreditMemoLineRet.TxnLineID.GetValue();
									if (CreditMemoLineRet.ItemRef != null) {
										//Get value of ListID
										if (CreditMemoLineRet.ItemRef.ListID != null) {
											string ListID6460 = (string)CreditMemoLineRet.ItemRef.ListID.GetValue();
										}
										//Get value of FullName
										if (CreditMemoLineRet.ItemRef.FullName != null) {
											string FullName6461 = (string)CreditMemoLineRet.ItemRef.FullName.GetValue();
										}
									}
									//Get value of Desc
									if (CreditMemoLineRet.Desc != null) {
										string Desc6462 = (string)CreditMemoLineRet.Desc.GetValue();
									}
									//Get value of Quantity
									if (CreditMemoLineRet.Quantity != null) {
										int Quantity6463 = (int)CreditMemoLineRet.Quantity.GetValue();
									}
									//Get value of UnitOfMeasure
									if (CreditMemoLineRet.UnitOfMeasure != null) {
										string UnitOfMeasure6464 = (string)CreditMemoLineRet.UnitOfMeasure.GetValue();
									}
									if (CreditMemoLineRet.OverrideUOMSetRef != null) {
										//Get value of ListID
										if (CreditMemoLineRet.OverrideUOMSetRef.ListID != null) {
											string ListID6465 = (string)CreditMemoLineRet.OverrideUOMSetRef.ListID.GetValue();
										}
										//Get value of FullName
										if (CreditMemoLineRet.OverrideUOMSetRef.FullName != null) {
											string FullName6466 = (string)CreditMemoLineRet.OverrideUOMSetRef.FullName.GetValue();
										}
									}
									if (CreditMemoLineRet.ORRate != null) {
										if (CreditMemoLineRet.ORRate.Rate != null) {
											//Get value of Rate
											if (CreditMemoLineRet.ORRate.Rate != null) {
												double Rate6468 = (double)CreditMemoLineRet.ORRate.Rate.GetValue();
											}
										}
										if (CreditMemoLineRet.ORRate.RatePercent != null) {
											//Get value of RatePercent
											if (CreditMemoLineRet.ORRate.RatePercent != null) {
												double RatePercent6469 = (double)CreditMemoLineRet.ORRate.RatePercent.GetValue();
											}
										}
									}
									if (CreditMemoLineRet.ClassRef != null) {
										//Get value of ListID
										if (CreditMemoLineRet.ClassRef.ListID != null) {
											string ListID6470 = (string)CreditMemoLineRet.ClassRef.ListID.GetValue();
										}
										//Get value of FullName
										if (CreditMemoLineRet.ClassRef.FullName != null) {
											string FullName6471 = (string)CreditMemoLineRet.ClassRef.FullName.GetValue();
										}
									}
									//Get value of Amount
									if (CreditMemoLineRet.Amount != null) {
										double Amount6472 = (double)CreditMemoLineRet.Amount.GetValue();
									}
									if (CreditMemoLineRet.InventorySiteRef != null) {
										//Get value of ListID
										if (CreditMemoLineRet.InventorySiteRef.ListID != null) {
											string ListID6473 = (string)CreditMemoLineRet.InventorySiteRef.ListID.GetValue();
										}
										//Get value of FullName
										if (CreditMemoLineRet.InventorySiteRef.FullName != null) {
											string FullName6474 = (string)CreditMemoLineRet.InventorySiteRef.FullName.GetValue();
										}
									}
									if (CreditMemoLineRet.InventorySiteLocationRef != null) {
										//Get value of ListID
										if (CreditMemoLineRet.InventorySiteLocationRef.ListID != null) {
											string ListID6475 = (string)CreditMemoLineRet.InventorySiteLocationRef.ListID.GetValue();
										}
										//Get value of FullName
										if (CreditMemoLineRet.InventorySiteLocationRef.FullName != null) {
											string FullName6476 = (string)CreditMemoLineRet.InventorySiteLocationRef.FullName.GetValue();
										}
									}
									if (CreditMemoLineRet.ORSerialLotNumber != null) {
										if (CreditMemoLineRet.ORSerialLotNumber.SerialNumber != null) {
											//Get value of SerialNumber
											if (CreditMemoLineRet.ORSerialLotNumber.SerialNumber != null) {
												string SerialNumber6478 = (string)CreditMemoLineRet.ORSerialLotNumber.SerialNumber.GetValue();
											}
										}
										if (CreditMemoLineRet.ORSerialLotNumber.LotNumber != null) {
											//Get value of LotNumber
											if (CreditMemoLineRet.ORSerialLotNumber.LotNumber != null) {
												string LotNumber6479 = (string)CreditMemoLineRet.ORSerialLotNumber.LotNumber.GetValue();
											}
										}
									}
									//Get value of ServiceDate
									if (CreditMemoLineRet.ServiceDate != null) {
										DateTime ServiceDate6480 = (DateTime)CreditMemoLineRet.ServiceDate.GetValue();
									}
									if (CreditMemoLineRet.SalesTaxCodeRef != null) {
										//Get value of ListID
										if (CreditMemoLineRet.SalesTaxCodeRef.ListID != null) {
											string ListID6481 = (string)CreditMemoLineRet.SalesTaxCodeRef.ListID.GetValue();
										}
										//Get value of FullName
										if (CreditMemoLineRet.SalesTaxCodeRef.FullName != null) {
											string FullName6482 = (string)CreditMemoLineRet.SalesTaxCodeRef.FullName.GetValue();
										}
									}
									//Get value of Other1
									if (CreditMemoLineRet.Other1 != null) {
										string Other16483 = (string)CreditMemoLineRet.Other1.GetValue();
									}
									//Get value of Other2
									if (CreditMemoLineRet.Other2 != null) {
										string Other26484 = (string)CreditMemoLineRet.Other2.GetValue();
									}
									if (CreditMemoLineRet.CreditCardTxnInfo != null) {
										//Get value of CreditCardNumber
										string CreditCardNumber6485 = (string)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardNumber.GetValue();
										//Get value of ExpirationMonth
										int ExpirationMonth6486 = (int)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationMonth.GetValue();
										//Get value of ExpirationYear
										int ExpirationYear6487 = (int)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationYear.GetValue();
										//Get value of NameOnCard
										string NameOnCard6488 = (string)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.NameOnCard.GetValue();
										//Get value of CreditCardAddress
										if (CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress != null) {
											string CreditCardAddress6489 = (string)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress.GetValue();
										}
										//Get value of CreditCardPostalCode
										if (CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode != null) {
											string CreditCardPostalCode6490 = (string)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode.GetValue();
										}
										//Get value of CommercialCardCode
										if (CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode != null) {
											string CommercialCardCode6491 = (string)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode.GetValue();
										}
										//Get value of TransactionMode
										if (CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode != null) {
											ENTransactionMode TransactionMode6492 = (ENTransactionMode)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode.GetValue();
										}
										//Get value of CreditCardTxnType
										if (CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType != null) {
											ENCreditCardTxnType CreditCardTxnType6493 = (ENCreditCardTxnType)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType.GetValue();
										}
										//Get value of ResultCode
										int ResultCode6494 = (int)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ResultCode.GetValue();
										//Get value of ResultMessage
										string ResultMessage6495 = (string)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ResultMessage.GetValue();
										//Get value of CreditCardTransID
										string CreditCardTransID6496 = (string)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.CreditCardTransID.GetValue();
										//Get value of MerchantAccountNumber
										string MerchantAccountNumber6497 = (string)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.MerchantAccountNumber.GetValue();
										//Get value of AuthorizationCode
										if (CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode != null) {
											string AuthorizationCode6498 = (string)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode.GetValue();
										}
										//Get value of AVSStreet
										if (CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet != null) {
											ENAVSStreet AVSStreet6499 = (ENAVSStreet)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet.GetValue();
										}
										//Get value of AVSZip
										if (CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip != null) {
											ENAVSZip AVSZip6500 = (ENAVSZip)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip.GetValue();
										}
										//Get value of CardSecurityCodeMatch
										if (CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch != null) {
											ENCardSecurityCodeMatch CardSecurityCodeMatch6501 = (ENCardSecurityCodeMatch)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch.GetValue();
										}
										//Get value of ReconBatchID
										if (CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchID != null) {
											string ReconBatchID6502 = (string)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchID.GetValue();
										}
										//Get value of PaymentGroupingCode
										if (CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode != null) {
											int PaymentGroupingCode6503 = (int)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode.GetValue();
										}
										//Get value of PaymentStatus
										ENPaymentStatus PaymentStatus6504 = (ENPaymentStatus)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentStatus.GetValue();
										//Get value of TxnAuthorizationTime
										DateTime TxnAuthorizationTime6505 = (DateTime)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationTime.GetValue();
										//Get value of TxnAuthorizationStamp
										if (CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp != null) {
											int TxnAuthorizationStamp6506 = (int)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp.GetValue();
										}
										//Get value of ClientTransID
										if (CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID != null) {
											string ClientTransID6507 = (string)CreditMemoLineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID.GetValue();
										}
									}
									if (CreditMemoLineRet.DataExtRetList != null) {
										for (int i6508 = 0; i6508 < CreditMemoLineRet.DataExtRetList.Count; i6508++) {
											IDataExtRet DataExtRet = CreditMemoLineRet.DataExtRetList.GetAt(i6508);
											//Get value of OwnerID
											if (DataExtRet.OwnerID != null) {
												string OwnerID6509 = (string)DataExtRet.OwnerID.GetValue();
											}
											//Get value of DataExtName
											string DataExtName6510 = (string)DataExtRet.DataExtName.GetValue();
											//Get value of DataExtType
											ENDataExtType DataExtType6511 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
											//Get value of DataExtValue
											string DataExtValue6512 = (string)DataExtRet.DataExtValue.GetValue();
										}
									}
								}
							}
							if (ORCreditMemoLineRet6393.CreditMemoLineGroupRet.DataExtRetList != null) {
								for (int i6513 = 0; i6513 < ORCreditMemoLineRet6393.CreditMemoLineGroupRet.DataExtRetList.Count; i6513++) {
									IDataExtRet DataExtRet = ORCreditMemoLineRet6393.CreditMemoLineGroupRet.DataExtRetList.GetAt(i6513);
									//Get value of OwnerID
									if (DataExtRet.OwnerID != null) {
										string OwnerID6514 = (string)DataExtRet.OwnerID.GetValue();
									}
									//Get value of DataExtName
									string DataExtName6515 = (string)DataExtRet.DataExtName.GetValue();
									//Get value of DataExtType
									ENDataExtType DataExtType6516 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
									//Get value of DataExtValue
									string DataExtValue6517 = (string)DataExtRet.DataExtValue.GetValue();
								}
							}
						}
					}
				}
			}
			if (CreditMemoRet.DataExtRetList != null) {
				for (int i6518 = 0; i6518 < CreditMemoRet.DataExtRetList.Count; i6518++) {
					IDataExtRet DataExtRet = CreditMemoRet.DataExtRetList.GetAt(i6518);
					//Get value of OwnerID
					if (DataExtRet.OwnerID != null) {
						string OwnerID6519 = (string)DataExtRet.OwnerID.GetValue();
					}
					//Get value of DataExtName
					string DataExtName6520 = (string)DataExtRet.DataExtName.GetValue();
					//Get value of DataExtType
					ENDataExtType DataExtType6521 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
					//Get value of DataExtValue
					string DataExtValue6522 = (string)DataExtRet.DataExtValue.GetValue();
				}
			}
		}
	}
}