using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Doctor_Management.Models;
using Doctor_Management.Models_View;

namespace Doctor_Management.Controllers
{
    public class BackController : Controller
    {
        private readonly DataContext db;

        public BackController(DataContext db)
        {
            this.db = db;
        }

        public JsonResult GetFixed()
        {
            var list = new List<NToastr>();
            var Fix = db.Fixed_Pays.ToList();
            list.Clear();
            foreach (var f in Fix)
            {
                var Pay = db.Account_Pays.Where(x => x.ToPay.ToLower() == f.itemName.ToLower())
                    .OrderByDescending(d => d.Date).FirstOrDefault();
                if(Pay is not null)
                {
                    var date = Pay.Date.AddDays(f.Timespan);
                    if(date.Date == DateTime.Now.Date || date == DateTime.Now.AddDays(2).Date)
                    {
                       list.Add(new NToastr { Name = f.itemName, Value = f.FixsedAmmount, Url = $"/Pays/Addingout/{f.Id}" });
                    }
                }
                else
                {
                    if (f.itemName.ToLower().Contains("salary"))
                    {
                        var Name = f.itemName.ToLower().Replace("salary ","");
                        var Start = db.Employees.FirstOrDefault(x => x.Name.ToLower() == Name);
                        if(Start is not null)
                        {
                           
                            var date = Start.datestart.AddDays(f.Timespan);
                            if(date.Date == DateTime.Now.Date || date.Date == DateTime.Now.AddDays(2).Date)
                            {
                                list.Add(new NToastr { Name = f.itemName, Value = f.FixsedAmmount, Url = $"/Pays/Addingout/{f.Id}" });
                            }
                        }
                    }
                }

            }

            var Surgers = db.Surgeries.Where(x => !x.Done && x.DateTime.Date == DateTime.Now.Date)
                .Include(c => c.customer).ToList();
            foreach (var item in Surgers)
            {
                list.Add(new NToastr { Name = $"Time Surgery:{item.DateTime.ToDate()} {item.NameSurgery} {item.customer.NameCustomer}", Url = $"/Surgery/Detail/{item.Id}" });
            }
            if (list.Count > 0)
            {
                return Json(list);
            }
            return Json("");
        }

        [HttpPost]
        public IActionResult Searched(string text)
        {
            Search search = new Search(text , db);
            string s = search;
            return Ok(search.ToString());
        }
        

        class NToastr
        {
            public string Name { get; set; }

            public decimal Value { get; set; }
           
            public string Url { get; set; }
        }

      
    }
}
