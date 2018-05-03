﻿using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace Yutaka.Images
{
	public static class ImageUtil
	{
		private static HttpWebRequest request;

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

		public static bool UrlExistsAndValid(string url)
		{
			if (String.IsNullOrEmpty(url))
				return false;

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
	}
}