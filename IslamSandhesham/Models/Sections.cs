
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace IslamSandhesham.Models
{
    public class Sections
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int SectionId { get; set; }
        public string Description { get; set; }
        public string ClassName { get; set; }
        public int? Type { get; set; }
    }
}