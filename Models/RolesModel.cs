using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebAPISalesManagement.Models
{
    [Table("roles")]
    public class RolesModel:BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid RoleId { get; set; }
        [Column("name")]
        public string RoleName { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}
