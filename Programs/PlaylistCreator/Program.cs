using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Yutaka.IO;

namespace PlaylistCreator
{
	class Program
	{
		// Config/Settings //
		const string srcFolder = @"C:\Music\00 Genres\";
		const string playlistFolder = @"C:\Music\01 Playlists\";
		private static bool consoleOut = true; // default = false
		private static bool doEnglish = false;
		private static bool doJpop = true;
		private static bool doJpopSpringSummer = true;
		private static bool doJpopFallWinter = false;
		private static DateTime newSongThreshold = DateTime.Now.AddYears(-2);
		private static HashSet<string> supportedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".mp3", ".m4a", ".wma" };

		#region Fields
		const string EN_US = @"M/d/yyyy h:mmtt";
		private static FileUtil _fileUtil = new FileUtil();
		private static List<SongFileInfo> playlist = new List<SongFileInfo>();
		private static List<SongFileInfo> allSongs = new List<SongFileInfo>();
		private static List<SongFileInfo> goodList = new List<SongFileInfo>();
		private static List<SongFileInfo> newList = new List<SongFileInfo>();
		private static List<SongFileInfo> newPlusGoodList = new List<SongFileInfo>();
		private static DateTime startTime = DateTime.Now;

		private static HashSet<string> goodSongs = new HashSet<string>() {
			"All The Shine",
			"All Of Me",
			"Blank Space",
			"Breakeven",
			"Clarity [ft Foxes]",
			"Different",
			"Everybody Knows",
			"Feels Like Home",
			"first day of my life", // gnash //
			"Give A Little More",
			"Hands Down (Acoustic)",
			"Heartbeat",
			"i hate you, i love you (PBH & Jack Shizzle Remix)",
			"It Might Be You",
			"Kiss From A Rose",
			"Like A Stone",
			"Lost & Found",
			"Love Lost",
			"Must Get Out",
			"Never Gonna Leave This Bed (Acoustic)",
			"Never Too Late",
			"nothing lasts forever",
			"One Last Time",
			"Ordinary People",
			"Our Youth",
			"Panic Switch",
			"Piece Of Me",
			"Say You Love Me",
			"Style",
			"Style (Cover)",
			"Sunday Morning",
			"Sweetest Goodbye",
			"The Sun",
			"Thinkin Bout You",
			"Through With You",
			"Who You Are",
			"Wildest Dreams",
			"Wildest Moments",
			"With Or Without You",
			"won't go home without you",
		};

		private static HashSet<string> folderExclusions = new HashSet<string>() { @"\Album", @"\Classical", @"\J-Pop", @"\J-Rap", @"\Spanish", };

		#region Exclude Artists
		private static HashSet<string> badArtists = new HashSet<string>() {
			"Britney Spears",
			"Felix Mendelssohn",
			"Justin Bieber",
			"Ke$ha",
			"Miley Cyrus",
			"Sergey Rachmaninov",
			"Wolfgang Amadeus Mozart",
		};
		#endregion

