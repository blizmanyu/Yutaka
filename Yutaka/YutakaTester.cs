using System;
using System.IO;
using Yutaka.IO;
using FileUtil = Yutaka.FileUtil;

namespace Yutaka
{
	class YutakaTester
	{
		static void Main(string[] args)
		{
			var paths = new string[] { @"\\DC-RCW\Public\images\Coins\134637b.jpg", @"\\DC-RCW\Public\images\Coins\134671b.jpg", @"\\DC-RCW\Public\images\Coins\134637bc.jpg" };
			var fileInfos = new FileInfo[] { new FileInfo(paths[0]), new FileInfo(paths[1]), new FileInfo(paths[2]) };

			Console.Write("\n\n======= IsSameDate(FileInfo fi1, FileInfo fi2) Test =======");
			//Console.Write("\n{0}: {1}", FileUtil.IsSameDate(v));
		}
	}
}