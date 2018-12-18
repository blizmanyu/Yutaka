using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using NLog;
using Yutaka.Images;
using Yutaka.IO;
using Yutaka.Text;
using Yutaka.Utils;
using Yutaka.Web;
using System.Numerics;
using System.Net.Mail;

namespace Yutaka.Tests
{
	static class Program
	{
		#region Fields
		#region Static Externs
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		const int SW_HIDE = 0;
		#endregion

		// Constants //
		const string PROGRAM_NAME = "Yutaka.Tests";
		const string TIMESTAMP = @"[HH:mm:ss] ";

		// PIVs //
		private static DateTime startTime = DateTime.Now;
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static bool consoleOut = true; // default = false //
		private static int errorCount = 0;
		private static int totalCount = 0;
		private static int errorCountThreshold = 7;
		private static double errorPerThreshold = 0.07;
		private static List<string> bots = new List<string> { "bot","crawler","spider","80legs","baidu","yahoo! slurp","ia_archiver","mediapartners-google","lwp-trivial","nederland.zoek","ahoy","anthill","appie","arale","araneo","ariadne","atn_worldwide","atomz","bjaaland","ukonline","calif","combine","cosmos","cusco","cyberspyder","digger","grabber","downloadexpress","ecollector","ebiness","esculapio","esther","felix ide","hamahakki","kit-fireball","fouineur","freecrawl","desertrealm","gcreep","golem","griffon","gromit","gulliver","gulper","whowhere","havindex","hotwired","htdig","ingrid","informant","inspectorwww","iron33","teoma","ask jeeves","jeeves","image.kapsi.net","kdd-explorer","label-grabber","larbin","linkidator","linkwalker","lockon","marvin","mattie","mediafox","merzscope","nec-meshexplorer","udmsearch","moget","motor","muncher","muninn","muscatferret","mwdsearch","sharp-info-agent","webmechanic","netscoop","newscan-online","objectssearch","orbsearch","packrat","pageboy","parasite","patric","pegasus","phpdig","piltdownman","pimptrain","plumtreewebaccessor","getterrobo-plus","raven","roadrunner","robbie","robocrawl","robofox","webbandit","scooter","search-au","searchprocess","senrigan","shagseeker","site valet","skymob","slurp","snooper","speedy","curl_image_client","suke","www.sygol.com","tach_bw","templeton","titin","topiclink","udmsearch","urlck","valkyrie libwww-perl","verticrawl","victoria","webscout","voyager","crawlpaper","webcatcher","t-h-u-n-d-e-r-s-t-o-n-e","webmoose","pagesinventory","webquest", "weborama","fetcher","webreaper","webwalker","winona","occam","robi","fdse","jobo","rhcs","gazz","dwcp","yeti","fido","wlm","wolp","wwwc","xget","legs","curl","webs","wget","sift","cmc" };
		#endregion

		static void Main(string[] args)
		{
			StartProgram();
			Test_MailAddress();
			EndProgram();
		}

		#region Test MailAddress
		private static void Test_MailAddress()
		{
			var tests = new string[] {
				"undisclosed recipients",
			};

			MailAddress address;

			for (int i = 0; i < tests.Length; i++) {
				address = new MailAddress(tests[i]);
				Console.Write("\n");
				Console.Write("\nTest #{0}: {1}", i + 1, tests[i]);
				Console.Write("\n=> {0}", address.Address);
			}
		}
		#endregion Test FileUtil.FixCreationTime

		#region Test FileUtil.FixCreationTime
		private static void Test_FileUtil_FixCreationTime()
		{
			var searchPattern = @"*";
			var tests = new string[] {
				@"D:\RCW_Imports",
				@"D:\RCW_Imports\03\RCW_Imports\RCW_Imports\bin\Debug\",
			};

			for (int i = 0; i < tests.Length; i++) {
				Console.Write("\n");
				Console.Write("\nTest #{0}: {1}", i+1, tests[i]);
				FileUtil.FixCreationTime(tests[i], searchPattern);
			}
		}
		#endregion Test FileUtil.FixCreationTime

		#region Test Base36.EncodeIp/DecodeIp
		private static void Test_Base36_EncodeIp_DecodeIp()
		{
			string output;

			var tests = new string[] {
				"0.0.0.1",
				"1.1.1.1",
				"98.189.176.208",
				"255.255.255.255",
			};

			for (int i = 0; i < tests.Length; i++) {
				output = Base36.EncodeIp(tests[i]);
				Console.Write("\n");
				Console.Write("\nInput: {0}", tests[i]);
				Console.Write("\nEncoded: {0}", output);
				Console.Write("\nDecoded: {0}", Base36.DecodeIp(output));
			}
		}
		#endregion Test Base36.EncodeIp/DecodeIp

