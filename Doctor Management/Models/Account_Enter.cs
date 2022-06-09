using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models
{
    [Table("account_enter")]
    public class Account_Enter
    {
        [Key]
        public int Id { get; set; }

        [StringLength(40)]
        public string From { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

    }
}
