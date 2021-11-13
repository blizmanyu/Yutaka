using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.WineShipping.Data
{

	public class OrderOnHoldItem
	{
		public string ItemNo { get; set; }

		public string Quantity { get; set; }

		public string ItemDescription { get; set; }

		public string QuantityReserved { get; set; }
	}

	public class OrderOnHoldResult
	{
		public string CustomerNo { get; set; }

		public string CustomerName { get; set; }

		public string OrderNo { get; set; }

		public string  OrderType { get; set; }

		public string ShipmentDate { get; set; }

		public string ShippingSite { get; set; }

		public string HoldReason { get; set; }

		public string ShippingCarrier { get; set; }

		public string ShippingCarrierService { get; set; }

		public List<OrderOnHoldItem> OrderOnHoldItems{ get; set; }

		public OrderOnHoldResult()
		{
			OrderOnHoldItems = new List<OrderOnHoldItem>();
		}
	}
}
