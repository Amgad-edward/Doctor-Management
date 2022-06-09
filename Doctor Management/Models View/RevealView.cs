using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doctor_Management.Models_View
{
    public class RevealView
    {
        public List<Reveal> GetReveals { get; set; }

        public RevealGetView Reveal { get; set; }

        public List<SelectListItem> Customets { get; set; }

        public List<SelectListItem> Prices { get; set; }
    }
}
