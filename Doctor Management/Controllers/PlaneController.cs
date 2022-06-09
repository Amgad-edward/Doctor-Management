using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;
using Doctor_Management.Models_View;
using Doctor_Management.IRepository;
using NToastNotify;

namespace Doctor_Management.Controllers
{
    public class PlaneController : Controller
    {
        private readonly IRepositoryData<PlaneReveals> plane;
        private readonly IToastNotification toast;

        public PlaneController(IRepositoryData<PlaneReveals> plane,IToastNotification toast)
        {
            this.plane = plane;
            this.toast = toast;
        }

        private Users GetUser()
        {
            Users users = new Users(HttpContext);
            ViewBag.user = users.User;
            ViewBag.login = users.Login;
            ViewBag.admin = users.Admin;
            return users;
        }
        public async Task<IActionResult> Index()
        {
            if (!GetUser().Admin)
                return NotFound();

            return View(await plane.GetAsync(x => x.DateDay.Date == DateTime.Now.Date));
        }

        [HttpPost ,ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string N , DateTime datefrom , DateTime dateto)
        {
            GetUser();
            if(N == "week")
            {
                var planes = await plane.GetAsync(x => x.DateDay.Date >= DateTime.Now.Date && x.DateDay.Date <= DateTime.Now.AddDays(7).Date);
                return View(planes);
            }
            else if(N == "month")
            {
                var planes = plane.GetAll().AsQueryable().Where(x => x.DateDay.Month == DateTime.Now.Month && x.DateDay.Year == DateTime.Now.Year);
                return View(planes);
            }
            else if(N == "date")
            {
                var planes = await plane.GetAsync(x => x.DateDay.Date >= datefrom.Date && x.DateDay.Date <= dateto.Date);
                return View(planes);
            }

            return View(await plane.GetAsync(x=>x.DateDay.Date == DateTime.Now.Date));
        }

        [HttpPost]
        public IActionResult getdates(DateTime? datefrom , DateTime? dateto)
        {
            GetUser();
            if(datefrom is null || dateto is null)
            {
                toast.AddErrorToastMessage("برجاء التاكد من ادخال التواريخ بشكل صحيح");
                return View("Create", new List<PlaneView>());
            }
            if (datefrom.Value.Date < DateTime.Now.Date)
            {
                toast.AddErrorToastMessage("لا يمكن اضافة تاريخ قد مضى");
                return View("Create", new List<PlaneView>());
            }
            else if(dateto.Value.Date < DateTime.Now.Date)
            {
                toast.AddErrorToastMessage("لا يمكن اضافة تاريخ قد مضى");
                return View("Create", new List<PlaneView>());
            }
            else if(datefrom.Value.Date > dateto.Value.Date)
            {
                toast.AddErrorToastMessage("يجب ان يكون تاريخ البداية اصغر من التاريخ التالى");
                return View("Create", new List<PlaneView>());
            }
            if (datefrom.Value.Date == dateto.Value.Date)
            {
                return View("Create", new List<PlaneView> { new PlaneView { DateDay = datefrom.Value , Day = datefrom.Value.ToDate() } });
            }
           
            var list = new List<PlaneView>();
            var dates = new List<DateTime>();
            dates.Add(datefrom.Value);
            var i = 1;
            do
            {
                var Date = datefrom.Value.AddDays(i);
                dates.Add(Date);
                ++i;
            } while (dates[dates.Count - 1].Date != dateto.Value.Date);

            foreach (var item in dates)
            {
                list.Add(new PlaneView { DateDay = item ,Day = item.ToDate()  });
            }

            return View("Create",list);
        }
        public IActionResult Create()
        {
            if (!GetUser().Admin)
                return NotFound();
           
            return View(new List<PlaneView>());
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<PlaneView> models)
        {
            var Mess = "";
            var list = new List<PlaneReveals>();
            foreach (var p in models)
            {
                if (plane.Any(x => x.DateDay.Date == p.DateDay.Date) || p.DateDay.Date < DateTime.Now.Date)
                {
                    Mess += $"ل لم يتم اضافة هذا التاريخ:{p.DateDay.ToDate()} للانة مضاف مسبقا\n";
                    continue;
                }
                else
                {
                    list.Add(new PlaneReveals
                    {
                        DateDay = p.DateDay,
                        Count = p.Count,
                        All = p.All,
                        Leave = p.Leave
                    });
                }
            }
            if (list.Count > 0)
            {
                await plane.AddAsync(list);
                toast.AddSuccessToastMessage("تم ااضافة المواعيد بنجاح");
            }
            if (Mess != "")
                toast.AddErrorToastMessage(Mess);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (GetUser().Admin && id != null)
            {
                var model = await plane.FindAsync(id);
                var modelview = new PlaneView
                {
                    Id = model.Id,DateDay = model.DateDay,
                    All = model.All,Leave = model.Leave
                    ,Count = model.Count
                };
                return View(modelview);
            }
            return NoContent();
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PlaneView Model)
        {
            //if(plane.Any(x=>x.DateDay.Date == Model.DateDay.Date && x.Id != Model.Id))
            //{
            //    GetUser();
            //    toast.AddErrorToastMessage("هذا الذى تم تعديل تاريخة مضاف مسبقا");
            //    return View(Model);
            //}
             if(Model.DateDay.Date < DateTime.Now.Date)
            {
                GetUser();
                toast.AddErrorToastMessage("لا يمكن وضع خطة لتاريخ مضى");
                return View(Model);
            }
            var Modelsave = new PlaneReveals
            {
                Id = Model.Id,DateDay = Model.DateDay,All = Model.All
                ,Count = Model.Count , 
                Leave = Model.Leave
                
            };
            plane.Update(Modelsave);
            toast.AddSuccessToastMessage("تم تعديل بنجاح");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DelOld()
        {
            var pal = plane.Get(x => x.DateDay.Date < DateTime.Now.Date).ToList();
            if (pal.Count > 0)
                plane.Delete(pal);

            return Ok();
        }

        public IActionResult DelAll()
        {
            var pal = plane.GetAll().ToList();
            if (pal.Count > 0)
                plane.Delete(pal);

            return Ok();
        }


    }
}
