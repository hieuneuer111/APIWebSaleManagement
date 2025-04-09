namespace WebAPISalesManagement.ModelResponses
{
    public class ProductResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } 
        public long ProductPrice { get; set; }
        public string ProductDes { get; set; }
        public string ProductImgUrl { get; set; }
        public bool ProductStatus { get; set; }
        public CategoryResponseDetail ProductCategory { get; set; }
        public ProductResponse(Guid productId, string productName, long productPrice, string productDes, bool productStatus, CategoryResponseDetail productCategory, string productImgUrl)
        {
            ProductId = productId;
            ProductName = productName;
            ProductPrice = productPrice;
            ProductDes = productDes;
            ProductStatus = productStatus;
            this.ProductCategory = productCategory;
            ProductImgUrl = productImgUrl;
        }
        public ProductResponse() { }
    }
}
