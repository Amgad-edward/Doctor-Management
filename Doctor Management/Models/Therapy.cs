using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models
{
    [Table("therapy")]
    public class Therapy
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("reveal")]
        public int Idreveal { get; set; }
        public Reveal reveal { get; set; }

        [ForeignKey("medicName")]
        public int Idmedic { get; set; }

        [StringLength(30)]
        public string Sub { get; set; }
        public MedicName medicName { get; set; }
    }
}
