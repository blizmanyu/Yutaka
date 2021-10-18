using System;
using Interop.QBFC13;

namespace com.intuit.idn.samples
{
	public partial class Sample
	{
		public void DoAdd()
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

				BuildAddRq(requestMsgSet);

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

				WalkAddRs(responseMsgSet);
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

		void BuildAddRq(IMsgSetRequest requestMsgSet)
		{
			IAdd AddRq= requestMsgSet.AppendAddRq();
			//Set attributes
			//Set field value for defMacro
			AddRq.defMacro.SetValue("IQBStringType");
			//Set field value for ListID
			AddRq.AccountRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			AddRq.AccountRef.FullName.SetValue("ab");
			//Set field value for TxnDate
			AddRq.TxnDate.SetValue(DateTime.Parse("12/15/2007"));
			//Set field value for RefNumber
			AddRq.RefNumber.SetValue("ab");
			//Set field value for ListID
			AddRq.InventorySiteRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			AddRq.InventorySiteRef.FullName.SetValue("ab");
			//Set field value for ListID
			AddRq.CustomerRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			AddRq.CustomerRef.FullName.SetValue("ab");
			//Set field value for ListID
			AddRq.ClassRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			AddRq.ClassRef.FullName.SetValue("ab");
			//Set field value for Memo
			AddRq.Memo.SetValue("ab");
			//Set field value for ExternalGUID
			AddRq.ExternalGUID.SetValue(Guid.NewGuid().ToString());
			ILineAdd LineAdd10753=AddRq.LineAddList.Append();
			//Set field value for ListID
			LineAdd10753.ItemRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			LineAdd10753.ItemRef.FullName.SetValue("ab");
			string ORTypeAdjustmentElementType10754 = "QuantityAdjustment";
			if (ORTypeAdjustmentElementType10754 == "QuantityAdjustment") {
				string ORQuantityAdjustmentElementType10755 = "NewQuantity";
				if (ORQuantityAdjustmentElementType10755 == "NewQuantity") {
					//Set field value for NewQuantity
					LineAdd10753.ORTypeAdjustment.QuantityAdjustment.ORQuantityAdjustment.NewQuantity.SetValue(2);
				}
				if (ORQuantityAdjustmentElementType10755 == "QuantityDifference") {
					//Set field value for QuantityDifference
					LineAdd10753.ORTypeAdjustment.QuantityAdjustment.ORQuantityAdjustment.QuantityDifference.SetValue(2);
				}
				string ORSerialLotNumberElementType10756 = "SerialNumber";
				if (ORSerialLotNumberElementType10756 == "SerialNumber") {
					//Set field value for SerialNumber
					LineAdd10753.ORTypeAdjustment.QuantityAdjustment.ORSerialLotNumber.SerialNumber.SetValue("ab");
				}
				if (ORSerialLotNumberElementType10756 == "LotNumber") {
					//Set field value for LotNumber
					LineAdd10753.ORTypeAdjustment.QuantityAdjustment.ORSerialLotNumber.LotNumber.SetValue("ab");
				}
				//Set field value for ListID
				LineAdd10753.ORTypeAdjustment.QuantityAdjustment.InventorySiteLocationRef.ListID.SetValue("200000-1011023419");
				//Set field value for FullName
				LineAdd10753.ORTypeAdjustment.QuantityAdjustment.InventorySiteLocationRef.FullName.SetValue("ab");
			}
			if (ORTypeAdjustmentElementType10754 == "ValueAdjustment") {
				string ORQuantityAdjustmentElementType10757 = "NewQuantity";
				if (ORQuantityAdjustmentElementType10757 == "NewQuantity") {
					//Set field value for NewQuantity
					LineAdd10753.ORTypeAdjustment.ValueAdjustment.ORQuantityAdjustment.NewQuantity.SetValue(2);
				}
				if (ORQuantityAdjustmentElementType10757 == "QuantityDifference") {
					//Set field value for QuantityDifference
					LineAdd10753.ORTypeAdjustment.ValueAdjustment.ORQuantityAdjustment.QuantityDifference.SetValue(2);
				}
				string ORValueAdjustmentElementType10758 = "NewValue";
				if (ORValueAdjustmentElementType10758 == "NewValue") {
					//Set field value for NewValue
					LineAdd10753.ORTypeAdjustment.ValueAdjustment.ORValueAdjustment.NewValue.SetValue(10.01);
				}
				if (ORValueAdjustmentElementType10758 == "ValueDifference") {
					//Set field value for ValueDifference
					LineAdd10753.ORTypeAdjustment.ValueAdjustment.ORValueAdjustment.ValueDifference.SetValue(10.01);
				}
			}
			if (ORTypeAdjustmentElementType10754 == "SerialNumberAdjustment") {
				string ORSerialNumberAdjustmentElementType10759 = "AddSerialNumber";
				if (ORSerialNumberAdjustmentElementType10759 == "AddSerialNumber") {
					//Set field value for AddSerialNumber
					LineAdd10753.ORTypeAdjustment.SerialNumberAdjustment.ORSerialNumberAdjustment.AddSerialNumber.SetValue("ab");
				}
				if (ORSerialNumberAdjustmentElementType10759 == "RemoveSerialNumber") {
					//Set field value for RemoveSerialNumber
					LineAdd10753.ORTypeAdjustment.SerialNumberAdjustment.ORSerialNumberAdjustment.RemoveSerialNumber.SetValue("ab");
				}
				//Set field value for ListID
				LineAdd10753.ORTypeAdjustment.SerialNumberAdjustment.InventorySiteLocationRef.ListID.SetValue("200000-1011023419");
				//Set field value for FullName
				LineAdd10753.ORTypeAdjustment.SerialNumberAdjustment.InventorySiteLocationRef.FullName.SetValue("ab");
			}
			if (ORTypeAdjustmentElementType10754 == "LotNumberAdjustment") {
				//Set field value for LotNumber
				LineAdd10753.ORTypeAdjustment.LotNumberAdjustment.LotNumber.SetValue("ab");
				//Set field value for CountAdjustment
				LineAdd10753.ORTypeAdjustment.LotNumberAdjustment.CountAdjustment.SetValue(6);
				//Set field value for ListID
				LineAdd10753.ORTypeAdjustment.LotNumberAdjustment.InventorySiteLocationRef.ListID.SetValue("200000-1011023419");
				//Set field value for FullName
				LineAdd10753.ORTypeAdjustment.LotNumberAdjustment.InventorySiteLocationRef.FullName.SetValue("ab");
			}
			if (ORTypeAdjustmentElementType10754 == "") {
			}
			//Set field value for IncludeRetElementList
			//May create more than one of these if needed
			AddRq.IncludeRetElementList.Add("ab");
		}

