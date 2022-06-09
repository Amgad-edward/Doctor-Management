using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;

namespace Doctor_Management.Models_View
{
    public class RevealsSearchView
    {
        public List<Reveal> Reveals { get; set; }

        public List<MedicName> MedicNames { get; set; }

        public List<string> Names { get; set; }

        public int Count { get; set; }
    }
}
