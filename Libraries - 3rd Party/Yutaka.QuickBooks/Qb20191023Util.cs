using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Interop.QBFC13;

namespace Yutaka.QuickBooks
{
	[Obsolete("Deprecated Oct 24, 2019. Use QB20191021Util instead.")]
	public class Qb20191023Util
	{
		#region Fields
		const string DEFAULT_APP_NAME = "Qb20191023Util";
		const string QB_FORMAT = "yyyy-MM-ddTHH:mm:ssK";
		private readonly DateTime MIN_DATE = DateTime.Now.AddYears(-10);
		private readonly DateTime MAX_DATE = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59, 999, DateTimeKind.Local);

		private QBSessionManager SessionManager;
		private bool ConnectionOpen;
		private bool SessionBegun;
		public bool Debug;
		public enum ActionType { InventoryAdjustmentAdd, InventoryAdjustmentQuery, };
		#endregion Fields

		public Qb20191023Util()
		{
			SessionManager = null;
			ConnectionOpen = false;
			SessionBegun = false;
			Debug = false;
		}

		#region Private Utilities
		protected void BuildRequest(IMsgSetRequest requestMsgSet, ActionType actionType, params KeyValuePair<string, object>[] parameters)
		{
			switch (actionType) {
				#region InventoryAdjustmentAdd
				case ActionType.InventoryAdjustmentAdd:
					//InventoryAdjustmentAdd = doc.CreateElement("InventoryAdjustmentAdd");
					//request.AppendChild(InventoryAdjustmentAdd);
					//AccountRef = doc.CreateElement("AccountRef");
					//InventoryAdjustmentAdd.AppendChild(AccountRef);
					//AccountRef.AppendChild(MakeSimpleElem(doc, "ListID", "IDTYPE"));
					//InventoryAdjustmentLineAdd = doc.CreateElement("InventoryAdjustmentLineAdd");
					//InventoryAdjustmentAdd.AppendChild(InventoryAdjustmentLineAdd);
					//ItemRef = doc.CreateElement("ItemRef");
					//InventoryAdjustmentLineAdd.AppendChild(ItemRef);
					//ItemRef.AppendChild(MakeSimpleElem(doc, "ListID", "IDTYPE"));
					//QuantityAdjustment = doc.CreateElement("QuantityAdjustment");
					//InventoryAdjustmentLineAdd.AppendChild(QuantityAdjustment);
					//QuantityAdjustment.AppendChild(MakeSimpleElem(doc, "QuantityDifference", "QUANTYPE"));
					break;
				#endregion InventoryAdjustmentAdd
				#region InventoryAdjustmentQuery
				case ActionType.InventoryAdjustmentQuery:
					var InventoryAdjustmentQueryRq= requestMsgSet.AppendInventoryAdjustmentQueryRq();
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.ORDateRangeFilter.ModifiedDateRangeFilter.FromModifiedDate.SetValue((DateTime) parameters[0].Value, false);
					InventoryAdjustmentQueryRq.ORInventoryAdjustmentQuery.TxnFilterWithItemFilter.ORDateRangeFilter.ModifiedDateRangeFilter.ToModifiedDate.SetValue((DateTime) parameters[1].Value, false);
					InventoryAdjustmentQueryRq.IncludeLineItems.SetValue(true);
					break;
				#endregion InventoryAdjustmentQuery
				default:
					break;
			}
		}

