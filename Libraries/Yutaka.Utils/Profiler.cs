using System;
using System.IO;

namespace Yutaka.Utils
{
	public static class Profiler
	{
		#region Fields
		const string LOG_FOLDER = @"C:\Logs\";
		private static ProfileSample[] samples;
		private static ProfileSampleHistory[] history;
		private static DateTime startTime;
		private static DateTime endTime;
		private static int maxProfileSamples;
		private static string name;
		#endregion

		#region Private Helpers
		private static void OutputToConsole(string buffer)
		{
			Console.Write(buffer);
		}

		private static void OutputToEmail(string buffer)
		{

		}

		private static void OutputToFile(string buffer)
		{
			var now = DateTime.Now;
			var programFolder = LOG_FOLDER + name + @"\";
			var filename = programFolder + now.ToString("yyyy MMdd HHmm ssff") + " Profiler" + ".txt";

			if (!Directory.Exists(LOG_FOLDER))
				Directory.CreateDirectory(LOG_FOLDER);

			if (!Directory.Exists(programFolder))
				Directory.CreateDirectory(programFolder);

			File.WriteAllText(filename, buffer.Replace("\n", Environment.NewLine));
		}
		#endregion

		#region Public Methods
		public static void Init(string programName, int size = 2048)
		{
			int i;
			samples = new ProfileSample[size];
			history = new ProfileSampleHistory[size];

			for (i=0; i<size; i++) {
				samples[i] = new ProfileSample();
				samples[i].IsValid = false;
				history[i] = new ProfileSampleHistory();
				history[i].IsValid = false;
			}

			name = programName;
			maxProfileSamples = size;
			startTime = DateTime.Now;
		}

		public static void Begin(string name)
		{
			var i = 0;

			while (i<maxProfileSamples && samples[i].IsValid) {
				if (samples[i].Name == name || samples[i].Name.ToLower() == name.ToLower()) {
					/* Found the sample */
					samples[i].OpenProfiles++;
					samples[i].ProfileInstances++;
					samples[i].StartTime = DateTime.Now;
					if (samples[i].OpenProfiles == 1)
						return;
					else
						throw new Exception("Max 1 open at a time: " + name);
				}
				i++;
			}

			if (i >= maxProfileSamples)
				throw new Exception("Exceeded Max Available Profile Samples: " + i);

			samples[i].Name = name;
			samples[i].IsValid = true;
			samples[i].OpenProfiles = 1;
			samples[i].ProfileInstances = 1;
			samples[i].Accumulator = new TimeSpan();
			samples[i].StartTime = DateTime.Now;
			samples[i].ChildrenSampleTime = new TimeSpan();
		}

		public static void End(string name)
		{
			var i = 0;
			var numParents = 0;

			while (i<maxProfileSamples && samples[i].IsValid) {
				if (samples[i].Name == name || samples[i].Name.ToLower() == name.ToLower()) {
					/* Found the sample */
					var inner = 0;
					var parent = -1;
					var endTime = DateTime.Now;
					samples[i].OpenProfiles--;

					/* Count all parents & find the immediate parent */
					while (samples[inner].IsValid) {
						if (samples[inner].OpenProfiles > 0) {
							/* Found parent */
							numParents++;
							if (parent < 0)
								parent = inner;
							else if (samples[inner].StartTime >= samples[parent].StartTime)
								parent = inner;
						}
						inner++;
					}

					/* Remember the current number of parents of the sample */
					samples[i].NumParents = numParents;

					if (parent >= 0) {
						/* Record this time in ChildrenSampleTime (add it) */
						samples[parent].ChildrenSampleTime += endTime - samples[i].StartTime;
					}

					/* Save sample time in accumulator */
					samples[i].Accumulator += endTime - samples[i].StartTime;
					return;
				}
				i++;
			}
		}

		public static void DumpOutputToBuffer(bool console=true, bool file=true, bool email=false)
		{
			endTime = DateTime.Now;
			var totalCalls = 0;
			var buffer = "\n";
			buffer += "\n   Time   |   %   |   #  | Profile Name";
			buffer += "\n---------------------------------------";

			foreach (var samp in samples) {
				if (samp.IsValid) {
					int indent;
					double percentTime;
					string indentedName, num, time, percent;

					if (samp.OpenProfiles < 0)
						throw new Exception("End() called without a Begin(): " + samp.Name);

					else if (samp.OpenProfiles > 0)
						throw new Exception("Begin() called without a End(): " + samp.Name);

					var sampleTime = samp.Accumulator - samp.ChildrenSampleTime;
					percentTime = (sampleTime.TotalSeconds / (endTime - startTime).TotalSeconds) * 100;

					totalCalls += samp.ProfileInstances;
					time = sampleTime.ToString(@"hh\:mm\:ss");
					percent = percentTime.ToString("#0.0").PadLeft(6);
					num = samp.ProfileInstances.ToString().PadLeft(5);

					indentedName = "";
					for (indent=0; indent<samp.NumParents; indent++)
						indentedName += "   ";
					indentedName += samp.Name;

					buffer += "\n " + time + " |" + percent + " |" + num + " | " + indentedName;
				}
			}

			buffer += "\n";
			buffer += "\n " + (endTime - startTime).ToString(@"hh\:mm\:ss") +" | 100.0 |" + totalCalls.ToString().PadLeft(5) + " | TOTAL";

			if (console)
				OutputToConsole(buffer);
			if (file)
				OutputToFile(buffer);
			if (email)
				OutputToEmail(buffer);
		}
		#endregion

		#region Nested Classes
		public class ProfileSample
		{
			public bool IsValid { get; set; }					// whether this data is valid
			public int ProfileInstances { get; set; }			// # times Begin() is called
			public int OpenProfiles { get; set; }				// # times Begin() w/o End()
			public string Name { get; set; }					// name of sample
			public DateTime StartTime { get; set; }				// current open profile start time
			public TimeSpan Accumulator { get; set; }			// ALL samples of this name
			public TimeSpan ChildrenSampleTime { get; set; }	// time taken by all children
			public int NumParents { get; set; }					// # profile parents
		}

		public class ProfileSampleHistory
		{
			public bool IsValid { get; set; }
			public string Name { get; set; }
			public TimeSpan Ave { get; set; }
			public TimeSpan Min { get; set; }
			public TimeSpan Max { get; set; }
		}
		#endregion
	}
}