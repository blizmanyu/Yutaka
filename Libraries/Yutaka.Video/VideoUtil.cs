using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.Video
{
	public static class VideoUtil
	{
		private static DateTime startTime = DateTime.Now;

		public static void CreateAnimatedGif(TimeSpan ts, bool createWindow=false)
		{
			try {
				int exitCode;
				Process ffmpeg;

				using (ffmpeg = new Process()) {
					ffmpeg.EnableRaisingEvents = true;
					ffmpeg.StartInfo.FileName = "ffmpeg.exe";
					ffmpeg.StartInfo.Arguments = "-y -ss 02:25:0%%x.000 -t %length% -i \"%source%\" -vf fps=10,scale=1000:-1:flags=lanczos,palettegen \"%filename%%%x0.png\"";
					ffmpeg.StartInfo.CreateNoWindow = !createWindow;

					ffmpeg.Start();
					ffmpeg.WaitForExit();

					exitCode = ffmpeg.ExitCode;
					Console.Write("\nExitCode: {0}", exitCode);
				}
			}
			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in VideoUtil.CreateAnimatedGif(){2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateAnimatedGif(){2}{2}{1}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine));
			}
		}
	}
}