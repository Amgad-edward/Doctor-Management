using Doctor_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models_View
{
    public class EditRevealView
    {
        public Reveal Reveal { get; set; }

        public IEnumerable<MedicName> medicNames { get; set; }

        public IEnumerable<NamesCheck> namesChecks { get; set; }


    }
}
