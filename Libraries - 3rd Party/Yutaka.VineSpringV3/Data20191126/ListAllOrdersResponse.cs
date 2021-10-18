namespace Yutaka.VineSpringV3.Data20191126
{
	public class ListAllOrdersResponse
	{
		public bool HasMore { get; set; }
		public string PaginationKey { get; set; }
		public Order[] Orders { get; set; }
	}
}