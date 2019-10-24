using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Interop.QBFC13;

namespace Yutaka.QuickBooks
{
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
		protected void BuildAddRequest(IMsgSetRequest requestMsgSet, ActionType actionType, params KeyValuePair<string, object>[] parameters)
		{

		}

		protected void BuildModRequest(IMsgSetRequest requestMsgSet, ActionType actionType, params KeyValuePair<string, object>[] parameters)
		{

		}

		protected void BuildQueryRequest(IMsgSetRequest requestMsgSet, ActionType actionType, params KeyValuePair<string, object>[] parameters)
		{

		}

		protected List<object> ProcessResponse(IMsgSetResponse responseMsgSet)
		{
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

			//if we sent only one request, there is only one response, we'll walk the list for this sample
			for (int i = 0; i < responseList.Count; i++) {
				var response = responseList.GetAt(i);
				//check the status code of the response, 0=ok, >0 is warning
				if (response.StatusCode > -1) {
					//the request-specific response is in the details, make sure we have some
					if (response.Detail != null)
						return ProcessRet(responseMsgSet.ToXMLString());
					else {
						if (Debug)
							Console.Write("\nresponse.Detail is null.");

						return new List<object>();
					}
				}

				else {
					if (Debug)
						Console.Write("\nStatusCode: {0}, StatusSeverity: {1}, StatusMessage: {2}", response.StatusCode, response.StatusSeverity, response.StatusMessage);

					return new List<object>();
				}
			}

			return new List<object>();
		}

		protected List<object> ProcessRet(string responseMsgSet)
		{
			return new List<object>();
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

				if (actionType.ToString().EndsWith("Query"))
					BuildQueryRequest(requestMsgSet, actionType, parameters);
				else if (actionType.ToString().EndsWith("Add"))
					BuildAddRequest(requestMsgSet, actionType, parameters);
				else
					BuildModRequest(requestMsgSet, actionType, parameters);

				if (Debug)
					File.WriteAllText(String.Format(@"C:\TEMP\{0}Request.xml", actionType.ToString()), requestMsgSet.ToXMLString());

				//Send the request and get the response from QuickBooks
				var responseMsgSet = SessionManager.DoRequests(requestMsgSet);

				if (Debug)
					File.WriteAllText(String.Format(@"C:\TEMP\{0}Response.xml", actionType.ToString()), responseMsgSet.ToXMLString());

				return ProcessResponse(responseMsgSet);
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