namespace WebAPISalesManagement.ModelResponses
{
    public class UserResponse
    {
        public Guid User_Id { get; set; }
        public string User_Name { get; set; }
        public string User_Email { get; set; }
        public string? FullName { get; set; }
    }
}
