
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IslamSandhesham.Models
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime? LastLoginTime { get; set; }
    }
}