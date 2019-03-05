using System;
using System.Collections.Generic;
using System.IO;
using WMPLib;
using Yutaka.IO;

namespace Yutaka.Video
{
	public class VideoUtil
	{
		private List<string> Images;
		public int FirstXMin;
		public int Fps;
		public int GifLength;
		public int NumMiddleSegments;
		public int Width;
		public double StartTime;
		public string DestFolder;
		public string DestFile;
		public string Source;

		private void CreateDestFile()
		{
			Directory.CreateDirectory(DestFolder);
			var dest = String.Format("{0}{1}", DestFolder, DestFile);

			if (File.Exists(dest))
				return;

			FileUtil.Write(String.Format("@echo off{0}", Environment.NewLine), dest);
		}

		public VideoUtil(string source, double startTime=0)
		{
			if (String.IsNullOrWhiteSpace(source))
				throw new Exception(String.Format("<source> is required.{0}Exception thrown in VideoUtil.VideoUtil(string source).{0}{0}", Environment.NewLine));

			Images = new List<string>();
			StartTime = startTime;
			FirstXMin = 5;
			Fps = 10;
			GifLength = 10;
			NumMiddleSegments = 21;
			Width = 1000;
			DestFolder = String.Format(@"C:\Temp\{0:yyyy MMdd HHmm ssff}\", DateTime.Now);
			DestFile = String.Format(@"{0:yyyy MMdd HHmm ssff}.bat", DateTime.Now);
			Source = source;
		}

		public void CreateAnimatedGif(TimeSpan startTime, double length)
		{
			try {
				CreateDestFile();
				var dest = String.Format("{0}REPLACE_ME{1:hh}h {1:mm}m {1:ss}s {1:fff}f", DestFolder, startTime);
				var arg = String.Format("ffmpeg -y -ss {0} -t {1} -i \"{2}\" -vf fps={3},scale={4}:-1:flags=lanczos,palettegen \"{5}.png\"", startTime.ToString(@"hh\:mm\:ss\.fff"), length, Source, Fps, Width, dest.Replace("REPLACE_ME", "z"));
				FileUtil.Write(String.Format("{0}{1}", arg, Environment.NewLine), String.Format("{0}{1}", DestFolder, DestFile));
				arg = String.Format("ffmpeg -y -ss {0} -t {1} -i \"{2}\" -i \"{3}.png\" -filter_complex \"fps={4},scale={5}:-1:flags=lanczos[x];[x][1:v]paletteuse\" \"{6}.gif\"", startTime.ToString(@"hh\:mm\:ss\.fff"), length, Source, dest.Replace("REPLACE_ME", "z"), Fps, Width, dest.Replace("REPLACE_ME", ""));
				FileUtil.Write(String.Format("{0}{1}", arg, Environment.NewLine), String.Format("{0}{1}", DestFolder, DestFile));
				Images.Add(String.Format("{0}.gif", dest.Replace("REPLACE_ME", "")));
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in VideoUtil.CreateAnimatedGif(TimeSpan startTime, double length={3}).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, length));

				throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateAnimatedGif(TimeSpan startTime, double length={3}).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, length));
			}
		}

		// Do Not Use // Work in Progress //
		public void CreateAnimatedGif(double start = 0, double end = -1)
		{
			if (start < 0)
				start = StartTime;
			if (end < 1)
				end = GetDuration(Source);

			try {
				CreateDestFile();
				var p1 = start + 300;
				var p2 = p1 + 60;
				var p3 = end - 120;

				// First 5min // 30 GIFs //
				CreateFirstXMin(start, FirstXMin, GifLength);

				// Middle // 21 GIFs //
				CreateEqualSegments(p1, p3, 21);

				// Last 2min // 12 GIFs //
				for (var i = p3; i < end; i += GifLength)
					CreateAnimatedGif(TimeSpan.FromSeconds(i), GifLength);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in VideoUtil.CreateAnimatedGif(double start={3}, double end={4}).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, start, end));

				throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateAnimatedGif(double start={3}, double end={4}).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, start, end));
			}
		}

		public void CreateEqualSegments(double start = -1, double end = -1, int numSegments = -1)
		{
			if (start < 0)
				start = 0;
			if (end < 1)
				end = GetDuration(Source);
			if (numSegments < 1)
				numSegments = NumMiddleSegments;

			try {
				CreateDestFile();
				var segment = (end - start) / (numSegments + 1);

				for (var i = start + segment; i < end; i += segment)
					CreateAnimatedGif(TimeSpan.FromSeconds(i), GifLength);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in VideoUtil.CreateEqualSegments(double start={3}, double end={4}, int numSegments={5}).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, start, end, numSegments));

				throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateEqualSegments(double start={3}, double end={4}, int numSegments={5}).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, start, end, numSegments));
			}
		}

		public void CreateFirstXMin(double start = -1, int min = -1, int interval =-1)
		{
			if (start < 0)
				start = 0;
			if (min < 1)
				min = FirstXMin;
			if (interval < 2)
				interval = GifLength;

			try {
				CreateDestFile();
				var end = start + (60 * min);

				for (var i = start; i < end; i += interval)
					CreateAnimatedGif(TimeSpan.FromSeconds(i), interval);
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in VideoUtil.FirstXMin(double start={3}, int min={4}, int interval={5}).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, start, min, interval));

				throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VideoUtil.FirstXMin(double start={3}, int min={4}, int interval={5}).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, start, min, interval));
			}
		}

		public void CreateHtml()
		{
			var dest = String.Format("{0}{1}", DestFolder, DestFile);
			var html = String.Format("<html>{0}<body>{0}<div>{0}REPLACE_ME</div>{0}</body>{0}</html>", Environment.NewLine);
			
			for (int i=0; i< Images.Count; i++)
				html = html.Replace("REPLACE_ME", String.Format("<img src='{0}' style='width:33%;max-width:320px;height:auto' />{1}REPLACE_ME", Images[i], Environment.NewLine));

			html = html.Replace("REPLACE_ME", "");
			FileUtil.Write(html, String.Format("{0}html", dest.Substring(0, dest.Length-3)));
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

		#region Commented Out
		//ffmpeg -i "G:\Projects\asdfasdf.mp4" -filter_complex \
		//"[0:v]trim=start=10:end=12,setpts=PTS-STARTPTS[a]; \
		// [0:v]trim=start=72:end=74,setpts=PTS-STARTPTS[b]; \
		// [a][b]concat[c]; \
		// [0:v]trim=start=134:end=136,setpts=PTS-STARTPTS[d]; \
		// [c][d]concat[e]; \
		// [0:v]trim=start=196:end=198,setpts=PTS-STARTPTS[f]; \
		// [e][f]concat[g]; \
		// [0:v]trim=start=258:end=260,setpts=PTS-STARTPTS[h]; \
		// [g][h]concat[i]; \
		// [0:v]trim=start=320:end=322,setpts=PTS-STARTPTS[j]; \
		// [i][j]concat[k]; \
		// [0:v]trim=start=382:end=384,setpts=PTS-STARTPTS[l]; \
		// [k][l]concat[out1]" -map [out1] _out.mkv

		#region Feb 26, 2019 - CreateVersion1(string source, string destFolder, double start = 0, double end = -1)
		//public void CreateVersion1(string source, string destFolder, double start = 0, double end = -1)
		//{
		//	if (String.IsNullOrWhiteSpace(source))
		//		throw new Exception(String.Format("<source> is required.{0}Exception thrown in VideoUtil.CreateVersion1(string source, string destFolder, double start, double end).{0}{0}", Environment.NewLine));
		//	if (String.IsNullOrWhiteSpace(destFolder))
		//		throw new Exception(String.Format("<destFolder> is required.{0}Exception thrown in VideoUtil.CreateVersion1(string source, string destFolder, double start, double end).{0}{0}", Environment.NewLine));
		//	if (start < 0)
		//		start = 0;
		//	if (end < 1)
		//		end = GetDuration(source);

		//	try {
		//		Console.Write("\nsource: {0}", source);
		//		Console.Write("\ndestFolder: {0}", destFolder);
		//		Console.Write("\nstart: {0}", start);
		//		Console.Write("\nend: {0}", end);
		//		var interval = (end - start) / 7;
		//		Console.Write("\ninterval: {0}", interval);

		//		for (var i = start; i < end; i += interval) {
		//			CreateAnimatedGif(TimeSpan.FromSeconds(i), 2, source, destFolder);
		//		}
		//	}

		//	catch (Exception ex) {
		//		if (ex.InnerException == null)
		//			throw new Exception(String.Format("{0}{2}Exception thrown in VideoUtil.CreateVersion1(string source='{3}', string destFolder='{4}', double start={5}, double end={6}).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, source, destFolder, start, end));

		//		throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateVersion1(string source='{3}', string destFolder='{4}', double start={5}, double end={6}).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, source, destFolder, start, end));
		//	}
		//}
		#endregion Feb 26, 2019 - CreateVersion1(string source, string destFolder, double start = 0, double end = -1)

		#region Feb 26, 2019 - CreateAnimatedGif(string source, double start = 0, double end = -1)
		//public void CreateAnimatedGif(string source, double start = 0, double end = -1)
		//{
		//	if (String.IsNullOrWhiteSpace(source))
		//		throw new Exception(String.Format("<source> is required.{0}Exception thrown in VideoUtil.CreateAnimatedGif(string source, double start, double end).{0}{0}", Environment.NewLine));
		//	if (start < 0)
		//		start = 0;
		//	if (end < 1)
		//		end = GetDuration(source);

		//	try {
		//		var destFolder = String.Format(@"C:\Temp\{0:yyyy MMdd HHmm ssff}\", DateTime.Now);
		//		var p1 = start + 120;
		//		var p2 = p1 + 60;
		//		var p4 = end - 120;
		//		var p3 = p4 - 60;

		//		// First 5min // 30 GIFs //
		//		for (var i = start; i < p1; i += 10)
		//			CreateAnimatedGif(TimeSpan.FromSeconds(i), 10, source, destFolder, 10, 1000);

		//		// Middle // 21 GIFs //
		//		for (var i = p2; i < p3; i += 70)
		//			CreateAnimatedGif(TimeSpan.FromSeconds(i), 10, source, destFolder, 10, 1000);

		//		// Last 2min // 12 GIFs //
		//		for (var i = p4; i < end; i += 10)
		//			CreateAnimatedGif(TimeSpan.FromSeconds(i), 10, source, destFolder, 10, 1000);
		//	}

		//	catch (Exception ex) {
		//		if (ex.InnerException == null)
		//			throw new Exception(String.Format("{0}{2}Exception thrown in VideoUtil.CreateAnimatedGif(string source='{3}', double start={4}, double end={5}).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, source, start, end));

		//		throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateAnimatedGif(string source='{3}', double start={4}, double end={5}).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, source, start, end));
		//	}
		//}
		#endregion Feb 26, 2019 - CreateAnimatedGif(string source, double start = 0, double end = -1)

		#region Feb 26, 2019 - CreateAnimatedGif(TimeSpan startTime, double length, string source, string destFolder, int fps = 15, int width = 640)
		//public void CreateAnimatedGif(TimeSpan startTime, double length, string source, string destFolder, int fps = 15, int width = 640)
		//{
		//	if (String.IsNullOrWhiteSpace(source))
		//		throw new Exception(String.Format("<source> is required.{0}Exception thrown in VideoUtil.CreateAnimatedGif(TimeSpan startTime, int length, string source, string destFolder, int fps, int width).{0}{0}", Environment.NewLine));
		//	if (!File.Exists(source))
		//		throw new Exception(String.Format("'{1}' doesn't exist.{0}Exception thrown in VideoUtil.CreateAnimatedGif(TimeSpan startTime, int length, string source, string destFolder, int fps, int width).{0}{0}", Environment.NewLine, source));

		//	try {
		//		int exitCode;
		//		var arg = "";
		//		var dest = "";

		//		using (var p = new Process()) {
		//			dest = String.Format("{0}{1:hh}h {1:mm}m {1:ss}s {1:fff}f", destFolder, startTime);
		//			Console.Write("\npng: {0}", dest);
		//			arg = String.Format("-y -ss {0} -t {1} -i \"{2}\" -vf fps={3},scale={4}:-1:flags=lanczos,palettegen \"{5}.png\"", startTime.ToString(@"hh\:mm\:ss\.fff"), length, source, fps, width, dest);
		//			Console.Write("\narg: {0}", arg);

		//			p.StartInfo.RedirectStandardOutput = true;
		//			p.StartInfo.RedirectStandardError = true;
		//			p.StartInfo.FileName = FFMPEG_PATH;
		//			p.StartInfo.Arguments = arg;
		//			p.StartInfo.UseShellExecute = false;
		//			p.StartInfo.CreateNoWindow = true;
		//			//Console.Write("\nParent Name: {0}", Directory.GetParent(source).Name);
		//			//Console.Write("\nParent FullName: {0}", Directory.GetParent(source).FullName);
		//			p.StartInfo.WorkingDirectory = Directory.GetParent(source).FullName + "\\";
		//			p.Start();
		//			p.WaitForExit();

		//			exitCode = p.ExitCode;
		//			//if (exitCode != 0)
		//			//	Console.Write("\nresult = {0}", p.StandardOutput.ReadToEnd());

		//			Console.Write("\nExitCode: {0}", exitCode);
		//		}

		//		if (exitCode == 0) {
		//			using (var p2 = new Process()) {
		//				arg = String.Format("-y -ss {0} -t {1} -i \"{2}\" -i \"{3}.png\" -filter_complex \"fps={4},scale={5}:-1:flags=lanczos[x];[x][1:v]paletteuse\" \"{6}.gif\"", startTime.ToString(@"hh\:mm\:ss\.fff"), length, source, dest, fps, width, dest);
		//				Console.Write("\narg: {0}", arg);

		//				p2.StartInfo.RedirectStandardOutput = true;
		//				p2.StartInfo.RedirectStandardError = true;
		//				p2.StartInfo.FileName = FFMPEG_PATH;
		//				p2.StartInfo.Arguments = arg;
		//				p2.StartInfo.UseShellExecute = false;
		//				p2.StartInfo.CreateNoWindow = true;
		//				p2.StartInfo.WorkingDirectory = Directory.GetParent(source).FullName + "\\";
		//				p2.Start();
		//				p2.WaitForExit(12000);
		//				p2.Close();
		//				//Thread.Sleep(2200);
		//				//p.Kill();

		//				//exitCode = p.ExitCode;
		//				//Console.Write("\nExitCode: {0}", exitCode);

		//				//if (exitCode == 0) {
		//				//	//Console.Write("\nDeleteing PNG palette...");
		//				//File.Delete(String.Format("{0}.png", dest));
		//				//}
		//			}
		//		}
		//	}

		//	catch (Exception ex) {
		//		if (ex.InnerException == null)
		//			Console.Write("{0}{2}{2}Exception thrown in VideoUtil.CreateAnimatedGif(){2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine);

		//		Console.Write("{0}{2}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateAnimatedGif(){2}{2}{1}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);
		//	}
		//}
		#endregion Feb 26, 2019 - CreateAnimatedGif(TimeSpan startTime, double length, string source, string destFolder, int fps = 15, int width = 640)

		#region Feb 26, 2019 - CreateAllBetween(string source, string destFolder = null, double start = 0, double end = -1, double interval = -1, double length = -1)
		//public void CreateAllBetween(string source, string destFolder = null, double start = 0, double end = -1, double interval = -1, double length = -1)
		//{
		//	if (String.IsNullOrWhiteSpace(source))
		//		throw new Exception(String.Format("<source> is required.{0}Exception thrown in VideoUtil.CreateVersion1(string source, string destFolder, double start, double end).{0}{0}", Environment.NewLine));
		//	if (String.IsNullOrWhiteSpace(destFolder))
		//		destFolder = String.Format(@"C:\Temp\{0:yyyy MMdd HHmm ssff}\", DateTime.Now);
		//	if (start < 0)
		//		start = 0;
		//	if (end < 1)
		//		end = start + 120;
		//	if (interval < 1)
		//		interval = 10;
		//	if (length < 1)
		//		length = 10;

		//	try {
		//		Console.Write("\nsource: {0}", source);
		//		Console.Write("\ndestFolder: {0}", destFolder);
		//		Console.Write("\nstart: {0}", start);
		//		Console.Write("\nend: {0}", end);

		//		for (var i = start; i < end; i += interval)
		//			CreateAnimatedGif(TimeSpan.FromSeconds(i), length, source, destFolder);
		//	}

		//	catch (Exception ex) {
		//		if (ex.InnerException == null)
		//			throw new Exception(String.Format("{0}{2}Exception thrown in VideoUtil.CreateVersion1(string source='{3}', string destFolder='{4}', double start={5}, double end={6}).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, source, destFolder, start, end));

		//		throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateVersion1(string source='{3}', string destFolder='{4}', double start={5}, double end={6}).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, source, destFolder, start, end));
		//	}
		//}
		#endregion Feb 26, 2019 - CreateAllBetween(string source, string destFolder = null, double start = 0, double end = -1, double interval = -1, double length = -1)

		#region Feb 26, 2019 - CreateGallery(string source, string destFolder, string extension = "jpg", double start = 0, double end = -1)
		//public void CreateGallery(string source, string destFolder, string extension = "jpg", double start = 0, double end = -1)
		//{
		//	if (String.IsNullOrWhiteSpace(source))
		//		throw new Exception(String.Format("<source> is required.{0}Exception thrown in VideoUtil.CreateGallery(string source, string destFolder, double start, double end).{0}{0}", Environment.NewLine));
		//	if (String.IsNullOrWhiteSpace(destFolder))
		//		throw new Exception(String.Format("<destFolder> is required.{0}Exception thrown in VideoUtil.CreateGallery(string source, string destFolder, double start, double end).{0}{0}", Environment.NewLine));
		//	if (start < 0)
		//		start = 0;
		//	if (end < 1)
		//		end = GetDuration(source);

		//	try {
		//		Console.Write("\nsource: {0}", source);
		//		Console.Write("\ndestFolder: {0}", destFolder);
		//		Console.Write("\nstart: {0}", start);
		//		Console.Write("\nend: {0}", end);

		//		for (var i = start; i < end; i++)
		//			CreateSingleImage(TimeSpan.FromSeconds(i), source, destFolder, extension);
		//	}

		//	catch (Exception ex) {
		//		if (ex.InnerException == null)
		//			throw new Exception(String.Format("{0}{2}Exception thrown in VideoUtil.CreateGallery(string source='{3}', string destFolder='{4}', double start={5}, double end={6}).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, source, destFolder, start, end));

		//		throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateGallery(string source='{3}', string destFolder='{4}', double start={5}, double end={6}).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, source, destFolder, start, end));
		//	}
		//}
		#endregion Feb 26, 2019 - CreateGallery(string source, string destFolder, string extension = "jpg", double start = 0, double end = -1)

		#region Feb 26, 2019 - CreateSingleImage(TimeSpan startTime, string source, string destFolder, string extension = "jpg")
		//public void CreateSingleImage(TimeSpan startTime, string source, string destFolder, string extension = "jpg")
		//{
		//	if (String.IsNullOrWhiteSpace(source))
		//		throw new Exception(String.Format("<source> is required.{0}Exception thrown in VideoUtil.CreateSingleImage(TimeSpan startTime, string source, string destFolder).{0}{0}", Environment.NewLine));
		//	if (!File.Exists(source))
		//		throw new Exception(String.Format("'{1}' doesn't exist.{0}Exception thrown in VideoUtil.CreateSingleImage(TimeSpan startTime, string source, string destFolder).{0}{0}", Environment.NewLine, source));

		//	try {
		//		using (var p = new Process()) {
		//			var dest = String.Format("{0}{1:hh}h {1:mm}m {1:ss}s {1:fff}f", destFolder, startTime);
		//			var arg = String.Format("-y -ss {0} -i \"{1}\" -frames:v 1 \"{2}.{3}\"", startTime.ToString(@"hh\:mm\:ss\.fff"), source, dest, extension);
		//			Console.Write("\narg: {0}", arg);

		//			p.StartInfo.RedirectStandardOutput = true;
		//			p.StartInfo.RedirectStandardError = true;
		//			p.StartInfo.FileName = FFMPEG_PATH;
		//			p.StartInfo.Arguments = arg;
		//			p.StartInfo.UseShellExecute = false;
		//			p.StartInfo.CreateNoWindow = true;
		//			Console.Write("\nParent FullName: {0}", Directory.GetParent(source).FullName);
		//			p.StartInfo.WorkingDirectory = Directory.GetParent(source).FullName + "\\";
		//			p.Start();
		//			p.WaitForExit();
		//		}
		//	}

		//	catch (Exception ex) {
		//		if (ex.InnerException == null)
		//			Console.Write("{0}{2}{2}Exception thrown in VideoUtil.CreateSingleImage(TimeSpan startTime={3}, string source='{4}', string destFolder='{5}'){2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine, startTime, source, destFolder);

		//		Console.Write("{0}{2}{2}Exception thrown in INNER EXCEPTION of VideoUtil.CreateSingleImage(TimeSpan startTime={3}, string source='{4}', string destFolder='{5}'){2}{2}{1}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, startTime, source, destFolder);
		//	}
		//}
		#endregion Feb 26, 2019 - CreateSingleImage(TimeSpan startTime, string source, string destFolder, string extension = "jpg")
		#endregion Commented Out
	}
}