namespace WebAPISalesManagement.Helpers
{
    public static class StoreProcedureSupabase
    {
        // Tên các store procedure trong Supabase
        public const string GetUserByUsername = "get_user_by_username";
        public const string GetRightByRoleId = "get_right_by_roleid";
        public const string GetRightByUId = "get_right_by_uid";
        public const string GetFilesByFolder = "get_files_by_folder";
        public const string DeleteAllFilesByFolder = "delete_files_by_folder_json";
        // Thêm các store procedure khác nếu cần
    }
}
