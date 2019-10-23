using System;
using Interop.QBFC13;

namespace com.intuit.idn.samples
{
	public partial class Sample
	{
		public void DoInventoryAdjustmentQuery()
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

				BuildInventoryAdjustmentQueryRq(requestMsgSet);

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

				WalkInventoryAdjustmentQueryRs(responseMsgSet);
			}
			catch (Exception e) {
				//MessageBox.Show(e.Message, "Error");
				if (sessionBegun) {
					sessionManager.EndSession();
				}
				if (connectionOpen) {
					sessionManager.CloseConnection();
				}
			}
		}

		void BuildInventoryAdjustmentQueryRq(IMsgSetRequest requestMsgSet)
		{
			IInventoryAdjustmentQuery InventoryAdjustmentQueryRq= requestMsgSet.AppendInventoryAdjustmentQueryRq();
			//Set attributes
			//Set field value for metaData
			//InventoryAdjustmentQueryRq.metaData.SetValue("IQBENmetaDataType");
			//Set field value for iterator
			//InventoryAdjustmentQueryRq.iterator.SetValue("IQBENiteratorType");
			//Set field value for iteratorID
			InventoryAdjustmentQueryRq.iteratorID.SetValue("IQBUUIDType");
			string ORInventoryAdjustmentQueryElementType10903 = "TxnIDList";
			if (ORInventoryAdjustmentQueryElementType10903 == "TxnIDList") {
				//Set field value for TxnIDList
				//May create more than one of these if needed
				InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnIDList.Add("200000-1011023419");
			}
			if (ORInventoryAdjustmentQueryElementType10903 == "RefNumberList") {
				//Set field value for RefNumberList
				//May create more than one of these if needed
				InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.RefNumberList.Add("ab");
			}
			if (ORInventoryAdjustmentQueryElementType10903 == "RefNumberCaseSensitiveList") {
				//Set field value for RefNumberCaseSensitiveList
				//May create more than one of these if needed
				InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.RefNumberCaseSensitiveList.Add("ab");
			}
			if (ORInventoryAdjustmentQueryElementType10903 == "TxnFilterWithItemFilter") {
				//Set field value for MaxReturned
				InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.MaxReturned.SetValue(6);
				string ORDateRangeFilterElementType10904 = "ModifiedDateRangeFilter";
				if (ORDateRangeFilterElementType10904 == "ModifiedDateRangeFilter") {
					//Set field value for FromModifiedDate
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.ORDateRangeFilter.ModifiedDateRangeFilter.FromModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
					//Set field value for ToModifiedDate
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.ORDateRangeFilter.ModifiedDateRangeFilter.ToModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				}
				if (ORDateRangeFilterElementType10904 == "TxnDateRangeFilter") {
					string ORTxnDateRangeFilterElementType10905 = "TxnDateFilter";
					if (ORTxnDateRangeFilterElementType10905 == "TxnDateFilter") {
						//Set field value for FromTxnDate
						InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.FromTxnDate.SetValue(DateTime.Parse("12/15/2007"));
						//Set field value for ToTxnDate
						InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.ToTxnDate.SetValue(DateTime.Parse("12/15/2007"));
					}
					if (ORTxnDateRangeFilterElementType10905 == "DateMacro") {
						//Set field value for DateMacro
						InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.DateMacro.SetValue(ENDateMacro.dmAll);
					}
				}
				string OREntityFilterElementType10906 = "ListIDList";
				if (OREntityFilterElementType10906 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.EntityFilter.OREntityFilter.ListIDList.Add("200000-1011023419");
				}
				if (OREntityFilterElementType10906 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.EntityFilter.OREntityFilter.FullNameList.Add("ab");
				}
				if (OREntityFilterElementType10906 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.EntityFilter.OREntityFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (OREntityFilterElementType10906 == "FullNameWithChildren") {
					//Set field value for FullNameWithChildren
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.EntityFilter.OREntityFilter.FullNameWithChildren.SetValue("ab");
				}
				string ORAccountFilterElementType10907 = "ListIDList";
				if (ORAccountFilterElementType10907 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.AccountFilter.ORAccountFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORAccountFilterElementType10907 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.AccountFilter.ORAccountFilter.FullNameList.Add("ab");
				}
				if (ORAccountFilterElementType10907 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.AccountFilter.ORAccountFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (ORAccountFilterElementType10907 == "FullNameWithChildren") {
					//Set field value for FullNameWithChildren
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.AccountFilter.ORAccountFilter.FullNameWithChildren.SetValue("ab");
				}
				string ORItemFilterElementType10908 = "ListIDList";
				if (ORItemFilterElementType10908 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.ItemFilter.ORItemFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORItemFilterElementType10908 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.ItemFilter.ORItemFilter.FullNameList.Add("ab");
				}
				if (ORItemFilterElementType10908 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.ItemFilter.ORItemFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (ORItemFilterElementType10908 == "FullNameWithChildren") {
					//Set field value for FullNameWithChildren
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.ItemFilter.ORItemFilter.FullNameWithChildren.SetValue("ab");
				}
				string ORRefNumberFilterElementType10909 = "RefNumberFilter";
				if (ORRefNumberFilterElementType10909 == "RefNumberFilter") {
					//Set field value for MatchCriterion
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.ORRefNumberFilter.RefNumberFilter.MatchCriterion.SetValue(ENMatchCriterion.mcStartsWith);
					//Set field value for RefNumber
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.ORRefNumberFilter.RefNumberFilter.RefNumber.SetValue("ab");
				}
				if (ORRefNumberFilterElementType10909 == "RefNumberRangeFilter") {
					//Set field value for FromRefNumber
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.ORRefNumberFilter.RefNumberRangeFilter.FromRefNumber.SetValue("ab");
					//Set field value for ToRefNumber
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.ORRefNumberFilter.RefNumberRangeFilter.ToRefNumber.SetValue("ab");
				}
			}
			//Set field value for IncludeLineItems
			InventoryAdjustmentQueryRq.IncludeLineItems.SetValue(true);
			//Set field value for IncludeRetElementList
			//May create more than one of these if needed
			InventoryAdjustmentQueryRq.IncludeRetElementList.Add("ab");
			//Set field value for OwnerIDList
			//May create more than one of these if needed
			InventoryAdjustmentQueryRq.OwnerIDList.Add(Guid.NewGuid().ToString());
		}

		void WalkInventoryAdjustmentQueryRs(IMsgSetResponse responseMsgSet)
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
						if (responseType == ENResponseType.rtInventoryAdjustmentQueryRs) {
							//upcast to more specific type here, this is safe because we checked with response.Type check above
							IInventoryAdjustmentRetList InventoryAdjustmentRet = (IInventoryAdjustmentRetList)response.Detail;
							WalkInventoryAdjustmentRet(InventoryAdjustmentRet);
						}
					}
				}
			}
		}

		void WalkInventoryAdjustmentRet(IInventoryAdjustmentRetList InventoryAdjustmentRetList)
		{
			if (InventoryAdjustmentRetList == null) return;
			//Go through all the elements of IInventoryAdjustmentRetList
			foreach (var InventoryAdjustmentRet in InventoryAdjustmentRetList)
			//Get value of TxnID
			string TxnID10910 = (string)InventoryAdjustmentRet.TxnID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated10911 = (DateTime)InventoryAdjustmentRet.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified10912 = (DateTime)InventoryAdjustmentRet.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence10913 = (string)InventoryAdjustmentRet.EditSequence.GetValue();
			//Get value of TxnNumber
			if (InventoryAdjustmentRet.TxnNumber != null) {
				int TxnNumber10914 = (int)InventoryAdjustmentRet.TxnNumber.GetValue();
			}
			//Get value of ListID
			if (InventoryAdjustmentRet.AccountRef.ListID != null) {
				string ListID10915 = (string)InventoryAdjustmentRet.AccountRef.ListID.GetValue();
			}
			//Get value of FullName
			if (InventoryAdjustmentRet.AccountRef.FullName != null) {
				string FullName10916 = (string)InventoryAdjustmentRet.AccountRef.FullName.GetValue();
			}
			if (InventoryAdjustmentRet.InventorySiteRef != null) {
				//Get value of ListID
				if (InventoryAdjustmentRet.InventorySiteRef.ListID != null) {
					string ListID10917 = (string)InventoryAdjustmentRet.InventorySiteRef.ListID.GetValue();
				}
				//Get value of FullName
				if (InventoryAdjustmentRet.InventorySiteRef.FullName != null) {
					string FullName10918 = (string)InventoryAdjustmentRet.InventorySiteRef.FullName.GetValue();
				}
			}
			//Get value of TxnDate
			DateTime TxnDate10919 = (DateTime)InventoryAdjustmentRet.TxnDate.GetValue();
			//Get value of RefNumber
			if (InventoryAdjustmentRet.RefNumber != null) {
				string RefNumber10920 = (string)InventoryAdjustmentRet.RefNumber.GetValue();
			}
			if (InventoryAdjustmentRet.CustomerRef != null) {
				//Get value of ListID
				if (InventoryAdjustmentRet.CustomerRef.ListID != null) {
					string ListID10921 = (string)InventoryAdjustmentRet.CustomerRef.ListID.GetValue();
				}
				//Get value of FullName
				if (InventoryAdjustmentRet.CustomerRef.FullName != null) {
					string FullName10922 = (string)InventoryAdjustmentRet.CustomerRef.FullName.GetValue();
				}
			}
			if (InventoryAdjustmentRet.ClassRef != null) {
				//Get value of ListID
				if (InventoryAdjustmentRet.ClassRef.ListID != null) {
					string ListID10923 = (string)InventoryAdjustmentRet.ClassRef.ListID.GetValue();
				}
				//Get value of FullName
				if (InventoryAdjustmentRet.ClassRef.FullName != null) {
					string FullName10924 = (string)InventoryAdjustmentRet.ClassRef.FullName.GetValue();
				}
			}
			//Get value of Memo
			if (InventoryAdjustmentRet.Memo != null) {
				string Memo10925 = (string)InventoryAdjustmentRet.Memo.GetValue();
			}
			//Get value of ExternalGUID
			if (InventoryAdjustmentRet.ExternalGUID != null) {
				string ExternalGUID10926 = (string)InventoryAdjustmentRet.ExternalGUID.GetValue();
			}
			if (InventoryAdjustmentRet.InventoryAdjustmentLineRetList != null) {
				for (int i10927 = 0; i10927 < InventoryAdjustmentRet.InventoryAdjustmentLineRetList.Count; i10927++) {
					IInventoryAdjustmentLineRet InventoryAdjustmentLineRet = InventoryAdjustmentRet.InventoryAdjustmentLineRetList.GetAt(i10927);
					//Get value of TxnLineID
					string TxnLineID10928 = (string)InventoryAdjustmentLineRet.TxnLineID.GetValue();
					//Get value of ListID
					if (InventoryAdjustmentLineRet.ItemRef.ListID != null) {
						string ListID10929 = (string)InventoryAdjustmentLineRet.ItemRef.ListID.GetValue();
					}
					//Get value of FullName
					if (InventoryAdjustmentLineRet.ItemRef.FullName != null) {
						string FullName10930 = (string)InventoryAdjustmentLineRet.ItemRef.FullName.GetValue();
					}
					if (InventoryAdjustmentLineRet.ORSerialLotNumberPreference != null) {
						if (InventoryAdjustmentLineRet.ORSerialLotNumberPreference.SerialNumberRet != null) {
							//Get value of SerialNumberRet
							if (InventoryAdjustmentLineRet.ORSerialLotNumberPreference.SerialNumberRet != null) {
								ISerialNumberRet nothing10932 = (ISerialNumberRet)InventoryAdjustmentLineRet.ORSerialLotNumberPreference.SerialNumberRet.GetValue();
							}
						}
						if (InventoryAdjustmentLineRet.ORSerialLotNumberPreference.LotNumber != null) {
							//Get value of LotNumber
							if (InventoryAdjustmentLineRet.ORSerialLotNumberPreference.LotNumber != null) {
								string LotNumber10933 = (string)InventoryAdjustmentLineRet.ORSerialLotNumberPreference.LotNumber.GetValue();
							}
						}
					}
					if (InventoryAdjustmentLineRet.InventorySiteLocationRef != null) {
						//Get value of ListID
						if (InventoryAdjustmentLineRet.InventorySiteLocationRef.ListID != null) {
							string ListID10934 = (string)InventoryAdjustmentLineRet.InventorySiteLocationRef.ListID.GetValue();
						}
						//Get value of FullName
						if (InventoryAdjustmentLineRet.InventorySiteLocationRef.FullName != null) {
							string FullName10935 = (string)InventoryAdjustmentLineRet.InventorySiteLocationRef.FullName.GetValue();
						}
					}
					//Get value of QuantityDifference
					int QuantityDifference10936 = (int)InventoryAdjustmentLineRet.QuantityDifference.GetValue();
					//Get value of ValueDifference
					double ValueDifference10937 = (double)InventoryAdjustmentLineRet.ValueDifference.GetValue();
				}
			}
			if (InventoryAdjustmentRet.DataExtRetList != null) {
				for (int i10938 = 0; i10938 < InventoryAdjustmentRet.DataExtRetList.Count; i10938++) {
					IDataExtRet DataExtRet = InventoryAdjustmentRet.DataExtRetList.GetAt(i10938);
					//Get value of OwnerID
					if (DataExtRet.OwnerID != null) {
						string OwnerID10939 = (string)DataExtRet.OwnerID.GetValue();
					}
					//Get value of DataExtName
					string DataExtName10940 = (string)DataExtRet.DataExtName.GetValue();
					//Get value of DataExtType
					ENDataExtType DataExtType10941 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
					//Get value of DataExtValue
					string DataExtValue10942 = (string)DataExtRet.DataExtValue.GetValue();
				}
			}
		}
	}
}