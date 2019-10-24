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
			string ORTxnQueryElementType16581 = "TxnIDList";
			if (ORTxnQueryElementType16581 == "TxnIDList") {
				//Set field value for TxnIDList
				//May create more than one of these if needed
				QueryRq.ORTxnQuery.TxnIDList.Add("200000-1011023419");
			}
			if (ORTxnQueryElementType16581 == "RefNumberList") {
				//Set field value for RefNumberList
				//May create more than one of these if needed
				QueryRq.ORTxnQuery.RefNumberList.Add("ab");
			}
			if (ORTxnQueryElementType16581 == "RefNumberCaseSensitiveList") {
				//Set field value for RefNumberCaseSensitiveList
				//May create more than one of these if needed
				QueryRq.ORTxnQuery.RefNumberCaseSensitiveList.Add("ab");
			}
			if (ORTxnQueryElementType16581 == "TxnFilter") {
				//Set field value for MaxReturned
				QueryRq.ORTxnQuery.TxnFilter.MaxReturned.SetValue(6);
				string ORDateRangeFilterElementType16582 = "ModifiedDateRangeFilter";
				if (ORDateRangeFilterElementType16582 == "ModifiedDateRangeFilter") {
					//Set field value for FromModifiedDate
					QueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.ModifiedDateRangeFilter.FromModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
					//Set field value for ToModifiedDate
					QueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.ModifiedDateRangeFilter.ToModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				}
				if (ORDateRangeFilterElementType16582 == "TxnDateRangeFilter") {
					string ORTxnDateRangeFilterElementType16583 = "TxnDateFilter";
					if (ORTxnDateRangeFilterElementType16583 == "TxnDateFilter") {
						//Set field value for FromTxnDate
						QueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.FromTxnDate.SetValue(DateTime.Parse("12/15/2007"));
						//Set field value for ToTxnDate
						QueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.ToTxnDate.SetValue(DateTime.Parse("12/15/2007"));
					}
					if (ORTxnDateRangeFilterElementType16583 == "DateMacro") {
						//Set field value for DateMacro
						QueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.DateMacro.SetValue(ENDateMacro.dmAll);
					}
				}
				string OREntityFilterElementType16584 = "ListIDList";
				if (OREntityFilterElementType16584 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					QueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.ListIDList.Add("200000-1011023419");
				}
				if (OREntityFilterElementType16584 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					QueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.FullNameList.Add("ab");
				}
				if (OREntityFilterElementType16584 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					QueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (OREntityFilterElementType16584 == "FullNameWithChildren") {
					//Set field value for FullNameWithChildren
					QueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.FullNameWithChildren.SetValue("ab");
				}
				string ORAccountFilterElementType16585 = "ListIDList";
				if (ORAccountFilterElementType16585 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					QueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORAccountFilterElementType16585 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					QueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.FullNameList.Add("ab");
				}
				if (ORAccountFilterElementType16585 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					QueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (ORAccountFilterElementType16585 == "FullNameWithChildren") {
					//Set field value for FullNameWithChildren
					QueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.FullNameWithChildren.SetValue("ab");
				}
				string ORRefNumberFilterElementType16586 = "RefNumberFilter";
				if (ORRefNumberFilterElementType16586 == "RefNumberFilter") {
					//Set field value for MatchCriterion
					QueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberFilter.MatchCriterion.SetValue(ENMatchCriterion.mcStartsWith);
					//Set field value for RefNumber
					QueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberFilter.RefNumber.SetValue("ab");
				}
				if (ORRefNumberFilterElementType16586 == "RefNumberRangeFilter") {
					//Set field value for FromRefNumber
					QueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberRangeFilter.FromRefNumber.SetValue("ab");
					//Set field value for ToRefNumber
					QueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberRangeFilter.ToRefNumber.SetValue("ab");
				}
				string ORCurrencyFilterElementType16587 = "ListIDList";
				if (ORCurrencyFilterElementType16587 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					QueryRq.ORTxnQuery.TxnFilter.CurrencyFilter.ORCurrencyFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORCurrencyFilterElementType16587 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					QueryRq.ORTxnQuery.TxnFilter.CurrencyFilter.ORCurrencyFilter.FullNameList.Add("ab");
				}
			}
			//Set field value for IncludeLineItems
			QueryRq.IncludeLineItems.SetValue(true);
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
			string TxnID16588 = (string)Ret.TxnID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated16589 = (DateTime)Ret.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified16590 = (DateTime)Ret.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence16591 = (string)Ret.EditSequence.GetValue();
			//Get value of TxnNumber
			if (Ret.TxnNumber != null) {
				int TxnNumber16592 = (int)Ret.TxnNumber.GetValue();
			}
			//Get value of TxnDate
			DateTime TxnDate16593 = (DateTime)Ret.TxnDate.GetValue();
			//Get value of RefNumber
			if (Ret.RefNumber != null) {
				string RefNumber16594 = (string)Ret.RefNumber.GetValue();
			}
			//Get value of IsAdjustment
			if (Ret.IsAdjustment != null) {
				bool IsAdjustment16595 = (bool)Ret.IsAdjustment.GetValue();
			}
			//Get value of IsHomeCurrencyAdjustment
			if (Ret.IsHomeCurrencyAdjustment != null) {
				bool IsHomeCurrencyAdjustment16596 = (bool)Ret.IsHomeCurrencyAdjustment.GetValue();
			}
			//Get value of IsAmountsEnteredInHomeCurrency
			if (Ret.IsAmountsEnteredInHomeCurrency != null) {
				bool IsAmountsEnteredInHomeCurrency16597 = (bool)Ret.IsAmountsEnteredInHomeCurrency.GetValue();
			}
			if (Ret.CurrencyRef != null) {
				//Get value of ListID
				if (Ret.CurrencyRef.ListID != null) {
					string ListID16598 = (string)Ret.CurrencyRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.CurrencyRef.FullName != null) {
					string FullName16599 = (string)Ret.CurrencyRef.FullName.GetValue();
				}
			}
			//Get value of ExchangeRate
			if (Ret.ExchangeRate != null) {
				IQBFloatType ExchangeRate16600 = (IQBFloatType)Ret.ExchangeRate.GetValue();
			}
			//Get value of ExternalGUID
			if (Ret.ExternalGUID != null) {
				string ExternalGUID16601 = (string)Ret.ExternalGUID.GetValue();
			}
			if (Ret.ORJournalLineList != null) {
				for (int i16602 = 0; i16602 < Ret.ORJournalLineList.Count; i16602++) {
					IORJournalLine ORJournalLine16603 = Ret.ORJournalLineList.GetAt(i16602);
					if (ORJournalLine16603.JournalDebitLine != null) {
						if (ORJournalLine16603.JournalDebitLine != null) {
							//Get value of TxnLineID
							if (ORJournalLine16603.JournalDebitLine.TxnLineID != null) {
								string TxnLineID16604 = (string)ORJournalLine16603.JournalDebitLine.TxnLineID.GetValue();
							}
							if (ORJournalLine16603.JournalDebitLine.AccountRef != null) {
								//Get value of ListID
								if (ORJournalLine16603.JournalDebitLine.AccountRef.ListID != null) {
									string ListID16605 = (string)ORJournalLine16603.JournalDebitLine.AccountRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORJournalLine16603.JournalDebitLine.AccountRef.FullName != null) {
									string FullName16606 = (string)ORJournalLine16603.JournalDebitLine.AccountRef.FullName.GetValue();
								}
							}
							//Get value of Amount
							if (ORJournalLine16603.JournalDebitLine.Amount != null) {
								double Amount16607 = (double)ORJournalLine16603.JournalDebitLine.Amount.GetValue();
							}
							//Get value of Memo
							if (ORJournalLine16603.JournalDebitLine.Memo != null) {
								string Memo16608 = (string)ORJournalLine16603.JournalDebitLine.Memo.GetValue();
							}
							if (ORJournalLine16603.JournalDebitLine.EntityRef != null) {
								//Get value of ListID
								if (ORJournalLine16603.JournalDebitLine.EntityRef.ListID != null) {
									string ListID16609 = (string)ORJournalLine16603.JournalDebitLine.EntityRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORJournalLine16603.JournalDebitLine.EntityRef.FullName != null) {
									string FullName16610 = (string)ORJournalLine16603.JournalDebitLine.EntityRef.FullName.GetValue();
								}
							}
							if (ORJournalLine16603.JournalDebitLine.ClassRef != null) {
								//Get value of ListID
								if (ORJournalLine16603.JournalDebitLine.ClassRef.ListID != null) {
									string ListID16611 = (string)ORJournalLine16603.JournalDebitLine.ClassRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORJournalLine16603.JournalDebitLine.ClassRef.FullName != null) {
									string FullName16612 = (string)ORJournalLine16603.JournalDebitLine.ClassRef.FullName.GetValue();
								}
							}
							if (ORJournalLine16603.JournalDebitLine.ItemSalesTaxRef != null) {
								//Get value of ListID
								if (ORJournalLine16603.JournalDebitLine.ItemSalesTaxRef.ListID != null) {
									string ListID16613 = (string)ORJournalLine16603.JournalDebitLine.ItemSalesTaxRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORJournalLine16603.JournalDebitLine.ItemSalesTaxRef.FullName != null) {
									string FullName16614 = (string)ORJournalLine16603.JournalDebitLine.ItemSalesTaxRef.FullName.GetValue();
								}
							}
							//Get value of BillableStatus
							if (ORJournalLine16603.JournalDebitLine.BillableStatus != null) {
								ENBillableStatus BillableStatus16615 = (ENBillableStatus)ORJournalLine16603.JournalDebitLine.BillableStatus.GetValue();
							}
						}
					}
					if (ORJournalLine16603.JournalCreditLine != null) {
						if (ORJournalLine16603.JournalCreditLine != null) {
							//Get value of TxnLineID
							if (ORJournalLine16603.JournalCreditLine.TxnLineID != null) {
								string TxnLineID16616 = (string)ORJournalLine16603.JournalCreditLine.TxnLineID.GetValue();
							}
							if (ORJournalLine16603.JournalCreditLine.AccountRef != null) {
								//Get value of ListID
								if (ORJournalLine16603.JournalCreditLine.AccountRef.ListID != null) {
									string ListID16617 = (string)ORJournalLine16603.JournalCreditLine.AccountRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORJournalLine16603.JournalCreditLine.AccountRef.FullName != null) {
									string FullName16618 = (string)ORJournalLine16603.JournalCreditLine.AccountRef.FullName.GetValue();
								}
							}
							//Get value of Amount
							if (ORJournalLine16603.JournalCreditLine.Amount != null) {
								double Amount16619 = (double)ORJournalLine16603.JournalCreditLine.Amount.GetValue();
							}
							//Get value of Memo
							if (ORJournalLine16603.JournalCreditLine.Memo != null) {
								string Memo16620 = (string)ORJournalLine16603.JournalCreditLine.Memo.GetValue();
							}
							if (ORJournalLine16603.JournalCreditLine.EntityRef != null) {
								//Get value of ListID
								if (ORJournalLine16603.JournalCreditLine.EntityRef.ListID != null) {
									string ListID16621 = (string)ORJournalLine16603.JournalCreditLine.EntityRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORJournalLine16603.JournalCreditLine.EntityRef.FullName != null) {
									string FullName16622 = (string)ORJournalLine16603.JournalCreditLine.EntityRef.FullName.GetValue();
								}
							}
							if (ORJournalLine16603.JournalCreditLine.ClassRef != null) {
								//Get value of ListID
								if (ORJournalLine16603.JournalCreditLine.ClassRef.ListID != null) {
									string ListID16623 = (string)ORJournalLine16603.JournalCreditLine.ClassRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORJournalLine16603.JournalCreditLine.ClassRef.FullName != null) {
									string FullName16624 = (string)ORJournalLine16603.JournalCreditLine.ClassRef.FullName.GetValue();
								}
							}
							if (ORJournalLine16603.JournalCreditLine.ItemSalesTaxRef != null) {
								//Get value of ListID
								if (ORJournalLine16603.JournalCreditLine.ItemSalesTaxRef.ListID != null) {
									string ListID16625 = (string)ORJournalLine16603.JournalCreditLine.ItemSalesTaxRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORJournalLine16603.JournalCreditLine.ItemSalesTaxRef.FullName != null) {
									string FullName16626 = (string)ORJournalLine16603.JournalCreditLine.ItemSalesTaxRef.FullName.GetValue();
								}
							}
							//Get value of BillableStatus
							if (ORJournalLine16603.JournalCreditLine.BillableStatus != null) {
								ENBillableStatus BillableStatus16627 = (ENBillableStatus)ORJournalLine16603.JournalCreditLine.BillableStatus.GetValue();
							}
						}
					}
				}
			}
			if (Ret.DataExtRetList != null) {
				for (int i16628 = 0; i16628 < Ret.DataExtRetList.Count; i16628++) {
					IDataExtRet DataExtRet = Ret.DataExtRetList.GetAt(i16628);
					//Get value of OwnerID
					if (DataExtRet.OwnerID != null) {
						string OwnerID16629 = (string)DataExtRet.OwnerID.GetValue();
					}
					//Get value of DataExtName
					string DataExtName16630 = (string)DataExtRet.DataExtName.GetValue();
					//Get value of DataExtType
					ENDataExtType DataExtType16631 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
					//Get value of DataExtValue
					string DataExtValue16632 = (string)DataExtRet.DataExtValue.GetValue();
				}
			}
		}
	}
}