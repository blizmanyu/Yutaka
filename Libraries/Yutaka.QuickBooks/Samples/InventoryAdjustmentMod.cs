using System;
using Interop.QBFC13;

namespace com.intuit.idn.samples
{
	public partial class Sample
	{
		public void DoMod()
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

				BuildModRq(requestMsgSet);

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

				WalkModRs(responseMsgSet);
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

		void BuildModRq(IMsgSetRequest requestMsgSet)
		{
			IMod ModRq= requestMsgSet.AppendModRq();
			//Set field value for TxnID
			ModRq.TxnID.SetValue("200000-1011023419");
			//Set field value for EditSequence
			ModRq.EditSequence.SetValue("ab");
			//Set field value for ListID
			ModRq.AccountRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			ModRq.AccountRef.FullName.SetValue("ab");
			//Set field value for ListID
			ModRq.InventorySiteRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			ModRq.InventorySiteRef.FullName.SetValue("ab");
			//Set field value for TxnDate
			ModRq.TxnDate.SetValue(DateTime.Parse("12/15/2007"));
			//Set field value for RefNumber
			ModRq.RefNumber.SetValue("ab");
			//Set field value for ListID
			ModRq.CustomerRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			ModRq.CustomerRef.FullName.SetValue("ab");
			//Set field value for ListID
			ModRq.ClassRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			ModRq.ClassRef.FullName.SetValue("ab");
			//Set field value for Memo
			ModRq.Memo.SetValue("ab");
			ILineMod LineMod10833=ModRq.LineModList.Append();
			//Set field value for TxnLineID
			LineMod10833.TxnLineID.SetValue("200000-1011023419");
			//Set field value for ListID
			LineMod10833.ItemRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			LineMod10833.ItemRef.FullName.SetValue("ab");
			string ORTypeAdjustmentModElementType10834 = "SerialNumber";
			if (ORTypeAdjustmentModElementType10834 == "SerialNumber") {
				//Set field value for SerialNumber
				LineMod10833.ORTypeAdjustmentMod.SerialNumber.SetValue("ab");
			}
			if (ORTypeAdjustmentModElementType10834 == "LotAdjustment") {
				//Set field value for LotNumber
				LineMod10833.ORTypeAdjustmentMod.LotAdjustment.LotNumber.SetValue("ab");
				//Set field value for CountAdjustment
				LineMod10833.ORTypeAdjustmentMod.LotAdjustment.CountAdjustment.SetValue(6);
			}
			//Set field value for ListID
			LineMod10833.InventorySiteLocationRef.ListID.SetValue("200000-1011023419");
			//Set field value for FullName
			LineMod10833.InventorySiteLocationRef.FullName.SetValue("ab");
			//Set field value for QuantityDifference
			LineMod10833.QuantityDifference.SetValue(2);
			//Set field value for ValueDifference
			LineMod10833.ValueDifference.SetValue(10.01);
			//Set field value for IncludeRetElementList
			//May create more than one of these if needed
			ModRq.IncludeRetElementList.Add("ab");
		}

