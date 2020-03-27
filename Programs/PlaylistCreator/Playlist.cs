using System;
using System.Collections.Generic;
using System.Linq;
using Yutaka.IO;

namespace PlaylistCreator
{
	public class Playlist
	{
		#region Fields
		public enum PType { English, JPopFallWinter, JPopSpringSummer };
		public PType Type;
		public string Name;
		#region public List<SongFileInfo> GoodEnglishSongs = new List<SongFileInfo> {
		public List<SongFileInfo> GoodEnglishSongs = new List<SongFileInfo> {
			new SongFileInfo("Acceptance", "Different"),
			new SongFileInfo("Ariana Grande", "One Last Time"),
			new SongFileInfo("Audioslave", "Like A Stone"),
			new SongFileInfo("Avicii", "Waiting For Love"),
			new SongFileInfo("Childish Gambino", "All The Shine"),
			new SongFileInfo("Childish Gambino", "Heartbeat"),
			new SongFileInfo("Childish Gambino", "IV. Sweatpants"),
			new SongFileInfo("Dashboard Confessional", "Hands Down (Acoustic)"),
			new SongFileInfo("Foo Fighters", "Everlong"),
			new SongFileInfo("Frank Ocean", "Thinkin Bout You"),
			new SongFileInfo("Gnash", "first day of my life"),
			new SongFileInfo("Gnash", "i hate you, i love you (PBH & Jack Shizzle Remix)"),
			new SongFileInfo("Gwen Stefani", "Cool"),
			new SongFileInfo("Howie Day", "Collide"),
			new SongFileInfo("Jessie J", "Who You Are"),
			new SongFileInfo("Jessie Ware", "Say You Love Me"),
			new SongFileInfo("Jessie Ware", "Wildest Moments"),
			new SongFileInfo("John Legend", "All Of Me"),
			new SongFileInfo("John Legend", "Everybody Knows"),
			new SongFileInfo("John Legend", "Ordinary People"),
			new SongFileInfo("Kai", "It Might Be You"),
			new SongFileInfo("Lianne La Havas", "Lost & Found"),
			new SongFileInfo("Lily Allen", "Somewhere Only We Know"),
			new SongFileInfo("Louisa Wendorff", "Style (Cover)"),
			new SongFileInfo("Maroon 5", "Give A Little More"),
			new SongFileInfo("Maroon 5", "Must Get Out"),
			new SongFileInfo("Maroon 5", "Never Gonna Leave This Bed (Acoustic)"),
			new SongFileInfo("Maroon 5", "nothing lasts forever"),
			new SongFileInfo("Maroon 5", "Sunday Morning"),
			new SongFileInfo("Maroon 5", "Sweetest Goodbye"),
			new SongFileInfo("Maroon 5", "The Sun"),
			new SongFileInfo("Maroon 5", "Through With You"),
			new SongFileInfo("Maroon 5", "won't go home without you"),
			new SongFileInfo("MK", "Piece Of Me"),
			new SongFileInfo("Nothing But Thieves", "Is Everybody Going Crazy"),
			new SongFileInfo("Seal", "Kiss From A Rose"),
			new SongFileInfo("Silk City", "Electricity"),
			new SongFileInfo("Silversun Pickups", "Panic Switch"),
			new SongFileInfo("Sonny Alven", "Our Youth"),
			new SongFileInfo("Swollen Members", "Counter Parts"),
			new SongFileInfo("Taylor Swift", "Blank Space"),
			new SongFileInfo("Taylor Swift", "Style"),
			new SongFileInfo("Taylor Swift", "Wildest Dreams"),
			new SongFileInfo("The Him", "Feels Like Home"),
			new SongFileInfo("The Script", "Breakeven"),
			new SongFileInfo("The Temper Trap", "Love Lost"),
			new SongFileInfo("Three Days Grace", "Never Too Late"),
			new SongFileInfo("U2", "With Or Without You"),
			new SongFileInfo("Zedd", "Clarity"),
		};
		#endregion GoodEnglishSongs
		#region public List<SongFileInfo> GoodJPopSpringSummerSongs = new List<SongFileInfo> {
		public List<SongFileInfo> GoodJPopSpringSummerSongs = new List<SongFileInfo> {
			new SongFileInfo("Yuzu", "Natsu Iro"),
		};
		#endregion GoodJPopSpringSummerSongs
		#region public List<SongFileInfo> GoodJPopFallWinterSongs = new List<SongFileInfo> {
		public List<SongFileInfo> GoodJPopFallWinterSongs = new List<SongFileInfo> {
			new SongFileInfo("Amuro Namie", "a walk in the park"),
			new SongFileInfo("Exile", "lovers again"),
			new SongFileInfo("Glay", "winter, again"),
			new SongFileInfo("Globe", "departures"),
			new SongFileInfo("Globe", "wanderin' destiny"),
			new SongFileInfo("Hamasaki Ayumi", "appears"),
			new SongFileInfo("L'Arc~en~Ciel", "snow drop"),
			new SongFileInfo("L'Arc~en~Ciel", "winter fall"),
			new SongFileInfo("Remioromen", "konayuki"),
		};
		#endregion GoodJPopFallWinterSongs
		#region public List<SongFileInfo> GoodJPopSongs = new List<SongFileInfo> {
		public List<SongFileInfo> GoodJPopSongs = new List<SongFileInfo> {
			new SongFileInfo("Amuro Namie", "Can You Celebrate"),
			new SongFileInfo("Brilliant Green", "There Will Be Love There"),
			new SongFileInfo("Chemistry", "Floatin'"),
			new SongFileInfo("Chemistry", "It Takes Two"),
			new SongFileInfo("Chemistry", "Pieces of a Dream"),
			new SongFileInfo("Chemistry", "Point of No Return"),
			new SongFileInfo("Do As Infinity", "fukai mori"),
			new SongFileInfo("Do As Infinity", "Oasis"),
			new SongFileInfo("Do As Infinity", "Under the Moon"),
			new SongFileInfo("Do As Infinity", "Yesterday & Today"),
			new SongFileInfo("Dragon Ash", "grateful days"),
			new SongFileInfo("Exile", "your eyes only"),
			new SongFileInfo("Glay", "Be With You"),
			new SongFileInfo("Glay", "Beloved"),
			new SongFileInfo("Glay", "However"),
			new SongFileInfo("Glay", "Kiseki no Hate"),
			new SongFileInfo("Glay", "Pure Soul"),
			new SongFileInfo("Globe", "Face"),
			new SongFileInfo("Globe", "Faces Places"),
			new SongFileInfo("Globe", "Perfume of Love"),
			new SongFileInfo("Hirai Ken", "Even If"),
			new SongFileInfo("Hirai Ken", "Hitomi wo Tojite"),
			new SongFileInfo("I WiSH", "asu e no tobira"),
			new SongFileInfo("Mr.Children", "Everything (It's you)"),
			new SongFileInfo("Mr.Children", "hana"),
			new SongFileInfo("Mr.Children", "kuchibue"),
			new SongFileInfo("Mr.Children", "kurumi"),
			new SongFileInfo("Mr.Children", "mirai"),
			new SongFileInfo("Mr.Children", "namonaki uta"),
			new SongFileInfo("Mr.Children", "owarinaki tabi"),
			new SongFileInfo("Mr.Children", "te no hira"),
			new SongFileInfo("Mr.Children", "yasashii uta"),
			new SongFileInfo("Mr.Children", "youthful days"),
			new SongFileInfo("Smap", "fly"),
			new SongFileInfo("Wands", "Sekai ga Owaru Madewa"),
			new SongFileInfo("Wands", "Sekaijuu no Dare Yori Kitto"),
		};
		#endregion GoodJPopSongs
		public DateTime NewSongThreshold;