		void WalkAddRs(IMsgSetResponse responseMsgSet)
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
						if (responseType == ENResponseType.rtAddRs) {
							//upcast to more specific type here, this is safe because we checked with response.Type check above
							IRet Ret = (IRet)response.Detail;
							WalkRet(Ret);
						}
					}
				}
			}
		}

		void WalkRet(IRet Ret)
		{
			if (Ret == null) return;
			//Go through all the elements of IRet
			//Get value of TxnID
			string TxnID10760 = (string)Ret.TxnID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated10761 = (DateTime)Ret.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified10762 = (DateTime)Ret.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence10763 = (string)Ret.EditSequence.GetValue();
			//Get value of TxnNumber
			if (Ret.TxnNumber != null) {
				int TxnNumber10764 = (int)Ret.TxnNumber.GetValue();
			}
			//Get value of ListID
			if (Ret.AccountRef.ListID != null) {
				string ListID10765 = (string)Ret.AccountRef.ListID.GetValue();
			}
			//Get value of FullName
			if (Ret.AccountRef.FullName != null) {
				string FullName10766 = (string)Ret.AccountRef.FullName.GetValue();
			}
			if (Ret.InventorySiteRef != null) {
				//Get value of ListID
				if (Ret.InventorySiteRef.ListID != null) {
					string ListID10767 = (string)Ret.InventorySiteRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.InventorySiteRef.FullName != null) {
					string FullName10768 = (string)Ret.InventorySiteRef.FullName.GetValue();
				}
			}
			//Get value of TxnDate
			DateTime TxnDate10769 = (DateTime)Ret.TxnDate.GetValue();
			//Get value of RefNumber
			if (Ret.RefNumber != null) {
				string RefNumber10770 = (string)Ret.RefNumber.GetValue();
			}
			if (Ret.CustomerRef != null) {
				//Get value of ListID
				if (Ret.CustomerRef.ListID != null) {
					string ListID10771 = (string)Ret.CustomerRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.CustomerRef.FullName != null) {
					string FullName10772 = (string)Ret.CustomerRef.FullName.GetValue();
				}
			}
			if (Ret.ClassRef != null) {
				//Get value of ListID
				if (Ret.ClassRef.ListID != null) {
					string ListID10773 = (string)Ret.ClassRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ClassRef.FullName != null) {
					string FullName10774 = (string)Ret.ClassRef.FullName.GetValue();
				}
			}
			//Get value of Memo
			if (Ret.Memo != null) {
				string Memo10775 = (string)Ret.Memo.GetValue();
			}
			//Get value of ExternalGUID
			if (Ret.ExternalGUID != null) {
				string ExternalGUID10776 = (string)Ret.ExternalGUID.GetValue();
			}
			if (Ret.LineRetList != null) {
				for (int i10777 = 0; i10777 < Ret.LineRetList.Count; i10777++) {
					ILineRet LineRet = Ret.LineRetList.GetAt(i10777);
					//Get value of TxnLineID
					string TxnLineID10778 = (string)LineRet.TxnLineID.GetValue();
					//Get value of ListID
					if (LineRet.ItemRef.ListID != null) {
						string ListID10779 = (string)LineRet.ItemRef.ListID.GetValue();
					}
					//Get value of FullName
					if (LineRet.ItemRef.FullName != null) {
						string FullName10780 = (string)LineRet.ItemRef.FullName.GetValue();
					}
					if (LineRet.ORSerialLotNumberPreference != null) {
						if (LineRet.ORSerialLotNumberPreference.SerialNumberRet != null) {
							//Get value of SerialNumberRet
							if (LineRet.ORSerialLotNumberPreference.SerialNumberRet != null) {
								ISerialNumberRet nothing10782 = (ISerialNumberRet)LineRet.ORSerialLotNumberPreference.SerialNumberRet.GetValue();
							}
						}
						if (LineRet.ORSerialLotNumberPreference.LotNumber != null) {
							//Get value of LotNumber
							if (LineRet.ORSerialLotNumberPreference.LotNumber != null) {
								string LotNumber10783 = (string)LineRet.ORSerialLotNumberPreference.LotNumber.GetValue();
							}
						}
					}
					if (LineRet.InventorySiteLocationRef != null) {
						//Get value of ListID
						if (LineRet.InventorySiteLocationRef.ListID != null) {
							string ListID10784 = (string)LineRet.InventorySiteLocationRef.ListID.GetValue();
						}
						//Get value of FullName
						if (LineRet.InventorySiteLocationRef.FullName != null) {
							string FullName10785 = (string)LineRet.InventorySiteLocationRef.FullName.GetValue();
						}
					}
					//Get value of QuantityDifference
					int QuantityDifference10786 = (int)LineRet.QuantityDifference.GetValue();
					//Get value of ValueDifference
					double ValueDifference10787 = (double)LineRet.ValueDifference.GetValue();
				}
			}
			if (Ret.DataExtRetList != null) {
				for (int i10788 = 0; i10788 < Ret.DataExtRetList.Count; i10788++) {
					IDataExtRet DataExtRet = Ret.DataExtRetList.GetAt(i10788);
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