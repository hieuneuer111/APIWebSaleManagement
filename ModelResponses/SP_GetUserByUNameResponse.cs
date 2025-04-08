namespace WebAPISalesManagement.ModelResponses
{
    
    public class SP_GetUserByUNameResponse
    {
        public Guid Uid { get; set; }      // Trường UID kiểu INT8 (long)
        public string NameUser { get; set; } // Trường NameUser kiểu varchar
        public string EmailUser { get; set; } // Trường EmailUser kiểu varchar
        public Guid rolesUser { get; set; }
        public bool? Email_verifi {  get; set; }
        public SP_GetUserByUNameResponse(Guid uid, string nameUser, string emailUser, string passUser, Guid userRole, bool? email_verifi)
        {
            Uid = uid;
            NameUser = nameUser;
            EmailUser = emailUser;
            rolesUser = userRole;
            Email_verifi = email_verifi;
        }

        // Hàm mặc định không tham số
        public SP_GetUserByUNameResponse() { }
    }
}