		#region Test MvcUtil.IsBot()
		//private static void Test_MvcUtil_IsBot()
		//{
		//	var tests = new string[] {
		//		@"Mozilla/5.0 (Windows NT 6.3; WOW64; rv:36.0) Gecko/20100101 Firefox/36.0 (NetShelter ContentScan, contact abuse@inpwrd.com for information)",
		//		@"panscient.com",
		//		@"mixnode.com",
		//		@"Mozilla/5.0 (compatible; Dataprovider.com;)",
		//		@"Barkrowler/0.5.1 (experimenting / debugging - sorry for your logs ) www.exensa.com/crawl - admin@exensa.com -- based on BUBing",
		//		@"Mozilla/5.0 (compatible; um-LN/1.0; mailto: techinfo@ubermetrics-technologies.com)",
		//		@"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0; Avant Browser [avantbrowser.com]; Hotbar 4.4.5.0)",
		//		@"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; TheFreeDictionary.com; .NET CLR 1.1.4322; .NET CLR 1.0.3705; .NET CLR 2.0.50727)",
		//		@"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; MAXTHON 2.0); Connect Us: webauth@cmcm.com",
		//		@"Barkrowler/0.4.8 (experimenting / debugging - sorry for your logs ) www.exensa.com/crawl - admin@exensa.com -- based on BUBing",
		//		@"dj-research/Nutch-1.11 (analytics@@demandjump.com)",
		//		@"Mozilla/5.0 (X11; U; Linux x86; en-US) adbeat.com/policy Gecko/20100423 Ubuntu/10.04 (lucid) Firefox/3.6.3 AppleWebKit/532.4 Safari/532.4",
		//		@"Mozilla/5.0 (compatible; um-LN/1.0; mailto: techinfo@ubermetrics-technologies.com; Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.1",
		//		@"Mozilla/5.0 (Windows NT 5.1; rv:11.0) Gecko Firefox/11.0 (via ggpht.com GoogleImageProxy)",
		//		@"Mozilla/5.0 (X11; Linux x86_64) adbeat.com/policy AppleWebKit/537.21 (KHTML, like Gecko) Firefox/34.0 Safari/537.21",
		//		@"Mozilla/5.0 (compatible; Dataprovider.com)",
		//		@"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.86 Safari/537.36 Scanning for research (researchscan.comsys.rwth-aachen.de)",
		//		@"Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3 like Mac OS X; en-US) adbeat.com/policy AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8F190 Safari/6533.18.5",
		//		@"Mozilla/5.0 (X11; Linux x86_64; rv:10.0.12) Gecko/20100101 Firefox/21.0 WordPress.com mShots",
		//		@"izsearch.com",
		//		@"VSE/1.0 (rabraham@multiview.com)",
		//		@"Mozilla/5.0 (X11; U; Linux x86; es-ES) adbeat.com/policy Gecko/20100423 Ubuntu/10.04 (lucid) Firefox/3.6.3 AppleWebKit/532.4 Safari/532.4",
		//		@"Icarus6j - (contact: phil@icarus6.com)",
		//		@"Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0) LinkCheck by Siteimprove.com",
		//		@"Mozilla/5.0 (Windows NT 10.0; WOW64; %lang_code%) adbeat.com/policy AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.116 Safari/537.36",
		//		@"Mozilla/5.0 (Windows NT 6.1; rv:12.0; StumbleUpon; noc@stumbleupon.com) Gecko/20100101 Firefox/12.0",
		//		@"Scanning for research (researchscan.comsys.rwth-aachen.de)",
		//		@"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_6) AppleWebKit/534.24 (KHTML, like Gecko) (Contact: backend@getprismatic.com)",
		//		@"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) WordPress.com mShots Safari/537.36",
		//		@"Mozilla/5.0 Project 25499 (project25499.com)",
		//		@"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/67.0.3372.0 Safari/537.36 WordPress.com mShots",
		//		@"SiteTruth.com site rating system",
		//	};

		//	for (int i = 0; i < tests.Length; i++) {
		//		Console.Write("\n");
		//		Console.Write("\nInput: {0}", tests[i]);
		//		Console.Write("\nOutput: {0}", MvcUtil.IsBot(tests[i]));
		//	}
		//}
		#endregion Test Base36.GetUniqueIdByEmail()

		#region Campaign Key Generator
		private static void CampaignKeyGenerator()
		{
		}
		#endregion Campaign Key Generator

