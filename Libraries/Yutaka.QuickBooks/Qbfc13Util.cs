using System;
using System.Net;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;
using Interop.QBFC13;
using Interop.QBXMLRP2Lib;
using System.Xml;

namespace Yutaka.QuickBooks
{
	public class Qbfc13Util
	{
		#region Fields
		protected const string QB_FORMAT = @"yyyy-MM-ddTHH:mm:ssK";
		protected const string TIMESTAMP = @"HH:mm:ss.fff";
		private QBSessionManager _sessionManager;
		private bool _connectionOpen;
		private bool _sessionBegun;
		private string _appName;
		public LogLevel _logLevel;
		#region public enum QueryType {
		public enum QueryType { Bill = 0, ItemNonInventory = 1, };
		#endregion public enum QueryType
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

		#region Constructors
		[Obsolete("Deprecated. This is only here for legacy support. Should NOT be used for new development.")]
		public Qbfc13Util()
		{
			_logLevel = LogLevel.Info;
			_sessionManager = null;
			_connectionOpen = false;
			_sessionBegun = false;
			_appName = "Yutaka.Qbfc13Util";
		}

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
		#endregion Constructors

		private XmlElement MakeSimpleElem(XmlDocument doc, string tagName, string tagVal)
		{
			var elem = doc.CreateElement(tagName);
			elem.InnerText = tagVal;
			return elem;
		}

		public XmlDocument BuildQueryRequest(QueryType queryType, DateTime? fromDate = null, DateTime? toDate = null)
		{
			var now = DateTime.Now;
			var minDate = now.AddYears(-10);
			var maxDate = now.AddYears(1);

			#region Input Validation
			if (fromDate == null || fromDate < minDate || maxDate < fromDate)
				fromDate = minDate;
			if (toDate == null || toDate < minDate || maxDate < toDate)
				toDate = maxDate;
			#endregion Input Validation

			try {
				string elem1Name, elem2Name;
				var doc = new XmlDocument();

				// Add the prolog processing instructions
				doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));
				doc.AppendChild(doc.CreateProcessingInstruction("qbxml", "version=\"13.0\""));

				// Create the outer request envelope tag
				var outer = doc.CreateElement("QBXML");
				doc.AppendChild(outer);

				// Create the inner request envelope & any needed attributes
				var inner = doc.CreateElement("QBXMLMsgsRq");
				outer.AppendChild(inner);
				inner.SetAttribute("onError", "stopOnError");

				switch (queryType) {
					case QueryType.Bill:
						elem1Name = "BillQueryRq";
						elem2Name = "ModifiedDateRangeFilter";
						break;
					case QueryType.ItemNonInventory:
						elem1Name = "ItemNonInventoryQueryRq";
						elem2Name = "";
						break;
					default:
						elem1Name = "";
						elem2Name = "";
						break;
				}

				var ItemQueryRq = doc.CreateElement(elem1Name);
				inner.AppendChild(ItemQueryRq);

				if (String.IsNullOrWhiteSpace(elem2Name)) {
					ItemQueryRq.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", fromDate.Value.ToString(QB_FORMAT)));
					ItemQueryRq.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", toDate.Value.ToString(QB_FORMAT)));
				}

