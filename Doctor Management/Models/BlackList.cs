using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Doctor_Management.Models
{
    [Table("blacklist")]
    public class BlackList
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("customer")]
        public int Idcunstomer { get; set; }
        public Customer customer { get; set; }

        [StringLength(100)]
        public string Report { get; set; }
    }
}
