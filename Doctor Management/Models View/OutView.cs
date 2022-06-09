using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doctor_Management.Models_View
{
    public class OutView
    {
        public List<SelectNameCustomer> Customers { get; private set; }

        public List<SelectListItem> Prices { get; private set; }


        public OutView(List<Customer> customers , List<Price> prices)
        {
            Add(customers);
            Add(prices);
        }

        public void Add(List<Customer> customers)
        {
            this.Customers = new List<SelectNameCustomer>();
            foreach (var item in customers)
            {
                this.Customers.Add(item);
            }
        }
        public void Add(List<Price> prices)
        {
            this.Prices = new List<SelectListItem>();
            foreach (var item in prices)
            {
                this.Prices.Add(new SelectListItem { Value = item.Id.ToString() , Text = item.genderprice + " " + item.ThePrice + " جم" });
            }
        }


    }
}
