using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.IO
{
	class Program
	{
		static void Main(string[] args)
		{
			var source = @"C:\TestSource\test.txt";
			var dest   = @"C:\TestDest\test" + DateTime.Now.ToString("yyyy MMdd HHmm ssff") + ".txt";
			FileHelper.CopyFile(source, dest);
			EndProgram();
		}

		private static void EndProgram()
		{
			Console.Write("\n");
			Console.Write("\n... Press any key to exit ... ");
			Console.ReadKey(true);
		}
	}
}
