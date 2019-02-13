using System;
using System.Diagnostics;
using System.IO;
using WMPLib;

namespace Yutaka.Video
{
	public static class VideoUtil
	{
		private const string FFMPEG_PATH = @"ffmpeg.exe"; // only change this is ffmpeg is NOT in your Environment Paths //

		public static void CreateAnimatedGif(TimeSpan startTime, int length, string source, string destFolder, int fps = 15, int width = 640)
		{
			try {
				int exitCode;
				var arg = "";
				var dest = "";
				Process p;

				using (p = new Process()) {
					dest = String.Format("{0}{1}", destFolder, startTime.ToString("hhmmssfff"));
					Console.Write("\npng: {0}", dest);
					arg = String.Format("-ss {0} -t {1} -i \"{2}\" -vf fps={3},scale={4}:-1:flags=lanczos,palettegen \"{5}.png\"", startTime.ToString(@"hh\:mm\:ss\.fff"), length, source, fps, width, dest);
					Console.Write("\narg: {0}", arg);

					p.StartInfo.RedirectStandardOutput = true;
					p.StartInfo.RedirectStandardError = true;
					p.StartInfo.FileName = FFMPEG_PATH;
					p.StartInfo.Arguments = arg;
					p.StartInfo.UseShellExecute = false;
					p.StartInfo.CreateNoWindow = true;
					p.Start();
					p.WaitForExit();

					exitCode = p.ExitCode;
					Console.Write("\nExitCode: {0}", exitCode);
				}

				if (exitCode == 0) {
					using (p = new Process()) {
						arg = String.Format("-ss {0} -t {1} -i \"{2}\" -i \"{3}.png\" -filter_complex \"fps={4},scale={5}:-1:flags=lanczos[x];[x][1:v]paletteuse\" \"{6}.gif\"", startTime.ToString(@"hh\:mm\:ss\.fff"), length, source, dest, fps, width, dest);
						Console.Write("\narg: {0}", arg);

						p.StartInfo.RedirectStandardOutput = true;
						p.StartInfo.RedirectStandardError = true;
						p.StartInfo.FileName = FFMPEG_PATH;
						p.StartInfo.Arguments = arg;
						p.StartInfo.UseShellExecute = false;
						p.StartInfo.CreateNoWindow = true;
						p.Start();
						p.WaitForExit();

						exitCode = p.ExitCode;
						Console.Write("\nExitCode: {0}", exitCode);

						if (exitCode == 0) {
							//Console.Write("\nDeleteing PNG palette...");
							File.Delete(String.Format("{0}.png", dest));
						}
					}
				}
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in VideoUtil.CreateAnimatedGif(){2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateAnimatedGif(){2}{2}{1}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine));
			}
		}

		// Warning: this method name will most likely change once complete //
		public static void CreateVersion1(string source, string destFolder, double start=0, double end=-1)
		{
			if (String.IsNullOrWhiteSpace(source))
				throw new Exception(String.Format("<source> is NULL.{0}Exception thrown in VideoUtil.CreateVersion1(string source, string destFolder, double start, double end).{0}{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(destFolder))
				throw new Exception(String.Format("<destFolder> is NULL.{0}Exception thrown in VideoUtil.CreateVersion1(string source, string destFolder, double start, double end).{0}{0}", Environment.NewLine));
			if (start < 0)
				start = 0;
			if (end < 1)
				end = GetDuration(source);

			try {
				Console.Write("\nsource: {0}", source);
				Console.Write("\ndestFolder: {0}", destFolder);
				Console.Write("\nstart: {0}", start);
				Console.Write("\nend: {0}", end);
				var interval = (end - start) / 7;
				Console.Write("\ninterval: {0}", interval);

				for (var i=start; i<end; i+=interval) {
					CreateAnimatedGif(TimeSpan.FromSeconds(i), 2, source, destFolder);
				}
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in VideoUtil.CreateVersion1(string source='{3}', string destFolder='{4}', double start={5}, double end={6}).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, source, destFolder, start, end));

				throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateVersion1(string source='{3}', string destFolder='{4}', double start={5}, double end={6}).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, source, destFolder, start, end));
			}
		}

		public static double GetDuration(string file)
		{
			if (String.IsNullOrWhiteSpace(file))
				throw new Exception(String.Format("<file> is NULL.{0}Exception thrown in VideoUtil.GetDuration(string file).{0}{0}", Environment.NewLine));

			try {
				var media = new WindowsMediaPlayer().newMedia(file);
				return media.duration;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in VideoUtil.GetDuration(string file='{3}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, file));

				throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VideoUtil.GetDuration(string file='{3}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, file));
			}
		}
	}
}

/*
 * ffmpeg -i "G:\Projects\FileCopier2\Videos\Jful\Angel Emily - Holy Holes Scene 3 anal dp 1080p.mp4" -filter_complex \
"[0:v]trim=start=10:end=12,setpts=PTS-STARTPTS[a]; \
 [0:v]trim=start=72:end=74,setpts=PTS-STARTPTS[b]; \
 [a][b]concat[c]; \
 [0:v]trim=start=134:end=136,setpts=PTS-STARTPTS[d]; \
 [c][d]concat[e]; \
 [0:v]trim=start=196:end=198,setpts=PTS-STARTPTS[f]; \
 [e][f]concat[g]; \
 [0:v]trim=start=258:end=260,setpts=PTS-STARTPTS[h]; \
 [g][h]concat[i]; \
 [0:v]trim=start=320:end=322,setpts=PTS-STARTPTS[j]; \
 [i][j]concat[k]; \
 [0:v]trim=start=382:end=384,setpts=PTS-STARTPTS[l]; \
 [k][l]concat[out1]" -map [out1] _out.mkv

 * 
 * */
