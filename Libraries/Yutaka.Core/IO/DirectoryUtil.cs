using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Yutaka.Core.IO
{
	public static class DirectoryUtil
	{
		private static readonly int InitialStackCapacity = 100;

		/// <summary>
		/// Returns an enumerable collection of full file names that match a search pattern in a specified path, and optionally searches subdirectories.
		/// </summary>
		/// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
		/// <param name="searchPattern">The search string to match against the names of files in &lt;path&gt;. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
		/// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories. The default value is <see cref="SearchOption.TopDirectoryOnly"/>.</param>
		/// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by &lt;path&gt; and that match the specified search pattern and search option.</returns>
		public static IEnumerable<string> EnumerateFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			#region Check Input
			if (path == null)
				throw new ArgumentNullException("path");
			else if (String.IsNullOrWhiteSpace(path))
				throw new ArgumentException("'path' is whitespace.", "path");
			if (!Directory.Exists(path))
				throw new ArgumentException(String.Format("Path '{0}' does not exist.", path), "path");
			if (String.IsNullOrEmpty(searchPattern))
				searchPattern = "*";
			#endregion Check Input

			if (searchOption == SearchOption.TopDirectoryOnly) {
				foreach (var file in Directory.EnumerateFiles(path, searchPattern, searchOption))
					yield return file;
			}

			else {
				IEnumerable<string> files;
				string currentDir;
				var dirs = new Stack<string>(InitialStackCapacity);
				dirs.Push(path);

				while (dirs.Count > 0) {
					currentDir = dirs.Pop();

					try {
						files = Directory.EnumerateFiles(currentDir, searchPattern, searchOption);
					}

					catch {
						continue;
					}

					foreach (var file in files)
						yield return file;

					try {
						foreach (var dir in Directory.EnumerateDirectories(currentDir))
							dirs.Push(dir);
					}

					catch {
						continue;
					}
				}
			}
		}

		/// <summary>
		/// Returns an enumerable collection of full image file names in a specified path, and optionally searches subdirectories.
		/// </summary>
		/// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
		/// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories. The default value is <see cref="SearchOption.TopDirectoryOnly"/>.</param>
		/// <returns>An enumerable collection of the full names (including paths) for the image files in the directory specified by &lt;path&gt; and that match the specified search option.</returns>
		public static IEnumerable<string> EnumerateImageFiles(string path, SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			#region Check Input
			if (path == null)
				throw new ArgumentNullException("path");
			else if (String.IsNullOrWhiteSpace(path))
				throw new ArgumentException("'path' is whitespace.", "path");
			if (!Directory.Exists(path))
				throw new ArgumentException(String.Format("Path '{0}' does not exist.", path), "path");
			#endregion Check Input

			if (searchOption == SearchOption.TopDirectoryOnly) {
				foreach (var file in Directory.EnumerateFiles(path).Where(x => x.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".ico", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".wdp", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".webm", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".webp", StringComparison.OrdinalIgnoreCase)))
					yield return file;
			}

			else {
				IEnumerable<string> files;
				string currentDir;
				var dirs = new Stack<string>(InitialStackCapacity);
				dirs.Push(path);

				while (dirs.Count > 0) {
					currentDir = dirs.Pop();

					try {
						files = Directory.EnumerateFiles(currentDir).Where(x => x.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".ico", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".wdp", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".webm", StringComparison.OrdinalIgnoreCase) ||
																				x.EndsWith(".webp", StringComparison.OrdinalIgnoreCase));
					}

					catch {
						continue;
					}

					foreach (var file in files)
						yield return file;

					try {
						foreach (var dir in Directory.EnumerateDirectories(currentDir))
							dirs.Push(dir);
					}

					catch {
						continue;
					}
				}
			}
		}
	}
}