namespace WebAPISalesManagement.ModelResponses
{
    public class CategoryResponseDetail
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

        // Constructor mặc định
        public CategoryResponseDetail() { }

        // Constructor với tất cả các tham số
       

        // Constructor không có CategoryDes
        public CategoryResponseDetail(Guid categoryId, string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
        }

    }
    public class CategoryResponse : CategoryResponseDetail
    {
        public string CategoryDes { get; set; }
        // Constructor với CategoryId, CategoryName và CategoryDes
        public CategoryResponse(Guid categoryId, string categoryName, string categoryDes)
            : base(categoryId, categoryName)
        {
            CategoryDes = categoryDes;
        }
        public CategoryResponse() { }
    }

}
