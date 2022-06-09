using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;
using Doctor_Management.IRepository;
using Doctor_Management.Models_View;
using NToastNotify;

namespace Doctor_Management.Controllers
{
    public class LoginController : Controller
    {
        private readonly IRepositoryData<Loging> log;
        private readonly IToastNotification _toastNotification;
        public LoginController(IRepositoryData<Loging> LOg , IToastNotification toastNotification)
        {
            this.log = LOg;
            this._toastNotification = toastNotification;
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
            GetUser();
            if (log.GetAll().ToList().Count == 0)
                return RedirectToAction(nameof(Create));

            if (GetUser().Admin)
                return View(await log.GetAllAsync());

            return NoContent();
        }

        public IActionResult Create()
        {
            GetUser();
            if (GetUser().Admin || log.GetAll().ToList().Count == 0)
                return View();

            return NotFound();
        } 

        [HttpPost ,ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LogingView model)
        {
            GetUser();
            if (model.Password != model.Re_Password)
            {
                ModelState.AddModelError("Password", "كلمة السر غير مطابقة");
                return View(model);
            }
            if(log.Any(x=>x.UserName == model.UserName))
            {
                ModelState.AddModelError("UserName", "هذا الاسم مستخدم سابقا!!");
                return View(model);
            }

            var savemodel = new Loging
            {
                 UserName = model.UserName,
                 Password = model.Password,
                 Admin = model.Admin
            };
            await log.AddAsync(savemodel);

            _toastNotification.AddSuccessToastMessage($"Add New User Name {model.UserName} id Done");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public JsonResult ExsistsName(string Name)
        {
            return Json(log.Any(x=>x.UserName == Name));
        }

        public IActionResult delete(int? id)
        {
            if (id == null)
                return NoContent();

            if(log.GetAll().ToList().Count == 1)
            {
                return BadRequest();
            }

            log.Delete(id);
            return Ok();
        }
    }
}
