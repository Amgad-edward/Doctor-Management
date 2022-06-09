using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;
using System.ComponentModel.DataAnnotations;

namespace Doctor_Management.Models_View
{
    public class CheckUPView
    {

        public List<ItemCheckup> ItemCheckups { get; set; }

        public List<ResultNormal> ResultNormal { get; set; }
        public DateTime date { get; set; }
        public string customerName { get; set; }
    }
}