		private FileUtil _fileUtil;
		private List<SongFileInfo> AllSongs = new List<SongFileInfo>();
		private List<SongFileInfo> GoodList = new List<SongFileInfo>();
		private List<SongFileInfo> NewList = new List<SongFileInfo>();
		private List<SongFileInfo> NewPlusGoodList = new List<SongFileInfo>();
		private List<SongFileInfo> ThePlaylist = new List<SongFileInfo>();
		private int AllSongsCount = 0;
		private int GoodListCount = 0;
		private int NewListCount = 0;
		private int NewPlusGoodListCount = 0;
		private int ThePlaylistCount = 0;
		#endregion Fields

		public Playlist(PType type, string name=null)
		{
			if (String.IsNullOrWhiteSpace(name)) {
				switch (type) {
					case PType.English:
						name = "English";
						break;
					case PType.JPopFallWinter:
						name = "J-Pop FaWi";
						break;
					case PType.JPopSpringSummer:
						name = "J-Pop SpSu";
						break;
					default:
						name = type.ToString();
						break;
				}
			}

			Type = type;
			Name = name;
			NewSongThreshold = DateTime.Now.AddYears(-2);
			_fileUtil = new FileUtil();
		}

		public void Create(string musicFolder)
		{
			List<string> files;
			int goodInd;
			string[] exclusions;

			try {
				switch (Type) {
					#region English
					case PType.English:
						exclusions = new string[] { @"\Album", @"\Classical", @"\J-Pop", @"\Spanish", };

						// Step 1: Get all songs //
						files = _fileUtil.GetAllAudioFiles(musicFolder, exclusions);
						for (int i = 0; i < files.Count; i++)
							AllSongs.Add(new SongFileInfo(files[i]));

						AllSongsCount = AllSongs.Count;

						// Step 2: Create GoodList //
						GoodList = AllSongs.Where(x => GoodEnglishSongs.Contains(x))
							.OrderBy(x => x.Title).ThenBy(y => y.Artist).ToList();
						GoodListCount = GoodList.Count;

						// Step 3: Create NewList // Ignored for J-Pop lists //
						// Step 4: Create NewPlusGoodList // Ignored for J-Pop lists //

						// Step 5: Create ThePlaylist //
						AllSongs = AllSongs.Except(GoodList).ToList();
						AllSongs = AllSongs.OrderBy(x => x.Title).ThenBy(y => y.Artist).ToList();
						goodInd = 0;

						for (int i = 0; i < AllSongs.Count; i++) {
							if (goodInd == GoodListCount)
								goodInd = 0;

							ThePlaylist.Add(GoodList[goodInd++]);
							ThePlaylist.Add(AllSongs[i++]);

							try {
								ThePlaylist.Add(AllSongs[i++]);
							}
							catch (Exception) {
								break;
							}

							try {
								ThePlaylist.Add(AllSongs[i]);
							}
							catch (Exception) {
								break;
							}
						}
						break;
					#endregion English
					#region JPopFallWinter
					case PType.JPopFallWinter:
						exclusions = new string[] { @"\_Album", @"_Christmas", @"_SpringSummer" };

						// Step 1: Get all songs //
						files = _fileUtil.GetAllAudioFiles(musicFolder, exclusions);
						for (int i = 0; i < files.Count; i++)
							AllSongs.Add(new SongFileInfo(files[i]));

						AllSongsCount = AllSongs.Count;

						// Step 2: Create GoodList //
						GoodJPopFallWinterSongs = GoodJPopFallWinterSongs.Union(GoodJPopSongs).ToList();
						GoodList = AllSongs.Where(x => GoodJPopFallWinterSongs.Contains(x))
							.OrderBy(x => x.Title).ThenBy(y => y.Artist).ToList();
						GoodListCount = GoodList.Count;

						// Step 3: Create NewList // Ignored for J-Pop lists //
						// Step 4: Create NewPlusGoodList // Ignored for J-Pop lists //

						// Step 5: Create ThePlaylist //
						AllSongs = AllSongs.Except(GoodList).ToList();
						AllSongs = AllSongs.OrderBy(x => x.Title).ThenBy(y => y.Artist).ToList();
						goodInd = 0;

						for (int i = 0; i < AllSongs.Count; i++) {
							if (goodInd == GoodListCount)
								goodInd = 0;

							ThePlaylist.Add(GoodList[goodInd++]);
							ThePlaylist.Add(AllSongs[i++]);

							try {
								ThePlaylist.Add(AllSongs[i++]);
							}
							catch (Exception) {
								break;
							}

							try {
								ThePlaylist.Add(AllSongs[i]);
							}
							catch (Exception) {
								break;
							}
						}
						break;
					#endregion JPopFallWinter
					#region JPopSpringSummer
					case PType.JPopSpringSummer:
						exclusions = new string[] { @"\_Album", @"_Christmas", @"_FallWinter" };

						// Step 1: Get all songs //
						files = _fileUtil.GetAllAudioFiles(musicFolder, exclusions);
						for (int i = 0; i < files.Count; i++)
							AllSongs.Add(new SongFileInfo(files[i]));

						AllSongsCount = AllSongs.Count;

						// Step 2: Create GoodList //
						GoodJPopSpringSummerSongs = GoodJPopSpringSummerSongs.Union(GoodJPopSongs).ToList();
						GoodList = AllSongs.Where(x => GoodJPopSpringSummerSongs.Contains(x))
							.OrderBy(x => x.Title).ThenBy(y => y.Artist).ToList();
						GoodListCount = GoodList.Count;

						// Step 3: Create NewList // Ignored for J-Pop lists //
						// Step 4: Create NewPlusGoodList // Ignored for J-Pop lists //

						// Step 5: Create ThePlaylist //
						AllSongs = AllSongs.Except(GoodList).ToList();
						AllSongs = AllSongs.OrderBy(x => x.Title).ThenBy(y => y.Artist).ToList();
						goodInd = 0;

						for (int i = 0; i < AllSongs.Count; i++) {
							if (goodInd == GoodListCount)
								goodInd = 0;

							ThePlaylist.Add(GoodList[goodInd++]);
							ThePlaylist.Add(AllSongs[i++]);

							try {
								ThePlaylist.Add(AllSongs[i++]);
							}
							catch (Exception) {
								break;
							}

							try {
								ThePlaylist.Add(AllSongs[i]);
							}
							catch (Exception) {
								break;
							}
						}
						break;
					#endregion JPopSpringSummer
					default:
						break;
				}
			}

			catch (Exception ex) {
				string error;

				if (ex.InnerException == null)
					error = String.Format("{0}{2}Exception thrown in Playlist.Create(string musicFolder='{3}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, musicFolder);
				else
					error = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Playlist.Create(string musicFolder='{3}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, musicFolder);

				Console.Write("\n{0}", error);
			}
		}

		public void WriteForBrowser()
		{

		}

		public void WriteForITunes(string destFolder)
		{
			if (String.IsNullOrWhiteSpace(destFolder))
				throw new Exception(String.Format("<destFolder> is required.{0}Exception thrown in Playlist.WriteForITunes(string destFolder).{0}", Environment.NewLine));

			try {
				var dest = String.Format("{0}{1} {2:yyyy MMdd HHmm ssff}.txt", destFolder, Name, DateTime.Now);
				var content = "Name\tArtist\tComposer\tAlbum\tGrouping\tGenre\tSize\tTime\tDisc Number\tDisc Count\tTrack Number\tTrack Count\tYear\tDate Modified\tDate Added\tBit Rate\tSample Rate\tVolume Adjustment\tKind\tEqualizer\tComments\tPlays\tLast Played\tSkips\tLast Skipped\tMy Rating\tLocation";

				foreach (var song in ThePlaylist)
					content += String.Format("\n{0}\t{1}\t\t{2}\t\t{3}\t\t{4}\t{5}\t1\t{6}\t\t{7}\t\t\t\t\t\t\t\t\t0\t\t\t\t\t{8}", song.Title, song.Artist, song.Album, song.Genre, song.Duration, song.DiscNum, song.TrackNum, song.Year, song.Path);

				content += "\n";
				_fileUtil.Write(content, dest);

				dest = String.Format("{0}{1} Good {2:yyyy MMdd HHmm}.txt", destFolder, Name, DateTime.Now);
				content = "Name\tArtist\tComposer\tAlbum\tGrouping\tGenre\tSize\tTime\tDisc Number\tDisc Count\tTrack Number\tTrack Count\tYear\tDate Modified\tDate Added\tBit Rate\tSample Rate\tVolume Adjustment\tKind\tEqualizer\tComments\tPlays\tLast Played\tSkips\tLast Skipped\tMy Rating\tLocation";

				foreach (var song in GoodList)
					content += String.Format("\n{0}\t{1}\t\t{2}\t\t{3}\t\t{4}\t{5}\t1\t{6}\t\t{7}\t\t\t\t\t\t\t\t\t0\t\t\t\t\t{8}", song.Title, song.Artist, song.Album, song.Genre, song.Duration, song.DiscNum, song.TrackNum, song.Year, song.Path);

				content += "\n";
				_fileUtil.Write(content, dest);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in Playlist.WriteForITunes(string destFolder='{3}').{0}{1}{0}{0}", ex.Message, ex.ToString(), Environment.NewLine, destFolder));

				throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Playlist.WriteForITunes(string destFolder='{3}').{0}{1}{0}{0}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, destFolder));
			}
		}

		public void WriteForWinamp(string destFolder)
		{
			var dest = String.Format("{0}{1} {2:yyyy MMdd HHmm ssff}.m3u", destFolder, Name, DateTime.Now);
			var content = "#EXTM3U";

			foreach (var song in ThePlaylist) {
				content += String.Format("\n#EXTINF:{0},{1} - {2}", song.Duration, song.Artist, song.Title);
				content += String.Format("\n{0}", song.Path);
			}

			content += "\n";
			_fileUtil.Write(content, dest);

			dest = String.Format("{0}{1} Good {2:yyyy MMdd HHmm}.m3u", destFolder, Name, DateTime.Now);
			content = "#EXTM3U";

			foreach (var song in GoodList) {
				content += String.Format("\n#EXTINF:{0},{1} - {2}", song.Duration, song.Artist, song.Title);
				content += String.Format("\n{0}", song.Path);
			}

			content += "\n";
			_fileUtil.Write(content, dest);
		}
	}
}