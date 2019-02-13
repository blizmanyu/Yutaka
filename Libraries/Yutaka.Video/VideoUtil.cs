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
		public static void CreateVersion1(string source, string destFolder)
		{

		}

		public static double GetDuration(string file)
		{
			return new WindowsMediaPlayer().newMedia(file).duration;
		}
	}
}