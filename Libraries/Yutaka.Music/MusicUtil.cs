using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.Music
{
	public class MusicUtil
	{
	}

	public class Song
	{
		#region Fields
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
		#endregion

		#region Methods
		public Song(string filePath)
		{
			if (String.IsNullOrEmpty(filePath))
				throw new Exception(String.Format("<filePath> is required.{0}{0}Except thrown in Song(string filePath)", Environment.NewLine));

			try {
				var fInfo = new FileInfo(filePath);
				Path = filePath;
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

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Except thrown in Song(string filePath='{3}'){2}{1}", ex.Message, ex.ToString(), Environment.NewLine, filePath));

				throw new Exception(String.Format("{0}{2}{2}Except thrown in InnerException of Song(string filePath='{3}'){2}{1}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, filePath));
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
		#endregion
	}
}