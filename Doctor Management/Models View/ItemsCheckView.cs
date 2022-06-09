using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Doctor_Management.Models_View
{
    public class ItemsCheckView
    {
        public int Id { get; set; }
        public int Idreveal { get; set; }
        public int Idcustomer { get; set; }

        [MaxLength(40),Required,Display(Name = "Name")]
        public string NameCheck { get; set; }

        [Display(Name = "Result")]
        public double Resulte { get; set; }

        public bool create { get; set; }

        [Display(Name = "Image")]
        public byte[] ImageReport { get; set; }
    }
}
