using System.Collections.Generic;

namespace CodeGenerator.Data
{
	public class SqlTable
	{
		public string Name;
		public string Database;
		public string Schema;
		public List<SqlTableColumn> Columns;

		public SqlTable()
		{
			Columns = new List<SqlTableColumn>();
		}
	}
}