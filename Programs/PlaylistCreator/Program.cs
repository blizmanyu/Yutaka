using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PlaylistCreator
{
	class Program
	{
		// Config/Settings //
		const string srcFolder = @"H:\Music\00 Genres\";
		const string playlistFolder = @"H:\Music\01 Playlists\";
		private static readonly bool doEnglish = false;
		private static readonly bool doJPopFallWinter = true;
		private static readonly bool doJPopSpringSummer = true;
		private static readonly DateTime newSongThreshold = DateTime.Now.AddYears(-2);
		private static readonly HashSet<string> supportedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".mp3", ".m4a", ".wav", ".wma" };
		private static bool consoleOut = true; // default = false

		#region Fields
		const string EN_US = @"M/d/yyyy h:mmtt";
		private static readonly DateTime startTime = DateTime.Now;
		#region private static readonly HashSet<string> badArtists = new HashSet<string>() {
		private static readonly HashSet<string> badArtists = new HashSet<string>() {
			"Britney Spears",
			"Felix Mendelssohn",
			"Justin Bieber",
			"Ke$ha",
			"Miley Cyrus",
			"Sergey Rachmaninov",
			"Wolfgang Amadeus Mozart",
		};
		#endregion
		#endregion Fields

		static void Main(string[] args)
		{
			StartProgram(args);
			CheckFolders();
			var openFolder = true;
			var destFolder = playlistFolder;

			#region English
			if (doEnglish) {
				var EnglishPlaylist = new Playlist(Playlist.PType.English);
				EnglishPlaylist.Create(@"H:\Music\00 Genres\");
				EnglishPlaylist.WriteForWinamp(destFolder);
				EnglishPlaylist.WriteForITunes(destFolder);

				if (openFolder) {
					Process.Start("explorer.exe", destFolder);
					openFolder = false;
				}
			}
			#endregion English

			#region JPopSpringSummer
			if (doJPopSpringSummer) {
				var playlist1 = new Playlist(Playlist.PType.JPopSpringSummer);
				playlist1.Create(@"H:\Music\00 Genres\J-Pop\");
				playlist1.WriteForWinamp(destFolder);
				playlist1.WriteForITunes(destFolder);

				if (openFolder) {
					Process.Start("explorer.exe", destFolder);
					openFolder = false;
				}
			}
			#endregion JPopSpringSummer

			#region JPopFallWinter
			if (doJPopFallWinter) {
				var playlist2 = new Playlist(Playlist.PType.JPopFallWinter);
				playlist2.Create(@"H:\Music\00 Genres\J-Pop\");
				playlist2.WriteForWinamp(destFolder);
				playlist2.WriteForITunes(destFolder);

				if (openFolder) {
					Process.Start("explorer.exe", destFolder);
					openFolder = false;
				}
			}
			#endregion JPopFallWinter

			EndProgram();
		}

		#region Methods
		private static void CheckFolders()
		{
			string[] folders = { @"C:\Music\", @"H:\Music\01 Playlists" };

			for (int i=0; i<folders.Length; i++)
				Directory.CreateDirectory(folders[i]);
		}

		#region StartProgram & EndProgram
		private static void StartProgram(string[] args)
		{
			if (args != null && args.Length > 0)
				if (args.Contains("-cons", StringComparer.OrdinalIgnoreCase))
					consoleOut = true;

			if (consoleOut) {
				Console.Clear();
				Console.Write("Program started at: {0}\n", startTime.ToString(EN_US).ToLower());
				Console.Write("        consoleOut: {0}\n", consoleOut.ToString().ToUpper());
			}
		}

		private static void EndProgram()
		{
			var endTime = DateTime.Now;
			var ts = endTime - startTime;

			if (consoleOut) {
				Console.Write("\n");
				Console.Write("\nProgram ended at: {0}", endTime.ToString(EN_US).ToLower());
				Console.Write("\nIt took: ");
				if (ts.TotalMinutes >= 60)
					Console.Write("{0}hr ", ts.Hours);
				if (ts.TotalSeconds >= 60)
					Console.Write("{0}min ", ts.Minutes);
				Console.Write("{0}sec to complete", ts.Seconds);
				Console.Write("\n");
				Console.Write("\n... Press any key to exit ...");
				Console.ReadKey(true);
			}
		}
		#endregion StartProgram & EndProgram
		#endregion
	}
}