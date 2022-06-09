using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models
{
    [Table("price")]
    public class Price
    {
        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        public string genderprice { get; set; }

        public decimal ThePrice { get; set; }

        public int Time { get; set; }
    }
}
