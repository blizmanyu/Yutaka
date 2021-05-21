using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Interop.QBXMLRP2Lib;
using Yutaka.QuickBooks.Data;

namespace Yutaka.QuickBooks
{
	/// <summary>
	/// To support a new Action/Query, simply perform these 5 steps:
	/// 1) Add the Action to the Enum ActionType.
	/// 2) Look at the Response portion of the XMLOps from the API Docs to create a C# class that corresponds to the Ret object.
	/// 3) In the method BuildRequest(), add a case to build the request XML for your action. Make sure you read/handle the params object that comes
	///	   in for you specific action.
	///	4) If your action is a Query, add a case in the ProcessReturn() method to convert the response XML to a list of ret class you created in number (2).
	///	   Also do this if your action isn't a query, but you still want to see the returned object.
	///	5) Create a public method that takes in logically-named parameters that converts them to the more-general params object to pass into DoAction().
	///	   If you want your public method to return a list, "upcast" it to a list of your Ret objects instead of generic objects.
	/// </summary>
	public class QB20191021Util
	{
		#region Fields
		protected const string DEFAULT_APP_NAME = "QB20191021Util";
		protected const string QB_FORMAT = "yyyy-MM-ddTHH:mm:ssK";
		protected readonly DateTime MIN_DATE = DateTime.Now.AddYears(-10);
		protected readonly DateTime MAX_DATE = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59, 999, DateTimeKind.Local);
		protected enum ActionType { ARRefundCreditCardQuery, BillQuery, BillPaymentCheckQuery, ChargeQuery, InventoryAdjustmentAdd, InventoryAdjustmentQuery, };
		protected RequestProcessor2 Rp;
		protected bool ConnectionOpen;
		protected bool SessionBegun;
		protected string SessionId;

		public bool Debug;
		#endregion Fields

		public QB20191021Util()
		{
			Debug = false;
			Rp = null;
			ConnectionOpen = false;
			SessionBegun = false;
			SessionId = null;
		}

		#region Utilities
		protected string BeautifyXml(string xml)
		{
			if (String.IsNullOrWhiteSpace(xml))
				return "";

			using (var sw = new StringWriter()) {
				var xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(xml);
				xmlDoc.Save(sw);

				return sw.ToString();
			}
		}

		protected void BuildRequest(XmlDocument doc, XmlElement parent, ActionType actionType, params KeyValuePair<string, object>[] parameters)
		{
			var request = doc.CreateElement(String.Format("{0}Rq", actionType.ToString()));
			parent.AppendChild(request);
			XmlElement AccountRef, ItemRef, InventoryAdjustmentAdd, InventoryAdjustmentLineAdd, QuantityAdjustment, ModifiedDateRangeFilter;

			#region Go through parameters
			var accountRefListId = "";
			var dtFrom = "";
			var dtTo = "";
			var itemRefFullName = "";
			var quantityDifference = 0;

			foreach (var param in parameters) {
				switch (param.Key) {
					case "accountRefListId":
						accountRefListId = param.Value.ToString();
						break;
					case "dtFrom":
						dtFrom = param.Value.ToString();
						break;
					case "dtTo":
						dtTo = param.Value.ToString();
						break;
					case "itemRefFullName":
						itemRefFullName = param.Value.ToString();
						break;
					case "quantityDifference":
						quantityDifference = (int) param.Value;
						break;
					default:
						break;
				}
			}
			#endregion Go through parameters

			switch (actionType) {
				#region ARRefundCreditCardQuery
				case ActionType.ARRefundCreditCardQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion

				#region BillQuery
				case ActionType.BillQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion

				#region BillPaymentCheckQuery
				case ActionType.BillPaymentCheckQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion

				#region ChargeQuery
				case ActionType.ChargeQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					break;
				#endregion

				#region InventoryAdjustmentAdd
				case ActionType.InventoryAdjustmentAdd:
					InventoryAdjustmentAdd = doc.CreateElement("InventoryAdjustmentAdd");
					request.AppendChild(InventoryAdjustmentAdd);
					AccountRef = doc.CreateElement("AccountRef");
					InventoryAdjustmentAdd.AppendChild(AccountRef);
					AccountRef.AppendChild(MakeSimpleElem(doc, "ListID", accountRefListId));
					InventoryAdjustmentLineAdd = doc.CreateElement("InventoryAdjustmentLineAdd");
					InventoryAdjustmentAdd.AppendChild(InventoryAdjustmentLineAdd);
					ItemRef = doc.CreateElement("ItemRef");
					InventoryAdjustmentLineAdd.AppendChild(ItemRef);
					ItemRef.AppendChild(MakeSimpleElem(doc, "FullName", itemRefFullName));
					QuantityAdjustment = doc.CreateElement("QuantityAdjustment");
					InventoryAdjustmentLineAdd.AppendChild(QuantityAdjustment);
					QuantityAdjustment.AppendChild(MakeSimpleElem(doc, "QuantityDifference", quantityDifference.ToString()));
					break;
				#endregion InventoryAdjustmentAdd

				#region InventoryAdjustmentQuery
				case ActionType.InventoryAdjustmentQuery:
					ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", dtFrom));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", dtTo));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion InventoryAdjustmentQuery

