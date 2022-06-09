using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;
using Doctor_Management.Models_View;
using System.IO;
using NToastNotify;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Threading;

namespace Doctor_Management.Controllers
{
    public class backupsController : Controller
    {
        private readonly DataContext db;
        private readonly IWebHostEnvironment _environment;
        private readonly IToastNotification _toast;
        

        public backupsController(IToastNotification toast,DataContext db , IWebHostEnvironment environment)
        {
            this.db = db;
            this._environment = environment;
            this._toast = toast;
           
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
            if (GetUser().Admin || db.Owners.ToList().Count == 0)
                return View();

            return NotFound();
        }

        [HttpGet]
        public FileContentResult download()
        {
            GetUser();
            var NameFile = $"DataBackup_{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}" +
                $"_{DateTime.Now.Minute}_{DateTime.Now.Second}_Time.sql";
            using(var con = new MySqlConnection(db.Database.GetDbConnection().ConnectionString))
            {
                using(var cmd = new MySqlCommand())
                {
                    using(var exp = new MySqlBackup(cmd))
                    {
                        using(var stream = new MemoryStream())
                        {
                            cmd.Connection = con;
                            con.Open();
                            exp.ExportToMemoryStream(stream);
                            con.Close();
                            return File(stream.ToArray(),"application/sql",NameFile);
                        }
                    }
                }
            }

        }

        [HttpPost , ValidateAntiForgeryToken]
        public IActionResult Upload(BackUpView model)
        {
            GetUser();
            var file = Request.Form.Files;
            if (file.Any())
            {
                if(Path.GetExtension(file[0].FileName).ToLower() == ".sql")
                {
                    using (var Con = new MySqlConnection(db.Database.GetDbConnection().ConnectionString))
                    {
                        using(var cmd = new MySqlCommand())
                        {
                            using (var Get = new MySqlBackup(cmd))
                            {
                                using(var stream = new MemoryStream(file.setimage()))
                                {
                                    cmd.Connection = Con;
                                    Con.Open();
                                    Get.ImportFromMemoryStream(stream);
                                    Con.Close();
                                    _toast.AddSuccessToastMessage("تم ارجاع نسخة البيانات بنجاح");
                                    return View("Index");
                                }
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("json", "يجب الملف يكون من امتداد SQL");
                    return View("Index", model);
                }
            }
            _toast.AddErrorToastMessage("برجاء اختر ملف البيانات");
            return View("Index");
        }
    }
}
