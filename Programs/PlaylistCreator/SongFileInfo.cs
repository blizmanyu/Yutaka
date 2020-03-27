using System;
using System.IO;

namespace PlaylistCreator
{
	public class SongFileInfo
	{
		private readonly DateTime DATE_THRESHOLD = new DateTime(1900, 1, 1);

		public string Path { get; set; }
		public DateTime Date { get; set; }
		public int Duration { get; set; }

		public uint TrackNum { get; set; }
		public uint DiscNum { get; set; }
		public string Title { get; set; }
		public string Artist { get; set; }
		public string Album { get; set; }
		public uint Year { get; set; }
		public string Genre { get; set; }
		public string Comment { get; set; }
		public string AlbumArtist { get; set; }
		public string Composer { get; set; }

		public SongFileInfo(string artist, string title)
		{
			if (String.IsNullOrWhiteSpace(artist))
				throw new Exception("<artist> is required. Exception thrown in SongFileInfo.SongFileInfo(string artist, string title).");
			if (String.IsNullOrWhiteSpace(title))
				throw new Exception("<title> is required. Exception thrown in SongFileInfo.SongFileInfo(string artist, string title).");

			Artist = artist;
			Title = title;
		}

		public SongFileInfo(FileInfo fInfo)
		{
			if (fInfo == null || String.IsNullOrEmpty(fInfo.FullName))
				throw new Exception("fInfo can't be NULL");

			Path = fInfo.FullName;
			var creationTime = fInfo.CreationTime;
			var lastWriteTime = fInfo.LastWriteTime;

			if (creationTime == null || creationTime < DATE_THRESHOLD)
				throw new Exception(String.Format("Can't determine CreationTime of {0}", Path));
			if (lastWriteTime == null || lastWriteTime < DATE_THRESHOLD)
				throw new Exception(String.Format("Can't determine LastWriteTime of {0}", Path));

			if (creationTime < lastWriteTime) {
				Date = creationTime;
				if ((lastWriteTime - creationTime).TotalDays > 1) {
					Console.Write("\n");
					Console.Write("\n{0}", Path);
					Console.Write("\nlastWriteTime - creationTime > 1");
					Console.Write("\n CreationTime: {0}", creationTime);
					Console.Write("\nLastWriteTime: {0}", lastWriteTime);
					Console.Write("\n{0} days", (lastWriteTime - creationTime).TotalDays);
					fInfo.LastWriteTime = Date;
					fInfo.LastAccessTime = Date;
				}
			}

			else /*lastWriteTime <= creationTime */ {
				Date = lastWriteTime;
				if ((creationTime - lastWriteTime).TotalDays > 1) {
					Console.Write("\n");
					Console.Write("\n{0}", Path);
					Console.Write("\ncreationTime - lastWriteTime > 1");
					Console.Write("\n CreationTime: {0}", creationTime);
					Console.Write("\nLastWriteTime: {0}", lastWriteTime);
					Console.Write("\n{0} days", (creationTime - lastWriteTime).TotalDays);
					fInfo.CreationTime = Date;
					fInfo.LastAccessTime = Date;
				}
			}

			try {
				var file = TagLib.File.Create(Path);
				Duration = (int) Math.Round(file.Properties.Duration.TotalSeconds, MidpointRounding.AwayFromZero);

				TrackNum = file.Tag.Track;
				DiscNum = file.Tag.Disc;
				Title = file.Tag.Title;
				Artist = (file.Tag.Performers == null || file.Tag.Performers.Length < 1) ? "" : file.Tag.Performers[0];
				Album = file.Tag.Album;
				Year = file.Tag.Year;
				Genre = (file.Tag.Genres == null || file.Tag.Genres.Length < 1) ? "" : file.Tag.Genres[0];
				Comment = file.Tag.Comment;
				AlbumArtist = (file.Tag.AlbumArtists == null || file.Tag.AlbumArtists.Length < 1) ? "" : file.Tag.AlbumArtists[0];
				Composer = (file.Tag.Composers == null || file.Tag.Composers.Length < 1) ? "" : file.Tag.Composers[0];
			}

			catch (Exception ex) {
				Console.Write("\nCould not create TagLib file: {0}\n{1}\n", ex.Message, ex.ToString());
				Duration = -1;
				TrackNum = 0;
				DiscNum = 0;
				Title = "";
				Artist = "";
				Album = "";
				Year = 0;
				Genre = "";
				Comment = "";
				AlbumArtist = "";
				Composer = "";
			}
		}