					default:
					break;
			}
		}

		// This is the main method that actually sends the request to QuickBooks. Most of the other methods are called from within this method.
		protected List<object> DoAction(ActionType actionType, params KeyValuePair<string, object>[] parameters)
		{
			if (actionType < 0)
				throw new Exception(String.Format("<actionType> is required.{0}Exception thrown in QB20191021Util.DoAction(ActionType actionType, DateTime? startTime, DateTime? endTime).{0}", Environment.NewLine));

			try {
				if (!ConnectionOpen || !SessionBegun || String.IsNullOrWhiteSpace(SessionId))
					OpenConnection();

				//Create the message set request object to hold our request
				var requestXmlDoc = new XmlDocument();

				//Add the prolog processing instructions
				requestXmlDoc.AppendChild(requestXmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null));
				requestXmlDoc.AppendChild(requestXmlDoc.CreateProcessingInstruction("qbxml", "version=\"13.0\""));

				//Create the outer request envelope tag
				var outer = requestXmlDoc.CreateElement("QBXML");
				requestXmlDoc.AppendChild(outer);

				//Create the inner request envelope & any needed attributes
				var inner = requestXmlDoc.CreateElement("QBXMLMsgsRq");
				outer.AppendChild(inner);
				inner.SetAttribute("onError", "stopOnError");
				BuildRequest(requestXmlDoc, inner, actionType, parameters);

				if (Debug)
					File.WriteAllText(String.Format(@"C:\TEMP\{0}Request.xml", actionType.ToString()), BeautifyXml(requestXmlDoc.OuterXml));

				//Send the request and get the response from QuickBooks
				var responseStr = Rp.ProcessRequest(SessionId, requestXmlDoc.OuterXml);

				if (Debug)
					File.WriteAllText(String.Format(@"C:\TEMP\{0}Response.xml", actionType.ToString()), BeautifyXml(responseStr));

				if (actionType.ToString().EndsWith("Query"))
					return ProcessResponse(actionType, responseStr);

				return new List<object>();
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in QBV20191021Util.DoAction(ActionType actionType='{3}', params KeyValuePair<string, object>[] parameters='{4}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, actionType.ToString(), String.Join(", ", parameters));
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of QBV20191021Util.DoAction(ActionType actionType='{3}', params KeyValuePair<string, object>[] parameters='{4}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, actionType.ToString(), String.Join(", ", parameters));

				if (Debug)
					Console.Write("\n{0}", log);
				#endregion Log

				return new List<object>();
			}
		}

		protected XmlElement MakeSimpleElem(XmlDocument doc, string tagName, string tagVal)
		{
			var elem = doc.CreateElement(tagName);
			elem.InnerText = tagVal;
			return elem;
		}

		protected List<object> ProcessResponse(ActionType actionType, string response)
		{
			#region Input Validation
			if (String.IsNullOrWhiteSpace(response))
				return null;
			if (actionType < 0)
				throw new Exception(String.Format("<actionType> is required.{0}Exception thrown in QB20191021Util.ProcessResponse(ActionType actionType, string response).{0}", Environment.NewLine));
			#endregion Input Validation

			var responseXmlDoc = new XmlDocument();
			responseXmlDoc.LoadXml(response);
			var QueryRsList = responseXmlDoc.GetElementsByTagName(String.Format("{0}Rs", actionType.ToString()));

			if (QueryRsList.Count == 1) {
				var responseNode = QueryRsList.Item(0);
				// Check the status code, info, and severity
				var rsAttributes = responseNode.Attributes;
				var statusCode = rsAttributes.GetNamedItem("statusCode").Value;
				var statusSeverity = rsAttributes.GetNamedItem("statusSeverity").Value;
				var statusMessage = rsAttributes.GetNamedItem("statusMessage").Value;

				if (Debug)
					Console.Write("\n{0} {1}", statusCode, statusMessage);

				// status code = 0 all OK, > 0 is warning
				if (Convert.ToInt32(statusCode) > -1)
					return ProcessReturn(actionType, responseNode);
			}

			return new List<object>();
		}

		protected List<object> ProcessReturn(ActionType actionType, XmlNode responseNode)
		{
			if (actionType < 0)
				throw new Exception(String.Format("<actionType> is required.{0}Exception thrown in QB20191021Util.ProcessReturn(ActionType actionType).{0}", Environment.NewLine));
			if (responseNode == null || String.IsNullOrWhiteSpace(responseNode.ToString()))
				return null;

			XmlNode Item, ItemRef, LineItem;
			XmlNodeList ItemList, LineItemList;
			var list = new List<object>();

			switch (actionType) {

				#region BillQuery
				case ActionType.BillQuery:
					BillRet billRet;
					Bill_ExpenseLineRet billLineRet;
					ItemList = responseNode.SelectNodes("//BillRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						billRet = new BillRet {
							TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
							TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
							DueDate = DateTime.Parse(Item.SelectSingleNode("DueDate").InnerText),
							AmountDue = Item.SelectSingleNode("AmountDue") == null ? 0 : decimal.Parse(Item.SelectSingleNode("AmountDue").InnerText),
							ExchangeRate = Item.SelectSingleNode("ExchangeRate") == null ? 0 : decimal.Parse(Item.SelectSingleNode("ExchangeRate").InnerText),
							AmountDueInHomeCurrency = Item.SelectSingleNode("AmountDueInHomeCurrency") == null ? 0 : decimal.Parse(Item.SelectSingleNode("AmountDueInHomeCurrency ").InnerText),
							RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
							Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
							IsTaxIncluded = Item.SelectSingleNode("IsTaxIncluded") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsTaxIncluded").InnerText),
							IsPaid = Item.SelectSingleNode("IsPaid") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsPaid").InnerText),
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
							OpenAmount = Item.SelectSingleNode("OpenAmount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("OpenAmount").InnerText),
						};

						#region VendorRef
						if (Item.SelectSingleNode("VendorRef") == null) {
							billRet.VendorRef.ListID = null;
							billRet.VendorRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("VendorRef");
							billRet.VendorRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billRet.VendorRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion VendorRef

						#region VendorAddress
						if (Item.SelectSingleNode("VendorAddress") == null) {
							billRet.VendorAddress.Addr1 = null;
							billRet.VendorAddress.Addr2 = null;
							billRet.VendorAddress.Addr3 = null;
							billRet.VendorAddress.Addr4 = null;
							billRet.VendorAddress.Addr5 = null;
							billRet.VendorAddress.City = null;
							billRet.VendorAddress.State = null;
							billRet.VendorAddress.PostalCode = null;
							billRet.VendorAddress.Country = null;
							billRet.VendorAddress.Note = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("VendorAddress");
							billRet.VendorAddress.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							billRet.VendorAddress.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							billRet.VendorAddress.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							billRet.VendorAddress.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							billRet.VendorAddress.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
							billRet.VendorAddress.City = ItemRef.SelectSingleNode("City") == null ? null : ItemRef.SelectSingleNode("City").InnerText;
							billRet.VendorAddress.State = ItemRef.SelectSingleNode("State") == null ? null : ItemRef.SelectSingleNode("State").InnerText;
							billRet.VendorAddress.PostalCode = ItemRef.SelectSingleNode("PostalCode") == null ? null : ItemRef.SelectSingleNode("PostalCode").InnerText;
							billRet.VendorAddress.Country = ItemRef.SelectSingleNode("Country") == null ? null : ItemRef.SelectSingleNode("Country").InnerText;
							billRet.VendorAddress.Note = ItemRef.SelectSingleNode("Note") == null ? null : ItemRef.SelectSingleNode("Note").InnerText;
						}
						#endregion

						#region APAccountRef
						if (Item.SelectSingleNode("APAccountRef") == null) {
							billRet.APAccountRef.ListID = null;
							billRet.APAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("APAccountRef");
							billRet.APAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billRet.APAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CurrencyRef
						if (Item.SelectSingleNode("CurrencyRef") == null) {
							billRet.CurrencyRef.ListID = null;
							billRet.CurrencyRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("CurrencyRef");
							billRet.CurrencyRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billRet.CurrencyRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region TermsRef
						if (Item.SelectSingleNode("TermsRef") == null) {
							billRet.TermsRef.ListID = null;
							billRet.TermsRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("TermsRef");
							billRet.TermsRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billRet.TermsRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region SalesTaxCodeRef
						if (Item.SelectSingleNode("SalesTaxCodeRef") == null) {
							billRet.SalesTaxCodeRef.ListID = null;
							billRet.SalesTaxCodeRef.FullName = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("SalesTaxCodeRef");
							billRet.SalesTaxCodeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billRet.SalesTaxCodeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region LinkedTxn
						if (Item.SelectSingleNode("LinkedTxn") == null) {
							billRet.LinkedTxn.TxnID = null;
							billRet.LinkedTxn.TxnType = null;
							billRet.LinkedTxn.TxnDate = null;
							billRet.LinkedTxn.RefNumber = null;
							billRet.LinkedTxn.LinkType = null;
							billRet.LinkedTxn.Amount = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("LinkedTxn");
							billRet.LinkedTxn.TxnID = ItemRef.SelectSingleNode("TxnID") == null ? null : ItemRef.SelectSingleNode("TxnID").InnerText;
							billRet.LinkedTxn.TxnType = ItemRef.SelectSingleNode("TxnType") == null ? null : ItemRef.SelectSingleNode("TxnType").InnerText;
							billRet.LinkedTxn.TxnDate = ItemRef.SelectSingleNode("TxnDate") == null ? null : ItemRef.SelectSingleNode("TxnDate").InnerText;
							billRet.LinkedTxn.RefNumber = ItemRef.SelectSingleNode("RefNumber") == null ? null : ItemRef.SelectSingleNode("RefNumber").InnerText;
							billRet.LinkedTxn.LinkType = ItemRef.SelectSingleNode("LinkType") == null ? null : ItemRef.SelectSingleNode("LinkType").InnerText;
							billRet.LinkedTxn.Amount = ItemRef.SelectSingleNode("Amount") == null ? (decimal?) null : decimal.Parse(ItemRef.SelectSingleNode("Amount").InnerText);
						}
						#endregion

						#region DataExtRet
						if (Item.SelectSingleNode("DataExtRet") == null) {
							billRet.DataExtRet.OwnerID = null;
							billRet.DataExtRet.DataExtName = null;
							billRet.DataExtRet.DataExtType = null;
							billRet.DataExtRet.DataExtValue = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("DataExtRet");
							billRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
							billRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
							billRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
							billRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
						}
						#endregion

						#region LineItems
						LineItemList = Item.SelectNodes("Bill_ExperienceLineRet");

						for (int j = 0; j < LineItemList.Count; j++) {
							LineItem = LineItemList.Item(j);
							billLineRet = new Bill_ExpenseLineRet {
								TxnLineID = LineItem.SelectSingleNode("TxnLineID") == null ? null : LineItem.SelectSingleNode("TxnLineID").InnerText,
								Amount = LineItem.SelectSingleNode("Amount") == null ? (decimal?) null : decimal.Parse(LineItem.SelectSingleNode("Amount").InnerText),
								Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
								BillableStatus = LineItem.SelectSingleNode("BillableStatus") == null ? null : LineItem.SelectSingleNode("BillableStatus").InnerText,
							};

							#region AccountRef
							if (LineItem.SelectSingleNode("AccountRef") == null) {
								billLineRet.AccountRef.ListID = null;
								billLineRet.AccountRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("AccountRef");
								billLineRet.AccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								billLineRet.AccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion 

							#region AccountRef
							if (LineItem.SelectSingleNode("AccountRef") == null) {
								billLineRet.AccountRef.ListID = null;
								billLineRet.AccountRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("AccountRef");
								billLineRet.AccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								billLineRet.AccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion 

							#region CustomerRef
							if (Item.SelectSingleNode("CustomerRef") == null) {
								billLineRet.CustomerRef.ListID = null;
								billLineRet.CustomerRef.FullName = null;
							}

							else {
								ItemRef = Item.SelectSingleNode("CustomerRef");
								billLineRet.CustomerRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								billLineRet.CustomerRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region ClassRef
							if (Item.SelectSingleNode("ClassRef") == null) {
								billLineRet.ClassRef.ListID = null;
								billLineRet.ClassRef.FullName = null;
							}

							else {
								ItemRef = Item.SelectSingleNode("ClassRef");
								billLineRet.ClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								billLineRet.ClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region SalesTaxCodeRef
							if (Item.SelectSingleNode("SalesTaxCodeRef") == null) {
								billLineRet.SalesTaxCodeRef.ListID = null;
								billLineRet.SalesTaxCodeRef.FullName = null;
							}

							else {
								ItemRef = Item.SelectSingleNode("SalesTaxCodeRef");
								billLineRet.SalesTaxCodeRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								billLineRet.SalesTaxCodeRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region DataExtRet
							if (Item.SelectSingleNode("DataExtRet") == null) {
								billLineRet.DataExtRet.OwnerID = null;
								billLineRet.DataExtRet.DataExtName = null;
								billLineRet.DataExtRet.DataExtType = null;
								billLineRet.DataExtRet.DataExtValue = null;
							}
							else {
								ItemRef = Item.SelectSingleNode("DataExtRet");
								billLineRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
								billLineRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
								billLineRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
								billLineRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
							}
							#endregion

							billRet.ExpenseLines.Add(billLineRet);
						}
						#endregion LineItems

						list.Add(billRet);
					}
					break;

				#endregion

				#region ChargeQuery
				case ActionType.ChargeQuery:
					ChargeRet chargeRet;
					ItemList = responseNode.SelectNodes("//ChargeRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);

						chargeRet = new ChargeRet {
							TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
							RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
							Rate = Item.SelectSingleNode("Rate") == null ? (float?) null : float.Parse(Item.SelectSingleNode("Rate").InnerText),
							Amount = Item.SelectSingleNode("Amount") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("Amount").InnerText),
							BalanceRemaining = Item.SelectSingleNode("BalanceRemaining ") == null ? (decimal?) null : decimal.Parse(Item.SelectSingleNode("BalanceRemaining ").InnerText),
							Desc = Item.SelectSingleNode("Desc") == null ? null : Item.SelectSingleNode("Desc").InnerText,
							BilledDate = DateTime.Parse(Item.SelectSingleNode("BilledDate").InnerText),
							DueDate = DateTime.Parse(Item.SelectSingleNode("DueDate").InnerText),
							IsPaid = Item.SelectSingleNode("IsPaid") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsPaid").InnerText),
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
						};


						#region CustomerRef
						if (Item.SelectSingleNode("CustomerRef") == null) {
							chargeRet.CustomerRef.ListID = null;
							chargeRet.CustomerRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CustomerRef");
							chargeRet.CustomerRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							chargeRet.CustomerRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region ItemRef
						if (Item.SelectSingleNode("ItemRef") == null) {
							chargeRet.ItemRef.ListID = null;
							chargeRet.ItemRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("ItemRef");
							chargeRet.ItemRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							chargeRet.ItemRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region InventorySiteRef
						if (Item.SelectSingleNode("InventorySiteRef") == null) {
							chargeRet.InventorySiteRef.ListID = null;
							chargeRet.InventorySiteRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("InventorySiteRef");
							chargeRet.InventorySiteRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							chargeRet.InventorySiteRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region InventorySiteLocationRef
						if (Item.SelectSingleNode("InventorySiteLocationRef") == null) {
							chargeRet.InventorySiteLocationRef.ListID = null;
							chargeRet.InventorySiteLocationRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("InventorySiteLocationRef");
							chargeRet.InventorySiteLocationRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							chargeRet.InventorySiteLocationRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region OverrideUOMSetRef
						if (Item.SelectSingleNode("OverrideUOMSetRef") == null) {
							chargeRet.OverrideUOMSetRef.ListID = null;
							chargeRet.OverrideUOMSetRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("OverrideUOMSetRef");
							chargeRet.OverrideUOMSetRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							chargeRet.OverrideUOMSetRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region ARAccountRef
						if (Item.SelectSingleNode("ARAccountRef") == null) {
							chargeRet.ARAccountRef.ListID = null;
							chargeRet.ARAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("ARAccountRef");
							chargeRet.ARAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							chargeRet.ARAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region ClassRef
						if (Item.SelectSingleNode("ClassRef") == null) {
							chargeRet.ClassRef.ListID = null;
							chargeRet.ClassRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("ClassRef");
							chargeRet.ClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							chargeRet.ClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region LinkedTxn
						if (Item.SelectSingleNode("LinkedTxn") == null) {
							chargeRet.LinkedTxn.TxnID = null;
							chargeRet.LinkedTxn.TxnType = null;
							chargeRet.LinkedTxn.TxnDate = null;
							chargeRet.LinkedTxn.RefNumber = null;
							chargeRet.LinkedTxn.LinkType = null;
							chargeRet.LinkedTxn.Amount = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("LinkedTxn");
							chargeRet.LinkedTxn.TxnID = ItemRef.SelectSingleNode("TxnID") == null ? null : ItemRef.SelectSingleNode("TxnID").InnerText;
							chargeRet.LinkedTxn.TxnType = ItemRef.SelectSingleNode("TxnType") == null ? null : ItemRef.SelectSingleNode("TxnType").InnerText;
							chargeRet.LinkedTxn.TxnDate = ItemRef.SelectSingleNode("TxnDate") == null ? null : ItemRef.SelectSingleNode("TxnDate").InnerText;
							chargeRet.LinkedTxn.RefNumber = ItemRef.SelectSingleNode("RefNumber") == null ? null : ItemRef.SelectSingleNode("RefNumber").InnerText;
							chargeRet.LinkedTxn.LinkType = ItemRef.SelectSingleNode("LinkType") == null ? null : ItemRef.SelectSingleNode("LinkType").InnerText;
							chargeRet.LinkedTxn.Amount = ItemRef.SelectSingleNode("Amount") == null ? (decimal?) null : decimal.Parse(ItemRef.SelectSingleNode("Amount").InnerText);
						}
						#endregion

						#region DataExtRet
						if (Item.SelectSingleNode("DataExtRet") == null) {
							chargeRet.DataExtRet.OwnerID = null;
							chargeRet.DataExtRet.DataExtName = null;
							chargeRet.DataExtRet.DataExtType = null;
							chargeRet.DataExtRet.DataExtValue = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("DataExtRet");
							chargeRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
							chargeRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
							chargeRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
							chargeRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
						}
						#endregion
					}
					break;
				#endregion

				#region InventoryAdjustment
				case ActionType.InventoryAdjustmentAdd:
				case ActionType.InventoryAdjustmentQuery:
					InventoryAdjustmentRet ent;
					InventoryAdjustmentLineRet entLine;
					ItemList = responseNode.SelectNodes("//InventoryAdjustmentRet");

					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						ent = new InventoryAdjustmentRet {
							TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
							TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
							RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
							Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
						};

						#region AccountRef
						if (Item.SelectSingleNode("AccountRef") == null) {
							ent.AccountRef.ListID = null;
							ent.AccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("AccountRef");
							ent.AccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							ent.AccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion AccountRef

						#region InventorySiteRef
						if (Item.SelectSingleNode("InventorySiteRef") == null) {
							ent.InventorySiteRef.ListID = null;
							ent.InventorySiteRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("InventorySiteRef");
							ent.InventorySiteRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							ent.InventorySiteRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion InventorySiteRef

						#region CustomerRef
						if (Item.SelectSingleNode("CustomerRef") == null) {
							ent.CustomerRef.ListID = null;
							ent.CustomerRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CustomerRef");
							ent.CustomerRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							ent.CustomerRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion CustomerRef

						#region ClassRef
						if (Item.SelectSingleNode("ClassRef") == null) {
							ent.ClassRef.ListID = null;
							ent.ClassRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("ClassRef");
							ent.ClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							ent.ClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion ClassRef

						#region LineItems
						LineItemList = Item.SelectNodes("InventoryAdjustmentLineRet");

						for (int j = 0; j < LineItemList.Count; j++) {
							LineItem = LineItemList.Item(j);
							entLine = new InventoryAdjustmentLineRet {
								TxnLineID = LineItem.SelectSingleNode("TxnLineID") == null ? null : LineItem.SelectSingleNode("TxnLineID").InnerText,
								SerialNumber = LineItem.SelectSingleNode("SerialNumber") == null ? null : LineItem.SelectSingleNode("SerialNumber").InnerText,
								SerialNumberAddedOrRemoved = LineItem.SelectSingleNode("SerialNumberAddedOrRemoved") == null ? null : LineItem.SelectSingleNode("SerialNumberAddedOrRemoved").InnerText,
								LotNumber = LineItem.SelectSingleNode("LotNumber") == null ? null : LineItem.SelectSingleNode("LotNumber").InnerText,
								QuantityDifference = decimal.Parse(LineItem.SelectSingleNode("QuantityDifference").InnerText),
								ValueDifference = decimal.Parse(LineItem.SelectSingleNode("ValueDifference").InnerText),
							};

							#region ItemRef
							if (LineItem.SelectSingleNode("ItemRef") == null) {
								entLine.ItemRef.ListID = null;
								entLine.ItemRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("ItemRef");
								entLine.ItemRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								entLine.ItemRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion ItemRef

							#region InventorySiteLocationRef
							if (LineItem.SelectSingleNode("InventorySiteLocationRef") == null) {
								entLine.InventorySiteLocationRef.ListID = null;
								entLine.InventorySiteLocationRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("InventorySiteLocationRef");
								entLine.InventorySiteLocationRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								entLine.InventorySiteLocationRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion InventorySiteLocationRef

							ent.LineItems.Add(entLine);
						}
						#endregion LineItems

						list.Add(ent);
					}

					break;
				#endregion InventoryAdjustment

				#region BillPaymentCheckQuery

				case ActionType.BillPaymentCheckQuery:
					BillPaymentCheckRet billPaymentCheckRet;
					BillPaymentCheck_AppliedToTxnRet appliedToTxnRet;
					ItemList = responseNode.SelectNodes("//BillPaymentCheckRet");
					for (int i = 0; i < ItemList.Count; i++) {
						Item = ItemList.Item(i);
						billPaymentCheckRet = new BillPaymentCheckRet {
							TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
							TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
							TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
							EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
							TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
							TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
							Amount = Item.SelectSingleNode("Amount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("Amount ").InnerText),
							ExchangeRate = Item.SelectSingleNode("ExchangeRate") == null ? 0 : decimal.Parse(Item.SelectSingleNode("ExchangeRate ").InnerText),
							AmountDueInHomeCurrency = Item.SelectSingleNode("AmountDueInHomeCurrency") == null ? 0 : decimal.Parse(Item.SelectSingleNode("AmountDueInHomeCurrency").InnerText),
							RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
							Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
							IsToBePrinted = Item.SelectSingleNode("IsToBePrinted") == null ? (bool?) null : bool.Parse(Item.SelectSingleNode("IsToBePrinted").InnerText),
							ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
						};

						#region PayeeEntityRef
						if (Item.SelectSingleNode("PayeeEntityRef") == null) {
							billPaymentCheckRet.APAccountRef.ListID = null;
							billPaymentCheckRet.APAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("PayeeEntityRef");
							billPaymentCheckRet.APAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billPaymentCheckRet.APAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region APAccountRef
						if (Item.SelectSingleNode("APAccountRef") == null) {
							billPaymentCheckRet.APAccountRef.ListID = null;
							billPaymentCheckRet.APAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("APAccountRef");
							billPaymentCheckRet.APAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billPaymentCheckRet.APAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region BankAccountRef
						if (Item.SelectSingleNode("BankAccountRef") == null) {
							billPaymentCheckRet.APAccountRef.ListID = null;
							billPaymentCheckRet.APAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("BankAccountRef");
							billPaymentCheckRet.APAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billPaymentCheckRet.APAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region CurrencyRef
						if (Item.SelectSingleNode("CurrencyRef") == null) {
							billPaymentCheckRet.APAccountRef.ListID = null;
							billPaymentCheckRet.APAccountRef.FullName = null;
						}

						else {
							ItemRef = Item.SelectSingleNode("CurrencyRef");
							billPaymentCheckRet.APAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
							billPaymentCheckRet.APAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
						}
						#endregion

						#region Address
						if (Item.SelectSingleNode("Address") == null) {
							billPaymentCheckRet.Address.Addr1 = null;
							billPaymentCheckRet.Address.Addr2 = null;
							billPaymentCheckRet.Address.Addr3 = null;
							billPaymentCheckRet.Address.Addr4 = null;
							billPaymentCheckRet.Address.Addr5 = null;
							billPaymentCheckRet.Address.City = null;
							billPaymentCheckRet.Address.State = null;
							billPaymentCheckRet.Address.PostalCode = null;
							billPaymentCheckRet.Address.Country = null;
							billPaymentCheckRet.Address.Note = null;

						}
						else {
							ItemRef = Item.SelectSingleNode("Address");
							billPaymentCheckRet.Address.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							billPaymentCheckRet.Address.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							billPaymentCheckRet.Address.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							billPaymentCheckRet.Address.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							billPaymentCheckRet.Address.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
							billPaymentCheckRet.Address.City = ItemRef.SelectSingleNode("City") == null ? null : ItemRef.SelectSingleNode("City").InnerText;
							billPaymentCheckRet.Address.State = ItemRef.SelectSingleNode("State") == null ? null : ItemRef.SelectSingleNode("State").InnerText;
							billPaymentCheckRet.Address.PostalCode = ItemRef.SelectSingleNode("PostalCode") == null ? null : ItemRef.SelectSingleNode("PostalCode").InnerText;
							billPaymentCheckRet.Address.Country = ItemRef.SelectSingleNode("Country") == null ? null : ItemRef.SelectSingleNode("Country").InnerText;
							billPaymentCheckRet.Address.Note = ItemRef.SelectSingleNode("Note") == null ? null : ItemRef.SelectSingleNode("Note").InnerText;
						}
						#endregion

						#region AddressBlock
						if (Item.SelectSingleNode("AddressBlock") == null) {
							billPaymentCheckRet.AddressBlock.Addr1 = null;
							billPaymentCheckRet.AddressBlock.Addr2 = null;
							billPaymentCheckRet.AddressBlock.Addr3 = null;
							billPaymentCheckRet.AddressBlock.Addr4 = null;
							billPaymentCheckRet.AddressBlock.Addr5 = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("AddressBlock");
							billPaymentCheckRet.AddressBlock.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
							billPaymentCheckRet.AddressBlock.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
							billPaymentCheckRet.AddressBlock.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
							billPaymentCheckRet.AddressBlock.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
							billPaymentCheckRet.AddressBlock.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
						}
						#endregion

						#region DataExtRet
						if (Item.SelectSingleNode("DataExtRet") == null) {
							billPaymentCheckRet.DataExtRet.OwnerID = null;
							billPaymentCheckRet.DataExtRet.DataExtName = null;
							billPaymentCheckRet.DataExtRet.DataExtType = null;
							billPaymentCheckRet.DataExtRet.DataExtValue = null;
						}
						else {
							ItemRef = Item.SelectSingleNode("DataExtRet");
							billPaymentCheckRet.DataExtRet.OwnerID = ItemRef.SelectSingleNode("OwnerID") == null ? null : ItemRef.SelectSingleNode("OwnerID").InnerText;
							billPaymentCheckRet.DataExtRet.DataExtName = ItemRef.SelectSingleNode("DataExtName") == null ? null : ItemRef.SelectSingleNode("DataExtName").InnerText;
							billPaymentCheckRet.DataExtRet.DataExtType = ItemRef.SelectSingleNode("DataExtType") == null ? null : ItemRef.SelectSingleNode("DataExtType").InnerText;
							billPaymentCheckRet.DataExtRet.DataExtValue = ItemRef.SelectSingleNode("DataExtValue") == null ? null : ItemRef.SelectSingleNode("DataExtValue").InnerText;
						}
						#endregion

						#region LineItems
						LineItemList = Item.SelectNodes("AppliedToTxnRet");

						for (int j = 0; j < LineItemList.Count; j++) {
							LineItem = LineItemList.Item(j);
							appliedToTxnRet = new BillPaymentCheck_AppliedToTxnRet {
								TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
								TxnLineID = LineItem.SelectSingleNode("TxnLineID") == null ? null : LineItem.SelectSingleNode("TxnLineID").InnerText,
								TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
								RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
								BalanceRemaining = Item.SelectSingleNode("BalanceRemaining") == null ? 0 : decimal.Parse(Item.SelectSingleNode("BalanceRemaining").InnerText),
								Amount = Item.SelectSingleNode("Amount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("Amount").InnerText),
								DiscountAmount = Item.SelectSingleNode("DiscountAmount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("DiscountAmount").InnerText),
							};

							#region DiscountAccountRef
							if (LineItem.SelectSingleNode("DiscountAccountRef") == null) {
								appliedToTxnRet.DiscountAccountRef.ListID = null;
								appliedToTxnRet.DiscountAccountRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("DiscountAccountRef");
								appliedToTxnRet.DiscountAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								appliedToTxnRet.DiscountAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region DiscountClassRef
							if (LineItem.SelectSingleNode("DiscountClassRef") == null) {
								appliedToTxnRet.DiscountClassRef.ListID = null;
								appliedToTxnRet.DiscountClassRef.FullName = null;
							}

							else {
								ItemRef = LineItem.SelectSingleNode("DiscountClassRef");
								appliedToTxnRet.DiscountClassRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
								appliedToTxnRet.DiscountClassRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
							}
							#endregion

							#region LinkedTxn
							if (Item.SelectSingleNode("LinkedTxn") == null) {
								appliedToTxnRet.LinkedTxn.TxnID = null;
								appliedToTxnRet.LinkedTxn.TxnType = null;
								appliedToTxnRet.LinkedTxn.TxnDate = null;
								appliedToTxnRet.LinkedTxn.RefNumber = null;
								appliedToTxnRet.LinkedTxn.LinkType = null;
								appliedToTxnRet.LinkedTxn.Amount = null;

							}
							else {
								ItemRef = Item.SelectSingleNode("LinkedTxn");
								appliedToTxnRet.LinkedTxn.TxnID = ItemRef.SelectSingleNode("TxnID") == null ? null : ItemRef.SelectSingleNode("TxnID").InnerText;
								appliedToTxnRet.LinkedTxn.TxnType = ItemRef.SelectSingleNode("TxnType") == null ? null : ItemRef.SelectSingleNode("TxnType").InnerText;
								appliedToTxnRet.LinkedTxn.TxnDate = ItemRef.SelectSingleNode("TxnDate") == null ? null : ItemRef.SelectSingleNode("TxnDate").InnerText;
								appliedToTxnRet.LinkedTxn.RefNumber = ItemRef.SelectSingleNode("RefNumber") == null ? null : ItemRef.SelectSingleNode("RefNumber").InnerText;
								appliedToTxnRet.LinkedTxn.LinkType = ItemRef.SelectSingleNode("LinkType") == null ? null : ItemRef.SelectSingleNode("LinkType").InnerText;
								appliedToTxnRet.LinkedTxn.Amount = ItemRef.SelectSingleNode("Amount") == null ? (decimal?) null : decimal.Parse(ItemRef.SelectSingleNode("Amount").InnerText);
							}
							#endregion


							billPaymentCheckRet.AppliedToTxnRets.Add(appliedToTxnRet);
						}
						#endregion LineItems

						list.Add(billPaymentCheckRet);
					}

						break;
				#endregion

				#region ARRefundCreditCardQuery

				case ActionType.ARRefundCreditCardQuery:
			ARRefundCreditCardRet ARRefundCreditCardRet;
			ARRefundCreditCard_CreditCardTxnInfo CreditCardTxnInfo;
			ItemList = responseNode.SelectNodes("//ARRefundCreditCardRet");
			for (int i = 0; i < ItemList.Count; i++) {
				Item = ItemList.Item(i);
				ARRefundCreditCardRet = new ARRefundCreditCardRet {
					TxnID = Item.SelectSingleNode("TxnID") == null ? null : Item.SelectSingleNode("TxnID").InnerText,
					TimeCreated = DateTime.Parse(Item.SelectSingleNode("TimeCreated").InnerText),
					TimeModified = DateTime.Parse(Item.SelectSingleNode("TimeModified").InnerText),
					EditSequence = Item.SelectSingleNode("EditSequence") == null ? null : Item.SelectSingleNode("EditSequence").InnerText,
					TxnNumber = Item.SelectSingleNode("TxnNumber") == null ? (int?) null : int.Parse(Item.SelectSingleNode("TxnNumber").InnerText),
					TxnDate = DateTime.Parse(Item.SelectSingleNode("TxnDate").InnerText),
					RefNumber = Item.SelectSingleNode("RefNumber") == null ? null : Item.SelectSingleNode("RefNumber").InnerText,
					TotalAmount = Item.SelectSingleNode("TotalAmount") == null ? 0 : decimal.Parse(Item.SelectSingleNode("TotalAmount").InnerText),
					ExchangeRate = Item.SelectSingleNode("ExchangeRate") == null ? 0 : float.Parse(Item.SelectSingleNode("ExchangeRate").InnerText),
					TotalAmountInHomeCurrency = Item.SelectSingleNode("TotalAmountInHomeCurrency") == null ? 0 : decimal.Parse(Item.SelectSingleNode("TotalAmountInHomeCurrency").InnerText),
					Memo = Item.SelectSingleNode("Memo") == null ? null : Item.SelectSingleNode("Memo").InnerText,
					ExternalGUID = Item.SelectSingleNode("ExternalGUID") == null ? null : Item.SelectSingleNode("ExternalGUID").InnerText,
				};

				#region CustomerRef
				if (Item.SelectSingleNode("CustomerRef") == null) {
					ARRefundCreditCardRet.CustomerRef.ListID = null;
					ARRefundCreditCardRet.CustomerRef.FullName = null;
				}

				else {
					ItemRef = Item.SelectSingleNode("CustomerRef");
					ARRefundCreditCardRet.CustomerRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
					ARRefundCreditCardRet.CustomerRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
				}
				#endregion CustomerRef

				#region RefundFromAccountRef
				if (Item.SelectSingleNode("RefundFromAccountRef") == null) {
					ARRefundCreditCardRet.RefundFromAccountRef.ListID = null;
					ARRefundCreditCardRet.RefundFromAccountRef.FullName = null;
				}
				else {
					ItemRef = Item.SelectSingleNode("RefundFromAccountRef");
					ARRefundCreditCardRet.RefundFromAccountRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
					ARRefundCreditCardRet.RefundFromAccountRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
				}
				#endregion

				#region CurrencyRef
				if (Item.SelectSingleNode("CurrencyRef") == null) {
					ARRefundCreditCardRet.CurrencyRef.ListID = null;
					ARRefundCreditCardRet.CurrencyRef.FullName = null;
				}
				else {
					ItemRef = Item.SelectSingleNode("CurrencyRef");
					ARRefundCreditCardRet.CurrencyRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
					ARRefundCreditCardRet.CurrencyRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
				}
				#endregion

				#region Address
				if (Item.SelectSingleNode("Address") == null) {
					ARRefundCreditCardRet.Address.Addr1 = null;
					ARRefundCreditCardRet.Address.Addr2 = null;
					ARRefundCreditCardRet.Address.Addr3 = null;
					ARRefundCreditCardRet.Address.Addr4 = null;
					ARRefundCreditCardRet.Address.Addr5 = null;
					ARRefundCreditCardRet.Address.City = null;
					ARRefundCreditCardRet.Address.State = null;
					ARRefundCreditCardRet.Address.PostalCode = null;
					ARRefundCreditCardRet.Address.Country = null;
					ARRefundCreditCardRet.Address.Note = null;

				}
				else {
					ItemRef = Item.SelectSingleNode("Address");
					ARRefundCreditCardRet.Address.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
					ARRefundCreditCardRet.Address.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
					ARRefundCreditCardRet.Address.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
					ARRefundCreditCardRet.Address.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
					ARRefundCreditCardRet.Address.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
					ARRefundCreditCardRet.Address.City = ItemRef.SelectSingleNode("City") == null ? null : ItemRef.SelectSingleNode("City").InnerText;
					ARRefundCreditCardRet.Address.State = ItemRef.SelectSingleNode("State") == null ? null : ItemRef.SelectSingleNode("State").InnerText;
					ARRefundCreditCardRet.Address.PostalCode = ItemRef.SelectSingleNode("PostalCode") == null ? null : ItemRef.SelectSingleNode("PostalCode").InnerText;
					ARRefundCreditCardRet.Address.Country = ItemRef.SelectSingleNode("Country") == null ? null : ItemRef.SelectSingleNode("Country").InnerText;
					ARRefundCreditCardRet.Address.Note = ItemRef.SelectSingleNode("Note") == null ? null : ItemRef.SelectSingleNode("Note").InnerText;
				}
				#endregion

				#region AddressBlock
				if (Item.SelectSingleNode("AddressBlock") == null) {
					ARRefundCreditCardRet.AddressBlock.Addr1 = null;
					ARRefundCreditCardRet.AddressBlock.Addr2 = null;
					ARRefundCreditCardRet.AddressBlock.Addr3 = null;
					ARRefundCreditCardRet.AddressBlock.Addr4 = null;
					ARRefundCreditCardRet.AddressBlock.Addr5 = null;			
				}
				else {
					ItemRef = Item.SelectSingleNode("AddressBlock");
					ARRefundCreditCardRet.AddressBlock.Addr1 = ItemRef.SelectSingleNode("Addr1") == null ? null : ItemRef.SelectSingleNode("Addr1").InnerText;
					ARRefundCreditCardRet.AddressBlock.Addr2 = ItemRef.SelectSingleNode("Addr2") == null ? null : ItemRef.SelectSingleNode("Addr2").InnerText;
					ARRefundCreditCardRet.AddressBlock.Addr3 = ItemRef.SelectSingleNode("Addr3") == null ? null : ItemRef.SelectSingleNode("Addr3").InnerText;
					ARRefundCreditCardRet.AddressBlock.Addr4 = ItemRef.SelectSingleNode("Addr4") == null ? null : ItemRef.SelectSingleNode("Addr4").InnerText;
					ARRefundCreditCardRet.AddressBlock.Addr5 = ItemRef.SelectSingleNode("Addr5") == null ? null : ItemRef.SelectSingleNode("Addr5").InnerText;
				}
				#endregion

				#region PaymentMethodRef
				if (Item.SelectSingleNode("PaymentMethodRef") == null) {
							ARRefundCreditCardRet.PaymentMethodRef.ListID = null;
					ARRefundCreditCardRet.PaymentMethodRef.FullName = null;
				}
				else {
					ItemRef = Item.SelectSingleNode("PaymentMethodRef");
					ARRefundCreditCardRet.PaymentMethodRef.ListID = ItemRef.SelectSingleNode("ListID") == null ? null : ItemRef.SelectSingleNode("ListID").InnerText;
					ARRefundCreditCardRet.PaymentMethodRef.FullName = ItemRef.SelectSingleNode("FullName") == null ? null : ItemRef.SelectSingleNode("FullName").InnerText;
				}
				#endregion
				
				#region CreditCardTxnInfo List

				LineItemList = Item.SelectNodes("CreditCardTxnInfo");
				for (int j = 0; j < LineItemList.Count; j++) {
					LineItem = LineItemList.Item(j);
					CreditCardTxnInfo = new ARRefundCreditCard_CreditCardTxnInfo();

							#region CreditCardTxnInputInfo
							if (LineItem.SelectSingleNode("CreditCardTxnInputInfo") == null) {
								CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardNumber = null;
								CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationMonth = 0;
								CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationYear = 0;
								CreditCardTxnInfo.CreditCardTxnInputInfo.NameOnCard = null;
								CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress = null;
								CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode = null;
								CreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode = null;
								CreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode = null;
								CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType = null;
					}
					else {
						ItemRef = LineItem.SelectSingleNode("CreditCardTxnInputInfo");
								CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardNumber = ItemRef.SelectSingleNode("CreditCardNumber") == null ? null : ItemRef.SelectSingleNode("CreditCardNumber").InnerText;
								CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationMonth = ItemRef.SelectSingleNode("ExpirationMonth") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("ExpirationMonth").InnerText);
								CreditCardTxnInfo.CreditCardTxnInputInfo.ExpirationYear = ItemRef.SelectSingleNode("ExpirationYear") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("ExpirationYear").InnerText);
								CreditCardTxnInfo.CreditCardTxnInputInfo.NameOnCard = ItemRef.SelectSingleNode("NameOnCard") == null ? null : ItemRef.SelectSingleNode("NameOnCard").InnerText;
								CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardAddress = ItemRef.SelectSingleNode("CreditCardAddress") == null ? null : ItemRef.SelectSingleNode("CreditCardAddress").InnerText;
								CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardPostalCode = ItemRef.SelectSingleNode("CreditCardPostalCode") == null ? null : ItemRef.SelectSingleNode("CreditCardPostalCode").InnerText;
								CreditCardTxnInfo.CreditCardTxnInputInfo.CommercialCardCode = ItemRef.SelectSingleNode("CommercialCardCode") == null ? null : ItemRef.SelectSingleNode("CommercialCardCode").InnerText;
								CreditCardTxnInfo.CreditCardTxnInputInfo.TransactionMode = ItemRef.SelectSingleNode("TransactionMode") == null ? null : ItemRef.SelectSingleNode("TransactionMode").InnerText;
								CreditCardTxnInfo.CreditCardTxnInputInfo.CreditCardTxnType = ItemRef.SelectSingleNode("CreditCardTxnType") == null ? null : ItemRef.SelectSingleNode("CreditCardTxnType").InnerText;
							}
							#endregion

							#region CreditCardTxnResultInfo
							if(LineItem.SelectSingleNode("CreditCardTxnResultInfo") == null) {
								CreditCardTxnInfo.CreditCardTxnResultInfo.ResultCode = 0;
								CreditCardTxnInfo.CreditCardTxnResultInfo.ResultMessage = null;
								CreditCardTxnInfo.CreditCardTxnResultInfo.CreditCardTransID = null;
								CreditCardTxnInfo.CreditCardTxnResultInfo.MerchantAccountNumber = null;
								CreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode = null;
								CreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet = null;
								CreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip = null;
								CreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch = null;
								CreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchId = null;
								CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode = 0;
								CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentStatus = null;
								CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorization = null;
								CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp = 0;
								CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp = 0;
								CreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID = null;

							}
							else {
								ItemRef = LineItem.SelectSingleNode("CreditCardTxnResultInfo");
								CreditCardTxnInfo.CreditCardTxnResultInfo.ResultCode = ItemRef.SelectSingleNode("ResultCode") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("ResultCode").InnerText);
								CreditCardTxnInfo.CreditCardTxnResultInfo.ResultMessage = ItemRef.SelectSingleNode("ResultMessage") == null ? null : ItemRef.SelectSingleNode("ResultMessage").InnerText;
								CreditCardTxnInfo.CreditCardTxnResultInfo.CreditCardTransID =  ItemRef.SelectSingleNode("CreditCardTransID") == null ? null : ItemRef.SelectSingleNode("CreditCardTransID").InnerText;
								CreditCardTxnInfo.CreditCardTxnResultInfo.MerchantAccountNumber = ItemRef.SelectSingleNode("MerchantAccountNumber") == null ? null : ItemRef.SelectSingleNode("MerchantAccountNumber").InnerText;
								CreditCardTxnInfo.CreditCardTxnResultInfo.AuthorizationCode = ItemRef.SelectSingleNode("AuthorizationCode") == null ? null : ItemRef.SelectSingleNode("AuthorizationCode").InnerText;
								CreditCardTxnInfo.CreditCardTxnResultInfo.AVSStreet = ItemRef.SelectSingleNode("AVSStreet") == null ? null : ItemRef.SelectSingleNode("AVSStreet").InnerText;
								CreditCardTxnInfo.CreditCardTxnResultInfo.AVSZip = ItemRef.SelectSingleNode("AVSZip") == null ? null : ItemRef.SelectSingleNode("AVSZip").InnerText;
								CreditCardTxnInfo.CreditCardTxnResultInfo.CardSecurityCodeMatch = ItemRef.SelectSingleNode("CardSecurityCodeMatch") == null ? null : ItemRef.SelectSingleNode("CardSecurityCodeMatch").InnerText;
								CreditCardTxnInfo.CreditCardTxnResultInfo.ReconBatchId = ItemRef.SelectSingleNode("ReconBatchId") == null ? null : ItemRef.SelectSingleNode("ReconBatchId").InnerText;
								CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentGroupingCode = ItemRef.SelectSingleNode("PaymentGroupingCode") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("PaymentGroupingCode").InnerText);
								CreditCardTxnInfo.CreditCardTxnResultInfo.PaymentStatus = ItemRef.SelectSingleNode("PaymentStatus") == null ? null : ItemRef.SelectSingleNode("PaymentStatus").InnerText;
								CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorization = ItemRef.SelectSingleNode("TxnAuthorization") == null ? null : ItemRef.SelectSingleNode("TxnAuthorization").InnerText;
								CreditCardTxnInfo.CreditCardTxnResultInfo.TxnAuthorizationStamp = ItemRef.SelectSingleNode("TxnAuthorizationStamp") == null ? 0 : int.Parse(ItemRef.SelectSingleNode("TxnAuthorizationStamp").InnerText);
								CreditCardTxnInfo.CreditCardTxnResultInfo.ClientTransID = ItemRef.SelectSingleNode("ClientTransID") == null ? null : ItemRef.SelectSingleNode("ClientTransID").InnerText;
							}

							#endregion

							ARRefundCreditCardRet.CreditCardTxnInfo.Add(CreditCardTxnInfo);
							//Recent Addition check tomorrow.
						}
						#endregion
					}

			break;

				#endregion

				default:
					break;
			}

			return list;
		}
		#endregion Utilities

		#region Public Methods

		#region Connection
		public void CloseConnection()
		{
			if (SessionBegun) {
				Rp.EndSession(SessionId);
				SessionBegun = false;
			}

			if (ConnectionOpen) {
				Rp.CloseConnection();
				ConnectionOpen = false;
			}
		}

		public bool OpenConnection(string appId = null, string appName = null, QBXMLRPConnectionType connPref = QBXMLRPConnectionType.localQBD, string qbFileName = null, QBFileMode reqFileMode = QBFileMode.qbFileOpenDoNotCare)
		{
			#region Input Validation
			if (String.IsNullOrWhiteSpace(appId))
				appId = DEFAULT_APP_NAME;
			if (String.IsNullOrWhiteSpace(appName))
				appName = DEFAULT_APP_NAME;
			if (connPref < 0)
				connPref = QBXMLRPConnectionType.localQBD;
			if (String.IsNullOrWhiteSpace(qbFileName))
				qbFileName = "";
			if (reqFileMode < 0)
				reqFileMode = QBFileMode.qbFileOpenDoNotCare;
			#endregion Input Validation

			try {
				Rp = new RequestProcessor2();
				Rp.OpenConnection2(appId, appName, connPref);
				ConnectionOpen = true;
				SessionId = Rp.BeginSession(qbFileName, reqFileMode);
				SessionBegun = true;
				return true;
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in QBV20191021Util.OpenConnection(string appId='{3}', string appName='{4}', QBXMLRPConnectionType connPref='{5}', string qbFileName='{6}', QBFileMode reqFileMode='{7}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, appId, appName, connPref.ToString(), qbFileName, reqFileMode.ToString());
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of QBV20191021Util.OpenConnection(string appId='{3}', string appName='{4}', QBXMLRPConnectionType connPref='{5}', string qbFileName='{6}', QBFileMode reqFileMode='{7}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, appId, appName, connPref.ToString(), qbFileName, reqFileMode.ToString());

				if (Debug)
					Console.Write("\n{0}", log);
				#endregion Log

				if (SessionBegun) {
					Rp.EndSession(SessionId);
					SessionBegun = false;
				}

				if (ConnectionOpen) {
					Rp.CloseConnection();
					ConnectionOpen = false;
				}

				return false;
			}
		}
		#endregion Connection

		#region InventoryAdjustment
		public bool AddInventoryAdjustment(string accountRefListId, string itemRefFullName, int quantityDifference)
		{
			#region Input Validation
			var errorMsg = "";

			if (String.IsNullOrWhiteSpace(accountRefListId))
				errorMsg = String.Format("{0}<accountRefListId> is required.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(itemRefFullName))
				errorMsg = String.Format("{0}<itemRefFullName> is required.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg)) {
				Console.Write("\n{0}Error in QB20191021Util.AddInventoryAdjustment(string accountRefListId, string itemRefFullName, int quantityDifference).{1}", errorMsg, Environment.NewLine);
				return false;
			}
			#endregion Input Validation

			try {
				var parameters = new KeyValuePair<string, object>[] {
					new KeyValuePair<string, object>("accountRefListId", accountRefListId),
					new KeyValuePair<string, object>("itemRefFullName", itemRefFullName),
					new KeyValuePair<string, object>("quantityDifference", quantityDifference),
				};

				var result = DoAction(ActionType.InventoryAdjustmentAdd, parameters);
				return true;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					errorMsg = String.Format("{0}{2}Exception in QB20191021Util.AddInventoryAdjustment(string accountRefListId='{3}', string itemRefFullName='{4}', int quantityDifference='{5}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, accountRefListId, itemRefFullName, quantityDifference);
				else
					errorMsg = String.Format("{0}{2}Exception in INNER EXCEPTION of QB20191021Util.AddInventoryAdjustment(string accountRefListId='{3}', string itemRefFullName='{4}', int quantityDifference='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, accountRefListId, itemRefFullName, quantityDifference);

				Console.Write("\n{0}", errorMsg);
				return false;
			}
		}

		public List<InventoryAdjustmentRet> GetAllInventoryAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<InventoryAdjustmentRet>();
			var rets = DoAction(ActionType.InventoryAdjustmentQuery, parameters);

			foreach (InventoryAdjustmentRet ret in rets)
				list.Add(ret);

			return list;
		}
		#endregion InventoryAdjustment

		#region ChargeQuery
		public List<ChargeRet> GetAllChargeAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<ChargeRet>();
			var rets = DoAction(ActionType.ChargeQuery, parameters);

			foreach (ChargeRet ret in rets)
				list.Add(ret);

			return list;
		}

		#endregion

		#region BillQuery
		public List<BillRet> GetAllBillAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<BillRet>();
			var rets = DoAction(ActionType.BillQuery, parameters);

			foreach (BillRet ret in rets)
				list.Add(ret);

			return list;
		}

		#endregion

		#region BillPaymentCreditCardQuery
		public List<BillPaymentCheckRet> GetAllBillPaymentCheckAdjustments(DateTime? dtFrom, DateTime? dtTo = null)
		{
			#region Input Validation
			if (dtFrom == null || dtFrom < MIN_DATE)
				dtFrom = MIN_DATE;
			if (dtTo == null || dtTo > MAX_DATE)
				dtTo = MAX_DATE;

			var dtFromStr = dtFrom.Value.ToString(QB_FORMAT);
			var dtToStr = dtTo.Value.ToString(QB_FORMAT);
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("dtFrom", dtFromStr),
				new KeyValuePair<string, object>("dtTo", dtToStr),
			};

			var list = new List<BillPaymentCheckRet>();
			var rets = DoAction(ActionType.ChargeQuery, parameters);

			foreach (BillPaymentCheckRet ret in rets)
				list.Add(ret);

			return list;
		}

		#endregion

		#endregion Public Methods
	}
}