using System;
using System.IO;
using Interop.QBFC13;
using Yutaka.IO;

namespace Yutaka.QuickBooks
{
	public class Qb13Util
	{
		#region Fields
		// Constants & Enums //
		public const string QB_FORMAT = @"yyyy-MM-ddTHH:mm:ssK";
		protected const string TIMESTAMP = @"HH:mm:ss.fff";
		/// <summary>
		/// Trace - very detailed logs, which may include high-volume information such as protocol payloads. This log level is typically only enabled during development. Ex: begin method X, end method X
		/// Debug - debugging information, less detailed than trace, typically not enabled in production environment. Ex: executed query, user authenticated, session expired
		/// Info - information messages, which are normally enabled in production environment. Ex: mail sent, user updated profile etc
		/// Warn - warning messages, typically for non-critical issues, which can be recovered or which are temporary failures. Ex: application will continue
		/// Error - error messages - most of the time these are Exceptions. Ex: application may or may not continue
		/// Fatal - very serious errors! Ex: application is going down
		/// Off - disables logging when used as the minimum log level.
		/// </summary>
		public enum LogLevel { Trace = 0, Debug = 1, Info = 2, Warn = 3, Error = 4, Fatal = 5, Off = 6, };
		public enum QueryType { Bill = 0, };

		// Private Fields //
		private FileUtil _fileUtil;
		private QBSessionManager _sessionManager;
		private bool _connectionOpen;
		private bool _sessionBegun;
		private string _appName;

		// Public Properties //
		public LogLevel logLevel;
		public string CompanyFilePath;
		public string LogFolder;
		#endregion Fields

		#region Constructors
		[Obsolete("Deprecated. This is only here for legacy support. Should NOT be used for new development.")]
		public Qb13Util()
		{
			logLevel = LogLevel.Info;
			_sessionManager = null;
			_connectionOpen = false;
			_sessionBegun = false;
			_appName = "Yutaka.QB13Util";
			_fileUtil = new FileUtil();
			CompanyFilePath = "";
			LogFolder = @"C:\Logs\Rcw.QuickBooks\";
		}

		public Qb13Util(string appName, LogLevel loglevel = LogLevel.Info)
		{
			if (String.IsNullOrWhiteSpace(appName))
				appName = "Yutaka.QB13Util";

			logLevel = loglevel;
			_sessionManager = null;
			_connectionOpen = false;
			_sessionBegun = false;
			_appName = appName;
			_fileUtil = new FileUtil();
			CompanyFilePath = "";
			LogFolder = @"C:\Logs\Rcw.QuickBooks\";
		}
		#endregion Constructors

