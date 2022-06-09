using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models
{
    [Table("reveal")]
    public class Reveal
    {
        [Key]
        public int ID { get; set; }

        public DateTime DateReservation { get; set; }

        public int Number { get; set; }

        [ForeignKey("customer")]
        public int Idcustomer { get; set; }
        public Customer customer { get; set; }

        [ForeignKey("price")]
        public int Idprice { get; set; }

        public Price price { get; set; }

        public bool Done { get; set; }

        [StringLength(80),Display(Name = "Diagnosis")]
        public string Diagnosis { get; set; }

        public IEnumerable<Therapy> Therapies { get; set; }

        public IEnumerable<ItemCheckup> ItemCheckups { get; set; }

        public bool IsRe_Reveal { get; set; }

        public DateTime? date_Re_Reveal { get; set; }

    }
}
