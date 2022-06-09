using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Doctor_Management.Models_View
{
    public class PlaneView
    {
        public int Id { get; set; }

        [Display(Name = "Date"),Required]
        public DateTime DateDay { get; set; }

        public string Day { get; set; }

        [Display(Name = "Reveal Count")]
        public int Count { get; set; }

        [Display(Name = "All Reveal")]
        public bool All { get; set; }
        [Display(Name = "Is Leave Day")]
        public bool Leave { get; set; }
    }
}
