using System;
using System.Drawing;
using System.IO;

namespace Yutaka.Images
{
	public static class ImageUtil
	{
		public static bool FileExistsAndValid(string filepath)
		{
			if (String.IsNullOrEmpty(filepath) || !File.Exists(filepath))
				return false;

			try {
				using (var bmp = new Bitmap(filepath)) { }
				return true;
			}

			catch (Exception) {
				return false;
			}
		}
	}
}