using System;
using System.Drawing;
using System.IO;

namespace Yutaka.IO2
{
	public class YuImage : YuFile
	{
		#region Fields
		public int Width;
		public int Height;
		#endregion Fields

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="YuImage"/> class, which acts as a wrapper for an image file path.
		/// </summary>
		/// <param name="filename">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
		public YuImage(string filename = null) : base(filename)
		{
			if (String.IsNullOrWhiteSpace(filename))
				throw new Exception(String.Format("<filename> is required.{0}Exception thrown in Constructor YuImage(string filename).{0}{0}", Environment.NewLine));

			try {
				SetDimensions();
			}

			catch (Exception ex) {
				#region Log
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in Constructor YuImage(string filename='{3}'){2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, filename);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of Constructor YuImage(string filename='{3}'){2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, filename);

				Console.Write("\n{0}", log);
				#endregion Log
			}
		}
		#endregion Constructor

		#region Utilities
		/// <summary>
		/// Dumps all field values to Console.
		/// </summary>
		public void Debug()
		{
			base.Debug();
			Console.Write("\n          Width: {0}", Width);
			Console.Write("\n         Height: {0}", Height);
			Console.Write("\n");
		}

		/// <summary>
		/// Sets the Width and Height WITHOUT loading the whole image.
		/// </summary>
		protected void SetDimensions()
		{
			try {
				using (var fs = new FileStream(FullName, FileMode.Open, FileAccess.Read)) {
					using (var img = Image.FromStream(fs, false, false)) {
						Width = img.Width;
						Height = img.Height;
					}
				}
			}

			catch (Exception) {
				Width = 0;
				Height = 0;
			}
		}
		#endregion Utilities
	}
}