		#region J-Pop
		private static List<SongFileInfo> playlistJpopFallWinter = new List<SongFileInfo>();
		private static List<SongFileInfo> playlistJpopSpringSummer = new List<SongFileInfo>();
		private static List<SongFileInfo> goodListJpop = new List<SongFileInfo>();
		private static List<SongFileInfo> jpopChristmas = new List<SongFileInfo>();
		private static List<SongFileInfo> jpopFallWinter = new List<SongFileInfo>();
		private static List<SongFileInfo> jpopSpringSummer = new List<SongFileInfo>();
		private static HashSet<string> folderExclusionsJpop = new HashSet<string>() { @"\_Album", @"_Christmas", @"_FallWinter", @"_SpringSummer" };
		private static HashSet<string> goodSongsJpop = new HashSet<string>() {
			"Floatin'", // Chemistry //
			"It Takes Two", // Chemistry //
			"Pieces of a Dream", // Chemistry //
			"Point of No Return", // Chemistry //
			"fukai mori", // Do As Infinity //
			"Oasis", // Do As Infinity //
			"Under the Moon", // Do As Infinity //
			"Yesterday & Today", // Do As Infinity //
			"grateful days", // Dragon Ash //
			"your eyes only", // Exile //
			"Be With You", // Glay //
			"Beloved", // Glay //
			"However", // Glay //
			"Kiseki no Hate", // Glay //
			"Pure Soul", // Glay //
			"Face", // Globe //
			"Faces Places", // Globe //
			"Perfume of Love", // Globe //
			"asu e no tobira", // I WiSH //
			"Everything (It's you)", // Mr.Children //
			"hana", // Mr.Children //
			"kuchibue", // Mr.Children //
			"kurumi", // Mr.Children //
			"mirai", // Mr.Children //
			"namonaki uta", // Mr.Children //
			"owarinaki tabi", // Mr.Children //
			"te no hira", // Mr.Children //
			"yasashii uta", // Mr.Children //
			"youthful days", // Mr.Children //
			"fly", // Smap //
			"Sekai ga Owaru Madewa", // Wands //
			"Sekaijuu no Dare Yori Kitto", // Wands //
		};
		#endregion J-Pop
		#endregion Fields

