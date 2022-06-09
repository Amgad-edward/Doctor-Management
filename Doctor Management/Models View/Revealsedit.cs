using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models_View
{
    public class Revealsedit
    {
        public int ID { get; set; }

        [Display(Name = "تاريخ الحجز") , Required]
        public DateTime DateReservation { get; set; }

        public string Date { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public int Idcustomer { get; set; }

        [Display(Name = "نوع الكشــف"), Required]
        public int Idprice { get; set; }

        public bool Done { get; set; }

        public string Diagnosis { get; set; }

        public bool IsRe_Reveal { get; set; }

        public DateTime? date_Re_Reveal { get; set; }

        [Display(Name = "كشف مستعجل")]
        public bool run { get; set; }

        [Display(Name = "حجز للاسم"),Required,MaxLength(30)]
        public string NameCustomer { get; set; }

        public string Name { get; set; }

    }
}
