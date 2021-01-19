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
			if (imagePath == null)
				throw new ArgumentNullException("imagePath");
			else if (String.IsNullOrWhiteSpace(imagePath))
				throw new ArgumentException("'imagePath' is empty or whitespace.", "imagePath");
			if (!File.Exists(imagePath))
				throw new FileNotFoundException("File not found.", imagePath);
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
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in DrawingUtil.ScaleImage(string imagePath='{3}', int maxWidth='{4}', int maxHeight='{5}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, imagePath, maxWidth, maxHeight));
				else
					throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of DrawingUtil.ScaleImage(string imagePath='{3}', int maxWidth='{4}', int maxHeight='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, imagePath, maxWidth, maxHeight));
			}
		}
	}
}