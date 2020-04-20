using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
//using Nop.Web.Models.RcwInventory;
//using Rcw.Sql.Hulk.Import.Data;
//using Rcw.Sql.Hulk.IntranetData.Data;
using Yutaka.Data;
using Yutaka.IO;
using Yutaka.Text;
using Yutaka.VineSpring.Data20200207;

namespace CodeGenerator
{
	class Program
	{
		// Config/Settings //
		const string PROGRAM_NAME = "CodeGenerator";
		private static readonly bool consoleOut = true; // default = false //

		#region Fields
		#region Static Externs
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		const int SW_HIDE = 0;
		#endregion

		const string TIMESTAMP = @"[HH:mm:ss] ";
		private static readonly DateTime startTime = DateTime.Now;
		private static readonly double errorPerThreshold = 0.07;
		private static readonly int errorCountThreshold = 7;
		private static readonly string DestFolder = @"C:\TEMP\";

		private static int errorCount = 0;
		private static int totalCount = 0;
		private static List<Column> Columns = new List<Column>();
		private static FileUtil _fileUtil = new FileUtil();
		private static TsqlUtil _tsqlUtil;
		#endregion

		static void Main(string[] args)
		{
			StartProgram();
			ScriptTables();
			EndProgram();
		}

		private static void ScriptTables()
		{
			var dest = Path.Combine(DestFolder, String.Format("{0:yyyy MMdd HHmm ssff}.", DateTime.Now));
			_fileUtil.Write(CSUtil.GenerateGetById("Chatter"), String.Format("{0}cs", dest));
			return;
			_tsqlUtil = new TsqlUtil("Yutaka Blizman");
			GetColumnsInformation();
			_fileUtil.Write(_tsqlUtil.ScriptTableCreateViewList(Columns), String.Format("{0}sql", dest));
			_fileUtil.Write(_tsqlUtil.ScriptTableCreateViewEdit(Columns), String.Format("{0}sql", dest));
			_fileUtil.Write(_tsqlUtil.ScriptTableInsert(Columns), String.Format("{0}sql", dest));
			_fileUtil.Write(_tsqlUtil.ScriptTableUpdate(Columns), String.Format("{0}sql", dest));
			_fileUtil.Write(_tsqlUtil.ScriptTableDelete(Columns), String.Format("{0}sql", dest));
			_fileUtil.Write(_tsqlUtil.ScriptTableRestore(Columns), String.Format("{0}sql", dest));
		}

		private static void GetColumnsInformation()
		{
			var connectionString = "asdfg";
			var database = "asdfg";
			var schema = "";
			var table = "_Example";

			Columns = _tsqlUtil.GetColumnsInformation(connectionString, database, schema, table).ToList();

			if (consoleOut) {
				foreach (var col in Columns)
					col.DumpToConsole();
			}
		}

		private static void MapEntities()
		{
			var sb = new StringBuilder();
			//sb.Append(Mapper.Map<InventoryAabModel>("InventoryAabView"));
			sb.Append(Mapper.Map<CustomerDefaultAddress>("CustomerDefaultAddress"));
			sb.Append(Mapper.Map<CustomerName>("CustomerName"));
			//sb.Append(Mapper.Map<Order_Customer_BillingAddress>("VSData.Order_Customer_BillingAddress"));
			//sb.Append(Mapper.Map<Order_Customer_Name>("VSData.Order_Customer_Name"));
			//sb.Append(Mapper.Map<Order_Discount>("VSData.Order_Discount"));
			//sb.Append(Mapper.Map<Order_Item>("VSData.Order_Item"));
			//sb.Append(Mapper.Map<Order_Note>("VSData.Order_Note"));
			//sb.Append(Mapper.Map<Order_ShippingAddress>("VSData.Order_ShippingAddress"));
			//sb.Append(Mapper.Map<Order_ShippingMethod>("VSData.Order_ShippingMethod"));
			//sb.Append(Mapper.Map<Order_ShippingMethod_AlternateAddress>("VSData.Order_ShippingMethod_AlternateAddress"));

			var filename = String.Format("{0:yyyy MMdd HHmm ssff}.cs", DateTime.Now);
			new FileUtil().Write(sb, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), filename));
		}

		private static void StartProgram()
		{
			var log = String.Format("Starting {0} program", PROGRAM_NAME);

			if (consoleOut) {
				Console.Clear();
				Console.Write("{0}{1}", DateTime.Now.ToString(TIMESTAMP), log);
			}

			else {
				var handle = GetConsoleWindow();
				ShowWindow(handle, SW_HIDE); // hide window //
			}
		}

		private static void EndProgram()
		{
			var endTime = DateTime.Now;
			var ts = endTime - startTime;
			var errorPer = (double) errorCount/totalCount;

			if (errorCount > errorCountThreshold || errorPer > errorPerThreshold) {
				if (errorCount > errorCountThreshold && errorPer > errorPerThreshold) {
					//MailUtil.Send("fromEmail", "fromEmail", PROGRAM_NAME, String.Format("Errors: {0} ({1})", errorCount, errorPer.ToString("P")));
				}
			}

			var log = new string[4];
			log[0] = "Ending program";
			log[1] = String.Format("It took {0} to complete", ts.ToString(@"hh\:mm\:ss\.fff"));
			log[2] = String.Format("Total: {0}", totalCount);
			log[3] = String.Format("Errors: {0} ({1}){2}", errorCount, errorPer.ToString("P"), Environment.NewLine + Environment.NewLine);

			if (consoleOut) {
				var timestamp = DateTime.Now.ToString(TIMESTAMP);
				Console.Write("\n");
				Console.Write("\n{0}{1}", timestamp, log[0]);
				Console.Write("\n{0}{1}", timestamp, log[1]);
				Console.Write("\n{0}{1}", timestamp, log[2]);
				Console.Write("\n{0}{1}", timestamp, log[3]);
				Console.Write("\n.... Press any key to close the program ....");
				Console.ReadKey(true);
			}

			Environment.Exit(0); // in case you want to call this method outside of a standard successful program completion, this line will close the app //
		}
	}
}