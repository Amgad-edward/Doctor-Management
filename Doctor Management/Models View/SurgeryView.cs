using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Doctor_Management.Models_View
{
    public class SurgeryView
    {
        public int Id { get; set; }

        [Display(Name = "Customer Name"),Required,MaxLength(50)]
        public string CustomerName { get; set; }

        [MaxLength(50) , Display(Name ="Surgery Name"),Required]
        public string NameSurgery { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Surgery Date"),Required]
        public DateTime DateTime { get; set; }

        [Display(Name = "IS Done")]
        public bool Done { get; set; }

        public bool Create { get; set; }

        public List<string> Names { get; set; }

    }
}
