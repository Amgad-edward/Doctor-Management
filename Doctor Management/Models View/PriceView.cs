using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models_View
{
    public class PriceView
    {
        public int Id { get; set; }

        [MaxLength(30) , Required , Display(Name = "Name")]
        public string genderprice { get; set; }

        [Required , Display(Name = " Price Amount")]
        public decimal ThePrice { get; set; }

        [Display(Name = "Time This Rreveals (Minutes)"),Required]
        public int Time { get; set; }
        public bool IsCreate { get; set; }
    }
}
