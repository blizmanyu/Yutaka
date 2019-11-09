using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Yutaka.Diagnostics
{
	public class FfmpegProcess : Process
	{
		/// <summary>
		/// Work-in-progress: Do not use yet!
		/// </summary>
		/// <param name="startTime"></param>
		/// <param name="length"></param>
		/// <param name="source"></param>
		/// <param name="output"></param>
		/// <param name="overwriteAll"></param>
		/// <param name="fps"></param>
		/// <param name="width"></param>
		/// <param name="createThumbnail"></param>
		/// <param name="createWindow"></param>
		public void CreateAnimatedGif(TimeSpan startTime, TimeSpan length, string source, string output, bool? overwriteAll = null, int fps = 24, int width = 960, bool createThumbnail = false, bool createWindow = true)
		{
			var args = "";

			//if (force)
			//	args = String.Format("{0} -f", args);
			//if (waitTime > 0)
			//	args = String.Format("{0} -t {1}", args, waitTime);
			//if (!String.IsNullOrWhiteSpace(remoteCompName))
			//	args = String.Format(@"{0} -m \\{1}", args, remoteCompName);

			try {
				var psi = new ProcessStartInfo("ffmpeg", args) {
					CreateNoWindow = !createWindow,
					UseShellExecute = false,
				};

				Process.Start(psi);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in FfmpegProcess.CreateAnimatedGif(bool createWindow={3}){2}{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, createWindow));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of FfmpegProcess.CreateAnimatedGif(bool createWindow={3}){2}{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, createWindow));
			}
		}
	}
}