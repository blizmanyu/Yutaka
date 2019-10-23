using System;
using Interop.QBFC13;

namespace com.intuit.idn.samples
{
	public partial class Sample
	{
		public void DoItemInventoryQuery()
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

				BuildItemInventoryQueryRq(requestMsgSet);

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

				WalkItemInventoryQueryRs(responseMsgSet);
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

		void BuildItemInventoryQueryRq(IMsgSetRequest requestMsgSet)
		{
			IItemInventoryQuery ItemInventoryQueryRq= requestMsgSet.AppendItemInventoryQueryRq();
			//Set attributes
			//Set field value for metaData
			ItemInventoryQueryRq.metaData.SetValue("IQBENmetaDataType");
			//Set field value for iterator
			ItemInventoryQueryRq.iterator.SetValue("IQBENiteratorType");
			//Set field value for iteratorID
			ItemInventoryQueryRq.iteratorID.SetValue("IQBUUIDType");
			string ORListQueryWithOwnerIDAndClassElementType13273 = "ListIDList";
			if (ORListQueryWithOwnerIDAndClassElementType13273 == "ListIDList") {
				//Set field value for ListIDList
				//May create more than one of these if needed
				ItemInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListIDList.Add("200000-1011023419");
			}
			if (ORListQueryWithOwnerIDAndClassElementType13273 == "FullNameList") {
				//Set field value for FullNameList
				//May create more than one of these if needed
				ItemInventoryQueryRq.ORListQueryWithOwnerIDAndClass.FullNameList.Add("ab");
			}
			if (ORListQueryWithOwnerIDAndClassElementType13273 == "ListWithClassFilter") {
				//Set field value for MaxReturned
				ItemInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.MaxReturned.SetValue(6);
				//Set field value for ActiveStatus
				ItemInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ActiveStatus.SetValue(ENActiveStatus.asActiveOnly[DEFAULT]);
				//Set field value for FromModifiedDate
				ItemInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.FromModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				//Set field value for ToModifiedDate
				ItemInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ToModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				string ORNameFilterElementType13274 = "NameFilter";
				if (ORNameFilterElementType13274 == "NameFilter") {
					//Set field value for MatchCriterion
					ItemInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameFilter.MatchCriterion.SetValue(ENMatchCriterion.mcStartsWith);
					//Set field value for Name
					ItemInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameFilter.Name.SetValue("ab");
				}
				if (ORNameFilterElementType13274 == "NameRangeFilter") {
					//Set field value for FromName
					ItemInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameRangeFilter.FromName.SetValue("ab");
					//Set field value for ToName
					ItemInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameRangeFilter.ToName.SetValue("ab");
				}
				string ORClassFilterElementType13275 = "ListIDList";
				if (ORClassFilterElementType13275 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					ItemInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ClassFilter.ORClassFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORClassFilterElementType13275 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					ItemInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ClassFilter.ORClassFilter.FullNameList.Add("ab");
				}
				if (ORClassFilterElementType13275 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					ItemInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ClassFilter.ORClassFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (ORClassFilterElementType13275 == "FullNameWithChildren") {
					//Set field value for FullNameWithChildren
					ItemInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ClassFilter.ORClassFilter.FullNameWithChildren.SetValue("ab");
				}
			}
			//Set field value for IncludeRetElementList
			//May create more than one of these if needed
			ItemInventoryQueryRq.IncludeRetElementList.Add("ab");
			//Set field value for OwnerIDList
			//May create more than one of these if needed
			ItemInventoryQueryRq.OwnerIDList.Add(Guid.NewGuid().ToString());
		}

		void WalkItemInventoryQueryRs(IMsgSetResponse responseMsgSet)
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
						if (responseType == ENResponseType.rtItemInventoryQueryRs) {
							//upcast to more specific type here, this is safe because we checked with response.Type check above
							IItemInventoryRetList ItemInventoryRet = (IItemInventoryRetList)response.Detail;
							WalkItemInventoryRet(ItemInventoryRet);
						}
					}
				}
			}
		}

		void WalkItemInventoryRet(IItemInventoryRetList ItemInventoryRet)
		{
			if (ItemInventoryRet == null) return;
			//Go through all the elements of IItemInventoryRetList
			//Get value of ListID
			string ListID13276 = (string)ItemInventoryRet.ListID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated13277 = (DateTime)ItemInventoryRet.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified13278 = (DateTime)ItemInventoryRet.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence13279 = (string)ItemInventoryRet.EditSequence.GetValue();
			//Get value of Name
			string Name13280 = (string)ItemInventoryRet.Name.GetValue();
			//Get value of FullName
			string FullName13281 = (string)ItemInventoryRet.FullName.GetValue();
			//Get value of BarCodeValue
			if (ItemInventoryRet.BarCodeValue != null) {
				string BarCodeValue13282 = (string)ItemInventoryRet.BarCodeValue.GetValue();
			}
			//Get value of IsActive
			if (ItemInventoryRet.IsActive != null) {
				bool IsActive13283 = (bool)ItemInventoryRet.IsActive.GetValue();
			}
			if (ItemInventoryRet.ClassRef != null) {
				//Get value of ListID
				if (ItemInventoryRet.ClassRef.ListID != null) {
					string ListID13284 = (string)ItemInventoryRet.ClassRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.ClassRef.FullName != null) {
					string FullName13285 = (string)ItemInventoryRet.ClassRef.FullName.GetValue();
				}
			}
			if (ItemInventoryRet.ParentRef != null) {
				//Get value of ListID
				if (ItemInventoryRet.ParentRef.ListID != null) {
					string ListID13286 = (string)ItemInventoryRet.ParentRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.ParentRef.FullName != null) {
					string FullName13287 = (string)ItemInventoryRet.ParentRef.FullName.GetValue();
				}
			}
			//Get value of Sublevel
			int Sublevel13288 = (int)ItemInventoryRet.Sublevel.GetValue();
			//Get value of ManufacturerPartNumber
			if (ItemInventoryRet.ManufacturerPartNumber != null) {
				string ManufacturerPartNumber13289 = (string)ItemInventoryRet.ManufacturerPartNumber.GetValue();
			}
			if (ItemInventoryRet.UnitOfMeasureSetRef != null) {
				//Get value of ListID
				if (ItemInventoryRet.UnitOfMeasureSetRef.ListID != null) {
					string ListID13290 = (string)ItemInventoryRet.UnitOfMeasureSetRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.UnitOfMeasureSetRef.FullName != null) {
					string FullName13291 = (string)ItemInventoryRet.UnitOfMeasureSetRef.FullName.GetValue();
				}
			}
			//Get value of IsTaxIncluded
			if (ItemInventoryRet.IsTaxIncluded != null) {
				bool IsTaxIncluded13292 = (bool)ItemInventoryRet.IsTaxIncluded.GetValue();
			}
			if (ItemInventoryRet.SalesTaxCodeRef != null) {
				//Get value of ListID
				if (ItemInventoryRet.SalesTaxCodeRef.ListID != null) {
					string ListID13293 = (string)ItemInventoryRet.SalesTaxCodeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.SalesTaxCodeRef.FullName != null) {
					string FullName13294 = (string)ItemInventoryRet.SalesTaxCodeRef.FullName.GetValue();
				}
			}
			//Get value of SalesDesc
			if (ItemInventoryRet.SalesDesc != null) {
				string SalesDesc13295 = (string)ItemInventoryRet.SalesDesc.GetValue();
			}
			//Get value of SalesPrice
			if (ItemInventoryRet.SalesPrice != null) {
				double SalesPrice13296 = (double)ItemInventoryRet.SalesPrice.GetValue();
			}
			if (ItemInventoryRet.IncomeAccountRef != null) {
				//Get value of ListID
				if (ItemInventoryRet.IncomeAccountRef.ListID != null) {
					string ListID13297 = (string)ItemInventoryRet.IncomeAccountRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.IncomeAccountRef.FullName != null) {
					string FullName13298 = (string)ItemInventoryRet.IncomeAccountRef.FullName.GetValue();
				}
			}
			//Get value of PurchaseDesc
			if (ItemInventoryRet.PurchaseDesc != null) {
				string PurchaseDesc13299 = (string)ItemInventoryRet.PurchaseDesc.GetValue();
			}
			//Get value of PurchaseCost
			if (ItemInventoryRet.PurchaseCost != null) {
				double PurchaseCost13300 = (double)ItemInventoryRet.PurchaseCost.GetValue();
			}
			if (ItemInventoryRet.PurchaseTaxCodeRef != null) {
				//Get value of ListID
				if (ItemInventoryRet.PurchaseTaxCodeRef.ListID != null) {
					string ListID13301 = (string)ItemInventoryRet.PurchaseTaxCodeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.PurchaseTaxCodeRef.FullName != null) {
					string FullName13302 = (string)ItemInventoryRet.PurchaseTaxCodeRef.FullName.GetValue();
				}
			}
			if (ItemInventoryRet.COGSAccountRef != null) {
				//Get value of ListID
				if (ItemInventoryRet.COGSAccountRef.ListID != null) {
					string ListID13303 = (string)ItemInventoryRet.COGSAccountRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.COGSAccountRef.FullName != null) {
					string FullName13304 = (string)ItemInventoryRet.COGSAccountRef.FullName.GetValue();
				}
			}
			if (ItemInventoryRet.PrefVendorRef != null) {
				//Get value of ListID
				if (ItemInventoryRet.PrefVendorRef.ListID != null) {
					string ListID13305 = (string)ItemInventoryRet.PrefVendorRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.PrefVendorRef.FullName != null) {
					string FullName13306 = (string)ItemInventoryRet.PrefVendorRef.FullName.GetValue();
				}
			}
			if (ItemInventoryRet.AssetAccountRef != null) {
				//Get value of ListID
				if (ItemInventoryRet.AssetAccountRef.ListID != null) {
					string ListID13307 = (string)ItemInventoryRet.AssetAccountRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.AssetAccountRef.FullName != null) {
					string FullName13308 = (string)ItemInventoryRet.AssetAccountRef.FullName.GetValue();
				}
			}
			//Get value of ReorderPoint
			if (ItemInventoryRet.ReorderPoint != null) {
				int ReorderPoint13309 = (int)ItemInventoryRet.ReorderPoint.GetValue();
			}
			//Get value of Max
			if (ItemInventoryRet.Max != null) {
				int Max13310 = (int)ItemInventoryRet.Max.GetValue();
			}
			//Get value of QuantityOnHand
			if (ItemInventoryRet.QuantityOnHand != null) {
				int QuantityOnHand13311 = (int)ItemInventoryRet.QuantityOnHand.GetValue();
			}
			//Get value of AverageCost
			if (ItemInventoryRet.AverageCost != null) {
				double AverageCost13312 = (double)ItemInventoryRet.AverageCost.GetValue();
			}
			//Get value of QuantityOnOrder
			if (ItemInventoryRet.QuantityOnOrder != null) {
				int QuantityOnOrder13313 = (int)ItemInventoryRet.QuantityOnOrder.GetValue();
			}
			//Get value of QuantityOnSalesOrder
			if (ItemInventoryRet.QuantityOnSalesOrder != null) {
				int QuantityOnSalesOrder13314 = (int)ItemInventoryRet.QuantityOnSalesOrder.GetValue();
			}
			//Get value of ExternalGUID
			if (ItemInventoryRet.ExternalGUID != null) {
				string ExternalGUID13315 = (string)ItemInventoryRet.ExternalGUID.GetValue();
			}
			if (ItemInventoryRet.DataExtRetList != null) {
				for (int i13316 = 0; i13316 < ItemInventoryRet.DataExtRetList.Count; i13316++) {
					IDataExtRet DataExtRet = ItemInventoryRet.DataExtRetList.GetAt(i13316);
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