using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doctor_Management.Models_View
{
    public class PaysViewInput
    {
        public int Id { get; set; }

        [MaxLength(50),Required , Display(Name = "Accout To")]
        public string ToPay { get; set; }

        [Required , Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required , Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "From Accounts")]
        public int? Idfixed_pay { get; set; }

        public List<SelectListItem> ListFiex { get; set; }
    }
}
