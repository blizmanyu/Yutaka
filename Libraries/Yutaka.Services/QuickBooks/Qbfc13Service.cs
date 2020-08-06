using System;
using Interop.QBFC13;
using Interop.QBXMLRP2Lib;

namespace Yutaka.QuickBooks
{
	public enum QueryType
	{
		AccountAdd,
		AccountMod,
		AccountQuery,
		BillAdd,
		BillPaymentCheckAdd,
	}

	public class Qbfc13Service
	{
		#region Fields
		protected static readonly short QBXMLMajorVersion = 13;
		protected static readonly short QBXMLMinorVersion = 0;
		protected static readonly string DefaultAppID = "Qbfc13Service";
		protected static readonly string DefaultCountry = "US";
		protected bool ConnectionOpen;
		protected bool SessionBegun;
		protected IMsgSetRequest RequestMsgSet;
		protected QBSessionManager SessionManager;
		public ENOpenMode OpenMode;
		public string AppID;
		public string AppName;
		public string QBFile;
		#endregion Fields

		public Qbfc13Service(string qbFile = null, string appID = null, string appName = null, ENOpenMode openMode = ENOpenMode.omDontCare)
		{
			#region Check Input
			if (qbFile == null)
				throw new ArgumentNullException("qbFile");
			else if (String.IsNullOrWhiteSpace(qbFile))
				throw new ArgumentException("<qbFile> is required.");

			if (String.IsNullOrWhiteSpace(appID)) {
				if (String.IsNullOrWhiteSpace(appName)) {
					appID = DefaultAppID;
					appName = DefaultAppID;
				}

				else
					appID = appName;
			}

			if (String.IsNullOrWhiteSpace(appName)) {
				if (String.IsNullOrWhiteSpace(appID)) {
					appID = DefaultAppID;
					appName = DefaultAppID;
				}

				else
					appName = appID;
			}
			#endregion Check Input

			ConnectionOpen = false;
			SessionBegun = false;
			RequestMsgSet = null;
			SessionManager = null;
			OpenMode = openMode;
			AppID = appID;
			AppName = appName;
			QBFile = qbFile;
		}

		public bool TryBeginSession(string qbFile, ENOpenMode? enOpenMode, out string response)
		{
			#region Check Input
			response = "";

			if (String.IsNullOrWhiteSpace(qbFile))
				response = String.Format("{0}<qbFile> is required.{1}", response, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(response)) {
				response = String.Format("{0}Exception thrown in Qbfc13Service.TryBeginSession(string qbFile, ENOpenMode? enOpenMode, out string response).{1}{1}", response, Environment.NewLine);
				return false;
			}

			if (enOpenMode == null)
				enOpenMode = ENOpenMode.omDontCare;
			#endregion Check Input

			if (SessionBegun && ConnectionOpen && SessionManager != null)
				return true;
			else {
				SessionManager = new QBSessionManager();
				//Create the message set request object to hold our request
				RequestMsgSet = SessionManager.CreateMsgSetRequest(DefaultCountry, QBXMLMajorVersion, QBXMLMinorVersion);
				RequestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

				try {
					SessionManager.OpenConnection(AppID, AppName);
					ConnectionOpen = true;
					SessionManager.BeginSession(qbFile, enOpenMode.Value);
					SessionBegun = true;
					return true;
				}

				catch (Exception ex) {
					#region Logging
					if (ex.InnerException == null)
						response = String.Format("{0}{2}Exception thrown in Qbfc13Service.TryBeginSession(string qbFile, ENOpenMode? enOpenMode, out string response).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
					else
						response = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Qbfc13Service.TryBeginSession(string qbFile, ENOpenMode? enOpenMode, out string response).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);
					#endregion Logging

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
		}

		//public void DoAccountAdd(QueryType queryType)
		//{
		//	if (SessionManager == null || RequestMsgSet == null) {
		//		//Create the session Manager object
		//		SessionManager = new QBSessionManager();

		//		//Create the message set request object to hold our request
		//		RequestMsgSet = SessionManager.CreateMsgSetRequest(DefaultCountry, QBXMLMajorVersion, QBXMLMinorVersion);
		//		RequestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;
		//	}

		//	try {
		//		BuildAccountAddRq(RequestMsgSet);

		//		if (!SessionBegun || !ConnectionOpen) {
		//			//Connect to QuickBooks and begin a session
		//			SessionManager.OpenConnection(AppID, AppName);
		//			ConnectionOpen = true;
		//			SessionManager.BeginSession(QBFile, OpenMode);
		//			SessionBegun = true;
		//		}

		//		//Send the request and get the response from QuickBooks
		//		IMsgSetResponse responseMsgSet = SessionManager.DoRequests(requestMsgSet);

		//		//End the session and close the connection to QuickBooks
		//		sessionManager.EndSession();
		//		sessionBegun = false;
		//		sessionManager.CloseConnection();
		//		connectionOpen = false;

		//		WalkAccountAddRs(responseMsgSet);
		//	}
		//	catch (Exception e) {
		//		MessageBox.Show(e.Message, "Error");
		//		if (sessionBegun) {
		//			sessionManager.EndSession();
		//		}
		//		if (connectionOpen) {
		//			sessionManager.CloseConnection();
		//		}
		//	}
		//}
	}
}