				else {
					var ModifiedDateRangeFilter = doc.CreateElement(elem2Name);
					ItemQueryRq.AppendChild(ModifiedDateRangeFilter);
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "FromModifiedDate", fromDate.Value.ToString(QB_FORMAT)));
					ModifiedDateRangeFilter.AppendChild(MakeSimpleElem(doc, "ToModifiedDate", toDate.Value.ToString(QB_FORMAT)));
					ItemQueryRq.AppendChild(MakeSimpleElem(doc, "IncludeLineItems", "1"));
				}

				return doc;
			}

			catch (Exception ex) {
				string msg;

				if (ex.InnerException == null)
					msg = String.Format("{0}{2}Exception thrown in Qbfc13Util.BuildQueryRequest(QueryType queryType={3}, DateTime? fromDate='{4}', DateTime? toDate='{5}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, queryType, fromDate, toDate);
				else
					msg = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Qbfc13Util.BuildQueryRequest(QueryType queryType={3}, DateTime? fromDate='{4}', DateTime? toDate='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, queryType, fromDate, toDate);

				if (_logLevel <= LogLevel.Error)
					Console.Write("\n[{0}] {1}", DateTime.Now.ToString(TIMESTAMP), msg);

				throw new Exception(msg);
			}
		}

		public string DateTimeToQbFormat(DateTime? dt)
		{
			if (dt == null)
				return "";

			return dt.Value.ToString(QB_FORMAT);
		} 

		public ItemNonInventoryRet XmlNodeToItemNonInventoryRet(XmlNode xml)
		{
			if (_logLevel <= LogLevel.Trace)
				Console.Write("\n[{0}] Begin method XmlNodeToItemNonInventoryRet(XmlNode xml).", DateTime.Now.ToString(TIMESTAMP));
			if (xml == null || !xml.HasChildNodes)
				throw new Exception("<xml> is required. Exception thrown in Qbfc13Util.XmlNodeToItemNonInventoryRet(XmlNode xml).{0}{0}");

			try {
				XmlNode node, subNode;
				var classRef = new ClassRef();
				var parentRef = new ParentRef();
				var unitOfMeasureSetRef = new UnitOfMeasureSetRef();
				var salesTaxCodeRef = new SalesTaxCodeRef();
				var salesOrPurchase = new SalesOrPurchase();
				var salesAndPurchase = new SalesAndPurchase();

				#region Get Subnodes
				node = xml.SelectSingleNode("ClassRef");

				if (node != null) {
					classRef.ListID = node.SelectSingleNode("ListID") == null ? "" : node.SelectSingleNode("ListID").InnerText;
					classRef.FullName = node.SelectSingleNode("FullName") == null ? "" : node.SelectSingleNode("FullName").InnerText;
				}

				node = xml.SelectSingleNode("ParentRef");

				if (node != null) {
					parentRef.ListID = node.SelectSingleNode("ListID") == null ? "" : node.SelectSingleNode("ListID").InnerText;
					parentRef.FullName = node.SelectSingleNode("FullName") == null ? "" : node.SelectSingleNode("FullName").InnerText;
				}

				node = xml.SelectSingleNode("UnitOfMeasureSetRef");

				if (node != null) {
					unitOfMeasureSetRef.ListID = node.SelectSingleNode("ListID") == null ? "" : node.SelectSingleNode("ListID").InnerText;
					unitOfMeasureSetRef.FullName = node.SelectSingleNode("FullName") == null ? "" : node.SelectSingleNode("FullName").InnerText;
				}

				node = xml.SelectSingleNode("SalesTaxCodeRef");

				if (node != null) {
					salesTaxCodeRef.ListID = node.SelectSingleNode("ListID") == null ? "" : node.SelectSingleNode("ListID").InnerText;
					salesTaxCodeRef.FullName = node.SelectSingleNode("FullName") == null ? "" : node.SelectSingleNode("FullName").InnerText;
				}

				node = xml.SelectSingleNode("SalesOrPurchase");

				if (node != null) {
					salesOrPurchase.Desc = node.SelectSingleNode("Desc") == null ? "" : node.SelectSingleNode("Desc").InnerText;
					salesOrPurchase.Price = node.SelectSingleNode("Price") == null ? (decimal?) null : decimal.Parse(node.SelectSingleNode("Price").InnerText);
					salesOrPurchase.PricePercent = node.SelectSingleNode("PricePercent") == null ? (decimal?) null : decimal.Parse(node.SelectSingleNode("PricePercent").InnerText);
					subNode = node.SelectSingleNode("AccountRef");

					if (subNode != null) {
						salesOrPurchase.AccountRef.ListID = subNode.SelectSingleNode("ListID") == null ? "" : subNode.SelectSingleNode("ListID").InnerText;
						salesOrPurchase.AccountRef.FullName = subNode.SelectSingleNode("FullName") == null ? "" : subNode.SelectSingleNode("FullName").InnerText;
					}
				}

				node = xml.SelectSingleNode("SalesAndPurchase");

				if (node != null) {
					salesAndPurchase.SalesDesc = node.SelectSingleNode("SalesDesc") == null ? "" : node.SelectSingleNode("SalesDesc").InnerText;
					salesAndPurchase.SalesPrice = node.SelectSingleNode("SalesPrice") == null ? (decimal?) null : decimal.Parse(node.SelectSingleNode("SalesPrice").InnerText);
					salesAndPurchase.PurchaseDesc = node.SelectSingleNode("PurchaseDesc") == null ? "" : node.SelectSingleNode("PurchaseDesc").InnerText;
					salesAndPurchase.PurchaseCost = node.SelectSingleNode("PurchaseCost") == null ? (decimal?) null : decimal.Parse(node.SelectSingleNode("PurchaseCost").InnerText);
					subNode = node.SelectSingleNode("IncomeAccountRef");

					if (subNode != null) {
						salesAndPurchase.IncomeAccountRef.ListID = subNode.SelectSingleNode("ListID") == null ? "" : subNode.SelectSingleNode("ListID").InnerText;
						salesAndPurchase.IncomeAccountRef.FullName = subNode.SelectSingleNode("FullName") == null ? "" : subNode.SelectSingleNode("FullName").InnerText;
					}

					subNode = node.SelectSingleNode("PurchaseTaxCodeRef");

					if (subNode != null) {
						salesAndPurchase.PurchaseTaxCodeRef.ListID = subNode.SelectSingleNode("ListID") == null ? "" : subNode.SelectSingleNode("ListID").InnerText;
						salesAndPurchase.PurchaseTaxCodeRef.FullName = subNode.SelectSingleNode("FullName") == null ? "" : subNode.SelectSingleNode("FullName").InnerText;
					}

					subNode = node.SelectSingleNode("ExpenseAccountRef");

					if (subNode != null) {
						salesAndPurchase.ExpenseAccountRef.ListID = subNode.SelectSingleNode("ListID") == null ? "" : subNode.SelectSingleNode("ListID").InnerText;
						salesAndPurchase.ExpenseAccountRef.FullName = subNode.SelectSingleNode("FullName") == null ? "" : subNode.SelectSingleNode("FullName").InnerText;
					}

					subNode = node.SelectSingleNode("PrefVendorRef");

					if (subNode != null) {
						salesAndPurchase.PrefVendorRef.ListID = subNode.SelectSingleNode("ListID") == null ? "" : subNode.SelectSingleNode("ListID").InnerText;
						salesAndPurchase.PrefVendorRef.FullName = subNode.SelectSingleNode("FullName") == null ? "" : subNode.SelectSingleNode("FullName").InnerText;
					}
				}
				#endregion Get Subnodes

				return new ItemNonInventoryRet {
					ListID = xml.SelectNodes("ListID")[0] == null ? "" : xml.SelectNodes("ListID")[0].InnerText,
					TimeCreated = DateTime.Parse(xml.SelectNodes("TimeCreated")[0].InnerText),
					TimeModified = DateTime.Parse(xml.SelectNodes("TimeModified")[0].InnerText),
					EditSequence = xml.SelectNodes("EditSequence")[0] == null ? "" : xml.SelectNodes("EditSequence")[0].InnerText,
					Name = xml.SelectNodes("Name")[0] == null ? "" : xml.SelectNodes("Name")[0].InnerText,
					FullName = xml.SelectNodes("FullName")[0] == null ? "" : xml.SelectNodes("FullName")[0].InnerText,
					BarCodeValue = xml.SelectNodes("BarCodeValue")[0] == null ? "" : xml.SelectNodes("BarCodeValue")[0].InnerText,
					IsActive = xml.SelectNodes("IsActive")[0] == null ? (bool?) null : bool.Parse(xml.SelectNodes("IsActive")[0].InnerText),
					ClassRef = classRef,
					ParentRef = parentRef,
					Sublevel = xml.SelectNodes("Sublevel")[0] == null ? (int?) null : int.Parse(xml.SelectNodes("Sublevel")[0].InnerText),
					ManufacturerPartNumber = xml.SelectNodes("ManufacturerPartNumber")[0] == null ? "" : xml.SelectNodes("ManufacturerPartNumber")[0].InnerText,
					UnitOfMeasureSetRef = unitOfMeasureSetRef,
					IsTaxIncluded = xml.SelectNodes("IsTaxIncluded")[0] == null ? (bool?) null : bool.Parse(xml.SelectNodes("IsTaxIncluded")[0].InnerText),
					SalesTaxCodeRef = salesTaxCodeRef,
					SalesOrPurchase = salesOrPurchase,
					SalesAndPurchase = salesAndPurchase,
					ExternalGUID = xml.SelectNodes("ExternalGUID")[0] == null ? "" : xml.SelectNodes("ExternalGUID")[0].InnerText,
				};
			}

			catch (Exception ex) {
				string msg;

				if (ex.InnerException == null)
					msg = String.Format("{0}{2}Exception thrown in Qbfc13Util.XmlNodeToItemNonInventoryRet(XmlNode xml).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					msg = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Qbfc13Util.XmlNodeToItemNonInventoryRet(XmlNode xml).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);

				if (_logLevel <= LogLevel.Error)
					Console.Write("\n[{0}] {1}", DateTime.Now.ToString(TIMESTAMP), msg);

				throw new Exception(msg);
			}
		}
	}
}