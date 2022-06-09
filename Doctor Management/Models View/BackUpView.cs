using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Doctor_Management.Models_View
{
    public class BackUpView
    {
        
        public string Json { get; set; }

        public byte[] Filesql { get; set; }
    }
}
