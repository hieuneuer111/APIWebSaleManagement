namespace WebAPISalesManagement.Swagger
{
    public class ModelDataResponse<T>
    {
        public ModelDataResponse()
        {
            IsValid = true;
            ValidationMessages = new List<string>();
        }
        public bool IsValid { get; set; }
        public List<string> ValidationMessages { get; set; }
        public T ItemResponse { get; set; }
    }
}
