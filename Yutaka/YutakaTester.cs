using System;
using System.IO;
using System.Linq;
using Yutaka.IO;
using Yutaka.Text;

namespace Yutaka
{
	class YutakaTester
	{
		static void Main(string[] args)
		{
			StartProgram();
			TestTextUtilConvertIpToBase36();
			EndProgram();
		}

		static void TestTextUtilConvertIpToBase36()
		{
			var tests = new string[] { "0.0.0.0", "10.0.0.1", "172.16.0.1", "192.168.0.1", "255.255.255.255" };

			foreach (var test in tests) {
				Console.Write("\n\nInput:  {0}\nInt64:  {1}\nBase36: {2}", test, TextUtil.ConvertIpToInt64(test), TextUtil.ConvertIpToBase36(test));
			}
		}

		static void TestTextUtilConvertIpToInt64()
		{
			var tests = new string[] { "0.0.0.0", "10.0.0.1", "172.16.0.1", "192.168.0.1", "255.255.255.255", "asdf", "asdfasdf", "asdfasdfasdfasdfasdf" };

			foreach (var test in tests) {
				Console.Write("\n\nInput:  {0}\nOutput: {1}", test, TextUtil.ConvertIpToInt64(test));
			}
		}

		static void StartProgram()
		{
			Console.Clear();
		}

		static void Process()
		{
			var images = FileUtil.EnumerateImageFiles(path: @"C:\Pictures", searchOption: SearchOption.AllDirectories);
			var songs = FileUtil.EnumerateAudioFiles(path: @"Y:\Music\00 Genres", searchOption: SearchOption.AllDirectories);

			Console.Write("\n");

			//foreach (var v in images) {
			//	Console.Write("\n{0}", v.FullName);
			//}

			Console.Write("\n");

			//foreach (var v in songs) {
			//	Console.Write("\n{0}", v.FullName);
			//}

			Console.Write("\nimagesCount: {0}", images.Count());
			Console.Write("\nsongsCount: {0}", songs.Count());
		}

		static void EndProgram()
		{
			Console.Write("\n\n.... Press any key to exit ....\n\n");
			Console.ReadKey(true);
		}
	}
}