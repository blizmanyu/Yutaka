using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Yutaka.Data;
using Yutaka.Images;
using Yutaka.IO;
using Yutaka.Net;
using Yutaka.QuickBooks;
using Yutaka.Text;
using Yutaka.Utils;
using Yutaka.Video;
using Yutaka.Web;

namespace Yutaka.Tests
{
	static class Program
	{
		private static bool consoleOut = true; // default = false //

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
		private static readonly Guid CLSID_Shell = Guid.Parse("13709620-C279-11CE-A49E-444553540000");
		private static DateTime startTime = DateTime.Now;
		private static FileUtil _fileUtil = new FileUtil();
		private static List<string> bots = new List<string> { "bot", "crawler", "spider", "80legs", "baidu", "yahoo! slurp", "ia_archiver", "mediapartners-google", "lwp-trivial", "nederland.zoek", "ahoy", "anthill", "appie", "arale", "araneo", "ariadne", "atn_worldwide", "atomz", "bjaaland", "ukonline", "calif", "combine", "cosmos", "cusco", "cyberspyder", "digger", "grabber", "downloadexpress", "ecollector", "ebiness", "esculapio", "esther", "felix ide", "hamahakki", "kit-fireball", "fouineur", "freecrawl", "desertrealm", "gcreep", "golem", "griffon", "gromit", "gulliver", "gulper", "whowhere", "havindex", "hotwired", "htdig", "ingrid", "informant", "inspectorwww", "iron33", "teoma", "ask jeeves", "jeeves", "image.kapsi.net", "kdd-explorer", "label-grabber", "larbin", "linkidator", "linkwalker", "lockon", "marvin", "mattie", "mediafox", "merzscope", "nec-meshexplorer", "udmsearch", "moget", "motor", "muncher", "muninn", "muscatferret", "mwdsearch", "sharp-info-agent", "webmechanic", "netscoop", "newscan-online", "objectssearch", "orbsearch", "packrat", "pageboy", "parasite", "patric", "pegasus", "phpdig", "piltdownman", "pimptrain", "plumtreewebaccessor", "getterrobo-plus", "raven", "roadrunner", "robbie", "robocrawl", "robofox", "webbandit", "scooter", "search-au", "searchprocess", "senrigan", "shagseeker", "site valet", "skymob", "slurp", "snooper", "speedy", "curl_image_client", "suke", "www.sygol.com", "tach_bw", "templeton", "titin", "topiclink", "udmsearch", "urlck", "valkyrie libwww-perl", "verticrawl", "victoria", "webscout", "voyager", "crawlpaper", "webcatcher", "t-h-u-n-d-e-r-s-t-o-n-e", "webmoose", "pagesinventory", "webquest", "weborama", "fetcher", "webreaper", "webwalker", "winona", "occam", "robi", "fdse", "jobo", "rhcs", "gazz", "dwcp", "yeti", "fido", "wlm", "wolp", "wwwc", "xget", "legs", "curl", "webs", "wget", "sift", "cmc" };
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static MailUtil _mailUtil = new MailUtil();
		private static SqlUtil _sqlUtil = new SqlUtil();
		private static Stopwatch stopwatch = new Stopwatch();
		private static WebUtil _webUtil = new WebUtil();
		private static int errorCount = 0;
		private static int totalCount = 0;
		private static int errorCountThreshold = 7;
		private static double errorPerThreshold = 0.07;
		#region private static string[][] Apps = new string[][] {
		private static string[][] Apps = new string[][] {
			new string[] { "Messenger", @"Apps\Messenger\", },
			new string[] { "Cash App", @"Apps\Cash App\", },
			new string[] { "Facebook", @"Apps\Facebook\", },
			new string[] { "Messages", @"Apps\Messages\", },
			new string[] { "WhatsApp", @"Apps\WhatsApp\", },
			new string[] { "Bitmoji", @"Apps\Bitmoji\", },
			new string[] { "CashApp", @"Apps\Cash App\", },
			new string[] { "Netflix", @"Apps\Netflix\", },
			new string[] { "Pandora", @"Apps\Pandora\", },
			new string[] { "Samsung", @"Apps\Samsung\", },
			new string[] { "Spotify", @"Apps\Spotify\", },
			new string[] { "Twitter", @"Apps\Twitter\", },
			new string[] { "YouTube", @"Apps\YouTube\", },
			new string[] { "Amazon", @"Apps\Amazon\", },
			new string[] { "Chrome", @"Apps\Chrome\", },
			new string[] { "TikTok", @"Apps\TikTok\", },
			new string[] { "Bixby", @"Apps\Bixby\", },
			new string[] { "Gmail", @"Apps\Gmail\", },
			new string[] { "Sleep", @"Apps\Sleep\", },
			new string[] { "Line", @"Apps\Line\", },
			new string[] { "Maps", @"Apps\Maps\", },
			new string[] { "Turo", @"Apps\Turo\", },
			new string[] { "Uber", @"Apps\Uber\", },
			new string[] { "Consumer Reports", @"Documents\Consumer Reports\", },
			new string[] { "ConsumerReports", @"Documents\Consumer Reports\", },
			new string[] { "United Airlines", @"Documents\United Airlines\", },
			new string[] { "UnitedAirlines", @"Documents\United Airlines\", },
			new string[] { "Womens Health", @"Documents\Womens Health\", },
			new string[] { "Confirmation", @"Documents\Receipts\", },
			new string[] { "Registration", @"Documents\Receipts\", },
			new string[] { "WomensHealth", @"Documents\Womens Health\", },
			new string[] { "Itineraries", @"Documents\Receipts\", },
			new string[] { "Mens Health", @"Documents\Mens Health\", },
			new string[] { "Green Card", @"Documents\Green Card\", },
			new string[] { "Maximum PC", @"Documents\Maximum PC\", },
			new string[] { "MensHealth", @"Documents\Mens Health\", },
			new string[] { "GreenCard", @"Documents\GreenCard\", },
			new string[] { "Itinerary", @"Documents\Receipts\", },
			new string[] { "MaximumPC", @"Documents\Maximum PC\", },
			new string[] { "Thank You", @"Documents\Receipts\", },
			new string[] { "Checkout", @"Documents\Receipts\", },
			new string[] { "Invoices", @"Documents\Invoices\", },
			new string[] { "Passport", @"Documents\Passport\", },
			new string[] { "PC Gamer", @"Documents\PC Gamer\", },
			new string[] { "Receipts", @"Documents\Receipts\", },
			new string[] { "ThankYou", @"Documents\Receipts\", },
			new string[] { "Invoice", @"Documents\Invoices\", },
			new string[] { "License", @"Documents\License\", },
			new string[] { "PCGamer", @"Documents\PC Gamer\", },
			new string[] { "Receipt", @"Documents\Receipts\", },
			new string[] { "Welcome", @"Documents\Receipts\", },
			new string[] { "Thanks", @"Documents\Receipts\", },
			new string[] { "Chase", @"Documents\Chase\", },
			new string[] { "Delta", @"Documents\Delta\", },
			new string[] { "Maxim", @"Documents\Maxim\", },
			new string[] { "Scans", @"Documents\Scans\", },
			new string[] { "ETNT", @"Documents\ETNT\", },
			new string[] { "FICO", @"Documents\FICO\", },
			new string[] { "Ikea", @"Documents\Ikea\", },
			new string[] { "Scan", @"Documents\Scans\", },
			new string[] { "Car", @"Documents\Car\", },
			new string[] { "GQ", @"Documents\GQ\", },
			new string[] { "Clash of Clans", @"Games\Clash of Clans\", },
			new string[] { "Clash Royale", @"Games\Clash Royale\", },
			new string[] { "ClashOfClans", @"Games\Clash of Clans\", },
			new string[] { "ClashRoyale", @"Games\Clash Royale\", },
			new string[] { "Castle Age", @"Games\Castle Age\", },
			new string[] { "CastleAge", @"Games\Castle Age\", },
			new string[] { "Fantasica", @"Games\Fantasica\", },
			new string[] { "zMe", @"zMe\", },
			new string[] { "Me", @"zMe\", },
			new string[] { "Philips Hue", @"Philips Hue\", },
			new string[] { "PhilipsHue", @"Philips Hue\", },
			new string[] { "Grooming", @"Grooming\", },
			new string[] { "Unsplash", @"Unsplash\", },
			new string[] { "Tattoos", @"Tattoos\", },
			new string[] { "Nanami", @"Nanami\", },
			new string[] { "Shirts", @"Shirts\", },
			new string[] { "Tattoo", @"Tattoos\", },
			new string[] { "Poses", @"Poses\", },
			new string[] { "Shirt", @"Shirts\", },
			new string[] { "Pose", @"Poses\", },
			new string[] { "PreciousO23_Bucket", @"zz\Olga\", },
			new string[] { "Video Player", @"zz\Video Player\", },
			new string[] { "DragonFruit", @"zz\DragonFruit\", },
			new string[] { "Instagram", @"zz\Instagram\", },
			new string[] { "Steamgirl", @"zz\Steamgirl\", },
			new string[] { "Patricia", @"zz\Patricia\", },
			new string[] { "Snapchat", @"zz\Snapchat\", },
			new string[] { "OkCupid", @"zz\OkCupid\", },
			new string[] { "P Shots", @"zz\P Shots\", },
			new string[] { "Vanessa", @"zz\Vanessa\", },
			new string[] { "Bumble", @"zz\Bumble\", },
			new string[] { "London", @"zz\London\", },
			new string[] { "Tinder", @"zz\Tinder\", },
			new string[] { "Happn", @"zz\Happn\", },
			new string[] { "Sarah", @"zz\Sarah\", },
			new string[] { "Leah", @"zz\Leah\", },
			new string[] { "Mely", @"zz\Mely\", },
			new string[] { "Olga", @"zz\Olga\", },



			//new string[] { "zz", @"zz\", },
			//new string[] { "Documents", @"Documents\", },
			//new string[] { "Games", @"Games\", },
			//new string[] { "Apps", @"Apps\", },
		};
		#endregion public string[][] Apps
		#endregion

		static void Main(string[] args)
		{
			StartProgram();
			Test_VideoUtil();
			EndProgram();
		}

		#region Test VideoUtil
		private static void Test_VideoUtil()
		{
			var source = @"asdf";
			var _videoUtil = new VideoUtil(source);

			if (!File.Exists(source))
				Console.Write("\nFile doesn't exist. Make sure its correct.");

			Console.Write("\n");
			Console.Write("\nframeRate: {0}", _videoUtil.FrameRate);
			Console.Write("\nframeWidth: {0}", _videoUtil.FrameWidth);
		}
		#endregion Test VideoUtil

		#region Test FileUtil.GetFrameRateAndGetWidth
		private static void Test_FileUtil_GetFrameRateAndGetWidth()
		{
			var DirectoryName = @"asdf\";
			var Name = @"asdf";
			var frameRate = _fileUtil.GetFrameRate(DirectoryName, Name);
			var frameWidth = _fileUtil.GetFrameWidth(DirectoryName, Name);

			Console.Write("\n");
			Console.Write("\nframeRate: {0}", frameRate);
			Console.Write("\nframeWidth: {0}", frameWidth);
		}
		#endregion Test FileUtil.GetFrameRateAndGetWidth

		#region Get All Extended Properties
		private static void GetAllExtendedProperties()
		{
			string label;
			dynamic shell = Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_Shell));
			var DirectoryName = @"G:\Projects\FileCopier2\Downloads\";
			var folder = shell.NameSpace(DirectoryName);

			for (int i=0; i<1000; i++) {
				label = folder.GetDetailsOf(null, i);

				if (!String.IsNullOrWhiteSpace(label))
					Console.Write("\n{0}: {1}", i, label);
			}

		}
		#endregion Get All Extended Properties

		#region Test QB20191021Util 2019 1021 1643
		private static void Test_QB20191021Util()
		{
			var now = DateTime.Now;
			DateTime? fromTime = null;
			DateTime? toTime = null;
			var appName = "Rcw.QuickBooks";
			var qbFileName = @"C:\Shared\Quickbooks\Little Creek Winery, LLC.QBW";
			var _qbUtil = new QB20191021Util();
			_qbUtil.Debug = true;

			if (_qbUtil.OpenConnection(appName, appName, qbFileName: qbFileName)) {
				fromTime = now.AddDays(-53);
				toTime = new DateTime(2020, 1, 1);
				_qbUtil.DoAction(QB20191021Util.ActionType.InventoryAdjustmentQuery, fromTime, toTime);
				_qbUtil.CloseConnection();
			}
		}
		#endregion Test QB20191021Util 2019 1021 1643

		#region Test TextUtil.ToTitleCaseSmart 2019 1016 1223
		private static void Test_TextUtil_ToTitleCaseSmart()
		{
			string[] tests = {
				null, "", "a tale of two cities", "gROWL to the rescue",
				"inside the US government", "sports and MLB baseball",
				"The Return of Sherlock Holmes", "UNICEF and children", "UNICEF AND CHILDREN",
				"Old McDonald", "old mcdonald", "OLD MCDONALD", };

			for (int i=0; i< tests.Length; i++) {
				Console.Write("\n");
				Console.Write("\n{0}) {1}", ++totalCount, tests[i] ?? "NULL");
				Console.Write("\n   {0}", TextUtil.ToTitleCaseSmart(tests[i]));
			}
		}
		#endregion Test TextUtil.ToTitleCaseSmart 2019 1016 1223

		#region Test FileUtil.DeleteAllThumbsDb 2019 1002 1419
		private static void Test_FileUtil_DeleteAllThumbsDb()
		{
			consoleOut = true;
			var count = 0;
			var tests = new string[] {
				//@"Z:\Downloads\",
				//@"Z:\Users\",
				@"Z:\",
			};

			for (int i = 0; i < tests.Length; i++) {
				Console.Write("\n");
				Console.Write("\n{0}) {1}", i + 1, tests[i]);
				count = _fileUtil.DeleteAllThumbsDb(tests[i]);
				Console.Write("\n   Count: {0}", count);
			}
		}
		#endregion Test FileUtil.DeleteAllThumbsDb 2019 1002 1419

		#region Test FileUtil.EnumerateFiles 2019 1002 1419
		private static void Test_FileUtil_EnumerateFiles()
		{
			consoleOut = true;
			var tests = new string[] {
				@"Z:\Downloads\",
				@"Z:\Users\",
			};

			for (int i = 0; i < tests.Length; i++) {
				Console.Write("\n==============================");
				Console.Write("\n{0}) {1}", i + 1, tests[i]);
				var files = _fileUtil.EnumerateFiles(tests[i], "desktop*.ini", SearchOption.AllDirectories);
				Console.Write("\n   files.Count: {0}", files.Count());

				foreach (var v in files)
					Console.Write("\n   {0}", v);
			}
		}
		#endregion Test FileUtil.EnumerateFiles 2019 1002 1419

		#region Test Base36.EncodeIP 2019 0930 0312
		private static void Test_Base36_EncodeIP()
		{
			string encoded, decoded;
			var tests = new string[] {
				"0.0.0.0",
				"0.0.0.1",
				"0.0.1.0",
				"0.1.0.0",
				"1.0.0.0",
				"1.255.255.255",
				"10.255.255.255",
				"100.255.255.255",
				"107.184.169.245",
				"255.255.255.255",
			};

			for (int i = 0; i < tests.Length; i++) {
				Console.Write("\n");
				Console.Write("\n{0}) {1}", i + 1, tests[i]);
				encoded = Base36.EncodeIP(tests[i]);
				Console.Write("\n   Encoded: {0}", encoded);
				decoded = Base36.DecodeIP(encoded);
				Console.Write("\n   Decoded: {0}", decoded);
			}

		}
		#endregion Test Base36.EncodeIP 2019 0930 0312

		#region Test Base36.DumbEncode 2019 0930 0251
		private static void Test_Base36_DumbEncode()
		{
			string encoded, decoded;
			var tests = new string[] { "blizmanyu@gmail.com", "yblizman@rcw1.com", };

			for (int i = 0; i < tests.Length; i++) {
				Console.Write("\n");
				Console.Write("\n{0}) {1}", i + 1, tests[i]);
				encoded = Base36.DumbEncode(tests[i]);
				Console.Write("\n   Encoded: {0}", encoded);
				decoded = Base36.DumbDecode(encoded);
				Console.Write("\n   Decoded: {0}", decoded);
			}

		}
		#endregion Test Base36.DumbEncode 2019 0930 0251

		#region Test DateTime 2019 0927 1804
		private static void Test201909271804()
		{
			long parsed;
			string[] split;
			var tests = new string[] { "0.0.0.0", "0.0.0.1", "1.1.1.1", "255.255.255.255", };

			for (int i = 0; i < tests.Length; i++) {
				split = tests[i].Split('.');
				parsed = long.Parse(tests[i].Replace(".", ""));
				Console.Write("\n");
				Console.Write("\n{0}) {1}", i + 1, tests[i]);
				Console.Write("\n   Encoded: {0}", Base36.Encode(parsed));
				Console.Write("\n     Hexed: ");

				for (int j = 0; j < split.Length; j++) {
					Console.Write("{0}", int.Parse(split[j]).ToString("x"));
				}
			}

		}
		#endregion Test DateTime 2019 0927 1804

		#region Test DateTime 2019 0927 1703
		private static void TestDateTime201909271703()
		{
			var now = DateTime.UtcNow;
			Console.Write("\n");
			Console.Write("\nToString('yyyyMMddHHmmssfff'): {0}", now.ToString("yyyyMMddHHmmssfff"));
			Console.Write("\n                        Ticks: {0}", now.Ticks);
			Console.Write("\n                  GetHashCode: {0}", now.GetHashCode().ToString("x"));
			Console.Write("\n       ToString().GetHashCode: {0}", now.ToString().GetHashCode().ToString("x"));
		}
		#endregion Test DateTime 2019 0927 1703

		#region Test_SqlUtil_TruncateTable - 2019 0905 1909
		private static void Test_SqlUtil_TruncateTable()
		{
			var conStr = "";
			var database = "";
			var schema = "dbo";
			var table = "";

			_sqlUtil.TruncateTable(conStr, database, schema, table);
		}
		#endregion Test_SqlUtil_TruncateTable - 2019 0905 1909

		//private static void Test_V3Util_CreateCustomer()
		//{
		//	var apiKey = "asdfasdf";
		//	//var _v3Util = new V3Util(apiKey, V3Util.MOCK_SERVER_URL);
		//	var _v3Util = new V3Util(apiKey, V3Util.PRODUCTION_URL);
		//	var customer = new Customer {
		//		Email = "test@test.com",
		//	};

		//	var response = _v3Util.CreateCustomer(customer);
		//	response.Wait();
		//	var json = JObject.Parse(response.Result);

		//	if (json["message"] == null || String.IsNullOrWhiteSpace(json["message"].ToString()))
		//		Console.Write("\n{0}", json.ToString());
		//	else
		//		Console.Write("\n{0}: {1}", json["type"], json["message"]);
		//}

		private static void Test_TextUtil_BeautifyJson()
		{
			var str = "{\"email\":\"test@test.com\",\"accountId\":\"acct_55e743f3123e3b057094768a\",\"updatedBy\":\"api - Key2019-0819-1603\",\"id\":\"cust_5d5c94136607c400012685cc\",\"authToken\":\"f992c1681c2dcf9da10ec591cd9b8f0a5b1d011aae9200ef9b2fc5e15334392d\",\"updatedOn\":\"2019-08-21T00:45:07.543Z\",\"createdOn\":\"2019-08-21T00:45:07.543Z\",\"customerSince\":\"2019-08-21T00:45:07.543Z\",\"newPassword\":\"53ed1982b33a0385390a\"}";
			Console.Write("\n");
			Console.Write("\n{0}", str);
			Console.Write("\n");
			Console.Write("\n{0}", TextUtil.BeautifyJson(str));
		}

		private static void TestDriveInfo()
		{
			var allDrives = DriveInfo.GetDrives().Where(x => x.DriveType.Equals(DriveType.Fixed));

			foreach (var d in allDrives) {
				Console.Write("\n============================");
				Console.Write("\n                Name: {0}", d.Name);
				Console.Write("\n           DriveType: {0}", d.DriveType);
				if (d.IsReady) {
					Console.Write("\n         VolumeLabel: {0}", d.VolumeLabel);
					Console.Write("\n         DriveFormat: {0}", d.DriveFormat);
					Console.Write("\n                 10%:  {0:n2} GB ({1:n0} bytes)", d.TotalSize * .1 / 1024.0 / 1024.0 / 1024.0, d.TotalSize * .1);
					Console.Write("\n  AvailableFreeSpace: {0:n2} GB ( {1:n0} bytes)", d.AvailableFreeSpace / 1024.0 / 1024.0 / 1024.0, d.AvailableFreeSpace);
					Console.Write("\n           TotalSize: {0:n2} GB ({1:n0} bytes)", d.TotalSize / 1024.0 / 1024.0 / 1024.0, d.TotalSize);
					Console.Write("\n       RootDirectory: {0}", d.RootDirectory);
				}
			}
		}

		private static void EnumerableSorter()
		{
			var bypassList = new List<string> { "TV", "VIDEOS", "ANIME", "MOVIES", "MUSIC VIDEOS", "TEST", "_TEST", };
			//var list = Apps.OrderByDescending(x => x[0].Length).ThenBy(x => x[0]).ToList();
			var list = bypassList.OrderBy(x => x).ToList();
			Console.Write("\n");
			Console.Write("\n{0}", String.Join("\", \"", list));

			//foreach (var v in list)
			//	Console.Write("\n\t\t\tnew string[] {{ \"{0}\", @\"{1}\", }},", v[0], v[1]);

			//foreach (var v in Apps)
			//	Console.Write("\n\t\t\tnew string[] {{ \"{0}\", @\"Apps\\{0}\\\", }},", v);
		}

		private static void GetMostCommonWordsInString()
		{
			consoleOut = true;
			var source = @"asdfasdf\";
			List<KeyValuePair<string, int>> list;
			FileInfo fi;
			StringBuilder sb;
			string nameWithoutExtension, newName, temp;
			string[] words;
			var dict = new Dictionary<string, int>();
			var files = Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories).ToList();
			var filesCount = files.Count;

			for (int i = 0; i < filesCount; i++) {
				if (consoleOut) {
					//Console.Write("\n");
					//Console.Write("\n{0}/{1}) {2}", ++totalCount, filesCount, files[i]);
				}

				fi = new FileInfo(files[i]);
				nameWithoutExtension = fi.Name.Replace(fi.Extension, "");
				temp = fi.FullName.Replace(source, "").Replace(fi.Extension, "");
				sb = new StringBuilder();

				foreach (char c in temp) {
					if (char.IsLetter(c))
						sb.Append(c);
					else
						sb.Append("_");
				}

				newName = sb.ToString().Trim();

				while (newName.Contains("__"))
					newName = newName.Replace("__", "_");

				newName = newName.Trim().ToLower();

				if (newName.Length > 1) {
					words = newName.Split('_');

					foreach (var w in words) {
						if (w.Length > 1) {
							if (dict.Keys.Contains(w))
								dict[w] += 1;
							else
								dict.Add(w, 1);
						}
					}
				}
			}

			list = dict.Where(x => x.Value > 70).OrderByDescending(x => x.Value).ThenBy(y => y.Key).ToList();
			var listCount = list.Count;
			Console.Write("\n  listCount: {0}", listCount);
			Console.Write("\n\n");

			if (listCount > 100) {
				foreach (KeyValuePair<string, int> kvp in list)
					Console.Write("\n  {0}: {1}", kvp.Key, kvp.Value);
			}

			else {
				foreach (KeyValuePair<string, int> kvp in list)
					Console.Write("\n  {0}: {1}", kvp.Key, kvp.Value);
			}
		}

		private static void Test_Top1000GirlNames()
		{
			consoleOut = true;
			//var Top1000GirlNames = new List<String> { "Alessandra", "Alexandria", "Clementine", "Evangeline", "Jacqueline", "Alejandra", "Alexandra", "Anastasia", "Annabella", "Annabelle", "Antonella", "Aubriella", "Aubrielle", "Brooklynn", "Brynleigh", "Cassandra", "Catherine", "Charleigh", "Charlotte", "Christina", "Christine", "Elisabeth", "Elizabeth", "Esmeralda", "Esperanza", "Everleigh", "Francesca", "Gabriella", "Gabrielle", "Genevieve", "Gracelynn", "Guadalupe", "Gwendolyn", "Josephine", "Katherine", "Lillianna", "Mackenzie", "Madeleine", "Paisleigh", "Priscilla", "Remington", "Scarlette", "Stephanie", "Valentina", "Addilynn", "Adelaide", "Adrianna", "Adrienne", "Angelica", "Angelina", "Annalise", "Arabella", "Beatrice", "Braelynn", "Brittany", "Brooklyn", "Calliope", "Carolina", "Caroline", "Cataleya", "Catalina", "Cheyenne", "Clarissa", "Coraline", "Daniella", "Danielle", "Ellianna", "Emmaline", "Emmalynn", "Emmeline", "Estrella", "Felicity", "Fernanda", "Florence", "Gabriela", "Giavanna", "Giovanna", "Giuliana", "Gracelyn", "Hadassah", "Hadleigh", "Harleigh", "Heavenly", "Isabella", "Isabelle", "Itzayana", "Izabella", "Jennifer", "Julianna", "Juliette", "Kataleya", "Katalina", "Kathleen", "Kaydence", "Kayleigh", "Khaleesi", "Kimberly", "Leighton", "Lilianna", "Lilliana", "Madalynn", "Maddison", "Madeline", "Madelynn", "Madilynn", "Magnolia", "Makenzie", "Malaysia", "Margaret", "Marianna", "Marleigh", "Mckenzie", "Mckinley", "Meredith", "Michaela", "Michelle", "Nathalie", "Patricia", "Penelope", "Princess", "Rosemary", "Samantha", "Savannah", "Scarlett", "Serenity", "Treasure", "Vannessa", "Veronica", "Victoria", "Virginia", "Vivienne", "Yamileth", "Aaliyah", "Abigail", "Adaline", "Adalynn", "Addilyn", "Addison", "Addisyn", "Addyson", "Adelina", "Adeline", "Adelynn", "Adriana", "Ainsley", "Aislinn", "Alannah", "Alessia", "Alianna", "Alisson", "Allison", "Allyson", "Alondra", "Annabel", "Annalee", "Ariadne", "Arianna", "Ariella", "Arielle", "Arlette", "Ashlynn", "Aurelia", "Avalynn", "Avianna", "Azariah", "Barbara", "Bellamy", "Berkley", "Bethany", "Blakely", "Braelyn", "Braylee", "Brianna", "Bridget", "Briella", "Brielle", "Brinley", "Bristol", "Brynlee", "Cadence", "Cameron", "Camilla", "Camille", "Carolyn", "Cassidy", "Cecelia", "Cecilia", "Celeste", "Charlee", "Charley", "Charlie", "Chelsea", "Claudia", "Colette", "Collins", "Corinne", "Crystal", "Cynthia", "Daleyza", "Daniela", "Deborah", "Delaney", "Delilah", "Destiny", "Dorothy", "Eleanor", "Elianna", "Elliana", "Elliott", "Ellison", "Emberly", "Emerson", "Emersyn", "Emmalyn", "Emmarie", "Estella", "Estelle", "Evelynn", "Everlee", "Frances", "Frankie", "Galilea", "Genesis", "Georgia", "Giselle", "Haisley", "Harmoni", "Harmony", "Holland", "Isabela", "Jaelynn", "Jaliyah", "Janelle", "Janessa", "Janiyah", "Jasmine", "Jayleen", "Jaylene", "Jazlynn", "Jazmine", "Jenesis", "Jessica", "Jillian", "Jocelyn", "Johanna", "Joselyn", "Journee", "Journey", "Juliana", "Julieta", "Julissa", "Juniper", "Justice", "Kadence", "Kailani", "Kaitlyn", "Kaliyah", "Kamilah", "Kamiyah", "Kassidy", "Katelyn", "Kathryn", "Kaylani", "Kehlani", "Keilani", "Kendall", "Kennedi", "Kennedy", "Kensley", "Kinslee", "Kinsley", "Kyleigh", "Leilani", "Liberty", "Liliana", "Lillian", "Lilyana", "Lindsey", "Lorelai", "Lorelei", "Luciana", "Lucille", "Madalyn", "Madelyn", "Madilyn", "Madison", "Madisyn", "Makayla", "Makenna", "Maliyah", "Mallory", "Mariana", "Marilyn", "Marisol", "Marissa", "Matilda", "Mckenna", "Meilani", "Melanie", "Melissa", "Mikaela", "Mikayla", "Miracle", "Miranda", "Natalia", "Natalie", "Natasha", "Novalee", "Oaklynn", "Octavia", "Ophelia", "Paislee", "Paisley", "Paulina", "Phoenix", "Presley", "Promise", "Raelynn", "Rebecca", "Rebekah", "Rosalee", "Rosalie", "Rosalyn", "Roselyn", "Royalty", "Ryleigh", "Sabrina", "Saniyah", "Saoirse", "Sariyah", "Savanna", "Scarlet", "Taliyah", "Tatiana", "Tiffany", "Tinsley", "Trinity", "Valeria", "Valerie", "Vanessa", "Violeta", "Viviana", "Waverly", "Whitley", "Whitney", "Xiomara", "Yaretzi", "Zaniyah", "Zariyah", "Adalee", "Adalyn", "Adelyn", "Ailani", "Aileen", "Aitana", "Aiyana", "Alaina", "Alanna", "Alayah", "Alayna", "Aleena", "Alexia", "Alexis", "Aliana", "Alicia", "Alisha", "Alison", "Alivia", "Aliyah", "Alyson", "Alyssa", "Amalia", "Amanda", "Amaris", "Amayah", "Amelia", "Amelie", "Amirah", "Amiyah", "Analia", "Andrea", "Angela", "Aniyah", "Annika", "Ansley", "Ariana", "Ariyah", "Armani", "Ashley", "Ashlyn", "Astrid", "Athena", "Aubree", "Aubrey", "Aubrie", "Audrey", "August", "Aurora", "Austyn", "Autumn", "Avalyn", "Averie", "Aviana", "Ayleen", "Azalea", "Azaria", "Bailee", "Bailey", "Baylee", "Bexley", "Bianca", "Blaire", "Bonnie", "Briana", "Brooke", "Brylee", "Callie", "Camila", "Camryn", "Carmen", "Carter", "Celine", "Chanel", "Charli", "Claire", "Dahlia", "Dakota", "Dalary", "Dallas", "Daphne", "Davina", "Dayana", "Eileen", "Elaina", "Elaine", "Eliana", "Elliot", "Eloise", "Emelia", "Emerie", "Emilee", "Emilia", "Ensley", "Esther", "Evelyn", "Everly", "Fatima", "Finley", "Gianna", "Gloria", "Gracie", "Hadlee", "Hadley", "Hailee", "Hailey", "Hallie", "Hannah", "Harlee", "Harley", "Harlow", "Harper", "Hattie", "Hayden", "Haylee", "Hayley", "Heaven", "Helena", "Henley", "Hunter", "Iliana", "Ingrid", "Isabel", "Ivanna", "Jaelyn", "Jaycee", "Jayden", "Jaylah", "Jaylee", "Jazlyn", "Jazmin", "Jessie", "Jimena", "Joanna", "Joelle", "Jolene", "Jordan", "Jordyn", "Journi", "Judith", "Juliet", "Jurnee", "Kaelyn", "Kailey", "Kailyn", "Kalani", "Kallie", "Kamila", "Kamryn", "Karina", "Karlee", "Karsyn", "Karter", "Kaylee", "Kaylie", "Kaylin", "Kelsey", "Kendra", "Kenley", "Kenzie", "Kimber", "Kimora", "Kinley", "Kynlee", "Lailah", "Lainey", "Landry", "Laurel", "Lauren", "Lauryn", "Laylah", "Leanna", "Legacy", "Lennon", "Lennox", "Leslie", "Lilian", "Lilith", "Lillie", "London", "Londyn", "Louisa", "Louise", "Luella", "Lyanna", "Maggie", "Maisie", "Malani", "Malaya", "Maleah", "Maliah", "Margot", "Mariah", "Mariam", "Marina", "Marlee", "Marley", "Martha", "Maryam", "Maxine", "Meadow", "Meghan", "Melany", "Melina", "Melody", "Milana", "Milani", "Milena", "Millie", "Miriam", "Monica", "Monroe", "Morgan", "Nalani", "Nataly", "Nayeli", "Nevaeh", "Nicole", "Noelle", "Oaklee", "Oakley", "Oaklyn", "Olivia", "Palmer", "Paloma", "Parker", "Payton", "Peyton", "Phoebe", "Rachel", "Raegan", "Raelyn", "Ramona", "Raquel", "Reagan", "Regina", "Renata", "Romina", "Samara", "Samira", "Sandra", "Sariah", "Sawyer", "Saylor", "Selena", "Selene", "Serena", "Shelby", "Shiloh", "Sienna", "Sierra", "Simone", "Skylar", "Skyler", "Sloane", "Sophia", "Sophie", "Stella", "Stevie", "Summer", "Sutton", "Sydney", "Sylvia", "Sylvie", "Taylor", "Teagan", "Tenley", "Teresa", "Thalia", "Tinley", "Valery", "Vienna", "Violet", "Vivian", "Willow", "Winter", "Wynter", "Ximena", "Yareli", "Zainab", "Zariah", "Zaylee", "Zhavia", "Adele", "Adley", "Aisha", "Alaia", "Alana", "Alani", "Alaya", "Aleah", "Alena", "Alexa", "Alice", "Alina", "Aliya", "Aliza", "Allie", "Alora", "Amaia", "Amani", "Amara", "Amari", "Amaya", "Amber", "Amina", "Amira", "Amiya", "Amora", "Anahi", "Anais", "Anaya", "Angel", "Angie", "Anika", "Aniya", "Annie", "April", "Arden", "Arely", "Ariah", "Ariel", "Ariya", "Aspen", "Averi", "Avery", "Aylin", "Belen", "Bella", "Belle", "Blair", "Blake", "Briar", "Brynn", "Carly", "Casey", "Celia", "Chana", "Chaya", "Chloe", "Clara", "Clare", "Daisy", "Danna", "Della", "Diana", "Dream", "Dulce", "Dylan", "Edith", "Egypt", "Elena", "Elina", "Elisa", "Elise", "Eliza", "Ellen", "Ellie", "Ellis", "Elora", "Elsie", "Elyse", "Ember", "Emely", "Emery", "Emily", "Emmie", "Emory", "Erica", "Erika", "Faith", "Fiona", "Freya", "Frida", "Gemma", "Giana", "Grace", "Greta", "Haley", "Halle", "Hanna", "Haven", "Hazel", "Heidi", "Helen", "Holly", "Imani", "India", "Irene", "Itzel", "Ivory", "Jamie", "Jayda", "Jayde", "Jayla", "Jemma", "Jenna", "Jessy", "Jewel", "Jolie", "Josie", "Joyce", "Julia", "Julie", "Kairi", "Kaiya", "Karen", "Karla", "Katie", "Kayla", "Keily", "Keira", "Kelly", "Kenia", "Kenna", "Keyla", "Khloe", "Kiana", "Kiara", "Kiera", "Kylee", "Kylie", "Lacey", "Laila", "Laney", "Laura", "Layla", "Leila", "Leona", "Lexie", "Leyla", "Liana", "Lilah", "Lilly", "Linda", "Livia", "Logan", "Lucia", "Lydia", "Lylah", "Lyric", "Mabel", "Macie", "Maeve", "Malia", "Maren", "Margo", "Maria", "Marie", "Mavis", "Megan", "Mercy", "Micah", "Milan", "Miley", "Molly", "Mylah", "Nadia", "Nancy", "Naomi", "Noemi", "Norah", "Novah", "Nylah", "Olive", "Paige", "Paola", "Paris", "Paula", "Pearl", "Penny", "Perla", "Piper", "Poppy", "Queen", "Quinn", "Raina", "Raven", "Rayna", "Rayne", "Reese", "Reign", "Reina", "Reyna", "Riley", "River", "Rivka", "Robin", "Rosie", "Rowan", "Royal", "Ryann", "Rylan", "Rylee", "Rylie", "Sadie", "Saige", "Salma", "Sarah", "Sarai", "Sasha", "Scout", "Selah", "Siena", "Skyla", "Sloan", "Sofia", "Sunny", "Talia", "Tatum", "Tessa", "Tiana", "Wendy", "Willa", "Zahra", "Zaria", "Zelda", "Abby", "Alia", "Alma", "Amia", "Andi", "Anna", "Anne", "Anya", "Aria", "Arya", "Avah", "Ayla", "Bria", "Cali", "Cara", "Cora", "Dana", "Dani", "Demi", "Dior", "Eden", "Ella", "Elle", "Elsa", "Emma", "Emmy", "Erin", "Esme", "Etta", "Evie", "Ezra", "Faye", "Gwen", "Hana", "Hope", "Iris", "Isla", "Jada", "Jade", "Jana", "Jane", "June", "Kaia", "Kali", "Kara", "Kate", "Kira", "Kora", "Kori", "Kyla", "Kyra", "Lana", "Lara", "Leah", "Leia", "Lena", "Lexi", "Lila", "Lily", "Lina", "Lisa", "Lola", "Lucy", "Luna", "Lyla", "Lyra", "Maci", "Macy", "Maia", "Mara", "Mary", "Maya", "Miah", "Mila", "Mina", "Mira", "Myah", "Myla", "Myra", "Nala", "Naya", "Nina", "Nola", "Noor", "Nora", "Nova", "Nyla", "Olga", "Opal", "Remi", "Remy", "Rhea", "Rory", "Rosa", "Rose", "Ruby", "Ruth", "Ryan", "Sage", "Sara", "Skye", "Thea", "Tori", "Vada", "Veda", "Vera", "Wren", "Yara", "Zara", "Zoey", "Zoie", "Zola", "Zora", "Zuri", "Ada", "Amy", "Ana", "Ann", "Ari", "Ava", "Aya", "Eva", "Eve", "Gia", "Ivy", "Joy", "Kai", "Lea", "Lia", "Liv", "Mae", "Mia", "Mya", "Nia", "Noa", "Sky", "Zoe", };
			var Top1000GirlNames = new List<String> { "Alessandra", "Alexandria", "Clementine", "Evangeline", "Jacqueline", "Alejandra", "Alexandra", "Anastasia", "Annabella", "Annabelle", "Antonella", "Aubriella", "Aubrielle", "Brooklynn", "Brynleigh", "Cassandra", "Catherine", "Charleigh", "Charlotte", "Christina", "Christine", "Elisabeth", "Elizabeth", "Esmeralda", "Esperanza", "Everleigh", "Francesca", "Gabriella", "Gabrielle", "Genevieve", "Gracelynn", "Guadalupe", "Gwendolyn", "Josephine", "Katherine", "Lillianna", "Mackenzie", "Madeleine", "Paisleigh", "Priscilla", "Remington", "Scarlette", "Stephanie", "Valentina", "Addilynn", "Adelaide", "Adrianna", "Adrienne", "Angelica", "Angelina", "Annalise", "Arabella", "Beatrice", "Braelynn", "Brittany", "Brooklyn", "Calliope", "Carolina", "Caroline", "Cataleya", "Catalina", "Cheyenne", "Clarissa", "Coraline", "Daniella", "Danielle", "Ellianna", "Emmaline", "Emmalynn", "Emmeline", "Estrella", "Felicity", "Fernanda", "Florence", "Gabriela", "Giavanna", "Giovanna", "Giuliana", "Gracelyn", "Hadassah", "Hadleigh", "Harleigh", "Heavenly", "Isabella", "Isabelle", "Itzayana", "Izabella", "Jennifer", "Julianna", "Juliette", "Kataleya", "Katalina", "Kathleen", "Kaydence", "Kayleigh", "Khaleesi", "Kimberly", "Leighton", "Lilianna", "Lilliana", "Madalynn", "Maddison", "Madeline", "Madelynn", "Madilynn", "Magnolia", "Makenzie", "Malaysia", "Margaret", "Marianna", "Marleigh", "Mckenzie", "Mckinley", "Meredith", "Michaela", "Michelle", "Nathalie", "Patricia", "Penelope", "Princess", "Rosemary", "Samantha", "Savannah", "Scarlett", "Serenity", "Treasure", "Vannessa", "Veronica", "Victoria", "Virginia", "Vivienne", "Yamileth", "Aaliyah", "Abigail", "Adaline", "Adalynn", "Addilyn", "Addison", "Addisyn", "Addyson", "Adelina", "Adeline", "Adelynn", "Adriana", "Ainsley", "Aislinn", "Alannah", "Alessia", "Alianna", "Alisson", "Allison", "Allyson", "Alondra", "Annabel", "Annalee", "Ariadne", "Arianna", "Ariella", "Arielle", "Arlette", "Ashlynn", "Aurelia", "Avalynn", "Avianna", "Azariah", "Barbara", "Bellamy", "Berkley", "Bethany", "Blakely", "Braelyn", "Braylee", "Brianna", "Bridget", "Briella", "Brielle", "Brinley", "Bristol", "Brynlee", "Cadence", "Cameron", "Camilla", "Camille", "Carolyn", "Cassidy", "Cecelia", "Cecilia", "Celeste", "Charlee", "Charley", "Charlie", "Chelsea", "Claudia", "Colette", "Collins", "Corinne", "Crystal", "Cynthia", "Daleyza", "Daniela", "Deborah", "Delaney", "Delilah", "Destiny", "Dorothy", "Eleanor", "Elianna", "Elliana", "Elliott", "Ellison", "Emberly", "Emerson", "Emersyn", "Emmalyn", "Emmarie", "Estella", "Estelle", "Evelynn", "Everlee", "Frances", "Frankie", "Galilea", "Genesis", "Georgia", "Giselle", "Haisley", "Harmoni", "Harmony", "Holland", "Isabela", "Jaelynn", "Jaliyah", "Janelle", "Janessa", "Janiyah", "Jasmine", "Jayleen", "Jaylene", "Jazlynn", "Jazmine", "Jenesis", "Jessica", "Jillian", "Jocelyn", "Johanna", "Joselyn", "Journee", "Journey", "Juliana", "Julieta", "Julissa", "Juniper", "Justice", "Kadence", "Kailani", "Kaitlyn", "Kaliyah", "Kamilah", "Kamiyah", "Kassidy", "Katelyn", "Kathryn", "Kaylani", "Kehlani", "Keilani", "Kendall", "Kennedi", "Kennedy", "Kensley", "Kinslee", "Kinsley", "Kyleigh", "Leilani", "Liberty", "Liliana", "Lillian", "Lilyana", "Lindsey", "Lorelai", "Lorelei", "Luciana", "Lucille", "Madalyn", "Madelyn", "Madilyn", "Madison", "Madisyn", "Makayla", "Makenna", "Maliyah", "Mallory", "Mariana", "Marilyn", "Marisol", "Marissa", "Matilda", "Mckenna", "Meilani", "Melanie", "Melissa", "Mikaela", "Mikayla", "Miracle", "Miranda", "Natalia", "Natalie", "Natasha", "Novalee", "Oaklynn", "Octavia", "Ophelia", "Paislee", "Paisley", "Paulina", "Phoenix", "Presley", "Promise", "Raelynn", "Rebecca", "Rebekah", "Rosalee", "Rosalie", "Rosalyn", "Roselyn", "Royalty", "Ryleigh", "Sabrina", "Saniyah", "Saoirse", "Sariyah", "Savanna", "Scarlet", "Taliyah", "Tatiana", "Tiffany", "Tinsley", "Trinity", "Valeria", "Valerie", "Vanessa", "Violeta", "Viviana", "Waverly", "Whitley", "Whitney", "Xiomara", "Yaretzi", "Zaniyah", "Zariyah", "Adalee", "Adalyn", "Adelyn", "Ailani", "Aileen", "Aitana", "Aiyana", "Alaina", "Alanna", "Alayah", "Alayna", "Aleena", "Alexia", "Alexis", "Aliana", "Alicia", "Alisha", "Alison", "Alivia", "Aliyah", "Alyson", "Alyssa", "Amalia", "Amanda", "Amaris", "Amayah", "Amelia", "Amelie", "Amirah", "Amiyah", "Analia", "Andrea", "Angela", "Aniyah", "Annika", "Ansley", "Ariana", "Ariyah", "Armani", "Ashley", "Ashlyn", "Astrid", "Athena", "Aubree", "Aubrey", "Aubrie", "Audrey", "August", "Aurora", "Austyn", "Autumn", "Avalyn", "Averie", "Aviana", "Ayleen", "Azalea", "Azaria", "Bailee", "Bailey", "Baylee", "Bexley", "Bianca", "Blaire", "Bonnie", "Briana", "Brooke", "Brylee", "Callie", "Camila", "Camryn", "Carmen", "Carter", "Celine", "Chanel", "Charli", "Claire", "Dahlia", "Dakota", "Dalary", "Dallas", "Daphne", "Davina", "Dayana", "Eileen", "Elaina", "Elaine", "Eliana", "Elliot", "Eloise", "Emelia", "Emerie", "Emilee", "Emilia", "Ensley", "Esther", "Evelyn", "Everly", "Fatima", "Finley", "Gianna", "Gloria", "Gracie", "Hadlee", "Hadley", "Hailee", "Hailey", "Hallie", "Hannah", "Harlee", "Harley", "Harlow", "Harper", "Hattie", "Hayden", "Haylee", "Hayley", "Heaven", "Helena", "Henley", "Hunter", "Iliana", "Ingrid", "Isabel", "Ivanna", "Jaelyn", "Jaycee", "Jayden", "Jaylah", "Jaylee", "Jazlyn", "Jazmin", "Jessie", "Jimena", "Joanna", "Joelle", "Jolene", "Jordan", "Jordyn", "Journi", "Judith", "Juliet", "Jurnee", "Kaelyn", "Kailey", "Kailyn", "Kalani", "Kallie", "Kamila", "Kamryn", "Karina", "Karlee", "Karsyn", "Karter", "Kaylee", "Kaylie", "Kaylin", "Kelsey", "Kendra", "Kenley", "Kenzie", "Kimber", "Kimora", "Kinley", "Kynlee", "Lailah", "Lainey", "Landry", "Laurel", "Lauren", "Lauryn", "Laylah", "Leanna", "Legacy", "Lennon", "Lennox", "Leslie", "Lilian", "Lilith", "Lillie", "London", "Londyn", "Louisa", "Louise", "Luella", "Lyanna", "Maggie", "Maisie", "Malani", "Malaya", "Maleah", "Maliah", "Margot", "Mariah", "Mariam", "Marina", "Marlee", "Marley", "Martha", "Maryam", "Maxine", "Meadow", "Meghan", "Melany", "Melina", "Melody", "Milana", "Milani", "Milena", "Millie", "Miriam", "Monica", "Monroe", "Morgan", "Nalani", "Nataly", "Nayeli", "Nevaeh", "Nicole", "Noelle", "Oaklee", "Oakley", "Oaklyn", "Olivia", "Palmer", "Paloma", "Parker", "Payton", "Peyton", "Phoebe", "Rachel", "Raegan", "Raelyn", "Ramona", "Raquel", "Reagan", "Regina", "Renata", "Romina", "Samara", "Samira", "Sandra", "Sariah", "Sawyer", "Saylor", "Selena", "Selene", "Serena", "Shelby", "Shiloh", "Sienna", "Sierra", "Simone", "Skylar", "Skyler", "Sloane", "Sophia", "Sophie", "Stella", "Stevie", "Summer", "Sutton", "Sydney", "Sylvia", "Sylvie", "Taylor", "Teagan", "Tenley", "Teresa", "Thalia", "Tinley", "Valery", "Vienna", "Violet", "Vivian", "Willow", "Winter", "Wynter", "Ximena", "Yareli", "Zainab", "Zariah", "Zaylee", "Zhavia", "Adele", "Adley", "Aisha", "Alaia", "Alana", "Alani", "Alaya", "Aleah", "Alena", "Alexa", "Alice", "Alina", "Aliya", "Aliza", "Allie", "Alora", "Amaia", "Amani", "Amara", "Amari", "Amaya", "Amber", "Amina", "Amira", "Amiya", "Amora", "Anahi", "Anais", "Anaya", "Angie", "Anika", "Aniya", "Annie", "April", "Arely", "Ariah", "Ariel", "Ariya", "Aspen", "Averi", "Avery", "Aylin", "Belen", "Bella", "Belle", "Blair", "Blake", "Briar", "Brynn", "Carly", "Casey", "Celia", "Chana", "Chaya", "Chloe", "Clare", "Daisy", "Danna", "Della", "Diana", "Dream", "Dulce", "Dylan", "Edith", "Egypt", "Elena", "Elina", "Elisa", "Elise", "Eliza", "Ellen", "Ellie", "Ellis", "Elora", "Elsie", "Elyse", "Emely", "Emery", "Emily", "Emmie", "Emory", "Erica", "Erika", "Faith", "Fiona", "Freya", "Frida", "Gemma", "Giana", "Grace", "Greta", "Haley", "Halle", "Hanna", "Haven", "Hazel", "Heidi", "Helen", "Holly", "Imani", "India", "Irene", "Itzel", "Ivory", "Jamie", "Jayda", "Jayde", "Jayla", "Jemma", "Jenna", "Jessy", "Jewel", "Jolie", "Josie", "Joyce", "Julia", "Julie", "Kairi", "Kaiya", "Karen", "Karla", "Katie", "Kayla", "Keily", "Keira", "Kelly", "Kenia", "Kenna", "Keyla", "Khloe", "Kiana", "Kiara", "Kiera", "Kylee", "Kylie", "Lacey", "Laila", "Laney", "Laura", "Layla", "Leila", "Leona", "Lexie", "Leyla", "Liana", "Lilah", "Lilly", "Linda", "Livia", "Lucia", "Lydia", "Lylah", "Lyric", "Mabel", "Macie", "Maeve", "Malia", "Maren", "Margo", "Maria", "Marie", "Mavis", "Megan", "Mercy", "Micah", "Milan", "Miley", "Molly", "Mylah", "Nadia", "Nancy", "Naomi", "Noemi", "Norah", "Novah", "Nylah", "Olive", "Paige", "Paola", "Paris", "Paula", "Pearl", "Penny", "Perla", "Piper", "Poppy", "Queen", "Quinn", "Raina", "Raven", "Rayna", "Rayne", "Reese", "Reign", "Reina", "Reyna", "Riley", "Rivka", "Robin", "Rosie", "Rowan", "Royal", "Ryann", "Rylan", "Rylee", "Rylie", "Sadie", "Saige", "Salma", "Sarah", "Sarai", "Sasha", "Scout", "Selah", "Siena", "Skyla", "Sloan", "Sofia", "Sunny", "Talia", "Tatum", "Tessa", "Tiana", "Wendy", "Willa", "Zahra", "Zaria", "Zelda", };
			var Top1000GirlNamesFalsePositives = new List<String> { "Angel", "Arden", "Clara", "Ember", "Logan", "River", "Abby", "Alex", "Alia", "Alma", "Amia", "Andi", "Anna", "Anne", "Anya", "Aria", "Arpy", "Arya", "Avah", "Ayla", "Bria", "Cali", "Cara", "Cora", "Dana", "Dani", "Demi", "Dior", "Eden", "Ella", "Elle", "Elsa", "Emma", "Emmy", "Erin", "Esme", "Etta", "Evie", "Ezra", "Faye", "Gwen", "Hana", "Hope", "Iris", "Isla", "Jada", "Jade", "Jana", "Jane", "June", "Kaia", "Kali", "Kara", "Kate", "Kira", "Kora", "Kori", "Kyla", "Kyra", "Lana", "Lara", "Leah", "Leia", "Lena", "Lexi", "Lila", "Lily", "Lina", "Lisa", "Lola", "Lucy", "Luna", "Lyla", "Lyra", "Maci", "Macy", "Maia", "Mara", "Mary", "Maya", "Mely", "Miah", "Mila", "Mina", "Mira", "Myah", "Myla", "Myra", "Nala", "Naya", "Nina", "Nola", "Noor", "Nora", "Nova", "Nyla", "Olga", "Opal", "Remi", "Remy", "Rhea", "Rory", "Rosa", "Rose", "Ruby", "Ruth", "Ryan", "Sage", "Sara", "Skye", "Thea", "Tori", "Vada", "Veda", "Vera", "Wren", "Yara", "Zara", "Zoey", "Zoie", "Zola", "Zora", "Zuri", "Ada", "Amy", "Ana", "Ann", "Ari", "Ava", "Aya", "Eva", "Eve", "Gia", "Ivy", "Jas", "Joy", "Kai", "Lea", "Lia", "Liv", "Mae", "Mia", "Mya", "Nia", "Noa", "Sky", "Zoe", };

			//Top1000GirlNames = Top1000GirlNames.Where(x => x.Length > 4).OrderByDescending(x => x.Length).ThenBy(x => x).ToList();
			//Console.Write("\n\n\"{0}\"", String.Join("\", \"", Top1000GirlNames));
			Console.Write("\n\nCount: {0}", Top1000GirlNames.Count);

			//Top1000GirlNamesFalsePositives = Top1000GirlNamesFalsePositives.OrderByDescending(x => x.Length).ThenBy(x => x).ToList();
			//Console.Write("\n\n\"{0}\"", String.Join("\", \"", Top1000GirlNamesFalsePositives));
			Console.Write("\n\nCount: {0}", Top1000GirlNamesFalsePositives.Count);
		}

		#region Test CreateDirectory
		private static void Test_CreateDirectory()
		{
			var path = @"C:\1\2\3\";
			Directory.CreateDirectory(path);
		}
		#endregion Test CreateDirectory

		#region Test FileInfo Properties
		private static void Test_FileInfo_Properties()
		{
			consoleOut = true;
			var tests = new string[] {
				@"C:\Pictures\2019\Window 2019 0227 0005 5300.jpg",
			};

			for (int i = 0; i < tests.Length; i++) {
				var fi = new FileInfo(tests[i]);
				Console.Write("\n");
				Console.Write("\n{0}) {1}", ++totalCount, tests[i]);
				Console.Write("\n  CreationTime: {0}", fi.CreationTime);
				Console.Write("\nLastAccessTime: {0}", fi.LastAccessTime);
				Console.Write("\n LastWriteTime: {0}", fi.LastWriteTime);
				Console.Write("\n");
				Console.Write("\nLength: {0}", fi.Length);
				Console.Write("\n");
				Console.Write("\nDirectory.Name: {0}", fi.Directory.Name);
				Console.Write("\nDirectoryName: {0}", fi.DirectoryName);
				Console.Write("\nExtension: {0}", fi.Extension);
				Console.Write("\nFullName: {0}", fi.FullName);
				Console.Write("\nName: {0}", fi.Name);
			}
		}
		#endregion Test FileInfo Properties

		#region Test FileUtil.IsStringInList
		private static void Test_FileUtil_IsStringInList()
		{
			consoleOut = true;
			var tests = new string[] {
				"",
				"asdf",
				"ASDF",
			};

			var lists = new List<String>[] {
				new List<String> { "" },
				new List<String> { "asdf" },
				new List<String> { "ASDF" },
			};

			bool result;
			var _fileUtil = new FileUtil();

			for (int i = 0; i < tests.Length; i++) {
				for (int j = 0; j < lists.Length; j++) {
					totalCount++;
					Console.Write("\n");
					Console.Write("\n{0}) Is '{1}' in [{2}]? {3}", totalCount, tests[i], String.Join(", ", lists[j]), _fileUtil.IsStringInList(tests[i], lists[j]));
				}
			}
		}
		#endregion Test FileUtil.IsStringInList

		#region Test FileUtil.IsStringInArray
		private static void Test_FileUtil_IsStringInArray()
		{
			consoleOut = true;
			var tests = new string[] {
				"",
				"asdf",
				"ASDF",
			};

			var arrays = new List<string[]> {
				new string[] { "", },
				new string[] { "asdf", },
				new string[] { "ASDF", },
			};

			bool result;
			var _fileUtil = new FileUtil();

			for (int i = 0; i < tests.Length; i++) {
				for (int j = 0; j < arrays.Count; j++) {
					totalCount++;
					Console.Write("\n");
					Console.Write("\n{0}) Is '{1}' in [{2}]? {3}", totalCount, tests[i], String.Join(", ", arrays[j]), _fileUtil.IsStringInArray(tests[i], arrays[j]));
				}
			}
		}
		#endregion Test FileUtil.IsStringInArray

		#region Test_NewDateTimeMinValue
		private static void Test_NewDateTimeMinValue()
		{
			consoleOut = true;
			Console.Write("\nnew DateTime(): {0}", new DateTime());
			Console.Write("\nDateTime.MinValue: {0}", DateTime.MinValue);
		}
		#endregion Test_NewDateTimeMinValue

		#region Test_2019_0503_2113
		private static void Test_2019_0503_2113()
		{
			consoleOut = true;
			var IgnoreListFolders = new List<string> { @"\Album", @"\Classical", @"\J-Pop", @"\J-Rap", @"\Spanish", };
			var files = Directory.EnumerateFiles(@"Z:\Music\00 Genres\", "*", SearchOption.AllDirectories);

			foreach (var file in files) {
				Console.Write("\n{0}", file);
			}
		}
		#endregion Test_2019_0503_2113

		#region Test MailUtil.EncodeEmail
		private static void Test_MailUtil_EncodeEmail()
		{
			var tests = new string[] {
				"yblizman@rcw1.com",
			};

			string result;

			for (int i = 0; i < tests.Length; i++) {
				totalCount++;
				Console.Write("\n");
				Console.Write("\n{0}) {1}: ", i + 1, tests[i]);
				result = _mailUtil.EncodeEmail(tests[i]);
				Console.Write("\nencoded: {0}", result);
				Console.Write("\ndecoded: {0}", _mailUtil.DecodeEmail(result));
			}
		}
		#endregion Test MailUtil.EncodeEmail

		#region Test WebUtil.EncodeIp
		//private static void Test_WebUtil_EncodeIp()
		//{
		//	var tests = new string[] {
		//		"0.0.0.0",
		//		"10.0.0.0",
		//		"98.98.98.98",
		//		"98.189.176.208",
		//		"172.16.0.0",
		//		"192.168.0.0",
		//		"255.255.255.255",
		//	};

		//	string encoded, decoded;
		//	Console.Write("\n\nEncode(0): {0}", Base36.Encode(0));

		//	for (int i = 0; i < tests.Length; i++) {
		//		totalCount++;
		//		Console.Write("\n");
		//		Console.Write("\n{0}) {1}: ", i + 1, tests[i]);
		//		encoded = _webUtil.EncodeIp(tests[i]);
		//		Console.Write("\nencoded: {0}", encoded);
		//		decoded = _webUtil.DecodeIp(encoded);
		//		Console.Write("\ndecoded: {0}", decoded);
		//	}
		//}
		#endregion Test WebUtil.EncodeIp

		#region Test WebUtil.IsBotUserAgent
		private static void Test_WebUtil_IsBotUserAgent()
		{
			var tests = new string[] {
				"LAKSDJFLSKADFJ@126.com",
				"lkasdjlkdjf@alskdfjlaskdjf.com",
				"LKASDJFLKASDJF@oncesex.com",
				"lkasjdflksadjf@alskdfjdl.com",
				"LASKDJLSDAKJF@alunos.eel.usp.br",
			};

			bool result;

			for (int i = 0; i < tests.Length; i++) {
				totalCount++;
				Console.Write("\n{0}) {1}: ", i + 1, tests[i]);
				result = _webUtil.IsBotUserAgent(tests[i]);
				Console.Write("{0}", result);
			}
		}
		#endregion Test WebUtil.IsBotUserAgent

		#region Test WebUtil.IsBotEmail
		private static void Test_WebUtil_IsBotEmail()
		{
			var tests = new string[] {
				"LAKSDJFLSKADFJ@126.com",
				"lkasdjlkdjf@alskdfjlaskdjf.com",
				"LKASDJFLKASDJF@oncesex.com",
				"lkasjdflksadjf@alskdfjdl.com",
				"LASKDJLSDAKJF@alunos.eel.usp.br",
			};

			bool result;

			for (int i = 0; i < tests.Length; i++) {
				totalCount++;
				Console.Write("\n{0}) {1}: ", i + 1, tests[i]);
				result = _webUtil.IsBotEmail(tests[i]);
				Console.Write("{0}", result);
			}
		}
		#endregion Test WebUtil.IsBotEmail

		#region Test VideoUtil.CreateAnimatedGif
		private static void Test_VideoUtil_CreateAnimatedGif()
		{
			consoleOut = false;
			VideoUtil _videoUtil;

			var tests = new string[] {
				@"asdfasdf",
			};

			for (int i = 0; i < tests.Length; i++) {
				totalCount++;
				_videoUtil = new VideoUtil(tests[i]);
				//_videoUtil.FirstXMin = 12;
				//_videoUtil.Width = 640; // default is 1000 //
				//_videoUtil.CreateFirstXMin(0, 1);
				_videoUtil.CreateAnimatedGif(.5);
				//_videoUtil.CreateAnimatedGif(134, 135);
				//_videoUtil.CreateAnimatedGif(134.5, 135);
				//_videoUtil.CreateHtml(@"C:\TEMP\2019 0226 0329 5118\");
			}

			Process.Start("explorer.exe", @"C:\Temp\");
		}
		#endregion Test Util.LocalTimeToGoogleInternalDate

		#region Test FileUtilStatic.DeleteFiles
		private static void Test_FileUtil_DeleteFiles()
		{
			var tests = new string[] {
				@"C:\TEMP\test1\",
			};

			for (int i = 0; i < tests.Length; i++) {
				totalCount++;
				_fileUtil.DeleteFiles(tests[i], "gif");
			}
		}
		#endregion Test FileUtilStatic.DeleteFiles

		#region Test SqlUtil.ToXls
		private static void Test_SqlUtil_ToXls()
		{

		}
		#endregion Test SqlUtil.ToXls

		#region Test VideoUtil.CreateVersion1
		//private static void Test_VideoUtil_CreateVersion1()
		//{
		//	string destFolder;
		//	var tests = new string[] {
		//		//@"G:\Projects\FileCopier2\Videos\MFC\NaomiDee - MyFreeCams - Google Chrome 2019-02-10 02-45-47.mp4",
		//		@"F:\Videos\Jful\Alex Grey - Slut Puppies 11 Scene Teen Ass Is Open For Business anal 4k 2160p.mp4",
		//		//@"G:\Projects\FileCopier2\Videos\MFC\NaomiDee - MyFreeCams - Google Chrome 2019-02-10 03-06-50.mp4",
		//	};

		//	for (int i = 0; i < tests.Length; i++) {
		//		destFolder = String.Format(@"C:\Temp\{0:yyyy MMdd HHmm ssff}\", DateTime.Now);
		//		Directory.CreateDirectory(destFolder);
		//		VideoUtil.CreateAnimatedGif(tests[i], 20);
		//		Process.Start("explorer.exe", destFolder);
		//	}

		//	//for (int i = 0; i < tests.Length; i++) {
		//	//	destFolder = String.Format(@"C:\Temp\{0:yyyy MMdd HHmm ssff}\", DateTime.Now);
		//	//	Directory.CreateDirectory(destFolder);
		//	//	//VideoUtil.CreateVersion1(tests[i], destFolder, 0, 7320);
		//	//	VideoUtil.CreateAllBetween(tests[i], destFolder, 0);
		//	//	Process.Start("explorer.exe", destFolder);
		//	//}
		//}
		#endregion Test VideoUtil.CreateVersion1

		#region Test VideoUtil.CreateAnimatedGif
		//private static void Test_VideoUtil_CreateAnimatedGif()
		//{
		//	var source = @"G:\Projects\FileCopier2\Videos\MFC\NaomiDee - MyFreeCams - Google Chrome 2019-02-10 03-06-50.mp4";
		//	var destFolder = String.Format(@"C:\Temp\{0:yyyy MMdd HHmm ssff}\", startTime);
		//	var tests = new TimeSpan[] {
		//		new TimeSpan(0, 0, 0),
		//	};

		//	Directory.CreateDirectory(destFolder);

		//	for (int i = 0; i < tests.Length; i++) {
		//		totalCount++;
		//		VideoUtil.CreateAnimatedGif(tests[i], 2, source, destFolder);
		//	}

		//	Process.Start("explorer.exe", destFolder);
		//}
		#endregion Test Util.LocalTimeToGoogleInternalDate

		#region Test Util.LocalTimeToGoogleInternalDate
		private static void Test_Util_LocalTimeToGoogleInternalDate()
		{
			var now = DateTime.Now;

			var tests = new DateTime[] {
				now,
				now.Date,
				now.Date.AddHours(14).AddMinutes(20),
				now.AddDays(-11),
				now.AddMonths(-1),
				now.AddYears(-1),
				now.AddYears(-2),
			};

			for (int i = 0; i < tests.Length; i++) {
				totalCount++;
				var v = Util.LocalTimeToGoogleInternalDate(tests[i]);
				Console.Write("\n");
				Console.Write("\n{0}) {1}", i + 1, tests[i]);
				Console.Write("\n   {0}", v);
				Console.Write("\n   {0}", Util.GoogleInternalDateToLocalTime(v));
			}
		}
		#endregion Test Util.LocalTimeToGoogleInternalDate

		#region Test Util.GetRelativeDateTimeString
		private static void Test_Util_GetRelativeDateTimeString()
		{
			var now = DateTime.Now;

			var tests = new DateTime[] {
				now,
				now.Date,
				now.Date.AddHours(14).AddMinutes(20),
				now.AddDays(-11),
				now.AddMonths(-1),
				now.AddYears(-1),
				now.AddYears(-2),
			};

			for (int i = 0; i < tests.Length; i++) {
				totalCount++;
				var v = Util.GetRelativeDateTimeString(tests[i]);
				Console.Write("\n");
				Console.Write("\n{0}) {1}", i + 1, tests[i]);
				Console.Write("\n   {0}", v);
			}
		}
		#endregion Test Util.GetRelativeDateTimeString

		#region Test MailUtil.ConvertStringToMailAddresses
		private static void Test_MailUtil_ConvertStringToMailAddresses()
		{
			var tests = new string[] {
				"Michael Contursi <mcontursi@rcw1.com>",
				"asdfsdf",
			};

			for (int i = 0; i < tests.Length; i++) {
				totalCount++;
				var v = _mailUtil.ConvertStringToMailAddresses(tests[i]);
				Console.Write("\n");
				Console.Write("\n{0}) {1}", i + 1, tests[i]);
				Console.Write("\n   DisplayName: {0}", v[0].DisplayName);
				Console.Write("\n   Address: {0}", v[0].Address);
			}
		}
		#endregion Test MailUtil.ConvertStringToMailAddresses

		#region Test TextUtil.BeautifyPhone
		private static void Test_TextUtil_BeautifyPhone()
		{
			var tests = new string[] {
				@"(415)418-2400 x225",
				@"(310) 396-4514     X 2",
				@"fax",
				@"(941)360-3990 xt. 154",
				@"(607) 844-xxxx",
				@"(314)822-9958  off X121",
				@"3103947577ext225",
				@"18004311018X307",
				@"5403498083x104",
				@"828 324 5555ext271",
				@"5132413415x22",
				@"9544973661Ext303",
				@"9549383550x216",
				@"9413431100X7",
				@"(516) 826-6500 x223",
				@"4157443080x3054",
				@"8003594255EXT221",
				@"7574911060x104",
				@"3044296741x2445",
				@"(617) 728-9111 X 635",
				@"3055948666ext#213",
				@"2038788194x201",
				@"(non) exx-xxxx",
				@"9513211960x210",
				@"2394037777ext375",
				@"(xxx) xxx-xxxx",
				@"(281) 419-4166  fax (?)",
				@"(xxx) xxx-xxxx",
				@"(xxx) xxx-xxxx",
				@"9735559901x",
				@"(xxx) xxx-xxxx",
				@"(631)231-3600 ex 1276",
				@"(xxx) xxx-xxxx",
				@"(XXX) XXX-XXXX",
				@"96638588877ext2",
				@"4067511111x19",
				@"5042676005ext205",
				@"7327389600ext231",
				@"609-581-4060x4002",
				@"(xxx) xxx-xxxx",
				@"(XXX) XXX-XXXX",
				@"(xxx) xxx-xxxx",
				@"(412)323-4900 x0 x241",
				@"2815587100X103",
				@"2153398453ext10",
				@"(248)837-7000 ext. 7003",
				@"(949)495-4040 home/fax",
				@"fax",
				@"(xxx) xxx-xxxx",
				@"602-233-3369 xt135",
				@"7177041000x3001",
				@"(xxx) xxx-xxxx",
				@"(303)534-1030  ext13",
				@"973-857-8991 (and fax)",
				@"(772)283-3838x 226",
				@"(407)855-2881 ext 205",
				@"(281)240-6600 xt 104",
				@"(919)231-6200 ext 100",
				@"(951)736-9386   X103",
				@"269-388-5300ext226",
				@"5614204225x160",
				@"1866-544-9622 ext 111",
				@"(281) 440-8284    X 11",
				@"4785990104 biz X1, thenX2",
				@"9495679923   X201",
				@"8662248867  X6  operator",
				@"7136641215 business x315",
				@"(281) 440-8284 X 11",
				@"(208) 429-xxxx",
				@"213-253-2677 X 4791 dept.",
				@"775-778-0754    X1",
				@"9137827787  X 104 biz #",
				@"713-438-6748 X 6748",
				@"800-777-6468 ext 6412",
				@"949 7544500 X1",
				@"617 2433888 (work) X353",
				@"949-453-1500 work # X102",
				@"(925) 947-0900 X111",
				@"6192412326  x1 delay ring",
				@"716-297-0981 biz # X 113",
				@"843-437-0255    X___",
				@"225-664-7167 x516",
				@"818-240-1236 fax #",
				@"949 2874191 work X4004",
				@"9496753690 work X 231",
				@"(805) 879-4800 x 4805",
				@"818 3401134 X 112",
				@"818 3401134 X 112",
				@"(715) 848-3474 X 1012",
				@"801 3164100 X801",
				@"415 9771200 X 1125",
				@"(410) 778-3500 X 1121",
				@"(212) 371-4446 x 2870 ",
				@"(212) 371-4446 x 7750",
				@"(860) 657-4445 Ext. 247",
				@"(860) 657-4445 Ext. 218",
				@"(303) 789-2664 Ext. 238",
				@"(303) 789-2664 Ext. 213",
				@"(303) 789-2664 Ext. 258",
				@"(407) 740-5900 xt 101",
				@"(858) 794-2800 Ext. 21",
				@"(212) 302-3300, Ext. 555",
				@"(516) 349-2150, Ext. 2123",
				@"(212) 302-3300, Ext. 532",
				@"(212) 302-3300, Ext. 580",
				@"(212) 302-3300, Ext. 582",
				@"(212) 302-3300, Ext. 511",
				@"(212) 302-3300, Ext. 503",
				@"(212) 302-3300, Ext. 527",
				@"(212) 302-3300, Ext. 516",
				@"(609) 924-4001 ext. 1207",
				@"(484) 708-4720 ext. 204",
				@"(310) 278-8232 x100",
				@"(310) 278-8232 x106",
				@"(310) 278-8232 x103",
				@"(310) 278-8232 x105",
				@"(310) 278-8232 x101",
				@"(847) 432-8700 Ext. 25",
				@"(847) 432-8700 Ext. 20 ",
				@"(847) 432-8700 Ext. 24 ",
				@"(847) 432-8700 Ext. 22 ",
				@"(312) 338-7788 Ext. 11",
				@"(312) 338-7788 Ext. 12",
				@"(312) 338-7788 Ext. 10",
				@"(312) 338-7788 Ext. 18",
				@"(713) 375-1300 x304",
				@"(713) 375-1300 x306",
				@"(713) 375-1300 x301",
				@"(713) 375-1300 x302",
				@"(713) 375-1300 x303",
				@"(972) 416-1626 x111",
				@"(972) 416-1626 x112",
				@"(704) 365-3325 Ext. 1",
				@"(417) 886-6590 ext. 116",
				@"(417) 886-6590 ext. 114",
				@"(417) 886-6590 ext. 112",
				@"(417) 886-6590 ext. 111",
				@"(713) 375-1300 x304",
				@"(713) 375-1300 x306",
				@"(713) 375-1300 x301",
				@"(713) 375-1300 x302",
				@"(713) 375-1300 x303",
				@"(203) 877-4281 x1302",
				@"(585) 203-1211, ext. 104",
				@"(585) 203-1211, ext. 105",
				@"(585) 203-1211, ext. 106",
				@"(585) 203-1211, ext. 108",
				@"(585) 203-1211, ext. 109",
				@"(734) 662-1200 x919",
				@"(800) 892-7100 x17329",
				@"(800) 892-7100 x17329",
				@"(775) 832-8314 Ext. 102",
				@"(901) 761.3003   ext. 114",
				@"(901) 761.3003   ext. 111",
				@"(901) 761.3003   ext. 119",
				@"(901) 761.3003   ext. 116",
				@"(901) 761.3003   ext. 115",
				@"(718) 433-4111 x5",
				@"(781) 849-7200 x214",
				@"(516) 759-3900 Ext. 1008 ",
				@"(516) 759-3900 Ext. 1013 ",
				@"(516) 759-3900 Ext. 1009",
				@"(516) 759-3900 Ext 1029 ",
				@"(305) 447-8350 Ext. 224",
				@"(305) 447-8350  Ext. 225",
				@"(305) 447-8350 Ext. 222",
				@"(813) 221-3400 Ext. 201",
				@"(415) 344-6180 (X1)",
				@"(804) 419-1100 x 1",
				@"(804) 419-1100 x 1",
				@"(323) 677-0550 x105",
				@"(323) 677-0550 x105",
				@"(323) 677-0550 x102",
				@"(323) 677-0550 x104",
				@"(206) 329-5546 ext. 224",
				@"(206) 329-5546 ext. 251",
				@"(206) 329-5546 ext. 210",
				@"(206) 329-5546 ext. 206",
				@"(206) 329-5546 ext. 222",
				@"(206) 329-5546 ext. 204",
				@"(212) 371-4446 x 2860 ",
				@"(212) 371-4446 x 2875 ",
				@" (212) 371-4446 x 2866",
				@"(212) 371-4446 x 2867",
				@"914-631-9100 ext.932",
				@"800.711.2027 ext. 289",
				@"512.439.4800 Ext. 827",
				@"(301) 657-1700 ext. 104",
				@"(800) 272-4745 x 222",
				@"(516) 767-7104 x20",
				@" 212-477-9626  ext. 11 ",
				@"303-861-4835 x208",
				@"(303) 694-1900 x310",
				@"(310) 821-6563 ext. 15",
				@"888-337-3720 Ext. 172",
				@"(414) 359-1000 Ext.104 ",
				@"858-792-3800 x211",
				@"216-595-3842 x115",
				@"212-452-5900 ext 10",
				@"(973) 425-8420 Ext. 230",
				@"(877) 768-4802 x4",
				@"(904) 399-0662 X203",
				@"212.871.7100 ext. 306",
				@"(516) 487-8220 Ext:12",
				@"212-486-6670 ext. 231",
				@"813-253-2388 x 222",
				@"212-629-7866 ext.201",
				@"(212) 583-9800 Ext. 13",
				@"(212) 583-9800 Ext. 15",
				@"804-323-6868 ext. 107",
				@"804-323-6868 etx. 115",
				@"(816) 461-3312, ext 301",
				@"(415) 677-9118 Ext. 223",
				@"212-972-8700 (ext: 2630)",
				@"(704) 372-1399 Ext. 104",
				@"404.874.7433 ext. 1580",
				@"(949) 477-9300 Ext. 2048",
				@"(214) 692-7248 ext. 106",
				@"203-254-3333 x222",
				@"203-254-3333 x224",
				@"(212) 260-2743 Ext. 103",
				@"(212) 260-2743 Ext. 108",
				@"(314) 991-5150 Ext. 315",
				@"(314) 991-5150 Ext. 309",
				@"(310) 696-4001 Ext. 222",
				@"(216) 241-2800 Ext.6612",
				@"212-486-3600 x110",
				@"212-486-3600 x102",
				@"(949) 769-3323 x. 302",
				@"(949) 769-3323 x. 303",
				@"203-341-3500 x207",
				@"203-341-3500 x201",
				@"212-661-6886 ext. 307",
				@"212-661-6886 ext. 309",
				@"(212) 682 2310 ext 314",
				@"(212) 682 2526 ext 306",
				@"(212) 682 2480 ext 313",
				@"212-260-2743 ext. 101 ",
				@"212-260-2743 ext. 104",
				@"212-260-2743 ext. 106",
				@"212-260-2743 ext. 109",
				@"212-260-2743 ext. 108",
				@"404-816-7540, x113",
				@"(949) 757-0400 x108",
				@"(949) 757-0400 x112",
				@"(949) 757-0400 x111",
				@"214-365-3099 ext 5",
				@"214-365-3090 ext 1",
				@"214-365-3099 ext 2",
				@"214-365-3099 ext 4",
				@"248-645-1520 ext.216",
				@"248-645-1520 ext.216",
				@"248-645-1520 ext.216",
				@"248-645-1520 ext.216",
				@"248-645-1520 ext.216",
				@"877.622.2257  x33722",
				@"303-768-0007 ext. 300",
				@"303-768-0007 ext. 512",
				@"678.569.2388 ext. 333",
				@"901.684.6605, ext. 122",
				@"901.684.6605, ext. 127",
				@"518.220.9100 x701 ",
				@"518.220.9100 x703 ",
				@"518.220.9100 x707",
				@"518.220.9100 x704",
				@"617-621-3100 x100",
				@"617-621-3100 x101",
				@"617-621-3100 x170",
				@"617-621-3100 x177",
				@"617-621-3100 x105",
				@"614-798-8500 ext.125",
				@"(626) 564-6000 Ext: 3494",
				@"(626) 564-6000 Ext: 3475",
				@"(518) 447-2900 Ext. 6250",
				@"(225) 925-6446 ext. 6467",
				@"(225) 925-6446 ext. 6454",
				@"(225) 925-6446 ext. 6476",
				@"(225) 925-6446 ext. 3910",
				@"(225) 925-6446 ext. 6109",
				@"(440) 924-3600 X120",
				@"(859) 858-3511 x2110",
				@"(859) 858-3511 x2106",
				@"(828) 254-6345 x4028",
				@"(908) 362-6121 x5627",
				@"(908) 362-6121 x5630",
				@"(434) 992-0538 x124",
				@"(434) 992-0510 x134",
				@"(978) 725-6300 x3202",
				@"(626) 449-6853 x2141",
				@"(626) 449-6853 x8951",
				@"(805) 684-4127 x241",
				@"(805) 684-8409 x272",
				@"(805) 684-8409 x208",
				@"(610) 606-4609 x4536",
				@"(610) 606-4609 x3644",
				@"(610) 606-4666 x3312",
				@"(360) 736-9391 x200",
				@"(360) 736-9391 x233",
				@"(360) 736-9391 x644",
				@"(513) 875-3344 x120",
				@"(513) 875-3344 xt139",
				@"(513) 8753344 x114",
				@"(434) 432-2941 x615",
				@"(301) 733-9330 x3014",
				@"(626) 799-5010 x202",
				@"(626) 799-5010 x201",
				@"(626) 799-5010 x200",
				@"(401) 246-1230 x3033",
				@"(401) 246-1230x 302",
				@"(931) 598-5651 x2124",
				@"(507) 786-2222 x3000",
				@"(512) 327-1213 x118",
				@"(512) 327-1213 x132",
				@"(860) 859-1900 x103",
				@"(860) 859-1900 x 103",
				@"(860) 859-1900 x105",
				@"(410) 486-7400 x3011",
				@"(410) 486-7400 x3063",
				@"(410) 486-7400 x3063",
				@"(410) 486-7400 x3035",
				@"(410) 486-7400 x3081",
				@"(727) 562-7800 x7847",
				@"(727) 562-7800 x7297",
				@"(806) 742-2970 x 282",
				@"(860) 868-7334 x283",
				@"(860) 868-7334 x283",
				@"(860) 868-7334 x271",
				@"(860) 868-7334 x279",
				@"(860) 868-7334 x220",
				@"(609) 921-7600 x2270",
				@"(609) 921-7600 x2103",
				@"(609) 921-7600 x2130",
				@"(609) 921-7600 x2203",
				@"(304) 234-4616 x216",
				@"(304) 234-4616 x221",
				@"(304) 234-4616 x225",
				@"(304) 234-4616 x 222",
				@"(304) 234-4616 x217",
				@"(212) 229-5600 x3822",
				@"(212) 229-5667 x3032",
				@"(212) 229-5662 x3600",
				@"(212) 229-5660 x3713",
				@"(805) 640-3201 x242",
				@"(805) 640-3201 x 216",
				@"(805) 640-3201 x254",
				@"(805) 646-4377 x237",
				@"(573) 581-1776 x 227",
				@"(573) 581-1776 x224",
				@"(319) 363-1323 x1660 ",
				@"(319) 363-1323 x1027 ",
				@"(319) 363-1323 ex1605",
				@"(319) 363-1323 x1603",
				@"(423) 323-0201 x3201 ",
				@"(423) 354-5255 x5255",
				@"(518) 523-3357 x203",
				@"(419) 772-2000 x2020",
				@"(215) 576-0800 x129",
				@"(215) 576-0800 x130",
				@"(215) 576-0800 x307",
				@"(215) 576-0800 x130",
				@"(215) 576-0800 x142",
				@"(770) 532-6251  x2188",
				@"301-733-9330 ext. 3015",
				@"(517) 371-5140 x2040",
				@"(517) 371-5140 x4522",
				@"(517) 371-5140 x2208",
				@"(517) 371-5140 x2200",
				@"(207) 282-3361 ext 4402",
				@"(866) 269-5677 x1796",
				@"(909) 748-8171 x8171",
				@"(909) 748-8176 x8176",
				@"(909) 748-8371 x8371",
				@" (610) 326-1000 x7280",
				@"(610) 326-1000 x7240",
				@"(610) 326-1000 x7294",
				@"(260) 562-2131 x234",
				@"(260) 562-2131 x224",
				@"(260) 562-2131 x225",
				@"(260) 562-2131 x224",
				@"(260) 562-2131 x227",
				@"(260) 422-5561 x2114",
				@"(260) 422-5561 x2300",
				@"(312) 427-2737 x664",
				@"(800) 955-4464 x9864",
				@"(570) 208-5900 x5881",
				@"(570) 208-5900 x2394",
				@"(920) 565-1101  x1101",
				@"(920) 565-1023 x2437",
				@"(920) 565-1023 x2152",
				@"(814) 824-2000 x2036",
				@"(814) 824-2392 x2276",
				@"(717) 766-2511 x2055",
				@"(805) 688-5114 x112",
				@"(805) 688-5114 ext. 118",
				@"(805) 688-5114 x119",
				@"(845) 677-6684 x 120 x129",
				@"(845) 677-6684 x121",
				@"(845) 677-8261 x124",
				@"(434) 823-4805 x234",
				@"(434) 823-4805 x211",
				@"(438) 823-4805 x221",
				@"(434) 823-4805 x235",
				@" (800) 231-2391 x7200",
				@"(417) 667-6333 x2136",
				@"(417) 667-6333 x2123",
				@"(417) 667-6333 x2120",
				@"(417) 667-6333 x2123",
				@"(417) 667-6333 x2123",
				@"(610) 282-1100 x1302",
				@"(610) 282-1100 x1265",
				@"(610) 282-1100 x1582",
				@"(203) 254-4000 x2675",
				@"(203) 254-4000 x2256",
				@"(203) 254-4000 x2289",
				@"(334) 272-5820 x7274",
				@"(334) 272-5820 x7168",
				@"(540) 365-4202 x4233",
				@"(540) 365-4202 x4211",
				@"(603) 899-4100 x4244",
				@"(603) 899-4100 x4249",
				@"(207) 935-2001 x3112",
				@"(732) 987-2700 x2252",
				@"(732) 987-2700 x2390",
				@"(732) 987-2700 x2416",
				@"(732) 987-2700 x2218",
				@"(814) 684-3000 x109",
				@"(814) 684-3000 x108",
				@"(814) 684-3000 x119",
				@"(717) 221-1300 x1201",
				@"(607) 431-4000 x4991",
				@"(607) 431-4000 x4344 ",
				@"(920) 885-3373 x 232",
				@"(516) 671-2213 x1102",
				@"(516) 759-2040 x1126",
				@"(562) 907-4200 x4205",
				@"(717) 262-2010 x3404",
				@"(717) 262-2010 x3241",
				@"(717) 262-2010 x3181",
				@"(717) 262-2017 x3311",
				@"(717) 262-2017 x3313",
				@"(212) 960-5400 x6983",
				@"(703) 734-9300 ext. 278",
				@"(866) 888-6563 x315",
				@" (800) 877-5264 ext. 233",
				@" (800) 877-5264 ext. 242",
				@"(973) 635-7070 ext. 207",
				@"(973) 635-7070 ext. 209",
				@"(973) 635-7070 ext. 206",
				@"(310) 556-2502ext 20",
				@"(310) 556-2502ext 14",
				@"310-556-2502, ext. 16",
				@"310-556-2502, ext. 12",
				@"310-556-2502, ext. 22",
				@"3103936300 X6 X1",
				@"450 4658880 X223",
				@"416 5971234 X102",
				@"1 (415) 344-6180 X 1",
				@"(415) 318-7980 X1",
				@"646 5653300 (X _ _ _ _)",
				@"(212) 485-5510   X25510",
				@"(516) 746-4141 ext. 132",
				@"Oxbow Advisors",
				@"(714) 544-7660  X3",
				@"314 9090103 fax (?)",
				@"ext 1006",
				@"(708) 951-0484 alexandria",
				@"903-796-4366 -TX house",
				@"561-746-5844 ex 129",
				@"            X 4793       ",
				@"International Executive",
				@"714 7518812secondary x121",
				@"603-778-2729 home fax",
				@"(562) 946-0150 X 282",
				@"(562) 946-0150 ext.282",
				@"(443)576-0041(work)X805",
				@"781.894.1117  Ext: 204",
				@" 619 298 1976  x314",
				@"512.439.4800 Ext. 847",
				@"(301) 657-1700 ext. 106",
				@"(800) 272-4745 x 223",
				@"(516) 767-7104 x33",
				@"212-477-9626 ext. 10",
				@"303-861-4835 x211",
				@"(303) 694-1900 x308",
				@"888-337-3720 Ext. 143",
				@"(414) 359-1000 Ext. 106 ",
				@"858-792-3800 x214",
				@"216-595-3842 x114 ",
				@"212-452-5900 ext 11",
				@"(973) 425-8420 Ext. 226",
				@"(877) 768-4802 x2",
				@"(904) 399-0662 X209",
				@"858-875-4500 ext. 545",
				@"866-459-2772 ext. 11",
				@"212.871.7100 ext. 311",
				@"(516) 487-8220 Ext:14",
				@"212-486-6670 ext. 241",
				@"813-253-2388 x 223",
				@"212-629-7866 ext.211",
				@"310 3936300 X111",
				@"212 4855500 (Main#) X25510",
				@"760-230-1880 ext 106",
				@"ex 24",
				@"(parts X3 at work)",
				@"(714)263-3600 x202",
				@"615-704-2469 fax too",
				@"(772)489-0401 (not a fax)",
				@"hbixler@chinamail.com",
				@"903 3452071 bank fax #",
				@"(414) 588-7577 Tami Executor",
				@"www.NeXManagement.com",
				@"8005730333x13220",
				@"9494433160x1",
				@"8008263207ext329",
			};

			for (int i = 0; i < tests.Length; i++) {
				Console.Write("\n");
				Console.Write("\nTest #{0}: {1}", i + 1, tests[i]);
				Console.Write("\n=> {0}", TextUtil.BeautifyPhone(tests[i]));
			}
		}
		#endregion Test FileUtilStatic.FixCreationTime

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
		#endregion Test MailAddress

		#region Test FileUtilStatic.FixCreationTime
		//private static void Test_FileUtil_FixCreationTime()
		//{
		//	var searchPattern = @"*";
		//	var tests = new string[] {
		//		@"D:\RCW_Imports",
		//		@"D:\RCW_Imports\03\RCW_Imports\RCW_Imports\bin\Debug\",
		//	};

		//	for (int i = 0; i < tests.Length; i++) {
		//		Console.Write("\n");
		//		Console.Write("\nTest #{0}: {1}", i+1, tests[i]);
		//		FileUtilStatic.FixCreationTime(tests[i], searchPattern);
		//	}
		//}
		#endregion Test FileUtilStatic.FixCreationTime

		#region Test Base36.EncodeIp/DecodeIp
		//private static void Test_Base36_EncodeIp_DecodeIp()
		//{
		//	string output;

		//	var tests = new string[] {
		//		"0.0.0.1",
		//		"1.1.1.1",
		//		"98.189.176.208",
		//		"255.255.255.255",
		//	};

		//	for (int i = 0; i < tests.Length; i++) {
		//		output = Base36.EncodeIp(tests[i]);
		//		Console.Write("\n");
		//		Console.Write("\nInput: {0}", tests[i]);
		//		Console.Write("\nEncoded: {0}", output);
		//		Console.Write("\nDecoded: {0}", Base36.DecodeIp(output));
		//	}
		//}
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
		//private static void Test_Base36_GetUniqueIdByEmail()
		//{
		//	var tests = new string[] {
		//		@"yblizman@rcw1.com",
		//		@" ! ! ! ! ! ! ! ! ",
		//	};

		//	for (int i = 0; i < tests.Length; i++) {
		//		Console.Write("\n");
		//		Console.Write("\nInput: {0}", tests[i]);
		//		Console.Write("\nOutput: {0}", Base36.GetUniqueIdByEmail(tests[i]));
		//	}
		//}
		#endregion Test Base36.GetUniqueIdByEmail()

		//#region Test FileUtilStatic.Write()
		//private static void Test_FileUtil_Write()
		//{
		//	FileUtilStatic.Write("delete me 2018 1023 0122", @"C:\TEMP\_DeleteMe 2018 1023 0122.txt");
		//}
		//#endregion Test FileUtilStatic.Write()

		#region Test FileUtilStatic.GetDirectorySize()
		private static void Test_FileUtil_GetDirectorySize()
		{
			var tests = new string[] {
				@"D:\_DeleteAfter2018_0822\Private\IT\backups\RCW_Imports(dev)\",
				@"D:\_DeleteAfter2018_0822\Public\Jeff's Ads\IT\cdrive clement\RCW_Imports\",
				@"D:\Departments\DevIT\Projects\RCW_Imports\",
			};

			for (int i = 0; i < tests.Length; i++) {
				Console.Write("\n\nTest: {0}\n  DirectorySize: {1}", tests[i], FileUtilStatic.GetDirectorySize(tests[i]));
			}
		}
		#endregion Test FileUtilStatic.GetDirectorySize()

		#region Test ImageUtil.ExistsAndValidByUrl()
		static void Test_ImageUtil_ExistsAndValidByUrl()
		{
			var tests = new string[] {
				"http://www.rarecoinwholesalers.com/Content/Images/Coins/134917fcs.jpg",
				"http://www.rarecoinwholesalers.com/Content/Images/Coins/134917fcs.JPEG",
				"http://www.rarecoinwholesalers.com/Content/Images/Coins/134917fcs.asdf",
			};

			for (int i = 0; i < tests.Length; i++) {
				Console.Write("\n\nTest: {0}\n  ExistsAndValidByUrl: {1}", tests[i], Yutaka.Images.ImageUtil.ExistsAndValidByUrl(tests[i]));
			}
		}
		#endregion Test ImageUtil.ExistsAndValidByUrl()

		#region Start & EndProgram
		static void StartProgram()
		{
			var log = String.Format("Starting {0} program", PROGRAM_NAME);
			logger.Info(log);

			if (consoleOut) {
				Console.Clear();
				Console.Write("{0}{1}", DateTime.Now.ToString(TIMESTAMP), log);
				stopwatch.Start();
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
				Console.Write("\n\n\n\n\n\n\n****************************");
				for (int i = 0; i < log.Length; i++) {
					Console.Write("\n{0}{1}", DateTime.Now.ToString(TIMESTAMP), log[i]);
				}
				Console.Write("\n.... Press any key to close the program ....");
				Console.ReadKey(true);
			}

			Environment.Exit(0); // in case you want to call this method outside of a standard successful program completion, this line will close the app //
		}
		#endregion Start & EndProgram

		#region Deprecated
		[Obsolete("Deprecated June 28, 2019. Use FileManager.Test_YuVideo() instead.", true)]
		private static void Test_YuVideo()
		{
			var deleteFile = false; // true/false //
			consoleOut = true;
			var source = @"asdfasdf\";
			var dest = @"asdfasdf\";

			Directory.CreateDirectory(dest);

			TimeSpan ts, timeRemaining;
			YuVideo vid;
			var videoExtensions = new Regex(".3gp|.asf|.avi|.f4a|.f4b|.f4v|.flv|.m4a|.m4b|.m4r|.m4v|.mkv|.mov|.mp4|.mpeg|.mpg|.webm|.wma|.wmv", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
			var videos = Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories).Where(x => videoExtensions.IsMatch(Path.GetExtension(x))).ToList();
			var videosCount = videos.Count;
			long thisSize = 0;
			long totalSize = 0;
			long processedSize = 0;
			long unprocessedSize = 0;
			var bytesPerSec = 0.0;
			var totalSizeStr = "";
			var processedSizeStr = "";
			var unprocessedSizeStr = "";

			for (int i = 0; i < videosCount; i++) {
				totalSize += new FileInfo(videos[i]).Length;
				totalSizeStr = String.Format("{0:n2} GB", totalSize / 1073741824.0);
			}

			if (consoleOut)
				Console.Write("\ntotalSize: {0}", totalSizeStr);

			if (totalSize > 107374182400) {
				Console.Write("\n******* totalSize > 100 GB. Press any key if you're sure you want to continue *******");
				Console.ReadKey(true);
			}

			stopwatch.Restart();

			for (int i = 0; i < videosCount; i++) {
				vid = new YuVideo(videos[i]);
				thisSize = vid.Size;
				if (consoleOut) {
					Console.Write("\n");
					Console.Write("\n{0}/{1}) {2}", ++totalCount, videosCount, videos[i]);
					Console.Write("\n     CreationTime: {0}", vid.CreationTime);
					Console.Write("\n   LastAccessTime: {0}", vid.LastAccessTime);
					Console.Write("\n    LastWriteTime: {0}", vid.LastWriteTime);
					Console.Write("\n     MediaCreated: {0}", vid.MediaCreated);
					Console.Write("\n     DateReleased: {0}", vid.DateReleased);
					Console.Write("\n      MinDateTime: {0}", vid.MinDateTime);
					Console.Write("\n");
					Console.Write("\n   ParentFolder: {0}", vid.ParentFolder);
					Console.Write("\n   NewFolder: {0}", vid.NewFolder);
					Console.Write("\n   NewFilename: {0}", vid.NewFilename);
					Console.Write("\n   Size: {0:n2} GB", thisSize / 1073741824.0);
				}

				Directory.CreateDirectory(String.Format("{0}{1}", dest, vid.NewFolder));
				_fileUtil.Move(videos[i], String.Format("{0}{1}{2}", dest, vid.NewFolder, vid.NewFilename), deleteFile);
				_fileUtil.Redate(String.Format("{0}{1}{2}", dest, vid.NewFolder, vid.NewFilename), vid.MinDateTime);

				if (consoleOut) {
					ts = stopwatch.Elapsed;
					processedSize += thisSize;
					processedSizeStr = String.Format("{0:n2}", processedSize / 1073741824.0);
					unprocessedSize = totalSize - processedSize;
					unprocessedSizeStr = String.Format("{0:n2} GB", unprocessedSize / 1073741824.0);
					bytesPerSec = processedSize / ts.TotalSeconds;
					timeRemaining = TimeSpan.FromSeconds(unprocessedSize / bytesPerSec);
					Console.Write("\n");
					Console.Write("\n[{0:00}:{1:00}:{2:00}] Processed {3}/{4} ({5:p2})", ts.Hours, ts.Minutes, ts.Seconds, processedSizeStr, totalSizeStr, ((double) processedSize / totalSize));
					Console.Write("\n  MB per second: {0:n2}", bytesPerSec / 1024.0 / 1024.0);
					Console.Write("\n  Approx time remaining: {0:00}:{1:00}:{2:00}", timeRemaining.Hours, timeRemaining.Minutes, timeRemaining.Seconds);
				}
			}
		}

		[Obsolete("Deprecated June 28, 2019. Use FileManager.Test_YuImage() instead.", true)]
		private static void Test_YuImage()
		{
			var deleteFile = false; // true/false //
			consoleOut = !deleteFile;
			var source = @"asdfasdf\";
			var dest = @"asdfasdf\";

			Directory.CreateDirectory(dest);

			YuImage img;
			var imageExtensions = new Regex(".ai|.bmp|.exif|.gif|.jpg|.jpeg|.nef|.png|.psd|.svg|.tiff|.webp", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
			var images = Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories).Where(x => imageExtensions.IsMatch(Path.GetExtension(x))).ToList();
			var imagesCount = images.Count;

			for (int i = 0; i < imagesCount; i++) {
				img = new YuImage(images[i]);
				if (consoleOut) {
					Console.Write("\n");
					Console.Write("\n{0}/{1} ({2})", ++totalCount, imagesCount, ((double) totalCount / imagesCount).ToString("p2"));
					Console.Write("\n{0}", images[i]);
					Console.Write("\n     CreationTime: {0}", img.CreationTime);
					Console.Write("\n        DateTaken: {0}", img.DateTaken);
					Console.Write("\n   LastAccessTime: {0}", img.LastAccessTime);
					Console.Write("\n    LastWriteTime: {0}", img.LastWriteTime);
					Console.Write("\n      MinDateTime: {0}", img.MinDateTime);
					Console.Write("\n");
					Console.Write("\n   DirectoryName: {0}", img.DirectoryName);
					Console.Write("\n   ParentFolder: {0}", img.ParentFolder);
					Console.Write("\n   NewFolder: {0}", img.NewFolder);
					Console.Write("\n   NewFilename: {0}", img.NewFilename);
				}

				Directory.CreateDirectory(String.Format("{0}{1}", dest, img.NewFolder));
				_fileUtil.Move(images[i], String.Format("{0}{1}{2}", dest, img.NewFolder, img.NewFilename), deleteFile);
				_fileUtil.Redate(String.Format("{0}{1}{2}", dest, img.NewFolder, img.NewFilename), img.MinDateTime);
			}

			var count = _fileUtil.DeleteAllThumbsDb(source);
			Console.Write("\n\nDeleted {0} 'Thumbs.db's.", count);
		}
		#endregion Deprecated
	}
}