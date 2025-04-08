namespace WebAPISalesManagement.Swagger
{
    public class ModelDataPageResponse<T>
    {
        public int totalItem { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public int maxPage { get; set; }
        public T items { get; set; }
    }
}
