using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.WineShipping.Data
{
	public class TrackingDetailRequest
	{
		public AuthenticationDetails AuthenticationDetails { get; set; }

		public string OrderNo { get; set; }

		public string ToJson()
		{
			var json = String.Format("{{ {0}", AuthenticationDetails.ToJson());

			if (!String.IsNullOrWhiteSpace(OrderNo))
				json = String.Format("{0}, \"OrderNo\": \"{1}\"", json, OrderNo);

			return String.Format("{0} }}", json);

		}
	}
}
