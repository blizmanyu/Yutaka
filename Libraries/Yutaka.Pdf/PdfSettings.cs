using System.Configuration;

namespace Yutaka.Pdf
{
	public class PdfSettings
	{
		#region Fields
		/// <summary>
		/// PDF logo picture path
		/// </summary>
		public string LogoPicturePath { get; set; }

		/// <summary>
		/// Gets or sets whether letter page size is enabled. Alternative is A4.
		/// </summary>
		public bool LetterPageSizeEnabled { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to render order notes in PDf reports
		/// </summary>
		public bool RenderOrderNotes { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow customers to print PDF invoices for pending orders
		/// </summary>
		public bool EnablePdfInvoicesForPendingOrders { get; set; }

		/// <summary>
		/// Gets or sets the default font folder that will be used
		/// </summary>
		public string FontFolder { get; set; }

		/// <summary>
		/// Gets or sets the default font file name that will be used
		/// </summary>
		public string FontFileName { get; set; }

		/// <summary>
		/// Gets or sets the text that will appear at the bottom of invoices (column 1)
		/// </summary>
		public string InvoiceFooterTextColumn1 { get; set; }

		/// <summary>
		/// Gets or sets the text that will appear at the bottom of invoices (column 2)
		/// </summary>
		public string InvoiceFooterTextColumn2 { get; set; }
		#endregion

		#region Constructor
		/// <summary>
		/// Loads PDF Settings from App.config file. Individual settings can still be changed programmatically after.
		/// </summary>
		public PdfSettings()
		{
			LogoPicturePath = ConfigurationManager.AppSettings["LogoPicturePath"] ?? "";
			FontFolder = ConfigurationManager.AppSettings["FontFolder"] ?? "";
			FontFileName = ConfigurationManager.AppSettings["FontFileName"] ?? "FreeSerif.ttf";
			InvoiceFooterTextColumn1 = ConfigurationManager.AppSettings["InvoiceFooterTextColumn1"] ?? "";
			InvoiceFooterTextColumn2 = ConfigurationManager.AppSettings["InvoiceFooterTextColumn2"] ?? "";

			if (bool.TryParse(ConfigurationManager.AppSettings["LetterPageSizeEnabled"], out var letterPageSizeEnabled))
				LetterPageSizeEnabled = letterPageSizeEnabled;
			else
				LetterPageSizeEnabled = true;

			if (bool.TryParse(ConfigurationManager.AppSettings["RenderOrderNotes"], out var renderOrderNotes))
				RenderOrderNotes = renderOrderNotes;
			else
				RenderOrderNotes = true;

			if (bool.TryParse(ConfigurationManager.AppSettings["EnablePdfInvoicesForPendingOrders"], out var enablePdfInvoicesForPendingOrders))
				EnablePdfInvoicesForPendingOrders = enablePdfInvoicesForPendingOrders;
			else
				EnablePdfInvoicesForPendingOrders = true;
		}
		#endregion
	}
}