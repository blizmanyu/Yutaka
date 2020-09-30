using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Yutaka.Core.IO
{
	public static class DirectoryUtil
	{
		private static readonly int InitialStackCapacity = 200;

		/// <summary>
		/// Returns the names of files (including their paths) that match the specified search pattern in the specified directory and subdirectories.
		/// </summary>
		/// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
		/// <param name="searchPattern">The search string to match against the names of files in &lt;path&gt;. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
		/// <returns>A list of the full names (including paths) for the files in the specified directory that match the specified search pattern, or an empty list if no files are found.</returns>
		public static IList<string> GetFiles(string path, string searchPattern = "*")
		{
			#region Check Input
			if (String.IsNullOrWhiteSpace(path))
				throw new ArgumentException("'path' is null or whitespace.", "path");
			if (!Directory.Exists(path))
				throw new ArgumentException(String.Format("'path' '{0}' does not exist.", path), "path");
			if (String.IsNullOrWhiteSpace(searchPattern))
				searchPattern = "*";
			#endregion Check Input

			try {
				string currentDir;
				var files = new List<string>();
				var dirs = new Stack<string>(InitialStackCapacity);
				dirs.Push(path);

				while (dirs.Count > 0) {
					currentDir = dirs.Pop();

					try {
						files.AddRange(Directory.EnumerateFiles(currentDir, searchPattern, SearchOption.TopDirectoryOnly));

						foreach (var dir in Directory.EnumerateDirectories(currentDir))
							dirs.Push(dir);
					}

					catch (Exception) {
						continue;
					}
				}

				return files;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in DirectoryUtil.GetFiles(string path='{3}', string searchPattern='{4}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, path, searchPattern));
				else
					throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of DirectoryUtil.GetFiles(string path='{3}', string searchPattern='{4}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, path, searchPattern));
			}
		}

		/// <summary>
		/// Returns the names of image files (including their paths) in the specified directory and subdirectories.
		/// </summary>
		/// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
		/// <returns>A list of the full names (including paths) for the image files in the specified directory, or an empty list if no files are found.</returns>
		public static IList<string> GetImageFiles(string path)
		{
			#region Check Input
			if (String.IsNullOrWhiteSpace(path))
				throw new ArgumentException("'path' is null or whitespace.", "path");
			if (!Directory.Exists(path))
				throw new ArgumentException(String.Format("'path' '{0}' does not exist.", path), "path");
			#endregion Check Input

			try {
				string currentDir;
				var files = new List<string>();
				var dirs = new Stack<string>(InitialStackCapacity);
				dirs.Push(path);

				while (dirs.Count > 0) {
					currentDir = dirs.Pop();

					try {
						files.AddRange(Directory.EnumerateFiles(currentDir).Where(x => x.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
																					   x.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
																					   x.EndsWith(".ico", StringComparison.OrdinalIgnoreCase) ||
																					   x.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
																					   x.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
																					   x.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
																					   x.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase) ||
																					   x.EndsWith(".wdp", StringComparison.OrdinalIgnoreCase) ||
																					   x.EndsWith(".webm", StringComparison.OrdinalIgnoreCase) ||
																					   x.EndsWith(".webp", StringComparison.OrdinalIgnoreCase)));

						foreach (var dir in Directory.EnumerateDirectories(currentDir))
							dirs.Push(dir);
					}

					catch (Exception) {
						continue;
					}
				}

				return files;
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in DirectoryUtil.GetImageFiles(string path='{3}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, path));
				else
					throw new Exception(String.Format("{0}{2}Exception thrown in INNER EXCEPTION of DirectoryUtil.GetImageFiles(string path='{3}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, path));
			}
		}
	}
}