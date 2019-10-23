using System;
using Interop.QBFC13;

namespace com.intuit.idn.samples
{
	public partial class Sample
	{
		public void DoInventoryAdjustmentAdd()
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

				BuildInventoryAdjustmentAddRq(requestMsgSet);

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

				WalkInventoryAdjustmentAddRs(responseMsgSet);
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

		void BuildInventoryAdjustmentAddRq(IMsgSetRequest requestMsgSet)
		{
			IInventoryAdjustmentAdd InventoryAdjustmentAddRq= requestMsgSet.AppendInventoryAdjustmentAddRq();
			//Set attributes
			//Set field value for defMacro
			InventoryAdjustmentAddRq.defMacro.SetValue("IQBStringType");
			//Set field value for ListID
			InventoryAdjustmentAddRq.AccountRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			InventoryAdjustmentAddRq.AccountRef.FullName.SetValue("ab");
			//Set field value for TxnDate
			InventoryAdjustmentAddRq.TxnDate.SetValue(DateTime.Parse("12/15/2007"));
			//Set field value for RefNumber
			InventoryAdjustmentAddRq.RefNumber.SetValue("ab");
			//Set field value for ListID
			InventoryAdjustmentAddRq.InventorySiteRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			InventoryAdjustmentAddRq.InventorySiteRef.FullName.SetValue("ab");
			//Set field value for ListID
			InventoryAdjustmentAddRq.CustomerRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			InventoryAdjustmentAddRq.CustomerRef.FullName.SetValue("ab");
			//Set field value for ListID
			InventoryAdjustmentAddRq.ClassRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			InventoryAdjustmentAddRq.ClassRef.FullName.SetValue("ab");
			//Set field value for Memo
			InventoryAdjustmentAddRq.Memo.SetValue("ab");
			//Set field value for ExternalGUID
			InventoryAdjustmentAddRq.ExternalGUID.SetValue(Guid.NewGuid().ToString());
			IInventoryAdjustmentLineAdd InventoryAdjustmentLineAdd10753=InventoryAdjustmentAddRq.InventoryAdjustmentLineAddList.Append();
			//Set field value for ListID
			InventoryAdjustmentLineAdd10753.ItemRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			InventoryAdjustmentLineAdd10753.ItemRef.FullName.SetValue("ab");
			string ORTypeAdjustmentElementType10754 = "QuantityAdjustment";
			if (ORTypeAdjustmentElementType10754 == "QuantityAdjustment") {
				string ORQuantityAdjustmentElementType10755 = "NewQuantity";
				if (ORQuantityAdjustmentElementType10755 == "NewQuantity") {
					//Set field value for NewQuantity
					InventoryAdjustmentLineAdd10753.ORTypeAdjustment.QuantityAdjustment.ORQuantityAdjustment.NewQuantity.SetValue(2);
				}
				if (ORQuantityAdjustmentElementType10755 == "QuantityDifference") {
					//Set field value for QuantityDifference
					InventoryAdjustmentLineAdd10753.ORTypeAdjustment.QuantityAdjustment.ORQuantityAdjustment.QuantityDifference.SetValue(2);
				}
				string ORSerialLotNumberElementType10756 = "SerialNumber";
				if (ORSerialLotNumberElementType10756 == "SerialNumber") {
					//Set field value for SerialNumber
					InventoryAdjustmentLineAdd10753.ORTypeAdjustment.QuantityAdjustment.ORSerialLotNumber.SerialNumber.SetValue("ab");
				}
				if (ORSerialLotNumberElementType10756 == "LotNumber") {
					//Set field value for LotNumber
					InventoryAdjustmentLineAdd10753.ORTypeAdjustment.QuantityAdjustment.ORSerialLotNumber.LotNumber.SetValue("ab");
				}
				//Set field value for ListID
				InventoryAdjustmentLineAdd10753.ORTypeAdjustment.QuantityAdjustment.InventorySiteLocationRef.ListID.SetValue("200000-1011023419");
				//Set field value for FullName
				InventoryAdjustmentLineAdd10753.ORTypeAdjustment.QuantityAdjustment.InventorySiteLocationRef.FullName.SetValue("ab");
			}
			if (ORTypeAdjustmentElementType10754 == "ValueAdjustment") {
				string ORQuantityAdjustmentElementType10757 = "NewQuantity";
				if (ORQuantityAdjustmentElementType10757 == "NewQuantity") {
					//Set field value for NewQuantity
					InventoryAdjustmentLineAdd10753.ORTypeAdjustment.ValueAdjustment.ORQuantityAdjustment.NewQuantity.SetValue(2);
				}
				if (ORQuantityAdjustmentElementType10757 == "QuantityDifference") {
					//Set field value for QuantityDifference
					InventoryAdjustmentLineAdd10753.ORTypeAdjustment.ValueAdjustment.ORQuantityAdjustment.QuantityDifference.SetValue(2);
				}
				string ORValueAdjustmentElementType10758 = "NewValue";
				if (ORValueAdjustmentElementType10758 == "NewValue") {
					//Set field value for NewValue
					InventoryAdjustmentLineAdd10753.ORTypeAdjustment.ValueAdjustment.ORValueAdjustment.NewValue.SetValue(10.01);
				}
				if (ORValueAdjustmentElementType10758 == "ValueDifference") {
					//Set field value for ValueDifference
					InventoryAdjustmentLineAdd10753.ORTypeAdjustment.ValueAdjustment.ORValueAdjustment.ValueDifference.SetValue(10.01);
				}
			}
			if (ORTypeAdjustmentElementType10754 == "SerialNumberAdjustment") {
				string ORSerialNumberAdjustmentElementType10759 = "AddSerialNumber";
				if (ORSerialNumberAdjustmentElementType10759 == "AddSerialNumber") {
					//Set field value for AddSerialNumber
					InventoryAdjustmentLineAdd10753.ORTypeAdjustment.SerialNumberAdjustment.ORSerialNumberAdjustment.AddSerialNumber.SetValue("ab");
				}
				if (ORSerialNumberAdjustmentElementType10759 == "RemoveSerialNumber") {
					//Set field value for RemoveSerialNumber
					InventoryAdjustmentLineAdd10753.ORTypeAdjustment.SerialNumberAdjustment.ORSerialNumberAdjustment.RemoveSerialNumber.SetValue("ab");
				}
				//Set field value for ListID
				InventoryAdjustmentLineAdd10753.ORTypeAdjustment.SerialNumberAdjustment.InventorySiteLocationRef.ListID.SetValue("200000-1011023419");
				//Set field value for FullName
				InventoryAdjustmentLineAdd10753.ORTypeAdjustment.SerialNumberAdjustment.InventorySiteLocationRef.FullName.SetValue("ab");
			}
			if (ORTypeAdjustmentElementType10754 == "LotNumberAdjustment") {
				//Set field value for LotNumber
				InventoryAdjustmentLineAdd10753.ORTypeAdjustment.LotNumberAdjustment.LotNumber.SetValue("ab");
				//Set field value for CountAdjustment
				InventoryAdjustmentLineAdd10753.ORTypeAdjustment.LotNumberAdjustment.CountAdjustment.SetValue(6);
				//Set field value for ListID
				InventoryAdjustmentLineAdd10753.ORTypeAdjustment.LotNumberAdjustment.InventorySiteLocationRef.ListID.SetValue("200000-1011023419");
				//Set field value for FullName
				InventoryAdjustmentLineAdd10753.ORTypeAdjustment.LotNumberAdjustment.InventorySiteLocationRef.FullName.SetValue("ab");
			}
			if (ORTypeAdjustmentElementType10754 == "") {
			}
			//Set field value for IncludeRetElementList
			//May create more than one of these if needed
			InventoryAdjustmentAddRq.IncludeRetElementList.Add("ab");
		}

