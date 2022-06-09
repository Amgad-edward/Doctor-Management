using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Doctor_Management.Models
{
    [Table("planereveals")]
    public class PlaneReveals
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateDay { get; set; }

        public int Count { get; set; }

        public bool All { get; set; }

        public bool Leave { get; set; }
    }
}
