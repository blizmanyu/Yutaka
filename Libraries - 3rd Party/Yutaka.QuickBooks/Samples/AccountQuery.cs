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
			string ORListQueryElementType433 = "ListIDList";
			if (ORListQueryElementType433 == "ListIDList") {
				//Set field value for ListIDList
				//May create more than one of these if needed
				QueryRq.ORListQuery.ListIDList.Add("200000-1011023419");
			}
			if (ORListQueryElementType433 == "FullNameList") {
				//Set field value for FullNameList
				//May create more than one of these if needed
				QueryRq.ORListQuery.FullNameList.Add("ab");
			}
			if (ORListQueryElementType433 == "ListFilter") {
				//Set field value for MaxReturned
				QueryRq.ORListQuery.ListFilter.MaxReturned.SetValue(6);
				//Set field value for ActiveStatus
				QueryRq.ORListQuery.ListFilter.ActiveStatus.SetValue(ENActiveStatus.asActiveOnly[DEFAULT]);
				//Set field value for FromModifiedDate
				QueryRq.ORListQuery.ListFilter.FromModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				//Set field value for ToModifiedDate
				QueryRq.ORListQuery.ListFilter.ToModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				string ORNameFilterElementType434 = "NameFilter";
				if (ORNameFilterElementType434 == "NameFilter") {
					//Set field value for MatchCriterion
					QueryRq.ORListQuery.ListFilter.ORNameFilter.NameFilter.MatchCriterion.SetValue(ENMatchCriterion.mcStartsWith);
					//Set field value for Name
					QueryRq.ORListQuery.ListFilter.ORNameFilter.NameFilter.Name.SetValue("ab");
				}
				if (ORNameFilterElementType434 == "NameRangeFilter") {
					//Set field value for FromName
					QueryRq.ORListQuery.ListFilter.ORNameFilter.NameRangeFilter.FromName.SetValue("ab");
					//Set field value for ToName
					QueryRq.ORListQuery.ListFilter.ORNameFilter.NameRangeFilter.ToName.SetValue("ab");
				}
				//Set field value for TypeList
				//May create more than one of these if needed
				QueryRq.ORListQuery.ListFilter.TypeList.Add(ENTypeList.atlsPayable);
				string ORCurrencyFilterElementType435 = "ListIDList";
				if (ORCurrencyFilterElementType435 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					QueryRq.ORListQuery.ListFilter.CurrencyFilter.ORCurrencyFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORCurrencyFilterElementType435 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					QueryRq.ORListQuery.ListFilter.CurrencyFilter.ORCurrencyFilter.FullNameList.Add("ab");
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
			string ListID436 = (string)Ret.ListID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated437 = (DateTime)Ret.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified438 = (DateTime)Ret.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence439 = (string)Ret.EditSequence.GetValue();
			//Get value of Name
			string Name440 = (string)Ret.Name.GetValue();
			//Get value of FullName
			string FullName441 = (string)Ret.FullName.GetValue();
			//Get value of IsActive
			if (Ret.IsActive != null) {
				bool IsActive442 = (bool)Ret.IsActive.GetValue();
			}
			if (Ret.ParentRef != null) {
				//Get value of ListID
				if (Ret.ParentRef.ListID != null) {
					string ListID443 = (string)Ret.ParentRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ParentRef.FullName != null) {
					string FullName444 = (string)Ret.ParentRef.FullName.GetValue();
				}
			}
			//Get value of Sublevel
			int Sublevel445 = (int)Ret.Sublevel.GetValue();
			//Get value of Type
			ENType Type446 = (ENType)Ret.Type.GetValue();
			//Get value of SpecialType
			if (Ret.SpecialType != null) {
				ENSpecialType SpecialType447 = (ENSpecialType)Ret.SpecialType.GetValue();
			}
			//Get value of IsTax
			if (Ret.IsTax != null) {
				bool IsTax448 = (bool)Ret.IsTax.GetValue();
			}
			//Get value of Number
			if (Ret.Number != null) {
				string Number449 = (string)Ret.Number.GetValue();
			}
			//Get value of BankNumber
			if (Ret.BankNumber != null) {
				string BankNumber450 = (string)Ret.BankNumber.GetValue();
			}
			//Get value of Desc
			if (Ret.Desc != null) {
				string Desc451 = (string)Ret.Desc.GetValue();
			}
			//Get value of Balance
			if (Ret.Balance != null) {
				double Balance452 = (double)Ret.Balance.GetValue();
			}
			//Get value of TotalBalance
			if (Ret.TotalBalance != null) {
				double TotalBalance453 = (double)Ret.TotalBalance.GetValue();
			}
			if (Ret.SalesTaxCodeRef != null) {
				//Get value of ListID
				if (Ret.SalesTaxCodeRef.ListID != null) {
					string ListID454 = (string)Ret.SalesTaxCodeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.SalesTaxCodeRef.FullName != null) {
					string FullName455 = (string)Ret.SalesTaxCodeRef.FullName.GetValue();
				}
			}
			if (Ret.TaxLineInfoRet != null) {
				//Get value of TaxLineID
				int TaxLineID456 = (int)Ret.TaxLineInfoRet.TaxLineID.GetValue();
				//Get value of TaxLineName
				if (Ret.TaxLineInfoRet.TaxLineName != null) {
					string TaxLineName457 = (string)Ret.TaxLineInfoRet.TaxLineName.GetValue();
				}
			}
			//Get value of CashFlowClassification
			if (Ret.CashFlowClassification != null) {
				ENCashFlowClassification CashFlowClassification458 = (ENCashFlowClassification)Ret.CashFlowClassification.GetValue();
			}
			if (Ret.CurrencyRef != null) {
				//Get value of ListID
				if (Ret.CurrencyRef.ListID != null) {
					string ListID459 = (string)Ret.CurrencyRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.CurrencyRef.FullName != null) {
					string FullName460 = (string)Ret.CurrencyRef.FullName.GetValue();
				}
			}
			if (Ret.DataExtRetList != null) {
				for (int i461 = 0; i461 < Ret.DataExtRetList.Count; i461++) {
					IDataExtRet DataExtRet = Ret.DataExtRetList.GetAt(i461);
					//Get value of OwnerID
					if (DataExtRet.OwnerID != null) {
						string OwnerID462 = (string)DataExtRet.OwnerID.GetValue();
					}
					//Get value of DataExtName
					string DataExtName463 = (string)DataExtRet.DataExtName.GetValue();
					//Get value of DataExtType
					ENDataExtType DataExtType464 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
					//Get value of DataExtValue
					string DataExtValue465 = (string)DataExtRet.DataExtValue.GetValue();
				}
			}
		}
	}
}