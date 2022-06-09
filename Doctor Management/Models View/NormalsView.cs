using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doctor_Management.Models_View
{
    public class NormalsView
    {
        public List<SelectListItem> ListNames { get; set; }

        public List<ResultNormal> Results { get; set; }

        public ResultNormal Resul { get; set; }

        public List<ResultNormal> GetAlls { get; set; }

        public int? ID { get; set; }

        public int? IdNor { get; set; }
    }
}
