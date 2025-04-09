using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Reactive;
using System.Xml.Linq;
using System;

namespace WebAPISalesManagement.Models
{
    [Table("productcategories")]
    public class CategoriesModel: BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Category_Id { get; set; }
        [Column("name")]
        public string Category_Name { get; set; }
        [Column("description")]
        public string Category_Description { get; set; }
        [Column("created_at")]
        public DateTime created_at { get; set; }
        [Column("updated_at")]
        public DateTime updated_at { get; set; }
    }
}
