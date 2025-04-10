﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Data;
using System.Numerics;
using System.Reactive;

namespace WebAPISalesManagement.Models
{
    [Table("users")]
    public class UsersModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid User_id { get; set; }
        [Column("username")]
        public string User_name { get; set; }
        [Column("email")]
        public string User_email { get; set; }
        [Column("create_date")]
        public DateTime Date_create { get; set; }
        [Column("updated_at")]
        public DateTime Date_update { get; set; }
        [Column("role_id")]
        public Guid Role_id { get; set; }
        [Column("status")]
        public bool User_Status { get; set; }
        [Column("phone")]
        public string? User_Phone { get; set; }
        [Column("full_name")]
        public string? User_FullName { get; set; }
        [Column("user_number")]
        public string User_Number { get; set; }
        [Column("vetify_email")]
        public bool? Verify_email { get; set; }
    }
}
