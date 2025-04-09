using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebAPISalesManagement.Models
{
    [Table("rolepermissions")]
    public class RoleRightsModel:BaseModel
    {
        [PrimaryKey("role_id", false)]
        public Guid Role_Id { get; set; }
        [Column("permission_id")]
        public string Right_Id { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}
