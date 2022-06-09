using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;
using Doctor_Management.Models_View;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doctor_Management.Controllers
{
    public class ChartController : Controller
    {
        private readonly DataContext db;

        public ChartController(DataContext db)
        {
            this.db = db;
        }


        private Users GetUser()
        {
            Users users = new Users(HttpContext);
            ViewBag.user = users.User;
            ViewBag.login = users.Login;
            ViewBag.admin = users.Admin;
            return users;
        }

        public IActionResult Index()
        {
            GetUser();
            ViewBag.list = getlist();
            return View();
        }

        private List<SelectListItem> getlist()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "حساب الكشوف حسب شهور السنة" });
            list.Add(new SelectListItem { Value = "2", Text = "اكثر الادوية استخداما" });
            list.Add(new SelectListItem { Value = "3", Text = "اعداد حسب فصيلة الدم" });
            list.Add(new SelectListItem { Value = "4", Text = "رسم بيانى حسب نوع الكشف" });
            list.Add(new SelectListItem { Value = "5", Text = "عدد الذكور والاناث" });
            list.Add(new SelectListItem { Value = "7", Text = "العدد حسب الفئة العمرية" });
            list.Add(new SelectListItem { Value = "8", Text = "اكثر التشخيصات"});
            return list;
        }

        private IEnumerable<ChartsView> GetMonthsAccountReveal()
        {
            var Months = db.Reveals.AsQueryable().ToList().Where(x => x.DateReservation.Year == DateTime.Now.Year).Select(x => x.DateReservation.Month)
                .Distinct().ToList();
            var Reveal = db.Reveals.Include(p => p.price).ToList();
            foreach (var item in Months)
            {
                yield return new ChartsView { Name = Enum.GetName(typeof(MonthName),item) , value = Reveal.AsQueryable().Where(x=>x.DateReservation.Year == DateTime.Now.Year && x.DateReservation.Month == item).Sum(x=>x.price.ThePrice)};
            }
        }
        private IEnumerable<ChartsView> GetTherapies()
        {
            var Thera = db.Therapies.Include(m=>m.medicName).ToList();
            var Distinct = Thera.Select(s => s.Idmedic).Distinct();
            var list = new List<(int id , int count)>();
            foreach (var item in Distinct)
            {
                var Count = Thera.Where(x =>x.Idmedic == item).Count();
                list.Add((id: item, count: Count));
            }
            var lon = list.Where(x => x.count >= (Thera.Count / x.count));
            foreach (var item in lon)
            {
                yield return new ChartsView { Name = Thera.FirstOrDefault(x => x.Idmedic == item.id).medicName.NameMedic, value = item.count };

            }
        }

        private IEnumerable<ChartsView> BloodCount()
        {
            var Blood = db.Customers.Select(s => s.Blood).ToList();
            var Distinct = Blood.Distinct();
            foreach (var item in Distinct)
            {
                yield return new ChartsView { Name = item, value = Blood.Where(x => x == item).Count() };
            }
        }

        private IEnumerable<ChartsView> Genderreveal()
        {
            var Prices = db.Prices.ToList();
            foreach (var item in Prices)
            {
                yield return new ChartsView { Name = item.genderprice, value = db.Reveals.Count(x => x.Idprice == item.Id) };
            }
        }

        private IEnumerable<ChartsView> getCountMaleFemal()
        {
            var Customers = db.Customers.Select(s => s.Gender).ToList();
            yield return new ChartsView { Name = "Male", value = Customers.Where(x => x.ToUpper() == "M").Count() };
            yield return new ChartsView { Name = "Female", value = Customers.Where(x => x.ToUpper() == "F").Count() };
        }

        private IEnumerable<ChartsView> getOlds()
        {
            var Customets = db.Customers.ToInfo();
            yield return new ChartsView
            {
                Name = "Kides", 
                value = Customets.Where(x=>x.Age > 0 && x.Age <=12).Count()
            };
            yield return new ChartsView
            {
                Name = "Teen",
                value = Customets.Where(x => x.Age > 12 && x.Age <= 18).Count()
            };
            yield return new ChartsView
            {
                Name = "Puberty",
                value = Customets.Where(x => x.Age > 18 && x.Age <= 21).Count()
            };
            yield return new ChartsView
            {
                Name = "Youths",
                value = Customets.Where(x => x.Age > 21 && x.Age <= 50).Count()
            };
            yield return new ChartsView
            {
                Name = "Old",
                value = Customets.Where(x => x.Age > 50 && x.Age <= 80).Count()
            };
            yield return new ChartsView
            {
                Name = "Elderly",
                value = Customets.Where(x => x.Age > 80).Count()
            };
        }

        private IEnumerable<ChartsView> GetDig()
        {
            var reveals = db.Reveals.Select(ss => ss.Diagnosis).ToList();
            var Distinct = reveals.Distinct();
            var list = new List<(string name, int count)>();
            foreach (var item in Distinct)
            {
                list.Add((name: item, count: reveals.Where(x => x == item).Count()));
            }
            var lon = list.Where(x => x.count >= (reveals.Count / x.count));
            foreach (var item in lon)
            {
                yield return new ChartsView { Name = item.name, value = item.count };
            }
        }
        
        public JsonResult Get(int? id)
        {
            if (id == null)
                return Json("Not");

            if(id.Value == 1)
            {
                ViewBag.name = "الكشوف حسب الشهور";
                return Json(GetMonthsAccountReveal());
            }
            else if(id.Value == 2)
            {
                return Json(GetTherapies());
            }
            else if(id.Value == 3)
            {
                return Json(BloodCount());
            }
            else if (id.Value == 4)
            {
                return Json(Genderreveal());
            }
            else if(id.Value == 5)
            {
                return Json(getCountMaleFemal());
            }
            else if(id.Value == 7)
            {
                return Json(getOlds());
            }
            else if(id.Value == 8)
            {
                return Json(GetDig());
            }
            return Json("");
        }
     

        [Flags]
        public enum MonthName
        {
            يناير = 1,
            فبراير = 2,
            مارس = 3,
            ابريل = 4,
            مايو = 5,
            يونيو = 6,
            يوليو = 7,
            اغسطس = 8,
            سبتمر = 9,
            اكتوبر = 10,
            نوفمبر = 11,
            ديسمبر  = 12
        }
    }


}