		void WalkModRs(IMsgSetResponse responseMsgSet)
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
						if (responseType == ENResponseType.rtModRs) {
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
			string TxnID10835 = (string)Ret.TxnID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated10836 = (DateTime)Ret.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified10837 = (DateTime)Ret.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence10838 = (string)Ret.EditSequence.GetValue();
			//Get value of TxnNumber
			if (Ret.TxnNumber != null) {
				int TxnNumber10839 = (int)Ret.TxnNumber.GetValue();
			}
			//Get value of ListID
			if (Ret.AccountRef.ListID != null) {
				string ListID10840 = (string)Ret.AccountRef.ListID.GetValue();
			}
			//Get value of FullName
			if (Ret.AccountRef.FullName != null) {
				string FullName10841 = (string)Ret.AccountRef.FullName.GetValue();
			}
			if (Ret.InventorySiteRef != null) {
				//Get value of ListID
				if (Ret.InventorySiteRef.ListID != null) {
					string ListID10842 = (string)Ret.InventorySiteRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.InventorySiteRef.FullName != null) {
					string FullName10843 = (string)Ret.InventorySiteRef.FullName.GetValue();
				}
			}
			//Get value of TxnDate
			DateTime TxnDate10844 = (DateTime)Ret.TxnDate.GetValue();
			//Get value of RefNumber
			if (Ret.RefNumber != null) {
				string RefNumber10845 = (string)Ret.RefNumber.GetValue();
			}
			if (Ret.CustomerRef != null) {
				//Get value of ListID
				if (Ret.CustomerRef.ListID != null) {
					string ListID10846 = (string)Ret.CustomerRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.CustomerRef.FullName != null) {
					string FullName10847 = (string)Ret.CustomerRef.FullName.GetValue();
				}
			}
			if (Ret.ClassRef != null) {
				//Get value of ListID
				if (Ret.ClassRef.ListID != null) {
					string ListID10848 = (string)Ret.ClassRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ClassRef.FullName != null) {
					string FullName10849 = (string)Ret.ClassRef.FullName.GetValue();
				}
			}
			//Get value of Memo
			if (Ret.Memo != null) {
				string Memo10850 = (string)Ret.Memo.GetValue();
			}
			//Get value of ExternalGUID
			if (Ret.ExternalGUID != null) {
				string ExternalGUID10851 = (string)Ret.ExternalGUID.GetValue();
			}
			if (Ret.LineRetList != null) {
				for (int i10852 = 0; i10852 < Ret.LineRetList.Count; i10852++) {
					ILineRet LineRet = Ret.LineRetList.GetAt(i10852);
					//Get value of TxnLineID
					string TxnLineID10853 = (string)LineRet.TxnLineID.GetValue();
					//Get value of ListID
					if (LineRet.ItemRef.ListID != null) {
						string ListID10854 = (string)LineRet.ItemRef.ListID.GetValue();
					}
					//Get value of FullName
					if (LineRet.ItemRef.FullName != null) {
						string FullName10855 = (string)LineRet.ItemRef.FullName.GetValue();
					}
					if (LineRet.ORSerialLotNumberPreference != null) {
						if (LineRet.ORSerialLotNumberPreference.SerialNumberRet != null) {
							//Get value of SerialNumberRet
							if (LineRet.ORSerialLotNumberPreference.SerialNumberRet != null) {
								ISerialNumberRet nothing10857 = (ISerialNumberRet)LineRet.ORSerialLotNumberPreference.SerialNumberRet.GetValue();
							}
						}
						if (LineRet.ORSerialLotNumberPreference.LotNumber != null) {
							//Get value of LotNumber
							if (LineRet.ORSerialLotNumberPreference.LotNumber != null) {
								string LotNumber10858 = (string)LineRet.ORSerialLotNumberPreference.LotNumber.GetValue();
							}
						}
					}
					if (LineRet.InventorySiteLocationRef != null) {
						//Get value of ListID
						if (LineRet.InventorySiteLocationRef.ListID != null) {
							string ListID10859 = (string)LineRet.InventorySiteLocationRef.ListID.GetValue();
						}
						//Get value of FullName
						if (LineRet.InventorySiteLocationRef.FullName != null) {
							string FullName10860 = (string)LineRet.InventorySiteLocationRef.FullName.GetValue();
						}
					}
					//Get value of QuantityDifference
					int QuantityDifference10861 = (int)LineRet.QuantityDifference.GetValue();
					//Get value of ValueDifference
					double ValueDifference10862 = (double)LineRet.ValueDifference.GetValue();
				}
			}
			if (Ret.DataExtRetList != null) {
				for (int i10863 = 0; i10863 < Ret.DataExtRetList.Count; i10863++) {
					IDataExtRet DataExtRet = Ret.DataExtRetList.GetAt(i10863);
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