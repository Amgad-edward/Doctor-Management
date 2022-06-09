using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doctor_Management.Models_View
{

    public class AccountMoveView
    {
        public List<Acccount_Reveal> Reveals { get; set; }

        public List<Account_Enter> Enter { get; set; }

        public List<Account_Pay> Pays { get; set; }

        public List<Surgery> Surgeries { get; set; }

        public List<SelectListItem> listNames { get; set; }

    }
}
