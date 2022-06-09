using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models_View
{
    public class AccountEnterView
    {
        public int Id { get; set; }

        [MaxLength(40),Required , Display(Name = "From")]
        public string From { get; set; }

        [Display(Name = "Amounts"),Required]
        public decimal Amount { get; set; }

        [Display(Name = "Date"),Required]
        public DateTime Date { get; set; }
    }
}
