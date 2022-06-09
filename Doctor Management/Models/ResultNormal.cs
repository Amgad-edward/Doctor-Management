using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models
{
    [Table("resultnormal")]
    public class ResultNormal
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Gender"),Required]
        public string Gender { get; set; }

        [Display(Name = "Name"),Required]
        public string Name { get; set; }

        [Required,Display(Name = "Normal Result")]
        public double Normal { get; set; }


    }
}
