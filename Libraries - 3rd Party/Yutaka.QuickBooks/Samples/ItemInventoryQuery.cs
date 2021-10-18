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
			string ORListQueryWithOwnerIDAndClassElementType13273 = "ListIDList";
			if (ORListQueryWithOwnerIDAndClassElementType13273 == "ListIDList") {
				//Set field value for ListIDList
				//May create more than one of these if needed
				QueryRq.ORListQueryWithOwnerIDAndClass.ListIDList.Add("200000-1011023419");
			}
			if (ORListQueryWithOwnerIDAndClassElementType13273 == "FullNameList") {
				//Set field value for FullNameList
				//May create more than one of these if needed
				QueryRq.ORListQueryWithOwnerIDAndClass.FullNameList.Add("ab");
			}
			if (ORListQueryWithOwnerIDAndClassElementType13273 == "ListWithClassFilter") {
				//Set field value for MaxReturned
				QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.MaxReturned.SetValue(6);
				//Set field value for ActiveStatus
				QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ActiveStatus.SetValue(ENActiveStatus.asActiveOnly[DEFAULT]);
				//Set field value for FromModifiedDate
				QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.FromModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				//Set field value for ToModifiedDate
				QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ToModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				string ORNameFilterElementType13274 = "NameFilter";
				if (ORNameFilterElementType13274 == "NameFilter") {
					//Set field value for MatchCriterion
					QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameFilter.MatchCriterion.SetValue(ENMatchCriterion.mcStartsWith);
					//Set field value for Name
					QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameFilter.Name.SetValue("ab");
				}
				if (ORNameFilterElementType13274 == "NameRangeFilter") {
					//Set field value for FromName
					QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameRangeFilter.FromName.SetValue("ab");
					//Set field value for ToName
					QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameRangeFilter.ToName.SetValue("ab");
				}
				string ORClassFilterElementType13275 = "ListIDList";
				if (ORClassFilterElementType13275 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ClassFilter.ORClassFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORClassFilterElementType13275 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ClassFilter.ORClassFilter.FullNameList.Add("ab");
				}
				if (ORClassFilterElementType13275 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ClassFilter.ORClassFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (ORClassFilterElementType13275 == "FullNameWithChildren") {
					//Set field value for FullNameWithChildren
					QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ClassFilter.ORClassFilter.FullNameWithChildren.SetValue("ab");
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
			string ListID13276 = (string)Ret.ListID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated13277 = (DateTime)Ret.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified13278 = (DateTime)Ret.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence13279 = (string)Ret.EditSequence.GetValue();
			//Get value of Name
			string Name13280 = (string)Ret.Name.GetValue();
			//Get value of FullName
			string FullName13281 = (string)Ret.FullName.GetValue();
			//Get value of BarCodeValue
			if (Ret.BarCodeValue != null) {
				string BarCodeValue13282 = (string)Ret.BarCodeValue.GetValue();
			}
			//Get value of IsActive
			if (Ret.IsActive != null) {
				bool IsActive13283 = (bool)Ret.IsActive.GetValue();
			}
			if (Ret.ClassRef != null) {
				//Get value of ListID
				if (Ret.ClassRef.ListID != null) {
					string ListID13284 = (string)Ret.ClassRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ClassRef.FullName != null) {
					string FullName13285 = (string)Ret.ClassRef.FullName.GetValue();
				}
			}
			if (Ret.ParentRef != null) {
				//Get value of ListID
				if (Ret.ParentRef.ListID != null) {
					string ListID13286 = (string)Ret.ParentRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ParentRef.FullName != null) {
					string FullName13287 = (string)Ret.ParentRef.FullName.GetValue();
				}
			}
			//Get value of Sublevel
			int Sublevel13288 = (int)Ret.Sublevel.GetValue();
			//Get value of ManufacturerPartNumber
			if (Ret.ManufacturerPartNumber != null) {
				string ManufacturerPartNumber13289 = (string)Ret.ManufacturerPartNumber.GetValue();
			}
			if (Ret.UnitOfMeasureSetRef != null) {
				//Get value of ListID
				if (Ret.UnitOfMeasureSetRef.ListID != null) {
					string ListID13290 = (string)Ret.UnitOfMeasureSetRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.UnitOfMeasureSetRef.FullName != null) {
					string FullName13291 = (string)Ret.UnitOfMeasureSetRef.FullName.GetValue();
				}
			}
			//Get value of IsTaxIncluded
			if (Ret.IsTaxIncluded != null) {
				bool IsTaxIncluded13292 = (bool)Ret.IsTaxIncluded.GetValue();
			}
			if (Ret.SalesTaxCodeRef != null) {
				//Get value of ListID
				if (Ret.SalesTaxCodeRef.ListID != null) {
					string ListID13293 = (string)Ret.SalesTaxCodeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.SalesTaxCodeRef.FullName != null) {
					string FullName13294 = (string)Ret.SalesTaxCodeRef.FullName.GetValue();
				}
			}
			//Get value of SalesDesc
			if (Ret.SalesDesc != null) {
				string SalesDesc13295 = (string)Ret.SalesDesc.GetValue();
			}
			//Get value of SalesPrice
			if (Ret.SalesPrice != null) {
				double SalesPrice13296 = (double)Ret.SalesPrice.GetValue();
			}
			if (Ret.IncomeAccountRef != null) {
				//Get value of ListID
				if (Ret.IncomeAccountRef.ListID != null) {
					string ListID13297 = (string)Ret.IncomeAccountRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.IncomeAccountRef.FullName != null) {
					string FullName13298 = (string)Ret.IncomeAccountRef.FullName.GetValue();
				}
			}
			//Get value of PurchaseDesc
			if (Ret.PurchaseDesc != null) {
				string PurchaseDesc13299 = (string)Ret.PurchaseDesc.GetValue();
			}
			//Get value of PurchaseCost
			if (Ret.PurchaseCost != null) {
				double PurchaseCost13300 = (double)Ret.PurchaseCost.GetValue();
			}
			if (Ret.PurchaseTaxCodeRef != null) {
				//Get value of ListID
				if (Ret.PurchaseTaxCodeRef.ListID != null) {
					string ListID13301 = (string)Ret.PurchaseTaxCodeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.PurchaseTaxCodeRef.FullName != null) {
					string FullName13302 = (string)Ret.PurchaseTaxCodeRef.FullName.GetValue();
				}
			}
			if (Ret.COGSAccountRef != null) {
				//Get value of ListID
				if (Ret.COGSAccountRef.ListID != null) {
					string ListID13303 = (string)Ret.COGSAccountRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.COGSAccountRef.FullName != null) {
					string FullName13304 = (string)Ret.COGSAccountRef.FullName.GetValue();
				}
			}
			if (Ret.PrefVendorRef != null) {
				//Get value of ListID
				if (Ret.PrefVendorRef.ListID != null) {
					string ListID13305 = (string)Ret.PrefVendorRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.PrefVendorRef.FullName != null) {
					string FullName13306 = (string)Ret.PrefVendorRef.FullName.GetValue();
				}
			}
			if (Ret.AssetAccountRef != null) {
				//Get value of ListID
				if (Ret.AssetAccountRef.ListID != null) {
					string ListID13307 = (string)Ret.AssetAccountRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.AssetAccountRef.FullName != null) {
					string FullName13308 = (string)Ret.AssetAccountRef.FullName.GetValue();
				}
			}
			//Get value of ReorderPoint
			if (Ret.ReorderPoint != null) {
				int ReorderPoint13309 = (int)Ret.ReorderPoint.GetValue();
			}
			//Get value of Max
			if (Ret.Max != null) {
				int Max13310 = (int)Ret.Max.GetValue();
			}
			//Get value of QuantityOnHand
			if (Ret.QuantityOnHand != null) {
				int QuantityOnHand13311 = (int)Ret.QuantityOnHand.GetValue();
			}
			//Get value of AverageCost
			if (Ret.AverageCost != null) {
				double AverageCost13312 = (double)Ret.AverageCost.GetValue();
			}
			//Get value of QuantityOnOrder
			if (Ret.QuantityOnOrder != null) {
				int QuantityOnOrder13313 = (int)Ret.QuantityOnOrder.GetValue();
			}
			//Get value of QuantityOnSalesOrder
			if (Ret.QuantityOnSalesOrder != null) {
				int QuantityOnSalesOrder13314 = (int)Ret.QuantityOnSalesOrder.GetValue();
			}
			//Get value of ExternalGUID
			if (Ret.ExternalGUID != null) {
				string ExternalGUID13315 = (string)Ret.ExternalGUID.GetValue();
			}
			if (Ret.DataExtRetList != null) {
				for (int i13316 = 0; i13316 < Ret.DataExtRetList.Count; i13316++) {
					IDataExtRet DataExtRet = Ret.DataExtRetList.GetAt(i13316);
					//Get value of OwnerID
					if (DataExtRet.OwnerID != null) {
						string OwnerID13317 = (string)DataExtRet.OwnerID.GetValue();
					}
					//Get value of DataExtName
					string DataExtName13318 = (string)DataExtRet.DataExtName.GetValue();
					//Get value of DataExtType
					ENDataExtType DataExtType13319 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
					//Get value of DataExtValue
					string DataExtValue13320 = (string)DataExtRet.DataExtValue.GetValue();
				}
			}
		}
	}
}