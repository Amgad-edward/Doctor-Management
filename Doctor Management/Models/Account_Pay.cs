using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models
{
    [Table("account_pay")]
    public class Account_Pay
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string ToPay { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

    }
}
