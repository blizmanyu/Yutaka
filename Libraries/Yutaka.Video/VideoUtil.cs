﻿using System;
using System.Diagnostics;
using System.IO;
using WMPLib;

namespace Yutaka.Video
{
	public class VideoUtil
	{
		private const string FFMPEG_PATH = @"ffmpeg.exe"; // only change this is ffmpeg is NOT in your Environment Paths //
		public int Fps;
		public int Width;
		public string DestFolder;
		public string FfmpegPath;

		public VideoUtil()
		{
			Fps = 10;
			Width = 640;
			DestFolder = String.Format(@"C:\Temp\{0:yyyy MMdd HHmm ssff}\", DateTime.Now);
			FfmpegPath = @"ffmpeg.exe";
		}

		public void CreateAllBetween(string source, string destFolder=null, double start = 0, double end = -1, double interval=-1, double length = -1)
		{
			if (String.IsNullOrWhiteSpace(source))
				throw new Exception(String.Format("<source> is required.{0}Exception thrown in VideoUtil.CreateVersion1(string source, string destFolder, double start, double end).{0}{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(destFolder))
				destFolder = String.Format(@"C:\Temp\{0:yyyy MMdd HHmm ssff}\", DateTime.Now);
			if (start < 0)
				start = 0;
			if (end < 1)
				end = start + 120;
			if (interval < 1)
				interval = 10;
			if (length < 1)
				length = 10;

			try {
				Console.Write("\nsource: {0}", source);
				Console.Write("\ndestFolder: {0}", destFolder);
				Console.Write("\nstart: {0}", start);
				Console.Write("\nend: {0}", end);

				for (var i = start; i < end; i += interval)
					CreateAnimatedGif(TimeSpan.FromSeconds(i), length, source, destFolder);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in VideoUtil.CreateVersion1(string source='{3}', string destFolder='{4}', double start={5}, double end={6}).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, source, destFolder, start, end));

				throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateVersion1(string source='{3}', string destFolder='{4}', double start={5}, double end={6}).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, source, destFolder, start, end));
			}
		}

