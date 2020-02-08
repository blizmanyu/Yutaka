namespace Yutaka.VineSpring.Data20200207
{
	public class ListAllResponse
	{
		public bool? HasMore { get; set; }
		public string PaginationKey { get; set; }
	}

	public class ListAllCustomersResponse : ListAllResponse
	{
		public Customer[] Customers { get; set; }
	}
}