using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace WebAPISalesManagement.Models
{
    [Table("toppings")]
    public class ToppingModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("price")]
        public int Price { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
