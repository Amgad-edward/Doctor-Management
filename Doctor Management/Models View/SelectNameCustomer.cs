using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;

namespace Doctor_Management.Models_View
{
    public class SelectNameCustomer
    {
        public int ID { get; set; }

        public string NameCustomer { get; set; }


        public static implicit operator SelectNameCustomer(Customer customer)
        {
            var List = new SelectNameCustomer();
            List.ID = customer.ID;
            List.NameCustomer = customer.NameCustomer;
            return List;
        }

  


    }
}