		#region Test Base36.Encode/Decode()
		private static void Test_Base36_Decode()
		{
			Console.Write("\nOutput: {0}", Base36.Decode("a0a0"));
		}

		private static void Test_Base36_Encode()
		{
			for (int i = 466920; i < 467220; i += 3) {
				Console.Write("\n");
				Console.Write("\nInput: {0}", i);
				Console.Write("\nOutput: {0}", Base36.Encode(i));
			}
		}
		#endregion Test Base36.GetUniqueIdByEmail()

		#region Test Base36.GetUniqueIdByEmail()
		private static void Test_Base36_GetUniqueIdByEmail()
		{
			var tests = new string[] {
				@"yblizman@rcw1.com",
				@" ! ! ! ! ! ! ! ! ",
			};

			for (int i = 0; i < tests.Length; i++) {
				Console.Write("\n");
				Console.Write("\nInput: {0}", tests[i]);
				Console.Write("\nOutput: {0}", Base36.GetUniqueIdByEmail(tests[i]));
			}
		}
		#endregion Test Base36.GetUniqueIdByEmail()

		//#region Test FileUtil.Write()
		//private static void Test_FileUtil_Write()
		//{
		//	FileUtil.Write("delete me 2018 1023 0122", @"C:\TEMP\_DeleteMe 2018 1023 0122.txt");
		//}
		//#endregion Test FileUtil.Write()

		#region Test FileUtil.GetDirectorySize()
		private static void Test_FileUtil_GetDirectorySize()
		{
			var tests = new string[] {
				@"D:\_DeleteAfter2018_0822\Private\IT\backups\RCW_Imports(dev)\",
				@"D:\_DeleteAfter2018_0822\Public\Jeff's Ads\IT\cdrive clement\RCW_Imports\",
				@"D:\Departments\DevIT\Projects\RCW_Imports\",
			};

			for (int i = 0; i < tests.Length; i++) {
				Console.Write("\n\nTest: {0}\n  DirectorySize: {1}", tests[i], FileUtil.GetDirectorySize(tests[i]));
			}
		}
		#endregion Test FileUtil.GetDirectorySize()

		#region Test ImageUtil.ExistsAndValidByUrl()
		static void Test_ImageUtil_ExistsAndValidByUrl()
		{
			var tests = new string[] {
				"http://www.rarecoinwholesalers.com/Content/Images/Coins/134917fcs.jpg",
				"http://www.rarecoinwholesalers.com/Content/Images/Coins/134917fcs.JPEG",
				"http://www.rarecoinwholesalers.com/Content/Images/Coins/134917fcs.asdf",
			};

			for (int i = 0; i < tests.Length; i++) {
				Console.Write("\n\nTest: {0}\n  ExistsAndValidByUrl: {1}", tests[i], ImageUtil.ExistsAndValidByUrl(tests[i]));
			}
		}
		#endregion Test ImageUtil.ExistsAndValidByUrl()

		#region Methods
		static void StartProgram()
		{
			var log = String.Format("Starting {0} program", PROGRAM_NAME);
			logger.Info(log);

			if (consoleOut) {
				Console.Clear();
				Console.Write("{0}{1}", DateTime.Now.ToString(TIMESTAMP), log);
			}

			else {
				var handle = GetConsoleWindow();
				ShowWindow(handle, SW_HIDE); // hide window //
			}
		}

		static void EndProgram()
		{
			var endTime = DateTime.Now;
			var ts = endTime - startTime;
			var errorPer = (double) errorCount/totalCount;

			if (errorCount > errorCountThreshold && errorPer > errorPerThreshold)
				logger.Error("The number of errors is above the threshold.");

			var log = new string[4];
			log[0] = "Ending program";
			log[1] = String.Format("It took {0} to complete", ts.ToString(@"hh\:mm\:ss\.fff"));
			log[2] = String.Format("Total: {0}", totalCount);
			log[3] = String.Format("Errors: {0} ({1}){2}", errorCount, errorPer.ToString("P"), Environment.NewLine + Environment.NewLine);

			for (int i=0; i<log.Length; i++) {
				logger.Info(log[i]);
			}

			if (consoleOut) {
				Console.Write("\n");
				for (int i = 0; i < log.Length; i++) {
					Console.Write("\n{0}{1}", DateTime.Now.ToString(TIMESTAMP), log[i]);
				}
				Console.Write("\n.... Press any key to close the program ....");
				Console.ReadKey(true);
			}

			Environment.Exit(0); // in case you want to call this method outside of a standard successful program completion, this line will close the app //
		}
		#endregion
	}
}