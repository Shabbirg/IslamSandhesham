using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IslamSandhesham.Models
{
    public class Content
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [ForeignKey("Button")]
        public int Button_Id { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public string Info { get; set; }
        public string ArabicText { get; set; }
        public virtual Buttons Button { get; set; }
        [NotMapped]
        public int Type { get; set; }
        public int? SectionId { get; set; }
        public int? ImageId { get; set; }
    }
}