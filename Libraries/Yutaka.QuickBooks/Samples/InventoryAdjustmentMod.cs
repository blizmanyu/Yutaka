using System;
using Interop.QBFC13;

namespace com.intuit.idn.samples
{
	public partial class Sample
	{
		public void DoInventoryAdjustmentMod()
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

				BuildInventoryAdjustmentModRq(requestMsgSet);

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

				WalkInventoryAdjustmentModRs(responseMsgSet);
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

		void BuildInventoryAdjustmentModRq(IMsgSetRequest requestMsgSet)
		{
			IInventoryAdjustmentMod InventoryAdjustmentModRq= requestMsgSet.AppendInventoryAdjustmentModRq();
			//Set field value for TxnID
			InventoryAdjustmentModRq.TxnID.SetValue("200000-1011023419");
			//Set field value for EditSequence
			InventoryAdjustmentModRq.EditSequence.SetValue("ab");
			//Set field value for ListID
			InventoryAdjustmentModRq.AccountRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			InventoryAdjustmentModRq.AccountRef.FullName.SetValue("ab");
			//Set field value for ListID
			InventoryAdjustmentModRq.InventorySiteRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			InventoryAdjustmentModRq.InventorySiteRef.FullName.SetValue("ab");
			//Set field value for TxnDate
			InventoryAdjustmentModRq.TxnDate.SetValue(DateTime.Parse("12/15/2007"));
			//Set field value for RefNumber
			InventoryAdjustmentModRq.RefNumber.SetValue("ab");
			//Set field value for ListID
			InventoryAdjustmentModRq.CustomerRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			InventoryAdjustmentModRq.CustomerRef.FullName.SetValue("ab");
			//Set field value for ListID
			InventoryAdjustmentModRq.ClassRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			InventoryAdjustmentModRq.ClassRef.FullName.SetValue("ab");
			//Set field value for Memo
			InventoryAdjustmentModRq.Memo.SetValue("ab");
			IInventoryAdjustmentLineMod InventoryAdjustmentLineMod10833=InventoryAdjustmentModRq.InventoryAdjustmentLineModList.Append();
			//Set field value for TxnLineID
			InventoryAdjustmentLineMod10833.TxnLineID.SetValue("200000-1011023419");
			//Set field value for ListID
			InventoryAdjustmentLineMod10833.ItemRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			InventoryAdjustmentLineMod10833.ItemRef.FullName.SetValue("ab");
			string ORTypeAdjustmentModElementType10834 = "SerialNumber";
			if (ORTypeAdjustmentModElementType10834 == "SerialNumber") {
				//Set field value for SerialNumber
				InventoryAdjustmentLineMod10833.ORTypeAdjustmentMod.SerialNumber.SetValue("ab");
			}
			if (ORTypeAdjustmentModElementType10834 == "LotAdjustment") {
				//Set field value for LotNumber
				InventoryAdjustmentLineMod10833.ORTypeAdjustmentMod.LotAdjustment.LotNumber.SetValue("ab");
				//Set field value for CountAdjustment
				InventoryAdjustmentLineMod10833.ORTypeAdjustmentMod.LotAdjustment.CountAdjustment.SetValue(6);
			}
			//Set field value for ListID
			InventoryAdjustmentLineMod10833.InventorySiteLocationRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			InventoryAdjustmentLineMod10833.InventorySiteLocationRef.FullName.SetValue("ab");
			//Set field value for QuantityDifference
			InventoryAdjustmentLineMod10833.QuantityDifference.SetValue(2);
			//Set field value for ValueDifference
			InventoryAdjustmentLineMod10833.ValueDifference.SetValue(10.01);
			//Set field value for IncludeRetElementList
			//May create more than one of these if needed
			InventoryAdjustmentModRq.IncludeRetElementList.Add("ab");
		}

