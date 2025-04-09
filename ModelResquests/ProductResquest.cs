using WebAPISalesManagement.ModelResponses;

namespace WebAPISalesManagement.ModelResquests
{
    public class ProductResquest
    {    
        public string ProductName { get; set; }
        public long ProductPrice { get; set; }
        public string ProductDes { get; set; }
        public bool ProductStatus { get; set; }
        public Guid ProductCategory { get; set; }
    }
}
