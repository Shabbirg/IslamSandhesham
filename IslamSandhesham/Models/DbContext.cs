using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IslamSandhesham.Models
{
    public class MyContext : DbContext
    {
        public MyContext() : base("myConnectionString") { }
        public virtual DbSet<Home> Home { get; set; }
        public virtual DbSet<Buttons> Buttons { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Sections> Sections { get; set; }
        public virtual DbSet<Questions_Answers> Questions_Answers { get; set; }
        public virtual DbSet<Images> Images { get; set; }
    }
}