		static void Main(string[] args)
		{
			StartProgram(args);
			CheckFolders();
			var openFolder = true;
			var destFolder = playlistFolder;

			#region English
			var EnglishPlaylist = new Playlist(Playlist.PType.English);
			EnglishPlaylist.Create(@"C:\Music\00 Genres\");
			EnglishPlaylist.WriteForWinamp(destFolder);
			EnglishPlaylist.WriteForITunes(destFolder);

			if (openFolder) {
				Process.Start("explorer.exe", destFolder);
				openFolder = false;
			}
			#endregion English

			#region JPopSpringSummer
			var playlist1 = new Playlist(Playlist.PType.JPopSpringSummer);
			playlist1.Create(@"C:\Music\00 Genres\J-Pop\");
			playlist1.WriteForWinamp(destFolder);
			playlist1.WriteForITunes(destFolder);

			if (openFolder) {
				Process.Start("explorer.exe", destFolder);
				openFolder = false;
			}
			#endregion JPopSpringSummer

			#region JPopFallWinter
			var playlist2 = new Playlist(Playlist.PType.JPopFallWinter);
			playlist2.Create(@"C:\Music\00 Genres\J-Pop\");
			playlist2.WriteForWinamp(destFolder);
			playlist2.WriteForITunes(destFolder);

			if (openFolder) {
				Process.Start("explorer.exe", destFolder);
				openFolder = false;
			}
			#endregion JPopFallWinter

			//if (doJpop) {
			//	DoJpop();

			//	if (openFolder) {
			//		Process.Start("explorer.exe", @"C:\Music\");
			//		openFolder = false;
			//	}
			//}

			//if (doEnglish) {
			//	DoEnglish();

			//	if (openFolder) {
			//		Process.Start("explorer.exe", @"C:\Music\");
			//		openFolder = false;
			//	}
			//}

			EndProgram();
		}

		#region Methods
		private static void DoEnglish()
		{
			GetAllSongs();
			CreateGoodList();
			RemoveExclusionArtists();
			CreateNewList();
			CreateNewPlusGoodList();
			var writeGoodList = true;
			#region Sort & Create Good List
			if (writeGoodList) {
				goodList = goodList.OrderBy(x => x.Title).ThenBy(y => y.AlbumArtist).ToList();
				WritePlaylistM3U(goodList, "Good");
				WritePlaylistITunes(goodList, "Good");
			}
			#endregion
			CreatePlaylist();
			WritePlaylistM3U(playlist, "All");
			WritePlaylistITunes(playlist, "All");
			//WriteHtmlFile(playlist, "All");
		}

		#region J-Pop
		private static void DoJpop()
		{
			GetAllSongsJpop();
			CreateGoodListJpop();
			CreatePlaylistJpop();
			//WritePlaylistM3U(playlistJpopFallWinter, "J-Pop Fall Winter");
			//WritePlaylistITunes(playlistJpopFallWinter, "J-Pop Fall Winter");
			WritePlaylistM3U(playlistJpopSpringSummer, "J-Pop Spring Summer");
			WritePlaylistITunes(playlistJpopSpringSummer, "J-Pop Spring Summer");
			//WriteHtmlFile(playlist, "All");
		}

		private static void GetAllSongsJpop()
		{
			List<string> files;
			string[] exclusions;
			var folder = @"C:\Music\00 Genres\J-Pop\";

			exclusions = new string[] { @"\_Album", @"_Christmas", @"_SpringSummer" };
			files = _fileUtil.GetAllAudioFiles(folder, exclusions);
			for (int i = 0; i < files.Count; i++)
				jpopFallWinter.Add(new SongFileInfo(files[i]));

			exclusions = new string[] { @"\_Album", @"_Christmas", @"_FallWinter" };
			files = _fileUtil.GetAllAudioFiles(folder, exclusions);
			for (int i = 0; i < files.Count; i++)
				jpopSpringSummer.Add(new SongFileInfo(files[i]));
		}

		private static void CreateGoodListJpop()
		{
			goodListJpop = jpopFallWinter.Where(x => goodSongsJpop.Contains(x.Title, StringComparer.OrdinalIgnoreCase)).OrderBy(x => x.Title).ThenBy(y => y.Artist).ToList();
			jpopFallWinter = jpopFallWinter.Except(goodListJpop).ToList();

			if (consoleOut) {
				for (int i = 0; i < goodListJpop.Count; i++)
					Console.Write("\n{0}) {1} - {2}", i + 1, goodListJpop[i].Artist, goodListJpop[i].Title);
			}
		}

		private static void CreatePlaylistJpop()
		{
			try {
				int goodInd;
				var goodListJpopCount = goodListJpop.Count;
				Console.Write("\ngoodListJpopCount: {0}", goodListJpopCount);

				// Fall/Winter //
				jpopFallWinter = jpopFallWinter.OrderBy(x => x.Title).ThenBy(y => y.Artist).ToList();
				goodInd = 0;

				for (int i = 0; i < jpopFallWinter.Count; i+=3) {
					if (goodInd == goodListJpopCount)
						goodInd = 0;
					playlistJpopFallWinter.Add(goodListJpop[goodInd]);
					goodInd++;
					playlistJpopFallWinter.Add(jpopFallWinter[i]);
					try {
						playlistJpopFallWinter.Add(jpopFallWinter[i + 1]);
					}
					catch (Exception) { }
					try {
						playlistJpopFallWinter.Add(jpopFallWinter[i + 2]);
					}
					catch (Exception) { }
				}
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in PlaylistCreator.CreatePlaylistJpop().{2}{1}", ex.Message, ex.ToString(), Environment.NewLine));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of PlaylistCreator.CreatePlaylistJpop().{2}{1}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine));
			}

			try {
				int goodInd;
				var goodListJpopCount = goodListJpop.Count;
				Console.Write("\ngoodListJpopCount: {0}", goodListJpopCount);

				// Spring/Summer //
				jpopSpringSummer = jpopSpringSummer.OrderBy(x => x.Title).ThenBy(y => y.Artist).ToList();
				goodInd = 0;

				for (int i = 0; i < jpopSpringSummer.Count; i += 3) {
					if (goodInd == goodListJpopCount)
						goodInd = 0;
					playlistJpopSpringSummer.Add(goodListJpop[goodInd]);
					goodInd++;
					playlistJpopSpringSummer.Add(jpopSpringSummer[i]);
					try {
						playlistJpopSpringSummer.Add(jpopSpringSummer[i + 1]);
					}
					catch (Exception) { }
					try {
						playlistJpopSpringSummer.Add(jpopSpringSummer[i + 2]);
					}
					catch (Exception) { }
				}
			}

			catch (Exception ex) {
				if (ex.InnerException == null) {
					Console.Write("\n{0}{2}Exception thrown in PlaylistCreator.CreatePlaylistJpop().{2}{1}{2}", ex.Message, ex.ToString(), Environment.NewLine);
					Console.Write("\n... Press any key to continue ...");
					Console.ReadKey(true);
					throw new Exception(String.Format("{0}{2}Exception thrown in PlaylistCreator.CreatePlaylistJpop().{2}{1}{2}", ex.Message, ex.ToString(), Environment.NewLine));
				}

				Console.Write("\n{0}{2}Exception thrown in INNER EXCEPTION of PlaylistCreator.CreatePlaylistJpop().{2}{1}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);
				Console.Write("\n... Press any key to continue ...");
				Console.ReadKey(true);
				throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of PlaylistCreator.CreatePlaylistJpop().{2}{1}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine));
			}
		}
		#endregion J-Pop

		private static void CheckFolders()
		{
			string[] folders = { @"C:\Music\", @"C:\Music\01 Playlists" };

			for (int i=0; i<folders.Length; i++)
				if (!Directory.Exists(folders[i]))
					Directory.CreateDirectory(folders[i]);
		}

		private static void GetAllSongs()
		{
			//var srcFolder = @"C:\Music\"; // for testing only //
			var files = _fileUtil.GetAllAudioFiles(srcFolder, folderExclusions.ToArray());
			for (int i = 0; i < files.Count; i++)
				allSongs.Add(new SongFileInfo(files[i]));
		}

		private static void RemoveExclusionArtists()
		{
			allSongs = allSongs.Except(allSongs.Where(x => badArtists.Contains(x.Artist, StringComparer.OrdinalIgnoreCase)).ToList()).ToList();
		}

		private static void CreateGoodList()
		{
			goodList = allSongs.Where(x => goodSongs.Contains(x.Title, StringComparer.OrdinalIgnoreCase)).ToList();
			allSongs = allSongs.Except(goodList).ToList();
		}

		private static void CreateNewList()
		{
			newList = allSongs.Where(x => newSongThreshold < x.Date).ToList();
			allSongs = allSongs.Except(newList).ToList();

			#region Logging
			if (consoleOut) {
				Console.Write("\n{0}: {1} songs", newSongThreshold.ToShortDateString(), newList.Count);
				Console.Write("\n");
				Console.Write("\n NewList.Count: {0}", newList.Count);
				Console.Write("\nAllSongs.Count: {0}", allSongs.Count);
			}
			#endregion
		}

		private static void CreateNewPlusGoodList()
		{
			newPlusGoodList.AddRange(newList);
			newPlusGoodList.AddRange(goodList);
			newPlusGoodList = newPlusGoodList.OrderBy(x => x.Title).ThenBy(y => y.Artist).ToList();

			if (consoleOut) {
				Console.Write("\n");
				for (int i = 0; i < newPlusGoodList.Count; i++)
					Console.Write("\n{0}) {1} - {2}", i + 1, newPlusGoodList[i].Artist, newPlusGoodList[i].Title);

				Console.Write("\n");
				Console.Write("\nNewPlusGoodList.Count: {0}", newPlusGoodList.Count);
				Console.Write("\n       AllSongs.Count: {0}", allSongs.Count);
			}
		}

		private static void CreatePlaylist()
		{
			var newPlusGoodListCount = newPlusGoodList.Count;
			if (consoleOut) {
				Console.Write("\n\n\n\n\n\n\n");
				Console.Write("\nnewPlusGoodListCount: {0}", newPlusGoodListCount);
			}

			allSongs = allSongs.OrderBy(x => x.Title).ThenBy(y => y.Artist).ToList();
			var goodInd = 0;

			for (int i = 0; i < allSongs.Count - 2; i++) {
				if (goodInd == newPlusGoodListCount)
					goodInd = 0;
				playlist.Add(newPlusGoodList[goodInd]);
				goodInd++;
				playlist.Add(allSongs[i]);
				playlist.Add(allSongs[i + 1]);
				playlist.Add(allSongs[i + 2]);
				i = i + 2;
			}

			if (consoleOut) {
				Console.Write("\n");
				var temp = newPlusGoodListCount * 2;
				for (int i = 0; i < playlist.Count; i++) {
					if (i % temp == 0 && i / temp > 0)
						Console.Write("\n\n============================\n");
					Console.Write("\n{0}) {1} - {2}", i + 1, playlist[i].Artist, playlist[i].Title);
				}
			}
		}

		private static void WritePlaylistITunes(List<SongFileInfo> songs, string filename, bool timestamp = true)
		{
			var dest = String.Format("{0}{1}{2}.txt", playlistFolder, filename, timestamp ? DateTime.Now.ToString(" yyyy MMdd HHmm ssff") : "");
			var fileHeader = "Name\tArtist\tComposer\tAlbum\tGrouping\tGenre\tSize\tTime\tDisc Number\tDisc Count\tTrack Number\tTrack Count\tYear\tDate Modified\tDate Added\tBit Rate\tSample Rate\tVolume Adjustment\tKind\tEqualizer\tComments\tPlays\tLast Played\tSkips\tLast Skipped\tMy Rating\tLocation";
			File.WriteAllText(dest, fileHeader, Encoding.Default);

			foreach (var song in songs)
				File.AppendAllText(dest, "\n" + song.Title + "\t" + song.Artist + "\t\t" + song.Album + "\t\t" + song.Genre + "\t\t" + song.Duration + "\t" + song.DiscNum + "\t1\t" + song.TrackNum + "\t\t" + song.Year + "\t\t\t\t\t\t\t\t\t0\t\t\t\t\t" + song.Path, Encoding.Default);

			File.AppendAllText(dest, "\n", Encoding.Default);
		}

		private static void WritePlaylistM3U(List<SongFileInfo> songs, string filename, bool timestamp = true)
		{
			var dest = String.Format("{0}{1}{2}.m3u", playlistFolder, filename, timestamp ? DateTime.Now.ToString(" yyyy MMdd HHmm ssff") : "");
			var fileHeader = "#EXTM3U";
			File.WriteAllText(dest, fileHeader, Encoding.Default);

			foreach (var song in songs) {
				File.AppendAllText(dest, "\n#EXTINF:" + song.Duration + "," + song.Artist + " - " + song.Title, Encoding.Default);
				File.AppendAllText(dest, "\n" + song.Path, Encoding.Default);
			}

			File.AppendAllText(dest, "\n", Encoding.Default);
		}

		private static void WriteHtmlFile(List<SongFileInfo> songs, string filename, bool timestamp = true)
		{
			//var url = "https://musicbrainz.org/ws/2/release?query=";
			var googleUrl = "https://www.google.com/search?q=";
			songs = songs.OrderBy(x => x.Title).ThenBy(y => y.Artist).ToList();

			var dest = String.Format("{0}{1}{2}.html", playlistFolder, filename, timestamp ? DateTime.Now.ToString(" yyyy MMdd HHmm ssff") : "");
			var fileHeader = "";
			File.WriteAllText(dest, fileHeader, Encoding.Default);

			foreach (var song in songs) {
				var comment = song.Comment ?? "";
				if (!comment.Contains("Release")) {
					File.AppendAllText(dest, String.Format("{0} - {1}<br/>", song.Artist, song.Title), Encoding.Default);
					File.AppendAllText(dest, String.Format("<a href='{0}{1} {2} song wiki' target=\"_blank\">Google</a><br/>", googleUrl, WebUtility.UrlEncode(song.Title), WebUtility.UrlEncode(song.Artist)), Encoding.Default);
					//File.AppendAllText(dest, String.Format("<a href='{0}\"{1}\" AND artist:\"{2}\" AND primarytype:single' target=\"_blank\">MusicBrainz</a><br/>", url, song.Title.Replace("'", "%27").Replace("&", "%26"), song.Artist.Replace("&", "%26").Replace("'", "%27")), Encoding.Default);
					//File.AppendAllText(dest, String.Format("<a href='{0}\"{1}\" AND artistname:\"{2}\" AND primarytype:single' target=\"_blank\">MusicBrainz</a><br/><br/>", url, song.Title.Replace("&", "%26").Replace("'", "%27"), song.Artist.Replace("&", "%26").Replace("'", "%27")), Encoding.Default);
				}
			}
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