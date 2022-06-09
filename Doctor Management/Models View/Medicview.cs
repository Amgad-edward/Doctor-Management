using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models_View
{
    public class Medicview
    {
        public int Id { get; set; }

        [MaxLength(50),Required , Display(Name = "Medic Name")]
        public string NameMedic { get; set; }
        public bool IsCreate { get; set; }
    }
}