		public List<object> DoAction(ActionType actionType, params KeyValuePair<string, object>[] parameters)
		{
			if (actionType < 0)
				throw new Exception(String.Format("<actionType> is required.{0}Exception thrown in Qb20191023Util.DoAction(ActionType actionType, params KeyValuePair<string, object>[] parameters).{0}", Environment.NewLine));

			try {
				if (!ConnectionOpen || !SessionBegun)
					OpenConnection();

				//Create the message set request object to hold our request
				var requestMsgSet = SessionManager.CreateMsgSetRequest("US",13,0);
				requestMsgSet.Attributes.OnError = ENRqOnError.roeStop;

				BuildRequest(requestMsgSet, actionType, parameters);

				if (Debug)
					File.WriteAllText(String.Format(@"C:\TEMP\{0}Request.xml", actionType.ToString()), requestMsgSet.ToXMLString());

				//Send the request and get the response from QuickBooks
				var responseMsgSet = SessionManager.DoRequests(requestMsgSet);

				if (Debug)
					File.WriteAllText(String.Format(@"C:\TEMP\{0}Response.xml", actionType.ToString()), responseMsgSet.ToXMLString());

				return new List<object>();
				//return ProcessResponse(responseMsgSet);
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in Qb20191023Util.DoAction(ActionType actionType='{3}', params KeyValuePair<string, object>[] parameters='{4}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, actionType.ToString(), String.Join(", ", parameters));
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Qb20191023Util.DoAction(ActionType actionType='{3}', params KeyValuePair<string, object>[] parameters='{4}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, actionType.ToString(), String.Join(", ", parameters));

				if (Debug)
					Console.Write("\n{0}", log);
				#endregion Log

				return new List<object>();
			}
		}

		protected List<object> ProcessResponse(IMsgSetResponse responseMsgSet)
		{
			#region Check Input
			if (responseMsgSet == null) {
				if (Debug)
					Console.Write("\n<responseMsgSet> is null.");

				return new List<object>();
			}

			var responseList = responseMsgSet.ResponseList;

			if (responseList == null) {
				if (Debug)
					Console.Write("\n<responseList> is null.");

				return new List<object>();
			}
			#endregion Check Input

			var response = responseList.GetAt(0);
			//check the status code of the response, 0=ok, >0 is warning
			if (response.StatusCode > -1) {
				//the request-specific response is in the details, make sure we have some
				if (response.Detail == null) {
					if (Debug)
						Console.Write("\nresponse.Detail is null.");

					return new List<object>();
				}

				return ProcessRet(responseMsgSet.ToXMLString());
			}

			else {
				if (Debug)
					Console.Write("\nStatusCode: {0}, StatusSeverity: {1}, StatusMessage: {2}", response.StatusCode, response.StatusSeverity, response.StatusMessage);

				return new List<object>();
			}
		}

		protected List<object> ProcessRet(string responseMsg)
		{
			#region Input Validation
			if (String.IsNullOrWhiteSpace(responseMsg)) {
				if (Debug)
					Console.Write("\n<responseMsg> is null.");

				return new List<object>();
			}
			#endregion Input Validation

			var list = new List<object>();



			return list;
		}
		#endregion Private Utilities

		#region Public Methods
		#region General
		public void CloseConnection()
		{
			if (SessionBegun) {
				SessionManager.EndSession();
				SessionBegun = false;
			}

			if (ConnectionOpen) {
				SessionManager.CloseConnection();
				ConnectionOpen = false;
			}
		}

		public bool OpenConnection(string appId = null, string appName = null, string qbFile = null, ENOpenMode openMode = ENOpenMode.omDontCare)
		{
			#region Input Validation
			if (String.IsNullOrWhiteSpace(appId))
				appId = DEFAULT_APP_NAME;
			if (String.IsNullOrWhiteSpace(appName))
				appName = DEFAULT_APP_NAME;
			if (String.IsNullOrWhiteSpace(qbFile))
				qbFile = "";
			if (openMode < 0)
				openMode = ENOpenMode.omDontCare;
			#endregion Input Validation

			try {
				SessionManager = new QBSessionManager();
				SessionManager.OpenConnection(appId, appName);
				ConnectionOpen = true;
				SessionManager.BeginSession(qbFile, ENOpenMode.omDontCare);
				SessionBegun = true;
				return true;
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in Qb20191023Util.OpenConnection(string appId='{3}', string appName='{4}', string qbFile='{5}', ENOpenMode openMode='{6}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, appId, appName, qbFile, openMode.ToString());
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Qb20191023Util.OpenConnection(string appId='{3}', string appName='{4}', string qbFile='{5}', ENOpenMode openMode='{6}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, appId, appName, qbFile, openMode.ToString());

				if (Debug)
					Console.Write("\n{0}", log);
				#endregion Log

				if (SessionBegun) {
					SessionManager.EndSession();
					SessionBegun = false;
				}

				if (ConnectionOpen) {
					SessionManager.CloseConnection();
					ConnectionOpen = false;
				}

				return false;
			}
		}
		#endregion General

		#region InventoryAdjustment
		public bool AddInventoryAdjustment(InventoryAdjustmentRet ia)
		{
			return true;
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

			if (dtFromStr.Length < 20 || dtToStr.Length < 20) {
				var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);

				if (dtFromStr.Length < 20)
					dtFromStr = String.Format("{0}-{1}", dtFromStr, offset);
				if (dtToStr.Length < 20)
					dtToStr = String.Format("{0}-{1}", dtToStr, offset);
			}
			#endregion Input Validation

			var parameters = new KeyValuePair<string, object>[] {
					new KeyValuePair<string, object>("dtFrom", dtFrom),
					new KeyValuePair<string, object>("dtTo", dtTo),
				};

			var list = new List<InventoryAdjustmentRet>();
			var results = DoAction(ActionType.InventoryAdjustmentQuery, parameters);

			foreach (InventoryAdjustmentRet v in results)
				list.Add(v);

			return list;
		}
		#endregion InventoryAdjustment
		#endregion Public Methods
	}
}