using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models_View
{
    public class EmployeeView
    {
        public int Id { get; set; }

        [MaxLength(50),Required,Display(Name = "Employee Name")]
        public string Name { get; set; }

        [Required,Display(Name = "Date Start Work")]
        public DateTime datestart { get; set; }

        [MaxLength(30) ,Required ,Display(Name = "Titel Jop")]
        public string TitleJop { get; set; }

        [Required, Display(Name = "Salary")]
        public decimal Salary { get; set; }

        public bool ISCreate { get; set; }
    }
}
