using System;
using System.Net;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;
using Interop.QBFC13;
using Interop.QBXMLRP2Lib;

namespace Yutaka.QuickBooks
{
	public class Qbfc13Util
	{
		#region Fields
		const string TIMESTAMP = @"HH:mm:ss.fff";
		private LogLevel _logLevel;
		private QBSessionManager _sessionManager;
		private bool _connectionOpen;
		private bool _sessionBegun;
		private string _appName;
		/// <summary>
		/// Trace - very detailed logs, which may include high-volume information such as protocol payloads. This log level is typically only enabled during development. Ex: begin method X, end method X
		/// Debug - debugging information, less detailed than trace, typically not enabled in production environment. Ex: executed query, user authenticated, session expired
		/// Info - information messages, which are normally enabled in production environment. Ex: mail sent, user updated profile etc
		/// Warn - warning messages, typically for non-critical issues, which can be recovered or which are temporary failures. Ex: application will continue
		/// Error - error messages - most of the time these are Exceptions. Ex: application may or may not continue
		/// Fatal - very serious errors! Ex: application is going down
		/// Off - disables logging when used as the minimum log level.
		/// </summary>
		#region public enum LogLevel {
		public enum LogLevel {
			Trace = 0,
			Debug = 1,
			Info = 2,
			Warn = 3,
			Error = 4,
			Fatal = 5,
			Off = 6,
		};
		#endregion public enum LogLevel
		#endregion Fields

		public Qbfc13Util(string appName, LogLevel loglevel = LogLevel.Info)
		{
			if (String.IsNullOrWhiteSpace(appName))
				appName = "Yutaka.Qbfc13Util";

			_logLevel = loglevel;
			_sessionManager = null;
			_connectionOpen = false;
			_sessionBegun = false;
			_appName = appName;
		}

		//public void DoItemNonInventoryQuery()
		//{
		//	if (_logLevel < LogLevel.Debug)
		//		Console.Write("\n[{0}] Begin method DoItemNonInventoryQuery().", DateTime.Now.ToString(TIMESTAMP));

		//	try {
		//		//Create the session Manager object
		//		_sessionManager = new QBSessionManager();

		//		//Create the message set request object to hold our request
		//		var requestMsgSet = _sessionManager.CreateMsgSetRequest("US",13,0);
		//		requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

		//		BuildItemNonInventoryQueryRq(requestMsgSet);

		//		//Connect to QuickBooks and begin a session
		//		_sessionManager.OpenConnection(_appName, _appName);
		//		_connectionOpen = true;
		//		_sessionManager.BeginSession("", ENOpenMode.omDontCare);
		//		_sessionBegun = true;

		//		//Send the request and get the response from QuickBooks
		//		IMsgSetResponse responseMsgSet = _sessionManager.DoRequests(requestMsgSet);

		//		//End the session and close the connection to QuickBooks
		//		_sessionManager.EndSession();
		//		_sessionBegun = false;
		//		_sessionManager.CloseConnection();
		//		_connectionOpen = false;

		//		WalkItemNonInventoryQueryRs(responseMsgSet);
		//	}

		//	catch (Exception ex) {
		//		if (_logLevel < LogLevel.Fatal) {
		//			string msg;

		//			if (ex.InnerException == null)
		//				msg = String.Format("{0}{2}Exception thrown in Qbfc13UtilDoItemNonInventoryQuery().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
		//			else
		//				msg = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Qbfc13UtilDoItemNonInventoryQuery().{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);

		//			Console.Write("\n[{0}] {1}", DateTime.Now.ToString(TIMESTAMP), msg);
		//		}

		//		if (_sessionBegun)
		//			_sessionManager.EndSession();
		//		if (_connectionOpen)
		//			_sessionManager.CloseConnection();
		//	}

		//	if (_logLevel < LogLevel.Debug)
		//		Console.Write("\n[{0}] End method DoItemNonInventoryQuery().", DateTime.Now.ToString(TIMESTAMP));
		//}

