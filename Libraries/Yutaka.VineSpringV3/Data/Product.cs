using System.Collections.Generic;

namespace Yutaka.VineSpringV3
{
	public class Product
	{
		public bool? canView { get; set; }
		public string accountId { get; set; }
		public List<Sku> skus { get; set; }
		public string teaser { get; set; }
		public string listImage { get; set; }
		public int? displayOrder { get; set; }
		public string attributes { get; set; }
		public string description { get; set; }
		public string id { get; set; }
		public string name { get; set; }
		public string[] tags { get; set; }
		public string detailImage { get; set; }
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
		public string Attribute3 { get; set; }
		public string Attribute2 { get; set; }
		public string Attribute1 { get; set; }
		public string Varietal { get; set; }
		public string BottleSize { get; set; }
		public string Vintage { get; set; }
		public string Attribute7 { get; set; }
	}

	public class Constraints
	{
		public int increment { get; set; }
		public int max { get; set; }
		public int min { get; set; }
	}

	public class Dimensions
	{
		public decimal? weight { get; set; }
	}

	public class Sku
	{
		public string shippingPlanId { get; set; }
		public Visibility visibility { get; set; }
		public bool? isTaxExempt { get; set; }
		public decimal? price { get; set; }
		public string name { get; set; }
		public Attributes attributes { get; set; }
		public string sku { get; set; }
		public Constraints constraints { get; set; }
		public decimal? retailPrice { get; set; }
		public Dimensions dimensions { get; set; }
		public string shipCompliantBrandKey { get; set; }
		public string shipCompliantProductKey { get; set; }
	}
}