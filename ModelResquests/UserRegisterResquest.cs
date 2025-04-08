namespace WebAPISalesManagement.ModelRequests
{
    public class UserRegisterResquest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public Guid RoleId { get; set; }    
        public string Usernumber { get; set; }
        public bool Status { get; set; }

    }
}
