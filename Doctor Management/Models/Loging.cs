using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models
{
    [Table("loging")]
    public class Loging
    {
        [Key]
        public int Id { get; set; }
        
        [StringLength(10)]
        public string UserName { get; set; }

        [StringLength(8)]
        public string Password { get; set; }

        public bool Admin { get; set; }
    }
}
