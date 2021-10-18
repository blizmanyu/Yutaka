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
			string ORTxnQueryElementType6301 = "TxnIDList";
			if (ORTxnQueryElementType6301 == "TxnIDList") {
				//Set field value for TxnIDList
				//May create more than one of these if needed
				QueryRq.ORTxnQuery.TxnIDList.Add("200000-1011023419");
			}
			if (ORTxnQueryElementType6301 == "RefNumberList") {
				//Set field value for RefNumberList
				//May create more than one of these if needed
				QueryRq.ORTxnQuery.RefNumberList.Add("ab");
			}
			if (ORTxnQueryElementType6301 == "RefNumberCaseSensitiveList") {
				//Set field value for RefNumberCaseSensitiveList
				//May create more than one of these if needed
				QueryRq.ORTxnQuery.RefNumberCaseSensitiveList.Add("ab");
			}
			if (ORTxnQueryElementType6301 == "TxnFilter") {
				//Set field value for MaxReturned
				QueryRq.ORTxnQuery.TxnFilter.MaxReturned.SetValue(6);
				string ORDateRangeFilterElementType6302 = "ModifiedDateRangeFilter";
				if (ORDateRangeFilterElementType6302 == "ModifiedDateRangeFilter") {
					//Set field value for FromModifiedDate
					QueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.ModifiedDateRangeFilter.FromModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
					//Set field value for ToModifiedDate
					QueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.ModifiedDateRangeFilter.ToModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				}
				if (ORDateRangeFilterElementType6302 == "TxnDateRangeFilter") {
					string ORTxnDateRangeFilterElementType6303 = "TxnDateFilter";
					if (ORTxnDateRangeFilterElementType6303 == "TxnDateFilter") {
						//Set field value for FromTxnDate
						QueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.FromTxnDate.SetValue(DateTime.Parse("12/15/2007"));
						//Set field value for ToTxnDate
						QueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.ToTxnDate.SetValue(DateTime.Parse("12/15/2007"));
					}
					if (ORTxnDateRangeFilterElementType6303 == "DateMacro") {
						//Set field value for DateMacro
						QueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.DateMacro.SetValue(ENDateMacro.dmAll);
					}
				}
				string OREntityFilterElementType6304 = "ListIDList";
				if (OREntityFilterElementType6304 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					QueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.ListIDList.Add("200000-1011023419");
				}
				if (OREntityFilterElementType6304 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					QueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.FullNameList.Add("ab");
				}
				if (OREntityFilterElementType6304 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					QueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (OREntityFilterElementType6304 == "FullNameWithChildren") {
					//Set field value for FullNameWithChildren
					QueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.FullNameWithChildren.SetValue("ab");
				}
				string ORAccountFilterElementType6305 = "ListIDList";
				if (ORAccountFilterElementType6305 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					QueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORAccountFilterElementType6305 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					QueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.FullNameList.Add("ab");
				}
				if (ORAccountFilterElementType6305 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					QueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (ORAccountFilterElementType6305 == "FullNameWithChildren") {
					//Set field value for FullNameWithChildren
					QueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.FullNameWithChildren.SetValue("ab");
				}
				string ORRefNumberFilterElementType6306 = "RefNumberFilter";
				if (ORRefNumberFilterElementType6306 == "RefNumberFilter") {
					//Set field value for MatchCriterion
					QueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberFilter.MatchCriterion.SetValue(ENMatchCriterion.mcStartsWith);
					//Set field value for RefNumber
					QueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberFilter.RefNumber.SetValue("ab");
				}
				if (ORRefNumberFilterElementType6306 == "RefNumberRangeFilter") {
					//Set field value for FromRefNumber
					QueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberRangeFilter.FromRefNumber.SetValue("ab");
					//Set field value for ToRefNumber
					QueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberRangeFilter.ToRefNumber.SetValue("ab");
				}
				string ORCurrencyFilterElementType6307 = "ListIDList";
				if (ORCurrencyFilterElementType6307 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					QueryRq.ORTxnQuery.TxnFilter.CurrencyFilter.ORCurrencyFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORCurrencyFilterElementType6307 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					QueryRq.ORTxnQuery.TxnFilter.CurrencyFilter.ORCurrencyFilter.FullNameList.Add("ab");
				}
			}
			//Set field value for IncludeLineItems
			QueryRq.IncludeLineItems.SetValue(true);
			//Set field value for IncludeLinkedTxns
			QueryRq.IncludeLinkedTxns.SetValue(true);
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
			//Get value of TxnID
			string TxnID6308 = (string)Ret.TxnID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated6309 = (DateTime)Ret.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified6310 = (DateTime)Ret.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence6311 = (string)Ret.EditSequence.GetValue();
			//Get value of TxnNumber
			if (Ret.TxnNumber != null) {
				int TxnNumber6312 = (int)Ret.TxnNumber.GetValue();
			}
			//Get value of ListID
			if (Ret.CustomerRef.ListID != null) {
				string ListID6313 = (string)Ret.CustomerRef.ListID.GetValue();
			}
			//Get value of FullName
			if (Ret.CustomerRef.FullName != null) {
				string FullName6314 = (string)Ret.CustomerRef.FullName.GetValue();
			}
			if (Ret.ClassRef != null) {
				//Get value of ListID
				if (Ret.ClassRef.ListID != null) {
					string ListID6315 = (string)Ret.ClassRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ClassRef.FullName != null) {
					string FullName6316 = (string)Ret.ClassRef.FullName.GetValue();
				}
			}
			if (Ret.ARAccountRef != null) {
				//Get value of ListID
				if (Ret.ARAccountRef.ListID != null) {
					string ListID6317 = (string)Ret.ARAccountRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ARAccountRef.FullName != null) {
					string FullName6318 = (string)Ret.ARAccountRef.FullName.GetValue();
				}
			}
			if (Ret.TemplateRef != null) {
				//Get value of ListID
				if (Ret.TemplateRef.ListID != null) {
					string ListID6319 = (string)Ret.TemplateRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.TemplateRef.FullName != null) {
					string FullName6320 = (string)Ret.TemplateRef.FullName.GetValue();
				}
			}
			//Get value of TxnDate
			DateTime TxnDate6321 = (DateTime)Ret.TxnDate.GetValue();
			//Get value of RefNumber
			if (Ret.RefNumber != null) {
				string RefNumber6322 = (string)Ret.RefNumber.GetValue();
			}
			if (Ret.BillAddress != null) {
				//Get value of Addr1
				if (Ret.BillAddress.Addr1 != null) {
					string Addr16323 = (string)Ret.BillAddress.Addr1.GetValue();
				}
				//Get value of Addr2
				if (Ret.BillAddress.Addr2 != null) {
					string Addr26324 = (string)Ret.BillAddress.Addr2.GetValue();
				}
				//Get value of Addr3
				if (Ret.BillAddress.Addr3 != null) {
					string Addr36325 = (string)Ret.BillAddress.Addr3.GetValue();
				}
				//Get value of Addr4
				if (Ret.BillAddress.Addr4 != null) {
					string Addr46326 = (string)Ret.BillAddress.Addr4.GetValue();
				}
				//Get value of Addr5
				if (Ret.BillAddress.Addr5 != null) {
					string Addr56327 = (string)Ret.BillAddress.Addr5.GetValue();
				}
				//Get value of City
				if (Ret.BillAddress.City != null) {
					string City6328 = (string)Ret.BillAddress.City.GetValue();
				}
				//Get value of State
				if (Ret.BillAddress.State != null) {
					string State6329 = (string)Ret.BillAddress.State.GetValue();
				}
				//Get value of PostalCode
				if (Ret.BillAddress.PostalCode != null) {
					string PostalCode6330 = (string)Ret.BillAddress.PostalCode.GetValue();
				}
				//Get value of Country
				if (Ret.BillAddress.Country != null) {
					string Country6331 = (string)Ret.BillAddress.Country.GetValue();
				}
				//Get value of Note
				if (Ret.BillAddress.Note != null) {
					string Note6332 = (string)Ret.BillAddress.Note.GetValue();
				}
			}
			if (Ret.BillAddressBlock != null) {
				//Get value of Addr1
				if (Ret.BillAddressBlock.Addr1 != null) {
					string Addr16333 = (string)Ret.BillAddressBlock.Addr1.GetValue();
				}
				//Get value of Addr2
				if (Ret.BillAddressBlock.Addr2 != null) {
					string Addr26334 = (string)Ret.BillAddressBlock.Addr2.GetValue();
				}
				//Get value of Addr3
				if (Ret.BillAddressBlock.Addr3 != null) {
					string Addr36335 = (string)Ret.BillAddressBlock.Addr3.GetValue();
				}
				//Get value of Addr4
				if (Ret.BillAddressBlock.Addr4 != null) {
					string Addr46336 = (string)Ret.BillAddressBlock.Addr4.GetValue();
				}
				//Get value of Addr5
				if (Ret.BillAddressBlock.Addr5 != null) {
					string Addr56337 = (string)Ret.BillAddressBlock.Addr5.GetValue();
				}
			}
			if (Ret.ShipAddress != null) {
				//Get value of Addr1
				if (Ret.ShipAddress.Addr1 != null) {
					string Addr16338 = (string)Ret.ShipAddress.Addr1.GetValue();
				}
				//Get value of Addr2
				if (Ret.ShipAddress.Addr2 != null) {
					string Addr26339 = (string)Ret.ShipAddress.Addr2.GetValue();
				}
				//Get value of Addr3
				if (Ret.ShipAddress.Addr3 != null) {
					string Addr36340 = (string)Ret.ShipAddress.Addr3.GetValue();
				}
				//Get value of Addr4
				if (Ret.ShipAddress.Addr4 != null) {
					string Addr46341 = (string)Ret.ShipAddress.Addr4.GetValue();
				}
				//Get value of Addr5
				if (Ret.ShipAddress.Addr5 != null) {
					string Addr56342 = (string)Ret.ShipAddress.Addr5.GetValue();
				}
				//Get value of City
				if (Ret.ShipAddress.City != null) {
					string City6343 = (string)Ret.ShipAddress.City.GetValue();
				}
				//Get value of State
				if (Ret.ShipAddress.State != null) {
					string State6344 = (string)Ret.ShipAddress.State.GetValue();
				}
				//Get value of PostalCode
				if (Ret.ShipAddress.PostalCode != null) {
					string PostalCode6345 = (string)Ret.ShipAddress.PostalCode.GetValue();
				}
				//Get value of Country
				if (Ret.ShipAddress.Country != null) {
					string Country6346 = (string)Ret.ShipAddress.Country.GetValue();
				}
				//Get value of Note
				if (Ret.ShipAddress.Note != null) {
					string Note6347 = (string)Ret.ShipAddress.Note.GetValue();
				}
			}
			if (Ret.ShipAddressBlock != null) {
				//Get value of Addr1
				if (Ret.ShipAddressBlock.Addr1 != null) {
					string Addr16348 = (string)Ret.ShipAddressBlock.Addr1.GetValue();
				}
				//Get value of Addr2
				if (Ret.ShipAddressBlock.Addr2 != null) {
					string Addr26349 = (string)Ret.ShipAddressBlock.Addr2.GetValue();
				}
				//Get value of Addr3
				if (Ret.ShipAddressBlock.Addr3 != null) {
					string Addr36350 = (string)Ret.ShipAddressBlock.Addr3.GetValue();
				}
				//Get value of Addr4
				if (Ret.ShipAddressBlock.Addr4 != null) {
					string Addr46351 = (string)Ret.ShipAddressBlock.Addr4.GetValue();
				}
				//Get value of Addr5
				if (Ret.ShipAddressBlock.Addr5 != null) {
					string Addr56352 = (string)Ret.ShipAddressBlock.Addr5.GetValue();
				}
			}
			//Get value of IsPending
			if (Ret.IsPending != null) {
				bool IsPending6353 = (bool)Ret.IsPending.GetValue();
			}
			//Get value of PONumber
			if (Ret.PONumber != null) {
				string PONumber6354 = (string)Ret.PONumber.GetValue();
			}
			if (Ret.TermsRef != null) {
				//Get value of ListID
				if (Ret.TermsRef.ListID != null) {
					string ListID6355 = (string)Ret.TermsRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.TermsRef.FullName != null) {
					string FullName6356 = (string)Ret.TermsRef.FullName.GetValue();
				}
			}
			//Get value of DueDate
			if (Ret.DueDate != null) {
				DateTime DueDate6357 = (DateTime)Ret.DueDate.GetValue();
			}
			if (Ret.SalesRepRef != null) {
				//Get value of ListID
				if (Ret.SalesRepRef.ListID != null) {
					string ListID6358 = (string)Ret.SalesRepRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.SalesRepRef.FullName != null) {
					string FullName6359 = (string)Ret.SalesRepRef.FullName.GetValue();
				}
			}
			//Get value of FOB
			if (Ret.FOB != null) {
				string FOB6360 = (string)Ret.FOB.GetValue();
			}
			//Get value of ShipDate
			if (Ret.ShipDate != null) {
				DateTime ShipDate6361 = (DateTime)Ret.ShipDate.GetValue();
			}
			if (Ret.ShipMethodRef != null) {
				//Get value of ListID
				if (Ret.ShipMethodRef.ListID != null) {
					string ListID6362 = (string)Ret.ShipMethodRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ShipMethodRef.FullName != null) {
					string FullName6363 = (string)Ret.ShipMethodRef.FullName.GetValue();
				}
			}
			//Get value of Subtotal
			if (Ret.Subtotal != null) {
				double Subtotal6364 = (double)Ret.Subtotal.GetValue();
			}
			if (Ret.ItemSalesTaxRef != null) {
				//Get value of ListID
				if (Ret.ItemSalesTaxRef.ListID != null) {
					string ListID6365 = (string)Ret.ItemSalesTaxRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ItemSalesTaxRef.FullName != null) {
					string FullName6366 = (string)Ret.ItemSalesTaxRef.FullName.GetValue();
				}
			}
			//Get value of SalesTaxPercentage
			if (Ret.SalesTaxPercentage != null) {
				double SalesTaxPercentage6367 = (double)Ret.SalesTaxPercentage.GetValue();
			}
			//Get value of SalesTaxTotal
			if (Ret.SalesTaxTotal != null) {
				double SalesTaxTotal6368 = (double)Ret.SalesTaxTotal.GetValue();
			}
			//Get value of TotalAmount
			if (Ret.TotalAmount != null) {
				double TotalAmount6369 = (double)Ret.TotalAmount.GetValue();
			}
			//Get value of CreditRemaining
			if (Ret.CreditRemaining != null) {
				double CreditRemaining6370 = (double)Ret.CreditRemaining.GetValue();
			}
			if (Ret.CurrencyRef != null) {
				//Get value of ListID
				if (Ret.CurrencyRef.ListID != null) {
					string ListID6371 = (string)Ret.CurrencyRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.CurrencyRef.FullName != null) {
					string FullName6372 = (string)Ret.CurrencyRef.FullName.GetValue();
				}
			}
			//Get value of ExchangeRate
			if (Ret.ExchangeRate != null) {
				IQBFloatType ExchangeRate6373 = (IQBFloatType)Ret.ExchangeRate.GetValue();
			}
			//Get value of CreditRemainingInHomeCurrency
			if (Ret.CreditRemainingInHomeCurrency != null) {
				double CreditRemainingInHomeCurrency6374 = (double)Ret.CreditRemainingInHomeCurrency.GetValue();
			}
			//Get value of Memo
			if (Ret.Memo != null) {
				string Memo6375 = (string)Ret.Memo.GetValue();
			}
			if (Ret.CustomerMsgRef != null) {
				//Get value of ListID
				if (Ret.CustomerMsgRef.ListID != null) {
					string ListID6376 = (string)Ret.CustomerMsgRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.CustomerMsgRef.FullName != null) {
					string FullName6377 = (string)Ret.CustomerMsgRef.FullName.GetValue();
				}
			}
			//Get value of IsToBePrinted
			if (Ret.IsToBePrinted != null) {
				bool IsToBePrinted6378 = (bool)Ret.IsToBePrinted.GetValue();
			}
			//Get value of IsToBeEmailed
			if (Ret.IsToBeEmailed != null) {
				bool IsToBeEmailed6379 = (bool)Ret.IsToBeEmailed.GetValue();
			}
			//Get value of IsTaxIncluded
			if (Ret.IsTaxIncluded != null) {
				bool IsTaxIncluded6380 = (bool)Ret.IsTaxIncluded.GetValue();
			}
			if (Ret.CustomerSalesTaxCodeRef != null) {
				//Get value of ListID
				if (Ret.CustomerSalesTaxCodeRef.ListID != null) {
					string ListID6381 = (string)Ret.CustomerSalesTaxCodeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.CustomerSalesTaxCodeRef.FullName != null) {
					string FullName6382 = (string)Ret.CustomerSalesTaxCodeRef.FullName.GetValue();
				}
			}
			//Get value of Other
			if (Ret.Other != null) {
				string Other6383 = (string)Ret.Other.GetValue();
			}
			//Get value of ExternalGUID
			if (Ret.ExternalGUID != null) {
				string ExternalGUID6384 = (string)Ret.ExternalGUID.GetValue();
			}
			if (Ret.LinkedTxnList != null) {
				for (int i6385 = 0; i6385 < Ret.LinkedTxnList.Count; i6385++) {
					ILinkedTxn LinkedTxn = Ret.LinkedTxnList.GetAt(i6385);
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
			if (Ret.ORLineRetList != null) {
				for (int i6392 = 0; i6392 < Ret.ORLineRetList.Count; i6392++) {
					IORLineRet ORLineRet6393 = Ret.ORLineRetList.GetAt(i6392);
					if (ORLineRet6393.LineRet != null) {
						if (ORLineRet6393.LineRet != null) {
							//Get value of TxnLineID
							string TxnLineID6394 = (string)ORLineRet6393.LineRet.TxnLineID.GetValue();
							if (ORLineRet6393.LineRet.ItemRef != null) {
								//Get value of ListID
								if (ORLineRet6393.LineRet.ItemRef.ListID != null) {
									string ListID6395 = (string)ORLineRet6393.LineRet.ItemRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORLineRet6393.LineRet.ItemRef.FullName != null) {
									string FullName6396 = (string)ORLineRet6393.LineRet.ItemRef.FullName.GetValue();
								}
							}
							//Get value of Desc
							if (ORLineRet6393.LineRet.Desc != null) {
								string Desc6397 = (string)ORLineRet6393.LineRet.Desc.GetValue();
							}
							//Get value of Quantity
							if (ORLineRet6393.LineRet.Quantity != null) {
								int Quantity6398 = (int)ORLineRet6393.LineRet.Quantity.GetValue();
							}
							//Get value of UnitOfMeasure
							if (ORLineRet6393.LineRet.UnitOfMeasure != null) {
								string UnitOfMeasure6399 = (string)ORLineRet6393.LineRet.UnitOfMeasure.GetValue();
							}
							if (ORLineRet6393.LineRet.OverrideUOMSetRef != null) {
								//Get value of ListID
								if (ORLineRet6393.LineRet.OverrideUOMSetRef.ListID != null) {
									string ListID6400 = (string)ORLineRet6393.LineRet.OverrideUOMSetRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORLineRet6393.LineRet.OverrideUOMSetRef.FullName != null) {
									string FullName6401 = (string)ORLineRet6393.LineRet.OverrideUOMSetRef.FullName.GetValue();
								}
							}
							if (ORLineRet6393.LineRet.ORRate != null) {
								if (ORLineRet6393.LineRet.ORRate.Rate != null) {
									//Get value of Rate
									if (ORLineRet6393.LineRet.ORRate.Rate != null) {
										double Rate6403 = (double)ORLineRet6393.LineRet.ORRate.Rate.GetValue();
									}
								}
								if (ORLineRet6393.LineRet.ORRate.RatePercent != null) {
									//Get value of RatePercent
									if (ORLineRet6393.LineRet.ORRate.RatePercent != null) {
										double RatePercent6404 = (double)ORLineRet6393.LineRet.ORRate.RatePercent.GetValue();
									}
								}
							}
							if (ORLineRet6393.LineRet.ClassRef != null) {
								//Get value of ListID
								if (ORLineRet6393.LineRet.ClassRef.ListID != null) {
									string ListID6405 = (string)ORLineRet6393.LineRet.ClassRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORLineRet6393.LineRet.ClassRef.FullName != null) {
									string FullName6406 = (string)ORLineRet6393.LineRet.ClassRef.FullName.GetValue();
								}
							}
							//Get value of Amount
							if (ORLineRet6393.LineRet.Amount != null) {
								double Amount6407 = (double)ORLineRet6393.LineRet.Amount.GetValue();
							}
							if (ORLineRet6393.LineRet.InventorySiteRef != null) {
								//Get value of ListID
								if (ORLineRet6393.LineRet.InventorySiteRef.ListID != null) {
									string ListID6408 = (string)ORLineRet6393.LineRet.InventorySiteRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORLineRet6393.LineRet.InventorySiteRef.FullName != null) {
									string FullName6409 = (string)ORLineRet6393.LineRet.InventorySiteRef.FullName.GetValue();
								}
							}
							if (ORLineRet6393.LineRet.InventorySiteLocationRef != null) {
								//Get value of ListID
								if (ORLineRet6393.LineRet.InventorySiteLocationRef.ListID != null) {
									string ListID6410 = (string)ORLineRet6393.LineRet.InventorySiteLocationRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORLineRet6393.LineRet.InventorySiteLocationRef.FullName != null) {
									string FullName6411 = (string)ORLineRet6393.LineRet.InventorySiteLocationRef.FullName.GetValue();
								}
							}
							if (ORLineRet6393.LineRet.ORSerialLotNumber != null) {
								if (ORLineRet6393.LineRet.ORSerialLotNumber.SerialNumber != null) {
									//Get value of SerialNumber
									if (ORLineRet6393.LineRet.ORSerialLotNumber.SerialNumber != null) {
										string SerialNumber6413 = (string)ORLineRet6393.LineRet.ORSerialLotNumber.SerialNumber.GetValue();
									}
								}
								if (ORLineRet6393.LineRet.ORSerialLotNumber.LotNumber != null) {
									//Get value of LotNumber
									if (ORLineRet6393.LineRet.ORSerialLotNumber.LotNumber != null) {
										string LotNumber6414 = (string)ORLineRet6393.LineRet.ORSerialLotNumber.LotNumber.GetValue();
									}
								}
							}
							//Get value of ServiceDate
							if (ORLineRet6393.LineRet.ServiceDate != null) {
								DateTime ServiceDate6415 = (DateTime)ORLineRet6393.LineRet.ServiceDate.GetValue();
							}
							if (ORLineRet6393.LineRet.SalesTaxCodeRef != null) {
								//Get value of ListID
								if (ORLineRet6393.LineRet.SalesTaxCodeRef.ListID != null) {
									string ListID6416 = (string)ORLineRet6393.LineRet.SalesTaxCodeRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORLineRet6393.LineRet.SalesTaxCodeRef.FullName != null) {
									string FullName6417 = (string)ORLineRet6393.LineRet.SalesTaxCodeRef.FullName.GetValue();
								}
							}
							//Get value of Other1
							if (ORLineRet6393.LineRet.Other1 != null) {
								string Other16418 = (string)ORLineRet6393.LineRet.Other1.GetValue();
							}
							//Get value of Other2
							if (ORLineRet6393.LineRet.Other2 != null) {
								string Other26419 = (string)ORLineRet6393.LineRet.Other2.GetValue();
							}
							if (ORLineRet6393.LineRet.CreditCardTxnInfo != null) {
								//Get value of CreditCardNumber
								string CreditCardNumber6420 = (string)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardNumber.GetValue();
								//Get value of ExpirationMonth
								int ExpirationMonth6421 = (int)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationMonth.GetValue();
								//Get value of ExpirationYear
								int ExpirationYear6422 = (int)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationYear.GetValue();
								//Get value of NameOnCard
								string NameOnCard6423 = (string)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.NameOnCard.GetValue();
								//Get value of CreditCardAddress
								if (ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress != null) {
									string CreditCardAddress6424 = (string)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress.GetValue();
								}
								//Get value of CreditCardPostalCode
								if (ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode != null) {
									string CreditCardPostalCode6425 = (string)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode.GetValue();
								}
								//Get value of CommercialCardCode
								if (ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode != null) {
									string CommercialCardCode6426 = (string)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode.GetValue();
								}
								//Get value of TransactionMode
								if (ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode != null) {
									ENTransactionMode TransactionMode6427 = (ENTransactionMode)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode.GetValue();
								}
								//Get value of CreditCardTxnType
								if (ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType != null) {
									ENCreditCardTxnType CreditCardTxnType6428 = (ENCreditCardTxnType)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType.GetValue();
								}
								//Get value of ResultCode
								int ResultCode6429 = (int)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ResultCode.GetValue();
								//Get value of ResultMessage
								string ResultMessage6430 = (string)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ResultMessage.GetValue();
								//Get value of CreditCardTransID
								string CreditCardTransID6431 = (string)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.CreditCardTransID.GetValue();
								//Get value of MerchantAccountNumber
								string MerchantAccountNumber6432 = (string)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.MerchantAccountNumber.GetValue();
								//Get value of AuthorizationCode
								if (ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode != null) {
									string AuthorizationCode6433 = (string)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode.GetValue();
								}
								//Get value of AVSStreet
								if (ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet != null) {
									ENAVSStreet AVSStreet6434 = (ENAVSStreet)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet.GetValue();
								}
								//Get value of AVSZip
								if (ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip != null) {
									ENAVSZip AVSZip6435 = (ENAVSZip)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip.GetValue();
								}
								//Get value of CardSecurityCodeMatch
								if (ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch != null) {
									ENCardSecurityCodeMatch CardSecurityCodeMatch6436 = (ENCardSecurityCodeMatch)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch.GetValue();
								}
								//Get value of ReconBatchID
								if (ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchID != null) {
									string ReconBatchID6437 = (string)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchID.GetValue();
								}
								//Get value of PaymentGroupingCode
								if (ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode != null) {
									int PaymentGroupingCode6438 = (int)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode.GetValue();
								}
								//Get value of PaymentStatus
								ENPaymentStatus PaymentStatus6439 = (ENPaymentStatus)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentStatus.GetValue();
								//Get value of TxnAuthorizationTime
								DateTime TxnAuthorizationTime6440 = (DateTime)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationTime.GetValue();
								//Get value of TxnAuthorizationStamp
								if (ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp != null) {
									int TxnAuthorizationStamp6441 = (int)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp.GetValue();
								}
								//Get value of ClientTransID
								if (ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID != null) {
									string ClientTransID6442 = (string)ORLineRet6393.LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID.GetValue();
								}
							}
							if (ORLineRet6393.LineRet.DataExtRetList != null) {
								for (int i6443 = 0; i6443 < ORLineRet6393.LineRet.DataExtRetList.Count; i6443++) {
									IDataExtRet DataExtRet = ORLineRet6393.LineRet.DataExtRetList.GetAt(i6443);
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
					if (ORLineRet6393.LineGroupRet != null) {
						if (ORLineRet6393.LineGroupRet != null) {
							//Get value of TxnLineID
							string TxnLineID6448 = (string)ORLineRet6393.LineGroupRet.TxnLineID.GetValue();
							//Get value of ListID
							if (ORLineRet6393.LineGroupRet.ItemGroupRef.ListID != null) {
								string ListID6449 = (string)ORLineRet6393.LineGroupRet.ItemGroupRef.ListID.GetValue();
							}
							//Get value of FullName
							if (ORLineRet6393.LineGroupRet.ItemGroupRef.FullName != null) {
								string FullName6450 = (string)ORLineRet6393.LineGroupRet.ItemGroupRef.FullName.GetValue();
							}
							//Get value of Desc
							if (ORLineRet6393.LineGroupRet.Desc != null) {
								string Desc6451 = (string)ORLineRet6393.LineGroupRet.Desc.GetValue();
							}
							//Get value of Quantity
							if (ORLineRet6393.LineGroupRet.Quantity != null) {
								int Quantity6452 = (int)ORLineRet6393.LineGroupRet.Quantity.GetValue();
							}
							//Get value of UnitOfMeasure
							if (ORLineRet6393.LineGroupRet.UnitOfMeasure != null) {
								string UnitOfMeasure6453 = (string)ORLineRet6393.LineGroupRet.UnitOfMeasure.GetValue();
							}
							if (ORLineRet6393.LineGroupRet.OverrideUOMSetRef != null) {
								//Get value of ListID
								if (ORLineRet6393.LineGroupRet.OverrideUOMSetRef.ListID != null) {
									string ListID6454 = (string)ORLineRet6393.LineGroupRet.OverrideUOMSetRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORLineRet6393.LineGroupRet.OverrideUOMSetRef.FullName != null) {
									string FullName6455 = (string)ORLineRet6393.LineGroupRet.OverrideUOMSetRef.FullName.GetValue();
								}
							}
							//Get value of IsPrintItemsInGroup
							bool IsPrintItemsInGroup6456 = (bool)ORLineRet6393.LineGroupRet.IsPrintItemsInGroup.GetValue();
							//Get value of TotalAmount
							double TotalAmount6457 = (double)ORLineRet6393.LineGroupRet.TotalAmount.GetValue();
							if (ORLineRet6393.LineGroupRet.LineRetList != null) {
								for (int i6458 = 0; i6458 < ORLineRet6393.LineGroupRet.LineRetList.Count; i6458++) {
									ILineRet LineRet = ORLineRet6393.LineGroupRet.LineRetList.GetAt(i6458);
									//Get value of TxnLineID
									string TxnLineID6459 = (string)LineRet.TxnLineID.GetValue();
									if (LineRet.ItemRef != null) {
										//Get value of ListID
										if (LineRet.ItemRef.ListID != null) {
											string ListID6460 = (string)LineRet.ItemRef.ListID.GetValue();
										}
										//Get value of FullName
										if (LineRet.ItemRef.FullName != null) {
											string FullName6461 = (string)LineRet.ItemRef.FullName.GetValue();
										}
									}
									//Get value of Desc
									if (LineRet.Desc != null) {
										string Desc6462 = (string)LineRet.Desc.GetValue();
									}
									//Get value of Quantity
									if (LineRet.Quantity != null) {
										int Quantity6463 = (int)LineRet.Quantity.GetValue();
									}
									//Get value of UnitOfMeasure
									if (LineRet.UnitOfMeasure != null) {
										string UnitOfMeasure6464 = (string)LineRet.UnitOfMeasure.GetValue();
									}
									if (LineRet.OverrideUOMSetRef != null) {
										//Get value of ListID
										if (LineRet.OverrideUOMSetRef.ListID != null) {
											string ListID6465 = (string)LineRet.OverrideUOMSetRef.ListID.GetValue();
										}
										//Get value of FullName
										if (LineRet.OverrideUOMSetRef.FullName != null) {
											string FullName6466 = (string)LineRet.OverrideUOMSetRef.FullName.GetValue();
										}
									}
									if (LineRet.ORRate != null) {
										if (LineRet.ORRate.Rate != null) {
											//Get value of Rate
											if (LineRet.ORRate.Rate != null) {
												double Rate6468 = (double)LineRet.ORRate.Rate.GetValue();
											}
										}
										if (LineRet.ORRate.RatePercent != null) {
											//Get value of RatePercent
											if (LineRet.ORRate.RatePercent != null) {
												double RatePercent6469 = (double)LineRet.ORRate.RatePercent.GetValue();
											}
										}
									}
									if (LineRet.ClassRef != null) {
										//Get value of ListID
										if (LineRet.ClassRef.ListID != null) {
											string ListID6470 = (string)LineRet.ClassRef.ListID.GetValue();
										}
										//Get value of FullName
										if (LineRet.ClassRef.FullName != null) {
											string FullName6471 = (string)LineRet.ClassRef.FullName.GetValue();
										}
									}
									//Get value of Amount
									if (LineRet.Amount != null) {
										double Amount6472 = (double)LineRet.Amount.GetValue();
									}
									if (LineRet.InventorySiteRef != null) {
										//Get value of ListID
										if (LineRet.InventorySiteRef.ListID != null) {
											string ListID6473 = (string)LineRet.InventorySiteRef.ListID.GetValue();
										}
										//Get value of FullName
										if (LineRet.InventorySiteRef.FullName != null) {
											string FullName6474 = (string)LineRet.InventorySiteRef.FullName.GetValue();
										}
									}
									if (LineRet.InventorySiteLocationRef != null) {
										//Get value of ListID
										if (LineRet.InventorySiteLocationRef.ListID != null) {
											string ListID6475 = (string)LineRet.InventorySiteLocationRef.ListID.GetValue();
										}
										//Get value of FullName
										if (LineRet.InventorySiteLocationRef.FullName != null) {
											string FullName6476 = (string)LineRet.InventorySiteLocationRef.FullName.GetValue();
										}
									}
									if (LineRet.ORSerialLotNumber != null) {
										if (LineRet.ORSerialLotNumber.SerialNumber != null) {
											//Get value of SerialNumber
											if (LineRet.ORSerialLotNumber.SerialNumber != null) {
												string SerialNumber6478 = (string)LineRet.ORSerialLotNumber.SerialNumber.GetValue();
											}
										}
										if (LineRet.ORSerialLotNumber.LotNumber != null) {
											//Get value of LotNumber
											if (LineRet.ORSerialLotNumber.LotNumber != null) {
												string LotNumber6479 = (string)LineRet.ORSerialLotNumber.LotNumber.GetValue();
											}
										}
									}
									//Get value of ServiceDate
									if (LineRet.ServiceDate != null) {
										DateTime ServiceDate6480 = (DateTime)LineRet.ServiceDate.GetValue();
									}
									if (LineRet.SalesTaxCodeRef != null) {
										//Get value of ListID
										if (LineRet.SalesTaxCodeRef.ListID != null) {
											string ListID6481 = (string)LineRet.SalesTaxCodeRef.ListID.GetValue();
										}
										//Get value of FullName
										if (LineRet.SalesTaxCodeRef.FullName != null) {
											string FullName6482 = (string)LineRet.SalesTaxCodeRef.FullName.GetValue();
										}
									}
									//Get value of Other1
									if (LineRet.Other1 != null) {
										string Other16483 = (string)LineRet.Other1.GetValue();
									}
									//Get value of Other2
									if (LineRet.Other2 != null) {
										string Other26484 = (string)LineRet.Other2.GetValue();
									}
									if (LineRet.CreditCardTxnInfo != null) {
										//Get value of CreditCardNumber
										string CreditCardNumber6485 = (string)LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardNumber.GetValue();
										//Get value of ExpirationMonth
										int ExpirationMonth6486 = (int)LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationMonth.GetValue();
										//Get value of ExpirationYear
										int ExpirationYear6487 = (int)LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationYear.GetValue();
										//Get value of NameOnCard
										string NameOnCard6488 = (string)LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.NameOnCard.GetValue();
										//Get value of CreditCardAddress
										if (LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress != null) {
											string CreditCardAddress6489 = (string)LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress.GetValue();
										}
										//Get value of CreditCardPostalCode
										if (LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode != null) {
											string CreditCardPostalCode6490 = (string)LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode.GetValue();
										}
										//Get value of CommercialCardCode
										if (LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode != null) {
											string CommercialCardCode6491 = (string)LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode.GetValue();
										}
										//Get value of TransactionMode
										if (LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode != null) {
											ENTransactionMode TransactionMode6492 = (ENTransactionMode)LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode.GetValue();
										}
										//Get value of CreditCardTxnType
										if (LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType != null) {
											ENCreditCardTxnType CreditCardTxnType6493 = (ENCreditCardTxnType)LineRet.CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType.GetValue();
										}
										//Get value of ResultCode
										int ResultCode6494 = (int)LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ResultCode.GetValue();
										//Get value of ResultMessage
										string ResultMessage6495 = (string)LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ResultMessage.GetValue();
										//Get value of CreditCardTransID
										string CreditCardTransID6496 = (string)LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.CreditCardTransID.GetValue();
										//Get value of MerchantAccountNumber
										string MerchantAccountNumber6497 = (string)LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.MerchantAccountNumber.GetValue();
										//Get value of AuthorizationCode
										if (LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode != null) {
											string AuthorizationCode6498 = (string)LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode.GetValue();
										}
										//Get value of AVSStreet
										if (LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet != null) {
											ENAVSStreet AVSStreet6499 = (ENAVSStreet)LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet.GetValue();
										}
										//Get value of AVSZip
										if (LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip != null) {
											ENAVSZip AVSZip6500 = (ENAVSZip)LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip.GetValue();
										}
										//Get value of CardSecurityCodeMatch
										if (LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch != null) {
											ENCardSecurityCodeMatch CardSecurityCodeMatch6501 = (ENCardSecurityCodeMatch)LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch.GetValue();
										}
										//Get value of ReconBatchID
										if (LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchID != null) {
											string ReconBatchID6502 = (string)LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchID.GetValue();
										}
										//Get value of PaymentGroupingCode
										if (LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode != null) {
											int PaymentGroupingCode6503 = (int)LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode.GetValue();
										}
										//Get value of PaymentStatus
										ENPaymentStatus PaymentStatus6504 = (ENPaymentStatus)LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentStatus.GetValue();
										//Get value of TxnAuthorizationTime
										DateTime TxnAuthorizationTime6505 = (DateTime)LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationTime.GetValue();
										//Get value of TxnAuthorizationStamp
										if (LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp != null) {
											int TxnAuthorizationStamp6506 = (int)LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp.GetValue();
										}
										//Get value of ClientTransID
										if (LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID != null) {
											string ClientTransID6507 = (string)LineRet.CreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID.GetValue();
										}
									}
									if (LineRet.DataExtRetList != null) {
										for (int i6508 = 0; i6508 < LineRet.DataExtRetList.Count; i6508++) {
											IDataExtRet DataExtRet = LineRet.DataExtRetList.GetAt(i6508);
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
							if (ORLineRet6393.LineGroupRet.DataExtRetList != null) {
								for (int i6513 = 0; i6513 < ORLineRet6393.LineGroupRet.DataExtRetList.Count; i6513++) {
									IDataExtRet DataExtRet = ORLineRet6393.LineGroupRet.DataExtRetList.GetAt(i6513);
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
			if (Ret.DataExtRetList != null) {
				for (int i6518 = 0; i6518 < Ret.DataExtRetList.Count; i6518++) {
					IDataExtRet DataExtRet = Ret.DataExtRetList.GetAt(i6518);
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