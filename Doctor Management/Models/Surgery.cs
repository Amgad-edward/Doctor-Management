using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Doctor_Management.Models
{
    [Table("surgery")]
    public class Surgery
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("customer")]
        public int IdCustomer { get; set; }

        public Customer customer { get; set; }

        [StringLength(50)]
        public string NameSurgery { get; set; }

        public decimal Price { get; set; }

        public DateTime DateTime { get; set; }

        public bool Done { get; set; }
    }
}
