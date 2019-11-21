namespace Yutaka.IO2
{
	/// <summary>
	/// Specifies whether to overwrite, skip, keep both, or use the "smart" algorithm.
	/// </summary>
	public enum OverwriteOption
	{
		Overwrite,
		Skip,
		KeepBoth,
		/// <summary>
		/// If the files are the same, it will skip to save resources. Otherwise, it will rename to keep both files.
		/// </summary>
		Smart,
	}
}