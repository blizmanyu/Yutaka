using System;
using System.Diagnostics;
using System.IO;

namespace Yutaka.Diagnostics
{
	public class FfmpegUtil
	{
		public static string DefaultPaletteFolder = @"G:\TEMP\";

		/// <summary>
		/// Creates an animated GIF.
		/// </summary>
		/// <param name="startTime">Start time in decimal seconds.</param>
		/// <param name="source">Full path of the source.</param>
		/// <param name="destFolder">Destination folder. Defaults to &lt;DefaultPaletteFolder&gt;</param>
		/// <param name="overwriteAll">null: prompt for each. true: overwrite all. false: overwrite none.</param>
		/// <param name="length">Default is 10. If length is less than .5, it will default to 10.</param>
		/// <param name="fps">Default is 24. If length is less than 1, it will default to 24.</param>
		/// <param name="width">Default is 960. If length is less than 1, it will default to 960.</param>
		/// <param name="createWindow">Whether to create a console window or not.</param>
		/// <returns></returns>
		public static Process StartCreatingAnimatedGif(double startTime, string source, bool? overwriteAll = null, double length = -1, int fps = -1, int width = -1, string destFolder = null, bool createWindow = true)
		{
			#region Parameter Check
			var errorMsg = "";

			if (startTime < 0)
				errorMsg = String.Format("{0}<startTime> must be at least 0.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(source))
				errorMsg = String.Format("{0}<source> is required.{1}", errorMsg, Environment.NewLine);
			else if (!File.Exists(source))
				errorMsg = String.Format("{0}<source> doesn't exist.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg))
				throw new Exception(String.Format("{0}{1}", errorMsg, Environment.NewLine));
			#endregion Parameter Check

			var fi = new FileInfo(source);
			var NameWithoutExtension = fi.Name.Replace(fi.Extension, "");
			fi = null;
			// startTime //
			var args = String.Format("-ss {0:0.000}", startTime);
			// overwriteAll //
			if (overwriteAll != null) {
				if (overwriteAll == true)
					args = String.Format("-y {0}", args);
				else
					args = String.Format("-n {0}", args);
			}
			// length //
			if (length < .5)
				length = 10;
			args = String.Format("{0} -t {1:0.000}", args, length);
			// source //
			args = String.Format("{0} -i \"{1}\"", args, source);
			// palette //
			args = String.Format("{0} -i \"{1}\"", args, Path.Combine(DefaultPaletteFolder, NameWithoutExtension, startTime.ToString("00000.00'.png'")));
			// fps & width //
			if (fps < 1)
				fps = 24;
			if (width < 1)
				width = 960;
			args = String.Format("{0} -filter_complex \"fps={1},scale={2}:-1:flags=lanczos[x];[x][1:v]paletteuse\"", args, fps, width);
			// destFolder //
			if (String.IsNullOrWhiteSpace(destFolder))
				destFolder = Path.Combine(DefaultPaletteFolder, NameWithoutExtension);
			Directory.CreateDirectory(destFolder);
			args = String.Format("{0} \"{1}.gif\"", args, Path.Combine(destFolder, startTime.ToString("00000.00")));
			Console.Write("\n\n******************************\n");
			Console.Write("args: {0}", args);
			Console.Write("\n******************************\n\n");

			try {
				var psi = new ProcessStartInfo("ffmpeg", args) {
					CreateNoWindow = !createWindow,
					UseShellExecute = false,
				};

				return Process.Start(psi);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in FfmpegUtil.StartCreatingAnimatedGif(double startTime='{3}', string source='{4}', bool? overwriteAll='{5}', double length='{6}', int fps='{7}', int width='{8}', string destFolder='{9}', bool createWindow='{10}'){2}{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, startTime, source, overwriteAll, length, fps, width, destFolder, createWindow));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of FfmpegUtil.StartCreatingAnimatedGif(double startTime='{3}', string source='{4}', bool? overwriteAll='{5}', double length='{6}', int fps='{7}', int width='{8}', string destFolder='{9}', bool createWindow='{10}'){2}{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, startTime, source, overwriteAll, length, fps, width, destFolder, createWindow));
			}
		}