		//void BuildItemNonInventoryQueryRq(IMsgSetRequest requestMsgSet)
		//{
		//	if (_logLevel < LogLevel.Debug)
		//		Console.Write("\n[{0}] Begin method BuildItemNonInventoryQueryRq(IMsgSetRequest requestMsgSet).", DateTime.Now.ToString(TIMESTAMP));

		//	IItemNonInventoryQuery ItemNonInventoryQueryRq= requestMsgSet.AppendItemNonInventoryQueryRq();
		//	//Set attributes
		//	//Set field value for metaData
		//	ItemNonInventoryQueryRq.metaData.SetValue("IQBENmetaDataType");
		//	//Set field value for iterator
		//	ItemNonInventoryQueryRq.iterator.SetValue("IQBENiteratorType");
		//	//Set field value for iteratorID
		//	ItemNonInventoryQueryRq.iteratorID.SetValue("IQBUUIDType");
		//	string ORListQueryWithOwnerIDAndClassElementType13553 = "ListIDList";
		//	if (ORListQueryWithOwnerIDAndClassElementType13553 == "ListIDList") {
		//		//Set field value for ListIDList
		//		//May create more than one of these if needed
		//		ItemNonInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListIDList.Add("200000-1011023419");
		//	}
		//	if (ORListQueryWithOwnerIDAndClassElementType13553 == "FullNameList") {
		//		//Set field value for FullNameList
		//		//May create more than one of these if needed
		//		ItemNonInventoryQueryRq.ORListQueryWithOwnerIDAndClass.FullNameList.Add("ab");
		//	}
		//	if (ORListQueryWithOwnerIDAndClassElementType13553 == "ListWithClassFilter") {
		//		//Set field value for MaxReturned
		//		ItemNonInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.MaxReturned.SetValue(6);
		//		//Set field value for ActiveStatus
		//		ItemNonInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ActiveStatus.SetValue(ENActiveStatus.asActiveOnly[DEFAULT]);
		//		//Set field value for FromModifiedDate
		//		ItemNonInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.FromModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
		//		//Set field value for ToModifiedDate
		//		ItemNonInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ToModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
		//		string ORNameFilterElementType13554 = "NameFilter";
		//		if (ORNameFilterElementType13554 == "NameFilter") {
		//			//Set field value for MatchCriterion
		//			ItemNonInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameFilter.MatchCriterion.SetValue(ENMatchCriterion.mcStartsWith);
		//			//Set field value for Name
		//			ItemNonInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameFilter.Name.SetValue("ab");
		//		}
		//		if (ORNameFilterElementType13554 == "NameRangeFilter") {
		//			//Set field value for FromName
		//			ItemNonInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameRangeFilter.FromName.SetValue("ab");
		//			//Set field value for ToName
		//			ItemNonInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ORNameFilter.NameRangeFilter.ToName.SetValue("ab");
		//		}
		//		string ORClassFilterElementType13555 = "ListIDList";
		//		if (ORClassFilterElementType13555 == "ListIDList") {
		//			//Set field value for ListIDList
		//			//May create more than one of these if needed
		//			ItemNonInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ClassFilter.ORClassFilter.ListIDList.Add("200000-1011023419");
		//		}
		//		if (ORClassFilterElementType13555 == "FullNameList") {
		//			//Set field value for FullNameList
		//			//May create more than one of these if needed
		//			ItemNonInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ClassFilter.ORClassFilter.FullNameList.Add("ab");
		//		}
		//		if (ORClassFilterElementType13555 == "ListIDWithChildren") {
		//			//Set field value for ListIDWithChildren
		//			ItemNonInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ClassFilter.ORClassFilter.ListIDWithChildren.SetValue("200000-1011023419");
		//		}
		//		if (ORClassFilterElementType13555 == "FullNameWithChildren") {
		//			//Set field value for FullNameWithChildren
		//			ItemNonInventoryQueryRq.ORListQueryWithOwnerIDAndClass.ListWithClassFilter.ClassFilter.ORClassFilter.FullNameWithChildren.SetValue("ab");
		//		}
		//	}
		//	//Set field value for IncludeRetElementList
		//	//May create more than one of these if needed
		//	ItemNonInventoryQueryRq.IncludeRetElementList.Add("ab");
		//	//Set field value for OwnerIDList
		//	//May create more than one of these if needed
		//	ItemNonInventoryQueryRq.OwnerIDList.Add(Guid.NewGuid().ToString());

