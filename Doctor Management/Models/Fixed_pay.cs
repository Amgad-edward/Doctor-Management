using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models
{
    [Table("fixed_pay")]
    public class Fixed_pay
    {
        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        public string itemName { get; set; }

        public decimal FixsedAmmount { get; set; }

        public int Timespan { get; set; }
    }
}
