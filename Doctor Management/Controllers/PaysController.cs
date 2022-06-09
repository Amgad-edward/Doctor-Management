using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;
using Doctor_Management.IRepository;
using Doctor_Management.Models_View;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;

namespace Doctor_Management.Controllers
{
    public class PaysController : Controller
    {
        private readonly IToastNotification toast;
        private readonly IRepositoryData<Account_Pay> pay;
        private readonly IRepositoryData<Fixed_pay> fixedpay;

        public PaysController(IToastNotification toast,IRepositoryData<Account_Pay> Pay , IRepositoryData<Fixed_pay> Fixedpay)
        {
            this.toast = toast;
            pay = Pay;
           fixedpay = Fixedpay;
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
            if (!GetUser().Admin)
                return NoContent();

            if (lis.Count > 0)
                lis.Clear();
            GetUser();
            var M = pay.GetAll().ToList();
            var model = M.AsQueryable().Where(x => x.Date.Month == DateTime.Now.Month).ToList();
            return View(model);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string N , DateTime date , DateTime from , DateTime to , string Name)
        {
            GetUser();
            if (N == "month")
            {
                var M = pay.GetAll().ToList();
                var model = M.AsQueryable().Where(x => x.Date.Month == DateTime.Now.Month).ToList();
                return View(model);
            }
            else if(N == "date")
            {
                var M = pay.GetAll().ToList();
                var model = M.AsQueryable().Where(x => x.Date == date.Date).ToList();
                return View(model);
            }
            else if(N == "ft")
            {
                var M = pay.GetAll().ToList();
                var model = M.AsQueryable().Where(x => x.Date >= from.Date && x.Date <= to.Date).ToList();
                return View(model);
            }
            else
            {
                
                var M = pay.GetAll().ToList();
                var model = M.AsQueryable().Where(x => x.ToPay.Contains(Name) ).ToList();
                return View(model);
            }
        }

        public async Task< IActionResult> Edits(int? id)
        {
            if (!GetUser().Admin)
                return NoContent();

            GetUser();
            var P = await pay.FindAsync(id);
            var Model = new PaysViewInput
            {
                Id = P.Id , ToPay = P.ToPay , Amount = P.Amount,
                Date = P.Date
                ,ListFiex = Items()
            };
            return View(Model);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Edits(PaysViewInput model)
        {
            GetUser();
            var savemodel = new Account_Pay
            {
                Id = model.Id , ToPay = model.ToPay , 
                Amount = model.Amount ,  Date = model.Date 
            };
            await pay.UpdateAsync(savemodel);
            toast.AddSuccessToastMessage($"Done Edit Save {model.ToPay}");
            return RedirectToAction(nameof(Index));
        }

        static List<PaysViewInput> lis = new List<PaysViewInput>();


        private List<SelectListItem> Items()
        {
            List<SelectListItem> itemlist = new List<SelectListItem>();
            var fxe = fixedpay.GetAll().ToList();
            foreach (var item in fxe)
            {
                itemlist.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.itemName });
            }
            return itemlist;
        }
        public IActionResult RemoveI(int? id)
        {
            if (id != null)
                lis.RemoveAt(id.Value);

            GetUser();
            return View("AddingPay", lis);
        }
        public IActionResult AddI()
        {
            lis.Add(new PaysViewInput { Date = DateTime.Now });
            GetUser();
            return View("AddingPay", lis);
        }
        public IActionResult AddingPay()
        {
            if (lis.Count == 0)
                lis.Add(new PaysViewInput { Date = DateTime.Now  , ListFiex = Items()});
            GetUser();
            return View(lis);
        }

        public IActionResult Addingout(int? id)
        {
            if (id == null)
                return NoContent();
            GetUser();
            lis.Clear();
            var fix = fixedpay.Find(id);
            lis.Add(new PaysViewInput { ToPay = fix.itemName, Amount = fix.FixsedAmmount, Date = DateTime.Now, ListFiex = Items() });
            return View("AddingPay",lis);
        }

        [HttpGet]
        public async Task<JsonResult> getfixe(int ? id)
        {
            var fixe = await fixedpay.FindAsync(id);
            return Json(fixe);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> AddingPay(List<PaysViewInput> model)
        {
            GetUser();
            var saveModels = new List<Account_Pay>();
            foreach (var item in model)
            {
                saveModels.Add(new Account_Pay
                {
                    ToPay = item.ToPay,Date = item.Date,
                    Amount = item.Amount
                });
            }
            await pay.AddAsync(saveModels);
            toast.AddSuccessToastMessage("Done Saved Pays");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || !GetUser().Admin)
                return NoContent();

            await pay.DeleteAsync(id);
            return Ok();
        }

        public IActionResult Fixed()
        {
            if (!GetUser().Admin)
                return NoContent();
            GetUser();
            return View(fixedpay.GetAll());
        }

        public IActionResult Addfixed()
        {
            if (!GetUser().Admin)
                return NoContent();
            GetUser();
            return View(new FixedView { IsCreate = true });
        }
        public IActionResult Editfixe(int ? id)
        {
            if (!GetUser().Admin || id == null)
                return NoContent();

            GetUser();
            var F = fixedpay.Find(id);
            var model = new FixedView
            {
                Id = F.Id , itemName = F.itemName , 
                FixsedAmmount = F.FixsedAmmount , Timespan = F.Timespan,
                IsCreate = false
            };
            return View("Addfixed" , model);
        }

        [HttpPost ,ValidateAntiForgeryToken]
        public async Task<IActionResult> Editfixe(FixedView model)
        {
            GetUser();
            var savemodel = new Fixed_pay
            {
                Id = model.Id,
                itemName = model.itemName,
                FixsedAmmount = model.FixsedAmmount,
                Timespan = model.Timespan
            };
            await fixedpay.UpdateAsync(savemodel);
            toast.AddSuccessToastMessage($"Done save change {model.itemName}");
            return RedirectToAction(nameof(Fixed));
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Addfixed(FixedView model)
        {
            GetUser();
            if (fixedpay.Any(x=>x.itemName == model.itemName))
            {
                ModelState.AddModelError("itemName", "This Name is Saved");
                return View(model);
            }
            var savemodel = new Fixed_pay
            {
                itemName = model.itemName,FixsedAmmount = model.FixsedAmmount,
                Timespan = model.Timespan
            };
            await fixedpay.AddAsync(savemodel);
            toast.AddSuccessToastMessage($"Done Add items {model.itemName}");
            return RedirectToAction(nameof(Fixed));
        }

        [HttpGet]
        public IActionResult deletefixe(int? id)
        {
            if (id == null || !GetUser().Admin)
                return NoContent();

            fixedpay.Delete(id);
            return Ok();
        }

    }
}
