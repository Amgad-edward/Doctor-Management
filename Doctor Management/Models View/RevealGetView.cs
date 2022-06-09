using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models_View
{
    public class RevealGetView
    {
        public int ID { get; set; }

        [Display(Name = "تاريخ الحجز")]
        public DateTime DateReservation { get; set; }

        [Display(Name = "رقد الدور")]
        public int Number { get; set; }

        [Display(Name = "حجز باســـم")]
        public int Idcustomer { get; set; }

        [Display(Name = "النوع")]
        public int Idprice { get; set; }

        public bool Done { get; set; }



    }
}
