using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Yutaka.Core.Drawing
{
	public static class DrawingUtil
	{
		/// <summary>
		/// Resize the image to the specified width and height.
		/// </summary>
		/// <param name="image">The image to resize.</param>
		/// <param name="width">The width to resize to.</param>
		/// <param name="height">The height to resize to.</param>
		/// <returns>The resized image.</returns>
		public static Bitmap ResizeImage(Image image, int width, int height)
		{
			#region Check Input
			var log = "";

			if (image == null)
				log = String.Format("{0}image is null.{1}", log, Environment.NewLine);
			if (width < 1)
				log = String.Format("{0}width is less than 1.{1}", log, Environment.NewLine);
			if (height < 1)
				log = String.Format("{0}height is less than 1.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in DrawingUtil.ResizeImage(Image image, int width, int height).{1}", log, Environment.NewLine);
				throw new Exception(log);
			}
			#endregion

			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);
			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage)) {
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes()) {
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}

		/// <summary>
		/// Resizes an image, keeping the same aspect ratio.
		/// </summary>
		/// <param name="image">The image to resize.</param>
		/// <param name="maxWidth">The max width to resize to.</param>
		/// <param name="maxHeight">The max height to resize to.</param>
		/// <returns>The resized image.</returns>
		public static Bitmap ScaleImage(Image image, int maxWidth, int maxHeight)
		{
			var ratioX = (decimal) maxWidth / image.Width;
			var ratioY = (decimal) maxHeight / image.Height;
			var ratio = Math.Min(ratioX, ratioY);
			var newWidth = Math.Round(image.Width * ratio, MidpointRounding.AwayFromZero);
			var newHeight = Math.Round(image.Height * ratio, MidpointRounding.AwayFromZero);

			return ResizeImage(image, (int) newWidth, (int) newHeight);
		}

		/// <summary>
		/// Resizes an image, keeping the same aspect ratio.
		/// </summary>
		/// <param name="imagePath">The path to the image to resize.</param>
		/// <param name="maxWidth">The max width to resize to.</param>
		/// <param name="maxHeight">The max height to resize to.</param>
		/// <returns>The resized image.</returns>
		public static Bitmap ScaleImage(string imagePath, int maxWidth, int maxHeight)
		{
			#region Check Input
			var log = "";

			if (imagePath == null)
				log = String.Format("{0}imagePath is null.{1}", log, Environment.NewLine);
			else if (String.IsNullOrWhiteSpace(imagePath))
				log = String.Format("{0}imagePath is empty.{1}", log, Environment.NewLine);
			else if (!File.Exists(imagePath))
				log = String.Format("{0}imagePath '{2}' doesn't exist.{1}", log, Environment.NewLine, imagePath);

			if (maxWidth < 1)
				log = String.Format("{0}maxWidth is less than 1.{1}", log, Environment.NewLine);
			if (maxHeight < 1)
				log = String.Format("{0}maxHeight is less than 1.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log)) {
				log = String.Format("{0}Exception thrown in DrawingUtil.ScaleImage(string imagePath, int maxWidth, int maxHeight).{1}", log, Environment.NewLine);
				throw new Exception(log);
			}
			#endregion Check Input

			try {
				using (var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
					using (var image = Image.FromStream(fileStream, false, false)) {
						var ratioX = (decimal) maxWidth / image.Width;
						var ratioY = (decimal) maxHeight / image.Height;
						var ratio = Math.Min(ratioX, ratioY);
						var newWidth = Math.Round(image.Width * ratio, MidpointRounding.AwayFromZero);
						var newHeight = Math.Round(image.Height * ratio, MidpointRounding.AwayFromZero);

						return ResizeImage(image, (int) newWidth, (int) newHeight);
					}
				}
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in DrawingUtil.ScaleImage(string imagePath='{3}', int maxWidth='{4}', int maxHeight='{5}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, imagePath, maxWidth, maxHeight);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of DrawingUtil.ScaleImage(string imagePath='{3}', int maxWidth='{4}', int maxHeight='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, imagePath, maxWidth, maxHeight);

				throw new Exception(log);
				#endregion
			}
		}
	}
}