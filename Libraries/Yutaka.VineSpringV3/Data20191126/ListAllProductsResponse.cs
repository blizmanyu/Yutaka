namespace Yutaka.VineSpringV3.Data20191126
{
	public class ListAllProductsResponse
	{
		public bool canView { get; set; }
		public string accountId { get; set; }
		public Sku[] skus { get; set; }
		public string teaser { get; set; }
		public string listImage { get; set; }
		public int displayOrder { get; set; }
		public string attributes { get; set; }
		public string description { get; set; }
		public string id { get; set; }
		public string name { get; set; }
		public object[] tags { get; set; }
		public string detailImage { get; set; }
	}

	public class Sku
	{
		public string shipCompliantBrandKey { get; set; }
		public string shippingPlanId { get; set; }
		public Visibility visibility { get; set; }
		public bool isTaxExempt { get; set; }
		public decimal price { get; set; }
		public string name { get; set; }
		public Attributes attributes { get; set; }
		public string shipCompliantProductKey { get; set; }
		public string sku { get; set; }
		public Constraints constraints { get; set; }
		public decimal retailPrice { get; set; }
		public Dimensions dimensions { get; set; }
		public Customfields customFields { get; set; }
	}

	public class Visibility
	{
		public bool customerView { get; set; }
		public string allocationMessage { get; set; }
		public bool customerPurchase { get; set; }
		public bool adminView { get; set; }
		public bool isAllocated { get; set; }
	}

	public class Attributes
	{
		public string Attribute6 { get; set; }
		public string Attribute5 { get; set; }
		public string Attribute4 { get; set; }
		public object Attribute3 { get; set; }
		public string Attribute2 { get; set; }
		public string Attribute1 { get; set; }
		public object Varietal { get; set; }
		public object BottleSize { get; set; }
		public object Vintage { get; set; }
		public string Attribute7 { get; set; }
		public string Attribute8 { get; set; }
		public string Attribute9 { get; set; }
		public string Attribute21 { get; set; }
		public string Attribute20 { get; set; }
		public string Attribute16 { get; set; }
		public string Attribute15 { get; set; }
		public string Attribute18 { get; set; }
		public string Attribute17 { get; set; }
		public string Attribute19 { get; set; }
		public string Attribute10 { get; set; }
		public string Attribute12 { get; set; }
		public string Attribute11 { get; set; }
		public string Attribute14 { get; set; }
		public string Attribute13 { get; set; }
		public string Attribute23 { get; set; }
		public string Attribute22 { get; set; }
		public string Attribute25 { get; set; }
		public string Attribute24 { get; set; }
		public string Attribute30 { get; set; }
		public string Attribute27 { get; set; }
		public string Attribute26 { get; set; }
		public string Attribute29 { get; set; }
		public string Attribute28 { get; set; }
		public string Attributes3 { get; set; }
		public string Attributes2 { get; set; }
		public string Attributes1 { get; set; }
	}

	public class Constraints
	{
		public int increment { get; set; }
		public int max { get; set; }
		public int min { get; set; }
	}

	public class Dimensions
	{
		public decimal weight { get; set; }
	}

	public class Customfields
	{
	}
}