		//	if (_logLevel < LogLevel.Debug)
		//		Console.Write("\n[{0}] End method BuildItemNonInventoryQueryRq(IMsgSetRequest requestMsgSet).", DateTime.Now.ToString(TIMESTAMP));
		//}

		//void WalkItemNonInventoryQueryRs(IMsgSetResponse responseMsgSet)
		//{
		//	if (_logLevel < LogLevel.Debug)
		//		Console.Write("\n[{0}] Begin method WalkItemNonInventoryQueryRs(IMsgSetResponse responseMsgSet).", DateTime.Now.ToString(TIMESTAMP));

		//	if (responseMsgSet == null) return;
		//	IResponseList responseList = responseMsgSet.ResponseList;
		//	if (responseList == null) return;
		//	//if we sent only one request, there is only one response, we'll walk the list for this sample
		//	for (int i = 0; i < responseList.Count; i++) {
		//		IResponse response = responseList.GetAt(i);
		//		//check the status code of the response, 0=ok, >0 is warning
		//		if (response.StatusCode >= 0) {
		//			//the request-specific response is in the details, make sure we have some
		//			if (response.Detail != null) {
		//				//make sure the response is the type we're expecting
		//				ENResponseType responseType = (ENResponseType)response.Type.GetValue();
		//				if (responseType == ENResponseType.rtItemNonInventoryQueryRs) {
		//					//upcast to more specific type here, this is safe because we checked with response.Type check above
		//					IItemNonInventoryRetList ItemNonInventoryRet = (IItemNonInventoryRetList)response.Detail;
		//					WalkItemNonInventoryRet(ItemNonInventoryRet);
		//				}
		//			}
		//		}
		//	}

		//	if (_logLevel < LogLevel.Debug)
		//		Console.Write("\n[{0}] End method WalkItemNonInventoryQueryRs(IMsgSetResponse responseMsgSet).", DateTime.Now.ToString(TIMESTAMP));
		//}

		//void WalkItemNonInventoryRet(IItemNonInventoryRetList ItemNonInventoryRet)
		//{
		//	if (_logLevel < LogLevel.Debug)
		//		Console.Write("\n[{0}] Begin method WalkItemNonInventoryRet(IItemNonInventoryRetList ItemNonInventoryRet).", DateTime.Now.ToString(TIMESTAMP));

