using System;
using Interop.QBFC13;

namespace com.intuit.idn.samples
{
	public class Sample
	{
		public void DoItemInventoryMod()
		{
			bool sessionBegun = false;
			bool connectionOpen = false;
			QBSessionManager sessionManager = null;

			try
			{
				//Create the session Manager object
				sessionManager = new QBSessionManager();

				//Create the message set request object to hold our request
				IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US",13,0);
				requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

				BuildItemInventoryModRq(requestMsgSet);

				//Connect to QuickBooks and begin a session
				sessionManager.OpenConnection("","Sample Code from OSR");
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

				WalkItemInventoryModRs(responseMsgSet);
			}
			catch (Exception e)
			{
				if (sessionBegun)
				{
					sessionManager.EndSession();
				}
				if (connectionOpen)
				{
					sessionManager.CloseConnection();
				}
			}
		}
		void BuildItemInventoryModRq(IMsgSetRequest requestMsgSet)
		{
			IItemInventoryMod ItemInventoryModRq= requestMsgSet.AppendItemInventoryModRq();
			//Set field value for ListID
			ItemInventoryModRq.ListID.SetValue("200000-1011023419");
			//Set field value for EditSequence
			ItemInventoryModRq.EditSequence.SetValue("ab");
			//Set field value for Name
			ItemInventoryModRq.Name.SetValue("ab");
			//Set field value for BarCodeValue
			ItemInventoryModRq.BarCode.BarCodeValue.SetValue("ab");
			//Set field value for AssignEvenIfUsed
			ItemInventoryModRq.BarCode.AssignEvenIfUsed.SetValue(true);
			//Set field value for AllowOverride
			ItemInventoryModRq.BarCode.AllowOverride.SetValue(true);
			//Set field value for IsActive
			ItemInventoryModRq.IsActive.SetValue(true);
			//Set field value for ListID
			ItemInventoryModRq.ClassRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			ItemInventoryModRq.ClassRef.FullName.SetValue("ab");
			//Set field value for ListID
			ItemInventoryModRq.ParentRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			ItemInventoryModRq.ParentRef.FullName.SetValue("ab");
			//Set field value for ManufacturerPartNumber
			ItemInventoryModRq.ManufacturerPartNumber.SetValue("ab");
			//Set field value for ListID
			ItemInventoryModRq.UnitOfMeasureSetRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			ItemInventoryModRq.UnitOfMeasureSetRef.FullName.SetValue("ab");
			//Set field value for ForceUOMChange
			ItemInventoryModRq.ForceUOMChange.SetValue(true);
			//Set field value for IsTaxIncluded
			ItemInventoryModRq.IsTaxIncluded.SetValue(true);
			//Set field value for ListID
			ItemInventoryModRq.SalesTaxCodeRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			ItemInventoryModRq.SalesTaxCodeRef.FullName.SetValue("ab");
			//Set field value for SalesDesc
			ItemInventoryModRq.SalesDesc.SetValue("ab");
			//Set field value for SalesPrice
			ItemInventoryModRq.SalesPrice.SetValue(15.65);
			//Set field value for ListID
			ItemInventoryModRq.IncomeAccountRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			ItemInventoryModRq.IncomeAccountRef.FullName.SetValue("ab");
			//Set field value for ApplyIncomeAccountRefToExistingTxns
			ItemInventoryModRq.ApplyIncomeAccountRefToExistingTxns.SetValue(true);
			//Set field value for PurchaseDesc
			ItemInventoryModRq.PurchaseDesc.SetValue("ab");
			//Set field value for PurchaseCost
			ItemInventoryModRq.PurchaseCost.SetValue(15.65);
			//Set field value for ListID
			ItemInventoryModRq.PurchaseTaxCodeRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			ItemInventoryModRq.PurchaseTaxCodeRef.FullName.SetValue("ab");
			//Set field value for ListID
			ItemInventoryModRq.COGSAccountRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			ItemInventoryModRq.COGSAccountRef.FullName.SetValue("ab");
			//Set field value for ApplyCOGSAccountRefToExistingTxns
			ItemInventoryModRq.ApplyCOGSAccountRefToExistingTxns.SetValue(true);
			//Set field value for ListID
			ItemInventoryModRq.PrefVendorRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			ItemInventoryModRq.PrefVendorRef.FullName.SetValue("ab");
			//Set field value for ListID
			ItemInventoryModRq.AssetAccountRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			ItemInventoryModRq.AssetAccountRef.FullName.SetValue("ab");
			//Set field value for ReorderPoint
			ItemInventoryModRq.ReorderPoint.SetValue(2);
			//Set field value for Max
			ItemInventoryModRq.Max.SetValue(2);
			//Set field value for IncludeRetElementList
			//May create more than one of these if needed
			ItemInventoryModRq.IncludeRetElementList.Add("ab");
		}




