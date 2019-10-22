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
		private const string QB_FORMAT = "yyyy-MM-ddTHH:mm:ssK";
		public const string DEFAULT_APP_NAME = "QB20191021Util";
		public enum ActionType { InventoryAdjustmentAdd, InventoryAdjustmentQuery, };
		public bool Debug;
		private RequestProcessor2 Rp;
		private bool ConnectionOpen;
		private bool SessionBegun;
		private string SessionId;
		#endregion Fields

		public QB20191021Util()
		{
			Debug = false;
			Rp = null;
			ConnectionOpen = false;
			SessionBegun = false;
			SessionId = null;
		}

		#region Private Utilities
		private string BeautifyXml(string xml)
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

		private void BuildRequest(XmlDocument doc, XmlElement parent, ActionType actionType, string startTime, string endTime)
		{
			var request = doc.CreateElement(String.Format("{0}Rq", actionType.ToString()));
			parent.AppendChild(request);

			switch (actionType) {
				#region InventoryAdjustmentAdd
				case ActionType.InventoryAdjustmentAdd:
					request.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", startTime));
					request.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", endTime));
					break;
				#endregion InventoryAdjustmentAdd
				#region InventoryAdjustmentQuery
				case ActionType.InventoryAdjustmentQuery:
					var ModifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
					request.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", startTime));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", endTime));
					request.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
					break;
				#endregion InventoryAdjustmentQuery
				default:
					break;
			}
		}

		private XmlElement MakeSimpleElem(XmlDocument doc, string tagName, string tagVal)
		{
			var elem = doc.CreateElement(tagName);
			elem.InnerText = tagVal;
			return elem;
		}

		private List<object> ProcessResponse(ActionType actionType, string response)
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

		private List<object> ProcessReturn(ActionType actionType, XmlNode responseNode)
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
							TxnID = Item.SelectSingleNode("ListID") == null ? null : Item.SelectSingleNode("ListID").InnerText,
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
								QuantityDifference = int.Parse(LineItem.SelectSingleNode("QuantityDifference").InnerText),
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
					}

					break;
				#endregion InventoryAdjustment
				default:
					break;
			}

			return list;
		}
		#endregion Private Utilities

		#region Public Methods
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

		public List<object> DoAction(ActionType actionType, DateTime? startTime = null, DateTime? endTime = null)
		{
			#region Input Validation
			if (actionType < 0)
				throw new Exception(String.Format("<actionType> is required.{0}Exception thrown in QB20191021Util.DoAction(ActionType actionType, DateTime? startTime, DateTime? endTime).{0}", Environment.NewLine));

			var now = DateTime.Now;
			var minDate = now.AddYears(-10);
			var maxDate = new DateTime(now.Year, 12, 31, 23, 59, 59, 999);

			if (startTime == null || startTime < minDate)
				startTime = minDate;
			if (endTime == null || endTime > maxDate)
				endTime = maxDate;

			var startTimeStr = startTime.Value.ToString(QB_FORMAT);
			var endTimeStr = endTime.Value.ToString(QB_FORMAT);

			if (endTimeStr.Length < 20)
				endTimeStr = string.Format("{0}-07:00", endTimeStr);
			#endregion Input Validation

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
				BuildRequest(requestXmlDoc, inner, actionType, startTimeStr, endTimeStr);

				if (Debug)
					File.WriteAllText(String.Format(@"C:\TEMP\{0}Request.xml", actionType.ToString()), BeautifyXml(requestXmlDoc.OuterXml));

				//Send the request and get the response from QuickBooks
				var responseStr = Rp.ProcessRequest(SessionId, requestXmlDoc.OuterXml);

				if (Debug)
					File.WriteAllText(String.Format(@"C:\TEMP\{0}Response.xml", actionType.ToString()), BeautifyXml(responseStr));

				return ProcessResponse(actionType, responseStr);
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in QBV20191021Util.DoAction(ActionType actionType='{3}', DateTime? startTime='{4}', DateTime? endTime='{5}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, actionType.ToString(), startTime, endTime);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of QBV20191021Util.DoAction(ActionType actionType='{3}', DateTime? startTime='{4}', DateTime? endTime='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, actionType.ToString(), startTime, endTime);

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

				return new List<object>();
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
		#endregion Public Methods
	}
}