using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;

namespace Doctor_Management.Models_View
{
    public class CustomerView
    {
        public int ID { get; set; }

        [MaxLength(75), Required ,Display(Name = "الاســـم")]
        public string NameCustomer { get; set; }

        [MaxLength(60) , Display(Name = "الهاتف")]
        public string Phones { get; set; }

        [MaxLength(10),Required, Display(Name = "النـــوع")]
        public string Gender { get; set; }

        [MaxLength(10) , Display(Name = "فصيلة الدم")]
        public string Blood { get; set; }

        [Display(Name = "تاريخ الميلاد"),Required]
        public DateTime dateBirth { get; set; }

        public bool create { get; set; }

        public IEnumerable<Reveal> Reveals { get; set; }

        public IEnumerable<ItemCheckup> ItemCheckups { get; set; }

        public IEnumerable<Informations> Informations { get; set; }

        public List<Price> Prices { get; set; }

        public List<MedicName> MedicNames { get; set; }

        public List<Therapy> therapies { get; set; }
    }
}
