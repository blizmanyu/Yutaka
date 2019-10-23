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
		const string DEFAULT_APP_NAME = "QB20191021Util";
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
		#endregion Private Utilities

		#region Public Methods
		#region Connection

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
					log = String.Format("{0}{2}Exception thrown in QB20191021Util.OpenConnection(string appId='{3}', string appName='{4}', string qbFile='{5}', ENOpenMode openMode='{6}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, appId, appName, qbFile, openMode.ToString());
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of QB20191021Util.OpenConnection(string appId='{3}', string appName='{4}', string qbFile='{5}', ENOpenMode openMode='{6}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, appId, appName, qbFile, openMode.ToString());

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
		#endregion Connection

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