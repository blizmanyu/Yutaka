namespace Yutaka.VineSpring.Data
{
	public class ListAllOrdersResponse
	{
		public bool? HasMore { get; set; }
		public string PaginationKey { get; set; }
		public Order[] Orders { get; set; }
	}
}