		public SongFileInfo(string filePath)
		{
			if (String.IsNullOrWhiteSpace(filePath))
				throw new Exception(String.Format("<filePath> is required.{0}Exception thrown in PlaylistCreator.SongFileInfo(string file).", Environment.NewLine));

			Path = filePath;
			var fInfo = new FileInfo(filePath);
			var creationTime = fInfo.CreationTime;
			var lastWriteTime = fInfo.LastWriteTime;

			if (creationTime == null || creationTime < DATE_THRESHOLD)
				throw new Exception(String.Format("Can't determine CreationTime of {0}", Path));
			if (lastWriteTime == null || lastWriteTime < DATE_THRESHOLD)
				throw new Exception(String.Format("Can't determine LastWriteTime of {0}", Path));

			//if (creationTime < lastWriteTime) {
			//	Date = creationTime;
			//	if ((lastWriteTime - creationTime).TotalDays > 1) {
			//		Console.Write("\n");
			//		Console.Write("\n{0}", Path);
			//		Console.Write("\nlastWriteTime - creationTime > 1");
			//		Console.Write("\n CreationTime: {0}", creationTime);
			//		Console.Write("\nLastWriteTime: {0}", lastWriteTime);
			//		Console.Write("\n{0} days", (lastWriteTime - creationTime).TotalDays);
			//		fInfo.LastWriteTime = Date;
			//		fInfo.LastAccessTime = Date;
			//	}
			//}

			//else /*lastWriteTime <= creationTime */ {
			//	Date = lastWriteTime;
			//	if ((creationTime - lastWriteTime).TotalDays > 1) {
			//		Console.Write("\n");
			//		Console.Write("\n{0}", Path);
			//		Console.Write("\ncreationTime - lastWriteTime > 1");
			//		Console.Write("\n CreationTime: {0}", creationTime);
			//		Console.Write("\nLastWriteTime: {0}", lastWriteTime);
			//		Console.Write("\n{0} days", (creationTime - lastWriteTime).TotalDays);
			//		fInfo.CreationTime = Date;
			//		fInfo.LastAccessTime = Date;
			//	}
			//}

			try {
				var file = TagLib.File.Create(Path);
				Duration = (int) Math.Round(file.Properties.Duration.TotalSeconds, MidpointRounding.AwayFromZero);

				TrackNum = file.Tag.Track;
				DiscNum = file.Tag.Disc;
				Title = file.Tag.Title;
				Artist = (file.Tag.Performers == null || file.Tag.Performers.Length < 1) ? "" : file.Tag.Performers[0];
				Album = file.Tag.Album;
				Year = file.Tag.Year;
				Genre = (file.Tag.Genres == null || file.Tag.Genres.Length < 1) ? "" : file.Tag.Genres[0];
				Comment = file.Tag.Comment;
				AlbumArtist = (file.Tag.AlbumArtists == null || file.Tag.AlbumArtists.Length < 1) ? "" : file.Tag.AlbumArtists[0];
				Composer = (file.Tag.Composers == null || file.Tag.Composers.Length < 1) ? "" : file.Tag.Composers[0];
			}

			catch (Exception ex) {
				Console.Write("\nCould not create TagLib file: {0}\n{1}\n", ex.Message, ex.ToString());
				Duration = -1;
				TrackNum = 0;
				DiscNum = 0;
				Title = "";
				Artist = "";
				Album = "";
				Year = 0;
				Genre = "";
				Comment = "";
				AlbumArtist = "";
				Composer = "";
			}
		}

		public void ConsoleOut()
		{
			Console.Write("\n");
			Console.Write("\nPath: {0}", Path);
			Console.Write("\nDate: {0:M/d/yyyy}", Date);
			Console.Write("\nDuration: {0}s", Duration);
			Console.Write("\n");
			Console.Write("\nTrackNum: {0}", TrackNum);
			Console.Write("\nDiscNum: {0}", DiscNum);
			Console.Write("\nTitle: {0}", Title);
			Console.Write("\nArtist: {0}", Artist);
			Console.Write("\nAlbum: {0}", Album);
			Console.Write("\nYear: {0}", Year);
			Console.Write("\nGenre: {0}", Genre);
			Console.Write("\nComment: {0}", Comment);
			Console.Write("\nAlbumArtist: {0}", AlbumArtist);
			Console.Write("\nComposer: {0}", Composer);
			Console.Write("\n");
		}

		public override bool Equals(Object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
				return false;

			try {
				var other = (SongFileInfo) obj;
				var thisArtist = this.Artist.ToUpper();
				var thisTitle = this.Title.ToUpper();
				var otherArtist = other.Artist.ToUpper();
				var otherTitle = other.Title.ToUpper();

				return otherArtist.StartsWith(thisArtist) && otherTitle.StartsWith(thisTitle);
			}

			catch (Exception) {
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return String.Format("{0} - {1}", Artist, Title);
		}
	}
}