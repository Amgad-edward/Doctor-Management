using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.IRepository;
using Doctor_Management.Models;
using Doctor_Management.Models_View;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Doctor_Management.Controllers
{
    public class AccountMoveController : Controller
    {
        private readonly IRepositoryData<Acccount_Reveal> reveal;
        private readonly IRepositoryData<Surgery> surgery;
        private readonly IRepositoryData<Price> price;
        private readonly IRepositoryData<Account_Enter> enter;
        private readonly IRepositoryData<Fixed_pay> fixe;
        private readonly IRepositoryData<Account_Pay> pays;
        private readonly IToastNotification toast;

        public AccountMoveController(IRepositoryData<Acccount_Reveal> reveal, IRepositoryData<Surgery> surgery,IRepositoryData<Price> price ,IRepositoryData<Account_Enter> enter,IRepositoryData<Fixed_pay> fixe,IRepositoryData<Account_Pay> pay,IToastNotification toast)
        {
            this.reveal = reveal;
            this.surgery = surgery;
            this.price = price;
            this.enter = enter;
            this.fixe = fixe;
            this.pays = pay;
            this.toast = toast;
        }

        private List<Acccount_Reveal> GetReveals(Func<Acccount_Reveal , bool> func)
        {
            var M = reveal.GetClude().Include(r => r.reveal).ThenInclude(c=>c.customer).Include(p => p.reveal).ThenInclude(p=>p.price).ToList();
            return M.AsQueryable().Where(func).ToList();
        }
        
        private List<Account_Enter> GetEnter(Func<Account_Enter , bool> func)
        {
            return enter.Get(func).ToList();
        }

        private List<Surgery> GetSergerys(Func<Surgery , bool> func)
        {
            return surgery.GetClude().Include(c => c.customer).Where(func).ToList();
        } 

        private List<Account_Pay> GetPays(Func<Account_Pay , bool> func)
        {
            var Models = pays.GetAll().ToList();
            return Models.AsQueryable().Where(func).ToList();
        }

        private List<SelectListItem> GetNames()
        {
            var lis = new List<SelectListItem>();

            var Price = price.GetAll().ToList();
            var Enter = enter.GetAll().Select(x=>x.From).Distinct().ToList();
            var Pay_2 = pays.GetAll().Select(s => s.ToPay).Distinct().ToList();
            foreach (var item in Price)
                lis.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.genderprice });
            foreach (var item in Enter)
                lis.Add(new SelectListItem { Value = item, Text = item});
            foreach (var item in Pay_2)
                lis.Add(new SelectListItem { Value = item, Text = item });

            return lis;
            
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

            var Model = new AccountMoveView
            {
                Enter = GetEnter(x=>x.Date.Month == DateTime.Now.Month &&x.Date.Year == DateTime.Now.Year),
                Reveals = GetReveals(x=>x.Date.Month == DateTime.Now.Month && x.Date.Year == DateTime.Now.Year),
                Pays = GetPays(x=>x.Date.Month == DateTime.Now.Month && x.Date.Year == DateTime.Now.Year),
                Surgeries = GetSergerys(x=>x.DateTime.Month == DateTime.Now.Month && x.DateTime.Year == DateTime.Now.Year &&x.Done),
                listNames = GetNames()
            };
            GetUser();
            return View(Model);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string N , string Name , DateTime date , DateTime from , DateTime to)
        {
            GetUser();
            if (N == "All")
            {
                var Model = new AccountMoveView
                {
                    Enter = enter.GetAll().ToList(),
                    Reveals = reveal.GetClude().Include(c=>c.reveal).ThenInclude(cus=>cus.customer).Include(rr=>rr.reveal)
                    .ThenInclude(p=>p.price).ToList(),
                    Pays = pays.GetAll().ToList(),
                    Surgeries = surgery.Get(x=> x.Done).ToList(),
                    listNames = GetNames()
                };
                return View(Model);
            }
            else if(N == "month")
            {
                var Model = new AccountMoveView
                {
                    Enter = GetEnter(x => x.Date.Month == DateTime.Now.Month && x.Date.Year == DateTime.Now.Year),
                    Reveals = GetReveals(x => x.Date.Month == DateTime.Now.Month && x.Date.Year == DateTime.Now.Year),
                    Pays = GetPays(x => x.Date.Month == DateTime.Now.Month && x.Date.Year == DateTime.Now.Year),
                    Surgeries = GetSergerys(x=>x.DateTime.Month == DateTime.Now.Month && x.DateTime.Year == DateTime.Now.Year && x.Done),
                    listNames = GetNames()
                };
                return View(Model);
            }
            else if(N == "date")
            {
                var Model = new AccountMoveView
                {
                    Enter = GetEnter(x => x.Date.Date == date.Date),
                    Reveals = GetReveals(x => x.Date.Date == date.Date),
                    Pays = GetPays(x => x.Date.Date == date.Date),
                    Surgeries = GetSergerys(x=>x.DateTime.Date == date.Date && x.Done),
                    listNames = GetNames()
                };
                return View(Model);
            }
            else if(N == "fr")
            {
                var Model = new AccountMoveView
                {
                    Enter = GetEnter(x => x.Date.Date >= from.Date && x.Date.Date <= to.Date),
                    Reveals = GetReveals(x => x.Date.Date >= from.Date && x.Date.Date <= to.Date),
                    Pays = GetPays(x => x.Date.Date >= from.Date && x.Date.Date <= to.Date),
                    Surgeries = GetSergerys(x=>x.DateTime.Date >= from.Date && x.DateTime.Date <= to.Date && x.Done),
                    listNames = GetNames()
                };
                return View(Model);
            }
            else
            {
              
                var Model = new AccountMoveView
                {
                    Enter = GetEnter(x => x.From.ToLower() == Name.ToLower()),
                    Reveals = GetReveals(x => x.reveal.Idprice.ToString() == Name),
                    Pays = GetPays(x => x.ToPay.ToLower() == Name.ToLower()),
                    Surgeries = GetSergerys(x=>x.NameSurgery.ToLower().Contains(Name.ToLower()) && x.Done),
                    listNames = GetNames()
                };
                return View(Model);
            }
           
            
        }


        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Print(string N, string Name, DateTime date, DateTime from, DateTime to)
        {
            GetUser();
            if (N == "All")
            {
                var Model = new AccountMoveView
                {
                    Enter = enter.GetAll().ToList(),
                    Reveals = reveal.GetAll().ToList(),
                    Pays = pays.GetAll().ToList(),
                    Surgeries = surgery.Get(x=>x.Done).ToList(),
                    listNames = GetNames()
                };
                return View(Model);
            }
            else if (N == "month")
            {
                var Model = new AccountMoveView
                {
                    Enter = GetEnter(x => x.Date.Month == DateTime.Now.Month),
                    Reveals = GetReveals(x => x.Date.Month == DateTime.Now.Month),
                    Pays = GetPays(x => x.Date.Month == DateTime.Now.Month),
                    Surgeries = GetSergerys(x => x.DateTime.Month == DateTime.Now.Month && x.DateTime.Year == DateTime.Now.Year && x.Done),
                    listNames = GetNames()
                };
                return View(Model);
            }
            else if (N == "date")
            {
                var Model = new AccountMoveView
                {
                    Enter = GetEnter(x => x.Date.Date == date.Date),
                    Reveals = GetReveals(x => x.Date.Date == date.Date),
                    Pays = GetPays(x => x.Date.Date == date.Date),
                    Surgeries = GetSergerys(x => x.DateTime.Date == date.Date && x.Done),
                    listNames = GetNames()
                };
                return View(Model);
            }
            else if (N == "fr")
            {
                var Model = new AccountMoveView
                {
                    Enter = GetEnter(x => x.Date.Date >= from && x.Date.Date <= to),
                    Reveals = GetReveals(x => x.Date.Date >= from && x.Date.Date <= to),
                    Pays = GetPays(x => x.Date.Date >= from && x.Date.Date <= to),
                    Surgeries = GetSergerys(x => x.DateTime.Date >= from.Date && x.DateTime.Date <= to.Date && x.Done),
                    listNames = GetNames()
                };
                return View(Model);
            }
            else
            {
               
                var Model = new AccountMoveView
                {
                    Enter = GetEnter(x => x.From.ToLower() == Name.ToLower()),
                    Reveals = GetReveals(x => x.reveal.Idprice.ToString() == Name),
                    Pays = GetPays(x => x.ToPay.ToLower() == Name.ToLower()),
                    Surgeries = GetSergerys(x => x.NameSurgery.ToLower().Contains(Name.ToLower()) && x.Done),
                    listNames = GetNames()
                };
                return View(Model);
            }
        }
    }
}
