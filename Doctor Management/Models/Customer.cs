using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Doctor_Management.Models
{
    [Table("customer")]
    public class Customer
    {
        [Key]
        public int ID { get; set; }

        [StringLength(75) , Required]
        public string NameCustomer { get; set; }

        [StringLength(60)]
        public string Phones { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        [StringLength(10)]
        public string Blood { get; set; }

        public DateTime dateBirth { get; set; }
        public IEnumerable<Reveal> Reveals { get; set; }

        public IEnumerable<ItemCheckup> ItemCheckups { get; set; }

        public IEnumerable<Informations> Informations { get; set; }

        public IEnumerable<Surgery> Surgeries { get; set; }
    }


}
