using System;
using System.Diagnostics;
using System.Threading;

namespace SpeedTester
{
	class Program
	{
		private static readonly int iterations = 1000000; // 1 million is usually a good starting point. Adjust as needed

		#region Fields
		private const string TIMESTAMP = "HH:mmtt";
		private static readonly DateTime StartTime = DateTime.Now;
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
			EndProgram();
		}

		private static void Test1()
		{
			sw.Reset();
			sw.Start();

			for (var i = 0; i < iterations; i++) {
				;
			}

			sw.Stop();
			Console.Write("Test1 took {0}\n", sw.Elapsed.ToString(@"hh\:mm\:ss\.fff"));
		}

		private static void Test2()
		{
			sw.Reset();
			sw.Start();

			for (var i = 0; i < iterations; i++) {
				;
			}

			sw.Stop();
			Console.Write("Test2 took {0}\n", sw.Elapsed.ToString(@"hh\:mm\:ss\.fff"));
		}

		private static void Test3()
		{
			sw.Reset();
			sw.Start();

			for (var i = 0; i < iterations; i++) {
				;
			}

			sw.Stop();
			Console.Write("Test3 took {0}\n", sw.Elapsed.ToString(@"hh\:mm\:ss\.fff"));
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