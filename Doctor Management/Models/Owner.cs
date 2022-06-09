using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Doctor_Management.Models
{
    [Table("owner")]
    public class Owner
    {
        [Key]
        public int Id { get; set; }

        [StringLength(70),Required]
        public string NameOwner { get; set; }

        [StringLength(200),Required]
        public string Titel { get; set; }

        [StringLength(150), Required]
        public string Addres { get; set; }

        [StringLength(100)]
        public string Phones { get; set; }

        [StringLength(10000000)]
        public byte[] Logo { get; set; }
    }
}
