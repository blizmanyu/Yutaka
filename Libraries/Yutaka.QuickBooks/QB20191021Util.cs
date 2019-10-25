using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Interop.QBXMLRP2Lib;

namespace Yutaka.QuickBooks
{
	public class QB20191021Util
	{
		#region Fields
		protected const string DEFAULT_APP_NAME = "QB20191021Util";
		protected const string QB_FORMAT = "yyyy-MM-ddTHH:mm:ssK";
		protected readonly DateTime MIN_DATE = DateTime.Now.AddYears(-10);
		protected readonly DateTime MAX_DATE = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59, 999, DateTimeKind.Local);
		protected enum ActionType { InventoryAdjustmentAdd, InventoryAdjustmentQuery, };
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

				return ProcessResponse(actionType, responseStr);
				//return new List<object>(); // For debugging only //
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
		#endregion Public Methods
	}
}