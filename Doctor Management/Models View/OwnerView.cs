using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models_View
{
    public class OwnerView
    {
        public int Id { get; set; }

        [MaxLength(70), Required , Display(Name = "Name Doctor")]
        public string NameOwner { get; set; }

        [MaxLength(200) ,Required ,Display(Name = "Title Jop")]
        public string Titel { get; set; }

        [MaxLength(150), Required , Display(Name = "Address")]
        public string Addres { get; set; }

        [MaxLength(100) , Display(Name = "Phones")]
        public string Phones { get; set; }

        [ Display(Name = "Logo Image")]
        public byte[] Logo { get; set; }

        public bool Create { get; set; }
    }
}
