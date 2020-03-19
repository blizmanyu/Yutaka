namespace Yutaka.IO2
{
	/// <summary>
	/// Specifies whether to overwrite, rename, skip, keep both, or use a "smart" algorithm.
	/// </summary>
	public enum OverwriteOption
	{
		Overwrite,
		OverwriteIfSourceNewer,
		OverwriteIfDifferentSize,
		OverwriteIfDifferentSizeOrSourceNewer,
		Rename,
		Skip,
	}
}