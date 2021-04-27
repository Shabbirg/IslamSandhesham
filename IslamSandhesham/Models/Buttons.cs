using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IslamSandhesham.Models
{
    public class Buttons
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Section")]
        public int? SectionId { get; set; }
        public virtual Sections Section { get; set; }
        public string Description { get; set; }
        public int? ImageId { get; set; }
        [NotMapped]
        public int Type { get; set; }
    }
}