		public IMsgSetResponse Query(QueryType queryType, DateTime? fromDate = null, DateTime? toDate = null)
		{
			#region Log
			if (logLevel <= LogLevel.Trace) {
				var log = String.Format("\n[{0}] Begin method Query(QueryType queryType, DateTime? fromDate = null, DateTime? toDate = null).", DateTime.Now.ToString(TIMESTAMP));
				Console.Write(log);
				_fileUtil.Write(log, String.Format("{0}{1}.txt", LogFolder, DateTime.Now.ToString("yyyy MMdd HH30")));
			}
			#endregion Log

			var now = DateTime.Now;
			var minDate = now.AddYears(-10);

			#region Input Validation
			if (fromDate == null || fromDate < minDate)
				fromDate = minDate;
			if (toDate == null)
				toDate = now.AddYears(1);
			#endregion Input Validation

			try {
				//Create the session Manager object
				_sessionManager = new QBSessionManager();

				//Create the message set request object to hold our request
				var requestMsgSet = _sessionManager.CreateMsgSetRequest("US",13,0);
				requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

				#region switch (queryType) {
				switch (queryType) {
					case QueryType.Bill:
						BuildBillQueryRq(requestMsgSet, fromDate, toDate);
						if (logLevel <= LogLevel.Debug)
							File.WriteAllText(@"C:\TEMP\BillRequest.xml", requestMsgSet.ToXMLString());
						break;
					default:
						return null;
				}
				#endregion switch (queryType)

				//Connect to QuickBooks and begin a session
				_sessionManager.OpenConnection(_appName, _appName);
				_connectionOpen = true;
				_sessionManager.BeginSession(CompanyFilePath, ENOpenMode.omDontCare);
				_sessionBegun = true;

				#region Log
				if (logLevel <= LogLevel.Debug) {
					var log = String.Format("\n[{0}] Session started.", DateTime.Now.ToString(TIMESTAMP));
					Console.Write(log);
					_fileUtil.Write(log, String.Format("{0}{1}.txt", LogFolder, DateTime.Now.ToString("yyyy MMdd HH30")));
				}
				#endregion Log

				//Send the request and get the response from QuickBooks
				var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
				if (logLevel <= LogLevel.Debug)
					File.WriteAllText(@"C:\TEMP\BillResponse.xml", responseMsgSet.ToXMLString());

				//End the session and close the connection to QuickBooks
				_sessionManager.EndSession();
				_sessionBegun = false;
				_sessionManager.CloseConnection();
				_connectionOpen = false;

				#region Log
				if (logLevel <= LogLevel.Debug) {
					var log = String.Format("\n[{0}] Session ended.", DateTime.Now.ToString(TIMESTAMP));
					Console.Write(log);
					_fileUtil.Write(log, String.Format("{0}{1}.txt", LogFolder, DateTime.Now.ToString("yyyy MMdd HH30")));
				}
				#endregion Log

				return responseMsgSet;
			}

			catch (Exception ex) {
				var msg = "";

				#region Log
				if (logLevel <= LogLevel.Error) {
					if (ex.InnerException == null)
						msg = String.Format("{0}{2}Exception thrown in QB13Util.Query(QueryType queryType={3}, DateTime? fromDate='{4}', DateTime? toDate='{5}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, queryType, fromDate, toDate);
					else
						msg = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of QB13Util.Query(QueryType queryType={3}, DateTime? fromDate='{4}', DateTime? toDate='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, queryType, fromDate, toDate);

					msg = String.Format("\n[{0}] {1}", DateTime.Now.ToString(TIMESTAMP), msg);
					Console.Write(msg);
					_fileUtil.Write(msg, String.Format("{0}{1}.txt", LogFolder, DateTime.Now.ToString("yyyy MMdd HH30")));
				}
				#endregion Log

				if (_sessionBegun) {
					_sessionManager.EndSession();
					_sessionBegun = false;
				}

				if (_connectionOpen) {
					_sessionManager.CloseConnection();
					_connectionOpen = false;
				}

				#region Log
				if (logLevel <= LogLevel.Debug) {
					var log = String.Format("\n[{0}] Session ended.", DateTime.Now.ToString(TIMESTAMP));
					Console.Write(log);
					_fileUtil.Write(log, String.Format("{0}{1}.txt", LogFolder, DateTime.Now.ToString("yyyy MMdd HH30")));
				}
				#endregion Log

				throw new Exception(msg);
			}
		}

		protected void BuildBillQueryRq(IMsgSetRequest requestMsgSet, DateTime? fromDate = null, DateTime? toDate = null)
		{
			#region Log
			if (logLevel <= LogLevel.Trace) {
				var log = String.Format("\n[{0}] Begin method BuildBillQueryRq(IMsgSetRequest requestMsgSet, DateTime? fromDate=null, DateTime? toDate=null).", DateTime.Now.ToString(TIMESTAMP));
				Console.Write(log);
				_fileUtil.Write(log, String.Format("{0}{1}.txt", LogFolder, DateTime.Now.ToString("yyyy MMdd HH30")));
			}
			#endregion Log

			var now = DateTime.Now;
			var minDate = now.AddYears(-10);

			#region Input Validation
			if (requestMsgSet == null)
				return;
			if (fromDate == null || fromDate < minDate)
				fromDate = minDate;
			if (toDate == null)
				toDate = now.AddYears(1);
			#endregion Input Validation

			var BillQueryRq = requestMsgSet.AppendBillQueryRq();
			//Set field value for FromModifiedDate
			BillQueryRq.ORBillQuery.BillFilter.ORDateRangeFilter.ModifiedDateRangeFilter.FromModifiedDate.SetValue(fromDate.Value, false);
			//Set field value for ToModifiedDate
			BillQueryRq.ORBillQuery.BillFilter.ORDateRangeFilter.ModifiedDateRangeFilter.ToModifiedDate.SetValue(toDate.Value, false);
			//Set field value for IncludeLineItems
			BillQueryRq.IncludeLineItems.SetValue(true);
		}

		protected void ProcessQueryResponseTemplate(IMsgSetResponse responseMsgSet)
		{
			#region Log & Input Validation
			if (logLevel <= LogLevel.Trace) {
				var log = String.Format("\n[{0}] Begin method ProcessQueryResponseTemplate(IMsgSetResponse responseMsgSet).", DateTime.Now.ToString(TIMESTAMP));
				Console.Write(log);
				_fileUtil.Write(log, String.Format("{0}{1}.txt", LogFolder, DateTime.Now.ToString("yyyy MMdd HH30")));
			}

			if (responseMsgSet == null) {
				if (logLevel <= LogLevel.Debug) {
					var log = String.Format("\n[{0}] <responseMsgSet> is empty.", DateTime.Now.ToString(TIMESTAMP));
					Console.Write(log);
					_fileUtil.Write(log, String.Format("{0}{1}.txt", LogFolder, DateTime.Now.ToString("yyyy MMdd HH30")));
				}

				return;
			}

			var responseList = responseMsgSet.ResponseList;

			if (responseList == null) {
				if (logLevel <= LogLevel.Debug) {
					var log = String.Format("\n[{0}] responseMsgSet.ResponseList is empty.", DateTime.Now.ToString(TIMESTAMP));
					Console.Write(log);
					_fileUtil.Write(log, String.Format("{0}{1}.txt", LogFolder, DateTime.Now.ToString("yyyy MMdd HH30")));
				}

				return;
			}
			#endregion Log & Input Validation

			IResponse response;
			ENResponseType responseType;

			// if we sent only one request, there is only one response, we'll walk the list for this sample
			for (int i = 0; i < responseList.Count; i++) {
				response = responseList.GetAt(i);
				// check the status code of the response, 0=ok, >0 is warning
				if (response.StatusCode < 0) {
					#region Log
					if (logLevel <= LogLevel.Error) {
						var log = String.Format("\n[{0}] {1}", DateTime.Now.ToString(TIMESTAMP), response.StatusMessage);
						Console.Write(log);
						_fileUtil.Write(log, String.Format("{0}{1}.txt", LogFolder, DateTime.Now.ToString("yyyy MMdd HH30")));
					}
					#endregion Log
				}

				else {
					// the request-specific response is in the details, make sure we have some
					if (response.Detail == null) {
						#region Log
						if (logLevel <= LogLevel.Warn) {
							var log = String.Format("\n[{0}] response.Detail is empty.", DateTime.Now.ToString(TIMESTAMP));
							Console.Write(log);
							_fileUtil.Write(log, String.Format("{0}{1}.txt", LogFolder, DateTime.Now.ToString("yyyy MMdd HH30")));
						}
						#endregion Log
					}

					else {
						// make sure the response is the type we're expecting
						responseType = (ENResponseType) response.Type.GetValue();

						switch (responseType) {
							case ENResponseType.rtBillQueryRs:
								// upcast to more specific type here, this is safe because we checked with response.Type check above
								var BillRet = (IBillRetList) response.Detail;
								// Do what you want to do here //
								break;
							default:
								break;
						}
					}
				}
			}
		}
	}
}