		void WalkInventoryAdjustmentAddRs(IMsgSetResponse responseMsgSet)
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
						if (responseType == ENResponseType.rtInventoryAdjustmentAddRs) {
							//upcast to more specific type here, this is safe because we checked with response.Type check above
							IInventoryAdjustmentRet InventoryAdjustmentRet = (IInventoryAdjustmentRet)response.Detail;
							WalkInventoryAdjustmentRet(InventoryAdjustmentRet);
						}
					}
				}
			}
		}

		void WalkInventoryAdjustmentRet(IInventoryAdjustmentRet InventoryAdjustmentRet)
		{
			if (InventoryAdjustmentRet == null) return;
			//Go through all the elements of IInventoryAdjustmentRet
			//Get value of TxnID
			string TxnID10760 = (string)InventoryAdjustmentRet.TxnID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated10761 = (DateTime)InventoryAdjustmentRet.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified10762 = (DateTime)InventoryAdjustmentRet.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence10763 = (string)InventoryAdjustmentRet.EditSequence.GetValue();
			//Get value of TxnNumber
			if (InventoryAdjustmentRet.TxnNumber != null) {
				int TxnNumber10764 = (int)InventoryAdjustmentRet.TxnNumber.GetValue();
			}
			//Get value of ListID
			if (InventoryAdjustmentRet.AccountRef.ListID != null) {
				string ListID10765 = (string)InventoryAdjustmentRet.AccountRef.ListID.GetValue();
			}
			//Get value of FullName
			if (InventoryAdjustmentRet.AccountRef.FullName != null) {
				string FullName10766 = (string)InventoryAdjustmentRet.AccountRef.FullName.GetValue();
			}
			if (InventoryAdjustmentRet.InventorySiteRef != null) {
				//Get value of ListID
				if (InventoryAdjustmentRet.InventorySiteRef.ListID != null) {
					string ListID10767 = (string)InventoryAdjustmentRet.InventorySiteRef.ListID.GetValue();
				}
				//Get value of FullName
				if (InventoryAdjustmentRet.InventorySiteRef.FullName != null) {
					string FullName10768 = (string)InventoryAdjustmentRet.InventorySiteRef.FullName.GetValue();
				}
			}
			//Get value of TxnDate
			DateTime TxnDate10769 = (DateTime)InventoryAdjustmentRet.TxnDate.GetValue();
			//Get value of RefNumber
			if (InventoryAdjustmentRet.RefNumber != null) {
				string RefNumber10770 = (string)InventoryAdjustmentRet.RefNumber.GetValue();
			}
			if (InventoryAdjustmentRet.CustomerRef != null) {
				//Get value of ListID
				if (InventoryAdjustmentRet.CustomerRef.ListID != null) {
					string ListID10771 = (string)InventoryAdjustmentRet.CustomerRef.ListID.GetValue();
				}
				//Get value of FullName
				if (InventoryAdjustmentRet.CustomerRef.FullName != null) {
					string FullName10772 = (string)InventoryAdjustmentRet.CustomerRef.FullName.GetValue();
				}
			}
			if (InventoryAdjustmentRet.ClassRef != null) {
				//Get value of ListID
				if (InventoryAdjustmentRet.ClassRef.ListID != null) {
					string ListID10773 = (string)InventoryAdjustmentRet.ClassRef.ListID.GetValue();
				}
				//Get value of FullName
				if (InventoryAdjustmentRet.ClassRef.FullName != null) {
					string FullName10774 = (string)InventoryAdjustmentRet.ClassRef.FullName.GetValue();
				}
			}
			//Get value of Memo
			if (InventoryAdjustmentRet.Memo != null) {
				string Memo10775 = (string)InventoryAdjustmentRet.Memo.GetValue();
			}
			//Get value of ExternalGUID
			if (InventoryAdjustmentRet.ExternalGUID != null) {
				string ExternalGUID10776 = (string)InventoryAdjustmentRet.ExternalGUID.GetValue();
			}
			if (InventoryAdjustmentRet.InventoryAdjustmentLineRetList != null) {
				for (int i10777 = 0; i10777 < InventoryAdjustmentRet.InventoryAdjustmentLineRetList.Count; i10777++) {
					IInventoryAdjustmentLineRet InventoryAdjustmentLineRet = InventoryAdjustmentRet.InventoryAdjustmentLineRetList.GetAt(i10777);
					//Get value of TxnLineID
					string TxnLineID10778 = (string)InventoryAdjustmentLineRet.TxnLineID.GetValue();
					//Get value of ListID
					if (InventoryAdjustmentLineRet.ItemRef.ListID != null) {
						string ListID10779 = (string)InventoryAdjustmentLineRet.ItemRef.ListID.GetValue();
					}
					//Get value of FullName
					if (InventoryAdjustmentLineRet.ItemRef.FullName != null) {
						string FullName10780 = (string)InventoryAdjustmentLineRet.ItemRef.FullName.GetValue();
					}
					if (InventoryAdjustmentLineRet.ORSerialLotNumberPreference != null) {
						if (InventoryAdjustmentLineRet.ORSerialLotNumberPreference.SerialNumberRet != null) {
							//Get value of SerialNumberRet
							if (InventoryAdjustmentLineRet.ORSerialLotNumberPreference.SerialNumberRet != null) {
								ISerialNumberRet nothing10782 = (ISerialNumberRet)InventoryAdjustmentLineRet.ORSerialLotNumberPreference.SerialNumberRet.GetValue();
							}
						}
						if (InventoryAdjustmentLineRet.ORSerialLotNumberPreference.LotNumber != null) {
							//Get value of LotNumber
							if (InventoryAdjustmentLineRet.ORSerialLotNumberPreference.LotNumber != null) {
								string LotNumber10783 = (string)InventoryAdjustmentLineRet.ORSerialLotNumberPreference.LotNumber.GetValue();
							}
						}
					}
					if (InventoryAdjustmentLineRet.InventorySiteLocationRef != null) {
						//Get value of ListID
						if (InventoryAdjustmentLineRet.InventorySiteLocationRef.ListID != null) {
							string ListID10784 = (string)InventoryAdjustmentLineRet.InventorySiteLocationRef.ListID.GetValue();
						}
						//Get value of FullName
						if (InventoryAdjustmentLineRet.InventorySiteLocationRef.FullName != null) {
							string FullName10785 = (string)InventoryAdjustmentLineRet.InventorySiteLocationRef.FullName.GetValue();
						}
					}
					//Get value of QuantityDifference
					int QuantityDifference10786 = (int)InventoryAdjustmentLineRet.QuantityDifference.GetValue();
					//Get value of ValueDifference
					double ValueDifference10787 = (double)InventoryAdjustmentLineRet.ValueDifference.GetValue();
				}
			}
			if (InventoryAdjustmentRet.DataExtRetList != null) {
				for (int i10788 = 0; i10788 < InventoryAdjustmentRet.DataExtRetList.Count; i10788++) {
					IDataExtRet DataExtRet = InventoryAdjustmentRet.DataExtRetList.GetAt(i10788);
					//Get value of OwnerID
					if (DataExtRet.OwnerID != null) {
						string OwnerID10789 = (string)DataExtRet.OwnerID.GetValue();
					}
					//Get value of DataExtName
					string DataExtName10790 = (string)DataExtRet.DataExtName.GetValue();
					//Get value of DataExtType
					ENDataExtType DataExtType10791 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
					//Get value of DataExtValue
					string DataExtValue10792 = (string)DataExtRet.DataExtValue.GetValue();
				}
			}
		}
	}
}