using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models
{
    [Table("medicname")]
    public class MedicName
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string NameMedic { get; set; }
    }
}
