using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Yutaka.Pdf
{
	/// <summary>
	/// PDF Util
	/// </summary>
	public class PdfUtil
	{
		#region Fields
		protected Font AttributesFont;
		protected Font MainFont;
		protected Font TitleFont;
		protected PdfSettings _pdfSettings;
		#endregion

		#region Constructor
		public PdfUtil()
		{
			_pdfSettings = new PdfSettings();
			MainFont = GetFont();
			AttributesFont = GetFont();
			AttributesFont.SetStyle(Font.ITALIC);
			TitleFont = GetFont();
			TitleFont.SetStyle(Font.BOLD);
			TitleFont.Color = BaseColor.BLACK;
		}
		#endregion Constructor

		#region Utilities
		/// <summary>
		/// Gets the default font set in <see cref="PdfSettings.FontFileName"/>.
		/// </summary>
		/// <returns>Font</returns>
		protected Font GetFont()
		{
			// Supports unicode characters
			// Uses Free Serif font by default (~/App_Data/Pdf/FreeSerif.ttf file)
			// It was downloaded from http://savannah.gnu.org/projects/freefont
			return GetFont(_pdfSettings.FontFileName);
		}

		/// <summary>
		/// Gets a font by filename. Optionally, a &lt;folderPath&gt; can be specified if it isn't in the default font folder.
		/// </summary>
		/// <param name="fontFileName">The font filename.</param>
		/// <param name="folderPath">Optional folder path. The default value pulls from <see cref="PdfSettings.FontFolder"/>.</param>
		/// <returns></returns>
		protected Font GetFont(string fontFileName, string folderPath = null)
		{
			#region Input Check
			if (String.IsNullOrWhiteSpace(fontFileName))
				throw new Exception(String.Format("<fontFileName> is required.{0}Exception thrown in PdfUtil.GetFont(string fontFileName, string folderPath).{0}", Environment.NewLine));
			if (String.IsNullOrWhiteSpace(folderPath))
				folderPath = _pdfSettings.FontFolder;
			#endregion

			var fontPath = Path.Combine(folderPath, fontFileName);
			var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
			return new Font(baseFont, 10, Font.NORMAL);
		}

		/// <summary>
		/// Get element alignment
		/// </summary>
		/// <param name="isOpposite">Is opposite?</param>
		/// <returns>Element alignment</returns>
		protected int GetAlignment(bool isOpposite = false)
		{
			// if we need the element to be opposite, like logo etc
			if (!isOpposite)
				return Element.ALIGN_LEFT;

			return Element.ALIGN_RIGHT;
		}

		protected void PrintItems<T>(Document doc, IList<T> items)
		{
			//var productsTable = new PdfPTable(_catalogSettings.ShowSkuOnProductDetailsPage ? 5 : 4);
			//productsTable.HeaderRows = 1;
			//productsTable.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
			//productsTable.WidthPercentage = 100f;
			//if (lang.Rtl) {
			//	productsTable.SetWidths(_catalogSettings.ShowSkuOnProductDetailsPage
			//		? new[] { 15, 10, 15, 15, 45 }
			//		: new[] { 20, 10, 20, 50 });
			//}
			//else {
			//	productsTable.SetWidths(_catalogSettings.ShowSkuOnProductDetailsPage
			//		? new[] { 45, 15, 15, 10, 15 }
			//		: new[] { 50, 20, 10, 20 });
			//}

			//#region Header
			////product name
			//var cellProductItem = new PdfPCell(new Phrase(_localizationService.GetResource("PDFInvoice.ProductName", lang.Id), font));
			//cellProductItem.BackgroundColor = BaseColor.LIGHT_GRAY;
			//cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
			//productsTable.AddCell(cellProductItem);

			////SKU
			//if (_catalogSettings.ShowSkuOnProductDetailsPage) {
			//	cellProductItem = new PdfPCell(new Phrase(_localizationService.GetResource("PDFInvoice.SKU", lang.Id), font));
			//	cellProductItem.BackgroundColor = BaseColor.LIGHT_GRAY;
			//	cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
			//	productsTable.AddCell(cellProductItem);
			//}

			////price
			//cellProductItem = new PdfPCell(new Phrase(_localizationService.GetResource("PDFInvoice.ProductPrice", lang.Id), font));
			//cellProductItem.BackgroundColor = BaseColor.LIGHT_GRAY;
			//cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
			//productsTable.AddCell(cellProductItem);

			////qty
			//cellProductItem = new PdfPCell(new Phrase(_localizationService.GetResource("PDFInvoice.ProductQuantity", lang.Id), font));
			//cellProductItem.BackgroundColor = BaseColor.LIGHT_GRAY;
			//cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
			//productsTable.AddCell(cellProductItem);

			////total
			//cellProductItem = new PdfPCell(new Phrase(_localizationService.GetResource("PDFInvoice.ProductTotal", lang.Id), font));
			//cellProductItem.BackgroundColor = BaseColor.LIGHT_GRAY;
			//cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
			//productsTable.AddCell(cellProductItem);
			//#endregion Header

			//var p = item.Product;

			////a vendor should have access only to his products
			//if (vendorId > 0 && p.VendorId != vendorId)
			//	continue;

			//var pAttribTable = new PdfPTable(1);
			//pAttribTable.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
			//pAttribTable.DefaultCell.Border = Rectangle.NO_BORDER;

			//#region Body
			////product name
			//string name = p.GetLocalized(x => x.Name, lang.Id);
			//pAttribTable.AddCell(new Paragraph(name, font));
			//cellProductItem.AddElement(new Paragraph(name, font));
			////attributes
			//if (!String.IsNullOrEmpty(item.AttributeDescription)) {
			//	var attributesParagraph = new Paragraph(HtmlHelper.ConvertHtmlToPlainText(item.AttributeDescription, true, true), AttributesFont);
			//	pAttribTable.AddCell(attributesParagraph);
			//}
			////rental info
			//if (item.Product.IsRental) {
			//	var rentalStartDate = item.RentalStartDateUtc.HasValue ? item.Product.FormatRentalDate(item.RentalStartDateUtc.Value) : "";
			//	var rentalEndDate = item.RentalEndDateUtc.HasValue ? item.Product.FormatRentalDate(item.RentalEndDateUtc.Value) : "";
			//	var rentalInfo = string.Format(_localizationService.GetResource("Order.Rental.FormattedDate"),
			//		rentalStartDate, rentalEndDate);

			//	var rentalInfoParagraph = new Paragraph(rentalInfo, AttributesFont);
			//	pAttribTable.AddCell(rentalInfoParagraph);
			//}
			//productsTable.AddCell(pAttribTable);

			////SKU
			//if (_catalogSettings.ShowSkuOnProductDetailsPage) {
			//	var sku = p.FormatSku(item.AttributesXml, _productAttributeParser);
			//	cellProductItem = new PdfPCell(new Phrase(sku ?? String.Empty, font));
			//	cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
			//	productsTable.AddCell(cellProductItem);
			//}

			////price
			//string unitPrice;
			//if (order.CustomerTaxDisplayType == TaxDisplayType.IncludingTax) {
			//	//including tax
			//	var unitPriceInclTaxInCustomerCurrency = _currencyService.ConvertCurrency(item.UnitPriceInclTax, order.CurrencyRate);
			//	unitPrice = _priceFormatter.FormatPrice(unitPriceInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, lang, true);
			//}
			//else {
			//	//excluding tax
			//	var unitPriceExclTaxInCustomerCurrency = _currencyService.ConvertCurrency(item.UnitPriceExclTax, order.CurrencyRate);
			//	unitPrice = _priceFormatter.FormatPrice(unitPriceExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, lang, false);
			//}
			//cellProductItem = new PdfPCell(new Phrase(unitPrice, font));
			//cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
			//productsTable.AddCell(cellProductItem);

			////qty
			//cellProductItem = new PdfPCell(new Phrase(item.Quantity.ToString(), font));
			//cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
			//productsTable.AddCell(cellProductItem);

			////total
			//string subTotal;
			//if (order.CustomerTaxDisplayType == TaxDisplayType.IncludingTax) {
			//	//including tax
			//	var priceInclTaxInCustomerCurrency = _currencyService.ConvertCurrency(item.PriceInclTax, order.CurrencyRate);
			//	subTotal = _priceFormatter.FormatPrice(priceInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, lang, true);
			//}
			//else {
			//	//excluding tax
			//	var priceExclTaxInCustomerCurrency = _currencyService.ConvertCurrency(item.PriceExclTax, order.CurrencyRate);
			//	subTotal = _priceFormatter.FormatPrice(priceExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, lang, false);
			//}
			//cellProductItem = new PdfPCell(new Phrase(subTotal, font));
			//cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
			//productsTable.AddCell(cellProductItem);
			//#endregion Body

			//doc.Add(productsTable);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Prints items to PDF.
		/// </summary>
		/// <param name="stream">Stream</param>
		/// <param name="items">Items</param>
		public void PrintItemsToPdf<T>(Stream stream, IList<T> items)
		{
			#region Input Check
			var log = "";

			if (stream == null)
				log = String.Format("{0}<stream> is required.{1}", log, Environment.NewLine);
			if (items == null)
				log = String.Format("{0}<items> is required.{1}", log, Environment.NewLine);
			else if (items.Count < 1)
				throw new Exception("<items> is empty, so there's nothing to print.");

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}Exception thrown in PdfUtil.PrintItemsToPdf<T>(Stream stream, IList<T> items).{1}{1}", log, Environment.NewLine));
			#endregion Input Check

			var pageSize = _pdfSettings.LetterPageSizeEnabled ? PageSize.LETTER : PageSize.A4;
			var doc = new Document(pageSize);
			var pdfWriter = PdfWriter.GetInstance(doc, stream);
			doc.Open();

			// Date //
			var dateTable = new PdfPTable(1);
			var pdfCell = new PdfPCell(new Phrase(String.Format("Date: {0:dddd, MMMM d, yyyy h:mm tt}", DateTime.Now), MainFont));
			pdfCell.Border = Rectangle.NO_BORDER;
			dateTable.AddCell(pdfCell);
			doc.Add(dateTable);

			// Items //
			var type = items.ElementAt(0).GetType();

			if (type.Equals(typeof(PdfSettings)))
				Console.Write("\nThis is a PdfSettings!");
			else if (type.Equals(typeof(AppDomain)))
				Console.Write("\nThis is a AppDomain!");
			else if (type.Equals(typeof(string)))
				Console.Write("\nThis is a string!");
			else
				Console.Write("\nUnsupported type");

			//PrintItems(doc, items);

			doc.Close();
		}
		#endregion
	}
}