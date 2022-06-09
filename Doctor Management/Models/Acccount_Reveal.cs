using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models
{
    [Table("account_reveal")]
    public class Acccount_Reveal
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("reveal")]
        public int IdReveal { get; set; }

        public Reveal reveal { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }
    }
}
