using System.Data;

namespace Yutaka.Data
{
	/// <summary>
	/// Specifies how a command string is interpreted.
	/// </summary>
	public enum CmdType
	{
		/// <summary>
		/// An SQL text command. (Default.)
		/// </summary>
		Text = CommandType.Text,
		/// <summary>
		/// The name of a stored procedure.
		/// </summary>
		StoredProcedure = CommandType.StoredProcedure,
		/// <summary>
		/// The name of a table.
		/// </summary>
		TableDirect = CommandType.TableDirect
	}
}