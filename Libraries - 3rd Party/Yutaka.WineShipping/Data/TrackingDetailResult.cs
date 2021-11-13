using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.WineShipping.Data
{ 
	/// <summary>
	/// Getting the Tracking details returns a list of results
	/// </summary>
	public class TrackingDetailResult
	{
		public TrackingDetailResult()
		{
			PackageItems = new List<PackageItem>();
		}

		public string OrderNumber { get; set; }

		public string Carrier { get; set; }

		public string Service { get; set; }

		public string TrackingNo { get; set; }

		public string TrackingUrl { get; set; }

		public string RequestedShipDate { get; set; }

		public string ShipDate { get; set; }

		public string Status { get; set; }

		public string StatusDescription { get; set; }

		public string Type { get; set; }

		public string Site { get; set; }

		public string Warehouse { get; set; }

		public string IcePack { get; set; }

		public string CarrierStatus { get; set; }

		public string CarrierStatusMessage { get; set; }

		public string CarrierStatusTimestamp { get; set; }

		public string PackageLocation { get; set; }

		public string EstimatedDeliveryDate { get; set; }

		public string GrossWeight { get; set; }

		public string ShipToName { get; set; }

		public string ShipToContact { get; set; }

		public string ShipToAddress { get; set; }

		public string ShipToAddress2 { get; set; }

		public string ShipToCity { get; set; }

		public string ShipToState { get; set; }

		public string ShipToZipCode { get; set; }

		public string ShipToCountry { get; set; }

		public List<PackageItem> PackageItems { get; set; }	
	}


	public class PackageItem
	{
		public string ItemNo { get; set; }

		public string Quantity { get; set; }

		public string ReservedQuantity { get; set; }

		public string ItemDescription { get; set; }
	}
}
