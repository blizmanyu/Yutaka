namespace CodeGenerator.Data
{
	public class SqlTableColumn
	{
		public bool IsPrimary;
		public string ColumnName;
		public DataType DataType;
		public int[] DataTypeSize;
		public bool AllowNulls;
	}

	public enum DataType
	{
		Bool, Int, Decimal, Varchar, Nvarchar, Datetime,
	}
}