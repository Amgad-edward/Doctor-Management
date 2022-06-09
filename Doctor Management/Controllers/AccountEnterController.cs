using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Doctor_Management.Models;
using Doctor_Management.Models_View;
using Doctor_Management.IRepository;
using NToastNotify;

namespace Doctor_Management.Controllers
{
    public class AccountEnterController : Controller
    {
        private readonly IRepositoryData<Account_Enter> account;
        private readonly IRepositoryData<Surgery> surgery;
        private readonly IToastNotification toast;

        public AccountEnterController(IRepositoryData<Account_Enter> account ,IRepositoryData<Surgery> surgery,IToastNotification toast)
        {
            this.account = account;
            this.surgery = surgery;
            this.toast = toast;
        }
        public IActionResult Index()
        {
            if (list.Count > 0)
                list.Clear();

            var Account = account.GetAll().ToList();
            var Surgery = surgery.GetClude().Include(c=>c.customer).Where(x=>x.Done);
            foreach (var item in Surgery)
            {
                Account.Add(new Account_Enter
                {
                    Amount = item.Price,
                    Date = item.DateTime,
                    From = item.customer.NameCustomer + " " + item.NameSurgery,
                    
                }) ;
            }

            GetUser();
            return View(Account);
        }


        static List<AccountEnterView> list = new List<AccountEnterView>();
        public IActionResult RemoveItem(int? id)
        {
            if (list.Count > 0)
                list.RemoveAt(id.Value);

            GetUser();
            return View("Adding", list);
        }  


        public IActionResult AddNew()
        {
            GetUser();
            list.Add(new AccountEnterView() { Date = DateTime.Now});
            return View("Adding", list);
        }
        public IActionResult Adding()
        {
            if (list.Count == 0)
                list.Add(new AccountEnterView() { Date = DateTime.Now });
            GetUser();
            return View(list);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Adding(List<AccountEnterView> models)
        {
            var ModelsSave = new List<Account_Enter>();
            foreach (var item in models)
            {
                ModelsSave.Add(new Account_Enter
                {
                    From = item.From,Amount = item.Amount , Date = item.Date
                });
            }
            await account.AddAsync(ModelsSave);
            GetUser();
            toast.AddSuccessToastMessage("Done Save All Account");
            return RedirectToAction(nameof(Index));
        }
        private Users GetUser()
        {
            Users users = new Users(HttpContext);
            ViewBag.user = users.User;
            ViewBag.login = users.Login;
            ViewBag.admin = users.Admin;
            return users;
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || !GetUser().Admin)
                return NotFound();

            var model = account.Find(id);
            var ModelEnter = new AccountEnterView
            {
                Id = model.Id,
                From = model.From , Amount = model.Amount ,
                Date = model.Date
            };
            GetUser();
            return View(ModelEnter);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult Edit(AccountEnterView model)
        {
            var SavedModels = new Account_Enter
            {
                Id = model.Id , From = model.From ,Amount = model.Amount,
                Date = model.Date
            };
            account.Update(SavedModels);
            GetUser();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || !GetUser().Admin)
                return NotFound();

            var model = await account.FindAsync(id);
            await account.DeleteAsync(model);
            return Ok();
        }
    }
}
