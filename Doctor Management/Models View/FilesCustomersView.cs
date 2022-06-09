using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models_View
{
    public class FilesCustomersView
    {
        [Required]
        public byte[] Filesup { get; set; }
    }
}
