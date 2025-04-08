namespace WebAPISalesManagement.Swagger
{
    public class ModelResponse
    {
        public ModelResponse()
        {
            IsValid = true;
            ValidationMessages = new List<string>();
        }
        public bool IsValid { get; set; }
        public List<string> ValidationMessages { get; set; }
    }
}