		/// <summary>
		/// Creates a palette for high-quality animated GIFs.
		/// </summary>
		/// <param name="startTime">Start time in decimal seconds.</param>
		/// <param name="source">Full path of the source.</param>
		/// <param name="length">Default is 10. If length is less than .5, it will default to 10.</param>
		/// <param name="fps">Default is 24. If length is less than 1, it will default to 24.</param>
		/// <param name="width">Default is 960. If length is less than 1, it will default to 960.</param>
		/// <param name="createWindow">Whether to create a console window or not.</param>
		/// <returns></returns>
		public static Process StartCreatingPalette(double startTime, string source, double length = -1, int fps = -1, int width = -1, bool createWindow = true)
		{
			#region Parameter Check
			var errorMsg = "";

			if (startTime < 0)
				errorMsg = String.Format("{0}<startTime> must be at least 0.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(source))
				errorMsg = String.Format("{0}<source> is required.{1}", errorMsg, Environment.NewLine);
			else if (!File.Exists(source))
				errorMsg = String.Format("{0}<source> doesn't exist.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg))
				throw new Exception(String.Format("{0}{1}", errorMsg, Environment.NewLine));
			#endregion Parameter Check

			var fi = new FileInfo(source);
			var NameWithoutExtension = fi.Name.Replace(fi.Extension, "");
			fi = null;
			// overwriteAll //
			var args = "-y";
			// startTime //
			args = String.Format("{0} -ss {1:0.000}", args, startTime);
			// length //
			if (length < .5)
				length = 10;
			args = String.Format("{0} -t {1:0.000}", args, length);
			// source //
			args = String.Format("{0} -i \"{1}\"", args, source);
			// fps & width //
			if (fps < 1)
				fps = 24;
			if (width < 1)
				width = 960;
			args = String.Format("{0} -vf fps={1},scale={2}:-1:flags=lanczos,palettegen", args, fps, width);
			// palette //
			var destFolder = Path.Combine(DefaultPaletteFolder, NameWithoutExtension);
			Directory.CreateDirectory(destFolder);
			args = String.Format("{0} \"{1}\"", args, Path.Combine(DefaultPaletteFolder, NameWithoutExtension, startTime.ToString("00000.00'.png'")));
			//Console.Write("\n\n******************************\n");
			//Console.Write("args: {0}", args);
			//Console.Write("\n******************************\n\n");

			try {
				var psi = new ProcessStartInfo("ffmpeg", args) {
					CreateNoWindow = !createWindow,
					UseShellExecute = false,
				};

				return Process.Start(psi);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in FfmpegUtil.StartCreatingPalette(double startTime='{3}', string source='{4}', double length='{5}', int fps='{6}', int width='{7}', bool createWindow='{8}'){2}{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, startTime, source, length, fps, width, createWindow));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of FfmpegUtil.StartCreatingPalette(double startTime='{3}', string source='{4}', double length='{5}', int fps='{6}', int width='{7}', bool createWindow='{8}'){2}{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, startTime, source, length, fps, width, createWindow));
			}
		}

		/// <summary>
		/// Creates a thumbnail for animated GIF.
		/// </summary>
		/// <param name="startTime">Start time in decimal seconds.</param>
		/// <param name="source">Full path of the source.</param>
		/// <param name="destFolder">Destination folder. Defaults to &lt;DefaultPaletteFolder&gt;</param>
		/// <param name="overwriteAll">null: prompt for each. true: overwrite all. false: overwrite none.</param>
		/// <param name="length">Default is 10. If length is less than .5, it will default to 10.</param>
		/// <param name="fps">Default is 24. If length is less than 1, it will default to 24.</param>
		/// <param name="width">Default is 480. If length is less than 1, it will default to 480.</param>
		/// <param name="createWindow">Whether to create a console window or not.</param>
		/// <returns></returns>
		public static Process StartCreatingThumbnail(double startTime, string source, bool? overwriteAll = null, double length = -1, int fps = -1, int width = -1, string destFolder = null, bool createWindow = true)
		{
			#region Parameter Check
			var errorMsg = "";

			if (startTime < 0)
				errorMsg = String.Format("{0}<startTime> must be at least 0.{1}", errorMsg, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(source))
				errorMsg = String.Format("{0}<source> is required.{1}", errorMsg, Environment.NewLine);
			else if (!File.Exists(source))
				errorMsg = String.Format("{0}<source> doesn't exist.{1}", errorMsg, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(errorMsg))
				throw new Exception(String.Format("{0}{1}", errorMsg, Environment.NewLine));
			#endregion Parameter Check

			var fi = new FileInfo(source);
			var NameWithoutExtension = fi.Name.Replace(fi.Extension, "");
			fi = null;
			// startTime //
			var args = String.Format("-ss {0:0.000}", startTime);
			// overwriteAll //
			if (overwriteAll != null) {
				if (overwriteAll == true)
					args = String.Format("-y {0}", args);
				else
					args = String.Format("-n {0}", args);
			}
			// length //
			if (length < .5)
				length = 10;
			args = String.Format("{0} -t {1:0.000}", args, length);
			// source //
			args = String.Format("{0} -i \"{1}\"", args, source);
			// palette //
			args = String.Format("{0} -i \"{1}\"", args, Path.Combine(DefaultPaletteFolder, NameWithoutExtension, startTime.ToString("00000.00'.png'")));
			// fps & width //
			if (fps < 1)
				fps = 24;
			if (width < 1)
				width = 480;
			args = String.Format("{0} -filter_complex \"fps={1},scale={2}:-1:flags=lanczos[x];[x][1:v]paletteuse\"", args, fps, width);
			// destFolder //
			if (String.IsNullOrWhiteSpace(destFolder))
				destFolder = Path.Combine(DefaultPaletteFolder, NameWithoutExtension);
			Directory.CreateDirectory(destFolder);
			args = String.Format("{0} \"{1}-thumb.gif\"", args, Path.Combine(destFolder, startTime.ToString("00000.00")));
			Console.Write("\n\n******************************\n");
			Console.Write("args: {0}", args);
			Console.Write("\n******************************\n\n");

			try {
				var psi = new ProcessStartInfo("ffmpeg", args) {
					CreateNoWindow = !createWindow,
					UseShellExecute = false,
				};

				return Process.Start(psi);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}{2}Exception thrown in FfmpegUtil.StartCreatingThumbnail(double startTime='{3}', string source='{4}', bool? overwriteAll='{5}', double length='{6}', int fps='{7}', int width='{8}', string destFolder='{9}', bool createWindow='{10}'){2}{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, startTime, source, overwriteAll, length, fps, width, destFolder, createWindow));

				throw new Exception(String.Format("{0}{2}{2}Exception thrown in INNER EXCEPTION of FfmpegUtil.StartCreatingThumbnail(double startTime='{3}', string source='{4}', bool? overwriteAll='{5}', double length='{6}', int fps='{7}', int width='{8}', string destFolder='{9}', bool createWindow='{10}'){2}{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, startTime, source, overwriteAll, length, fps, width, destFolder, createWindow));
			}
		}
	}
}