		void WalkInventoryAdjustmentModRs(IMsgSetResponse responseMsgSet)
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
						if (responseType == ENResponseType.rtInventoryAdjustmentModRs) {
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
			string TxnID10835 = (string)InventoryAdjustmentRet.TxnID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated10836 = (DateTime)InventoryAdjustmentRet.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified10837 = (DateTime)InventoryAdjustmentRet.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence10838 = (string)InventoryAdjustmentRet.EditSequence.GetValue();
			//Get value of TxnNumber
			if (InventoryAdjustmentRet.TxnNumber != null) {
				int TxnNumber10839 = (int)InventoryAdjustmentRet.TxnNumber.GetValue();
			}
			//Get value of ListID
			if (InventoryAdjustmentRet.AccountRef.ListID != null) {
				string ListID10840 = (string)InventoryAdjustmentRet.AccountRef.ListID.GetValue();
			}
			//Get value of FullName
			if (InventoryAdjustmentRet.AccountRef.FullName != null) {
				string FullName10841 = (string)InventoryAdjustmentRet.AccountRef.FullName.GetValue();
			}
			if (InventoryAdjustmentRet.InventorySiteRef != null) {
				//Get value of ListID
				if (InventoryAdjustmentRet.InventorySiteRef.ListID != null) {
					string ListID10842 = (string)InventoryAdjustmentRet.InventorySiteRef.ListID.GetValue();
				}
				//Get value of FullName
				if (InventoryAdjustmentRet.InventorySiteRef.FullName != null) {
					string FullName10843 = (string)InventoryAdjustmentRet.InventorySiteRef.FullName.GetValue();
				}
			}
			//Get value of TxnDate
			DateTime TxnDate10844 = (DateTime)InventoryAdjustmentRet.TxnDate.GetValue();
			//Get value of RefNumber
			if (InventoryAdjustmentRet.RefNumber != null) {
				string RefNumber10845 = (string)InventoryAdjustmentRet.RefNumber.GetValue();
			}
			if (InventoryAdjustmentRet.CustomerRef != null) {
				//Get value of ListID
				if (InventoryAdjustmentRet.CustomerRef.ListID != null) {
					string ListID10846 = (string)InventoryAdjustmentRet.CustomerRef.ListID.GetValue();
				}
				//Get value of FullName
				if (InventoryAdjustmentRet.CustomerRef.FullName != null) {
					string FullName10847 = (string)InventoryAdjustmentRet.CustomerRef.FullName.GetValue();
				}
			}
			if (InventoryAdjustmentRet.ClassRef != null) {
				//Get value of ListID
				if (InventoryAdjustmentRet.ClassRef.ListID != null) {
					string ListID10848 = (string)InventoryAdjustmentRet.ClassRef.ListID.GetValue();
				}
				//Get value of FullName
				if (InventoryAdjustmentRet.ClassRef.FullName != null) {
					string FullName10849 = (string)InventoryAdjustmentRet.ClassRef.FullName.GetValue();
				}
			}
			//Get value of Memo
			if (InventoryAdjustmentRet.Memo != null) {
				string Memo10850 = (string)InventoryAdjustmentRet.Memo.GetValue();
			}
			//Get value of ExternalGUID
			if (InventoryAdjustmentRet.ExternalGUID != null) {
				string ExternalGUID10851 = (string)InventoryAdjustmentRet.ExternalGUID.GetValue();
			}
			if (InventoryAdjustmentRet.InventoryAdjustmentLineRetList != null) {
				for (int i10852 = 0; i10852 < InventoryAdjustmentRet.InventoryAdjustmentLineRetList.Count; i10852++) {
					IInventoryAdjustmentLineRet InventoryAdjustmentLineRet = InventoryAdjustmentRet.InventoryAdjustmentLineRetList.GetAt(i10852);
					//Get value of TxnLineID
					string TxnLineID10853 = (string)InventoryAdjustmentLineRet.TxnLineID.GetValue();
					//Get value of ListID
					if (InventoryAdjustmentLineRet.ItemRef.ListID != null) {
						string ListID10854 = (string)InventoryAdjustmentLineRet.ItemRef.ListID.GetValue();
					}
					//Get value of FullName
					if (InventoryAdjustmentLineRet.ItemRef.FullName != null) {
						string FullName10855 = (string)InventoryAdjustmentLineRet.ItemRef.FullName.GetValue();
					}
					if (InventoryAdjustmentLineRet.ORSerialLotNumberPreference != null) {
						if (InventoryAdjustmentLineRet.ORSerialLotNumberPreference.SerialNumberRet != null) {
							//Get value of SerialNumberRet
							if (InventoryAdjustmentLineRet.ORSerialLotNumberPreference.SerialNumberRet != null) {
								ISerialNumberRet nothing10857 = (ISerialNumberRet)InventoryAdjustmentLineRet.ORSerialLotNumberPreference.SerialNumberRet.GetValue();
							}
						}
						if (InventoryAdjustmentLineRet.ORSerialLotNumberPreference.LotNumber != null) {
							//Get value of LotNumber
							if (InventoryAdjustmentLineRet.ORSerialLotNumberPreference.LotNumber != null) {
								string LotNumber10858 = (string)InventoryAdjustmentLineRet.ORSerialLotNumberPreference.LotNumber.GetValue();
							}
						}
					}
					if (InventoryAdjustmentLineRet.InventorySiteLocationRef != null) {
						//Get value of ListID
						if (InventoryAdjustmentLineRet.InventorySiteLocationRef.ListID != null) {
							string ListID10859 = (string)InventoryAdjustmentLineRet.InventorySiteLocationRef.ListID.GetValue();
						}
						//Get value of FullName
						if (InventoryAdjustmentLineRet.InventorySiteLocationRef.FullName != null) {
							string FullName10860 = (string)InventoryAdjustmentLineRet.InventorySiteLocationRef.FullName.GetValue();
						}
					}
					//Get value of QuantityDifference
					int QuantityDifference10861 = (int)InventoryAdjustmentLineRet.QuantityDifference.GetValue();
					//Get value of ValueDifference
					double ValueDifference10862 = (double)InventoryAdjustmentLineRet.ValueDifference.GetValue();
				}
			}
			if (InventoryAdjustmentRet.DataExtRetList != null) {
				for (int i10863 = 0; i10863 < InventoryAdjustmentRet.DataExtRetList.Count; i10863++) {
					IDataExtRet DataExtRet = InventoryAdjustmentRet.DataExtRetList.GetAt(i10863);
					//Get value of OwnerID
					if (DataExtRet.OwnerID != null) {
						string OwnerID10864 = (string)DataExtRet.OwnerID.GetValue();
					}
					//Get value of DataExtName
					string DataExtName10865 = (string)DataExtRet.DataExtName.GetValue();
					//Get value of DataExtType
					ENDataExtType DataExtType10866 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
					//Get value of DataExtValue
					string DataExtValue10867 = (string)DataExtRet.DataExtValue.GetValue();
				}
			}
		}
	}
}