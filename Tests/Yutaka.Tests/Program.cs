using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using NLog;
using Yutaka.Data;
using Yutaka.Images;
using Yutaka.IO;
using Yutaka.Net;
using Yutaka.Text;
using Yutaka.Utils;
using Yutaka.Video;
using Yutaka.Web;
using System.Text.RegularExpressions;

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
		private static DateTime startTime = DateTime.Now;
		private static FileUtil _fileUtil = new FileUtil();
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static MailUtil _mailUtil = new MailUtil();
		private static SqlUtil _sqlUtil = new SqlUtil();
		private static WebUtil _webUtil = new WebUtil();
		private static int errorCount = 0;
		private static int totalCount = 0;
		private static int errorCountThreshold = 7;
		private static double errorPerThreshold = 0.07;
		private static List<string> bots = new List<string> { "bot","crawler","spider","80legs","baidu","yahoo! slurp","ia_archiver","mediapartners-google","lwp-trivial","nederland.zoek","ahoy","anthill","appie","arale","araneo","ariadne","atn_worldwide","atomz","bjaaland","ukonline","calif","combine","cosmos","cusco","cyberspyder","digger","grabber","downloadexpress","ecollector","ebiness","esculapio","esther","felix ide","hamahakki","kit-fireball","fouineur","freecrawl","desertrealm","gcreep","golem","griffon","gromit","gulliver","gulper","whowhere","havindex","hotwired","htdig","ingrid","informant","inspectorwww","iron33","teoma","ask jeeves","jeeves","image.kapsi.net","kdd-explorer","label-grabber","larbin","linkidator","linkwalker","lockon","marvin","mattie","mediafox","merzscope","nec-meshexplorer","udmsearch","moget","motor","muncher","muninn","muscatferret","mwdsearch","sharp-info-agent","webmechanic","netscoop","newscan-online","objectssearch","orbsearch","packrat","pageboy","parasite","patric","pegasus","phpdig","piltdownman","pimptrain","plumtreewebaccessor","getterrobo-plus","raven","roadrunner","robbie","robocrawl","robofox","webbandit","scooter","search-au","searchprocess","senrigan","shagseeker","site valet","skymob","slurp","snooper","speedy","curl_image_client","suke","www.sygol.com","tach_bw","templeton","titin","topiclink","udmsearch","urlck","valkyrie libwww-perl","verticrawl","victoria","webscout","voyager","crawlpaper","webcatcher","t-h-u-n-d-e-r-s-t-o-n-e","webmoose","pagesinventory","webquest", "weborama","fetcher","webreaper","webwalker","winona","occam","robi","fdse","jobo","rhcs","gazz","dwcp","yeti","fido","wlm","wolp","wwwc","xget","legs","curl","webs","wget","sift","cmc" };
		#endregion

		static void Main(string[] args)
		{
			StartProgram();
			Test_YuImage();
			EndProgram();
		}

		#region Test YuImage
		private static void Test_YuImage()
		{
			var deleteFile = true; // true/false //
			consoleOut = !deleteFile;
			var source = @"G:\Pictures\_Unprocessed\Screenshots\";
			var dest = @"G:\Pictures\";

			Directory.CreateDirectory(dest);

			YuImage img;
			var imageExtensions = new Regex(".ai|.bmp|.exif|.gif|.jpg|.jpeg|.nef|.png|.psd|.svg|.tiff", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
			var images = Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories).Where(x => imageExtensions.IsMatch(Path.GetExtension(x))).ToList();

			for (int i = 0; i < images.Count; i++) {
				img = new YuImage(images[i]);
				if (consoleOut) {
					Console.Write("\n");
					Console.Write("\n{0}) {1}", ++totalCount, images[i]);
					Console.Write("\n     CreationTime: {0}", img.CreationTime);
					Console.Write("\n        DateTaken: {0}", img.DateTaken);
					Console.Write("\n   LastAccessTime: {0}", img.LastAccessTime);
					Console.Write("\n    LastWriteTime: {0}", img.LastWriteTime);
					Console.Write("\n      MinDateTime: {0}", img.MinDateTime);
					Console.Write("\n");
					Console.Write("\n   ParentFolder: {0}", img.ParentFolder);
					Console.Write("\n   NewFolder: {0}", img.NewFolder);
					Console.Write("\n   NewFilename: {0}", img.NewFilename);
				}

				Directory.CreateDirectory(String.Format("{0}{1}", dest, img.NewFolder));
				_fileUtil.Move(images[i], String.Format("{0}{1}{2}", dest, img.NewFolder, img.NewFilename), deleteFile);
				_fileUtil.Redate(String.Format("{0}{1}{2}", dest, img.NewFolder, img.NewFilename), img.MinDateTime);
			}
		}
		#endregion Test YuImage

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