		void WalkItemInventoryModRs(IMsgSetResponse responseMsgSet)
		{
			if (responseMsgSet == null) return;
			IResponseList responseList = responseMsgSet.ResponseList;
			if (responseList == null) return;
			//if we sent only one request, there is only one response, we'll walk the list for this sample
			for(int i=0; i < responseList.Count; i++)
			{
				IResponse response = responseList.GetAt(i);
				//check the status code of the response, 0=ok, >0 is warning
				if (response.StatusCode >= 0)
				{
					//the request-specific response is in the details, make sure we have some
					if (response.Detail != null)
					{
						//make sure the response is the type we're expecting
						ENResponseType responseType = (ENResponseType)response.Type.GetValue();
						if (responseType == ENResponseType.rtItemInventoryModRs)
						{
							//upcast to more specific type here, this is safe because we checked with response.Type check above
							IItemInventoryRet ItemInventoryRet = (IItemInventoryRet)response.Detail;
							WalkItemInventoryRet(ItemInventoryRet);
						}
					}
				}
			}
		}




		void WalkItemInventoryRet(IItemInventoryRet ItemInventoryRet)
		{
			if (ItemInventoryRet == null) return;
			//Go through all the elements of IItemInventoryRet
			//Get value of ListID
			string ListID13183 = (string)ItemInventoryRet.ListID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated13184 = (DateTime)ItemInventoryRet.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified13185 = (DateTime)ItemInventoryRet.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence13186 = (string)ItemInventoryRet.EditSequence.GetValue();
			//Get value of Name
			string Name13187 = (string)ItemInventoryRet.Name.GetValue();
			//Get value of FullName
			string FullName13188 = (string)ItemInventoryRet.FullName.GetValue();
			//Get value of BarCodeValue
			if (ItemInventoryRet.BarCodeValue != null)
			{
				string BarCodeValue13189 = (string)ItemInventoryRet.BarCodeValue.GetValue();
			}
			//Get value of IsActive
			if (ItemInventoryRet.IsActive != null)
			{
				bool IsActive13190 = (bool)ItemInventoryRet.IsActive.GetValue();
			}
			if (ItemInventoryRet.ClassRef != null)
			{
				//Get value of ListID
				if (ItemInventoryRet.ClassRef.ListID != null)
				{
					string ListID13191 = (string)ItemInventoryRet.ClassRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.ClassRef.FullName != null)
				{
					string FullName13192 = (string)ItemInventoryRet.ClassRef.FullName.GetValue();
				}
			}
			if (ItemInventoryRet.ParentRef != null)
			{
				//Get value of ListID
				if (ItemInventoryRet.ParentRef.ListID != null)
				{
					string ListID13193 = (string)ItemInventoryRet.ParentRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.ParentRef.FullName != null)
				{
					string FullName13194 = (string)ItemInventoryRet.ParentRef.FullName.GetValue();
				}
			}
			//Get value of Sublevel
			int Sublevel13195 = (int)ItemInventoryRet.Sublevel.GetValue();
			//Get value of ManufacturerPartNumber
			if (ItemInventoryRet.ManufacturerPartNumber != null)
			{
				string ManufacturerPartNumber13196 = (string)ItemInventoryRet.ManufacturerPartNumber.GetValue();
			}
			if (ItemInventoryRet.UnitOfMeasureSetRef != null)
			{
				//Get value of ListID
				if (ItemInventoryRet.UnitOfMeasureSetRef.ListID != null)
				{
					string ListID13197 = (string)ItemInventoryRet.UnitOfMeasureSetRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.UnitOfMeasureSetRef.FullName != null)
				{
					string FullName13198 = (string)ItemInventoryRet.UnitOfMeasureSetRef.FullName.GetValue();
				}
			}
			//Get value of IsTaxIncluded
			if (ItemInventoryRet.IsTaxIncluded != null)
			{
				bool IsTaxIncluded13199 = (bool)ItemInventoryRet.IsTaxIncluded.GetValue();
			}
			if (ItemInventoryRet.SalesTaxCodeRef != null)
			{
				//Get value of ListID
				if (ItemInventoryRet.SalesTaxCodeRef.ListID != null)
				{
					string ListID13200 = (string)ItemInventoryRet.SalesTaxCodeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.SalesTaxCodeRef.FullName != null)
				{
					string FullName13201 = (string)ItemInventoryRet.SalesTaxCodeRef.FullName.GetValue();
				}
			}
			//Get value of SalesDesc
			if (ItemInventoryRet.SalesDesc != null)
			{
				string SalesDesc13202 = (string)ItemInventoryRet.SalesDesc.GetValue();
			}
			//Get value of SalesPrice
			if (ItemInventoryRet.SalesPrice != null)
			{
				double SalesPrice13203 = (double)ItemInventoryRet.SalesPrice.GetValue();
			}
			if (ItemInventoryRet.IncomeAccountRef != null)
			{
				//Get value of ListID
				if (ItemInventoryRet.IncomeAccountRef.ListID != null)
				{
					string ListID13204 = (string)ItemInventoryRet.IncomeAccountRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.IncomeAccountRef.FullName != null)
				{
					string FullName13205 = (string)ItemInventoryRet.IncomeAccountRef.FullName.GetValue();
				}
			}
			//Get value of PurchaseDesc
			if (ItemInventoryRet.PurchaseDesc != null)
			{
				string PurchaseDesc13206 = (string)ItemInventoryRet.PurchaseDesc.GetValue();
			}
			//Get value of PurchaseCost
			if (ItemInventoryRet.PurchaseCost != null)
			{
				double PurchaseCost13207 = (double)ItemInventoryRet.PurchaseCost.GetValue();
			}
			if (ItemInventoryRet.PurchaseTaxCodeRef != null)
			{
				//Get value of ListID
				if (ItemInventoryRet.PurchaseTaxCodeRef.ListID != null)
				{
					string ListID13208 = (string)ItemInventoryRet.PurchaseTaxCodeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.PurchaseTaxCodeRef.FullName != null)
				{
					string FullName13209 = (string)ItemInventoryRet.PurchaseTaxCodeRef.FullName.GetValue();
				}
			}
			if (ItemInventoryRet.COGSAccountRef != null)
			{
				//Get value of ListID
				if (ItemInventoryRet.COGSAccountRef.ListID != null)
				{
					string ListID13210 = (string)ItemInventoryRet.COGSAccountRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.COGSAccountRef.FullName != null)
				{
					string FullName13211 = (string)ItemInventoryRet.COGSAccountRef.FullName.GetValue();
				}
			}
			if (ItemInventoryRet.PrefVendorRef != null)
			{
				//Get value of ListID
				if (ItemInventoryRet.PrefVendorRef.ListID != null)
				{
					string ListID13212 = (string)ItemInventoryRet.PrefVendorRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.PrefVendorRef.FullName != null)
				{
					string FullName13213 = (string)ItemInventoryRet.PrefVendorRef.FullName.GetValue();
				}
			}
			if (ItemInventoryRet.AssetAccountRef != null)
			{
				//Get value of ListID
				if (ItemInventoryRet.AssetAccountRef.ListID != null)
				{
					string ListID13214 = (string)ItemInventoryRet.AssetAccountRef.ListID.GetValue();
				}
				//Get value of FullName
				if (ItemInventoryRet.AssetAccountRef.FullName != null)
				{
					string FullName13215 = (string)ItemInventoryRet.AssetAccountRef.FullName.GetValue();
				}
			}
			//Get value of ReorderPoint
			if (ItemInventoryRet.ReorderPoint != null)
			{
				int ReorderPoint13216 = (int)ItemInventoryRet.ReorderPoint.GetValue();
			}
			//Get value of Max
			if (ItemInventoryRet.Max != null)
			{
				int Max13217 = (int)ItemInventoryRet.Max.GetValue();
			}
			//Get value of QuantityOnHand
			if (ItemInventoryRet.QuantityOnHand != null)
			{
				int QuantityOnHand13218 = (int)ItemInventoryRet.QuantityOnHand.GetValue();
			}
			//Get value of AverageCost
			if (ItemInventoryRet.AverageCost != null)
			{
				double AverageCost13219 = (double)ItemInventoryRet.AverageCost.GetValue();
			}
			//Get value of QuantityOnOrder
			if (ItemInventoryRet.QuantityOnOrder != null)
			{
				int QuantityOnOrder13220 = (int)ItemInventoryRet.QuantityOnOrder.GetValue();
			}
			//Get value of QuantityOnSalesOrder
			if (ItemInventoryRet.QuantityOnSalesOrder != null)
			{
				int QuantityOnSalesOrder13221 = (int)ItemInventoryRet.QuantityOnSalesOrder.GetValue();
			}
			//Get value of ExternalGUID
			if (ItemInventoryRet.ExternalGUID != null)
			{
				string ExternalGUID13222 = (string)ItemInventoryRet.ExternalGUID.GetValue();
			}
			if (ItemInventoryRet.DataExtRetList != null)
			{
				for (int i13223 = 0; i13223 < ItemInventoryRet.DataExtRetList.Count; i13223++)
				{
					IDataExtRet DataExtRet = ItemInventoryRet.DataExtRetList.GetAt(i13223);
					//Get value of OwnerID
					if (DataExtRet.OwnerID != null)
					{
						string OwnerID13224 = (string)DataExtRet.OwnerID.GetValue();
					}
					//Get value of DataExtName
					string DataExtName13225 = (string)DataExtRet.DataExtName.GetValue();
					//Get value of DataExtType
					ENDataExtType DataExtType13226 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
					//Get value of DataExtValue
					string DataExtValue13227 = (string)DataExtRet.DataExtValue.GetValue();
				}
			}
		}
	}
}