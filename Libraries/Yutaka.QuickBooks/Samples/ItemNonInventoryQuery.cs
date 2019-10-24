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
			string ORListQueryWithOwnerIDAndClassElementType13553 = "ListIDList";
			if (ORListQueryWithOwnerIDAndClassElementType13553 == "ListIDList") {
				//Set field value for ListIDList
				//May create more than one of these if needed
				QueryRq.ORListQueryWithOwnerIDAndClass.ListIDList.Add("200000-1011023419");
			}
			if (ORListQueryWithOwnerIDAndClassElementType13553 == "FullNameList") {
				//Set field value for FullNameList
				//May create more than one of these if needed
				QueryRq.ORListQueryWithOwnerIDAndClass.FullNameList.Add("ab");
			}
			if (ORListQueryWithOwnerIDAndClassElementType13553 == "ListWithClassFilter") {
				//Set field value for MaxReturned
				QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.MaxReturned.SetValue(6);
				//Set field value for ActiveStatus
				QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ActiveStatus.SetValue(ENActiveStatus.asActiveOnly[DEFAULT]);
				//Set field value for FromModifiedDate
				QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.FromModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				//Set field value for ToModifiedDate
				QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ToModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
				string ORNameFilterElementType13554 = "NameFilter";
				if (ORNameFilterElementType13554 == "NameFilter") {
					//Set field value for MatchCriterion
					QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameFilter.MatchCriterion.SetValue(ENMatchCriterion.mcStartsWith);
					//Set field value for Name
					QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameFilter.Name.SetValue("ab");
				}
				if (ORNameFilterElementType13554 == "NameRangeFilter") {
					//Set field value for FromName
					QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameRangeFilter.FromName.SetValue("ab");
					//Set field value for ToName
					QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameRangeFilter.ToName.SetValue("ab");
				}
				string ORClassFilterElementType13555 = "ListIDList";
				if (ORClassFilterElementType13555 == "ListIDList") {
					//Set field value for ListIDList
					//May create more than one of these if needed
					QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ClassFilter.ORClassFilter.ListIDList.Add("200000-1011023419");
				}
				if (ORClassFilterElementType13555 == "FullNameList") {
					//Set field value for FullNameList
					//May create more than one of these if needed
					QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ClassFilter.ORClassFilter.FullNameList.Add("ab");
				}
				if (ORClassFilterElementType13555 == "ListIDWithChildren") {
					//Set field value for ListIDWithChildren
					QueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ClassFilter.ORClassFilter.ListIDWithChildren.SetValue("200000-1011023419");
				}
				if (ORClassFilterElementType13555 == "FullNameWithChildren") {
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
			string ListID13556 = (string)Ret.ListID.GetValue();
			//Get value of TimeCreated
			DateTime TimeCreated13557 = (DateTime)Ret.TimeCreated.GetValue();
			//Get value of TimeModified
			DateTime TimeModified13558 = (DateTime)Ret.TimeModified.GetValue();
			//Get value of EditSequence
			string EditSequence13559 = (string)Ret.EditSequence.GetValue();
			//Get value of Name
			string Name13560 = (string)Ret.Name.GetValue();
			//Get value of FullName
			string FullName13561 = (string)Ret.FullName.GetValue();
			//Get value of BarCodeValue
			if (Ret.BarCodeValue != null) {
				string BarCodeValue13562 = (string)Ret.BarCodeValue.GetValue();
			}
			//Get value of IsActive
			if (Ret.IsActive != null) {
				bool IsActive13563 = (bool)Ret.IsActive.GetValue();
			}
			if (Ret.ClassRef != null) {
				//Get value of ListID
				if (Ret.ClassRef.ListID != null) {
					string ListID13564 = (string)Ret.ClassRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ClassRef.FullName != null) {
					string FullName13565 = (string)Ret.ClassRef.FullName.GetValue();
				}
			}
			if (Ret.ParentRef != null) {
				//Get value of ListID
				if (Ret.ParentRef.ListID != null) {
					string ListID13566 = (string)Ret.ParentRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.ParentRef.FullName != null) {
					string FullName13567 = (string)Ret.ParentRef.FullName.GetValue();
				}
			}
			//Get value of Sublevel
			int Sublevel13568 = (int)Ret.Sublevel.GetValue();
			//Get value of ManufacturerPartNumber
			if (Ret.ManufacturerPartNumber != null) {
				string ManufacturerPartNumber13569 = (string)Ret.ManufacturerPartNumber.GetValue();
			}
			if (Ret.UnitOfMeasureSetRef != null) {
				//Get value of ListID
				if (Ret.UnitOfMeasureSetRef.ListID != null) {
					string ListID13570 = (string)Ret.UnitOfMeasureSetRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.UnitOfMeasureSetRef.FullName != null) {
					string FullName13571 = (string)Ret.UnitOfMeasureSetRef.FullName.GetValue();
				}
			}
			//Get value of IsTaxIncluded
			if (Ret.IsTaxIncluded != null) {
				bool IsTaxIncluded13572 = (bool)Ret.IsTaxIncluded.GetValue();
			}
			if (Ret.SalesTaxCodeRef != null) {
				//Get value of ListID
				if (Ret.SalesTaxCodeRef.ListID != null) {
					string ListID13573 = (string)Ret.SalesTaxCodeRef.ListID.GetValue();
				}
				//Get value of FullName
				if (Ret.SalesTaxCodeRef.FullName != null) {
					string FullName13574 = (string)Ret.SalesTaxCodeRef.FullName.GetValue();
				}
			}
			if (Ret.ORSalesPurchase != null) {
				if (Ret.ORSalesPurchase.SalesOrPurchase != null) {
					if (Ret.ORSalesPurchase.SalesOrPurchase != null) {
						//Get value of Desc
						if (Ret.ORSalesPurchase.SalesOrPurchase.Desc != null) {
							string Desc13576 = (string)Ret.ORSalesPurchase.SalesOrPurchase.Desc.GetValue();
						}
						if (Ret.ORSalesPurchase.SalesOrPurchase.ORPrice != null) {
							if (Ret.ORSalesPurchase.SalesOrPurchase.ORPrice.Price != null) {
								//Get value of Price
								if (Ret.ORSalesPurchase.SalesOrPurchase.ORPrice.Price != null) {
									double Price13578 = (double)Ret.ORSalesPurchase.SalesOrPurchase.ORPrice.Price.GetValue();
								}
							}
							if (Ret.ORSalesPurchase.SalesOrPurchase.ORPrice.PricePercent != null) {
								//Get value of PricePercent
								if (Ret.ORSalesPurchase.SalesOrPurchase.ORPrice.PricePercent != null) {
									double PricePercent13579 = (double)Ret.ORSalesPurchase.SalesOrPurchase.ORPrice.PricePercent.GetValue();
								}
							}
						}
						if (Ret.ORSalesPurchase.SalesOrPurchase.AccountRef != null) {
							//Get value of ListID
							if (Ret.ORSalesPurchase.SalesOrPurchase.AccountRef.ListID != null) {
								string ListID13580 = (string)Ret.ORSalesPurchase.SalesOrPurchase.AccountRef.ListID.GetValue();
							}
							//Get value of FullName
							if (Ret.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName != null) {
								string FullName13581 = (string)Ret.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.GetValue();
							}
						}
					}
				}
				if (Ret.ORSalesPurchase.SalesAndPurchase != null) {
					if (Ret.ORSalesPurchase.SalesAndPurchase != null) {
						//Get value of SalesDesc
						if (Ret.ORSalesPurchase.SalesAndPurchase.SalesDesc != null) {
							string SalesDesc13582 = (string)Ret.ORSalesPurchase.SalesAndPurchase.SalesDesc.GetValue();
						}
						//Get value of SalesPrice
						if (Ret.ORSalesPurchase.SalesAndPurchase.SalesPrice != null) {
							double SalesPrice13583 = (double)Ret.ORSalesPurchase.SalesAndPurchase.SalesPrice.GetValue();
						}
						if (Ret.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef != null) {
							//Get value of ListID
							if (Ret.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef.ListID != null) {
								string ListID13584 = (string)Ret.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef.ListID.GetValue();
							}
							//Get value of FullName
							if (Ret.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef.FullName != null) {
								string FullName13585 = (string)Ret.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef.FullName.GetValue();
							}
						}
						//Get value of PurchaseDesc
						if (Ret.ORSalesPurchase.SalesAndPurchase.PurchaseDesc != null) {
							string PurchaseDesc13586 = (string)Ret.ORSalesPurchase.SalesAndPurchase.PurchaseDesc.GetValue();
						}
						//Get value of PurchaseCost
						if (Ret.ORSalesPurchase.SalesAndPurchase.PurchaseCost != null) {
							double PurchaseCost13587 = (double)Ret.ORSalesPurchase.SalesAndPurchase.PurchaseCost.GetValue();
						}
						if (Ret.ORSalesPurchase.SalesAndPurchase.PurchaseTaxCodeRef != null) {
							//Get value of ListID
							if (Ret.ORSalesPurchase.SalesAndPurchase.PurchaseTaxCodeRef.ListID != null) {
								string ListID13588 = (string)Ret.ORSalesPurchase.SalesAndPurchase.PurchaseTaxCodeRef.ListID.GetValue();
							}
							//Get value of FullName
							if (Ret.ORSalesPurchase.SalesAndPurchase.PurchaseTaxCodeRef.FullName != null) {
								string FullName13589 = (string)Ret.ORSalesPurchase.SalesAndPurchase.PurchaseTaxCodeRef.FullName.GetValue();
							}
						}
						if (Ret.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef != null) {
							//Get value of ListID
							if (Ret.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef.ListID != null) {
								string ListID13590 = (string)Ret.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef.ListID.GetValue();
							}
							//Get value of FullName
							if (Ret.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef.FullName != null) {
								string FullName13591 = (string)Ret.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef.FullName.GetValue();
							}
						}
						if (Ret.ORSalesPurchase.SalesAndPurchase.PrefVendorRef != null) {
							//Get value of ListID
							if (Ret.ORSalesPurchase.SalesAndPurchase.PrefVendorRef.ListID != null) {
								string ListID13592 = (string)Ret.ORSalesPurchase.SalesAndPurchase.PrefVendorRef.ListID.GetValue();
							}
							//Get value of FullName
							if (Ret.ORSalesPurchase.SalesAndPurchase.PrefVendorRef.FullName != null) {
								string FullName13593 = (string)Ret.ORSalesPurchase.SalesAndPurchase.PrefVendorRef.FullName.GetValue();
							}
						}
					}
				}
			}
			//Get value of ExternalGUID
			if (Ret.ExternalGUID != null) {
				string ExternalGUID13594 = (string)Ret.ExternalGUID.GetValue();
			}
			if (Ret.DataExtRetList != null) {
				for (int i13595 = 0; i13595 < Ret.DataExtRetList.Count; i13595++) {
					IDataExtRet DataExtRet = Ret.DataExtRetList.GetAt(i13595);
					//Get value of OwnerID
					if (DataExtRet.OwnerID != null) {
						string OwnerID13596 = (string)DataExtRet.OwnerID.GetValue();
					}
					//Get value of DataExtName
					string DataExtName13597 = (string)DataExtRet.DataExtName.GetValue();
					//Get value of DataExtType
					ENDataExtType DataExtType13598 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
					//Get value of DataExtValue
					string DataExtValue13599 = (string)DataExtRet.DataExtValue.GetValue();
				}
			}
		}
	}
}