		//	if (ItemNonInventoryRet == null) return;
		//	//Go through all the elements of IItemNonInventoryRetList
		//	//Get value of ListID
		//	string ListID13556 = (string)ItemNonInventoryRet.ListID.GetValue();
		//	//Get value of TimeCreated
		//	DateTime TimeCreated13557 = (DateTime)ItemNonInventoryRet.TimeCreated.GetValue();
		//	//Get value of TimeModified
		//	DateTime TimeModified13558 = (DateTime)ItemNonInventoryRet.TimeModified.GetValue();
		//	//Get value of EditSequence
		//	string EditSequence13559 = (string)ItemNonInventoryRet.EditSequence.GetValue();
		//	//Get value of Name
		//	string Name13560 = (string)ItemNonInventoryRet.Name.GetValue();
		//	//Get value of FullName
		//	string FullName13561 = (string)ItemNonInventoryRet.FullName.GetValue();
		//	//Get value of BarCodeValue
		//	if (ItemNonInventoryRet.BarCodeValue != null) {
		//		string BarCodeValue13562 = (string)ItemNonInventoryRet.BarCodeValue.GetValue();
		//	}
		//	//Get value of IsActive
		//	if (ItemNonInventoryRet.IsActive != null) {
		//		bool IsActive13563 = (bool)ItemNonInventoryRet.IsActive.GetValue();
		//	}
		//	if (ItemNonInventoryRet.ClassRef != null) {
		//		//Get value of ListID
		//		if (ItemNonInventoryRet.ClassRef.ListID != null) {
		//			string ListID13564 = (string)ItemNonInventoryRet.ClassRef.ListID.GetValue();
		//		}
		//		//Get value of FullName
		//		if (ItemNonInventoryRet.ClassRef.FullName != null) {
		//			string FullName13565 = (string)ItemNonInventoryRet.ClassRef.FullName.GetValue();
		//		}
		//	}
		//	if (ItemNonInventoryRet.ParentRef != null) {
		//		//Get value of ListID
		//		if (ItemNonInventoryRet.ParentRef.ListID != null) {
		//			string ListID13566 = (string)ItemNonInventoryRet.ParentRef.ListID.GetValue();
		//		}
		//		//Get value of FullName
		//		if (ItemNonInventoryRet.ParentRef.FullName != null) {
		//			string FullName13567 = (string)ItemNonInventoryRet.ParentRef.FullName.GetValue();
		//		}
		//	}
		//	//Get value of Sublevel
		//	int Sublevel13568 = (int)ItemNonInventoryRet.Sublevel.GetValue();
		//	//Get value of ManufacturerPartNumber
		//	if (ItemNonInventoryRet.ManufacturerPartNumber != null) {
		//		string ManufacturerPartNumber13569 = (string)ItemNonInventoryRet.ManufacturerPartNumber.GetValue();
		//	}
		//	if (ItemNonInventoryRet.UnitOfMeasureSetRef != null) {
		//		//Get value of ListID
		//		if (ItemNonInventoryRet.UnitOfMeasureSetRef.ListID != null) {
		//			string ListID13570 = (string)ItemNonInventoryRet.UnitOfMeasureSetRef.ListID.GetValue();
		//		}
		//		//Get value of FullName
		//		if (ItemNonInventoryRet.UnitOfMeasureSetRef.FullName != null) {
		//			string FullName13571 = (string)ItemNonInventoryRet.UnitOfMeasureSetRef.FullName.GetValue();
		//		}
		//	}
		//	//Get value of IsTaxIncluded
		//	if (ItemNonInventoryRet.IsTaxIncluded != null) {
		//		bool IsTaxIncluded13572 = (bool)ItemNonInventoryRet.IsTaxIncluded.GetValue();
		//	}
		//	if (ItemNonInventoryRet.SalesTaxCodeRef != null) {
		//		//Get value of ListID
		//		if (ItemNonInventoryRet.SalesTaxCodeRef.ListID != null) {
		//			string ListID13573 = (string)ItemNonInventoryRet.SalesTaxCodeRef.ListID.GetValue();
		//		}
		//		//Get value of FullName
		//		if (ItemNonInventoryRet.SalesTaxCodeRef.FullName != null) {
		//			string FullName13574 = (string)ItemNonInventoryRet.SalesTaxCodeRef.FullName.GetValue();
		//		}
		//	}
		//	if (ItemNonInventoryRet.ORSalesPurchase != null) {
		//		if (ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase != null) {
		//			if (ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase != null) {
		//				//Get value of Desc
		//				if (ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.Desc != null) {
		//					string Desc13576 = (string)ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.Desc.GetValue();
		//				}
		//				if (ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.ORPrice != null) {
		//					if (ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.ORPrice.Price != null) {
		//						//Get value of Price
		//						if (ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.ORPrice.Price != null) {
		//							double Price13578 = (double)ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.ORPrice.Price.GetValue();
		//						}
		//					}
		//					if (ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.ORPrice.PricePercent != null) {
		//						//Get value of PricePercent
		//						if (ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.ORPrice.PricePercent != null) {
		//							double PricePercent13579 = (double)ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.ORPrice.PricePercent.GetValue();
		//						}
		//					}
		//				}
		//				if (ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.AccountRef != null) {
		//					//Get value of ListID
		//					if (ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.AccountRef.ListID != null) {
		//						string ListID13580 = (string)ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.AccountRef.ListID.GetValue();
		//					}
		//					//Get value of FullName
		//					if (ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName != null) {
		//						string FullName13581 = (string)ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.GetValue();
		//					}
		//				}
		//			}
		//		}
		//		if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase != null) {
		//			if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase != null) {
		//				//Get value of SalesDesc
		//				if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.SalesDesc != null) {
		//					string SalesDesc13582 = (string)ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.SalesDesc.GetValue();
		//				}
		//				//Get value of SalesPrice
		//				if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.SalesPrice != null) {
		//					double SalesPrice13583 = (double)ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.SalesPrice.GetValue();
		//				}
		//				if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef != null) {
		//					//Get value of ListID
		//					if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef.ListID != null) {
		//						string ListID13584 = (string)ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef.ListID.GetValue();
		//					}
		//					//Get value of FullName
		//					if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef.FullName != null) {
		//						string FullName13585 = (string)ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef.FullName.GetValue();
		//					}
		//				}
		//				//Get value of PurchaseDesc
		//				if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.PurchaseDesc != null) {
		//					string PurchaseDesc13586 = (string)ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.PurchaseDesc.GetValue();
		//				}
		//				//Get value of PurchaseCost
		//				if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.PurchaseCost != null) {
		//					double PurchaseCost13587 = (double)ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.PurchaseCost.GetValue();
		//				}
		//				if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.PurchaseTaxCodeRef != null) {
		//					//Get value of ListID
		//					if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.PurchaseTaxCodeRef.ListID != null) {
		//						string ListID13588 = (string)ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.PurchaseTaxCodeRef.ListID.GetValue();
		//					}
		//					//Get value of FullName
		//					if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.PurchaseTaxCodeRef.FullName != null) {
		//						string FullName13589 = (string)ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.PurchaseTaxCodeRef.FullName.GetValue();
		//					}
		//				}
		//				if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef != null) {
		//					//Get value of ListID
		//					if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef.ListID != null) {
		//						string ListID13590 = (string)ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef.ListID.GetValue();
		//					}
		//					//Get value of FullName
		//					if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef.FullName != null) {
		//						string FullName13591 = (string)ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef.FullName.GetValue();
		//					}
		//				}
		//				if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.PrefVendorRef != null) {
		//					//Get value of ListID
		//					if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.PrefVendorRef.ListID != null) {
		//						string ListID13592 = (string)ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.PrefVendorRef.ListID.GetValue();
		//					}
		//					//Get value of FullName
		//					if (ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.PrefVendorRef.FullName != null) {
		//						string FullName13593 = (string)ItemNonInventoryRet.ORSalesPurchase.SalesAndPurchase.PrefVendorRef.FullName.GetValue();
		//					}
		//				}
		//			}
		//		}
		//	}
		//	//Get value of ExternalGUID
		//	if (ItemNonInventoryRet.ExternalGUID != null) {
		//		string ExternalGUID13594 = (string)ItemNonInventoryRet.ExternalGUID.GetValue();
		//	}
		//	if (ItemNonInventoryRet.DataExtRetList != null) {
		//		for (int i13595 = 0; i13595 < ItemNonInventoryRet.DataExtRetList.Count; i13595++) {
		//			IDataExtRet DataExtRet = ItemNonInventoryRet.DataExtRetList.GetAt(i13595);
		//			//Get value of OwnerID
		//			if (DataExtRet.OwnerID != null) {
		//				string OwnerID13596 = (string)DataExtRet.OwnerID.GetValue();
		//			}
		//			//Get value of DataExtName
		//			string DataExtName13597 = (string)DataExtRet.DataExtName.GetValue();
		//			//Get value of DataExtType
		//			ENDataExtType DataExtType13598 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
		//			//Get value of DataExtValue
		//			string DataExtValue13599 = (string)DataExtRet.DataExtValue.GetValue();
		//		}
		//	}

		//	if (_logLevel < LogLevel.Debug)
		//		Console.Write("\n[{0}] End method WalkItemNonInventoryRet(IItemNonInventoryRetList ItemNonInventoryRet).", DateTime.Now.ToString(TIMESTAMP));
		//}
	}
}