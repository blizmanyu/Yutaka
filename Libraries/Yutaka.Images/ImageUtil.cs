using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace Yutaka.Images
{
	public static class ImageUtil
	{
		private static HttpWebRequest request;
		private static string[] supportedExtensions = { ".BMP", ".EXIF", ".GIF", ".JPG", ".JPEG", ".PNG", ".TIFF" };

		public static bool ExistsAndValidByFilePath(string filePath)
		{
			if (String.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
				return false;

			try {
				using (var bmp = new Bitmap(filePath)) { }
				return true;
			}

			catch (Exception) {
				return false;
			}
		}

		public static bool ExistsAndValidByUrl(string url)
		{
			if (String.IsNullOrWhiteSpace(url))
				return false;

			request = (HttpWebRequest) WebRequest.Create(url);
			request.KeepAlive = true;
			request.Method = "HEAD";
			request.Proxy = null;

			try {
				using (var response = (HttpWebResponse) request.GetResponse()) {
					if (response.ContentLength > 1024)
						return true;
					else
						return false;
				}
			}

			catch (Exception) {
				return false;
			}
		}
	}
}