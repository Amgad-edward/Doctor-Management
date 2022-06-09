using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models
{
    [Table("informations")]
    public class Informations
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("customer")]
        public int IdCustomer { get; set; }

        public Customer customer { get; set; }

        [StringLength(200)]
        public string Info { get; set; }
    }
}
