using Yutaka.VineSpring.Data;

namespace Yutaka.VineSpring.Data20191126
{
	public class ListAllOrdersResponse
	{
		public bool HasMore { get; set; }
		public string PaginationKey { get; set; }
		public Order[] Orders { get; set; }
	}
}