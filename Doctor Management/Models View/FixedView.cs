using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models_View
{
    public class FixedView
    {
        public int Id { get; set; }

        [MaxLength(30) ,Required , Display(Name = "Name Item")]
        public string itemName { get; set; }

        [Required , Display(Name = "Amount")]
        public decimal FixsedAmmount { get; set; }

        [Display(Name = "Count days")]
        public int Timespan { get; set; }

        public bool IsCreate { get; set; }
    }
}
