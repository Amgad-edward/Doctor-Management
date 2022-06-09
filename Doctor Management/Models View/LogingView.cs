using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models_View
{
    public class LogingView
    {
        public int Id { get; set; }

        [MaxLength(10),Required , Display(Name = "User Name")]
        public string UserName { get; set; }

        [MaxLength(8),Required , Display(Name = "Password")]
        public string Password { get; set; }

        [MaxLength(8),Required,Display(Name = "Re-Password")]
        public string Re_Password { get; set; }

        [Display(Name = "Is Admin (this user Admin)")]
        public bool Admin { get; set; }
    }
}
