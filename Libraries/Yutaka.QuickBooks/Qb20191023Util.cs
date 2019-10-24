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

		public enum ActionType { InventoryAdjustmentAdd, InventoryAdjustmentQuery, };
		public bool Debug;

		private QBSessionManager SessionManager;
		private bool ConnectionOpen;
		private bool SessionBegun;
		#endregion Fields

		public Qb20191023Util()
		{
			Debug = false;
			SessionManager = null;
			ConnectionOpen = false;
			SessionBegun = false;
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

				//return ProcessResponse(actionType, responseStr);
				return new List<object>();
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

		public List<InventoryAdjustmentRet> GetAllInventoryAdjustments(DateTime? dtFrom)
		{
			return new List<InventoryAdjustmentRet>();
		}
		#endregion InventoryAdjustment
		#endregion Public Methods
	}
}