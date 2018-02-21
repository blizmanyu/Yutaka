using System;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace Yutaka.Excel
{
    public static class ExcelUtil
    {
		public static void Read(Stream stream, bool hasHeader=true)
		{

		}

		public static void Read(string path, bool hasHeader=true)
		{
			if (String.IsNullOrEmpty(path))
				throw new ArgumentNullException("path", @"<path> is required.");

			using (var ePackage = new ExcelPackage()) {
				using (var stream = File.OpenRead(path)) {
					ePackage.Load(stream);
				}

				var worksheet = ePackage.Workbook.Worksheets.FirstOrDefault();

				if (worksheet == null)
					throw new Exception("No worksheet found");
				if (worksheet.Dimension == null)
					throw new Exception("Empty worksheet");

				//foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column]) {
				//	tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
				//}
				//var startRow = hasHeader ? 2 : 1;
				//for (int rowNum = startRow; rowNum <= worksheet.Dimension.End.Row; rowNum++) {
				//	var wsRow = worksheet.Cells[rowNum, 1, rowNum, worksheet.Dimension.End.Column];
				//	DataRow row = tbl.Rows.Add();
				//	foreach (var cell in wsRow) {
				//		row[cell.Start.Column - 1] = cell.Text;
				//	}
				//}
				//return tbl;
			}
		}
	}
}