		public void CreateGallery(string source, string destFolder, string extension = "jpg", double start = 0, double end = -1)
		{
			if (String.IsNullOrWhiteSpace(source))
				throw new Exception(String.Format("<source> is required.{0}Exception thrown in VideoUtil.CreateGallery(string source, string destFolder, double start, double end).{0}{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(destFolder))
				throw new Exception(String.Format("<destFolder> is required.{0}Exception thrown in VideoUtil.CreateGallery(string source, string destFolder, double start, double end).{0}{0}", Environment.NewLine));
			if (start < 0)
				start = 0;
			if (end < 1)
				end = GetDuration(source);

			try {
				Console.Write("\nsource: {0}", source);
				Console.Write("\ndestFolder: {0}", destFolder);
				Console.Write("\nstart: {0}", start);
				Console.Write("\nend: {0}", end);

				for (var i = start; i < end; i++)
					CreateSingleImage(TimeSpan.FromSeconds(i), source, destFolder, extension);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in VideoUtil.CreateGallery(string source='{3}', string destFolder='{4}', double start={5}, double end={6}).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, source, destFolder, start, end));

				throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateGallery(string source='{3}', string destFolder='{4}', double start={5}, double end={6}).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, source, destFolder, start, end));
			}
		}

		public void CreateSingleImage(TimeSpan startTime, string source, string destFolder, string extension="jpg")
		{
			if (String.IsNullOrWhiteSpace(source))
				throw new Exception(String.Format("<source> is required.{0}Exception thrown in VideoUtil.CreateSingleImage(TimeSpan startTime, string source, string destFolder).{0}{0}", Environment.NewLine));
			if (!File.Exists(source))
				throw new Exception(String.Format("'{1}' doesn't exist.{0}Exception thrown in VideoUtil.CreateSingleImage(TimeSpan startTime, string source, string destFolder).{0}{0}", Environment.NewLine, source));

			try {
				using (var p = new Process()) {
					var dest = String.Format("{0}{1:hh}h {1:mm}m {1:ss}s {1:fff}f", destFolder, startTime);
					var arg = String.Format("-y -ss {0} -i \"{1}\" -frames:v 1 \"{2}.{3}\"", startTime.ToString(@"hh\:mm\:ss\.fff"), source, dest, extension);
					Console.Write("\narg: {0}", arg);

					p.StartInfo.RedirectStandardOutput = true;
					p.StartInfo.RedirectStandardError = true;
					p.StartInfo.FileName = FFMPEG_PATH;
					p.StartInfo.Arguments = arg;
					p.StartInfo.UseShellExecute = false;
					p.StartInfo.CreateNoWindow = true;
					Console.Write("\nParent FullName: {0}", Directory.GetParent(source).FullName);
					p.StartInfo.WorkingDirectory = Directory.GetParent(source).FullName + "\\";
					p.Start();
					p.WaitForExit();
				}
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					Console.Write("{0}{2}{2}Exception thrown in VideoUtil.CreateSingleImage(TimeSpan startTime={3}, string source='{4}', string destFolder='{5}'){2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine, startTime, source, destFolder);

				Console.Write("{0}{2}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateSingleImage(TimeSpan startTime={3}, string source='{4}', string destFolder='{5}'){2}{2}{1}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, startTime, source, destFolder);
			}
		}

		public void CreateAnimatedGif(TimeSpan startTime, double length, string source, string destFolder, int fps = 15, int width = 640)
		{
			if (String.IsNullOrWhiteSpace(source))
				throw new Exception(String.Format("<source> is required.{0}Exception thrown in VideoUtil.CreateAnimatedGif(TimeSpan startTime, int length, string source, string destFolder, int fps, int width).{0}{0}", Environment.NewLine));
			if (!File.Exists(source))
				throw new Exception(String.Format("'{1}' doesn't exist.{0}Exception thrown in VideoUtil.CreateAnimatedGif(TimeSpan startTime, int length, string source, string destFolder, int fps, int width).{0}{0}", Environment.NewLine, source));

			try {
				int exitCode;
				var arg = "";
				var dest = "";

				using (var p = new Process()) {
					dest = String.Format("{0}{1:hh}h {1:mm}m {1:ss}s {1:fff}f", destFolder, startTime);
					Console.Write("\npng: {0}", dest);
					arg = String.Format("-y -ss {0} -t {1} -i \"{2}\" -vf fps={3},scale={4}:-1:flags=lanczos,palettegen \"{5}.png\"", startTime.ToString(@"hh\:mm\:ss\.fff"), length, source, fps, width, dest);
					Console.Write("\narg: {0}", arg);

					p.StartInfo.RedirectStandardOutput = true;
					p.StartInfo.RedirectStandardError = true;
					p.StartInfo.FileName = FFMPEG_PATH;
					p.StartInfo.Arguments = arg;
					p.StartInfo.UseShellExecute = false;
					p.StartInfo.CreateNoWindow = true;
					//Console.Write("\nParent Name: {0}", Directory.GetParent(source).Name);
					//Console.Write("\nParent FullName: {0}", Directory.GetParent(source).FullName);
					p.StartInfo.WorkingDirectory = Directory.GetParent(source).FullName + "\\";
					p.Start();
					p.WaitForExit();

					exitCode = p.ExitCode;
					//if (exitCode != 0)
					//	Console.Write("\nresult = {0}", p.StandardOutput.ReadToEnd());
					
					Console.Write("\nExitCode: {0}", exitCode);
				}

				if (exitCode == 0) {
					using (var p2 = new Process()) {
						arg = String.Format("-y -ss {0} -t {1} -i \"{2}\" -i \"{3}.png\" -filter_complex \"fps={4},scale={5}:-1:flags=lanczos[x];[x][1:v]paletteuse\" \"{6}.gif\"", startTime.ToString(@"hh\:mm\:ss\.fff"), length, source, dest, fps, width, dest);
						Console.Write("\narg: {0}", arg);

						p2.StartInfo.RedirectStandardOutput = true;
						p2.StartInfo.RedirectStandardError = true;
						p2.StartInfo.FileName = FFMPEG_PATH;
						p2.StartInfo.Arguments = arg;
						p2.StartInfo.UseShellExecute = false;
						p2.StartInfo.CreateNoWindow = true;
						p2.StartInfo.WorkingDirectory = Directory.GetParent(source).FullName + "\\";
						p2.Start();
						p2.WaitForExit(12000);
						p2.Close();
						//Thread.Sleep(2200);
						//p.Kill();

						//exitCode = p.ExitCode;
						//Console.Write("\nExitCode: {0}", exitCode);

						//if (exitCode == 0) {
						//	//Console.Write("\nDeleteing PNG palette...");
						//File.Delete(String.Format("{0}.png", dest));
						//}
					}
				}
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					Console.Write("{0}{2}{2}Exception thrown in VideoUtil.CreateAnimatedGif(){2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine);

				Console.Write("{0}{2}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateAnimatedGif(){2}{2}{1}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);
			}
		}

		public void CreateAnimatedGif(string source, double start=0, double end=-1)
		{
			if (String.IsNullOrWhiteSpace(source))
				throw new Exception(String.Format("<source> is required.{0}Exception thrown in VideoUtil.CreateAnimatedGif(string source, double start, double end).{0}{0}", Environment.NewLine));
			if (start < 0)
				start = 0;
			if (end < 1)
				end = GetDuration(source);

			try {
				var destFolder = String.Format(@"C:\Temp\{0:yyyy MMdd HHmm ssff}\", DateTime.Now);
				var p1 = start + 120;
				var p2 = p1 + 60;
				var p4 = end - 120;
				var p3 = p4 - 60;

				for (var i = start; i < p1; i += 10)
					CreateAnimatedGif(TimeSpan.FromSeconds(i), 10, source, destFolder, 10, 1000);

				for (var i = p2; i < p3; i += 70)
					CreateAnimatedGif(TimeSpan.FromSeconds(i), 10, source, destFolder, 10, 1000);

				for (var i = p4; i < end; i += 10)
					CreateAnimatedGif(TimeSpan.FromSeconds(i), 10, source, destFolder, 10, 1000);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in VideoUtil.CreateAnimatedGif(string source='{3}', double start={4}, double end={5}).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, source, start, end));

				throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateAnimatedGif(string source='{3}', double start={4}, double end={5}).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, source, start, end));
			}
		}

		// Warning: this method name will most likely change once complete //
		public void CreateVersion1(string source, string destFolder, double start=0, double end=-1)
		{
			if (String.IsNullOrWhiteSpace(source))
				throw new Exception(String.Format("<source> is required.{0}Exception thrown in VideoUtil.CreateVersion1(string source, string destFolder, double start, double end).{0}{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(destFolder))
				throw new Exception(String.Format("<destFolder> is required.{0}Exception thrown in VideoUtil.CreateVersion1(string source, string destFolder, double start, double end).{0}{0}", Environment.NewLine));
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

		public double GetDuration(string file)
		{
			if (String.IsNullOrWhiteSpace(file))
				throw new Exception(String.Format("<file> is required.{0}Exception thrown in VideoUtil.GetDuration(string file).{0}{0}", Environment.NewLine));

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
 * ffmpeg -i "G:\Projects\asdfasdf.mp4" -filter_complex \
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
