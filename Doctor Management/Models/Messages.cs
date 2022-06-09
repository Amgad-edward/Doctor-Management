using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models
{
    [Table("messages")]
    public class Messages
    {
        [Key]
        public int Id { get; set; }

        [StringLength(150)]
        public string Message { get; set; }

        [StringLength(30)]
        public string From { get; set; }

        public DateTime date { get; set; }

        public bool ISRead { get; set; }
    }
}
