using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace SpeedTester
{
	class Program
	{
		private static readonly int iterations = 1000000; // 1 million is usually a good starting point. Adjust as needed

		#region Fields
		private const string TIMESTAMP = "HH:mmtt";
		private static readonly DateTime StartTime = DateTime.Now;
		private static readonly HashSet<string> imageExtensionsHashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".ai", ".bmp", ".eps", ".exif", ".gif", ".ico", ".jpeg", ".jpg", ".nef", ".png", ".psd", ".svg", ".tiff", ".webp", };
		private static readonly Regex imageExtensionsRegex = new Regex(".ai|.bmp|.eps|.exif|.gif|.ico|.jpeg|.jpg|.nef|.png|.psd|.svg|.tiff|.webp", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
		private static readonly string path = @"asdfasdf\";
		private static readonly string[] imageExtensionsArray = { ".ai", ".bmp", ".eps", ".exif", ".gif", ".ico", ".jpeg", ".jpg", ".nef", ".png", ".psd", ".svg", ".tiff", ".webp", };
		private static Stopwatch sw = new Stopwatch();
		#endregion Fields

		static void Main(string[] args)
		{
			StartProgram();
			// Run once just in case there's an initial performance cost
			sw.Start();
			Thread.Sleep(1);
			sw.Stop();

			Test1();
			Test2();
			Test3();
			Test4();
			Test5();
			Test6();
			Test1();
			Test2();
			Test3();
			Test4();
			Test5();
			Test6();
			Test1();
			Test2();
			Test3();
			Test4();
			Test5();
			Test6();
			EndProgram();
		}

		private static void Test1()
		{
			sw.Reset();
			sw.Start();
			var count = 0;
			var files = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).Where(x => imageExtensionsHashSet.Contains(Path.GetExtension(x)));

			for (int i = 0; i < files.Count(); i++)
				count++;

			sw.Stop();
			Console.Write("\n");
			Console.Write("Count: {0}\n", count);
			Console.Write("Directory.EnumerateFiles() HashSet took {0}\n", sw.Elapsed.ToString(@"hh\:mm\:ss\.fff"));
		}

		private static void Test2()
		{
			sw.Reset();
			sw.Start();
			var count = 0;
			var files = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).Where(x => imageExtensionsRegex.IsMatch(Path.GetExtension(x)));

			for (int i = 0; i < files.Count(); i++)
				count++;

			sw.Stop();
			Console.Write("\n");
			Console.Write("Count: {0}\n", count);
			Console.Write("Directory.EnumerateFiles() Regex took {0}\n", sw.Elapsed.ToString(@"hh\:mm\:ss\.fff"));
		}

		private static void Test3()
		{
			sw.Reset();
			sw.Start();
			var count = 0;
			var files = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).Where(x => imageExtensionsArray.Contains(Path.GetExtension(x), StringComparer.OrdinalIgnoreCase));

			for (int i = 0; i < files.Count(); i++)
				count++;

			sw.Stop();
			Console.Write("\n");
			Console.Write("Count: {0}\n", count);
			Console.Write("Directory.EnumerateFiles() array took {0}\n", sw.Elapsed.ToString(@"hh\:mm\:ss\.fff"));
		}

		private static void Test4()
		{
			sw.Reset();
			sw.Start();
			var count = 0;
			var files = new DirectoryInfo(path).EnumerateFiles("*", SearchOption.AllDirectories).Where(x => imageExtensionsHashSet.Contains(x.Extension));

			for (int i = 0; i < files.Count(); i++)
				count++;

			sw.Stop();
			Console.Write("\n");
			Console.Write("Count: {0}\n", count);
			Console.Write("DirectoryInfo.EnumerateFiles() HashSet took {0}\n", sw.Elapsed.ToString(@"hh\:mm\:ss\.fff"));
		}

		private static void Test5()
		{
			sw.Reset();
			sw.Start();
			var count = 0;
			var files = new DirectoryInfo(path).EnumerateFiles("*", SearchOption.AllDirectories).Where(x => imageExtensionsRegex.IsMatch(x.Extension));

			for (int i = 0; i < files.Count(); i++)
				count++;

			sw.Stop();
			Console.Write("\n");
			Console.Write("Count: {0}\n", count);
			Console.Write("DirectoryInfo.EnumerateFiles() Regex took {0}\n", sw.Elapsed.ToString(@"hh\:mm\:ss\.fff"));
		}

		private static void Test6()
		{
			sw.Reset();
			sw.Start();
			var count = 0;
			var files = new DirectoryInfo(path).EnumerateFiles("*", SearchOption.AllDirectories).Where(x => imageExtensionsArray.Contains(x.Extension, StringComparer.OrdinalIgnoreCase));

			for (int i = 0; i < files.Count(); i++)
				count++;

			sw.Stop();
			Console.Write("\n");
			Console.Write("Count: {0}\n", count);
			Console.Write("DirectoryInfo.EnumerateFiles() array took {0}\n", sw.Elapsed.ToString(@"hh\:mm\:ss\.fff"));
		}

		#region StartProgram & EndProgram
		private static void StartProgram()
		{
			Console.Clear();
			Console.Write("Start time: {0}\n\n", StartTime.ToString(TIMESTAMP).ToLower());
		}

		private static void EndProgram()
		{
			Console.Write("\n\n\n\n\n\n");
			Console.Write("\nStart time: {0}", StartTime.ToString(TIMESTAMP).ToLower());
			Console.Write("\n  End time: {0}", DateTime.Now.ToString(TIMESTAMP).ToLower());
			Console.Write("\n\n.... Press any key to close the program ....");
			Console.ReadKey(true);
		}
		#endregion StartProgram & EndProgram
	}
}