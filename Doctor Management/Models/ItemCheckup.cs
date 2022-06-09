using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models
{
    [Table("itemcheckup")]
    public class ItemCheckup
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("reveal")]
        public int Idreveal { get; set; }

        public Reveal reveal { get; set; }

        [ForeignKey("customer")]
        public int Idcustomer { get; set; }

        public Customer customer { get; set; }

        [StringLength(40)]
        public string NameCheck { get; set; }

        public double Resulte { get; set; }

        [StringLength(100000000)]
        public byte[] ImageReport { get; set; }
    }
}
