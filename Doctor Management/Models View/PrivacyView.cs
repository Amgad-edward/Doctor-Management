using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Doctor_Management.Models_View
{
    public class PrivacyView
    {
        public Reveal Reveal { get; set; }

        public List<Informations> Info { get; set; }

        public BlackList BlackList { get; set; }

        public List<Surgery> Surgeries { get; set; }

        public Reveal lastrevale { get; set; }

        public int? CountdayesRe { get; set; }

        [Display(Name = "Re-reveal")]
        public bool Re_reveal { get; set; }

        public List<SelectListItem> Price { get; set; }

        public int? idprice { get; set; }

        public List<MedicName> MedicNames { get; set; }

        public List<NamesCheck> Names { get; set; }

        public List<string> ResultNamesCheck { get; set; }

        public List<string> Dia { get; set; }

    }
}
