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
			string ORQueryElementType11877 = "TxnIDList";
			if (ORQueryElementType11877 == "TxnIDList") {
				//Set field value for TxnIDList
				//May create more than one of these if needed
				QueryRq.ORQuery.TxnIDList.Add("200000-1011023419");
			}
			if (ORQueryElementType11877 == "RefNumberList") {
				//Set field value for RefNumberList
				//May create more than one of these if needed
				QueryRq.ORQuery.RefNumberList.Add("ab");
			}
			if (ORQueryElementType11877 == "RefNumberCaseSensitiveList") {
				//Set field value for RefNumberCaseSensitiveList
				//May create more than one of these if needed
				QueryRq.ORQuery.RefNumberCaseSensitiveList.Add("ab");
			}
			if (ORQueryElementType11877 == "Filter") {
				//Set field value for MaxReturned
				QueryRq.ORQuery.Filter.MaxReturned.SetValue(6);
				string ORDateRangeFilterElementType11878 = "ModifiedDateRangeFilter";
				if (ORDateRangeFilterElementType11878 == "ModifiedDateRangeFilter") {
					//Set field value for FromModifiedDate
					QueryRq.ORQuery.Filter.ORDateRangeFilter.ModifiedDateRangeFilter.FromModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
					//Set field value for ToModifiedDate
					QueryRq.ORQuery.Filter.ORDateRangeFilter.ModifiedDateRangeFilter.ToModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				}
				if (ORDateRangeFilterElementType11878 == "TxnDateRangeFilter") {
					string ORTxnDateRangeFilterElementType11879 = "TxnDateFilter";
					if (ORTxnDateRangeFilterElementType11879 == "TxnDateFilter") {
						//Set field value for FromTxnDate
						QueryRq.ORQuery.Filter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.FromTxnDate.SetValue(DateTime.Parse("12/15/2007"));
						//Set field value for ToTxnDate
						QueryRq.ORQuery.Filter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.ToTxnDate.SetValue(DateTime.Parse("12/15/2007"));
					}
					if (ORTxnDateRangeFilterElementType11879 == "DateMacro") {
						//Set field value for DateMacro
						QueryRq.ORQuery.Filter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.DateMacro.SetValue(ENDateMacro.dmAll);
					}
				}
				string OREntityFilterElementType11880 = "ListIDList";
				if (OREntityFilterElementType11880 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					QueryRq.ORQuery.Filter.EntityFilter.OREntityFilter.ListIDList.Add("200000-1011023419");
				}
				if (OREntityFilterElementType11880 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					QueryRq.ORQuery.Filter.EntityFilter.OREntityFilter.FullNameList.Add("ab");
				}
				if (OREntityFilterElementType11880 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					QueryRq.ORQuery.Filter.EntityFilter.OREntityFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (OREntityFilterElementType11880 == "FullNameWithChildren") {
					//Set field value for FullNameWithChildren
					QueryRq.ORQuery.Filter.EntityFilter.OREntityFilter.FullNameWithChildren.SetValue("ab");
				}
				string ORAccountFilterElementType11881 = "ListIDList";
				if (ORAccountFilterElementType11881 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					QueryRq.ORQuery.Filter.AccountFilter.ORAccountFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORAccountFilterElementType11881 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					QueryRq.ORQuery.Filter.AccountFilter.ORAccountFilter.FullNameList.Add("ab");
				}
				if (ORAccountFilterElementType11881 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					QueryRq.ORQuery.Filter.AccountFilter.ORAccountFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (ORAccountFilterElementType11881 == "FullNameWithChildren") {
					//Set field value for FullNameWithChildren
					QueryRq.ORQuery.Filter.AccountFilter.ORAccountFilter.FullNameWithChildren.SetValue("ab");
				}
				string ORRefNumberFilterElementType11882 = "RefNumberFilter";
				if (ORRefNumberFilterElementType11882 == "RefNumberFilter") {
					//Set field value for MatchCriterion
					QueryRq.ORQuery.Filter.ORRefNumberFilter.RefNumberFilter.MatchCriterion.SetValue(ENMatchCriterion.mcStartsWith);
					//Set field value for RefNumber
					QueryRq.ORQuery.Filter.ORRefNumberFilter.RefNumberFilter.RefNumber.SetValue("ab");
				}
				if (ORRefNumberFilterElementType11882 == "RefNumberRangeFilter") {
					//Set field value for FromRefNumber
					QueryRq.ORQuery.Filter.ORRefNumberFilter.RefNumberRangeFilter.FromRefNumber.SetValue("ab");
					//Set field value for ToRefNumber
					QueryRq.ORQuery.Filter.ORRefNumberFilter.RefNumberRangeFilter.ToRefNumber.SetValue("ab");
				}
				string ORCurrencyFilterElementType11883 = "ListIDList";
				if (ORCurrencyFilterElementType11883 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					QueryRq.ORQuery.Filter.CurrencyFilter.ORCurrencyFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORCurrencyFilterElementType11883 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					QueryRq.ORQuery.Filter.CurrencyFilter.ORCurrencyFilter.FullNameList.Add("ab");
				}
				//Set field value for PaidStatus
				QueryRq.ORQuery.Filter.PaidStatus.SetValue(ENPaidStatus.psAll[DEFAULT]);
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
			string TxnID11884 = (string)Ret.TxnID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated11885 = (DateTime)Ret.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified11886 = (DateTime)Ret.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence11887 = (string)Ret.EditSequence.GetValue();
			//Get value of TxnNumber
			if (Ret.TxnNumber != null) {
				int TxnNumber11888 = (int)Ret.TxnNumber.GetValue();
			}
			//Get value of ListID
			if (Ret.CustomerRef.ListID != null) {
				string ListID11889 = (string)Ret.CustomerRef.ListID.GetValue();
			}
			//Get value of FullName
			if (Ret.CustomerRef.FullName != null) {
				string FullName11890 = (string)Ret.CustomerRef.FullName.GetValue();
			}
			if (Ret.ClassRef != null) {
				//Get value of ListID
				if (Ret.ClassRef.ListID != null) {
					string ListID11891 = (string)Ret.ClassRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ClassRef.FullName != null) {
					string FullName11892 = (string)Ret.ClassRef.FullName.GetValue();
				}
			}
			if (Ret.ARAccountRef != null) {
				//Get value of ListID
				if (Ret.ARAccountRef.ListID != null) {
					string ListID11893 = (string)Ret.ARAccountRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ARAccountRef.FullName != null) {
					string FullName11894 = (string)Ret.ARAccountRef.FullName.GetValue();
				}
			}
			if (Ret.TemplateRef != null) {
				//Get value of ListID
				if (Ret.TemplateRef.ListID != null) {
					string ListID11895 = (string)Ret.TemplateRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.TemplateRef.FullName != null) {
					string FullName11896 = (string)Ret.TemplateRef.FullName.GetValue();
				}
			}
			//Get value of TxnDate
			DateTime TxnDate11897 = (DateTime)Ret.TxnDate.GetValue();
			//Get value of RefNumber
			if (Ret.RefNumber != null) {
				string RefNumber11898 = (string)Ret.RefNumber.GetValue();
			}
			if (Ret.BillAddress != null) {
				//Get value of Addr1
				if (Ret.BillAddress.Addr1 != null) {
					string Addr111899 = (string)Ret.BillAddress.Addr1.GetValue();
				}
				//Get value of Addr2
				if (Ret.BillAddress.Addr2 != null) {
					string Addr211900 = (string)Ret.BillAddress.Addr2.GetValue();
				}
				//Get value of Addr3
				if (Ret.BillAddress.Addr3 != null) {
					string Addr311901 = (string)Ret.BillAddress.Addr3.GetValue();
				}
				//Get value of Addr4
				if (Ret.BillAddress.Addr4 != null) {
					string Addr411902 = (string)Ret.BillAddress.Addr4.GetValue();
				}
				//Get value of Addr5
				if (Ret.BillAddress.Addr5 != null) {
					string Addr511903 = (string)Ret.BillAddress.Addr5.GetValue();
				}
				//Get value of City
				if (Ret.BillAddress.City != null) {
					string City11904 = (string)Ret.BillAddress.City.GetValue();
				}
				//Get value of State
				if (Ret.BillAddress.State != null) {
					string State11905 = (string)Ret.BillAddress.State.GetValue();
				}
				//Get value of PostalCode
				if (Ret.BillAddress.PostalCode != null) {
					string PostalCode11906 = (string)Ret.BillAddress.PostalCode.GetValue();
				}
				//Get value of Country
				if (Ret.BillAddress.Country != null) {
					string Country11907 = (string)Ret.BillAddress.Country.GetValue();
				}
				//Get value of Note
				if (Ret.BillAddress.Note != null) {
					string Note11908 = (string)Ret.BillAddress.Note.GetValue();
				}
			}
			if (Ret.BillAddressBlock != null) {
				//Get value of Addr1
				if (Ret.BillAddressBlock.Addr1 != null) {
					string Addr111909 = (string)Ret.BillAddressBlock.Addr1.GetValue();
				}
				//Get value of Addr2
				if (Ret.BillAddressBlock.Addr2 != null) {
					string Addr211910 = (string)Ret.BillAddressBlock.Addr2.GetValue();
				}
				//Get value of Addr3
				if (Ret.BillAddressBlock.Addr3 != null) {
					string Addr311911 = (string)Ret.BillAddressBlock.Addr3.GetValue();
				}
				//Get value of Addr4
				if (Ret.BillAddressBlock.Addr4 != null) {
					string Addr411912 = (string)Ret.BillAddressBlock.Addr4.GetValue();
				}
				//Get value of Addr5
				if (Ret.BillAddressBlock.Addr5 != null) {
					string Addr511913 = (string)Ret.BillAddressBlock.Addr5.GetValue();
				}
			}
			if (Ret.ShipAddress != null) {
				//Get value of Addr1
				if (Ret.ShipAddress.Addr1 != null) {
					string Addr111914 = (string)Ret.ShipAddress.Addr1.GetValue();
				}
				//Get value of Addr2
				if (Ret.ShipAddress.Addr2 != null) {
					string Addr211915 = (string)Ret.ShipAddress.Addr2.GetValue();
				}
				//Get value of Addr3
				if (Ret.ShipAddress.Addr3 != null) {
					string Addr311916 = (string)Ret.ShipAddress.Addr3.GetValue();
				}
				//Get value of Addr4
				if (Ret.ShipAddress.Addr4 != null) {
					string Addr411917 = (string)Ret.ShipAddress.Addr4.GetValue();
				}
				//Get value of Addr5
				if (Ret.ShipAddress.Addr5 != null) {
					string Addr511918 = (string)Ret.ShipAddress.Addr5.GetValue();
				}
				//Get value of City
				if (Ret.ShipAddress.City != null) {
					string City11919 = (string)Ret.ShipAddress.City.GetValue();
				}
				//Get value of State
				if (Ret.ShipAddress.State != null) {
					string State11920 = (string)Ret.ShipAddress.State.GetValue();
				}
				//Get value of PostalCode
				if (Ret.ShipAddress.PostalCode != null) {
					string PostalCode11921 = (string)Ret.ShipAddress.PostalCode.GetValue();
				}
				//Get value of Country
				if (Ret.ShipAddress.Country != null) {
					string Country11922 = (string)Ret.ShipAddress.Country.GetValue();
				}
				//Get value of Note
				if (Ret.ShipAddress.Note != null) {
					string Note11923 = (string)Ret.ShipAddress.Note.GetValue();
				}
			}
			if (Ret.ShipAddressBlock != null) {
				//Get value of Addr1
				if (Ret.ShipAddressBlock.Addr1 != null) {
					string Addr111924 = (string)Ret.ShipAddressBlock.Addr1.GetValue();
				}
				//Get value of Addr2
				if (Ret.ShipAddressBlock.Addr2 != null) {
					string Addr211925 = (string)Ret.ShipAddressBlock.Addr2.GetValue();
				}
				//Get value of Addr3
				if (Ret.ShipAddressBlock.Addr3 != null) {
					string Addr311926 = (string)Ret.ShipAddressBlock.Addr3.GetValue();
				}
				//Get value of Addr4
				if (Ret.ShipAddressBlock.Addr4 != null) {
					string Addr411927 = (string)Ret.ShipAddressBlock.Addr4.GetValue();
				}
				//Get value of Addr5
				if (Ret.ShipAddressBlock.Addr5 != null) {
					string Addr511928 = (string)Ret.ShipAddressBlock.Addr5.GetValue();
				}
			}
			//Get value of IsPending
			if (Ret.IsPending != null) {
				bool IsPending11929 = (bool)Ret.IsPending.GetValue();
			}
			//Get value of IsFinanceCharge
			if (Ret.IsFinanceCharge != null) {
				bool IsFinanceCharge11930 = (bool)Ret.IsFinanceCharge.GetValue();
			}
			//Get value of PONumber
			if (Ret.PONumber != null) {
				string PONumber11931 = (string)Ret.PONumber.GetValue();
			}
			if (Ret.TermsRef != null) {
				//Get value of ListID
				if (Ret.TermsRef.ListID != null) {
					string ListID11932 = (string)Ret.TermsRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.TermsRef.FullName != null) {
					string FullName11933 = (string)Ret.TermsRef.FullName.GetValue();
				}
			}
			//Get value of DueDate
			if (Ret.DueDate != null) {
				DateTime DueDate11934 = (DateTime)Ret.DueDate.GetValue();
			}
			if (Ret.SalesRepRef != null) {
				//Get value of ListID
				if (Ret.SalesRepRef.ListID != null) {
					string ListID11935 = (string)Ret.SalesRepRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.SalesRepRef.FullName != null) {
					string FullName11936 = (string)Ret.SalesRepRef.FullName.GetValue();
				}
			}
			//Get value of FOB
			if (Ret.FOB != null) {
				string FOB11937 = (string)Ret.FOB.GetValue();
			}
			//Get value of ShipDate
			if (Ret.ShipDate != null) {
				DateTime ShipDate11938 = (DateTime)Ret.ShipDate.GetValue();
			}
			if (Ret.ShipMethodRef != null) {
				//Get value of ListID
				if (Ret.ShipMethodRef.ListID != null) {
					string ListID11939 = (string)Ret.ShipMethodRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ShipMethodRef.FullName != null) {
					string FullName11940 = (string)Ret.ShipMethodRef.FullName.GetValue();
				}
			}
			//Get value of Subtotal
			if (Ret.Subtotal != null) {
				double Subtotal11941 = (double)Ret.Subtotal.GetValue();
			}
			if (Ret.ItemSalesTaxRef != null) {
				//Get value of ListID
				if (Ret.ItemSalesTaxRef.ListID != null) {
					string ListID11942 = (string)Ret.ItemSalesTaxRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ItemSalesTaxRef.FullName != null) {
					string FullName11943 = (string)Ret.ItemSalesTaxRef.FullName.GetValue();
				}
			}
			//Get value of SalesTaxPercentage
			if (Ret.SalesTaxPercentage != null) {
				double SalesTaxPercentage11944 = (double)Ret.SalesTaxPercentage.GetValue();
			}
			//Get value of SalesTaxTotal
			if (Ret.SalesTaxTotal != null) {
				double SalesTaxTotal11945 = (double)Ret.SalesTaxTotal.GetValue();
			}
			//Get value of AppliedAmount
			if (Ret.AppliedAmount != null) {
				double AppliedAmount11946 = (double)Ret.AppliedAmount.GetValue();
			}
			//Get value of BalanceRemaining
			if (Ret.BalanceRemaining != null) {
				double BalanceRemaining11947 = (double)Ret.BalanceRemaining.GetValue();
			}
			if (Ret.CurrencyRef != null) {
				//Get value of ListID
				if (Ret.CurrencyRef.ListID != null) {
					string ListID11948 = (string)Ret.CurrencyRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.CurrencyRef.FullName != null) {
					string FullName11949 = (string)Ret.CurrencyRef.FullName.GetValue();
				}
			}
			//Get value of ExchangeRate
			if (Ret.ExchangeRate != null) {
				IQBFloatType ExchangeRate11950 = (IQBFloatType)Ret.ExchangeRate.GetValue();
			}
			//Get value of BalanceRemainingInHomeCurrency
			if (Ret.BalanceRemainingInHomeCurrency != null) {
				double BalanceRemainingInHomeCurrency11951 = (double)Ret.BalanceRemainingInHomeCurrency.GetValue();
			}
			//Get value of Memo
			if (Ret.Memo != null) {
				string Memo11952 = (string)Ret.Memo.GetValue();
			}
			//Get value of IsPaid
			if (Ret.IsPaid != null) {
				bool IsPaid11953 = (bool)Ret.IsPaid.GetValue();
			}
			if (Ret.CustomerMsgRef != null) {
				//Get value of ListID
				if (Ret.CustomerMsgRef.ListID != null) {
					string ListID11954 = (string)Ret.CustomerMsgRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.CustomerMsgRef.FullName != null) {
					string FullName11955 = (string)Ret.CustomerMsgRef.FullName.GetValue();
				}
			}
			//Get value of IsToBePrinted
			if (Ret.IsToBePrinted != null) {
				bool IsToBePrinted11956 = (bool)Ret.IsToBePrinted.GetValue();
			}
			//Get value of IsToBeEmailed
			if (Ret.IsToBeEmailed != null) {
				bool IsToBeEmailed11957 = (bool)Ret.IsToBeEmailed.GetValue();
			}
			//Get value of IsTaxIncluded
			if (Ret.IsTaxIncluded != null) {
				bool IsTaxIncluded11958 = (bool)Ret.IsTaxIncluded.GetValue();
			}
			if (Ret.CustomerSalesTaxCodeRef != null) {
				//Get value of ListID
				if (Ret.CustomerSalesTaxCodeRef.ListID != null) {
					string ListID11959 = (string)Ret.CustomerSalesTaxCodeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.CustomerSalesTaxCodeRef.FullName != null) {
					string FullName11960 = (string)Ret.CustomerSalesTaxCodeRef.FullName.GetValue();
				}
			}
			//Get value of SuggestedDiscountAmount
			if (Ret.SuggestedDiscountAmount != null) {
				double SuggestedDiscountAmount11961 = (double)Ret.SuggestedDiscountAmount.GetValue();
			}
			//Get value of SuggestedDiscountDate
			if (Ret.SuggestedDiscountDate != null) {
				DateTime SuggestedDiscountDate11962 = (DateTime)Ret.SuggestedDiscountDate.GetValue();
			}
			//Get value of Other
			if (Ret.Other != null) {
				string Other11963 = (string)Ret.Other.GetValue();
			}
			//Get value of ExternalGUID
			if (Ret.ExternalGUID != null) {
				string ExternalGUID11964 = (string)Ret.ExternalGUID.GetValue();
			}
			if (Ret.LinkedTxnList != null) {
				for (int i11965 = 0; i11965 < Ret.LinkedTxnList.Count; i11965++) {
					ILinkedTxn LinkedTxn = Ret.LinkedTxnList.GetAt(i11965);
					//Get value of TxnID
					string TxnID11966 = (string)LinkedTxn.TxnID.GetValue();
					//Get value of TxnType
					ENTxnType TxnType11967 = (ENTxnType)LinkedTxn.TxnType.GetValue();
					//Get value of TxnDate
					DateTime TxnDate11968 = (DateTime)LinkedTxn.TxnDate.GetValue();
					//Get value of RefNumber
					if (LinkedTxn.RefNumber != null) {
						string RefNumber11969 = (string)LinkedTxn.RefNumber.GetValue();
					}
					//Get value of LinkType
					if (LinkedTxn.LinkType != null) {
						ENLinkType LinkType11970 = (ENLinkType)LinkedTxn.LinkType.GetValue();
					}
					//Get value of Amount
					double Amount11971 = (double)LinkedTxn.Amount.GetValue();
				}
			}
			if (Ret.ORLineRetList != null) {
				for (int i11972 = 0; i11972 < Ret.ORLineRetList.Count; i11972++) {
					IORLineRet ORLineRet11973 = Ret.ORLineRetList.GetAt(i11972);
					if (ORLineRet11973.LineRet != null) {
						if (ORLineRet11973.LineRet != null) {
							//Get value of TxnLineID
							string TxnLineID11974 = (string)ORLineRet11973.LineRet.TxnLineID.GetValue();
							if (ORLineRet11973.LineRet.ItemRef != null) {
								//Get value of ListID
								if (ORLineRet11973.LineRet.ItemRef.ListID != null) {
									string ListID11975 = (string)ORLineRet11973.LineRet.ItemRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORLineRet11973.LineRet.ItemRef.FullName != null) {
									string FullName11976 = (string)ORLineRet11973.LineRet.ItemRef.FullName.GetValue();
								}
							}
							//Get value of Desc
							if (ORLineRet11973.LineRet.Desc != null) {
								string Desc11977 = (string)ORLineRet11973.LineRet.Desc.GetValue();
							}
							//Get value of Quantity
							if (ORLineRet11973.LineRet.Quantity != null) {
								int Quantity11978 = (int)ORLineRet11973.LineRet.Quantity.GetValue();
							}
							//Get value of UnitOfMeasure
							if (ORLineRet11973.LineRet.UnitOfMeasure != null) {
								string UnitOfMeasure11979 = (string)ORLineRet11973.LineRet.UnitOfMeasure.GetValue();
							}
							if (ORLineRet11973.LineRet.OverrideUOMSetRef != null) {
								//Get value of ListID
								if (ORLineRet11973.LineRet.OverrideUOMSetRef.ListID != null) {
									string ListID11980 = (string)ORLineRet11973.LineRet.OverrideUOMSetRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORLineRet11973.LineRet.OverrideUOMSetRef.FullName != null) {
									string FullName11981 = (string)ORLineRet11973.LineRet.OverrideUOMSetRef.FullName.GetValue();
								}
							}
							if (ORLineRet11973.LineRet.ORRate != null) {
								if (ORLineRet11973.LineRet.ORRate.Rate != null) {
									//Get value of Rate
									if (ORLineRet11973.LineRet.ORRate.Rate != null) {
										double Rate11983 = (double)ORLineRet11973.LineRet.ORRate.Rate.GetValue();
									}
								}
								if (ORLineRet11973.LineRet.ORRate.RatePercent != null) {
									//Get value of RatePercent
									if (ORLineRet11973.LineRet.ORRate.RatePercent != null) {
										double RatePercent11984 = (double)ORLineRet11973.LineRet.ORRate.RatePercent.GetValue();
									}
								}
							}
							if (ORLineRet11973.LineRet.ClassRef != null) {
								//Get value of ListID
								if (ORLineRet11973.LineRet.ClassRef.ListID != null) {
									string ListID11985 = (string)ORLineRet11973.LineRet.ClassRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORLineRet11973.LineRet.ClassRef.FullName != null) {
									string FullName11986 = (string)ORLineRet11973.LineRet.ClassRef.FullName.GetValue();
								}
							}
							//Get value of Amount
							if (ORLineRet11973.LineRet.Amount != null) {
								double Amount11987 = (double)ORLineRet11973.LineRet.Amount.GetValue();
							}
							if (ORLineRet11973.LineRet.InventorySiteRef != null) {
								//Get value of ListID
								if (ORLineRet11973.LineRet.InventorySiteRef.ListID != null) {
									string ListID11988 = (string)ORLineRet11973.LineRet.InventorySiteRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORLineRet11973.LineRet.InventorySiteRef.FullName != null) {
									string FullName11989 = (string)ORLineRet11973.LineRet.InventorySiteRef.FullName.GetValue();
								}
							}
							if (ORLineRet11973.LineRet.InventorySiteLocationRef != null) {
								//Get value of ListID
								if (ORLineRet11973.LineRet.InventorySiteLocationRef.ListID != null) {
									string ListID11990 = (string)ORLineRet11973.LineRet.InventorySiteLocationRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORLineRet11973.LineRet.InventorySiteLocationRef.FullName != null) {
									string FullName11991 = (string)ORLineRet11973.LineRet.InventorySiteLocationRef.FullName.GetValue();
								}
							}
							if (ORLineRet11973.LineRet.ORSerialLotNumber != null) {
								if (ORLineRet11973.LineRet.ORSerialLotNumber.SerialNumber != null) {
									//Get value of SerialNumber
									if (ORLineRet11973.LineRet.ORSerialLotNumber.SerialNumber != null) {
										string SerialNumber11993 = (string)ORLineRet11973.LineRet.ORSerialLotNumber.SerialNumber.GetValue();
									}
								}
								if (ORLineRet11973.LineRet.ORSerialLotNumber.LotNumber != null) {
									//Get value of LotNumber
									if (ORLineRet11973.LineRet.ORSerialLotNumber.LotNumber != null) {
										string LotNumber11994 = (string)ORLineRet11973.LineRet.ORSerialLotNumber.LotNumber.GetValue();
									}
								}
							}
							//Get value of ServiceDate
							if (ORLineRet11973.LineRet.ServiceDate != null) {
								DateTime ServiceDate11995 = (DateTime)ORLineRet11973.LineRet.ServiceDate.GetValue();
							}
							if (ORLineRet11973.LineRet.SalesTaxCodeRef != null) {
								//Get value of ListID
								if (ORLineRet11973.LineRet.SalesTaxCodeRef.ListID != null) {
									string ListID11996 = (string)ORLineRet11973.LineRet.SalesTaxCodeRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORLineRet11973.LineRet.SalesTaxCodeRef.FullName != null) {
									string FullName11997 = (string)ORLineRet11973.LineRet.SalesTaxCodeRef.FullName.GetValue();
								}
							}
							//Get value of Other1
							if (ORLineRet11973.LineRet.Other1 != null) {
								string Other111998 = (string)ORLineRet11973.LineRet.Other1.GetValue();
							}
							//Get value of Other2
							if (ORLineRet11973.LineRet.Other2 != null) {
								string Other211999 = (string)ORLineRet11973.LineRet.Other2.GetValue();
							}
							if (ORLineRet11973.LineRet.DataExtRetList != null) {
								for (int i12000 = 0; i12000 < ORLineRet11973.LineRet.DataExtRetList.Count; i12000++) {
									IDataExtRet DataExtRet = ORLineRet11973.LineRet.DataExtRetList.GetAt(i12000);
									//Get value of OwnerID
									if (DataExtRet.OwnerID != null) {
										string OwnerID12001 = (string)DataExtRet.OwnerID.GetValue();
									}
									//Get value of DataExtName
									string DataExtName12002 = (string)DataExtRet.DataExtName.GetValue();
									//Get value of DataExtType
									ENDataExtType DataExtType12003 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
									//Get value of DataExtValue
									string DataExtValue12004 = (string)DataExtRet.DataExtValue.GetValue();
								}
							}
						}
					}
					if (ORLineRet11973.LineGroupRet != null) {
						if (ORLineRet11973.LineGroupRet != null) {
							//Get value of TxnLineID
							string TxnLineID12005 = (string)ORLineRet11973.LineGroupRet.TxnLineID.GetValue();
							//Get value of ListID
							if (ORLineRet11973.LineGroupRet.ItemGroupRef.ListID != null) {
								string ListID12006 = (string)ORLineRet11973.LineGroupRet.ItemGroupRef.ListID.GetValue();
							}
							//Get value of FullName
							if (ORLineRet11973.LineGroupRet.ItemGroupRef.FullName != null) {
								string FullName12007 = (string)ORLineRet11973.LineGroupRet.ItemGroupRef.FullName.GetValue();
							}
							//Get value of Desc
							if (ORLineRet11973.LineGroupRet.Desc != null) {
								string Desc12008 = (string)ORLineRet11973.LineGroupRet.Desc.GetValue();
							}
							//Get value of Quantity
							if (ORLineRet11973.LineGroupRet.Quantity != null) {
								int Quantity12009 = (int)ORLineRet11973.LineGroupRet.Quantity.GetValue();
							}
							//Get value of UnitOfMeasure
							if (ORLineRet11973.LineGroupRet.UnitOfMeasure != null) {
								string UnitOfMeasure12010 = (string)ORLineRet11973.LineGroupRet.UnitOfMeasure.GetValue();
							}
							if (ORLineRet11973.LineGroupRet.OverrideUOMSetRef != null) {
								//Get value of ListID
								if (ORLineRet11973.LineGroupRet.OverrideUOMSetRef.ListID != null) {
									string ListID12011 = (string)ORLineRet11973.LineGroupRet.OverrideUOMSetRef.ListID.GetValue();
								}
								//Get value of FullName
								if (ORLineRet11973.LineGroupRet.OverrideUOMSetRef.FullName != null) {
									string FullName12012 = (string)ORLineRet11973.LineGroupRet.OverrideUOMSetRef.FullName.GetValue();
								}
							}
							//Get value of IsPrintItemsInGroup
							bool IsPrintItemsInGroup12013 = (bool)ORLineRet11973.LineGroupRet.IsPrintItemsInGroup.GetValue();
							//Get value of TotalAmount
							double TotalAmount12014 = (double)ORLineRet11973.LineGroupRet.TotalAmount.GetValue();
							if (ORLineRet11973.LineGroupRet.LineRetList != null) {
								for (int i12015 = 0; i12015 < ORLineRet11973.LineGroupRet.LineRetList.Count; i12015++) {
									ILineRet LineRet = ORLineRet11973.LineGroupRet.LineRetList.GetAt(i12015);
									//Get value of TxnLineID
									string TxnLineID12016 = (string)LineRet.TxnLineID.GetValue();
									if (LineRet.ItemRef != null) {
										//Get value of ListID
										if (LineRet.ItemRef.ListID != null) {
											string ListID12017 = (string)LineRet.ItemRef.ListID.GetValue();
										}
										//Get value of FullName
										if (LineRet.ItemRef.FullName != null) {
											string FullName12018 = (string)LineRet.ItemRef.FullName.GetValue();
										}
									}
									//Get value of Desc
									if (LineRet.Desc != null) {
										string Desc12019 = (string)LineRet.Desc.GetValue();
									}
									//Get value of Quantity
									if (LineRet.Quantity != null) {
										int Quantity12020 = (int)LineRet.Quantity.GetValue();
									}
									//Get value of UnitOfMeasure
									if (LineRet.UnitOfMeasure != null) {
										string UnitOfMeasure12021 = (string)LineRet.UnitOfMeasure.GetValue();
									}
									if (LineRet.OverrideUOMSetRef != null) {
										//Get value of ListID
										if (LineRet.OverrideUOMSetRef.ListID != null) {
											string ListID12022 = (string)LineRet.OverrideUOMSetRef.ListID.GetValue();
										}
										//Get value of FullName
										if (LineRet.OverrideUOMSetRef.FullName != null) {
											string FullName12023 = (string)LineRet.OverrideUOMSetRef.FullName.GetValue();
										}
									}
									if (LineRet.ORRate != null) {
										if (LineRet.ORRate.Rate != null) {
											//Get value of Rate
											if (LineRet.ORRate.Rate != null) {
												double Rate12025 = (double)LineRet.ORRate.Rate.GetValue();
											}
										}
										if (LineRet.ORRate.RatePercent != null) {
											//Get value of RatePercent
											if (LineRet.ORRate.RatePercent != null) {
												double RatePercent12026 = (double)LineRet.ORRate.RatePercent.GetValue();
											}
										}
									}
									if (LineRet.ClassRef != null) {
										//Get value of ListID
										if (LineRet.ClassRef.ListID != null) {
											string ListID12027 = (string)LineRet.ClassRef.ListID.GetValue();
										}
										//Get value of FullName
										if (LineRet.ClassRef.FullName != null) {
											string FullName12028 = (string)LineRet.ClassRef.FullName.GetValue();
										}
									}
									//Get value of Amount
									if (LineRet.Amount != null) {
										double Amount12029 = (double)LineRet.Amount.GetValue();
									}
									if (LineRet.InventorySiteRef != null) {
										//Get value of ListID
										if (LineRet.InventorySiteRef.ListID != null) {
											string ListID12030 = (string)LineRet.InventorySiteRef.ListID.GetValue();
										}
										//Get value of FullName
										if (LineRet.InventorySiteRef.FullName != null) {
											string FullName12031 = (string)LineRet.InventorySiteRef.FullName.GetValue();
										}
									}
									if (LineRet.InventorySiteLocationRef != null) {
										//Get value of ListID
										if (LineRet.InventorySiteLocationRef.ListID != null) {
											string ListID12032 = (string)LineRet.InventorySiteLocationRef.ListID.GetValue();
										}
										//Get value of FullName
										if (LineRet.InventorySiteLocationRef.FullName != null) {
											string FullName12033 = (string)LineRet.InventorySiteLocationRef.FullName.GetValue();
										}
									}
									if (LineRet.ORSerialLotNumber != null) {
										if (LineRet.ORSerialLotNumber.SerialNumber != null) {
											//Get value of SerialNumber
											if (LineRet.ORSerialLotNumber.SerialNumber != null) {
												string SerialNumber12035 = (string)LineRet.ORSerialLotNumber.SerialNumber.GetValue();
											}
										}
										if (LineRet.ORSerialLotNumber.LotNumber != null) {
											//Get value of LotNumber
											if (LineRet.ORSerialLotNumber.LotNumber != null) {
												string LotNumber12036 = (string)LineRet.ORSerialLotNumber.LotNumber.GetValue();
											}
										}
									}
									//Get value of ServiceDate
									if (LineRet.ServiceDate != null) {
										DateTime ServiceDate12037 = (DateTime)LineRet.ServiceDate.GetValue();
									}
									if (LineRet.SalesTaxCodeRef != null) {
										//Get value of ListID
										if (LineRet.SalesTaxCodeRef.ListID != null) {
											string ListID12038 = (string)LineRet.SalesTaxCodeRef.ListID.GetValue();
										}
										//Get value of FullName
										if (LineRet.SalesTaxCodeRef.FullName != null) {
											string FullName12039 = (string)LineRet.SalesTaxCodeRef.FullName.GetValue();
										}
									}
									//Get value of Other1
									if (LineRet.Other1 != null) {
										string Other112040 = (string)LineRet.Other1.GetValue();
									}
									//Get value of Other2
									if (LineRet.Other2 != null) {
										string Other212041 = (string)LineRet.Other2.GetValue();
									}
									if (LineRet.DataExtRetList != null) {
										for (int i12042 = 0; i12042 < LineRet.DataExtRetList.Count; i12042++) {
											IDataExtRet DataExtRet = LineRet.DataExtRetList.GetAt(i12042);
											//Get value of OwnerID
											if (DataExtRet.OwnerID != null) {
												string OwnerID12043 = (string)DataExtRet.OwnerID.GetValue();
											}
											//Get value of DataExtName
											string DataExtName12044 = (string)DataExtRet.DataExtName.GetValue();
											//Get value of DataExtType
											ENDataExtType DataExtType12045 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
											//Get value of DataExtValue
											string DataExtValue12046 = (string)DataExtRet.DataExtValue.GetValue();
										}
									}
								}
							}
							if (ORLineRet11973.LineGroupRet.DataExtRetList != null) {
								for (int i12047 = 0; i12047 < ORLineRet11973.LineGroupRet.DataExtRetList.Count; i12047++) {
									IDataExtRet DataExtRet = ORLineRet11973.LineGroupRet.DataExtRetList.GetAt(i12047);
									//Get value of OwnerID
									if (DataExtRet.OwnerID != null) {
										string OwnerID12048 = (string)DataExtRet.OwnerID.GetValue();
									}
									//Get value of DataExtName
									string DataExtName12049 = (string)DataExtRet.DataExtName.GetValue();
									//Get value of DataExtType
									ENDataExtType DataExtType12050 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
									//Get value of DataExtValue
									string DataExtValue12051 = (string)DataExtRet.DataExtValue.GetValue();
								}
							}
						}
					}
				}
			}
			if (Ret.DataExtRetList != null) {
				for (int i12052 = 0; i12052 < Ret.DataExtRetList.Count; i12052++) {
					IDataExtRet DataExtRet = Ret.DataExtRetList.GetAt(i12052);
					//Get value of OwnerID
					if (DataExtRet.OwnerID != null) {
						string OwnerID12053 = (string)DataExtRet.OwnerID.GetValue();
					}
					//Get value of DataExtName
					string DataExtName12054 = (string)DataExtRet.DataExtName.GetValue();
					//Get value of DataExtType
					ENDataExtType DataExtType12055 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
					//Get value of DataExtValue
					string DataExtValue12056 = (string)DataExtRet.DataExtValue.GetValue();
				}
			}
		}
	}
}