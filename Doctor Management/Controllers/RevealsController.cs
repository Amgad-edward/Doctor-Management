using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Doctor_Management.IRepository;
using Doctor_Management.Models;

using Doctor_Management.Models_View;
using NToastNotify;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doctor_Management.Controllers
{
    public class RevealsController : Controller
    {
        private readonly IRepositoryData<Reveal> reveal;
        private readonly IRepositoryData<Price> price;
        private readonly IRepositoryData<Therapy> therapy;
        private readonly IRepositoryData<ItemCheckup> checks;
        private readonly IRepositoryData<NamesCheck> checkname;
        private readonly IRepositoryData<PlaneReveals> plane;
        private readonly IRepositoryData<MedicName> medic;
        private readonly IToastNotification toast;
        private readonly IRepositoryData<Customer> customer;
        private readonly IRepositoryData<Acccount_Reveal> acc;

        public RevealsController(IRepositoryData<Reveal> reveal,IRepositoryData<Price> price,IRepositoryData<Therapy> therapy,IRepositoryData<ItemCheckup> Checks,IRepositoryData<NamesCheck> checkname,IRepositoryData<PlaneReveals> plane,IRepositoryData<MedicName> Medic ,IToastNotification toast, IRepositoryData<Customer> customer,IRepositoryData<Acccount_Reveal> Acc )
        {
            this.reveal = reveal;
            this.price = price;
            this.therapy = therapy;
            checks = Checks;
            this.checkname = checkname;
            this.plane = plane;
            medic = Medic;
            this.toast = toast;
            this.customer = customer;
            acc = Acc;
        }
        public IActionResult Index()
        {
            if (GetUser().Login)
            {
                var model = new RevealsSearchView
                {
                    Reveals = reveal.GetClude().Where(x => x.DateReservation == DateTime.Now).Include(C => C.customer).Include(p => p.price).Include(t=>t.Therapies).ToList(),
                    Names = customer.GetAll().Select(x => x.NameCustomer).ToList()
                };
                model.Count = model.Reveals.Count;
                return View(model);
            }
                
            return NotFound();
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string N , string Name , DateTime date, DateTime form , DateTime to)
        {
            GetUser();
            var model = new RevealsSearchView
            {
                Names = customer.GetAll().Select(x => x.NameCustomer).ToList()
            };
            if (N == "month")
            {
               var Mg = reveal.GetClude().Include(c => c.customer)
                    .Include(p => p.price).Include(t => t.Therapies).ToList();
                model.Reveals = Mg.AsQueryable().Where(x => x.DateReservation.Month == DateTime.Now.Month).ToList();
            }
            else if(N == "Date")
            {
                model.Reveals = reveal.GetClude().Where(x => x.DateReservation.Date == date.Date).Include(c => c.customer)
                   .Include(p => p.price).Include(t => t.Therapies).ToList();
            }
            else if(N == "datefrom")
            {
                model.Reveals = reveal.GetClude().Where(x => x.DateReservation.Date >= form.Date && x.DateReservation.Date <= to.Date ).Include(c => c.customer)
                  .Include(p => p.price).Include(t => t.Therapies).ToList();
            }
            else if(N == "Name")
            {
                var id = customer.Find(x => x.NameCustomer == Name | x.NameCustomer.Contains(Name)).ID;
                model.Reveals = reveal.GetClude().Where(x=>x.Idcustomer == id).Include(c => c.customer)
                  .Include(p => p.price).Include(t => t.Therapies).ToList();
            }
            else if(N == "today")
            {
                model.Reveals = reveal.GetClude().Where(x => x.DateReservation.Date == DateTime.Now.Date)
                    .Include(p => p.price).Include(T => T.Therapies).Include(c => c.customer).ToList();
            }
            model.Count = model.Reveals.Count;
            return View(model);
        }
        private Users GetUser()
        {
            Users users = new Users(HttpContext);
            ViewBag.user = users.User;
            ViewBag.login = users.Login;
            ViewBag.admin = users.Admin;
            return users;
        }

        [HttpPost]
        public JsonResult GetreveaslRoll(DateTime date)
        {
            var jsonrsult = new jsonreslut();
            if(date.Date < DateTime.Now.Date)
            {
                jsonrsult.Text = "لا يمكن تعديل الحجز لهذا التاريخ للانة تاريخ سابق";
                return Json(jsonrsult);
            }
            var reveslget = reveal.Get(x => x.DateReservation.Date == date.Date);
            var p = plane.Find(x => x.DateDay.Date == date.Date);
            if(p != null)
            {
                if (p.Leave)
                {
                    jsonrsult.Text = $"هذا اليوم اجارة ولا يوجد بة حجز";
                    jsonrsult.Value = 0;
                    return Json(jsonrsult);
                }
                if (!p.All)
                {
                    if((reveslget.ToList().Count + 1) > p.Count)
                    {
                        jsonrsult.Text = $"لقد تم حجز الحد الاقصى لهذا اليوم وهو ({p.Count})";
                        jsonrsult.Value = 0;
                        return Json(jsonrsult);
                    }
                }
            }
            jsonrsult.Text = $"تعديل الى يوم {date.ToDate()} سيكون رقم ({reveslget.ToList().Count + 1})";
            jsonrsult.Value = reveslget.Count();
            return Json(jsonrsult);
        }

        private IEnumerable<SelectListItem> GetPriceList()
        {
            foreach (var p in price.GetAll())
                yield return new SelectListItem { Value = p.Id.ToString(), Text = p.genderprice + $" {p.ThePrice} Eg" };
        }

        public IActionResult EditThis(int? id)
        {
            if (id == null)
                return NotFound();
            GetUser();
            var Modelview = new EditRevealView
            {
                Reveal = reveal.GetClude().Include(c => c.customer).Include(ch => ch.ItemCheckups).Include(th => th.Therapies)
                .ThenInclude(m => m.medicName).SingleOrDefault(x => x.ID == id.Value),
                medicNames = medic.GetAll(),
                namesChecks = checkname.GetAll()
            };
            return View(Modelview);
        }

        [HttpPost]
        public IActionResult EditReveals(int? id , string dia)
        {
            if (id == null)
                return NoContent();

            var model = reveal.Find(id);
            model.Diagnosis = dia;
            reveal.Update(model);
            return Ok();
        }


        public IActionResult deletemedic(int? id)
        {
            therapy.Delete(id);
            return Ok();
        }
        public IActionResult deletecheck(int? id)
        {
            checks.Delete(id);
            return Ok();
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || !GetUser().Login)
                return NotFound();
            var revealget = reveal.GetClude().Include(c => c.customer).Where(x => x.ID == id.Value).FirstOrDefault();
            ViewBag.list = GetPriceList().ToList();
            var model = new Revealsedit
            {
                ID = revealget.ID,Number = revealget.Number,DateReservation =revealget.DateReservation,
                date_Re_Reveal = revealget.date_Re_Reveal,Diagnosis = revealget.Diagnosis,Done = revealget.Done,
                Idcustomer = revealget.Idcustomer,Idprice = revealget.Idprice,IsRe_Reveal = revealget.IsRe_Reveal,
                Name = revealget.customer.NameCustomer
            };
            return View(model);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Revealsedit model )
        {
            DateTime date = Convert.ToDateTime(TempData["date"]);
            int count = 0;
            if(model.DateReservation.Date < DateTime.Now.Date)
            {
                toast.AddErrorToastMessage("هذا تاريخ سابق ولا يمكن تسجيل حجز بة");
                return RedirectToAction(nameof(Edit),model.ID);
            }    
            model.Done = false;
            if (model.run)
                model.Number = 0;

            if(model.DateReservation.Date != date.Date)
            {
                if (model.run)
                    model.Number = 0;
                else
                {
                    var list = reveal.Get(x => x.DateReservation.Date == model.DateReservation.Date).ToList();
                    count = list.Count;
                    var number = list.Count > 0 ? list.Max(x => x.Number) + 1 : 1;
                    model.Number = number;
                }
            }
            var P = plane.Find(x => x.DateDay.Date == model.DateReservation.Date);
            if(P is not null)
            {
                if (P.Leave)
                {
                    toast.AddErrorToastMessage("هذا اليوم اجارة ولا يوجد بة حجز");
                    return RedirectToAction(nameof(Edit), model.ID);
                }
                if (!P.All)
                {
                    if(count > P.Count)
                    {
                        toast.AddErrorToastMessage($"لقد وصل الحد الاقصى للحجز لذها اليوم وهو ({P.Count})");
                        return RedirectToAction(nameof(Edit), model.ID);
                    }
                }
            }
            var saveing = new Reveal
            {
                ID = model.ID , DateReservation = model.DateReservation,date_Re_Reveal = model.date_Re_Reveal,
                Diagnosis = model.Diagnosis,Idcustomer = model.Idcustomer,Done = model.Done,Number = model.Number,
                Idprice = model.Idprice,IsRe_Reveal = model.IsRe_Reveal
            };
            await reveal.UpdateAsync(saveing);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Account()
        {
            if (!GetUser().Admin)
                return NoContent();

            GetUser();
            var Account = acc.GetClude().Include(Rr => Rr.reveal.customer).Include(p=>p.reveal.price).ToList();
            var Model = Account.AsQueryable().Where(x => x.Date.Month == DateTime.Now.Month).ToList();
            return View(Model);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Account(string N , string txtname,DateTime date , DateTime from , DateTime to )
        {
            GetUser();
            var Account = acc.GetClude().Include(Rr => Rr.reveal.customer).Include(p => p.reveal.price);
            if (N == "month")
            {
                var Model = Account.AsQueryable().Where(x => x.Date.Month == DateTime.Now.Month).ToList();
                return View(Model);
            }
            else if(N == "date")
            {
                var Model = Account.AsQueryable().Where(x => x.Date == date).ToList();
                return View(Model);
            }
            else if(N == "today")
            {
                var Model = Account.Where(x => x.Date.Date == DateTime.Now.Date).ToList();
                return View(Model);
            }
            else if(N == "Name")
            {
                var Model = Account.Where(x => x.reveal.customer.NameCustomer.ToLower().Contains(txtname.ToLower())).ToList();
                return View(Model);
            }
            else
            {
                var Model = Account.AsQueryable().Where(x => x.Date.Date >= from.Date && x.Date.Date <= to.Date).ToList();
                return View(Model);
            }
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NoContent();

            reveal.Delete(id);
            return Ok();
        }


        [HttpGet]
        public JsonResult Getdate()
        {
            var dates = new List<DateTime>();
            for (int i = 1; i <= 15; i++)
                dates.Add(DateTime.Now.AddDays(i));

            foreach (var item in dates)
            {
                var thereveal = reveal.Get(x => x.DateReservation.Date == item.Date).Select(x=>new { x.DateReservation , x.Number }).ToList();
                var P = plane.Find(x => x.DateDay == item.Date);
                if(P != null)
                {
                    if (P.Leave)
                    {
                        continue;
                    }
                    if (!P.All)
                    {
                        if ((thereveal.Count + 1) <= P.Count)
                        {
                            var _number = thereveal.Count > 0 ? thereveal.Max(x => x.Number) + 1 : 1;
                            var dateday = new Thedate { date = item.Date, number = _number, text = $"اليوم {item.ToDate()} رقم الدور ({_number})" };
                            return Json(dateday);
                        }
                        else
                            continue;
                    }
                }
                if(thereveal.Count < 15)
                {
                    var _number = thereveal.Count > 0 ? thereveal.Max(x => x.Number) + 1 : 1;
                    var dateday = new Thedate { date = item.Date , number = _number , text =$"اليوم {item.ToDate()} رقم الدور ({_number})" };
                    return Json(dateday);
                }
            }
            return Json("no");
        }

        [HttpPost]
        public JsonResult Getname(string name)
        {
            var customerid = customer.Get(x => x.NameCustomer.ToLower() == name.ToLower()).FirstOrDefault()?.ID;
            if (customerid is not null)
                return Json(customerid);

            return Json("");
        }

        [HttpPost]
        public JsonResult getday(DateTime date)
        {
            if (date.Date < DateTime.Now.Date)
                return Json("");

            var P = plane.Find(x => x.DateDay.Date == date.Date);
            var reve = reveal.Get(x => x.DateReservation.Date == date.Date).Select(x=> new {x.DateReservation ,x.Number }).ToList();
            int num = reve.Count > 0 ? reve.Max(x => x.Number) + 1 : 1;
            if (P != null)
            {
                if (P.Leave)
                {
                    var day1 = new Thedate
                    {
                        number = -1,
                        text = $"هذا اليوم اجازة حسب ما ادرجة الدكتور",
                        date = date
                    };
                    return Json(day1);
                }
                else if (!P.All)
                {
                    if(num <= P.Count)
                    {
                        var day_2 = new Thedate
                        {
                            number = num,
                            text = $"يـوم {date.ToDate()} رقم الحجز ({num})",
                            date = date
                        };
                        return Json(day_2);
                    }
                    else
                    {
                        var day3 = new Thedate
                        {
                            number = -1,
                            text = $"قائمة الحجز كاملة حتى الان ولا يمكن قيول حجوزات اخرى",
                            date = date
                        };
                        return Json(day3);
                    }
                }
            }
            var day = new Thedate
            {
                number = num,
                text = $"يـوم {date.ToDate()} رقم الحجز ({num})",
                date = date
            };
            return Json(day);
        }

        public async Task<IActionResult> GetReveals()
        {
            GetUser();
            ViewBag.list = GetPriceList().ToList();
            ViewBag.customers = customer.GetAll().Select(x => x.NameCustomer).ToList();
            return View();
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> GetReveals(Revealsedit model)
        {
            GetUser();
            if(model.Date != "" && model.Date != null)
            {
                if (!DateTime.TryParse(model.Date, out DateTime dateTime))
                {
                    toast.AddErrorToastMessage("التاريخ المدخل غير صالح");
                    return RedirectToAction(nameof(GetReveals));
                }
                model.DateReservation = dateTime;
            }
            
            if(reveal.Any(x=>x.Idcustomer == model.Idcustomer && x.DateReservation.Date > DateTime.Now.Date&& !x.Done))
            {
                toast.AddErrorToastMessage("هناك حجز اخر مسجل لهذا العميل");
                return RedirectToAction(nameof(GetReveals));
            }
            var p = plane.Find(x => x.DateDay.Date == model.DateReservation.Date);
            if(p is not null)
            {
                if (p.Leave)
                {
                    toast.AddErrorToastMessage("هذا اليوم اجارة");
                    return RedirectToAction(nameof(GetReveals));
                }
                if (!p.All)
                {
                    var RevealDay = reveal.Get(x => x.DateReservation.Date == model.DateReservation.Date).Select(x=>x.ID).ToList().Count;
                    if(RevealDay > p.Count)
                    {
                        toast.AddErrorToastMessage($"لقد تم الوصول للحد الاقصى للحجز لهذا اليوم وهو ({p.Count})");
                        return RedirectToAction(nameof(GetReveals));
                    }
                }
                
            }
            if (model.run)
                model.Number = 0;

            var modelsave = new Reveal
            {
                 Idcustomer = model.Idcustomer,Idprice = model.Idprice,Number = model.Number,
                 DateReservation = model.DateReservation,Done = false
            };
            await reveal.AddAsync(modelsave);
            toast.AddSuccessToastMessage($"تم عمل الحجز للعميل {model.NameCustomer} بنجاح");
            return RedirectToAction(nameof(GetReveals));
        }

        class jsonreslut
        {
            public string Text { get; set; }

            public int Value { get; set; }
        }
        class Thedate
        {
            public string text { get; set; }

            public DateTime date { get; set; }

            public int number { get; set; }
        }

    }
}
