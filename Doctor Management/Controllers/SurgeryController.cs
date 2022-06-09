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

namespace Doctor_Management.Controllers
{
    public class SurgeryController : Controller
    {
        private readonly IRepositoryData<Surgery> surgery;
        private readonly IToastNotification toast;
        private readonly IRepositoryData<Customer> customers;

        public SurgeryController(IRepositoryData<Surgery> surgery ,IToastNotification toast  ,IRepositoryData<Customer> customers)
        {
            this.surgery = surgery;
            this.toast = toast;
            this.customers = customers;
        }

        private Users GetUser()
        {
            Users users = new Users(HttpContext);
            ViewBag.user = users.User;
            ViewBag.login = users.Login;
            ViewBag.admin = users.Admin;
            return users;
        }

        private IEnumerable<string> NamesCustomers()
        {
            foreach (var item in customers.GetAll().Select(x=>x.NameCustomer).ToList())
            {
                yield return item;
            }
        }
        public IActionResult Index()
        {
            if (!GetUser().Admin)
                return NoContent();

            GetUser();
            var model = surgery.GetClude().Include(c => c.customer).ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            if (!GetUser().Admin)
                return NoContent();

            return View(new SurgeryView { Create = true ,Names = NamesCustomers().ToList()});
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SurgeryView Model)
        {
            GetUser();
            if(surgery.Any(x=>x.NameSurgery == Model.NameSurgery && x.DateTime == Model.DateTime))
            {
                toast.AddErrorToastMessage("هناك عملية مسجلة فى نفس الموعد");
                Model.Create = true;
                Model.Names = NamesCustomers().ToList();
                return View(Model);
            }
            var Cus = customers.Find(x => x.NameCustomer == Model.CustomerName);
            if(Cus == null)
            {
                toast.AddErrorToastMessage("هذا العميل اسمة غير مسجل!!");
                Model.Create = true;
                Model.Names = NamesCustomers().ToList();
                return View(Model);
            }
            var SaveModel = new Surgery
            {
                IdCustomer = Cus.ID,
                DateTime = Model.DateTime,
                Done = Model.Done,Price = Model.Price ,
                NameSurgery = Model.NameSurgery
            };
            await surgery.AddAsync(SaveModel);
            toast.AddSuccessToastMessage($"Done Add Surgery {Model.CustomerName}");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(int? id)
        {
            if (!GetUser().Admin || id == null)
                return NoContent();

            var sur = surgery.GetClude().Include(c=>c.customer).Where(x=>x.Id == id).FirstOrDefault();
            var Views = new SurgeryView
            {
                Id = sur.Id,
                CustomerName = sur.customer.NameCustomer,NameSurgery = sur.NameSurgery,
                DateTime = sur.DateTime , Done = sur.Done
            };
            return View(Views);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null && !GetUser().Admin)
                return NoContent();

            GetUser();
            var Sr = surgery.Find(id);
            if(Sr == null)
            {
                toast.AddErrorToastMessage("هذا غير مسجل");
                return RedirectToAction("Index");
            }
            var SetModel = new SurgeryView
            {
                Create = false,Names = NamesCustomers().ToList(),Id = Sr.Id,
                CustomerName = customers.Find(Sr.Id).NameCustomer , DateTime = Sr.DateTime,
                Done = Sr.Done ,  NameSurgery = Sr.NameSurgery 
                ,Price = Sr.Price
            };
            return View("Create", SetModel);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SurgeryView Model)
        {
            var Cus = customers.Find(x => x.NameCustomer == Model.CustomerName);
            if (Cus == null)
            {
                toast.AddErrorToastMessage("هذا العميل اسمة غير مسجل!!");
                Model.Create = false;
                Model.Names = NamesCustomers().ToList();
                return View(Model);
            }
            var SaveModel = new Surgery
            {
                Id = Model.Id,
                IdCustomer = Cus.ID,
                DateTime = Model.DateTime,
                Done = Model.Done,
                Price = Model.Price,
                NameSurgery = Model.NameSurgery
            };
            await surgery.UpdateAsync(SaveModel);
            toast.AddSuccessToastMessage($"Done Edit Surgery {Model.CustomerName}");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult Index(string N , DateTime date , DateTime from,DateTime to)
        {
            GetUser();
            if(N == "date")
            {
                var model = surgery.GetClude().Include(c => c.customer).Where(x=>x.DateTime.Date == date.Date).ToList();
                return View(model);
            }
            else if(N == "fromdate")
            {
                var model = surgery.GetClude().Include(c => c.customer)
                    .Where(x => x.DateTime.Date >= from.Date && x.DateTime.Date <= to.Date).ToList();
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeDone(int ID , bool isDone)
        {
            var S = surgery.Find(ID);
            S.Done = isDone;
            surgery.Update(S);
            return Ok();
        }

        [HttpPost]
        public JsonResult ExistsName(string Name)
        {
            return Json(customers.Any(x => x.NameCustomer.ToLower() == Name.ToLower()));
        }

        public IActionResult Delete(int? id)
        {
            if (!GetUser().Admin)
                return NotFound();

            surgery.Delete(id);
            return Ok();
        }


        
    }
}
