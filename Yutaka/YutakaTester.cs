using System;
using System.IO;
using System.Linq;
using Yutaka.IO;

namespace Yutaka
{
	class YutakaTester
	{
		static void Main(string[] args)
		{
			StartProgram();
			Process();
			EndProgram();
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