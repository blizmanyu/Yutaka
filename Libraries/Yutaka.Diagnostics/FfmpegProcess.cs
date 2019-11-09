﻿using System;
using System.Diagnostics;
using System.IO;
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
		public static Process CreateAnimatedGif(TimeSpan startTime, TimeSpan length, string source, string output, bool? overwriteAll = null, int fps = 24, int width = 960, bool createThumbnail = false, bool createWindow = true)
		{
			#region Parameter Check
			var errorMsg = "";

			if (startTime.TotalSeconds < 0)
				errorMsg = String.Format("{0}<startTime> must be at least 0.{1}", errorMsg, Environment.NewLine);
			if (length.TotalSeconds < 1)
				errorMsg = String.Format("{0}<length> must be at least 1.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(source))
				errorMsg = String.Format("{0}<source> is required.{1}", errorMsg, Environment.NewLine);
			else if (!File.Exists(source))
				errorMsg = String.Format("{0}<source> doesn't exist.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(output))
				errorMsg = String.Format("{0}<output> is required.{1}", errorMsg, Environment.NewLine);
			if (fps < 1)
				errorMsg = String.Format("{0}<fps> must be at least 1.{1}", errorMsg, Environment.NewLine);
			if (width < 1)
				errorMsg = String.Format("{0}<width> must be at least 1.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg))
				throw new Exception(String.Format("{0}{1}", errorMsg, Environment.NewLine));
			#endregion Parameter Check

			var args = "-y -ss";
			args = String.Format("{0} {1}", args, startTime.ToString(@"hh\:mm\:ss\.fff"));
			args = String.Format("{0} -t {1:0.000}", args, length.TotalSeconds);
			args = String.Format("{0} -i \"{1}\"", args, source);
			args = String.Format("{0} -vf fps={1},scale={2}:-1:flags=lanczos,palettegen", args, fps, width);
			args = String.Format("{0} \"{1}.png\"", args, startTime.ToString(@"hh\h\ mm\m\ ss\s\ fff"));

			Console.Write("\nargs: {0}", args);

			try {
				var psi = new ProcessStartInfo("ffmpeg", args) {
					CreateNoWindow = !createWindow,
					UseShellExecute = false,
				};

				return Start(psi);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in FfmpegProcess.CreateAnimatedGif(bool createWindow={3}){2}{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, createWindow));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of FfmpegProcess.CreateAnimatedGif(bool createWindow={3}){2}{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, createWindow));
			}
		}
	}
}