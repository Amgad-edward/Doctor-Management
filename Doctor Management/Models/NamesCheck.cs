using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models
{
    [Table("namescheck")]
    public class NamesCheck
    {
        [Key]
        public int Id { get; set; }

        [Required, Display(Name = "Enter Titel Name")]
        public string TitelName { get; set; }

        [Required,Display(Name = "Names Of checks")]
        public string Nameschiled { get; set; }
    }
}
