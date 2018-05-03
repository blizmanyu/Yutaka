using System;
using System.Collections.Generic;
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
			if (String.IsNullOrEmpty(filePath) || !File.Exists(filePath))
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
			if (String.IsNullOrEmpty(url))
				return false;

			var contains = false;
			url = url.ToUpper();

			for (int i=0; i<supportedExtensions.Length; i++) {
				if (url.Contains(supportedExtensions[i])) {
					contains = true;
					break;
				}
			}

			if (contains) {
				Console.Write("\nsupportedExtensions.Contains(url)");
				request = (HttpWebRequest) WebRequest.Create(url);
				request.KeepAlive = true;
				request.Method = "HEAD";
				request.Proxy = null;

				try {
					using (var response = (HttpWebResponse) request.GetResponse()) {
						var len = response.ContentLength;

						if (len > 1024)
							return true;
						else
							return false;
					}
				}

				catch (Exception) {
					return false;
				}
			}

			else {
				Console.Write("\n!!!supportedExtensions.Contains(url)");
				return false;
			}
		}
	}
}