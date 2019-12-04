namespace Yutaka.VineSpring.Data20191126
{
	public class Product
	{
		public string AccountId { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }
		public bool? CanView { get; set; }
		public int? DisplayOrder { get; set; }
		public string[] Tags { get; set; }
		public string ListImage { get; set; }
		public string DetailImage { get; set; }
		public string Attributes { get; set; }
		public string Teaser { get; set; }
		public string Description { get; set; }
		public Sku[] Skus { get; set; }
	}

	public class Sku
	{
		public string SkuStr { get; set; }
		public string ShipCompliantProductKey { get; set; }
		public string ShipCompliantBrandKey { get; set; }
		public string Name { get; set; }
		public decimal? Price { get; set; }
		public decimal? RetailPrice { get; set; }
		public string ShippingPlanId { get; set; }
		public bool? IsTaxExempt { get; set; }
		public Attributes Attributes { get; set; }
		public Constraint Constraints { get; set; }
		public CustomFields CustomFields { get; set; }
		public Dimensions Dimensions { get; set; }
		public Visibility Visibility { get; set; }
	}

	public class Visibility
	{
		public bool? AdminView { get; set; }
		public bool? CustomerView { get; set; }
		public bool? CustomerPurchase { get; set; }
		public bool? IsAllocated { get; set; }
		public string AllocationMessage { get; set; }
	}

	public class Attributes
	{
		public string BottleSize { get; set; }
		public string Varietal { get; set; }
		public string Vintage { get; set; }
		public string Attribute1 { get; set; }
		public string Attribute2 { get; set; }
		public string Attribute3 { get; set; }
		public string Attribute4 { get; set; }
		public string Attribute5 { get; set; }
		public string Attribute6 { get; set; }
		public string Attribute7 { get; set; }
		public string Attribute8 { get; set; }
		public string Attribute9 { get; set; }
		public string Attribute10 { get; set; }
		public string Attribute11 { get; set; }
		public string Attribute12 { get; set; }
		public string Attribute13 { get; set; }
		public string Attribute14 { get; set; }
		public string Attribute15 { get; set; }
		public string Attribute16 { get; set; }
		public string Attribute17 { get; set; }
		public string Attribute18 { get; set; }
		public string Attribute19 { get; set; }
		public string Attribute20 { get; set; }
		public string Attribute21 { get; set; }
		public string Attribute22 { get; set; }
		public string Attribute23 { get; set; }
		public string Attribute24 { get; set; }
		public string Attribute25 { get; set; }
		public string Attribute26 { get; set; }
		public string Attribute27 { get; set; }
		public string Attribute28 { get; set; }
		public string Attribute29 { get; set; }
		public string Attribute30 { get; set; }
		public string Attributes1 { get; set; }
		public string Attributes2 { get; set; }
		public string Attributes3 { get; set; }
	}

	public class Constraint
	{
		public int? Increment { get; set; }
		public int? Max { get; set; }
		public int? Min { get; set; }
	}

	public class Dimensions
	{
		public decimal? Depth { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Weight { get; set; }
	}

	public class CustomFields
	{
		public string Custom1 { get; set; }
		public string Custom2 { get; set; }
		public string Custom3 { get; set; }
		public string Custom4 { get; set; }
		public string Custom5 { get; set; }
		public string Custom6 { get; set; }
		public string Custom7 { get; set; }
		public string Custom8 { get; set; }
		public string Custom9 { get; set; }
		public string Custom10 { get; set; }
	}
}